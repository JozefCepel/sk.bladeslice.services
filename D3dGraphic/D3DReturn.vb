Imports D3dGraphic2.FormD3dGraphic
Imports D3dGraphic2.FormD3dGraphic2

Public Class D3DReturn
    Public Property Type As String
    Public Property SN As String
    Public Property Sarza As String
    Public Property Location As String
    Public Property SklCena As Decimal
    Public Property Blok As List(Of ReturnBlok)
    Public Property Valec As List(Of ReturnValec)

    Public Sub New(_SN As String, _Sarza As String, _Location As String, _SklCena As Decimal, ByVal _D3dOp As D3dOperation)
        SN = _SN
        Location = _Location
        Sarza = _Sarza
        SklCena = _SklCena

        If _D3dOp = D3dOperation.D3dOperation_SimBlok Then
            Blok = New List(Of ReturnBlok)
            Type = "blok"
        ElseIf _D3dOp = D3dOperation.D3dOperation_SimValec Then
            Valec = New List(Of ReturnValec)
            Type = "valec"
        End If
    End Sub
End Class

Public Class ReturnBlok
    Inherits ReturnObject
    Public Property a1 As Integer
    Public Property a2 As Integer
    Public Property b1 As Integer
    Public Property b2 As Integer
    Public Property L1 As Integer
    Public Property L2 As Integer

    Public Sub New(coord As CoordinatesBlok, type As D3dType)
        a1 = coord.a1
        a2 = coord.a2
        L1 = coord.L1
        L2 = coord.L2
        b1 = coord.b1
        b2 = coord.b2
        d3dSimType = type
    End Sub

End Class

Public Class ReturnValec
    Inherits ReturnObject
    Public Property D1 As Integer
    Public Property d2 As Integer
    Public Property L1 As Integer
    Public Property L2 As Integer

    Public Sub New(coord As CoordinatesValec, type As D3dType)
        D1 = coord.D1
        d2 = coord.d2
        L1 = coord.L1
        L2 = coord.L2
        d3dSimType = type
    End Sub

End Class

Public MustInherit Class ReturnObject
    Friend Property d3dSimType As D3dType

    Public ReadOnly Property matColor As Integer
        Get
            Dim ret As Integer
            Select Case d3dSimType
                Case D3dType.D3dType_Olive
                    ret = &H8000
                Case D3dType.D3dType_FoundRezList
                    ret = &HFF0000
                Case D3dType.D3dType_NedotknutaCast
                    ret = &H808080
            End Select
            Return ret
        End Get
    End Property

    Public ReadOnly Property borderHeight As Integer
        Get
            Dim ret As Integer = 0
            If d3dSimType = D3dType.D3dType_FoundRezList Then
                ret = 8
            End If
            Return ret
        End Get
    End Property

    Public ReadOnly Property op As Decimal
        Get
            Dim ret As Decimal
            Select Case d3dSimType
                Case D3dType.D3dType_Olive
                    ret = 0.4
                Case D3dType.D3dType_FoundRezList
                    ret = 1
                Case D3dType.D3dType_NedotknutaCast
                    ret = 0.8
            End Select
            Return ret
        End Get
    End Property

    Public ReadOnly Property imgTexture As Boolean
        Get
            Return d3dSimType = D3dType.D3dType_FoundRezList
        End Get
    End Property

    Public Enum D3dType
        D3dType_NedotknutaCast = 1
        D3dType_Olive = 2
        D3dType_FoundRezList = 3
    End Enum

End Class