Imports System.Threading
Imports System.IO
Imports Microsoft.Win32

Module Module1
 Public scanPath As String

 Public inputArgument As String = "/path="
 Dim MyLibrary As New MyLibrary.MyLib
 Delegate Sub TaskDelegate()
 Dim tmpBasekolwo As Long


 Sub Main()
  'главная проца
  Console.Title = ""
  Try
   MyPath = My.Computer.FileSystem.CurrentDirectory
   QuarantinePath = MyPath & "\quarantine"
   If Trim(MyPath) = "" Then
    Console.WriteLine("Dont work now. Interrupt")
    End
   End If
   Dim t12 As New System.Threading.Thread(AddressOf CountSheep12)
   t12.Start()

   chk_first_ST9()
   mi_prior()
   Console.ForegroundColor = ConsoleColor.Red
   chk_quar()

   Console.WriteLine("-------------------------------------------------------------------------------")
   LogPrint("-------------------------------------------------------------------------------")
   Console.WriteLine("Belyash Anti-Trojan 2009b v." & My.Application.Info.Version.ToString)
   LogPrint("Belyash Anti-Trojan 2009b v." & My.Application.Info.Version.ToString)
   gt_dll()
   check_registeredAppl()
   check_lastUpdate()
   Console.CursorTop = 8
   Console.CursorLeft = 0
   Console.WriteLine("Virus Record:")
   Console.WriteLine("-------------------------------------------------------------------------------")
   Dim t As New System.Threading.Thread(AddressOf CountSheep)
   t.Start()
   tmpBasekolwo = MyLibrary.FormFunction.ScanFileBase1(MyPath)
   t.Abort()
   Console.CursorTop = 8
   Console.CursorLeft = 13
   Console.WriteLine(tmpBasekolwo)
   LogPrint("Virus Record:" & CStr(tmpBasekolwo))
   LogPrint("-------------------------------------------------------------------------------")
   t = Nothing
   Console.ForegroundColor = ConsoleColor.White
   ParseCommandLineArgs()
   Console.ForegroundColor = ConsoleColor.Cyan
   Console.WriteLine("PRESS ANY KEY TO CONTINUE...")
   Console.ReadKey()
   t12.Abort()
   t12 = Nothing
  Catch ex As IOException
   ErrorLog("Main " & ErrorToString())
   Console.WriteLine(ErrorToString)
  End Try
 End Sub
 Sub chk_quar()
  On Error GoTo 100
  If Directory.Exists(QuarantinePath) = False Then
   Directory.CreateDirectory(QuarantinePath)
  End If
  Exit Sub
