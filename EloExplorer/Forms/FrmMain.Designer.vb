<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.ContextMenuExplorer = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuNewFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuNewDepartment = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuIntern = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuInternCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuInternCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuInternPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuFolderSort = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuComboFolderSortBy = New System.Windows.Forms.ToolStripComboBox()
        Me.MenuComboFolderSortWay = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuMoveUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMoveDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuFilesSort = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuComboFileSortBy = New System.Windows.Forms.ToolStripComboBox()
        Me.MenuComboFileSortWay = New System.Windows.Forms.ToolStripComboBox()
        Me.MenuSecurity = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuUserSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOpenInExplorer = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWinProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageListExplorer = New System.Windows.Forms.ImageList(Me.components)
        Me.MainSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.TvwExplorer = New System.Windows.Forms.TreeView()
        Me.FilesSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.LvwFiles = New System.Windows.Forms.ListView()
        Me.ColumnHeaderName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderReminder = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuFiles = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuOpenFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCutFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCopyFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPasteFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPasteFileSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPasteScan = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDeleteFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRenameFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuReminderFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDoneReminderFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDeleteReminderFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuSortFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSortMoveUpFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSortMoveDownFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuShowFilePreviewer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuRefreshFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPropertiesFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageListFiles = New System.Windows.Forms.ImageList(Me.components)
        Me.MainStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.StatusLabelInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FilesRefreshTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TimerLoadSecurity = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuExplorer.SuspendLayout()
        CType(Me.MainSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainSplitContainer.Panel1.SuspendLayout()
        Me.MainSplitContainer.Panel2.SuspendLayout()
        Me.MainSplitContainer.SuspendLayout()
        CType(Me.FilesSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FilesSplitContainer.Panel1.SuspendLayout()
        Me.FilesSplitContainer.SuspendLayout()
        Me.ContextMenuFiles.SuspendLayout()
        Me.MainStatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuExplorer
        '
        Me.ContextMenuExplorer.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContextMenuExplorer.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuNewFolder, Me.MenuNewDepartment, Me.ToolStripSeparator1, Me.MenuCut, Me.MenuCopy, Me.MenuPaste, Me.MenuIntern, Me.MenuRename, Me.MenuDelete, Me.ToolStripSeparator2, Me.MenuFolderSort, Me.MenuFilesSort, Me.MenuSecurity, Me.MenuUserSettings, Me.ToolStripSeparator6, Me.MenuRefresh, Me.MenuOpenInExplorer, Me.MenuWinProperties, Me.MenuProperties})
        Me.ContextMenuExplorer.Name = "mnuContextExplorer"
        Me.ContextMenuExplorer.Size = New System.Drawing.Size(206, 374)
        '
        'MenuNewFolder
        '
        Me.MenuNewFolder.Name = "MenuNewFolder"
        Me.MenuNewFolder.Size = New System.Drawing.Size(205, 22)
        Me.MenuNewFolder.Text = "Neuer Ordner..."
        '
        'MenuNewDepartment
        '
        Me.MenuNewDepartment.Name = "MenuNewDepartment"
        Me.MenuNewDepartment.Size = New System.Drawing.Size(205, 22)
        Me.MenuNewDepartment.Text = "Neue Abteilung..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(202, 6)
        '
        'MenuCut
        '
        Me.MenuCut.Name = "MenuCut"
        Me.MenuCut.Size = New System.Drawing.Size(205, 22)
        Me.MenuCut.Text = "Ausschneiden"
        '
        'MenuCopy
        '
        Me.MenuCopy.Name = "MenuCopy"
        Me.MenuCopy.Size = New System.Drawing.Size(205, 22)
        Me.MenuCopy.Text = "Kopieren"
        '
        'MenuPaste
        '
        Me.MenuPaste.Name = "MenuPaste"
        Me.MenuPaste.Size = New System.Drawing.Size(205, 22)
        Me.MenuPaste.Text = "Einfügen"
        '
        'MenuIntern
        '
        Me.MenuIntern.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuInternCut, Me.MenuInternCopy, Me.MenuInternPaste})
        Me.MenuIntern.Name = "MenuIntern"
        Me.MenuIntern.Size = New System.Drawing.Size(205, 22)
        Me.MenuIntern.Text = "Intern"
        '
        'MenuInternCut
        '
        Me.MenuInternCut.Name = "MenuInternCut"
        Me.MenuInternCut.Size = New System.Drawing.Size(150, 22)
        Me.MenuInternCut.Text = "Ausschneiden"
        '
        'MenuInternCopy
        '
        Me.MenuInternCopy.Name = "MenuInternCopy"
        Me.MenuInternCopy.Size = New System.Drawing.Size(150, 22)
        Me.MenuInternCopy.Text = "Kopieren"
        '
        'MenuInternPaste
        '
        Me.MenuInternPaste.Enabled = False
        Me.MenuInternPaste.Name = "MenuInternPaste"
        Me.MenuInternPaste.Size = New System.Drawing.Size(150, 22)
        Me.MenuInternPaste.Text = "Einfügen"
        '
        'MenuRename
        '
        Me.MenuRename.Name = "MenuRename"
        Me.MenuRename.Size = New System.Drawing.Size(205, 22)
        Me.MenuRename.Text = "Umbenennen"
        '
        'MenuDelete
        '
        Me.MenuDelete.Name = "MenuDelete"
        Me.MenuDelete.Size = New System.Drawing.Size(205, 22)
        Me.MenuDelete.Text = "Löschen"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(202, 6)
        '
        'MenuFolderSort
        '
        Me.MenuFolderSort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuComboFolderSortBy, Me.MenuComboFolderSortWay, Me.ToolStripSeparator3, Me.MenuMoveUp, Me.MenuMoveDown})
        Me.MenuFolderSort.Name = "MenuFolderSort"
        Me.MenuFolderSort.Size = New System.Drawing.Size(205, 22)
        Me.MenuFolderSort.Text = "Sortierung (Ordner)"
        '
        'MenuComboFolderSortBy
        '
        Me.MenuComboFolderSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MenuComboFolderSortBy.DropDownWidth = 150
        Me.MenuComboFolderSortBy.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuComboFolderSortBy.Items.AddRange(New Object() {"Benutzerdefiniert", "Name", "Datum/Uhrzeit", "Größe"})
        Me.MenuComboFolderSortBy.Name = "MenuComboFolderSortBy"
        Me.MenuComboFolderSortBy.Size = New System.Drawing.Size(150, 23)
        '
        'MenuComboFolderSortWay
        '
        Me.MenuComboFolderSortWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MenuComboFolderSortWay.DropDownWidth = 150
        Me.MenuComboFolderSortWay.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuComboFolderSortWay.Items.AddRange(New Object() {"Aufsteigend", "Absteigend"})
        Me.MenuComboFolderSortWay.Name = "MenuComboFolderSortWay"
        Me.MenuComboFolderSortWay.Size = New System.Drawing.Size(150, 23)
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(207, 6)
        '
        'MenuMoveUp
        '
        Me.MenuMoveUp.Name = "MenuMoveUp"
        Me.MenuMoveUp.Size = New System.Drawing.Size(210, 22)
        Me.MenuMoveUp.Text = "Nach oben"
        '
        'MenuMoveDown
        '
        Me.MenuMoveDown.Name = "MenuMoveDown"
        Me.MenuMoveDown.Size = New System.Drawing.Size(210, 22)
        Me.MenuMoveDown.Text = "Nach unten"
        '
        'MenuFilesSort
        '
        Me.MenuFilesSort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuComboFileSortBy, Me.MenuComboFileSortWay})
        Me.MenuFilesSort.Name = "MenuFilesSort"
        Me.MenuFilesSort.Size = New System.Drawing.Size(205, 22)
        Me.MenuFilesSort.Text = "Sortierung (Dateien)"
        '
        'MenuComboFileSortBy
        '
        Me.MenuComboFileSortBy.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuComboFileSortBy.Items.AddRange(New Object() {"Name", "Datum", "Typ", "Größe", "Benutzerdefiniert"})
        Me.MenuComboFileSortBy.Name = "MenuComboFileSortBy"
        Me.MenuComboFileSortBy.Size = New System.Drawing.Size(150, 23)
        '
        'MenuComboFileSortWay
        '
        Me.MenuComboFileSortWay.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuComboFileSortWay.Items.AddRange(New Object() {"Aufsteigend", "Absteigend"})
        Me.MenuComboFileSortWay.Name = "MenuComboFileSortWay"
        Me.MenuComboFileSortWay.Size = New System.Drawing.Size(150, 23)
        '
        'MenuSecurity
        '
        Me.MenuSecurity.Name = "MenuSecurity"
        Me.MenuSecurity.Size = New System.Drawing.Size(205, 22)
        Me.MenuSecurity.Text = "Sicherheit"
        '
        'MenuUserSettings
        '
        Me.MenuUserSettings.Name = "MenuUserSettings"
        Me.MenuUserSettings.Size = New System.Drawing.Size(205, 22)
        Me.MenuUserSettings.Text = "Einstellungen"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(202, 6)
        '
        'MenuRefresh
        '
        Me.MenuRefresh.Name = "MenuRefresh"
        Me.MenuRefresh.Size = New System.Drawing.Size(205, 22)
        Me.MenuRefresh.Text = "Aktualisieren"
        '
        'MenuOpenInExplorer
        '
        Me.MenuOpenInExplorer.Name = "MenuOpenInExplorer"
        Me.MenuOpenInExplorer.Size = New System.Drawing.Size(205, 22)
        Me.MenuOpenInExplorer.Text = "Öffnen in Explorer"
        '
        'MenuWinProperties
        '
        Me.MenuWinProperties.Name = "MenuWinProperties"
        Me.MenuWinProperties.Size = New System.Drawing.Size(205, 22)
        Me.MenuWinProperties.Text = "Windows Eigenschaften"
        '
        'MenuProperties
        '
        Me.MenuProperties.Name = "MenuProperties"
        Me.MenuProperties.Size = New System.Drawing.Size(205, 22)
        Me.MenuProperties.Text = "Eigenschaften"
        '
        'ImageListExplorer
        '
        Me.ImageListExplorer.ImageStream = CType(resources.GetObject("ImageListExplorer.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListExplorer.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListExplorer.Images.SetKeyName(0, "DocumentRepository.ico")
        Me.ImageListExplorer.Images.SetKeyName(1, "Folder.ico")
        Me.ImageListExplorer.Images.SetKeyName(2, "Chrome.ico")
        Me.ImageListExplorer.Images.SetKeyName(3, "Browser.ico")
        Me.ImageListExplorer.Images.SetKeyName(4, "Reminder.ico")
        '
        'MainSplitContainer
        '
        Me.MainSplitContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainSplitContainer.Location = New System.Drawing.Point(0, 1)
        Me.MainSplitContainer.Name = "MainSplitContainer"
        '
        'MainSplitContainer.Panel1
        '
        Me.MainSplitContainer.Panel1.Controls.Add(Me.TvwExplorer)
        '
        'MainSplitContainer.Panel2
        '
        Me.MainSplitContainer.Panel2.Controls.Add(Me.FilesSplitContainer)
        Me.MainSplitContainer.Size = New System.Drawing.Size(1149, 818)
        Me.MainSplitContainer.SplitterDistance = 383
        Me.MainSplitContainer.TabIndex = 1
        '
        'TvwExplorer
        '
        Me.TvwExplorer.AllowDrop = True
        Me.TvwExplorer.ContextMenuStrip = Me.ContextMenuExplorer
        Me.TvwExplorer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TvwExplorer.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TvwExplorer.HideSelection = False
        Me.TvwExplorer.ImageIndex = 0
        Me.TvwExplorer.ImageList = Me.ImageListExplorer
        Me.TvwExplorer.Location = New System.Drawing.Point(0, 0)
        Me.TvwExplorer.Name = "TvwExplorer"
        Me.TvwExplorer.SelectedImageIndex = 0
        Me.TvwExplorer.ShowNodeToolTips = True
        Me.TvwExplorer.Size = New System.Drawing.Size(383, 818)
        Me.TvwExplorer.StateImageList = Me.ImageListExplorer
        Me.TvwExplorer.TabIndex = 1
        '
        'FilesSplitContainer
        '
        Me.FilesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FilesSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.FilesSplitContainer.Name = "FilesSplitContainer"
        '
        'FilesSplitContainer.Panel1
        '
        Me.FilesSplitContainer.Panel1.Controls.Add(Me.LvwFiles)
        Me.FilesSplitContainer.Panel1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'FilesSplitContainer.Panel2
        '
        Me.FilesSplitContainer.Panel2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FilesSplitContainer.Size = New System.Drawing.Size(762, 818)
        Me.FilesSplitContainer.SplitterDistance = 498
        Me.FilesSplitContainer.TabIndex = 3
        '
        'LvwFiles
        '
        Me.LvwFiles.AllowDrop = True
        Me.LvwFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeaderName, Me.ColumnHeaderDate, Me.ColumnHeaderType, Me.ColumnHeaderSize, Me.ColumnHeaderReminder})
        Me.LvwFiles.ContextMenuStrip = Me.ContextMenuFiles
        Me.LvwFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LvwFiles.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LvwFiles.FullRowSelect = True
        Me.LvwFiles.HideSelection = False
        Me.LvwFiles.LargeImageList = Me.ImageListFiles
        Me.LvwFiles.Location = New System.Drawing.Point(0, 0)
        Me.LvwFiles.Name = "LvwFiles"
        Me.LvwFiles.ShowItemToolTips = True
        Me.LvwFiles.Size = New System.Drawing.Size(498, 818)
        Me.LvwFiles.SmallImageList = Me.ImageListFiles
        Me.LvwFiles.TabIndex = 0
        Me.LvwFiles.UseCompatibleStateImageBehavior = False
        Me.LvwFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeaderName
        '
        Me.ColumnHeaderName.Text = "Name"
        Me.ColumnHeaderName.Width = 350
        '
        'ColumnHeaderDate
        '
        Me.ColumnHeaderDate.Text = "Datum"
        Me.ColumnHeaderDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeaderDate.Width = 120
        '
        'ColumnHeaderType
        '
        Me.ColumnHeaderType.Text = "Typ"
        Me.ColumnHeaderType.Width = 100
        '
        'ColumnHeaderSize
        '
        Me.ColumnHeaderSize.Text = "Größe"
        Me.ColumnHeaderSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeaderSize.Width = 100
        '
        'ColumnHeaderReminder
        '
        Me.ColumnHeaderReminder.Text = "Wiedervorlage"
        Me.ColumnHeaderReminder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeaderReminder.Width = 120
        '
        'ContextMenuFiles
        '
        Me.ContextMenuFiles.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContextMenuFiles.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuOpenFile, Me.MenuCutFile, Me.MenuCopyFile, Me.MenuPasteFile, Me.MenuPasteFileSelect, Me.MenuPasteScan, Me.MenuDeleteFile, Me.MenuRenameFile, Me.ToolStripSeparator4, Me.MenuReminderFile, Me.MenuDoneReminderFile, Me.MenuDeleteReminderFile, Me.ToolStripSeparator5, Me.MenuSortFile, Me.MenuShowFilePreviewer, Me.ToolStripSeparator7, Me.MenuRefreshFiles, Me.MenuPropertiesFile})
        Me.ContextMenuFiles.Name = "ContextMenuFiles"
        Me.ContextMenuFiles.Size = New System.Drawing.Size(202, 352)
        '
        'MenuOpenFile
        '
        Me.MenuOpenFile.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuOpenFile.Name = "MenuOpenFile"
        Me.MenuOpenFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuOpenFile.Text = "Öffnen"
        '
        'MenuCutFile
        '
        Me.MenuCutFile.Name = "MenuCutFile"
        Me.MenuCutFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuCutFile.Text = "Ausschneiden"
        '
        'MenuCopyFile
        '
        Me.MenuCopyFile.Name = "MenuCopyFile"
        Me.MenuCopyFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuCopyFile.Text = "Kopieren"
        '
        'MenuPasteFile
        '
        Me.MenuPasteFile.Name = "MenuPasteFile"
        Me.MenuPasteFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuPasteFile.Text = "Einfügen"
        '
        'MenuPasteFileSelect
        '
        Me.MenuPasteFileSelect.Name = "MenuPasteFileSelect"
        Me.MenuPasteFileSelect.Size = New System.Drawing.Size(201, 22)
        Me.MenuPasteFileSelect.Text = "Einfügen von..."
        '
        'MenuPasteScan
        '
        Me.MenuPasteScan.Name = "MenuPasteScan"
        Me.MenuPasteScan.Size = New System.Drawing.Size(201, 22)
        Me.MenuPasteScan.Text = "Scan-Import..."
        '
        'MenuDeleteFile
        '
        Me.MenuDeleteFile.Name = "MenuDeleteFile"
        Me.MenuDeleteFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuDeleteFile.Text = "Löschen"
        '
        'MenuRenameFile
        '
        Me.MenuRenameFile.Name = "MenuRenameFile"
        Me.MenuRenameFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuRenameFile.Text = "Umbenennen"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(198, 6)
        '
        'MenuReminderFile
        '
        Me.MenuReminderFile.Name = "MenuReminderFile"
        Me.MenuReminderFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuReminderFile.Text = "Auf Wiedervorlage..."
        '
        'MenuDoneReminderFile
        '
        Me.MenuDoneReminderFile.Name = "MenuDoneReminderFile"
        Me.MenuDoneReminderFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuDoneReminderFile.Text = "Als Erledigt speichern"
        '
        'MenuDeleteReminderFile
        '
        Me.MenuDeleteReminderFile.Name = "MenuDeleteReminderFile"
        Me.MenuDeleteReminderFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuDeleteReminderFile.Text = "Wiedervorlage löschen"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(198, 6)
        '
        'MenuSortFile
        '
        Me.MenuSortFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuSortMoveUpFile, Me.MenuSortMoveDownFile})
        Me.MenuSortFile.Name = "MenuSortFile"
        Me.MenuSortFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuSortFile.Text = "Sortierung"
        '
        'MenuSortMoveUpFile
        '
        Me.MenuSortMoveUpFile.Name = "MenuSortMoveUpFile"
        Me.MenuSortMoveUpFile.Size = New System.Drawing.Size(136, 22)
        Me.MenuSortMoveUpFile.Text = "Nach oben"
        '
        'MenuSortMoveDownFile
        '
        Me.MenuSortMoveDownFile.Name = "MenuSortMoveDownFile"
        Me.MenuSortMoveDownFile.Size = New System.Drawing.Size(136, 22)
        Me.MenuSortMoveDownFile.Text = "Nach unten"
        '
        'MenuShowFilePreviewer
        '
        Me.MenuShowFilePreviewer.Name = "MenuShowFilePreviewer"
        Me.MenuShowFilePreviewer.Size = New System.Drawing.Size(201, 22)
        Me.MenuShowFilePreviewer.Text = "Vorschaufenster"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(198, 6)
        '
        'MenuRefreshFiles
        '
        Me.MenuRefreshFiles.Name = "MenuRefreshFiles"
        Me.MenuRefreshFiles.Size = New System.Drawing.Size(201, 22)
        Me.MenuRefreshFiles.Text = "Aktualisieren"
        '
        'MenuPropertiesFile
        '
        Me.MenuPropertiesFile.Name = "MenuPropertiesFile"
        Me.MenuPropertiesFile.Size = New System.Drawing.Size(201, 22)
        Me.MenuPropertiesFile.Text = "Eigenschaften"
        '
        'ImageListFiles
        '
        Me.ImageListFiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageListFiles.ImageSize = New System.Drawing.Size(32, 32)
        Me.ImageListFiles.TransparentColor = System.Drawing.Color.Transparent
        '
        'MainStatusStrip
        '
        Me.MainStatusStrip.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabelInfo})
        Me.MainStatusStrip.Location = New System.Drawing.Point(0, 820)
        Me.MainStatusStrip.Name = "MainStatusStrip"
        Me.MainStatusStrip.ShowItemToolTips = True
        Me.MainStatusStrip.Size = New System.Drawing.Size(1149, 24)
        Me.MainStatusStrip.TabIndex = 2
        Me.MainStatusStrip.Text = "Bereit"
        '
        'StatusLabelInfo
        '
        Me.StatusLabelInfo.Name = "StatusLabelInfo"
        Me.StatusLabelInfo.Size = New System.Drawing.Size(21, 19)
        Me.StatusLabelInfo.Text = "..."
        '
        'FilesRefreshTimer
        '
        Me.FilesRefreshTimer.Interval = 2000
        '
        'TimerLoadSecurity
        '
        Me.TimerLoadSecurity.Enabled = True
        Me.TimerLoadSecurity.Interval = 5000
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1149, 844)
        Me.Controls.Add(Me.MainStatusStrip)
        Me.Controls.Add(Me.MainSplitContainer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmMain"
        Me.Text = "ELO Explorer"
        Me.ContextMenuExplorer.ResumeLayout(False)
        Me.MainSplitContainer.Panel1.ResumeLayout(False)
        Me.MainSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.MainSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainSplitContainer.ResumeLayout(False)
        Me.FilesSplitContainer.Panel1.ResumeLayout(False)
        CType(Me.FilesSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FilesSplitContainer.ResumeLayout(False)
        Me.ContextMenuFiles.ResumeLayout(False)
        Me.MainStatusStrip.ResumeLayout(False)
        Me.MainStatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ContextMenuExplorer As ContextMenuStrip
    Friend WithEvents ImageListExplorer As ImageList
    Friend WithEvents MenuNewFolder As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents MenuCopy As ToolStripMenuItem
    Friend WithEvents MenuCut As ToolStripMenuItem
    Friend WithEvents MenuPaste As ToolStripMenuItem
    Friend WithEvents MenuRename As ToolStripMenuItem
    Friend WithEvents MenuDelete As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents MenuProperties As ToolStripMenuItem
    Friend WithEvents MenuNewDepartment As ToolStripMenuItem
    Friend WithEvents MenuRefresh As ToolStripMenuItem
    Friend WithEvents MainSplitContainer As SplitContainer
    Friend WithEvents TvwExplorer As TreeView
    Friend WithEvents MainStatusStrip As StatusStrip
    Friend WithEvents LvwFiles As ListView
    Friend WithEvents ColumnHeaderName As ColumnHeader
    Friend WithEvents ColumnHeaderDate As ColumnHeader
    Friend WithEvents ColumnHeaderType As ColumnHeader
    Friend WithEvents ColumnHeaderSize As ColumnHeader
    Friend WithEvents MenuFolderSort As ToolStripMenuItem
    Friend WithEvents MenuComboFolderSortBy As ToolStripComboBox
    Friend WithEvents MenuComboFolderSortWay As ToolStripComboBox
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents MenuMoveUp As ToolStripMenuItem
    Friend WithEvents MenuMoveDown As ToolStripMenuItem
    Public WithEvents ImageListFiles As ImageList
    Friend WithEvents ContextMenuFiles As ContextMenuStrip
    Friend WithEvents MenuOpenFile As ToolStripMenuItem
    Friend WithEvents MenuCutFile As ToolStripMenuItem
    Friend WithEvents MenuCopyFile As ToolStripMenuItem
    Friend WithEvents MenuPasteFile As ToolStripMenuItem
    Friend WithEvents MenuDeleteFile As ToolStripMenuItem
    Friend WithEvents MenuRenameFile As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents MenuPropertiesFile As ToolStripMenuItem
    Friend WithEvents MenuRefreshFiles As ToolStripMenuItem
    Friend WithEvents FilesRefreshTimer As Timer
    Friend WithEvents MenuOpenInExplorer As ToolStripMenuItem
    Friend WithEvents MenuIntern As ToolStripMenuItem
    Friend WithEvents MenuInternCopy As ToolStripMenuItem
    Friend WithEvents MenuInternCut As ToolStripMenuItem
    Friend WithEvents MenuInternPaste As ToolStripMenuItem
    Friend WithEvents MenuWinProperties As ToolStripMenuItem
    Friend WithEvents MenuFilesSort As ToolStripMenuItem
    Friend WithEvents MenuComboFileSortBy As ToolStripComboBox
    Friend WithEvents MenuComboFileSortWay As ToolStripComboBox
    Friend WithEvents MenuSecurity As ToolStripMenuItem
    Friend WithEvents MenuUserSettings As ToolStripMenuItem
    Friend WithEvents MenuPasteScan As ToolStripMenuItem
    Friend WithEvents MenuPasteFileSelect As ToolStripMenuItem
    Friend WithEvents StatusLabelInfo As ToolStripStatusLabel
    Friend WithEvents MenuReminderFile As ToolStripMenuItem
    Friend WithEvents MenuDoneReminderFile As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ColumnHeaderReminder As ColumnHeader
    Friend WithEvents MenuDeleteReminderFile As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents MenuSortFile As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents MenuSortMoveUpFile As ToolStripMenuItem
    Friend WithEvents MenuSortMoveDownFile As ToolStripMenuItem
    Friend WithEvents TimerLoadSecurity As Timer
    Friend WithEvents FilesSplitContainer As SplitContainer
    Friend WithEvents MenuShowFilePreviewer As ToolStripMenuItem
    Friend WithEvents FilePreviewHandlerHost As Win.Common.Tools.PreviewHandlerHost
End Class
