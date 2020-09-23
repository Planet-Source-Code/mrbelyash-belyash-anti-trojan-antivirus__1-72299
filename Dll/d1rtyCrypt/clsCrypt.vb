Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Runtime.InteropServices

'*************************************************************************'
'                               d1rtyCrypt                                '
'*************************************************************************'
'                                                                         '
' c0d3r          :      d1rtyw0rm                                         '
' E-Mail         :      d1rtyw0rm@Ca.Tc                                   '
'                                                                         '
' Starting Date  :      30 December 2003                                  '
' Ended Date     :      04 Janurary 2004                                  '
'                                                                         '
' ONE YEAR OF DEVELOPPEMENT :P                                            '
'                                                                         '
' Principal Function :                                                    '
'            -String Encryption, String Decription                        '
'            -File Encryption, File Decryption                            '
'            -Folder Encryption, Folder Decryption                        '
'                                                                         '
' Current Supported Encryption Algorithm :                                '
'            -RSA                                                         '
'            -DES                                                         '
'*************************************************************************'

Public Class clsDES : Inherits ApplicationException
#Region "Private Var"
    Public mKey As String

    'Random Value Vector (Protect against encryption repetition (ex. "aloaloalo" <> "ģ!ģ!ģ!")
    Private Vector() As Byte = {&H12, &H44, &H16, &HEE, &H88, &H15, &HDD, &H41}
    Private TheKey(7) As Byte    '8-Byte Key
    Private objDES As New DESCryptoServiceProvider    'DES Encryption Object
#End Region

#Region "Propertys"
    Public Property Key() As String
        Get
            Return mKey
        End Get
        Set(ByVal strKey As String)

            strKey = "Belyash"
            ' Temporary buffer to hold the key
            Dim arrKeyBuffer(7) As Byte
            Dim AscEncod As New ASCIIEncoding
            Dim i As Integer = 0

            Try
                mKey = strKey

                'Convert key string into byte array
                AscEncod.GetBytes(strKey, i, strKey.Length, arrKeyBuffer, i)
            Catch ex As Exception
                Throw New ApplicationException("Key Conversion Error.")
            End Try



            Try
                'Hash the key
                Dim hashSha As New SHA1CryptoServiceProvider
                Dim arrHash() As Byte = hashSha.ComputeHash(arrKeyBuffer)

                'Hold hashed key in TheKey array
                For i = 0 To 7
                    TheKey(i) = arrHash(i)
                Next i
            Catch ex As Exception
                Throw New ApplicationException("Hashing Key Error.")
            End Try
        End Set
    End Property
#End Region

