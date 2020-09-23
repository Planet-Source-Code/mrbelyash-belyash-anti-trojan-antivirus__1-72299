Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports datax = System.Diagnostics.FileVersionInfo
Imports System.Security.Permissions
Imports System.DateTime
Imports System.Security.AccessControl
Imports System.Security

Module Module2
 Public MyPath As String
 Public QuarantinePath As String
 Dim line As String
 Public countCons As Integer = 0
 Public globalExclude As String
 Public Virname As String
 Public myRegister As Boolean = False
 Dim MyLibrary As New MyLibrary.MyLib
 Dim keysize As New cript.clsAESV2.KeySize
 Dim objCryptDES As New cript.clsDES
 Dim BelUnpack As New BelUnpack.unpax
 Public SL As New SoftwareLOCK
 Public inCount As Long = 0
 Public inFound As Long = 0
 Public inCure As Long = 0
 Public inNonCure As Long = 0
 Public inDELETE As Long = 0
 Public inMove As Long = 0
 Public nocure As Long = 0
 Public inError As Long = 0
 Public results As String
 Public result As String
 Public a1 As Long
 Public a3 As Long
 Public a2 As Long
 Public f1 As Long
 Public f2 As Long
 Public a4 As Long
 Private cksu As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
 Private lksu As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SoftWare\Microsoft\Windows\CurrentVersion\Run", True)
 Private lksu1 As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", True)
 ' Private lksu2 As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", True)
 Private Hksu As Microsoft.Win32.RegistryKey = My.Computer.Registry.Users.OpenSubKey(".DEFAULT\Software\Microsoft\Windows\CurrentVersion\Run", True)
 Private lksu3 As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnceEx", True)
 ' Private lksu4 As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\RunServices", True)
 Private imgOP As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options", True)
 Private Structure SHFILEINFO
  Public hIcon As IntPtr
  Public iIcon As Integer
  Public dwAttributes As Integer
  <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public szDisplayName As String
  <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)> Public szTypeName As String
 End Structure

 Dim kolwoZap As Long
 Public line2 As String
 Public line3 As String

 Private Declare Auto Function SHGetFileInfo Lib "shell32.dll" (ByVal pszPath As String, _
          ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As IntPtr

 Private Const SHGFI_ICON = &H100
 Private Const SHGFI_SMALLICON = &H1
 Private Const SHGFI_LARGEICON = &H0
 Private Const MAX_PATH = 260
 
 Public Function MD5_Hash(ByVal FileName As String) As String
  On Error GoTo 11
  'Dim f2 As New FileIOPermission(FileIOPermissionAccess.Read, FileName)
  Dim f = New FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, 8192)
  Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
  md5.ComputeHash(f)
  f.Close()
  Dim hash As Byte() = md5.Hash
  Dim buff As StringBuilder = New StringBuilder
  Dim hashByte As Byte
  For Each hashByte In hash
   buff.Append(String.Format("{0:X2}", hashByte))
  Next
  Return buff.ToString()
  f2 = Nothing
  Exit Function
11:
  Return "None"
 End Function
 Function chkLogHS() As Boolean
  'писать в лог хеш файла ?
  chkLogHS = False
  On Error GoTo 101
  Dim f1 As String = sGetINI(sINIFile, "Console", "LogHash", "True")
  If f1 = "True" Then
   chkLogHS = True
   Exit Function
  End If
  Exit Function
101:
  ErrorLog("chkLogHS " & ErrorToString())

 End Function
 Public Function yes_vir_temp(ByVal SearchChar7 As String, ByVal mytmpb As String) As Boolean
  'md5
  yes_vir_temp = False
  If Trim(SearchChar7) = "" Then
   Exit Function
  End If
  keysize = cript.clsAESV2.KeySize.Bits128
  Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
  Dim tmpstr As String = a.Encrypt(SearchChar7)
  a1 = 0
  a1 = (DateTime.Now.Millisecond)
  a3 = 0
  a3 = (DateTime.Now.Second)
  ' Create an instance of StreamReader to read from a file.

  Dim d As String = (MyLibrary.FormFunction.MakeTopMost(mytmpb, tmpstr))

  If Trim(d) <> "0" Then
   sound_me("1")
   If MyLibrary.FormFunction.get_all_pos(d, mytmpb) <> "None" Then
    Dim tmpstr2 As String = MyLibrary.FormFunction.get_all_pos(d, mytmpb)
    If Trim(tmpstr2) <> "" Then
     Virname = tmpstr2.Substring(tmpstr.Length, tmpstr2.Length - tmpstr.Length)
    End If
    yes_vir_temp = True

   End If
  End If
 End Function

 Public Function yes_vir(ByVal SearchChar As String) As Boolean
  yes_vir = False
  With My.Computer.FileSystem
   For Each file1 As String In .GetFiles(MyPath)

    If IO.Path.GetExtension(file1.ToString) = ".bvb" Then
     'System.Diagnostics.Debug.WriteLine(file1.ToString)
     'Stop
     If yes_vir_temp(SearchChar, file1.ToString) = True Then
      yes_vir = True
     End If
    End If

   Next
  End With

 End Function
 Public Sub get_virname(ByVal sd As String, ByVal ps As Integer, ByVal alg As Integer)
  If alg = 2 Then
   Virname = Right(sd, Len(sd) - 9)
  Else
   Virname = Right(sd, Len(sd) - 33)
  End If
 End Sub




 Public Sub sound_me(ByVal d As String)
  'звук
  On Error GoTo 10
  Dim s1 As String = sGetINI(sINIFile, "Console", "Sound", "True")
  If s1 = "False" Then
   Exit Sub
  End If

  Select Case d
   Case "1"
    'vir found
    My.Computer.Audio.Play(MyPath & "\infected_p.wav", AudioPlayMode.Background)
   Case "2"
    'error
    My.Computer.Audio.Play(MyPath & "\m3.wav", AudioPlayMode.Background)
   Case "3"
    'fon
    My.Computer.Audio.Play(MyPath & "\m3.wav", AudioPlayMode.Background)
  End Select
  Exit Sub
10:
  ErrorLog("sound_me " & ErrorToString())
 End Sub



 '===================================
 Public Function yes_vir_UZER(ByVal SearchChar As String) As Boolean
  'проверка по пользовательской базе
  Try
   yes_vir_UZER = False
   If Trim(SearchChar) = "" Then
    Exit Function
   End If
   If File.Exists(MyPath & "\uzerbase.bvb") = False Then
    Exit Function
   End If
   a1 = 0
   a1 = (DateTime.Now.Millisecond)
   a3 = 0
   a3 = (DateTime.Now.Second)
   ' Create an instance of StreamReader to read from a file.
   Using sr As StreamReader = New StreamReader(MyPath & "\uzerbase.bvb")
    Dim line As String
    ' Read and display the lines from the file until the end 
    ' of the file is reached.
    Do
     line = sr.ReadLine()
     Dim TestPos As Integer
     ' A textual comparison starting at position 4. Returns 6.
     TestPos = InStr(1, UCase(line), UCase(SearchChar), CompareMethod.Binary)
     If TestPos <> 0 And SearchChar <> "None" Then
      get_virname(line, TestPos, 1)
      sound_me("1")
      yes_vir_UZER = True
      sr.Close()
      Exit Function
     End If
    Loop Until line Is Nothing
    sr.Close()
   End Using
  Catch E As Exception
   ' Let the user know what went wrong.
   Console.WriteLine("Unknow problem with user base")
   ErrorLog("yes_vir_UZER " & ErrorToString())
  End Try
 End Function


 Public Sub ErrorLog(ByVal smess45 As String)
  'будем записывать ошибки
  On Error GoTo 100
  inError = inError + 1
  Dim path As String = MyPath & "\ERRORLOG.LOG"
  If File.Exists(path) = True Then
   Dim fileDetail As IO.FileInfo
   fileDetail = My.Computer.FileSystem.GetFileInfo(path)
   If CLng(fileDetail.Length) >= 5860966 Then
    File.Delete(path)
   End If
  End If
  Dim sw As StreamWriter = File.AppendText(path)
  sw.WriteLine(Format$(Now, "dd-MM-yyyy HH:mm:ss") & " " & smess45 & "-Console")
  sw.Flush()
  sw.Close()
  Exit Sub
100:

 End Sub
 Public Sub LogQuarant(ByVal smess456 As String)
  'будем записывать ошибки
  On Error GoTo 100
  If Directory.Exists(QuarantinePath) = False Then
   Directory.CreateDirectory(QuarantinePath)
  End If


  Dim path As String = MyPath & "\quarantine\MovedFiles.LOG"
  If File.Exists(path) = True Then
   Dim fileDetail As IO.FileInfo
   fileDetail = My.Computer.FileSystem.GetFileInfo(path)

   If CLng(fileDetail.Length) >= 5860966 Then
    File.Delete(path)
   End If
  End If
  Dim sw As StreamWriter = File.AppendText(path)
  sw.WriteLine(Format$(Now, "dd/mm/yyyy") & "-" & smess456)
  sw.Flush()
  sw.Close()
  Exit Sub
