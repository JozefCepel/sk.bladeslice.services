
Friend Class CoordinatesBlok
    Private _a1 As Integer
    Private _L1 As Integer
    Private _b1 As Integer
    Private _a2 As Integer
    Private _L2 As Integer
    Private _b2 As Integer

    Friend Sub New()

  End Sub

  Friend Sub New(ByVal coord As CoordinatesBlok)

    _a1 = coord.a1
    _L1 = coord.L1
    _b1 = coord.b1
    _a2 = coord.a2
    _L2 = coord.L2
    _b2 = coord.b2

  End Sub

    Friend Sub New(ByVal a1 As Integer, ByVal a2 As Integer, ByVal L1 As Integer, ByVal L2 As Integer, ByVal b1 As Integer, ByVal b2 As Integer)

        _a1 = a1
        _L1 = L1
        _b1 = b1
        _a2 = a2
        _L2 = L2
        _b2 = b2

    End Sub

    Friend Function GetOsValue(ByVal iOs As Os_bod, ByVal iCoordRank As CoordRank) As Integer
        Select Case iOs
            Case Os_bod.Os_aD
                Return IIf(iCoordRank = CoordRank.CoordRank1, _a1, _a2)
            Case Os_bod.Os_bd
                Return IIf(iCoordRank = CoordRank.CoordRank1, _L1, _L2)
            Case Os_bod.Os_L
                Return IIf(iCoordRank = CoordRank.CoordRank1, _b1, _b2)
        End Select
    End Function

    Friend Property a1() As Integer
        Get
            Return _a1
        End Get
        Set(ByVal value As Integer)
            _a1 = value
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
    Friend Property b1() As Integer
        Get
            Return _b1
        End Get
        Set(ByVal value As Integer)
            _b1 = value
        End Set
    End Property
    Friend Property a2() As Integer
        Get
            Return _a2
        End Get
        Set(ByVal value As Integer)
            _a2 = value
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
    Friend Property b2() As Integer
        Get
            Return _b2
        End Get
        Set(ByVal value As Integer)
            _b2 = value
        End Set
    End Property

End Class
