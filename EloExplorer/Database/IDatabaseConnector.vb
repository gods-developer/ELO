Public Interface IDatabaseConnector
    ReadOnly Property Path As String
    Function GetRootTreeItems() As List(Of OrgManTreeItem)
    Function GetChildTreeItems(parentNodeId As Integer) As List(Of OrgManTreeItem)
    Function AddNewTreeItem(newName As String, Optional newRootPath As String = "", Optional parentNodeId As Integer = 0) As OrgManTreeItem
    Function GetRootPath(nodeId As Integer) As String
    Function CheckIfNameExists(parentNodeId As Integer, newName As String) As Boolean
    Sub DeleteTreeItem(id As Integer)
    Sub SaveTreeItem(item As OrgManTreeItem)
    Sub SaveRootPath(item As OrgManTreeItem)
    Sub MoveTreeItem(item As OrgManTreeItem, offset As Integer)
    Function GetDbSetting(Key As String, Optional [Default] As String = "") As String
    Function GetDbUserSetting(Key As String, Optional [Default] As String = "") As String
    Sub SaveDbSetting(Key As String, Setting As String)
    Sub SaveDbUserSetting(Key As String, Setting As String)
    Function HasAccess() As Boolean
    Function IsAdmin() As Boolean
    Function GetGroupRight(treeItemId As Integer, sid As String) As OrgManEnums.AccessRight
    Sub SetGroupRight(treeItemId As Integer, sid As String, accessRight As OrgManEnums.AccessRight)
    Function GetUserRight(treeItemId As Integer, sid As String) As OrgManEnums.AccessRight
    Sub SetUserRight(treeItemId As Integer, sid As String, accessRight As OrgManEnums.AccessRight)
    Function GetGroupRights(treeItemId As Integer) As IList(Of OrgManAccessRight)
    Function GetUserRights(treeItemId As Integer) As IList(Of OrgManAccessRight)
    Function GetReminderDate(treeItemId As Integer, filename As String) As DateTime
    Sub SetReminderDate(treeItemId As Integer, filename As String, reminderDate As DateTime)
    Sub DeleteReminderDate(treeItemId As Integer, filename As String)
    Sub FinishReminder(treeItemId As Integer, filename As String)
    Function GetReminders() As List(Of OrgManReminder)
    Function AddNewTreeItemFile(treeItemId As Integer, filename As String) As OrgManTreeItemFile
    Sub DeleteTreeItemFile(id As Integer)
    Sub SaveTreeItemFile(item As OrgManTreeItemFile)
    Sub MoveTreeItemFile(item As OrgManTreeItemFile, offset As Integer)
    Function GetTreeItemFile(treeItemId As Integer, filename As String) As OrgManTreeItemFile
    Sub Close()
End Interface
