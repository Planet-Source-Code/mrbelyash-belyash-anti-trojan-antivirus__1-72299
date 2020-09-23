Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports datax = System.Diagnostics.FileVersionInfo
Imports System.Security.Permissions
Imports System.DateTime
Imports System.Security.AccessControl


Public Module Module1
 Dim a1 As Long
 Dim a3 As Long
 Dim a2 As Long
 Dim f1 As Long
 Dim f2 As Long
 Dim a4 As Long
 Public kolwoZap As Long
 Public line2 As String
 Public line3 As String
 Public Monitoring As Boolean
 Public Virname As String
 Public myRegister As Boolean = False
 Dim MyLibrary As New MyLibrary.MyLib
 Dim keysize As New cript.clsAESV2.KeySize
 Public SL As New SoftwareLOCK
 Private Structure SHFILEINFO
  Public hIcon As IntPtr
  Public iIcon As Integer
  Public dwAttributes As Integer
  <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public szDisplayName As String
  <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)> Public szTypeName As String
 End Structure


 Private Declare Auto Function SHGetFileInfo Lib "shell32.dll" (ByVal pszPath As String, _
          ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As IntPtr

 Private Const SHGFI_ICON = &H100
 Private Const SHGFI_SMALLICON = &H1
 Private Const SHGFI_LARGEICON = &H0
 Private Const MAX_PATH = 260
 Private cksu As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
 Private lksu As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SoftWare\Microsoft\Windows\CurrentVersion\Run", True)
 Private lksu1 As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", True)
 Private lksu2 As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", True)
 Private Hksu As Microsoft.Win32.RegistryKey = My.Computer.Registry.Users.OpenSubKey(".DEFAULT\Software\Microsoft\Windows\CurrentVersion\Run", True)
 Private lksu3 As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnceEx", True)
 ' Private lksu4 As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\RunServices", True)
 Private imgOP As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options", True)
 Sub AddDirectorySecurity(ByVal FileName As String, ByVal Account As String, ByVal Rights As FileSystemRights, ByVal ControlType As AccessControlType)

  'Create a new DirectoryInfoobject. 
  Dim dInfo As New DirectoryInfo(FileName)

  'Get a DirectorySecurity object that represents the current security settings. 
  Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()

  'Add the FileSystemAccessRule to the security settings. 
  dSecurity.AddAccessRule(New FileSystemAccessRule(Account, Rights, ControlType))

  'Set the new access settings. 
  dInfo.SetAccessControl(dSecurity)

 End Sub

 Sub RemoveDirectorySecurity(ByVal FileName As String, ByVal Account As String, ByVal Rights As FileSystemRights, ByVal ControlType As AccessControlType)

  'Create a new DirectoryInfo object. 
  Dim dInfo As New DirectoryInfo(FileName)

  'Get a DirectorySecurity object that represents the 
  'current security settings. 
  Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()

  'Add the FileSystemAccessRule to the security settings. 
  dSecurity.RemoveAccessRule(New FileSystemAccessRule(Account, Rights, ControlType))

  'Set the new access settings. 
  dInfo.SetAccessControl(dSecurity)

 End Sub

 Public Function MD5_Hash(ByVal FileName As String) As String
  On Error GoTo 11
  'Get Owner Information 
  Dim sFile As String = FileName
  Dim sOwner As String = ""
  Dim sEnvironment As String = ""
  Dim DirectoryName As String = IO.Path.GetDirectoryName(FileName)
  Dim dInfo As New DirectoryInfo(DirectoryName)
  Dim myOwner As Security.Principal.IdentityReference = dInfo.GetAccessControl.GetOwner(GetType(Security.Principal.NTAccount))

  sOwner = myOwner.ToString
  sEnvironment = Environment.OSVersion.ToString()

  'XP = 5.1 and WIN 2000 = 5.0
  If InStr(1, sEnvironment, "NT 5.1") > 0 Then
   sOwner = "CREATOR OWNER"
  Else
   sOwner = "Everyone"
  End If

  'Add the access control entry to the directory. 
  AddDirectorySecurity(DirectoryName, sOwner, FileSystemRights.FullControl, AccessControlType.Allow)
  'Set File Security 
  Dim oFI As FileInfo = New FileInfo(sFile)

  'Check to see if file exists as path location 
  If oFI.Exists = False Then
   MD5_Hash = "None"
   Exit Function
  End If

  'Add the access control entry to the file. 
  AddDirectorySecurity(sFile, sOwner, FileSystemRights.FullControl, AccessControlType.Allow)
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
      f = Nothing
  Exit Function
11:
  Return "None"
 End Function
 Function chkLogHS() As Boolean
  'писать в лог хеш файла ?
  chkLogHS = False
  On Error GoTo 101
  Dim f1 As String = sGetINI(sINIFile, "Shield", "LogHash", "True")
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
    Virname = tmpstr2.Substring(tmpstr.Length, tmpstr2.Length - tmpstr.Length)
    yes_vir_temp = True

   End If
  End If
 End Function

 Public Function yes_vir(ByVal SearchChar As String) As Boolean
  yes_vir = False
  With My.Computer.FileSystem
   For Each file1 As String In .GetFiles(Application.StartupPath)

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

 Sub timeprow()

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

  Form1.lblScore.Text = f2 & "." & f1
  Form1.lblScore.Refresh()
 End Sub

 Public Sub get_virname(ByVal sd As String, ByVal ps As Integer, ByVal alg As Integer)
  If alg = 2 Then
   Virname = Right(sd, Len(sd) - 9)
  Else
   Virname = Right(sd, Len(sd) - 33)
  End If
 End Sub
 Public Sub LogPrint(ByVal selfile As String, ByVal smess As String)
  'главная проца ведения лога
  On Error GoTo 100
  Dim path As String = "otchetMon.log"
  Dim keyName4 As String = "5860966"
  Select Case Trim(LCase(selfile))
   Case "Scanner"
    Dim keyName2 As String = sGetINI(sINIFile, "Scanner", "Log", "True")
    If keyName2 = "False" Then
     Exit Sub
    End If
    path = "otchetScan.log"
    keyName4 = sGetINI(sINIFile, "Scanner", "LogSize", "5860966")
   Case "Shield"
    Dim keyName2 As String = sGetINI(sINIFile, "Shield", "Log", "True")
    If keyName2 = "False" Then
     Exit Sub
    End If
    path = "otchetMon.log"
    keyName4 = sGetINI(sINIFile, "Shield", "LogSize", "5860966")
   Case "firewall"
    Dim keyName2 As String = sGetINI(sINIFile, "firewall", "Log", "True")
    If keyName2 = "False" Then
     Exit Sub
    End If
    path = "otchetFirewall.log"
    keyName4 = sGetINI(sINIFile, "firewall", "LogSize", "5860966")
   Case "vault"
    Dim keyName2 As String = sGetINI(sINIFile, "firewall", "Log", "True")
    If keyName2 = "False" Then
     Exit Sub
    End If
    path = "otchetVault.log"
    keyName4 = sGetINI(sINIFile, "firewall", "LogSize", "5860966")
   Case "update"
    Dim keyName2 As String = sGetINI(sINIFile, "update", "Log", "True")
    If keyName2 = "False" Then
     Exit Sub
    End If
    path = "otchetUpdate.log"
    keyName4 = sGetINI(sINIFile, "update", "LogSize", "5860966")
   Case "update"
    Dim keyName2 As String = sGetINI(sINIFile, "error", "Log", "True")
    If keyName2 = "False" Then
     Exit Sub
    End If
    path = "ERRORLOG.log"
    keyName4 = sGetINI(sINIFile, "error", "LogSize", "5860966")
    'Registry
   Case "registry"
    Dim keyName2 As String = sGetINI(sINIFile, "registry", "Log", "True")
    If keyName2 = "False" Then
     Exit Sub
    End If
    path = "registryScan.log"
    keyName4 = sGetINI(sINIFile, "registry", "LogSize", "5860966")
  End Select


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
 Public Function get_kolwovirus() As Boolean
  'получить общее кол-во записей во всех базах(пользовательскую со строками пока-что игнорируем)
  '====================
  With My.Computer.FileSystem
   For Each file1 As String In .GetFiles(Application.StartupPath)

    If IO.Path.GetExtension(file1.ToString) = ".bvb" Then
     'MsgBox(file1.ToString)
     n111(file1.ToString)
    End If

   Next
  End With

  sbor_userbase()
  Form1.lblZap.Text = kolwoZap
  Form1.Label53.Text = kolwoZap
 End Function
 Sub sbor_userbase()
  'подсчет пользовательских записей
  Try
   If File.Exists(Application.StartupPath & "\uzerbase.bvb") = False Then
    Exit Sub
   End If
   Using sr1 As StreamReader = New StreamReader(Application.StartupPath & "\uzerbase.bvb")
    Dim line1 As String
    Do
     line1 = sr1.ReadLine()
     If Trim(line1) <> "" Then
      kolwoZap = kolwoZap + 1
     End If
    Loop Until line1 Is Nothing
    sr1.Close()
    '=======
   End Using
  Catch E As Exception
   ' Let the user know what went wrong.
   MsgBox("Unknow problem with bases(user base)")
   ErrorLog("module1.sbor_userbase " & ErrorToString())
   'Console.WriteLine(E.Message)
  End Try
 End Sub
 Sub n111(ByVal nb As String)
  'перебор всех баз + подсчет их содержимого
  Try

   Using sr1 As StreamReader = New StreamReader(nb)
    Dim line1 As String
    Do

     line1 = sr1.ReadLine()
     If Trim(line1) <> "" Then
      kolwoZap = kolwoZap + 1
     End If
    Loop Until line1 Is Nothing
    sr1.Close()
    '=======
   End Using
  Catch E As Exception
   ' Let the user know what went wrong.
   ErrorLog("module1.n111 " & ErrorToString())

  End Try
 End Sub
 Public Function yes_vir_registry(ByVal SearchChar3 As String) As Boolean
  'подчистка реестра
  'очень неприятный параметр в реестре блокирующий запуск некоторых (в основном антивирусных программ,административных утилит).
  'подумать...сделать рандомное имя щилда или же сделать рандомное имя если щилд будет запущен с помощью коммандной строки с опред. параметром
  'хотя врядли в данном случе поможет,если попадет под прессинг вирусописателей.....

  Try
   yes_vir_registry = False

   ' Create an instance of StreamReader to read from a file.
   If File.Exists(Application.StartupPath & "\regbase.txt") = False Then
    Exit Function
   End If
   Using sr As StreamReader = New StreamReader(Application.StartupPath & "\regbase.txt")
    Dim line As String
    ' Read and display the lines from the file until the end 
    ' of the file is reached.
    Do
     line = sr.ReadLine()
     Dim TestPos As Integer
     ' A textual comparison starting at position 4. Returns 6.
     TestPos = InStr(1, UCase(line), UCase(SearchChar3), CompareMethod.Binary)
     If TestPos <> 0 Then
      'Dim li As New ListViewItem(SearchChar3, 0)
      'li.SubItems.Add("HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options")
      'li.SubItems.Add("DELETE")
      'Form1.ListView1.Items.Add(li)
      yes_vir_registry = True
      sr.Close()
      timeprow()
      sound_me("1")
      Exit Function
     End If
    Loop Until line Is Nothing
    sr.Close()
    timeprow()
   End Using
  Catch E As Exception
   ' Let the user know what went wrong.
   MsgBox("Unknow problem with bases registry entries")
   'Console.WriteLine(E.Message)
   ErrorLog("yes_vir_registry " & ErrorToString())
  End Try
 End Function
 Public Sub sound_me(ByVal d As String)
  'звук
  On Error GoTo 10
  Dim s1 As String = sGetINI(sINIFile, "Shield", "Sound", "True")
  If s1 = "False" Then
   Exit Sub
  End If

  Select Case d
   Case "1"
    'vir found
    My.Computer.Audio.Play(Application.StartupPath & "\infected_p.wav", AudioPlayMode.Background)
   Case "2"
    'error
    My.Computer.Audio.Play(Application.StartupPath & "\m3.wav", AudioPlayMode.Background)
   Case "3"
    'fon
    My.Computer.Audio.Play(Application.StartupPath & "\m3.wav", AudioPlayMode.Background)
   Case "exit"
    My.Computer.Audio.Play(Application.StartupPath & "\exit.wav", AudioPlayMode.Background)
   Case "lic"
    My.Computer.Audio.Play(Application.StartupPath & "\init.wav", AudioPlayMode.Background)

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
   If File.Exists(Application.StartupPath & "\uzerbase.bvb") = False Then
    Exit Function
   End If
   a1 = 0
   a1 = (DateTime.Now.Millisecond)
   a3 = 0
   a3 = (DateTime.Now.Second)
   ' Create an instance of StreamReader to read from a file.
   Using sr As StreamReader = New StreamReader(Application.StartupPath & "\uzerbase.bvb")
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
   MsgBox("Unknow problem with user base", MsgBoxStyle.Critical)
   ErrorLog("yes_vir_UZER " & ErrorToString())
  End Try
 End Function
 '==========================================
 Sub notify_chkreestr(ByVal v As String)
  'тултип при исправлении реестра
  Form1.NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Исправлен реестр" & vbCrLf & "Image File Execution Options\" & v, ToolTipIcon.Info)
  Form1.txtScanning.Text = "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & v
 End Sub
 '===========================
 Public Sub chk_first_ST9()
  'проверить есть ли файл настроек..Если нет..то создать дефолтные настройки

  If File.Exists(Application.StartupPath & "\SETTINGS.INI") = False Then
   first_start_registry2()
  End If

 End Sub
 Public Sub first_start_registry2()
  'дефолтные настройки записать в файл настроек,если он отсутствует
  On Error GoTo 100
  writeINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName)
  writeINI(sINIFile, "USER", "Machine", MyLibrary.FormFunction.GetComputerName)
  writeINI(sINIFile, "USER", "Number", MyLibrary.FormFunction.get_serialdisk)
  writeINI(sINIFile, "USER", "SN", "")
  writeINI(sINIFile, "USER", "LicExpirid", "Unknown")
  writeINI(sINIFile, "Shield", "Timer", "50")
      writeINI(sINIFile, "Shield", "FirstStart", "True")
  writeINI(sINIFile, "Shield", "Log", "True")
  writeINI(sINIFile, "Shield", "Append", "True")
  writeINI(sINIFile, "Shield", "Chick", "True")
  writeINI(sINIFile, "Shield", "LogSize", "5860843")
  writeINI(sINIFile, "Shield", "LogActiviti", "False")
  writeINI(sINIFile, "Shield", "Action", "Report")
  writeINI(sINIFile, "Shield", "ActionUnknow", "Report")
  writeINI(sINIFile, "Shield", "ActionZip", "Report")
  writeINI(sINIFile, "Shield", "Sound", "False")
  writeINI(sINIFile, "Shield", "Fon", "True")
  writeINI(sINIFile, "Shield", "Status", "True")
  writeINI(sINIFile, "Shield", "LogHash", "True")
  writeINI(sINIFile, "Shield", "Evristic", "False")
  writeINI(sINIFile, "Shield", "Registry", "True")
  writeINI(sINIFile, "Shield", "FirstStart", "True")
  writeINI(sINIFile, "Shield", "NoCheckLenght", "True")
  writeINI(sINIFile, "Shield", "Zapros", "True")
  writeINI(sINIFile, "Shield", "CheckLenght", "15142784")
  writeINI(sINIFile, "Shield", "Monitoring", "True")
  writeINI(sINIFile, "Shield", "Time", "True")
  writeINI(sINIFile, "Shield", "Extentions", "False")
  writeINI(sINIFile, "Shield", "Autorun", "True")
  writeINI(sINIFile, "Shield", "Memory", "True")
  writeINI(sINIFile, "Shield", "Priority", "Normal")
  writeINI(sINIFile, "Shield", "CheckZip", "True")
  writeINI(sINIFile, "Shield", "AutoZapusk", "True")
  writeINI(sINIFile, "Shield", "Count_Exclude", "0")
  writeINI(sINIFile, "Exclude_Shield", "0", "")
  writeINI(sINIFile, "Shield", "Run", "True")
  '===============
  writeINI(sINIFile, "Scanner", "LastScan", "Unknown")
  writeINI(sINIFile, "Scanner", "Custom", "True")
  writeINI(sINIFile, "Scanner", "Transparent", "80") ' записываешь(имя ключа, параметры)
  writeINI(sINIFile, "Scanner", "ScanAutoStart", "True") ' записываешь(имя ключа, параметры)
  writeINI(sINIFile, "Scanner", "Log", "True")
  writeINI(sINIFile, "Scanner", "Append", "True")
  writeINI(sINIFile, "Scanner", "LogSize", "122222222")
  writeINI(sINIFile, "Scanner", "Action", "REPORT")
  writeINI(sINIFile, "Scanner", "NoCheckLenght", "True")
  writeINI(sINIFile, "Scanner", "ActionUncknow", "REPORT")
  writeINI(sINIFile, "Scanner", "SCANALL", "True")
  writeINI(sINIFile, "Scanner", "Ask", "False")
  writeINI(sINIFile, "Scanner", "CheckZip", "True")
  writeINI(sINIFile, "Scanner", "FILESIZE", "55242880")
  writeINI(sINIFile, "Scanner", "SOUND", "True")
  writeINI(sINIFile, "Scanner", "CheckRegistry", "True")
  writeINI(sINIFile, "Scanner", "ScanHeur", "True")
  writeINI(sINIFile, "Scanner", "ScanMemory", "True")
  writeINI(sINIFile, "Scanner", "Izbitochnoe", "True")
  writeINI(sINIFile, "Scanner", "PRIORITY", "Normal")
  writeINI(sINIFile, "Scanner", "Count_Exclude_Scan", "0")
  writeINI(sINIFile, "Exclude_Scanner", "0", "")
  writeINI(sINIFile, "Update", "LastUpdate", "Unknown")
  '==========================
  writeINI(sINIFile, "Console", "Custom", "True")
  writeINI(sINIFile, "Console", "ScanAutoStart", "True")
  writeINI(sINIFile, "Console", "Log", "True")
  writeINI(sINIFile, "Console", "Append", "True")
  writeINI(sINIFile, "Console", "LogSize", "122222222")
  writeINI(sINIFile, "Console", "Action", "REPORT")
  writeINI(sINIFile, "Console", "ActionUncknow", "REPORT")
  writeINI(sINIFile, "Console", "SCANALL", "True")
  writeINI(sINIFile, "Console", "Ask", "False")
  writeINI(sINIFile, "Console", "CheckZip", "False")
  writeINI(sINIFile, "Console", "FILESIZE", "55242880")
  writeINI(sINIFile, "Console", "SOUND", "True")
  writeINI(sINIFile, "Console", "CheckRegistry", "False")
  writeINI(sINIFile, "Console", "ScanHeur", "False")
  writeINI(sINIFile, "Console", "ScanMemory", "True")
  writeINI(sINIFile, "Console", "Izbitochnoe", "False")
  writeINI(sINIFile, "Console", "PRIORITY", "Normal")
  '==========
  writeINI(sINIFile, "Vault", "Extension", "###")
  writeINI(sINIFile, "Vault", "Cript", "False")
  '=================
  writeINI(sINIFile, "Registry", "Log", "True")
  writeINI(sINIFile, "Registry", "LogSize", "5860966")
  writeINI(sINIFile, "Registry", "CureAuto", "True")
  writeINI(sINIFile, "Registry", "Backup", "True")
      '==================
      writeINI(sINIFile, "Blocker", "Enable", "False")
      writeINI(sINIFile, "Blocker", "Time", "100")
      writeINI(sINIFile, "Blocker", "Scan", "False")

  Const userRoot As String = "HKEY_LOCAL_MACHINE"
  Const subkey As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"
  Const keyName As String = userRoot & "\" & subkey
  Registry.SetValue(keyName, _
"Belyash Shield", Application.ExecutablePath, RegistryValueKind.ExpandString)
  '"Exclude_Shield"
  Exit Sub
