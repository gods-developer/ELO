Imports System.Data.Entity

Public Class OrgManDbConfiguration
    Inherits DbConfiguration

    Protected Friend Sub New()
        If My.Settings.UseApplicationRole Then
            AddInterceptor(New OrgManDbConnectionInterceptor(EloExplorerGlobals.ApplicationRole, EloExplorerGlobals.ApplicationPassword))
        Else
            AddInterceptor(New OrgManDbConnectionInterceptor())
        End If
    End Sub
End Class
