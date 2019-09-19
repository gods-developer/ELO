<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DlgEditor
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
        Me.RichTextBoxEditor = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'RichTextBoxEditor
        '
        Me.RichTextBoxEditor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBoxEditor.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBoxEditor.Location = New System.Drawing.Point(0, 0)
        Me.RichTextBoxEditor.Name = "RichTextBoxEditor"
        Me.RichTextBoxEditor.ReadOnly = True
        Me.RichTextBoxEditor.Size = New System.Drawing.Size(800, 450)
        Me.RichTextBoxEditor.TabIndex = 0
        Me.RichTextBoxEditor.Text = ""
        '
        'DlgEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.RichTextBoxEditor)
        Me.MinimizeBox = False
        Me.Name = "DlgEditor"
        Me.ShowIcon = False
        Me.Text = "DlgEditor"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents RichTextBoxEditor As RichTextBox
End Class
