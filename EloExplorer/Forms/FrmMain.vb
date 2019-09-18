﻿Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports IWshRuntimeLibrary
Imports OrgMan
Imports Win.Common.Tools
Imports CefSharp
Imports CefSharp.WinForms
Public Class FrmMain
    Dim copyNodeKey As String, copyMode As Byte
    Public dbc As IDatabaseConnector
    Public IsAdmin As Boolean
    Public TemplateFolderName As String, DefaultRootPath As String
    Public chrome As ChromiumWebBrowser, chromeExe As String

    Friend AdGroups As IList(Of WinAdGroupPrincipal)
    Friend AdUsers As IList(Of WinAdUserPrincipal)
    Friend AdMembers As New Dictionary(Of String, IList(Of WinAdUserPrincipal))


    <StructLayout(LayoutKind.Sequential)>
    Public Structure SHELLEXECUTEINFO
        Public cbSize As Integer
        Public fMask As UInteger
        Public hwnd As IntPtr
        Public lpVerb As [String]
        Public lpFile As [String]
        Public lpParameters As [String]
        Public lpDirectory As [String]
        Public nShow As Integer
        Public hInstApp As Integer
        Public lpIDList As Integer
        Public lpClass As [String]
        Public hkeyClass As Integer
        Public dwHotKey As UInteger
        Public hIcon As Integer
        Public hProcess As Integer
    End Structure

    Private Const SW_SHOW As Integer = 5
    Private Const SEE_MASK_INVOKEIDLIST As UInteger = 12 ' 0x0000000C

    <DllImport("shell32.dll")>
    Private Shared Function ShellExecuteEx(ByRef lpExecInfo As SHELLEXECUTEINFO) As Boolean
    End Function

    Private Sub SetEnvironment(env As String)
        OrgManGlobals.AppEnvironment = env 'todo
        DropDownEnvironment.Text = OrgManGlobals.AppEnvironment
    End Sub

    Private Sub InitChromeControl()
        Dim settings = New CefSettings()
        CefSharp.Cef.Initialize(settings)
        chrome = New ChromiumWebBrowser()
        Me.FilesSplitContainer.Panel1.Controls.Add(Me.chrome)
        Me.chrome.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chrome.Visible = False
    End Sub

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim args = New ApplicationHelper().GetStartUpParameters()
        'For Each a In args
        '    'MsgBox(a)
        '    If a Like "environment=*" Then
        '        OrgManGlobals.AppEnvironment = a.Substring(a.IndexOf("=") + 1)
        '    End If
        'Next
        If String.IsNullOrEmpty(OrgManGlobals.AppEnvironment) Then
            SetEnvironment(RegistryHandler.GetStringValue(Application.ProductName, "Environment", My.Settings.EnvironmentName))
            Me.Refresh()
        End If
        Dim s As New SplashScreen()
        s.LabelInfo.Text = "OrgMan " + Application.ProductVersion + " [" + OrgManGlobals.AppEnvironment + "] wird geladen. Einen Moment noch..."
        s.Show(Me)
        s.Refresh()
        Thread.Sleep(100)
        If My.Settings.DatabaseMode = "MsAccess" Then
            dbc = New MsAccessConnector
        Else
            dbc = New MsSqlConnector
        End If
        Try
            If Not dbc.HasAccess() Then
                MsgBox("Sie haben keine Berechtigung, den Organisationsmanager zu benutzen!" & vbCrLf & "Bitte wenden Sie sich an den Administrator.", MsgBoxStyle.Exclamation, "Hinweis")
                End
            End If
        Catch ex As Exception
            MsgBox("Es ist ein Fehler aufgetreten beim Verbinden mit der Datenbank!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Fehler beim Starten")
            If OrgManGlobals.AppEnvironment <> My.Settings.EnvironmentName Then
                MsgBox("Es wird versucht, die Standard-Umgebung [" & My.Settings.EnvironmentName & "] zu starten...", MsgBoxStyle.Information, "Hinweis")
                RegistryHandler.SetStringValue(Application.ProductName, "Environment", My.Settings.EnvironmentName)
                Application.Restart()
            End If
            End
        End Try
        IsAdmin = dbc.IsAdmin()
        If My.Settings.EnvironmentName = "Prod" And OrgManGlobals.AppEnvironment <> "Prod" And Not IsAdmin Then
            ChangeEnvironment("Prod")
            'SetEnvironment(RegistryHandler.GetStringValue(Application.ProductName, "Environment", Properties.Settings.Default.Environment));
            'MainForm_Shown(sender, e);
            Exit Sub
        End If
        If My.Settings.EnvironmentName = "Prod" Then
            DropDownEnvironment.Enabled = IsAdmin
        End If
        'InitChromeControl()
        'FindChromeExe()
        Me.MenuSecurity.Enabled = IsAdmin
        Me.MenuNewDepartment.Enabled = IsAdmin
        FilesRefreshTimer.Enabled = My.Settings.ListAutoRefresh
        Me.TemplateFolderName = dbc.GetDbSetting("TemplateFolderName", "Vorlagen")
        Me.DefaultRootPath = dbc.GetDbSetting("DefaultRootPath", "C:\OrgMan")
        Me.Text += " " + Application.ProductVersion + " [" + OrgManGlobals.AppEnvironment + "]" + If(IsAdmin, " (" + dbc.Path + ")", "")
        Me.WindowState = GetSetting(Application.ProductName, "Window", "LastWindowState", FormWindowState.Normal)
        Me.Top = GetSetting(Application.ProductName, "Window", "LastWindowTop", Me.Top)
        Me.Left = GetSetting(Application.ProductName, "Window", "LastWindowLeft", Me.Left)
        Me.Width = GetSetting(Application.ProductName, "Window", "LastWindowWidth", Me.Width)
        Me.Height = GetSetting(Application.ProductName, "Window", "LastWindowHeight", Me.Height)
        Me.MainSplitContainer.SplitterDistance = GetSetting(Application.ProductName, "Window", "LastMainSplitterDistance", Me.MainSplitContainer.SplitterDistance)
        If Boolean.Parse(dbc.GetDbUserSetting("ShowFilePreviewer", "False")) Then
            Me.FilesSplitContainer.SplitterDistance = GetSetting(Application.ProductName, "Window", "LastFilesSplitterDistance", Me.FilesSplitContainer.SplitterDistance)
            MenuShowFilePreviewer.Checked = True
            MenuReminderShowFilePreviewer.Checked = True
        Else
            HideFilePreviewer()
        End If
        Me.LvwFiles.Columns(0).Width = GetSetting(Application.ProductName, "Window", "LastColumn0With", Me.LvwFiles.Columns(0).Width)
        Me.LvwFiles.Columns(1).Width = GetSetting(Application.ProductName, "Window", "LastColumn1With", Me.LvwFiles.Columns(1).Width)
        Me.LvwFiles.Columns(2).Width = GetSetting(Application.ProductName, "Window", "LastColumn2With", Me.LvwFiles.Columns(2).Width)
        Me.LvwFiles.Columns(3).Width = GetSetting(Application.ProductName, "Window", "LastColumn3With", Me.LvwFiles.Columns(3).Width)
        LoadTree()
        s.Close()
    End Sub

    Private Sub HideFilePreviewer()
        Me.FilesSplitContainer.Panel2Collapsed = True
        Me.FilesSplitContainer.Panel2.Hide()
    End Sub

    Private Sub ShowFilePreviewer()
        Me.FilesSplitContainer.Panel2Collapsed = False
        Me.FilesSplitContainer.Panel2.Show()
    End Sub

    Private Sub FindChromeExe()
        ScanFolder("C:\Program Files", "chrome.exe")
        If chromeExe Is Nothing Then
            ScanFolder("C:\Program Files (x86)", "chrome.exe")
        End If
    End Sub

    Private Sub ScanFolder(path As String, pattern As String)
        Try
            If Not chromeExe Is Nothing Then
                Exit Sub
            End If

            For Each file In Directory.EnumerateFiles(path, pattern)
                chromeExe = file
                Exit Sub
            Next

            For Each folder In Directory.EnumerateDirectories(path)
                ScanFolder(folder, pattern)
            Next

        Catch uae As UnauthorizedAccessException
            Console.WriteLine("Error: " + path)
        End Try

    End Sub

    Private Sub LoadTree()
        ClearFilePreview()
        LvwReminders.Items.Clear()
        LvwFiles.Items.Clear()
        TvwExplorer.Nodes.Clear()
        Dim reminder = TvwExplorer.Nodes.Add("reminder", "Wiedervorlagen", "Reminder.ico", "Reminder.ico")
        reminder.Tag = New OrgManTreeItem()
        RefreshReminders()
        Dim rootNodes As List(Of OrgManTreeItem) = dbc.GetRootTreeItems()
        rootNodes = SortNodesByConfig(rootNodes, Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortBy", "0")), Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortWay", "0")))
        For Each rootNode In rootNodes
            AddTreeNode(Nothing, rootNode, rootNode.NodeText, True)
        Next
        TvwExplorer.SelectedNode = Nothing
        TreeNodeClick(reminder)
    End Sub

    Private Function AddTreeNode(parentNode As TreeNode, newItem As OrgManTreeItem, caption As String, noSelect As Boolean) As TreeNode
        Dim node As TreeNode, imageName As String
        If parentNode Is Nothing Then
            imageName = IIf(newItem.RootPath?.ToLower() Like "http*", "Browser.ico", "DocumentRepository.ico")
            node = TvwExplorer.Nodes.Add("R" & newItem.Id, caption, imageName, imageName)
        Else
            imageName = IIf(newItem.RootPath?.ToLower() Like "http*", "Browser.ico", "Folder.ico")
            node = parentNode.Nodes.Add("C" & newItem.Id, caption, imageName, imageName)
        End If
        node.Tag = newItem
        LoadChildren(node)
        If node.Name Like "R*" Then
            node.Expand()
        ElseIf Not parentNode Is Nothing And Not noSelect Then
            parentNode.Expand()
            TvwExplorer.SelectedNode = node
            If IsBrowserNode(TvwExplorer.SelectedNode) Then
                LoadBrowser(newItem.RootPath)
            Else
                LoadFiles()
            End If
        End If
        AddTreeNode = node
    End Function

    Private Sub LoadChildren(parentNode As TreeNode)
        Dim treeItem = DirectCast(parentNode.Tag, OrgManTreeItem)
        Dim childNodes As List(Of OrgManTreeItem) = dbc.GetChildTreeItems(treeItem.Id)
        childNodes = SortNodesByConfig(childNodes, treeItem.ChildrenSortBy, treeItem.ChildrenSortWay)
        For Each childNode In childNodes
            AddTreeNode(parentNode, childNode, childNode.NodeText, True)
        Next
    End Sub

    Private Function SortNodesByConfig(childNodes As List(Of OrgManTreeItem), sortBy As OrgManEnums.FoldersSortBy, sortWay As OrgManEnums.SortWay) As List(Of OrgManTreeItem)
        Select Case sortBy
            Case OrgManEnums.FoldersSortBy.Name
                SortNodesByConfig = IIf(sortWay = OrgManEnums.SortWay.Ascending, childNodes.OrderBy(Function(x) x.NodeText).ToList(), childNodes.OrderByDescending(Function(x) x.NodeText).ToList())
            Case OrgManEnums.FoldersSortBy.DateTime
                SortNodesByConfig = IIf(sortWay = OrgManEnums.SortWay.Ascending, childNodes.OrderBy(Function(x) x.Creation).ToList(), childNodes.OrderByDescending(Function(x) x.Creation).ToList())
                'Case OrgManEnums.FoldersSortBy.Size
                'SortListByConfig = IIf(sortWay = OrgManEnums.SortWay.Ascending, childNodes.OrderBy(Function(x) x.).ToList()
            Case Else
                SortNodesByConfig = IIf(sortWay = OrgManEnums.SortWay.Ascending, childNodes, childNodes.OrderByDescending(Function(x) x.SortOrder).ToList())
        End Select
    End Function

    Private Function GetIdFromTag(tag As Object) As Integer
        Dim treeItem = DirectCast(tag, OrgManTreeItem)
        GetIdFromTag = treeItem.Id
    End Function

    Private Function GetFileIdFromTag(tag As Object) As Integer
        Dim fileItem = DirectCast(tag, OrgManTreeItemFile)
        GetFileIdFromTag = fileItem?.Id
    End Function

    Private Sub MenuNewFolder_Click(sender As Object, e As EventArgs) Handles MenuNewFolder.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim newText As String, parentNodeId As Integer, newRootPath As String = "", newNode As OrgManTreeItem
        newText = ""