100:
  ErrorLog("LogQuarant " & ErrorToString())
 End Sub
 Function gt_extension_zip(ByVal ag As String) As Boolean
  gt_extension_zip = False
  Select Case LCase(ag)
   Case LCase(".zip")
    gt_extension_zip = True
    Exit Function
   Case LCase(".GZIP")
    gt_extension_zip = True
    Exit Function
   Case LCase(".TAR")
    gt_extension_zip = True
    Exit Function
   Case LCase(".ARJ")
    gt_extension_zip = True
    Exit Function
   Case LCase(".rar")
    gt_extension_zip = True
    Exit Function
   Case LCase(".LZH")
    gt_extension_zip = True
    Exit Function
   Case LCase(".LHA")
    gt_extension_zip = True
    Exit Function
   Case LCase(".cab")
    gt_extension_zip = True
    Exit Function
  End Select
 End Function
 Public Function chk_uzerBase(ByVal vMD5 As String, ByVal vFile As String, ByVal pids As Long, ByVal Component1 As String) As Boolean
  On Error GoTo 100
  If File.Exists(MyPath & "\uzerbase.txt") = False Then
   Exit Function
  End If
  chk_uzerBase = False
  ' If GetInputState() <> 0 Then
  ' End If
  If yes_vir_UZER(vMD5) = True Then
   Process.GetProcessById(pids).Kill()
   chk_uzerBase = True
   If action_virus_CRC(vFile, Virname, Component1) = False Then
    'MoveFileEx(tmp1, "", MOVEFILE_DELAY_UNTIL_REBOOT)
    inNonCure = inNonCure + 1
    Console.WriteLine(vFile & " -Error cure")
    LogPrint(vFile & "(" & Virname & ")-Error cure(user md5)")
    SecondActions(vFile, Virname, Component1)
   End If
  End If
  Exit Function
100:
  ErrorLog("chk_uzerBase " & ErrorToString())

 End Function
 Function Exclude_pats(ByVal tmpPath As String) As Boolean
  'пользовательские исключения
  On Error GoTo 100
  Exclude_pats = False

  If LCase(tmpPath) = LCase(globalExclude) Or MyLibrary.FormFunction.get_shortpt(tmpPath) = MyLibrary.FormFunction.get_shortpt(globalExclude) Then
   Exclude_pats = True
   Exit Function
  End If
  'MyLibrary.FormFunction.get_shortpt
  Dim tmpint As Integer = CInt(sGetINI(sINIFile, "Console", "Count_Console", "0"))
  If tmpint = 0 Then
   Exit Function
  End If
  For i = 0 To tmpint

   Dim tExpand3 As String = sGetINI(sINIFile, "Exclude_Console", i, "0")
   sGetINI(sINIFile, "Exclude_Console", i, "0")
   If tExpand3 <> "0" And tExpand3 <> "" Then
    ' MsgBox("совпало")
    If UCase(Trim(tExpand3)) = UCase(Trim(tmpPath)) Then
     LogPrint(tExpand3 & "-folder exclude")
     Exclude_pats = True
     Exit Function
    End If
   End If
  Next
  'End If

  Exit Function
100:
  ErrorLog("Exclude_pats " & ErrorToString())

 End Function
 '========heur============================================

 Public Sub main_heur(ByVal fileName2 As String, ByVal Component1 As String)
  'главная будет проца по эвристике
  On Error GoTo 100
  Dim r1 As Boolean = CBool(sGetINI(sINIFile, "Console", "Evristic", "True"))
  If r1 = False Then
   Exit Sub
  End If
  'в будущем вставить API функцию для проверки экзешник это или нет
  'GetFileVersionInfoSize GetFileVersionInfo
  If Trim(fileName2) = "" Then
   Exit Sub
  End If
  If Exclude_pats(fileName2) = True Then
   Exit Sub 'исключен из проверки пользователем 
  End If
  'Dim fileDetail As IO.FileInfo
  ' fileDetail = My.Computer.FileSystem.GetFileInfo(fileName2)
  'Dim sizeFile As Long = CLng(fileDetail.Length)

  'If sizeFile = 0 Then
  'Exit Sub
  'End If
  Select Case LCase(IO.Path.GetExtension(fileName2))
   Case ".exe"
    heur_scan(fileName2, Component1)
   Case ".com"
    heur_scan(fileName2, Component1)
   Case ".vxd"
    heur_scan(fileName2, Component1)
   Case ".ocx"
    heur_scan(fileName2, Component1)
   Case ".vbs"
    heur_vbs(fileName2, Component1)
   Case ".js"
    heur_vbs(fileName2, Component1)
   Case ".inf"
    heur_inf(fileName2)
   Case ".lnk"
    check_lnk(fileName2) 'пока проверку линков вставлю в эвристику...потом пеенесу в главную...главное не забыть
   Case ".bat"
    heur_bat(fileName2)
  End Select
  Exit Sub
100:
  ErrorLog("main_heur " & ErrorToString())

 End Sub
 Public Sub check_lnk(ByVal nmFile As String)
  'проверять таргет линков
  Dim ext As String = IO.Path.GetExtension(nmFile)
  Dim p As String = Mid(nmFile, 1, (Len(nmFile) - Len(nmFile & ext) - 1))
  If ext.ToLower = ".lnk" Then
   Dim lnkt As String = getShortcutTarget(p, nmFile & ".lnk")
   Scan(lnkt, "[LNK]", 0)
  End If
  Exit Sub
500:
  ErrorLog("check_lnk " & ErrorToString())
 End Sub
 Private Function getShortcutTarget(ByVal shortcutPath As String, ByVal shortcutName As String) As String
  'получить содержимое ярлыков в каталоге автозагрузки
  Dim theShell As Shell32.Shell
  Dim folderContainingLink As Shell32.Folder
  Dim folderItemLink As Shell32.FolderItem
  Dim link As Shell32.ShellLinkObject

  theShell = New Shell32.Shell()
  folderContainingLink = theShell.NameSpace(shortcutPath)
  folderItemLink = folderContainingLink.ParseName(shortcutName)
  link = folderItemLink.GetLink

  Return link.Path
 End Function
 Function chkExtention_File(ByVal fext As String) As Boolean
  chkExtention_File = False
  On Error GoTo 100
  If Trim(fext) = "" Then
   Exit Function
  End If
  Select Case LCase(IO.Path.GetExtension(fext))
   Case ".exe"
    chkExtention_File = True
    Exit Function
   Case ".com"
    chkExtention_File = True
    Exit Function
   Case ".vbs"
    chkExtention_File = True
    Exit Function
   Case ".js"
    chkExtention_File = True
    Exit Function
   Case ".inf"
    chkExtention_File = True
    Exit Function
   Case ".dll"
    chkExtention_File = True
    Exit Function
   Case ".vxd"
    chkExtention_File = True
    Exit Function
   Case ".sys"
    chkExtention_File = True
    Exit Function
   Case ".cmd"
    chkExtention_File = True
    Exit Function
   Case ".386"
    chkExtention_File = True
    Exit Function
   Case ".bat"
    chkExtention_File = True
    Exit Function
   Case ".bin"
    chkExtention_File = True
    Exit Function
   Case ".chm"
    chkExtention_File = True
    Exit Function
   Case ".html"
    chkExtention_File = True
    Exit Function
   Case ".htm"
    chkExtention_File = True
    Exit Function
   Case ".mht"
    chkExtention_File = True
    Exit Function
   Case ".cpl"
    chkExtention_File = True
    Exit Function
   Case ".drv"
    chkExtention_File = True
    Exit Function
   Case ".pif"
    chkExtention_File = True
    Exit Function
   Case ".hlp"
    chkExtention_File = True
    Exit Function
   Case ".scr"
    chkExtention_File = True
    Exit Function
   Case ".ocx"
    chkExtention_File = True
    Exit Function
   Case ".eml"
    chkExtention_File = True
    Exit Function
   Case ".asp"
    chkExtention_File = True
    Exit Function
  End Select
  Exit Function
