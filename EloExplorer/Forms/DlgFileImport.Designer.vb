<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DlgFileImport
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
        Me.ButtonOverwrite = New System.Windows.Forms.Button()
        Me.LabelInfo = New System.Windows.Forms.Label()
        Me.TextBoxFileName = New System.Windows.Forms.TextBox()
        Me.ButtonBoth = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.TextBoxFileDateTimeSource = New System.Windows.Forms.TextBox()
        Me.TextBoxFileSizeSource = New System.Windows.Forms.TextBox()
        Me.TextBoxFileSizeDestination = New System.Windows.Forms.TextBox()
        Me.TextBoxFileDateTimeDestination = New System.Windows.Forms.TextBox()
        Me.LabelSource = New System.Windows.Forms.Label()
        Me.LabelDestination = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ButtonOverwrite
        '
        Me.ButtonOverwrite.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonOverwrite.Location = New System.Drawing.Point(15, 166)
        Me.ButtonOverwrite.Name = "ButtonOverwrite"
        Me.ButtonOverwrite.Size = New System.Drawing.Size(174, 42)
        Me.ButtonOverwrite.TabIndex = 2
        Me.ButtonOverwrite.Text = "Datei im Ziel Ersetzen"
        Me.ButtonOverwrite.UseVisualStyleBackColor = True
        '
        'LabelInfo
        '
        Me.LabelInfo.AutoSize = True
        Me.LabelInfo.Location = New System.Drawing.Point(12, 9)
        Me.LabelInfo.Name = "LabelInfo"
        Me.LabelInfo.Size = New System.Drawing.Size(329, 15)
        Me.LabelInfo.TabIndex = 3
        Me.LabelInfo.Text = "Im Ziel ist bereits eine Datei mit diesem Namen vorhanden."
        '
        'TextBoxFileName
        '
        Me.TextBoxFileName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxFileName.Location = New System.Drawing.Point(15, 41)
        Me.TextBoxFileName.Name = "TextBoxFileName"
        Me.TextBoxFileName.ReadOnly = True
        Me.TextBoxFileName.Size = New System.Drawing.Size(531, 16)
        Me.TextBoxFileName.TabIndex = 4
        '
        'ButtonBoth
        '
        Me.ButtonBoth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonBoth.Location = New System.Drawing.Point(195, 166)
        Me.ButtonBoth.Name = "ButtonBoth"
        Me.ButtonBoth.Size = New System.Drawing.Size(174, 42)
        Me.ButtonBoth.TabIndex = 6
        Me.ButtonBoth.Text = "Beide beibehalten"
        Me.ButtonBoth.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(375, 166)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(174, 42)
        Me.ButtonCancel.TabIndex = 7
        Me.ButtonCancel.Text = "Abbrechen"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'TextBoxFileDateTimeSource
        '
        Me.TextBoxFileDateTimeSource.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxFileDateTimeSource.Location = New System.Drawing.Point(15, 105)
        Me.TextBoxFileDateTimeSource.Name = "TextBoxFileDateTimeSource"
        Me.TextBoxFileDateTimeSource.ReadOnly = True
        Me.TextBoxFileDateTimeSource.Size = New System.Drawing.Size(215, 16)
        Me.TextBoxFileDateTimeSource.TabIndex = 8
        '
        'TextBoxFileSizeSource
        '
        Me.TextBoxFileSizeSource.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxFileSizeSource.Location = New System.Drawing.Point(15, 127)
        Me.TextBoxFileSizeSource.Name = "TextBoxFileSizeSource"
        Me.TextBoxFileSizeSource.ReadOnly = True
        Me.TextBoxFileSizeSource.Size = New System.Drawing.Size(215, 16)
        Me.TextBoxFileSizeSource.TabIndex = 9
        '
        'TextBoxFileSizeDestination
        '
        Me.TextBoxFileSizeDestination.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxFileSizeDestination.Location = New System.Drawing.Point(269, 127)
        Me.TextBoxFileSizeDestination.Name = "TextBoxFileSizeDestination"
        Me.TextBoxFileSizeDestination.ReadOnly = True
        Me.TextBoxFileSizeDestination.Size = New System.Drawing.Size(215, 16)
        Me.TextBoxFileSizeDestination.TabIndex = 11
        '
        'TextBoxFileDateTimeDestination
        '
        Me.TextBoxFileDateTimeDestination.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxFileDateTimeDestination.Location = New System.Drawing.Point(269, 105)
        Me.TextBoxFileDateTimeDestination.Name = "TextBoxFileDateTimeDestination"
        Me.TextBoxFileDateTimeDestination.ReadOnly = True
        Me.TextBoxFileDateTimeDestination.Size = New System.Drawing.Size(215, 16)
        Me.TextBoxFileDateTimeDestination.TabIndex = 10
        '
        'LabelSource
        '
        Me.LabelSource.AutoSize = True
        Me.LabelSource.Location = New System.Drawing.Point(21, 82)
        Me.LabelSource.Name = "LabelSource"
        Me.LabelSource.Size = New System.Drawing.Size(43, 15)
        Me.LabelSource.TabIndex = 12
        Me.LabelSource.Text = "Quelle"
        '
        'LabelDestination
        '
        Me.LabelDestination.AutoSize = True
        Me.LabelDestination.Location = New System.Drawing.Point(276, 82)
        Me.LabelDestination.Name = "LabelDestination"
        Me.LabelDestination.Size = New System.Drawing.Size(27, 15)
        Me.LabelDestination.TabIndex = 13
        Me.LabelDestination.Text = "Ziel"
        '
        'DlgFileImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(566, 220)
        Me.Controls.Add(Me.LabelDestination)
        Me.Controls.Add(Me.LabelSource)
        Me.Controls.Add(Me.TextBoxFileSizeDestination)
        Me.Controls.Add(Me.TextBoxFileDateTimeDestination)
        Me.Controls.Add(Me.TextBoxFileSizeSource)
        Me.Controls.Add(Me.TextBoxFileDateTimeSource)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonBoth)
        Me.Controls.Add(Me.TextBoxFileName)
        Me.Controls.Add(Me.LabelInfo)
        Me.Controls.Add(Me.ButtonOverwrite)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DlgFileImport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Datei ersetzen oder beibehalten"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonOverwrite As Button
    Friend WithEvents LabelInfo As Label
    Friend WithEvents TextBoxFileName As TextBox
    Friend WithEvents ButtonBoth As Button
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents TextBoxFileDateTimeSource As TextBox
    Friend WithEvents TextBoxFileSizeSource As TextBox
    Friend WithEvents TextBoxFileSizeDestination As TextBox
    Friend WithEvents TextBoxFileDateTimeDestination As TextBox
    Friend WithEvents LabelSource As Label
    Friend WithEvents LabelDestination As Label
End Class
