Imports System.IO
Public Class frmActivate
    Dim MyLibrary As New MyLibrary.MyLib
    Public hlpfile As String = Application.StartupPath & "\BelyashAV.chm" ' справка
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim Successful As Boolean
        Successful = SL.Register(txtSK1.Text & txtSK2.Text & txtSK3.Text & txtSK4.Text)
        'Successful = SL.Register(txtSK1.Text)
        If Successful = True Then
            GlassBox.ShowMessage("Software successfully registered. Thank You!", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Information, MessageBoxButtons.OK)
            'MsgBox("Software successfully registered. Thank You!", MsgBoxStyle.Information)
            Dim licendUs As String = sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName)
            If Trim(licendUs).Length > 23 Then
                Form1.Label17.Text = "Licensed To [" & sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName).Substring(0, 23) & "]"
            Else
                Form1.Label17.Text = "Licensed To [" & sGetINI(sINIFile, "USER", "Name", MyLibrary.FormFunction.GetUserName) & "]"
            End If
            Form1.Label12.Text = "License Expired: " & Format(Now, "dd.MM") & "." & (Format(Now, "yyyy")) + 1
            Form1.LinkLabel7.Enabled = False
            Form1.Label17.BackColor = Color.Yellow
            myRegister = True
        Else
            sound_me("lic")
   GlassBox.ShowMessage("There was an error registering your software!", "      Belyash Anti-Trojan 2009b", MessageBoxIcon.Exclamation, MessageBoxButtons.OK)
            'MsgBox(", MsgBoxStyle.Critical)
            Form1.Label17.Text = "Software not registered."
            Form1.Label17.BackColor = Color.Red
        End If
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmActivate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F1 Then
            Help.ShowHelp(Me, hlpfile) 'вызов справки
        End If
    End Sub


    Private Sub frmActivate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        get_key()

    End Sub
    Sub get_key()

        If File.Exists(Application.StartupPath & "\SETTINGS.INI") = False Then
            TextBox1.Text = UCase$(MyLibrary.FormFunction.get_serialdisk)
            TextBox2.Text = UCase$(MyLibrary.FormFunction.GetUserName)
            TextBox3.Text = UCase$(MyLibrary.FormFunction.GetComputerName)
            txtSK1.Text = ""
        Else
            Try
                TextBox1.Text = sGetINI(sINIFile, "USER", "Number", UCase$(MyLibrary.FormFunction.get_serialdisk))
                TextBox2.Text = sGetINI(sINIFile, "USER", "Name", UCase$(MyLibrary.FormFunction.GetUserName))
                TextBox3.Text = sGetINI(sINIFile, "USER", "Machine", UCase$(MyLibrary.FormFunction.GetComputerName))
                txtSK1.Text = sGetINI(sINIFile, "USER", "SN", "")

                If SL.isRegistered = True Then
                    Label2.Text = "Software registered."
                    Button2.Enabled = False
                Else
                    Label2.Text = "Software not registered."
                End If
            Catch ex As IOException
                MsgBox(ex.ToString)
            End Try
        End If



    End Sub

  
    Private Sub txtSK1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSK1.TextChanged, txtSK4.TextChanged, txtSK3.TextChanged, txtSK2.TextChanged
        Button2.Enabled = Enabled
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        Button2.Enabled = Enabled
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        Button2.Enabled = Enabled
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Button2.Enabled = Enabled
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Help.ShowHelp(Me, hlpfile) 'вызов справки
    End Sub
End Class