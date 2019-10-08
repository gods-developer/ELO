Imports System.Data.Entity

Public Class OrgManDbConfiguration
    Inherits DbConfiguration

    Protected Friend Sub New()
        If My.Settings.UseApplicationRole Then
            AddInterceptor(New OrgManDbConnectionInterceptor(ArcExplorerGlobals.ApplicationRole, ArcExplorerGlobals.ApplicationPassword))
        Else
            AddInterceptor(New OrgManDbConnectionInterceptor())
        End If
    End Sub
End Class
