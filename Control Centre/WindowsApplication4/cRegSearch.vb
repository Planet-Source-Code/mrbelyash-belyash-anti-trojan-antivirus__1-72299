Imports Microsoft.VisualBasic
Imports System.Text

Friend Class cRegSearch
 Dim MyLibrary As New MyLibrary.MyLib
 Private Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
 Private Declare Function RegEnumKey Lib "advapi32.dll" Alias "RegEnumKeyA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByVal cbName As Integer) As Integer
 Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
 Private Declare Function RegEnumValue Lib "advapi32.dll" Alias "RegEnumValueA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpValueName As String, ByRef lpcbValueName As Integer, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Byte, ByRef lpcbData As Integer) As Integer

 Enum ROOT_KEYS
  HKEY_ALL = &H0
  HKEY_CLASSES_ROOT = &H80000000
  HKEY_CURRENT_USER = &H80000001
  HKEY_LOCAL_MACHINE = &H80000002
  HKEY_USERS = &H80000003
  HKEY_PERFORMANCE_DATA = &H80000004
  HKEY_CURRENT_CONFIG = &H80000005
  HKEY_DYN_DATA = &H80000006
 End Enum

 Enum SEARCH_FLAGS
  KEY_NAME = 0
  VALUE_NAME = 1
  VALUE_VALUE = 2
  WHOLE_STRING = 4
 End Enum

 Enum FOUND_WHERE
  FOUND_IN_KEY_NAME
  FOUND_IN_VALUE_NAME
  FOUND_IN_VALUE_VALUE
 End Enum

 Private Const STANDARD_RIGHTS_ALL As Integer = &H1F0000
 Private Const KEY_QUERY_VALUE As Integer = &H1
 Private Const KEY_SET_VALUE As Integer = &H2
 Private Const KEY_CREATE_SUB_KEY As Integer = &H4
 Private Const KEY_ENUMERATE_SUB_KEYS As Integer = &H8
 Private Const KEY_NOTIFY As Integer = &H10
 Private Const KEY_CREATE_LINK As Integer = &H20
 Private Const SYNCHRONIZE As Integer = &H100000
 Private Const KEY_ALL_ACCESS As Boolean = ((STANDARD_RIGHTS_ALL Or KEY_QUERY_VALUE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or KEY_CREATE_LINK) And (Not SYNCHRONIZE))
 Const KEY_READ As Integer = &H20019 ' ((READ_CONTROL Or KEY_QUERY_VALUE Or
 ' KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY) And (Not
 ' SYNCHRONIZE))

 Private Const ERROR_SUCCESS As Short = 0
 Private Const ERR_MORE_DATA As Short = 234
 Private Const ERROR_NO_MORE_ITEMS As Short = 259

 Private Const REG_NONE As Short = 0
 Private Const REG_SZ As Short = 1
 Private Const REG_EXPAND_SZ As Short = 2
 Private Const REG_BINARY As Short = 3
 Private Const REG_DWORD As Short = 4
 Private Const REG_DWORD_LITTLE_ENDIAN As Short = 4
 Private Const REG_DWORD_BIG_ENDIAN As Short = 5
 Private Const REG_LINK As Short = 6
 Private Const REG_MULTI_SZ As Short = 7
 Private Const REG_RESOURCE_LIST As Short = 8
 Private Const REG_FULL_RESOURCE_DESCRIPTOR As Short = 9
 Private Const REG_RESOURCE_REQUIREMENTS_LIST As Short = 10

 Private Const MAX_KEY_SIZE As Short = 260
 Private Const MAX_VALUE_SIZE As Short = 4096

 'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
 'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
 Private Declare Sub CopyMem Lib "kernel32" Alias "RtlMoveMemory" (ByRef pDest As Integer, ByRef pSource As Byte, ByVal ByteLen As Integer)

 Public Event SearchFound(ByVal sRootKey As String, ByVal sKey As String, ByVal sValue As Object, ByVal lFound As FOUND_WHERE)
 Public Event SearchFinished(ByVal lReason As Integer)
 Public Event SearchKeyChanged(ByVal sFullKeyName As String)

 Private mvarRootKey As ROOT_KEYS
 Private mvarSearchFlags As SEARCH_FLAGS
 Private mvarSearchString As String
 Private mvarSubKey As String

 Dim lStopSearch As Integer

 Public WriteOnly Property SubKey() As String
  Set(ByVal Value As String)
   mvarSubKey = Value
  End Set
 End Property

 Public WriteOnly Property SearchString() As String
  Set(ByVal Value As String)
   mvarSearchString = Value
  End Set
 End Property

 Public WriteOnly Property SearchFlags() As SEARCH_FLAGS
  Set(ByVal Value As SEARCH_FLAGS)
   mvarSearchFlags = Value
  End Set
 End Property

 Public WriteOnly Property RootKey() As ROOT_KEYS
  Set(ByVal Value As ROOT_KEYS)
   mvarRootKey = Value
  End Set
 End Property

 Public Sub DoSearch()
  If mvarRootKey <> ROOT_KEYS.HKEY_ALL Then
   If (mvarSearchFlags And SEARCH_FLAGS.VALUE_NAME) = SEARCH_FLAGS.VALUE_NAME Or (mvarSearchFlags And SEARCH_FLAGS.VALUE_VALUE) = SEARCH_FLAGS.VALUE_VALUE Then
    Call EnumRegValues(mvarRootKey, mvarSubKey)
   End If
   Call EnumRegKeys(mvarRootKey, mvarSubKey)
  Else
   Call EnumRegKeys(ROOT_KEYS.HKEY_CLASSES_ROOT, mvarSubKey)
   If lStopSearch Then GoTo Search_Terminated
   Call EnumRegKeys(ROOT_KEYS.HKEY_CURRENT_USER, mvarSubKey)
   If lStopSearch Then GoTo Search_Terminated
   Call EnumRegKeys(ROOT_KEYS.HKEY_LOCAL_MACHINE, mvarSubKey)
   If lStopSearch Then GoTo Search_Terminated
   Call EnumRegKeys(ROOT_KEYS.HKEY_USERS, mvarSubKey)
   If lStopSearch Then GoTo Search_Terminated
   Call EnumRegKeys(ROOT_KEYS.HKEY_PERFORMANCE_DATA, mvarSubKey)
   If lStopSearch Then GoTo Search_Terminated
   Call EnumRegKeys(ROOT_KEYS.HKEY_CURRENT_CONFIG, mvarSubKey)
   If lStopSearch Then GoTo Search_Terminated
   Call EnumRegKeys(ROOT_KEYS.HKEY_DYN_DATA, mvarSubKey)
  End If
