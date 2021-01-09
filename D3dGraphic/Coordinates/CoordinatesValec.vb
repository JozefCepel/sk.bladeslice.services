Friend Class CoordinatesValec
    Private _D1 As Integer
    Private _d2 As Integer
    Private _L1 As Integer
    Private _L2 As Integer

    Friend Sub New()

  End Sub

  Friend Sub New(ByVal coord As CoordinatesValec)
    _D1 = coord.D1
    _d2 = coord.d2
    _L1 = coord.L1
    _L2 = coord.L2
  End Sub

    Friend Sub New(ByVal D1 As Integer, ByVal d2 As Integer, ByVal L1 As Integer, ByVal L2 As Integer)
        _D1 = D1
        _d2 = d2
        _L1 = L1
        _L2 = L2
    End Sub

    Friend Property D1() As Integer
        Get
            Return _D1
        End Get
        Set(ByVal value As Integer)
            _D1 = value
        End Set
    End Property
    Friend Property d2() As Integer
        Get
            Return _d2
        End Get
        Set(ByVal value As Integer)
            _d2 = value
        End Set
    End Property

    Friend Property L1() As Integer
        Get
            Return _L1
        End Get
        Set(ByVal value As Integer)
            _L1 = value
        End Set
    End Property
    Friend Property L2() As Integer
        Get
            Return _L2
        End Get
        Set(ByVal value As Integer)
            _L2 = value
        End Set
    End Property
End Class