100:
  ErrorLog("chk_quar " & ErrorToString())
 End Sub
 Private Sub theLongRunningTask3()
  Dim progressForm As New reg2()
  progressForm.chk_2(tmpBasekolwo)

 End Sub
 Public Sub CountSheep12()
  Do While (True) ' Endless loop.
   Dim Text As String = "Belyash Anti-Trojan 2009b v." & My.Application.Info.Version.ToString
   Static x As Integer
   x = x + 1
   If x > Len(Text) Then
    x = 1
    Console.Title = ""
   End If
   Console.Title = Console.Title & Mid(Text, x, 1)
   'Console.Title = "Belyash Anti-Trojan 2009b Console Scanner"
   Thread.Sleep(100)
   ' System.Threading.Thread.Sleep(100) 'Wait 1 second.
  Loop

  'Timer3.Enabled = True

  'Me.Refresh()
 End Sub

 Public Sub CountSheep()
  Dim a(4) As String
  Dim i As Integer
  a(0) = "-"
  a(1) = "\"
  a(2) = "|"
  a(3) = "/"
  ' Dim i As Integer = 1 ' Sheep do not count from 0.
  Do While (True) ' Endless loop.
   For i = 0 To 3
    Console.CursorTop = 8
    Console.CursorLeft = 13
    Console.WriteLine(Trim(a(i)))
    Thread.Sleep(10)
   Next
   ' System.Threading.Thread.Sleep(100) 'Wait 1 second.
  Loop
 End Sub


 Sub ParseCommandLineArgs()
  On Error GoTo 100
  Dim inputArgument As String = "/path="
  Dim inputName As String = ""
  Dim comline As String = ""
  Dim countArg As Integer = 0
  For Each s As String In My.Application.CommandLineArgs
   comline = comline & " " & s
   countArg = +1
   Select Case Trim(LCase(s))
    Case "/help"
     Print_help()
     Console.WriteLine("PRESS ANY KEY TO CONTINUE...")
     Console.ReadKey()
     End
    Case "/?"
     Print_help()
     Console.WriteLine("PRESS ANY KEY TO CONTINUE...")
     Console.ReadKey()
     End
    Case "/*"
     scanPath = "*"
     'Thread.Sleep(500)
    Case "/rp"
     writeINI(sINIFile, "Console", "Action", "REPORT")
    Case "/move"
     writeINI(sINIFile, "Console", "Action", "MOVE")
    Case "/del"
     writeINI(sINIFile, "Console", "Action", "DELETE")
    Case "/lock"
     writeINI(sINIFile, "Console", "Action", "LOCK")
    Case "/urp"
     writeINI(sINIFile, "Console", "ActionUncknow", "REPORT")
    Case "/smove"
     writeINI(sINIFile, "Console", "ActionUncknow", "MOVE")
    Case "/udel"
     writeINI(sINIFile, "Console", "ActionUncknow", "DELETE")
    Case "/ulock"
     writeINI(sINIFile, "Console", "ActionUncknow", "LOCK")
    Case "/alno"
     writeINI(sINIFile, "Console", "SCANALL", "False")
    Case "/ask"
     writeINI(sINIFile, "Console", "Ask", "True")
    Case "/ar"
     writeINI(sINIFile, "Console", "CheckZip", "True")
    Case "/st"
     writeINI(sINIFile, "Console", "ScanAutoStart", "True")
    Case "/log"
     writeINI(sINIFile, "Console", "Log", "True")
    Case "/heur"
     writeINI(sINIFile, "Console", "ScanHeur", "True")
    Case "/reg"
     writeINI(sINIFile, "Console", "CheckRegistry", "True")
    Case "/logfull"
     writeINI(sINIFile, "Console", "Izbitochnoe", "True")
    Case "/nomem"
     writeINI(sINIFile, "Console", "ScanMemory", "False")
    Case "/mem"
     writeINI(sINIFile, "Console", "ScanMemory", "True")
    Case "/pr0"
     writeINI(sINIFile, "Console", "Priority", "Idle")
    Case "/pr1"
     writeINI(sINIFile, "Console", "PRIORITY", "Normal")
    Case "/pr2"
     writeINI(sINIFile, "Console", "Priority", "Hight")
    Case "/pr3"
     writeINI(sINIFile, "Console", "Priority", "RealTime")
    Case Else
     If Trim(s) <> "" Then
      If s.ToLower.StartsWith(inputArgument) Then
       inputName = s.Remove(0, inputArgument.Length)
       scanPath = inputName
       'Console.WriteLine("Check path " & IO.Path.GetFullPath(inputName))
      Else
       scanPath = "*"

       'Thread.Sleep(500)
      End If
     End If
   End Select
  Next
  If Trim(comline) = "" Then
   'Console.WriteLine("Command line arguments is not valid.")
   writeINI(sINIFile, "Console", "Action", "REPORT")
   writeINI(sINIFile, "Console", "ActionUncknow", "REPORT")
   writeINI(sINIFile, "Console", "SCANALL", "True")
   writeINI(sINIFile, "Console", "Ask", "False")
   writeINI(sINIFile, "Console", "CheckZip", "True")
   writeINI(sINIFile, "Console", "ScanAutoStart", "True")
   writeINI(sINIFile, "Console", "Log", "True")
   writeINI(sINIFile, "Console", "ScanHeur", "True")
   writeINI(sINIFile, "Console", "CheckRegistry", "True")
   writeINI(sINIFile, "Console", "Izbitochnoe", "True")
   writeINI(sINIFile, "Console", "Priority", "Normal")
   writeINI(sINIFile, "Console", "ScanMemory", "True")
   writeINI(sINIFile, "Console", "NoWindow", "False")
   Console.WriteLine("")
   Console.WriteLine("Command line /st /ar /alno /urp /rp /pr2 /logfull /reg /heur /log /nomem")
   Console.WriteLine("")
   Thread.Sleep(1000)
   START_SCANALL()
  Else
   Console.WriteLine("")
   Console.WriteLine("Command line" & comline)
   If scanPath = "*" Then
    START_SCANALL()
   Else
    Custom_scann(scanPath)
   End If
  End If
  Exit Sub