mRetry:
        newText = InputBox("Bitte Name des neuen Ordners eingeben:", "Neuer Ordner", newText)
        If newText <> "" Then
            parentNodeId = GetIdFromTag(TvwExplorer.SelectedNode.Tag)
            If dbc.CheckIfNameExists(parentNodeId, newText) Then
                MsgBox("Der Name existiert bereits auf dieser Ebene!", MsgBoxStyle.Exclamation, "Hinweis")
                GoTo mRetry
            End If
            Dim dlgResult = SelectRootPath(newRootPath, "Neuer Ordner", "Möchten Sie einen Root-Pfad für den neuen Ordner festlegen?" + vbCrLf + "(Bei keiner Eingabe wird vom übergeordneten Ordner geerbt > " + vbCrLf + GetFullPathOfNode(TvwExplorer.SelectedNode) + newText + ")", True)
            If dlgResult = DialogResult.Cancel Then
                Exit Sub
            End If
            Dim parentNode As New OrgManTreeItem With {
            .Id = parentNodeId,
            .Node = TvwExplorer.SelectedNode
        }
            Dim selectedNode As TreeNode = TvwExplorer.SelectedNode
            newNode = AddNewTreeItem(parentNode, newText, False, Trim(newRootPath))
            If Not newNode Is Nothing And IsRootLevel(selectedNode) Then
                Dim vorlagenNode As TreeNode = GetVorlagenNode(selectedNode)
                Dim vorlage As OrgManTreeItem
                For Each vorlage In dbc.GetChildTreeItems(GetIdFromTag(vorlagenNode.Tag))
                    vorlage.Node = GetTreeNodeByKey("C" & vorlage.Id)
                    CopyNodes(vorlage, newNode.Node, 0)
                Next
            End If
        End If
    End Sub

    Private Sub CopyNodes(copyNode As OrgManTreeItem, pasteNode As TreeNode, mode As Byte, Optional withFiles As Boolean = True)
        Dim node As New OrgManTreeItem
        Dim newNode As OrgManTreeItem, newName As String, newRootPath As String = String.Empty, c As Integer
        If Not IsRootLevel(copyNode.Node) Then
            node.Id = GetIdFromTag(pasteNode.Tag)
            node.Node = pasteNode
        End If
mRetry:
        newName = copyNode.NodeText
        Do While dbc.CheckIfNameExists(node.Id, newName)
            c = c + 1
            newName = copyNode.NodeText + " (" + c.ToString() + ")"
        Loop
        If IsRootLevel(copyNode.Node) Then
            newRootPath = Me.DefaultRootPath & "\" & newName
            Dim dlgResult = SelectRootPath(newRootPath) 'InputBox("Bitte Root-Pfad der neuen Abteilung eingeben:", "Neue Abteilung", newRootPath)
            If dlgResult = DialogResult.Cancel Then
                Exit Sub
            End If
            If newRootPath = dbc.GetRootPath(copyNode.Id) Then
                GoTo mRetry
            ElseIf newRootPath = "" Then
                Exit Sub
            End If
        End If
        newNode = AddNewTreeItem(node, newName, True, newRootPath)
        If newNode Is Nothing Then
            Exit Sub
        End If
        If withFiles Then
            CopyFiles(copyNode.Node, newNode.Node)
        End If
        CopyChildren(copyNode, newNode, withFiles)
        pasteNode.Expand()
        pasteNode = newNode.Node
        If mode = 1 Then
            'Ausschneiden
            DeleteTreeItem(copyNode.Node, True)
        End If
    End Sub

    Private Function SelectRootPath(ByRef input As String, Optional title As String = "", Optional info As String = "", Optional emptyAllowed As Boolean = False) As DialogResult
        Dim dlg As New DlgRootPath
        With dlg
            If Not String.IsNullOrEmpty(title) Then
                .Text = title
            End If
            If Not String.IsNullOrEmpty(info) Then
                .LabelInfo.Text = info
            End If
            .TextBoxInput.Text = input
            .EmptyAllowed = emptyAllowed
            Dim dlgResult = .ShowDialog(Me)
            SelectRootPath = dlgResult
            If dlgResult = DialogResult.OK Then
                input = .TextBoxInput.Text
                .Close()
            End If
        End With
    End Function

    Private Sub CopyFiles(copyNode As TreeNode, newNode As TreeNode)
        Dim sourcePath As String = GetFullPathOfNode(copyNode)
        Dim importPath As String = GetFullPathOfNode(newNode)
        Dim treeItem = DirectCast(newNode.Tag, OrgManTreeItem)
        Dim files As List(Of OrgManFileInfo) = GetFiles(sourcePath)
        Dim cp As Integer, file As OrgManFileInfo
        For cp = 0 To files.Count - 1
            file = files.Item(cp)
            ImportFile(treeItem, importPath, sourcePath & file.Filename, DragDropEffects.Copy, True)
        Next
    End Sub

    Private Function GetVorlagenNode(rootNode As TreeNode) As TreeNode
        Dim node As TreeNode
        GetVorlagenNode = Nothing
        For Each node In rootNode.Nodes
            If node.Text = Me.TemplateFolderName Then
                GetVorlagenNode = node
                Exit For
            End If
        Next
    End Function

    Private Function IsRootLevel(node As TreeNode) As Boolean
        If node Is Nothing Then
            IsRootLevel = False
        Else
            IsRootLevel = node.Name Like "R*"
        End If
    End Function

    Private Function AddNewTreeItem(parentNode As OrgManTreeItem, newName As String, noSelect As Boolean, Optional newRootPath As String = "") As OrgManTreeItem
        Dim newNode As OrgManTreeItem = dbc.AddNewTreeItem(newName, newRootPath, parentNode.Id)
        If Not newNode Is Nothing Then
            newNode.Node = AddTreeNode(parentNode.Node, newNode, newName, noSelect)
        End If
        AddNewTreeItem = newNode
    End Function

    Private Sub MenuNewDepartment_Click(sender As Object, e As EventArgs) Handles MenuNewDepartment.Click
        Dim newText As String, newRootPath As String, rootNode As OrgManTreeItem, newNode As OrgManTreeItem
        newText = ""
