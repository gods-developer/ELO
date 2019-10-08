Public Class ArcTreeItem
    Public Guid As String
    Public PhysicalName As String
    Public PhysicalDate As Date
    Public Sub New(guid As String, physicalName As String, physicalDate As String)
        Me.Guid = guid
        Me.PhysicalName = physicalName
        DateTime.TryParse(physicalDate, Me.PhysicalDate)
    End Sub

End Class
