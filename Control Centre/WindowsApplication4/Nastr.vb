Imports Microsoft.Win32

Public Class Nastr

    Public ve As Integer
 'Private imgOP As Microsoft.Win32.RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Belyash\Monitor\Exclude", True)
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Save_me_nastr()
        Me.Close()
        Me.Dispose()
    End Sub
    Sub load_my_nasrtr()
        On Error GoTo 10
        CheckBox1.Checked = CBool(sGetINI(sINIFile, "Shield", "Log", "True"))
  Dim f1 As Boolean = CBool(sGetINI(sINIFile, "Shield", "Append", "True"))
  If f1 = True Then
   RadioButton1.Checked = True
  Else
   RadioButton2.Checked = True
  End If
        CheckBox2.Checked = CBool(sGetINI(sINIFile, "Shield", "Chick", "True"))
        TextBox2.Text = sGetINI(sINIFile, "Shield", "LogSize", "5860843")
        CheckBox3.Checked = CBool(sGetINI(sINIFile, "Shield", "LogActiviti", "False"))
        CheckBox5.Checked = CBool(sGetINI(sINIFile, "Shield", "Sound", "True"))
        chk_autozap.Checked = CBool(sGetINI(sINIFile, "Shield", "AutoZapusk", "True"))
        chkWopr.Checked = CBool(sGetINI(sINIFile, "Shield", "Zapros", "True"))
        Dim f3 As String = sGetINI(sINIFile, "Shield", "Action", "REPORT")
        Select Case f3
            Case "REPORT"
                RadioButton3.Checked = True
            Case "MOVE"
                RadioButton4.Checked = True
            Case "DELETE"
                RadioButton5.Checked = True
            Case "LOCK"
                RadioButton10.Checked = True

        End Select
        chkHistory.Checked = CBool(sGetINI(sINIFile, "Shield", "DelIEHistory", "True"))
        chkArck.Checked = CBool(sGetINI(sINIFile, "Shield", "CheckZip", "True"))

        chkMon.Checked = CBool(sGetINI(sINIFile, "Shield", "Status", "True"))
        chkHash.Checked = CBool(sGetINI(sINIFile, "Shield", "LogHash", "True"))
        chkEvristic.Checked = CBool(sGetINI(sINIFile, "Shield", "Evristic", "True"))
        chkReg.Checked = CBool(sGetINI(sINIFile, "Shield", "Registry", "True"))

        CHKTime.Checked = CBool(sGetINI(sINIFile, "Shield", "Time", "True"))
        chkRash.Checked = CBool(sGetINI(sINIFile, "Shield", "Extentions", "True"))
        chfFileSZ.Checked = CBool(sGetINI(sINIFile, "Shield", "NoCheckLenght", "True"))
  chkMEM.Checked = CBool(sGetINI(sINIFile, "Shield", "Memory", "True"))
        txtCheckLen.Text = sGetINI(sINIFile, "Shield", "CheckLenght", "15142784")

        chkAutorun.Checked = CBool(sGetINI(sINIFile, "Shield", "Autorun", "True"))

        Dim ht As String = sGetINI(sINIFile, "Shield", "Priority", "Normal")

        Select Case ht
            Case "RealTime"
                RadioButton6.Checked = True
            Case "Hight"
                RadioButton7.Checked = True
            Case "Idle"
                RadioButton9.Checked = True
            Case "Normal"
                RadioButton8.Checked = True
            Case Else
                RadioButton8.Checked = True
        End Select
        Exit Sub
