'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Namespace OrgMan

    Partial Public Class Reminder
        Public Property Id As Integer
        Public Property AppUserId As Integer
        Public Property TreeItemId As Integer
        Public Property Filename As String
        Public Property ReminderDate As Date
        Public Property Done As Boolean
        Public Property Creation As Date
        Public Property CreationUser As String
        Public Property LastUpdate As Nullable(Of Date)
        Public Property LastUpdateUser As String
        Public Property RowVersion As Integer
    
        Public Overridable Property AppUser As AppUser
        Public Overridable Property TreeItem As TreeItem
    
    End Class

End Namespace
