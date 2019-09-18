Imports System.Windows.Forms
Imports Win.Common.Tools

Public Class DlgScanImport

    Private sPath As String
    Public Sub LoadFiles(sPath As String)
        Me.Text += sPath
        LvwFiles.Items.Clear()
        Dim files As List(Of OrgManFileInfo) = GetFiles(sPath)
        Dim cp As Integer, file As OrgManFileInfo
        For cp = 0 To files.Count - 1
            file = files.Item(cp)
            AddFileItem(file, cp)
        Next
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function AddFileItem(file As OrgManFileInfo, index As Integer) As ListViewItem
        Dim sIcon As String = AddIconToImageList(file.FilePath, file.Filename, ImageListFiles, "file", file.FileType)
        Dim item As ListViewItem = LvwFiles.Items.Add("F" & index, file.Filename, sIcon)
        Dim dateSubItem As ListViewItem.ListViewSubItem = item.SubItems.Add(file.FileDateTime.ToString())
        dateSubItem.Tag = dateSubItem.Text
        item.SubItems.Add(file.FileType)
        Dim lenSubItem As ListViewItem.ListViewSubItem = item.SubItems.Add(file.FileLen.ToString("#,##0"))
        lenSubItem.Tag = lenSubItem.Text
        AddFileItem = item
    End Function

    Private Sub LvwFiles_KeyDown(sender As Object, e As KeyEventArgs) Handles LvwFiles.KeyDown
        If e.Control And e.KeyCode = Keys.A Then
            Dim itm As ListViewItem
            For Each itm In LvwFiles.Items
                itm.Selected = True
            Next
        End If
    End Sub

    Private Sub LvwFiles_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles LvwFiles.ColumnClick
        Dim listView As ListView = DirectCast(sender, ListView)
        Dim lvwColumnSorter As ListViewItemComparer
        If (listView.ListViewItemSorter Is Nothing) Then
            'object.   
            lvwColumnSorter = New ListViewItemComparer()
            listView.ListViewItemSorter = lvwColumnSorter
        Else
            lvwColumnSorter = DirectCast(listView.ListViewItemSorter, ListViewItemComparer)
        End If

        If (listView.Items.Count > 0) Then
            ' Determine if clicked column Is already the column that Is being sorted.
            ' If the Module Dropdown has changed, reset the Sorting order
            If (e.Column = lvwColumnSorter.SortColumn) Then
                ' Reverse the current sort direction for this column.
                If (lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending) Then
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending
                Else
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending
                End If
            Else
                ' Set the column number that Is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column
                lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending
            End If

            ' Perform the sort with these New sort options.
            listView.Sort()
        End If

    End Sub
End Class
