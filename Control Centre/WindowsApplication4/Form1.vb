Imports System.Security.Permissions
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.Threading
Imports System.Security
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Security.AccessControl

Public Class Form1
 Public GlobalErrorRegistry As Boolean
 Dim a1 As Long
 Dim a3 As Long
 Dim a2 As Long
 Dim f1 As Long
 Dim f2 As Long
 Dim a4 As Long
 Public VirReg As String
 Public key As String
 Private Declare Function LOCKFile Lib "kernel32" (ByVal hFile As Long, ByVal dwFileOffsetLow As Long, ByVal dwFileOffsetHigh As Long, ByVal nNumberOfBytesToLOCKLow As Long, ByVal nNumberOfBytesToLOCKHigh As Long) As Long
 Private Declare Function RemoveDirectory Lib "kernel32.dll" Alias "RemoveDirectoryA" (ByVal lpPathName As String) As Long
 Public me_top As Boolean = False
 Public scanReg As Boolean = False
 Dim state As New detection
 Dim DevicePresent As Boolean = False
 Dim str As String
 Dim info As IO.FileInfo
 Dim results As String
 Dim mal As String
 Dim ret As String
 Dim y As Boolean = False
 Dim MyLibrary As New MyLibrary.MyLib
 Dim BelUnpack As New BelUnpack.unpax
 Dim RegMon As New MonitorReg.Myreg
 Dim objCryptDES As New cript.clsDES
 Dim module2 As New oldModule1
 Dim WithEvents cReg As New cRegSearch
 Public iregCount As Long = 0
 Public KolwoREG As Long = 0
 ' Private lksu As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SoftWare\Microsoft\Windows\CurrentVersion\Run", True)
 Public result As Integer
 Public line As String
 Public hlpfile As String = Application.StartupPath & "\BelyashAV.chm" ' справка
 Public countProc As Integer
 Public flgState As Boolean = False
 Friend WithEvents lblSpeed As System.Windows.Forms.Label

 Public ArrProc() As String
 Public kolwo_exe As Integer
 Public kolwo_dll As Integer
 Private HASH As New Hashtable
 Private ProcessID As New List(Of Integer)
 Private InternetID As New List(Of Integer)
 Private SecondCount As Integer
 Public inCount As Long = 0
 Public inFound As Long = 0
 Public inCure As Long = 0
 Public inNonCure As Long = 0
 Public inDELETE As Long = 0
 Public inMove As Long = 0
 Public ireG As Long = 0
 Public ireGFound As Long = 0 'найдено в реестре
 Public ireGDelete As Long = 0 'удалено в реестре

 Declare Function MoveFileEx Lib "kernel32" Alias "MoveFileExA" (ByVal lpExistingFileName As String, ByVal lpNewFileName As String, ByVal dwFlags As Long) As Long
 Const MOVEFILE_DELAY_UNTIL_REBOOT = 4
 Private T As Boolean
 Public globalExclude As String

 Public stop_Scan As Boolean
 Declare Function GetInputState Lib "user32.dll" () As Long
 'Dim keysize As clsAESV2.KeySize
 Dim keysize As New cript.clsAESV2.KeySize
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
 Public fileTODel As String
 Public fileTONum As Integer
 Public inCounter As Long
 Public infound2 As Long
 Public inCured As Long
 Public FArchive As Integer
 Public Fhidden As Integer
 Public fNormal As Integer
 Public fRead As Integer
 Public mintlbluncheck As Integer
 Public chk_File As String
 Private mintCompressedFiles As Long = 0
 Private mintCompressedFolders As Long = 0
 Private mintCount As Long = 0
 Private mintErrors As Long = 0
 Private mintFBMax As Long = 0
 Private mintFBValue As Long = 0
 Private mintFolderCount As Long = 0
 Private mintHiddenFiles As Long = 0
 Private mintHiddenFolders As Long = 0
 Private mintIndex As Long = 0
 Private mintNormalFiles As Long = 0
 Private mintNormalFolders As Long = 0
 Private mintNumberFolders As Long = 0
 Private mintSystemFiles As Long = 0
 Private mintSystemFolders As Long = 0
 Private mintTotalFiles As Long = 0
 Private mintArchive As Long = 0
 Private mintEncr As Long = 0
 '------
 Private mintCompressedFiles2 As Long
 Private mintCompressedFolders2 As Long
 Private mintCount2 As Long
 Private mintErrors2 As Long
 Private mintFBMax2 As Long
 Private mintFBValue2 As Long
 Private mintFolderCount2 As Long
 Private mintHiddenFiles2 As Long
 Private mintHiddenFolders2 As Long
 Private mintIndex2 As Long
 Private mintNormalFiles2 As Long
 Private mintNormalFolders2 As Long
 Private mintNumberFolders2 As Long
 Private mintSystemFiles2 As Long
 Private mintSystemFolders2 As Long
 Private mintTotalFiles2 As Long
 Private mintArchive2 As Long
 Private mintEncr2 As Long
 Private Declare Function IsDebuggerPresent Lib "kernel32" () As Long
 Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
  'детект процессов и детект появления новых процессов...главная проца монитора
  '=======================
  'Разработчик-помни!!!
  'На более мощных машинах как правило запущенно много программ(процессов)-поэтому 
  'Timer1.Interval нужно увеличить,т.к. программа не будет успевать их все обрабатывать.
  ' в принципе эту фичу можно вывести в инишник (алгоритм соберет быстродействие и макс. кол-во процессов и отсюда сделаем оптимальный интервал таймера)
  ' но блядь нет "других" машин  для экспериментов ;(
  'Сделал изменяемое значение Interval, его получает проца SETtimerInterval
  'по умолчанию 50, но можно в инишнике изменить параметр Timer
  '======================
  On Error Resume Next
  Dim ITM As ListViewItem
  Dim proc() As System.Diagnostics.Process
  proc = System.Diagnostics.Process.GetProcesses
  Application.DoEvents()
  For i As Int32 = 0 To proc.Length - 1
   If Monitoring = False Then
    If Timer1.Enabled = True Then
     Timer1.Enabled = False
    End If
    Exit Sub
   End If
   'If GetInputState() <> 0 Then


   If Not ProcessID.Contains(proc(i).Id) Then
    proc(i).EnableRaisingEvents = True
    ProcessID.Add(proc(i).Id)
    'Get Owner Information 
    Dim sOwner As String = ""
    Dim sEnvironment As String = ""
    Dim dInfo As New DirectoryInfo(IO.Path.GetDirectoryName(proc(i).MainModule.FileName))
    Dim myOwner As Security.Principal.IdentityReference = dInfo.GetAccessControl.GetOwner(GetType(Security.Principal.NTAccount))
    sOwner = myOwner.ToString
    sEnvironment = Environment.OSVersion.ToString()
    'XP = 5.1 and WIN 2000 = 5.0
    If InStr(1, sEnvironment, "NT 5.1") > 0 Then
     sOwner = "CREATOR OWNER"
    Else
     sOwner = "Everyone"
    End If
    'Add the access control entry to the file. 
    AddDirectorySecurity(proc(i).ProcessName, sOwner, FileSystemRights.FullControl, AccessControlType.Allow)
    'chk_dll_newproc(proc(i).ProcessName)
    AddHandler proc(i).Exited, AddressOf ProcessExit
    'If GetInputState() <> 0 Then
    'не зависать...слушать ОСЬ и брать от нее комманды
    Application.DoEvents()
    ' End If
    ITM = New ListViewItem(New String() {proc(i).MainModule.FileName, proc(i).ProcessName, CStr(proc(i).Id), ""}) 'proc.StartTime.ToLongTimeString, ""})
    ListView1.Items.Add(ITM)
    a1 = 0
    a1 = (DateTime.Now.Millisecond)
    a3 = 0
    a3 = (DateTime.Now.Second)
    Dim my_md5 As String 'хеш
    Dim my_tmp_f As String 'имя процесса/модуля/файла
    Dim StartTime As New DateTime()
    Dim EndTime As New DateTime()
    If proc(i).MainWindowHandle.ToInt32 <> 0 Then
     MyLibrary.FormFunction.suspend_proc(proc(i).Id) 'усыпить
     'Stop
    End If
    StartTime = DateTime.Now
    txtScanning.Text = proc(i).MainModule.FileName
    txtScanning.Refresh()
    inCount = inCount + 1
    lblScaning.Text = inCount
    lblScaning.Refresh()
    getmyfileatrShield(proc(i).MainModule.FileName)
    If inCount > 100 And ListView1.Items.Count > 0 Then
     ListView1.Items.Clear()
    End If
    Dim fileDetail As IO.FileInfo
    fileDetail = My.Computer.FileSystem.GetFileInfo(proc(i).MainModule.FileName)
    lblSize.Text = fileDetail.Length
    lblSize.Refresh()
    ITM = New ListViewItem(New String() {proc(i).MainModule.FileName, proc(i).ProcessName, CStr(proc(i).Id), ""}) 'proc(i).StartTime.ToLongTimeString, ""})
    ListView1.Items.Add(ITM)

    ' my_md5 = MD5_Hash(proc(i).MainModule.FileName)
    my_md5 = MyLibrary.FormFunction.gtmd5(proc(i).MainModule.FileName)
    Dim tmpTime As Boolean = CBool(sGetINI(sINIFile, "Shield", "Time", "True"))
    If tmpTime = False Then
     If chkLogHS() = True Then
      LogPrint("Shield", proc(i).MainModule.FileName & "|" & my_md5)
     End If
    End If
    my_tmp_f = proc(i).MainModule.FileName
    If CStr(my_tmp_f.Substring(0, 4)) = "\??\" Then
     my_tmp_f = CStr(proc(i).MainModule.FileName).Remove(0, 4)
    End If
    If yes_vir(my_md5) And my_md5 <> "None" Then
     'MsgBox(virname)

     inFound = inFound + 1
     lblInfected.Text = inFound
     NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Suspicious file" & vbCrLf & "File :" & proc(i).ProcessName & vbCrLf & "Virus: " & Virname, ToolTipIcon.Info)
     ' Process.GetProcessById(proc.Id).Kill()
     MyLibrary.FormFunction.killEnyBody(CStr(proc(i).Id))
     If action_virus(my_tmp_f, Virname, "Shield") = False Then
      inNonCure = inNonCure + 1
      lblNOCured.Text = inNonCure
      NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Error cure" & vbCrLf & "File :" & proc(i).ProcessName & vbCrLf & "Virus :" & Virname, ToolTipIcon.Error)
      LogPrint("Shield", proc(i).ProcessName & "-infected (" & Virname & ")-Error cure(md5)")
      SecondActions(proc(i).ProcessName, Virname, "Shield")
     End If
     'get_mem_time(proc(i).MainModule.FileName, my_md5) 'время проверки 
     GoTo 200
    Else
     If chk_uzerBase(my_md5, proc(i).MainModule.FileName, proc(i).Id, "Shield") = True Then
      GoTo 200
     End If
    End If

    ' get_mem_time(proc(i).MainModule.FileName, my_md5) 'время проверки 
    '                chk_dll_newproc(proc(i).ProcessName)
    'пока что отключил,сильно медленно...попробывать в другом потоке проверять
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
    Dim answer1 As Long
    answer1 = 0
    answer1 = EndTime.Ticks() - StartTime.Ticks()
    'If chkLogHS() = True Then
    ' logprint ("Shield", fFile & "|" & my_md5)
    'End If
    Dim tmpTime1 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Time", "True"))
    If tmpTime1 = True Then
     If MyLibrary.FormFunction.check_upx_file(proc(i).MainModule.FileName) = True Then
      LogPrint("Shield", proc(i).MainModule.FileName & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & lblSize.Text & "[MEM]" & " |UPX)")
     Else
      LogPrint("Shield", proc(i).MainModule.FileName & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & lblSize.Text & "[MEM]" & ")")
     End If
    Else
     LogPrint("Shield", proc(i).MainModule.FileName & "-OK")
    End If
50:
    lblScore.Text = f2 & "." & f1 & "." & answer1
    lblScore.Refresh()
   End If

   If proc(i).MainWindowHandle.ToInt32 <> 0 Then
    MyLibrary.FormFunction.resume_proc(proc(i).Id) 'разбудить
   End If


200:


   'MsgBox(proc(i)(i).MainModule.FileName, MsgBoxStyle.ApplicationModal)
   ProcessID.Add(proc(i).Id)
  Next
 End Sub
 Sub get_mem_time(ByVal vrname As String, ByVal tmpmd5 As String)
  On Error GoTo 10
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
  Dim tmpTime As Boolean = CBool(sGetINI(sINIFile, "Shield", "Time", "True"))
  If tmpTime = True Then
   LogPrint("Shield", vrname & "|" & tmpmd5 & "(Time=" & f2 & "." & f1 & "|SZ=" & lblSize.Text & "[MEM])")
  End If
  Exit Sub
10:
  ErrorLog("form1.get_mem_time " & ErrorToString())

 End Sub

 Private Delegate Sub CheckListview(ByVal str As String)

 Private Sub SetExitTime(ByVal S As String)
  'процесс завершился в такое-то время
  Dim Arr() As String = Split(S, "|")
  Dim IdIndex As Integer = ProcessID.IndexOf(Integer.Parse(Arr(0)))

  If IdIndex >= 0 Then
   ListView1.Items(IdIndex).SubItems(3).Text = Arr(1)
   ListView1.Items(IdIndex).Remove()
  Else
   InternetID.RemoveAt(InternetID.IndexOf(Integer.Parse(Arr(0))))
  End If
 End Sub


 Private Sub ProcessExit(ByVal sender As Object, ByVal e As System.EventArgs)
  'процесс завершился в такое-то время
  On Error Resume Next
  Dim P As Process = DirectCast(sender, Process)
  Dim f2 As New FileIOPermission(FileIOPermissionAccess.AllAccess, P.ProcessName)
  Dim vremen As String = Trim(IO.Path.GetFullPath(P.ProcessName)) & ".exe"
  ' Me.Invoke(New CheckListview(AddressOf SetExitTime), vremen)
  'chk_dll_newproc(IO.Path.GetFullPath(P.ProcessName) & ".exe")
  ' MsgBox(IO.Path.GetFullPath(P.ProcessName) & ".exe")
  'Scan(Trim(IO.Path.GetFullPath(P.ProcessName)) & ".exe", "[MEM_EXIT]")
  Dim tmpTime As Boolean = CBool(sGetINI(sINIFile, "Shield", "Time", "True"))
  If tmpTime = True Then
   LogPrint("Shield", vremen & "- application closed at " & CStr(P.ExitTime.ToLongTimeString) & " [MEM_EXIT]")
  End If
  'check_close(vremen)
  Exit Sub
