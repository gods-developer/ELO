Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports IWshRuntimeLibrary
Imports OrgMan
Imports DigiSped.Common.Tools
Imports CefSharp
Imports CefSharp.WinForms
Imports DigiSped.Common.Tools.Logging
Imports DigiSped.Common.Tools.Filesystem
Imports DigiSped.Common.Database.Logging
Imports DigiSped.Common
Imports DigiSped.Common.Logging

Public Class FrmMain
    Dim copyNodeKey As String, copyMode As Byte
    Public dbc As MsSqlConnector
    Public IsAdmin As Boolean
    Public TemplateFolderName As String, DefaultRootPath As String
    'Public chrome As ChromiumWebBrowser, chromeExe As String

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

    Private initDir As String

    Private Sub CheckDbConnection()
        If Not dbc Is Nothing Then
            Exit Sub
        End If
        dbc = New MsSqlConnector
        Try
            If Not dbc.HasAccess() Then
                MsgBox("Sie haben keine Berechtigung, den Organisationsmanager zu benutzen!" & vbCrLf & "Bitte wenden Sie sich an den Administrator.", MsgBoxStyle.Exclamation, "Hinweis")
                End
            End If
        Catch ex As Exception
            MsgBox("Es ist ein Fehler aufgetreten beim Verbinden mit der Datenbank!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Fehler beim Starten")
            If ArcExplorerGlobals.AppEnvironment <> My.Settings.EnvironmentName Then
                MsgBox("Es wird versucht, die Standard-Umgebung [" & My.Settings.EnvironmentName & "] zu starten...", MsgBoxStyle.Information, "Hinweis")
                RegistryHandler.SetStringValue(Application.ProductName, "Environment", My.Settings.EnvironmentName)
                Application.Restart()
            End If
            End
        End Try
        DsErrorHandler.Initialize(New DsDatabaseLogger(dbc.DataSource, dbc.InitialCatalog, Application.ProductName, Application.ProductName + " " + Application.ProductVersion, DsLogLevel.Warning))
        IsAdmin = dbc.IsAdmin()
        Me.DefaultRootPath = dbc.GetDbSetting("DefaultRootPath", "C:\OrgMan")
        Me.TemplateFolderName = dbc.GetDbSetting("TemplateFolderName", "Vorlagen")
    End Sub

    Private Sub SetEnvironment(env As String)
        ArcExplorerGlobals.AppEnvironment = env 'todo
        'DropDownEnvironment.Text = OrgManGlobals.AppEnvironment
    End Sub

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initDir = ""
        Dim parameters = ApplicationHelper.GetStartUpParameters()
        Dim exe = System.Reflection.Assembly.GetEntryAssembly().Location
        For Each param In parameters
            If param <> exe And param.StartsWith("init.dir=") Then
                initDir = param.Substring(param.IndexOf("=") + 1)
            End If
        Next
        'initDir = Command() '"A:\Elo_Neuarchiv_29..09.2019 0930\000353F6"
        If String.IsNullOrEmpty(initDir) Or Dir(initDir, FileAttribute.Directory) = "" Then
            MsgBox("Der ELO Archiv Explorer muss mit einem gültigen Pfad als Parameter gestartet werden!", MsgBoxStyle.Exclamation, "Fehler beim Starten")
            End
        End If
        'Dim args = New ApplicationHelper().GetStartUpParameters()
        'For Each a In args
        '    'MsgBox(a)
        '    If a Like "environment=*" Then
        '        OrgManGlobals.AppEnvironment = a.Substring(a.IndexOf("=") + 1)
        '    End If
        'Next
        SetEnvironment(My.Settings.Default.EnvironmentName)
        'InitChromeControl()
        'FindChromeExe()
        'FilesRefreshTimer.Enabled = My.Settings.ListAutoRefresh
        Me.Text += " " + Application.ProductVersion + " [" + initDir + "]" + If(IsAdmin, " (" + dbc.Path + ")", "")
        Me.WindowState = GetSetting(Application.ProductName, "Window", "LastWindowState", FormWindowState.Normal)
        Me.Top = GetSetting(Application.ProductName, "Window", "LastWindowTop", Me.Top)
        Me.Left = GetSetting(Application.ProductName, "Window", "LastWindowLeft", Me.Left)
        Me.Width = GetSetting(Application.ProductName, "Window", "LastWindowWidth", Me.Width)
        Me.Height = GetSetting(Application.ProductName, "Window", "LastWindowHeight", Me.Height)
        Me.MainSplitContainer.SplitterDistance = GetSetting(Application.ProductName, "Window", "LastMainSplitterDistance", Me.MainSplitContainer.SplitterDistance)
        If Boolean.Parse(GetSetting(Application.ProductName, "Window", "ShowFilePreviewer", "False")) Then
            Me.FilesSplitContainer.SplitterDistance = GetSetting(Application.ProductName, "Window", "LastFilesSplitterDistance", Me.FilesSplitContainer.SplitterDistance)
            MenuShowFilePreviewer.Checked = True
        Else
            HideFilePreviewer()
        End If
        Me.LvwFiles.Columns(0).Width = GetSetting(Application.ProductName, "Window", "LastColumn0With", Me.LvwFiles.Columns(0).Width)
        Me.LvwFiles.Columns(1).Width = GetSetting(Application.ProductName, "Window", "LastColumn1With", Me.LvwFiles.Columns(1).Width)
        Me.LvwFiles.Columns(2).Width = GetSetting(Application.ProductName, "Window", "LastColumn2With", Me.LvwFiles.Columns(2).Width)
        Me.LvwFiles.Columns(3).Width = GetSetting(Application.ProductName, "Window", "LastColumn3With", Me.LvwFiles.Columns(3).Width)
        LoadTree()
    End Sub

    Private Sub HideFilePreviewer()
        Me.FilesSplitContainer.Panel2Collapsed = True
        Me.FilesSplitContainer.Panel2.Hide()
    End Sub

    Private Sub ShowFilePreviewer()
        Me.FilesSplitContainer.Panel2Collapsed = False
        Me.FilesSplitContainer.Panel2.Show()
    End Sub

    Private Function GetEswValue(item As String, key As String) As String
        'Dim fname = item.Substring(0, item.LastIndexOf("."))
        GetEswValue = IniFileHelper.IniReadValue(item & ".ESW", "GENERAL", key, "???")
    End Function

    Private Function SetEswValue(item As String, key As String, val As String) As String
        'Dim fname = item.Substring(0, item.LastIndexOf("."))
        IniFileHelper.IniWriteValue(item & ".ESW", "GENERAL", key, val)
    End Function

    Private Sub LoadTree()
        ClearFilePreview()
        LvwFiles.Items.Clear()
        TvwExplorer.Nodes.Clear()
        Dim guid = GetEswValue(initDir, "GUID")
        Dim rootNode = TvwExplorer.Nodes.Add(guid, GetEswValue(initDir, "SHORTDESC"), "DocumentRepository.ico", "DocumentRepository.ico")
        rootNode.Tag = New ArcTreeItem(guid, initDir, GetEswValue(initDir, "ABLDATE"))
        LoadSubDirs(initDir, rootNode)

        'Dim rootNodes As List(Of OrgManTreeItem) = dbc.GetRootTreeItems()
        'rootNodes = SortNodesByConfig(rootNodes, Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortBy", "0")), Convert.ToInt32(dbc.GetDbSetting("RootChildrenSortWay", "0")))
        'For Each rootNode In rootNodes
        '    AddTreeNode(Nothing, rootNode, rootNode.NodeText, True)
        'Next
        TvwExplorer.SelectedNode = rootNode
        TvwExplorer.Sort()
        rootNode.Expand()
        TreeNodeClick(rootNode)
    End Sub

    Private Sub LoadSubDirs(baseDir As String, parentNode As TreeNode)
        Dim SourceDir As DirectoryInfo = New DirectoryInfo(baseDir)
        If SourceDir.Exists Then
            Dim SubDir As DirectoryInfo
            For Each SubDir In SourceDir.GetDirectories()
                'Console.WriteLine(SubDir.Name)
                Dim guid = GetEswValue(SubDir.FullName, "GUID")
                Dim newNode = parentNode.Nodes.Add(guid, GetEswValue(SubDir.FullName, "SHORTDESC"), "Folder.ico", "Folder.ico")
                newNode.Tag = New ArcTreeItem(guid, SubDir.Name, GetEswValue(SubDir.FullName, "ABLDATE"))
                newNode.Checked = parentNode.Checked
                newNode.Nodes.Add("..")
            Next
        End If
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
            LoadFiles()
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

    Private Function GetIdFromTag(tag As Object) As String
        Dim treeItem = DirectCast(tag, ArcTreeItem)
        GetIdFromTag = treeItem.Guid
    End Function

    Private Function GetPhysicalNameFromTag(tag As Object) As String
        Dim treeItem = DirectCast(tag, ArcTreeItem)
        GetPhysicalNameFromTag = treeItem.PhysicalName
    End Function

    Private Function GetFileIdFromTag(tag As Object) As Integer
        Dim fileItem = DirectCast(tag, OrgManListItem)
        GetFileIdFromTag = fileItem?.Id
    End Function

    Private Sub MenuNewFolder_Click(sender As Object, e As EventArgs)
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
        Dim files As List(Of ArcExplorerFileInfo) = GetFiles(sourcePath, False, False)
        Dim cp As Integer, file As ArcExplorerFileInfo
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

    Private Sub MenuNewDepartment_Click(sender As Object, e As EventArgs)
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
                    newNode = AddNewTreeItem(rootNode, Me.TemplateFolderName, True, "")
                End If
                rootNode?.Node.Expand()
            End If
        End If
    End Sub

    Private FilesAreLoading As Boolean

    Private Sub FrmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If SecurityIsLoading Or FilesAreLoading Then
            MsgBox("Es läuft noch ein Hintergrundprozess. Bitte noch ein paar Sekunden warten...", MsgBoxStyle.Information, "Hinweis")
            e.Cancel = True
            Exit Sub
        End If
        dbc?.Close()
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
        DsErrorHandler.Dispose()
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
            If MsgBox("Wollen Sie auch die im Knoten " & TvwExplorer.SelectedNode.Text & " enthaltenen Dateien samt Unterordner löschen?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.Yes Then
                If MsgBox("Ganz sicher?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.Yes Then
                    includeFiles = True
                End If
            End If
            DeleteTreeItem(TvwExplorer.SelectedNode, includeFiles)
            ClearFilePreview()
            LvwFiles.Items.Clear()
        End If
    End Sub

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
            'DeleteFiles(node)
        End If
        'Dim nodeId As Integer = GetIdFromTag(node.Tag)
        ''dbc.DeleteTreeItem(nodeId)
        Dim treePath As String = GetFullPathOfNode(node)
        Directory.Delete(treePath)
        IO.File.Delete(treePath & ".ESW")
        node.Remove()
    End Sub

    Private Sub DeleteFiles(node As TreeNode)
        Dim fullPath As String = GetFullPathOfNode(node)
        Dim files As List(Of ArcExplorerFileInfo) = GetFiles(fullPath, False, False)
        Dim cp As Integer, file As ArcExplorerFileInfo
        Dim treeItemId As Integer = GetIdFromTag(node.Tag)
        For cp = 0 To files.Count - 1
            file = files.Item(cp)
            Dim fileId = dbc.GetListItem(treeItemId, file.Filename).Id
            DeleteListItem(fileId)
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
        Dim newText As String
        newText = TvwExplorer.SelectedNode.Text
mRetry:
        newText = InputBox("Bitte neuen Namen eingeben:", "Ordner umbenennen", newText)
        If newText <> "" And newText <> TvwExplorer.SelectedNode.Text Then
            Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
            SetEswValue(fullPath, "SHORTDESC", newText)
            TvwExplorer.SelectedNode.Text = newText
        End If
    End Sub

    Private Sub MenuProperties_Click(sender As Object, e As EventArgs)
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
        MenuWinProperties.Enabled = enabled
        MenuOpenInExplorer.Enabled = enabled
        MenuDelete.Enabled = enabled
        MenuRename.Enabled = enabled
        MenuCopy.Enabled = enabled
        MenuCut.Enabled = enabled
        MenuPaste.Enabled = enabled
    End Sub

    Private Sub TvwExplorer_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TvwExplorer.NodeMouseClick
        '        If e.Node?.Name = TvwExplorer.SelectedNode?.Name Then Exit Sub
        '        TreeNodeClick(e.Node)
    End Sub
    Private Sub TreeNodeClick(clickNode As TreeNode)
        TvwExplorer.SelectedNode = clickNode
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        TvwExplorer.ContextMenuStrip = ContextMenuExplorer
        Dim node As ArcTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, ArcTreeItem)
        SetTreeContextMenu(True)
        MenuOpenInExplorer.Text = "Öffnen in Explorer"
        LoadFiles()
    End Sub

    Private Sub ClearFilePreview()
        If Not Me.FilesSplitContainer.Panel2Collapsed Then
            FilePreviewHandlerHost.Open(String.Empty)
        End If
    End Sub

    Private Sub LoadFiles()
        Me.Cursor = Cursors.AppStarting
        FilesAreLoading = True
        Dim startPoint = DateTime.Now
        StatusLabelInfo.Text = "Dateien werden aufgelistet..."
        Me.Refresh()
        ClearFilePreview()
        LvwFiles.Items.Clear()
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim files As List(Of ArcExplorerFileInfo) = GetFiles(fullPath & "\", True, False)
        For cp = 0 To files.Count - 1
            Dim file = files.Item(cp)
            AddFileItem(file, cp)
        Next
        Dim span = DateTime.Now - startPoint
        StatusLabelInfo.Text = files.Count & " Datei(en) in " & Math.Round(span.TotalSeconds) & " Sekunden"
        FilesAreLoading = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Function AddFileItem(file As ArcExplorerFileInfo, count As Integer) As ListViewItem
        Dim item As ListViewItem
        If ListViewItemExists(file.Filename) Then
            item = GetListViewItemByText(file.Filename)
            item.Tag = file
            item.SubItems(ColumnHeaderDate.Index).Text = file.FileDateTime.ToString()
            item.SubItems(ColumnHeaderSize.Index).Text = file.FileLen.ToString("#,##0")
            item.SubItems(ColumnHeaderVersion.Index).Text = file.Version
        Else
            Dim sIcon As String = AddIconToImageList(file.FilePath, file.Physicalname, ImageListFiles, "file", file.FileType)
            item = LvwFiles.Items.Add("F" & count, file.Filename, sIcon)
            item.Tag = file
            item.SubItems.Add(file.FileDateTime.ToString())
            item.SubItems.Add(file.FileType)
            item.SubItems.Add(file.FileLen.ToString("#,##0"))
            item.SubItems.Add(file.Version)
        End If
        AddFileItem = item
    End Function

    Private Function GetFullPathOfNode(node As TreeNode, Optional withoutMe As Boolean = False) As String
        Dim currentNode As TreeNode, fullPath As String ', currentNodeId As Integer, rootPath As String
        fullPath = ""
        'rootPath = ""
        currentNode = node
        'currentNodeId = GetIdFromTag(currentNode.Tag)
        If Not withoutMe Then
            fullPath = GetPhysicalNameFromTag(currentNode.Tag)
        End If
        While (currentNode.Parent IsNot Nothing)
            currentNode = currentNode.Parent
            fullPath = GetPhysicalNameFromTag(currentNode.Tag) + "\" + fullPath
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
        Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, ArcExplorerFileInfo)
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Process.Start(fullPath & "\" & fileItem.Physicalname)
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
        Dim ListItem As OrgManListItem
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
                ListItem = dbc.GetListItem(treeItem.Id, newFileName)
            Else
                ListItem = AddNewListItem(treeItem.Id, newFileName, String.Empty)
                If ListItem Is Nothing Then
                    Return False
                End If
            End If
            ImportFile = True
        Catch e As Exception
            Exit Function
        End Try
        If Not noAddToList Then
            'Dim fileInfo = GetFileInfo(sPath, newFileName)
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
            End If
        End If
    End Sub

    Private Sub MenuPropertiesFile_Click(sender As Object, e As EventArgs) Handles MenuPropertiesFile.Click
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, ArcExplorerFileInfo)
        Dim fi As New IO.FileInfo(fullPath & "\" & fileItem.Physicalname)
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
                DeleteListItem(fileId)
                KillFileIfExists(fullPath + LvwFiles.SelectedItems(0).Text)
                LvwFiles.SelectedItems(0).Remove()
            Loop
        End If
    End Sub

    Private Sub DeleteListItem(ListItemId As Integer)
        dbc.DeleteListItem(ListItemId)
    End Sub

    Private Sub MenuRefreshFiles_Click(sender As Object, e As EventArgs) Handles MenuRefreshFiles.Click
        LoadFiles()
    End Sub

    Private Sub LvwFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LvwFiles.SelectedIndexChanged
        'FilesRefreshTimer.Enabled = False
        If Not Me.FilesSplitContainer.Panel2Collapsed Then
            If (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0) Then
                Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, ArcExplorerFileInfo)
                Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
                FilePreviewHandlerHost.Open(fullPath & "\" & fileItem.Physicalname)
            Else
                FilePreviewHandlerHost.Open(String.Empty)
            End If
        End If
        Dim mnuItem As MenuItem
        Do While MenuVersions.DropDownItems.Count > 0
            MenuVersions.DropDownItems.RemoveAt(0)
        Loop
        AddVersionMenuItems()

    End Sub

    Private Sub AddVersionMenuItems()
        If LvwFiles.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, ArcExplorerFileInfo)
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim fname = fullPath & "\" & fileItem.Physicalname
        fname = fname.Substring(0, fname.LastIndexOf(".")) & ".ESW"
        Dim sections = IniFileHelper.IniReadSections(fname)
        For Each section In sections
            If section.StartsWith("DOCVERS") Then
                Dim version = IniFileHelper.IniReadValue(fname, section, "HISTVERSION")
                If String.IsNullOrEmpty(version) Then
                    version = "1.0"
                End If
                Dim docid = IniFileHelper.IniReadValue(fname, section, "DOCID")
                If docid <> "00000000" Then
                    Dim menu As New ToolStripMenuItem() With {.Text = version, .Tag = docid}
                    MenuVersions.DropDownItems.Add(menu)
                    AddHandler menu.Click, AddressOf mnuItem_Clicked
                End If
            End If
        Next
        If MenuVersions.DropDownItems.Count = 0 Then
            Dim menu As New ToolStripMenuItem() With {.Text = "keine", .Enabled = False}
            MenuVersions.DropDownItems.Add(menu)
        End If
    End Sub

    Private Sub mnuItem_Clicked(sender As Object, e As EventArgs)
        'ContextMenuStrip1.Hide() 'Sometimes the menu items can remain open.  May not be necessary for you.
        Dim item As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If item IsNot Nothing Then
            Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, ArcExplorerFileInfo)
            Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
            Dim fname = fullPath & "\" & fileItem.Physicalname
            fname = fname.Substring(0, fname.LastIndexOf(".")) & ".ESW"
            Dim extension = IniFileHelper.IniReadValue(fname, "GENERAL", "DOCEXT")
            Process.Start(fullPath & "\D" & item.Tag.ToString().Substring(1) & extension)
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

    Private Sub MenuPasteFile_Click(sender As Object, e As EventArgs)
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
        Dim sPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        'Process.Start("explorer.exe", sPath)
        'Shell("explorer " & sPath, AppWinStyle.NormalFocus)
        'Directory.SetCurrentDirectory(sPath)
        'Process.Start("explorer.exe")
        Process.Start(sPath)
    End Sub

    Private Sub MenuInternCopy_Click(sender As Object, e As EventArgs)
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        copyNodeKey = TvwExplorer.SelectedNode.Name
        copyMode = 0
    End Sub

    Private Sub MenuInternCut_Click(sender As Object, e As EventArgs)
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        copyNodeKey = TvwExplorer.SelectedNode.Name
        copyMode = 1
    End Sub

    Private Sub MenuInternPaste_Click(sender As Object, e As EventArgs)
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
    End Sub

    Private Sub MenuWinProperties_Click(sender As Object, e As EventArgs) Handles MenuWinProperties.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim fi As New IO.FileInfo(fullPath)
        Dim info As New SHELLEXECUTEINFO()
        info.hwnd = Me.Handle
        info.cbSize = Marshal.SizeOf(info)
        info.lpVerb = "properties"
        info.lpParameters = ""
        'info.lpFile = fi.Name
        info.lpFile = fi.FullName & "\"
        info.lpDirectory = Nothing 'fi.FullName ' fi.DirectoryName
        info.nShow = SW_SHOW
        info.fMask = SEE_MASK_INVOKEIDLIST
        Dim success = ShellExecuteEx(info)
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
            Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, OrgManListItem)
            fileItem.Filename = newText
            dbc.SaveListItem(fileItem)
            IO.File.Move(sPath & LvwFiles.SelectedItems(0).Text, sPath & newText)
            LvwFiles.SelectedItems(0).Text = newText
        End If
    End Sub

    Private Sub MenuMoveUp_Click(sender As Object, e As EventArgs)
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

    Private Sub MenuMoveDown_Click(sender As Object, e As EventArgs)
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

    Private treeInUpdate As Boolean

    Private Sub TvwExplorer_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles TvwExplorer.BeforeExpand
        'TvwExplorer.SelectedNode = e.Node
        If e.Node.Nodes.Count = 1 Then
            If e.Node.Nodes(0).Text = ".." Then
                Me.Cursor = Cursors.AppStarting
                e.Node.Nodes.Clear()
                If Not treeInUpdate Then
                    TvwExplorer.Refresh()
                End If
                Dim baseDir = GetFullPathOfNode(e.Node)
                LoadSubDirs(baseDir, e.Node)
                If Not treeInUpdate Then
                    TvwExplorer.Sort()
                End If
                Me.Cursor = Cursors.Default
            End If
        End If
        If Not treeInUpdate Then
            TvwExplorer.Refresh()
        End If
    End Sub

    Private Sub MenuSecurity_Click(sender As Object, e As EventArgs)
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

    Private Sub MenuUserSettings_Click(sender As Object, e As EventArgs)
        With DlgSettings
            .TextScanFolder.Text = dbc.GetDbUserSetting("ScanImportFolder")
            Dim dlgResult As DialogResult = .ShowDialog(Me)
            If dlgResult = DialogResult.OK Then
                dbc.SaveDbUserSetting("ScanImportFolder", .TextScanFolder.Text)
                .Close()
            End If
        End With
    End Sub

    Private Sub MenuPasteFileSelect_Click(sender As Object, e As EventArgs)
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

    Private Sub MenuEnvDev_Click(sender As Object, e As EventArgs)
        ChangeEnvironment("Dev")
    End Sub

    Private Sub MenuEnvLocal_Click(sender As Object, e As EventArgs)
        ChangeEnvironment("Local")
    End Sub

    Private Sub MenuEnvTest_Click(sender As Object, e As EventArgs)
        ChangeEnvironment("Test")
    End Sub

    Private Sub MenuEnvProd_Click(sender As Object, e As EventArgs)
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

    Private Sub LvwFiles_KeyDown(sender As Object, e As KeyEventArgs) Handles LvwFiles.KeyDown
        If e.Control And e.KeyCode = Keys.A Then
            Dim itm As ListViewItem
            For Each itm In LvwFiles.Items
                itm.Selected = True
            Next
        End If
    End Sub

    Private Sub MenuSortMoveUpFile_Click(sender As Object, e As EventArgs)
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
                Dim moveItem As OrgManListItem = DirectCast(toMove.Tag, OrgManListItem)
                Dim previewItem As OrgManListItem = DirectCast(inPreview.Tag, OrgManListItem)

                .Items.Remove(toMove)
                .Items.Insert(oldIndex - 1, toMove)

                dbc.MoveListItem(moveItem, offset)
                moveItem.SortOrder = moveItem.SortOrder + offset
                previewItem.SortOrder = previewItem.SortOrder - offset

                toMove.Selected = True
                toMove.Focused = True
            End If
        End With
    End Sub

    Private Sub MenuSortMoveDownFile_Click(sender As Object, e As EventArgs)
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
                Dim moveItem As OrgManListItem = DirectCast(toMove.Tag, OrgManListItem)
                Dim downItem As OrgManListItem = DirectCast(inDown.Tag, OrgManListItem)

                .Items.Remove(toMove)
                .Items.Insert(oldIndex + 1, toMove)

                dbc.MoveListItem(moveItem, offset)
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
        MenuDeleteFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        MenuRenameFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        Dim treeNode As ArcTreeItem = DirectCast(TvwExplorer.SelectedNode.Tag, ArcTreeItem)
        MenuRefreshFiles.Enabled = Not TvwExplorer.SelectedNode Is Nothing
        MenuPropertiesFile.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
        MenuEloData.Enabled = (Not TvwExplorer.SelectedNode Is Nothing And LvwFiles.SelectedItems.Count > 0)
    End Sub

    Private Function GetTreeNodeOfReminder(treeItemId As Integer) As TreeNode
        Dim homeNode = GetTreeNodeByKey("C" & treeItemId)
        If homeNode Is Nothing Then
            homeNode = GetTreeNodeByKey("R" & treeItemId)
        End If
        GetTreeNodeOfReminder = homeNode
    End Function

    Friend SecurityIsLoading As Boolean
    Friend Cancel As Boolean
    Friend CancelForm As DlgCancel

    Private Sub OpenCancelForm()
        CancelForm = New DlgCancel
        CancelForm.Show(Me)
        CancelForm.Height = MainStatusStrip.Height
        CancelForm.Width = 125
        CancelForm.Top = Me.Top + Me.Height - (CancelForm.Height + 8)
        CancelForm.Left = Me.Left + Me.Width - (CancelForm.Width + 25)
    End Sub

    Private Sub MenuEloData_Click(sender As Object, e As EventArgs) Handles MenuEloData.Click
        Me.Cursor = Cursors.AppStarting
        Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, ArcExplorerFileInfo)
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim fname = fullPath & "\" & fileItem.Physicalname
        fname = fname.Substring(0, fname.LastIndexOf(".")) & ".ESW"
        Dim sections = IniFileHelper.IniReadSections(fname)
        Dim dlg = New DlgEloData()
        dlg.Text = "ELO Daten - " & LvwFiles.SelectedItems(0).Text
        Dim fitem = dlg.LvwFiles.Items.Add("Phys. Dateiname")
        fitem.SubItems.Add(fileItem.Physicalname)
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "SHORTDESC")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "DOCEXT")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "DOCTYPE")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "DOCDATE")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "ABLDATE")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "Version")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "USER")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "ACL")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "SREG")
        AddGeneralKeyToListView(dlg.LvwFiles, fname, "GUID")
        For Each section In sections
            If section.StartsWith("KEY") Then
                Dim item = dlg.LvwFiles.Items.Add(IniFileHelper.IniReadValue(fname, section, "KEYNAME"))
                item.SubItems.Add(IniFileHelper.IniReadValue(fname, section, "KEYTEXT").Replace(Chr(182), ""))
                item.SubItems.Add(IniFileHelper.IniReadValue(fname, section, "KEYKEY"))
                item.SubItems.Add(IniFileHelper.IniReadValue(fname, section, "Acl"))
            End If
        Next
        dlg.LvwFiles.AutoResizeColumns(IIf(sections.Count() = 0, ColumnHeaderAutoResizeStyle.HeaderSize, ColumnHeaderAutoResizeStyle.ColumnContent))
        Me.Cursor = Cursors.Default
        dlg.ShowDialog(Me)
    End Sub

    Private Sub AddGeneralKeyToListView(lvw As ListView, ini As String, key As String)
        Dim fitem = lvw.Items.Add(key)
        fitem.SubItems.Add(IniFileHelper.IniReadValue(ini, "GENERAL", key))
    End Sub

    Private Sub AddGeneralKeyToDatabase(fileId As Integer, ini As String, key As String)
        Dim value = IniFileHelper.IniReadValue(ini, "GENERAL", key)
        dbc.AddNewListItemIndex(fileId, "ELO " & key, value, True)
    End Sub

    Private Sub MenuFileEloIndex_Click(sender As Object, e As EventArgs) Handles MenuFileEloIndex.Click
        If LvwFiles.SelectedItems.Count = 0 Or TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim fileItem = DirectCast(LvwFiles.SelectedItems(0).Tag, ArcExplorerFileInfo)
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim fname = fullPath & "\" & fileItem.Physicalname
        fname = fname.Substring(0, fname.LastIndexOf(".")) & ".ESW"
        Dim dlg = New DlgEditor()
        dlg.Text = New FileInfo(fname).Name
        If IO.File.Exists(fname) Then
            dlg.RichTextBoxEditor.Text = IO.File.ReadAllText(fname)
        End If
        dlg.ShowDialog(Me)
    End Sub

    Private Sub MenuEloIndex_Click(sender As Object, e As EventArgs) Handles MenuEloIndex.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim fullPath As String = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Dim fname = fullPath & ".ESW"
        Dim dlg = New DlgEditor()
        dlg.Text = New FileInfo(fname).Name
        If IO.File.Exists(fname) Then
            dlg.RichTextBoxEditor.Text = IO.File.ReadAllText(fname)
        End If
        dlg.Show(Me)
    End Sub

    Private Sub MenuExpandAll_Click(sender As Object, e As EventArgs) Handles MenuExpandAll.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        treeInUpdate = True
        TvwExplorer.BeginUpdate()
        TvwExplorer.SelectedNode.ExpandAll()
        TvwExplorer.EndUpdate()
        treeInUpdate = False
    End Sub

    Private SumBytes As Double
    Private CountFolders As Long
    Private CountFiles As Long

    Private Sub MenuMigrate_Click(sender As Object, e As EventArgs) Handles MenuMigrate.Click
        If TvwExplorer.SelectedNode Is Nothing Or Not TvwExplorer.SelectedNode.Checked Then
            Exit Sub
        End If
        SumBytes = 0
        CountFolders = 0
        CountFiles = 0
        CheckDbConnection()
        treeInUpdate = True
        TvwExplorer.BeginUpdate()
        Dim newName = GetPhysicalNameFromTag(TvwExplorer.SelectedNode.Tag)
        newName = New DirectoryInfo(newName).Name
        Dim newText = TvwExplorer.SelectedNode.Text
        newName = ConvertToValidName(newText)
        Dim newRootPath = Me.DefaultRootPath & "\" & newName
        Dim dlgResult = SelectRootPath(newRootPath)
        If dlgResult = DialogResult.Cancel Then
            TvwExplorer.EndUpdate()
            treeInUpdate = False
            Exit Sub
        End If
        Dim startPoint = DateTime.Now
        Try
            LvwFiles.Items.Clear()
            'StatusLabelInfo.Text = "Baum wird aufgebaut..."
            'Me.Refresh()
            'TvwExplorer.SelectedNode.ExpandAll()
            Me.Cancel = False
            Dim created As Boolean
            Dim rootNode = dbc.GetOrAddNewTreeItem(newName, newText, newRootPath, created)
            If created Then
                CountFolders = CountFolders + 1
                If Not rootNode Is Nothing Then
                    dbc.AddNewTreeItem(Me.TemplateFolderName, , , rootNode.Id)
                End If
            End If
            OpenCancelForm()
            Me.Refresh()
            Migrate(TvwExplorer.SelectedNode, rootNode.Id, newRootPath, created, True)
        Catch ex As Exception
            DsErrorHandler.ShowAndSaveException(NameOf(MenuMigrate_Click), ex)  'MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler")
        End Try
        TvwExplorer.EndUpdate()
        treeInUpdate = False
        CancelForm.Close()
        CancelForm = Nothing
        Dim span = DateTime.Now - startPoint
        StatusLabelInfo.Text = CountFolders & " Ordner, " & CountFiles & " Dateien, " & (Math.Round(SumBytes / 1024)) & " KB verarbeitet in " & Math.Round(span.TotalSeconds) & " Sekunden, " & Math.Round(span.TotalMinutes) & " Minuten!"
    End Sub

    Private Sub Migrate(rootNode As TreeNode, rootId As Integer, rootPath As String, processFiles As Boolean, useReference As Boolean)
        If Me.Cancel Then
            Exit Sub
        End If
        If rootId.ToString().EndsWith("0") Then
            Application.DoEvents()
        End If
        rootNode.Expand()
        Dim treePath As String = GetFullPathOfNode(rootNode)
        Dim node As TreeNode
        StatusLabelInfo.Text = "Ordner " + GetTreePathOfNode(rootNode) + " wird verarbeitet..."
        Me.Refresh()
        If processFiles Then
            'TreeNodeClick(rootNode)
            CountFolders = CountFolders + 1
            MigrateFiles(rootId, treePath, rootPath)
        End If
        'Thread.Sleep(100)
        Dim personalNr As String = Nothing
        For Each node In rootNode.Nodes
            If node Is Nothing Then
                Debug.Print("What?")
            ElseIf node?.Checked Then
                'Dim newName = ConvertToValidName(node.Text).Trim()
                Dim physName = GetPhysicalNameFromTag(node.Tag)
                Dim newText = GetEswValue(treePath & "\" & physName, "SHORTDESC")
                Dim created As Boolean
                If Me.Cancel Then
                    Exit Sub
                End If
                If useReference Then
                    personalNr = GetPersonalNrFromNode(node)
                End If
                Dim newItem = dbc.GetOrAddNewTreeItem(physName, newText, , rootId, created, personalNr)
                Migrate(node, newItem.Id, rootPath & "\" & physName, created, False)
            End If
        Next
    End Sub

    Private Function ConvertToValidName(text As String) As String
        Dim newPath = text
        Dim chars = Path.GetInvalidFileNameChars()
        Dim ch As Char
        For Each ch In chars
            newPath = newPath.Replace(ch, "")
        Next
        Do While newPath.EndsWith(".")
            newPath = newPath.Left(newPath.Length - 1)
        Loop
        ConvertToValidName = newPath.TrimAll()
    End Function

    Private Sub MigrateFiles(treeItemId As Integer, treePath As String, rootPath As String)
        If Me.Cancel Then
            Exit Sub
        End If
        If treeItemId.ToString().EndsWith("0") Then
            Application.DoEvents()
        End If
        Dim item As ArcExplorerFileInfo
        'System.IO.Directory.CreateDirectory(rootPath)
        'If Not Delimon.Win32.IO.Directory.Exists(rootPath) Then
        'Try
        LongPathHandler.DirectoryCreate(rootPath)
        'Catch ex As Exception

        'End Try
        'End If

        Dim files As List(Of ArcExplorerFileInfo) = GetFiles(treePath & "\", True, True)
        StatusLabelInfo.Text += " " & files.Count & " Datei(en)"
        Me.Refresh()

        For Each item In files
            If Me.Cancel Then
                Exit Sub
            End If
            If CountFiles.ToString().EndsWith("00") Then
                dbc?.Close()
                dbc = Nothing
                CheckDbConnection()
            End If
            Dim fname = treePath & "\" & item.Physicalname
            fname = fname.Substring(0, fname.LastIndexOf(".")) & ".ESW"
            If IO.File.Exists(fname) Then
                CountFiles = CountFiles + 1
                SumBytes = SumBytes + item.FileLen
                Dim displayname = IniFileHelper.IniReadValue(fname, "GENERAL", "SHORTDESC") '& IniFileHelper.IniReadValue(fname, "GENERAL", "DOCEXT")
                'Dim filename = ConvertToValidName(displayname)
                'If filename = displayname Then
                '    displayname = Nothing
                'End If
                'filename = filename & item.Fileextension
                'FileIO.FileSystem.CopyFile(treePath & "\" & fileItem.Physicalname, rootPath & "\" & filename, FileIO.UIOption.OnlyErrorDialogs, FileIO.UICancelOption.ThrowException)
                LongPathHandler.FileCopy(treePath & "\" & item.Physicalname, rootPath & "\" & item.Physicalname, True)
                Dim newFile = dbc.AddNewListItem(treeItemId, item.Physicalname, displayname)
                MigrateFileIndexes(fname, item.Physicalname, newFile.Id, item.Version)
            End If
        Next
    End Sub

    Private Sub MigrateFileIndexes(fname As String, filename As String, fileId As Integer, version As String)
        Dim sections = IniFileHelper.IniReadSections(fname)
        dbc.AddNewListItemIndex(fileId, "ELO Dateiname", filename, True)
        dbc.AddNewListItemIndex(fileId, "ELO Version", version, True)
        'AddGeneralKeyToDatabase(fileId, fname, "SHORTDESC")
        'AddGeneralKeyToDatabase(fileId, fname, "DOCEXT")
        AddGeneralKeyToDatabase(fileId, fname, "DOCTYPE")
        AddGeneralKeyToDatabase(fileId, fname, "DOCDATE")
        AddGeneralKeyToDatabase(fileId, fname, "ABLDATE")
        'AddGeneralKeyToDatabase(fileId, fname, "Version")
        AddGeneralKeyToDatabase(fileId, fname, "USER")
        AddGeneralKeyToDatabase(fileId, fname, "ACL")
        'AddGeneralKeyToDatabase(fileId, fname, "SREG")
        AddGeneralKeyToDatabase(fileId, fname, "GUID")
        For Each section In sections
            If section.StartsWith("KEY") Then
                Dim keyName = IniFileHelper.IniReadValue(fname, section, "KEYNAME")
                If String.IsNullOrEmpty(keyName) Then
                    keyName = IniFileHelper.IniReadValue(fname, section, "KEYKEY")
                Else
                    keyName = "ELO " & keyName
                End If
                dbc.AddNewListItemIndex(fileId, keyName, IniFileHelper.IniReadValue(fname, section, "KEYTEXT").Replace(Chr(182), ""), True)
            End If
        Next
        dbc.SaveChanges()
    End Sub

    Private Sub FrmMain_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Not CancelForm Is Nothing Then
            If Me.WindowState = FormWindowState.Minimized Then
                CancelForm.Visible = False
            Else
                CancelForm.Top = Me.Top + Me.Height - (CancelForm.Height + 8)
                CancelForm.Left = Me.Left + Me.Width - (CancelForm.Width + 25)
                CancelForm.Visible = True
            End If
        End If
    End Sub

    Private Sub TvwExplorer_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles TvwExplorer.AfterCheck
        'e.Node.ExpandAll()
        SetNodeChildrenChecked(e.Node, e.Node.Checked)
    End Sub

    Private Sub SetNodeChildrenChecked(node As TreeNode, checked As Boolean)
        Dim child As TreeNode
        For Each child In node.Nodes
            child.Checked = checked
            SetNodeChildrenChecked(child, checked)
        Next
    End Sub

    Private Sub TvwExplorer_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TvwExplorer.AfterSelect
        TreeNodeClick(e.Node)
    End Sub

    Private Sub MenuCombine_Click(sender As Object, e As EventArgs) Handles MenuCombine.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        'MenuFindDoubles_Click(sender, e)
        'Dim parentNode = TvwExplorer.SelectedNode.Parent
        Dim child As TreeNode
        Dim checkedNodes As List(Of TreeNode) = New List(Of TreeNode)
        For Each child In TvwExplorer.SelectedNode.Nodes
            If child.Checked Then
                checkedNodes.Add(child)
            End If
        Next
        If checkedNodes.Count = 2 Then
            If MsgBox("Knoten " & checkedNodes.ElementAt(0).Text & " und " & checkedNodes.ElementAt(1).Text & " zusammenführen?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.Yes Then
                MergeNodes(checkedNodes.ElementAt(0), checkedNodes.ElementAt(1))
                CheckEmptyFolders(GetFullPathOfNode(checkedNodes.ElementAt(1)))
                TvwExplorer.Nodes.Remove(checkedNodes.ElementAt(1))
                LoadTree()
            End If
        End If
    End Sub

    Private Sub MergeNodes(baseNode As TreeNode, secondNode As TreeNode)
        Dim child As TreeNode
        Dim basePath As String = GetFullPathOfNode(baseNode)
        secondNode.Expand()
        For Each child In secondNode.Nodes
            Dim oldPath As String = GetFullPathOfNode(child)
            Dim di = New DirectoryInfo(oldPath)
            If Not IsInNodes(baseNode, child.Text) Then
                Directory.Move(oldPath, basePath & "\" & di.Name)
                Dim fname = oldPath & ".ESW"
                IO.File.Move(fname, basePath & "\" & di.Name + ".ESW")
                'baseNode.Nodes.Insert(1, child)
            Else
                MergeNodes(GetChildNodeByText(baseNode, child.Text), child)
            End If
            CheckEmptyFolders(oldPath)
        Next
        MergeFiles(baseNode, secondNode)
    End Sub

    Private Sub CheckEmptyFolders(oldPath As String)
        If Not Directory.Exists(oldPath) Then
            Return
        End If
        Dim checkFiles = Directory.GetFiles(oldPath)
        If Directory.GetDirectories(oldPath).Length = 0 Then
            Dim fname = oldPath & ".ESW"
            If checkFiles.Length = 0 Then
                Directory.Delete(oldPath)
                IO.File.Delete(fname)
            ElseIf checkFiles.Length = 1 Then
                If checkFiles.ElementAt(0).ToLower().EndsWith(".esw") Then
                    Directory.Delete(oldPath, True)
                    IO.File.Delete(fname)
                End If
            End If
        End If
    End Sub

    Private Sub MergeFiles(baseNode As TreeNode, secondNode As TreeNode)
        Dim basePath As String = GetFullPathOfNode(baseNode)
        Dim oldPath As String = GetFullPathOfNode(secondNode)
        Dim file As String
        For Each file In Directory.GetFiles(oldPath)
            If Not file.ToLower().EndsWith(".esw") Then
                Dim fi = New FileInfo(file)
                IO.File.Move(file, basePath & "\" & fi.Name)
                Dim fname = file.Substring(0, file.LastIndexOf("."))
                Dim eswName = fname & ".ESW"
                fi = New FileInfo(eswName)
                If IO.File.Exists(eswName) Then
                    IO.File.Move(eswName, basePath & "\" & fi.Name)
                End If
            End If
        Next
    End Sub

    Private Function IsInNodes(node As TreeNode, text As String) As Boolean
        Dim child As TreeNode
        node.Expand()
        For Each child In node.Nodes
            If child.Text = text Then
                Return True
            End If
        Next
    End Function

    Private Function GetChildNodeByText(node As TreeNode, text As String) As TreeNode
        Dim child As TreeNode
        node.Expand()
        For Each child In node.Nodes
            If child.Text = text Then
                Return child
            End If
        Next
    End Function

    Private Sub MenuFindDoubles_Click(sender As Object, e As EventArgs) Handles MenuFindDoubles.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        'Dim parentNode = TvwExplorer.SelectedNode.Parent
        Dim child As TreeNode, personalNr As Int32
        For Each child In TvwExplorer.SelectedNode.Nodes
            personalNr = GetPersonalNrFromNode(child)
            If personalNr > 0 Then
                If FindDouble(child, personalNr) Then
                    MenuCombine_Click(sender, e)
                    Exit For
                End If
            End If
        Next
    End Sub

    Private Function FindDouble(node As TreeNode, baseNr As Int32) As Boolean
        Dim personalNr As Int32, child As TreeNode
        For Each child In TvwExplorer.SelectedNode.Nodes
            If child.Name <> node.Name Then
                personalNr = GetPersonalNrFromNode(child)
                If personalNr = baseNr Then
                    child.Checked = True
                    node.Checked = True
                    'child.ExpandAll()
                    'node.ExpandAll()
                    Return True
                End If
            End If
        Next
    End Function

    Private Function FindDoubleName(node As TreeNode, baseName As String) As Boolean
        Dim personalName As String, child As TreeNode
        For Each child In TvwExplorer.SelectedNode.Nodes
            If child.Name <> node.Name Then
                personalName = GetNameFromNode(child)
                If personalName = baseName Then
                    child.Checked = True
                    node.Checked = True
                    'child.ExpandAll()
                    'node.ExpandAll()
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Private Function GetPersonalNrFromNode(node As TreeNode) As Int32
        Dim personalNr As Int32, i As Integer
        If IsNumeric(node.Text.Left(1)) Then
            i = 2
            Do While IsNumeric(node.Text.Left(i)) And i < node.Text.Length
                personalNr = node.Text.Left(i)
                i = i + 1
            Loop
        ElseIf IsNumeric(node.Text.Right(1)) Then
            i = 2
            Do While IsNumeric(node.Text.Right(i)) And i < node.Text.Length
                personalNr = node.Text.Right(i)
                i = i + 1
            Loop
        End If
        Return personalNr
    End Function

    Private Function GetNameFromNode(node As TreeNode) As String
        Dim personalName As String = ""
        If Not node.Text.Contains(" ") Then
            Return ""
        ElseIf IsNumeric(node.Text.Left(1)) Then
            personalName = node.Text.Substring(node.Text.IndexOf(" ")).Trim()
        ElseIf IsNumeric(node.Text.Right(1)) Then
            personalName = node.Text.Substring(0, node.Text.LastIndexOf(" ")).Trim()
        End If
        Return personalName
    End Function

    Private Sub MenuCombineAll_Click(sender As Object, e As EventArgs) Handles MenuCombineAll.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim foundDouble As Boolean
        Dim child As TreeNode, personalNr As Int32
        foundDouble = True
        Do While foundDouble
            For Each child In TvwExplorer.SelectedNode.Nodes
                personalNr = GetPersonalNrFromNode(child)
                If personalNr > 0 Then
                    If FindDouble(child, personalNr) Then
                        foundDouble = True
                        Exit For
                    End If
                End If
            Next
            'Dim parentNode = TvwExplorer.SelectedNode.Parent
            Dim checkedNodes As List(Of TreeNode) = New List(Of TreeNode)
            For Each child In TvwExplorer.SelectedNode.Nodes
                If child.Checked Then
                    checkedNodes.Add(child)
                End If
            Next
            If checkedNodes.Count = 2 Then
                'If MsgBox("Knoten " & checkedNodes.ElementAt(0).Text & " und " & checkedNodes.ElementAt(1).Text & " zusammenführen?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Nachfrage") = MsgBoxResult.Yes Then
                MergeNodes(checkedNodes.ElementAt(0), checkedNodes.ElementAt(1))
                CheckEmptyFolders(GetFullPathOfNode(checkedNodes.ElementAt(1)))
                TvwExplorer.Nodes.Remove(checkedNodes.ElementAt(1))
                LoadTree()
                'End If
            Else
                Exit Do
            End If
        Loop
    End Sub

    'alle Knoten unterhalb des selektierten auf gleiche Eben wie selektierten bringen und danach leeren Knoten entfernen
    Private Sub MenuMoveAllHigher_Click(sender As Object, e As EventArgs) Handles MenuMoveAllHigher.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim basePath = GetFullPathOfNode(TvwExplorer.SelectedNode.Parent)
        TvwExplorer.SelectedNode.Expand()
        For Each child In TvwExplorer.SelectedNode.Nodes
            Dim oldPath As String = GetFullPathOfNode(child)
            Dim di = New DirectoryInfo(oldPath)
            Directory.Move(oldPath, basePath & "\" & di.Name)
            Dim fname = oldPath & ".ESW"
            IO.File.Move(fname, basePath & "\" & di.Name + ".ESW")
        Next
        basePath = GetFullPathOfNode(TvwExplorer.SelectedNode)
        Directory.Delete(basePath)
        IO.File.Delete(basePath & ".ESW")
        LoadTree()
    End Sub

    Private Sub MenuFindDoubleNames_Click(sender As Object, e As EventArgs) Handles MenuFindDoubleNames.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        'Dim parentNode = TvwExplorer.SelectedNode.Parent
        Dim child As TreeNode, personalName As String
        For Each child In TvwExplorer.SelectedNode.Nodes
            personalName = GetNameFromNode(child)
            If personalName <> "" Then
                If FindDoubleName(child, personalName) Then
                    MenuCombine_Click(sender, e)
                    Exit For
                End If
            End If
        Next
    End Sub

    Private Sub MenuMoveHigher_Click(sender As Object, e As EventArgs) Handles MenuMoveHigher.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim basePath = GetFullPathOfNode(TvwExplorer.SelectedNode.Parent.Parent)
        'TvwExplorer.SelectedNode.Expand()
        Dim child = TvwExplorer.SelectedNode
        Dim oldPath As String = GetFullPathOfNode(child)
        Dim di = New DirectoryInfo(oldPath)
        Directory.Move(oldPath, basePath & "\" & di.Name)
        Dim fname = oldPath & ".ESW"
        IO.File.Move(fname, basePath & "\" & di.Name + ".ESW")
        basePath = GetFullPathOfNode(TvwExplorer.SelectedNode)
        'Directory.Delete(basePath)
        'IO.File.Delete(basePath & ".ESW")
        LoadTree()
    End Sub

    Private Sub MenuExpandFirst_Click(sender As Object, e As EventArgs) Handles MenuExpandFirst.Click
        If TvwExplorer.SelectedNode Is Nothing Then
            Exit Sub
        End If
        Dim child As TreeNode
        Dim checkedNodes As List(Of TreeNode) = New List(Of TreeNode)
        For Each child In TvwExplorer.SelectedNode.Nodes
            child.Expand()
        Next
    End Sub

    Private Sub MenuShowFilePreviewer_Click(sender As Object, e As EventArgs) Handles MenuShowFilePreviewer.Click
        MenuShowFilePreviewer.Checked = Not MenuShowFilePreviewer.Checked
        SaveSetting(Application.ProductName, "Window", "ShowFilePreviewer", MenuShowFilePreviewer.Checked.ToString())
        If MenuShowFilePreviewer.Checked Then
            ShowFilePreviewer()
            LvwFiles_SelectedIndexChanged(sender, e)
        Else
            HideFilePreviewer()
        End If
    End Sub

    Private Sub ContextMenuExplorer_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuExplorer.Opening
        'MenuNewFolder.Enabled = Not TvwExplorer.SelectedNode Is Nothing
        MenuCut.Enabled = Not TvwExplorer.SelectedNode Is Nothing
    End Sub

    Private Function AddNewListItem(treeItemId As Integer, fileName As String, displayname As String) As OrgManListItem
        Dim newFile As OrgManListItem = dbc.AddNewListItem(treeItemId, fileName, displayname)
        AddNewListItem = newFile
    End Function

End Class