#Region "Methods"
    Public Function FolderEncrypt(ByVal InitialPath As String, ByVal Recursion As Boolean, ByVal DeleteSourceFile As Boolean)
        Dim x As Long

        'RECURSION VERIFICATION
        If Recursion = True Then
            'Find files in all sub-folder, recursion
            FillRecursiveArray(InitialPath)
        Else
            'Find InitialPath file only, no recursion
            FillNonRecursiveArray(InitialPath)
        End If

        'ENCRYPT FILE'
        For x = 1 To arrFiles.Length - 1 Step 1
            FileEncrypt(arrFiles(x), arrFiles(x) & ".CRP", True)
            'Application.DoEvents()
        Next

        Try
            If DeleteSourceFile = True Then
                'DELETE SOURCE FILE (Uncrypted)
                For x = 1 To arrFiles.Length - 1 Step 1
                    System.IO.File.Delete(arrFiles(x))

                    'Application.DoEvents()
                Next
            End If
        Catch
            Throw New ApplicationException("Error, can't delete uncrypted files.")
        End Try
    End Function

    Public Function FolderDecrypt(ByVal InitialPath As String, ByVal Recursion As Boolean, ByVal DeleteSourceFile As Boolean)
        Dim x As Long

        'RECURSION VERIFICATION
        If Recursion = True Then
            'Find files in all sub-folder, recursion
            FillRecursiveArray(InitialPath)
        Else
            'Find InitialPath file only, no recursion
            FillNonRecursiveArray(InitialPath)
        End If

        'DECRYPT FILE'
        For x = 1 To arrFiles.Length - 1 Step 1
            FileDecrypt(arrFiles(x), Microsoft.VisualBasic.Left(arrFiles(x), arrFiles(x).Length - 3), True)

            'Application.DoEvents()
        Next

        Try
            If DeleteSourceFile = True Then
                'DELETE CRYPTED FILE
                For x = 1 To arrFiles.Length - 1 Step 1
                    System.IO.File.Delete(arrFiles(x))

                    'Application.DoEvents()
                Next
            End If
        Catch
            Throw New ApplicationException("Error, can't delete crypted files.")
        End Try
    End Function


    Public Function FileEncrypt(ByVal inName As String, ByVal outName As String, ByVal DeleteSourceFile As Boolean)
        Dim bufPacket(4096) As Byte    'Create packet separator buffer (4096 Byte / Packet)
        Dim totalBytesWritten As Long = 8  'Written Bytes Cmptr
        Dim packageSize As Integer    'Set number of byte to be write at same time

        Try
            'Source File Stream (input, uncrypted)
            Dim fIn As New FileStream(inName, _
            FileMode.Open, FileAccess.Read)

            'File Stream to be Created (output, crypted)
            Dim fOut As New FileStream(outName, _
            FileMode.OpenOrCreate, FileAccess.Write)

            Try
                fOut.SetLength(0)

                Dim totalFileLength As Long = fIn.Length    'Set Source File Size

                'cryptostream object, DES Encryption
                'Will write encypted data into output file
                Dim crStream As New CryptoStream(fOut, _
                  objDES.CreateEncryptor(TheKey, Vector), _
                  CryptoStreamMode.Write)


                'Encrypt data into output file.'
                'Cut Source file in 4096 Packet
                While totalBytesWritten < totalFileLength
                    'Read 4096 Byte
                    packageSize = fIn.Read(bufPacket, 0, 4096)
                    'Encrypt and write data into output file
                    crStream.Write(bufPacket, 0, packageSize)
                    'Update totalBytesWritten
                    totalBytesWritten = Convert.ToInt32(totalBytesWritten + packageSize / objDES.BlockSize * objDES.BlockSize)
                End While

                'Close all file stream object
                crStream.Close()
                fIn.Close()
                fOut.Close()

                If DeleteSourceFile = True Then
                    System.IO.File.Delete(inName)
                End If


            Catch ex As Exception
                Throw New ApplicationException(ex.Message)

                fIn.Close()
                fOut.Close()
            End Try

        Catch ex As Exception
            Throw New ApplicationException("Input and/or Output File are invalid.")
        End Try
    End Function

    Public Function FileDecrypt(ByVal inName As String, ByVal outName As String, ByVal DeleteSourceFile As Boolean)
        Dim bufPacket(4096) As Byte    'Create packet separator buffer (4096 Byte / Packet)
        Dim totalBytesWritten As Long = 8  'Written Bytes Cmptr
        Dim packageSize As Integer    'Set number of byte to be write at same time

        Try
            'Input File Stream, to be decrypted (Source)
            Dim fIn As New FileStream(inName, _
            FileMode.Open, FileAccess.Read)

            'Output File Stream, decrypted 
            Dim fOut As New FileStream(outName, _
            FileMode.OpenOrCreate, FileAccess.Write)


            Try
                fOut.SetLength(0)

                Dim totalFileLength As Long = fIn.Length    'Input File Size

                'cryptostream object, DES Encryption
                'Will write decrypted data into output file
                Dim crStream As New CryptoStream(fOut, _
                  objDES.CreateDecryptor(TheKey, Vector), _
                  CryptoStreamMode.Write)


                'Decrypt data into output file.'
                'Cut Source file in 4096 Packet
                While totalBytesWritten < totalFileLength
                    packageSize = fIn.Read(bufPacket, 0, 4096)
                    crStream.Write(bufPacket, 0, packageSize)
                    totalBytesWritten = Convert.ToInt32(totalBytesWritten + packageSize / objDES.BlockSize * objDES.BlockSize)
                End While

                'Close stream object
                fIn.Close()
                fOut.Close()

                If DeleteSourceFile = True Then
                    System.IO.File.Delete(inName)
                End If

            Catch ex As Exception
                Throw New ApplicationException(ex.Message)

                fIn.Close()
                fOut.Close()
            End Try

        Catch ex As Exception
            Throw New ApplicationException("Input and/or Output File are invalid.")
        End Try
    End Function


    Public Function StringEncrypt(ByVal strSource As String) As String
        Dim memStream As New MemoryStream  'Create memory stream to hold encrypted string


        Try
            ' Convert uncrypted string into a byte array
            Dim bufSource() As Byte = Encoding.UTF8.GetBytes(strSource)

            'Crypt the byte array
            Dim crStream As New CryptoStream(memStream, objDES.CreateEncryptor(TheKey, Vector), CryptoStreamMode.Write)
            crStream.Write(bufSource, 0, bufSource.Length)
            crStream.FlushFinalBlock()

            ' Convert to Base64 String (XMLDOM)
            Return Convert.ToBase64String(memStream.ToArray())

        Catch ex As Exception
            Throw New ApplicationException("String encryption Error.")
        End Try
    End Function

    Public Function StringDecrypt(ByVal strSource As String) As String
        Dim memStream As New MemoryStream  'Create memory stream to hold decypted string
        Dim objDecode As System.Text.Encoding = System.Text.Encoding.UTF8 ' Memory Decode Object

        Try
            ' Uncode strSource
            Dim bufSource() As Byte = Convert.FromBase64String(strSource)

            'Uncrypt array byte into memory stream'
            Dim crStream As New CryptoStream(memStream, objDES.CreateDecryptor(TheKey, Vector), CryptoStreamMode.Write)
            crStream.Write(bufSource, 0, bufSource.Length)
            crStream.FlushFinalBlock()

            'Uncode memory stream to string'
            Return objDecode.GetString(memStream.ToArray())

        Catch ex As Exception
            Throw New ApplicationException("String decryption Error.")
        End Try
    End Function