200:
  ErrorLog("ProcessExit " & ErrorToString())
 End Sub
 Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
  'монитор процессов
  startMon()
 End Sub
 Public Sub stopMon()
  Monitoring = False
  Button1.Text = "Start"
  Button1.Refresh()
  Label8.Text = "    Disabled"
  Label8.ImageKey = "uncheck.PNG"
  Label8.ForeColor = Color.Red
  NotifyIcon1.Text = "Belyash Shield OFF"

  'Timer_ico.Enabled = True
  ВключитьМониторингToolStripMenuItem.Text = "Enable Protection"
  LogPrint("Shield", Format(Now, "dd-MM-yyyy") & " " & Format(Now, "hh:mm:ss") & "--------------------Монитор ВЫКЛючен------------------")
  Timer1.Enabled = False
  RemAll()
  tmrMonitor.Enabled = False
  TextBox1.Text = ""
      Button1.Enabled = True
  Button3.Enabled = False
  Dim tmp_str As String
  tmp_str = "Scanning:" & CStr(inCount) & vbCrLf & "Infected:" & CStr(inFound) & vbCrLf & "Delete:" & CStr(inDELETE) & vbCrLf & "Move:" & CStr(inMove) & vbCrLf & "No cure:" & CStr(inNonCure) & vbCrLf & "Virus records:" & lblZap.Text
      NotifyIcon1.ShowBalloonTip(100, "Belyash Shield OFF", tmp_str, ToolTipIcon.Warning)
      NotifyIcon1.Icon = New System.Drawing.Icon(Application.StartupPath & "\n2.ico")
      Me.Icon = New System.Drawing.Icon(Application.StartupPath & "\n2.ico")
      Me.Refresh()
 End Sub
 Public Sub startMon()
  'включение/отключение мониторинга...главная процедура
  On Error Resume Next
  Button1.Enabled = False
  Button3.Enabled = True
  Button1.Refresh()
  Label8.ForeColor = Color.Blue
  Label8.Text = "    Enabled"
  Label8.ImageKey = "check.PNG"
  NotifyIcon1.Text = "Belyash Shield ON"
  NotifyIcon1.Icon = New System.Drawing.Icon(Application.StartupPath & "\belyash.ico")
  Me.Icon = New System.Drawing.Icon(Application.StartupPath & "\belyash.ico")
  Me.Refresh()
  chk_my_log()
  SETtimerInterval() 'получить интервал таймера
  logcomponentVersion("Shield")
  ВключитьМониторингToolStripMenuItem.Text = "Disable Protection"
  ' Timer_ico.Enabled = False
  Monitoring = True
  ListView1.Items.Clear()
  Dim hist As Boolean = CBool(sGetINI(sINIFile, "Shield", "DelIEHistory", "True"))
  If hist = True Then 'удалить IE History
   If MyLibrary.FormFunction.dl_history = True Then
    NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "IE History" & vbCrLf & "Action: Delete", ToolTipIcon.Info)
    LogPrint("Shield", "IE History deleted")
   Else
    NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "IE History" & vbCrLf & "Action: Error delete", ToolTipIcon.Error)
    LogPrint("Shield", "IE History no deleted-error")
   End If
  End If
  Timer1.Enabled = True
  ' chk_reg_funct() 'проверка реестра
  AUto_run_Thismashine()
  IsItSafe()
  getAutorun()
  populateList()
  For i As Integer = Asc("a"c) To Asc("z"c)
   If IO.Directory.Exists(Chr(i) & ":\") Then add(Chr(i) & ":\", True)
  Next
  '================================================
  Dim mpmem As Boolean = CBool(sGetINI(sINIFile, "Shield", "Memory", "True"))
  If mpmem = True Then
   GetAllProcesses() 'временно выключено...НЕ ЗАБЫТЬ ВКЛЮЧИТЬ !!!
   'не забыть реализовать полноценный механиз проверки повторного вхождения библиотеки в массив
   '---------------------------------------------
  End If
 End Sub

 Sub chk_autoruns_my()
  'проверить автозапуск в реестр
  On Error GoTo 100
  Const userRoot1 As String = "HKEY_CURRENT_USER"
  Const subkey1 As String = "SOFTWARE\Belyash\Monitor"
  Dim keyName1 As String = userRoot1 & "\" & subkey1
  Dim keyName2 As String = Registry.GetValue(keyName1, _
       "AutoZapusk", "True")
  If keyName2 = "False" Then
   Exit Sub
  End If
  'autorun1()
  Exit Sub
100:
  ErrorLog("form1.chk_autoruns_my " & ErrorToString())
 End Sub
 Public Sub chk_my_log()
  On Error GoTo 200
  Dim path As String = "otchetMon.log"

  Dim keyName2 As String = CBool(sGetINI(sINIFile, "Shield", "Log", "True"))
  If keyName2 = False Then
   Exit Sub
  End If
  Dim keyName3 As String = CBool(sGetINI(sINIFile, "Shield", "Append", "True"))
  If keyName3 = False Then
   If File.Exists(path) = True Then
    File.Delete(path)
   End If
  End If
  Exit Sub
200:
  ErrorLog("form1.chk_my_log " & ErrorToString())
 End Sub
 Sub GetAllProcesses3()
  Dim allProcesses(), thisProcess As Process
  allProcesses = System.Diagnostics.Process.GetProcesses
  If ListView1.Items.Count > 0 Then
   ListView1.Items.Clear()
  End If
  For Each thisProcess In allProcesses
   'If GetInputState() <> 0 Then
   Application.DoEvents()
   ' End If
   If Monitoring = True Then
    Exit Sub
   End If
   Try
    Dim thisModule As ProcessModule
    For Each thisModule In thisProcess.Modules
     If GetInputState() <> 0 Then
      Application.DoEvents()
     End If
     If Monitoring = True Then
      Exit Sub
     End If
     With thisModule
      'lvItem.SubItems.Add(.ModuleName)
      If .FileName <> "" And .ModuleName <> "" Then
       'lvItem.SubItems.Add(.FileName)
       Dim a As String
       a = .FileName.Substring(Len(.FileName) - 3)
       'MsgBox(a)
       If a <> "exe" Then
        Scan(.FileName, "[MEM]")
       End If
      End If
     End With
     'Exit For

    Next
   Catch ee As Exception
    'Some Exception may occure here. You can check it
    'Like Thread is in Sleep mode
    ErrorLog("Getallprocess " & ErrorToString())
   End Try
  Next
 End Sub

 Sub GetAllProcesses()
  LogPrint("Shield", "getallpr-----------------------------------")
  Dim allProcesses(), thisProcess As Process
  Dim tmpModulename As String = ""
  Dim tmpProcname As String = ""
  allProcesses = System.Diagnostics.Process.GetProcesses
  If ListView1.Items.Count > 0 Then
   ListView1.Items.Clear()
  End If
  Application.DoEvents()
  For Each thisProcess In allProcesses
   '  If GetInputState() <> 0 Then

   '  End If
   Try
    If Monitoring = False Then
     Exit Sub
    End If
    tmpProcname = thisProcess.ProcessName
    Dim thisModule As ProcessModule
    For Each thisModule In thisProcess.Modules
     If Monitoring = False Then
      Exit Sub
     End If

     With thisModule
      If .FileName <> "" Or .ModuleName <> "" Then
       ' My.Application.DoEvents()
       ' If .FileName.Substring(0, 4) = "\??\" And Len(.FileName) <> 0 Then
       'Scan(.FileName.Remove(0, 4))
       'Else
       tmpModulename = .ModuleName
       Dim a As String
       a = .FileName.Substring(Len(.FileName) - 3)
       If a <> "exe" Then
        ' logprint ("Shield", .ModuleName)
        'Scan(IO.Path.GetFullPath(.FileName), "[MEM]")
        chk_dllNow(.FileName)
       End If
       'Scan(.FileName)
      End If
      'End If
     End With
     'Exit For

    Next

   Catch ee As Exception
    'Some Exception may occure here. You can check it
    'Like Thread is in Sleep mode
    ErrorLog("GetAllProcesses " & ErrorToString() & " " & tmpModulename & "-" & tmpProcname)

   End Try
  Next
  'Timer1.Enabled = True
 End Sub
 Private Sub Fast_check()
  Dim allProcesses(), thisProcess As Process
  Dim temp_exe As Integer
  allProcesses = System.Diagnostics.Process.GetProcesses
  If ListView1.Items.Count > 0 Then
   ListView1.Items.Clear()
  End If
  For Each thisProcess In allProcesses
   Try
    Dim thisModule As ProcessModule
    ' Dim lv As ListView = CType(sender, ListView)
    ' Dim lvItem As ListViewItem
    'lvItem = lvProcesses.Items.Add(thisProcess.Id)
    'lvItem.SubItems.Add(thisProcess.ProcessName)
    For Each thisModule In thisProcess.Modules
     With thisModule
      'lvItem.SubItems.Add(.ModuleName)
      If .FileName <> "" Or .ModuleName <> "" Then
       'lvItem.SubItems.Add(.FileName)
       Dim a As String
       a = .FileName.Substring(Len(.FileName) - 3)

       If a = "exe" Then
        ' My.Application.DoEvents()

       Else
        '  My.Application.DoEvents()

       End If
      End If
     End With
     'Exit For

    Next

   Catch ee As Exception
    'Some Exception may occure here. You can check it
    'Like Thread is in Sleep mode
    ErrorLog("Fast_check " & ErrorToString())

   End Try
   temp_exe = temp_exe + 1
  Next
  If temp_exe <> kolwo_exe Then
   kolwo_exe = temp_exe
  End If
 End Sub
 Public Sub RemAll()
  'прекратить следить за файловой активностью
  On Error GoTo 10
  For Each w As IO.FileSystemWatcher In HASH.Values
   w.Dispose()
  Next
  HASH.Clear()
  W_List.Items.Clear()
  Exit Sub
10:
  ErrorLog("RemAll " & ErrorToString())
 End Sub
 Public Sub add(ByVal Path As String, Optional ByVal includeSubDirectory As Boolean = True)
  If IO.Directory.Exists(Path) Then
   If includeSubDirectory = False Then
    a(Path)
   Else
    a(Path, includeSubDirectory)
   End If
  End If
 End Sub
 Public Sub a(ByVal path As String, Optional ByVal b As Boolean = False)
  '  If GetInputState() <> 0 Then
  Application.DoEvents()
  '  End If
  Dim it As New ListViewItem(path)
  W_List.Items.Add(it)
  Dim w As New System.IO.FileSystemWatcher(path)
  w.IncludeSubdirectories = b
  w.EnableRaisingEvents = True
  AddHandler w.Changed, AddressOf change
  AddHandler w.Created, AddressOf create
  AddHandler w.Deleted, AddressOf delete
  AddHandler w.Renamed, AddressOf rename
  HASH.Add(it, w)
 End Sub
 Public Sub delete(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs)
  status("[" & e.ChangeType.ToString() & "]" & e.FullPath)
 End Sub
 Public Sub create(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs)
  On Error GoTo 100
  status("[" & e.ChangeType.ToString() & "]" & e.FullPath)
  If Exclude_pats(IO.Path.GetDirectoryName(e.FullPath)) = True Or exclude_sys_critical(IO.Path.GetDirectoryName(e.FullPath)) = True Then
   Exit Sub
  End If
  If UCase(IO.Path.GetDirectoryName(e.FullPath)) = UCase(Application.StartupPath & "\quarantine") Then
   Exit Sub
  End If
  If chk_name(IO.Path.GetFileName(e.FullPath), IO.Path.GetExtension(e.FullPath)) = False Then
   Scan(e.FullPath, "[CR]")
  End If
  If LCase(IO.Path.GetFileName(e.FullPath)) = LCase("autorun.inf") Then
   If File.Exists(e.FullPath) = True Then
    podchistka()
   End If
  End If
  Exit Sub
100:
  ErrorLog("create " & ErrorToString())
 End Sub
 Public Sub change(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs)
  On Error GoTo 200
  'MsgBox(IO.Path.GetDirectoryName(e.FullPath))
  If Exclude_pats(IO.Path.GetDirectoryName(e.FullPath)) = True Or exclude_sys_critical(IO.Path.GetDirectoryName(e.FullPath)) = True Then
   Exit Sub
  End If
  If UCase(IO.Path.GetDirectoryName(e.FullPath)) = UCase(Application.StartupPath & "\quarantine") Then
   Exit Sub
  End If
  If chk_name(IO.Path.GetFileName(e.FullPath), IO.Path.GetExtension(e.FullPath)) = False Then
   status("[" & e.ChangeType.ToString() & "]" & e.FullPath)
   Scan(e.FullPath, "[CH]")
  End If
  If LCase(IO.Path.GetFileName(e.FullPath)) = LCase("autorun.inf") Then
   If File.Exists(e.FullPath) = True Then
    podchistka()
   End If
  End If
  Exit Sub
200:
  ErrorLog("change " & ErrorToString())
 End Sub
 Public Sub rename(ByVal sender As Object, ByVal e As System.IO.RenamedEventArgs)
  On Error GoTo 100
  status("[" & e.ChangeType.ToString & "]" & e.OldName & " [Change to] " & e.Name & " in " & e.FullPath)
  If Exclude_pats(IO.Path.GetDirectoryName(e.FullPath)) = True Or exclude_sys_critical(IO.Path.GetDirectoryName(e.FullPath)) = True Then
   Exit Sub
  End If
  If UCase(IO.Path.GetDirectoryName(e.FullPath)) = UCase(Application.StartupPath & "\quarantine") Then
   Exit Sub
  End If
  If chk_name(IO.Path.GetFileName(e.FullPath), IO.Path.GetExtension(e.FullPath)) = False Then
   Scan(e.OldName, "[RN]")
   Scan(e.FullPath, "[RN]")
  End If
  If LCase(IO.Path.GetFileName(e.FullPath)) = LCase("autorun.inf") Then
   If File.Exists(e.FullPath) = True Then
    podchistka()
   End If
  End If
  Exit Sub
100:
  ErrorLog("rename " & ErrorToString())

 End Sub
 Function chk_name(ByVal g As String, ByVal hExten As String) As Boolean
  chk_name = False
  If g <> "" Then
   Select Case LCase(g)
    Case LCase("otchetMon.log")
     chk_name = True
     Exit Function
    Case LCase("ntuser.dat.LOG")
     chk_name = True
     Exit Function
    Case LCase("software.LOG")
     chk_name = True
     Exit Function
    Case LCase("change.log")
     chk_name = True
     Exit Function
    Case LCase("system.LOG")
     chk_name = True
     Exit Function
    Case LCase("errorlog.LOG")
     chk_name = True
     Exit Function
    Case LCase("otchetScan.log")
     chk_name = True
     Exit Function
    Case LCase("ERRORLOG.LOG")
     chk_name = True
     Exit Function
   End Select
  End If
  ' If hExten <> "" Then
  'Select Case hExten
  '   Case "log"
  'chk_name = True
  'Exit Function
  '   Case "txt"
  'chk_name = True
  'Exit Function
  'End Select
  'End If
 End Function
 Private Sub status(ByVal text As String)
  On Error GoTo 100
  Control.CheckForIllegalCrossThreadCalls = False
  TextBox1.AppendText(text & vbCrLf)
  TextBox1.ScrollToCaret()
  If TextBox1.TextLength >= 300 Then
   TextBox1.Text = ""
  End If
  Dim keyName2 As Boolean = CBool(sGetINI(sINIFile, "Shield", "LogActiviti", "False"))
  If keyName2 = True Then
   LogPrint("Shield", text)
  End If
  Exit Sub
100:
  ErrorLog("status " & ErrorToString())

 End Sub
 Private Sub RestoreWindow()
  Me.Show()
  NotifyIcon1.Visible = True
  Me.WindowState = FormWindowState.Normal
  Me.Focus()
 End Sub
 Private Sub NotifyIcon1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
  RestoreWindow()
 End Sub
 'Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
 'On Error Resume Next
 'If tmrMonitor.Enabled = True Then
 'tmrMonitor.Enabled = False
 'writeINI(sINIFile, "Shield", "Monitoring", "True")
 'End If
 'If Timer_ico.Enabled = True Then
 'Timer_ico.Enabled = False
 'End If
 'If Sbor.Enabled = True Then
 'Sbor.Enabled = False
 'Sbor.Dispose()
 'End If
 'If Timer2.Enabled = True Then
 'Timer2.Enabled = False
 'Timer2.Dispose()
 'End If
 'If tmrDevice.Enabled = True Then
 '  tmrDevice.Enabled = False
 '   tmrDevice.Dispose()
 'End If
 'If Timer1.Enabled = True Then
 '  Timer1.Enabled = False
 '   Timer1.Dispose()
 'End If

 'Me.Dispose()
 'Application.Exit()
 'End Sub
 Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
  If e.KeyCode = Keys.F1 Then
   Help.ShowHelp(Me, hlpfile) 'вызов справки
  End If
 End Sub
 Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  On Error Resume Next
  If IsDebuggerPresent <> 0 Then
   End
  End If
  ' Timer3.Enabled = True
  'Me.Text = "Belyash Anti-Trojan 2009b v." & Application.ProductVersion

  ' NotifyIcon1.Icon = Me.Icon
  Label13.Text = "Version: " & Application.ProductVersion
  chk_first_ST9()
  Label40.Text = "The Belyash Anti-Trojan 2009b v." & Application.ProductVersion & " consists of components that ensure full protection for your computer."
  With ListViewProg.Columns
   .Add("Name", 110, HorizontalAlignment.Right)
   .Add("Version", 100, HorizontalAlignment.Right)
   .Add("Description", 260, HorizontalAlignment.Left)

  End With
  With ListViewFind.Columns
   .Add("Virus", 180, HorizontalAlignment.Right)
   .Add("Path", 210, HorizontalAlignment.Right)
   .Add("Actions", 90, HorizontalAlignment.Left)
   .Add("Component", 110, HorizontalAlignment.Left)
  End With
  With REGFound.Columns
   .Add("Key", 150, HorizontalAlignment.Right)
   .Add("Root", 210, HorizontalAlignment.Right)
   .Add("Path", 150, HorizontalAlignment.Left)
   .Add("Value", 150, HorizontalAlignment.Left)
   .Add("Descriptions", 150, HorizontalAlignment.Left)
  End With
  ComboBox1.Items.Add("Resident Shield")
  ComboBox1.Items.Add("Virus Vault")
  ComboBox1.Items.Add("Scanner")
  ComboBox1.Items.Add("Firewall")
  ComboBox1.Items.Add("Update")
  ComboBox1.Items.Add("Registry")
  ComboBox1.Items.Add("Error")
  ListViewQ.SmallImageList = Icons16
  ListViewQ.LargeImageList = Icons32
  ListViewQ.Columns.Add("Name", 180, HorizontalAlignment.Left)
  ListViewQ.Columns.Add("Type", 60, HorizontalAlignment.Left)
  ListViewQ.Columns.Add("Date", 150, HorizontalAlignment.Left)
  ListViewQ.Columns.Add("Attribute", 70, HorizontalAlignment.Left)
  ListViewQ.Columns.Add("Description", 150, HorizontalAlignment.Left)
  SETtimerInterval() 'получить значение таймера
  Monitoring = False
  NotifyIcon1.Icon = New System.Drawing.Icon(Application.StartupPath & "\n2.ico")
  get_kolwovirus()
  licensed_read()
  mi_prior()
  gt_formaStatus()
  chk_zwuk()
  HelpProvider1.HelpNamespace = Application.StartupPath & "\BelyashAV.chm"
  gt_fon()
  chk_mon()
  chk_Blocker()
  Engine()
  check_registeredAppl()
  check_lastScan()
  check_lastUpdate()
  nastrVault()
  lblNormalFiles.Text = "0"
  lblCompressedFiles.Text = "0"
  lblSystemFiles.Text = "0"
  lblHidden.Text = "0"
  lblmintArchive.Text = "0"
  lblmintEncr.Text = "0"
  lblTotalFiles.Text = "0"
  lblSizef.Text = "0"
  lblSpeed.Text = "0"
  lbluncheck.Text = "0"
  chk_autozaxist()
  If DevicePresent = True Then
   str = state.x.Name
   'exes.Items.Clear()
  End If

 End Sub
 Sub chk_Blocker()
  Try
         Dim rZ As Boolean = CBool(sGetINI(sINIFile, "Blocker", "Enable", "False"))
   If rZ = True Then
    EnableBlockerToolStripMenuItem.Checked = True
    reg_blocker()

   Else
    EnableBlockerToolStripMenuItem.Checked = False
    UNreg_blocker()

   End If
  Catch ex As Exception
   ErrorLog("chk_Blocker " & ErrorToString())
  End Try
 End Sub
 Public Sub SETtimerInterval()
  Dim rZ As Integer = CInt(sGetINI(sINIFile, "Shield", "Timer", "50"))
  Timer1.Interval = rZ
 End Sub
 Private Sub nastrVault()
  'настройки карантина
  Try

   txtVaultExt.Text = sGetINI(sINIFile, "Vault", "Extension", "#??")
   Dim gh1111 As Boolean = CBool(sGetINI(sINIFile, "Vault", "Cript", "False"))
   If gh1111 = True Then
    chkVaultCript.Checked = True
   Else
    chkVaultExt.Checked = True
   End If

   Dim gh2 As Boolean = CBool(sGetINI(sINIFile, "Registry", "Backup", "False"))
   If gh2 = True Then
    chkBackup.Checked = True
   Else
    chkBackup.Checked = True
   End If
   Dim gh3 As Boolean = CBool(sGetINI(sINIFile, "Registry", "CureAuto", "False"))
   If gh3 = True Then
    chkFixReg.Checked = True
   Else
    chkFixReg.Checked = True
   End If

  Catch ex As Exception
   ErrorLog("nastrVault " & ErrorToString())
  End Try
 End Sub
 Private Sub check_lastUpdate()
  On Error Resume Next
  Dim name5 As String = sGetINI(sINIFile, "Update", "LastUpdate", "Unknown")
  Label52.Text = "Last Update: " & name5
  Label10.Text = "Last Update: " & name5
  Dim rmp As String = sGetINI(sINIFile, "USER", "LicExpirid", "Unknown")
  If rmp <> "Unknown" Then
   Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
   Dim a1 As String = a.Decrypt(rmp)
   Label12.Text = "License Expired: " & a1

   Dim b As Date = CDate(a1)
   If DateDiff("d", Now, b) = 0 Or DateDiff("d", Now, b) < 0 Then
    myRegister = False
    GlassBox.ShowMessage("Invalid license", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
    'MsgBox("Invalid license", MsgBoxStyle.Critical)
   Else
    myRegister = True
   End If
  Else
   Label12.Text = "License Expired: Unknown"
  End If

 End Sub

 Private Sub check_lastScan()
  Dim name5 As String = sGetINI(sINIFile, "Scanner", "LastScan", "Unknown")
  LabellstScan.Text = "Last Scan: " & name5
  Dim name3 As String = sGetINI(sINIFile, "Scanner", "Action", "REPORT")
  Select Case name3
   Case "MOVE"
    RadioButton6.Checked = True
   Case "DELETE"
    RadioButton7.Checked = True
   Case "LOCK"
    RadioButton5.Checked = True
   Case Else
    RadioButton8.Checked = True

  End Select
  Dim name31 As String = sGetINI(sINIFile, "Shield", "Action", "REPORT")
  Select Case name31
   Case "MOVE"
    RadioButton3.Checked = True
   Case "DELETE"
    RadioButton2.Checked = True
   Case "LOCK"
    RadioButton4.Checked = True
   Case "REPORT"
    RadioButton1.Checked = True
   Case Else
    RadioButton1.Checked = True
  End Select
  CheckBox3.Checked = CBool(sGetINI(sINIFile, "Scanner", "Ask", "True"))
  CheckBox2.Checked = CBool(sGetINI(sINIFile, "Shield", "Zapros", "True"))
 End Sub
 Sub check_registeredAppl()
  If SL.isRegistered = True Then
   Dim licendUs As String = sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName)
   If Trim(licendUs).Length > 23 Then
    Label17.Text = "Licensed To [" & sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName).Substring(0, 23) & "]"
    Label68.Text = sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName).Substring(0, 23)
   Else
    Label17.Text = "Licensed To " & sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName)
    Label68.Text = sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName)
   End If
   Label70.Text = sGetINI(sINIFile, "USER", "SN", "")
   LinkLabel7.Enabled = False
   Label17.BackColor = Color.Yellow
  Else
   Label17.Text = "Software not registered."
   Label17.BackColor = Color.Red
   Label68.Text = UCase$(MyLibrary.FormFunction.GetUserName)
   Label70.Text = "0000000000000"
  End If
 End Sub
 Sub licensed_read()
  Try
   Dim line As String
   Dim readFile As System.IO.TextReader = New  _
StreamReader(Application.StartupPath & "\copying.txt")
   line = readFile.ReadToEnd()
   lblLicense.Text = line
   readFile.Close()
   readFile = Nothing
  Catch ex As IOException
   ErrorLog(ex.ToString)
  End Try
 End Sub
 Public Sub mi_prior()
  On Error GoTo 101
  MenuItem1.Checked = False
  MenuItem2.Checked = False
  MenuItem3.Checked = False
  MenuItem4.Checked = False
  Dim Mi_status1 As String
  Dim ht As String = sGetINI(sINIFile, "Shield", "Priority", "Hight")
  Select Case ht
   Case "RealTime"
    MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "RealTime")
    MenuItem1.Checked = True
   Case "Hight"
    MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Hight")
    MenuItem2.Checked = True
   Case "Idle"
    MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Idle")
    MenuItem4.Checked = True
   Case Else
    MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Normal")
    MenuItem3.Checked = True
  End Select
  Exit Sub
101:
  ErrorLog("mi_prior " & ErrorToString())
 End Sub
 Sub chk_mon()
  On Error GoTo 101
  ChkMastLoad.Checked = CBool(sGetINI(sINIFile, "Shield", "Run", "True"))
  Dim Mi_status1 As Boolean
  Mi_status1 = CBool(sGetINI(sINIFile, "Shield", "Status", "True"))
  If Mi_status1 = True Then
   Dim Mi_status As Boolean
   Mi_status = CBool(sGetINI(sINIFile, "Shield", "Monitoring", "True"))
   If Mi_status = "True" Then
    getAutorun()
    populateList()
    startMon()
    NotifyIcon1.BalloonTipText = "Belyash Shield ON"
    NotifyIcon1.Text = "Belyash Shield ON"
    NotifyIcon1.Icon = New System.Drawing.Icon(Application.StartupPath & "\belyash.ico")
    Label8.ForeColor = Color.Blue
    Label8.Text = "    Enabled"
    Label8.ImageKey = "check.PNG"
   Else
    NotifyIcon1.BalloonTipText = "Belyash Shield OFF"
    NotifyIcon1.Text = "Belyash Shield OFF"
    'Timer_ico.Enabled = True
    NotifyIcon1.Icon = New System.Drawing.Icon(Application.StartupPath & "\n2.ico")
    Label8.ImageKey = "uncheck.PNG"
    NotifyIcon1.Text = "Belyash Shield OFF"
    Label8.ForeColor = Color.Red
   End If
  End If
  Exit Sub
101:
  ErrorLog("chk_mon " & ErrorToString())

 End Sub
 Sub gt_formaStatus()
  'запускать свернутым....желательно
  On Error GoTo 101
  Dim tmpForm As Boolean = CBool(sGetINI(sINIFile, "Shield", "Fon", "True"))
  If tmpForm <> False Then
   StartMinimizedToolStripMenuItem.Checked = True
   Me.Hide()
  Else
   StartMinimizedToolStripMenuItem.Checked = False
  End If
  Exit Sub
101:
  ErrorLog("gt_formaStatus " & ErrorToString())

  Me.Hide()

 End Sub

 Sub gt_fon()
  'загружаться в фоновом режиме и сразу в трей
  On Error Resume Next
  Dim s1 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Fon", "True"))
  If s1 = True Then
   Me.WindowState = FormWindowState.Minimized
  Else
   Me.WindowState = FormWindowState.Normal
  End If
 End Sub
 Sub chk_zwuk()
  'загрузка..проверка...звук будет или нет
  On Error GoTo 10
  Dim s1 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Sound", "True"))
  If s1 = False Then
   ToolStripButton13.Text = "Sound OFF"
   My.Computer.Audio.Stop()
   ToolStripButton13.Image = ImageList1.Images.Item(1)
  Else
   ToolStripButton13.Text = "Sound ON"
   sound_me("3")
   ToolStripButton13.Image = ImageList1.Images.Item(0)
  End If
  Exit Sub
