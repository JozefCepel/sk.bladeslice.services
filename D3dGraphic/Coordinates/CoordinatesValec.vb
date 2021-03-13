Public Class CoordinatesValec
    Private _D1 As Decimal
    Private _d2 As Decimal
    Private _L1 As Decimal
    Private _L2 As Decimal

    Public Sub New()

    End Sub

    Public Sub New(ByVal coord As CoordinatesValec)
        _D1 = coord.D1
        _d2 = coord.d2
        _L1 = coord.L1
        _L2 = coord.L2
    End Sub

    Public Sub New(ByVal d2 As Decimal, ByVal D1 As Decimal, ByVal L1 As Decimal, ByVal L2 As Decimal)
        _D1 = D1
        _d2 = d2
        _L1 = L1
        _L2 = L2
    End Sub

    Public Property D1() As Decimal
        Get
            Return _D1
        End Get
        Set(ByVal value As Decimal)
            _D1 = value
        End Set
    End Property
    Public Property d2() As Decimal
        Get
            Return _d2
        End Get
        Set(ByVal value As Decimal)
            _d2 = value
        End Set
    End Property

    Public Property L1() As Decimal
        Get
            Return _L1
        End Get
        Set(ByVal value As Decimal)
            _L1 = value
        End Set
    End Property
    Public Property L2() As Decimal
        Get
            Return _L2
        End Get
        Set(ByVal value As Decimal)
            _L2 = value
        End Set
    End Property
End Class

