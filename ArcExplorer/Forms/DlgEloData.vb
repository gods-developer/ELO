Imports System.Windows.Forms
Imports DigiSped.Common.Tools

Public Class DlgEloData

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

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
