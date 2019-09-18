Imports Microsoft.Office.Interop.Access.Dao
Imports OrgMan

Public Class MsAccessConnector
    Implements IDatabaseConnector
    Dim dbe As New Microsoft.Office.Interop.Access.Dao.DBEngine
    Dim dbs As Microsoft.Office.Interop.Access.Dao.Database
    ReadOnly dbPath As String

    Public Sub New()
        If My.Settings.LocalVersion Then
            dbPath = My.Application.Info.DirectoryPath & "\OrgMan.accdb"
        Else
#If DEBUG Then
            dbPath = "C:\Office\Privat\Entwicklung\BOIC\OrganiMan\Database\OrgMan.accdb"
#Else
            dbPath = My.Settings.DatabasePath
#End If
        End If
        dbs = dbe.OpenDatabase(dbPath)
    End Sub

    Public ReadOnly Property Path As String Implements IDatabaseConnector.Path
        Get
            Path = dbPath
        End Get
    End Property

    Public Sub Close() Implements IDatabaseConnector.Close
        dbs?.Close()
    End Sub

    Public Sub DeleteTreeItem(id As Integer) Implements IDatabaseConnector.DeleteTreeItem
        dbs.Execute("delete from TreeItems where [Id] = " + id.ToString(), Microsoft.Office.Interop.Access.Dao.RecordsetOptionEnum.dbFailOnError)
    End Sub

    Public Sub SaveTreeItem(item As OrgManTreeItem) Implements IDatabaseConnector.SaveTreeItem
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset
        rst = dbs.OpenRecordset("select * from TreeItems where [Id] = " & item.Id, Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        rst.Edit()
        rst.Fields("NodeText").Value = item.NodeText
        rst.Update()
        rst.Close()
    End Sub

    Public Sub SaveRootPath(item As OrgManTreeItem) Implements IDatabaseConnector.SaveRootPath
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset
        rst = dbs.OpenRecordset("select * from RootPaths where TreeItemId = " & item.Id, Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        If rst.RecordCount = 0 Then
            rst.AddNew()
            rst.Fields("TreeItemId").Value = item.Id
        Else
            rst.Edit()
        End If
        rst.Fields("RootPath").Value = item.RootPath
        rst.Update()
        rst.Close()
    End Sub

    Public Function GetRootTreeItems() As List(Of OrgManTreeItem) Implements IDatabaseConnector.GetRootTreeItems
        Dim results As New List(Of OrgManTreeItem)
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset = dbs.OpenRecordset("select * from TreeItems where ParentNodeId Is Null order by SortOrder asc", Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        If rst.RecordCount > 0 Then
            rst.MoveFirst()
            Do While Not rst.EOF
                results.Add(New OrgManTreeItem(rst.Fields("Id").Value, rst.Fields("NodeText").Value))
                rst.MoveNext()
            Loop
        End If
        rst.Close()
        GetRootTreeItems = results
    End Function

    Public Function GetChildTreeItems(parentNodeId As Integer) As List(Of OrgManTreeItem) Implements IDatabaseConnector.GetChildTreeItems
        Dim results As New List(Of OrgManTreeItem)
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset = dbs.OpenRecordset("select * from TreeItems where ParentNodeId = " + parentNodeId.ToString + " order by SortOrder asc", Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        If rst.RecordCount > 0 Then
            rst.MoveFirst()
            Do While Not rst.EOF
                results.Add(New OrgManTreeItem(rst.Fields("Id").Value, rst.Fields("NodeText").Value))
                rst.MoveNext()
            Loop
        End If
        rst.Close()
        GetChildTreeItems = results
    End Function

    Public Function GetTreeItem(id As Integer) As OrgManTreeItem
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset
        rst = dbs.OpenRecordset("select * from TreeItems where [Id] = " + id.ToString(), Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        If rst.RecordCount > 0 Then
            GetTreeItem = New OrgManTreeItem(rst.Fields("Id").Value, rst.Fields("NodeText").Value)
        Else
            GetTreeItem = Nothing
        End If
        rst.Close()
    End Function

    Public Function AddNewTreeItem(newName As String, Optional newRootPath As String = "", Optional parentNodeId As Integer = 0) As OrgManTreeItem Implements IDatabaseConnector.AddNewTreeItem
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset
        rst = dbs.OpenRecordset("TreeItems", Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        rst.AddNew()
        rst.Fields("NodeText").Value = newName
        If parentNodeId > 0 Then
            rst.Fields("ParentNodeId").Value = parentNodeId
        End If
        rst.Fields("SortOrder").Value = GetMaxId(parentNodeId) + 10
        rst.Fields("ChildrenSortBy").Value = 0 'Benutzerdefiniert
        AddNewTreeItem = New OrgManTreeItem() 'todo! AddNewTreeItem = rst.Fields("Id").Value
        rst.Update()
        rst.Close()
        If newRootPath <> "" Then
            rst = dbs.OpenRecordset("RootPaths", Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
            rst.AddNew()
            'todo! rst.Fields("TreeItemId").Value = AddNewTreeItem
            rst.Fields("RootPath").Value = newRootPath
            rst.Update()
            rst.Close()
        End If
    End Function

    Public Function GetRootPath(nodeId As Integer) As String Implements IDatabaseConnector.GetRootPath
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset
        rst = dbs.OpenRecordset("select RootPath from RootPaths where TreeItemId = " + nodeId.ToString(), Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        If rst.RecordCount > 0 Then
            GetRootPath = rst.Fields(0).Value
        Else
            GetRootPath = ""
        End If
        rst.Close()
    End Function

    Public Function CheckIfNameExists(parentNodeId As Integer, newName As String) As Boolean Implements IDatabaseConnector.CheckIfNameExists
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset
        rst = dbs.OpenRecordset("select * from TreeItems where ParentNodeId " + IIf(parentNodeId = 0, "Is Null", "= " + parentNodeId.ToString()) + " and NodeText = '" + newName.Replace("'", "''") + "'", Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        CheckIfNameExists = (rst.RecordCount > 0)
        rst.Close()
    End Function

    Private Function GetMaxId(parentNodeId As Integer) As Integer
        Dim rst As Microsoft.Office.Interop.Access.Dao.Recordset
        rst = dbs.OpenRecordset("select Max(SortOrder) from TreeItems where ParentNodeId " + IIf(parentNodeId = 0, "Is Null", "= " + parentNodeId.ToString()), Microsoft.Office.Interop.Access.Dao.RecordsetTypeEnum.dbOpenDynaset)
        If IsNumeric(rst.Fields(0).Value) Then
            GetMaxId = rst.Fields(0).Value
        Else
            GetMaxId = 0
        End If
        rst.Close()
    End Function

    Public Sub MoveTreeItem(item As OrgManTreeItem, offset As Integer) Implements IDatabaseConnector.MoveTreeItem
        Throw New NotImplementedException()
    End Sub

    Public Function GetDbSetting(Key As String, Optional [Default] As String = "") As String Implements IDatabaseConnector.GetDbSetting
        Throw New NotImplementedException()
    End Function

    Public Sub SaveDbSetting(Key As String, Setting As String) Implements IDatabaseConnector.SaveDbSetting
        Throw New NotImplementedException()
    End Sub

    Public Function HasAccess() As Boolean Implements IDatabaseConnector.HasAccess
        Throw New NotImplementedException()
    End Function

    Public Function GetDbUserSetting(Key As String, Optional [Default] As String = "") As String Implements IDatabaseConnector.GetDbUserSetting
        Throw New NotImplementedException()
    End Function

    Public Sub SaveDbUserSetting(Key As String, Setting As String) Implements IDatabaseConnector.SaveDbUserSetting
        Throw New NotImplementedException()
    End Sub

    Public Function IsAdmin() As Boolean Implements IDatabaseConnector.IsAdmin
        Throw New NotImplementedException()
    End Function

    Public Function GetGroupRight(treeItemId As Integer, sid As String) As OrgManEnums.AccessRight Implements IDatabaseConnector.GetGroupRight
        Throw New NotImplementedException()
    End Function

    Public Sub SetGroupRight(treeItemId As Integer, sid As String, accessRight As OrgManEnums.AccessRight) Implements IDatabaseConnector.SetGroupRight
        Throw New NotImplementedException()
    End Sub

    Public Function GetUserRight(treeItemId As Integer, sid As String) As OrgManEnums.AccessRight Implements IDatabaseConnector.GetUserRight
        Throw New NotImplementedException()
    End Function

    Public Sub SetUserRight(treeItemId As Integer, sid As String, accessRight As OrgManEnums.AccessRight) Implements IDatabaseConnector.SetUserRight
        Throw New NotImplementedException()
    End Sub

    Public Function GetGroupRights(treeItemId As Integer) As IList(Of OrgManAccessRight) Implements IDatabaseConnector.GetGroupRights
        Throw New NotImplementedException()
    End Function

    Public Function GetUserRights(treeItemId As Integer) As IList(Of OrgManAccessRight) Implements IDatabaseConnector.GetUserRights
        Throw New NotImplementedException()
    End Function

    Public Function GetReminderDate(treeItemId As Integer, filename As String) As Date Implements IDatabaseConnector.GetReminderDate
        Throw New NotImplementedException()
    End Function

    Public Sub SetReminderDate(treeItemId As Integer, filename As String, reminderDate As Date) Implements IDatabaseConnector.SetReminderDate
        Throw New NotImplementedException()
    End Sub

    Public Sub DeleteReminderDate(treeItemId As Integer, filename As String) Implements IDatabaseConnector.DeleteReminderDate
        Throw New NotImplementedException()
    End Sub

    Public Function GetReminders() As List(Of OrgManReminder) Implements IDatabaseConnector.GetReminders
        Throw New NotImplementedException()
    End Function

    Public Sub FinishReminder(treeItemId As Integer, filename As String) Implements IDatabaseConnector.FinishReminder
        Throw New NotImplementedException()
    End Sub

    Public Function AddNewTreeItemFile(treeItemId As Integer, filename As String) As OrgManTreeItemFile Implements IDatabaseConnector.AddNewTreeItemFile
        Throw New NotImplementedException()
    End Function

    Public Sub DeleteTreeItemFile(id As Integer) Implements IDatabaseConnector.DeleteTreeItemFile
        Throw New NotImplementedException()
    End Sub

    Public Sub SaveTreeItemFile(item As OrgManTreeItemFile) Implements IDatabaseConnector.SaveTreeItemFile
        Throw New NotImplementedException()
    End Sub

    Public Sub MoveTreeItemFile(item As OrgManTreeItemFile, offset As Integer) Implements IDatabaseConnector.MoveTreeItemFile
        Throw New NotImplementedException()
    End Sub

    Public Function GetTreeItemFile(treeItemId As Integer, filename As String) As OrgManTreeItemFile Implements IDatabaseConnector.GetTreeItemFile
        Throw New NotImplementedException()
    End Function
End Class
