Public Class OrgManFileInfo
    Public FilePath As String
    Public Filename As String
    Public FileLen As Long
    Public FileDateTime As Date
    Public FileType As String
    Public ReadOnly Property SortOrder() As Integer
        Get
            Return TreeItemFile?.SortOrder
        End Get
    End Property

    Public TreeItemFile As OrgManTreeItemFile
End Class