100:
  ErrorLog("chkExtention_File " & ErrorToString())
 End Function
 Sub heur_inf(ByVal f1 As String)
  On Error GoTo 10
  Dim r1 As Boolean = CBool(sGetINI(sINIFile, "Console", "Autorun", "True"))
  If r1 = False Then
   Exit Sub
  End If

  If File.Exists(QuarantinePath & "\autorun.inf") = True Then
   File.Delete(QuarantinePath & "\autorun.inf")
  End If

  If LCase(IO.Path.GetFileName(f1)) = LCase("autorun.inf") Then
   System.IO.File.Move(Trim(f1), QuarantinePath & "\" & IO.Path.GetFileName(f1))

   inMove = inMove + 1

   Console.WriteLine(f1 & " infected AutoRun.inf-Move")
   LogQuarant(f1)
   LogPrint(f1 & "-infected AutoRun.inf -Move to quarantine")
  End If
  Exit Sub
10:
  ErrorLog("heur_inf " & ErrorToString())

 End Sub

 Public Sub heur_vbs(ByVal fileName3 As String, ByVal Component1 As String)
  Try
   'line = ""
   result = 0

   check_vbs1(fileName3)
   If result >= 1 Then

    If check_vbs2(fileName3) = True Then
     inFound = inFound + 1

     LogPrint(fileName3 & " -Suspicious Script.Mail")
     Console.WriteLine(fileName3 & "-Modifications Script.Mail")
    Else
     inFound = inFound + 1

     LogPrint(fileName3 & " -Suspicious Script.Virus")
     Console.WriteLine(fileName3 & "-Modifications Script.Virus")
    End If
    If action_virus_CRC(fileName3, "Suspicious Script.Virus", Component1) = False Then
     inNonCure = inNonCure + 1
     Console.WriteLine(fileName3 & " Error cure")
     LogPrint("Error cure " & fileName3 & "-(Suspicious Script.Virus)")
     SecondActions(fileName3, "Modifications Script.Virus", Component1)
    End If

   End If
  Catch ex As IOException

   ErrorLog("heur_vbs " & ErrorToString())
  End Try

 End Sub
 Function heur_bat(ByVal fileName31 As String)
  heur_bat = False
  On Error GoTo 100
  Dim a(9) As String
  a(0) = "erase"
  a(1) = "reg"
  a(2) = "format"
  a(3) = "tskill"
  a(4) = "run"
  a(5) = "hidden"
  a(6) = "HideFileExt"
  a(7) = "startup"
  a(8) = "NoFolderOptions"



  Dim i As Integer
  For i = 0 To 9
   Dim d As String = (MyLibrary.FormFunction.MakeTopMost(fileName31, a(i)))
   If Trim(d) <> "0" Then
    heur_bat = True
    Exit Function
   End If
  Next
  a(9) = Nothing
  Exit Function
100:
  ErrorLog("heur_bat " & ErrorToString())
 End Function
 Public Function check_vbs2(ByVal fileName31 As String) As Boolean
  check_vbs2 = False
  On Error GoTo 100
  Dim a(4) As String
  a(1) = "Outlook"
  a(2) = "Mapi"
  a(3) = "Attachments"
  a(0) = "Subject"
  Dim i As Integer
  For i = 0 To 4
   Dim d As String = (MyLibrary.FormFunction.MakeTopMost(fileName31, a(i)))
   If Trim(d) <> "0" Then
    check_vbs2 = True
    Exit Function
   End If


  Next
  a(4) = Nothing
  Exit Function
100:
  ErrorLog("check_vbs2 " & ErrorToString())
 End Function
 Public Sub check_vbs1(ByVal mytmpb6 As String)
  On Error GoTo 100
  Dim a(4) As String
  a(1) = ".CreateTextFile"
  a(2) = ".write"
  a(3) = ".CopyFile"
  a(0) = "Print"

  Dim i As Integer
  For i = 0 To 4
   Dim d As String = (MyLibrary.FormFunction.MakeTopMost(mytmpb6, a(i)))
   If Trim(d) <> "0" Then
    result = result + 1
   End If
  Next
  a(4) = Nothing
  Exit Sub
100:
  ErrorLog("check_vbs1 " & ErrorToString())
 End Sub
 Public Sub heur_scan(ByVal fileName As String, ByVal Component1 As String)
  'типа эвристика для экзешников
  If exclude_sys_critical(fileName) = True Then
   Exit Sub
  End If

  Try
   result = 0
   Dim a(9) As String
   a(1) = "GetProcAddress"
   a(2) = "CreateFileA"
   a(3) = "FindFirstFileA"
   a(4) = "FindNextFileA"
   a(5) = "SetFilePointer"
   a(6) = "ReadFile"
   a(7) = "WriteFile"
   a(8) = "CloseHandle"
   a(0) = "GetModuleHandleA"

   Dim i As Integer
   For i = 0 To 8
    '    If GetInputState() <> 0 Then

    '   End If
    Dim d As String = (MyLibrary.FormFunction.MakeTopMost(fileName, a(i)))
    If Trim(d) <> "0" Then
     result = result + 1
    End If
    d = "0"
   Next
   a(9) = Nothing
   If result >= 8 Then
    inFound = inFound + 1

    LogPrint(fileName & " -Modification Win32.Virus")
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fileName & " -modification Win32.Virus")
    Console.ForegroundColor = ConsoleColor.White
    If action_virus_CRC(fileName, "Modification Win32.Virus", Component1) = False Then
     inNonCure = inNonCure + 1
     Console.ForegroundColor = ConsoleColor.Red
     Console.WriteLine(fileName & " -error cure")
     Console.ForegroundColor = ConsoleColor.White
     LogPrint("Error cure " & fileName & "-(modification Win32.Virus)")
    End If
   End If
  Catch ex As IOException
   ErrorLog("heur_scan " & ErrorToString())
  End Try

 End Sub
 Function pre_heur(ByVal SearchChar As String) As Boolean
  pre_heur = False

  Dim TestPos As Integer
  ' A textual comparison starting at position 4. Returns 6.
  TestPos = InStr(1, UCase(line), UCase(SearchChar), CompareMethod.Binary)
  If TestPos <> 0 Then
   'result = result + 1
   pre_heur = True
  End If
 End Function
 Private Sub chk_infected_exe()
  'основные фу-ции используемые вирусами,написаными на языках высокого уровня
  Dim a(9) As String
  a(1) = "GetProcAddress"
  a(2) = "CreateFileA"
  a(3) = "FindFirstFileA"
  a(4) = "FindNextFileA"
  a(5) = "SetFilePointer"
  a(6) = "ReadFile"
  a(7) = "WriteFile"
  a(8) = "CloseHandle"
  a(0) = "GetModuleHandleA"

  Dim i As Integer
  For i = 0 To 8
   If pre_heur(a(i)) = True Then
    result = result + 1
   End If
  Next
  a(9) = Nothing
  '==========
  ' FindFirstFileA()
  'FindNextFileA()
  'CreateFileA()
  'SetFilePointer()
  'ReadFile()
  'WriteFile()
  'CloseHandle()
  'EXITPROCESS()
  'GetCurrentDirectoryA()
  'SetCurrentDirectoryA()
  'GetWindowsDirectoryA()
  'GetCommandLineA()
 End Sub
 Function exclude_sys_critical(ByVal k1 As String) As Boolean
  'не проверять мои файлы,у них будет своя самозащита
  exclude_sys_critical = False
  Select Case LCase(k1)
   Case LCase(MyPath & "\monitor.exe")
    exclude_sys_critical = True
    Exit Function
   Case LCase(MyPath & "\ERRORLOG.LOG")
    exclude_sys_critical = True
    Exit Function
   Case LCase(MyPath & "\otchetMon.log")
    exclude_sys_critical = True
    Exit Function
   Case LCase(MyPath & "\SETTINGS.INI")
    exclude_sys_critical = True
    Exit Function
  End Select
  If IO.Path.GetFileName(k1) = "ntldr" Then
   exclude_sys_critical = True
   Exit Function
  End If
 End Function
 Function chk_userbase_string(ByVal kfile As String) As Boolean
  chk_userbase_string = False
  Try
   If File.Exists(MyPath & "\uzerbaseSTR.bvb") = False Then
    Exit Function
   End If
   Dim nb = MyPath & "\uzerbaseSTR.bvb"

   Using sr1 As StreamReader = New StreamReader(nb)
    Dim line71 As String
    Do
     line71 = sr1.ReadLine()
     If Trim(line71) <> "" Then
      If fnk(kfile, line71) = True Then

       chk_userbase_string = True
       Exit Function
      End If
     End If
    Loop Until line71 Is Nothing
    sr1.Close()
    '=======
   End Using
  Catch E As Exception
   ' Let the user know what went wrong.
   'MsgBox("Unknow problem with bases-all bases")
   ErrorLog("chk_userbase_string " & ErrorToString())

  End Try

 End Function
 Function fnk(ByVal zb As String, ByVal nm66 As String) As Boolean
  fnk = False

  Dim d As String = (MyLibrary.FormFunction.MakeTopMost(zb, nm66))

  If Trim(d) <> "0" Then
   fnk = True
   Exit Function
  End If
 End Function


 Public Sub gt_dll()
  On Error GoTo 100
  With My.Computer.FileSystem
   For Each file1 As String In .GetFiles(MyPath)
    '   If GetInputState() <> 0 Then
    '   End If
    If IO.Path.GetExtension(file1.ToString) = ".exe" Or IO.Path.GetExtension(file1.ToString) = ".dll" Then
     'MsgBox(file1.ToString)
     gh1111(file1.ToString())
    End If

   Next

  End With
  Exit Sub