100:
  ErrorLog("first_start_registry2 " & ErrorToString())

 End Sub
 Public Sub ErrorLog(ByVal smess45 As String)
  'будем записывать ошибки
  On Error GoTo 100
  Dim path As String = Application.StartupPath & "\ERRORLOG.LOG"
  If File.Exists(path) = True Then
   Dim fileDetail As IO.FileInfo
   fileDetail = My.Computer.FileSystem.GetFileInfo(path)

   If CInt(fileDetail.Length) >= 5860966 Then
    File.Delete(path)
   End If
  End If
  Dim sw As StreamWriter = File.AppendText(path)
  sw.WriteLine(Format$(Now, "dd-mm-yyyy") & " " & smess45 & "-Shield")
  sw.Flush()
  sw.Close()
  Exit Sub
100:

 End Sub
 Public Sub LogQuarant(ByVal smess456 As String)
  'будем записывать ошибки
  On Error GoTo 100
  If Directory.Exists(Application.StartupPath & "\quarantine") = False Then
   Directory.CreateDirectory(Application.StartupPath & "\quarantine")
  End If


  Dim path As String = Application.StartupPath & "\quarantine\MovedFiles.LOG"
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
 'автозагрузка=============================
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
 Public Sub AUto_run_Thismashine()
  'проверка автозагрузки
  getAllRunAuto()
  MapDirectory(My.Application.GetEnvironmentVariable("ALLUSERSPROFILE") & "\Главное меню\Программы\Автозагрузка")
  MapDirectory(My.Application.GetEnvironmentVariable("HOMEDRIVE") & My.Application.GetEnvironmentVariable("HOMEPATH") & "\Главное меню\Программы\Автозагрузка")
  MapDirectory(My.Application.GetEnvironmentVariable("windir") & "\Tasks")
  load_reg()
  load_Startup()
  '  End If
  ' If CheckBox7.Checked = True Then
  ' chk_registry()
  ' End If
  ' If CheckBox4.Checked = True Then
  ' alldisks()
  ' End If
  '   getUserinint()

 End Sub

 Sub getAllRunAuto()
  'сбор автозагрузки...по отдельным параметрам
  On Error Resume Next

  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "Common Startup", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "Common AltStartup", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", "Common", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", "Common Startup", "None")
  getRegWetki("HKCU", "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "Startup", "None")
  getRegWetki("HKCU", "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "AltStartup", "None")
  getRegWetki("HKCU", "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", "AltStartup", "None")
  getRegWetki("HKCU", "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", "Startup", "None")
  getRegWetki("HKCU", "Software\Microsoft\Windows NT\CurrentVersion\Windows", "load", "None")
  getRegWetki("HKCU", "Software\Microsoft\Windows NT\CurrentVersion\Windows", "run", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "run", "None")
  getRegWetki("HKCU", "Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "run", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "shell", "None")
  getRegWetki("HKCU", "Software\Microsoft\Windows\CurrentVersion\Policies\System", "shell", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AppSetup", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "GinaDLL", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "System", "None")
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "Taskman", "None")
  'getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "UIHost", "" , 1)
  ' getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "VmApplet", "rundll32 shell32,Control_RunDLL " & Chr(34) & "sysdm.cpl" & Chr(34), 0)
  getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "shell", "Explorer.exe")
  getRegWetki("HKCU", "Software\Microsoft\Windows NT\CurrentVersion\Winlogon", "shell", "Explorer.exe")
  'getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "UIHost", "logonui.exe", "Автозагрузка(Опасность)")
  'getRegWetki("HKLM", "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AppSetup", "" )



 End Sub
 Sub getRegistry(ByVal userRoot1 As String, ByVal subkey1 As String, ByVal keySod As String, ByVal zna4 As String, ByVal camment As String)
  On Error GoTo 101
  Select Case userRoot1
   Case "HKLM"
    userRoot1 = "HKEY_LOCAL_MACHINE"
   Case "HCR"
    userRoot1 = "HKEY_CLASSES_ROOT"
   Case "HKCU"
    userRoot1 = "HKEY_CURRENT_USER"
   Case "HKCC"
    userRoot1 = "HKEY_CURRENT_CONFIG"
   Case "HKU"
    userRoot1 = "HKEY_USERS"

  End Select
  'Const userRoot1 As String = "HKEY_LOCAL_MACHINE"
  'Const subkey1 As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\policies\WinOldApp"
  Dim keyName1 As String = userRoot1 & "\" & subkey1
  Dim tExpand3 As String = My.Computer.Registry.GetValue(keyName1, _
       keySod, zna4)
  'If LCase(tExpand3) <> LCase(zna4) Then
  ' Registry.SetValue(keyName1, _
  ' keySod, zna4)
  If tExpand3 <> zna4 Then
   MapDirectory(tExpand3)
  End If

  'End If
  Exit Sub
101:
  ErrorLog("getRegistry " & ErrorToString())

 End Sub

 Sub getRegWetki(ByVal userRoot1 As String, ByVal subkey1 As String, ByVal keySod As String, ByVal zna4 As String)
  On Error GoTo 101
  Select Case userRoot1
   Case "HKLM"
    userRoot1 = "HKEY_LOCAL_MACHINE"
   Case "HCR"
    userRoot1 = "HKEY_CLASSES_ROOT"
   Case "HKCU"
    userRoot1 = "HKEY_CURRENT_USER"
   Case "HKCC"
    userRoot1 = "HKEY_CURRENT_CONFIG"
   Case "HKU"
    userRoot1 = "HKEY_USERS"

  End Select
  'Const userRoot1 As String = "HKEY_LOCAL_MACHINE"
  'Const subkey1 As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\policies\WinOldApp"
  Dim keyName1 As String = userRoot1 & "\" & subkey1
  Dim tExpand3 As String = My.Computer.Registry.GetValue(keyName1, _
       keySod, zna4)
  'If LCase(tExpand3) <> LCase(zna4) Then
  ' Registry.SetValue(keyName1, _
  ' keySod, zna4)
  If tExpand3 <> "None" Then
   'CheckedListBox1.Items.Add(tExpand3)
   MapDirectory(tExpand3)
  End If

  'End If
  Exit Sub
101:
  ErrorLog("getRegWetki " & ErrorToString())

 End Sub

 Public Sub MapDirectory(ByVal dir As String)
  On Error GoTo 200
  If Trim(dir) = "" Then
   Exit Sub
  End If
  If IO.Directory.Exists(dir) = False Then
   Exit Sub
  End If
  With My.Computer.FileSystem
   Application.DoEvents()
   For Each file1 As String In .GetFiles(dir)
    Application.DoEvents()
    If Monitoring = False Then
     Exit Sub
    End If
    Form1.txtScanning.Text = file1
    Form1.txtScanning.Refresh()
    Form1.Scan(file1, "[AUTO]")
   Next file1
  End With
  Exit Sub
200:
  ErrorLog("MapDirectory " & ErrorToString())

  Resume Next
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
     Form1.Scan(lnkt, "[AUTO]")

    End If


   End If

  Next
  Exit Sub
500:
  ErrorLog("load_Startup" & ErrorToString())

 End Sub
 Private Sub load_reg()
  On Error GoTo 200

  For Each k As String In cksu.GetValueNames
   If Trim(k) <> "" Then

    Form1.Scan(cksu.GetValue(k).ToString, "[AUTO]")

   End If
  Next
  Dim lksu As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SoftWare\Microsoft\Windows\CurrentVersion\Run", True)
  For Each k As String In lksu.GetValueNames
   If Trim(k) <> "" Then
    Form1.Scan(lksu.GetValue(k).ToString, "[AUTO]")
   End If
  Next


  For Each k As String In lksu1.GetValueNames
   'Dim g As ListViewGroup = lvwProcs1.Groups(1)

   If Trim(k) <> "" Then
    Form1.Scan(lksu1.GetValue(k).ToString, "[AUTO]")
   End If
  Next


  For Each k As String In lksu2.GetValueNames
   'Dim g As ListViewGroup = lvwProcs1.Groups(1)

   If Trim(k) <> "" Then
    Form1.Scan(lksu2.GetValue(k).ToString, "[AUTO]")
   End If
  Next

  For Each k As String In Hksu.GetValueNames
   If Trim(k) <> "" Then
    Form1.Scan(Hksu.GetValue(k).ToString, "[AUTO]")
   End If
  Next


  '==========
  For Each k As String In lksu3.GetValueNames
   If Trim(k) <> "" Then
    Form1.Scan(lksu3.GetValue(k).ToString, "[AUTO]")
   End If
  Next




  Exit Sub
200:
  ErrorLog("load_reg " & ErrorToString())
 End Sub
 Public Function perevodAscii(ByVal unicodeString As String) As String
  ' The encoding.
  perevodAscii = "None"
  If unicodeString = "" Then
   Exit Function
  End If
  Dim ascii As New ASCIIEncoding()
  ' Save positions of the special characters for later reference.
  Dim indexOfPi As Integer = unicodeString.IndexOf(ChrW(928))
  Dim indexOfSigma As Integer = unicodeString.IndexOf(ChrW(931))
  Dim encodedBytes As Byte() = ascii.GetBytes(unicodeString)
  Dim decodedString As String = ascii.GetString(encodedBytes)
  perevodAscii = decodedString

 End Function
 Public Sub check_lnk(ByVal nmFile As String)
  'проверять таргет линков
  Dim ext As String = IO.Path.GetExtension(nmFile)
  Dim p As String = Mid(nmFile, 1, (Len(nmFile) - Len(nmFile & ext) - 1))
  If ext.ToLower = ".lnk" Then
   Dim lnkt As String = getShortcutTarget(p, nmFile & ".lnk")
   Form1.Scan(lnkt, "[LNK]")
  End If
  Exit Sub
500:
  ErrorLog("check_lnk " & ErrorToString())
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
 '=============
 Public Sub LogINI(ByVal smess As String)
  'главная проца ведения лога
  On Error GoTo 100
  Dim path As String = Application.StartupPath & "\SETTINGS.INI"

  Dim sw As StreamWriter = File.AppendText(path)
  sw.WriteLine(smess)
  sw.Flush()
  sw.Close()
  Exit Sub
100:
  ErrorLog("LogINI " & ErrorToString())

 End Sub

End Module
