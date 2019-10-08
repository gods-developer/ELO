Public Class DlgCancel
    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Dim mainForm = DirectCast(Me.Owner, FrmMain)
        mainForm.Cancel = True
    End Sub
End Class