Imports System.IO

Module FileIcons
    Private exeCounter As Double
    Private lnkCounter As Double
    Private icoCounter As Double

    Public Function AddIconToImageList(
                                      ByVal sPath As String,
      ByVal sFile As String,
      ByRef ilsThis As ImageList,
      ByVal sDefault As String, ByRef sFileTypeName As String
   ) As String
        Dim sExt As String = ""
        Dim sTempFile As String
        Dim i As Long
        Dim iFile As Long
        Dim iTag As Object

        For i = Len(sFile) To 1 Step -1
            If (Mid$(sFile, i, 1) = ".") Then
                sExt = Mid$(sFile, i)
                Exit For
            End If
        Next i
        sExt = UCase$(sExt)
        If (sExt <> "") And (sExt <> "EXE") Then
            On Error Resume Next
            iTag = ilsThis.Images(sExt).Tag
            If (Err.Number = 0) Then
                AddIconToImageList = sExt
                sFileTypeName = ilsThis.Images(sExt).Tag
            Else
                On Error GoTo ErrorHandler
                If sExt = ".EXE" Then
                    exeCounter = exeCounter + 1
                    ilsThis.Images.Add(sExt & exeCounter, GetIcon(sPath & sFile))
                    AddIconToImageList = sExt & exeCounter
                    sFileTypeName = ilsThis.Images(sExt & exeCounter).Tag
                ElseIf sExt = ".LNK" Then
                    sTempFile = TempDir() & "\" & sFile
                    File.Copy(sPath & sFile, sTempFile, True)
                    lnkCounter = lnkCounter + 1
                    ilsThis.Images.Add(sExt & lnkCounter, GetIcon(sTempFile))
                    KillFileIfExists(sTempFile)
                    AddIconToImageList = sExt & lnkCounter
                    sFileTypeName = ilsThis.Images(sExt & lnkCounter).Tag
                ElseIf sExt = ".ICO" Then
                    icoCounter = icoCounter + 1
                    ilsThis.Images.Add(sExt & icoCounter, GetIcon(sPath & sFile))
                    AddIconToImageList = sExt & icoCounter
                    sFileTypeName = ilsThis.Images(sExt & icoCounter).Tag
                Else
                    sTempFile = TempDir()
                    If (Right$(sTempFile, 1) <> "\") Then sTempFile = sTempFile & "\"
                    sTempFile = sTempFile & "VBUZTEMP" & sExt
                    KillFileIfExists(sTempFile)
                    iFile = FreeFile()
                    FileOpen(iFile, sTempFile, OpenMode.Binary, OpenAccess.Write)
                    FilePutObject(iFile, "TEMP")
                    FileClose(iFile)
                    Dim img As Image = GetIcon(sTempFile)
                    ilsThis.Images.Add(sExt, img)
                    KillFileIfExists(sTempFile)
                    AddIconToImageList = sExt
                    sFileTypeName = ilsThis.Images(sExt).Tag
                End If
            End If
            If sFileTypeName = "" Then
                sFileTypeName = GetFileTypeName(sFile)
                If sFileTypeName = "" Then
                    sFileTypeName = Mid(sExt, 2) & "-Datei"
                End If
            End If
        Else
            AddIconToImageList = sDefault
        End If
        Exit Function
ErrorHandler:
        KillFileIfExists(sTempFile)
        AddIconToImageList = sDefault
        Exit Function
    End Function
    Public Sub KillFileIfExists(sFile As String)
        On Error Resume Next
        Kill(sFile)
    End Sub
    Public Sub RemoveDirIfExists(sDir As String)
        On Error Resume Next
        RmDir(sDir)
    End Sub
    Public Function TempDir() As String
        'Dim sRet As String, c As Long
        'sRet = New String(Chr(0), MAX_PATH)
        'c = GetTempPath(MAX_PATH, sRet)
        'If c = 0 Then Err.Raise(Err.LastDllError)
        TempDir = My.Computer.FileSystem.SpecialDirectories.Temp & "\OrgMan"
        System.IO.Directory.CreateDirectory(TempDir)
    End Function
    Private Function GetFileTypeName(
        ByVal sFile As String
    ) As String
        GetFileTypeName = GetFileType(IO.Path.GetExtension(sFile))
    End Function
    Private Function GetFileType(ByVal extension As String) As String
        If String.IsNullOrEmpty(extension) Then
            Return "Datei"
        End If
        Dim extensionValue = My.Computer.Registry.GetValue("HKEY_CLASSES_ROOT\" & extension, "", extension)
        If extensionValue Is Nothing Then
            Return extension.Substring(1) & "-Datei"
        End If
        Return My.Computer.Registry.GetValue("HKEY_CLASSES_ROOT\" & extensionValue.ToString, "", extension).ToString
    End Function

    Public Function GetIcon(
        ByVal sFile As String
    ) As Image
        GetIcon = Icon.ExtractAssociatedIcon(sFile).ToBitmap()
        GetIcon.Tag = GetFileTypeName(sFile)
    End Function
    Private Function IconToPicture(ByVal hIcon As Long) As Image

        If hIcon = 0 Then
            IconToPicture = Nothing
            Exit Function
        End If

        'Create a New icon from the handle. 
        Dim newIcon As Icon = Icon.FromHandle(hIcon)

        IconToPicture = newIcon.ToBitmap()

    End Function

    Public Sub ListFoldersFiles(ByVal path As String, ByVal lvTemp As ListView, ByVal imgLtemp As ImageList)
        ' Create a reference to the current directory.
        Dim di As New DirectoryInfo(path)

        If Directory.Exists(di.ToString) Then
            ' Create an array representing the files in the current directory.
            Dim fi As FileInfo() = di.GetFiles()
            Dim fiTemp As FileInfo
            'Array.Sort(fi, New compclass(SortOrder.Ascending))

            lvTemp.Items.Clear()
            ' Loop through each file in the directory
            For Each fiTemp In fi
                Dim strImageKey As String = String.Empty
                Try
                    ' gets the icon from file
                    Dim ico As Icon = Icon.ExtractAssociatedIcon(path & fiTemp.Name)
                    If ico IsNot Nothing Then
                        Dim bmp As Bitmap = ico.ToBitmap()
                        strImageKey = bmp.GetHashCode.ToString
                        imgLtemp.Images.Add(strImageKey, bmp)
                        'Form1.ImageList3.Images.Add(strImageKey, bmp)
                    End If
                Catch ex As Exception

                End Try

                ' split the extension off so we can use just the text.

                Dim rFileName As Array = Split(fiTemp.Name, ".")
                Dim fname As String = rFileName(0)

                ' add full name to tag so we can call it later seeing how we 
                ' don't use the extension for the text.
                Dim item As New ListViewItem(fname, imgLtemp.Images.Count - 1) With {.Tag = path & fiTemp.Name}
                lvTemp.Items.Add(item)

                'If Form1.tvFolders.SelectedNode.Name <> "Jackson County" Then
                '    item.SubItems.Add(Form1.tvFolders.SelectedNode.Parent.Name)
                '    item.SubItems.Add(fiTemp.Name)
                'End If

            Next fiTemp

        End If
    End Sub

    Public Function GetFiles(folder As String) As List(Of OrgManFileInfo)
        Dim foundFiles As New List(Of OrgManFileInfo)
        Dim foundFile As String
        foundFile = Dir(folder, vbNormal + vbHidden + vbSystem) ' Ersten Eintrag abrufen.
        Do While foundFile <> ""    ' Schleife beginnen.
            Dim file As OrgManFileInfo = GetFileInfo(folder, foundFile)
            foundFiles.Add(file)
            foundFile = Dir()   ' Nächsten Eintrag abrufen.
            Application.DoEvents()
        Loop
        GetFiles = foundFiles
    End Function

    Public Function GetFileInfo(folder As String, filename As String) As OrgManFileInfo
        Dim file As New OrgManFileInfo With {
            .FilePath = folder,
            .Filename = filename,
            .FileLen = FileLen(folder & filename),
            .FileDateTime = FileDateTime(folder & filename),
            .FileType = GetFileTypeName(filename)
        }
        GetFileInfo = file
    End Function

End Module
