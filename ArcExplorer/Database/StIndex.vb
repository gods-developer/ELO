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

Partial Public Class StIndex
    Public Property Id As Integer
    Public Property IndexName As String
    Public Property Creation As Date
    Public Property CreationUser As String
    Public Property LastUpdate As Nullable(Of Date)
    Public Property LastUpdateUser As String
    Public Property RowVersion As Integer

    Public Overridable Property ListItemIndexes As ICollection(Of ListItemIndex) = New HashSet(Of ListItemIndex)
    Public Overridable Property TreeItemIndexes As ICollection(Of TreeItemIndex) = New HashSet(Of TreeItemIndex)

End Class