Search_Terminated:
  RaiseEvent SearchFinished(lStopSearch)
  lStopSearch = 0
 End Sub

 Public Sub StopSearch()
  lStopSearch = 1
 End Sub

 Private Sub EnumRegKeys(ByVal lKeyRoot As Integer, ByVal sSubKey As String)
  Dim curidx As Integer
  Dim KeyName As String
  Dim hKey As Integer
  Dim sTemp As String
  If lStopSearch Then Exit Sub
  On Error GoTo ErrEnum
  If RegOpenKeyEx(lKeyRoot, sSubKey, 0, KEY_READ, hKey) Then Exit Sub
  Do
   System.Windows.Forms.Application.DoEvents()
   KeyName = Space(MAX_KEY_SIZE)
   If RegEnumKey(hKey, curidx, KeyName, MAX_KEY_SIZE) <> ERROR_SUCCESS Then Exit Do
   curidx = curidx + 1
   KeyName = TrimNull(KeyName)
   If sSubKey <> "" Then
    sTemp = sSubKey & "\" & KeyName
   Else
    sTemp = KeyName
   End If
  

   '****************************************************
   'This event is used for showing currently viewing key.
   'Usually you don't need this.
   'To increase performance, remove this event
   If lStopSearch = 0 Then RaiseEvent SearchKeyChanged(RootKeyName(lKeyRoot) & "\" & sTemp)
   '****************************************************
   If (mvarSearchFlags And SEARCH_FLAGS.KEY_NAME) = SEARCH_FLAGS.KEY_NAME Then
    If CheckMatching(KeyName) Then
     RaiseEvent SearchFound(RootKeyName(lKeyRoot), sTemp, "*", FOUND_WHERE.FOUND_IN_KEY_NAME)
     'MsgBox(KeyName)
    End If
   End If
   If (mvarSearchFlags And SEARCH_FLAGS.VALUE_NAME) = SEARCH_FLAGS.VALUE_NAME Or (mvarSearchFlags And SEARCH_FLAGS.VALUE_VALUE) = SEARCH_FLAGS.VALUE_VALUE Then
    Call EnumRegValues(lKeyRoot, sTemp)
   End If
   Call EnumRegKeys(lKeyRoot, sTemp)
  Loop
ErrEnum:
  If Err.Number Then lStopSearch = Err.Number
  RegCloseKey(hKey)
 End Sub
 Function enc(ByVal unicodeString As String) As String
  Dim utf7 As New UTF7Encoding()

  ' Encode the string.
  Dim encodedBytes As Byte() = utf7.GetBytes(unicodeString)

  Dim b As Byte
  For Each b In encodedBytes
   'Console.Write("[{0}]", b)
  Next b


  ' Decode bytes back to string.
  ' Notice Pi and Sigma characters are still present.
  Dim decodedString As String = utf7.GetString(encodedBytes)

  Return (decodedString)


 End Function
 Private Sub EnumRegValues(ByVal lKeyRoot As Integer, ByVal sSubKey As String)
  Dim curidx As Integer
  Dim ValueName, ValueValue As String
  Dim hKey As Integer
  Dim lType As Integer
  Dim arrData() As Byte
  Dim cbDataSize As Integer
  If lStopSearch Then Exit Sub
  On Error GoTo ErrEnum
  If RegOpenKeyEx(lKeyRoot, sSubKey, 0, KEY_READ, hKey) Then Exit Sub
  Do
   ValueName = New String(Chr(0), MAX_KEY_SIZE)
   cbDataSize = MAX_VALUE_SIZE
   ReDim arrData(cbDataSize - 1)
   If RegEnumValue(hKey, curidx, ValueName, MAX_KEY_SIZE, 0, lType, arrData(0), cbDataSize) <> ERROR_SUCCESS Then Exit Do
   If cbDataSize < 1 Then cbDataSize = 1
   ReDim Preserve arrData(cbDataSize - 1)
   ValueName = TrimNull(ValueName)
   If (mvarSearchFlags And SEARCH_FLAGS.VALUE_NAME) = SEARCH_FLAGS.VALUE_NAME Then
    If CheckMatching(ValueName) Then RaiseEvent SearchFound(RootKeyName(lKeyRoot), sSubKey & "\" & ValueName, GetRegData(lType, arrData), FOUND_WHERE.FOUND_IN_VALUE_NAME)
   End If
   'MyLibrary.FormFunction.TrimNull()
   ' MyLibrary.FormFunction.GetRegData()
   If (mvarSearchFlags And SEARCH_FLAGS.VALUE_VALUE) = SEARCH_FLAGS.VALUE_VALUE Then
    ValueValue = MyLibrary.FormFunction.TrimNull(GetRegData(lType, arrData))
    Form1.ireG = Form1.ireG + 1
    Form1.Label80.Text = Form1.ireG
    If CheckMatching(ValueValue) Then
     RaiseEvent SearchFound(RootKeyName(lKeyRoot), sSubKey & "\" & ValueName, ValueValue, FOUND_WHERE.FOUND_IN_VALUE_VALUE)
     ' MsgBox(ValueValue)
    End If
   End If
   curidx = curidx + 1
  Loop
ErrEnum:
  If Err.Number Then lStopSearch = Err.Number
  RegCloseKey(hKey)
 End Sub

 Public Function TrimNull(ByRef startstr As String) As String
  Dim pos As Short
  pos = InStr(startstr, Chr(0))
  If pos Then
   TrimNull = Left(startstr, pos - 1)
   Exit Function
  End If
  TrimNull = startstr
 End Function

 Private Function CheckMatching(ByVal sCheck As String) As Boolean
  If (mvarSearchFlags And SEARCH_FLAGS.WHOLE_STRING) = SEARCH_FLAGS.WHOLE_STRING Then
   CheckMatching = (UCase(sCheck) = UCase(mvarSearchString))
  Else
   CheckMatching = InStr(1, sCheck, mvarSearchString, CompareMethod.Text)
  End If
 End Function

 Private Function GetRegData(ByVal lType As Integer, ByRef abData() As Byte) As String
  Dim lData, i As Integer
  Dim sTemp As String
  sTemp = ""
  Select Case lType
   Case REG_SZ, REG_MULTI_SZ
    'UPGRADE_ISSUE: Constant vbUnicode was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'

    ' GetRegData = TrimNull(System.Text.UnicodeEncoding.Unicode.GetString(abData))
    'GetRegData = TrimNull(StrConv(abData, vbUnicode))
    'GetRegData = TrimNull(CType(System.Text.UnicodeEncoding.Unicode.GetString(abData), String))
    Dim utf7 As New UTF7Encoding()
    Dim encodedBytes As Byte() = abData
    Dim b As Byte
    For Each b In encodedBytes
     'Console.Write("[{0}]", b)
    Next b
    Dim decodedString As String = utf7.GetString(encodedBytes)
    GetRegData = TrimNull(decodedString)
   Case REG_DWORD
    CopyMem(lData, abData(0), 4)
    GetRegData = "0x" & Format(Hex(lData), "00000000") & "(" & lData & ")"
   Case REG_BINARY
    For i = 0 To UBound(abData)
     sTemp = sTemp & Right("00" & Hex(abData(i)), 2) & " "
    Next i
    GetRegData = Left(sTemp, Len(sTemp) - 1)
   Case Else
    GetRegData = "Temporary unsupported"
  End Select
 End Function

 Private Function RootKeyName(ByRef lKey As Integer) As String
  Select Case lKey
   Case ROOT_KEYS.HKEY_CLASSES_ROOT : RootKeyName = "HKEY_CLASSES_ROOT"
   Case ROOT_KEYS.HKEY_CURRENT_USER : RootKeyName = "HKEY_CURRENT_USER"
   Case ROOT_KEYS.HKEY_LOCAL_MACHINE : RootKeyName = "HKEY_LOCAL_MACHINE"
   Case ROOT_KEYS.HKEY_USERS : RootKeyName = "HKEY_USERS"
   Case ROOT_KEYS.HKEY_PERFORMANCE_DATA : RootKeyName = "HKEY_PERFORMANCE_DATA"
   Case ROOT_KEYS.HKEY_CURRENT_CONFIG : RootKeyName = "HKEY_CURRENT_CONFIG"
   Case ROOT_KEYS.HKEY_DYN_DATA : RootKeyName = "HKEY_DYN_DATA"
   Case Else
    Return "None"
  End Select
 End Function
 'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
 Private Sub Class_Initialize_Renamed()
  mvarRootKey = ROOT_KEYS.HKEY_ALL
  mvarSubKey = ""
  mvarSearchString = ""
 End Sub
 Public Sub New()
  MyBase.New()
  Class_Initialize_Renamed()
 End Sub

 'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
 Private Sub Class_Terminate_Renamed()
  lStopSearch = 1
 End Sub
 Protected Overrides Sub Finalize()
  Class_Terminate_Renamed()
  MyBase.Finalize()
 End Sub
End Class