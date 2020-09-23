Public Class NastrScanner

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        save_nastr()
        Me.Close()
        Me.Dispose()
    End Sub

   
    Private Sub NastrScanner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        load_Nastr_Scanner()
  ComboBox22.Items.Clear()
  ComboBox22.Items.Add("Infected")
  ComboBox22.Items.Add("Suspisious")
  ComboBox22.Items.Add("Archive")
  ComboBox22.Text = "Infected"
  Exclude_pats2()
 End Sub
 Sub Exclude_pats2()
  On Error GoTo 10
  ListBox1Sc.Items.Clear()
  Dim i As Integer

  Dim po As Integer = CInt(sGetINI(sINIFile, "Scanner", " Count_Exclude_Scan", "0"))
  If po = 0 Then
   Exit Sub
  End If
  For i = 0 To po
   If Trim(sGetINI(sINIFile, "Exclude_Scanner", i, "")) <> "" Then
    ListBox1Sc.Items.Add(sGetINI(sINIFile, "Exclude_Scanner", i, ""))
   End If
  Next
  'End If
  Exit Sub
10:
  ErrorLog("nastrScanner.Exclude_pats " & ErrorToString())

 End Sub
 Private Sub save_nastr()
  'Dim newVersion As String = "test" ' параметры создаваемого ключа
  On Error Resume Next
  If CheckBox1Sc.Checked = True Then
   writeINI(sINIFile, "Scanner", "Log", "True")
  Else
   writeINI(sINIFile, "Scanner", "Log", "False")
  End If
  If RadioButton1Sc.Checked = True Then
   writeINI(sINIFile, "Scanner", "Append", "True")
  Else
   writeINI(sINIFile, "Scanner", "Append", "False")
  End If
  If chkCustomSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "Custom", "True")
  Else
   writeINI(sINIFile, "Scanner", "Custom", "False")
  End If


  If chk_autozapSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "ScanAutoStart", "True")
  Else
   writeINI(sINIFile, "Scanner", "ScanAutoStart", "False")
  End If

  If CheckBox2Sc.Checked = True Then
   writeINI(sINIFile, "Scanner", "LogSize", TextBox1Sc.Text)
  Else
   writeINI(sINIFile, "Scanner", "LogSize", "0")
  End If
  If ComboBox22.Text = "Infected" Then
   If RadioButton3Sc.Checked = True Then
    writeINI(sINIFile, "Scanner", "Action", "MOVE")
   End If

   If RadioButton4Sc.Checked = True Then
    writeINI(sINIFile, "Scanner", "Action", "DELETE")
   End If
   If RadioButton5Sc.Checked = True Then
    writeINI(sINIFile, "Scanner", "Action", "REPORT")
   End If
   If RadioButton10Sc.Checked = True Then
    writeINI(sINIFile, "Scanner", "Action", "LOCK")
   End If
  Else
   If RadioButton3Sc.Checked = True Then
    writeINI(sINIFile, "Scanner", "ActionUncknow", "MOVE")
   End If

   If RadioButton4Sc.Checked = True Then
    writeINI(sINIFile, "Scanner", "ActionUncknow", "DELETE")
   End If
   If RadioButton5Sc.Checked = True Then
    writeINI(sINIFile, "Scanner", "ActionUncknow", "REPORT")
   End If
   If RadioButton10Sc.Checked = True Then
    writeINI(sINIFile, "Scanner", "ActionUncknow", "LOCK")
   End If
  End If
  If chkRashSc.Checked = False Then
   writeINI(sINIFile, "Scanner", "SCANALL", "True")
  Else
   writeINI(sINIFile, "Scanner", "SCANALL", "False")
  End If
  If chkWoprSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "Ask", "True")
  Else
   writeINI(sINIFile, "Scanner", "Ask", "False")
  End If
  If chfFileSZSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "FILESIZE", txtCheckLenSc.Text)
  Else
   writeINI(sINIFile, "Scanner", "FILESIZE", "55242880")
  End If

  If CheckBox5Sc.Checked = True Then
   writeINI(sINIFile, "Scanner", "SOUND", "True")
  Else
   writeINI(sINIFile, "Scanner", "SOUND", "False")
  End If
  '==============================
  If CheckBox5Sc.Checked = True Then
   writeINI(sINIFile, "Scanner", "CheckRegistry", "True")
  Else
   writeINI(sINIFile, "Scanner", "CheckRegistry", "False")
  End If
  If chkEvristicSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "ScanHeur", "True")
  Else
   writeINI(sINIFile, "Scanner", "ScanHeur", "False")
  End If
  If CheckmemSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "ScanMemory", "True")
  Else
   writeINI(sINIFile, "Scanner", "ScanMemory", "False")
  End If
  If chkIzbitokSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "Izbitochnoe", "True")
  Else
   writeINI(sINIFile, "Scanner", "Izbitochnoe", "False")
  End If
  '=========
  If chkArckSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "CheckZip", "True")
  Else
   writeINI(sINIFile, "Scanner", "CheckZip", "False")
  End If
  '================


 End Sub
 Private Sub load_Nastr_Scanner()
  On Error Resume Next
  'получить настройки
  CheckBox1Sc.Checked = CBool(sGetINI(sINIFile, "Scanner", "Log", "True"))
  Dim name2 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "Append", "True"))
  If name2 = True Then
   RadioButton1Sc.Checked = True
  Else
   RadioButton2Sc.Checked = True
  End If
  TextBox1Sc.Text = sGetINI(sINIFile, "Scanner", "LogSize", "125555555")

  Dim name3 As String = sGetINI(sINIFile, "Scanner", "Action", "REPORT")
  Select Case name3
   Case "MOVE"
    RadioButton3Sc.Checked = True
   Case "DELETE"
    RadioButton4Sc.Checked = True
   Case "LOCK"
    RadioButton10Sc.Checked = True
   Case Else
    RadioButton5Sc.Checked = True
  End Select
  '================
  Dim keyName135 As Boolean = CBool(sGetINI(sINIFile, "Scanner", _
    "CheckZip", "True"))
  If keyName135 = False Then
   chkArckSc.Checked = False
  Else
   chkArckSc.Checked = True
  End If
  '============
  Dim name4 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "SCANALL", "True"))

  If name4 = False Then
   chkRashSc.Checked = True
  Else
   chkRashSc.Checked = True
  End If
  txtCheckLenSc.Text = sGetINI(sINIFile, "Scanner", "FILESIZE", "55242880")

  If txtCheckLenSc.Text = "" Then
   txtCheckLenSc.Text = "55242880"
  End If
  Dim name5 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "SOUND", "True"))
  If name5 = True Then
   CheckBox5Sc.Checked = True
  Else
   CheckBox5Sc.Checked = False
  End If
  Dim name75 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "Custom", "True"))
  If name75 = True Then
   chkCustomSc.Checked = True
  Else
   chkAllSc.Checked = False
  End If
  Dim name15 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "Ask", "True"))
  If name15 = True Then
   chkWoprSc.Checked = True
  Else
   chkWoprSc.Checked = False
  End If
  '=============
  Dim name25 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "CheckRegistry", "True"))
  If name25 = True Then
   CheckBox5Sc.Checked = True
  Else
   CheckBox5Sc.Checked = False
  End If
  Dim name26 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "ScanHeur", "False"))

  If name26 = True Then
   chkEvristicSc.Checked = True
  Else
   chkEvristicSc.Checked = False
  End If

  Dim name27 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "ScanMemory", "True"))
  If name27 = True Then
   CheckmemSc.Checked = True
  Else
   CheckmemSc.Checked = False
  End If
  Dim name287 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "ScanAutoStart", "True"))
  If name287 = True Then
   chk_autozapSc.Checked = True
  Else
   chk_autozapSc.Checked = False
  End If


  Dim name87 As Boolean = CBool(sGetINI(sINIFile, "Scanner", "Izbitochnoe", "False"))
  If name87 = True Then
   chkIzbitokSc.Checked = True
  Else
   chkIzbitokSc.Checked = False
  End If
  Exit Sub
