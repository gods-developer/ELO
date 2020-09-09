Imports ArcExplorer.ArcExplorer

Public Class OrgManTreeItem
    Public Id As Integer
    Public ParentId As Integer?
    Public Node As TreeNode
    Public NodeName As String
    Public NodeText As String
    Public RootPath As String
    Public SortOrder As Integer
    Public ChildrenSortBy As OrgManEnums.FoldersSortBy
    Public ChildrenSortWay As OrgManEnums.SortWay
    Public FilesSortBy As OrgManEnums.FilesSortBy
    Public FilesSortWay As OrgManEnums.SortWay
    Public Creation As DateTime
    Public CreationUser As String
    Public LastUpdate As DateTime?
    Public LastUpdateUser As String
    Public RowVersion As Integer

    Public Sub New(Optional parentId As Integer? = Nothing, Optional item As TreeItem = Nothing)
        If item Is Nothing Then Exit Sub
        Me.Id = item.Id
        Me.ParentId = parentId
        Me.NodeName = item.NodeName
        Me.NodeText = item.NodeText
        If item.RootPaths.Any() Then
            Me.RootPath = item.RootPaths(0).RootPath1
        End If
        Me.SortOrder = item.SortOrder
        Me.ChildrenSortBy = item.ChildrenSortBy
        Me.ChildrenSortWay = item.ChildrenSortWay
        Me.FilesSortBy = item.FilesSortBy
        Me.FilesSortWay = item.FilesSortWay
        Me.Creation = item.Creation
        Me.CreationUser = item.CreationUser
        If Not item.LastUpdate Is Nothing Then
            Me.LastUpdate = item.LastUpdate
        End If
        If Not item.LastUpdateUser Is Nothing Then
            Me.LastUpdateUser = item.LastUpdateUser
        End If
        Me.RowVersion = item.RowVersion
    End Sub
End Class
