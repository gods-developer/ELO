Imports System.Data.Entity.Core.Objects
Imports System.Data.Entity.Infrastructure
Imports Microsoft.Office.Interop.Access.Dao
Imports EloExplorer
Imports OrgMan.OrgMan
Imports Win.Common.Tools
Imports EloExplorer.EloExplorer

Public Class MsSqlConnector
    Implements IDatabaseConnector
    Dim dbs As EloExplorer.OrgManEntities
    Private _mySid As String, _myGroupSids As List(Of String), adminState As Integer

    Public Sub New()
        '        If My.Settings.LocalVersion Then
        '            dbPath = My.Application.Info.DirectoryPath & "\OrgMan.accdb"
        '        Else
#If DEBUG Then
        dbs = New EloExplorer.OrgManEntities()
#Else
                    dbs = New OrgManEntities()
#End If
        '        End If
    End Sub

    Private ReadOnly Property MySid() As String
        Get
            If String.IsNullOrEmpty(_mySid) Then
                Dim adcon As New WinAdConnector()
                If adcon.DomainIsAvailable() Then
                    Dim user = adcon.GetUser(Environment.UserName)
                    _mySid = user.Id
                Else
                    _mySid = "---"
                End If
            End If
            Return _mySid
        End Get
    End Property

    Private ReadOnly Property MyGroupSids() As List(Of String)
        Get
            If _myGroupSids Is Nothing Then
                _myGroupSids = New List(Of String)
                Dim adcon As New WinAdConnector()
                If adcon.DomainIsAvailable() Then
                    Dim groups = adcon.GetUserGroups(Environment.UserName)
                    For Each grp In groups
                        _myGroupSids.Add(grp.Id)
                    Next
                End If
            End If
            Return _myGroupSids
        End Get
    End Property

    Public ReadOnly Property Path As String Implements IDatabaseConnector.Path
        Get
            Dim dbConnectionString As String = dbs.Database.Connection.ConnectionString
            Path = GetConnectionStringValue(dbConnectionString, "data source") + "\" + GetConnectionStringValue(dbConnectionString, "initial catalog")
        End Get
    End Property

    Private Function GetConnectionStringValue(connectionString As String, key As String) As String
        Dim result As String = ""
        Dim pos As Integer = connectionString.IndexOf(key)
        pos += key.Length
        result = connectionString.Substring(pos + 1)
        pos = result.IndexOf(";")
        result = result.Substring(0, pos)
        GetConnectionStringValue = result
    End Function

    Public Sub Close() Implements IDatabaseConnector.Close
        dbs?.Dispose()
    End Sub

    Public Sub DeleteTreeItem(id As Integer) Implements IDatabaseConnector.DeleteTreeItem
        Dim item As TreeItem = dbs.TreeItems.Find(id) 'From r In dbs.TreeItems Where r.Id = id
        If Not item Is Nothing Then
            dbs.TreeItems.Remove(item)
            SaveChanges()
        End If
    End Sub

    Public Sub SaveTreeItem(item As OrgManTreeItem) Implements IDatabaseConnector.SaveTreeItem
        Dim record As TreeItem = dbs.TreeItems.Find(item.Id)
        If Not record Is Nothing Then
            record.NodeText = item.NodeText
            record.ChildrenSortBy = item.ChildrenSortBy
            record.ChildrenSortWay = item.ChildrenSortWay
            record.FilesSortBy = item.FilesSortBy
            record.FilesSortWay = item.FilesSortWay
            record.LastUpdate = Now
            record.LastUpdateUser = Environment.UserName
            record.RowVersion = record.RowVersion + 1
            SaveChanges()
        End If
    End Sub

    Public Sub SaveRootPath(item As OrgManTreeItem) Implements IDatabaseConnector.SaveRootPath
        Dim query = From r In dbs.RootPaths Where r.TreeItemId = item.Id
        Dim record As RootPath
        If query.Count() = 0 Then
            record = dbs.RootPaths.Create()
            record.TreeItemId = item.Id
            record.Creation = Now
            record.CreationUser = Environment.UserName
            record.RowVersion = 1
            dbs.RootPaths.Add(record)
        Else
            record = query.FirstOrDefault()
            record.LastUpdate = Now
            record.LastUpdateUser = Environment.UserName
            record.RowVersion = record.RowVersion + 1
        End If
        record.RootPath1 = item.RootPath
        SaveChanges()
    End Sub

    Public Sub MoveTreeItem(item As OrgManTreeItem, offset As Integer) Implements IDatabaseConnector.MoveTreeItem
        Dim query1 = From r In dbs.TreeItems Where r.Id = item.Id
        Dim record1 As TreeItem = query1.FirstOrDefault()
        Dim query2 = From r In dbs.TreeItems Where r.ParentNodeId = item.ParentId And r.SortOrder = (item.SortOrder + offset)
        Dim record2 As TreeItem = query2.FirstOrDefault()
        record1.LastUpdate = Now
        record1.LastUpdateUser = Environment.UserName
        record1.RowVersion = record1.RowVersion + 1
        record1.SortOrder = record1.SortOrder + offset
        record2.LastUpdate = Now
        record2.LastUpdateUser = Environment.UserName
        record2.RowVersion = record2.RowVersion + 1
        record2.SortOrder = record2.SortOrder - offset
        SaveChanges()
    End Sub

    Public Function GetRootTreeItems() As List(Of OrgManTreeItem) Implements IDatabaseConnector.GetRootTreeItems
        Dim results As New List(Of OrgManTreeItem)
        Dim query = From r In dbs.TreeItems Where r.ParentNodeId Is Nothing Order By r.SortOrder
        For Each treeItem In query
            If IsTreeItemAccess(treeItem.Id) Then
                results.Add(New OrgManTreeItem(Nothing, treeItem))
            End If
        Next
        GetRootTreeItems = results
    End Function

    Private Function IsTreeItemAccess(treeItemId As Integer) As Boolean
        If IsAdmin() Then
            IsTreeItemAccess = True
            Exit Function
        End If
        Dim userQuery = From r In dbs.TreeItemUserRights Where r.TreeItemId = treeItemId
        For Each rec In userQuery
            If rec.UserId = MySid And rec.AccessRight >= OrgManEnums.AccessRight.Read Then
                IsTreeItemAccess = True
                Exit Function
            End If
        Next
        Dim groupQuery = From r In dbs.TreeItemGroupRights Where r.TreeItemId = treeItemId
        For Each rec In groupQuery
            If MyGroupSids.Contains(rec.GroupId) And rec.AccessRight >= OrgManEnums.AccessRight.Read Then
                IsTreeItemAccess = True
                Exit Function
            End If
        Next
        IsTreeItemAccess = False
    End Function

    Public Function GetChildTreeItems(parentNodeId As Integer) As List(Of OrgManTreeItem) Implements IDatabaseConnector.GetChildTreeItems
        Dim results As New List(Of OrgManTreeItem)
        Dim query = From r In dbs.TreeItems Where r.ParentNodeId = parentNodeId Order By r.SortOrder
        For Each treeItem In query
            results.Add(New OrgManTreeItem(parentNodeId, treeItem))
        Next
        GetChildTreeItems = results
    End Function

    Public Function AddNewTreeItem(newName As String, Optional newRootPath As String = "", Optional parentNodeId As Integer = 0) As OrgManTreeItem Implements IDatabaseConnector.AddNewTreeItem
        Dim treeItem As TreeItem = dbs.TreeItems.Create()
        treeItem.Creation = Now
        treeItem.CreationUser = Environment.UserName
        treeItem.RowVersion = 1
        treeItem.NodeText = newName
        If parentNodeId > 0 Then
            treeItem.ParentNodeId = parentNodeId
        End If
        treeItem.SortOrder = GetMaxTreeItemId(parentNodeId) + 10
        treeItem.ChildrenSortBy = 0 'Benutzerdefiniert
        treeItem.ChildrenSortWay = 0 'Aufsteigend
        treeItem.FilesSortBy = 0 'Name
        treeItem.FilesSortWay = 0 'Aufsteigend
        dbs.TreeItems.Add(treeItem)
        Dim rows = SaveChanges()
        If newRootPath <> "" And rows > 0 Then
            Dim rootPath As RootPath = dbs.RootPaths.Create()
            rootPath.Creation = Now
            rootPath.CreationUser = Environment.UserName
            rootPath.RowVersion = 1
            rootPath.TreeItemId = treeItem.Id
            rootPath.RootPath1 = newRootPath
            dbs.RootPaths.Add(rootPath)
            SaveChanges()
        End If
        If rows > 0 Then
            Dim result = New OrgManTreeItem(parentNodeId, treeItem)
            result.RootPath = newRootPath
            AddNewTreeItem = result
        Else
            AddNewTreeItem = Nothing
        End If
    End Function

    Public Function GetRootPath(nodeId As Integer) As String Implements IDatabaseConnector.GetRootPath
        Dim query = From r In dbs.RootPaths Where r.TreeItemId = nodeId
        If query.Count() > 0 Then
            GetRootPath = query.FirstOrDefault().RootPath1
        Else
            GetRootPath = ""
        End If
        If GetRootPath.EndsWith("\") Then
            GetRootPath = Left(GetRootPath, Len(GetRootPath) - 1)
        End If
    End Function

    Public Function CheckIfNameExists(parentNodeId As Integer, newName As String) As Boolean Implements IDatabaseConnector.CheckIfNameExists
        Dim query As IQueryable(Of TreeItem)
        If parentNodeId = 0 Then
            query = (From r In dbs.TreeItems Where r.ParentNodeId Is Nothing And r.NodeText = newName.Replace("'", "''"))
        Else
            query = (From r In dbs.TreeItems Where r.ParentNodeId = parentNodeId And r.NodeText = newName.Replace("'", "''"))
        End If
        CheckIfNameExists = (query.Count() > 0)
    End Function

    Private Function GetMaxTreeItemId(parentNodeId As Integer) As Integer
        DirectCast(dbs, IObjectContextAdapter).ObjectContext.Refresh(RefreshMode.StoreWins, dbs.TreeItems)
        If parentNodeId = 0 Then
            Dim query = (From r In dbs.TreeItems Where r.ParentNodeId Is Nothing Select r.SortOrder)
            If query.Any() Then
                GetMaxTreeItemId = query.Max()
            End If
        Else
            Dim query = (From r In dbs.TreeItems Where r.ParentNodeId = parentNodeId Select r.SortOrder)
            If query.Any() Then
                GetMaxTreeItemId = query.Max()
            End If
        End If
    End Function

    Private Function GetMaxTreeItemFileId(treeItemId As Integer) As Integer
        DirectCast(dbs, IObjectContextAdapter).ObjectContext.Refresh(RefreshMode.StoreWins, dbs.TreeItemFiles)
        Dim query = (From r In dbs.TreeItemFiles Where r.TreeItemId = treeItemId Select r.SortOrder)
        Dim maxId As Integer
        If query.Any() Then
            maxId = query.Max()
        End If
        GetMaxTreeItemFileId = maxId
    End Function

    Public Function GetDbSetting(Key As String, Optional [Default] As String = "") As String Implements IDatabaseConnector.GetDbSetting
        Dim query = From r In dbs.Settings Where r.SettingName = Key
        If query.Count() > 0 Then
            GetDbSetting = query.FirstOrDefault().SettingValue
        Else
            GetDbSetting = [Default]
        End If
    End Function

    Public Sub SaveDbSetting(Key As String, Setting As String) Implements IDatabaseConnector.SaveDbSetting
        Dim query = From r In dbs.Settings Where r.SettingName = Key
        Dim record As Setting
        If query.Count() = 0 Then
            record = dbs.Settings.Create()
            record.SettingName = Key
            record.Creation = Now
            record.CreationUser = Environment.UserName
            record.RowVersion = 1
            dbs.Settings.Add(record)
        Else
            record = query.FirstOrDefault()
            record.LastUpdate = Now
            record.LastUpdateUser = Environment.UserName
            record.RowVersion = record.RowVersion + 1
        End If
        record.SettingValue = Setting
        SaveChanges()
    End Sub

    Public Function HasAccess() As Boolean Implements IDatabaseConnector.HasAccess
        Dim query = From r In dbs.AppUsers Where r.UserDomain.ToLower() = Environment.UserDomainName.ToLower() And r.UserName.ToLower() = Environment.UserName.ToLower()
        HasAccess = query.Count() = 1
    End Function

    Public Function GetDbUserSetting(Key As String, Optional [Default] As String = "") As String Implements IDatabaseConnector.GetDbUserSetting
        Dim query = From r In dbs.AppUserSettings Where r.AppUser.UserDomain.ToLower() = Environment.UserDomainName.ToLower() And r.AppUser.UserName.ToLower() = Environment.UserName.ToLower() And r.SettingName = Key
        If query.Count() > 0 Then
            GetDbUserSetting = query.FirstOrDefault().SettingValue
        Else
            GetDbUserSetting = [Default]
        End If
    End Function

    Public Sub SaveDbUserSetting(Key As String, Setting As String) Implements IDatabaseConnector.SaveDbUserSetting
        Dim query = From r In dbs.AppUserSettings Where r.AppUser.UserDomain.ToLower() = Environment.UserDomainName.ToLower() And r.AppUser.UserName.ToLower() = Environment.UserName.ToLower() And r.SettingName = Key
        Dim record As AppUserSetting
        If query.Count() = 0 Then
            record = dbs.AppUserSettings.Create()
            record.AppUserId = GetAppUserId()
            record.SettingName = Key
            record.Creation = Now
            record.CreationUser = Environment.UserName
            record.RowVersion = 1
            dbs.AppUserSettings.Add(record)
        Else
            record = query.FirstOrDefault()
            record.LastUpdate = Now
            record.LastUpdateUser = Environment.UserName
            record.RowVersion = record.RowVersion + 1
        End If
        record.SettingValue = Setting
        SaveChanges()
    End Sub

    Private appUserId As Integer

    Private Function GetAppUserId() As Integer
        If appUserId = 0 Then
            Dim query = From r In dbs.AppUsers Where r.UserDomain.ToLower() = Environment.UserDomainName.ToLower() And r.UserName.ToLower() = Environment.UserName.ToLower()
            If query.Count() > 0 Then
                appUserId = query.FirstOrDefault().Id
            End If
        End If
        GetAppUserId = appUserId
    End Function

    Public Function IsAdmin() As Boolean Implements IDatabaseConnector.IsAdmin
        If adminState = 0 Then
            Dim query = From r In dbs.AppUsers Where r.UserDomain.ToLower() = Environment.UserDomainName.ToLower() And r.UserName.ToLower() = Environment.UserName.ToLower() And r.IsAdmin = True
            adminState = IIf(query.Count() = 0, -1, 1)
        End If
        IsAdmin = adminState = 1
    End Function

    Public Function GetGroupRight(treeItemId As Integer, sid As String) As OrgManEnums.AccessRight Implements IDatabaseConnector.GetGroupRight
        Dim query = From r In dbs.TreeItemGroupRights Where r.GroupId = sid And r.TreeItemId = treeItemId
        If query.Count() > 0 Then
            GetGroupRight = query.FirstOrDefault().AccessRight
        Else
            GetGroupRight = OrgManEnums.AccessRight.NoAccess
        End If
    End Function

    Public Sub SetGroupRight(treeItemId As Integer, sid As String, accessRight As OrgManEnums.AccessRight) Implements IDatabaseConnector.SetGroupRight
        Dim query = From r In dbs.TreeItemGroupRights Where r.GroupId = sid And r.TreeItemId = treeItemId
        Dim record As TreeItemGroupRight
        If query.Count() = 0 Then
            If accessRight = OrgManEnums.AccessRight.NoAccess Then
                Exit Sub
            End If
            record = dbs.TreeItemGroupRights.Create()
            record.TreeItemId = treeItemId
            record.GroupId = sid
            record.AccessRight = accessRight
            record.Creation = Now
            record.CreationUser = Environment.UserName
            record.RowVersion = 1
            dbs.TreeItemGroupRights.Add(record)
        Else
            record = query.FirstOrDefault()
            If accessRight = OrgManEnums.AccessRight.NoAccess Then
                dbs.TreeItemGroupRights.Remove(record)
            Else
                record.AccessRight = accessRight
                record.LastUpdate = Now
                record.LastUpdateUser = Environment.UserName
                record.RowVersion = record.RowVersion + 1
            End If
        End If
        SaveChanges()
    End Sub

    Public Function GetUserRight(treeItemId As Integer, sid As String) As OrgManEnums.AccessRight Implements IDatabaseConnector.GetUserRight
        Dim query = From r In dbs.TreeItemUserRights Where r.UserId = sid And r.TreeItemId = treeItemId
        If query.Count() > 0 Then
            GetUserRight = query.FirstOrDefault().AccessRight
        Else
            GetUserRight = OrgManEnums.AccessRight.NoAccess
        End If
    End Function

    Public Sub SetUserRight(treeItemId As Integer, sid As String, accessRight As OrgManEnums.AccessRight) Implements IDatabaseConnector.SetUserRight
        Dim query = From r In dbs.TreeItemUserRights Where r.UserId = sid And r.TreeItemId = treeItemId
        Dim record As TreeItemUserRight
        If query.Count() = 0 Then
            If accessRight = OrgManEnums.AccessRight.NoAccess Then
                Exit Sub
            End If
            record = dbs.TreeItemUserRights.Create()
            record.TreeItemId = treeItemId
            record.UserId = sid
            record.AccessRight = accessRight
            record.Creation = Now
            record.CreationUser = Environment.UserName
            record.RowVersion = 1
            dbs.TreeItemUserRights.Add(record)
        Else
            record = query.FirstOrDefault()
            If accessRight = OrgManEnums.AccessRight.NoAccess Then
                dbs.TreeItemUserRights.Remove(record)
            Else
                record.AccessRight = accessRight
                record.LastUpdate = Now
                record.LastUpdateUser = Environment.UserName
                record.RowVersion = record.RowVersion + 1
            End If
        End If
        SaveChanges()
    End Sub

    Public Function GetGroupRights(treeItemId As Integer) As IList(Of OrgManAccessRight) Implements IDatabaseConnector.GetGroupRights
        Dim results = New List(Of OrgManAccessRight)
        Dim query = From r In dbs.TreeItemGroupRights Where r.TreeItemId = treeItemId
        If query.Count() > 0 Then
            For Each rec In query
                results.Add(New OrgManAccessRight(rec.GroupId, rec.AccessRight))
            Next
        End If
        GetGroupRights = results
    End Function

    Public Function GetUserRights(treeItemId As Integer) As IList(Of OrgManAccessRight) Implements IDatabaseConnector.GetUserRights
        Dim results = New List(Of OrgManAccessRight)
        Dim query = From r In dbs.TreeItemUserRights Where r.TreeItemId = treeItemId
        If query.Count() > 0 Then
            For Each rec In query
                results.Add(New OrgManAccessRight(rec.UserId, rec.AccessRight))
            Next
        End If
        GetUserRights = results
    End Function

    Public Function GetReminderDate(treeItemId As Integer, filename As String) As Date Implements IDatabaseConnector.GetReminderDate
        Dim userId = GetAppUserId()
        Dim query = From r In dbs.Reminders Where r.AppUserId = userId And r.TreeItemId = treeItemId And r.Filename = filename And r.Done = False
        If query.Count() > 0 Then
            GetReminderDate = query.FirstOrDefault().ReminderDate
        Else
            GetReminderDate = Nothing
        End If
    End Function

    Public Sub SetReminderDate(treeItemId As Integer, filename As String, reminderDate As Date) Implements IDatabaseConnector.SetReminderDate
        Dim userId = GetAppUserId()
        Dim query = From r In dbs.Reminders Where r.AppUserId = userId And r.TreeItemId = treeItemId And r.Filename = filename
        Dim record As Reminder
        If query.Count() = 0 Then
            record = dbs.Reminders.Create()
            record.AppUserId = GetAppUserId()
            record.TreeItemId = treeItemId
            record.Filename = filename
            record.Creation = Now
            record.CreationUser = Environment.UserName
            record.RowVersion = 1
            dbs.Reminders.Add(record)
        Else
            record = query.FirstOrDefault()
            record.LastUpdate = Now
            record.LastUpdateUser = Environment.UserName
            record.RowVersion = record.RowVersion + 1
        End If
        record.Done = False
        record.ReminderDate = reminderDate
        SaveChanges()
    End Sub

    Public Sub DeleteReminderDate(treeItemId As Integer, filename As String) Implements IDatabaseConnector.DeleteReminderDate
        Dim userId = GetAppUserId()
        Dim query = From r In dbs.Reminders Where r.AppUserId = userId And r.TreeItemId = treeItemId And r.Filename = filename
        If query.Count() > 0 Then
            dbs.Reminders.Remove(query.FirstOrDefault())
            SaveChanges()
        End If
    End Sub

    Public Function GetReminders() As List(Of OrgManReminder) Implements IDatabaseConnector.GetReminders
        Dim results = New List(Of OrgManReminder)
        Dim userId = GetAppUserId()
        Dim query = From r In dbs.Reminders Where r.AppUserId = userId And r.ReminderDate <= DateTime.Now And r.Done = False Order By r.ReminderDate
        If query.Count() > 0 Then
            For Each rec In query
                results.Add(New OrgManReminder(rec.TreeItemId, rec.Filename, rec.ReminderDate))
            Next
        End If
        GetReminders = results
    End Function

    Public Sub FinishReminder(treeItemId As Integer, filename As String) Implements IDatabaseConnector.FinishReminder
        Dim userId = GetAppUserId()
        Dim query = From r In dbs.Reminders Where r.AppUserId = userId And r.TreeItemId = treeItemId And r.Filename = filename
        If query.Count() > 0 Then
            Dim record = query.FirstOrDefault()
            record.Done = True
            record.LastUpdate = Now
            record.LastUpdateUser = Environment.UserName
            record.RowVersion = record.RowVersion + 1
            SaveChanges()
        End If
    End Sub

    Public Function AddNewTreeItemFile(treeItemId As Integer, filename As String) As OrgManTreeItemFile Implements IDatabaseConnector.AddNewTreeItemFile
        If treeItemId <= 0 Or filename Is Nothing Then
            AddNewTreeItemFile = Nothing
            Exit Function
        End If
        Dim item As TreeItemFile = dbs.TreeItemFiles.Create()
        item.Creation = Now
        item.CreationUser = Environment.UserName
        item.RowVersion = 1
        item.Filename = filename
        item.TreeItemId = treeItemId
        item.SortOrder = GetMaxTreeItemFileId(treeItemId) + 10
        dbs.TreeItemFiles.Add(item)
        If SaveChanges() > 0 Then
            Dim result = New OrgManTreeItemFile(item)
            AddNewTreeItemFile = result
        Else
            AddNewTreeItemFile = Nothing
        End If
    End Function

    Public Function AddNewFileIndex(fileId As Integer, indexName As String, indexValue As String) As Boolean
        If String.IsNullOrEmpty(indexValue) Then
            Exit Function
        End If
        Dim indexId = GetIndexId(indexName)
        Dim item As FileIndex = dbs.FileIndexes.Create()
        item.Creation = Now
        item.CreationUser = Environment.UserName
        item.RowVersion = 1
        item.FileId = fileId
        item.IndexId = indexId
        item.IndexValue = indexValue
        dbs.FileIndexes.Add(item)
        AddNewFileIndex = (SaveChanges() > 0)
    End Function

    Private Function GetIndexId(indexName As String) As Integer
        Dim query = From r In dbs.StIndexes Where r.IndexName = indexName
        If query.Count() > 0 Then
            Dim record = query.FirstOrDefault()
            GetIndexId = record.Id
        Else
            Dim item As StIndex = dbs.StIndexes.Create()
            item.Creation = Now
            item.CreationUser = Environment.UserName
            item.RowVersion = 1
            item.IndexName = indexName
            dbs.StIndexes.Add(item)
            SaveChanges()
            GetIndexId = item.Id
        End If
    End Function

    Private Function SaveChanges() As Integer
        Try
            dbs.Database.BeginTransaction()
            SaveChanges = dbs.SaveChanges()
            dbs.Database.CurrentTransaction.Commit()
        Catch ex As Exception
            dbs.Database.CurrentTransaction.Rollback()
            Throw ex
        End Try
    End Function

    Public Sub DeleteTreeItemFile(id As Integer) Implements IDatabaseConnector.DeleteTreeItemFile
        If id <= 0 Then
            Exit Sub
        End If
        Dim item As TreeItemFile = dbs.TreeItemFiles.Find(id)
        If Not item Is Nothing Then
            dbs.TreeItemFiles.Remove(item)
            SaveChanges()
        End If
    End Sub

    Public Sub SaveTreeItemFile(item As OrgManTreeItemFile) Implements IDatabaseConnector.SaveTreeItemFile
        If item.Id <= 0 Then
            Exit Sub
        End If
        Dim record As TreeItemFile = dbs.TreeItemFiles.Find(item.Id)
        If Not record Is Nothing Then
            If record.Filename <> item.Filename Then
                dbs.Database.ExecuteSqlCommand("UPDATE [OrgMan].[TreeItemFiles] SET [Filename] = {0} WHERE [TreeItemId] = {1} AND [Filename] = {2}", item.Filename, item.TreeItemId, record.Filename)
                Exit Sub
            End If
            record.TreeItemId = item.TreeItemId
            record.Filename = item.Filename
            record.LastUpdate = Now
            record.LastUpdateUser = Environment.UserName
            record.RowVersion = record.RowVersion + 1
            SaveChanges()
        End If
    End Sub

    Public Sub MoveTreeItemFile(item As OrgManTreeItemFile, offset As Integer) Implements IDatabaseConnector.MoveTreeItemFile
        Dim query1 = From r In dbs.TreeItemFiles Where r.Id = item.Id
        Dim record1 As TreeItemFile = query1.FirstOrDefault()
        Dim query2 = From r In dbs.TreeItemFiles Where r.TreeItemId = item.TreeItemId And r.SortOrder = (item.SortOrder + offset)
        Dim record2 As TreeItemFile = query2.FirstOrDefault()
        record1.LastUpdate = Now
        record1.LastUpdateUser = Environment.UserName
        record1.RowVersion = record1.RowVersion + 1
        record1.SortOrder = record1.SortOrder + offset
        record2.LastUpdate = Now
        record2.LastUpdateUser = Environment.UserName
        record2.RowVersion = record2.RowVersion + 1
        record2.SortOrder = record2.SortOrder - offset
        SaveChanges()
    End Sub

    Public Function GetTreeItemFile(treeItemId As Integer, filename As String) As OrgManTreeItemFile Implements IDatabaseConnector.GetTreeItemFile
        Dim query = From r In dbs.TreeItemFiles Where r.TreeItemId = treeItemId And r.Filename = filename
        If query.Count() > 0 Then
            GetTreeItemFile = New OrgManTreeItemFile(query.FirstOrDefault())
        Else
            GetTreeItemFile = AddNewTreeItemFile(treeItemId, filename)
        End If
    End Function


End Class