10:

        ErrorLog("load_my_nasrtr " & ErrorToString())
        Resume Next

    End Sub

    Sub Save_me_nastr()
        On Error Resume Next
        If CheckBox1.Checked = True Then
            writeINI(sINIFile, "Shield", "Log", "True")
        Else
            writeINI(sINIFile, "Shield", "Log", "False")
        End If

        If chkHistory.Checked = True Then
            writeINI(sINIFile, "Shield", "DelIEHistory", "True")
        Else
            writeINI(sINIFile, "Shield", "DelIEHistory", "False")
        End If
        If RadioButton1.Checked = True Then
            writeINI(sINIFile, "Shield", "Append", "True")
        End If
        If RadioButton2.Checked = True Then
            writeINI(sINIFile, "Shield", "Append", "False")
        End If
        If chkArck.Checked = True Then
            writeINI(sINIFile, "Shield", "CheckZip", "True")
        Else
            writeINI(sINIFile, "Shield", "CheckZip", "False")
        End If
        If CheckBox2.Checked = True Then
            writeINI(sINIFile, "Shield", "Chick", "True")
            writeINI(sINIFile, "Shield", "LogSize", TextBox2.Text)
        Else
            writeINI(sINIFile, "Shield", "Chick", "False")
            writeINI(sINIFile, "Shield", "LogSize", TextBox2.Text)
        End If
        If CheckBox3.Checked = True Then
            writeINI(sINIFile, "Shield", "LogActiviti", "True")
        Else
            writeINI(sINIFile, "Shield", "LogActiviti", "False")
        End If

        If chkRash.Checked = True Then
            writeINI(sINIFile, "Shield", "Extentions", "True")
        Else
            writeINI(sINIFile, "Shield", "Extentions", "False")
        End If
        If chk_autozap.Checked = True Then
            writeINI(sINIFile, "Shield", "AutoZapusk", "True")
        Else
            writeINI(sINIFile, "Shield", "AutoZapusk", "False")
        End If
  If chkMEM.Checked = True Then
   writeINI(sINIFile, "Shield", "Memory", "True")
  Else
   writeINI(sINIFile, "Shield", "Memory", "False")
  End If

        If CHKTime.Checked = True Then
            writeINI(sINIFile, "Shield", "Time", "True")
        Else
            writeINI(sINIFile, "Shield", "Time", "False")
        End If
  Select Case ComboBox2.Text
   Case "Suspisious"
    If RadioButton3.Checked = True Then
     writeINI(sINIFile, "Shield", "ActionUnknow", "REPORT")
    End If
    If RadioButton4.Checked = True Then
     writeINI(sINIFile, "Shield", "ActionUnknow", "MOVE")
    End If
    If RadioButton5.Checked = True Then
     writeINI(sINIFile, "Shield", "ActionUnknow", "DELETE")
    End If
    If RadioButton10.Checked = True Then
     writeINI(sINIFile, "Shield", "ActionUnknow", "LOCK")
    End If
   Case "Infected"
    If RadioButton3.Checked = True Then
     writeINI(sINIFile, "Shield", "Action", "REPORT")
    End If
    If RadioButton4.Checked = True Then
     writeINI(sINIFile, "Shield", "Action", "MOVE")
    End If
    If RadioButton5.Checked = True Then
     writeINI(sINIFile, "Shield", "Action", "DELETE")
    End If
    If RadioButton10.Checked = True Then
     writeINI(sINIFile, "Shield", "Action", "LOCK")
    End If
   Case "Archive"
    If RadioButton3.Checked = True Then
     writeINI(sINIFile, "Shield", "ActionZip", "REPORT")
    End If
    If RadioButton4.Checked = True Then
     writeINI(sINIFile, "Shield", "ActionZip", "MOVE")
    End If
    If RadioButton5.Checked = True Then
     writeINI(sINIFile, "Shield", "ActionZip", "DELETE")
    End If
    If RadioButton10.Checked = True Then
     writeINI(sINIFile, "Shield", "ActionZip", "LOCK")
    End If
  End Select


  If CheckBox5.Checked = True Then
   writeINI(sINIFile, "Shield", "Sound", "True")
  Else
   writeINI(sINIFile, "Shield", "Sound", "False")
  End If
  If chkWopr.Checked = True Then
   writeINI(sINIFile, "Shield", "Zapros", "True")
  Else
   writeINI(sINIFile, "Shield", "Zapros", "False")
  End If

  If chkAutorun.Checked = True Then
   writeINI(sINIFile, "Shield", "Autorun", "True")
  Else
   writeINI(sINIFile, "Shield", "Autorun", "False")
  End If


  If chkMon.Checked = True Then
   writeINI(sINIFile, "Shield", "Status", "True")
  Else
   writeINI(sINIFile, "Shield", "Status", "False")
  End If

  If chkHash.Checked = True Then
   writeINI(sINIFile, "Shield", "LogHash", "True")
  Else
   writeINI(sINIFile, "Shield", "LogHash", "False")
  End If

  If chkEvristic.Checked = True Then
   writeINI(sINIFile, "Shield", "Evristic", "True")
  Else
   writeINI(sINIFile, "Shield", "Evristic", "False")
  End If
  If chkReg.Checked = True Then
   writeINI(sINIFile, "Shield", "Registry", "True")
  Else
   writeINI(sINIFile, "Shield", "Registry", "False")
  End If

  If chfFileSZ.Checked = True Then
   writeINI(sINIFile, "Shield", "NoCheckLenght", "True")
  Else
   writeINI(sINIFile, "Shield", "NoCheckLenght", "False")
  End If
  '===========================
  If RadioButton6.Checked = True Then
   writeINI(sINIFile, "Shield", "Priority", "RealTime")
  End If
  If RadioButton7.Checked = True Then
   writeINI(sINIFile, "Shield", "Priority", "Hight")
  End If
  If RadioButton8.Checked = True Then
   writeINI(sINIFile, "Shield", "Priority", "Normal")
  End If
  If RadioButton9.Checked = True Then
   writeINI(sINIFile, "Shield", "Priority", "Idle")
  End If
  '===========================

  If Trim(txtCheckLen.Text) <> "" Then
   writeINI(sINIFile, "Shield", "CheckLenght", txtCheckLen.Text)
   ' Else
   '    writeINI(sINIFile, "Shield", "CheckLenght", "15142784")
  End If

  If ListBox1.Items.Count >= 1 Then

   Dim i2 As Integer
   writeINI(sINIFile, "Shield", "Count_Exclude", ListBox1.Items.Count)

   For i2 = 0 To ListBox1.Items.Count - 1
    writeINI(sINIFile, "Exclude_Shield", i2, ListBox1.Items(i2).ToString)
   Next
  End If

  Exit Sub
