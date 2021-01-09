Friend Class MultiSimulationValec
  Friend SourceObj As ValecSourceObj
  Private _iSize As Integer
    Private _maxRez As Decimal
    Friend colComputations As Collection
    Friend iListItemID As Integer
    Friend Key As String ' Kvoli identifikacii v collection
    Friend Event SelectedSimulation(ByRef SingleSim As SingleSimulation, ByRef MultiSimVal As Object)
    Friend Event StartNewTocenie(ByRef SingleSim As SingleSimulation)

    Private Sub Me_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        ss_CloseSimulation(Me.SSV)
    End Sub

    Friend Property AA() As Boolean = False

    Friend Property SVP() As Boolean = False

    Friend Property PocKs() As Integer

    Friend Property Desired() As Coord_bod

    Friend Property rotationSpeed() As Decimal = 10000

    Friend ReadOnly Property SelectedSN() As String
        Get
            Return cbSNs.Text
        End Get
    End Property

    Friend ReadOnly Property CountMoznosti() As Integer
        Get
            Return colComputations.Count
        End Get
    End Property

    Friend Sub RedrawSS(ByVal iSize As Integer, ByVal aaEnable As Boolean, ByVal svpEnable As Boolean)
        _iSize = iSize
        If Not AA = aaEnable Or Not SVP = svpEnable Then
            AA = aaEnable
            SVP = svpEnable
            TMResetAA.Stop()
            TMResetAA.Start()
        Else
            TMReset.Stop()
            TMReset.Start()
        End If
    End Sub

    Friend Sub changeSpeed(ByVal speed As Decimal)
        Me.SSV.rotateSpeed = speed
    End Sub

    Friend Function GraphicsCompute(ByVal iSize As Integer,
                                  ByRef SourceObjShape As ValecSourceObj,
                                  ByVal iPocKS As Integer,
                                  ByVal DdLDesired As Coord_bod,
                                  ByVal intValecType As ValecType) As Boolean

        Me.PocKs = iPocKS
        Me.Desired = DdLDesired
        If Not colComputations Is Nothing Then colComputations.Clear()
        colComputations = New Collection
        Me.SourceObj = SourceObjShape.Clone
        _iSize = iSize
        Me.Width = _iSize
        Me.Height = _iSize
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
            lblSN.Text = GetMaterialName(.SNs, .Sarza, .SklCena, .Location, False, .SizeDescription) & s
        End With

        Return True

    End Function

    Friend Sub GraphicsCombine()
        SSV.cmpCurrent = colComputations(1)
        SSV.MSKey = Me.Key
    End Sub

    Friend Function GraphicsDraw() As Boolean
        Dim iDone As Integer = 0
        Dim i As Integer

        For i = 1 To SourceObj.ColSNs.Count
            cbSNs.Items.Add(SourceObj.ColSNs(i))
        Next
        lblPopis.Text = SourceObj.ColSNs(1)
        cbSNs.SelectedItem = cbSNs.Items(0)

        If (SourceObj.ColSNs.Count = 1) Then
            cbSNs.Hide()
        Else
            lblPopis.Hide()
        End If

        iDone += 1
        If SSV.GraphicsDraw(iDone, AA, SVP, rotationSpeed) Then
            AddHandler SSV.CloseSimulation, AddressOf ss_CloseSimulation
            AddHandler SSV.AfterMaxSimulation, AddressOf ss_AfterMaxSimulation
            AddHandler SSV.BeforeMaxSimulation, AddressOf ss_BeforeMaxSimulation
            AddHandler SSV.SelectedSimulation, AddressOf ss_SelectedSimulation
            AddHandler SSV.StartNewTocenie, AddressOf ss_StartNewTocenie
        Else
            MsgBox("Inicializácia grafiky valca zlyhala!", vbExclamation, "Chyba")
            Return False
        End If

        Return True

    End Function

    Private Sub ss_BeforeMaxSimulation(ByRef SingleSim As SingleSimulation)
        Static defWMS As Integer
        Static defHMS As Integer
        Dim max As Boolean

        max = Not SingleSim.maximize
        If defWMS = 0 Or max Then defWMS = Me.Width
        If defHMS = 0 Or max Then defHMS = Me.Height
        If max Then
            If Me.Parent.Width > Me.Parent.Height Then
                Me.Width = Me.Parent.Height - 20
                Me.Height = Me.Parent.Height - 20
            Else
                Me.Width = Me.Parent.Width - 30
                Me.Height = Me.Parent.Width - 30
            End If
        Else
            Me.Width = defWMS
            Me.Height = defHMS
        End If
    End Sub

    Private Sub ss_AfterMaxSimulation(ByRef SingleSim As SingleSimulation)
    End Sub

    Private Sub ss_CloseSimulation(ByRef SingleSim As SingleSimulation)
        SingleSim.RemovePaintEvent()
        SingleSim.Dispose()
        Me.Dispose()
    End Sub

    Friend Sub RemoveAllSSPaintEvents()
        Me.SSV.RemovePaintEvent()
    End Sub

    Private Sub ss_SelectedSimulation(ByRef SingleSim As SingleSimulation)
        RaiseEvent SelectedSimulation(SingleSim, Me)
    End Sub

    Private Sub ss_StartNewTocenie(ByRef SingleSim As SingleSimulation)
        RaiseEvent StartNewTocenie(SingleSim)
    End Sub

    Private Sub TMReset_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMReset.Tick
        TMReset.Stop()
        If Not Me.SSV.maximize Then
            Me.Height = _iSize
            Me.Width = _iSize
        End If
        Me.SSV.enabledAA = AA
        Me.SSV.enabledSVP = SVP
        Me.SSV.ResetDevice()
    End Sub

    Private Sub TMResetAA_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TMResetAA.Tick
        TMResetAA.Stop()
        If Not Me.SSV.maximize Then
            Me.Height = _iSize
            Me.Width = _iSize
        End If
        Me.SSV.enabledAA = AA
        Me.SSV.enabledSVP = SVP
        Me.SSV.ResetDeviceCore()
  End Sub

End Class