100:
  ErrorLog("gt_dll " & ErrorToString())
 End Sub

 Sub gh1111(ByVal s As String)

  Select Case UCase(IO.Path.GetFileName(s))
   Case UCase("cript.dll")
    Console.WriteLine("cript.dll - v." & System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileVersion.ToString)
    LogPrint("cript.dll - v." & System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileVersion.ToString)
   Case UCase("MyLibraryBase.dll")
    Console.WriteLine("MyLibraryBase.dll - v." & System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileVersion.ToString)
    LogPrint("MyLibraryBase.dll - v." & System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileVersion.ToString)
   Case UCase("unpack.dll")
    Console.WriteLine("unpack.dll - v." & System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileVersion.ToString)
    LogPrint("unpack.dll - v." & System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileVersion.ToString)
  End Select
 End Sub

 Public Sub Scan(ByVal fFile As String, ByVal prichina As String, ByVal slID As Long)
  'тупо-сканирование
  On Error GoTo 101
  Dim myPerms As PermissionSet = New PermissionSet(PermissionState.None)
  myPerms.AddPermission(New FileIOPermission _
   (FileIOPermissionAccess.AllAccess, IO.Path.GetDirectoryName(fFile)))
  myPerms.AddPermission(New FileIOPermission _
     (FileIOPermissionAccess.Write, IO.Path.GetDirectoryName(fFile)))

  Dim my_md5 As String
  If File.Exists(fFile) = False Or Trim(fFile) = "" Then
   Exit Sub
  End If
  If gt_extension_zip(IO.Path.GetExtension(fFile)) = True Then
   Dim tmp_dir As String = IO.Path.GetFileNameWithoutExtension(fFile)
   Dim s As String = My.Application.GetEnvironmentVariable("TEMP") & "\" & tmp_dir
   '  Stop
   globalExclude = s
   unpackFiles(fFile, prichina, "Console")
   GoTo 50
  End If
  Dim extens As Boolean = CBool(sGetINI(sINIFile, "Console", "Extentions", "True"))
  If extens = True Then
   'проверка на расширение
   If chkExtention_File(IO.Path.GetExtension(fFile)) = False Then
    Exit Sub
   End If
  End If
  Dim StartTime As New DateTime()
  Dim EndTime As New DateTime()
  StartTime = DateTime.Now()
  inCount = inCount + 1
  Dim fileDetail As IO.FileInfo
  fileDetail = My.Computer.FileSystem.GetFileInfo(fFile)
  Dim sizeFile As Long = CLng(fileDetail.Length)
  Dim r1 As Boolean = CBool(sGetINI(sINIFile, "Console", "NoCheckLenght", "False"))
  Dim r2 As Long = CLng(sGetINI(sINIFile, "Console", "CheckLenght", "15142784"))
  If r1 = True Then
   If sizeFile > 0 And sizeFile > r2 Then
    LogPrint(fFile & " - exclude (" & sizeFile & ")")
    Exit Sub
   End If
  End If
  a1 = 0
  a1 = (DateTime.Now.Millisecond)
  a3 = 0
  a3 = (DateTime.Now.Second)
  ' my_md5 = MD5_Hash(fFile)
  my_md5 = MyLibrary.FormFunction.gtmd5(Trim(fFile))

  If yes_vir(my_md5) And my_md5 <> "None" Then
   If slID <> 0 Then
    Process.GetProcessById(slID).Kill()
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fFile & " - terminated")
    Console.ForegroundColor = ConsoleColor.White
   End If
   inFound = inFound + 1
   Console.ForegroundColor = ConsoleColor.Red
   LogPrint(fFile & " -infected " & Virname)
   Console.ForegroundColor = ConsoleColor.White
   Console.WriteLine(fFile & " -infected " & Virname)
   If action_virus(fFile, Virname, "Console") = False Then
    inNonCure = inNonCure + 1
    SecondActions(fFile, Virname, "Console")
   End If
   GoTo 100
  Else
   If chk_uzerBase_NoPids(my_md5, fFile, "Console") = True Then
    GoTo 100
   Else
    main_heur(fFile, "Console") 'эвристика
   End If
  End If
  If chk_userbase_string(fFile) = True Then
   LogPrint(fFile & "-string in user base")
   ' NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Инфицирован" & vbCrLf & vbCrLf & IO.Path.GetFileName(fFile) & vbCrLf & "Пользовательская запись", ToolTipIcon.Info)
   If action_virus_CRC(fFile, "User record", "Console") = False Then
    inNonCure = inNonCure + 1
    SecondActions(fFile, Virname, "Console")
   End If
   GoTo 100
  End If
  'my_md5
  LogPrint(fFile & "-OK")
  Console.WriteLine(fFile & "-OK")
100:
  f1 = 0
  f2 = 0
  a2 = 0
  a2 = (DateTime.Now.Millisecond)
  a4 = 0
  a4 = (DateTime.Now.Second)
  If a2 >= a1 Then
   f1 = a2 - a1
  End If
  If a4 >= a1 Then
   f2 = a4 - a3
  End If
  EndTime = DateTime.Now
  Dim answer1, answer2 As Long
  answer1 = 0
  answer1 = EndTime.Ticks() - StartTime.Ticks()
  'If chkLogHS() = True Then
  ' LogPrint ("Console", fFile & "|" & my_md5)
  'End If
  Dim tmpTime As String = sGetINI(sINIFile, "Console", "Time", "True")
  If tmpTime = "True" Then
   If MyLibrary.FormFunction.check_upx_file(fFile) = True Then
    LogPrint(fFile & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & sizeFile & prichina & " |UPX)")
   Else
    LogPrint(fFile & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & sizeFile & prichina & ")")
   End If

  End If
50:

  Exit Sub
101:
  ErrorLog("Scan " & fFile & vbCrLf & ErrorToString())
  Resume Next
 End Sub
 Public Function zapros_na_deystw(ByVal k2 As String) As Boolean
  zapros_na_deystw = False
  Select Case k2
   Case "REPORT"
    zapros_na_deystw = True
    Exit Function
   Case "MOVE"
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine("Do you really want move file ? Y-yes,N-no")
    Dim cki As ConsoleKeyInfo
    Console.TreatControlCAsInput = True
    cki = Console.ReadKey(True)
    If cki.Key.ToString = "Y" Or cki.Key.ToString = "y" Then
     Console.WriteLine(cki.Key.ToString)
     Console.ForegroundColor = ConsoleColor.White
     zapros_na_deystw = True
    End If
    Exit Function
   Case "DELETE"
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine("Do you really want delete file ? Y-yes,N-no")
    Dim cki As ConsoleKeyInfo
    Console.TreatControlCAsInput = True
    cki = Console.ReadKey(True)
    If cki.Key.ToString = "Y" Or cki.Key.ToString = "y" Then
     Console.WriteLine(cki.Key.ToString)
     Console.ForegroundColor = ConsoleColor.White
     zapros_na_deystw = True
    End If
    Exit Function
   Case "LOCK"
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine("Do you really want lock file ? Y-yes,N-no")
    Dim cki As ConsoleKeyInfo
    Console.TreatControlCAsInput = True
    cki = Console.ReadKey(True)
    If cki.Key.ToString = "Y" Or cki.Key.ToString = "y" Then
     Console.WriteLine(cki.Key.ToString)
     Console.ForegroundColor = ConsoleColor.White
     zapros_na_deystw = True
    End If
    Exit Function

  End Select
 End Function
 Function action_virus_zip(ByVal fl_name As String, ByVal st As String, ByVal Component1 As String) As Boolean
  action_virus_zip = False
  On Error GoTo 100
  If Trim(fl_name) = "" Then
   Exit Function
  End If
  If Not File.Exists(fl_name) Then
   Exit Function
  End If
  If myRegister = False Then
   Console.ForegroundColor = ConsoleColor.Red
   Console.WriteLine(fl_name & " Invalid License:REPORT")
   Console.ForegroundColor = ConsoleColor.White
   Exit Function
  End If
  Dim keyName2 As String = sGetINI(sINIFile, "Console", "ActionZip", "REPORT")
  Dim keyName3 As String = sGetINI(sINIFile, "Console", "Ask", "False")
  If keyName3 = "True" Then
   If zapros_na_deystw(keyName2) = False Then
    Exit Function
   End If
  End If
  Select Case keyName2
   Case "REPORT"
    LogPrint(fl_name & "-Arcive infected " & Virname & "(REPORT)")
    inNonCure = inNonCure + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & "-REPORT")
    Console.ForegroundColor = ConsoleColor.White
    action_virus_zip = True
    Exit Function
   Case "MOVE"
    LogPrint(fl_name & "-Arcive infected " & st & "(Move to quarantine)")
    If File.Exists(QuarantinePath & "\" & My.Computer.FileSystem.GetName(fl_name)) = True Then
     File.Delete(QuarantinePath & "\" & My.Computer.FileSystem.GetName(fl_name))
    End If

    My.Computer.FileSystem.MoveFile(fl_name, _
QuarantinePath & "\" & My.Computer.FileSystem.GetName(fl_name), True)
    inMove = inMove + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & "-Move")
    Console.ForegroundColor = ConsoleColor.White
    LogQuarant(fl_name)
    action_virus_zip = True
    Exit Function
   Case "DELETE"
    'удалить
    LogPrint(fl_name & "-Arcive infected " & st & "(DELETE)")
    File.Delete(fl_name)
    inDELETE = inDELETE + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & "-DELETE")
    Console.ForegroundColor = ConsoleColor.White
    action_virus_zip = True
    Exit Function
   Case "LOCK"
    LogPrint(fl_name & "-Arcive infected " & st & "(LOCK)")
    If MyLibrary.FormFunction.lock_file(fl_name) = False Then
     Dim s82 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
     Exit Function
    End If
    Dim s2 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & "-LOCK")
    Console.ForegroundColor = ConsoleColor.White
    action_virus_zip = True
    Exit Function
  End Select
  Exit Function
