Imports System.IO

Public Class detection
    Public x As IO.DriveInfo
 Public Function detected() As Boolean
  Dim x1 As Object
  For Each x1 In My.Computer.FileSystem.Drives
   If x.DriveType = IO.DriveType.Removable Then
    'frmMonitor.lblDrive.Text = "Drive " + x.Name + "        Label:" + x.VolumeLabel
    Return True
   End If
  Next
 End Function

    Public Function notDetected() As Boolean
        Try
            If x.IsReady = False Then
                Return True
            End If
        Catch ex As Exception
            ' MessageBox.Show(ex.Message)
        End Try

    End Function

End Class
