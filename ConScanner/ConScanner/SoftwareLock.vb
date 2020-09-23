'I'll be using Microsoft's Cryptography Class
'for encryption purposes:
Imports System.Security.Cryptography

'All registration data is stored to windows
'Registry:
Imports Microsoft.Win32

'I need UnicodeEncoding class of
'this namespace:
Imports System.Text

Public Class SoftwareLOCK

    Dim keysize As New cript.clsAESV2.KeySize
    Protected _username As String
    Protected _myMachine As String
    Protected _HashAlgorithm As HashAlgorithms
    Protected _DiskSerial As String
    Protected _isRegistered As Boolean
    Protected _CustRef As String
    Protected _SerialKey As String
    Protected _CustRefLength As Integer
    Dim MyLibrary As New MyLibrary.MyLib

    Public Enum HashAlgorithms
        MD5 = 0
        SHA1 = 1
        SHA256 = 2
        SHA384 = 3
        SHA512 = 4
    End Enum


    Public Sub New()
        MyBase.New()
        _DiskSerial = UCase$(MyLibrary.FormFunction.get_serialdisk)

        _CustRefLength = 20
    End Sub

    Property username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Property myMachine() As String
        Get
            Return _myMachine
        End Get
        Set(ByVal value As String)
            _myMachine = value
        End Set
    End Property

    'return of set HashAlgorithm for generating
    'Reference and serialkey
    Property HashAlgorithm() As HashAlgorithms
        Get
            Return _HashAlgorithm
        End Get
        Set(ByVal value As HashAlgorithms)
            _HashAlgorithm = value
        End Set
    End Property

    'returns end-user's hard disk serial number
    Property HardDiskSerial() As String
        Get
            Return _DiskSerial
        End Get
        Set(ByVal value As String)
            _DiskSerial = value
        End Set
    End Property

    'use this property to check if your software is registered
    ReadOnly Property isRegistered() As Boolean

        Get

            Try
                Dim SKey As String = sGetINI(sINIFile, "USER", "SN", "")
                If SKey = SerialKey Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As NullReferenceException
                Return False
            End Try

        End Get

    End Property

    Property ReferenceLength() As Integer
        Get
            Return _CustRefLength
        End Get
        Set(ByVal value As Integer)
            If value >= 5 And value <= 20 Then
                _CustRefLength = value
            Else
                Throw New Exception("ReferenceLength cannot be less than 6 or more than 20 characters")
            End If
        End Set
    End Property

    'use this method in your software's registration form's
    'OK button, and pass it the serial key entered by the end-user
    'as argument, returns True for a successful registration, False
    'for unsuccessful
    Function Register(ByVal strSerialKey As String) As Boolean
        On Error Resume Next
        GenerateCodes()
        ' MsgBox(_SerialKey)
        Debug.WriteLine(_SerialKey)
        If strSerialKey = _SerialKey Then
            writeINI(sINIFile, "USER", "SN", _SerialKey)
            Debug.Print(_SerialKey)
            Dim a = New cript.clsAESV2(keysize, MyLibrary.FormFunction.my_label)
            Dim tmpstr As String = a.Encrypt(Format(Now, "dd.MM") & "." & (Format(Now, "yyyy")) + 1)
            Debug.Print(Format(Now, "dd.MM") & "." & (Format(Now, "yyyy")) + 1)
            writeINI(sINIFile, "USER", "LicExpirid", tmpstr)
            Return True
        Else
            Return False
        End If
    End Function

    'returns customer reference based on username,myMachine,harddisk serial
    Overridable Property Reference() As String
        Get
            GenerateCodes()
            Return _CustRef
        End Get
        Set(ByVal value As String)
            If value.Length >= 5 And value.Length <= 20 Then
                _CustRef = value
            Else
                Throw New Exception("Reference cannot be less than 6 or more than 20 characters")
            End If
        End Set
    End Property

    'returns serial key based on Reference
    Overridable ReadOnly Property SerialKey() As String
        Get
            GenerateCodes()
            Return _SerialKey
        End Get
    End Property

    Public Overridable Function CancelRegistration() As Boolean
        'you can use this function for testing purposes
        writeINI(sINIFile, "USER", "SN", "")

    End Function

    Protected Sub GenerateCodes()
        'generate Reference based on 
        'username,myMachine,and harddisk serial
        'generate serial key based on Reference
        Dim Hash As HashAlgorithm
        Dim HashBytes() As Byte
        Dim UNIEncoding As New UnicodeEncoding
        Dim temp As String

        'load the user selected hash-algorithm
        Select Case _HashAlgorithm
            Case HashAlgorithms.MD5
                Hash = New MD5CryptoServiceProvider
            Case HashAlgorithms.SHA1
                Hash = New SHA1CryptoServiceProvider
            Case HashAlgorithms.SHA256
                Hash = New SHA256Managed
            Case HashAlgorithms.SHA384
                Hash = New SHA384Managed
            Case HashAlgorithms.SHA512
                Hash = New SHA512Managed
            Case Else 'default hash algorithm
                Hash = New MD5CryptoServiceProvider
        End Select

        'generate hash using username, myMachine, and HD serial no.
        temp = _username & _myMachine & _DiskSerial
        'temp = _CustRef & _AppName & _Password
        HashBytes = Hash.ComputeHash(UNIEncoding.GetBytes(temp))

        _CustRef = Convert.ToBase64String(HashBytes)
        'if longer than 20 chars, trim it to 20 chars
        If _CustRef.Trim.Length > _CustRefLength Then _CustRef = Left(_CustRef, _CustRefLength)

        'convert it to upper case
        _CustRef = UCase(_CustRef)

        'clear the hash array
        Array.Clear(HashBytes, 0, HashBytes.Length)

        'use the above generated Reference to generate a 20 characters 
        'serial key for the end user
        temp = _CustRef & _username & _myMachine
        HashBytes = Hash.ComputeHash(UNIEncoding.GetBytes(temp))

        _SerialKey = Convert.ToBase64String(HashBytes)
        If _SerialKey.Trim.Length > 20 Then _SerialKey = Left(_SerialKey, 20)
        _SerialKey = UCase(_SerialKey)

    End Sub


End Class
