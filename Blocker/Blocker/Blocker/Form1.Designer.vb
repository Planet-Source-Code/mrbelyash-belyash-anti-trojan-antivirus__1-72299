<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
      Me.GroupBox1 = New System.Windows.Forms.GroupBox
      Me.Button4 = New System.Windows.Forms.Button
      Me.Button3 = New System.Windows.Forms.Button
      Me.Button2 = New System.Windows.Forms.Button
      Me.Button1 = New System.Windows.Forms.Button
      Me.PictureBox1 = New System.Windows.Forms.PictureBox
      Me.Label1 = New System.Windows.Forms.Label
      Me.GroupBox2 = New System.Windows.Forms.GroupBox
      Me.Label6 = New System.Windows.Forms.Label
      Me.Label4 = New System.Windows.Forms.Label
      Me.Label5 = New System.Windows.Forms.Label
      Me.Label3 = New System.Windows.Forms.Label
      Me.Label2 = New System.Windows.Forms.Label
      Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
      Me.GroupBox3 = New System.Windows.Forms.GroupBox
      Me.ComboBox1 = New System.Windows.Forms.ComboBox
      Me.Button5 = New System.Windows.Forms.Button
      Me.lblInfected = New System.Windows.Forms.Label
      Me.Label8 = New System.Windows.Forms.Label
      Me.Label7 = New System.Windows.Forms.Label
      Me.PictureBox2 = New System.Windows.Forms.PictureBox
      Me.PictureBox3 = New System.Windows.Forms.PictureBox
      Me.GroupBox4 = New System.Windows.Forms.GroupBox
      Me.GroupBox1.SuspendLayout()
      CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.GroupBox2.SuspendLayout()
      Me.GroupBox3.SuspendLayout()
      CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'GroupBox1
      '
      Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
      Me.GroupBox1.Controls.Add(Me.Button4)
      Me.GroupBox1.Controls.Add(Me.Button3)
      Me.GroupBox1.Controls.Add(Me.Button2)
      Me.GroupBox1.Controls.Add(Me.Button1)
      Me.GroupBox1.Location = New System.Drawing.Point(13, 260)
      Me.GroupBox1.Name = "GroupBox1"
      Me.GroupBox1.Size = New System.Drawing.Size(445, 52)
      Me.GroupBox1.TabIndex = 0
      Me.GroupBox1.TabStop = False
      Me.GroupBox1.Text = "Prompt for Action"
      '
      'Button4
      '
      Me.Button4.Location = New System.Drawing.Point(333, 19)
      Me.Button4.Name = "Button4"
      Me.Button4.Size = New System.Drawing.Size(87, 23)
      Me.Button4.TabIndex = 3
      Me.Button4.Text = "Always &Deny"
      Me.ToolTip1.SetToolTip(Me.Button4, "Deny access for this application every time.")
      Me.Button4.UseVisualStyleBackColor = True
      '
      'Button3
      '
      Me.Button3.Location = New System.Drawing.Point(234, 19)
      Me.Button3.Name = "Button3"
      Me.Button3.Size = New System.Drawing.Size(91, 23)
      Me.Button3.TabIndex = 2
      Me.Button3.Text = "Deny T&his Time"
      Me.ToolTip1.SetToolTip(Me.Button3, "Block this application it now.")
      Me.Button3.UseVisualStyleBackColor = True
      '
      'Button2
      '
      Me.Button2.Location = New System.Drawing.Point(125, 19)
      Me.Button2.Name = "Button2"
      Me.Button2.Size = New System.Drawing.Size(97, 23)
      Me.Button2.TabIndex = 1
      Me.Button2.Text = "Allow &This Time"
      Me.ToolTip1.SetToolTip(Me.Button2, "Trust this application it now.")
      Me.Button2.UseVisualStyleBackColor = True
      '
      'Button1
      '
      Me.Button1.Location = New System.Drawing.Point(20, 19)
      Me.Button1.Name = "Button1"
      Me.Button1.Size = New System.Drawing.Size(94, 23)
      Me.Button1.TabIndex = 0
      Me.Button1.Text = "A&lways Allow"
      Me.ToolTip1.SetToolTip(Me.Button1, "Trust this application.")
      Me.Button1.UseVisualStyleBackColor = True
      '
      'PictureBox1
      '
      Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
      Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
      Me.PictureBox1.Name = "PictureBox1"
      Me.PictureBox1.Size = New System.Drawing.Size(106, 89)
      Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.PictureBox1.TabIndex = 1
      Me.PictureBox1.TabStop = False
      '
      'Label1
      '
      Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
      Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
      Me.Label1.Location = New System.Drawing.Point(6, 16)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(321, 70)
      Me.Label1.TabIndex = 2
      '
      'GroupBox2
      '
      Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
      Me.GroupBox2.Controls.Add(Me.Label6)
      Me.GroupBox2.Controls.Add(Me.Label4)
      Me.GroupBox2.Controls.Add(Me.Label5)
      Me.GroupBox2.Controls.Add(Me.Label3)
      Me.GroupBox2.Controls.Add(Me.Label2)
      Me.GroupBox2.Controls.Add(Me.Label1)
      Me.GroupBox2.Location = New System.Drawing.Point(124, 12)
      Me.GroupBox2.Name = "GroupBox2"
      Me.GroupBox2.Size = New System.Drawing.Size(333, 242)
      Me.GroupBox2.TabIndex = 3
      Me.GroupBox2.TabStop = False
      Me.GroupBox2.Text = "File"
      '
      'Label6
      '
      Me.Label6.AutoSize = True
      Me.Label6.ForeColor = System.Drawing.Color.Black
      Me.Label6.Location = New System.Drawing.Point(11, 100)
      Me.Label6.Name = "Label6"
      Me.Label6.Size = New System.Drawing.Size(38, 13)
      Me.Label6.TabIndex = 5
      Me.Label6.Text = "Name:"
      '
      'Label4
      '
      Me.Label4.AutoSize = True
      Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
      Me.Label4.Location = New System.Drawing.Point(55, 100)
      Me.Label4.Name = "Label4"
      Me.Label4.Size = New System.Drawing.Size(0, 13)
      Me.Label4.TabIndex = 5
      '
      'Label5
      '
      Me.Label5.AutoSize = True
      Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
      Me.Label5.Location = New System.Drawing.Point(40, 226)
      Me.Label5.Name = "Label5"
      Me.Label5.Size = New System.Drawing.Size(0, 13)
      Me.Label5.TabIndex = 4
      '
      'Label3
      '
      Me.Label3.AutoSize = True
      Me.Label3.ForeColor = System.Drawing.Color.Black
      Me.Label3.Location = New System.Drawing.Point(11, 226)
      Me.Label3.Name = "Label3"
      Me.Label3.Size = New System.Drawing.Size(33, 13)
      Me.Label3.TabIndex = 4
      Me.Label3.Text = "MD5:"
      '
      'Label2
      '
      Me.Label2.ForeColor = System.Drawing.Color.Black
      Me.Label2.Location = New System.Drawing.Point(10, 113)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(317, 113)
      Me.Label2.TabIndex = 3
      '
      'ToolTip1
      '
      Me.ToolTip1.IsBalloon = True
      Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
      Me.ToolTip1.ToolTipTitle = "Belyash Application Blocker"
      '
      'GroupBox3
      '
      Me.GroupBox3.BackColor = System.Drawing.Color.Red
      Me.GroupBox3.Controls.Add(Me.GroupBox4)
      Me.GroupBox3.Controls.Add(Me.ComboBox1)
      Me.GroupBox3.Controls.Add(Me.Button5)
      Me.GroupBox3.Controls.Add(Me.lblInfected)
      Me.GroupBox3.Controls.Add(Me.Label8)
      Me.GroupBox3.Controls.Add(Me.Label7)
      Me.GroupBox3.Controls.Add(Me.PictureBox2)
      Me.GroupBox3.Controls.Add(Me.PictureBox3)
      Me.GroupBox3.Location = New System.Drawing.Point(4, 107)
      Me.GroupBox3.Name = "GroupBox3"
      Me.GroupBox3.Size = New System.Drawing.Size(462, 147)
      Me.GroupBox3.TabIndex = 6
      Me.GroupBox3.TabStop = False
      Me.GroupBox3.Visible = False
      '
      'ComboBox1
      '
      Me.ComboBox1.FormattingEnabled = True
      Me.ComboBox1.Location = New System.Drawing.Point(296, 110)
      Me.ComboBox1.Name = "ComboBox1"
      Me.ComboBox1.Size = New System.Drawing.Size(111, 21)
      Me.ComboBox1.TabIndex = 4
      Me.ComboBox1.Text = "Ignore"
      '
      'Button5
      '
      Me.Button5.Location = New System.Drawing.Point(413, 112)
      Me.Button5.Name = "Button5"
      Me.Button5.Size = New System.Drawing.Size(40, 19)
      Me.Button5.TabIndex = 3
      Me.Button5.Text = "OK"
      Me.Button5.UseVisualStyleBackColor = True
      '
      'lblInfected
      '
      Me.lblInfected.AutoSize = True
      Me.lblInfected.BackColor = System.Drawing.Color.Red
      Me.lblInfected.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
      Me.lblInfected.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.lblInfected.Location = New System.Drawing.Point(166, 57)
      Me.lblInfected.Name = "lblInfected"
      Me.lblInfected.Size = New System.Drawing.Size(0, 15)
      Me.lblInfected.TabIndex = 2
      '
      'Label8
      '
      Me.Label8.AutoSize = True
      Me.Label8.BackColor = System.Drawing.Color.Red
      Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
      Me.Label8.ForeColor = System.Drawing.SystemColors.ControlLightLight
      Me.Label8.Location = New System.Drawing.Point(111, 59)
      Me.Label8.Name = "Label8"
      Me.Label8.Size = New System.Drawing.Size(58, 13)
      Me.Label8.TabIndex = 2
      Me.Label8.Text = "Infected:"
      '
      'Label7
      '
      Me.Label7.AutoSize = True
      Me.Label7.BackColor = System.Drawing.Color.Red
      Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
      Me.Label7.ForeColor = System.Drawing.Color.Blue
      Me.Label7.Location = New System.Drawing.Point(192, 16)
      Me.Label7.Name = "Label7"
      Me.Label7.Size = New System.Drawing.Size(131, 24)
      Me.Label7.TabIndex = 1
      Me.Label7.Text = "WARNING !!!"
      '
      'PictureBox2
      '
      Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
      Me.PictureBox2.Location = New System.Drawing.Point(9, 19)
      Me.PictureBox2.Name = "PictureBox2"
      Me.PictureBox2.Size = New System.Drawing.Size(99, 99)
      Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.PictureBox2.TabIndex = 0
      Me.PictureBox2.TabStop = False
      '
      'PictureBox3
      '
      Me.PictureBox3.BackColor = System.Drawing.Color.Red
      Me.PictureBox3.Location = New System.Drawing.Point(6, 8)
      Me.PictureBox3.Name = "PictureBox3"
      Me.PictureBox3.Size = New System.Drawing.Size(450, 133)
      Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
      Me.PictureBox3.TabIndex = 6
      Me.PictureBox3.TabStop = False
      '
      'GroupBox4
      '
      Me.GroupBox4.BackColor = System.Drawing.Color.Red
      Me.GroupBox4.Location = New System.Drawing.Point(111, 84)
      Me.GroupBox4.Name = "GroupBox4"
      Me.GroupBox4.Size = New System.Drawing.Size(342, 20)
      Me.GroupBox4.TabIndex = 7
      Me.GroupBox4.TabStop = False
      '
      'Form1
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
      Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
      Me.ClientSize = New System.Drawing.Size(470, 324)
      Me.Controls.Add(Me.GroupBox3)
      Me.Controls.Add(Me.GroupBox2)
      Me.Controls.Add(Me.PictureBox1)
      Me.Controls.Add(Me.GroupBox1)
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "Form1"
      Me.Opacity = 0.93
      Me.ShowInTaskbar = False
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.Text = "Belyash Application Blocker"
      Me.GroupBox1.ResumeLayout(False)
      CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.GroupBox2.ResumeLayout(False)
      Me.GroupBox2.PerformLayout()
      Me.GroupBox3.ResumeLayout(False)
      Me.GroupBox3.PerformLayout()
      CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

   End Sub
 Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
 Friend WithEvents Button4 As System.Windows.Forms.Button
 Friend WithEvents Button3 As System.Windows.Forms.Button
 Friend WithEvents Button2 As System.Windows.Forms.Button
 Friend WithEvents Button1 As System.Windows.Forms.Button
 Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
 Friend WithEvents Label1 As System.Windows.Forms.Label
 Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
 Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
 Friend WithEvents Label3 As System.Windows.Forms.Label
 Friend WithEvents Label2 As System.Windows.Forms.Label
 Friend WithEvents Label5 As System.Windows.Forms.Label
 Friend WithEvents Label4 As System.Windows.Forms.Label
   Friend WithEvents Label6 As System.Windows.Forms.Label
   Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
   Friend WithEvents Label7 As System.Windows.Forms.Label
   Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
   Friend WithEvents Button5 As System.Windows.Forms.Button
   Friend WithEvents lblInfected As System.Windows.Forms.Label
   Friend WithEvents Label8 As System.Windows.Forms.Label
   Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
   Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
   Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox

End Class