10:
  ErrorLog("chk_zwuk " & ErrorToString())

 End Sub
 Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
  If Me.WindowState = FormWindowState.Minimized Then
   If me_top = True Then
    If GlassBox.ShowMessage("Enabled options form on top. Continue?", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Question, MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel Then
     'If MsgBox("Enabled options form on top. Continue?", MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
     RestoreWindow()
     Exit Sub
    Else
     sn_top()
    End If
   Else
    Me.Height = 554
    Me.Width = 777

   End If

   NotifyIcon1.Visible = True
   If Me.WindowState <> FormWindowState.Minimized Then
    Me.WindowState = FormWindowState.Minimized
   End If
  End If
 End Sub
 Protected Overrides Sub OnClosing(ByVal e As CancelEventArgs)
  MyBase.OnClosing(e)
  On Error Resume Next
  If Monitoring = True Then
   writeINI(sINIFile, "Shield", "Status", "True")
  Else
   writeINI(sINIFile, "Shield", "Status", "False")
  End If
  If GlassBox.ShowMessage("Turn OFF protection and close application?", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Question, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
   'If MessageBox.Show("Turn OFF protection and close application?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
   If Me.WindowState <> FormWindowState.Minimized Then
    Me.WindowState = FormWindowState.Minimized
   End If
   e.Cancel = True
   Exit Sub
  End If
  sound_me("exit")

  MyLibrary = Nothing
  Monitoring = False
 End Sub
 Function action_virus(ByVal fl_name As String, ByVal st As String, ByVal Component1 As String) As Boolean
  action_virus = False
  On Error GoTo 100
  If myRegister = False Then
   sound_me("lic")
   GlassBox.ShowMessage("Invalid license", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   'MsgBox("Invalid license", MsgBoxStyle.Critical)
   Dim li As New ListViewItem(st, 5)
   li.SubItems.Add(fl_name)
   li.SubItems.Add("Error cure.Invalid license")
   li.SubItems.Add(Component1)
   ListViewFind.Items.Add(li)
   Exit Function
  End If
  If Trim(fl_name) = "" Then
   Exit Function
  End If
  If Not File.Exists(fl_name) Then
   Exit Function
  End If
  Dim keyName2 As String = sGetINI(sINIFile, "Shield", "Action", "REPORT")
  Dim keyName3 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Zapros", "False"))

  If keyName3 = True Then
   If zapros_na_deystw(keyName2) = False Then
    Exit Function
   End If
  End If

  Select Case keyName2
   Case "REPORT"
    LogPrint(Component1, fl_name & "-infected " & Virname & "(REPORT)")
    inNonCure = inNonCure + 1
    lblNOCured.Text = inNonCure
    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(fl_name)
    li.SubItems.Add("REPORT")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Found infected file" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus:" & Virname & vbCrLf & "Action:REPORT", ToolTipIcon.Info)
    action_virus = True
    Exit Function
   Case "MOVE"

    LogPrint(Component1, fl_name & "-infected " & Virname & "(Move to quarantine)")
    If File.Exists(Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name)) = True Then
     File.Delete(Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name))
    End If

    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(fl_name)
    li.SubItems.Add("Move")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    Dim tmpcr As Boolean = CBool(sGetINI(sINIFile, "Vault", "Cript", "False"))
    If tmpcr = True Then
     vaultCript(fl_name)
    Else
     If File.Exists(Application.StartupPath & "\quarantine\" & IO.Path.GetFileName(fl_name) & "." & Trim(txtVaultExt.Text)) = True Then
      File.Delete(Application.StartupPath & "\quarantine\" & IO.Path.GetFileName(fl_name) & "." & Trim(txtVaultExt.Text))
     End If
     Dim tmp_r As String = Application.StartupPath & "\quarantine\" & IO.Path.GetFileName(fl_name) & "." & Trim(txtVaultExt.Text)
     My.Computer.FileSystem.MoveFile(fl_name, tmp_r, True)
     'MyLibrary.FormFunction.ren_ME(fl_name, tmp_r)
    End If
    inMove = inMove + 1
    lblMoved.Text = inMove
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Infected file move" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus:" & Virname & vbCrLf & "Action:Move", ToolTipIcon.Info)
    LogQuarant(fl_name)
    action_virus = True
    Exit Function

   Case "DELETE"
    'удалить
    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(fl_name)
    li.SubItems.Add("DELETE")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    LogPrint(Component1, fl_name & "-infected " & Virname & "(DELETE)")
    File.Delete(fl_name)
    inDELETE = inDELETE + 1
    lblDELETE.Text = inDELETE
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Infected file is delete" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus :" & Virname & vbCrLf & "Action:DELETE", ToolTipIcon.Info)
    action_virus = True
    Exit Function
   Case "LOCK"
    'заблокировать файл

    LogPrint(Component1, fl_name & "-infected " & Virname & "(LOCK)")
    inNonCure = inNonCure + 1
    lblNOCured.Text = inNonCure
    If MyLibrary.FormFunction.lock_file(fl_name) = False Then
     Dim s3 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
     Exit Function
    End If
    Dim s2 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Infected file locked" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus :" & Virname & vbCrLf & "Action:LOCK", ToolTipIcon.Info)
    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(fl_name)
    li.SubItems.Add("LOCK")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    action_virus = True
    Exit Function
  End Select
  Exit Function
100:
  ErrorLog("action_virus " & ErrorToString())
  ' MsgBox(ErrorToString)

 End Function
 Function zapros_na_deystw(ByVal k2 As String) As Boolean
  zapros_na_deystw = False
  Select Case k2
   Case "REPORT"
    zapros_na_deystw = True
    Exit Function
   Case "MOVE"
    If GlassBox.ShowMessage("Do you really want move file ?", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Question, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
     'If MsgBox("Do you really want move file ?", MsgBoxStyle.YesNo + MsgBoxStyle.ApplicationModal + MsgBoxStyle.Question + MsgBoxStyle.ApplicationModal + MsgBoxStyle.MsgBoxSetForeground, "Внимание") = MsgBoxResult.Yes Then
     zapros_na_deystw = True
     Exit Function
    End If
   Case "DELETE"
    If GlassBox.ShowMessage("Do you really want delete file ?", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Question, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
     'If MsgBox("Do you really want delete file ?", MsgBoxStyle.YesNo + MsgBoxStyle.ApplicationModal + MsgBoxStyle.Question + MsgBoxStyle.ApplicationModal + MsgBoxStyle.MsgBoxSetForeground, "Внимание") = MsgBoxResult.Yes Then
     zapros_na_deystw = True
     Exit Function
    End If
   Case "LOCK"
    If GlassBox.ShowMessage("Do you really want lock file ?", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Question, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
     'If MsgBox("Do you really want lock file ?", MsgBoxStyle.YesNo + MsgBoxStyle.ApplicationModal + MsgBoxStyle.Question + MsgBoxStyle.ApplicationModal + MsgBoxStyle.MsgBoxSetForeground, "Внимание") = MsgBoxResult.Yes Then
     zapros_na_deystw = True
     Exit Function
    End If
  End Select
 End Function
 Function action_virus_CRC(ByVal fl_name As String, ByVal st As String, ByVal Component1 As String) As Boolean
  action_virus_CRC = False
  On Error GoTo 100
  If Not File.Exists(fl_name) Then
   Exit Function
  End If
  If myRegister = False Then
   sound_me("lic")
   GlassBox.ShowMessage("Invalid license", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   'MsgBox("Invalid license", MsgBoxStyle.Critical)
   Dim li As New ListViewItem(st, 5)
   li.SubItems.Add(fl_name)
   li.SubItems.Add("Error cure.Invalid license")
   li.SubItems.Add(Component1)
   ListViewFind.Items.Add(li)
   Exit Function
  End If
  Dim keyName2 As String = sGetINI(sINIFile, "Shield", "ActionUnknow", "REPORT")
  Dim keyName3 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Zapros", "False"))
  If keyName3 = True Then
   If zapros_na_deystw(keyName2) = False Then
    Exit Function
   End If
  End If
  Select Case keyName2
   Case "REPORT"
    'только отчет
    LogPrint(Component1, fl_name & "-infected " & Virname & "(Ignore)")
    inNonCure = inNonCure + 1
    lblNOCured.Text = inNonCure
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Suspicious file" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus :" & st & vbCrLf & "Action:REPORT", ToolTipIcon.Info)
    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(fl_name)
    li.SubItems.Add("REPORT")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    action_virus_CRC = True
    Exit Function
   Case "MOVE"
    'переместить в карантин
    LogPrint(Component1, fl_name & "-infected " & st & "(Move to quarantine)")
    If File.Exists(Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name)) = True Then
     File.Delete(Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name))
    End If
    My.Computer.FileSystem.MoveFile(fl_name, _
Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name), True)
    inMove = inMove + 1
    lblMoved.Text = inMove
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Suspicious file" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus :" & st & vbCrLf & "Action:Move", ToolTipIcon.Info)
    LogQuarant(fl_name)
    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(fl_name)
    li.SubItems.Add("Move")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    action_virus_CRC = True
    Exit Function
   Case "DELETE"
    'удалить
    LogPrint(Component1, fl_name & "-infected " & st & "(DELETE)")
    File.Delete(fl_name)
    inDELETE = inDELETE + 1
    lblDELETE.Text = inDELETE
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Suspicious file" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus :" & st & vbCrLf & "Action :DELETE", ToolTipIcon.Info)
    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(fl_name)
    li.SubItems.Add("DELETE")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    action_virus_CRC = True
    Exit Function
   Case "LOCK"
    'блокировать
    LogPrint(Component1, fl_name & "-infected " & st & "(LOCK)")
    If MyLibrary.FormFunction.lock_file(fl_name) = False Then
     Dim s4 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
     Exit Function
    End If
    Dim s2 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(fl_name)
    li.SubItems.Add("LOCK")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Suspicious file" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus :" & st & vbCrLf & "Action :LOCK", ToolTipIcon.Info)
    action_virus_CRC = True
    Exit Function
  End Select
  Exit Function
100:
  ErrorLog("action_virus_CRC " & ErrorToString())

 End Function
 Public Sub Scan(ByVal fFile As String, ByVal prichina As String)
  'тупо-сканирование
  On Error GoTo 101
  'Dim myPerms As PermissionSet = New PermissionSet(PermissionState.None)
  ' myPerms.AddPermission(New FileIOPermission _
  '  (FileIOPermissionAccess.AllAccess, IO.Path.GetDirectoryName(fFile)))
  ' myPerms.AddPermission(New FileIOPermission _
  '   (FileIOPermissionAccess.Write, IO.Path.GetDirectoryName(fFile)))
  If Monitoring = False Then
   Exit Sub
  End If
  Dim my_md5 As String
  If Trim(fFile) = "" Then
   Exit Sub
  End If
  If File.Exists(fFile) = False Then
   Exit Sub
  End If
  If Exclude_pats(IO.Path.GetDirectoryName(fFile)) = True Or exclude_sys_critical(fFile) = True Then
   Exit Sub
  End If
  If gt_extension_zip(IO.Path.GetExtension(fFile)) = True Then
   Dim tmp_dir As String = IO.Path.GetFileNameWithoutExtension(fFile)
   Dim s As String = My.Application.GetEnvironmentVariable("TEMP") & "\" & tmp_dir
   '  Stop
   globalExclude = s
   unpackFiles(fFile, prichina, "Shield")
   GoTo 50
  End If
  Dim extens As Boolean = CBool(sGetINI(sINIFile, "Shield", "Extentions", "True"))
  If extens = True Then
   'проверка на расширение
   If chkExtention_File(IO.Path.GetExtension(fFile)) = False Then
    Exit Sub
   End If
  End If
  Dim StartTime As New DateTime()
  Dim EndTime As New DateTime()
  StartTime = DateTime.Now()
  txtScanning.Text = fFile
  txtScanning.Refresh()
  inCount = inCount + 1
  lblScaning.Text = inCount
  lblScaning.Refresh()
  getmyfileatrShield(fFile)
  Dim fileDetail As IO.FileInfo
  fileDetail = My.Computer.FileSystem.GetFileInfo(fFile)
  lblSize.Text = fileDetail.Length
  lblSize.Refresh()
  If CLng(fileDetail.Length) < 0.1 Or CLng(fileDetail.Length) = 0 Then
   LogPrint("Shield", fFile & " - exclude ,zero lenght")
   Exit Sub
  End If
  Dim r1 As Boolean = CBool(sGetINI(sINIFile, "Shield", "NoCheckLenght", "False"))
  Dim r2 As Integer = CLng(sGetINI(sINIFile, "Shield", "CheckLenght", "15142784"))
  If r1 = True Then
   If CLng(lblSize.Text) > 0 And CLng(lblSize.Text) > r2 Then
    LogPrint("Shield", fFile & " - exclude (" & lblSize.Text & ")")
    Exit Sub
   End If
  End If
  a1 = 0
  a1 = (DateTime.Now.Millisecond)
  a3 = 0
  a3 = (DateTime.Now.Second)
  my_md5 = MD5_Hash(fFile)
  'my_md5 = MyLibrary.FormFunction.gtmd5(Trim(fFile))

  If yes_vir(my_md5) And my_md5 <> "None" Then
   'MsgBox(virname)
   inFound = inFound + 1
   lblInfected.Text = inFound
   LogPrint("Shield", fFile & " -инфицирован" & Virname)
   ' NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Обнаружено вредоносное ПО" & vbCrLf & vbCrLf & IO.Path.GetFileName(fFile) & vbCrLf & Virname, ToolTipIcon.Info)

   If action_virus(fFile, Virname, "Shield") = False Then
    inNonCure = inNonCure + 1
    lblNOCured.Text = inNonCure
    NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Error cure" & vbCrLf & "File :" & IO.Path.GetFileName(fFile) & vbCrLf & "Virus :" & Virname, ToolTipIcon.Error)
    LogPrint("Shield", "Error cure " & fFile & "-(" & Virname & ")")
    SecondActions(fFile, Virname, "Shield")
   End If
   GoTo 100
  Else
   If chk_uzerBase_NoPids(my_md5, fFile, "Shield") = True Then
    GoTo 100
   Else
    main_heur(fFile, "Shield") 'эвристика
   End If
  End If
  If chk_userbase_string(fFile) = True Then
   LogPrint("Shield", fFile & "-string in user base")
   ' NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Инфицирован" & vbCrLf & vbCrLf & IO.Path.GetFileName(fFile) & vbCrLf & "Пользовательская запись", ToolTipIcon.Info)
   If action_virus_CRC(fFile, "User record", "Shield") = False Then
    inNonCure = inNonCure + 1
    lblNOCured.Text = inNonCure
    NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Error cure" & vbCrLf & "File :" & IO.Path.GetFileName(fFile) & vbCrLf & "User record", ToolTipIcon.Info)
    LogPrint("Shield", "Error cure " & fFile & "-(user record)")
    SecondActions(fFile, Virname, "Shield")
   End If
   GoTo 100
  End If
  'my_md5
  LogPrint("Shield", fFile & "-OK")
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
  ' logprint ("Shield", fFile & "|" & my_md5)
  'End If
  Dim tmpTime As Boolean = CBool(sGetINI(sINIFile, "Shield", "Time", "True"))
  If tmpTime = True Then
   If MyLibrary.FormFunction.check_upx_file(fFile) = True Then
    LogPrint("Shield", fFile & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & lblSize.Text & prichina & " |UPX)")
   Else
    LogPrint("Shield", fFile & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & lblSize.Text & prichina & ")")
   End If

  End If
50:
  lblScore.Text = f2 & "." & f1 & "." & answer1
  lblScore.Refresh()
  Exit Sub
101:
  ErrorLog("Scan " & fFile & vbCrLf & ErrorToString())
  Resume Next
 End Sub

 Private Function gt_extension_zip(ByVal ag As String) As Boolean
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
 Public Sub unpackFiles(ByVal zipetfile As String, ByVal pr1 As String, ByVal Component1 As String)
  On Error GoTo 100
  Dim keyName13 As Boolean = CBool(sGetINI(sINIFile, Component1, _
    "CheckZip", "True"))

  If keyName13 = False Then
   Exit Sub
  End If
  Dim tmp_dir As String = IO.Path.GetFileNameWithoutExtension(zipetfile)
  Dim s As String = My.Application.GetEnvironmentVariable("TEMP") & "\" & tmp_dir
  Application.DoEvents()
  If BelUnpack.FormUnpack.Show_ZipContents3(zipetfile, s) = True Then
   ' If GetInputState() <> 0 Then

   'End If
   LogPrint(Component1, zipetfile & "-unpack ОК")
   If IO.Directory.Exists(s) = True Then

    If ScanFull(s, pr1, Component1, zipetfile) = True Then
     LogPrint(Component1, s & "-archive infected")
     If action_virus_zip(zipetfile, Virname, Component1) = False Then
      inNonCure = inNonCure + 1
      lblNOCured.Text = inNonCure
      NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Error cure" & vbCrLf & "File :" & IO.Path.GetFileName(zipetfile) & vbCrLf & "Virus :" & Virname, ToolTipIcon.Error)
      LogPrint(Component1, "Error cure " & zipetfile & "-(" & Virname & ")")
      SecondActions(zipetfile, Virname, "Shield")
     End If
    End If
    Directory.Delete(s, True)
    ' If Directory.Exists(s) = True Then
    'del_temp_folder(s)
    'Debug.Print(s & "-не удалился каталог")
    'End If
   End If
  End If



  Exit Sub
100:
  ErrorLog("unpackFiles " & ErrorToString())

 End Sub
 Sub del_temp_folder(ByVal zpath As String)
  'удалить временный каталог со всеми файлами
  On Error Resume Next
  Dim myDirectory As DirectoryInfo
  myDirectory = New DirectoryInfo(zpath)
  Dim aFile As FileInfo
  For Each aFile In myDirectory.GetFiles
   aFile.Delete()
  Next

  IO.Directory.Delete(zpath)

 End Sub
 Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
  Nastroyka.ShowDialog()
 End Sub

 Private Sub ToolStripButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton13.Click
  On Error GoTo 10
  If ToolStripButton13.Text = "Sound ON" Then
   ToolStripButton13.Text = "Sound OFF"
   My.Computer.Audio.Stop()
   ToolStripButton13.Image = ImageList1.Images.Item(1)
   writeINI(sINIFile, "Shield", "Sound", "False")
  Else
   ToolStripButton13.Text = "Sound ON"
   ToolStripButton13.Image = ImageList1.Images.Item(0)
   writeINI(sINIFile, "Shield", "Sound", "True")
   sound_me("3")
  End If
  Exit Sub
10:

  ErrorLog("ToolStripButton13_Click " & ErrorToString())
 End Sub

 Sub look_quar()
  On Error GoTo 10
  'карантин
  If Directory.Exists(Application.StartupPath & "\quarantine") = True Then
   'Process.Start("explorer", Application.StartupPath & "\quarantine")
   sn_top()

   TabControl1.SelectTab(6)
  End If
  Exit Sub
10:

  ErrorLog("look_quar " & ErrorToString())
 End Sub


 Private Sub ToolStripButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton9.Click

  look_quar()
 End Sub

 Private Sub КарантинToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles КарантинToolStripMenuItem.Click

  look_quar()
  RestoreWindow()
 End Sub


 Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
  sn_top()
  Help.ShowHelp(Me, hlpfile) 'вызов справки
 End Sub

 Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
  sn_top()
  NwZapis.ShowDialog() 'внести новую запись в базы
 End Sub

 Private Sub ДобавитьЗаписьToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ДобавитьЗаписьToolStripMenuItem.Click
  sn_top()
  NwZapis.ShowDialog()
 End Sub
 Public Function chk_uzerBase(ByVal vMD5 As String, ByVal vFile As String, ByVal pids As Long, ByVal Component1 As String) As Boolean
  On Error GoTo 100
  If File.Exists(Application.StartupPath & "\uzerbase.txt") = False Then
   Exit Function
  End If
  chk_uzerBase = False
  ' If GetInputState() <> 0 Then
  Application.DoEvents()
  ' End If
  If yes_vir_UZER(vMD5) = True Then
   Process.GetProcessById(pids).Kill()
   chk_uzerBase = True
   If action_virus_CRC(vFile, Virname, Component1) = False Then
    'MoveFileEx(tmp1, "", MOVEFILE_DELAY_UNTIL_REBOOT)
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Error cure" & vbCrLf & "Файл:" & vFile & vbCrLf & "Вирус:" & Virname, ToolTipIcon.Error)
    LogPrint(Component1, vFile & "(" & Virname & ")-Error cure(user md5)")
    SecondActions(vFile, Virname, Component1)
   End If
  End If
  Exit Function
100:
  ErrorLog("chk_uzerBase " & ErrorToString())

 End Function
 Public Function chk_uzerBase_NoPids(ByVal vMD5 As String, ByVal vFile As String, ByVal Component1 As String) As Boolean
  chk_uzerBase_NoPids = False
  On Error GoTo 100

  If File.Exists(Application.StartupPath & "\uzerbase.txt") = False Then
   Exit Function
  End If
  If yes_vir_UZER(vMD5) = True Then
   chk_uzerBase_NoPids = True
   If action_virus_CRC(vFile, Virname, Component1) = False Then
    'MoveFileEx(tmp1, "", MOVEFILE_DELAY_UNTIL_REBOOT)
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Error cure" & vbCrLf & "Файл:" & vFile & vbCrLf & "Вирус:" & Virname, ToolTipIcon.Error)
    LogPrint(Component1, vFile & "(" & Virname & ")-Error cure(user md5)")
   End If
  End If
  Exit Function