100:
  ErrorLog("action_virus_zip " & ErrorToString())

 End Function
 Function action_virus_CRC(ByVal fl_name As String, ByVal st As String, ByVal Component1 As String) As Boolean
  action_virus_CRC = False
  On Error GoTo 100
  If Not File.Exists(fl_name) Then
   Exit Function
  End If
  If myRegister = False Then
   Console.ForegroundColor = ConsoleColor.Red
   Console.WriteLine(fl_name & " Invalid License:REPORT")
   Console.ForegroundColor = ConsoleColor.White
   Exit Function
  End If
  Dim keyName2 As String = sGetINI(sINIFile, "Console", "ActionUnknow", "REPORT")
  Dim keyName3 As String = sGetINI(sINIFile, "Console", "Ask", "False")
  If keyName3 = "True" Then
   If zapros_na_deystw(keyName2) = False Then
    Exit Function
   End If
  End If
  Select Case keyName2
   Case "REPORT"
    'только отчет
    LogPrint(fl_name & "-infected " & Virname & "(Report)")
    inNonCure = inNonCure + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & " -REPORT")
    Console.ForegroundColor = ConsoleColor.White
    action_virus_CRC = True
    Exit Function
   Case "MOVE"
    'переместить в карантин
    LogPrint(fl_name & "-infected " & st & "(Move to quarantine)")
    If File.Exists(QuarantinePath & "\" & My.Computer.FileSystem.GetName(fl_name)) = True Then
     File.Delete(QuarantinePath & "\" & My.Computer.FileSystem.GetName(fl_name))
    End If
    My.Computer.FileSystem.MoveFile(fl_name, _
QuarantinePath & "\" & My.Computer.FileSystem.GetName(fl_name), True)
    inMove = inMove + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & " Move")
    Console.ForegroundColor = ConsoleColor.White
    LogQuarant(fl_name)

    action_virus_CRC = True
    Exit Function
   Case "DELETE"
    'удалить
    LogPrint(fl_name & "-infected " & st & "(DELETE)")
    File.Delete(fl_name)
    inDELETE = inDELETE + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & " -DELETE")
    Console.ForegroundColor = ConsoleColor.White
    action_virus_CRC = True
    Exit Function
   Case "LOCK"
    'блокировать
    LogPrint(fl_name & "-infected " & st & "(LOCK)")
    If MyLibrary.FormFunction.lock_file(fl_name) = False Then
     Dim s4 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
     Exit Function
    End If
    Dim s2 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & " -LOCK")
    Console.ForegroundColor = ConsoleColor.White
    action_virus_CRC = True
    Exit Function
  End Select
  Exit Function
