<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DlgRootPath
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
        Me.TableLayoutPanelButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.LabelInfo = New System.Windows.Forms.Label()
        Me.TextBoxInput = New System.Windows.Forms.TextBox()
        Me.ButtonSelect = New System.Windows.Forms.Button()
        Me.TableLayoutPanelButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanelButtons
        '
        Me.TableLayoutPanelButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanelButtons.ColumnCount = 2
        Me.TableLayoutPanelButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanelButtons.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanelButtons.Location = New System.Drawing.Point(226, 76)
        Me.TableLayoutPanelButtons.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanelButtons.Name = "TableLayoutPanelButtons"
        Me.TableLayoutPanelButtons.RowCount = 1
        Me.TableLayoutPanelButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelButtons.Size = New System.Drawing.Size(195, 42)
        Me.TableLayoutPanelButtons.TabIndex = 2
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 4)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(89, 34)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(101, 4)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(89, 34)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Abbrechen"
        '
        'LabelInfo
        '
        Me.LabelInfo.AutoSize = True
        Me.LabelInfo.Location = New System.Drawing.Point(12, 9)
        Me.LabelInfo.Name = "LabelInfo"
        Me.LabelInfo.Size = New System.Drawing.Size(256, 15)
        Me.LabelInfo.TabIndex = 9
        Me.LabelInfo.Text = "Bitte Root-Pfad der neuen Abteilung eingeben:"
        '
        'TextBoxInput
        '
        Me.TextBoxInput.Location = New System.Drawing.Point(12, 34)
        Me.TextBoxInput.Name = "TextBoxInput"
        Me.TextBoxInput.Size = New System.Drawing.Size(365, 23)
        Me.TextBoxInput.TabIndex = 10
        '
        'ButtonSelect
        '
        Me.ButtonSelect.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonSelect.Location = New System.Drawing.Point(383, 30)
        Me.ButtonSelect.Name = "ButtonSelect"
        Me.ButtonSelect.Size = New System.Drawing.Size(32, 32)
        Me.ButtonSelect.TabIndex = 11
        Me.ButtonSelect.Text = "..."
        Me.ButtonSelect.UseVisualStyleBackColor = True
        '
        'DlgRootPath
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(434, 131)
        Me.Controls.Add(Me.LabelInfo)
        Me.Controls.Add(Me.TextBoxInput)
        Me.Controls.Add(Me.ButtonSelect)
        Me.Controls.Add(Me.TableLayoutPanelButtons)
        Me.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DlgRootPath"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Neue Abteilung"
        Me.TableLayoutPanelButtons.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TableLayoutPanelButtons As TableLayoutPanel
    Friend WithEvents OK_Button As Button
    Friend WithEvents Cancel_Button As Button
    Friend WithEvents LabelInfo As Label
    Friend WithEvents TextBoxInput As TextBox
    Friend WithEvents ButtonSelect As Button
End Class
