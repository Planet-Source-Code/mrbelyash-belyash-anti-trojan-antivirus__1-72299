Namespace My

 ' The following events are available for MyApplication:
 ' 
 ' Startup: Raised when the application starts, before the startup form is created.
 ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
 ' UnhandledException: Raised if the application encounters an unhandled exception.
 ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
 ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
 Partial Friend Class MyApplication


  Private Sub MyApplication_StartupNextInstance(ByVal sender As Object, _
ByVal e As Microsoft.VisualBasic.ApplicationServices. _
 StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
   Try
    If Form1.my_copy = True Then
     Form1.UNreg_blocker()
     Process.Start(Form1.Label1.Text)
               Threading.Thread.Sleep(Form1.MYTime)
     Form1.reg_blocker()
     Form1.my_copy = False
    End If
    End
   Catch ex As Exception
    Form1.ErrorLog("MyApplication_StartupNextInstance" & ErrorToString())
   End Try
  End Sub
 End Class

End Namespace

