Public Class DlgFileImport

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ButtonOverwrite_Click(sender As Object, e As EventArgs) Handles ButtonOverwrite.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Ignore
        Me.Close()
    End Sub

    Private Sub ButtonBoth_Click(sender As Object, e As EventArgs) Handles ButtonBoth.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub
End Class