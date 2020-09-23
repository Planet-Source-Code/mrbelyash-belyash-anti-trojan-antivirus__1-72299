Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports Microsoft.Win32
Imports System.Security.AccessControl

Public Class Form1
   Public Virname As String = ""
   Public pr As String = ""
   Dim keysize As New cript.clsAESV2.KeySize
   Dim MyLibrary As New MyLibrary.MyLib
   Dim objCryptDES As New cript.clsDES
   Public my_copy As Boolean = False
   Public VN As Integer = 0
   Public MYTime As Integer = 100


   Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
      keysize = cript.clsAESV2.KeySize.Bits128
      ' Encrypt text to a file using the file name, key, and IV.
      Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
      Dim u As String = a.Encrypt(Trim(Label5.Text))
      If addtobase(Application.StartupPath & "\trusted.db", u, IO.Path.GetFileName(Label1.Text)) = False Then
         MsgBox("Не получилось добавить новую запись", MsgBoxStyle.Critical)
      End If
      End
   End Sub


   Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      On Error Resume Next
      Me.Hide()
      ComboBox1.Items.Add("Ignore")
      ComboBox1.Items.Add("Delete")
      ComboBox1.Items.Add("Move")

      chk_first_ST9()
      MYTime = CInt(sGetINI(sINIFile, "Blocker", "Time", "1000"))
      my_copy = True
      Me.Text = "Belyash Application Blocker v." & Application.ProductVersion
      pr = "Belyash Application Blocker v." & Application.ProductVersion
      ParseCommandLineArgs()
   End Sub

   Sub ParseCommandLineArgs()
      Try
         Me.WindowState = FormWindowState.Minimized
         For Each s As String In My.Application.CommandLineArgs
            Label1.Text = IO.Path.GetFullPath(s)
         Next
         If Trim(Label1.Text) = "" Then
            'MsgBox("Dont find a path", MsgBoxStyle.Critical)
            Exit Sub
         End If
         Dim f As FileStream = New FileStream(Label1.Text, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
         f = New FileStream(Label1.Text, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
         Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
         md5.ComputeHash(f)
         f.Close()

         'Utilisation des FSO pour recuperer les information de base sur le fichier sйlectionner
         Dim chemin As String = Label1.Text
         Dim ObjFSO As Object = CreateObject("Scripting.FileSystemObject")
         Dim objFile = ObjFSO.GetFile(Label1.Text)

         Label4.Text = objFile.Name
         Label2.Text += "Size : " & (objFile.Size) & " Octects" & " soit : " & (objFile.Size) \ 1048576 & " Mo" & vbCrLf
         Label2.Text += "File Version : " & ObjFSO.GetFileVersion(Label1.Text) & vbCrLf
         Label2.Text += "Company : " & System.Diagnostics.FileVersionInfo.GetVersionInfo(Label1.Text).LegalCopyright.ToString & vbCrLf
         Label2.Text += "Description : " & System.Diagnostics.FileVersionInfo.GetVersionInfo(Label1.Text).FileDescription.ToString & vbCrLf
         Label2.Text += "Date created : " & objFile.DateCreated & vbCrLf
         Label2.Text += "Date last accessed : " & objFile.DateLastAccessed & vbCrLf
         Label2.Text += "Date last modified : " & objFile.DateLastModified & vbCrLf
         Label2.Text += "Short name : " & objFile.ShortName & vbCrLf

         'affichage du CRC32 dans label2.text


         'affichage du MD5 dans label3.text
         Dim hash As Byte() = md5.Hash
         Dim buff As StringBuilder = New StringBuilder
         Dim hashByte As Byte
         For Each hashByte In hash
            buff.Append(String.Format("{0:X1}", hashByte))
         Next
         Label5.Text = UCase(buff.ToString())
         If LCase(Label1.Text) = LCase(Application.StartupPath & "\monitor.exe") Then
            'пропускать главный компонент
            my_copy = True
            VN = VN + 1
            Process.Start(Label1.Text, vbNormal)
            Threading.Thread.Sleep(100)
            Exit Sub
         End If
         If LCase(Label1.Text) = LCase(Application.ExecutablePath) Then
            'пропускать самого себя
            my_copy = True
            VN = VN + 1
            Process.Start(Label1.Text, vbNormal)
            Threading.Thread.Sleep(100)
            Exit Sub
         End If
         Dim name26 As Boolean = CBool(sGetINI(sINIFile, "Blocker", "Scan", "False"))
         If name26 = True Then
            If yes_vir(Trim(Label5.Text)) = True Then
               GroupBox3.Visible = True
               GroupBox1.Enabled = False
               lblInfected.Text = Virname
            End If
         End If
         If yesinbaseBlocked(Label5.Text) = True Then
            MsgBox("The application [" & IO.Path.GetFileName(Label1.Text) & "] is blocked", MsgBoxStyle.Critical, pr)
            End
         End If

         If yesinbaseTrust(Label5.Text) = True Then
            my_copy = True
            VN = VN + 1
            Process.Start(Label1.Text, vbNormal)
            Threading.Thread.Sleep(100)
         End If
         Me.WindowState = FormWindowState.Normal
         Me.Show()
         Me.TopMost = True
      
      Catch ex As Exception
         ErrorLog("ParseCommandLineArgs " & ErrorToString() & "-Blocker")
      End Try
   End Sub
   Function yesinbaseTrust(ByVal SearchChar7 As String) As Boolean
      yesinbaseTrust = False
      Try
         If File.Exists(Application.StartupPath & "\trusted.db") = False Then
            Exit Function
         End If
         If Trim(SearchChar7) = "" Then
            'Exit Function
         End If
         keysize = cript.clsAESV2.KeySize.Bits128
         Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
         Dim tmpstr As String = a.Encrypt(SearchChar7)
         ' Create an instance of StreamReader to read from a file.
         Dim d As String = (MyLibrary.FormFunction.MakeTopMost(Application.StartupPath & "\trusted.db", tmpstr))
         If Trim(d) <> "0" Then

            yesinbaseTrust = True
            sound_me("exit")
         End If
      Catch ex As Exception
         ErrorLog("yesinbaseTrust " & ErrorToString())
      End Try
   End Function
   Function yesinbaseBlocked(ByVal SearchChar7 As String) As Boolean
      yesinbaseBlocked = False
      Try
         If File.Exists(Application.StartupPath & "\blocked.db") = False Then
            Exit Function
         End If
         If Trim(SearchChar7) = "" Then
            Exit Function
         End If
         keysize = cript.clsAESV2.KeySize.Bits128
         Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
         Dim tmpstr As String = a.Encrypt(SearchChar7)
         ' Create an instance of StreamReader to read from a file.
         Dim d As String = (MyLibrary.FormFunction.MakeTopMost(Application.StartupPath & "\blocked.db", tmpstr))

         If Trim(d) <> "0" Then
            sound_me("1")
            If MyLibrary.FormFunction.get_all_pos(d, Application.StartupPath & "\blocked.db") <> "None" Then
               Dim tmpstr2 As String = MyLibrary.FormFunction.get_all_pos(d, Application.StartupPath & "\blocked.db")
               Virname = tmpstr2.Substring(tmpstr.Length, tmpstr2.Length - tmpstr.Length)
               yesinbaseBlocked = True

            End If
         End If
      Catch ex As Exception
         ErrorLog("yesinbaseTrust " & ErrorToString())
      End Try
   End Function
   Public Sub sound_me(ByVal d As String)
      'звук
      On Error GoTo 10
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
      sw.WriteLine(Format$(Now, "dd-mm-yyyy") & " " & smess45 & "-Blocker")
      sw.Flush()
      sw.Close()
      Exit Sub
100:

   End Sub

   Function addtobase(ByVal path As String, ByVal smess45 As String, ByVal f1 As String) As Boolean
      'будем записывать ошибки

      Try
         addtobase = False
         If Trim(path) = "" Then
            Exit Function
         End If
         If Trim(smess45) = "" Then
            Exit Function
         End If

         Dim sw As StreamWriter = File.AppendText(path)
         sw.WriteLine(smess45 & "|" & f1)
         sw.Close()
         addtobase = True
      Catch ex As Exception
         ErrorLog("addtobase " & ErrorToString())
      End Try
   End Function
   Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
      Try
         my_copy = True
         Process.Start(Label1.Text, vbNormal)
      Catch ex As Exception
         MsgBox("Unknow error" & vbCrLf & ErrorToString(), MsgBoxStyle.Critical)
      End Try
   End Sub

   Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
      Application.Exit()
   End Sub

   Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
      keysize = cript.clsAESV2.KeySize.Bits128
      ' Encrypt text to a file using the file name, key, and IV.
      Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
      Dim u As String = a.Encrypt(Trim(Label5.Text))
      If addtobase(Application.StartupPath & "\blocked.db", u, IO.Path.GetFileName(Label1.Text)) = False Then
         MsgBox("Не получилось добавить новую запись", MsgBoxStyle.Critical)
      End If
      End
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

         Const userRoot As String = "HKEY_CLASSES_ROOT"
         Const subkey As String = "exefile\shell\open\command"
         Const keyName As String = userRoot & "\" & subkey
         Registry.SetValue(keyName, _
       "", Application.StartupPath & "\Blocker.exe %1 %*", RegistryValueKind.String)
      Catch ex As Exception
         ErrorLog("reg_blocker " & ErrorToString())
      End Try
   End Sub
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
      writeINI(sINIFile, "Blocker", "Transparent", "93")
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

   Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
      Try

         GroupBox1.Enabled = True
         Select Case ComboBox1.Text
            Case "Ignore"

            Case "Delete"
               delFile(Trim(Label1.Text))
            Case "Move"
               moveToq(Trim(Label1.Text))
         End Select

      Catch ex As Exception
         ErrorLog("Button5_Click " & ErrorToString())
      End Try
      GroupBox3.Hide()
      GroupBox1.Enabled = True
      End
   End Sub
   'MyLibrary.FormFunction.kiilONReboot(secondfilename)
   Sub delFile(ByVal fl_name As String)
      On Error GoTo 100
      File.Delete(fl_name)
      GroupBox3.Hide()
      Exit Sub
100:

      MyLibrary.FormFunction.kiilONReboot(fl_name)
   End Sub
   Sub moveToq(ByVal fl_name As String)
      Try

  
         If File.Exists(Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name)) = True Then
            File.Delete(Application.StartupPath & "\quarantine\" & My.Computer.FileSystem.GetName(fl_name))
         End If

         Dim tmpcr As Boolean = CBool(sGetINI(sINIFile, "Vault", "Cript", "False"))
         If tmpcr = True Then
            vaultCript(fl_name)
         Else
            Dim tmpcrZ As String = sGetINI(sINIFile, "Vault", "Extension", "###")

            If File.Exists(Application.StartupPath & "\quarantine\" & IO.Path.GetFileName(fl_name) & "." & Trim(tmpcrZ)) = True Then
               File.Delete(Application.StartupPath & "\quarantine\" & IO.Path.GetFileName(fl_name) & "." & Trim(tmpcrZ))
            End If
            Dim tmp_r As String = Application.StartupPath & "\quarantine\" & IO.Path.GetFileName(fl_name) & "." & Trim(tmpcrZ)
            My.Computer.FileSystem.MoveFile(fl_name, tmp_r, True)
            'MyLibrary.FormFunction.ren_ME(fl_name, tmp_r)
         End If

      Catch ex As Exception
         ErrorLog("moveToq " & ErrorToString())
      End Try
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
   '=========================

   Public Function yes_vir_temp(ByVal SearchChar7 As String, ByVal mytmpb As String) As Boolean
      'md5
      yes_vir_temp = False
      If Trim(SearchChar7) = "" Then
         Exit Function
      End If
      keysize = cript.clsAESV2.KeySize.Bits128
      Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
      Dim tmpstr As String = a.Encrypt(SearchChar7)
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
End Class