100:
  ErrorLog("chk_uzerBase_NoPids " & ErrorToString())

 End Function


 Private Sub СканерToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles СканерToolStripMenuItem.Click
  startscanner()
  RestoreWindow()
 End Sub
 Sub startscanner()
  'показ сканера
  TabControl1.SelectTab(2)

 End Sub
 Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
  TabControl1.SelectTab(2)
  logcomponentVersion("Scanner")
  starting_scan() 'запустить сканер
 End Sub
 Private Sub logcomponentVersion(ByVal component As String)
  Try

   LogPrint(component, "-------------------------------------------------------------------------------")
   If File.Exists(Application.StartupPath & "\Monitor.exe") = True Then
    LogPrint(component, "Main Module=" & System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.StartupPath & "\Monitor.exe").FileVersion.ToString())
   End If
   If File.Exists(Application.StartupPath & "\Blocker.exe") = True Then
    LogPrint(component, "Blocker=" & System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.StartupPath & "\Blocker.exe").FileVersion.ToString())
   End If
   If File.Exists(Application.StartupPath & "\MyLibraryBase.dll") = True Then
    LogPrint(component, "Engine=" & System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.StartupPath & "\MyLibraryBase.dll").FileVersion.ToString())
   End If
   If File.Exists(Application.StartupPath & "\unpack.dll") = True Then
    LogPrint(component, "Unpack Library=" & System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.StartupPath & "\unpack.dll").FileVersion.ToString())
   End If
   If File.Exists(Application.StartupPath & "\registrymon.dll") = True Then
    LogPrint(component, "Protecrion Module=" & System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.StartupPath & "\registrymon.dll").FileVersion.ToString())
   End If
   If File.Exists(Application.StartupPath & "\cript.dll") = True Then
    LogPrint(component, "Belyash Cryptography Library=" & System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.StartupPath & "\registrymon.dll").FileVersion.ToString())
   End If
   LogPrint(component, "Bases=" & lblZap.Text)
   LogPrint(component, Format(Now, "dd-MM-yyyy") & " " & Format(Now, "hh:mm:ss") & " Shield Enabled")
   LogPrint(component, "-------------------------------------------------------------------------------")

  Catch ex As Exception
   'ErrorLog("logcomponentVersion " & ErrorToString())
  End Try

 End Sub

 '======================================================
 Public Sub chk_reg_funct()
  'вызвать функцию проверки реестра
  Try


   'Dim rk As Microsoft.Win32.RegistryKey
   'rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options")
   'PrintKeys(rk)
   'rk.Close()
   zapretZapuckaProg()
   chk_reestr2() 'проверить реестр
   chk_reestr3()

  Catch ex As Exception
   ErrorLog("chk_reg_funct " & ErrorToString())
  End Try


 End Sub
 Public Sub PrintKeys(ByVal rkey As RegistryKey)
  ' Retrieve all the subkeys for the specified key.
  On Error GoTo 10
  Dim names As String() = rkey.GetSubKeyNames()
  Dim icount As Integer = 0
  Dim s As String
  For Each s In names
   If scanReg = False Then
    Exit Sub
   End If
   ireG = ireG + 1
   Label80.Text = ireG
   txtRegistry.Text = "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & s
   chk_debug(s)
   icount += 1
  Next s
  Exit Sub