#End Region

#Region "Private Methods"
    Private arrFiles() As String

    Private Function GetFileContents(ByVal FullPath As String) As String
        Dim strContents As String
        Dim objReader As StreamReader

        Try

            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Return strContents
        Catch Ex As Exception
            Throw New ApplicationException("Read Input File Error.")
        End Try
    End Function

    Private Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String) As Boolean
        Dim Contents As String
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter

        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            Throw New ApplicationException("Write Ouput File Error.")
        End Try

        Return bAns
    End Function

    Private Function FillRecursiveArray(ByVal strSourcePath As String) As Long
        Dim Cmptr As Integer = 0 'Sub-Folder Cmptr
        Dim lstStringFolders As New ArrayList 'Folder Array
        Dim strSubFolders As String() 'Sub-Folder Array
        Dim lstSortedFolders As New ArrayList 'Sort Folder and Sub-Folder
        Dim bufFolder As String 'Folder Name Buffer
        Dim bufFile As String 'File Name Buffer

        Try
            'Set Initial Path
            lstStringFolders.Add(strSourcePath)

            'Global Files Array
            ReDim arrFiles(0)

            'Set all sub-folder in the InitialPath
            Do Until Cmptr = lstStringFolders.Count
                strSubFolders = System.IO.Directory.GetDirectories(lstStringFolders.Item(Cmptr))
                lstStringFolders.AddRange(strSubFolders)
                Cmptr += 1
            Loop
        Catch ex As Exception
            Throw New ApplicationException("Folder Recursion Error.")
        End Try

        Try
            'Sort, to get sub-folder under his mother folder
            lstStringFolders.Sort()

            'Fill sorted folder
            For Each bufFolder In lstStringFolders
                lstSortedFolders.Add(bufFolder)
            Next
        Catch ex As Exception
            Throw New ApplicationException("Folder Sort Error.")
        End Try


        Try
            'Each folder
            For Each bufFolder In lstSortedFolders

                'Each File
                For Each bufFile In Directory.GetFiles(bufFolder)

                    'Set File Path into Private array
                    ReDim Preserve arrFiles(UBound(arrFiles) + 1)
                    arrFiles(UBound(arrFiles)) = bufFile

                Next

            Next
        Catch ex As Exception
            Throw New ApplicationException("File Recursion Error.")
        End Try

        Return arrFiles.Length

    End Function

    Private Function FillNonRecursiveArray(ByVal strSourcePath As String) As Long
        Dim bufFile As String 'File Name Buffer
        ReDim arrFiles(0) 'File Path Array (Only in the initial path)

        Try
            'For each file in the initial folder
            For Each bufFile In Directory.GetFiles(strSourcePath)

                'Set File Path in private array
                ReDim Preserve arrFiles(UBound(arrFiles) + 1)
                arrFiles(UBound(arrFiles)) = bufFile

            Next
        Catch ex As Exception
            Throw New ApplicationException("Error, can't list Initial Path File")
        End Try


        Return arrFiles.Length

    End Function

