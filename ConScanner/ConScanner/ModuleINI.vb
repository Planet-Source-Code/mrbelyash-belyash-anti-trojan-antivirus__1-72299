Module ModuleINI
    Declare Function GetPrivateProfileString Lib "kernel32" Alias _
     "GetPrivateProfileStringA" (ByVal lpApplicationName _
     As String, ByVal lpKeyName As String, ByVal lpDefault _
     As String, ByVal lpReturnedString As String, ByVal _
     nSize As Integer, ByVal lpFileName As String) As Integer
    Declare Function WritePrivateProfileString Lib "kernel32" Alias _
    "WritePrivateProfileStringA" (ByVal lpApplicationName _
    As String, ByVal lpKeyName As String, ByVal lpString As String, _
    ByVal lpFileName As String) As Integer
 Public sINIFile As String = MyPath & "\SETTINGS.INI"
    Public Function sGetINI(ByVal sINIFile As String, ByVal sSection As String, ByVal sKey _
    As String, ByVal sDefault As String) As String
        'получить настройки из инишника
        On Error Resume Next
        Dim sTemp As String = Space(255)
        Dim nLength As Integer


        nLength = GetPrivateProfileString(sSection, sKey, sDefault, sTemp, _
        255, sINIFile)
        Return sTemp.Substring(0, nLength)

    End Function
    Public Sub writeINI(ByVal sINIFile As String, ByVal sSection As String, ByVal sKey _
 As String, ByVal sValue As String)
        On Error Resume Next
        'записать настройки в инишник
        'Remove CR/LF characters
        sValue = sValue.Replace(vbCr, vbNullChar)
        sValue = sValue.Replace(vbLf, vbNullChar)

        'Write information to INI file
        WritePrivateProfileString(sSection, sKey, sValue, sINIFile)

    End Sub
End Module
