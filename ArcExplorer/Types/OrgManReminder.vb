Imports OrgMan.OrgMan

Public Class OrgManReminder

    Private _treeItemId As Integer

    Public Property TreeItemId() As Integer
        Get
            Return _treeItemId
        End Get
        Private Set(ByVal value As Integer)
            _treeItemId = value
        End Set
    End Property

    Private _filename As String

    Public Property Filename() As String
        Get
            Return _filename
        End Get
        Private Set(ByVal value As String)
            _filename = value
        End Set
    End Property

    Private _reminderDate As DateTime

    Public Property ReminderDate() As DateTime
        Get
            Return _reminderDate
        End Get
        Private Set(ByVal value As DateTime)
            _reminderDate = value
        End Set
    End Property

    Public Sub New(treeItemId As Integer, filename As String, reminderDate As DateTime)
        _treeItemId = treeItemId
        _filename = filename
        _reminderDate = reminderDate
    End Sub
End Class