#End Region

End Class

Public Class clsRSA : Inherits ApplicationException
#Region "Private Var"
    Private mPrivateKey As String
    Private mPublicKey As String

    Private objRSA As New RSACryptoServiceProvider
#End Region

#Region "Propertys"
    Public Property PrivateKey() As String
        Get
            Return mPrivateKey
        End Get

        Set(ByVal Value As String)
            mPrivateKey = Value
        End Set
    End Property

    Public Property PublicKey() As String
        Get
            Return mPublicKey
        End Get

        Set(ByVal Value As String)
            mPublicKey = Value
        End Set
    End Property
#End Region

#Region "Methods"
    Public Function CreateNewKeys()
        Dim RSA As New RSACryptoServiceProvider

        'Hold Private Key (DECRYPTION)'
        mPrivateKey = RSA.ToXmlString(True)
        'Hold Public Key (ENCRYPTION)'
        mPublicKey = RSA.ToXmlString(False)
    End Function

    Public Function SaveKeysToFile(ByVal PrivateKeyPath As String, ByVal PublicKeyPath As String)
        Try
            SaveTextToFile(PrivateKey, PrivateKeyPath)
            SaveTextToFile(PublicKey, PublicKeyPath)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function LoadKeysFromFile(ByVal PrivateKeyPath As String, ByVal PublicKeyPath As String)
        Try
            PrivateKey = GetFileContents(PrivateKeyPath)
            PublicKey = GetFileContents(PublicKeyPath)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function StringEncrypt(ByVal strSource As String) As String
        Dim bufSource() As Byte
        Dim strEncrypted As String

        Try
            'Use public key to crypt
            objRSA.FromXmlString(PublicKey)

            'Convert Source String into byte array, and crypt it
            bufSource = objRSA.Encrypt(Encoding.Unicode.GetBytes(strSource), False)

            'Convert crypted byte array into string
            strEncrypted = Encoding.Unicode.GetString(bufSource)

            Return (strEncrypted)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function StringDecrypt(ByVal strSource As String) As String
        Dim bufSource() As Byte
        Dim strDecrypted As String

        Try
            'Use private file to decrypt
            objRSA.FromXmlString(PrivateKey)

            'Convert Source String into byte array, and decrypt it
            bufSource = objRSA.Decrypt(Encoding.Unicode.GetBytes(strSource), False)

            'Convert decrypted byte array into string
            strDecrypted = Encoding.Unicode.GetString(bufSource)
            Return (strDecrypted)
        Catch ex As Exception
            Throw (ex)
        End Try
    End Function

    Public Function FileEncrypt(ByVal inName As String, ByVal outName As String) As Boolean
        Dim sContents As String
        Dim strEncrypted As String

        sContents = GetFileContents(inName)

        strEncrypted = StringEncrypt(sContents)

        Dim bufSource() As Byte = Encoding.UTF8.GetBytes(strEncrypted)
        strEncrypted = System.Convert.ToBase64String(bufSource)


        If SaveTextToFile(strEncrypted, outName) = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function FileDecrypt(ByVal inName As String, ByVal outName As String) As Boolean
        Dim sContents As String
        Dim strEncrypted As String
        Dim bufSource() As Byte

        sContents = GetFileContents(inName)

        bufSource = System.Convert.FromBase64String(sContents)
        sContents = Encoding.UTF8.GetString(bufSource)

        strEncrypted = StringDecrypt(sContents)

        If SaveTextToFile(strEncrypted, outName) = True Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

