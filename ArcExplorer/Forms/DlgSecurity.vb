Imports System.Threading
Imports System.Windows.Forms
Imports DigiSped.Common.Tools

Public Class DlgSecurity
    Dim adcon As New WinAdConnector()
    Dim lastSelectedGroupId As String
    Private ReadOnly Property OwnerForm() As FrmMain
        Get
            Return CType(Me.Owner, FrmMain)
        End Get
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub DlgSecurity_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboGroupRights.SelectedIndex = 0
        ComboMemberRights.SelectedIndex = 0
        TextGroupFilter.Text = ""
        TextMemberFilter.Text = ""
        If Not adcon.DomainIsAvailable() Then
            TimerLoading.Enabled = False
            TextGroupFilter.Enabled = False
            TextMemberFilter.Enabled = False
            ComboGroupRights.Enabled = False
            ComboMemberRights.Enabled = False
            MsgBox("Die Windows-Domäne ist momentan nicht erreichbar!", MsgBoxStyle.Exclamation, "Hinweis")
            Exit Sub
        End If
    End Sub

    Private Sub FillGroups()
        If OwnerForm.AdGroups Is Nothing Then
            Exit Sub
        End If
        lastSelectedGroupId = ""
        If ListAdGroups.SelectedIndex >= 0 Then
            FillUsers()
        End If
        ComboGroupRights.SelectedIndex = 0
        ListAdGroups.Items.Clear()
        Dim grp As WinAdGroupPrincipal
        For Each grp In OwnerForm.AdGroups.Where(Function(g) g.ToString().ToLower() Like "*" & TextGroupFilter.Text.ToLower() & "*")
            ListAdGroups.Items.Add(grp.ToString())
        Next
    End Sub

    Private Function GetGroupIdFromName(groupName As String) As String
        GetGroupIdFromName = ""
        If groupName Is Nothing Or groupName = "" Then
            Exit Function
        Else
            For Each grp In OwnerForm.AdGroups
                If grp.ToString() = groupName Then
                    GetGroupIdFromName = grp.Id
                    Exit For
                End If
            Next
        End If
    End Function

    Private Function GetGroupNameFromId(groupId As String) As String
        GetGroupNameFromId = ""
        If groupId Is Nothing Or groupId = "" Then
            Exit Function
        Else
            For Each grp In OwnerForm.AdGroups
                If grp.Id = groupId Then
                    GetGroupNameFromId = grp.ToString()
                    Exit For
                End If
            Next
        End If
    End Function

    Private Function GetUserIdFromName(userName As String) As String
        GetUserIdFromName = ""
        If userName Is Nothing Or userName = "" Then
            Exit Function
        Else
            For Each usr In OwnerForm.AdUsers
                If usr.ToString() = userName Then
                    GetUserIdFromName = usr.Id
                    Exit For
                End If
            Next
        End If
    End Function

    Private Function GetUserNameFromId(userId As String) As String
        GetUserNameFromId = ""
        If userId Is Nothing Or userId = "" Then
            Exit Function
        Else
            For Each usr In OwnerForm.AdUsers
                If usr.Id = userId Then
                    GetUserNameFromId = usr.ToString()
                    Exit For
                End If
            Next
        End If
    End Function

    Private Sub ListAdGroups_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListAdGroups.SelectedIndexChanged
        Dim gid As String = GetGroupIdFromName(ListAdGroups.SelectedItem?.ToString())
        If gid = lastSelectedGroupId Then
            Exit Sub
        ElseIf gid = "" Or gid <> lastSelectedGroupId Then
            ComboMemberRights.SelectedIndex = 0
            ListAdMembers.Items.Clear()
            Me.Refresh()
        End If
        If gid = "" Then
            Exit Sub
        End If
        If Not OwnerForm.AdMembers.ContainsKey(gid) Then
            Me.Cursor = Cursors.AppStarting
            OwnerForm.AdMembers.Add(gid, adcon.GetMembers(ListAdGroups.SelectedItem))
        End If
        FillMembers()
        GetGroupRight(gid)
        lastSelectedGroupId = gid
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub GetGroupRight(gid As String)
        If gid = "" Then
            Exit Sub
        End If
        ComboGroupRights.SelectedIndex = OwnerForm.dbc.GetGroupRight(txtId.Text, gid)
    End Sub

    Private Sub SetGroupRight(gid As String)
        If gid = "" Then
            Exit Sub
        End If
        OwnerForm.dbc.SetGroupRight(txtId.Text, gid, ComboGroupRights.SelectedIndex)
    End Sub

    Private Sub GetUserRight(gid As String)
        If gid = "" Then
            Exit Sub
        End If
        ComboMemberRights.SelectedIndex = OwnerForm.dbc.GetUserRight(txtId.Text, gid)
    End Sub

    Private Sub SetUserRight(gid As String)
        If gid = "" Then
            Exit Sub
        End If
        OwnerForm.dbc.SetUserRight(txtId.Text, gid, ComboMemberRights.SelectedIndex)
    End Sub

    Private Sub TextGroupFilter_TextChanged(sender As Object, e As EventArgs) Handles TextGroupFilter.TextChanged
        FillGroups()
    End Sub

    Private Sub TextMemberFilter_TextChanged(sender As Object, e As EventArgs) Handles TextMemberFilter.TextChanged
        ComboMemberRights.SelectedIndex = 0
        If ListAdGroups.SelectedIndex < 0 Then
            FillUsers()
        Else
            FillMembers()
        End If
    End Sub

    Private Sub FillMembers()
        If OwnerForm.AdMembers Is Nothing Then
            Exit Sub
        End If
        Dim list As IList(Of WinAdUserPrincipal) = GetMembersList()
        Dim member As WinAdUserPrincipal
        For Each member In list.Where(Function(g) g.ToString().ToLower() Like "*" & TextMemberFilter.Text.ToLower() & "*")
            ListAdMembers.Items.Add(member.ToString())
        Next
    End Sub

    Private Function GetMembersList() As IList(Of WinAdUserPrincipal)
        Dim list As IList(Of WinAdUserPrincipal) = Nothing
        Dim gid As String = GetGroupIdFromName(ListAdGroups.SelectedItem?.ToString())
        If OwnerForm.AdMembers.TryGetValue(gid, list) Then
            GetMembersList = list
        Else
            GetMembersList = Nothing
        End If
    End Function

    Private Sub FillUsers()
        If OwnerForm.AdUsers Is Nothing Then
            Exit Sub
        End If
        ComboMemberRights.SelectedIndex = 0
        ListAdMembers.Items.Clear()
        Dim user As WinAdUserPrincipal
        For Each user In OwnerForm.AdUsers.Where(Function(g) g.ToString().ToLower() Like "*" & TextMemberFilter.Text.ToLower() & "*")
            ListAdMembers.Items.Add(user.ToString())
        Next
    End Sub

    Private Sub ComboGroupRights_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboGroupRights.SelectedIndexChanged
        Dim cb As ComboBox = DirectCast(sender, ComboBox)
        If Not cb.Focused Then
            Exit Sub
        End If
        Dim gid As String = GetGroupIdFromName(ListAdGroups.SelectedItem?.ToString())
        SetGroupRight(gid)
        FillGroupAssignedList()
    End Sub

    Private Sub ComboMemberRights_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboMemberRights.SelectedIndexChanged
        Dim cb As ComboBox = DirectCast(sender, ComboBox)
        If Not cb.Focused Then
            Exit Sub
        End If
        Dim gid As String = GetUserIdFromName(ListAdMembers.SelectedItem?.ToString())
        SetUserRight(gid)
        FillUserAssignedList()
    End Sub

    Private Sub ListAdMembers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListAdMembers.SelectedIndexChanged
        Dim gid As String = GetUserIdFromName(ListAdMembers.SelectedItem?.ToString())
        GetUserRight(gid)
    End Sub

    Private Sub FillListsOnStart()
        FillGroups()
        FillUsers()
    End Sub

    Private Sub FillGroupAssignedList()
        ListAdGroupsAssigned.Items.Clear()
        Dim groupRights = OwnerForm.dbc.GetGroupRights(txtId.Text)
        For Each groupRight In groupRights
            Dim ename = [Enum].GetName(GetType(OrgManEnums.AccessRight), groupRight.AccessRight)
            ListAdGroupsAssigned.Items.Add(GetGroupNameFromId(groupRight.Sid) & " [" & ename & "]")
        Next
        ListAdGroupsAssigned.Enabled = True
    End Sub

    Private Sub FillUserAssignedList()
        ListAdMembersAssigned.Items.Clear()
        Dim userRights = OwnerForm.dbc.GetUserRights(txtId.Text)
        For Each userRight In userRights
            Dim ename = [Enum].GetName(GetType(OrgManEnums.AccessRight), userRight.AccessRight)
            ListAdMembersAssigned.Items.Add(GetUserNameFromId(userRight.Sid) & " [" & ename & "]")
        Next
        ListAdMembersAssigned.Enabled = True
    End Sub

    Private Async Sub TimerLoading_Tick(sender As Object, e As EventArgs) Handles TimerLoading.Tick
        If OwnerForm.SecurityIsLoading Then
            'waiting...
            TimerLoading.Interval = 500
        Else
            TimerLoading.Enabled = False
            Me.Cursor = Cursors.AppStarting
            If OwnerForm.AdGroups Is Nothing Then
                OwnerForm.SecurityIsLoading = True
                Dim t = New Task(Sub() LoadSecurityData())
                t.Start()
                Await t
                OwnerForm.SecurityIsLoading = False
            End If
            ListAdGroups.Enabled = True
            ListAdMembers.Enabled = True
            FillListsOnStart()
            Application.DoEvents()
            FillGroupAssignedList()
            FillUserAssignedList()
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub LoadSecurityData()
        If adcon.DomainIsAvailable() Then
            OwnerForm.AdGroups = adcon.GetDomainGroups()
            OwnerForm.AdUsers = adcon.GetDomainUsers(True)
        End If
    End Sub

End Class
