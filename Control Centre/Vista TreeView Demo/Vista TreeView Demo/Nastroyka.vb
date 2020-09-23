Public Class Nastroyka
 Public ve As Integer
 Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
  Dim z As Integer = 0
  Dim i As Int32
  For i = 1 To 10
   Dim parrentNode As New VistaNode
   'parrentNode.Text = CStr(i)
   parrentNode.Name = CStr(i)
   parrentNode.ImageKey = "stickies24.gif"
   parrentNode.SelectedImageKey = "customize24.gif"
   Select Case i
    Case 1
     parrentNode.PostText = "Shield"
     'For a = 1 To 3
     Dim a As Int32
     For a = 1 To 5

      Select Case a
       Case 1
        Dim subNode8 As New VistaNode
        subNode8.Text = "Scanning"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)
       Case 2
        Dim subNode8 As New VistaNode
        subNode8.Text = "Report"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)
       Case 3
        Dim subNode8 As New VistaNode
        subNode8.Text = "Action"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)
       Case 4
        Dim subNode8 As New VistaNode
        subNode8.Text = "Excluded Path"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)

       Case 5
        Dim subNode8 As New VistaNode
        subNode8.Text = "Misc"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)

      End Select


      ' parrentNode.Nodes.Add(subNode8)
     Next a
    Case 2
     parrentNode.PostText = "Scanner"
     'For a = 1 To 3
     Dim a As Int32
     For a = 1 To 5

      Select Case a
       Case 1
        Dim subNode8 As New VistaNode
        subNode8.Text = "Scanning"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)
       Case 2
        Dim subNode8 As New VistaNode
        subNode8.Text = "Report"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)
       Case 3
        Dim subNode8 As New VistaNode
        subNode8.Text = "Action"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)
       Case 4
        Dim subNode8 As New VistaNode
        subNode8.Text = "Excluded Path"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)

       Case 5
        Dim subNode8 As New VistaNode
        subNode8.Text = "Misc"
        subNode8.Name = CStr(i) & CStr(a)
        subNode8.ImageKey = "stickies24.gif"
        subNode8.SelectedImageKey = "customize24.gif"
        parrentNode.Nodes.Add(subNode8)

      End Select

     
      ' parrentNode.Nodes.Add(subNode8)
     Next a
    Case 3
     parrentNode.PostText = "Firewall"
    Case 4
     parrentNode.PostText = "Update"
    Case 5
     parrentNode.PostText = "Registry Fix Tool"

   End Select
   parrentNode.PostTextColor = Color.Yellow
   VistaTreeView1.Nodes.Add(parrentNode)

  Next
  load_my_nasrtr()
  ComboBox2.Items.Clear()
  ComboBox2.Items.Add("Infected")
  ComboBox2.Items.Add("Suspisious")
  ComboBox2.Items.Add("Archive")
  ComboBox2.Text = "Infected"
  Exclude_pats()
  ComboBox22.Items.Clear()
  ComboBox22.Items.Add("Infected")
  ComboBox22.Items.Add("Suspisious")
  ComboBox22.Items.Add("Archive")
  ComboBox22.Text = "Infected"
  Exclude_pats2()
  'VistaTreeView1.ExpandAll()
 End Sub
 Sub Exclude_pats2()
  On Error GoTo 10
  ListBox1.Items.Clear()
  Dim i As Integer

  Dim po As Integer = CInt(sGetINI(sINIFile, "Scanner", " Count_Exclude_Scan", "0"))
  If po = 0 Then
   Exit Sub
  End If
  For i = 0 To po
   If Trim(sGetINI(sINIFile, "Exclude_Scanner", i, "")) <> "" Then
    ListBox1.Items.Add(sGetINI(sINIFile, "Exclude_Scanner", i, ""))
   End If
  Next
  'End If
  Exit Sub
10:
  ' ErrorLog("nastrScanner.Exclude_pats " & ErrorToString())

 End Sub
 Private Sub VistaTreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles VistaTreeView1.AfterSelect
  VistaTreeView1.Text = "Text: " + VistaTreeView1.SelectedNode.Text + vbCrLf + "Tag: " + VistaTreeView1.SelectedNode.Tag + vbCrLf + "Index: " + VistaTreeView1.SelectedNode.Index.ToString + vbCrLf + "Childs:" + VistaTreeView1.SelectedNode.GetNodeCount(True).ToString
  txtText.Text = VistaTreeView1.SelectedNode.Text
  Select Case VistaTreeView1.SelectedNode.Name
   Case "11"
    TabControl1.SelectTab(0)
   Case "12"
    TabControl1.SelectTab(1)
   Case "13"
    TabControl1.SelectTab(2)
   Case "14"
    TabControl1.SelectTab(3)
   Case "15"
    TabControl1.SelectTab(4)
    '===========
   Case "21"
    TabControl1.SelectTab(5)
   Case "22"
    TabControl1.SelectTab(6)
   Case "23"
    TabControl1.SelectTab(7)
   Case "24"
    TabControl1.SelectTab(8)
   Case "25"
    TabControl1.SelectTab(9)
  End Select

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
  ' ErrorLog("get_my_priority " & ErrorToString())

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
  ' ErrorLog("nastr.Exclude_pats " & ErrorToString())

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


 Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage1.Click

 End Sub




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
  '===================Shield
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

  'ErrorLog("load_my_nasrtr " & ErrorToString())
  Resume Next

 End Sub

 Sub Save_me_nastr()
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
  If chkArckSc.Checked = True Then
   writeINI(sINIFile, "Scanner", "CheckZip", "True")
  Else
   writeINI(sINIFile, "Scanner", "CheckZip", "False")
  End If
  '================
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

  'ErrorLog(ErrorToString)
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



 
 Private Sub ComboBox22_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox22.SelectedIndexChanged
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

 Private Sub chkEvristicSc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEvristicSc.CheckedChanged
  If chkEvristicSc.Checked = True Then

   If MsgBox("Использование эвристики не безопастно. Возможно потеря данных" & vbCrLf & "Использовать эвристику", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Внимание") = MsgBoxResult.Yes Then
    chkEvristicSc.Checked = True
   Else
    chkEvristicSc.Checked = False

   End If
  End If
 End Sub

 Public Sub New()

  ' This call is required by the Windows Form Designer.
  InitializeComponent()

  ' Add any initialization after the InitializeComponent() call.

 End Sub
End Class
