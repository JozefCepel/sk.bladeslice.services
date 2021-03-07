Friend Class MultiSimulationValec2
    Friend SourceObj As ValecSourceObj
    'Private _maxRez As Decimal
    Friend colComputations As Collection
    'Friend iListItemID As Integer
    Friend Key As String ' Kvoli identifikacii v collection
    'Friend Event SelectedSimulation(ByRef SingleSim As SingleSimulation2, ByRef MultiSimVal As Object)
    'Friend Event StartNewTocenie(ByRef SingleSim As SingleSimulation2)
    Friend SSV As SingleSimulationValec2

    Friend Property AA() As Boolean = False

    Friend Property SVP() As Boolean = False

    Friend Property PocKs() As Integer

    Friend Property Desired() As Coord_bod

    Friend Property rotationSpeed() As Decimal = 10000

    'Friend ReadOnly Property SelectedSN() As String
    '    Get
    '        Return cbSNs.Text
    '    End Get
    'End Property

    Friend ReadOnly Property CountMoznosti() As Integer
        Get
            Return colComputations.Count
        End Get
    End Property

    'Friend Sub RedrawSS(ByVal aaEnable As Boolean, ByVal svpEnable As Boolean)
    '    If Not AA = aaEnable Or Not SVP = svpEnable Then
    '        AA = aaEnable
    '        SVP = svpEnable
    '        TMResetAA.Stop()
    '        TMResetAA.Start()
    '    Else
    '        TMReset.Stop()
    '        TMReset.Start()
    '    End If
    'End Sub

    'Friend Sub changeSpeed(ByVal speed As Decimal)
    '    Me.SSV.rotateSpeed = speed
    'End Sub

    Public Sub New()
        SSV = New SingleSimulationValec2
    End Sub

    Friend Function GraphicsCompute(ByRef SourceObjShape As ValecSourceObj,
                                  ByVal iPocKS As Integer,
                                  ByVal DdLDesired As Coord_bod,
                                  ByVal intValecType As ValecType) As Boolean

        Me.PocKs = iPocKS
        Me.Desired = DdLDesired
        If Not colComputations Is Nothing Then colComputations.Clear()
        colComputations = New Collection
        Me.SourceObj = SourceObjShape.Clone
        'MultiSimulation bude vzdy obsahovat len jednu Computation
        Dim cmp As New ComputationValec
        If cmp.Compute(Me.SourceObj, Desired, PocKs, intValecType) Then
            colComputations.Add(cmp, cmp.Popis)
        End If

        With Me.SourceObj
            Dim s As String = ""
            If cmp.iValecType = ValecType.ValecType_Left Then
                s = " (L)"
            ElseIf cmp.iValecType = ValecType.ValecType_Right Then
                s = " (R)"
            ElseIf cmp.iValecType = ValecType.ValecType_Inner Then
                s = " (I)"
            End If
            'lblSN.Text = GetMaterialName(.SNs, .Sarza, .SklCena, .Location, False, .SizeDescription) & s
        End With

        Return True

    End Function

    Friend Sub GraphicsCombine()
        SSV.cmpCurrent = colComputations(1)
        SSV.MSKey = Me.Key
    End Sub

    'Friend Function GraphicsDraw() As Boolean
    '    Dim iDone As Integer = 0
    '    Dim i As Integer

    '    For i = 1 To SourceObj.ColSNs.Count
    '        cbSNs.Items.Add(SourceObj.ColSNs(i))
    '    Next
    '    lblPopis.Text = SourceObj.ColSNs(1)
    '    cbSNs.SelectedItem = cbSNs.Items(0)

    '    If (SourceObj.ColSNs.Count = 1) Then
    '        cbSNs.Hide()
    '    Else
    '        lblPopis.Hide()
    '    End If

    '    iDone += 1

    '    Return True

    'End Function

End Class
