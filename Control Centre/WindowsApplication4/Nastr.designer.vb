<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Nastr
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
  Me.components = New System.ComponentModel.Container
  Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Nastr))
  Me.GroupBox1 = New System.Windows.Forms.GroupBox
  Me.ListBox1 = New System.Windows.Forms.ListBox
  Me.GroupBox5 = New System.Windows.Forms.GroupBox
  Me.txtCheckLen = New System.Windows.Forms.TextBox
  Me.chfFileSZ = New System.Windows.Forms.CheckBox
  Me.Button4 = New System.Windows.Forms.Button
  Me.Button5 = New System.Windows.Forms.Button
  Me.GroupBox3 = New System.Windows.Forms.GroupBox
  Me.RadioButton10 = New System.Windows.Forms.RadioButton
  Me.chkAutorun = New System.Windows.Forms.CheckBox
  Me.chkWopr = New System.Windows.Forms.CheckBox
  Me.ComboBox2 = New System.Windows.Forms.ComboBox
  Me.RadioButton5 = New System.Windows.Forms.RadioButton
  Me.RadioButton4 = New System.Windows.Forms.RadioButton
  Me.RadioButton3 = New System.Windows.Forms.RadioButton
  Me.CheckBox5 = New System.Windows.Forms.CheckBox
  Me.GroupBox2 = New System.Windows.Forms.GroupBox
  Me.CHKTime = New System.Windows.Forms.CheckBox
  Me.chkHash = New System.Windows.Forms.CheckBox
  Me.CheckBox3 = New System.Windows.Forms.CheckBox
  Me.CheckBox2 = New System.Windows.Forms.CheckBox
  Me.TextBox2 = New System.Windows.Forms.TextBox
  Me.CheckBox1 = New System.Windows.Forms.CheckBox
  Me.RadioButton2 = New System.Windows.Forms.RadioButton
  Me.RadioButton1 = New System.Windows.Forms.RadioButton
  Me.Button1 = New System.Windows.Forms.Button
  Me.Button2 = New System.Windows.Forms.Button
  Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
  Me.GroupBox4 = New System.Windows.Forms.GroupBox
  Me.chkHistory = New System.Windows.Forms.CheckBox
  Me.GroupBox6 = New System.Windows.Forms.GroupBox
  Me.RadioButton9 = New System.Windows.Forms.RadioButton
  Me.RadioButton8 = New System.Windows.Forms.RadioButton
  Me.RadioButton7 = New System.Windows.Forms.RadioButton
  Me.RadioButton6 = New System.Windows.Forms.RadioButton
  Me.chkMon = New System.Windows.Forms.CheckBox
  Me.chkArck = New System.Windows.Forms.CheckBox
  Me.chkRash = New System.Windows.Forms.CheckBox
  Me.chk_autozap = New System.Windows.Forms.CheckBox
  Me.chkReg = New System.Windows.Forms.CheckBox
  Me.chkEvristic = New System.Windows.Forms.CheckBox
  Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
  Me.TabControl1 = New System.Windows.Forms.TabControl
  Me.TabPage5 = New System.Windows.Forms.TabPage
  Me.GroupBox7 = New System.Windows.Forms.GroupBox
  Me.chkMEM = New System.Windows.Forms.CheckBox
  Me.TabPage1 = New System.Windows.Forms.TabPage
  Me.TabPage4 = New System.Windows.Forms.TabPage
  Me.TabPage3 = New System.Windows.Forms.TabPage
  Me.TabPage2 = New System.Windows.Forms.TabPage
  Me.GroupBox1.SuspendLayout()
  Me.GroupBox5.SuspendLayout()
  Me.GroupBox3.SuspendLayout()
  Me.GroupBox2.SuspendLayout()
  Me.GroupBox4.SuspendLayout()
  Me.GroupBox6.SuspendLayout()
  Me.TabControl1.SuspendLayout()
  Me.TabPage5.SuspendLayout()
  Me.GroupBox7.SuspendLayout()
  Me.TabPage1.SuspendLayout()
  Me.TabPage4.SuspendLayout()
  Me.TabPage3.SuspendLayout()
  Me.TabPage2.SuspendLayout()
  Me.SuspendLayout()
  '
  'GroupBox1
  '
  Me.GroupBox1.Controls.Add(Me.ListBox1)
  Me.GroupBox1.Controls.Add(Me.GroupBox5)
  Me.GroupBox1.Controls.Add(Me.Button4)
  Me.GroupBox1.Controls.Add(Me.Button5)
  Me.GroupBox1.Location = New System.Drawing.Point(6, 7)
  Me.GroupBox1.Name = "GroupBox1"
  Me.GroupBox1.Size = New System.Drawing.Size(305, 188)
  Me.GroupBox1.TabIndex = 7
  Me.GroupBox1.TabStop = False
  Me.GroupBox1.Text = "Excluded Folder"
  '
  'ListBox1
  '
  Me.ListBox1.FormattingEnabled = True
  Me.ListBox1.HorizontalScrollbar = True
  Me.ListBox1.Location = New System.Drawing.Point(12, 19)
  Me.ListBox1.Name = "ListBox1"
  Me.ListBox1.Size = New System.Drawing.Size(224, 108)
  Me.ListBox1.TabIndex = 4
  '
  'GroupBox5
  '
  Me.GroupBox5.Controls.Add(Me.txtCheckLen)
  Me.GroupBox5.Controls.Add(Me.chfFileSZ)
  Me.GroupBox5.Location = New System.Drawing.Point(6, 135)
  Me.GroupBox5.Name = "GroupBox5"
  Me.GroupBox5.Size = New System.Drawing.Size(293, 43)
  Me.GroupBox5.TabIndex = 3
  Me.GroupBox5.TabStop = False
  Me.GroupBox5.Text = "Size scanning file"
  '
  'txtCheckLen
  '
  Me.txtCheckLen.Location = New System.Drawing.Point(173, 16)
  Me.txtCheckLen.Name = "txtCheckLen"
  Me.txtCheckLen.Size = New System.Drawing.Size(84, 20)
  Me.txtCheckLen.TabIndex = 1
  '
  'chfFileSZ
  '
  Me.chfFileSZ.AutoSize = True
  Me.chfFileSZ.Location = New System.Drawing.Point(6, 19)
  Me.chfFileSZ.Name = "chfFileSZ"
  Me.chfFileSZ.Size = New System.Drawing.Size(143, 17)
  Me.chfFileSZ.TabIndex = 0
  Me.chfFileSZ.Text = "Scanning longer , byte(s)"
  Me.chfFileSZ.UseVisualStyleBackColor = True
  '
  'Button4
  '
  Me.Button4.Location = New System.Drawing.Point(242, 48)
  Me.Button4.Name = "Button4"
  Me.Button4.Size = New System.Drawing.Size(45, 23)
  Me.Button4.TabIndex = 2
  Me.Button4.Text = "-"
  Me.ToolTip1.SetToolTip(Me.Button4, "Remove record")
  Me.Button4.UseVisualStyleBackColor = True
  '
  'Button5
  '
  Me.Button5.Location = New System.Drawing.Point(242, 19)
  Me.Button5.Name = "Button5"
  Me.Button5.Size = New System.Drawing.Size(45, 23)
  Me.Button5.TabIndex = 1
  Me.Button5.Text = "+"
  Me.ToolTip1.SetToolTip(Me.Button5, "Add folder")
  Me.Button5.UseVisualStyleBackColor = True
  '
  'GroupBox3
  '
  Me.GroupBox3.Controls.Add(Me.RadioButton10)
  Me.GroupBox3.Controls.Add(Me.chkAutorun)
  Me.GroupBox3.Controls.Add(Me.chkWopr)
  Me.GroupBox3.Controls.Add(Me.ComboBox2)
  Me.GroupBox3.Controls.Add(Me.RadioButton5)
  Me.GroupBox3.Controls.Add(Me.RadioButton4)
  Me.GroupBox3.Controls.Add(Me.RadioButton3)
  Me.GroupBox3.Location = New System.Drawing.Point(6, 7)
  Me.GroupBox3.Name = "GroupBox3"
  Me.GroupBox3.Size = New System.Drawing.Size(305, 185)
  Me.GroupBox3.TabIndex = 6
  Me.GroupBox3.TabStop = False
  Me.GroupBox3.Text = "Actions"
  '
  'RadioButton10
  '
  Me.RadioButton10.AutoSize = True
  Me.RadioButton10.Location = New System.Drawing.Point(11, 117)
  Me.RadioButton10.Name = "RadioButton10"
  Me.RadioButton10.Size = New System.Drawing.Size(53, 17)
  Me.RadioButton10.TabIndex = 9
  Me.RadioButton10.TabStop = True
  Me.RadioButton10.Text = "LOCK"
  Me.RadioButton10.UseVisualStyleBackColor = True
  '
  'chkAutorun
  '
  Me.chkAutorun.AutoSize = True
  Me.chkAutorun.Location = New System.Drawing.Point(11, 163)
  Me.chkAutorun.Name = "chkAutorun"
  Me.chkAutorun.Size = New System.Drawing.Size(128, 17)
  Me.chkAutorun.TabIndex = 8
  Me.chkAutorun.Text = "DELETE autorun-files"
  Me.chkAutorun.UseVisualStyleBackColor = True
  '
  'chkWopr
  '
  Me.chkWopr.AutoSize = True
  Me.chkWopr.Location = New System.Drawing.Point(11, 140)
  Me.chkWopr.Name = "chkWopr"
  Me.chkWopr.Size = New System.Drawing.Size(61, 17)
  Me.chkWopr.TabIndex = 5
  Me.chkWopr.Text = "Ask me"
  Me.chkWopr.UseVisualStyleBackColor = True
  '
  'ComboBox2
  '
  Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
  Me.ComboBox2.FormattingEnabled = True
  Me.ComboBox2.Location = New System.Drawing.Point(11, 21)
  Me.ComboBox2.Name = "ComboBox2"
  Me.ComboBox2.Size = New System.Drawing.Size(197, 21)
  Me.ComboBox2.TabIndex = 4
  Me.ToolTip1.SetToolTip(Me.ComboBox2, "Выбор действия с вредоносным ПО")
  '
  'RadioButton5
  '
  Me.RadioButton5.AutoSize = True
  Me.RadioButton5.Location = New System.Drawing.Point(11, 94)
  Me.RadioButton5.Name = "RadioButton5"
  Me.RadioButton5.Size = New System.Drawing.Size(67, 17)
  Me.RadioButton5.TabIndex = 2
  Me.RadioButton5.Text = "DELETE"
  Me.RadioButton5.UseVisualStyleBackColor = True
  '
  'RadioButton4
  '
  Me.RadioButton4.AutoSize = True
  Me.RadioButton4.Location = New System.Drawing.Point(11, 71)
  Me.RadioButton4.Name = "RadioButton4"
  Me.RadioButton4.Size = New System.Drawing.Size(117, 17)
  Me.RadioButton4.TabIndex = 1
  Me.RadioButton4.Text = "Move to quarantine"
  Me.RadioButton4.UseVisualStyleBackColor = True
  '
  'RadioButton3
  '
  Me.RadioButton3.AutoSize = True
  Me.RadioButton3.Checked = True
  Me.RadioButton3.Location = New System.Drawing.Point(11, 48)
  Me.RadioButton3.Name = "RadioButton3"
  Me.RadioButton3.Size = New System.Drawing.Size(70, 17)
  Me.RadioButton3.TabIndex = 0
  Me.RadioButton3.TabStop = True
  Me.RadioButton3.Text = "REPORT"
  Me.RadioButton3.UseVisualStyleBackColor = True
  '
  'CheckBox5
  '
  Me.CheckBox5.AutoSize = True
  Me.CheckBox5.Location = New System.Drawing.Point(19, 20)
  Me.CheckBox5.Name = "CheckBox5"
  Me.CheckBox5.Size = New System.Drawing.Size(57, 17)
  Me.CheckBox5.TabIndex = 4
  Me.CheckBox5.Text = "Sound"
  Me.ToolTip1.SetToolTip(Me.CheckBox5, "Воспроизводить звук")
  Me.CheckBox5.UseVisualStyleBackColor = True
  '
  'GroupBox2
  '
  Me.GroupBox2.Controls.Add(Me.CHKTime)
  Me.GroupBox2.Controls.Add(Me.chkHash)
  Me.GroupBox2.Controls.Add(Me.CheckBox3)
  Me.GroupBox2.Controls.Add(Me.CheckBox2)
  Me.GroupBox2.Controls.Add(Me.TextBox2)
  Me.GroupBox2.Controls.Add(Me.CheckBox1)
  Me.GroupBox2.Controls.Add(Me.RadioButton2)
  Me.GroupBox2.Controls.Add(Me.RadioButton1)
  Me.GroupBox2.Location = New System.Drawing.Point(8, 6)
  Me.GroupBox2.Name = "GroupBox2"
  Me.GroupBox2.Size = New System.Drawing.Size(303, 186)
  Me.GroupBox2.TabIndex = 5
  Me.GroupBox2.TabStop = False
  Me.GroupBox2.Text = "REPORT"
  '
  'CHKTime
  '
  Me.CHKTime.AutoSize = True
  Me.CHKTime.Location = New System.Drawing.Point(17, 164)
  Me.CHKTime.Name = "CHKTime"
  Me.CHKTime.Size = New System.Drawing.Size(115, 17)
  Me.CHKTime.TabIndex = 7
  Me.CHKTime.Text = "Surplus information"
  Me.CHKTime.UseVisualStyleBackColor = True
  '
  'chkHash
  '
  Me.chkHash.AutoSize = True
  Me.chkHash.Checked = True
  Me.chkHash.CheckState = System.Windows.Forms.CheckState.Checked
  Me.chkHash.Location = New System.Drawing.Point(17, 141)
  Me.chkHash.Name = "chkHash"
  Me.chkHash.Size = New System.Drawing.Size(97, 17)
  Me.chkHash.TabIndex = 6
  Me.chkHash.Text = "Logh hash files"
  Me.chkHash.UseVisualStyleBackColor = True
  '
  'CheckBox3
  '
  Me.CheckBox3.AutoSize = True
  Me.CheckBox3.Location = New System.Drawing.Point(17, 118)
  Me.CheckBox3.Name = "CheckBox3"
  Me.CheckBox3.Size = New System.Drawing.Size(101, 17)
  Me.CheckBox3.TabIndex = 5
  Me.CheckBox3.Text = "Log files activity"
  Me.CheckBox3.UseVisualStyleBackColor = True
  '
  'CheckBox2
  '
  Me.CheckBox2.AutoSize = True
  Me.CheckBox2.Checked = True
  Me.CheckBox2.CheckState = System.Windows.Forms.CheckState.Checked
  Me.CheckBox2.Location = New System.Drawing.Point(17, 95)
  Me.CheckBox2.Name = "CheckBox2"
  Me.CheckBox2.Size = New System.Drawing.Size(122, 17)
  Me.CheckBox2.TabIndex = 4
  Me.CheckBox2.Text = "Limit log size, byte(s)"
  Me.ToolTip1.SetToolTip(Me.CheckBox2, "Limit log size, byte(s)")
  Me.CheckBox2.UseVisualStyleBackColor = True
  '
  'TextBox2
  '
  Me.TextBox2.Location = New System.Drawing.Point(145, 93)
  Me.TextBox2.Name = "TextBox2"
  Me.TextBox2.Size = New System.Drawing.Size(72, 20)
  Me.TextBox2.TabIndex = 3
  Me.TextBox2.Text = "5860966"
  '
  'CheckBox1
  '
  Me.CheckBox1.AutoSize = True
  Me.CheckBox1.Checked = True
  Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
  Me.CheckBox1.Location = New System.Drawing.Point(17, 26)
  Me.CheckBox1.Name = "CheckBox1"
  Me.CheckBox1.Size = New System.Drawing.Size(72, 17)
  Me.CheckBox1.TabIndex = 2
  Me.CheckBox1.Text = "Log to file"
  Me.CheckBox1.UseVisualStyleBackColor = True
  '
  'RadioButton2
  '
  Me.RadioButton2.AutoSize = True
  Me.RadioButton2.Location = New System.Drawing.Point(26, 72)
  Me.RadioButton2.Name = "RadioButton2"
  Me.RadioButton2.Size = New System.Drawing.Size(70, 17)
  Me.RadioButton2.TabIndex = 1
  Me.RadioButton2.Text = "Overwrite"
  Me.RadioButton2.UseVisualStyleBackColor = True
  '
  'RadioButton1
  '
  Me.RadioButton1.AutoSize = True
  Me.RadioButton1.Checked = True
  Me.RadioButton1.Location = New System.Drawing.Point(26, 49)
  Me.RadioButton1.Name = "RadioButton1"
  Me.RadioButton1.Size = New System.Drawing.Size(62, 17)
  Me.RadioButton1.TabIndex = 0
  Me.RadioButton1.TabStop = True
  Me.RadioButton1.Text = "Append"
  Me.RadioButton1.UseVisualStyleBackColor = True
  '
  'Button1
  '
  Me.Button1.Location = New System.Drawing.Point(354, 31)
  Me.Button1.Name = "Button1"
  Me.Button1.Size = New System.Drawing.Size(75, 23)
  Me.Button1.TabIndex = 8
  Me.Button1.Text = "OK"
  Me.ToolTip1.SetToolTip(Me.Button1, "Save settings")
  Me.Button1.UseVisualStyleBackColor = True
  '
  'Button2
  '
  Me.Button2.Location = New System.Drawing.Point(354, 60)
  Me.Button2.Name = "Button2"
  Me.Button2.Size = New System.Drawing.Size(75, 23)
  Me.Button2.TabIndex = 9
  Me.Button2.Text = "Cancel"
  Me.ToolTip1.SetToolTip(Me.Button2, "Close")
  Me.Button2.UseVisualStyleBackColor = True
  '
  'GroupBox4
  '
  Me.GroupBox4.Controls.Add(Me.chkHistory)
  Me.GroupBox4.Controls.Add(Me.GroupBox6)
  Me.GroupBox4.Controls.Add(Me.chkMon)
  Me.GroupBox4.Controls.Add(Me.CheckBox5)
  Me.GroupBox4.Location = New System.Drawing.Point(8, 6)
  Me.GroupBox4.Name = "GroupBox4"
  Me.GroupBox4.Size = New System.Drawing.Size(303, 183)
  Me.GroupBox4.TabIndex = 11
  Me.GroupBox4.TabStop = False
  Me.GroupBox4.Text = "Misc"
  '
  'chkHistory
  '
  Me.chkHistory.AutoSize = True
  Me.chkHistory.Location = New System.Drawing.Point(19, 67)
  Me.chkHistory.Name = "chkHistory"
  Me.chkHistory.Size = New System.Drawing.Size(105, 17)
  Me.chkHistory.TabIndex = 17
  Me.chkHistory.Text = "Delete IE History"
  Me.chkHistory.UseVisualStyleBackColor = True
  '
  'GroupBox6
  '
  Me.GroupBox6.Controls.Add(Me.RadioButton9)
  Me.GroupBox6.Controls.Add(Me.RadioButton8)
  Me.GroupBox6.Controls.Add(Me.RadioButton7)
  Me.GroupBox6.Controls.Add(Me.RadioButton6)
  Me.GroupBox6.Location = New System.Drawing.Point(179, 10)
  Me.GroupBox6.Name = "GroupBox6"
  Me.GroupBox6.Size = New System.Drawing.Size(99, 113)
  Me.GroupBox6.TabIndex = 0
  Me.GroupBox6.TabStop = False
  Me.GroupBox6.Text = "Priority"
  Me.ToolTip1.SetToolTip(Me.GroupBox6, "Priority")
  '
  'RadioButton9
  '
  Me.RadioButton9.AutoSize = True
  Me.RadioButton9.Location = New System.Drawing.Point(6, 92)
  Me.RadioButton9.Name = "RadioButton9"
  Me.RadioButton9.Size = New System.Drawing.Size(42, 17)
  Me.RadioButton9.TabIndex = 3
  Me.RadioButton9.Text = "Idle"
  Me.RadioButton9.UseVisualStyleBackColor = True
  '
  'RadioButton8
  '
  Me.RadioButton8.AutoSize = True
  Me.RadioButton8.Checked = True
  Me.RadioButton8.Location = New System.Drawing.Point(6, 69)
  Me.RadioButton8.Name = "RadioButton8"
  Me.RadioButton8.Size = New System.Drawing.Size(58, 17)
  Me.RadioButton8.TabIndex = 2
  Me.RadioButton8.TabStop = True
  Me.RadioButton8.Text = "Normal"
  Me.RadioButton8.UseVisualStyleBackColor = True
  '
  'RadioButton7
  '
  Me.RadioButton7.AutoSize = True
  Me.RadioButton7.Location = New System.Drawing.Point(6, 46)
  Me.RadioButton7.Name = "RadioButton7"
  Me.RadioButton7.Size = New System.Drawing.Size(50, 17)
  Me.RadioButton7.TabIndex = 1
  Me.RadioButton7.Text = "Hight"
  Me.RadioButton7.UseVisualStyleBackColor = True
  '
  'RadioButton6
  '
  Me.RadioButton6.AutoSize = True
  Me.RadioButton6.Location = New System.Drawing.Point(6, 23)
  Me.RadioButton6.Name = "RadioButton6"
  Me.RadioButton6.Size = New System.Drawing.Size(73, 17)
  Me.RadioButton6.TabIndex = 0
  Me.RadioButton6.Text = "Real Time"
  Me.RadioButton6.UseVisualStyleBackColor = True
  '
  'chkMon
  '
  Me.chkMon.AutoSize = True
  Me.chkMon.Location = New System.Drawing.Point(19, 44)
  Me.chkMon.Name = "chkMon"
  Me.chkMon.Size = New System.Drawing.Size(77, 17)
  Me.chkMon.TabIndex = 11
  Me.chkMon.Text = "Save state"
  Me.ToolTip1.SetToolTip(Me.chkMon, "Запоминать состояние мониторинга")
  Me.chkMon.UseVisualStyleBackColor = True
  '
  'chkArck
  '
  Me.chkArck.AutoSize = True
  Me.chkArck.Location = New System.Drawing.Point(19, 28)
  Me.chkArck.Name = "chkArck"
  Me.chkArck.Size = New System.Drawing.Size(95, 17)
  Me.chkArck.TabIndex = 17
  Me.chkArck.Text = "Check archive"
  Me.chkArck.UseVisualStyleBackColor = True
  '
  'chkRash
  '
  Me.chkRash.AutoSize = True
  Me.chkRash.Location = New System.Drawing.Point(19, 51)
  Me.chkRash.Name = "chkRash"
  Me.chkRash.Size = New System.Drawing.Size(147, 17)
  Me.chkRash.TabIndex = 15
  Me.chkRash.Text = "Scan only insecure types "
  Me.ToolTip1.SetToolTip(Me.chkRash, "Эта проверка только для измененных или новых файлов,на процессы она не распростра" & _
          "няется")
  Me.chkRash.UseVisualStyleBackColor = True
  '
  'chk_autozap
  '
  Me.chk_autozap.AutoSize = True
  Me.chk_autozap.Location = New System.Drawing.Point(19, 77)
  Me.chk_autozap.Name = "chk_autozap"
  Me.chk_autozap.Size = New System.Drawing.Size(97, 17)
  Me.chk_autozap.TabIndex = 14
  Me.chk_autozap.Text = "Startup objects"
  Me.chk_autozap.UseVisualStyleBackColor = True
  '
  'chkReg
  '
  Me.chkReg.AutoSize = True
  Me.chkReg.Location = New System.Drawing.Point(19, 100)
  Me.chkReg.Name = "chkReg"
  Me.chkReg.Size = New System.Drawing.Size(93, 17)
  Me.chkReg.TabIndex = 13
  Me.chkReg.Text = "Check registry"
  Me.chkReg.UseVisualStyleBackColor = True
  '
  'chkEvristic
  '
  Me.chkEvristic.AutoSize = True
  Me.chkEvristic.Location = New System.Drawing.Point(19, 123)
  Me.chkEvristic.Name = "chkEvristic"
  Me.chkEvristic.Size = New System.Drawing.Size(67, 17)
  Me.chkEvristic.TabIndex = 12
  Me.chkEvristic.Text = "Heuristic"
  Me.chkEvristic.UseVisualStyleBackColor = True
  '
  'ToolTip1
  '
  Me.ToolTip1.IsBalloon = True
  '
  'TabControl1
  '
  Me.TabControl1.Controls.Add(Me.TabPage5)
  Me.TabControl1.Controls.Add(Me.TabPage1)
  Me.TabControl1.Controls.Add(Me.TabPage4)
  Me.TabControl1.Controls.Add(Me.TabPage3)
  Me.TabControl1.Controls.Add(Me.TabPage2)
  Me.TabControl1.Location = New System.Drawing.Point(12, 12)
  Me.TabControl1.Name = "TabControl1"
  Me.TabControl1.SelectedIndex = 0
  Me.TabControl1.Size = New System.Drawing.Size(325, 221)
  Me.TabControl1.TabIndex = 12
  '
  'TabPage5
  '
  Me.TabPage5.Controls.Add(Me.GroupBox7)
  Me.TabPage5.Location = New System.Drawing.Point(4, 22)
  Me.TabPage5.Name = "TabPage5"
  Me.TabPage5.Size = New System.Drawing.Size(317, 195)
  Me.TabPage5.TabIndex = 4
  Me.TabPage5.Text = "Scanning"
  Me.TabPage5.UseVisualStyleBackColor = True
  '
  'GroupBox7
  '
  Me.GroupBox7.Controls.Add(Me.chkMEM)
  Me.GroupBox7.Controls.Add(Me.chkArck)
  Me.GroupBox7.Controls.Add(Me.chk_autozap)
  Me.GroupBox7.Controls.Add(Me.chkRash)
  Me.GroupBox7.Controls.Add(Me.chkEvristic)
  Me.GroupBox7.Controls.Add(Me.chkReg)
  Me.GroupBox7.Location = New System.Drawing.Point(3, 13)
  Me.GroupBox7.Name = "GroupBox7"
  Me.GroupBox7.Size = New System.Drawing.Size(311, 179)
  Me.GroupBox7.TabIndex = 0
  Me.GroupBox7.TabStop = False
  Me.GroupBox7.Text = "Options"
  '
  'chkMEM
  '
  Me.chkMEM.AutoSize = True
  Me.chkMEM.Location = New System.Drawing.Point(19, 146)
  Me.chkMEM.Name = "chkMEM"
  Me.chkMEM.Size = New System.Drawing.Size(91, 17)
  Me.chkMEM.TabIndex = 18
  Me.chkMEM.Text = "Scan Memory"
  Me.chkMEM.UseVisualStyleBackColor = True
  '
  'TabPage1
  '
  Me.TabPage1.Controls.Add(Me.GroupBox2)
  Me.TabPage1.Location = New System.Drawing.Point(4, 22)
  Me.TabPage1.Name = "TabPage1"
  Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
  Me.TabPage1.Size = New System.Drawing.Size(317, 195)
  Me.TabPage1.TabIndex = 0
  Me.TabPage1.Text = "REPORT"
  Me.TabPage1.UseVisualStyleBackColor = True
  '
  'TabPage4
  '
  Me.TabPage4.Controls.Add(Me.GroupBox3)
  Me.TabPage4.Location = New System.Drawing.Point(4, 22)
  Me.TabPage4.Name = "TabPage4"
  Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
  Me.TabPage4.Size = New System.Drawing.Size(317, 195)
  Me.TabPage4.TabIndex = 3
  Me.TabPage4.Text = "Actions"
  Me.TabPage4.UseVisualStyleBackColor = True
  '
  'TabPage3
  '
  Me.TabPage3.Controls.Add(Me.GroupBox1)
  Me.TabPage3.Location = New System.Drawing.Point(4, 22)
  Me.TabPage3.Name = "TabPage3"
  Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
  Me.TabPage3.Size = New System.Drawing.Size(317, 195)
  Me.TabPage3.TabIndex = 2
  Me.TabPage3.Text = "Exclude"
  Me.TabPage3.UseVisualStyleBackColor = True
  '
  'TabPage2
  '
  Me.TabPage2.Controls.Add(Me.GroupBox4)
  Me.TabPage2.Location = New System.Drawing.Point(4, 22)
  Me.TabPage2.Name = "TabPage2"
  Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
  Me.TabPage2.Size = New System.Drawing.Size(317, 195)
  Me.TabPage2.TabIndex = 1
  Me.TabPage2.Text = "Misc"
  Me.TabPage2.UseVisualStyleBackColor = True
  '
  'Nastr
  '
  Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
  Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
  Me.ClientSize = New System.Drawing.Size(438, 237)
  Me.Controls.Add(Me.TabControl1)
  Me.Controls.Add(Me.Button2)
  Me.Controls.Add(Me.Button1)
  Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
  Me.MaximizeBox = False
  Me.MinimizeBox = False
  Me.Name = "Nastr"
  Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
  Me.Text = "Shield settings"
  Me.GroupBox1.ResumeLayout(False)
  Me.GroupBox5.ResumeLayout(False)
  Me.GroupBox5.PerformLayout()
  Me.GroupBox3.ResumeLayout(False)
  Me.GroupBox3.PerformLayout()
  Me.GroupBox2.ResumeLayout(False)
  Me.GroupBox2.PerformLayout()
  Me.GroupBox4.ResumeLayout(False)
  Me.GroupBox4.PerformLayout()
  Me.GroupBox6.ResumeLayout(False)
  Me.GroupBox6.PerformLayout()
  Me.TabControl1.ResumeLayout(False)
  Me.TabPage5.ResumeLayout(False)
  Me.GroupBox7.ResumeLayout(False)
  Me.GroupBox7.PerformLayout()
  Me.TabPage1.ResumeLayout(False)
  Me.TabPage4.ResumeLayout(False)
  Me.TabPage3.ResumeLayout(False)
  Me.TabPage2.ResumeLayout(False)
  Me.ResumeLayout(False)

 End Sub
 Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
 Friend WithEvents Button4 As System.Windows.Forms.Button
 Friend WithEvents Button5 As System.Windows.Forms.Button
 Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
 Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
 Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
 Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
 Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
 Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
 Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
 Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
 Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
 Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
 Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
 Friend WithEvents Button1 As System.Windows.Forms.Button
 Friend WithEvents Button2 As System.Windows.Forms.Button
 Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
 Friend WithEvents CheckBox5 As System.Windows.Forms.CheckBox
 Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMon As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chkHash As System.Windows.Forms.CheckBox
    Friend WithEvents chkEvristic As System.Windows.Forms.CheckBox
    Friend WithEvents chkReg As System.Windows.Forms.CheckBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents txtCheckLen As System.Windows.Forms.TextBox
    Friend WithEvents chfFileSZ As System.Windows.Forms.CheckBox
    Friend WithEvents chkWopr As System.Windows.Forms.CheckBox
    Friend WithEvents CHKTime As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutorun As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton9 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton8 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents chk_autozap As System.Windows.Forms.CheckBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents RadioButton10 As System.Windows.Forms.RadioButton
    Friend WithEvents chkRash As System.Windows.Forms.CheckBox
    Friend WithEvents chkArck As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
 Friend WithEvents chkHistory As System.Windows.Forms.CheckBox
 Friend WithEvents chkMEM As System.Windows.Forms.CheckBox
End Class
