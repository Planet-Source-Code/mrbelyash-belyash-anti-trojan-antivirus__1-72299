Imports System
Public Class clsAESV2
    Implements IDisposable
#Region "New"
    Public Enum KeySize
        Bits128
        Bits192
        Bits256
    End Enum
    Private Nb As Integer ' block size in 32-bit words.  Always 4 for AES.  (128 bits).
    Private Nk As Integer ' key size in 32-bit words.  4, 6, 8.  (128, 192, 256 bits).
    Private Nr As Integer ' number of rounds. 10, 12, 14.
    Private key() As Byte ' the seed key. size will be 4 * keySize from ctor.
    Private Sbox(,) As Byte ' Substitution box
    Private iSbox(,) As Byte ' inverse Substitution box 
    Private w(,) As Byte ' key schedule array. 
    Private Rcon(,) As Byte ' Round constants.
    Private State(,) As Byte ' State matrix
    Private BlockSize As Integer = 16
    Public Sub New(ByVal keySize As KeySize, ByVal Password As String)
        ' convert the password to a byte array, then call the other init func
        Dim keyBytes As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(Password)
        Init(keySize, keyBytes)
    End Sub 'New
    Public Sub New(ByVal keySize As KeySize, ByVal keyBytes() As Byte)
        Init(keySize, keyBytes)
    End Sub 'New
    Private Sub Init(ByVal keySize As KeySize, ByVal keyBytes() As Byte)
        Dim num1 As Integer = 0
        Dim num2 As Integer = 0
        Dim num3 As Integer = 0
        Dim num4 As Byte = 1
        Try
            Me.SetNbNkNr(keySize)
            Me.key = New Byte((Me.Nk * 4) - 1) {}
            If (Me.key.Length = keyBytes.Length) Then
                keyBytes.CopyTo(Me.key, 0)
            Else
                num2 = Me.key.Length
                num3 = keyBytes.Length
                'num1()
                For num1 = 0 To num2 - 1
                    If (num1 < num3) Then
                        Me.key(num1) = keyBytes(num1)
                    Else
                        num4 = CType((num4 + 1), Byte)
                        Me.key(num1) = num4
                    End If
                Next num1
            End If
            Me.BuildSbox()
            Me.BuildInvSbox()
            Me.BuildRcon()
            Me.KeyExpansion()
        Catch exception1 As Exception
            Throw
        End Try
    End Sub 'Init
    Private Sub ClassCleanUp()
        Try
        Catch ex As Exception
        End Try
    End Sub
    Public Sub Dispose() Implements System.IDisposable.Dispose
        Try
            System.GC.SuppressFinalize(Me)
            ClassCleanUp()
        Catch ex As Exception
        End Try
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        ClassCleanUp()
    End Sub
