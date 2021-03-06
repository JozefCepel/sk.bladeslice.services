﻿Friend Class MultiSimulationBlok2
    Friend SourceObj As BlokSourceObj
    Private _maxRez As Decimal
    Friend colComputations As Collection
    Friend colSSimulations As Collection
    'Friend iListItemID As Integer
    Private threshold As Decimal
    Friend Key As String ' Kvoli identifikacii v collection
    'Friend Event SelectedSimulation(ByRef SingleSim As SingleSimulation2, ByRef MultiSimBlok As Object)
    'Friend Event StartNewTocenie(ByRef SingleSim As SingleSimulation2)
    'Friend Event Resized(ByRef MultiSimBlok As MultiSimulationBlok2)

    Public Sub New()
    End Sub

    Private Function porovnajabL(ByVal value1 As CoordinatesBlok, ByVal value2 As CoordinatesBlok) As Boolean

        If value1.b1 < value2.b1 Then
            Return True
        ElseIf value1.b1 = value2.b1 Then

            If value1.L1 < value2.L1 Then
                Return True
            ElseIf value1.L1 = value2.L1 Then

                If value1.a1 < value2.a1 Then
                    Return True
                End If
            End If
        End If

        Return False

    End Function

    Private Sub usporiadaj(ByRef coordList As ArrayList)

        Dim i As Integer
        Dim i2 As Integer
        Dim temp As New CoordinatesBlok

        For i = 1 To coordList.Count - 1

            For i2 = 1 To coordList.Count - i

                If porovnajabL(coordList.Item(i2), coordList.Item(i2 - 1)) Then
                    temp = coordList.Item(i2)
                    coordList.Item(i2) = coordList.Item(i2 - 1)
                    coordList.Item(i2 - 1) = temp
                End If

            Next

        Next

    End Sub

    Friend Property AA() As Boolean = False

    Friend Property SVP() As Boolean = False

    Friend Property PocKs() As Integer

    Friend Property Desired() As Coord_bod

    Friend Property rotationSpeed() As Decimal = 10000

    Friend ReadOnly Property ObjemHladania() As Decimal
        Get
            Return PocKs * Desired.aD * Desired.L * Desired.bd
        End Get
    End Property

    'Friend ReadOnly Property SelectedSN() As String
    '    Get
    '        Return cbSNs.Text
    '    End Get
    'End Property

    Friend ReadOnly Property CountMoznosti() As Integer
        Get
            Return colComputations.Count ' FLP.Controls.Count
        End Get
    End Property

    'Friend Sub RedrawSS(ByVal iSize As Integer, ByVal aaEnable As Boolean, ByVal svpEnable As Boolean)
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
    '    For Each ssim As SingleSimulationBlok2 In FLP.Controls
    '        If Not ssim Is Nothing Then
    '            ssim.rotateSpeed = speed
    '        End If
    '    Next
    'End Sub

    Private Function compareCmp(ByRef newCmp As ComputationBlok, ByRef cmpCol As Collection) As Boolean
        Dim found As Boolean = False

        For Each cmpToCompare As ComputationBlok In cmpCol
            If porovnajListy(newCmp, cmpToCompare, 0) And porovnajListy(newCmp, cmpToCompare, 1) Then
                found = True
                Exit For
            End If
        Next


        Return found

    End Function

    Private Function filterOtoceni(ByVal otocenie As Byte, ByVal podobnostStran As Integer, ByVal orientL As Boolean) As Boolean

        If orientL Then
            If podobnostStran = 4 Or podobnostStran = 2 Then
                Return (otocenie = 1)
            Else
                Return (otocenie = 1 Or otocenie = 3)
            End If
        End If

        Select Case podobnostStran
            Case 0
                Return True
            Case 1
                Return (otocenie = 1 Or otocenie = 3 Or otocenie = 4)
            Case 2
                Return (otocenie = 1 Or otocenie = 2 Or otocenie = 4)
            Case 3
                Return (otocenie = 1 Or otocenie = 2 Or otocenie = 3)
            Case 4
                Return (otocenie = 1)
        End Select
    End Function

    Friend Function GraphicsCompute(ByRef SourceObjShape As BlokSourceObj,
                                      ByVal iPocKS As Integer,
                                      ByVal abLDesired As Coord_bod,
                                      ByVal bAllCorners As Boolean,
                                      ByVal mRez As Decimal,
                                      ByVal thresh As Decimal) As Boolean

        Dim cisty As Boolean = (SourceObjShape.coordList.Count = 1)
        Dim podobnostStran As Integer = 0
        Dim bExitFor As Boolean = False
        threshold = thresh

        Me.PocKs = iPocKS
        _maxRez = mRez
        Me.Desired = abLDesired
        If Not colComputations Is Nothing Then colComputations.Clear()
        colComputations = New Collection

        Me.SourceObj = SourceObjShape.Clone

        If abLDesired.aD = abLDesired.L And abLDesired.aD = abLDesired.bd Then
            podobnostStran = 4
        Else
            If abLDesired.aD = abLDesired.L Then
                podobnostStran = 1
            Else
                If abLDesired.aD = abLDesired.bd Then
                    podobnostStran = 2
                Else
                    If abLDesired.bd = abLDesired.L Then
                        podobnostStran = 3
                    End If
                End If
            End If
        End If

        With Me.SourceObj
            'lblSN.Text = GetMaterialName(.SNs, .Sarza, .SklCena, .Location, True, .SizeDescription)

            Dim iRohStart As Integer = IIf(bAllCorners, 1, 5)
            Dim iRohEnd As Integer = 8

            If cisty Then
                iRohStart = 6
                iRohEnd = 6
            End If

            For roh As Byte = iRohStart To iRohEnd
                For riadky As Byte = 1 To 2
                    For otocenie As Byte = 1 To 6

                        If filterOtoceni(otocenie, podobnostStran, SourceObjShape.OrientedL) Then
                            otoc_abL(Desired, otocenie)   '**** Otocenie
                            Dim rozmer As New Coord_bod(Desired)

                            For strana As Byte = 1 To 3
                                Dim cmp As New ComputationBlok
                                cmp.Compute(Me.SourceObj, roh, riadky, otocenie, strana, rozmer, _maxRez, PocKs, threshold)
                                If cmp.CelyRez.Count > 0 And cmp.FoundRezList.Count > 0 Then

                                    If Not compareCmp(cmp, colComputations) Then
                                        colComputations.Add(cmp, cmp.Popis)
                                        If Me.PocKs = 0 Then
                                            cmp.CelyRez = New ArrayList
                                            cmp.FoundRezList = New ArrayList
                                            bExitFor = True
                                            Exit For

                                        End If

                                    End If

                                End If

                            Next
                            If bExitFor Then Exit For
                            otoc_abL(Desired, -otocenie)   '**** Vrat spat

                        End If

                    Next
                    If bExitFor Then Exit For
                Next
                If bExitFor Then Exit For
            Next
            '*******************************************************

            Return True
        End With

    End Function

    Friend Sub GraphicsCombine(ByVal bShowAllSimulations As Boolean)
        Dim bFound As Boolean
        Dim ssTest As SingleSimulationBlok2
        Dim i As Integer

        If Not colSSimulations Is Nothing Then colSSimulations.Clear()
        colSSimulations = New Collection

        For Each cmp As ComputationBlok In colComputations
            bFound = False
            For i = 1 To colSSimulations.Count
                ssTest = colSSimulations(i)
                For Each cmpToCompare As ComputationBlok In ssTest.Computations
                    If porovnajListy(cmp, cmpToCompare, 1) Then
                        ssTest.Computations.Add(cmp)
                        bFound = True
                        Exit For
                    End If
                Next
                If bFound Then Exit For
            Next

            If Not bFound Then
                If Not cmp.bDoNotDraw Then
                    ssTest = New SingleSimulationBlok2

                    ssTest.Computations.Add(cmp)
                    ssTest.cmpCurrent = cmp ' Nastav daky default, neskor sa moze prebit
                    ssTest.MSKey = Me.Key
                    If bShowAllSimulations Then ssTest.ShowSimulation = True ' Pouzite pri "Rohy" chceme zobrazit vsetko, inac sa to zobrazuje podla sortovania
                    colSSimulations.Add(ssTest)
                End If
            End If
        Next
    End Sub

    'Friend Function GraphicsDraw() As Boolean
    '    Dim ssTest As SingleSimulationBlok2
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

    '    lblSimsCount.Text = "Počet simulácii: " & FLP.Controls.Count & " z " & colSSimulations.Count
    '    Return True
    'End Function

    'Friend Function RawGraphicsDraw() As Boolean

    '    Dim ssTest As SingleSimulationBlok2

    '    btnVsetkyRohy.Visible = False
    '    lblSimsCount.Visible = False
    '    ssTest = New SingleSimulationBlok2

    '    If colComputations.Count = 0 Then
    '        MsgBox("Nenašla sa žiadna alternatíva!", vbExclamation, "Chyba")
    '        Return False
    '    End If
    '    ssTest.Computations.Add(colComputations(1))

    '    FLP.Controls.Add(ssTest)

    '    Return True
    'End Function

    'Private Sub btnVsetkyRohy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVsetkyRohy.Click

    '    'Vycisti povodne
    '    DisposeSSs()

    '    Me.GraphicsCompute(Me.SourceObj, Me.PocKs, Me.Desired, True, _maxRez, threshold)
    '    Me.GraphicsCombine(True)
    '    SortSingleSimulations(Me.colSSimulations) ' Zotried SS v MS
    '    Me.GraphicsDraw()
    'End Sub

    Private Function porovnajListy(ByVal cmpNew As ComputationBlok, ByVal cmpToCompare As ComputationBlok, ByVal type As Integer) As Boolean

        If cmpToCompare.FoundRezList.Count <> cmpNew.FoundRezList.Count Then Return False

        If type = 0 Then
            usporiadaj(cmpToCompare.FoundRezList)
            usporiadaj(cmpNew.FoundRezList)

            If cmpNew.FoundRezList.Count = cmpToCompare.FoundRezList.Count Then
                If porovnajList(cmpNew.FoundRezList, cmpToCompare.FoundRezList) Then Return True
            End If
        End If


        If type = 1 Then
            usporiadaj(cmpToCompare.CelyRez)
            usporiadaj(cmpNew.CelyRez)

            If cmpNew.CelyRez.Count = cmpToCompare.CelyRez.Count Then
                If porovnajList(cmpNew.CelyRez, cmpToCompare.CelyRez) Then Return True
            End If
        End If


        Return False

    End Function

    Private Function porovnajList(ByVal coordList As ArrayList, ByVal coordList2 As ArrayList) As Boolean

        Dim i As Integer
        Dim temp As New CoordinatesBlok
        Dim temp2 As New CoordinatesBlok

        For i = 0 To coordList.Count - 1

            temp = coordList.Item(i)
            temp2 = coordList2.Item(i)

            If temp.a1 <> temp2.a1 Or temp.b1 <> temp2.b1 Or temp.L1 <> temp2.L1 Or temp.a2 <> temp2.a2 Or temp.b2 <> temp2.b2 Or temp.L2 <> temp2.L2 Then
                Return False
            End If
        Next

        Return True

    End Function

End Class
