Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.Security.AccessControl
Imports System.Security.Principal
Imports System.Security
Imports System.Collections

Public Class RegistryFunctions

    Public Sub AddAdministratorsGroup(ByVal path As String)
        If Strings.Right(path, 1) <> "\" Then path = path & "\"
        Dim objKey As RegistryKey = GetRegistryKey(RegistryHive.CurrentUser, path, True)
        If Not (objKey Is Nothing) Then
            Dim strSubKeys As String() = objKey.GetSubKeyNames
            Dim objProtectedStorageKey As RegistryKey = objKey.OpenSubKey(strSubKeys(0), True)
            Dim strKeyPath As String = path + strSubKeys(0)
            AddAdministratorsGroupToRegistryKey(RegistryHive.CurrentUser, strKeyPath)
        Else
            Throw New Exception("Protected sub key not found")
        End If
    End Sub

    Public Sub RemoveAdministratorsGroup(ByVal path As String)
        If Strings.Right(path, 1) <> "\" Then path = path & "\"
        Dim objKey As RegistryKey = GetRegistryKey(RegistryHive.CurrentUser, path, True)
        If Not (objKey Is Nothing) Then
            Dim strSubKeys As String() = objKey.GetSubKeyNames
            Dim objProtectedStorageKey As RegistryKey = objKey.OpenSubKey(strSubKeys(0), True)
            Dim strKeyPath As String = path + strSubKeys(0)
            RemoveAdministratorGroupFromRegistryKey(RegistryHive.CurrentUser, strKeyPath)
        Else
            Throw New Exception("Protected sub key not found.")
        End If
    End Sub

    Public Sub AddAdministratorsGroupToRegistryKey(ByVal pobjRegistryHive As RegistryHive, ByVal pstrKeyPath As String)
        Dim objKey As RegistryKey = GetRegistryKey(pobjRegistryHive, pstrKeyPath, True)
        If Not (objKey Is Nothing) Then
            Dim objSecurityInfo As RegistrySecurity = objKey.GetAccessControl(AccessControlSections.All)
            AddAccessRule(objSecurityInfo, "BUILTIN\Administrators")
            objKey.SetAccessControl(objSecurityInfo)
        Else
            Throw New Exception("Key doesn't exist")
        End If
    End Sub

    Public Sub RemoveAdministratorGroupFromRegistryKey(ByVal pobjRegistryHive As RegistryHive, ByVal pstrKeyPath As String)
        Dim objKey As RegistryKey = GetRegistryKey(pobjRegistryHive, pstrKeyPath, True)
        If Not (objKey Is Nothing) Then
            Dim objSecurityInfo As RegistrySecurity = objKey.GetAccessControl(AccessControlSections.All)
            RemoveAccessRule(objSecurityInfo, "BUILTIN\Administrators")
            objKey.SetAccessControl(objSecurityInfo)
        Else
            Throw New Exception("Key doesn't exist")
        End If
    End Sub

    Private Function GetRegistryKey(ByVal pobjRegistryHive As RegistryHive, ByVal pstrKeyPath As String, ByVal pblnAccessibility As Boolean) As RegistryKey
        Dim objRegistryKey As RegistryKey = Nothing
        Select Case pobjRegistryHive
            Case RegistryHive.LocalMachine
                objRegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(pstrKeyPath, pblnAccessibility)
                Exit Select
            Case RegistryHive.ClassesRoot
                objRegistryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(pstrKeyPath, pblnAccessibility)
                Exit Select
            Case RegistryHive.CurrentConfig
                objRegistryKey = Microsoft.Win32.Registry.CurrentConfig.OpenSubKey(pstrKeyPath, pblnAccessibility)
                Exit Select
            Case RegistryHive.CurrentUser
                objRegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(pstrKeyPath, pblnAccessibility)
                Exit Select
            Case RegistryHive.DynData
                objRegistryKey = Microsoft.Win32.Registry.DynData.OpenSubKey(pstrKeyPath, pblnAccessibility)
                Exit Select
            Case RegistryHive.PerformanceData
                objRegistryKey = Microsoft.Win32.Registry.PerformanceData.OpenSubKey(pstrKeyPath, pblnAccessibility)
                Exit Select
            Case RegistryHive.Users
                objRegistryKey = Microsoft.Win32.Registry.Users.OpenSubKey(pstrKeyPath, pblnAccessibility)
                Exit Select
        End Select
        Return objRegistryKey
    End Function

    Private Function GetNonInheritedAccessRules(ByVal pobjRegistrySecurity As RegistrySecurity, ByVal pstrIdentityReference As String) As RegistrySecurity
        Dim objNewRegistrySecurity As RegistrySecurity = New RegistrySecurity
        Dim objRulesCollection As AuthorizationRuleCollection = pobjRegistrySecurity.GetAccessRules(True, True, GetType(NTAccount))
        For Each objAccessRule As AuthorizationRule In objRulesCollection
            If (objAccessRule.IdentityReference.Value = pstrIdentityReference) Then
                If Not (objAccessRule.PropagationFlags = PropagationFlags.InheritOnly) Then
                    objNewRegistrySecurity.AddAccessRule(CType(objAccessRule, RegistryAccessRule))
                End If
            Else
                objNewRegistrySecurity.AddAccessRule(CType(objAccessRule, RegistryAccessRule))
            End If
        Next
        Return objNewRegistrySecurity
    End Function

    Private Sub CopyAccessRules(ByVal pobjSource As RegistrySecurity, ByRef pobjDestination As RegistrySecurity)
        Dim objRulesCollection As AuthorizationRuleCollection = pobjSource.GetAccessRules(True, True, GetType(NTAccount))
        For Each objAccessRule As AccessRule In objRulesCollection
            pobjDestination.SetAccessRule(CType(objAccessRule, RegistryAccessRule))
        Next
    End Sub

    Private Sub AddAccessRule(ByRef pobjRegistrySecurity As RegistrySecurity, ByVal pstrIdentityReference As String)
        Dim objIdentityReference As IdentityReference = GetIdentityReference(pstrIdentityReference)
        Dim objAccessRule As RegistryAccessRule = New RegistryAccessRule(objIdentityReference, RegistryRights.FullControl, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow)
        pobjRegistrySecurity.SetAccessRule(objAccessRule)
        pobjRegistrySecurity.SetAccessRuleProtection(False, False)
    End Sub

    Private Function RemoveAccessRule(ByRef pobjRegistrySecurity As RegistrySecurity, ByVal pstrIdentityReference As String) As Boolean
        Dim blnRemoveAccessRule As Boolean = False
        Dim objAccessRuleFound As AccessRule = Nothing
        Dim objIdentityReference As IdentityReference = GetIdentityReference(pstrIdentityReference)
        Dim objAccount As NTAccount = CType(objIdentityReference.Translate(GetType(NTAccount)), NTAccount)
        Dim objRulesCollection As AuthorizationRuleCollection = pobjRegistrySecurity.GetAccessRules(True, True, GetType(NTAccount))
        For Each objAccessRule As RegistryAccessRule In objRulesCollection
            If objAccessRule.IdentityReference.Value = objAccount.Value AndAlso objAccessRule.IsInherited = True Then
                objAccessRuleFound = objAccessRule
                Exit For
            End If
        Next
        If Not (objAccessRuleFound Is Nothing) Then
            If objAccessRuleFound.IsInherited = True Then
                Dim objTempRegistrySecurity As RegistrySecurity = GetNonInheritedAccessRules(pobjRegistrySecurity, objAccount.Value)
                pobjRegistrySecurity.SetAccessRuleProtection(True, False)
                CopyAccessRules(objTempRegistrySecurity, pobjRegistrySecurity)
                pobjRegistrySecurity.RemoveAccessRule(CType(objAccessRuleFound, RegistryAccessRule))
                blnRemoveAccessRule = True
            Else
                pobjRegistrySecurity.RemoveAccessRule(CType(objAccessRuleFound, RegistryAccessRule))
                blnRemoveAccessRule = True
            End If
        End If
        Return blnRemoveAccessRule
    End Function

    Private Function GetIdentityReference(ByVal pstrIdentityReference As String) As IdentityReference
        Dim objFoundIdentityReference As IdentityReference = Nothing
        Dim objDomain As AppDomain = System.Threading.Thread.GetDomain
        objDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal)
        Dim objPrincipal As WindowsPrincipal = CType(System.Threading.Thread.CurrentPrincipal, WindowsPrincipal)
        Dim objIdentity As WindowsIdentity = CType(objPrincipal.Identity, WindowsIdentity)
        Dim objIdentityReferenceCollection As IdentityReferenceCollection = objIdentity.Groups
        For Each objIdentityReference As IdentityReference In objIdentityReferenceCollection
            Dim objAccount As NTAccount = CType(objIdentityReference.Translate(GetType(NTAccount)), NTAccount)
            If objAccount.Value = pstrIdentityReference Then
                objFoundIdentityReference = objIdentityReference
                Exit For
            End If
        Next
        If objFoundIdentityReference Is Nothing Then
            Return objIdentity.User
        Else
            Return objFoundIdentityReference
        End If
    End Function

End Class