100:
  ErrorLog("action_virus_CRC " & ErrorToString())

 End Function
 Public Function action_virus(ByVal fl_name As String, ByVal st As String, ByVal Component1 As String) As Boolean
  action_virus = False
  On Error GoTo 100
  If myRegister = False Then
   Exit Function
  End If
  If Trim(fl_name) = "" Then
   Exit Function
  End If
  If Not File.Exists(fl_name) Then
   Exit Function
  End If
  If myRegister = False Then
   Console.ForegroundColor = ConsoleColor.Red
   Console.WriteLine(fl_name & " Invalid License:REPORT")
   Console.ForegroundColor = ConsoleColor.White
   Exit Function
  End If
  Dim keyName2 As String = sGetINI(sINIFile, "Console", "Action", "REPORT")
  Dim keyName31 As Boolean = CBool(sGetINI(sINIFile, "Console", "Ask", "False"))

  If keyName31 = True Then
   If zapros_na_deystw(keyName2) = False Then
    Exit Function
   End If
  End If

  Select Case keyName2
   Case "REPORT"
    LogPrint(fl_name & "-infected " & Virname & "(REPORT)")
    inNonCure = inNonCure + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & "-REPORT")
    Console.ForegroundColor = ConsoleColor.White
    action_virus = True
    Exit Function
   Case "MOVE"

    LogPrint(fl_name & "-infected " & Virname & "(Move to quarantine)")
    If File.Exists(QuarantinePath & "\" & My.Computer.FileSystem.GetName(fl_name)) = True Then
     File.Delete(QuarantinePath & "\" & My.Computer.FileSystem.GetName(fl_name))
    End If
    Dim tmpExt As String = sGetINI(sINIFile, "Vault", "Extension", "###")
    If File.Exists(QuarantinePath & "\" & IO.Path.GetFileName(fl_name) & "." & Trim(tmpExt)) = True Then
     File.Delete(QuarantinePath & "\" & IO.Path.GetFileName(fl_name) & "." & Trim(tmpExt))
    End If
    Dim tmpcr As Boolean = CBool(sGetINI(sINIFile, "Vault", "Cript", "False"))
    If tmpcr = True Then
     vaultCript(fl_name)
    Else

     Dim tmp_r As String = QuarantinePath & "\" & IO.Path.GetFileName(fl_name) & "." & Trim(tmpExt)
     My.Computer.FileSystem.MoveFile(fl_name, tmp_r, True)
     ' MyLibrary.FormFunction.ren_ME(fl_name, tmp_r)
    End If
    inMove = inMove + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & "-MOVE")
    Console.ForegroundColor = ConsoleColor.White
    LogQuarant(fl_name)
    action_virus = True
    Exit Function

   Case "DELETE"
    'удалить
    LogPrint(fl_name & "-infected " & Virname & "(DELETE)")
    File.Delete(fl_name)
    inDELETE = inDELETE + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & " -DELETE")
    Console.ForegroundColor = ConsoleColor.White
    action_virus = True
    Exit Function
   Case "LOCK"
    'заблокировать файл

    LogPrint(fl_name & "-infected " & Virname & "(LOCK)")
    inNonCure = inNonCure + 1
    If MyLibrary.FormFunction.lock_file(fl_name) = False Then
     Dim s3 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
     Exit Function
    End If
    Dim s2 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(fl_name & "-LOCK")
    Console.ForegroundColor = ConsoleColor.White
    action_virus = True
    Exit Function
  End Select
  Exit Function
100:
  ErrorLog("action_virus " & ErrorToString())
  ' MsgBox(ErrorToString)

 End Function
 Sub vaultCript(ByVal oldpath As String)
  '*** FILE ENCRYPT ***'
  Try
   objCryptDES.Key = "3"
   objCryptDES.FileEncrypt(oldpath, QuarantinePath & "\" & IO.Path.GetFileName(oldpath) & ".CRP", True)
   'File.Delete(oldpath)
  Catch exp As System.IO.IOException
   ErrorLog("get_drive An I/O error occurs.")
  Catch exp As System.Security.SecurityException
   ErrorLog("get_drive The caller does not have the " + _
                              "required permission.")
  End Try

 End Sub
 Public Sub unpackFiles(ByVal zipetfile As String, ByVal pr1 As String, ByVal Component1 As String)
  On Error GoTo 100
  Dim keyName13 As Boolean = CBool(sGetINI(sINIFile, "Console", _
    "CheckZip", "True"))

  If keyName13 <> True Then
   Exit Sub
  End If
  Dim tmp_dir As String = IO.Path.GetFileNameWithoutExtension(zipetfile)
  Dim s As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\" & tmp_dir
  ' My.Application.GetEnvironmentVariable("TEMP") & "\" & tmp_dir

  If BelUnpack.FormUnpack.Show_ZipContents3(zipetfile, s) = True Then
   LogPrint(zipetfile & "-unpack ОК")
   If IO.Directory.Exists(s) = True Then
    If ScanFull(s, pr1, zipetfile) = True Then
     LogPrint(s & "-archive infected")
     Console.ForegroundColor = ConsoleColor.Red
     Console.WriteLine(zipetfile & "-archive infected " & Virname)
     Console.ForegroundColor = ConsoleColor.White
     If action_virus_zip(zipetfile, Virname, Component1) = False Then
      inNonCure = inNonCure + 1
      Console.WriteLine(zipetfile & " -Error cure")
      LogPrint("Error cure " & zipetfile & "-(" & Virname & ")")
      SecondActions(zipetfile, Virname, "Console")
     End If
    End If
    Directory.Delete(s, True)
   End If
  End If

  'del_temp_folder(s)

  Exit Sub
100:
  ErrorLog("unpackFiles " & ErrorToString())

 End Sub
 Public Sub del_temp_folder(ByVal zpath As String)
  'удалить временный каталог со всеми файлами
  On Error GoTo 200
  With My.Computer.FileSystem
   If Directory.Exists(zpath) = True Then
    For Each file1 As String In .GetFiles(zpath)
     If File.Exists(file1) = True Then
      File.Delete(file1)
     End If
    Next file1
   End If
   ' Search child directories.
   For Each folder As Object In .GetDirectories(zpath)
    del_temp_folder(folder)
   Next folder
  End With

  Exit Sub
200:
  ErrorLog("del_temp_folder " & ErrorToString())


 End Sub
 Public Function ScanFull(ByVal dir1 As String, ByVal prichina As String, ByVal zipetfile As String) As Boolean
  ScanFull = False
  On Error GoTo 200
  Dim sod_zip As String
  Dim my_md5 As String
  Dim StartTime As New DateTime()
  Dim EndTime As New DateTime()
  With My.Computer.FileSystem
   For Each file1 As String In .GetFiles(dir1)
    sod_zip = zipetfile & "/" & IO.Path.GetFileName(file1)
    Dim fileDetail As IO.FileInfo
    fileDetail = My.Computer.FileSystem.GetFileInfo(file1)
    Dim tmpLen As Long = CLng(fileDetail.Length)
    Dim r1 As Boolean = CBool(sGetINI(sINIFile, "Console", "NoCheckLenght", "False"))
    Dim r2 As Long = CLng(sGetINI(sINIFile, "Console", "CheckLenght", "15142784"))
    If r1 = True Then
     If CLng(tmpLen) > 0 And CLng(tmpLen) > r2 Then
      LogPrint(sod_zip & " - exclude (" & tmpLen & ")")
      Exit Function
     End If
    End If

    a1 = 0
    a1 = (DateTime.Now.Millisecond)
    a3 = 0
    a3 = (DateTime.Now.Second)
    'my_md5 = MD5_Hash(file1)
    my_md5 = MyLibrary.FormFunction.gtmd5(Trim(file1))

    If yes_vir(my_md5) And my_md5 <> "None" Then

     inFound = inFound + 1
     LogPrint(sod_zip & " -infected " & Virname)
     ' NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Архив инфицирован" & vbCrLf & vbCrLf & IO.Path.GetFileName(file1) & vbCrLf & Virname, ToolTipIcon.Info)
     ScanFull = True
     Exit Function
    Else
     If chk_uzerBase_NoPids(my_md5, file1, "Console") = True Then
      inFound = inFound + 1

      LogPrint(file1 & " -archive infected " & Virname)
      Console.WriteLine(zipetfile & " -archive infecded " & Virname)
      ' NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Архив инфицирован" & vbCrLf & vbCrLf & IO.Path.GetFileName(file1) & vbCrLf & Virname, ToolTipIcon.Info)
      ScanFull = True
      Exit Function
      'Else
      'main_heur(file1) 'эвристика
     End If
    End If

    'my_md5
    LogPrint(sod_zip & "-OK")
    Console.WriteLine(sod_zip & "-OK")
100:
    f1 = 0
    f2 = 0
    a2 = 0
    a2 = (DateTime.Now.Millisecond)
    a4 = 0
    a4 = (DateTime.Now.Second)
    If a2 >= a1 Then
     f1 = a2 - a1
    End If
    If a4 >= a1 Then
     f2 = a4 - a3
    End If
    EndTime = DateTime.Now
    Dim answer1, answer2 As Long
    answer1 = 0
    answer1 = EndTime.Ticks() - StartTime.Ticks()
    'If chkLogHS() = True Then
    ' LogPrint ("Console", fFile & "|" & my_md5)
    'End If
    Dim tmpTime As String = sGetINI(sINIFile, "Console", "Time", "True")
    If tmpTime = "True" Then
     If MyLibrary.FormFunction.check_upx_file(file1) = True Then
      LogPrint(sod_zip & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & tmpLen & prichina & " |UPX)")
     Else
      LogPrint(sod_zip & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & tmpLen & prichina & ")")
     End If
    End If
150:
   Next file1
   ' Search child directories.
   For Each folder As Object In .GetDirectories(dir1)
    Console.WriteLine("[" & folder & "]")
    ScanFull(folder, prichina, zipetfile)
   Next folder
  End With

  Exit Function
200:
  ErrorLog("ScanFull " & ErrorToString())
  'Resume Next
 End Function
 Public Function chk_uzerBase_NoPids(ByVal vMD5 As String, ByVal vFile As String, ByVal Component1 As String) As Boolean
  chk_uzerBase_NoPids = False
  On Error GoTo 100

  If File.Exists(MyPath & "\uzerbase.txt") = False Then
   Exit Function
  End If
  If yes_vir_UZER(vMD5) = True Then
   chk_uzerBase_NoPids = True
   If action_virus_CRC(vFile, Virname, Component1) = False Then
    'MoveFileEx(tmp1, "", MOVEFILE_DELAY_UNTIL_REBOOT)
    inNonCure = inNonCure + 1
    Console.WriteLine(vFile & "-error cure")
    LogPrint(vFile & "(" & Virname & ")-Error cure(user md5)")
   End If
  End If
  Exit Function
100:
  ErrorLog("chk_uzerBase_NoPids " & ErrorToString())

 End Function
 Public Sub check_registeredAppl()
  If SL.isRegistered = True Then
   Dim licendUs As String = sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName)
   If Trim(licendUs).Length > 23 Then
    Console.WriteLine("Licensed To [" & sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName).Substring(0, 23) & "]")
    LogPrint("Licensed To [" & sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName).Substring(0, 23) & "]")
   Else
    Console.WriteLine("Licensed To " & sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName))
    LogPrint("Licensed To " & sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName))
   End If
   Console.WriteLine("SN:" & sGetINI(sINIFile, "USER", "SN", ""))
   LogPrint("SN:" & sGetINI(sINIFile, "USER", "SN", ""))
  Else
   Console.WriteLine("Software not registered.")
   Console.WriteLine("SN:0000000000000")
   LogPrint("Software not registered.")
   LogPrint("SN:0000000000000")
  End If
 End Sub
 Public Sub check_lastUpdate()
  On Error Resume Next
  Dim rmp As String = sGetINI(sINIFile, "USER", "LicExpirid", "Unknown")
  If rmp <> "Unknown" Then
   Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
   Dim a1 As String = a.Decrypt(rmp)
   Dim b As Date = CDate(a1)
   If DateDiff("d", Now, b) = 0 Or DateDiff("d", Now, b) < 0 Then
    myRegister = False
    Console.WriteLine("Your copy is old")
    LogPrint("Your copy is old")
    'MsgBox("Invalid license", MsgBoxStyle.Critical)
   Else
    myRegister = True
    Console.WriteLine("License Expired: " & a1)
    LogPrint("License Expired: " & a1)
   End If
  Else
   Console.WriteLine("License Expired: Unknown")
   LogPrint("License Expired: Unknown")
  End If

 End Sub
 Public Function gt_extension(ByVal a As String) As Boolean
  gt_extension = False
  Select Case a
   Case ".exe"
    gt_extension = True
    Exit Function
   Case ".com"
    gt_extension = True
    Exit Function
   Case ".bat"
    gt_extension = True
    Exit Function
   Case ".sys"
    gt_extension = True
    Exit Function
   Case ".cmd"
    gt_extension = True
    Exit Function
   Case ".dll"
    gt_extension = True
    Exit Function
   Case ".asp"
    gt_extension = True
    Exit Function
   Case ".js"
    gt_extension = True
    Exit Function
   Case ".vbs"
    gt_extension = True
    Exit Function
   Case ".htm"
    gt_extension = True
    Exit Function
   Case ".html"
    gt_extension = True
    Exit Function
   Case ".class"
    gt_extension = True
    Exit Function
   Case ".hta"
    gt_extension = True
    Exit Function
   Case ".eml"
    gt_extension = True
    Exit Function
   Case ".bin"
    gt_extension = True
    Exit Function
   Case ".chm"
    gt_extension = True
    Exit Function
   Case ".ocx"
    gt_extension = True
    Exit Function
   Case ".php"
    gt_extension = True
    Exit Function
   Case ".386"
    gt_extension = True
    Exit Function
   Case ".cpl"
    gt_extension = True
    Exit Function
   Case ".drv"
    gt_extension = True
    Exit Function
   Case ".inf"
    gt_extension = True
    Exit Function
   Case ".ovl"
    gt_extension = True
    Exit Function
   Case ".pif"
    gt_extension = True
    Exit Function
   Case ".vxd"
    gt_extension = True
    Exit Function
  End Select

 End Function
 Public Sub ScanScanner(ByVal dir1 As String)
  'сканирование опред. области...сканер
  On Error GoTo 200
  Dim myPerms As PermissionSet = New PermissionSet(PermissionState.None)
  myPerms.AddPermission(New FileIOPermission _
   (FileIOPermissionAccess.AllAccess, dir1))
  myPerms.AddPermission(New FileIOPermission _
     (FileIOPermissionAccess.Write, dir1))

  Dim hs As String = "None"
  With My.Computer.FileSystem
   '  If GetInputState() <> 0 Then

   '  End If
   ' List this directory's files.

   For Each file1 As String In .GetFiles(dir1)
    If gt_extension_zip(IO.Path.GetExtension(file1)) = True Then
     unpackFiles(file1, "[scan]", "Console")
     GoTo 150
    End If
    Dim fi As FileInfo = New FileInfo(file1)
    inCount = inCount + 1
    countCons = +1
    If countCons > 320 Then
     countCons = 0
     Console.Clear()
    End If
    Dim fileDetail As IO.FileInfo
    fileDetail = My.Computer.FileSystem.GetFileInfo(file1)
    Dim tmplen As Long = CLng(fileDetail.Length)
    Dim fp As String = sGetINI(sINIFile, "Console", "FILESIZE", "55242880")
    If tmplen >= CLng(fp) Then
     LogPrint(file1 & "-exclude (big size=)" & CStr(tmplen))
     GoTo 100
    End If
    Dim name4F As String = sGetINI(sINIFile, "Console", "SCANALL", "True")
    If name4F = False Then
     If gt_extension(fileDetail.Extension) = False Then
      GoTo 150
     End If
    End If
    a1 = 0
    a1 = (DateTime.Now.Millisecond)
    a3 = 0
    a3 = (DateTime.Now.Second)
    Dim StartTime As New DateTime()
    Dim EndTime As New DateTime()
    StartTime = DateTime.Now()
    '============================================================================
    hs = MD5_Hash(file1)
    If yes_vir(hs) = True Then
     LogPrint(file1 & "-infected " & Virname)
     Console.ForegroundColor = ConsoleColor.Red
     Console.WriteLine(file1 & "-infected " & Virname)
     Console.ForegroundColor = ConsoleColor.White
     inFound = inFound + 1
     'NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Found infected file" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus:" & Virname & vbCrLf & "Actions:REPORT", ToolTipIcon.Info)
     If action_virus(file1, Virname, "Console") = False Then
      inNonCure = inNonCure + 1
      SecondActions(file1.ToString, Virname, "Console")
     End If
     GoTo 100
    Else
     Dim name26 As Boolean = CBool(sGetINI(sINIFile, "Console", "ScanHeur", "False"))
     If name26 = True Then
      main_heur(file1, "Console")
     End If

    End If

    If chk_userbase_string(file1) = True Then
     LogPrint(file1 & "-find in user base(string in base)")
     inFound = inFound + 1
     Console.ForegroundColor = ConsoleColor.Red
     Console.WriteLine(file1 & "-infected unknow virus(user base record)")
     Console.ForegroundColor = ConsoleColor.White
     inFound = inFound + 1
     If action_virus_CRC(file1, "User record", "Console") = False Then
      inNonCure = inNonCure + 1
      SecondActions(file1, "User record", "Console")
     End If
    End If
    Console.WriteLine(file1 & "-OK")