10:

  ErrorLog(ErrorToString)
 End Sub


 Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
  FolderBrowserDialog1.ShowDialog()
  ListBox1.Items.Add(FolderBrowserDialog1.SelectedPath)
 End Sub

 Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
  If ListBox1.Items.Count >= 1 Then
   writeINI(sINIFile, "Exclude_Shield", ListBox1.SelectedIndex, "")
   ListBox1.Items.Remove(ListBox1.SelectedItem)
  End If

 End Sub

 Private Sub Nastr_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
  On Error Resume Next
  load_my_nasrtr()
  ComboBox2.Items.Clear()
  ComboBox2.Items.Add("Infected")
  ComboBox2.Items.Add("Suspisious")
  ComboBox2.Items.Add("Archive")
  ComboBox2.Text = "Infected"
  Exclude_pats()
 End Sub

 Private Sub get_my_priority()
  'приоритет читаем из инишника
  On Error GoTo 101
  RadioButton6.Checked = True
  RadioButton7.Checked = True
  RadioButton8.Checked = True
  RadioButton9.Checked = True

  Dim Mi_status1 As String
  Dim ht As String = sGetINI(sINIFile, "Shield", "Priority", "Normal")
  Select Case ht
   Case "RealTime"
    RadioButton6.Checked = True
   Case "Hight"
    RadioButton7.Checked = True
   Case "Idle"
    RadioButton9.Checked = True
   Case Else
    RadioButton8.Checked = True
  End Select
  Exit Sub
101:
  ErrorLog("get_my_priority " & ErrorToString())

 End Sub
 Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
  On Error Resume Next
  Select Case ComboBox2.Text
   Case "Suspisious"
    Dim f3 As String = sGetINI(sINIFile, "Shield", "ActionUnknow", "REPORT")
    Select Case f3
     Case "REPORT"
      RadioButton3.Checked = True
     Case "MOVE"
      RadioButton4.Checked = True
     Case "DELETE"
      RadioButton5.Checked = True
     Case "LOCK"
      RadioButton10.Checked = True
    End Select
   Case "Infected"
    Dim f3 As String = sGetINI(sINIFile, "Shield", "Action", "REPORT")
    Select Case f3
     Case "REPORT"
      RadioButton3.Checked = True
     Case "MOVE"
      RadioButton4.Checked = True
     Case "DELETE"
      RadioButton5.Checked = True
     Case "LOCK"
      RadioButton10.Checked = True
    End Select
   Case "Archive"
    Dim f3 As String = sGetINI(sINIFile, "Shield", "ActionZip", "REPORT")
    Select Case f3
     Case "REPORT"
      RadioButton3.Checked = True
     Case "MOVE"
      RadioButton4.Checked = True
     Case "DELETE"
      RadioButton5.Checked = True
     Case "LOCK"
      RadioButton10.Checked = True
    End Select
  End Select

 End Sub

    Sub Exclude_pats()
        On Error GoTo 10
        ListBox1.Items.Clear()
        Dim i As Integer

        Dim po As Integer = CInt(sGetINI(sINIFile, "Shield", " Count_Exclude", "0"))
        If po = 0 Then
            Exit Sub
        End If
        For i = 0 To po
            If Trim(sGetINI(sINIFile, "Exclude_Shield", i, "")) <> "" Then
                ListBox1.Items.Add(sGetINI(sINIFile, "Exclude_Shield", i, ""))
            End If
        Next
        'End If
        Exit Sub
10:
        ErrorLog("nastr.Exclude_pats " & ErrorToString())

    End Sub




    Private Sub chkEvristic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles chkEvristic.MouseClick
        If chkEvristic.Checked = True Then

            If MsgBox("Использование эвристики не безопастно. Возможно потеря данных" & vbCrLf & "Использовать эвристику", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Внимание") = MsgBoxResult.Yes Then
                chkEvristic.Checked = True
            Else
                chkEvristic.Checked = False

            End If
        End If
    End Sub


    
End Class