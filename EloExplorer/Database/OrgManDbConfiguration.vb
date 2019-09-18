Imports System.Data.Entity

Public Class OrgManDbConfiguration
    Inherits DbConfiguration

    Protected Friend Sub New()
        If My.Settings.UseApplicationRole Then
            AddInterceptor(New OrgManDbConnectionInterceptor(OrgManGlobals.ApplicationRole, OrgManGlobals.ApplicationPassword))
        Else
            AddInterceptor(New OrgManDbConnectionInterceptor())
        End If
    End Sub
End Class