10:
  ErrorLog("PrintKeys " & ErrorToString())

 End Sub
 Public Sub chk_debug(ByVal s1 As String)
  'проверить блокировку в реестре для приложений
  On Error GoTo 100
  'SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\





  If yes_vir_registry(s1) = True Then
   'My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & s1)
   'My.Computer.Registry.LocalMachine.Close()
   LogPrint("Registry", "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & s1 & "-DELETE")
   'notify_chkreestr(s1)
   ireGFound = ireGFound + 1
   txtRegFound.Text = ireGFound
   Dim li As New ListViewItem(s1, 5)
   li.SubItems.Add("HKEY_LOCAL_MACHINE")
   li.SubItems.Add("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & s1)
   li.SubItems.Add("No Execute this programm")
   REGFound.Items.Add(li)
  End If
  Exit Sub
100:

  ErrorLog("chk_debug " & ErrorToString())
 End Sub

 Sub chk_reestr3()
  On Error GoTo 101
  Const userRoot1 As String = "HKEY_LOCAL_MACHINE"
  Const subkey1 As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\policies\WinOldApp"
  Const keyName1 As String = userRoot1 & "\" & subkey1
  Dim tExpand3 As String = Registry.GetValue(keyName1, _
       "Disabled", "0")
  If LCase(tExpand3) <> LCase("0") And LCase(tExpand3) <> LCase("00000000") Then
   Registry.SetValue(keyName1, _
    "Disabled", "0")
   LogPrint("Shield", userRoot1 & "\" & subkey1 & "," & keyName1 & "-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "WinOldApp", ToolTipIcon.Info)
  End If

  Const userRoot As String = "HKEY_LOCAL_MACHINE"
  Const subkey As String = "Software\Microsoft\Windows\CurrentVersion\Policies\System"
  Const keyName As String = userRoot & "\" & subkey
  Dim tExpand As String = Registry.GetValue(keyName, _
       "DisableRegistryTools", 0)
  If LCase(tExpand) <> LCase("0") And LCase(tExpand) <> LCase("00000000") Then
   Registry.SetValue(keyName, _
    "DisableRegistryTools", 0, RegistryValueKind.DWord)
   LogPrint("Shield", userRoot & "\" & subkey & ",DisableRegistryTools-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "DisableRegistryTools", ToolTipIcon.Info)
  End If
  Dim tExpand2 As String = Registry.GetValue(keyName, _
"DisableTaskMgr", 0)
  If LCase(tExpand2) <> LCase("0") And LCase(tExpand2) <> LCase("00000000") Then
   Registry.SetValue(keyName, _
    "DisableTaskMgr", 0, RegistryValueKind.DWord)
   LogPrint("Shield", userRoot & "\" & subkey & "," & "DisableTaskMgr-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "DisableTaskMgr", ToolTipIcon.Info)
  End If
  Dim tExpand5 As String = Registry.GetValue(keyName, _
"DisableRegedit", 0)
  If LCase(tExpand5) <> LCase("0") And LCase(tExpand5) <> LCase("00000000") Then
   Registry.SetValue(keyName, _
    "DisableRegedit", "0", RegistryValueKind.DWord)
   LogPrint("Shield", userRoot & "\" & subkey & "," & "DisableRegedit-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "DisableRegedit", ToolTipIcon.Info)
  End If
  '===============

  Const userRoot8 As String = "HKEY_CURRENT_USER"
  Const subkey8 As String = "Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"
  Const keyName8 As String = userRoot8 & "\" & subkey8
  Dim tExpand8 As String = Registry.GetValue(keyName8, _
       "restrictrun", 0)
  If LCase(tExpand8) <> LCase("0") And LCase(tExpand8) <> LCase("00000000") Then
   Registry.SetValue(keyName8, _
    "restrictrun", 0, RegistryValueKind.DWord)
   LogPrint("Shield", userRoot & "\" & subkey & "," & "restrictrun-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "restrictrun", ToolTipIcon.Info)
  End If
  '=======================
  Const userRoot9 As String = "HKEY_CURRENT_USER"
  Const subkey9 As String = "Software\Microsoft\Windows\CurrentVersion\Policies\System"
  Const keyName9 As String = userRoot9 & "\" & subkey9
  Dim tExpand9 As String = Registry.GetValue(keyName9, _
       "DisableTaskMgr", 0)
  If LCase(tExpand9) <> LCase("0") And LCase(tExpand9) <> LCase("00000000") Then
   ' Registry.SetValue(keyName9, _
   ' "DisableTaskMgr", "0")
   Dim regSave As Microsoft.Win32.RegistryKey
   regSave = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\System")
   'Dim newVersion As String = "test" ' параметры создаваемого ключа
   regSave.SetValue("DisableTaskMgr", 0, RegistryValueKind.DWord) ' записываешь(имя ключа, параметры)
   regSave.Close()
   LogPrint("Shield", userRoot & "\" & subkey & "," & "DisableTaskMgr-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "DisableTaskMgr", ToolTipIcon.Info)
  End If
  '====================
  Const userRoot10 As String = "HKEY_LOCAL_MACHINE"
  Const subkey10 As String = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"
  Const keyName10 As String = userRoot10 & "\" & subkey10
  Dim tExpand10 As Double = My.Computer.Registry.GetValue(keyName10, _
       "DisableSR", RegistryValueKind.DWord)
  If tExpand10 <> 0 Then
   ' Registry.SetValue(keyName9, _
   ' "DisableTaskMgr", "0")
   Dim regSave As Microsoft.Win32.RegistryKey
   regSave = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore")
   'Dim newVersion As String = "test" ' параметры создаваемого ключа
   regSave.SetValue("DisableSR", 0, RegistryValueKind.DWord) ' записываешь(имя ключа, параметры)
   regSave.Close()
   LogPrint("Shield", userRoot & "\" & subkey & "," & "DisableSR-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "DisableSR", ToolTipIcon.Info)
  End If
  Exit Sub
101:
  ErrorLog("chk_reestr3 " & ErrorToString())

 End Sub
 Sub chk_reestr2()
  On Error GoTo 101
  Const userRoot As String = "HKEY_LOCAL_MACHINE"
  Const subkey As String = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"
  Const keyName As String = userRoot & "\" & subkey
  Dim tExpand As String = Registry.GetValue(keyName, _
       "Shell", "hello")
  If LCase(tExpand) <> LCase("Explorer.exe") And LCase(tExpand) <> LCase(My.Application.GetEnvironmentVariable("windir") & "\explorer.exe") Then
   Registry.SetValue(keyName, _
    "Shell", "Explorer.exe")
   LogPrint("Shield", userRoot & "\" & subkey & "," & "Explorer.exe-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "Explorer.exe", ToolTipIcon.Info)
  End If

  '----------------
  Dim tExpand1 As String = Registry.GetValue(keyName, _
"Userinit", "hello")
  If LCase(tExpand1) <> LCase("userinit.exe") And LCase(tExpand1) <> LCase(My.Application.GetEnvironmentVariable("windir") & "\system32\userinit.exe") Then
   Registry.SetValue(keyName, _
    "Userinit", "userinit.exe")
   LogPrint("Shield", userRoot & "\" & subkey & "," & "userinit.exe-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "userinit.exe", ToolTipIcon.Info)
  End If
  Dim tExpand2 As String = Registry.GetValue(keyName, _
"UIHost", "hello")
  If LCase(tExpand2) <> LCase("logonui.exe") Then
   Registry.SetValue(keyName, _
    "UIHost", "logonui.exe")
   LogPrint("Shield", userRoot & "\" & subkey & "," & "logonui.exe-Fixed")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Registry fixed" & vbCrLf & "logonui.exe", ToolTipIcon.Info)
  End If

  '   My.Computer.Registry.CurrentUser.DELETESubKeyTree("Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2")
  '  My.Computer.Registry.CurrentUser.Close()
  ' logprint ("Shield", "HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2-Fixed")
  'NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Исправлен реестр" & vbCrLf & "MountPoints2", ToolTipIcon.Info)
  'My.Computer.Registry.Users.DELETESubKeyTree(".DEFAULT\Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2")
  'My.Computer.Registry.Users.Close()
  'logprint ("Shield", "HKEY_USERS\Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2-Fixed")
  'NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Исправлен реестр" & vbCrLf & "MountPoints2", ToolTipIcon.Info)
  Exit Sub
101:
  'MsgBox(ErrorToString, MsgBoxStyle.Critical, "Проверка реестра-chk_reestr2")
  ErrorLog("chk_reestr2 " & ErrorToString())

  Resume Next
 End Sub
 Function Exclude_pats(ByVal tmpPath As String) As Boolean
  'пользовательские исключения
  On Error GoTo 100
  Exclude_pats = False

  If LCase(tmpPath) = LCase(globalExclude) Or MyLibrary.FormFunction.get_shortpt(tmpPath) = MyLibrary.FormFunction.get_shortpt(globalExclude) Then
   Exclude_pats = True
   Exit Function
  End If
  'MyLibrary.FormFunction.get_shortpt
  Dim tmpint As Integer = CInt(sGetINI(sINIFile, "Shield", "Count_Exclude", "0"))
  If tmpint = 0 Then
   Exit Function
  End If
  Dim i As Integer
  For i = 0 To tmpint

   Dim tExpand3 As String = sGetINI(sINIFile, "Exclude_Shield", i, "0")
   sGetINI(sINIFile, "Exclude_Shield", i, "0")
   If tExpand3 <> "0" And tExpand3 <> "" Then
    ' MsgBox("совпало")
    If UCase(Trim(tExpand3)) = UCase(Trim(tmpPath)) Then
     LogPrint("Shield", tExpand3 & "-folder exclude")
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
  Dim r1 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Evristic", "True"))
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
  Dim r1 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Autorun", "True"))
  If r1 = False Then
   Exit Sub
  End If

  If File.Exists(Application.StartupPath & "\quarantine\autorun.inf" & IO.Path.GetFileName(f1)) = True Then
   File.Delete(Application.StartupPath & "\quarantine\autorun.inf" & Trim(IO.Path.GetFileName(f1)))
  End If

  If LCase(IO.Path.GetFileName(f1)) = LCase("autorun.inf") Then
   System.IO.File.Move(Trim(f1), Application.StartupPath & "\quarantine\" & IO.Path.GetFileName(f1))

   inMove = inMove + 1
   lblMoved.Text = inMove
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Suspicious file" & vbCrLf & "AutoRun.inf" & vbCrLf & "Action:Move", ToolTipIcon.Info)
   LogQuarant(f1)
   LogPrint("Shield", f1 & "-infected AutoRun.inf -Move to quarantine")
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
     lblInfected.Text = inFound
     LogPrint("Shield", fileName3 & " -Suspicious Script.Mail")
     NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Suspicious file" & vbCrLf & "File :" & IO.Path.GetFileName(fileName3) & vbCrLf & "Modifications Script.Mail", ToolTipIcon.Info)
    Else
     inFound = inFound + 1
     lblInfected.Text = inFound
     LogPrint("Shield", fileName3 & " -Suspicious Script.Mail")
     NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Suspicious file" & vbCrLf & "File :" & IO.Path.GetFileName(fileName3) & vbCrLf & "Modifications Script.Virus", ToolTipIcon.Info)
    End If
    If action_virus_CRC(fileName3, "Suspicious Script.Virus", Component1) = False Then
     inNonCure = inNonCure + 1
     lblNOCured.Text = inNonCure
     NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Error cure" & vbCrLf & "File :" & IO.Path.GetFileName(fileName3) & vbCrLf & "Modifications Script.Virus", ToolTipIcon.Error)
     LogPrint("Shield", "Error cure " & fileName3 & "-(Suspicious Script.Virus)")
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
    Application.DoEvents()
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
    lblInfected.Text = inFound
    LogPrint("Shield", fileName & " -Modification Win32.Virus")
    NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Suspicious file" & vbCrLf & "File :" & IO.Path.GetFileName(fileName) & vbCrLf & "Modification Win32.Virus", ToolTipIcon.Info)
    If action_virus_CRC(fileName, "Modification Win32.Virus", Component1) = False Then
     inNonCure = inNonCure + 1
     lblNOCured.Text = inNonCure
     NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Error cure" & vbCrLf & "File :" & IO.Path.GetFileName(fileName) & vbCrLf & "Modification Win32.Virus", ToolTipIcon.Error)
     LogPrint("Shield", "Error cure " & fileName & "-(modification Win32.Virus)")
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

 '============================================================
 Sub chk_first()
  'проверить первое вхождение библиотеки
  On Error GoTo 100
  Dim n As Integer
  Dim s As String = My.Application.GetEnvironmentVariable("windir") & "\system32"
  Dim a(106) As String
  a(0) = s & "\xpsp2res.dll"
  a(1) = s & "\appevent.evt"
  a(2) = s & "\apphelp.dll"
  a(3) = s & "\authz.dll"
  a(4) = s & "\ctype.nls"
  a(5) = s & "\eventlog.dll"
  a(6) = s & "\gdi32.dll"
  a(7) = s & "\kernel32.dll"
  a(8) = s & "\locale.nls"
  a(9) = s & "\msvcp60.dll"
  a(10) = s & "\msvcrt.dll"
  a(11) = s & "\ncobjapi.dll"
  a(12) = s & "\ntdll.dll"
  a(13) = s & "\psapi.dll"
  a(14) = s & "\scesrv.dll"
  a(15) = s & "\secevent.evt"
  a(16) = s & "\secur32.dll"
  a(17) = s & "\services.exe"
  a(18) = s & "\sorttbls.nls"
  a(19) = s & "\sysevent.evt"
  a(20) = s & "\tuneup.evt"
  a(21) = s & "\umpnpmgr.dll"
  a(22) = s & "\unicode.nls"
  a(23) = s & "\userenv.dll"
  a(24) = s & "\version.dll"
  a(25) = s & "\winsta.dll"
  a(26) = s & "\ws2_32.dll"
  a(27) = s & "\ws2help.dll"
  a(28) = s & "\advapi32.dll"
  a(29) = s & "\atl.dll"
  a(30) = s & "\batmeter.dll"
  a(31) = s & "\browseui.dll"
  a(32) = s & "\c_1251.nls"
  a(33) = s & "\clbcatq.dll"
  a(34) = s & "\comctl32.dll"
  a(35) = s & "\comres.dll"
  a(36) = s & "\credui.dll"
  a(37) = s & "\crypt32.dll"
  a(38) = s & "\cscdll.dll	"
  a(39) = s & "\cscui.dll	"
  a(40) = s & "\davclnt.dll"
  a(41) = s & "\dot3api.dll"
  a(42) = s & "\dot3dlg.dll"
  a(43) = s & "\drprov.dll"
  a(44) = s & "\eappcfg.dll"
  a(45) = s & "\eappprxy.dll"
  a(46) = s & "\explorer.exe"
  a(47) = s & "\imagehlp.dll"
  a(48) = s & "\index.dat"
  a(49) = s & "\iphlpapi.dll"
  a(50) = s & "\linkinfo.dll"
  a(51) = s & "\midimap.dll"
  a(52) = s & "\mlang.dll"
  a(53) = s & "\mpr.dll"
  a(54) = s & "\msacm32.dll"
  a(55) = s & "\msacm32.drv"
  a(56) = s & "\msasn1.dll"
  a(57) = s & "\msctf.dll"
  a(58) = s & "\msi.dll"
  a(59) = s & "\msimg32.dll"
  a(60) = s & "\msutb.dll"
  a(61) = s & "\msvcp60.dll"
  a(62) = s & "\msvcrt.dll"
  a(63) = s & "\netapi32.dll"
  a(64) = s & "\netrap.dll"
  a(65) = s & "\netshell.dll"
  a(66) = s & "\netui0.dll"
  a(67) = s & "\netui1.dll"
  a(68) = s & "\ntlanman.dll"
  a(69) = s & "\ntshrui.dll"
  a(70) = s & "\ole32.dll"
  a(71) = s & "\oleaut32.dll"
  a(72) = s & "\onex.dll"
  a(73) = s & "\powrprof.dll"
  a(74) = s & "\rpcrt4.dll"
  a(75) = s & "\rsaenh.dll"
  a(76) = s & "\rtutils.dll"
  a(77) = s & "\samlib.dll"
  a(78) = s & "\setupapi.dll"
  a(79) = s & "\shdoclc.dll"
  a(80) = s & "\shdocvw.dll"
  a(81) = s & "\shell32.dll"
  a(82) = s & "\shlwapi.dll"
  a(83) = s & "\sortkey.nls"
  a(84) = s & "\stobject.dll"
  a(85) = s & "\sxs.dll"
  a(86) = s & "\themeui.dll"
  a(87) = s & "\urlmon.dll"
  a(88) = s & "\user32.dll"
  a(89) = s & "\uxtheme.dll"
  a(90) = s & "\wdmaud.drv"
  a(91) = s & "\webcheck.dll"
  a(92) = s & "\wininet.dll"
  a(93) = s & "\winmm.dll"
  a(94) = s & "\wintrust.dll"
  a(95) = s & "\wldap32.dll"
  a(96) = s & "\wsock32.dll"
  a(97) = s & "\wtsapi32.dll"
  a(98) = s & "\MSVBVM60.DLL"
  a(99) = s & "\riched20.dll"
  a(100) = s & "\nview.dll"
  a(101) = s & "\AVIFIL32.dll"
  a(102) = s & "\MSVFW32.dll"
  a(103) = s & "\pjlmon.dll"
  a(104) = s & "\kerberos.dll"
  a(105) = s & "\localspl.dll"
  a(106) = s & "\w32time.dll"




  For n = 0 To 106
   '   If GetInputState() <> 0 Then
   Application.DoEvents()
   '   End If
   If Monitoring = False Then
    Exit Sub
   End If
   Application.DoEvents()
   If File.Exists(a(n)) Then
    '====================
    Scan(a(n), "[MEM]")
    '=====================
   End If
  Next

  Exit Sub
100:

  ErrorLog("chk_first " & ErrorToString())
  'Resume Next
 End Sub

 Public Sub chk_dllNow(ByVal a1 As String)
  'проверить первое вхождение библиотеки
  On Error GoTo 100
  Dim n As Integer
  Dim s As String = My.Application.GetEnvironmentVariable("windir") & "\system32"
  Dim a(106) As String

  a(0) = s & "\xpsp2res.dll"
  a(1) = s & "\appevent.evt"
  a(2) = s & "\apphelp.dll"
  a(3) = s & "\authz.dll"
  a(4) = s & "\ctype.nls"
  a(5) = s & "\eventlog.dll"
  a(6) = s & "\gdi32.dll"
  a(7) = s & "\kernel32.dll"
  a(8) = s & "\locale.nls"
  a(9) = s & "\msvcp60.dll"
  a(10) = s & "\msvcrt.dll"
  a(11) = s & "\ncobjapi.dll"
  a(12) = s & "\ntdll.dll"
  a(13) = s & "\psapi.dll"
  a(14) = s & "\scesrv.dll"
  a(15) = s & "\secevent.evt"
  a(16) = s & "\secur32.dll"
  a(17) = s & "\services.exe"
  a(18) = s & "\sorttbls.nls"
  a(19) = s & "\sysevent.evt"
  a(20) = s & "\tuneup.evt"
  a(21) = s & "\umpnpmgr.dll"
  a(22) = s & "\unicode.nls"
  a(23) = s & "\userenv.dll"
  a(24) = s & "\version.dll"
  a(25) = s & "\winsta.dll"
  a(26) = s & "\ws2_32.dll"
  a(27) = s & "\ws2help.dll"
  a(28) = s & "\advapi32.dll"
  a(29) = s & "\atl.dll"
  a(30) = s & "\batmeter.dll"
  a(31) = s & "\browseui.dll"
  a(32) = s & "\c_1251.nls"
  a(33) = s & "\clbcatq.dll"
  a(34) = s & "\comctl32.dll"
  a(35) = s & "\comres.dll"
  a(36) = s & "\credui.dll"
  a(37) = s & "\crypt32.dll"
  a(38) = s & "\cscdll.dll	"
  a(39) = s & "\cscui.dll	"
  a(40) = s & "\davclnt.dll"
  a(41) = s & "\dot3api.dll"
  a(42) = s & "\dot3dlg.dll"
  a(43) = s & "\drprov.dll"
  a(44) = s & "\eappcfg.dll"
  a(45) = s & "\eappprxy.dll"
  a(46) = s & "\explorer.exe"
  a(47) = s & "\imagehlp.dll"
  a(48) = s & "\index.dat"
  a(49) = s & "\iphlpapi.dll"
  a(50) = s & "\linkinfo.dll"
  a(51) = s & "\midimap.dll"
  a(52) = s & "\mlang.dll"
  a(53) = s & "\mpr.dll"
  a(54) = s & "\msacm32.dll"
  a(55) = s & "\msacm32.drv"
  a(56) = s & "\msasn1.dll"
  a(57) = s & "\msctf.dll"
  a(58) = s & "\msi.dll"
  a(59) = s & "\msimg32.dll"
  a(60) = s & "\msutb.dll"
  a(61) = s & "\msvcp60.dll"
  a(62) = s & "\msvcrt.dll"
  a(63) = s & "\netapi32.dll"
  a(64) = s & "\netrap.dll"
  a(65) = s & "\netshell.dll"
  a(66) = s & "\netui0.dll"
  a(67) = s & "\netui1.dll"
  a(68) = s & "\ntlanman.dll"
  a(69) = s & "\ntshrui.dll"
  a(70) = s & "\ole32.dll"
  a(71) = s & "\oleaut32.dll"
  a(72) = s & "\onex.dll"
  a(73) = s & "\powrprof.dll"
  a(74) = s & "\rpcrt4.dll"
  a(75) = s & "\rsaenh.dll"
  a(76) = s & "\rtutils.dll"
  a(77) = s & "\samlib.dll"
  a(78) = s & "\setupapi.dll"
  a(79) = s & "\shdoclc.dll"
  a(80) = s & "\shdocvw.dll"
  a(81) = s & "\shell32.dll"
  a(82) = s & "\shlwapi.dll"
  a(83) = s & "\sortkey.nls"
  a(84) = s & "\stobject.dll"
  a(85) = s & "\sxs.dll"
  a(86) = s & "\themeui.dll"
  a(87) = s & "\urlmon.dll"
  a(88) = s & "\user32.dll"
  a(89) = s & "\uxtheme.dll"
  a(90) = s & "\wdmaud.drv"
  a(91) = s & "\webcheck.dll"
  a(92) = s & "\wininet.dll"
  a(93) = s & "\winmm.dll"
  a(94) = s & "\wintrust.dll"
  a(95) = s & "\wldap32.dll"
  a(96) = s & "\wsock32.dll"
  a(97) = s & "\wtsapi32.dll"

  a(98) = s & "\MSVBVM60.DLL"
  a(99) = s & "\riched20.dll"
  a(100) = s & "\nview.dll"
  a(101) = s & "\AVIFIL32.dll"
  a(102) = s & "\MSVFW32.dll"
  a(103) = s & "\pjlmon.dll"
  a(104) = s & "\kerberos.dll"
  a(105) = s & "\localspl.dll"
  a(106) = s & "\w32time.dll"


  For n = 0 To 106
   '   If GetInputState() <> 0 Then
   Application.DoEvents()
   '   End If
   If Monitoring = False Then
    Exit Sub
   End If
   If LCase(a(n)) = LCase(a1) Then
    Exit Sub
   End If
  Next
  If Not File.Exists(a1) Then
   Exit Sub
  End If
  Scan(a1, "[MEM]")
  Exit Sub
100:

  ErrorLog("chk_dllNow " & ErrorToString())
 End Sub
 Function exclude_sys_critical(ByVal k1 As String) As Boolean
  'не проверять мои файлы,у них будет своя самозащита
  exclude_sys_critical = False
  Select Case LCase(k1)
   Case LCase(Application.StartupPath & "\monitor.exe")
    exclude_sys_critical = True
    Exit Function
   Case LCase(Application.StartupPath & "\ERRORLOG.LOG")
    exclude_sys_critical = True
    Exit Function
   Case LCase(Application.StartupPath & "\otchetMon.log")
    exclude_sys_critical = True
    Exit Function
   Case LCase(Application.StartupPath & "\SETTINGS.INI")
    exclude_sys_critical = True
    Exit Function
  End Select
  If IO.Path.GetFileName(k1) = "ntldr" Then
   exclude_sys_critical = True
   Exit Function
  End If
 End Function

 'Private Sub Timer_ico_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer_ico.Tick
 'моргание иконки
 'If T = False Then
 '   NotifyIcon1.Icon = New System.Drawing.Icon(Application.StartupPath & "\belyash.ico")
 ' T = True
 ' Else
 'NotifyIcon1.Icon = New System.Drawing.Icon(Application.StartupPath & "\n2.ico")
 '     T = False
 ' End If
 ' Timer_ico.Enabled = False
 '  Timer_ico.Enabled = True
 'End Sub

 Private Sub NotifyIcon1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseDown

  Select Case e.Button
   Case MouseButtons.Left
    Dim tmp_str As String
    tmp_str = "Scanning:" & CStr(inCount) & vbCrLf & "Infected:" & CStr(inFound) & vbCrLf & "Delete:" & CStr(inDELETE) & vbCrLf & "Move:" & CStr(inMove) & vbCrLf & "No cure:" & CStr(inNonCure) & vbCrLf & "Virus records:" & lblZap.Text
    If Monitoring = True Then
     NotifyIcon1.ShowBalloonTip(100, "Belyash Shield ON", tmp_str, ToolTipIcon.Info)
    Else
     NotifyIcon1.ShowBalloonTip(100, "Belyash Shield OFF", tmp_str, ToolTipIcon.Warning)
    End If
   Case MouseButtons.Right
    'ContextMenuStrip1.Visible = True

  End Select


 End Sub
 Private Sub killallotch(ByVal rtfile As String)
  On Error GoTo 10

  If File.Exists(rtfile) = True Then
   File.Delete(rtfile)
   GlassBox.ShowMessage("File [" & IO.Path.GetFileName(rtfile) & "] was deleted", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Information, MessageBoxButtons.OK)
   ' MsgBox("File [" & IO.Path.GetFileName(rtfile) & "] was deleted", MsgBoxStyle.Information)
  Else
   GlassBox.ShowMessage("File [" & IO.Path.GetFileName(rtfile) & "] is not exist", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   ' MsgBox("File [" & IO.Path.GetFileName(rtfile) & "] is not exist", MsgBoxStyle.Critical)
  End If
  Exit Sub
10:

  ErrorLog("killallotch " & ErrorToString())
 End Sub

 Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
  On Error Resume Next
  killallotch(Application.StartupPath & "\otchetMon.log")
  killallotch(Application.StartupPath & "\otchetVault.log")
  killallotch(Application.StartupPath & "\otchetScan.log")
  killallotch(Application.StartupPath & "\otchetFirewall.log")
  killallotch(Application.StartupPath & "\otchetUpdate.log")
  killallotch(Application.StartupPath & "\ERRORLOG.LOG")
 End Sub

 Private Sub tmrMonitor_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMonitor.Tick
  On Error GoTo 100
  If state.notDetected() = True Then
   tmrMonitor.Enabled = False
   DevicePresent = False
   Engine()
   If DevicePresent = True Then
    ' ShowForm()
    ' MsgBox("1")
   End If
  End If
  Exit Sub
100:
  ErrorLog("tmrMonitor_Tick " & ErrorToString())
 End Sub

 Private Sub tmrDevice_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDevice.Tick
  On Error GoTo 10
  y = state.detected()
  ' If GetInputState() <> 0 Then
  Application.DoEvents()
  ' End If
  If y = True Then

   DevicePresent = True
  End If
  Exit Sub
10:
  ErrorLog("tmrDevice_Tick " & ErrorToString())
 End Sub
 'usb
 '=========================================================
 Sub populateList()
  On Error Resume Next
  For Each File As String In My.Computer.FileSystem.GetFiles(str, FileIO.SearchOption.SearchTopLevelOnly, "*.exe")
   Scan(Trim(File.ToString), "[USB]")
  Next
  For Each File As String In My.Computer.FileSystem.GetFiles(str, FileIO.SearchOption.SearchTopLevelOnly, "*.com")
   Scan(Trim(File.ToString), "[USB]")

  Next
  For Each File As String In My.Computer.FileSystem.GetFiles(str, FileIO.SearchOption.SearchTopLevelOnly, "*.bat")
   Scan(Trim(File.ToString), "[USB]")
  Next
  For Each file As String In My.Computer.FileSystem.GetFiles(str, FileIO.SearchOption.SearchTopLevelOnly, "*.vbs")
   Scan(Trim(file.ToString), "[USB]")
  Next
  For Each file As String In My.Computer.FileSystem.GetFiles(str, FileIO.SearchOption.SearchTopLevelOnly, "*.cmd")
   Scan(Trim(file.ToString), "[USB]")
  Next

 End Sub
 Sub IsItSafe()
  On Error GoTo 200
  If Trim(str) = "" Or str = vbNull Then
   Exit Sub
  End If
  If My.Computer.FileSystem.GetFiles(str, FileIO.SearchOption.SearchTopLevelOnly, "autorun.inf").Count = 0 Then

  Else
   '  ret = MsgBox("Autorun.inf file still exists in the drive." & vbCrLf & "Ignore this file?", MsgBoxStyle.YesNo)
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Autorun.inf dont delete", ToolTipIcon.Warning)
  End If
  '   If ret = vbNo Then
  'My.Computer.FileSystem.DELETEFile(str & "autorun.inf", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DELETEPermanently)
  'End If
  Exit Sub
200:
  ErrorLog("IsItSafe " & ErrorToString())

 End Sub
 Public Sub Engine()
  'tmrDevice.Enabled = True
  Do
   If Monitoring = False Then
    Exit Do
   End If
   ' If GetInputState() <> 0 Then
   Application.DoEvents()
   ' End If
   y = state.detected()
   If y = True Then
    DevicePresent = True
   End If
  Loop Until y = True

 End Sub
 Sub getAutorun()
  On Error GoTo 100

  If My.Computer.FileSystem.FileExists(str & "autorun.inf") = False Then
   ' MsgBox("Autorun.inf file does not exist in this drive2")
   'NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Autorun.inf отсутствует на этом диске", ToolTipIcon.Info)
  Else
   podchistka()
   If My.Computer.FileSystem.FileExists(str & "autorun.inf") = True Then
    NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "No delete Autorun.inf", ToolTipIcon.Error)
    LogPrint("Shield", str & "autorun.inf-error cure ")
   End If

  End If
  Exit Sub
100:
  ErrorLog("getAutorun " & ErrorToString())

 End Sub
 Sub podchistka()
  On Error GoTo 10
  Dim n As Boolean = CBool(sGetINI(sINIFile, "Shield", "Autorun", "True"))
  If n = True Then
   If File.Exists(Application.StartupPath & "\quarantine\autorun.inf") = True Then
    File.Delete(Application.StartupPath & "\quarantine\autorun.inf")
   End If
   ' My.Computer.FileSystem.DELETEFile(str & "autorun.inf", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DELETEPermanently)
   System.IO.File.Move(Trim(str & "autorun.inf"), Application.StartupPath & "\quarantine\autorun.inf")
   NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Suspicious file" & vbCrLf & "AutoRun.inf" & vbCrLf & "Action:Move", ToolTipIcon.Info)
   LogPrint("Shield", str & "autorun.inf - delete")
   LogQuarant(str & "autorun.inf")
  End If
  Exit Sub
10:
  ErrorLog("podchistka " & ErrorToString())

 End Sub

 Private Sub ЧтоНовенькогоToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ЧтоНовенькогоToolStripMenuItem.Click
  On Error GoTo 10
  If File.Exists(Application.StartupPath & "\History.txt") = True Then
   Process.Start(Application.StartupPath & "\History.txt")
  Else
   GlassBox.ShowMessage("File History.txt not exist", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   'MsgBox("File History.txt not exist", MsgBoxStyle.)
  End If
  Exit Sub
10:

  ErrorLog("ЧтоНовенькогоToolStripMenuItem_Click " & ErrorToString())
 End Sub

 Private Sub ПрислатьВирусToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ПрислатьВирусToolStripMenuItem.Click
  mailme()
 End Sub
 Sub mailme()
  On Error GoTo 10
  Process.Start("mailto:mrbelyash@rambler.ru")
  Exit Sub
10:

  ErrorLog("mailme " & ErrorToString())
 End Sub
 Private Sub ПомощьToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ПомощьToolStripMenuItem1.Click
  On Error GoTo 10
  Help.ShowHelp(Me, hlpfile) 'вызов справки
  Exit Sub
10:
  ErrorLog("ПомощьToolStripMenuItem1_Click " & ErrorToString())

 End Sub

 Private Sub СайтToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles СайтToolStripMenuItem.Click
  goto_syte()
 End Sub
 Sub goto_syte()
  On Error GoTo 10
  Process.Start("http://www.mrbelyash.ucoz.ru")
  Exit Sub
10:

  ErrorLog("Сайтgoto_syte " & ErrorToString())
 End Sub


 Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
  If CheckBox1.Checked = True Then
   Me.TopMost = True
   me_top = True
   CheckBox1.BackColor = Color.Yellow
  Else
   Me.TopMost = False
   me_top = False
   CheckBox1.BackColor = Color.LightBlue
  End If
 End Sub

 Public Sub sn_top()
  If CheckBox1.Checked = True Then
   CheckBox1.Checked = False
  End If
 End Sub
 Function chk_userbase_string(ByVal kfile As String) As Boolean
  chk_userbase_string = False
  Try
   If File.Exists(Application.StartupPath & "\uzerbaseSTR.bvb") = False Then
    Exit Function
   End If
   Dim nb As String = Application.StartupPath & "\uzerbaseSTR.bvb"

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


 Private Sub ToolStripButton10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton10.Click
  'a variable to the context menu
  Dim mnu As ContextMenu = mnuContext

  'this point is the position you want the menu to popup at.
  Dim p As New Point(217, 20)

  'show the menu
  mnu.Show(ToolStrip, p)
 End Sub

 Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
  snyat_prior()
  MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Idle")
  MenuItem4.Checked = True
 End Sub

 Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
  snyat_prior()
  MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Normal")
  MenuItem3.Checked = True
 End Sub

 Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
  snyat_prior()
  MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Hight")
  MenuItem2.Checked = True
 End Sub

 Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
  snyat_prior()
  MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "RealTime")
  MenuItem1.Checked = True
 End Sub
 Sub snyat_prior()
  MenuItem1.Checked = False
  MenuItem2.Checked = False
  MenuItem3.Checked = False
  MenuItem4.Checked = False
 End Sub
 Public Sub chk_dll_newproc(ByVal den As String)
  If Trim(den) = "" Then
   Exit Sub
  End If
  Dim allProcesses(), thisProcess As Process
  Dim tmpProcname As String
  allProcesses = System.Diagnostics.Process.GetProcesses

  For Each thisProcess In allProcesses
   Try
    If Monitoring = False Then
     Exit Sub
    End If
    '   If GetInputState() <> 0 Then
    Application.DoEvents()
    '   End If
    tmpProcname = thisProcess.ProcessName
    If tmpProcname = den Then
     Dim thisModule As ProcessModule
     For Each thisModule In thisProcess.Modules

      'If GetInputState() <> 0 Then
      Application.DoEvents()
      ' End If
      If Monitoring = False Then
       Exit Sub
      End If
      With thisModule
       'Application.DoEvents()
       If .FileName <> "" Or .ModuleName <> "" Then
        Dim a As String
        a = .FileName.Substring(Len(.FileName) - 3)
        'If a <> "exe" Then
        'Scan(IO.Path.GetFullPath(.FileName), "[MEM]")
        'End If
        ' MsgBox(.FileName())
        Scan(.FileName, "[MEM]")
       End If
       'End If
      End With
     Next
    End If
   Catch ee As Exception
    LogPrint("Shield", "chk_dll_newproc " & ErrorToString())
   End Try

  Next
 End Sub


 Private Sub ContextMenuStrip1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ContextMenuStrip1.MouseDoubleClick
  RestoreWindow()
 End Sub


 Private Function ScanFull(ByVal dir1 As String, ByVal prichina As String, ByVal Component1 As String, ByVal filezip As String) As Boolean
  ScanFull = False
  On Error GoTo 200

  Dim my_md5 As String
  Dim StartTime As New DateTime()
  Dim EndTime As New DateTime()
  With My.Computer.FileSystem
   '  If GetInputState() <> 0 Then
   Application.DoEvents()
   ' End If
   ' List this directory's files.
   If Component1 = "Shield" Then
    If Monitoring = False Then
     Exit Function
    End If
   Else
    If stop_Scan = True Then
     Exit Function
    End If
   End If
   txtScanningNow.Text = dir1
   For Each file1 As String In .GetFiles(dir1)
    '   If GetInputState() <> 0 Then
    Application.DoEvents()
    '   End If
    If Component1 = "Shield" Then
     If Monitoring = False Then
      Exit Function
     End If
    Else
     If stop_Scan = True Then
      Exit Function
     End If
    End If

    txtScanningNow.Text = filezip & "/" & IO.Path.GetFileName(file1)
    txtScanningNow.Refresh()
    Dim fileDetail As IO.FileInfo
    fileDetail = My.Computer.FileSystem.GetFileInfo(file1)
    lblSize.Text = fileDetail.Length
    Dim r1 As Boolean = CBool(sGetINI(sINIFile, Component1, "NoCheckLenght", "False"))
    Dim r2 As Integer = CLng(sGetINI(sINIFile, Component1, "CheckLenght", "15142784"))
    If r1 = True Then
     If CLng(lblSize.Text) > 0 And CLng(lblSize.Text) > r2 Then
      LogPrint(Component1, file1 & " - exclude (" & lblSize.Text & ")")
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
     lblInfected.Text = inFound
     LogPrint(Component1, filezip & "/" & IO.Path.GetFileName(file1) & " -infected " & Virname)
     ' NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Архив инфицирован" & vbCrLf & vbCrLf & IO.Path.GetFileName(file1) & vbCrLf & Virname, ToolTipIcon.Info)
     ScanFull = True
     Exit Function
    Else
     If chk_uzerBase_NoPids(my_md5, file1, Component1) = True Then
      inFound = inFound + 1
      lblInfected.Text = inFound
      LogPrint(Component1, filezip & "/" & IO.Path.GetFileName(file1) & " -infected " & Virname)
      ' NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Архив инфицирован" & vbCrLf & vbCrLf & IO.Path.GetFileName(file1) & vbCrLf & Virname, ToolTipIcon.Info)
      ScanFull = True
      Exit Function
     Else
      'main_heur(file1) 'эвристика
     End If
    End If

    'my_md5
    LogPrint(Component1, filezip & "/" & IO.Path.GetFileName(file1) & "-OK")
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
    ' logprint ("Shield", fFile & "|" & my_md5)
    'End If
    Dim tmpTime As Boolean = CBool(sGetINI(sINIFile, Component1, "Time", "True"))
    If tmpTime = True Then
     If MyLibrary.FormFunction.check_upx_file(file1) = True Then
      LogPrint(Component1, file1 & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & lblSize.Text & prichina & " |UPX)")
     Else
      LogPrint(Component1, file1 & "|" & my_md5 & "(Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & lblSize.Text & prichina & ")")
     End If
    End If
150:
   Next file1
   ' Search child directories.
   For Each folder As Object In .GetDirectories(Dir)
    'TODO: Do something with the folder.
    '   If GetInputState() <> 0 Then

    '   End If
    txtScanningNow.Text = folder
    ScanFull(folder, prichina, Component1, filezip)
    If Component1 = "Shield" Then
     If Monitoring = False Then
      Exit Function
     End If
    Else
     If stop_Scan = True Then
      Exit Function
     End If
    End If
    Application.DoEvents()
   Next folder
  End With

  Exit Function
200:
  ErrorLog("ScanFull " & ErrorToString())
  'Resume Next
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
   sound_me("lic")
   GlassBox.ShowMessage("Invalid license", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   'MsgBox("Invalid license", MsgBoxStyle.Critical)
   Dim li As New ListViewItem(st, 5)
   li.SubItems.Add(fl_name)
   li.SubItems.Add("Error cure.Invalid license")
   li.SubItems.Add(Component1)
   ListViewFind.Items.Add(li)
   Exit Function
  End If
  Dim keyName2 As String = sGetINI(sINIFile, "Shield", "ActionZip", "REPORT")
  Dim keyName3 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Zapros", "False"))
  If keyName3 = True Then
   If zapros_na_deystw(keyName2) = False Then
    Exit Function
   End If
  End If
  Select Case keyName2
   Case "REPORT"
    LogPrint(Component1, fl_name & "-Arcive infected " & Virname & "(REPORT)")
    inNonCure = inNonCure + 1
    lblNOCured.Text = inNonCure
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Arcive infected" & vbCrLf & "Virus:" & st & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Action:REPORT", ToolTipIcon.Info)
    action_virus_zip = True
    Exit Function
   Case "MOVE"
    LogPrint(Component1, fl_name & "-Arcive infected " & st & "(Move to quarantine)")
    If File.Exists(Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name)) = True Then
     File.Delete(Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name))
    End If

    My.Computer.FileSystem.MoveFile(fl_name, _
Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name), True)
    inMove = inMove + 1
    lblMoved.Text = inMove
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Arcive infected" & vbCrLf & "Virus:" & st & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Action:Move", ToolTipIcon.Info)
    LogQuarant(fl_name)
    action_virus_zip = True
    Exit Function
   Case "DELETE"
    'удалить
    LogPrint(Component1, fl_name & "-Arcive infected " & st & "(DELETE)")
    File.Delete(fl_name)
    inDELETE = inDELETE + 1
    lblDELETE.Text = inDELETE
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Arcive infected" & vbCrLf & "Virus :" & st & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Action:DELETE", ToolTipIcon.Info)
    action_virus_zip = True
    Exit Function
   Case "LOCK"
    LogPrint("Shield", fl_name & "-Arcive infected " & st & "(LOCK)")
    If MyLibrary.FormFunction.lock_file(fl_name) = False Then
     Dim s82 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
     Exit Function
    End If
    Dim s2 As New FileStream(fl_name, FileMode.Open, FileAccess.Read, FileShare.None)
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Arcive infected" & vbCrLf & "Virus  :" & st & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Action:LOCK", ToolTipIcon.Info)
    action_virus_zip = True
    Exit Function
  End Select
  Exit Function