mRetry:
        newText = InputBox("Bitte Name der neuen Abteilung eingeben:", "Neue Abteilung", newText)
        If newText <> "" Then
            If dbc.CheckIfNameExists(0, newText) Then
                MsgBox("Der Name existiert bereits auf dieser Ebene!", MsgBoxStyle.Exclamation, "Hinweis")
                GoTo mRetry
            End If
            newRootPath = Me.DefaultRootPath & "\" & newText
            Dim dlgResult = SelectRootPath(newRootPath)
            If dlgResult = DialogResult.Cancel Then
                Exit Sub
            End If
            If newRootPath <> "" Then
                rootNode = AddNewTreeItem(New OrgManTreeItem(), newText, False, newRootPath)
                If Not rootNode Is Nothing Then
                    If Not IsBrowserNode(rootNode.Node) Then
                        newNode = AddNewTreeItem(rootNode, Me.TemplateFolderName, True, "")
                    End If
                End If
                rootNode?.Node.Expand()
            End If
        End If
    End Sub

    Private Sub FrmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If SecurityIsLoading Then
            MsgBox("Es läuft noch ein Hintergrundprozess. Bitte noch ein paar Sekunden warten...", MsgBoxStyle.Information, "Hinweis")
            e.Cancel = True
            Exit Sub
        End If
        dbc.Close()
        SaveSetting(Application.ProductName, "Window", "LastWindowState", Me.WindowState)
        SaveSetting(Application.ProductName, "Window", "LastWindowTop", Me.Top)
        SaveSetting(Application.ProductName, "Window", "LastWindowLeft", Me.Left)
        SaveSetting(Application.ProductName, "Window", "LastWindowWidth", Me.Width)
        SaveSetting(Application.ProductName, "Window", "LastWindowHeight", Me.Height)
        SaveSetting(Application.ProductName, "Window", "LastMainSplitterDistance", Me.MainSplitContainer.SplitterDistance)
        If Not Me.FilesSplitContainer.Panel2Collapsed Then
            SaveSetting(Application.ProductName, "Window", "LastFilesSplitterDistance", Me.FilesSplitContainer.SplitterDistance)
        End If
        SaveSetting(Application.ProductName, "Window", "LastColumn0With", Me.LvwFiles.Columns(0).Width)
        SaveSetting(Application.ProductName, "Window", "LastColumn1With", Me.LvwFiles.Columns(1).Width)
        SaveSetting(Application.ProductName, "Window", "LastColumn2With", Me.LvwFiles.Columns(2).Width)
        SaveSetting(Application.ProductName, "Window", "LastColumn3With", Me.LvwFiles.Columns(3).Width)
    End Sub

    Private Sub MenuCopy_Click(sender As Object, e As EventArgs) Handles MenuCopy.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim SC As New System.Collections.Specialized.StringCollection
        Dim item As ListViewItem
        For Each item In LvwFiles.SelectedItems
            SC.Add(sPath)
        Next
        Dim DTO = New DataObject
        DTO.SetFileDropList(SC)
        DTO.SetData("Preferred DropEffect", New MemoryStream(BitConverter.GetBytes(Convert.ToInt32(DragDropEffects.Copy))))
        Clipboard.SetDataObject(DTO, True)
    End Sub

    Private Sub NotReady()
        MsgBox("Noch nicht implementiert. Bitte noch ein bisschen Geduld.", MsgBoxStyle.Information, "Hinweis")
    End Sub

    Private Sub MenuCut_Click(sender As Object, e As EventArgs) Handles MenuCut.Click
        If TvwExplorer.SelectedNode Is Nothing Or Not MenuCut.Enabled Then
            Exit Sub
        End If
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim SC As New System.Collections.Specialized.StringCollection
        Dim item As ListViewItem
        For Each item In LvwFiles.SelectedItems
            SC.Add(sPath)
        Next
        Dim DTO = New DataObject
        DTO.SetFileDropList(SC)
        DTO.SetData("Preferred DropEffect", New MemoryStream(BitConverter.GetBytes(Convert.ToInt32(DragDropEffects.Move))))
        Clipboard.SetDataObject(DTO, True)
    End Sub

    Private Function GetTreeNodeByKey(nodeKey As String) As TreeNode
        Dim nodes() As TreeNode = TvwExplorer.Nodes.Find(nodeKey, True)
        If nodes.Length > 0 Then
            GetTreeNodeByKey = nodes.First()
        Else
            GetTreeNodeByKey = Nothing
        End If
    End Function

    Private Function GetListViewItemByKey(itemKey As String) As ListViewItem
        Dim items() As ListViewItem = LvwFiles.Items.Find(itemKey, False)
        If items.Length > 0 Then
            GetListViewItemByKey = items.First()
        Else
            GetListViewItemByKey = Nothing
        End If
    End Function

    Private Sub MenuPaste_Click(sender As Object, e As EventArgs) Handles MenuPaste.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        Dim filelist As System.Collections.Specialized.StringCollection
        filelist = My.Computer.Clipboard.GetFileDropList()
        Dim DropEffectData(3) As Byte
        Dim DropEffectCheck As Object = My.Computer.Clipboard.GetData("Preferred DropEffect")
        DropEffectCheck.Read(DropEffectData, 0, DropEffectData.Length)
        For Each filePath As String In filelist
            If IsDirectory(filePath) Then
                ProcessImportDirectory(treeItem, sPath, filePath, IIf(DropEffectData(0) = 2, DragDropEffects.Move, DragDropEffects.Copy))
            Else
                ImportFile(treeItem, sPath, filePath, IIf(DropEffectData(0) = 2, DragDropEffects.Move, DragDropEffects.Copy), False)
            End If
        Next
    End Sub

    Private Sub CopyChildren(sourceNode As OrgManTreeItem, newParentNode As OrgManTreeItem, withFiles As Boolean)
        Dim newNode As OrgManTreeItem, nextNode As New OrgManTreeItem
        Dim childNodes As List(Of OrgManTreeItem) = dbc.GetChildTreeItems(sourceNode.Id)
        For Each childNode In childNodes
            newNode = AddNewTreeItem(newParentNode, childNode.NodeText, True, dbc.GetRootPath(childNode.Id))
            If newNode Is Nothing Then
                Exit Sub
            End If
            nextNode.Id = childNode.Id
            nextNode.Node = GetTreeNodeByKey("C" & childNode.Id)
            If withFiles Then
                CopyFiles(nextNode.Node, newNode.Node)
            End If
            CopyChildren(nextNode, newNode, withFiles)
        Next
    End Sub

    Private Sub MenuDelete_Click(sender As Object, e As EventArgs) Handles MenuDelete.Click
        If TvwExplorer.SelectedNode Is Nothing Or Not MenuDelete.Enabled Then
            Exit Sub
        End If
        If IsRootLevel(TvwExplorer.SelectedNode.Parent) And TvwExplorer.SelectedNode.Text = Me.TemplateFolderName Then
            MsgBox("Der Vorlagen-Ordner darf nicht gelöscht werden!", MsgBoxStyle.Exclamation, "Hinweis")
        ElseIf MsgBox("Knoten " & TvwExplorer.SelectedNode.Text & " unwiderruflich löschen?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.Yes Then
            Dim includeFiles As Boolean
            If Not IsBrowserNode(TvwExplorer.SelectedNode) Then
                If MsgBox("Wollen Sie auch die im Knoten " & TvwExplorer.SelectedNode.Text & " enthaltenen Dateien samt Unterordner löschen?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.Yes Then
                    If MsgBox("Ganz sicher?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.Yes Then
                        includeFiles = True
                    End If
                End If
            End If
            DeleteTreeItem(TvwExplorer.SelectedNode, includeFiles)
            ClearFilePreview()
            LvwFiles.Items.Clear()
        End If
    End Sub

    Private Function IsBrowserNode(node As TreeNode) As Boolean
        Dim onode As OrgManTreeItem = DirectCast(node.Tag, OrgManTreeItem)
        IsBrowserNode = (onode.RootPath?.ToLower() Like "http*")
    End Function

    Private Function GetUrlFromNode(node As TreeNode) As String
        Dim onode As OrgManTreeItem = DirectCast(node.Tag, OrgManTreeItem)
        GetUrlFromNode = onode.RootPath
    End Function

    Private Sub DeleteTreeItem(node As TreeNode, includeFiles As Boolean)
        'If node Is Nothing Then Exit Sub
        Do While node.Nodes.Count > 0
            DeleteTreeItem(node.Nodes(0), includeFiles)
        Loop
        If includeFiles Then
            DeleteFiles(node)
        End If
        Dim nodeId As Integer = GetIdFromTag(node.Tag)
        dbc.DeleteTreeItem(nodeId)
        node.Remove()
    End Sub

    Private Sub DeleteFiles(node As TreeNode)
        Dim fullPath As String = GetFullPathOfNode(node)
        Dim files As List(Of OrgManFileInfo) = GetFiles(fullPath)
        Dim cp As Integer, file As OrgManFileInfo
        Dim treeItemId As Integer = GetIdFromTag(node.Tag)
        For cp = 0 To files.Count - 1
            file = files.Item(cp)
            Dim fileId = dbc.GetTreeItemFile(treeItemId, file.Filename).Id
            DeleteTreeItemFile(fileId)
            KillFileIfExists(fullPath + file.Filename)
        Next
        RemoveDirIfExists(fullPath)
    End Sub

    Private Sub MenuRename_Click(sender As Object, e As EventArgs) Handles MenuRename.Click
        If TvwExplorer.SelectedNode Is Nothing Or Not MenuRename.Enabled Then
            Exit Sub
        End If
        If IsRootLevel(TvwExplorer.SelectedNode.Parent) And TvwExplorer.SelectedNode.Text = Me.TemplateFolderName Then
            MsgBox("Der Vorlagen-Ordner darf nicht umbenannt werden!", MsgBoxStyle.Exclamation, "Hinweis")
            Exit Sub
        End If
        Dim oldPath = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim newText As String, node As OrgManTreeItem, parentNodeId As Integer
        newText = TvwExplorer.SelectedNode.Text
mRetry:
        newText = InputBox("Bitte neuen Namen eingeben:", "Ordner umbenennen", newText)
        If newText <> "" And newText <> TvwExplorer.SelectedNode.Text Then
            If Not TvwExplorer.SelectedNode.Parent Is Nothing Then
                parentNodeId = GetIdFromTag(TvwExplorer.SelectedNode.Parent.Tag)
            End If
            node = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
            If dbc.CheckIfNameExists(parentNodeId, newText) Then
                MsgBox("Der Name existiert bereits auf dieser Ebene!", MsgBoxStyle.Exclamation, "Hinweis")
                GoTo mRetry
            End If
            node.NodeText = newText
            dbc.SaveTreeItem(node)
            TvwExplorer.SelectedNode.Text = newText
            Dim newPath = GetFullPathOfNode(TvwExplorer.SelectedNode)
            MoveFolder(oldPath, newPath)
        End If
    End Sub

    Private Sub MenuProperties_Click(sender As Object, e As EventArgs) Handles MenuProperties.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim node As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        With DlgProperties
            .Text = TvwExplorer.SelectedNode.Text + " - Eigenschaften"
            .txtId.Text = node.Id
            .txtName.Text = node.NodeText
            .txtRoot.Text = node.RootPath
            .txtRoot.Tag = .txtRoot.Text
            .txtPath.Text = GetFullPathOfNode(TvwExplorer.SelectedNode)
            .txtPath.Tag = GetFullPathOfNode(TvwExplorer.SelectedNode, True)
            .ComboBoxFolderSortBy.SelectedIndex = Convert.ToInt32(node.ChildrenSortBy)
            .ComboBoxFolderSortWay.SelectedIndex = Convert.ToInt32(node.ChildrenSortWay)
            .ComboBoxFilesSortBy.SelectedIndex = Convert.ToInt32(node.FilesSortBy)
            .ComboBoxFilesSortWay.SelectedIndex = Convert.ToInt32(node.FilesSortWay)
            .TextCreation.Text = node.Creation.ToString()
            .TextCreationUser.Text = node.CreationUser
            .TextLastUpdate.Text = node.LastUpdate?.ToString()
            .TextLastUpdateUser.Text = node.LastUpdateUser
            .TextRowVersion.Text = node.RowVersion.ToString()
            Dim oldPath = GetFullPathOfNode(TvwExplorer.SelectedNode)
            If IsRootLevel(TvwExplorer.SelectedNode) Then
                .txtName.ReadOnly = Not IsAdmin
                .txtRoot.ReadOnly = Not IsAdmin
                .ComboBoxFilesSortBy.Enabled = IsAdmin
                .ComboBoxFilesSortWay.Enabled = IsAdmin
                .ComboBoxFolderSortBy.Enabled = IsAdmin
                .ComboBoxFolderSortWay.Enabled = IsAdmin
                .OK_Button.Enabled = IsAdmin
            Else
                .txtName.ReadOnly = node.NodeText = Me.TemplateFolderName
                .txtRoot.ReadOnly = node.NodeText = Me.TemplateFolderName
                .ComboBoxFilesSortBy.Enabled = node.NodeText <> Me.TemplateFolderName
                .ComboBoxFilesSortWay.Enabled = node.NodeText <> Me.TemplateFolderName
                .ComboBoxFolderSortBy.Enabled = node.NodeText <> Me.TemplateFolderName
                .ComboBoxFolderSortWay.Enabled = node.NodeText <> Me.TemplateFolderName
                .OK_Button.Enabled = node.NodeText <> Me.TemplateFolderName
            End If
            '. = IsAdmin
            Dim dlgResult As DialogResult = .ShowDialog(Me)
            If dlgResult = DialogResult.OK Then
                node.NodeText = .txtName.Text
                node.ChildrenSortBy = .ComboBoxFolderSortBy.SelectedIndex
                node.ChildrenSortWay = .ComboBoxFolderSortWay.SelectedIndex
                node.FilesSortBy = .ComboBoxFilesSortBy.SelectedIndex
                node.FilesSortWay = .ComboBoxFilesSortWay.SelectedIndex
                dbc.SaveTreeItem(node)
                TvwExplorer.SelectedNode.Text = node.NodeText
                node.RootPath = .txtRoot.Text
                dbc.SaveRootPath(node)
                Dim newPath = GetFullPathOfNode(TvwExplorer.SelectedNode)
                MoveFolder(oldPath, newPath)
                .Close()
            End If
        End With
    End Sub

    Private Sub MoveFolder(oldPath As String, newPath As String)
        If oldPath <> newPath Then
            Dim msgResult = MsgBox("Der Ablageordner hat sich geändert!" + vbCrLf + "Sollen enthaltene Dateien mit umgezogen werden?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage")
            If msgResult = MsgBoxResult.Yes Then
                'todo gut überlegen! FileIO.FileSystem.MoveFile(oldPath, newPath, FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
                MsgBox("Das Verschieben von Ordnern im Hintergrund ist noch nicht scharf geschaltet, weil es zu viel Gefahren in sich birgt!" & vbCrLf & "Bitte ggf. selbst umziehen von " & vbCrLf & oldPath & vbCrLf & " nach " & vbCrLf & newPath, MsgBoxStyle.Exclamation, "Hinweis")
            End If
        End If
    End Sub

    Private Sub MenuRefresh_Click(sender As Object, e As EventArgs) Handles MenuRefresh.Click
        LoadTree()
    End Sub

    Private Sub SetTreeContextMenu(enabled As Boolean)
        MenuNewFolder.Enabled = enabled
        MenuProperties.Enabled = enabled
        MenuWinProperties.Enabled = enabled
        MenuFolderSort.Enabled = enabled
        MenuFilesSort.Enabled = enabled
        MenuMoveUp.Enabled = enabled
        MenuMoveDown.Enabled = enabled
        MenuOpenInExplorer.Enabled = enabled
        MenuDelete.Enabled = enabled
        MenuRename.Enabled = enabled
        MenuIntern.Enabled = enabled
        MenuCopy.Enabled = enabled
        MenuCut.Enabled = enabled
        MenuPaste.Enabled = enabled
        MenuSecurity.Enabled = enabled
    End Sub

    Private Sub TvwExplorer_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TvwExplorer.NodeMouseClick
        If e.Node?.Name = TvwExplorer.SelectedNode?.Name And MenuComboFolderSortBy.SelectedIndex <> -1 Then Exit Sub
        TreeNodeClick(e.Node)
    End Sub
    Private Sub TreeNodeClick(clickNode As TreeNode)
        TvwExplorer.SelectedNode = clickNode
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        If TvwExplorer.SelectedNode.Name = "reminder" Then
            TvwExplorer.ContextMenuStrip = ContextMenuExplorerReminder
            LoadReminders()
            Exit Sub
        Else
            TvwExplorer.ContextMenuStrip = ContextMenuExplorer
        End If
        Dim node As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        SetTreeContextMenu(True)
        MenuComboFolderSortBy.SelectedIndex = Convert.ToInt32(node.ChildrenSortBy)
        MenuComboFolderSortWay.SelectedIndex = Convert.ToInt32(node.ChildrenSortWay)
        MenuComboFileSortBy.SelectedIndex = Convert.ToInt32(node.FilesSortBy)
        MenuComboFileSortWay.SelectedIndex = Convert.ToInt32(node.FilesSortWay)
        If IsRootLevel(TvwExplorer.SelectedNode) Then
            'is root node (department)
            Dim rootChildrenSortBy = Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortBy", "0"))
            MenuMoveUp.Enabled = (rootChildrenSortBy = 0)
            MenuMoveDown.Enabled = (rootChildrenSortBy = 0)
            MenuSecurity.Enabled = IsAdmin
            MenuFolderSort.Enabled = IsAdmin
            MenuFilesSort.Enabled = IsAdmin
            MenuCut.Enabled = IsAdmin
            MenuDelete.Enabled = IsAdmin
            MenuRename.Enabled = IsAdmin
        Else
            Dim parentNode As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Parent.Tag, OrgManTreeItem)
            MenuMoveUp.Enabled = (parentNode.ChildrenSortBy = OrgManEnums.FoldersSortBy.UserDefined)
            MenuMoveDown.Enabled = (parentNode.ChildrenSortBy = OrgManEnums.FoldersSortBy.UserDefined)
            MenuSecurity.Enabled = False
        End If
        If TvwExplorer.SelectedNode.Text = Me.TemplateFolderName Then
            MenuCut.Enabled = False
            MenuDelete.Enabled = False
            MenuRename.Enabled = False
        End If
        If IsBrowserNode(TvwExplorer.SelectedNode) Then
            MenuNewFolder.Enabled = False
            MenuOpenInExplorer.Text = "Öffnen in Chrome"
            LoadBrowser(node.RootPath)
        Else
            MenuNewFolder.Enabled = True
            MenuOpenInExplorer.Text = "Öffnen in Explorer"
            LoadFiles()
        End If
    End Sub

    Private Sub ClearFilePreview()
        If Not Me.FilesSplitContainer.Panel2Collapsed Then
            FilePreviewHandlerHost.Open(String.Empty)
        End If
    End Sub

    Private Sub LoadReminders()
        ClearFilePreview()
        LvwReminders.Visible = True
        MsBrowser.Visible = False
        LvwFiles.Visible = False
        LvwReminders.Items.Clear()
        Dim reminders = dbc.GetReminders()
        Dim cp As Integer
        For Each reminder In reminders
            cp = cp + 1
            Dim homeNode = GetTreeNodeByKey("C" & reminder.TreeItemId)
            If homeNode Is Nothing Then
                homeNode = GetTreeNodeByKey("R" & reminder.TreeItemId)
            End If
            Dim filePath As String = GetFullPathOfNode(homeNode)
            Dim fileinfo = FileIcons.GetFileInfo(filePath, reminder.Filename), filetype As String
            Dim sIcon As String = AddIconToImageList(filePath, reminder.Filename, ImageListFiles, "file", filetype)
            Dim item = LvwReminders.Items.Add("R" & cp, reminder.Filename, sIcon)
            item.Tag = reminder
            item.SubItems.Add(GetRootNodeOfNode(homeNode).Text)
            item.SubItems.Add(GetTreePathOfNode(homeNode))
            item.SubItems.Add(reminder.ReminderDate.ToString())
            item.SubItems.Add(fileinfo.FileDateTime.ToString())
            item.SubItems.Add(fileinfo.FileType)
            item.SubItems.Add(fileinfo.FileLen.ToString("#,##0"))
        Next
        StatusLabelInfo.Text = reminders.Count & " Wiedervolage(n)"
    End Sub

    Private Sub LoadBrowser(sURL As String)
        LvwFiles.Visible = False
        LvwReminders.Visible = False
        'chrome.Visible = True
        'chrome.Load(sURL)
        MsBrowser.Visible = True
        MsBrowser.Navigate(sURL)
        StatusLabelInfo.Text = sURL
    End Sub

    Private Sub LoadFiles()
        Dim timerEnabled = FilesRefreshTimer.Enabled
        If timerEnabled Then
            FilesRefreshTimer.Enabled = False
            FilesRefreshTimer.Stop()
        End If
        Do While FilesRefreshTimerIsRunning
            Thread.Sleep(200)
        Loop
        'chrome.Visible = False
        MsBrowser.Visible = False
        LvwReminders.Visible = False
        LvwFiles.Visible = True
        ClearFilePreview()
        LvwFiles.Items.Clear()
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        If fullPath Like "http*" Then
            Exit Sub
        End If
        Dim files As List(Of OrgManFileInfo) = GetFiles(fullPath)
        Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        Dim cp As Integer, file As OrgManFileInfo
        For cp = 0 To files.Count - 1
            file = files.Item(cp)
            file.TreeItemFile = dbc.GetTreeItemFile(treeItem.Id, file.Filename)
        Next
        files = SortFilesByConfig(treeItem, files)
        For cp = 0 To files.Count - 1
            file = files.Item(cp)
            AddFileItem(treeItem.Id, file, cp)
        Next
        StatusLabelInfo.Text = files.Count & " Datei(en)"
        If timerEnabled Then
            FilesRefreshTimer.Enabled = True
        End If
    End Sub

    Private Function SortFilesByConfig(selectedNode As OrgManTreeItem, childNodes As List(Of OrgManFileInfo)) As List(Of OrgManFileInfo)
        Select Case selectedNode.FilesSortBy
            Case OrgManEnums.FilesSortBy.Type
                SortFilesByConfig = IIf(selectedNode.FilesSortWay = OrgManEnums.SortWay.Ascending, childNodes.OrderBy(Function(x) x.FileType).ToList(), childNodes.OrderByDescending(Function(x) x.FileType).ToList())
            Case OrgManEnums.FilesSortBy.DateTime
                SortFilesByConfig = IIf(selectedNode.FilesSortWay = OrgManEnums.SortWay.Ascending, childNodes.OrderBy(Function(x) x.FileDateTime).ToList(), childNodes.OrderByDescending(Function(x) x.FileDateTime).ToList())
            Case OrgManEnums.FilesSortBy.Size
                SortFilesByConfig = IIf(selectedNode.FilesSortWay = OrgManEnums.SortWay.Ascending, childNodes.OrderBy(Function(x) x.FileLen).ToList(), childNodes.OrderByDescending(Function(x) x.FileLen).ToList())
            Case OrgManEnums.FilesSortBy.UserDefined
                SortFilesByConfig = IIf(selectedNode.FilesSortWay = OrgManEnums.SortWay.Ascending, childNodes.OrderBy(Function(x) x.SortOrder).ToList(), childNodes.OrderByDescending(Function(x) x.SortOrder).ToList())
            Case Else
                SortFilesByConfig = IIf(selectedNode.FilesSortWay = OrgManEnums.SortWay.Ascending, childNodes.OrderBy(Function(x) x.Filename).ToList(), childNodes.OrderByDescending(Function(x) x.Filename).ToList())
        End Select
    End Function

    Private Function AddFileItem(treeItemId As Integer, file As OrgManFileInfo, count As Integer) As ListViewItem
        Dim item As ListViewItem
        Dim reminderDate As Date = dbc.GetReminderDate(treeItemId, file.Filename)
        If ListViewItemExists(file.Filename) Then
            item = GetListViewItemByText(file.Filename)
            item.Tag = file.TreeItemFile
            item.SubItems(ColumnHeaderName.Index).Text = file.FileDateTime.ToString()
            item.SubItems(ColumnHeaderSize.Index).Text = file.FileLen.ToString("#,##0")
            item.SubItems(ColumnHeaderReminder.Index).Text = IIf(reminderDate = Nothing, " ", reminderDate.ToString())
        Else
            Dim sIcon As String = AddIconToImageList(file.FilePath, file.Filename, ImageListFiles, "file", file.FileType)
            item = LvwFiles.Items.Add("F" & count, file.Filename, sIcon)
            item.Tag = file.TreeItemFile
            item.SubItems.Add(file.FileDateTime.ToString())
            item.SubItems.Add(file.FileType)
            item.SubItems.Add(file.FileLen.ToString("#,##0"))
            item.SubItems.Add(IIf(reminderDate = Nothing, " ", reminderDate.ToString()))
        End If
        AddFileItem = item
    End Function

    Private Function GetFullPathOfNode(node As TreeNode, Optional withoutMe As Boolean = False) As String
        Dim currentNode As TreeNode, currentNodeId As Integer, fullPath As String, rootPath As String
        fullPath = ""
        rootPath = ""
        currentNode = node
        currentNodeId = GetIdFromTag(currentNode.Tag)
        If Not withoutMe Then
            rootPath = dbc.GetRootPath(currentNodeId)
        End If
        fullPath = IIf(rootPath = "", currentNode.Text, rootPath) + "\" + fullPath
        While (currentNode.Parent IsNot Nothing And rootPath = "")
            currentNode = currentNode.Parent
            currentNodeId = GetIdFromTag(currentNode.Tag)
            rootPath = dbc.GetRootPath(currentNodeId)
            fullPath = IIf(rootPath = "", currentNode.Text, rootPath) + "\" + fullPath
        End While
        GetFullPathOfNode = fullPath
    End Function

    Private Function GetTreePathOfNode(node As TreeNode, Optional withoutMe As Boolean = False) As String
        Dim currentNode As TreeNode, treePath As String
        treePath = ""
        currentNode = node
        If Not withoutMe Then
            treePath = currentNode.Text
        End If
        While (currentNode.Parent IsNot Nothing)
            currentNode = currentNode.Parent
            If currentNode.Parent IsNot Nothing Then
                treePath = currentNode.Text & "\" & treePath
            End If
        End While
        GetTreePathOfNode = treePath
    End Function

    Private Function GetRootNodeOfNode(node As TreeNode) As TreeNode
        Dim currentNode As TreeNode
        currentNode = node
        While (currentNode.Parent IsNot Nothing)
            currentNode = currentNode.Parent
        End While
        GetRootNodeOfNode = currentNode
    End Function

    Private Sub TvwExplorer_KeyUp(sender As Object, e As KeyEventArgs) Handles TvwExplorer.KeyUp
        If e.KeyCode = Keys.F5 Then
            MenuRefresh_Click(sender, New EventArgs())
        ElseIf TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        ElseIf e.KeyCode = Keys.F2 Then
            MenuRename_Click(sender, New EventArgs())
        ElseIf e.KeyCode = Keys.Delete Then
            MenuDelete_Click(sender, New EventArgs())
        End If
    End Sub

    Private Sub LvwFiles_KeyUp(sender As Object, e As KeyEventArgs) Handles LvwFiles.KeyUp
        If e.KeyCode = Keys.F5 And Not TvwExplorer.SelectedNode Is Nothing Then
            MenuRefreshFiles_Click(sender, New EventArgs())
        ElseIf (TvwExplorer.SelectedNode Is Nothing Or LvwFiles.SelectedItems.Count = 0) Then
            Exit Sub
        ElseIf e.KeyCode = Keys.F2 Then
            MenuRenameFile_Click(sender, New EventArgs())
        ElseIf e.KeyCode = Keys.Delete Then
            MenuDeleteFile_Click(sender, New EventArgs())
        End If
    End Sub

    Private Sub LvwFiles_DoubleClick(sender As Object, e As EventArgs) Handles LvwFiles.DoubleClick
        If LvwFiles.SelectedItems.Count = 0 Or TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        System.Diagnostics.Process.Start(fullPath + LvwFiles.SelectedItems(0).Text)
    End Sub

    Private Sub TvwExplorer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TvwExplorer.KeyPress
        If e.KeyChar = Chr(3) Then
            MenuCopy_Click(sender, New EventArgs())
        ElseIf e.KeyChar = Chr(24) Then
            MenuCut_Click(sender, New EventArgs())
        ElseIf e.KeyChar = Chr(22) Then
            MenuPaste_Click(sender, New EventArgs())
        End If
    End Sub

    Private Sub LvwFiles_DragDrop(sender As Object, e As DragEventArgs) Handles LvwFiles.DragDrop
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        DragDropFiles(treeItem, sPath, e, False)
    End Sub

    Private Sub ProcessImportDirectory(treeItem As OrgManTreeItem, sPath As String, filepath As String, effect As DragDropEffects, Optional asLink As Boolean = False)
        Dim newFolder As String = Path.GetFileName(filepath)
        Dim parentNodeId = GetIdFromTag(TvwExplorer.SelectedNode.Tag)
        If dbc.CheckIfNameExists(parentNodeId, newFolder) Then
            For Each file In Directory.GetFiles(filepath)
                If Not file Is Nothing Then
                    ImportFile(treeItem, sPath & newFolder & "\", file, effect, True)
                End If
            Next
        Else
            Dim parentNode As New OrgManTreeItem With {
                            .Id = parentNodeId,
                            .Node = TvwExplorer.SelectedNode
                            }
            ImportDirectory(parentNode, sPath, filepath, effect, False)
        End If
    End Sub

    Private Sub DragDropFiles(treeItem As OrgManTreeItem, sPath As String, e As DragEventArgs, noAddToList As Boolean)
        If (e.Effect = DragDropEffects.Copy Or e.Effect = DragDropEffects.Move) Then
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim asLink As Boolean = Control.ModifierKeys = Keys.Control
                Dim fileNames = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
                Dim fp As Long
                For fp = 1 To fileNames.Count
                    If IsDirectory(fileNames(fp - 1)) Then
                        ProcessImportDirectory(treeItem, sPath, fileNames(fp - 1), e.Effect, asLink)
                    Else
                        ImportFile(treeItem, sPath, fileNames(fp - 1), e.Effect, noAddToList, asLink)
                    End If
                Next fp
            ElseIf e.Data.GetDataPresent("FileGroupDescriptor") Then
                Dim dataObject As DragDropHelper = New DragDropHelper(e.Data)
                Dim filenames As String() = CType(dataObject.GetData("FileGroupDescriptorW"), String())
                Dim memoryStreams As MemoryStream() = CType(dataObject.GetData("FileContents", True), MemoryStream())
                For fileIndex As Integer = 0 To filenames.Length - 1
                    Dim filename As String = filenames(fileIndex)
                    Dim memoryStream As MemoryStream = memoryStreams(fileIndex)
                    Dim tempFilename As String = System.IO.Path.GetTempPath() & filename.ToString()
                    Dim outputStream As FileStream = New FileStream(tempFilename, FileMode.Create)
                    memoryStream.WriteTo(outputStream)
                    outputStream.Close()
                    outputStream.Close()
                    ImportFile(treeItem, sPath, tempFilename, e.Effect, noAddToList)
                    KillFileIfExists(tempFilename)
                Next
            End If
        End If
    End Sub

    Private Function IsDirectory(filepath As String) As Boolean
        IsDirectory = (IO.File.GetAttributes(filepath) = FileAttributes.Directory)
    End Function

    Private Function ImportFile(treeItem As OrgManTreeItem, sPath As String, filename As String, effect As DragDropEffects, Optional noAddToList As Boolean = False, Optional asLink As Boolean = False) As Boolean
        Dim newFileName As String = Path.GetFileName(filename)
        If asLink Then
            newFileName += ".lnk"
        ElseIf LCase(filename) = LCase(sPath & newFileName) Then
            Exit Function
        End If
        Dim dlgResult As DialogResult = DialogResult.No
        If FileIO.FileSystem.FileExists(sPath & newFileName) Then
            Dim dlg As New DlgFileImport
            With dlg
                .TextBoxFileName.Text = newFileName
                If asLink Then
                    .Text = "Datei-Verknüpfung ersetzen oder beibehalten"
                    .LabelInfo.Text = "Im Ziel ist bereits eine Datei-Verknüpfung mit diesem Namen vorhanden."
                    .LabelSource.Visible = False
                Else
                    .TextBoxFileDateTimeSource.Text = FileDateTime(filename)
                    .TextBoxFileSizeSource.Text = Format(FileLen(filename), "#,##0") & " Byte(s)"
                End If
                .TextBoxFileDateTimeDestination.Text = FileDateTime(sPath & newFileName)
                .TextBoxFileSizeDestination.Text = Format(FileLen(sPath & newFileName), "#,##0") & " Byte(s)"
                dlgResult = .ShowDialog(Me)
                If dlgResult = DialogResult.Cancel Then
                    Exit Function
                End If
            End With
        End If
        If IsDirectory(filename) Then
            'Not yet supported
            Exit Function
        End If
        Dim treeItemFile As OrgManTreeItemFile
        Try
            System.IO.Directory.CreateDirectory(sPath)
            If dlgResult = DialogResult.Ignore Then
                KillFileIfExists(sPath & newFileName)
            ElseIf dlgResult = DialogResult.Yes Then
                newFileName = GetNewFreeFileName(sPath, newFileName)
            End If
            If asLink Then
                ImportFileAsLink(sPath, filename, newFileName)
            ElseIf effect = DragDropEffects.Copy Then
                FileIO.FileSystem.CopyFile(filename, sPath & newFileName, FileIO.UIOption.AllDialogs, FileIO.UICancelOption.ThrowException)
            Else
                FileIO.FileSystem.MoveFile(filename, sPath & newFileName, FileIO.UIOption.AllDialogs, FileIO.UICancelOption.ThrowException)
            End If
            If dlgResult = DialogResult.Ignore Then
                treeItemFile = dbc.GetTreeItemFile(treeItem.Id, newFileName)
            Else
                treeItemFile = AddNewTreeItemFile(treeItem.Id, newFileName)
                If treeItemFile Is Nothing Then
                    Return False
                End If
            End If
            ImportFile = True
        Catch e As Exception
            Exit Function
        End Try
        If Not noAddToList Then
            Dim fileInfo = GetFileInfo(sPath, newFileName)
            fileInfo.TreeItemFile = treeItemFile
            Dim newItem As ListViewItem = AddFileItem(treeItem.Id, fileInfo, LvwFiles.Items.Count + 1)
            newItem.Selected = True
        End If
    End Function

    Private Function GetNewFreeFileName(path As String, filename As String) As String
        If Not FileIO.FileSystem.FileExists(path & filename) Then
            GetNewFreeFileName = filename
            Exit Function
        End If
        Dim i As Integer, fname As String, fextension As String
        i = 1
        fname = filename.Substring(0, InStrRev(filename, ".") - 1)
        fextension = filename.Substring(InStrRev(filename, "."))
        Do While FileIO.FileSystem.FileExists(path & fname & " (" & i & ")." & fextension)
            i += 1
        Loop
        GetNewFreeFileName = fname & " (" & i & ")." & fextension
    End Function

    Private Sub ImportFileAsLink(sPath As String, filename As String, newFileName As String)
        'If IO.File.Exists(sPath & newFileName & ".lnk") Then
        '    If MsgBox("Vorhandenen Link " & sPath & newFileName & " überschreiben?", vbExclamation + vbYesNo) = MsgBoxResult.No Then
        '        Exit Sub
        '    End If
        '    KillFileIfExists(sPath & newFileName & ".lnk")
        'End If
        Dim wsh As New WshShell()
        Dim shortcut As IWshShortcut = wsh.CreateShortcut(sPath & newFileName)
        shortcut.Arguments = ""
        shortcut.TargetPath = filename
        ' Not sure about what this Is for
        shortcut.WindowStyle = 1
        shortcut.Description = "Link erstellt von OrgMan"
        shortcut.WorkingDirectory = Path.GetDirectoryName(filename)
        'shortcut.IconLocation = "specify icon location"
        shortcut.Save()
    End Sub

    Private Sub ImportDirectory(parentNode As OrgManTreeItem, sPath As String, filepath As String, effect As DragDropEffects, Optional noAddToTree As Boolean = False)
        Dim newFolderName As String = Path.GetFileName(filepath)
        If LCase(filepath) <> LCase(sPath & newFolderName) Then
            System.IO.Directory.CreateDirectory(sPath)
            If effect = DragDropEffects.Copy Then
                FileIO.FileSystem.CopyDirectory(filepath, sPath & newFolderName, FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
            Else
                FileIO.FileSystem.MoveFile(filepath, sPath & newFolderName, FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
            End If
            If Not noAddToTree Then
                Dim newItem As OrgManTreeItem = AddNewTreeItem(parentNode, newFolderName, True)
            End If
        End If
    End Sub

    Private Sub LvwFiles_DragEnter(sender As Object, e As DragEventArgs) Handles LvwFiles.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        ElseIf e.Data.GetDataPresent("FileGroupDescriptor") Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private _PrevMouse As New MouseEventArgs(0, 0, 0, 0, 0)
    Private MouseDownPos As Point
    Private Sub LvwFiles_MouseDown(sender As Object, e As MouseEventArgs) Handles LvwFiles.MouseDown
        _PrevMouse = e
        MouseDownPos = e.Location 'New Point(e.X, e.Y)
    End Sub

    Private Sub LvwFiles_MouseMove(sender As Object, e As MouseEventArgs) Handles LvwFiles.MouseMove
        If LvwFiles.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        If _PrevMouse.Button = MouseButtons.Left AndAlso
            e.Button = MouseButtons.Left Then
            Dim dx = e.X - MouseDownPos.X
            Dim dy = e.Y - MouseDownPos.Y
            If Math.Abs(dx) >= SystemInformation.DoubleClickSize.Width OrElse Math.Abs(dy) >= SystemInformation.DoubleClickSize.Height Then
                Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
                Dim SC As New System.Collections.Specialized.StringCollection
                Dim item As ListViewItem
                For Each item In LvwFiles.SelectedItems
                    SC.Add(fullPath + item.Text)
                Next
                Dim DTO = New DataObject
                DTO.SetFileDropList(SC)
                LvwFiles.DoDragDrop(DTO, DragDropEffects.Copy)
            End If
        End If
    End Sub

    Private Sub TvwExplorer_DragEnter(sender As Object, e As DragEventArgs) Handles TvwExplorer.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        ElseIf e.Data.GetDataPresent("FileGroupDescriptor") Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TvwExplorer_DragDrop(sender As Object, e As DragEventArgs) Handles TvwExplorer.DragDrop
        Dim pt As Point =
            CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
        targetNode = TvwExplorer.GetNodeAt(pt)
        If Not targetNode Is Nothing Then
            'targetNode.NodeFont = New Font(TvwExplorer.Font, FontStyle.Regular)
            'targetNode.ForeColor = Color.Black
            Dim sPath As String = GetFullPathOfNode(targetNode)
            Dim treeItem = DirectCast(targetNode.Tag, OrgManTreeItem)
            DragDropFiles(treeItem, sPath, e, targetNode.Name <> selectedNode?.Name)
        End If
        TvwExplorer.SelectedNode = selectedNode
    End Sub

    Private targetNode As TreeNode, selectedNode As TreeNode

    Private Sub TvwExplorer_DragOver(sender As Object, e As DragEventArgs) Handles TvwExplorer.DragOver
        'As the mouse moves over nodes, provide feedback to
        'the user by highlighting the node that is the
        'current drop target
        If targetNode Is Nothing Then
            selectedNode = TvwExplorer.SelectedNode
            'targetNode.NodeFont = New Font(TvwExplorer.Font, FontStyle.Regular)
            'targetNode.ForeColor = Color.Black
        End If
        Dim pt As Point =
            CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
        targetNode = TvwExplorer.GetNodeAt(pt)
        If Not targetNode Is Nothing Then 'And targetNode?.Name <> selectedNode.Name Then
            'targetNode.NodeFont = New Font(TvwExplorer.Font, FontStyle.Bold)
            'targetNode.ForeColor = Color.Blue
            TvwExplorer.SelectedNode = targetNode
        Else
            TvwExplorer.SelectedNode = selectedNode
        End If
    End Sub

    Private Sub TvwExplorer_MouseDown(sender As Object, e As MouseEventArgs) Handles TvwExplorer.MouseDown
        If e.Button = MouseButtons.Right Then
            Dim clickNode = TvwExplorer.GetNodeAt(e.X, e.Y)
            If clickNode Is Nothing Then
                TvwExplorer.SelectedNode = Nothing
                SetTreeContextMenu(False)
                MenuComboFolderSortBy.SelectedIndex = Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortBy", "0"))
                MenuComboFolderSortWay.SelectedIndex = Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortWay", "0"))
            End If
        End If
    End Sub

    Private Sub MenuPropertiesFile_Click(sender As Object, e As EventArgs) Handles MenuPropertiesFile.Click
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim fi As New IO.FileInfo(fullPath + LvwFiles.SelectedItems(0).Text)
        Dim info As New SHELLEXECUTEINFO()
        info.cbSize = Marshal.SizeOf(info)
        info.lpVerb = "properties"
        info.lpFile = fi.Name
        info.lpDirectory = fi.DirectoryName
        info.nShow = SW_SHOW
        info.fMask = SEE_MASK_INVOKEIDLIST
        ShellExecuteEx(info)
    End Sub

    Private Sub MenuOpenFile_Click(sender As Object, e As EventArgs) Handles MenuOpenFile.Click
        LvwFiles_DoubleClick(sender, e)
    End Sub

    Private Sub MenuDeleteFile_Click(sender As Object, e As EventArgs) Handles MenuDeleteFile.Click
        If MsgBox("Ausgewählte Datei(en) unwiderruflich löschen?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.Yes Then
            Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
            Do While LvwFiles.SelectedItems.Count > 0
                Dim fileId = GetFileIdFromTag(LvwFiles.SelectedItems(0).Tag)
                DeleteTreeItemFile(fileId)
                KillFileIfExists(fullPath + LvwFiles.SelectedItems(0).Text)
                LvwFiles.SelectedItems(0).Remove()
            Loop
        End If
    End Sub

    Private Sub DeleteTreeItemFile(treeItemFileId As Integer)
        dbc.DeleteTreeItemFile(treeItemFileId)
    End Sub

    Private Sub MenuRefreshFiles_Click(sender As Object, e As EventArgs) Handles MenuRefreshFiles.Click
        LoadFiles()
    End Sub

    Private Sub LvwFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LvwFiles.SelectedIndexChanged
        'FilesRefreshTimer.Enabled = False
        If Not Me.FilesSplitContainer.Panel2Collapsed Then
            If (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0) Then
                Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
                FilePreviewHandlerHost.Open(fullPath + LvwFiles.SelectedItems(0).Text)
            Else
                FilePreviewHandlerHost.Open(String.Empty)
            End If
        End If
    End Sub

    Private Sub TvwExplorer_DragLeave(sender As Object, e As EventArgs) Handles TvwExplorer.DragLeave
        TvwExplorer.SelectedNode = selectedNode
    End Sub

    Private Sub MenuCopyFile_Click(sender As Object, e As EventArgs) Handles MenuCopyFile.Click
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim SC As New System.Collections.Specialized.StringCollection
        Dim item As ListViewItem
        For Each item In LvwFiles.SelectedItems
            SC.Add(sPath + item.Text)
        Next
        Dim DTO = New DataObject
        DTO.SetFileDropList(SC)
        Clipboard.SetDataObject(DTO, True)
    End Sub

    Private Sub LvwFiles_KeyPress(sender As Object, e As KeyPressEventArgs) Handles LvwFiles.KeyPress
        If Not TvwExplorer.SelectedNode Is Nothing Then
            If LvwFiles.SelectedItems.Count > 0 Then
                If e.KeyChar = Chr(3) Then
                    MenuCopyFile_Click(sender, New EventArgs())
                ElseIf e.KeyChar = Chr(24) Then
                    MenuCutFile_Click(sender, New EventArgs())
                End If
            End If
            If e.KeyChar = Chr(22) Then
                MenuPasteFile_Click(sender, New EventArgs())
            End If
        End If
    End Sub

    Private Sub MenuPasteFile_Click(sender As Object, e As EventArgs) Handles MenuPasteFile.Click
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        Dim filelist As System.Collections.Specialized.StringCollection
        filelist = My.Computer.Clipboard.GetFileDropList()
        Dim DropEffectData(3) As Byte
        Dim DropEffectCheck As Object = My.Computer.Clipboard.GetData("Preferred DropEffect")
        DropEffectCheck.Read(DropEffectData, 0, DropEffectData.Length)
        For Each filePath As String In filelist
            If IsDirectory(filePath) Then
                ProcessImportDirectory(treeItem, sPath, filePath, IIf(DropEffectData(0) = 2, DragDropEffects.Move, DragDropEffects.Copy))
            Else
                ImportFile(treeItem, sPath, filePath, IIf(DropEffectData(0) = 2, DragDropEffects.Move, DragDropEffects.Copy), False)
            End If
        Next
    End Sub

    Private Sub MenuCutFile_Click(sender As Object, e As EventArgs) Handles MenuCutFile.Click
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim SC As New System.Collections.Specialized.StringCollection
        Dim item As ListViewItem
        For Each item In LvwFiles.SelectedItems
            SC.Add(sPath + item.Text)
        Next
        Dim DTO = New DataObject
        'DTO.SetData("FileDrop", SC)
        DTO.SetFileDropList(SC)
        'DTO.SetData("Preferred DropEffect", DragDropEffects.Move)
        DTO.SetData("Preferred DropEffect", New MemoryStream(BitConverter.GetBytes(Convert.ToInt32(DragDropEffects.Move))))
        Clipboard.SetDataObject(DTO, True)
        'FilesRefreshTimer.Enabled = True
    End Sub

    Private FilesRefreshTimerIsRunning As Boolean

    Private Sub FilesRefreshTimer_Tick(sender As Object, e As EventArgs) Handles FilesRefreshTimer.Tick
        On Error Resume Next
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        ElseIf TvwExplorer.SelectedNode.Name = "reminder" Then
            Exit Sub
        ElseIf IsBrowserNode(TvwExplorer.SelectedNode) Then
            Exit Sub
        End If
        FilesRefreshTimerIsRunning = True
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        Dim i As Integer
        For i = LvwFiles.Items.Count - 1 To 0 Step -1
            If Not IO.File.Exists(sPath & LvwFiles.Items(i).Text) Then
                LvwFiles.Items(i).Remove()
            End If
        Next
        For Each file In Directory.GetFiles(sPath)
            If Not file Is Nothing Then
                Dim shortFile As String = Path.GetFileName(file)
                If Not ListViewItemExists(shortFile) Then
                    Dim treeItemFile = AddNewTreeItemFile(treeItem.Id, shortFile)
                    If Not treeItemFile Is Nothing Then
                        Dim fileInfo = GetFileInfo(sPath, shortFile)
                        fileInfo.TreeItemFile = treeItemFile
                        Dim newItem As ListViewItem = AddFileItem(treeItem.Id, fileInfo, LvwFiles.Items.Count + 1)
                    End If
                End If
            End If
        Next
        FilesRefreshTimerIsRunning = False
    End Sub

    Private Function ListViewItemExists(itemText As String) As Boolean
        ListViewItemExists = False
        Dim item As ListViewItem
        For Each item In LvwFiles.Items
            If item.Text = itemText Then
                ListViewItemExists = True
                Exit For
            End If
        Next
    End Function

    Private Function GetListViewItemByText(itemText As String) As ListViewItem
        Dim item As ListViewItem
        For Each item In LvwFiles.Items
            If item.Text = itemText Then
                GetListViewItemByText = item
                Exit Function
            End If
        Next
        GetListViewItemByText = Nothing
    End Function

    Private Sub MenuOpenInExplorer_Click(sender As Object, e As EventArgs) Handles MenuOpenInExplorer.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        If Not IsBrowserNode(TvwExplorer.SelectedNode) Then
            Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
            'Process.Start("explorer.exe", sPath)
            'Shell("explorer " & sPath, AppWinStyle.NormalFocus)
            'Directory.SetCurrentDirectory(sPath)
            'Process.Start("explorer.exe")
            Process.Start(sPath)
        Else
            Try
                If Not chromeExe Is Nothing Then
                    Process.Start("chrome.exe", GetUrlFromNode(TvwExplorer.SelectedNode))
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler beim Öffnen")
            End Try
        End If
    End Sub

    Private Sub MenuInternCopy_Click(sender As Object, e As EventArgs) Handles MenuInternCopy.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        copyNodeKey = TvwExplorer.SelectedNode.Name
        copyMode = 0
        MenuInternPaste.Enabled = True
    End Sub

    Private Sub MenuInternCut_Click(sender As Object, e As EventArgs) Handles MenuInternCut.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        copyNodeKey = TvwExplorer.SelectedNode.Name
        copyMode = 1
        MenuInternPaste.Enabled = True
    End Sub

    Private Sub MenuInternPaste_Click(sender As Object, e As EventArgs) Handles MenuInternPaste.Click
        If TvwExplorer.SelectedNode Is Nothing Or copyNodeKey = "" Then
            Exit Sub
        End If
        Dim copyNode As TreeNode, msgResult As MsgBoxResult
        copyNode = GetTreeNodeByKey(copyNodeKey)
        If copyNode Is Nothing Then
            Exit Sub
        End If
        msgResult = MsgBox("Enthaltende Dateien auch kopieren?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question, "Nachfrage")
        If msgResult = MsgBoxResult.Cancel Then
            Exit Sub
        End If
        Dim copyItem As OrgManTreeItem = DirectCast(copyNode.Tag, OrgManTreeItem)
        CopyNodes(copyItem, TvwExplorer.SelectedNode, copyMode, msgResult = MsgBoxResult.Yes)
        MenuInternPaste.Enabled = (copyMode <> 1)
    End Sub

    Private Sub MenuWinProperties_Click(sender As Object, e As EventArgs) Handles MenuWinProperties.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim fi As New IO.FileInfo(fullPath)
        Dim info As New SHELLEXECUTEINFO()
        info.cbSize = Marshal.SizeOf(info)
        info.lpVerb = "properties"
        info.lpFile = fi.Name
        info.lpDirectory = fi.DirectoryName
        info.nShow = SW_SHOW
        info.fMask = SEE_MASK_INVOKEIDLIST
        ShellExecuteEx(info)
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


    Private Sub MenuRenameFile_Click(sender As Object, e As EventArgs) Handles MenuRenameFile.Click
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim newText As String
        newText = LvwFiles.SelectedItems(0).Text
mRetry:
        newText = InputBox("Bitte neuen Namen eingeben:", "Datei umbenennen", newText)
        If newText <> "" And newText <> LvwFiles.SelectedItems(0).Text Then
            If IO.File.Exists(sPath & newText) Then
                MsgBox("Der Name existiert bereits in diesem Ordner!", MsgBoxStyle.Exclamation, "Hinweis")
                GoTo mRetry
            End If
            Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, OrgManTreeItemFile)
            fileItem.Filename = newText
            dbc.SaveTreeItemFile(fileItem)
            IO.File.Move(sPath & LvwFiles.SelectedItems(0).Text, sPath & newText)
            LvwFiles.SelectedItems(0).Text = newText
        End If
    End Sub

    Private Sub MenuComboFolderSortWay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MenuComboFolderSortWay.SelectedIndexChanged
        If TvwExplorer.SelectedNode Is Nothing Then
            If MenuComboFolderSortWay.SelectedIndex <> Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortWay", "0")) Then
                dbc.SaveDbSetting("RootChildrenSortWay", MenuComboFolderSortWay.SelectedIndex.ToString())
                LoadTree()
            End If
        Else
            Dim node As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
            If node.ChildrenSortWay <> MenuComboFolderSortWay.SelectedIndex Then
                node.ChildrenSortWay = MenuComboFolderSortWay.SelectedIndex
                dbc.SaveTreeItem(node)
                If TvwExplorer.SelectedNode.Nodes.Count > 0 Then
                    TvwExplorer.SelectedNode.Nodes.Clear()
                    LoadChildren(TvwExplorer.SelectedNode)
                    TvwExplorer.SelectedNode.Expand()
                End If
            End If
        End If
    End Sub

    Private Sub MenuComboFileSortBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MenuComboFileSortBy.SelectedIndexChanged
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim node As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        If node.FilesSortBy <> MenuComboFileSortBy.SelectedIndex Then
            node.FilesSortBy = MenuComboFileSortBy.SelectedIndex
            dbc.SaveTreeItem(node)
            LoadFiles()
        End If
    End Sub

    Private Sub MenuComboFileSortWay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MenuComboFileSortWay.SelectedIndexChanged
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim node As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        If node.FilesSortWay <> MenuComboFileSortWay.SelectedIndex Then
            node.FilesSortWay = MenuComboFileSortWay.SelectedIndex
            dbc.SaveTreeItem(node)
            LoadFiles()
        End If
    End Sub

    Private Sub MenuMoveUp_Click(sender As Object, e As EventArgs) Handles MenuMoveUp.Click
        With TvwExplorer
            Dim hNode As TreeNode = .SelectedNode
            If Not (hNode Is Nothing) Then
                Dim offset As Integer = -10
                If Not hNode.Parent Is Nothing Then
                    Dim parentNode As OrgManTreeItem = DirectCast(hNode.Parent.Tag, OrgManTreeItem)
                    If parentNode.ChildrenSortWay = OrgManEnums.SortWay.Descending Then
                        offset = 10
                    End If
                End If
                Dim nodeItem As OrgManTreeItem = DirectCast(hNode.Tag, OrgManTreeItem)
                Dim PreviewNode As TreeNode = hNode.PrevNode
                If Not (PreviewNode Is Nothing) Then
                    Dim previewItem As OrgManTreeItem = DirectCast(PreviewNode.Tag, OrgManTreeItem)
                    .Nodes.Remove(hNode)
                    If Not (PreviewNode.Parent Is Nothing) Then
                        PreviewNode.Parent.Nodes.Insert(PreviewNode.Index, hNode)
                    Else
                        .Nodes.Insert(PreviewNode.Index, hNode)
                    End If
                    .SelectedNode = hNode
                    dbc.MoveTreeItem(nodeItem, offset)
                    nodeItem.SortOrder = nodeItem.SortOrder + offset
                    previewItem.SortOrder = previewItem.SortOrder - offset
                    .Focus()
                Else
                    .Focus()
                End If
            End If
        End With
    End Sub

    Private Sub MenuMoveDown_Click(sender As Object, e As EventArgs) Handles MenuMoveDown.Click
        With TvwExplorer
            Dim hNode As TreeNode = .SelectedNode
            If Not (hNode Is Nothing) Then
                Dim offset As Integer = 10
                If Not hNode.Parent Is Nothing Then
                    Dim parentNode As OrgManTreeItem = DirectCast(hNode.Parent.Tag, OrgManTreeItem)
                    If parentNode.ChildrenSortWay = OrgManEnums.SortWay.Descending Then
                        offset = -10
                    End If
                End If
                Dim nodeItem As OrgManTreeItem = DirectCast(hNode.Tag, OrgManTreeItem)
                Dim DownNode As TreeNode = hNode.NextNode
                If Not (DownNode Is Nothing) Then
                    Dim downItem As OrgManTreeItem = DirectCast(DownNode.Tag, OrgManTreeItem)
                    .Nodes.Remove(DownNode)
                    If Not (hNode.Parent Is Nothing) Then
                        hNode.Parent.Nodes.Insert(hNode.Index, DownNode)
                    Else
                        .Nodes.Insert(hNode.Index, DownNode)
                    End If
                    .SelectedNode = .GetNodeAt(.PointToClient(Control.MousePosition))
                    .SelectedNode = hNode
                    dbc.MoveTreeItem(nodeItem, offset)
                    nodeItem.SortOrder = nodeItem.SortOrder + offset
                    downItem.SortOrder = downItem.SortOrder - offset
                    .Focus()
                Else
                    .Focus()
                End If
            End If
        End With
    End Sub

    Private Sub TvwExplorer_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles TvwExplorer.BeforeExpand
        'TvwExplorer.SelectedNode = e.Node
    End Sub

    Private Sub MenuSecurity_Click(sender As Object, e As EventArgs) Handles MenuSecurity.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim node As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        Dim dlg = New DlgSecurity
        With dlg
            .Text = TvwExplorer.SelectedNode.Text + " - Sicherheit"
            .txtId.Text = node.Id
            .txtName.Text = node.NodeText
            Dim dlgResult As DialogResult = .ShowDialog(Me)
            'If dlgResult = DialogResult.OK Then
            '    .Close()
            'End If
        End With
    End Sub

    Private Sub MenuUserSettings_Click(sender As Object, e As EventArgs) Handles MenuUserSettings.Click
        With DlgSettings
            .TextScanFolder.Text = dbc.GetDbUserSetting("ScanImportFolder")
            Dim dlgResult As DialogResult = .ShowDialog(Me)
            If dlgResult = DialogResult.OK Then
                dbc.SaveDbUserSetting("ScanImportFolder", .TextScanFolder.Text)
                .Close()
            End If
        End With
    End Sub

    Private Sub MenuPasteScan_Click(sender As Object, e As EventArgs) Handles MenuPasteScan.Click
        If dbc.GetDbUserSetting("ScanImportFolder") = "" Then
            If MsgBox("Sie haben noch keinen Scan-Import-Ordner unter Einstellungen festgelegt. Wollen Sie das jetzt tun?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Hinweis") = MsgBoxResult.No Then
                Exit Sub
            End If
            MenuUserSettings_Click(sender, e)
        End If
        If dbc.GetDbUserSetting("ScanImportFolder") = "" Then
            Exit Sub
        End If
        With DlgScanImport
            Dim scanPath As String = dbc.GetDbUserSetting("ScanImportFolder")
            If Not scanPath Like "*\" Then
                scanPath += "\"
            End If
            .LoadFiles(scanPath)
            Dim dlgResult As DialogResult = .ShowDialog(Me)
            If dlgResult = DialogResult.OK Then
                Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
                Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
                Dim itm As ListViewItem
                For Each itm In .LvwFiles.Items
                    If itm.Selected Then
                        ImportFile(treeItem, sPath, scanPath & itm.Text, IIf(.CheckBoxDeleteAfterImport.Checked, DragDropEffects.Move, DragDropEffects.Copy), False)
                    End If
                Next
                .Close()
            End If
        End With
    End Sub

    Private Sub MenuComboFolderSortBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MenuComboFolderSortBy.SelectedIndexChanged
        If TvwExplorer.SelectedNode Is Nothing Then
            If MenuComboFolderSortBy.SelectedIndex <> Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortBy", "0")) Then
                dbc.SaveDbSetting("RootChildrenSortBy", MenuComboFolderSortBy.SelectedIndex.ToString())
                LoadTree()
            End If
        Else
            Dim node As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
            If node.ChildrenSortBy <> MenuComboFolderSortBy.SelectedIndex Then
                node.ChildrenSortBy = MenuComboFolderSortBy.SelectedIndex
                dbc.SaveTreeItem(node)
                If TvwExplorer.SelectedNode.Nodes.Count > 0 Then
                    TvwExplorer.SelectedNode.Nodes.Clear()
                    LoadChildren(TvwExplorer.SelectedNode)
                    TvwExplorer.SelectedNode.Expand()
                End If
            End If
        End If
    End Sub

    Private Sub MenuPasteFileSelect_Click(sender As Object, e As EventArgs) Handles MenuPasteFileSelect.Click
        Using ofd As New OpenFileDialog
            ofd.Title = "Datei(en) auswählen zum Einfügen"
            ofd.Multiselect = True
            If ofd.ShowDialog() <> DialogResult.OK Then Return
            Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
            Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
            For Each filePath As String In ofd.FileNames
                ImportFile(treeItem, sPath, filePath, DragDropEffects.Copy, False)
            Next
        End Using
    End Sub

    Private Sub MenuEnvDev_Click(sender As Object, e As EventArgs) Handles MenuEnvDev.Click
        ChangeEnvironment("Dev")
    End Sub

    Private Sub MenuEnvLocal_Click(sender As Object, e As EventArgs) Handles MenuEnvLocal.Click
        ChangeEnvironment("Local")
    End Sub

    Private Sub MenuEnvTest_Click(sender As Object, e As EventArgs) Handles MenuEnvTest.Click
        ChangeEnvironment("Test")
    End Sub

    Private Sub MenuEnvProd_Click(sender As Object, e As EventArgs) Handles MenuEnvProd.Click
        ChangeEnvironment("Prod")
    End Sub

    Private Sub ChangeEnvironment(env As String)
        RegistryHandler.SetStringValue(Application.ProductName, "Environment", env)
        Application.Restart()
        Me.Close()
        End
    End Sub

    Private Sub TvwExplorer_MouseClick(sender As Object, e As MouseEventArgs) Handles TvwExplorer.MouseClick
        'If e.Button = MouseButtons.Right Then
        '    Dim pt As Point =
        '    CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
        '    Dim clickNode = TvwExplorer.GetNodeAt(pt)
        '    If clickNode Is Nothing Then
        '        TvwExplorer.SelectedNode = Nothing
        '    End If
        'End If
    End Sub

    Private Sub MenuReminderFile_Click(sender As Object, e As EventArgs) Handles MenuReminderFile.Click
        With DlgReminder
            If Not String.IsNullOrEmpty(LvwFiles.SelectedItems(0).SubItems(ColumnHeaderReminder.Index).Text.Trim()) Then
                Dim reminderDate As DateTime = DateTime.Parse(LvwFiles.SelectedItems(0).SubItems(ColumnHeaderReminder.Index).Text)
                .TextBoxDate.Text = reminderDate.Date.ToShortDateString()
                .TextBoxTime.Text = reminderDate.TimeOfDay.ToString()
            End If
            Dim dlgResult As DialogResult = .ShowDialog(Me)
            If dlgResult = DialogResult.OK Then
                Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
                Dim reminderDate As Date
                If DateTime.TryParse(.TextBoxDate.Text, reminderDate) Then
                    Dim timeDate As Date
                    If DateTime.TryParse(.TextBoxTime.Text, timeDate) Then
                        reminderDate = reminderDate.Add(timeDate.TimeOfDay)
                    End If
                    dbc.SetReminderDate(treeItem.Id, LvwFiles.SelectedItems(0).SubItems(ColumnHeaderName.Index).Text, reminderDate)
                    LvwFiles.SelectedItems(0).SubItems(ColumnHeaderReminder.Index).Text = IIf(reminderDate = Nothing, " ", reminderDate.ToString())
                    RefreshReminders()
                End If
                .Close()
            End If
        End With
    End Sub

    Private Sub LvwFiles_KeyDown(sender As Object, e As KeyEventArgs) Handles LvwFiles.KeyDown
        If e.Control And e.KeyCode = Keys.A Then
            Dim itm As ListViewItem
            For Each itm In LvwFiles.Items
                itm.Selected = True
            Next
        End If
    End Sub

    Private Sub MenuDoneReminderFile_Click(sender As Object, e As EventArgs) Handles MenuDoneReminderFile.Click
        If MsgBox("Wiedervorlage auf Status Erledigt setzen?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.No Then
            Exit Sub
        End If
        Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        Dim filename = LvwFiles.SelectedItems(0).SubItems(ColumnHeaderName.Index).Text
        dbc.FinishReminder(treeItem.Id, filename)
        LvwFiles.SelectedItems(0).SubItems(ColumnHeaderReminder.Index).Text = " "
        RefreshReminders()
    End Sub

    Private Sub MenuDeleteReminderFile_Click(sender As Object, e As EventArgs) Handles MenuDeleteReminderFile.Click
        If MsgBox("Wiedervorlage unwiderruflich löschen?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.No Then
            Exit Sub
        End If
        Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        Dim filename = LvwFiles.SelectedItems(0).SubItems(ColumnHeaderName.Index).Text
        dbc.DeleteReminderDate(treeItem.Id, filename)
        LvwFiles.SelectedItems(0).SubItems(ColumnHeaderReminder.Index).Text = " "
        RefreshReminders()
    End Sub

    Private Sub MenuSortMoveUpFile_Click(sender As Object, e As EventArgs) Handles MenuSortMoveUpFile.Click
        With LvwFiles
            If Not .SelectedItems(0).Index = 0 Then
                Dim offset As Integer = -10
                Dim treeNode As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
                If treeNode.FilesSortWay = OrgManEnums.SortWay.Descending Then
                    offset = 10
                End If
                Dim toMove As ListViewItem, inPreview As ListViewItem
                Dim oldIndex As Integer

                oldIndex = .SelectedItems(0).Index
                toMove = .SelectedItems(0)
                inPreview = .Items(oldIndex - 1)
                Dim moveItem As OrgManTreeItemFile = DirectCast(toMove.Tag, OrgManTreeItemFile)
                Dim previewItem As OrgManTreeItemFile = DirectCast(inPreview.Tag, OrgManTreeItemFile)

                .Items.Remove(toMove)
                .Items.Insert(oldIndex - 1, toMove)

                dbc.MoveTreeItemFile(moveItem, offset)
                moveItem.SortOrder = moveItem.SortOrder + offset
                previewItem.SortOrder = previewItem.SortOrder - offset

                toMove.Selected = True
                toMove.Focused = True
            End If
        End With
    End Sub

    Private Sub MenuSortMoveDownFile_Click(sender As Object, e As EventArgs) Handles MenuSortMoveDownFile.Click
        With LvwFiles
            If Not .SelectedItems(0).Index + 1 = .Items.Count Then
                Dim offset As Integer = 10
                Dim treeNode As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
                If treeNode.FilesSortWay = OrgManEnums.SortWay.Descending Then
                    offset = -10
                End If
                Dim toMove As ListViewItem, inDown As ListViewItem
                Dim oldIndex As Integer

                oldIndex = .SelectedItems(0).Index
                toMove = .SelectedItems(0)
                inDown = .Items(oldIndex + 1)
                Dim moveItem As OrgManTreeItemFile = DirectCast(toMove.Tag, OrgManTreeItemFile)
                Dim downItem As OrgManTreeItemFile = DirectCast(inDown.Tag, OrgManTreeItemFile)

                .Items.Remove(toMove)
                .Items.Insert(oldIndex + 1, toMove)

                dbc.MoveTreeItemFile(moveItem, offset)
                moveItem.SortOrder = moveItem.SortOrder + offset
                downItem.SortOrder = downItem.SortOrder - offset

                toMove.Selected = True
                toMove.Focused = True
            End If
        End With
    End Sub

    Private Sub ContextMenuFiles_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuFiles.Opening
        MenuOpenFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        MenuCutFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        MenuCopyFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        MenuPasteFile.Enabled = Not TvwExplorer.SelectedNode Is Nothing
        MenuPasteFileSelect.Enabled = Not TvwExplorer.SelectedNode Is Nothing
        MenuPasteScan.Enabled = Not TvwExplorer.SelectedNode Is Nothing
        MenuDeleteFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        MenuRenameFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        MenuReminderFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        Dim reminderDate As String = String.Empty
        If LvwFiles.SelectedItems.Count > 0 Then
            reminderDate = LvwFiles.SelectedItems(0).SubItems(ColumnHeaderReminder.Index).Text.Trim()
        End If
        MenuDoneReminderFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0 And Not String.IsNullOrEmpty(reminderDate))
        MenuDeleteReminderFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0 And Not String.IsNullOrEmpty(reminderDate))
        MenuSortFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        Dim treeNode As OrgManTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
        MenuSortMoveUpFile.Enabled = (treeNode.FilesSortBy = OrgManEnums.FilesSortBy.UserDefined)
        MenuSortMoveDownFile.Enabled = (treeNode.FilesSortBy = OrgManEnums.FilesSortBy.UserDefined)
        MenuRefreshFiles.Enabled = Not TvwExplorer.SelectedNode Is Nothing
        MenuPropertiesFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
    End Sub

    Private Function GetTreeNodeOfReminder(treeItemId As Integer) As TreeNode
        Dim homeNode = GetTreeNodeByKey("C" & treeItemId)
        If homeNode Is Nothing Then
            homeNode = GetTreeNodeByKey("R" & treeItemId)
        End If
        GetTreeNodeOfReminder = homeNode
    End Function

    Private Sub MenuReminderOpen_Click(sender As Object, e As EventArgs) Handles MenuReminderOpen.Click
        Dim reminderItem = DirectCast(LvwReminders.SelectedItems(0).Tag, OrgManReminder)
        Dim homeNode = GetTreeNodeOfReminder(reminderItem.TreeItemId)
        Dim fullPath As String = GetFullPathOfNode(homeNode)
        System.Diagnostics.Process.Start(fullPath + LvwReminders.SelectedItems(0).Text)
    End Sub

    Private Sub LvwReminders_DoubleClick(sender As Object, e As EventArgs) Handles LvwReminders.DoubleClick
        If MenuReminderOpen.Enabled Then
            MenuReminderOpen_Click(sender, e)
        End If
    End Sub

    Private Sub ContextMenuReminder_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuReminder.Opening
        MenuReminderOpen.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwReminders.SelectedItems.Count > 0)
        MenuReminderEdit.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwReminders.SelectedItems.Count > 0)
        MenuReminderDone.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwReminders.SelectedItems.Count > 0)
        MenuReminderDelete.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwReminders.SelectedItems.Count > 0)

        MenuReminderOpenInTree.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwReminders.SelectedItems.Count > 0)
        MenuReminderProperties.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwReminders.SelectedItems.Count > 0)

        MenuReminderRefresh.Enabled = (Not TvwExplorer.SelectedNode Is Nothing)
    End Sub

    Private Sub MenuExplorerReminderNewDepartment_Click(sender As Object, e As EventArgs) Handles MenuExplorerReminderNewDepartment.Click
        MenuNewDepartment_Click(sender, e)
    End Sub

    Friend SecurityIsLoading As Boolean

    Private Async Sub TimerLoadSecurity_TickAsync(sender As Object, e As EventArgs) Handles TimerLoadSecurity.Tick
        TimerLoadSecurity.Enabled = False
        If Not SecurityIsLoading And AdGroups Is Nothing Then
            SecurityIsLoading = True
            Me.Cursor = Cursors.AppStarting
            Dim t = New Task(Sub() LoadSecurityData())
            t.Start()
            Await t
            SecurityIsLoading = False
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub LoadSecurityData()
        Dim adcon As New WinAdConnector()
        If adcon.DomainIsAvailable() Then
            AdGroups = adcon.GetGroups()
            AdUsers = adcon.GetDomainUsers(True)
        End If
    End Sub

    Private Sub LvwReminders_KeyPress(sender As Object, e As KeyPressEventArgs) Handles LvwReminders.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If (MenuReminderOpen.Enabled) Then
                e.Handled = True
                MenuReminderOpen_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub MenuReminderOpenInTree_Click(sender As Object, e As EventArgs) Handles MenuReminderOpenInTree.Click
        Dim reminderItem = DirectCast(LvwReminders.SelectedItems(0).Tag, OrgManReminder)
        Dim homeNode = GetTreeNodeOfReminder(reminderItem.TreeItemId)
        TreeNodeClick(homeNode)
        LvwFiles.Focus()
        Dim listItem = GetListViewItemByText(reminderItem.Filename)
        listItem.Selected = True
        listItem.Focused = True
    End Sub

    Private Sub MenuReminderRefresh_Click(sender As Object, e As EventArgs) Handles MenuReminderRefresh.Click
        LoadReminders()
    End Sub

    Private Sub LvwReminders_KeyUp(sender As Object, e As KeyEventArgs) Handles LvwReminders.KeyUp
        If e.KeyCode = Keys.F5 Then
            If MenuReminderRefresh.Enabled Then
                MenuReminderRefresh_Click(sender, New EventArgs())
            End If
        End If
    End Sub

    Private Sub MenuReminderProperties_Click(sender As Object, e As EventArgs) Handles MenuReminderProperties.Click
        Dim reminderItem = DirectCast(LvwReminders.SelectedItems(0).Tag, OrgManReminder)
        Dim homeNode = GetTreeNodeOfReminder(reminderItem.TreeItemId)
        Dim fullPath As String = GetFullPathOfNode(homeNode)
        Dim fi As New IO.FileInfo(fullPath + LvwReminders.SelectedItems(0).Text)
        Dim info As New SHELLEXECUTEINFO()
        info.cbSize = Marshal.SizeOf(info)
        info.lpVerb = "properties"
        info.lpFile = fi.Name
        info.lpDirectory = fi.DirectoryName
        info.nShow = SW_SHOW
        info.fMask = SEE_MASK_INVOKEIDLIST
        ShellExecuteEx(info)
    End Sub

    Private Sub MenuReminderEdit_Click(sender As Object, e As EventArgs) Handles MenuReminderEdit.Click
        With DlgReminder
            Dim reminderDate As DateTime = DateTime.Parse(LvwReminders.SelectedItems(0).SubItems(ColumnHeaderReminderReminderDate.Index).Text)
            .TextBoxDate.Text = reminderDate.Date.ToShortDateString()
            .TextBoxTime.Text = reminderDate.TimeOfDay.ToString()
            Dim dlgResult As DialogResult = .ShowDialog(Me)
            If dlgResult = DialogResult.OK Then
                reminderDate = Nothing
                'Dim treeItem = DirectCast(TvwExplorer.SelectedNode.Tag, OrgManTreeItem)
                Dim reminderItem = DirectCast(LvwReminders.SelectedItems(0).Tag, OrgManReminder)
                If DateTime.TryParse(.TextBoxDate.Text, reminderDate) Then
                    Dim timeDate As Date
                    If DateTime.TryParse(.TextBoxTime.Text, timeDate) Then
                        reminderDate = reminderDate.Add(timeDate.TimeOfDay)
                    End If
                    dbc.SetReminderDate(reminderItem.TreeItemId, LvwReminders.SelectedItems(0).SubItems(ColumnHeaderReminderFilename.Index).Text, reminderDate)
                    LvwReminders.SelectedItems(0).SubItems(ColumnHeaderReminderReminderDate.Index).Text = IIf(reminderDate = Nothing, " ", reminderDate.ToString())
                    If reminderDate = Nothing Or reminderDate > DateTime.Now Then
                        LvwReminders.Items.Remove(LvwReminders.SelectedItems(0))
                    End If
                    RefreshReminders()
                    StatusLabelInfo.Text = LvwReminders.Items.Count & " Wiedervolage(n)"
                End If
                .Close()
            End If
        End With
    End Sub

    Private Sub MenuReminderDone_Click(sender As Object, e As EventArgs) Handles MenuReminderDone.Click
        If MsgBox("Wiedervorlage auf Status Erledigt setzen?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.No Then
            Exit Sub
        End If
        Dim reminderItem = DirectCast(LvwReminders.SelectedItems(0).Tag, OrgManReminder)
        dbc.FinishReminder(reminderItem.TreeItemId, reminderItem.Filename)
        LvwReminders.Items.Remove(LvwReminders.SelectedItems(0))
        RefreshReminders()
        StatusLabelInfo.Text = LvwReminders.Items.Count & " Wiedervolage(n)"
    End Sub

    Private Sub MenuReminderDelete_Click(sender As Object, e As EventArgs) Handles MenuReminderDelete.Click
        If MsgBox("Wiedervorlage unwiderruflich löschen?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.No Then
            Exit Sub
        End If
        Dim reminderItem = DirectCast(LvwReminders.SelectedItems(0).Tag, OrgManReminder)
        dbc.DeleteReminderDate(reminderItem.TreeItemId, reminderItem.Filename)
        LvwReminders.Items.Remove(LvwReminders.SelectedItems(0))
        RefreshReminders()
        StatusLabelInfo.Text = LvwReminders.Items.Count & " Wiedervolage(n)"
    End Sub

    Private Sub MenuShowFilePreviewer_Click(sender As Object, e As EventArgs) Handles MenuShowFilePreviewer.Click
        MenuShowFilePreviewer.Checked = Not MenuShowFilePreviewer.Checked
        dbc.SaveDbUserSetting("ShowFilePreviewer", MenuShowFilePreviewer.Checked.ToString())
        If MenuShowFilePreviewer.Checked Then
            ShowFilePreviewer()
            LvwFiles_SelectedIndexChanged(sender, e)
        Else
            HideFilePreviewer()
        End If
        MenuReminderShowFilePreviewer.Checked = MenuShowFilePreviewer.Checked
    End Sub

    Private Sub MenuReminderShowFilePreviewer_Click(sender As Object, e As EventArgs) Handles MenuReminderShowFilePreviewer.Click
        MenuReminderShowFilePreviewer.Checked = Not MenuReminderShowFilePreviewer.Checked
        dbc.SaveDbUserSetting("ShowFilePreviewer", MenuReminderShowFilePreviewer.Checked.ToString())
        If MenuReminderShowFilePreviewer.Checked Then
            ShowFilePreviewer()
            LvwReminders_SelectedIndexChanged(sender, e)
        Else
            HideFilePreviewer()
        End If
        MenuShowFilePreviewer.Checked = MenuReminderShowFilePreviewer.Checked
    End Sub

    Private Sub LvwReminders_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LvwReminders.SelectedIndexChanged
        If Not Me.FilesSplitContainer.Panel2Collapsed Then
            If (Not TvwExplorer.SelectedNode Is Nothing And LvwReminders.SelectedItems.Count > 0) Then
                Dim reminderItem = DirectCast(LvwReminders.SelectedItems(0).Tag, OrgManReminder)
                Dim homeNode = GetTreeNodeOfReminder(reminderItem.TreeItemId)
                Dim fullPath As String = GetFullPathOfNode(homeNode)
                FilePreviewHandlerHost.Open(fullPath + LvwReminders.SelectedItems(0).Text)
            Else
                FilePreviewHandlerHost.Open(String.Empty)
            End If
        End If
    End Sub

    Private Sub ContextMenuExplorerReminder_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuExplorerReminder.Opening
        MenuExplorerReminderNewDepartment.Enabled = IsAdmin And TvwExplorer.SelectedNode Is Nothing
    End Sub

    Private Sub ContextMenuExplorer_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuExplorer.Opening
        'MenuNewFolder.Enabled = Not TvwExplorer.SelectedNode Is Nothing
        MenuCut.Enabled = Not TvwExplorer.SelectedNode Is Nothing
    End Sub

    Private Sub RefreshReminders()
        Dim count = dbc.GetReminders().Count
        Dim reminderNode = GetTreeNodeByKey("reminder")
        If count > 0 Then
            reminderNode.NodeFont = New Font(TvwExplorer.Font, FontStyle.Bold)
            reminderNode.Text = "Wiedervorlagen (" & count & ")"
        Else
            reminderNode.NodeFont = New Font(TvwExplorer.Font, FontStyle.Regular)
            reminderNode.Text = "Wiedervorlagen"
        End If
    End Sub
    Private Function AddNewTreeItemFile(treeItemId As Integer, fileName As String) As OrgManTreeItemFile
        Dim newFile As OrgManTreeItemFile = dbc.AddNewTreeItemFile(treeItemId, fileName)
        AddNewTreeItemFile = newFile
    End Function

End Class