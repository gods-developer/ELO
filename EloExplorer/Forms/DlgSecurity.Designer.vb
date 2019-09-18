<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DlgSecurity
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.ListAdGroups = New System.Windows.Forms.ListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ListAdMembers = New System.Windows.Forms.ListBox()
        Me.TextGroupFilter = New System.Windows.Forms.TextBox()
        Me.TextMemberFilter = New System.Windows.Forms.TextBox()
        Me.ComboMemberRights = New System.Windows.Forms.ComboBox()
        Me.ComboGroupRights = New System.Windows.Forms.ComboBox()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ListAdGroupsAssigned = New System.Windows.Forms.ListBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ListAdMembersAssigned = New System.Windows.Forms.ListBox()
        Me.TimerLoading = New System.Windows.Forms.Timer(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtId
        '
        Me.txtId.Location = New System.Drawing.Point(72, 22)
        Me.txtId.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtId.Name = "txtId"
        Me.txtId.ReadOnly = True
        Me.txtId.Size = New System.Drawing.Size(116, 23)
        Me.txtId.TabIndex = 1
        Me.txtId.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(18, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Id"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 15)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Name"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(72, 48)
        Me.txtName.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtName.Name = "txtName"
        Me.txtName.ReadOnly = True
        Me.txtName.Size = New System.Drawing.Size(484, 23)
        Me.txtName.TabIndex = 3
        '
        'ListAdGroups
        '
        Me.ListAdGroups.Enabled = False
        Me.ListAdGroups.FormattingEnabled = True
        Me.ListAdGroups.ItemHeight = 15
        Me.ListAdGroups.Items.AddRange(New Object() {"Daten werden geladen..."})
        Me.ListAdGroups.Location = New System.Drawing.Point(24, 109)
        Me.ListAdGroups.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ListAdGroups.Name = "ListAdGroups"
        Me.ListAdGroups.Size = New System.Drawing.Size(246, 259)
        Me.ListAdGroups.Sorted = True
        Me.ListAdGroups.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 15)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Benutzergruppen"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(306, 92)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 15)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Mitglieder"
        '
        'ListAdMembers
        '
        Me.ListAdMembers.Enabled = False
        Me.ListAdMembers.FormattingEnabled = True
        Me.ListAdMembers.ItemHeight = 15
        Me.ListAdMembers.Items.AddRange(New Object() {"Daten werden geladen..."})
        Me.ListAdMembers.Location = New System.Drawing.Point(310, 109)
        Me.ListAdMembers.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ListAdMembers.Name = "ListAdMembers"
        Me.ListAdMembers.Size = New System.Drawing.Size(246, 259)
        Me.ListAdMembers.Sorted = True
        Me.ListAdMembers.TabIndex = 7
        '
        'TextGroupFilter
        '
        Me.TextGroupFilter.Location = New System.Drawing.Point(147, 85)
        Me.TextGroupFilter.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextGroupFilter.Name = "TextGroupFilter"
        Me.TextGroupFilter.Size = New System.Drawing.Size(124, 23)
        Me.TextGroupFilter.TabIndex = 9
        '
        'TextMemberFilter
        '
        Me.TextMemberFilter.Location = New System.Drawing.Point(432, 85)
        Me.TextMemberFilter.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextMemberFilter.Name = "TextMemberFilter"
        Me.TextMemberFilter.Size = New System.Drawing.Size(124, 23)
        Me.TextMemberFilter.TabIndex = 10
        '
        'ComboMemberRights
        '
        Me.ComboMemberRights.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboMemberRights.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboMemberRights.FormattingEnabled = True
        Me.ComboMemberRights.Items.AddRange(New Object() {"Keine Berechtigung", "Leserechte", "Lese-/Schreibrechte"})
        Me.ComboMemberRights.Location = New System.Drawing.Point(310, 372)
        Me.ComboMemberRights.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ComboMemberRights.Name = "ComboMemberRights"
        Me.ComboMemberRights.Size = New System.Drawing.Size(246, 23)
        Me.ComboMemberRights.TabIndex = 11
        '
        'ComboGroupRights
        '
        Me.ComboGroupRights.BackColor = System.Drawing.SystemColors.Window
        Me.ComboGroupRights.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboGroupRights.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboGroupRights.FormattingEnabled = True
        Me.ComboGroupRights.Items.AddRange(New Object() {"Keine Berechtigung", "Leserechte", "Lese-/Schreibrechte"})
        Me.ComboGroupRights.Location = New System.Drawing.Point(25, 372)
        Me.ComboGroupRights.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ComboGroupRights.Name = "ComboGroupRights"
        Me.ComboGroupRights.Size = New System.Drawing.Size(246, 23)
        Me.ComboGroupRights.TabIndex = 12
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(6, 3)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(95, 27)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Schließen"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(471, 562)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(108, 33)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 401)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 15)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Berechtigte"
        '
        'ListAdGroupsAssigned
        '
        Me.ListAdGroupsAssigned.Enabled = False
        Me.ListAdGroupsAssigned.FormattingEnabled = True
        Me.ListAdGroupsAssigned.ItemHeight = 15
        Me.ListAdGroupsAssigned.Items.AddRange(New Object() {"Daten werden geladen..."})
        Me.ListAdGroupsAssigned.Location = New System.Drawing.Point(24, 418)
        Me.ListAdGroupsAssigned.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ListAdGroupsAssigned.Name = "ListAdGroupsAssigned"
        Me.ListAdGroupsAssigned.Size = New System.Drawing.Size(246, 139)
        Me.ListAdGroupsAssigned.Sorted = True
        Me.ListAdGroupsAssigned.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(306, 401)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 15)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Berechtigte"
        '
        'ListAdMembersAssigned
        '
        Me.ListAdMembersAssigned.Enabled = False
        Me.ListAdMembersAssigned.FormattingEnabled = True
        Me.ListAdMembersAssigned.ItemHeight = 15
        Me.ListAdMembersAssigned.Items.AddRange(New Object() {"Daten werden geladen..."})
        Me.ListAdMembersAssigned.Location = New System.Drawing.Point(310, 418)
        Me.ListAdMembersAssigned.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ListAdMembersAssigned.Name = "ListAdMembersAssigned"
        Me.ListAdMembersAssigned.Size = New System.Drawing.Size(246, 139)
        Me.ListAdMembersAssigned.Sorted = True
        Me.ListAdMembersAssigned.TabIndex = 15
        '
        'TimerLoading
        '
        Me.TimerLoading.Enabled = True
        '
        'DlgSecurity
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(591, 603)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ListAdMembersAssigned)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ListAdGroupsAssigned)
        Me.Controls.Add(Me.ComboGroupRights)
        Me.Controls.Add(Me.ComboMemberRights)
        Me.Controls.Add(Me.TextMemberFilter)
        Me.Controls.Add(Me.TextGroupFilter)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ListAdMembers)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ListAdGroups)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtId)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Cursor = System.Windows.Forms.Cursors.AppStarting
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DlgSecurity"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Sicherheit"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtId As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents ListAdGroups As ListBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ListAdMembers As ListBox
    Friend WithEvents TextGroupFilter As TextBox
    Friend WithEvents TextMemberFilter As TextBox
    Friend WithEvents ComboMemberRights As ComboBox
    Friend WithEvents ComboGroupRights As ComboBox
    Friend WithEvents OK_Button As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label5 As Label
    Friend WithEvents ListAdGroupsAssigned As ListBox
    Friend WithEvents Label6 As Label
    Friend WithEvents ListAdMembersAssigned As ListBox
    Friend WithEvents TimerLoading As Timer
End Class