100:
  ErrorLog("action_virus_zip " & ErrorToString())

 End Function


 '-------------------------------------------------------
 'о программе
 Public Sub gt_dll()
  On Error GoTo 100
  With My.Computer.FileSystem
   For Each file1 As String In .GetFiles(Application.StartupPath)
    '   If GetInputState() <> 0 Then
    Application.DoEvents()
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
  Dim lvItem As ListViewItem
  lvItem = ListViewProg.Items.Add(IO.Path.GetFileName(s))
  lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileVersion.ToString)
  Select Case UCase(IO.Path.GetFileName(s))
   Case UCase("Monitor.exe")
    lvItem.SubItems.Add("Belyash Anti-Trojan 2009b Control Center")
   Case UCase("ConScanner.exe")
    lvItem.SubItems.Add("Belyash Anti-Trojan 2009b Console Scanner")
   Case UCase("cript.dll")
    lvItem.SubItems.Add("Belyash Cryptography Library")
    lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).ProductName.ToString)
   Case UCase("MyLibraryBase.dll")
    lvItem.SubItems.Add("Belyash Scan Engine")
    lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).ProductName.ToString)
    'Blocker.exe 

   Case UCase("Blocker.exe")
    lvItem.SubItems.Add("Belyash Blocker Application")
    lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).ProductName.ToString)
    'Blocker.exe 
   Case UCase("unpack.dll")
    lvItem.SubItems.Add("Belyash Unpack Module")
    lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).ProductName.ToString)
   Case UCase("registrymon.dll")
    lvItem.SubItems.Add("Belyash Registry Monitor")
    lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).ProductName.ToString)
   Case Else
    'lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileDescription.ToString)
    'lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).ProductName.ToString)
    lvItem.SubItems.Add(System.Diagnostics.FileVersionInfo.GetVersionInfo(s).ProductName.ToString)
  End Select
  Application.DoEvents()
  ListViewProg.Refresh()
 End Sub

 '==============
 Public Sub get_kolwovirus_z()
  gt_dll()
  Application.DoEvents()


  With My.Computer.FileSystem
   For Each file1 As String In .GetFiles(Application.StartupPath)
    ' If GetInputState() <> 0 Then
    Application.DoEvents()
    '   End If
    If IO.Path.GetExtension(file1.ToString) = ".bvb" Then
     'MsgBox(file1.ToString)
     n111_z(file1.ToString)
    End If

   Next
  End With
  'kolwoZap = MyLibrary.FormFunction.ScanFileBase1(Application.StartupPath)

  Dim lvItem2 As ListViewItem
  lvItem2 = ListViewProg.Items.Add("Virus records")
  lvItem2.SubItems.Add(kolwoZap)

 End Sub

 Sub n111_z(ByVal nb As String)
  Try

   Using sr1 As StreamReader = New StreamReader(nb)
    Dim line1 As String
    Dim tmpallb As Long = 0
    Do
     Application.DoEvents()
     line1 = sr1.ReadLine()
     If Trim(line1) <> "" Then
      'kolwoZap = kolwoZap + 1
      tmpallb = tmpallb + 1
     End If
    Loop Until line1 Is Nothing
    sr1.Close()
    Dim lvItem As ListViewItem
    lvItem = ListViewProg.Items.Add(IO.Path.GetFileName(nb))
    lvItem.SubItems.Add(tmpallb)
    lvItem.SubItems.Add("Virus definition: " & System.IO.File.GetCreationTime(nb).ToString)

    '=======
   End Using
  Catch E As Exception
   ' Let the user know what went wrong.
   GlassBox.ShowMessage("Unknow problem with bases-all bases", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   '            MsgBox("Unknow problem with bases-all bases")
   ErrorLog("n111 " & ErrorToString())
  End Try
 End Sub
 Private Sub AddImages16(ByVal strFileName As String)
  'This adds the files icon to the small image list 'icons16'
  Dim shInfo As SHFILEINFO = New SHFILEINFO
  shInfo.szDisplayName = New String(vbNullChar, MAX_PATH)
  shInfo.szTypeName = New String(vbNullChar, 80)

  Dim hIcon As IntPtr
  hIcon = SHGetFileInfo(strFileName, 0, shInfo, Marshal.SizeOf(shInfo), SHGFI_ICON Or SHGFI_SMALLICON)

  Dim MyIcon As Drawing.Bitmap
  MyIcon = Drawing.Icon.FromHandle(shInfo.hIcon).ToBitmap
  Icons16.Images.Add(strFileName.ToString(), MyIcon) 'Add the image to the imagelist
 End Sub

 Private Sub AddImages32(ByVal strFileName As String)
  'This adds the files icon to the large image list 'icons32'
  Dim shInfo As SHFILEINFO = New SHFILEINFO
  shInfo.szDisplayName = New String(vbNullChar, MAX_PATH)
  shInfo.szTypeName = New String(vbNullChar, 80)

  Dim hIcon As IntPtr
  hIcon = SHGetFileInfo(strFileName, 0, shInfo, Marshal.SizeOf(shInfo), SHGFI_ICON Or SHGFI_LARGEICON)

  Dim MyIcon As Drawing.Bitmap
  MyIcon = Drawing.Icon.FromHandle(shInfo.hIcon).ToBitmap
  Icons32.Images.Add(strFileName.ToString(), MyIcon) 'Add the image to the imagelist
 End Sub

 Private Sub AddFolders(ByVal TNode As TreeNode, ByVal FolderPath As String)
  Try
   For Each FolderNode As String In Directory.GetDirectories(FolderPath)
    Dim SubFolderNode As TreeNode = TNode.Nodes.Add(FolderNode.Substring(FolderNode.LastIndexOf("\"c) + 1))
    SubFolderNode.Tag = FolderNode
    SubFolderNode.Nodes.Add("Loading...")
   Next
  Catch ex As Exception
   GlassBox.ShowMessage("AddFolders " & ex.Message, "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   'MsgBox("AddFolders " & ex.Message)
  End Try
 End Sub




 Private Sub TabPage6_Selecting(ByVal sender As Object, ByVal e As TabControlCancelEventArgs) _
  Handles TabControl1.Selecting

  Select Case (e.TabPageIndex)
   Case 5
    go_t()
   Case 8
    Static i As Integer
    i = i + 1
    If i = 1 Then
     Sbor.Enabled = True
    End If

  End Select


 End Sub

 Public Sub go_t()
  On Error Resume Next
  Dim FileExtension As String
  Dim SubItemIndex As Integer
  Dim DateMod As String
  Dim dtT As String
  Dim z As Integer = 0
  ListViewQ.Items.Clear()



  Dim folder As String = Application.StartupPath & "\quarantine"
  ' If Not folder Is Nothing AndAlso IO.Directory.Exists(folder) Then
  ' Try
  For Each file As String In IO.Directory.GetFiles(folder)
   If UCase(IO.Path.GetFileName(file)) = UCase("MovedFiles.LOG") Then
    GoTo 11
   End If
   z = z + 1
   FileExtension = IO.Path.GetExtension(file)
   DateMod = IO.File.GetLastWriteTime(file).ToString()
   dtT = IO.File.GetAttributes(file).ToString()
   Dim f1 As String
   f1 = System.Diagnostics.FileVersionInfo.GetVersionInfo(file).FileDescription.ToString() & " - " & System.Diagnostics.FileVersionInfo.GetVersionInfo(file).CompanyName.ToString()
   AddImages16(file) 'Add the icons for 16x16 (details view)
   AddImages32(file) 'Add the icons for 32x32 (large icon view)

   ListViewQ.Items.Add(file.Substring(file.LastIndexOf("\"c) + 1), file) 'Add the image & file name
   ListViewQ.Items(SubItemIndex).SubItems.Add(FileExtension.ToString()) 'Add the file exten.
   ListViewQ.Items(SubItemIndex).SubItems.Add(DateMod.ToString()) 'Add the date modified
   If Microsoft.VisualBasic.Right(file, 3) = "CRP" Then
    ListViewQ.Items(SubItemIndex).SubItems.Add("Encrypts")
   Else
    ListViewQ.Items(SubItemIndex).SubItems.Add(dtT.ToString())
   End If
   If Trim(f1) <> "" Then
    ListViewQ.Items(SubItemIndex).SubItems.Add(f1) 'Add the date modified
   End If
   f1 = ""
   SubItemIndex += 1
11:
  Next
  Label44.Text = CStr(z)
  'Catch ex As Exception
  'MsgBox(ex.Message)
  'End Try
  ' End If
 End Sub

 Private Sub ВосстановитьToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ВосстановитьToolStripMenuItem.Click
  On Error GoTo 101
  If Trim(fileTODel) = "" Then
   Exit Sub
  End If
  If File.Exists(Application.StartupPath & "\quarantine\" & fileTODel) = True Then

   Dim dest As String
   FolderBrowserDialog1.ShowDialog()
   dest = FolderBrowserDialog1.SelectedPath & "\" & fileTODel
   If Microsoft.VisualBasic.Right(Application.StartupPath & "\quarantine\" & fileTODel, 3) = "CRP" Then
    objCryptDES.Key = "0"
    objCryptDES.FileDecrypt(Application.StartupPath & "\quarantine\" & fileTODel, dest.Remove(Len(dest) - 3, 3), True)

   Else
    File.Move(Application.StartupPath & "\quarantine\" & fileTODel, dest.Remove(Len(dest) - 3, 3))
   End If

   LogQuarant(fileTONum & "recovery to " & dest)
   ListViewQ.Items.RemoveAt(fileTONum)
   GlassBox.ShowMessage("File [" & fileTODel & "] is recovery", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Information, MessageBoxButtons.OK)
   'MsgBox("File [" & fileTODel & "] is recovery", MsgBoxStyle.Information)
   go_t()
   ListViewQ.Refresh()
  End If
  Exit Sub
101:

  ErrorLog("ВосстановитьToolStripMenuItem_Click " & ErrorToString())
 End Sub

 Private Sub УдалитьToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles УдалитьToolStripMenuItem.Click
  On Error GoTo 101
  If Trim(fileTODel) = "" Then
   Exit Sub
  End If
  If File.Exists(Application.StartupPath & "\quarantine\" & fileTODel) = True Then
   File.Delete(Application.StartupPath & "\quarantine\" & fileTODel)
   ListViewQ.Items.RemoveAt(fileTONum)
   GlassBox.ShowMessage("File [" & fileTODel & "] was deleted", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Information, MessageBoxButtons.OK)
   'MsgBox("File [" & fileTODel & "] was deleted", MsgBoxStyle.Information)
   LogQuarant(fileTODel & "-delete")
   go_t()
  End If
  Exit Sub
101:

  ErrorLog("УдалитьToolStripMenuItem_Click " & ErrorToString())
 End Sub

 Private Sub ListViewQ_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListViewQ.SelectedIndexChanged
  On Error GoTo 100
  Dim i As Integer
  For i = 0 To ListViewQ.Items.Count
   If ListViewQ.SelectedItems(i).Text <> "" Then
    'MsgBox(ListView1.SelectedItems(i).Text)
    fileTODel = ListViewQ.SelectedItems(i).Text
    'ContextMenuStrip1.Visible = True
    fileTONum = i
    Exit Sub
   End If
  Next
  Exit Sub
100:


 End Sub

 Private Sub RadioButton9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton9.CheckedChanged
  ListViewQ.View = View.LargeIcon 'Switch to Large icon view
 End Sub

 Private Sub RadioButton10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton10.CheckedChanged
  ListViewQ.View = View.Details 'Switch to Details view
 End Sub

 Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
  go_t()
 End Sub

 Private Sub Sbor_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Sbor.Tick
  If Sbor.Enabled = True Then
   Sbor.Enabled = False
  End If
  get_kolwovirus_z()
  ListViewProg.Refresh()

 End Sub

 Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
  mailme()
 End Sub

 Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
  goto_syte()
 End Sub



 Private Sub LinkLabel5_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
  On Error GoTo 10
  Process.Start("http://www.mrbelyash.narod.ru")
  Exit Sub
10:

  ErrorLog("LinkLabel5_LinkClicked " & ErrorToString())
 End Sub
 Private Sub ОбновлениеToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ОбновлениеToolStripMenuItem.Click
  TabControl1.SelectTab(5)
  RestoreWindow()
 End Sub

 Private Sub ПоказатьToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ПоказатьToolStripMenuItem.Click
  'TabControl1.SelectTab(0)
  RestoreWindow()
 End Sub

 Private Sub ВыходToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ВыходToolStripMenuItem.Click

  On Error Resume Next
  sound_me("exit")
  If tmrMonitor.Enabled = True Then
   tmrMonitor.Enabled = False
   writeINI(sINIFile, "Shield", "Monitoring", "True")
  End If
  'If Timer_ico.Enabled = True Then
  'Timer_ico.Enabled = False
  'End If
  If Sbor.Enabled = True Then
   Sbor.Enabled = False
   ' Sbor.Dispose()
  End If
  If Timer2.Enabled = True Then
   Timer2.Enabled = False
   'Timer2.Dispose()
  End If
  If tmrDevice.Enabled = True Then
   tmrDevice.Enabled = False
   'tmrDevice.Dispose()
  End If
  If Timer1.Enabled = True Then
   Timer1.Enabled = False
   ' Timer1.Dispose()
  End If
  Me.Close()
  'Me.Dispose()

 End Sub



 Private Sub Label20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label20.Click
  TabControl1.SelectTab(0)
 End Sub

 Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click
  sn_top()
  Help.ShowHelp(Me, hlpfile) 'вызов справки
 End Sub

 Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
  Select Case ComboBox1.Text
   Case ("Resident Shield")
    dell_otchet(Application.StartupPath & "\otchetMon.log")
   Case ("Virus Vault")
    dell_otchet(Application.StartupPath & "\quarantine\MovedFiles.LOG")
   Case ("Scanner")
    dell_otchet(Application.StartupPath & "\otchetScan.log")
   Case ("Firewall")
    dell_otchet(Application.StartupPath & "\otchetFirewall.log")
   Case ("Update")
    dell_otchet(Application.StartupPath & "\otchetUpdate.log")
   Case ("Error")
    dell_otchet(Application.StartupPath & "\ERRORLOG.LOG")
   Case ("Registry")
    dell_otchet(Application.StartupPath & "\registryScan.log")

  End Select
  RichTextBox1.Text = ""
 End Sub
 Sub dell_otchet(ByVal cr_file As String)
  'удалить отчет
  On Error GoTo 10
  If File.Exists(cr_file) = True Then
   File.Delete(cr_file)
   GlassBox.ShowMessage("File [" & IO.Path.GetFileName(cr_file) & "] was deleted", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Information, MessageBoxButtons.OK)
   'MsgBox("File " & IO.Path.GetFileName(cr_file) & " was deleted", MsgBoxStyle.Information)
  Else
   GlassBox.ShowMessage("File [" & IO.Path.GetFileName(cr_file) & "] not exist", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)

  End If
  Exit Sub
10:

  ErrorLog("dell_otchet " & ErrorToString())
 End Sub
 Sub viw_external(ByVal otch As String)
  On Error GoTo 10
  If File.Exists(otch) = True Then
   Process.Start(otch)
  End If
  Exit Sub
10:

  ErrorLog("viw_external " & ErrorToString())
 End Sub
 Sub view_otchet(ByVal otc4 As String)
  'просмотр отчета
  If File.Exists(otc4) = False Then
   GlassBox.ShowMessage("File [" & IO.Path.GetFileName(otc4) & "] not exist", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   'MsgBox("File [" & IO.Path.GetFileName(otc4) & "] not exist", MsgBoxStyle.Critical)
   Exit Sub
  End If
  If RadioButton12.Checked = True Then
   viw_external(otc4)
   Exit Sub
  End If
  Try

   RichTextBox1.Text = ""
   Dim line As String
   Dim readFile As System.IO.TextReader = New  _
StreamReader(otc4)
   line = readFile.ReadToEnd()
   RichTextBox1.Text = line
   readFile.Close()
   readFile = Nothing
  Catch ex As IOException
   GlassBox.ShowMessage(ex.ToString, "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
   ErrorLog("view_otchet " & ErrorToString())
  End Try
 End Sub
 Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
  RichTextBox1.Text = ""
  Select Case ComboBox1.Text
   Case ("Resident Shield")
    view_otchet(Application.StartupPath & "\otchetMon.log")
   Case ("Virus Vault")
    view_otchet(Application.StartupPath & "\quarantine\MovedFiles.LOG")
   Case ("Scanner")
    view_otchet(Application.StartupPath & "\otchetScan.log")
   Case ("Firewall")
    view_otchet(Application.StartupPath & "\otchetFirewall.log")
   Case ("Update")
    view_otchet(Application.StartupPath & "\otchetUpdate.log")
   Case ("Error")
    view_otchet(Application.StartupPath & "\ERRORLOG.LOG")
   Case ("Registry")
    view_otchet(Application.StartupPath & "\registryScan.log")

  End Select
 End Sub

 Private Sub Label17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  frmActivate.ShowDialog()
 End Sub


 Private Sub PictureBox5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox5.Click
  TabControl1.SelectTab(1)
 End Sub

 Private Sub Label15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label15.Click
  TabControl1.SelectTab(1)
 End Sub

 Private Sub PictureBox9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox9.Click
  TabControl1.SelectTab(2)
 End Sub

 Private Sub Label37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label37.Click
  TabControl1.SelectTab(2)
 End Sub

 Private Sub Label38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label38.Click
  TabControl1.SelectTab(4)
 End Sub

 Private Sub PictureBox10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox10.Click
  TabControl1.SelectTab(4)
 End Sub

 Private Sub Label39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label39.Click
  TabControl1.SelectTab(3)
 End Sub

 Private Sub PictureBox11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox11.Click
  TabControl1.SelectTab(3)
 End Sub

 Private Sub LinkLabel7_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
  frmActivate.ShowDialog()
 End Sub

 Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
  Nastroyka.ShowDialog()
 End Sub
 Private Sub getmyfileatr(ByVal diFile As String)

  Try

   If (System.IO.File.GetAttributes(diFile) And (FileAttributes.Compressed _
       + FileAttributes.Hidden + FileAttributes.System)) = _
       (FileAttributes.Compressed + FileAttributes.Hidden + _
       FileAttributes.System + FileAttributes.Archive) Then
    mintCompressedFiles += 1
    mintHiddenFiles += 1
    mintSystemFiles += 1
    mintTotalFiles += 1
    mintArchive += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And (FileAttributes.Hidden _
       + FileAttributes.System)) = (FileAttributes.Hidden _
       + FileAttributes.System) Then
    mintHiddenFiles += 1
    mintSystemFiles += 1
    mintTotalFiles += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.Hidden) = _
       FileAttributes.Hidden Then
    mintHiddenFiles += 1
    mintTotalFiles += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.System) = _
       FileAttributes.System Then
    mintSystemFiles += 1
    mintTotalFiles += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.Archive) = _
       FileAttributes.Archive Then
    mintArchive += 1
    mintTotalFiles += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.Compressed) = _
       FileAttributes.Compressed Then
    mintCompressedFiles += 1
    mintTotalFiles += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.Encrypted) = _
       FileAttributes.Encrypted Then
    mintEncr += 1
    mintTotalFiles += 1
   Else

    mintNormalFiles += 1
    mintTotalFiles += 1
   End If
   lblNormalFiles.Text = mintNormalFiles
   lblCompressedFiles.Text = mintCompressedFiles
   lblSystemFiles.Text = mintSystemFiles
   lblHidden.Text = mintHiddenFiles
   lblmintArchive.Text = mintArchive
   lblmintEncr.Text = mintEncr

   lblTotalFiles.Text = mintTotalFiles
   inCounter = inCounter + 1
  Catch ex As Exception
   ErrorLog("getmyfileatr " & ErrorToString())
  End Try

  'lblTotalFiles.Text = inCounter
 End Sub
 Private Sub clear_Statist()
  mintlbluncheck = 0
  mintNormalFiles = 0
  mintCompressedFiles = 0
  mintSystemFiles = 0
  mintHiddenFiles = 0
  mintlbluncheck = 0
  mintArchive = 0
  mintEncr = 0
  mintTotalFiles = 0
  lbluncheck.Text = mintlbluncheck
  lblNormalFiles.Text = mintNormalFiles
  lblCompressedFiles.Text = mintCompressedFiles
  lblSystemFiles.Text = mintSystemFiles
  lblHidden.Text = mintHiddenFiles
  lblmintArchive.Text = mintArchive
  lblmintEncr.Text = mintEncr
  Label28.Text = "0"
  lblSizef.Text = "0"
  txtTime.Text = "0"

  lblTotalFiles.Text = mintTotalFiles
 End Sub
 Private Sub clear_StatistShield()

  mintNormalFiles2 = 0
  mintCompressedFiles2 = 0
  mintSystemFiles2 = 0
  mintHiddenFiles2 = 0
  mintArchive2 = 0
  mintEncr2 = 0
  mintTotalFiles2 = 0
  Label84.Text = mintNormalFiles2
  Label85.Text = mintCompressedFiles2
  Label87.Text = mintSystemFiles2
  Label88.Text = mintHiddenFiles2
  Label83.Text = mintArchive2

 End Sub
 Private Sub getmyfileatrShield(ByVal diFile As String)

  Try

   If (System.IO.File.GetAttributes(diFile) And (FileAttributes.Compressed _
       + FileAttributes.Hidden + FileAttributes.System)) = _
       (FileAttributes.Compressed + FileAttributes.Hidden + _
       FileAttributes.System + FileAttributes.Archive) Then
    mintCompressedFiles2 += 1
    mintHiddenFiles2 += 1
    mintSystemFiles2 += 1
    mintTotalFiles2 += 1
    mintArchive2 += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And (FileAttributes.Hidden _
       + FileAttributes.System)) = (FileAttributes.Hidden _
       + FileAttributes.System) Then
    mintHiddenFiles2 += 1
    mintSystemFiles2 += 1
    mintTotalFiles2 += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.Hidden) = _
       FileAttributes.Hidden Then
    mintHiddenFiles2 += 1
    mintTotalFiles2 += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.System) = _
       FileAttributes.System Then
    mintSystemFiles2 += 1
    mintTotalFiles2 += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.Archive) = _
       FileAttributes.Archive Then
    mintArchive2 += 1
    mintTotalFiles2 += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.Compressed) = _
       FileAttributes.Compressed Then
    mintCompressedFiles2 += 1
    mintTotalFiles2 += 1
   ElseIf (System.IO.File.GetAttributes(diFile) And FileAttributes.Encrypted) = _
       FileAttributes.Encrypted Then
    mintEncr2 += 1
    mintTotalFiles2 += 1
   Else

    mintNormalFiles2 += 1
    mintTotalFiles2 += 1
   End If
   Label84.Text = mintNormalFiles2
   Label85.Text = mintCompressedFiles2
   Label87.Text = mintSystemFiles2
   Label88.Text = mintHiddenFiles2
   Label83.Text = mintArchive2
  Catch ex As Exception
   ErrorLog("getmyfileatrShield " & ErrorToString())
  End Try
 End Sub
 Public Sub SubtractFromCounter()
  ' If GetInputState() <> 0 Then
  Application.DoEvents()
  ' End If
  ProgressBar1.Style = ProgressBarStyle.Marquee

 End Sub
 Public Sub SubtractFromCounter2()
  ' If GetInputState() <> 0 Then
  Application.DoEvents()
  ' End If
  ProgressBar2.Style = ProgressBarStyle.Marquee

 End Sub
 Public Sub starting_scan()

  stop_Scan = False

  Dim name75 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "Custom", "True"))
  If name75 = True Then
   If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
    myScanPath.Text = FolderBrowserDialog1.SelectedPath
   Else
    Exit Sub
   End If
   If Trim(myScanPath.Text) <> "" Then
    Label36.Text = "    Scanning..."
    Label36.ImageKey = "check.PNG"
    cmdStopScan.Enabled = True
    cmdStartScanning.Enabled = False
    writeINI(sINIFile, "Scanner", "LastScan", Format(Now, "dd:MM:yyyy"))
    myScanPath.Text = myScanPath.Text
    SubtractFromCounter()
    ScanScanner(myScanPath.Text)
   Else
    GlassBox.ShowMessage("Select folder for scan", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
    'MsgBox("Select folder for scan", MsgBoxStyle.Critical)
    Exit Sub
   End If
  Else
   Label36.Text = "    Scanning..."
   Label36.ImageKey = "check.PNG"
   cmdStopScan.Enabled = True
   cmdStartScanning.Enabled = False
   Dim drives() As String
   Dim aDrive As String
   drives = Directory.GetLogicalDrives()
   SubtractFromCounter()
   Application.DoEvents()
   For Each aDrive In drives

    myScanPath.Text = aDrive
    ScanScanner(myScanPath.Text)
    Application.DoEvents()
   Next
  End If
  stop_Scan = True
  LabellstScan.Text = "Last Scan: " & Format(Now, "dd:MM:yyyy")
  txtScanningNow.Text = ""
  myScanPath.Text = ""
  cmdStartScanning.Enabled = True
  cmdStopScan.Enabled = False
  Label36.Text = "    Scanning finished"
  Label36.ImageKey = "uncheck.PNG"
  ProgressBar1.Style = ProgressBarStyle.Blocks
 End Sub
 Public Sub ScanScanner(ByVal dir As String)
  'сканирование опред. области...сканер

  On Error GoTo 200
  Dim hs As String = "None"
  With My.Computer.FileSystem
   '  If GetInputState() <> 0 Then
   Application.DoEvents()
   '  End If
   ' List this directory's files.
   If stop_Scan = True Then
    Exit Sub
   End If
   For Each file1 As String In .GetFiles(dir)
    '    If GetInputState() <> 0 Then

    '    End If
    If stop_Scan = True Then
     Button1.Enabled = True
     Exit Sub
    End If
    If Exclude_pats(IO.Path.GetDirectoryName(file1.ToString)) = True Then
     LogPrint("Scanner", file1.ToString & "-exclude")
     GoTo 150
    End If
    If exclude_sys_critical(file1.ToString) = True Then
     LogPrint("Scanner", file1.ToString & "-exclude")
     GoTo 150
    End If
    txtScanningNow.Text = file1
    If gt_extension_zip(IO.Path.GetExtension(file1)) = True Then
     unpackFiles(file1, "[scan]", "Scanner")
     GoTo 150
    End If
    Dim fi As FileInfo = New FileInfo(file1)
    'lblNormalFolders.Text = fi.Attributes.ToString()
    Call getmyfileatr(file1)
    '==================================

    Dim fileDetail As IO.FileInfo
    fileDetail = My.Computer.FileSystem.GetFileInfo(file1)
    lblSizef.Text = fileDetail.Length & " byte(s)"
    Dim fp As Long = CLng(sGetINI(sINIFile, "Scanner", "FILESIZE", "55242880"))
    If CLng(fileDetail.Length) >= CLng(fp) Or fileDetail.Length < 0.2 Then
     LogPrint("Scanner", file1 & "-exclude (big size=)" & fileDetail.Length)
     GoTo 100
    End If
    Dim name4F As Boolean = CBool(sGetINI(sINIFile, "Scanner", "SCANALL", "True"))
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
    txtScanningNow.Text = file1

    '============================================================================
    hs = MD5_Hash(file1)
    If yes_vir(hs) = True Then
     LogPrint("Scanner", file1 & "-infected " & Virname)
     infound2 = infound2 + 1
     Label28.Text = infound2
     'NotifyIcon1.ShowBalloonTip(1000, "Belyash Shield", "Found infected file" & vbCrLf & "File :" & IO.Path.GetFileName(fl_name) & vbCrLf & "Virus:" & Virname & vbCrLf & "Actions:REPORT", ToolTipIcon.Info)
     If action_virus(file1.ToString, Virname, "Scanner") = False Then
      SecondActions(file1.ToString, Virname, "Scanner")
     End If
     GoTo 100
    Else
     Dim name26 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "ScanHeur", "False"))
     If name26 = True Then
      main_heur(file1, "Scanner")
     End If

    End If

    If chk_userbase_string(file1) = True Then
     LogPrint("Scanner", file1 & "-find in user base(string in base)")
     infound2 = infound2 + 1
     Label28.Text = infound2
     If action_virus_CRC(file1, "User record", "Scanner") = False Then
      inFound = inFound + 1
      LogPrint("Scanner", "Error cure " & file1 & "-(User record)")
      SecondActions(file1, "User record", "Scanner")
     End If
    End If
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
    txtTime.Text = f2 & "." & f1 & "." & answer1
    txtTime.Refresh()

    MyLibrary.FormFunction.check_upx_file(file1)
    Dim name87 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "Izbitochnoe", "False"))
    If name87 = True Then
     If MyLibrary.FormFunction.check_upx_file(file1) = True Then
      LogPrint("Scanner", file1 & "|" & hs & "|Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & lblSizef.Text & "[UPX]")

     Else
      LogPrint("Scanner", file1 & "|" & hs & "|Time=" & f2 & "." & f1 & "." & answer1 & "|SZ=" & lblSizef.Text)
     End If
    End If
150:
    Application.DoEvents()
   Next file1
   ' Search child directories.
   For Each folder As Object In .GetDirectories(dir)
    'TODO: Do something with the folder.
    '    If GetInputState() <> 0 Then

    '   End If
    txtScanningNow.Text = folder
    ScanScanner(folder)
    Application.DoEvents()
   Next folder
  End With

  Exit Sub
200:
  MsgBox(ErrorToString)
  ErrorLog("Scan " & ErrorToString())
  Resume Next


 End Sub


 Private Sub Label67_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label67.Click
  'обнулить статистику сканера
  Call clear_Statist()
 End Sub

 Private Sub cmdStopScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStopScan.Click
  'остановить сканирование(сканер)
  stop_Scan = True
  ProgressBar1.Style = ProgressBarStyle.Blocks
  'txtScanningNow.Text = ""
  myScanPath.Text = ""
  cmdStopScan.Enabled = False
  cmdStartScanning.Enabled = True
  Label36.Text = "    Scanning interrupted..."
 End Sub
 Private Sub cmdStartScanning_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStartScanning.Click
  logcomponentVersion("Scanner")
  starting_scan()
 End Sub
 Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
  Nastroyka.ShowDialog()
 End Sub
 Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Nastroyka.ShowDialog()
 End Sub
 Private Sub ВключитьМониторингToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ВключитьМониторингToolStripMenuItem.Click
  'вкл/выкл мониторинг файлов
  If Label8.Text = "    Disabled" Then
   startMon()

  Else
   stopMon()
  End If

 End Sub
 Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
  'запустить обновление
  Label8.ForeColor = Color.Blue
  Label8.Text = "    Enabled"
  Label8.ImageKey = "check.PNG"
  cmdUpdate.Enabled = False
  cmdUpdAbort.Enabled = True
  writeINI(sINIFile, "Update", "LastUpdate", Format(Now, "dd:MM:yyyy"))
  Label8.Text = "    Disabled"
  Label8.ImageKey = "uncheck.PNG"
  Label8.ForeColor = Color.Red
  Label52.Text = "Last Update: " & Format(Now, "dd:MM:yyyy")
  Label10.Text = "Last Update: " & Format(Now, "dd:MM:yyyy")
 End Sub
 Private Sub cmdUpdAbort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdAbort.Click
  'прервать обновление
  cmdUpdAbort.Enabled = False
  cmdUpdate.Enabled = True
 End Sub
 Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click

  TabControl1.SelectTab(7)
  RestoreWindow()
 End Sub
 Private Sub LinkLabel9_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
  Nastroyka.ShowDialog()
 End Sub

 Private Sub LinkLabel8_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
  Nastroyka.ShowDialog()
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

  Dim keyName2 As String = sGetINI(sINIFile, "Shield", "Action", "REPORT")
  Dim keyName3 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Zapros", "False"))

  If keyName3 = True Then
   If zapros_na_deystw(keyName2) = False Then
    Exit Sub
   End If
  End If

  Select Case keyName2
   Case "DELETE"
    'удалить
    MyLibrary.FormFunction.kiilONReboot(secondfilename)
    LogPrint(Component1, secondfilename & "-infected,delete after reboot " & Virname & "(DELETE)")
    inDELETE = inDELETE + 1
    lblDELETE.Text = inDELETE
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Infected file is delete" & vbCrLf & "File :" & IO.Path.GetFileName(secondfilename) & vbCrLf & "Virus :" & Virname & vbCrLf & "Action:DELETE", ToolTipIcon.Info)
    If GlassBox.ShowMessage("Curing some infected files requires a system reboot." & vbCrLf & "Continue ?", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Question, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
     '    If MsgBox(, MsgBoxStyle.Question, "Warning") = MsgBoxResult.Yes Then
     MyLibrary.FormFunction.reboot()
     End
    End If

    Exit Sub
   Case "REPORT"

   Case Else
    inNonCure = inNonCure + 1
    lblNOCured.Text = inNonCure
    Dim li As New ListViewItem(st, 5)
    li.SubItems.Add(secondfilename)
    li.SubItems.Add("ERROR ACTION,REPORT")
    li.SubItems.Add(Component1)
    ListViewFind.Items.Add(li)
    NotifyIcon1.ShowBalloonTip(1000, "Belyash " & Component1, "Found infected file" & vbCrLf & "File :" & IO.Path.GetFileName(secondfilename) & vbCrLf & "Virus:" & Virname & vbCrLf & "Error Action:REPORT", ToolTipIcon.Info)
    Exit Sub

  End Select
  Exit Sub
100:
  ErrorLog("SecondActions " & ErrorToString())
  ' MsgBox(ErrorToString)

 End Sub


 Public Sub chk_autozaxist()
  'самозащита...защищаем запись в реестре-автозагрузку
  Dim tmpAuto As Boolean = CBool(sGetINI(sINIFile, "Shield", "Run", "True"))
  If tmpAuto = True Then
   ChkMastLoad.Checked = True
   Const userRoot As String = "HKEY_LOCAL_MACHINE"
   Const subkey As String = "SoftWare\Microsoft\Windows\CurrentVersion\Run"
   Const keyName As String = userRoot & "\" & subkey
   Dim noSuch As String = _
      Registry.GetValue(keyName, "Belyash Shield", _
      "None")
   If noSuch = "None" Then
    Registry.SetValue(keyName, _
        "Belyash Shield", Application.ExecutablePath, RegistryValueKind.ExpandString)
   End If
   RegMon.FormFunction.start_monreg()
   Timer2.Enabled = True
  Else
   ChkMastLoad.Checked = False
  End If
 End Sub



 Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
  Application.DoEvents()
  If RegMon.FormFunction.chek_r = True Then

   Dim tmpAuto As Boolean = CBool(sGetINI(sINIFile, "Shield", "Run", "True"))
   If tmpAuto = True Then
    Const userRoot As String = "HKEY_LOCAL_MACHINE"
    Const subkey As String = "SoftWare\Microsoft\Windows\CurrentVersion\Run"
    Const keyName As String = userRoot & "\" & subkey
    Dim noSuch As String = _
       Registry.GetValue(keyName, "Belyash Shield", _
       "None")
    If noSuch = "None" Then
     Dim frm As New Window
     ShowPopup("Module protection ", "Unknown application can disabled startup Belyash Shield." & vbCrLf & vbCrLf & "Startup Belyash shield recovery")
    End If
   End If
   Timer2.Enabled = False
   chk_autozaxist()
  End If

  If Timer2.Enabled = False Then
   Timer2.Enabled = True
  End If
 End Sub
 Public Sub ShowPopup(ByVal sTitle As String, ByVal sMessage As String)
  Dim frm As New Window
  frm.Label2.Text = UCase(sTitle)
  frm.TextBox1.Text = UCase(sMessage)
  frm.Show()
 End Sub

 ' Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)

 'Timer3.Enabled = True
 'Dim Text As String = "Belyash Anti-Trojan 2009b v."
 'Static x As Integer
 '   x = x + 1
 '  If x > Len(Text) Then
 '     x = 1
 '    Me.Text = ""
 'End If
 'Me.Text = Me.Text & Mid(Text, x, 1)
 'Me.Refresh()
 'End Sub

 Private Sub CheckBox3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox3.Click
  writeINI(sINIFile, "Scanner", "Ask", CheckBox3.Checked)
 End Sub

 Private Sub CheckBox2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox2.Click
  writeINI(sINIFile, "Shield", "Zapros", CheckBox2.Checked)
 End Sub

 Private Sub RadioButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.Click
  writeINI(sINIFile, "Shield", "Action", "REPORT")
 End Sub

 Private Sub RadioButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton2.Click
  writeINI(sINIFile, "Shield", "Action", "DELETE")
 End Sub

 Private Sub RadioButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton3.Click

  writeINI(sINIFile, "Shield", "Action", "MOVE")


 End Sub

 Private Sub RadioButton4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton4.Click
  writeINI(sINIFile, "Shield", "Action", "LOCK")
 End Sub


 Private Sub RadioButton8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton8.Click
  writeINI(sINIFile, "Scanner", "Action", "REPORT")
 End Sub

 Private Sub RadioButton7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton7.Click
  writeINI(sINIFile, "Scanner", "Action", "DELETE")
 End Sub

 Private Sub RadioButton6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton6.Click
  writeINI(sINIFile, "Scanner", "Action", "MOVE")
 End Sub

 Private Sub RadioButton5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton5.Click
  writeINI(sINIFile, "Scanner", "Action", "LOCK")
 End Sub

 Private Sub ChkMastLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMastLoad.Click
  'автозагрузка
  If ChkMastLoad.Checked = True Then
   ChkMastLoad.Checked = False
   writeINI(sINIFile, "Shield", "Run", "False")
   Dim test9999 As RegistryKey = _
     Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
   test9999.DeleteValue("Belyash Shield")
   If Me.Timer2.Enabled = True Then
    Me.Timer2.Enabled = False
   End If
  Else
   ChkMastLoad.Checked = True

   writeINI(sINIFile, "Shield", "Run", "True")
   Const userRoot As String = "HKEY_LOCAL_MACHINE"
   Const subkey As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"
   Const keyName As String = userRoot & "\" & subkey
   Registry.SetValue(keyName, _
"Belyash Shield", Application.ExecutablePath, RegistryValueKind.ExpandString)
   If Me.Timer2.Enabled = False Then
    Me.Timer2.Enabled = True
   End If

  End If
 End Sub


 Private Sub StartMinimizedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartMinimizedToolStripMenuItem.Click
  'чекбокс..загрузка свернутым илил нет...меняются настройки
  If StartMinimizedToolStripMenuItem.Checked = True Then
   StartMinimizedToolStripMenuItem.Checked = False
   writeINI(sINIFile, "Shield", "Fon", "False")
  Else
   StartMinimizedToolStripMenuItem.Checked = True
   writeINI(sINIFile, "Shield", "Fon", "True")
  End If
 End Sub
 Sub vaultCript(ByVal oldpath As String)
  '*** FILE ENCRYPT ***'
  Try
   objCryptDES.Key = "3"
   objCryptDES.FileEncrypt(oldpath, Application.StartupPath & "\quarantine\" & IO.Path.GetFileName(oldpath) & ".CRP", True)
   'File.Delete(oldpath)
  Catch ex As Exception
   ErrorLog("vaultCript " & ErrorToString())
  End Try

 End Sub

 Private Sub chkVaultExt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVaultExt.Click
  If chkVaultExt.Checked = True Then

   writeINI(sINIFile, "Vault", "Cript", "False")
  End If
  If Trim(txtVaultExt.Text) = "" Then
   txtVaultExt.Text = "#??"
   writeINI(sINIFile, "Vault", "Extension", "#??")
  Else
   writeINI(sINIFile, "Vault", "Extension", Trim(txtVaultExt.Text))

  End If
 End Sub

 Private Sub chkVaultCript_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVaultCript.Click
  writeINI(sINIFile, "Vault", "Cript", "True")
 End Sub


   Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
      GlassBox.ShowMessage("Hello", "ggg", MessageBoxIcon.Asterisk, MessageBoxButtons.OK)
   End Sub

 Private Sub txtVaultExt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVaultExt.TextChanged
  On Error GoTo 100
  Dim i As Integer
  If Len(Trim(txtVaultExt.Text)) = 0 Then
   Exit Sub
  End If
  Dim b(13) As String
  b(0) = "&"
  b(1) = "?"
  b(2) = "*"
  b(3) = "%"
  b(4) = "!"
  b(5) = "@"
  b(6) = "^"
  b(7) = "("
  b(8) = ")"
  b(9) = "-"
  b(10) = "="
  b(11) = "+"
  b(12) = "_"
  Dim s As String = Trim(txtVaultExt.Text)
  ' Строка, которую ищем.
  For i = 0 To Len(Trim(txtVaultExt.Text))
   Dim find As String = b(i)
   ' Номер позиции найденного элемента.
   Dim pos As Int32 = 0
   Do
    ' Получаем позицию очередного элемента.
    pos = s.IndexOf(find, pos)
    ' Если что-то найдено...
    If pos <> -1 Then
     ' то показываем позицию найденного элемента.
     GlassBox.ShowMessage("Please replace this symbol." & vbCrLf & "               [" & b(i) & "]", "      Incorrect symbol", MessageBoxIcon.Error, MessageBoxButtons.OK)
     'MsgBox("Please replace this symbol." & vbCrLf & "               [" & b(i) & "]", MsgBoxStyle.Critical, "Incorrect symbol")
     Exit Sub
     ' Увеличиваем позицию поиска на длину строки для поиска.
     pos += find.Length
    End If
   Loop Until pos = -1
  Next
  b(13) = Nothing
  Exit Sub
100:
  GlassBox.ShowMessage(ErrorToString, "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)

 End Sub



 Private Sub PictureBox7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox7.Click
  On Error Resume Next
  If tmrMonitor.Enabled = True Then
   tmrMonitor.Enabled = False
   writeINI(sINIFile, "Shield", "Monitoring", "True")
  Else
   writeINI(sINIFile, "Shield", "Monitoring", "False")
  End If
  'If Timer_ico.Enabled = True Then
  'Timer_ico.Enabled = False
  'End If
  If Sbor.Enabled = True Then
   Sbor.Enabled = False
   Sbor.Dispose()
  End If
  If Timer2.Enabled = True Then
   Timer2.Enabled = False
   Timer2.Dispose()
  End If
  If tmrDevice.Enabled = True Then
   tmrDevice.Enabled = False
   tmrDevice.Dispose()
  End If
  If Timer1.Enabled = True Then
   Timer1.Enabled = False
   Timer1.Dispose()
  End If
  Me.Dispose()
  Application.Exit()
 End Sub

 Private Sub PictureBox14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox14.Click
  ' Me.WindowState = FormWindowState.Minimized
  If me_top = True Then
   'If MsgBox("", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
   If GlassBox.ShowMessage("Enabled options form on top. Send form in tray?", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Question, MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
    Me.Hide()
   End If

  Else
   Me.Hide()
  End If
 End Sub



 Private Sub PictureBox7_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox7.MouseLeave
  PictureBox7.Image = ImageList1.Images.Item(9)
  PictureBox7.BackColor = Color.Blue
 End Sub


 Private Sub PictureBox7_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox7.MouseMove
  PictureBox7.Image = ImageList1.Images.Item(10)
  PictureBox7.BackColor = Color.Red
 End Sub


 Private Sub PictureBox14_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox14.MouseLeave
  PictureBox14.Image = ImageList1.Images.Item(7)
  PictureBox14.BackColor = Color.Blue
 End Sub

 Private Sub PictureBox14_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox14.MouseMove
  PictureBox14.Image = ImageList1.Images.Item(12)
  PictureBox14.BackColor = Color.Red
 End Sub



 Private Sub Label17_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label17.Click
  sn_top()
  frmActivate.ShowDialog()
 End Sub



 Private Sub PictureBox15_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox15.MouseLeave
  PictureBox15.Image = ImageList1.Images.Item(8)
  PictureBox15.BackColor = Color.Blue
 End Sub

 Private Sub PictureBox15_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox15.MouseMove
  PictureBox15.Image = ImageList1.Images.Item(11)
  PictureBox15.BackColor = Color.Red
 End Sub


 Private Shared WinLocation As Point
 Private Sub PictureBox3_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox3.MouseDown
  WinLocation = e.Location
 End Sub

 Private Sub PictureBox3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox3.MouseMove
  If String.Compare(Control.MouseButtons.ToString(), "Left") = 0 Then
   Dim MSize As New Size(WinLocation)
   MSize.Width = e.X - WinLocation.X
   MSize.Height = e.Y - WinLocation.Y
   Me.Location = Point.Add(Me.Location, MSize)
  End If

 End Sub


 Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
  stopMon()

 End Sub


 Private Sub PictureBox15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox15.Click
  GlassBox.ShowMessage("Form dont maximized", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Information, MessageBoxButtons.OK)
 End Sub

 Private Sub PictureBox19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox19.Click
  TabControl1.SelectTab(5)
 End Sub

 Private Sub Label46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label46.Click
  TabControl1.SelectTab(5)
 End Sub

 Private Sub PictureBox20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox20.Click
  TabControl1.SelectTab(7)
 End Sub

 Private Sub Label47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label47.Click
  TabControl1.SelectTab(7)
 End Sub


 Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
  Try
   REGFound.Items.Clear()
   scanReg = True
   SubtractFromCounter2()
   GlobalErrorRegistry = False
   Label58.Text = "    Scanning...."
   Label58.ImageKey = "check.PNG"
   Button4.Enabled = True
   Button5.Enabled = False
   chk_reg_funct()
   'MyLibrary.FormFunction.
   txtRegistry.Text = ""
   Dim tmpint As Long = CInt(sGetINI(Application.StartupPath & "\reg.INI", "Main", "Count", "0"))
   KolwoREG = tmpint
   If tmpint = 0 Or iregCount > tmpint Then
    'MsgBox("done")
    'Label58.Text = "    Done...."
    GoTo 15
   End If
   Do While iregCount < tmpint
    iregCount = iregCount + 1
    Label80.Text = iregCount
    searchreg()
   Loop
15:
   chk_allReg()
   
   scanReg = False
   Button4.Enabled = False
   Button5.Enabled = True
   Label58.Text = "    Done...."
   Label58.ImageKey = "uncheck.PNG"
   ProgressBar2.Style = ProgressBarStyle.Blocks
  Catch ex As Exception
   ErrorLog("Button5_Click " & ErrorToString())
  End Try
 End Sub
 Sub chk_allReg()
  Label58.Text = "    Scanning...."
  Label58.ImageKey = "check.PNG"
  GlobalErrorRegistry = True
  cReg.RootKey = 0
  'Don't search in any specific subkey (Search in all subkeys)
  cReg.SubKey = ""
  'Only find errors in value names and value values
  'cReg.SearchFlags = KEY_NAME * 0 + VALUE_NAME * 1 + VALUE_VALUE * 1 + WHOLE_STRING * 0
  cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 1 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 0
  'Search for registry values with the suffix "C:\"
  cReg.SearchString = My.Application.GetEnvironmentVariable("HOMEDRIVE")
  ' MsgBox(My.Application.GetEnvironmentVariable("HOMEDRIVE"))
  'Start searching for invalid registry values
  cReg.DoSearch()
 End Sub
 Private Sub cReg_SearchKeyChanged(ByVal sFullKeyName As String) Handles cReg.SearchKeyChanged
  'Note: This event cause a lot of printing.
  'To increase performance remove this event.
  txtRegistry.Text = sFullKeyName
 End Sub
 Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
  scanReg = False
  cReg.StopSearch()
 End Sub

 Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
  Try


   Dim i As Integer
   For i = 0 To REGFound.Items.Count - 1
    'REGFound.CheckedItems(i).Checked = True
    REGFound.Items(i).Checked = True

   Next
  Catch ex As Exception
   MsgBox(ex.ToString)
  End Try

 End Sub

 Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
  Try
   Dim i As Integer
   For i = 0 To REGFound.Items.Count - 1
    REGFound.Items(i).Checked = False
   Next
  Catch ex As Exception
   MsgBox(ex.ToString)
  End Try
 End Sub
 '=================REG===================

 Private Sub cReg_SearchFinished(ByVal lReason As Integer) Handles cReg.SearchFinished
  If lReason = 0 Then
   Label58.Text = "  Done!"

  ElseIf lReason = 1 Then
   Label3.Text = "  Terminated by user!"
  Else
   Label58.Text = "  An Error occured! Err number = " & lReason
   Err.Raise(lReason)
  End If

 End Sub

 Private Sub cReg_SearchFound(ByVal sRootKey As String, ByVal sKey As String, ByVal sValue As Object, ByVal lFound As cRegSearch.FOUND_WHERE) Handles cReg.SearchFound
  'Dim sTemp As Object
  Try

  
   Dim sTemp As String = ""
   Select Case lFound
    Case cRegSearch.FOUND_WHERE.FOUND_IN_KEY_NAME
     'UPGRADE_WARNING: Couldn't resolve default property of object sTemp. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
     sTemp = "KEY_NAME"
    Case cRegSearch.FOUND_WHERE.FOUND_IN_VALUE_NAME
     'UPGRADE_WARNING: Couldn't resolve default property of object sTemp. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
     sTemp = "VALUE NAME"
    Case cRegSearch.FOUND_WHERE.FOUND_IN_VALUE_VALUE
     'UPGRADE_WARNING: Couldn't resolve default property of object sTemp. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
     sTemp = "VALUE VALUE"
   End Select
   'With ListView1
   ' lvItm = .ListItems.Add(, , sTemp)
   'lvItm.SubItems(1) = sRootKey
   ' lvItm.SubItems(2) = sKey
   'UPGRADE_WARNING: Couldn't resolve default property of object sValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
   ' lvItm.SubItems(3) = sValue
   'End With

   'If MyLibrary.FormFunction.FileorFolderExists(CStr(sValue)) = False Then 'not exist => invalid key
   'Dim li As New ListViewItem(sTemp, 5)
   If GlobalErrorRegistry = False Then
    Dim li As New ListViewItem(key, 5)
    li.SubItems.Add(sRootKey)
    li.SubItems.Add(sKey)
    li.SubItems.Add(sValue)
    li.SubItems.Add(VirReg)
    REGFound.Items.Add(li)
    ireGFound = ireGFound + 1

    txtRegFound.Text = ireGFound
   Else
    If chkLegalSymbol(sValue) = True Then
     GoTo 18
    End If
    If MyLibrary.FormFunction.FileorFolderExists(sValue) = False Then
     'поиск ошибок в реестре
     Dim li As New ListViewItem(key, 5)
     li.SubItems.Add(sRootKey)
     li.SubItems.Add(sKey)
     li.SubItems.Add(sValue)
     li.SubItems.Add("Error structure registry")
     REGFound.Items.Add(li)
     ireGFound = ireGFound + 1
     txtRegFound.Text = ireGFound
     LogPrint("Registry", sRootKey & "\" & sKey & "\" & key & "=" & sValue & "- error registry strukture")
    End If
18:
   End If
 
  Catch ex As Exception
   ErrorLog("cReg_SearchFound " & ErrorToString())
  End Try
 End Sub
 Function chkLegalSymbol(ByVal adtemp As String) As Boolean
  chkLegalSymbol = False
  On Error GoTo 100
  Dim i As Integer
  If Len(Trim(adtemp)) = 0 Then
   Exit Function
  End If
  Dim b(14) As String
  b(0) = "&"
  b(1) = "?"
  b(2) = "*"
  b(3) = "_"
  b(4) = "!"
  b(5) = "@"
  b(6) = "^"
  b(7) = "("
  b(8) = ")"
  b(9) = "-"
  b(10) = "="
  b(11) = "+"
  b(12) = ","
  b(13) = "/"
  b(14) = Chr(34)
  Dim s As String = Trim(adtemp)
  ' Строка, которую ищем.
  For i = 0 To Len(Trim(adtemp))
   Dim find As String = b(i)
   ' Номер позиции найденного элемента.
   Dim pos As Int32 = 0
   Do
    ' Получаем позицию очередного элемента.
    pos = s.IndexOf(find, pos)
    ' Если что-то найдено...
    If pos <> -1 Then
     ' то показываем позицию найденного элемента.
     'MsgBox(b(i))
     chkLegalSymbol = True
     Exit Function
     ' Увеличиваем позицию поиска на длину строки для поиска.
     pos += find.Length
    End If
   Loop Until pos = -1
  Next
  b(14) = Nothing
  Exit Function
100:
  'GlassBox.ShowMessage(ErrorToString, "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Error, MessageBoxButtons.OK)
 End Function
 Sub start_search(ByVal kust As String, ByVal vetka As String, ByVal key As String, ByVal znach As String)
  Select Case kust
   Case "HKEY_LOCAL_MACHINE"

    cReg.RootKey = cRegSearch.ROOT_KEYS.HKEY_LOCAL_MACHINE
   Case "HKEY_CURRENT_USER"
    cReg.RootKey = cRegSearch.ROOT_KEYS.HKEY_CURRENT_USER
   Case "HKEY_ALL"
    cReg.RootKey = cRegSearch.ROOT_KEYS.HKEY_ALL
   Case "HKEY_CLASSES_ROOT"
    cReg.RootKey = cRegSearch.ROOT_KEYS.HKEY_CLASSES_ROOT
   Case "HKEY_USERS"
    cReg.RootKey = cRegSearch.ROOT_KEYS.HKEY_USERS
   Case "HKEY_PERFORMANCE_DATA"
    cReg.RootKey = cRegSearch.ROOT_KEYS.HKEY_PERFORMANCE_DATA
   Case "HKEY_CURRENT_CONFIG"
    cReg.RootKey = cRegSearch.ROOT_KEYS.HKEY_CURRENT_CONFIG
   Case "HKEY_DYN_DATA"
    cReg.RootKey = cRegSearch.ROOT_KEYS.HKEY_DYN_DATA
  End Select
  cReg.SubKey = vetka
  Dim topt As Integer = CInt(sGetINI(Application.StartupPath & "\reg.INI", CStr(iregCount), "options", "33"))

  Select Case topt
   Case "1"
    cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 0 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 0
   Case "2"
    cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 0 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 0
   Case "3"
    cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 1 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 0
   Case "12"
    cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 0 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 0
   Case "13"
    cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 1 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 0
   Case "23"
    cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 1 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 0
   Case "33"
    cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 1 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 1
   Case "0"
    cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 0 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 1
  End Select
  cReg.SearchFlags = cRegSearch.SEARCH_FLAGS.KEY_NAME * 0 + cRegSearch.SEARCH_FLAGS.VALUE_NAME * 1 + cRegSearch.SEARCH_FLAGS.VALUE_VALUE * 1 + cRegSearch.SEARCH_FLAGS.WHOLE_STRING * 0
  cReg.SearchString = key
  cReg.DoSearch()
 End Sub
 Sub searchreg()
  Dim kust As String = sGetINI(Application.StartupPath & "\reg.INI", CStr(iregCount), "kust", "HKEY_ALL")

  Dim vetka As String = sGetINI(Application.StartupPath & "\reg.INI", CStr(iregCount), "vetka", "")

  key = sGetINI(Application.StartupPath & "\reg.INI", CStr(iregCount), "key", "")

  Dim znach As String = sGetINI(Application.StartupPath & "\reg.INI", CStr(iregCount), "znach", "")
  VirReg = sGetINI(Application.StartupPath & "\reg.INI", CStr(iregCount), "Name", "")
  'searchReg(kust, vetka, key, znach)
  start_search(kust, vetka, key, znach)
 End Sub
 '=================
 Sub zapretZapuckaProg()
  Dim imgOP As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options", True)
  For Each k As String In imgOP.GetSubKeyNames


   If Trim(k) <> "" Then
    Call gtzapretpr(k)
   End If
  Next
 End Sub
 Sub gtzapretpr(ByVal f As String)
  ' On Error GoTo 101
  Const userRoot1 As String = "HKEY_LOCAL_MACHINE"
  Dim subkey1 As String = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & f
  Dim keyName1 As String = userRoot1 & "\" & subkey1
  Dim tExpand3 As String = Registry.GetValue(keyName1, _
       "Debugger", RegistryValueKind.String)

  'If LCase(tExpand3) <> LCase(zna4) Then
  ' Registry.SetValue(keyName1, _
  ' keySod, zna4)
  If tExpand3 = "ntsd -d" Then
   ' My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & f)
   ' My.Computer.Registry.LocalMachine.Close()
   ireGFound = ireGFound + 1
   txtRegFound.Text = ireGFound
   Dim li As New ListViewItem("Debugger", 4)
   li.SubItems.Add("HKEY_LOCAL_MACHINE")
   li.SubItems.Add(subkey1)
   li.SubItems.Add("ntsd - d")
   li.SubItems.Add("Запрет запуска программ(Опасность)")
   REGFound.Items.Add(li)
  End If

  'End If
  Exit Sub
101:
  MsgBox(ErrorToString, MsgBoxStyle.Critical)
 End Sub

 

 Private Sub chkFixReg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFixReg.Click
  writeINI(sINIFile, "Registry", "CureAuto", chkFixReg.Checked)
 End Sub

 Private Sub chkBackup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBackup.Click
  writeINI(sINIFile, "Registry", "Backup", chkBackup.Checked)

 End Sub

 Private Sub FirewallToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FirewallToolStripMenuItem1.Click
  TabControl1.SelectTab(4)
  RestoreWindow()
 End Sub

 Private Sub RegistryToolFixToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegistryToolFixToolStripMenuItem.Click
  TabControl1.SelectTab(3)
  RestoreWindow()
 End Sub

 Private Sub ShieldToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShieldToolStripMenuItem1.Click
  viw_external(Application.StartupPath & "\otchetMon.log")
 End Sub

 Private Sub ScannerToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScannerToolStripMenuItem1.Click
  'scan
  viw_external(Application.StartupPath & "\otchetScan.log")
 End Sub

 Private Sub RegistryToolFixToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegistryToolFixToolStripMenuItem1.Click
  'reg
  viw_external(Application.StartupPath & "\registryScan.log")
 End Sub

 Private Sub FirewallToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FirewallToolStripMenuItem2.Click
  'firewal
  viw_external(Application.StartupPath & "\otchetFirewall.log")
 End Sub

 Private Sub UpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateToolStripMenuItem.Click
  'update
  viw_external(Application.StartupPath & "\otchetUpdate.log")
 End Sub

 Private Sub VirusVaultToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VirusVaultToolStripMenuItem.Click
  'vault
  viw_external(Application.StartupPath & "\quarantine\MovedFiles.LOG")
 End Sub
  
 Private Sub ErrorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ErrorToolStripMenuItem.Click
  viw_external(Application.StartupPath & "\ERRORLOG.LOG")
 End Sub

 Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
  TabControl1.SelectTab(8)
  RestoreWindow()
 End Sub

 
 Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
  Try
   Dim i As Integer
   For i = 0 To REGFound.Items.Count - 1
    If REGFound.Items(i).Checked = True Then
     del_vetka(REGFound.Items(i).Text, REGFound.Items(i).SubItems(1).Text, REGFound.Items(i).SubItems(2).Text)
     REGFound.Items(i).Remove()
     ireGDelete = ireGDelete + 1
     Label79.Text = ireGDelete
    End If
   Next
  Catch ex As Exception
   ErrorLog("Button13_Click " & ErrorToString())
  End Try
 End Sub
 Sub del_vetka(ByVal key As String, ByVal rrot As String, ByVal vetka As String)
  Try

   Select Case rrot
    Case "HKEY_CURRENT_USER"
     Dim test9999 As RegistryKey = _
                 Registry.CurrentUser.OpenSubKey(vetka, True)
     test9999.DeleteValue(key)
     test9999.Close()
    Case "HKEY_CLASSES_ROOT"
     Dim test9999 As RegistryKey = _
               Registry.ClassesRoot.OpenSubKey(vetka, True)
     test9999.DeleteValue(key)
     test9999.Close()
    Case "HKEY_USERS"
     Dim test9999 As RegistryKey = _
               Registry.Users.OpenSubKey(vetka, True)
     test9999.DeleteValue(key)
     test9999.Close()
    Case "HKEY_PERFORMANCE_DATA"
     Dim test9999 As RegistryKey = _
               Registry.PerformanceData.OpenSubKey(vetka, True)
     test9999.DeleteValue(key)
     test9999.Close()
    Case "HKEY_CURRENT_CONFIG"
     Dim test9999 As RegistryKey = _
               Registry.CurrentConfig.OpenSubKey(vetka, True)
     test9999.DeleteValue(key)
     test9999.Close()
    Case "HKEY_DYN_DATA"
     Dim test9999 As RegistryKey = _
               Registry.DynData.OpenSubKey(vetka, True)
     test9999.DeleteValue(key)
     test9999.Close()
    Case "HKEY_LOCAL_MACHINE"
     Dim test9999 As RegistryKey = _
               Registry.LocalMachine.OpenSubKey(vetka, True)
     test9999.DeleteValue(key)
     test9999.Close()
   End Select


  Catch ex As Exception
   ErrorLog("del_vetka " & ErrorToString())
  End Try
 End Sub
 Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
  REGFound.Items.Clear()
 End Sub

 Private Sub Label82_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label82.Click
  clear_StatistShield()
 End Sub

 
 Private Sub ShieldToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShieldToolStripMenuItem.Click
  sn_top()
  Nastroyka.ShowDialog()
 End Sub

 Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click

 End Sub

 Private Sub LinkLabel6_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel6.LinkClicked
  Nastroyka.ShowDialog()
 End Sub

   Private Sub EnableBlockerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnableBlockerToolStripMenuItem.Click
      'автозагрузка
      If EnableBlockerToolStripMenuItem.Checked = True Then
         EnableBlockerToolStripMenuItem.Checked = False
         writeINI(sINIFile, "Blocker", "Enable", "False")
         UNreg_blocker()
      Else
         If File.Exists(Application.StartupPath & "\Blocker.exe") = True Then
            EnableBlockerToolStripMenuItem.Checked = True
            reg_blocker()
         End If
      End If
   End Sub
   Public Sub UNreg_blocker()
      Try
         Const userRoot As String = "HKEY_CLASSES_ROOT"
         Const subkey As String = "exefile\shell\open\command"
         '"HKCR\exefile\shell\open\command\", App.Path & "\AppBlock.EXE %1 %*"
         Const keyName As String = userRoot & "\" & subkey
         Registry.SetValue(keyName, _
       "", Chr(34) + "%1" + Chr(34) + " %*", RegistryValueKind.String)
      Catch ex As Exception
         ErrorLog("UNreg_blocker " & ErrorToString())
      End Try
   End Sub
   Public Sub reg_blocker()
      Try
         writeINI(sINIFile, "Blocker", "Enable", "True")
         Const userRoot As String = "HKEY_CLASSES_ROOT"
         Const subkey As String = "exefile\shell\open\command"
         Const keyName As String = userRoot & "\" & subkey
         Registry.SetValue(keyName, _
       "", Application.StartupPath & "\Blocker.exe %1 %*", RegistryValueKind.String)
      Catch ex As Exception
         ErrorLog("reg_blocker " & ErrorToString())
      End Try
   End Sub
End Class
