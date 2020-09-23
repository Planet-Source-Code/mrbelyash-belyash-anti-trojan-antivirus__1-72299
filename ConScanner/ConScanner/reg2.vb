Imports System.Threading
Imports System.IO
Imports Microsoft.Win32

Public Class reg2
 Public Sub chk_2(ByVal tmpPerem As Integer)

  Dim a(4) As String
  Dim i As Integer
  a(0) = "-"
  a(1) = "\"
  a(2) = "|"
  a(3) = "/"
  Do While tmpPerem = 0
   For i = 0 To 3
    Console.CursorTop = 6
    Console.CursorLeft = 0
    Console.Write("Virus Record:" & a(i))
    Thread.Sleep(10)
   Next
  Loop
 End Sub
 
End Class
