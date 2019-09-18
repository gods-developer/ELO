Imports EloExplorer.EloExplorer
Imports OrgMan.OrgMan

Public Class OrgManTreeItemFile
    Public Id As Integer
    Public TreeItemId As Integer
    Public Filename As String
    Public SortOrder As Integer
    Public Creation As DateTime
    Public CreationUser As String
    Public LastUpdate As DateTime?
    Public LastUpdateUser As String
    Public RowVersion As Integer

    Public Sub New(Optional item As TreeItemFile = Nothing)
        If item Is Nothing Then Exit Sub
        Me.Id = item.Id
        Me.TreeItemId = item.TreeItemId
        Me.Filename = item.Filename
        Me.SortOrder = item.SortOrder
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
