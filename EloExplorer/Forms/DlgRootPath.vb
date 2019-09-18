Public Class DlgRootPath

    Friend EmptyAllowed As Boolean

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If String.IsNullOrEmpty(TextBoxInput.Text) And Not EmptyAllowed Then
            Exit Sub
        End If
        If Not String.IsNullOrEmpty(TextBoxInput.Text) And Not FileIO.FileSystem.DirectoryExists(TextBoxInput.Text) Then
            Dim dlgResult = MsgBox("Der Ordner " & TextBoxInput.Text & " existiert nicht!" & vbCrLf & "Soll er angelegt werden?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Hinweis")
            If dlgResult = MsgBoxResult.Yes Then
                IO.Directory.CreateDirectory(TextBoxInput.Text)
            Else
                Exit Sub
            End If
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ButtonSelect_Click(sender As Object, e As EventArgs) Handles ButtonSelect.Click
        Dim fbd As New FolderBrowserDialog()
        fbd.SelectedPath = TextBoxInput.Text
        Dim result As DialogResult = fbd.ShowDialog(Me)
        If (result = DialogResult.OK) Then
            TextBoxInput.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub LabelInfo_Resize(sender As Object, e As EventArgs) Handles LabelInfo.Resize
        Dim offset = LabelInfo.Height - labelHeight
        If offset > 0 Then
            TextBoxInput.Top += offset
            ButtonSelect.Top += offset
            'TableLayoutPanelButtons.Top += offset
            Me.Height += offset
        End If
    End Sub

    Private labelHeight As Integer = 15

    Private Sub TextBoxInput_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextBoxInput.Validating
        If TextBoxInput.Text.EndsWith("\") Then
            TextBoxInput.Text = Strings.Left(TextBoxInput.Text, Len(TextBoxInput.Text) - 1)
        End If
    End Sub
End Class