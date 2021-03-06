﻿Friend Class ComputationValec

    Friend SourceObjShape As ValecSourceObj
    Private _Rozmery As Coord_bod
    Private _Popis As String = ""
    Private _ObjemVyrez As Decimal = 0
    Private _ObjemZvysok As Decimal = 0
    Friend iValecType As ValecType
    Private foundRez As CoordinatesValec

    Friend OliveRez As ArrayList
    Friend CelyRez As ArrayList
    Friend NedotknutaCast As ArrayList
    Friend FoundRezList As ArrayList

    Private vyrezInfo As CoordinatesValec
    Private _OuterSizeFinalBod As Coord_bod

    Private Sub computeRezPily()
        vyrezInfo = New CoordinatesValec(CelyRez.Item(0))

        Dim t As CoordinatesValec
        Dim o As Integer

        For o = 0 To CelyRez.Count - 1
            t = CelyRez.Item(o)
            If (t.D1 > vyrezInfo.D1) Then vyrezInfo.D1 = t.D1
            If (t.d2 < vyrezInfo.d2) Then vyrezInfo.d2 = t.d2
            If (t.L1 < vyrezInfo.L1) Then vyrezInfo.L1 = t.L1
            If (t.L2 > vyrezInfo.L2) Then vyrezInfo.L2 = t.L2
        Next

    End Sub

    Friend Sub Recompute(ByVal iPocKS As Integer, ByVal strana_D1 As Boolean, ByVal strana_d2 As Boolean, ByVal strana_L As Boolean)
        Compute(SourceObjShape, Rozmery, iPocKS, iValecType, strana_D1, strana_d2, strana_L)
    End Sub

    Friend Function Compute(ByRef oSourceObjShape As ValecSourceObj,
                       ByVal DdLRozmery As Coord_bod,
                       ByVal iPocKS As Integer,
                       ByVal intValecType As ValecType,
                       Optional ByVal strana_D1 As Boolean = False,
                       Optional ByVal strana_d2 As Boolean = False,
                       Optional ByVal strana_L As Boolean = False) As Boolean
        Dim bRet As Boolean = False

        SourceObjShape = oSourceObjShape
        iValecType = intValecType

        foundRez = Nothing
        FoundRezList = New ArrayList
        NedotknutaCast = New ArrayList
        CelyRez = New ArrayList
        OliveRez = New ArrayList

        _Rozmery = DdLRozmery

        Dim o As Integer
        Dim _ZalohaNed As ArrayList = Nothing
        Dim _ZalohaCelyRez As ArrayList = Nothing

        Dim _Source As ArrayList = oSourceObjShape.coordList

        If (intValecType = ValecType.ValecType_Inner) Then
            FoundRezList = innerValecRez(_Source, strana_D1, strana_d2, strana_L, iPocKS)
            If (FoundRezList.Count > 0) Then
                bRet = True
            End If

            'Výpočet Olive časti pre INNER
            For j As Integer = 0 To CelyRez.Count - 1
                Dim cr As CoordinatesValec = CelyRez.Item(j)

                Dim frZlepene As New ArrayList
                For Each a As CoordinatesValec In FoundRezList
                    frZlepene.Add(New CoordinatesValec(a.d2, a.D1, a.L1, a.L2))
                Next
                OptimiseValec(frZlepene)

                For k As Integer = 0 To frZlepene.Count - 1
                    Dim fr As CoordinatesValec = frZlepene.Item(k)
                    If fr.L1 > cr.L1 Then
                        OliveRez.Add(New CoordinatesValec(cr.d2, cr.D1, cr.L1, fr.L1))
                    End If
                    If fr.L2 < cr.L2 Then
                        OliveRez.Add(New CoordinatesValec(cr.d2, cr.D1, fr.L2, cr.L2))
                    End If

                    If cr.d2 < fr.d2 Then
                        OliveRez.Add(New CoordinatesValec(cr.d2, fr.d2, fr.L1, fr.L2))
                    End If
                Next
            Next

        Else
            For o = 1 To iPocKS
                NedotknutaCast.Clear()
                foundRez = TryInsertValec(_Source, strana_D1, strana_d2, strana_L)
                If foundRez IsNot Nothing Then
                    bRet = True
                    FoundRezList.Add(foundRez)
                Else
                    If Not _ZalohaNed Is Nothing Then NedotknutaCast = _ZalohaNed
                    If Not _ZalohaCelyRez Is Nothing Then CelyRez = _ZalohaCelyRez
                    o = iPocKS
                End If
                _Source = NedotknutaCast.Clone
                _ZalohaNed = NedotknutaCast.Clone
                _ZalohaCelyRez = CelyRez.Clone
            Next

            'Výpočet Olive časti
            For j As Integer = 0 To CelyRez.Count - 1
                Dim cr As CoordinatesValec = CelyRez.Item(j)
                Dim f As Boolean = False
                For k As Integer = 0 To FoundRezList.Count - 1
                    Dim fr As CoordinatesValec = FoundRezList.Item(k)
                    If fr.L1 <= cr.L1 And fr.L2 >= cr.L2 Then
                        'Našiel som prislúchajúci valček k celému rezu
                        If cr.D1 > fr.D1 Then
                            OliveRez.Add(New CoordinatesValec(fr.D1, cr.D1, cr.L1, cr.L2))
                        End If
                        If cr.d2 < fr.d2 Then
                            OliveRez.Add(New CoordinatesValec(cr.d2, fr.d2, cr.L1, cr.L2))
                        End If
                        f = True
                        Exit For
                    End If
                Next
                If Not f Then
                    OliveRez.Add(New CoordinatesValec(cr.d2, cr.D1, cr.L1, cr.L2))
                End If
            Next
        End If

        OptimiseValec(OliveRez)

        If bRet Then computeRezPily()

        _OuterSizeFinalBod = GetValecSize(NedotknutaCast)

        _ObjemVyrez = GetValecObjem(CelyRez)
        _ObjemZvysok = _ObjemVyrez - GetValecObjem(FoundRezList)

        Select Case iValecType
            Case ValecType.ValecType_Left
                _Popis = "Zľava"
            Case ValecType.ValecType_Right
                _Popis = "Sprava"
            Case ValecType.ValecType_Inner
                _Popis = "Zvnútra"
        End Select
        Return bRet

    End Function

    Friend ReadOnly Property OuterSizeFinalBod() As Coord_bod
        Get
            Return _OuterSizeFinalBod
        End Get
    End Property

    Friend ReadOnly Property OuterSizeFinal() As String
        Get
            Return SizeDescriptionValec(OuterSizeFinalBod)
        End Get
    End Property

    Friend ReadOnly Property Rozmery() As Coord_bod
        Get
            Return _Rozmery
        End Get
    End Property

    Friend ReadOnly Property rezPilou() As CoordinatesValec
        Get
            Return vyrezInfo
        End Get
    End Property

    Friend ReadOnly Property Popis() As String
        Get
            Return _Popis
        End Get
    End Property

    Friend ReadOnly Property ObjemVyrezu() As Decimal
        Get
            Return _ObjemVyrez
        End Get
    End Property

    Friend ReadOnly Property ObjemOdrezku() As Decimal
        Get
            Return Me._ObjemZvysok
        End Get
    End Property

    Friend ReadOnly Property ObjemPolotovaru() As Decimal
        Get
            Return Me.ObjemVyrezu - Me.ObjemOdrezku
        End Get
    End Property

    Friend ReadOnly Property Pocet() As Integer
        Get
            Return FoundRezList.Count
        End Get
    End Property

    Friend Function innerValecRez(ByRef Source As ArrayList,
                                   ByVal strana_D1 As Boolean,
                                   ByVal strana_d2 As Boolean,
                                   ByVal strana_L As Boolean, ByVal iPocKS As Integer) As ArrayList
        Dim found As New ArrayList
        Dim temp As CoordinatesValec = Nothing
        Dim DesiredValec As New CoordinatesValec(Rozmery.bd, Rozmery.aD, 0, Rozmery.L)

        Dim i As Integer
        For i = 0 To Source.Count - 1
            temp = Source.Item(i)
            If (temp.D1 > DesiredValec.D1) Then
                NedotknutaCast.Add(New CoordinatesValec(DesiredValec.D1, temp.D1, temp.L1, temp.L2))
            End If
            If (temp.d2 < DesiredValec.D1) Then
                CelyRez.Add(New CoordinatesValec(temp.d2, IIf(DesiredValec.D1 < temp.D1, DesiredValec.D1, temp.D1), temp.L1, temp.L2))
            End If
        Next

        OptimiseValec(CelyRez)

        i = 0
        Dim f As Boolean = False

        While (i < CelyRez.Count And f = False)
            temp = CelyRez.Item(i)
            If (temp.D1 = DesiredValec.D1 And temp.d2 <= DesiredValec.d2) Then 'Pridal som kontrolu aj na d2
                temp = New CoordinatesValec(CelyRez.Item(i))
                f = True
            End If
            i += 1
        End While

        If Not f Then
            Return found
        End If

        i = iPocKS

        While (i > 0)
            If ((temp.L2 - temp.L1) >= DesiredValec.L2) Then
                found.Add(New CoordinatesValec(DesiredValec.d2, DesiredValec.D1, temp.L1, temp.L1 + DesiredValec.L2))
                temp.L1 += DesiredValec.L2
                i -= 1
            Else
                i = 0
            End If
        End While

        Return found

    End Function

    Friend Function TryInsertValec(ByRef Source As ArrayList,
                                   ByVal strana_D1 As Boolean,
                                   ByVal strana_d2 As Boolean,
                                   ByVal strana_L As Boolean) As CoordinatesValec
        Dim temp As CoordinatesValec
        Dim previous As CoordinatesValec = Nothing
        Dim possible As CoordinatesValec = Nothing

        Dim found As CoordinatesValec = Nothing
        Dim i As Integer

        Dim DesiredValec As New CoordinatesValec(Rozmery.bd, Rozmery.aD, 0, Rozmery.L)

        Dim L1 As Decimal
        Dim L2 As Decimal
        Dim L As Decimal = DesiredValec.L2 - DesiredValec.L1
        Dim bSpod As Boolean = (iValecType = ValecType.ValecType_Left)
        'Dim bInner As Boolean = (iValecType = ValecType.ValecType_Inner)

        sortValce(Source, bSpod)
        For i = 0 To Source.Count - 1
            temp = Source.Item(i)

            If found IsNot Nothing Then
                NedotknutaCast.Add(New CoordinatesValec(temp))
            Else

                If possible Is Nothing Then
                    If (temp.D1 >= DesiredValec.D1 And temp.d2 <= DesiredValec.d2) Then
                        If (temp.L2 - temp.L1) >= L Then

                            L1 = IIf(bSpod, temp.L1, temp.L2 - L)
                            L2 = IIf(bSpod, temp.L1 + L, temp.L2)

                            If Not strana_d2 Then
                                If (temp.d2 <> DesiredValec.d2) Then NedotknutaCast.Add(New CoordinatesValec(temp.d2, DesiredValec.d2, L1, L2))
                                If (temp.D1 <> DesiredValec.d2) Then CelyRez.Add(New CoordinatesValec(DesiredValec.d2, temp.D1, L1, L2))
                            Else
                                CelyRez.Add(New CoordinatesValec(temp.d2, temp.D1, L1, L2))
                            End If
                            If (temp.L2 - temp.L1) > L Then NedotknutaCast.Add(New CoordinatesValec(temp.d2, temp.D1,
                                temp.L1 + IIf(bSpod, L, 0),
                                temp.L2 - IIf(bSpod, 0, L)))
                            found = New CoordinatesValec(DesiredValec.d2, DesiredValec.D1, L1, L2)
                        Else
                            If Not strana_d2 Then
                                If (temp.d2 <> DesiredValec.d2) Then
                                    NedotknutaCast.Add(New CoordinatesValec(temp.d2, DesiredValec.d2, temp.L1, temp.L2))

                                End If
                                If (temp.D1 <> DesiredValec.d2) Then
                                    CelyRez.Add(New CoordinatesValec(DesiredValec.d2, temp.D1, temp.L1, temp.L2))

                                End If
                            Else
                                CelyRez.Add(New CoordinatesValec(temp.d2, temp.D1, temp.L1, temp.L2))

                            End If
                            possible = New CoordinatesValec(DesiredValec.d2, DesiredValec.D1, temp.L1, temp.L2)
                        End If
                    Else
                        If previous IsNot Nothing AndAlso previous.D1 > temp.D1 Then
                            Exit For
                        End If
                        If Not strana_d2 Then
                            If (temp.d2 <> DesiredValec.d2) Then NedotknutaCast.Add(New CoordinatesValec(temp.d2, DesiredValec.d2, temp.L1, temp.L2))
                            If (temp.D1 <> DesiredValec.d2) Then CelyRez.Add(New CoordinatesValec(DesiredValec.d2, temp.D1, temp.L1, temp.L2))
                        Else
                            CelyRez.Add(New CoordinatesValec(temp.d2, temp.D1, temp.L1, temp.L2))
                        End If
                        previous = temp
                    End If
                Else
                    If (temp.D1 >= DesiredValec.D1 And temp.d2 <= DesiredValec.d2) Then
                        If ((temp.L2 - temp.L1) >= (L - (possible.L2 - possible.L1))) Then

                            L1 = IIf(bSpod, temp.L1, possible.L2 - L)
                            L2 = IIf(bSpod, possible.L1 + L, temp.L2)

                            If Not strana_d2 Then
                                If (temp.d2 <> DesiredValec.d2) Then NedotknutaCast.Add(New CoordinatesValec(temp.d2, DesiredValec.d2, L1, L2))
                                If (temp.D1 <> DesiredValec.d2) Then CelyRez.Add(New CoordinatesValec(DesiredValec.d2, temp.D1, L1, L2))
                            Else
                                CelyRez.Add(New CoordinatesValec(temp.d2, temp.D1, L1, L2))
                            End If
                            If bSpod Then
                                If ((temp.L2 - temp.L1) > (L - (possible.L2 - possible.L1))) Then NedotknutaCast.Add(New CoordinatesValec(temp.d2, temp.D1, temp.L1 + (L - (possible.L2 - possible.L1)), temp.L2))
                                found = New CoordinatesValec(DesiredValec.d2, DesiredValec.D1, possible.L1, possible.L1 + L)
                            Else
                                If ((temp.L2 - temp.L1) > (L - (possible.L2 - possible.L1))) Then NedotknutaCast.Add(New CoordinatesValec(temp.d2, temp.D1, temp.L1, temp.L2 - (L - (possible.L2 - possible.L1))))
                                found = New CoordinatesValec(DesiredValec.d2, DesiredValec.D1, possible.L2 - L, possible.L2)
                            End If
                        Else
                            If Not strana_d2 Then
                                If (temp.d2 <> DesiredValec.d2) Then
                                    NedotknutaCast.Add(New CoordinatesValec(temp.d2, DesiredValec.d2, temp.L1, temp.L2))

                                End If
                                If (temp.D1 <> DesiredValec.d2) Then
                                    CelyRez.Add(New CoordinatesValec(DesiredValec.d2, temp.D1, temp.L1, temp.L2))

                                End If
                            Else
                                CelyRez.Add(New CoordinatesValec(temp.d2, temp.D1, temp.L1, temp.L2))

                            End If

                            possible = New CoordinatesValec(DesiredValec.d2, DesiredValec.D1,
                                IIf(bSpod, possible.L1, temp.L1),
                                IIf(bSpod, temp.L2, possible.L2))
                        End If
                    Else
                        Exit For
                    End If
                End If

            End If
        Next

        '********************************************

        For i = i To Source.Count - 1
            temp = Source.Item(i)
            NedotknutaCast.Add(New CoordinatesValec(temp))
        Next

        Return found

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class