#End Region
#Region "Encrypt"
    Public Function Encrypt(ByVal input As String) As String
        Dim rText As String = ""
        Dim i As Integer = 0
        Dim sLen As Integer = 0
        Dim bInput, bText As Byte()
        Try
            ' convert the string to a byte
            bInput = System.Text.ASCIIEncoding.ASCII.GetBytes(input)
            ' encrypt
            bText = Encrypt(bInput)
            ' convert the byte array to a string
            Return Convert.ToBase64String(bText)
        Catch excep As Exception
            Throw
        End Try
        ' return the text
    End Function 'Encrypt
    Public Function Encrypt(ByVal input() As Byte) As Byte()
        Dim i As Integer = 0
        Dim iLen As Integer = input.Length
        Dim output(0) As Byte
        Dim newInput() As Byte
        Dim inBuffer(BlockSize - 1) As Byte
        Dim buffer(BlockSize - 1) As Byte
        Dim count As Integer = 0
        Try
            ' we need to resize the arrays so they are 16 byte blocks
            count = GetArraySize(input.Length)
            output = New Byte(count - 1) {}
            newInput = New Byte(count - 1) {}
            ' copy the data from input to newInput
            System.Array.Copy(input, 0, newInput, 0, input.Length)
            ' we need to send the cipher function 16 bytes at a time to encrypt
            For i = 0 To count - BlockSize Step BlockSize
                ' copy the input into the input buffer array
                System.Array.Copy(newInput, i, inBuffer, 0, BlockSize) ' copy all 16 bytes
                ' encrypt this block
                System.Array.Copy(Cipher(inBuffer), 0, output, i, BlockSize)
            Next i
        Catch excep As Exception
            Throw
        End Try
        Return output
    End Function 'Encrypt
    'Private Function Cipher(ByVal byt() As Byte) As Byte()
    '    Dim bTmp(BlockSize - 1) As Byte
    '    mAES.Cipher(byt, bTmp)
    '    Return bTmp
    'End Function
    'Private Function InvCipher(ByVal byt() As Byte) As Byte()
    '    Dim bTmp(BlockSize - 1) As Byte
    '    mAES.InvCipher(byt, bTmp)
    '    Return bTmp
    'End Function
    Private Function GetArraySize(ByVal ArrayLen As Integer) As Integer
        ' if this is divisible by blocksize, return arraylen
        If ArrayLen Mod BlockSize = 0 Then
            Return ArrayLen
        End If
        ' return the new array size
        Return Int(ArrayLen / BlockSize + 1) * BlockSize
    End Function 'GetArraySize
    Public Function Decrypt(ByVal input As String) As String
        Dim rText As String = ""
        Dim i As Integer = 0
        Dim sLen As Integer = 0
        Dim bInput() As Byte
        Try
            bInput = Convert.FromBase64String(input)
            Return System.Text.ASCIIEncoding.ASCII.GetString(Decrypt(bInput))
        Catch excep As Exception
            Throw
        End Try
        ' return
    End Function 'Decrypt
    Public Function Decrypt(ByVal input() As Byte) As Byte()
        Dim i As Integer = 0
        Dim iLen As Integer = input.Length
        Dim inBuffer(BlockSize - 1) As Byte
        Dim buffer(BlockSize - 1) As Byte
        Dim output(input.Length - 1) As Byte
        Dim count As Integer = 0
        Try
            ' we need to send the cipher function 16 bytes at a time to encrypt
            For i = 0 To iLen - BlockSize Step BlockSize
                ' copy the input into the input buffer array
                System.Array.Copy(input, i, inBuffer, 0, BlockSize) ' copy all 16 bytes
                ' decrypt this block
                System.Array.Copy(InvCipher(inBuffer), 0, output, i, BlockSize)
            Next i
        Catch excep As Exception
            Throw
        End Try
        ' return the byte array
        Return output
    End Function 'Decrypt