100:
  ErrorLog("ParseCommandLineArgs " & ErrorToString())
  Console.WriteLine(ErrorToString)

 End Sub

 Sub Custom_scann(ByVal argtoscan As String)
  'сканировать указанный каталог
  Try

   If Trim(argtoscan) <> "" And My.Computer.FileSystem.DirectoryExists(argtoscan) = True Then
    Console.ForegroundColor = ConsoleColor.White
    Console.WriteLine("Start scanning at " & Format(Now, "dd:MM:yy HH:mm:ss"))
    LogPrint("Start scanning at " & Format(Now, "dd:MM:yy HH:mm:ss"))
    Dim startMil As Long = 0
    Dim StartSec As Long = 0
    Dim StartMin As Long = 0
    Dim StartHour As Long = 0
    Dim EndMil As Long = 0
    Dim EndSec As Long = 0
    Dim EndMin As Long = 0
    Dim EndHour As Long = 0
    Dim z10 As Long = 0
    Dim z11 As Long = 0
    Dim z12 As Long = 0
    Dim z13 As Long = 0
    startMil = (DateTime.Now.Millisecond)
    StartSec = (DateTime.Now.Second)
    StartMin = (DateTime.Now.Minute)
    StartHour = (DateTime.Now.Hour)
    Dim tmpMem As Boolean = sGetINI(sINIFile, "Console", "ScanMemory", "True")
    If tmpMem = True Then
     'Dim t2 As New System.Threading.Thread(AddressOf GetAllProcesses)
     't2.Priority = ThreadPriority.Lowest
     't2.Start() 'проверить память
     GetAllProcesses() 'проверить память
    End If
    AUto_run_Thismashine()
    get_drive()
    ScanScanner(argtoscan)
    Dim tmpMem2 As Boolean = CBool(sGetINI(sINIFile, "Console", "ScanAutoStart", "False"))
    If tmpMem2 = True Then
     chk_reg_funct()
     AUto_run_Thismashine()
    End If
    Dim tmpMem3 As Boolean = CBool(sGetINI(sINIFile, "Console", "CheckRegistry", "False"))
    If tmpMem3 = True Then
     chk_reg_funct()
    End If


    Console.WriteLine("End scanning at " & Format(Now, "dd:MM:yy HH:mm:ss"))
    LogPrint("End scanning at " & Format(Now, "dd:MM:yy HH:mm:ss"))
    Console.ForegroundColor = ConsoleColor.Red
    Console.WriteLine("-------------------------------------------------------------------------------")
    Console.WriteLine("Scaning:" & CStr(inCount))
    Console.WriteLine("Infected:" & CStr(inFound))
    Console.WriteLine("Cure:" & CStr(inCure))
    Console.WriteLine("Delete:" & CStr(inDELETE))
    Console.WriteLine("Move:" & CStr(inMove))
    Console.WriteLine("No cure:" & CStr(inNonCure))
    Console.WriteLine("Error count:" & inError)

    EndMil = DateTime.Now.Millisecond
    EndSec = DateTime.Now.Second
    EndMin = Date.Now.Minute
    EndHour = DateTime.Now.Hour

    If EndMil >= startMil Then
     z10 = EndMil - startMil
    End If
    If EndSec >= StartSec Then
     z11 = EndSec - StartSec
    End If
    If EndMin >= StartMin Then
     z12 = EndMin - StartMin
    End If
    If EndHour >= StartHour Then
     z13 = EndHour - StartHour
    End If
    Console.WriteLine("Time: " & z13 & ":" & z12 & ":" & z11 & ":" & z10)
    Console.WriteLine("-------------------------------------------------------------------------------")
    LogPrint("-------------------------------------------------------------------------------")
    LogPrint("Scaning:" & CStr(inCount))
    LogPrint("Infected:" & CStr(inFound))
    LogPrint("Cure:" & CStr(inCure))
    LogPrint("Delete:" & CStr(inDELETE))
    LogPrint("Move:" & CStr(inMove))
    LogPrint("No cure:" & CStr(inNonCure))
    LogPrint("Error count:" & inError)
    LogPrint("Time: " & z13 & ":" & z12 & ":" & z11 & ":" & z10)

    LogPrint("-------------------------------------------------------------------------------")
    Console.ForegroundColor = ConsoleColor.White
   End If

  Catch ex As IOException
   ErrorLog("Custom_scann " & ErrorToString())
   Console.WriteLine(ErrorToString)
  End Try

 End Sub
 Sub get_drive()
  Dim drives() As String
  Dim aDrive As String
  drives = Directory.GetLogicalDrives()
  For Each aDrive In drives
   getAutorun(aDrive)
  Next
 End Sub
 Sub START_SCANALL()
  Try
   Console.ForegroundColor = ConsoleColor.White
   Console.WriteLine("Start scanning at " & Format(Now, "dd:MM:yy HH:mm:ss"))
   LogPrint("Start scanning at " & Format(Now, "dd:MM:yy HH:mm:ss"))
   Dim startMil As Long = 0
   Dim StartSec As Long = 0
   Dim StartMin As Long = 0
   Dim StartHour As Long = 0
   Dim EndMil As Long = 0
   Dim EndSec As Long = 0
   Dim EndMin As Long = 0
   Dim EndHour As Long = 0
   Dim z10 As Long = 0
   Dim z11 As Long = 0
   Dim z12 As Long = 0
   Dim z13 As Long = 0
   startMil = (DateTime.Now.Millisecond)
   StartSec = (DateTime.Now.Second)
   StartMin = (DateTime.Now.Minute)
   StartHour = (DateTime.Now.Hour)
   Dim tmpMem As Boolean = sGetINI(sINIFile, "Console", "ScanMemory", "True")
   If tmpMem = True Then
    ' Dim t1 As New System.Threading.Thread(AddressOf GetAllProcesses)
    ' t1.Start() 'проверить память
    GetAllProcesses()
   End If
   Dim tmpMem2 As Boolean = CBool(sGetINI(sINIFile, "Console", "ScanAutoStart", "False"))
   If tmpMem2 = True Then
    chk_reg_funct()
    AUto_run_Thismashine()
   End If
   Dim tmpMem3 As Boolean = CBool(sGetINI(sINIFile, "Console", "CheckRegistry", "False"))
   If tmpMem3 = True Then
    chk_reg_funct()
   End If
   Dim drives() As String
   Dim aDrive As String
   drives = Directory.GetLogicalDrives()
   For Each aDrive In drives

    ScanScanner(aDrive)
   Next
   get_drive()
   Console.WriteLine("End scanning at " & Format(Now, "dd:MM:yy HH:mm:ss"))
   LogPrint("End scanning at " & Format(Now, "dd:MM:yy HH:mm:ss"))
   Console.ForegroundColor = ConsoleColor.Red
   EndMil = DateTime.Now.Millisecond
   EndSec = DateTime.Now.Second
   EndMin = Date.Now.Minute
   EndHour = DateTime.Now.Hour

   If EndMil >= startMil Then
    z10 = EndMil - startMil
   End If
   If EndSec >= StartSec Then
    z11 = EndSec - StartSec
   End If
   If EndMin >= StartMin Then
    z12 = EndMin - StartMin
   End If
   If EndHour >= StartHour Then
    z13 = EndHour - StartHour
   End If
   Console.WriteLine("-------------------------------------------------------------------------------")
   Console.WriteLine("Scaning:" & CStr(inCount))
   Console.WriteLine("Infected:" & CStr(inFound))
   Console.WriteLine("Cure:" & CStr(inCure))
   Console.WriteLine("Delete:" & CStr(inDELETE))
   Console.WriteLine("Move:" & CStr(inMove))
   Console.WriteLine("No cure:" & CStr(inNonCure))
   Console.WriteLine("Error count:" & inError)
   Console.WriteLine("Time: " & z13 & ":" & z12 & ":" & z11 & ":" & z10)
   Console.WriteLine("-------------------------------------------------------------------------------")
   LogPrint("-------------------------------------------------------------------------------")
   LogPrint("Scaning:" & CStr(inCount))
   LogPrint("Infected:" & CStr(inFound))
   LogPrint("Cure:" & CStr(inCure))
   LogPrint("Delete:" & CStr(inDELETE))
   LogPrint("Move:" & CStr(inMove))
   LogPrint("No cure:" & CStr(inNonCure))
   LogPrint("Error count:" & inError)
   LogPrint("Time: " & z13 & ":" & z12 & ":" & z11 & ":" & z10)
   LogPrint("-------------------------------------------------------------------------------")
   Console.ForegroundColor = ConsoleColor.White
  Catch ex As IOException
   ErrorLog("START_SCANALL " & ErrorToString())
   Console.WriteLine(ErrorToString)
  End Try

 End Sub
 Public Sub chk_first_ST9()
  'проверить есть ли файл настроек..Если нет..то создать дефолтные настройки

  If File.Exists(MyPath & "\SETTINGS.INI") = False Then
   'Console.WriteLine(MyPath)
   'Stop
   first_start_registry2()
  End If

 End Sub
 Sub Print_help()
  Console.WriteLine("ConScanner.exe [/key][/path=PATH]")
  Console.WriteLine("/rp - Only report for infected files.")
  Console.WriteLine("/urp - Only report for suspicious files.")
  Console.WriteLine("/move - Move to quarantine infected files.")
  Console.WriteLine("/smove - Move to quarantine suspicious files.")
  Console.WriteLine("/del - Delete infected files.")
  Console.WriteLine("/udel - Delete suspicious files.")
  Console.WriteLine("/lock - Lock infected files.")
  Console.WriteLine("/ulock - Lock suspicious files.")
  Console.WriteLine("/help - display short help on the program.")
  Console.WriteLine("/? - display short help on the program.")
  Console.WriteLine("/* - to scan all files in the all device.")
  Console.WriteLine("/ar - to scan files inside the archives. At present, the scanning of archives (without curing) created by the ZIP,GZIP,TAR,RAR,ARJ,LZH,LHA,CAB, etc. ")
  Console.WriteLine("/st - scan autostart programs.")
  Console.WriteLine("/na - scan archive.")
  Console.WriteLine("/alno - instruct to check only those files whose extensions. (*.exe,*.com,*.vbs,*.js,*.inf,*.dll,*.vxd,*.sys,*.cmd,*.386,*.bat,*.bin,*.chm,*.html,*.htm,*.mht,*.cpl,*.drv,*.pif,*.hlp,*.scr,*.ocx,*.eml,*.asp)")
  Console.WriteLine("/reg - scan and fix registry.")
  Console.WriteLine("/logfull - log to file advansed information.")
  Console.WriteLine("/log - create log file.")
  Console.WriteLine("/heur - enable the heuristic analyzer ")
  Console.WriteLine("/nomem - no check running programm.")
  Console.WriteLine("/prX - allows to modify the priority of the scan process in the system.")
  Console.WriteLine("/pr0-Idle")
  Console.WriteLine("/pr1-Normal")
  Console.WriteLine("/pr2-Hight")
  Console.WriteLine("/pr3-RealTime")
  Console.WriteLine("")
  Console.WriteLine("For example:")
  Console.WriteLine("ConScanner.exe /st /ar /na /alno /rps /rp /pr2 /logfull /reg /heur /log /mem /path=C:\")


 End Sub
 Private Sub mi_prior()
  On Error GoTo 101
  Dim ht As String = sGetINI(sINIFile, "Console", "Priority", "Hight")
  Select Case ht
   Case "RealTime"
    MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "RealTime")
   Case "Hight"
    MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Hight")
   Case "Idle"
    MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Idle")
   Case Else
    MyLibrary.FormFunction.priority_sets(CLng(Diagnostics.Process.GetCurrentProcess().Id), "Normal")
  End Select
  Exit Sub
