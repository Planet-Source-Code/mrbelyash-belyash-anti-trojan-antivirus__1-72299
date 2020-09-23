Option Explicit On

Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Public Class GlassBox

#Region "Message Variables ..."
    Private Shared ObjWinMessage As GlassBox

    Private Shared WithEvents CmdOk As Button
    Private Shared WithEvents CmdYes As Button
    Private Shared WithEvents CmdNo As Button
    Private Shared WithEvents CmdCancel As Button
    Private Shared WithEvents CmdAbort As Button
    Private Shared WithEvents CmdRetry As Button
    Private Shared WithEvents CmdIgnore As Button

    Private Shared WithEvents WinMsg As Form
    Private Shared WithEvents LblHeader As Label
    Private Shared PicIcon As PictureBox
    Private Shared WithEvents WinMessage As Label
    Private Shared PicIcon22 As PictureBox
    Private Shared MaxWidth As Integer = SystemInformation.WorkingArea.Width * 0.8
    Private Shared MaxHeight As Integer = SystemInformation.WorkingArea.Height

    Private Shared FormWidth As Integer = 0
    Private Shared FormHeight As Integer = 0

    Private Shared HeaderWidth As Integer = 0

    Private Shared MsgWidth As Integer = 0
    Private Shared MsgHeight As Integer = 0

    Private Shared WinReturn As Integer = 0

    Private Shared WinFnt As New Font("Consolas", 9, FontStyle.Bold)
    Private Shared WinLocation As Point

    Private Shared FormSize As Size
    Private Shared MessageSize As Size

    Private Shared WinButtons As MessageBoxButtons
    Private Shared WinDefault As MessageBoxDefaultButton

    Private Shared WinResult As DialogResult
    Private Shared WinMake As DialogResult

    Private Shared MessageText As String = ""
    Private Shared HeaderText As String = ""

#End Region
#Region "Class Constructor"
    Private Sub New()

        WinMsg = New Form()

    End Sub
#End Region

    ''' <summary>
    ''' Win Message Box :- Display Body Message, Header Message, Message Icon, Message Buttons With Default Focus
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ShowMessage(ByVal WinText As String, Optional ByVal WinHeader As String = "", _
                                          Optional ByVal WinIcon As MessageBoxIcon = MessageBoxIcon.None, _
                                          Optional ByVal WinButtons As MessageBoxButtons = MessageBoxButtons.OK, _
                                          Optional ByVal WinDefault As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1) As DialogResult


        WinResult = MakeMessage(WinText, WinHeader, WinIcon, WinButtons, WinDefault)
        Return WinResult

    End Function
    Private Shared Function MakeMessage(ByVal WinText As String, Optional ByVal WinHeader As String = "", _
                                        Optional ByVal WinIcon As MessageBoxIcon = MessageBoxIcon.None, Optional ByVal WinButtons As MessageBoxButtons = MessageBoxButtons.OK, _
                                        Optional ByVal WinDefault As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1) As DialogResult

        ObjWinMessage = Nothing
        ObjWinMessage = New GlassBox()

        MessageText = "" : MessageText = WinText
        HeaderText = "" : HeaderText = WinHeader

        FormWidth = 0 : FormHeight = 0
        HeaderWidth = 0
        MsgWidth = 0 : MsgHeight = 0
        WinReturn = 0

        REM Check Message And Header Text Length When Equal To Zero
        If MessageText.Trim().Length = 0 And HeaderText.Trim().Length = 0 Then
            FormSize = New Size(305, 135)
            FormWidth = 305 : FormHeight = 135
            WinMsg.Size = New Size(FormSize.Width, FormSize.Height)
            GoTo Mess
        End If

        HeaderWidth = StringSize(HeaderText.Trim(), MaxWidth, WinFnt).Width
        MessageSize = StringSize(MessageText.Trim(), MaxWidth, WinFnt)
        MsgWidth = MessageSize.Width : MsgHeight = MessageSize.Height

        If HeaderText.Trim().Length > 0 And MessageText.Trim().Length = 0 Then
            HeaderWidth = HeaderWidth + 80
            WinReturn = Math.Max(HeaderWidth, 305)
            FormSize = New Size(WinReturn, 135)
            FormWidth = FormSize.Width : FormHeight = FormSize.Height
            GoTo Mess
        End If

        HeaderWidth = HeaderWidth + 80
        FormWidth = MsgWidth + 60
        FormHeight = MsgHeight + 120

        If HeaderText.Trim().Length = 0 And MessageText.Trim().Length > 0 Then
            FormWidth = Math.Max(FormWidth, 305)
            FormHeight = Math.Max(FormHeight, 135)
            FormSize = New Size(FormWidth, FormHeight)
            FormWidth = FormSize.Width : FormHeight = FormSize.Height
            GoTo Mess
        End If

        If HeaderText.Trim().Length > 0 And MessageText.Trim().Length > 0 Then
            WinReturn = Math.Max(HeaderWidth, FormWidth)
            FormWidth = Math.Max(WinReturn, 305)
            FormHeight = Math.Max(FormHeight, 135)
            FormSize = New Size(FormWidth, FormHeight)
            FormWidth = FormSize.Width : FormHeight = FormSize.Height
            GoTo Mess
        End If

