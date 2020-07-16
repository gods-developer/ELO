Imports System.Data.Entity.Core.Objects
Imports System.Data.Entity.Infrastructure
Imports Microsoft.Office.Interop.Access.Dao
Imports ArcExplorer
Imports DigiSped.Common.Tools
Imports ArcExplorer.ArcExplorer

Public Class MsSqlConnector
    Dim dbs As ArcExplorer.OrgManEntities
    Private _mySid As String, _myGroupSids As List(Of String), adminState As Integer

    Public Sub New()
        '        If My.Settings.LocalVersion Then
        '            dbPath = My.Application.Info.DirectoryPath & "\OrgMan.accdb"
        '        Else
#If DEBUG Then
        dbs = New ArcExplorer.OrgManEntities()
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

    Public ReadOnly Property Path As String
        Get
            Dim dbConnectionString As String = dbs.Database.Connection.ConnectionString
            Path = GetConnectionStringValue(dbConnectionString, "data source") + "\" + GetConnectionStringValue(dbConnectionString, "initial catalog")
        End Get
    End Property

    Public ReadOnly Property DataSource As String
        Get
            Dim dbConnectionString As String = dbs.Database.Connection.ConnectionString
            DataSource = GetConnectionStringValue(dbConnectionString, "data source")
        End Get
    End Property

    Public ReadOnly Property InitialCatalog As String
        Get
            Dim dbConnectionString As String = dbs.Database.Connection.ConnectionString
            InitialCatalog = GetConnectionStringValue(dbConnectionString, "initial catalog")
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

    Public Sub Close()
        dbs?.Dispose()
    End Sub

    Public Sub DeleteTreeItem(id As Integer)
        Dim item As TreeItem = dbs.TreeItems.Find(id) 'From r In dbs.TreeItems Where r.Id = id
        If Not item Is Nothing Then
            dbs.TreeItems.Remove(item)
            SaveChanges()
        End If
    End Sub

    Public Sub SaveTreeItem(item As OrgManTreeItem)
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

    Public Sub SaveRootPath(item As OrgManTreeItem)
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

    Public Sub MoveTreeItem(item As OrgManTreeItem, offset As Integer)
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

    Public Function GetRootTreeItems() As List(Of OrgManTreeItem)
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

    Public Function GetChildTreeItems(parentNodeId As Integer) As List(Of OrgManTreeItem)
        Dim results As New List(Of OrgManTreeItem)
        Dim query = From r In dbs.TreeItems Where r.ParentNodeId = parentNodeId Order By r.SortOrder
        For Each treeItem In query
            results.Add(New OrgManTreeItem(parentNodeId, treeItem))
        Next
        GetChildTreeItems = results
    End Function
    Public Function GetOrAddNewTreeItem(newName As String, Optional newText As String = "", Optional newRootPath As String = "", Optional parentNodeId As Integer = 0, Optional ByRef created As Boolean = False) As OrgManTreeItem
        Dim query = From r In dbs.TreeItems Where r.NodeName = newName And r.NodeText = newText
        If query.Count() > 0 Then
            GetOrAddNewTreeItem = New OrgManTreeItem(parentNodeId, query.First()) With {.RootPath = newRootPath}
        Else
            GetOrAddNewTreeItem = AddNewTreeItem(newName, newText, newRootPath, parentNodeId)
            created = True
        End If
    End Function

    Private lastParentNodeId As Integer = -1, lastMaxTreeItemId As Integer

    Private Function GetMaxTreeItemIdEx(parentNodeId As Integer) As Integer
        If lastParentNodeId <> parentNodeId Then
            lastMaxTreeItemId = GetMaxTreeItemId(parentNodeId)
        End If
        lastParentNodeId = parentNodeId
        lastMaxTreeItemId = lastMaxTreeItemId + 10
        GetMaxTreeItemIdEx = lastMaxTreeItemId
    End Function

    Public Function AddNewTreeItem(newName As String, Optional newText As String = "", Optional newRootPath As String = "", Optional parentNodeId As Integer = 0) As OrgManTreeItem
        Dim treeItem As TreeItem = dbs.TreeItems.Create()
        treeItem.Creation = Now
        treeItem.CreationUser = Environment.UserName
        treeItem.RowVersion = 1
        treeItem.NodeName = newName
        If newText <> newName Then
            treeItem.NodeText = newText
        End If
        If parentNodeId > 0 Then
            treeItem.ParentNodeId = parentNodeId
        End If
        treeItem.SortOrder = GetMaxTreeItemIdEx(parentNodeId) + 10
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

    Public Function GetRootPath(nodeId As Integer) As String
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

    Public Function CheckIfNameExists(parentNodeId As Integer, newName As String) As Boolean
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

    Private Function GetMaxListItemId(treeItemId As Integer) As Integer
        DirectCast(dbs, IObjectContextAdapter).ObjectContext.Refresh(RefreshMode.StoreWins, dbs.ListItems)
        Dim query = (From r In dbs.ListItems Where r.TreeItemId = treeItemId Select r.SortOrder)
        Dim maxId As Integer
        If query.Any() Then
            maxId = query.Max()
        End If
        GetMaxListItemId = maxId
    End Function

    Public Function GetDbSetting(Key As String, Optional [Default] As String = "") As String
        Dim query = From r In dbs.Settings Where r.SettingName = Key
        If query.Count() > 0 Then
            GetDbSetting = query.FirstOrDefault().SettingValue
        Else
            GetDbSetting = [Default]
        End If
    End Function

    Public Sub SaveDbSetting(Key As String, Setting As String)
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

    Public Function HasAccess() As Boolean
        Dim query = From r In dbs.AppUsers Where r.UserDomain.ToLower() = Environment.UserDomainName.ToLower() And r.UserName.ToLower() = Environment.UserName.ToLower()
        HasAccess = query.Count() = 1
    End Function

    Public Function GetDbUserSetting(Key As String, Optional [Default] As String = "") As String
        Dim query = From r In dbs.AppUserSettings Where r.AppUser.UserDomain.ToLower() = Environment.UserDomainName.ToLower() And r.AppUser.UserName.ToLower() = Environment.UserName.ToLower() And r.SettingName = Key
        If query.Count() > 0 Then
            GetDbUserSetting = query.FirstOrDefault().SettingValue
        Else
            GetDbUserSetting = [Default]
        End If
    End Function

    Public Sub SaveDbUserSetting(Key As String, Setting As String)
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

    Public Function IsAdmin() As Boolean
        If adminState = 0 Then
            Dim query = From r In dbs.AppUsers Where r.UserDomain.ToLower() = Environment.UserDomainName.ToLower() And r.UserName.ToLower() = Environment.UserName.ToLower() And r.IsAdmin = True
            adminState = IIf(query.Count() = 0, -1, 1)
        End If
        IsAdmin = adminState = 1
    End Function

    Public Function GetGroupRight(treeItemId As Integer, sid As String) As OrgManEnums.AccessRight
        Dim query = From r In dbs.TreeItemGroupRights Where r.GroupId = sid And r.TreeItemId = treeItemId
        If query.Count() > 0 Then
            GetGroupRight = query.FirstOrDefault().AccessRight
        Else
            GetGroupRight = OrgManEnums.AccessRight.NoAccess
        End If
    End Function

    Public Sub SetGroupRight(treeItemId As Integer, sid As String, accessRight As OrgManEnums.AccessRight)
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

    Public Function GetUserRight(treeItemId As Integer, sid As String) As OrgManEnums.AccessRight
        Dim query = From r In dbs.TreeItemUserRights Where r.UserId = sid And r.TreeItemId = treeItemId
        If query.Count() > 0 Then
            GetUserRight = query.FirstOrDefault().AccessRight
        Else
            GetUserRight = OrgManEnums.AccessRight.NoAccess
        End If
    End Function

    Public Sub SetUserRight(treeItemId As Integer, sid As String, accessRight As OrgManEnums.AccessRight)
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

    Public Function GetGroupRights(treeItemId As Integer) As IList(Of OrgManAccessRight)
        Dim results = New List(Of OrgManAccessRight)
        Dim query = From r In dbs.TreeItemGroupRights Where r.TreeItemId = treeItemId
        If query.Count() > 0 Then
            For Each rec In query
                results.Add(New OrgManAccessRight(rec.GroupId, rec.AccessRight))
            Next
        End If
        GetGroupRights = results
    End Function

    Public Function GetUserRights(treeItemId As Integer) As IList(Of OrgManAccessRight)
        Dim results = New List(Of OrgManAccessRight)
        Dim query = From r In dbs.TreeItemUserRights Where r.TreeItemId = treeItemId
        If query.Count() > 0 Then
            For Each rec In query
                results.Add(New OrgManAccessRight(rec.UserId, rec.AccessRight))
            Next
        End If
        GetUserRights = results
    End Function

    Private lastTreeItemId As Integer, lastMaxListItemId As Integer

    Private Function GetMaxListItemIdEx(treeItemId As Integer) As Integer
        If lastTreeItemId <> treeItemId Then
            lastMaxListItemId = GetMaxListItemId(treeItemId)
        End If
        lastTreeItemId = treeItemId
        lastMaxListItemId = lastMaxListItemId + 10
        GetMaxListItemIdEx = lastMaxListItemId
    End Function

    Public Function AddNewListItem(treeItemId As Integer, filename As String, Optional displayname As String = "") As OrgManListItem
        If treeItemId <= 0 Or filename Is Nothing Then
            AddNewListItem = Nothing
            Exit Function
        End If
        Dim item As ListItem = dbs.ListItems.Create()
        item.Creation = Now
        item.CreationUser = Environment.UserName
        item.RowVersion = 1
        item.Filename = filename
        item.Displayname = displayname
        item.TreeItemId = treeItemId
        item.SortOrder = GetMaxListItemIdEx(treeItemId)
        dbs.ListItems.Add(item)
        If SaveChanges() > 0 Then
            Dim result = New OrgManListItem(item)
            AddNewListItem = result
        Else
            AddNewListItem = Nothing
        End If
    End Function

    Public Function AddNewListItemIndex(fileId As Integer, indexName As String, indexValue As String, noSave As Boolean) As Boolean
        If String.IsNullOrEmpty(indexValue) Then
            Exit Function
        End If
        Dim indexId = GetIndexId(indexName)
        Dim item As ListItemIndex = dbs.ListItemIndexes.Create()
        item.Creation = Now
        item.CreationUser = Environment.UserName
        item.RowVersion = 1
        item.ListItemId = fileId
        item.IndexId = indexId
        item.IndexValue = indexValue
        dbs.ListItemIndexes.Add(item)
        If Not noSave Then
            AddNewListItemIndex = (SaveChanges() > 0)
        End If
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

    Public Function SaveChanges() As Integer
        Try
            dbs.Database.BeginTransaction()
            SaveChanges = dbs.SaveChanges()
            dbs.Database.CurrentTransaction.Commit()
        Catch ex As Exception
            dbs.Database.CurrentTransaction.Rollback()
            Throw ex
        End Try
    End Function

    Public Sub DeleteListItem(id As Integer)
        If id <= 0 Then
            Exit Sub
        End If
        Dim item As ListItem = dbs.ListItems.Find(id)
        If Not item Is Nothing Then
            dbs.ListItems.Remove(item)
            SaveChanges()
        End If
    End Sub

    Public Sub SaveListItem(item As OrgManListItem)
        If item.Id <= 0 Then
            Exit Sub
        End If
        Dim record As ListItem = dbs.ListItems.Find(item.Id)
        If Not record Is Nothing Then
            If record.Filename <> item.Filename Then
                dbs.Database.ExecuteSqlCommand("UPDATE [OrgMan].[ListItems] SET [Filename] = {0} WHERE [TreeItemId] = {1} AND [Filename] = {2}", item.Filename, item.TreeItemId, record.Filename)
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

    Public Sub MoveListItem(item As OrgManListItem, offset As Integer)
        Dim query1 = From r In dbs.ListItems Where r.Id = item.Id
        Dim record1 As ListItem = query1.FirstOrDefault()
        Dim query2 = From r In dbs.ListItems Where r.TreeItemId = item.TreeItemId And r.SortOrder = (item.SortOrder + offset)
        Dim record2 As ListItem = query2.FirstOrDefault()
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

    Public Function GetListItem(treeItemId As Integer, filename As String) As OrgManListItem
        Dim query = From r In dbs.ListItems Where r.TreeItemId = treeItemId And r.Filename = filename
        If query.Count() > 0 Then
            GetListItem = New OrgManListItem(query.FirstOrDefault())
        Else
            GetListItem = AddNewListItem(treeItemId, filename)
        End If
    End Function


End Class
