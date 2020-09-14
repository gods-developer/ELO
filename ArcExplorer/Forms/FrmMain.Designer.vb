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
        Me.MenuCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuFindDoubles = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuFindDoubleNames = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCombineAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCombine = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCombineSameFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMoveHigher = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMoveAllHigher = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuMigrate = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuExpandAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExpandFirst = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOpenInExplorer = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuEloIndex = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuWinProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageListExplorer = New System.Windows.Forms.ImageList(Me.components)
        Me.MainSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.TvwExplorer = New System.Windows.Forms.TreeView()
        Me.FilesSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.LvwFiles = New System.Windows.Forms.ListView()
        Me.ColumnHeaderName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderVersion = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuFiles = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuOpenFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCutFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCopyFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDeleteFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRenameFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuShowFilePreviewer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuRefreshFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuVersions = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuEloData = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuFileEloIndex = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPropertiesFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageListFiles = New System.Windows.Forms.ImageList(Me.components)
        Me.FilePreviewHandlerHost = New DigiSped.Common.Tools.PreviewHandlerHost()
        Me.MainStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.StatusLabelInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FilesRefreshTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MenuCountFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCombineSameFolderAl = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuExplorer.SuspendLayout()
        CType(Me.MainSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainSplitContainer.Panel1.SuspendLayout()
        Me.MainSplitContainer.Panel2.SuspendLayout()
        Me.MainSplitContainer.SuspendLayout()
        CType(Me.FilesSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FilesSplitContainer.Panel1.SuspendLayout()
        Me.FilesSplitContainer.Panel2.SuspendLayout()
        Me.FilesSplitContainer.SuspendLayout()
        Me.ContextMenuFiles.SuspendLayout()
        Me.MainStatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuExplorer
        '
        Me.ContextMenuExplorer.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContextMenuExplorer.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCut, Me.MenuCopy, Me.MenuPaste, Me.MenuRename, Me.MenuDelete, Me.ToolStripSeparator6, Me.MenuFindDoubles, Me.MenuFindDoubleNames, Me.MenuCombineAll, Me.MenuCombine, Me.MenuCombineSameFolderAl, Me.MenuCombineSameFolder, Me.MenuMoveHigher, Me.MenuMoveAllHigher, Me.MenuCountFiles, Me.ToolStripSeparator2, Me.MenuMigrate, Me.ToolStripSeparator1, Me.MenuExpandAll, Me.MenuExpandFirst, Me.MenuRefresh, Me.MenuOpenInExplorer, Me.MenuEloIndex, Me.MenuWinProperties})
        Me.ContextMenuExplorer.Name = "mnuContextExplorer"
        Me.ContextMenuExplorer.Size = New System.Drawing.Size(254, 506)
        '
        'MenuCut
        '
        Me.MenuCut.Name = "MenuCut"
        Me.MenuCut.Size = New System.Drawing.Size(253, 22)
        Me.MenuCut.Text = "Ausschneiden"
        '
        'MenuCopy
        '
        Me.MenuCopy.Name = "MenuCopy"
        Me.MenuCopy.Size = New System.Drawing.Size(253, 22)
        Me.MenuCopy.Text = "Kopieren"
        '
        'MenuPaste
        '
        Me.MenuPaste.Name = "MenuPaste"
        Me.MenuPaste.Size = New System.Drawing.Size(253, 22)
        Me.MenuPaste.Text = "Einfügen"
        '
        'MenuRename
        '
        Me.MenuRename.Name = "MenuRename"
        Me.MenuRename.Size = New System.Drawing.Size(253, 22)
        Me.MenuRename.Text = "Umbenennen"
        '
        'MenuDelete
        '
        Me.MenuDelete.Name = "MenuDelete"
        Me.MenuDelete.Size = New System.Drawing.Size(253, 22)
        Me.MenuDelete.Text = "Löschen"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(250, 6)
        '
        'MenuFindDoubles
        '
        Me.MenuFindDoubles.Name = "MenuFindDoubles"
        Me.MenuFindDoubles.Size = New System.Drawing.Size(253, 22)
        Me.MenuFindDoubles.Text = "ID Dopplungen finden"
        '
        'MenuFindDoubleNames
        '
        Me.MenuFindDoubleNames.Name = "MenuFindDoubleNames"
        Me.MenuFindDoubleNames.Size = New System.Drawing.Size(253, 22)
        Me.MenuFindDoubleNames.Text = "Name Dopplungen finden"
        '
        'MenuCombineAll
        '
        Me.MenuCombineAll.Name = "MenuCombineAll"
        Me.MenuCombineAll.Size = New System.Drawing.Size(253, 22)
        Me.MenuCombineAll.Text = "Alle zusammenführen"
        '
        'MenuCombine
        '
        Me.MenuCombine.Name = "MenuCombine"
        Me.MenuCombine.Size = New System.Drawing.Size(253, 22)
        Me.MenuCombine.Text = "Zusammenführen"
        '
        'MenuCombineSameFolder
        '
        Me.MenuCombineSameFolder.Name = "MenuCombineSameFolder"
        Me.MenuCombineSameFolder.Size = New System.Drawing.Size(253, 22)
        Me.MenuCombineSameFolder.Text = "Gleiche Namen zusammenführen"
        '
        'MenuMoveHigher
        '
        Me.MenuMoveHigher.Name = "MenuMoveHigher"
        Me.MenuMoveHigher.Size = New System.Drawing.Size(253, 22)
        Me.MenuMoveHigher.Text = "1 Ebene höher"
        '
        'MenuMoveAllHigher
        '
        Me.MenuMoveAllHigher.Name = "MenuMoveAllHigher"
        Me.MenuMoveAllHigher.Size = New System.Drawing.Size(253, 22)
        Me.MenuMoveAllHigher.Text = "Alle 1 Ebene höher"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(250, 6)
        '
        'MenuMigrate
        '
        Me.MenuMigrate.Name = "MenuMigrate"
        Me.MenuMigrate.Size = New System.Drawing.Size(253, 22)
        Me.MenuMigrate.Text = "Zu OrgMan migrieren"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(250, 6)
        '
        'MenuExpandAll
        '
        Me.MenuExpandAll.Name = "MenuExpandAll"
        Me.MenuExpandAll.Size = New System.Drawing.Size(253, 22)
        Me.MenuExpandAll.Text = "Alles öffnen"
        '
        'MenuExpandFirst
        '
        Me.MenuExpandFirst.Name = "MenuExpandFirst"
        Me.MenuExpandFirst.Size = New System.Drawing.Size(253, 22)
        Me.MenuExpandFirst.Text = "Alle 1x öffnen"
        '
        'MenuRefresh
        '
        Me.MenuRefresh.Name = "MenuRefresh"
        Me.MenuRefresh.Size = New System.Drawing.Size(253, 22)
        Me.MenuRefresh.Text = "Aktualisieren"
        '
        'MenuOpenInExplorer
        '
        Me.MenuOpenInExplorer.Name = "MenuOpenInExplorer"
        Me.MenuOpenInExplorer.Size = New System.Drawing.Size(253, 22)
        Me.MenuOpenInExplorer.Text = "Öffnen in Explorer"
        '
        'MenuEloIndex
        '
        Me.MenuEloIndex.Name = "MenuEloIndex"
        Me.MenuEloIndex.Size = New System.Drawing.Size(253, 22)
        Me.MenuEloIndex.Text = "ELO Index"
        '
        'MenuWinProperties
        '
        Me.MenuWinProperties.Name = "MenuWinProperties"
        Me.MenuWinProperties.Size = New System.Drawing.Size(253, 22)
        Me.MenuWinProperties.Text = "Eigenschaften"
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
        Me.TvwExplorer.CheckBoxes = True
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
        Me.FilesSplitContainer.Panel2.Controls.Add(Me.FilePreviewHandlerHost)
        Me.FilesSplitContainer.Panel2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FilesSplitContainer.Size = New System.Drawing.Size(762, 818)
        Me.FilesSplitContainer.SplitterDistance = 498
        Me.FilesSplitContainer.TabIndex = 3
        '
        'LvwFiles
        '
        Me.LvwFiles.AllowDrop = True
        Me.LvwFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeaderName, Me.ColumnHeaderDate, Me.ColumnHeaderType, Me.ColumnHeaderSize, Me.ColumnHeaderVersion})
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
        'ColumnHeaderVersion
        '
        Me.ColumnHeaderVersion.Text = "Version"
        Me.ColumnHeaderVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeaderVersion.Width = 35
        '
        'ContextMenuFiles
        '
        Me.ContextMenuFiles.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContextMenuFiles.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuOpenFile, Me.MenuCutFile, Me.MenuCopyFile, Me.MenuDeleteFile, Me.MenuRenameFile, Me.ToolStripSeparator4, Me.MenuShowFilePreviewer, Me.ToolStripSeparator7, Me.MenuRefreshFiles, Me.MenuVersions, Me.MenuEloData, Me.MenuFileEloIndex, Me.MenuPropertiesFile})
        Me.ContextMenuFiles.Name = "ContextMenuFiles"
        Me.ContextMenuFiles.Size = New System.Drawing.Size(164, 258)
        '
        'MenuOpenFile
        '
        Me.MenuOpenFile.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuOpenFile.Name = "MenuOpenFile"
        Me.MenuOpenFile.Size = New System.Drawing.Size(163, 22)
        Me.MenuOpenFile.Text = "Öffnen"
        '
        'MenuCutFile
        '
        Me.MenuCutFile.Name = "MenuCutFile"
        Me.MenuCutFile.Size = New System.Drawing.Size(163, 22)
        Me.MenuCutFile.Text = "Ausschneiden"
        '
        'MenuCopyFile
        '
        Me.MenuCopyFile.Name = "MenuCopyFile"
        Me.MenuCopyFile.Size = New System.Drawing.Size(163, 22)
        Me.MenuCopyFile.Text = "Kopieren"
        '
        'MenuDeleteFile
        '
        Me.MenuDeleteFile.Name = "MenuDeleteFile"
        Me.MenuDeleteFile.Size = New System.Drawing.Size(163, 22)
        Me.MenuDeleteFile.Text = "Löschen"
        '
        'MenuRenameFile
        '
        Me.MenuRenameFile.Name = "MenuRenameFile"
        Me.MenuRenameFile.Size = New System.Drawing.Size(163, 22)
        Me.MenuRenameFile.Text = "Umbenennen"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(160, 6)
        '
        'MenuShowFilePreviewer
        '
        Me.MenuShowFilePreviewer.Name = "MenuShowFilePreviewer"
        Me.MenuShowFilePreviewer.Size = New System.Drawing.Size(163, 22)
        Me.MenuShowFilePreviewer.Text = "Vorschaufenster"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(160, 6)
        '
        'MenuRefreshFiles
        '
        Me.MenuRefreshFiles.Name = "MenuRefreshFiles"
        Me.MenuRefreshFiles.Size = New System.Drawing.Size(163, 22)
        Me.MenuRefreshFiles.Text = "Aktualisieren"
        '
        'MenuVersions
        '
        Me.MenuVersions.Name = "MenuVersions"
        Me.MenuVersions.Size = New System.Drawing.Size(163, 22)
        Me.MenuVersions.Text = "Versionen"
        '
        'MenuEloData
        '
        Me.MenuEloData.Name = "MenuEloData"
        Me.MenuEloData.Size = New System.Drawing.Size(163, 22)
        Me.MenuEloData.Text = "ELO Daten"
        '
        'MenuFileEloIndex
        '
        Me.MenuFileEloIndex.Name = "MenuFileEloIndex"
        Me.MenuFileEloIndex.Size = New System.Drawing.Size(163, 22)
        Me.MenuFileEloIndex.Text = "ELO Index"
        '
        'MenuPropertiesFile
        '
        Me.MenuPropertiesFile.Name = "MenuPropertiesFile"
        Me.MenuPropertiesFile.Size = New System.Drawing.Size(163, 22)
        Me.MenuPropertiesFile.Text = "Eigenschaften"
        '
        'ImageListFiles
        '
        Me.ImageListFiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageListFiles.ImageSize = New System.Drawing.Size(32, 32)
        Me.ImageListFiles.TransparentColor = System.Drawing.Color.Transparent
        '
        'FilePreviewHandlerHost
        '
        Me.FilePreviewHandlerHost.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FilePreviewHandlerHost.Location = New System.Drawing.Point(0, 0)
        Me.FilePreviewHandlerHost.Name = "FilePreviewHandlerHost"
        Me.FilePreviewHandlerHost.Size = New System.Drawing.Size(260, 818)
        Me.FilePreviewHandlerHost.TabIndex = 0
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
        'MenuCountFiles
        '
        Me.MenuCountFiles.Name = "MenuCountFiles"
        Me.MenuCountFiles.Size = New System.Drawing.Size(253, 22)
        Me.MenuCountFiles.Text = "Dateien zählen"
        '
        'MenuCombineSameFolderAl
        '
        Me.MenuCombineSameFolderAl.Name = "MenuCombineSameFolderAl"
        Me.MenuCombineSameFolderAl.Size = New System.Drawing.Size(253, 22)
        Me.MenuCombineSameFolderAl.Text = "Alle 1. Ebene gl. Namen zus."
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
        Me.Text = "Archiv Explorer"
        Me.ContextMenuExplorer.ResumeLayout(False)
        Me.MainSplitContainer.Panel1.ResumeLayout(False)
        Me.MainSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.MainSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainSplitContainer.ResumeLayout(False)
        Me.FilesSplitContainer.Panel1.ResumeLayout(False)
        Me.FilesSplitContainer.Panel2.ResumeLayout(False)
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
    Friend WithEvents MenuCopy As ToolStripMenuItem
    Friend WithEvents MenuCut As ToolStripMenuItem
    Friend WithEvents MenuPaste As ToolStripMenuItem
    Friend WithEvents MenuRename As ToolStripMenuItem
    Friend WithEvents MenuDelete As ToolStripMenuItem
    Friend WithEvents MenuRefresh As ToolStripMenuItem
    Friend WithEvents MainSplitContainer As SplitContainer
    Friend WithEvents TvwExplorer As TreeView
    Friend WithEvents MainStatusStrip As StatusStrip
    Friend WithEvents LvwFiles As ListView
    Friend WithEvents ColumnHeaderName As ColumnHeader
    Friend WithEvents ColumnHeaderDate As ColumnHeader
    Friend WithEvents ColumnHeaderType As ColumnHeader
    Friend WithEvents ColumnHeaderSize As ColumnHeader
    Public WithEvents ImageListFiles As ImageList
    Friend WithEvents ContextMenuFiles As ContextMenuStrip
    Friend WithEvents MenuOpenFile As ToolStripMenuItem
    Friend WithEvents MenuCutFile As ToolStripMenuItem
    Friend WithEvents MenuCopyFile As ToolStripMenuItem
    Friend WithEvents MenuDeleteFile As ToolStripMenuItem
    Friend WithEvents MenuRenameFile As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents MenuPropertiesFile As ToolStripMenuItem
    Friend WithEvents MenuRefreshFiles As ToolStripMenuItem
    Friend WithEvents FilesRefreshTimer As Timer
    Friend WithEvents MenuOpenInExplorer As ToolStripMenuItem
    Friend WithEvents MenuWinProperties As ToolStripMenuItem
    Friend WithEvents StatusLabelInfo As ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents FilesSplitContainer As SplitContainer
    Friend WithEvents MenuShowFilePreviewer As ToolStripMenuItem
    Friend WithEvents FilePreviewHandlerHost As DigiSped.Common.Tools.PreviewHandlerHost
    Friend WithEvents MenuEloData As ToolStripMenuItem
    Friend WithEvents MenuFileEloIndex As ToolStripMenuItem
    Friend WithEvents MenuEloIndex As ToolStripMenuItem
    Friend WithEvents MenuExpandAll As ToolStripMenuItem
    Friend WithEvents MenuMigrate As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents MenuVersions As ToolStripMenuItem
    Friend WithEvents ColumnHeaderVersion As ColumnHeader
    Friend WithEvents MenuCombine As ToolStripMenuItem
    Friend WithEvents MenuFindDoubles As ToolStripMenuItem
    Friend WithEvents MenuCombineAll As ToolStripMenuItem
    Friend WithEvents MenuMoveAllHigher As ToolStripMenuItem
    Friend WithEvents MenuFindDoubleNames As ToolStripMenuItem
    Friend WithEvents MenuMoveHigher As ToolStripMenuItem
    Friend WithEvents MenuExpandFirst As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents MenuCombineSameFolder As ToolStripMenuItem
    Friend WithEvents MenuCountFiles As ToolStripMenuItem
    Friend WithEvents MenuCombineSameFolderAl As ToolStripMenuItem
End Class
