Imports System.IO

Public Class NwZapis
    Dim keysize As cript.clsAESV2.KeySize
    Dim MyLibrary As New MyLibrary.MyLib
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        lvwbaze.Items.Clear()
        ListView1.Items.Clear()
        Label2.Text = "Для добавления определенного файла в базу троянов -необходимо найти этот файл" & vbCrLf & " на диске. Для этого используется специальный алгоритм. Необходимо найти файл" & vbCrLf & " и дать имя новой записи.Желательно не давать одинаковые имена(хотя это и не" & vbCrLf & " значительно,т.к. поск в базе ведется по md5 хешу)."
        view_baze()
        view_baze2()
        HelpProvider1.HelpNamespace = Application.StartupPath & "\Shield.chm"

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        On Error GoTo 101
        TextBox1.Text = ""
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = Application.StartupPath
        OpenFileDialog.Filter = "Programm(*.exe)|*.exe|All Files (*.*)|*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
            ' TODO: Add code here to open the file.
            ' view_baze() 'просмотр добавленного файла
            Dim tmp_name As String
            tmp_name = InputBox("Enter name", "Save", "None")
            If tmp_name = "None" Or Trim(tmp_name) = "" Then
                MsgBox("Enter another name", MsgBoxStyle.Critical)
                Exit Sub
            End If

            If UCase(FileName) = UCase(Application.ExecutablePath) Then
                MsgBox("Dont add this application on virus base", MsgBoxStyle.Critical, "Error")
                Exit Sub
            End If
            Dim tmp_hs As String = MD5_Hash(FileName)
            lblMD5.Text = tmp_hs

         Dim li As New ListViewItem(tmp_hs)
            li.SubItems.Add(UCase(tmp_name))
            lvwbaze.Items.Add(li)
            TextBox1.Text = FileName
        End If
        Exit Sub
101:

        ErrorLog("Button3_Click " & ErrorToString())
    End Sub
    Public Sub save_baze()
        On Error GoTo 10
        Dim path As String = Application.StartupPath & "\uzerbase.bvb"
        If File.Exists(path) = True Then
            ' Create a file to write to.
            File.DELETE(path)
        End If
        If lvwbaze.Items.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer
        For i = 0 To lvwbaze.Items.Count - 1
            Dim sw As StreamWriter = File.AppendText(path)
            sw.WriteLine(lvwbaze.Items(i).Text & "|" & lvwbaze.Items(i).SubItems(1).Text)
            'sw.WriteLine(lvwbaze.Items(i).SubItems(1))
            sw.Flush()
            sw.Close()
        Next
        MsgBox("New record add in base", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
        Exit Sub
10:

        ErrorLog("save_baze " & ErrorToString())
    End Sub
    Public Sub view_baze()

        Dim fname As String = Application.StartupPath & "\uzerbase.bvb"
        If File.Exists(fname) = False Then
            Exit Sub
        End If
        Try
            ' If lvwbaze.Items.Count > 1 Then
            'lvwbaze.Items.Clear()
            'End If
            ' Create an instance of StreamReader to read from a file.
            Using sr As StreamReader = New StreamReader(fname)
                Dim line As String
                ' Read and display the lines from the file until the end 
                ' of the file is reached.
                Do
                    line = sr.ReadLine()
                    Dim TestPos As Integer
                    ' A textual comparison starting at position 4. Returns 6.
                    TestPos = InStr(1, UCase(line), "|", CompareMethod.Binary)
                    If TestPos <> 0 Then
                        Dim li As New ListViewItem(line.Substring(0, 32))
                        li.SubItems.Add(line.Substring(TestPos, line.Length - TestPos))
                        lvwbaze.Items.Add(li)
                    End If
                Loop Until line Is Nothing
                sr.Close()
            End Using
        Catch E As Exception
            ' Let the user know what went wrong.
            MsgBox("Unknow problem with bases-md5")
            ErrorLog("view_baze " & ErrorToString())
        End Try

    End Sub
    Public Sub view_baze2()

        Dim fname As String = Application.StartupPath & "\uzerbaseSTR.bvb"
        If File.Exists(fname) = False Then
            Exit Sub
        End If
        Try
            If ListView1.Items.Count >= 1 Then
                ListView1.Items.Clear()
            End If
            ' Create an instance of StreamReader to read from a file.
            Using sr As StreamReader = New StreamReader(fname)
                Dim line As String
                ' Read and display the lines from the file until the end 
                ' of the file is reached.
                Do
                    line = sr.ReadLine()
                    If Trim(line) <> "" Then
                        Dim li As New ListViewItem(line)
                        ListView1.Items.Add(li)
                    End If
                Loop Until line Is Nothing
                sr.Close()
            End Using
        Catch E As Exception
            ' Let the user know what went wrong.
            MsgBox("Unknow problem with bases-md5")
            ErrorLog("view_baze2 " & ErrorToString())
        End Try

    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        save_baze()
        get_kolwovirus()
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If lvwbaze.Items.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer
        For i = 0 To lvwbaze.Items.Count - 1
            lvwbaze.Items(i).Remove()
            Exit Sub
        Next
    End Sub



    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click

        If Trim(TextBox3.Text) = "" Then
            MsgBox("Please select spesial string", MsgBoxStyle.Critical)
            Exit Sub
        End If

        Dim li8 As New ListViewItem(TextBox3.Text)
        ListView1.Items.Add(li8)

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        save_baze2()
        get_kolwovirus()
        Me.Close()
    End Sub
    Public Sub save_baze2()
        On Error GoTo 10
        Dim path As String = Application.StartupPath & "\uzerbaseSTR.bvb"
        If File.Exists(path) = True Then
            ' Create a file to write to.
            File.DELETE(path)
        End If
        If ListView1.Items.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer
        For i = 0 To ListView1.Items.Count - 1
            Dim sw As StreamWriter = File.AppendText(path)
            sw.WriteLine(ListView1.Items(i).Text)
            sw.Flush()
            sw.Close()
        Next
        MsgBox("New record add in custom base", MsgBoxStyle.OkOnly + MsgBoxStyle.Information)
        Exit Sub
10:

        ErrorLog("save_baze2 " & ErrorToString())
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Me.Close()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If ListView1.Items.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer
        For i = 0 To ListView1.Items.Count - 1
            ListView1.Items(i).Remove()
            Exit Sub
        Next
    End Sub


   
End Class