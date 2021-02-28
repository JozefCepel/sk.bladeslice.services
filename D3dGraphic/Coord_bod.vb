Public Class Coord_bod

    Private _aD As Decimal
    Private _bd As Decimal
    Private _L As Decimal

    Public Sub New()

    End Sub

    Public Sub New(ByVal Bod As Coord_bod)
        _aD = Bod.aD
        _bd = Bod.bd
        _L = Bod.L
    End Sub

    Public Sub New(ByVal aD As Decimal, ByVal L As Decimal, ByVal bd As Decimal)
        _aD = aD
        _bd = bd
        _L = L
    End Sub

    Public Function GetOsValue(ByVal iOs As Os_bod) As Decimal
        Select Case iOs
            Case Os_bod.Os_aD
                Return aD
            Case Os_bod.Os_L
                Return L
            Case Os_bod.Os_bd
                Return bd
        End Select
    End Function

    Public Property aD() As Decimal
        Get
            'Dim s As String = "ahoj " & vbCrLf & "cau"`
            Return _aD
        End Get
        Set(ByVal value As Decimal)
            _aD = value
        End Set
    End Property

    Public Property bd() As Decimal
        Get
            Return _bd
        End Get
        Set(ByVal value As Decimal)
            _bd = value
        End Set
    End Property

    Public Property L() As Decimal
        Get
            Return _L
        End Get
        Set(ByVal value As Decimal)
            _L = value
        End Set
    End Property

End Class
