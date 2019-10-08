<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DlgProperties
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtRoot = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.ComboBoxFolderSortBy = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboBoxFolderSortWay = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextCreation = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextCreationUser = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TextLastUpdate = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextLastUpdateUser = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TextRowVersion = New System.Windows.Forms.TextBox()
        Me.ComboBoxFilesSortWay = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.ComboBoxFilesSortBy = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(406, 347)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(171, 33)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 3)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(77, 27)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(89, 3)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(78, 27)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Abbrechen"
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
        Me.txtName.Size = New System.Drawing.Size(501, 23)
        Me.txtName.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 15)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Root"
        '
        'txtRoot
        '
        Me.txtRoot.Location = New System.Drawing.Point(72, 74)
        Me.txtRoot.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtRoot.Name = "txtRoot"
        Me.txtRoot.Size = New System.Drawing.Size(501, 23)
        Me.txtRoot.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 15)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Pfad"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(72, 100)
        Me.txtPath.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.ReadOnly = True
        Me.txtPath.Size = New System.Drawing.Size(501, 23)
        Me.txtPath.TabIndex = 7
        Me.txtPath.TabStop = False
        '
        'ComboBoxFolderSortBy
        '
        Me.ComboBoxFolderSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxFolderSortBy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxFolderSortBy.FormattingEnabled = True
        Me.ComboBoxFolderSortBy.Items.AddRange(New Object() {"Benutzerdefiniert", "Name", "Datum/Uhrzeit", "Größe"})
        Me.ComboBoxFolderSortBy.Location = New System.Drawing.Point(149, 137)
        Me.ComboBoxFolderSortBy.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ComboBoxFolderSortBy.Name = "ComboBoxFolderSortBy"
        Me.ComboBoxFolderSortBy.Size = New System.Drawing.Size(154, 23)
        Me.ComboBoxFolderSortBy.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(21, 139)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(113, 15)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Sortierung (Ordner)"
        '
        'ComboBoxFolderSortWay
        '
        Me.ComboBoxFolderSortWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxFolderSortWay.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxFolderSortWay.FormattingEnabled = True
        Me.ComboBoxFolderSortWay.Items.AddRange(New Object() {"Aufsteigend", "Absteigend"})
        Me.ComboBoxFolderSortWay.Location = New System.Drawing.Point(308, 137)
        Me.ComboBoxFolderSortWay.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ComboBoxFolderSortWay.Name = "ComboBoxFolderSortWay"
        Me.ComboBoxFolderSortWay.Size = New System.Drawing.Size(113, 23)
        Me.ComboBoxFolderSortWay.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(60, 204)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 15)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Erstellt am"
        '
        'TextCreation
        '
        Me.TextCreation.Location = New System.Drawing.Point(149, 198)
        Me.TextCreation.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextCreation.Name = "TextCreation"
        Me.TextCreation.ReadOnly = True
        Me.TextCreation.Size = New System.Drawing.Size(273, 23)
        Me.TextCreation.TabIndex = 12
        Me.TextCreation.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(60, 231)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(69, 15)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Erstellt von"
        '
        'TextCreationUser
        '
        Me.TextCreationUser.Location = New System.Drawing.Point(149, 224)
        Me.TextCreationUser.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextCreationUser.Name = "TextCreationUser"
        Me.TextCreationUser.ReadOnly = True
        Me.TextCreationUser.Size = New System.Drawing.Size(273, 23)
        Me.TextCreationUser.TabIndex = 14
        Me.TextCreationUser.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(60, 257)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(77, 15)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Geändert am"
        '
        'TextLastUpdate
        '
        Me.TextLastUpdate.Location = New System.Drawing.Point(149, 250)
        Me.TextLastUpdate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextLastUpdate.Name = "TextLastUpdate"
        Me.TextLastUpdate.ReadOnly = True
        Me.TextLastUpdate.Size = New System.Drawing.Size(273, 23)
        Me.TextLastUpdate.TabIndex = 16
        Me.TextLastUpdate.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(60, 283)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 15)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Geändert von"
        '
        'TextLastUpdateUser
        '
        Me.TextLastUpdateUser.Location = New System.Drawing.Point(149, 276)
        Me.TextLastUpdateUser.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextLastUpdateUser.Name = "TextLastUpdateUser"
        Me.TextLastUpdateUser.ReadOnly = True
        Me.TextLastUpdateUser.Size = New System.Drawing.Size(273, 23)
        Me.TextLastUpdateUser.TabIndex = 18
        Me.TextLastUpdateUser.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(60, 309)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(30, 15)
        Me.Label10.TabIndex = 21
        Me.Label10.Text = "DS #"
        '
        'TextRowVersion
        '
        Me.TextRowVersion.Location = New System.Drawing.Point(149, 302)
        Me.TextRowVersion.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextRowVersion.Name = "TextRowVersion"
        Me.TextRowVersion.ReadOnly = True
        Me.TextRowVersion.Size = New System.Drawing.Size(273, 23)
        Me.TextRowVersion.TabIndex = 20
        Me.TextRowVersion.TabStop = False
        '
        'ComboBoxFilesSortWay
        '
        Me.ComboBoxFilesSortWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxFilesSortWay.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxFilesSortWay.FormattingEnabled = True
        Me.ComboBoxFilesSortWay.Items.AddRange(New Object() {"Aufsteigend", "Absteigend"})
        Me.ComboBoxFilesSortWay.Location = New System.Drawing.Point(308, 163)
        Me.ComboBoxFilesSortWay.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ComboBoxFilesSortWay.Name = "ComboBoxFilesSortWay"
        Me.ComboBoxFilesSortWay.Size = New System.Drawing.Size(113, 23)
        Me.ComboBoxFilesSortWay.TabIndex = 24
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(21, 165)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(116, 15)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "Sortierung (Dateien)"
        '
        'ComboBoxFilesSortBy
        '
        Me.ComboBoxFilesSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxFilesSortBy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxFilesSortBy.FormattingEnabled = True
        Me.ComboBoxFilesSortBy.Items.AddRange(New Object() {"Name", "Datum/Uhrzeit", "Typ", "Größe"})
        Me.ComboBoxFilesSortBy.Location = New System.Drawing.Point(149, 163)
        Me.ComboBoxFilesSortBy.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ComboBoxFilesSortBy.Name = "ComboBoxFilesSortBy"
        Me.ComboBoxFilesSortBy.Size = New System.Drawing.Size(154, 23)
        Me.ComboBoxFilesSortBy.TabIndex = 22
        '
        'DlgProperties
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(591, 395)
        Me.Controls.Add(Me.ComboBoxFilesSortWay)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.ComboBoxFilesSortBy)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.TextRowVersion)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.TextLastUpdateUser)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TextLastUpdate)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TextCreationUser)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextCreation)
        Me.Controls.Add(Me.ComboBoxFolderSortWay)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ComboBoxFolderSortBy)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtRoot)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtId)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DlgProperties"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Eigenschaften"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents txtId As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtRoot As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtPath As TextBox
    Friend WithEvents ComboBoxFolderSortBy As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ComboBoxFolderSortWay As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TextCreation As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents TextCreationUser As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents TextLastUpdate As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents TextLastUpdateUser As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents TextRowVersion As TextBox
    Friend WithEvents ComboBoxFilesSortWay As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents ComboBoxFilesSortBy As ComboBox
End Class