#Region "Private Methods"
    Private arrFiles() As String

    Private Function GetFileContents(ByVal FullPath As String) As String
        Dim strContents As String
        Dim objReader As StreamReader

        Try

            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Return strContents
        Catch Ex As Exception
            Throw New ApplicationException("Read Input File Error.")
        End Try
    End Function

    Private Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String) As Boolean
        Dim Contents As String
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter

        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            Throw New ApplicationException("Write Ouput File Error.")
        End Try

        Return bAns
    End Function

    Private Function FillRecursiveArray(ByVal strSourcePath As String) As Long
        Dim Cmptr As Integer = 0 'Sub-Folder Cmptr
        Dim lstStringFolders As New ArrayList 'Folder Array
        Dim strSubFolders As String() 'Sub-Folder Array
        Dim lstSortedFolders As New ArrayList 'Sort Folder and Sub-Folder
        Dim bufFolder As String 'Folder Name Buffer
        Dim bufFile As String 'File Name Buffer

        Try
            'Set Initial Path
            lstStringFolders.Add(strSourcePath)

            'Global Files Array
            ReDim arrFiles(0)

            'Set all sub-folder in the InitialPath
            Do Until Cmptr = lstStringFolders.Count
                strSubFolders = System.IO.Directory.GetDirectories(lstStringFolders.Item(Cmptr))
                lstStringFolders.AddRange(strSubFolders)
                Cmptr += 1
            Loop
        Catch ex As Exception
            Throw New ApplicationException("Folder Recursion Error.")
        End Try

        Try
            'Sort, to get sub-folder under his mother folder
            lstStringFolders.Sort()

            'Fill sorted folder
            For Each bufFolder In lstStringFolders
                lstSortedFolders.Add(bufFolder)
            Next
        Catch ex As Exception
            Throw New ApplicationException("Folder Sort Error.")
        End Try


        Try
            'Each folder
            For Each bufFolder In lstSortedFolders

                'Each File
                For Each bufFile In Directory.GetFiles(bufFolder)

                    'Set File Path into Private array
                    ReDim Preserve arrFiles(UBound(arrFiles) + 1)
                    arrFiles(UBound(arrFiles)) = bufFile

                Next

            Next
        Catch ex As Exception
            Throw New ApplicationException("File Recursion Error.")
        End Try

        Return arrFiles.Length

    End Function

    Private Function FillNonRecursiveArray(ByVal strSourcePath As String) As Long
        Dim bufFile As String 'File Name Buffer
        ReDim arrFiles(0) 'File Path Array (Only in the initial path)

        Try
            'For each file in the initial folder
            For Each bufFile In Directory.GetFiles(strSourcePath)

                'Set File Path in private array
                ReDim Preserve arrFiles(UBound(arrFiles) + 1)
                arrFiles(UBound(arrFiles)) = bufFile

            Next
        Catch ex As Exception
            Throw New ApplicationException("Error, can't list Initial Path File")
        End Try


        Return arrFiles.Length

    End Function

#End Region

End Class
