Public Class DlgReminder
    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If TextBoxDate.Text = String.Empty Then
            Exit Sub
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub

    Private Sub ButtonClearDate_Click(sender As Object, e As EventArgs) Handles ButtonClearDate.Click
        TextBoxDate.Text = String.Empty
        TextBoxTime.Text = String.Empty
    End Sub

    Private Sub ButtonClearTime_Click(sender As Object, e As EventArgs) Handles ButtonClearTime.Click
        TextBoxTime.Text = String.Empty
    End Sub
End Class