101:
  ErrorLog("mi_prior " & ErrorToString())
 End Sub
 Public Sub first_start_registry2()
  'дефолтные настройки записать в файл настроек,если он отсутствует
  On Error GoTo 100
  writeINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName)
  writeINI(sINIFile, "USER", "Machine", MyLibrary.FormFunction.GetComputerName)
  writeINI(sINIFile, "USER", "Number", MyLibrary.FormFunction.get_serialdisk)
  writeINI(sINIFile, "USER", "SN", "")
  writeINI(sINIFile, "USER", "LicExpirid", "Unknown")
  '===============
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
  writeINI(sINIFile, "Shield", "Priority", "Normal")
  writeINI(sINIFile, "Shield", "CheckZip", "True")
  writeINI(sINIFile, "Shield", "AutoZapusk", "True")
  writeINI(sINIFile, "Shield", "Count_Exclude", "0")
  writeINI(sINIFile, "Shield", "Memory", "True")
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
  writeINI(sINIFile, "Console", "NoWindow", "False")
  '==========
  writeINI(sINIFile, "Vault", "Extension", "###")
  writeINI(sINIFile, "Vault", "Cript", "False")


  Exit Sub
100:
  ErrorLog("first_start_registry2 " & ErrorToString())

 End Sub
 Public Sub GetAllProcesses()
  Dim allProcesses(), thisProcess As Process
  Dim tmpModulename As String = ""
  Dim tmpProcname As String = ""
  allProcesses = System.Diagnostics.Process.GetProcesses
  For Each thisProcess In allProcesses
   Try
    tmpProcname = thisProcess.ProcessName
    Dim thisModule As ProcessModule
    For Each thisModule In thisProcess.Modules
     With thisModule
      If .FileName <> "" Or .ModuleName <> "" Then
       tmpModulename = .ModuleName
       Scan(IO.Path.GetFullPath(.FileName), "[MEM]", thisProcess.Id)
      End If
     End With
    Next
   Catch ee As Exception
    ErrorLog("GetAllProcesses " & ErrorToString() & " " & tmpModulename & "-" & tmpProcname)
   End Try
  Next
 End Sub
 Public Sub chk_reg_funct()
  'вызвать функцию проверки реестра
  On Error GoTo 100
  Dim chek_reg As Boolean = CBool(sGetINI(sINIFile, "Shield", "Registry", "True"))
  If chek_reg = False Or CStr(Trim(chek_reg)) = "" Then
   Exit Sub
  End If
  Dim rk As Microsoft.Win32.RegistryKey
  rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options")
  PrintKeys(rk)
  rk.Close()
  '  chk_reestr2() 'проверить реестр
  'chk_reestr3()
  Exit Sub
