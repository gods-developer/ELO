Imports System.Windows.Forms
Imports Win.Common.Tools

Public Class DlgSettings

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextScanFolder.Text <> "" And Not IO.Directory.Exists(TextScanFolder.Text) Then
            MsgBox("Der Ordner " & IO.Directory.Exists(TextScanFolder.Text) & " existiert nicht!", MsgBoxStyle.Exclamation, "Hinweis")
            Exit Sub
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ButtonSelectFolder_Click(sender As Object, e As EventArgs) Handles ButtonSelectFolder.Click
        Dim fbd As New FolderBrowserDialog()
        fbd.SelectedPath = TextScanFolder.Text
        Dim result As DialogResult = fbd.ShowDialog(Me)
        If (result = DialogResult.OK) Then
            TextScanFolder.Text = fbd.SelectedPath
        End If
    End Sub
End Class