Mess:
        Call CreateBaseScreen()

        Call CreateHeader()
        Call CreateMessageIcon()
        Call CreateMeaasge()

        Call AddToBaseScreen()

        WinMessage.Text = WinText
        LblHeader.Text = WinHeader

        Select Case WinIcon
            Case MessageBoxIcon.Asterisk
                PicIcon.Image = Drawing.SystemIcons.Asterisk.ToBitmap()
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
            Case MessageBoxIcon.Error
                PicIcon.Image = Drawing.SystemIcons.Error.ToBitmap()
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
            Case MessageBoxIcon.Exclamation
                PicIcon.Image = Drawing.SystemIcons.Exclamation.ToBitmap()
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
            Case MessageBoxIcon.Hand
                PicIcon.Image = Drawing.SystemIcons.Hand.ToBitmap()
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
            Case MessageBoxIcon.Information
                PicIcon.Image = Drawing.SystemIcons.Information.ToBitmap()
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
            Case MessageBoxIcon.None
                PicIcon.Image = Nothing
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
            Case MessageBoxIcon.Question
                PicIcon.Image = Drawing.SystemIcons.Question.ToBitmap()
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
            Case MessageBoxIcon.Stop
                PicIcon.Image = Drawing.SystemIcons.Error.ToBitmap()
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
            Case MessageBoxIcon.Warning
                PicIcon.Image = Drawing.SystemIcons.Warning.ToBitmap()
                PicIcon22.Image = Form1.ImageList1.Images.Item(15)
        End Select

        Call CreateMessageButtons(WinButtons)

        Select Case WinButtons

            Case MessageBoxButtons.AbortRetryIgnore
                Select Case WinDefault
                    Case MessageBoxDefaultButton.Button1
                        CmdAbort.Select()
                        CmdAbort.Focus()
                    Case MessageBoxDefaultButton.Button2
                        CmdRetry.Select()
                        CmdRetry.Focus()
                    Case MessageBoxDefaultButton.Button3
                        CmdIgnore.Select()
                        CmdIgnore.Focus()
                End Select

            Case MessageBoxButtons.OK
                Select Case WinDefault
                    Case MessageBoxDefaultButton.Button1
                        CmdOk.Select()
                        CmdOk.Focus()
                    Case MessageBoxDefaultButton.Button2
                        CmdOk.Select()
                        CmdOk.Focus()
                    Case MessageBoxDefaultButton.Button3
                        CmdOk.Select()
                        CmdOk.Focus()
                End Select

            Case MessageBoxButtons.OKCancel
                Select Case WinDefault
                    Case MessageBoxDefaultButton.Button1
                        CmdOk.Select()
                        CmdOk.Focus()
                    Case MessageBoxDefaultButton.Button2
                        CmdCancel.Select()
                        CmdCancel.Focus()
                    Case MessageBoxDefaultButton.Button3
                        CmdCancel.Select()
                        CmdCancel.Focus()
                End Select

            Case MessageBoxButtons.RetryCancel
                Select Case WinDefault
                    Case MessageBoxDefaultButton.Button1
                        CmdRetry.Select()
                        CmdRetry.Focus()
                    Case MessageBoxDefaultButton.Button2
                        CmdCancel.Select()
                        CmdCancel.Focus()
                    Case MessageBoxDefaultButton.Button3
                        CmdCancel.Select()
                        CmdCancel.Focus()
                End Select

            Case MessageBoxButtons.YesNo
                Select Case WinDefault
                    Case MessageBoxDefaultButton.Button1
                        CmdYes.Select()
                        CmdYes.Focus()
                    Case MessageBoxDefaultButton.Button2
                        CmdNo.Select()
                        CmdNo.Focus()
                    Case MessageBoxDefaultButton.Button3
                        CmdNo.Select()
                        CmdNo.Focus()
                End Select

            Case MessageBoxButtons.YesNoCancel
                Select Case WinDefault
                    Case MessageBoxDefaultButton.Button1
                        CmdYes.Select()
                        CmdYes.Focus()
                    Case MessageBoxDefaultButton.Button2
                        CmdNo.Select()
                        CmdNo.Focus()
                    Case MessageBoxDefaultButton.Button3
                        CmdCancel.Select()
                        CmdCancel.Focus()
                End Select
        End Select
        WinMsg.StartPosition = FormStartPosition.CenterScreen
        WinMsg.TopMost = True
        WinMsg.ShowDialog()
        Return WinMake

    End Function
    Private Shared Sub CreateBaseScreen()

        With WinMsg
            .Text = "   "
            .Size = New Size(FormWidth, FormHeight)
            .StartPosition = FormStartPosition.CenterParent
            .FormBorderStyle = FormBorderStyle.None
            .ShowInTaskbar = False
            .ShowIcon = False
            '.Icon = Form1.NotifyIcon1.Icon
            .Opacity = 0.85
            .Font = WinFnt
        End With

    End Sub
    Private Shared Sub CreateHeader()

        LblHeader = New Label()

        With LblHeader
            .Left = 50
            .Text = "   "
            .AutoSize = False
            .Dock = DockStyle.Top
            .BackColor = Color.Transparent
            .TextAlign = ContentAlignment.MiddleLeft
            .Height = 24
            .Font = WinFnt
            .SendToBack()
            .Visible = True
            .Image = Form1.ImageList1.Images.Item(15)
            .ImageAlign = ContentAlignment.MiddleLeft
        End With

    End Sub
    Private Shared Sub CreateMessageIcon()

        PicIcon = New PictureBox()

        With PicIcon
            .Size = New Size(35, 35)
            .Location = New Point(8, 32)
            .BackColor = Color.Transparent
            .BorderStyle = BorderStyle.None
            .SendToBack()
            .Visible = True
        End With
        PicIcon22 = New PictureBox()

        With PicIcon22
            .Size = New Size(10, 10)
            .Location = New Point(0, 0)
            .BackColor = Color.Transparent
            .BorderStyle = BorderStyle.None
            .Image = Form1.ImageList1.Images.Item(15)
            .SizeMode = PictureBoxSizeMode.StretchImage
            .SendToBack()
            .Visible = True
        End With
    End Sub
    Private Shared Sub CreateMeaasge()

        WinMessage = New Label()

        With WinMessage
            .Text = "   "
            .Size = New Size(MsgWidth, MsgHeight)
            .Location = New Point(48, 32)
            .AutoSize = False
            .Font = WinFnt
            .TextAlign = ContentAlignment.TopLeft
            .BackColor = Color.Transparent
            .SendToBack()
            .Visible = True
        End With

    End Sub
    Private Shared Sub AddToBaseScreen()

        With WinMsg
            .Controls.Add(LblHeader)
            .Controls.Add(PicIcon)
            .Controls.Add(PicIcon22)
            .Controls.Add(WinMessage)
            .Refresh()
        End With

    End Sub
    Private Shared Sub CreateMessageButtons(ByVal WinButtons As MessageBoxButtons)

        Select Case WinButtons
            Case MessageBoxButtons.AbortRetryIgnore
                If FormWidth <= 305 And FormHeight <= 135 Then
                    CmdAbort = New Button()
                    Call ButtonProperties(CmdAbort, "Abort", New Size(56, 24), New Point(FormWidth - (90 * 2), FormHeight - 65))
                    CmdRetry = New Button()
                    Call ButtonProperties(CmdRetry, "Retry", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                    CmdIgnore = New Button()
                    Call ButtonProperties(CmdIgnore, "Ignore", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                    Exit Select
                Else
                    If FormWidth > 305 And FormHeight <= 135 Then
                        CmdAbort = New Button()
                        Call ButtonProperties(CmdAbort, "Abort", New Size(56, 24), New Point(FormWidth - (90 * 2), FormHeight - 65))
                        CmdRetry = New Button()
                        Call ButtonProperties(CmdRetry, "Retry", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdIgnore = New Button()
                        Call ButtonProperties(CmdIgnore, "Ignore", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                    If FormWidth >= 305 And FormHeight >= 135 Then
                        CmdAbort = New Button()
                        Call ButtonProperties(CmdAbort, "Abort", New Size(56, 24), New Point(FormWidth - (90 * 2), FormHeight - 65))
                        CmdRetry = New Button()
                        Call ButtonProperties(CmdRetry, "Retry", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdIgnore = New Button()
                        Call ButtonProperties(CmdIgnore, "Ignore", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                End If
            Case MessageBoxButtons.OK
                If FormWidth <= 305 And FormHeight <= 135 Then
                    CmdOk = New Button()
                    Call ButtonProperties(CmdOk, "Ok", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                    Exit Select
                Else
                    If FormWidth >= 305 And FormHeight <= 135 Then
                        CmdOk = New Button()
                        Call ButtonProperties(CmdOk, "Ok", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                    If FormWidth >= 305 And FormHeight >= 135 Then
                        CmdOk = New Button()
                        Call ButtonProperties(CmdOk, "Ok", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                End If
            Case MessageBoxButtons.OKCancel
                If FormWidth <= 305 And FormHeight <= 135 Then
                    CmdOk = New Button()
                    Call ButtonProperties(CmdOk, "Ok", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                    CmdCancel = New Button()
                    Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                    Exit Select
                Else
                    If FormWidth > 305 And FormHeight <= 135 Then
                        CmdOk = New Button()
                        Call ButtonProperties(CmdOk, "Ok", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdCancel = New Button()
                        Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                    If FormWidth >= 305 And FormHeight >= 135 Then
                        CmdOk = New Button()
                        Call ButtonProperties(CmdOk, "Ok", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdCancel = New Button()
                        Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - FormHeight - 65))
                        Exit Select
                    End If
                End If
            Case MessageBoxButtons.RetryCancel
                If FormWidth <= 305 And FormHeight <= 135 Then
                    CmdRetry = New Button()
                    Call ButtonProperties(CmdRetry, "Retry", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                    CmdCancel = New Button()
                    Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                    Exit Select
                Else
                    If FormWidth >= 305 And FormHeight <= 135 Then
                        CmdRetry = New Button()
                        Call ButtonProperties(CmdRetry, "Retry", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdCancel = New Button()
                        Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                    If FormWidth >= 305 And FormHeight >= 135 Then
                        CmdRetry = New Button()
                        Call ButtonProperties(CmdRetry, "Retry", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdCancel = New Button()
                        Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                End If
            Case MessageBoxButtons.YesNo
                If FormWidth <= 305 And FormHeight <= 135 Then
                    CmdYes = New Button()
                    Call ButtonProperties(CmdYes, "Yes", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                    CmdNo = New Button()
                    Call ButtonProperties(CmdNo, "No", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                    Exit Select
                Else
                    If FormWidth >= 305 And FormHeight <= 135 Then
                        CmdYes = New Button()
                        Call ButtonProperties(CmdYes, "Yes", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdNo = New Button()
                        Call ButtonProperties(CmdNo, "No", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                    If FormWidth >= 225 And FormHeight >= 135 Then
                        CmdYes = New Button()
                        Call ButtonProperties(CmdYes, "Yes", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdNo = New Button()
                        Call ButtonProperties(CmdNo, "No", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                End If
            Case MessageBoxButtons.YesNoCancel
                If FormWidth <= 305 And FormHeight <= 135 Then
                    CmdYes = New Button()
                    Call ButtonProperties(CmdYes, "Yes", New Size(56, 24), New Point(FormWidth - (90 * 2), FormHeight - 65))
                    CmdNo = New Button()
                    Call ButtonProperties(CmdNo, "No", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                    CmdCancel = New Button()
                    Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                    Exit Select
                Else
                    If FormWidth >= 305 And FormHeight <= 135 Then
                        CmdYes = New Button()
                        Call ButtonProperties(CmdYes, "Yes", New Size(56, 24), New Point(FormWidth - (90 * 2), FormHeight - 65))
                        CmdNo = New Button()
                        Call ButtonProperties(CmdNo, "No", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdCancel = New Button()
                        Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                    If FormWidth >= 305 And FormHeight >= 135 Then
                        CmdYes = New Button()
                        Call ButtonProperties(CmdYes, "Yes", New Size(56, 24), New Point(FormWidth - (90 * 2), FormHeight - 65))
                        CmdNo = New Button()
                        Call ButtonProperties(CmdNo, "No", New Size(56, 24), New Point(FormWidth - (62.5 * 2), FormHeight - 65))
                        CmdCancel = New Button()
                        Call ButtonProperties(CmdCancel, "Cancel", New Size(56, 24), New Point(FormWidth - 70, FormHeight - 65))
                        Exit Select
                    End If
                End If
        End Select

    End Sub
    Private Shared Sub ButtonProperties(ByVal Btn As Button, ByVal Txt As String, ByVal Sz As Size, ByVal Lc As Point)

        With Btn
            .BringToFront()
            .Size = Sz
            .Text = Txt
            .BackColor = Color.Transparent
            .FlatAppearance.BorderSize = 0
            .FlatStyle = FlatStyle.Standard
            .Location = Lc
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New Font("FixedSys", 10, FontStyle.Regular)
            .Visible = True
        End With

        WinMsg.Controls.Add(Btn)

    End Sub
    Private Shared Function StringSize(ByVal WinMsgText As String, _
                                       ByVal WinWdth As Integer, _
                                       ByVal WinFnt As Font) As Size

        Dim GRA As Graphics = WinMsg.CreateGraphics()
        Dim SZF As SizeF = GRA.MeasureString(WinMsgText, WinFnt, WinWdth)
        GRA.Dispose()

        Dim SZ As New Size(DirectCast(Convert.ToInt16(SZF.Width + 100), Int16), _
                           DirectCast(Convert.ToInt16(SZF.Height), Int16))

        Return SZ

    End Function
    Private Shared Sub WinMsg_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles WinMsg.Paint

        Dim MGraphics As Graphics = e.Graphics
        Dim MPen As New Pen(Color.FromArgb(96, 155, 173), 1)

        Dim Area As New Rectangle(0, 0, WinMsg.Width - 1, WinMsg.Height - 1)
        Dim LGradient As New LinearGradientBrush(Area, Color.FromArgb(166, 197, 227), Color.FromArgb(245, 251, 251), LinearGradientMode.BackwardDiagonal)
        MGraphics.CompositingMode = CompositingMode.SourceOver
        MGraphics.CompositingQuality = CompositingQuality.HighQuality
        MGraphics.FillRectangle(LGradient, Area)
        MGraphics.DrawRectangle(MPen, Area)

    End Sub
    Private Shared Sub LblHeader_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LblHeader.MouseDown

        WinLocation = e.Location

    End Sub
    Private Shared Sub LblHeader_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LblHeader.MouseMove

        If String.Compare(Control.MouseButtons.ToString(), "Left") = 0 Then
            Dim MSize As New Size(WinLocation)
            MSize.Width = e.X - WinLocation.X
            MSize.Height = e.Y - WinLocation.Y
            WinMsg.Location = Point.Add(WinMsg.Location, MSize)
        End If

    End Sub
    Private Shared Sub LblHeader_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles LblHeader.Paint

        Dim MGraphics As Graphics = e.Graphics
        Dim MPen As New Pen(Color.FromArgb(96, 155, 173), 1)

        Dim Area As New Rectangle(20, 0, LblHeader.Width - 1, LblHeader.Height - 1)
        Dim LGradient As New LinearGradientBrush(Area, Color.FromArgb(166, 197, 227), Color.FromArgb(245, 251, 251), LinearGradientMode.BackwardDiagonal)
        MGraphics.CompositingMode = CompositingMode.SourceOver
        MGraphics.CompositingQuality = CompositingQuality.HighQuality
        MGraphics.FillRectangle(LGradient, Area)
        MGraphics.DrawRectangle(MPen, Area)

        Dim DrawFont As New Font("FixedSys", 10, FontStyle.Bold)
        Dim DrawBrush As New SolidBrush(Color.Blue)
        Dim DrawPoint As New PointF(2.0F, 3.0F)

        Dim DrawGradientBrush As New LinearGradientBrush(e.Graphics.ClipBounds, Color.White, _
               Color.FromArgb(122, 158, 226), LinearGradientMode.ForwardDiagonal)

        e.Graphics.CompositingMode = CompositingMode.SourceOver
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality
        e.Graphics.DrawString(HeaderText.ToString(), DrawFont, DrawBrush, DrawPoint)

    End Sub
    Private Shared Sub CmdAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdAbort.Click
        WinMsg.Visible = False
        WinMsg.Dispose() : WinMsg = Nothing
        WinMake = DialogResult.Abort

    End Sub
    Private Shared Sub CmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCancel.Click
        WinMsg.Visible = False
        WinMsg.Dispose() : WinMsg = Nothing
        WinMake = DialogResult.Cancel

    End Sub
    Private Shared Sub CmdIgnore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdIgnore.Click
        WinMsg.Visible = False
        WinMsg.Dispose() : WinMsg = Nothing
        WinMake = DialogResult.Ignore

    End Sub
    Private Shared Sub CmdNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdNo.Click
        WinMsg.Visible = False
        WinMsg.Dispose() : WinMsg = Nothing
        WinMake = DialogResult.No

    End Sub
    Private Shared Sub CmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdOk.Click
        WinMsg.Visible = False
        WinMsg.Dispose() : WinMsg = Nothing
        WinMake = DialogResult.OK

    End Sub
    Private Shared Sub CmdRetry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRetry.Click
        WinMsg.Visible = False
        WinMsg.Dispose() : WinMsg = Nothing
        WinMake = DialogResult.Retry

    End Sub
    Private Shared Sub CmdYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdYes.Click
        WinMsg.Visible = False
        WinMsg.Dispose() : WinMsg = Nothing
        WinMake = DialogResult.Yes

    End Sub

End Class