100:
  ErrorLog("chk_reg_funct " & ErrorToString())

 End Sub
 Public Sub PrintKeys(ByVal rkey As RegistryKey)
  ' Retrieve all the subkeys for the specified key.
  On Error GoTo 10
  Dim names As String() = rkey.GetSubKeyNames()
  Dim icount As Integer = 0
  Dim s As String
  For Each s In names

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
  If yes_vir_registry(s1) = True Then
   My.Computer.Registry.LocalMachine.DeleteSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & s1)
   My.Computer.Registry.LocalMachine.Close()
   Console.ForegroundColor = ConsoleColor.Red
   LogPrint("HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & s1 & "-DELETE")
   Console.WriteLine("HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & s1 & "-DELETE")
   Console.ForegroundColor = ConsoleColor.White
  End If
  Exit Sub
100:

  ErrorLog("chk_debug " & ErrorToString())
 End Sub
 Public Function yes_vir_registry(ByVal SearchChar3 As String) As Boolean
  'подчистка реестра
  'очень неприятный параметр в реестре блокирующий запуск некоторых (в основном антивирусных программ,административных утилит).
  'подумать...сделать рандомное имя щилда или же сделать рандомное имя если щилд будет запущен с помощью коммандной строки с опред. параметром
  'хотя врядли в данном случе поможет,если попадет под прессинг вирусописателей.....

  Try
   yes_vir_registry = False
   ' Create an instance of StreamReader to read from a file.
   If File.Exists(MyPath & "\regbase.txt") = False Then
    Exit Function
   End If
   Using sr As StreamReader = New StreamReader(MyPath & "\regbase.txt")
    Dim line As String
    ' Read and display the lines from the file until the end 
    ' of the file is reached.
    Do
     line = sr.ReadLine()
     Dim TestPos As Integer
     ' A textual comparison starting at position 4. Returns 6.
     TestPos = InStr(1, UCase(line), UCase(SearchChar3), CompareMethod.Binary)
     If TestPos <> 0 Then
      Console.ForegroundColor = ConsoleColor.Red
      Console.WriteLine("HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & SearchChar3 & "-suspicious value")
      LogPrint("HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" & SearchChar3 & "-suspicious value")
      Console.ForegroundColor = ConsoleColor.White
      yes_vir_registry = True
      sr.Close()
      sound_me("1")
      Exit Function
     End If
    Loop Until line Is Nothing
    sr.Close()
   End Using
  Catch E As Exception
   ' Let the user know what went wrong.
   ErrorLog("yes_vir_registry " & ErrorToString())
  End Try
 End Function

End Module
