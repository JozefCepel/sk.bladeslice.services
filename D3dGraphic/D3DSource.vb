Public Class D3DSource
    Public Property Type As String
    Public Property SN As String
    Public Property Sarza As String
    Public Property Location As String
    Public Property SklCena As Decimal
    Public Property Blok As List(Of Blok)
    Public Property Valec As List(Of Valec)

    'Dodatočné parametre výstupu
    Public Property PocKs As Integer
    Public Property ObjemVyrez As Decimal
    Public Property ObjemZvysok As Decimal
    Public Property ObjemPlt As Decimal
    Public Property OuterSize As String
    Public Property OuterSizeFinal As String

End Class

Public Class Blok
    Public Property Add As Boolean
    Public Property a1 As Integer
    Public Property a2 As Integer
    Public Property b1 As Integer
    Public Property b2 As Integer
    Public Property L1 As Integer
    Public Property L2 As Integer

End Class

Public Class Valec
    Public Property Add As Boolean
    Public Property D1 As Integer
    Public Property d2 As Integer
    Public Property L1 As Integer
    Public Property L2 As Integer
End Class
