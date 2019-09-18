Public Class OrgManAccessRight
    Private _sid As String

    Public Property Sid() As String
        Get
            Return _sid
        End Get
        Private Set(ByVal value As String)
            _sid = value
        End Set
    End Property

    Private _accessRight As OrgManEnums.AccessRight

    Public Property AccessRight() As OrgManEnums.AccessRight
        Get
            Return _accessRight
        End Get
        Private Set(ByVal value As OrgManEnums.AccessRight)
            _accessRight = value
        End Set
    End Property

    Public Sub New(sid As String, accessRight As OrgManEnums.AccessRight)
        _sid = sid
        _accessRight = accessRight
    End Sub

End Class
