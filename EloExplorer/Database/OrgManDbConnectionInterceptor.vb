Imports System.Data.Common
Imports System.Data.Entity.Infrastructure.Interception
Imports System.Data.SqlClient

Public Class OrgManDbConnectionInterceptor
    Implements IDbConnectionInterceptor
    Private ReadOnly _appRole As String
    Private ReadOnly _password As String
    Private _cookie(8000) As Byte
    Private useCookie As Boolean = True

    Public Sub New()

    End Sub

    Public Sub New(appRole As String, password As String)
        _appRole = appRole
        _password = password
    End Sub

    Public Sub BeginningTransaction(connection As DbConnection, interceptionContext As BeginTransactionInterceptionContext) Implements IDbConnectionInterceptor.BeginningTransaction

    End Sub

    Public Sub BeganTransaction(connection As DbConnection, interceptionContext As BeginTransactionInterceptionContext) Implements IDbConnectionInterceptor.BeganTransaction

    End Sub

    Public Sub Closing(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext) Implements IDbConnectionInterceptor.Closing
        If (connection.State <> ConnectionState.Open) Then
            Exit Sub
        End If
        If (String.IsNullOrEmpty(_appRole)) Then
            Exit Sub
        End If
        DeActivateApplicationRole(connection, _cookie)
    End Sub

    Private Sub DeActivateApplicationRole(dbConn As DbConnection, cookie() As Byte)
        Dim cmd = dbConn.CreateCommand()

        cmd.CommandText = "sp_unsetapprole"
        cmd.CommandType = CommandType.StoredProcedure
        Dim paramEnableCookie = New SqlParameter()
        paramEnableCookie.Direction = ParameterDirection.Input
        paramEnableCookie.ParameterName = "@cookie"
        paramEnableCookie.Value = cookie
        cmd.Parameters.Add(paramEnableCookie)
        cmd.ExecuteNonQuery()

        cmd.Dispose()
    End Sub

    Public Sub Closed(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext) Implements IDbConnectionInterceptor.Closed

    End Sub

    Private Function GetDatabaseName(database As String) As String
        GetDatabaseName = ""
        If (String.IsNullOrEmpty(database)) Then Exit Function

        GetDatabaseName = "SEE_" & EloExplorerGlobals.AppEnvironment

    End Function
    Private Function GetDatasourceName(datasource As String) As String
        GetDatasourceName = ""
        If (String.IsNullOrEmpty(datasource)) Then Exit Function

        If EloExplorerGlobals.AppEnvironment.ToLower() = "local" Then
            GetDatasourceName = "localhost"
        Else
            GetDatasourceName = "seeland-db.gseeland.de"
        End If

    End Function

    Public Sub ConnectionStringGetting(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of String)) Implements IDbConnectionInterceptor.ConnectionStringGetting
        Dim database As String = GetDatabaseName(connection.Database)
        If (Not String.IsNullOrEmpty(database) And Not connection.ConnectionString.Contains("initial catalog=" + database)) Then
            Dim pos As Integer = connection.ConnectionString.IndexOf("initial catalog=") + 16
            connection.ConnectionString = connection.ConnectionString.Substring(0, pos) + database + connection.ConnectionString.Substring(connection.ConnectionString.IndexOf(";", pos))
        End If
        Dim datasource As String = GetDatasourceName(connection.DataSource)
        If (Not String.IsNullOrEmpty(datasource) And Not connection.ConnectionString.Contains("data source=" + datasource)) Then
            Dim pos As Integer = connection.ConnectionString.IndexOf("data source=") + 12
            connection.ConnectionString = connection.ConnectionString.Substring(0, pos) + datasource + connection.ConnectionString.Substring(connection.ConnectionString.IndexOf(";", pos))
        End If
    End Sub

    Public Sub ConnectionStringGot(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of String)) Implements IDbConnectionInterceptor.ConnectionStringGot

    End Sub

    Public Sub ConnectionStringSetting(connection As DbConnection, interceptionContext As DbConnectionPropertyInterceptionContext(Of String)) Implements IDbConnectionInterceptor.ConnectionStringSetting

    End Sub

    Public Sub ConnectionStringSet(connection As DbConnection, interceptionContext As DbConnectionPropertyInterceptionContext(Of String)) Implements IDbConnectionInterceptor.ConnectionStringSet

    End Sub

    Public Sub ConnectionTimeoutGetting(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of Integer)) Implements IDbConnectionInterceptor.ConnectionTimeoutGetting

    End Sub

    Public Sub ConnectionTimeoutGot(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of Integer)) Implements IDbConnectionInterceptor.ConnectionTimeoutGot

    End Sub

    Public Sub DatabaseGetting(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of String)) Implements IDbConnectionInterceptor.DatabaseGetting

    End Sub

    Public Sub DatabaseGot(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of String)) Implements IDbConnectionInterceptor.DatabaseGot

    End Sub

    Public Sub DataSourceGetting(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of String)) Implements IDbConnectionInterceptor.DataSourceGetting

    End Sub

    Public Sub DataSourceGot(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of String)) Implements IDbConnectionInterceptor.DataSourceGot

    End Sub

    Public Sub Disposing(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext) Implements IDbConnectionInterceptor.Disposing

    End Sub

    Public Sub Disposed(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext) Implements IDbConnectionInterceptor.Disposed

    End Sub

    Public Sub EnlistingTransaction(connection As DbConnection, interceptionContext As EnlistTransactionInterceptionContext) Implements IDbConnectionInterceptor.EnlistingTransaction

    End Sub

    Public Sub EnlistedTransaction(connection As DbConnection, interceptionContext As EnlistTransactionInterceptionContext) Implements IDbConnectionInterceptor.EnlistedTransaction

    End Sub

    Public Sub Opening(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext) Implements IDbConnectionInterceptor.Opening

    End Sub

    Public Sub Opened(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext) Implements IDbConnectionInterceptor.Opened
        If (connection.State <> ConnectionState.Open) Then
            Exit Sub
        End If
        If (String.IsNullOrEmpty(_appRole)) Then
            Exit Sub
        End If
        ActivateApplicationRole(connection, _appRole, _password)
    End Sub

    Private Sub ActivateApplicationRole(dbConn As DbConnection, appRoleName As String, password As String)
        If (dbConn Is Nothing) Then
            Throw New ArgumentNullException("DbConnection")
        End If
        If (dbConn.State <> ConnectionState.Open) Then
            Throw New InvalidOperationException("DBConnection must be opened before activating application role")
        End If
        If (String.IsNullOrEmpty(appRoleName)) Then
            Throw New ArgumentNullException("appRoleName")
        End If
        If (String.IsNullOrEmpty(password)) Then
            Throw New ArgumentNullException("password")
        End If
        SetApplicationRole(dbConn, appRoleName, password)
    End Sub

    Private Sub SetApplicationRole(dbConn As DbConnection, appRoleName As String, password As String)
        Dim currentUser = GetCurrentUserName(dbConn)
        Dim cmd = dbConn.CreateCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "sp_setapprole"
        cmd.Parameters.Add(New SqlParameter("@rolename", appRoleName))
        cmd.Parameters.Add(New SqlParameter("@password", password))

        cmd.Parameters.Add(New SqlParameter("@fCreateCookie", SqlDbType.Bit) With {.Value = useCookie})

        Dim paramEnableCookie As SqlParameter = Nothing
        If (useCookie) Then
            paramEnableCookie = New SqlParameter()
            paramEnableCookie.ParameterName = "@cookie"
            paramEnableCookie.DbType = DbType.Binary
            paramEnableCookie.Direction = ParameterDirection.Output
            paramEnableCookie.Size = 8000
            cmd.Parameters.Add(paramEnableCookie)
        End If
        cmd.ExecuteNonQuery()
        If (useCookie) Then
            If (paramEnableCookie.Value Is Nothing) Then
                Throw New InvalidOperationException(
                            "Failed to set application role, verify the database is configure correctly and the application role name / passwordis valid.")
            End If
            _cookie = paramEnableCookie.Value
        End If
        cmd.Dispose()

        Dim appUserName = GetCurrentUserName(dbConn)
        'The New user name should be the application role And Not the app pool account.

        If (String.Compare(currentUser, appUserName, True) = 0) Then
            Throw New InvalidOperationException(
                    "Failed to set MediaTypeNames.Application Role, verify the app role is configure correctly or the web configuration is valid.")
        End If
    End Sub

    Private Function GetCurrentUserName(dbConn As DbConnection) As String
        Dim cmd = dbConn.CreateCommand()
        cmd.CommandText = "SELECT USER_NAME();"
        GetCurrentUserName = cmd.ExecuteScalar()
        cmd.Dispose()
    End Function

    Public Sub ServerVersionGetting(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of String)) Implements IDbConnectionInterceptor.ServerVersionGetting

    End Sub

    Public Sub ServerVersionGot(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of String)) Implements IDbConnectionInterceptor.ServerVersionGot

    End Sub

    Public Sub StateGetting(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of ConnectionState)) Implements IDbConnectionInterceptor.StateGetting

    End Sub

    Public Sub StateGot(connection As DbConnection, interceptionContext As DbConnectionInterceptionContext(Of ConnectionState)) Implements IDbConnectionInterceptor.StateGot

    End Sub
End Class