150:
  'save_nastr()
  'MsgBox(ErrorToString, MsgBoxStyle.Critical)
 End Sub
 Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox22.SelectedIndexChanged
  On Error GoTo 100
  If ComboBox22.Text = "Infected" Then
   Dim name3 As String = sGetINI(sINIFile, "Scanner", "Action", "REPORT")
   Select Case name3
    Case "MOVE"
     RadioButton3Sc.Checked = True
    Case "DELETE"
     RadioButton4Sc.Checked = True
    Case "LOCK"
     RadioButton10Sc.Checked = True
    Case Else
     RadioButton5Sc.Checked = True
   End Select

  Else
   Dim name3 As String = sGetINI(sINIFile, "Scanner", "ActionUncknow", "REPORT")
   Select Case name3
    Case "MOVE"
     RadioButton3Sc.Checked = True
    Case "DELETE"
     RadioButton4Sc.Checked = True
    Case "LOCK"
     RadioButton10Sc.Checked = True
    Case Else
     RadioButton5Sc.Checked = True
   End Select
  End If
  Exit Sub
100:
 End Sub

 Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
  Me.Close()
  Me.Dispose()
 End Sub


 Private Sub chkEvristic_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEvristicSc.CheckedChanged

  If chkEvristicSc.Checked = True Then

   If MsgBox("Использование эвристики не безопастно. Возможно потеря данных" & vbCrLf & "Использовать эвристику", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Внимание") = MsgBoxResult.Yes Then
    chkEvristicSc.Checked = True
   Else
    chkEvristicSc.Checked = False

   End If
  End If
 End Sub

 
 Private Sub Button5Sc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5Sc.Click

 End Sub
End Class