#End Region
    Private Function Cipher(ByVal input() As Byte) As Byte() ' encipher 16-bit input
        Dim buffer1 As Byte() = New Byte(16 - 1) {}
        Try
            Me.State = New Byte(4 - 1, Me.Nb - 1) {}
            Dim num1 As Integer
            For num1 = 0 To (4 * Me.Nb) - 1
                Me.State((num1 Mod 4), Int(num1 / 4)) = input(num1)
            Next num1
            Me.AddRoundKey(0)
            Dim num2 As Integer = 1
            Do While (num2 <= (Me.Nr - 1))
                Me.SubBytes()
                Me.ShiftRows()
                Me.MixColumns()
                Me.AddRoundKey(num2)
                num2 += 1
            Loop
            Me.SubBytes()
            Me.ShiftRows()
            Me.AddRoundKey(Me.Nr)
            Dim num3 As Integer
            For num3 = 0 To (4 * Me.Nb) - 1
                buffer1(num3) = Me.State((num3 Mod 4), Int(num3 / 4))
            Next num3
        Catch exception1 As Exception
            Throw
        End Try
        Return buffer1
    End Function 'Cipher
    Private Function InvCipher(ByVal input() As Byte) As Byte() ' decipher 16-bit input
        Dim buffer1 As Byte() = New Byte(16 - 1) {}
        Try
            Me.State = New Byte(4 - 1, Me.Nb - 1) {}
            Dim num1 As Integer
            For num1 = 0 To (4 * Me.Nb) - 1
                Me.State((num1 Mod 4), Int(num1 / 4)) = input(num1)
            Next num1
            Me.AddRoundKey(Me.Nr)
            Dim num2 As Integer = (Me.Nr - 1)
            Do While (num2 >= 1)
                Me.InvShiftRows()
                Me.InvSubBytes()
                Me.AddRoundKey(num2)
                Me.InvMixColumns()
                num2 -= 1
            Loop
            Me.InvShiftRows()
            Me.InvSubBytes()
            Me.AddRoundKey(0)
            Dim num3 As Integer
            For num3 = 0 To (4 * Me.Nb) - 1
                buffer1(num3) = Me.State((num3 Mod 4), Int(num3 / 4))
            Next num3
        Catch exception1 As Exception
            Throw
        End Try
        Return buffer1
    End Function 'InvCipher
    Private Sub SetNbNkNr(ByVal keySize As KeySize) '
        Me.Nb = 4 ' block size always = 4 words = 16 bytes = 128 bits for AES
        If keySize = keySize.Bits128 Then
            Me.Nk = 4 ' key size = 4 words = 16 bytes = 128 bits
            Me.Nr = 10 ' rounds for algorithm = 10
        Else
            If keySize = keySize.Bits192 Then
                Me.Nk = 6 ' 6 words = 24 bytes = 192 bits
                Me.Nr = 12
            Else
                If keySize = keySize.Bits256 Then
                    Me.Nk = 8 ' 8 words = 32 bytes = 256 bits
                    Me.Nr = 14
                End If
            End If ' SetNbNkNr()
        End If
    End Sub 'SetNbNkNr

    Private Sub BuildSbox()
        Me.Sbox = New Byte(,) {{99, 124, 119, 123, 242, 107, 111, 197, 48, 1, 103, 43, 254, 215, 171, 118}, {202, 130, 201, 125, 250, 89, 71, 240, 173, 212, 162, 175, 156, 164, 114, 192}, {183, 253, 147, 38, 54, 63, 247, 204, 52, 165, 229, 241, 113, 216, 49, 21}, {4, 199, 35, 195, 24, 150, 5, 154, 7, 18, 128, 226, 235, 39, 178, 117}, {9, 131, 44, 26, 27, 110, 90, 160, 82, 59, 214, 179, 41, 227, 47, 132}, {83, 209, 0, 237, 32, 252, 177, 91, 106, 203, 190, 57, 74, 76, 88, 207}, {208, 239, 170, 251, 67, 77, 51, 133, 69, 249, 2, 127, 80, 60, 159, 168}, {81, 163, 64, 143, 146, 157, 56, 245, 188, 182, 218, 33, 16, 255, 243, 210}, {205, 12, 19, 236, 95, 151, 68, 23, 196, 167, 126, 61, 100, 93, 25, 115}, {96, 129, 79, 220, 34, 42, 144, 136, 70, 238, 184, 20, 222, 94, 11, 219}, {224, 50, 58, 10, 73, 6, 36, 92, 194, 211, 172, 98, 145, 149, 228, 121}, {231, 200, 55, 109, 141, 213, 78, 169, 108, 86, 244, 234, 101, 122, 174, 8}, {186, 120, 37, 46, 28, 166, 180, 198, 232, 221, 116, 31, 75, 189, 139, 138}, {112, 62, 181, 102, 72, 3, 246, 14, 97, 53, 87, 185, 134, 193, 29, 158}, {225, 248, 152, 17, 105, 217, 142, 148, 155, 30, 135, 233, 206, 85, 40, 223}, {140, 161, 137, 13, 191, 230, 66, 104, 65, 153, 45, 15, 176, 84, 187, 22}}
    End Sub 'BuildSbox
    Private Sub BuildInvSbox()
        Me.iSbox = New Byte(,) {{82, 9, 106, 213, 48, 54, 165, 56, 191, 64, 163, 158, 129, 243, 215, 251}, {124, 227, 57, 130, 155, 47, 255, 135, 52, 142, 67, 68, 196, 222, 233, 203}, {84, 123, 148, 50, 166, 194, 35, 61, 238, 76, 149, 11, 66, 250, 195, 78}, {8, 46, 161, 102, 40, 217, 36, 178, 118, 91, 162, 73, 109, 139, 209, 37}, {114, 248, 246, 100, 134, 104, 152, 22, 212, 164, 92, 204, 93, 101, 182, 146}, {108, 112, 72, 80, 253, 237, 185, 218, 94, 21, 70, 87, 167, 141, 157, 132}, {144, 216, 171, 0, 140, 188, 211, 10, 247, 228, 88, 5, 184, 179, 69, 6}, {208, 44, 30, 143, 202, 63, 15, 2, 193, 175, 189, 3, 1, 19, 138, 107}, {58, 145, 17, 65, 79, 103, 220, 234, 151, 242, 207, 206, 240, 180, 230, 115}, {150, 172, 116, 34, 231, 173, 53, 133, 226, 249, 55, 232, 28, 117, 223, 110}, {71, 241, 26, 113, 29, 41, 197, 137, 111, 183, 98, 14, 170, 24, 190, 27}, {252, 86, 62, 75, 198, 210, 121, 32, 154, 219, 192, 254, 120, 205, 90, 244}, {31, 221, 168, 51, 136, 7, 199, 49, 177, 18, 16, 89, 39, 128, 236, 95}, {96, 81, 127, 169, 25, 181, 74, 13, 45, 229, 122, 159, 147, 201, 156, 239}, {160, 224, 59, 77, 174, 42, 245, 176, 200, 235, 187, 60, 131, 83, 153, 97}, {23, 43, 4, 126, 186, 119, 214, 38, 225, 105, 20, 99, 85, 33, 12, 125}}
    End Sub 'BuildInvSbox
    Private Sub BuildRcon()
        Me.Rcon = New Byte(10, 3) {{0, 0, 0, 0}, {1, 0, 0, 0}, {2, 0, 0, 0}, {4, 0, 0, 0}, {8, 0, 0, 0}, {16, 0, 0, 0}, {32, 0, 0, 0}, {64, 0, 0, 0}, {128, 0, 0, 0}, {27, 0, 0, 0}, {54, 0, 0, 0}}
    End Sub 'BuildRcon
    Private Sub AddRoundKey(ByVal round As Integer)
        Dim num1 As Integer
        For num1 = 0 To 4 - 1
            Dim num2 As Integer
            For num2 = 0 To 4 - 1
                Me.State(num1, num2) = CType((Me.State(num1, num2) Xor Me.w(((round * 4) + num2), num1)), Byte)
            Next num2
        Next num1
    End Sub 'AddRoundKey
    Private Sub SubBytes()
        Dim num1 As Integer
        For num1 = 0 To 4 - 1
            Dim num2 As Integer
            For num2 = 0 To 4 - 1
                Me.State(num1, num2) = Me.Sbox((Me.State(num1, num2) >> 4), (Me.State(num1, num2) And 15))
            Next num2
        Next num1
    End Sub 'SubBytes
    Private Sub InvSubBytes()
        Dim num1 As Integer
        For num1 = 0 To 4 - 1
            Dim num2 As Integer
            For num2 = 0 To 4 - 1
                Me.State(num1, num2) = Me.iSbox((Me.State(num1, num2) >> 4), (Me.State(num1, num2) And 15))
            Next num2
        Next num1
    End Sub 'InvSubBytes
    Private Sub ShiftRows()
        Dim buffer1(,) As Byte = New Byte(4 - 1, 4 - 1) {}
        Dim num1 As Integer
        For num1 = 0 To 4 - 1
            Dim num2 As Integer
            For num2 = 0 To 4 - 1
                buffer1(num1, num2) = Me.State(num1, num2)
            Next num2
        Next num1
        Dim num3 As Integer
        For num3 = 1 To 4 - 1
            Dim num4 As Integer
            For num4 = 0 To 4 - 1
                Me.State(num3, num4) = buffer1(num3, ((num4 + num3) Mod Me.Nb))
            Next num4
        Next num3
    End Sub 'ShiftRows
    Private Sub InvShiftRows()
        Dim buffer1(,) As Byte = New Byte(4 - 1, 4 - 1) {}
        Dim num1 As Integer
        For num1 = 0 To 4 - 1
            Dim num2 As Integer
            For num2 = 0 To 4 - 1
                buffer1(num1, num2) = Me.State(num1, num2)
            Next num2
        Next num1
        Dim num3 As Integer
        For num3 = 1 To 4 - 1
            Dim num4 As Integer
            For num4 = 0 To 4 - 1
                Me.State(num3, ((num4 + num3) Mod Me.Nb)) = buffer1(num3, num4)
            Next num4
        Next num3
    End Sub 'InvShiftRows
    Private Sub MixColumns()
        Dim buffer1(,) As Byte = New Byte(4 - 1, 4 - 1) {}
        Dim num1 As Integer
        For num1 = 0 To 4 - 1
            Dim num2 As Integer
            For num2 = 0 To 4 - 1
                buffer1(num1, num2) = Me.State(num1, num2)
            Next num2
        Next num1
        Dim num3 As Integer
        For num3 = 0 To 4 - 1
            Me.State(0, num3) = CType((((gfmultby02(buffer1(0, num3)) Xor gfmultby03(buffer1(1, num3))) Xor gfmultby01(buffer1(2, num3))) Xor gfmultby01(buffer1(3, num3))), Byte)
            Me.State(1, num3) = CType((((gfmultby01(buffer1(0, num3)) Xor gfmultby02(buffer1(1, num3))) Xor gfmultby03(buffer1(2, num3))) Xor gfmultby01(buffer1(3, num3))), Byte)
            Me.State(2, num3) = CType((((gfmultby01(buffer1(0, num3)) Xor gfmultby01(buffer1(1, num3))) Xor gfmultby02(buffer1(2, num3))) Xor gfmultby03(buffer1(3, num3))), Byte)
            Me.State(3, num3) = CType((((gfmultby03(buffer1(0, num3)) Xor gfmultby01(buffer1(1, num3))) Xor gfmultby01(buffer1(2, num3))) Xor gfmultby02(buffer1(3, num3))), Byte)
        Next num3
    End Sub 'MixColumns
    Private Sub InvMixColumns()
        Dim buffer1(,) As Byte = New Byte(4 - 1, 4 - 1) {}
        Dim num1 As Integer
        For num1 = 0 To 4 - 1
            Dim num2 As Integer
            For num2 = 0 To 4 - 1
                buffer1(num1, num2) = Me.State(num1, num2)
            Next num2
        Next num1
        Dim num3 As Integer
        For num3 = 0 To 4 - 1
            Me.State(0, num3) = CType((((gfmultby0e(buffer1(0, num3)) Xor gfmultby0b(buffer1(1, num3))) Xor gfmultby0d(buffer1(2, num3))) Xor gfmultby09(buffer1(3, num3))), Byte)
            Me.State(1, num3) = CType((((gfmultby09(buffer1(0, num3)) Xor gfmultby0e(buffer1(1, num3))) Xor gfmultby0b(buffer1(2, num3))) Xor gfmultby0d(buffer1(3, num3))), Byte)
            Me.State(2, num3) = CType((((gfmultby0d(buffer1(0, num3)) Xor gfmultby09(buffer1(1, num3))) Xor gfmultby0e(buffer1(2, num3))) Xor gfmultby0b(buffer1(3, num3))), Byte)
            Me.State(3, num3) = CType((((gfmultby0b(buffer1(0, num3)) Xor gfmultby0d(buffer1(1, num3))) Xor gfmultby09(buffer1(2, num3))) Xor gfmultby0e(buffer1(3, num3))), Byte)
        Next num3
    End Sub 'InvMixColumns
    Private Shared Function gfmultby01(ByVal b As Byte) As Byte
        Return b
    End Function 'gfmultby01
    Private Shared Function gfmultby02(ByVal b As Byte) As Byte
        If (b < 128) Then
            Return CType((b << 1), Byte)
        End If
        Return CType(((b << 1) Xor 27), Byte)
    End Function 'gfmultby02
    Private Shared Function gfmultby03(ByVal b As Byte) As Byte
        Return CType((gfmultby02(b) Xor b), Byte)
    End Function 'gfmultby03
    Private Shared Function gfmultby09(ByVal b As Byte) As Byte
        Return CType((gfmultby02(gfmultby02(gfmultby02(b))) Xor b), Byte)
    End Function 'gfmultby09
    Private Shared Function gfmultby0b(ByVal b As Byte) As Byte
        Return CType(((gfmultby02(gfmultby02(gfmultby02(b))) Xor gfmultby02(b)) Xor b), Byte)
    End Function 'gfmultby0b
    Private Shared Function gfmultby0d(ByVal b As Byte) As Byte
        Return CType(((gfmultby02(gfmultby02(gfmultby02(b))) Xor gfmultby02(gfmultby02(b))) Xor b), Byte)
    End Function 'gfmultby0d
    Private Shared Function gfmultby0e(ByVal b As Byte) As Byte
        Return CType(((gfmultby02(gfmultby02(gfmultby02(b))) Xor gfmultby02(gfmultby02(b))) Xor gfmultby02(b)), Byte)
    End Function 'gfmultby0e
    Private Sub KeyExpansion()
        Me.w = New Byte((Me.Nb * (Me.Nr + 1)) - 1, 4 - 1) {}
        Dim num1 As Integer
        For num1 = 0 To Me.Nk - 1
            Me.w(num1, 0) = Me.key((4 * num1))
            Me.w(num1, 1) = Me.key(((4 * num1) + 1))
            Me.w(num1, 2) = Me.key(((4 * num1) + 2))
            Me.w(num1, 3) = Me.key(((4 * num1) + 3))
        Next num1
        Dim buffer1 As Byte() = New Byte(4 - 1) {}
        Dim num2 As Integer
        For num2 = Me.Nk To (Me.Nb * (Me.Nr + 1)) - 1
            buffer1(0) = Me.w((num2 - 1), 0)
            buffer1(1) = Me.w((num2 - 1), 1)
            buffer1(2) = Me.w((num2 - 1), 2)
            buffer1(3) = Me.w((num2 - 1), 3)
            If ((num2 Mod Me.Nk) = 0) Then
                buffer1 = Me.SubWord(Me.RotWord(buffer1))
                buffer1(0) = CType((buffer1(0) Xor Me.Rcon((num2 / Me.Nk), 0)), Byte)
                buffer1(1) = CType((buffer1(1) Xor Me.Rcon((num2 / Me.Nk), 1)), Byte)
                buffer1(2) = CType((buffer1(2) Xor Me.Rcon((num2 / Me.Nk), 2)), Byte)
                buffer1(3) = CType((buffer1(3) Xor Me.Rcon((num2 / Me.Nk), 3)), Byte)
            Else
                If ((Me.Nk > 6) AndAlso ((num2 Mod Me.Nk) = 4)) Then
                    buffer1 = Me.SubWord(buffer1)
                End If
            End If
            Me.w(num2, 0) = CType((Me.w((num2 - Me.Nk), 0) Xor buffer1(0)), Byte)
            Me.w(num2, 1) = CType((Me.w((num2 - Me.Nk), 1) Xor buffer1(1)), Byte)
            Me.w(num2, 2) = CType((Me.w((num2 - Me.Nk), 2) Xor buffer1(2)), Byte)
            Me.w(num2, 3) = CType((Me.w((num2 - Me.Nk), 3) Xor buffer1(3)), Byte)
        Next num2
    End Sub 'KeyExpansion
    Private Function SubWord(ByVal word() As Byte) As Byte()
        Return New Byte() {Me.Sbox((word(0) >> 4), (word(0) And 15)), Me.Sbox((word(1) >> 4), (word(1) And 15)), Me.Sbox((word(2) >> 4), (word(2) And 15)), Me.Sbox((word(3) >> 4), (word(3) And 15))}
    End Function 'SubWord
    Private Function RotWord(ByVal word() As Byte) As Byte()
        Return New Byte() {word(1), word(2), word(3), word(0)}
    End Function 'RotWord
    'Private Function ConvertStringToByteArray(ByVal StrToConvert As String) As Byte()
    '    Return System.Text.ASCIIEncoding.ASCII.GetBytes(StrToConvert)
    'End Function 'ConvertStringToByteArray
    'Private Function ConvertByteArrayToString(ByVal ByteToConvert() As Byte) As String
    '    Dim tempStr As String = ""
    '    Dim i As Integer
    '    For i = 0 To ByteToConvert.Length - 1
    '        ' do not convert 0
    '        If ByteToConvert(i) > 0 Then
    '            tempStr += System.Convert.ToChar(ByteToConvert(i))
    '        End If
    '    Next i
    '    Return tempStr
    'End Function 'ConvertByteArrayToString
End Class