100:
    f1 = 0
    f2 = 0
    a2 = 0
    a2 = (DateTime.Now.Millisecond)
    a4 = 0
    a4 = (DateTime.Now.Second)
    If a2 >= a1 Then
     f1 = a2 - a1
    End If
    If a4 >= a1 Then
     f2 = a4 - a3
    End If
    EndTime = DateTime.Now
    Dim answer1, answer2 As Long
    answer1 = 0
    ' Количество 100-наносекундных интервалов
    answer1 = EndTime.Ticks() - StartTime.Ticks()
    MyLibrary.FormFunction.check_upx_file(file1)
    Dim name87 As Boolean = CBool(sGetINI(sINIFile, "Console", "Izbitochnoe", "False"))
    If name87 = True Then
     If MyLibrary.FormFunction.check_upx_file(file1) = True Then
      LogPrint(file1 & "|" & hs & "|Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & tmplen & "[UPX]")

     Else
      LogPrint(file1 & "|" & hs & "|Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & tmplen)
     End If
    End If
150:
   Next file1
   ' Search child directories.
   For Each folder1 As Object In .GetDirectories(dir1)
    'TODO: Do something with the folder.
    '    If GetInputState() <> 0 Then
    '   End If
    Console.WriteLine("[" & folder1 & "]")
    ScanScanner(folder1)

   Next folder1

  End With

  Exit Sub
200:
  ErrorLog("Scan " & ErrorToString())
  Resume Next


 End Sub
 Public Sub SecondActions(ByVal secondfilename As String, ByVal st As String, ByVal Component1 As String)
  'второе действие с вирусом..если первое не удалось...
  On Error GoTo 100
  If Trim(secondfilename) = "" Then
   Exit Sub
  End If
  If File.Exists(secondfilename) = False Then
   Exit Sub
  End If
  If myRegister = False Then
   Exit Sub
  End If
  Dim keyName2 As String = sGetINI(sINIFile, "Shield", "Action", "REPORT")
  Dim keyName3 As String = sGetINI(sINIFile, "Shield", "Zapros", "False")
  Select Case keyName2
   Case "DELETE"
    'удалить
    MyLibrary.FormFunction.kiilONReboot(secondfilename)
    LogPrint(secondfilename & "-infected,delete after reboot " & Virname & "(DELETE)")
    Console.WriteLine(secondfilename & "-infected,delete after reboot")
    inDELETE = inDELETE + 1
    '    If GlassBox.ShowMessage("Curing some infected files requires a system reboot." & vbCrLf & "Continue ?", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Question, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
    '    If MsgBox(, MsgBoxStyle.Question, "Warning") = MsgBoxResult.Yes Then
    'MyLibrary.FormFunction.reboot()
    'End
    ' End If

    Exit Sub
   Case "REPORT"

   Case Else
    inNonCure = inNonCure + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(secondfilename & "-ERROR ACTION,REPORT")
    Console.ForegroundColor = ConsoleColor.White
    Exit Sub

  End Select
  Exit Sub
100:
  ErrorLog("SecondActions " & ErrorToString())
  ' MsgBox(ErrorToString)

 End Sub
 Public Sub LogPrint(ByVal smess As String)
  'главная проца ведения лога
  On Error GoTo 100
  Dim path As String = "consoleScan.log"
  Dim keyName4 As String = "5860966"
  Dim keyName2 As String = sGetINI(sINIFile, "Console", "Log", "True")
  If keyName2 = "False" Then
   Exit Sub
  End If
  keyName4 = sGetINI(sINIFile, "Console", "LogSize", "5860966")

  If File.Exists(path) = True Then
   Dim fileDetail As IO.FileInfo
   fileDetail = My.Computer.FileSystem.GetFileInfo(path)
   If CLng(keyName4) <= CLng(fileDetail.Length) Then
    File.Delete(path)
   End If
  End If
  Dim sw As StreamWriter = File.AppendText(path)
  sw.WriteLine(smess)
  sw.Flush()
  sw.Close()
  Exit Sub
100:
  ErrorLog("logprint " & ErrorToString())

 End Sub
 Public Sub getAutorun(ByVal str2 As String)
  On Error GoTo 100
  If My.Computer.FileSystem.FileExists(str2 & "autorun.inf") = False Then
   ' MsgBox("Autorun.inf file does not exist in this drive2")
   'NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Autorun.inf отсутствует на этом диске", ToolTipIcon.Info)
  Else
   podchistka(str2)
   ' If My.Computer.FileSystem.FileExists(str2 & "autorun.inf") = True Then
   ' LogPrint(str2 & "autorun.inf-error cure ")
   'Console.WriteLine(Trim(str2 & "autorun.inf-error cure"))
   'End If

  End If
  Exit Sub
100:
  ErrorLog("getAutorun " & ErrorToString())

 End Sub
 Sub podchistka(ByVal str2 As String)
  On Error GoTo 10
  Dim n As String = sGetINI(sINIFile, "Shield", "Autorun", "True")
  If n = "True" Then
   If File.Exists(MyPath & "\quarantine\autorun.inf") = True Then
    File.Delete(MyPath & "\quarantine\autorun.inf")
   End If
   ' My.Computer.FileSystem.DELETEFile(str & "autorun.inf", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DELETEPermanently)
   Console.ForegroundColor = ConsoleColor.Red
   Console.WriteLine(Trim(str2 & "autorun.inf-AutoIT"))
   Console.ForegroundColor = ConsoleColor.White
   If action_virus(Trim(str2 & "autorun.inf"), "AutoIT", "Console") = False Then
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(Trim(str2 & "autorun.inf-error cure"))
    Console.ForegroundColor = ConsoleColor.White
   End If


  End If
  Exit Sub
10:
  ErrorLog("podchistka " & ErrorToString())

 End Sub
 Public Sub AUto_run_Thismashine()
  'проверка автозагрузки
  'ScanAutoStart
  Dim tmp As Boolean = CBool(sGetINI(sINIFile, "Console", "ScanAutoStart", "True"))
  If tmp = False Then
   Exit Sub
  End If
  ScanScanner(My.Application.GetEnvironmentVariable("ALLUSERSPROFILE") & "\Главное меню\Программы\Автозагрузка")
  ScanScanner(My.Application.GetEnvironmentVariable("HOMEDRIVE") & My.Application.GetEnvironmentVariable("HOMEPATH") & "\Главное меню\Программы\Автозагрузка")

  'ScanScanner(My.Application.GetEnvironmentVariable("windir") & "\Tasks")
  load_reg()
  load_Startup()
  '  End If


 End Sub
 Private Sub load_Startup()
  On Error GoTo 500
  Dim Path As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
  Dim f() As String = IO.Directory.GetFiles(Path, "*.*", IO.SearchOption.AllDirectories)

  For Each file As String In f
   Dim name As String = IO.Path.GetFileNameWithoutExtension(file)
   Dim ext As String = IO.Path.GetExtension(file)
   Dim p As String = Mid(file, 1, (Len(file) - Len(name & ext) - 1))
   If Not ext.ToLower = ".ini" Then
    If ext.ToLower = ".lnk" Then
     Dim lnkt As String = getShortcutTarget(p, name & ".lnk")
     scanOneFile(lnkt)

    End If


   End If

  Next
  Exit Sub
500:
  ErrorLog("load_Startup" & ErrorToString())

 End Sub
 Private Sub load_reg()
  On Error GoTo 200
  Dim tmpVar As String = 0
  For Each k As String In cksu.GetValueNames
   tmpVar = 1
   If Trim(k.ToString) <> "" Then
    scanOneFile(cksu.GetValue(k).ToString)
   End If
  Next
  For Each k As String In lksu.GetValueNames
   tmpVar = 3
   If Trim(k.ToString) <> "" Then
    scanOneFile(lksu.GetValue(k).ToString)
   End If
  Next
  For Each k As String In lksu1.GetValueNames
   'Dim g As ListViewGroup = lvwProcs1.Groups(1)
   tmpVar = 4
   If Trim(k.ToString) <> "" Then
    scanOneFile(lksu1.GetValue(k).ToString)
   End If
  Next


  ' For Each k As String In lksu2.GetValueNames
  ''Dim g As ListViewGroup = lvwProcs1.Groups(1)
  'tmpVar = 5
  'If Trim(k.ToString) <> "" Then
  'scanOneFile(lksu2.GetValue(k).ToString)
  'End If
  'Next

  For Each k As String In Hksu.GetValueNames
   tmpVar = 6
   If Trim(k.ToString) <> "" Then
    scanOneFile(Hksu.GetValue(k).ToString)
   End If
  Next


  '==========
  For Each k As String In lksu3.GetValueNames
   tmpVar = 7
   If Trim(k.ToString) <> "" Then
    scanOneFile(lksu3.GetValue(k).ToString)
   End If
  Next




  Exit Sub
200:
  ErrorLog("load_reg предшеств. перем." & tmpVar & " " & ErrorToString())
 End Sub
 Sub scanOneFile(ByVal zac As String)
  'сканирование опред. области...сканер
  On Error GoTo 200
  If File.Exists(zac) = False Then
   Exit Sub
  End If
  Dim myPerms As PermissionSet = New PermissionSet(PermissionState.None)
  myPerms.AddPermission(New FileIOPermission _
   (FileIOPermissionAccess.AllAccess, zac))
  myPerms.AddPermission(New FileIOPermission _
     (FileIOPermissionAccess.Write, zac))

  Dim hs As String = "None"
  With My.Computer.FileSystem

   If gt_extension_zip(IO.Path.GetExtension(zac)) = True Then
    unpackFiles(zac, "[scan]", "Console")
    GoTo 150
   End If
   Dim fi As FileInfo = New FileInfo(zac)
   inCount = inCount + 1
   countCons = +1
   If countCons > 320 Then
    countCons = 0
    Console.Clear()
   End If
   Dim fileDetail As IO.FileInfo
   fileDetail = My.Computer.FileSystem.GetFileInfo(zac)
   Dim tmplen As Long = CLng(fileDetail.Length)
   Dim fp As Long = CLng(sGetINI(sINIFile, "Console", "FILESIZE", "55242880"))
   If tmplen >= CLng(fp) Then
    LogPrint(zac & "-exclude (big size=)" & CStr(fileDetail.Length))
    GoTo 100
   End If
   Dim name4F As String = sGetINI(sINIFile, "Console", "SCANALL", "True")
   If name4F = False Then
    If gt_extension(fileDetail.Extension) = False Then
     GoTo 150
    End If
   End If
   a1 = 0
   a1 = (DateTime.Now.Millisecond)
   a3 = 0
   a3 = (DateTime.Now.Second)
   Dim StartTime As New DateTime()
   Dim EndTime As New DateTime()
   StartTime = DateTime.Now()
   '============================================================================
   hs = MD5_Hash(zac)
   If yes_vir(hs) = True Then
    LogPrint(zac & "-infected " & Virname)
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(zac & "-infected " & Virname)
    Console.ForegroundColor = ConsoleColor.White
    inFound = inFound + 1
    'NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Found infected file" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus:" & Virname & vbCrLf & "Actions:REPORT", ToolTipIcon.Info)
    If action_virus(zac, Virname, "Console") = False Then
     inNonCure = inNonCure + 1
     SecondActions(zac.ToString, Virname, "Console")
    End If
    GoTo 100
   Else
    Dim name26 As Boolean = CBool(sGetINI(sINIFile, "Console", "ScanHeur", "False"))
    If name26 = True Then
     main_heur(zac, "Console")
    End If

   End If

   If chk_userbase_string(zac) = True Then
    LogPrint(zac & "-find in user base(string in base)")
    inFound = inFound + 1
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine(zac & "-infected unknow virus(user base record)")
    Console.ForegroundColor = ConsoleColor.White
    inFound = inFound + 1
    If action_virus_CRC(zac, "User record", "Console") = False Then
     inNonCure = inNonCure + 1
     SecondActions(zac, "User record", "Console")
    End If
   End If
   Console.WriteLine(zac & "-OK")
100:
   f1 = 0
   f2 = 0
   a2 = 0
   a2 = (DateTime.Now.Millisecond)
   a4 = 0
   a4 = (DateTime.Now.Second)
   If a2 >= a1 Then
    f1 = a2 - a1
   End If
   If a4 >= a1 Then
    f2 = a4 - a3
   End If
   EndTime = DateTime.Now
   Dim answer1, answer2 As Long
   answer1 = 0
   ' Количество 100-наносекундных интервалов
   answer1 = EndTime.Ticks() - StartTime.Ticks()
   MyLibrary.FormFunction.check_upx_file(zac)
   Dim name87 As Boolean = CBool(sGetINI(sINIFile, "Console", "Izbitochnoe", "False"))
   If name87 = True Then
    If MyLibrary.FormFunction.check_upx_file(zac) = True Then
     LogPrint(zac & "|" & hs & "|Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & tmplen & "[UPX]")

    Else
     LogPrint(zac & "|" & hs & "|Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & tmplen)
    End If
   End If
150:
  End With

  Exit Sub
200:
  ErrorLog("Scan " & ErrorToString())
 End Sub
End Module
