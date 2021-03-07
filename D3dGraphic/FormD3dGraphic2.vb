Option Explicit On
Imports System.IO
Imports System.Linq
Imports Newtonsoft.Json

Public Class FormD3dGraphic2

    Private colSNKeys As Collection
    Private colSNObjectShapes As Collection
    'Private grd As New clsGrid

    Public Enum D3dOperation
        D3dOperation_SimBlok = 1
        D3dOperation_SimValec = 2
        D3dOperation_ShowBlok = 3
        D3dOperation_ShowValec = 4
    End Enum

    Private _D3dOp As D3dOperation

    Public Sub New()

    End Sub

    'Private Sub setRbnLabels(ByVal sCaption As String, ByVal bSim As Boolean, ByVal aD As Decimal, ByVal bd As Decimal, ByVal L As Decimal, ByVal PocetKusov As Integer)

    '    uiDetail.Text = sCaption
    '    rbnGrpInput.Visible = bSim
    '    rbn_cont_max_rez.Visible = bSim And Not _D3dOp = D3dOperation.D3dOperation_SimValec
    '    rbn_cont_threshold.Visible = bSim And Not _D3dOp = D3dOperation.D3dOperation_SimValec

    '    If bSim Then
    '        If _D3dOp = D3dOperation.D3dOperation_SimValec Then
    '            cont_rbn_aD.Text = "D [" & sMjRozmery & "]"
    '            cont_rbn_bd.Text = "d [" & sMjRozmery & "]"
    '            rbn_bd.Minimum = 0
    '        Else
    '            cont_rbn_aD.Text = "a [" & sMjRozmery & "]"
    '            cont_rbn_bd.Text = "b [" & sMjRozmery & "]"
    '            rbn_bd.Minimum = 1
    '        End If
    '        cont_rbn_L.Text = "L [" & sMjRozmery & "]"

    '        rbn_aD.Value = aD
    '        rbn_bd.Value = bd
    '        rbn_L.Value = L
    '        rbn_poc_ks.Value = PocetKusov
    '    End If
    '    uiSN.Closed = Not bSim

    '    HKEY_SECTION_NAME_TYP = HKEY_SECTION_NAME & "_" & CStr(_D3dOp)

    'End Sub

    Private Sub DebugLog(ByVal sTxt As String) ' Log to file debug info
        Dim sFile As String = ""
        Try
            sFile = "C:\Log.txt"
            File.AppendAllText(sFile, sTxt & vbCrLf, System.Text.Encoding.Default)
        Catch ex As Exception
            MsgBox("Error saving '" & sFile & "'." & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Function RunValec(ByVal sCaption As String, ByVal bOnlyShow As Boolean,
                          ByVal strMjObjem As String, ByVal strMjRozmery As String,
                          ByVal intPocetMiestObjem As Integer, ByVal intKoefMjObjemRozmery As Integer,
                          Optional ByVal PocetKusov As Integer = 0,
                          Optional ByRef D1 As Decimal = 0, Optional ByRef d2 As Decimal = 0, Optional ByRef L As Decimal = 0,
                          Optional ByVal Input3DData As String = "", Optional ByRef Output3DData As String = "",
                          Optional ByVal bOnlyReturnObjem As Boolean = False, Optional ByVal bOrientedL As Boolean = False) As Decimal


        InicializeVariables(sCaption, bOnlyShow, strMjObjem, strMjRozmery, intPocetMiestObjem, intKoefMjObjemRozmery, PocetKusov, 0, 0, D1, d2, L)

        '*********************************
        If String.IsNullOrEmpty(Input3DData) Then
            Exit Function
        Else

            Dim fetch = New List(Of D3DSource)()
            fetch = JsonConvert.DeserializeObject(Of D3DSource())(Input3DData)

            PrepareValecData(fetch)
        End If

        If bOnlyReturnObjem Then
            If colSNObjectShapes.Count = 0 Then
                Return 0
            Else
                Return GetValecObjem(colSNObjectShapes.Item(colSNObjectShapes.Count).coordList)
            End If
        Else
            'Dim ret As DialogResult = Me.ShowDialog()
            'If ret = DialogResult.OK Then
            '    Output3DData = grd.GetOutput3DData
            '    D1 = rbn_aD.Value
            '    d2 = rbn_bd.Value
            '    L = rbn_L.Value
            'Else
            '    Output3DData = ""
            'End If
            Return 0
        End If

    End Function

    Public Sub PrepareValecData(fetch As D3DSource())
        Dim CS As ValecSourceObj = Nothing
        Dim f As D3DSource
        Dim va As Valec

        Dim sorted = fetch.OrderBy(Function(x) x.SN)

        For Each f In sorted
            If f.Type.ToLower() = "valec" Then

                Dim sortedvalec = f.Valec.
                    OrderBy(Function(x) x.D1).ThenBy(Function(x) x.d2).
                    ThenBy(Function(x) x.L1).ThenBy(Function(x) x.L2)

                CS = New ValecSourceObj(f.SN, f.Sarza, f.Location, f.SklCena, f.SN, sortedvalec)

                'Ak už také rozmery existujú, tak ich nepridám, iba rozšírim SN
                Dim sRef = CS.CoordToString()
                Dim s As String
                Dim bEx As Boolean = False

                For Each s In colSNKeys
                    Dim v As ValecSourceObj = colSNObjectShapes(s)
                    If sRef = v.CoordToString() Then
                        bEx = True
                        v.SNs = v.SNs & "; " & f.SN
                        v.ColSNs.Add(f.SN, f.SN)
                    End If
                Next

                If Not bEx Then

                    For Each va In sortedvalec
                        If va.Add Then
                            CS.CoordToAdd(New CoordinatesValec(va.D1, va.d2, va.L1, va.L2))
                        Else
                            CS.CoordToRemove(New CoordinatesValec(va.D1, va.d2, va.L1, va.L2))
                        End If
                    Next

                    CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu
                    If Not IsExistKey(colSNKeys, f.SN) Then
                        colSNKeys.Add(f.SN, f.SN)
                        colSNObjectShapes.Add(CS, f.SN)
                    End If

                End If
            End If
        Next

    End Sub

    Public Function RunBlok(ByVal sCaption As String, ByVal bOnlyShow As Boolean,
                          ByVal strMjObjem As String, ByVal strMjRozmery As String,
                          ByVal intPocetMiestObjem As Integer, ByVal intKoefMjObjemRozmery As Integer,
                          Optional ByVal PocetKusov As Integer = 0,
                          Optional ByRef a As Decimal = 0, Optional ByRef b As Decimal = 0, Optional ByRef L As Decimal = 0,
                          Optional ByVal Input3DData As String = "", Optional ByRef Output3DData As String = "",
                          Optional ByVal bOnlyReturnObjem As Boolean = False, Optional ByVal bOrientedL As Boolean = False) As Decimal

        InicializeVariables(sCaption, bOnlyShow, strMjObjem, strMjRozmery, intPocetMiestObjem, intKoefMjObjemRozmery, PocetKusov, a, b, 0, 0, L)

        '*********************************
        If String.IsNullOrEmpty(Input3DData) Then
            Exit Function
        Else

            Dim fetch = New List(Of D3DSource)()
            fetch = JsonConvert.DeserializeObject(Of D3DSource())(Input3DData)

            PrepareBlokData(bOrientedL, fetch)
        End If

        If bOnlyReturnObjem Then
            If colSNObjectShapes.Count = 0 Then
                Return 0
            Else
                'Return GetValecObjem(CS.coordList)
                Return GetBlokObjem(colSNObjectShapes.Item(colSNObjectShapes.Count).coordList)
            End If
        Else
            'Dim ret As DialogResult = Me.ShowDialog()
            'If ret = DialogResult.OK Then
            '    Output3DData = grd.GetOutput3DData
            '    a = rbn_aD.Value
            '    b = rbn_bd.Value
            '    L = rbn_L.Value
            'Else
            '    Output3DData = ""
            'End If
            Return 0
        End If

    End Function

    Public Sub InicializeVariables(sCaption As String, bOnlyShow As Boolean, strMjObjem As String, strMjRozmery As String, intPocetMiestObjem As Integer, intKoefMjObjemRozmery As Integer, PocetKusov As Integer,
                                   a As Integer, b As Integer, D1 As Integer, d2 As Integer, L As Integer)
        sMjObjem = strMjObjem
        sMjRozmery = strMjRozmery
        iPocetMiestObjem = intPocetMiestObjem
        iKoefMjObjemRozmery = intKoefMjObjemRozmery

        Dim bSim As Boolean = (a <> 0 And b <> 0 And L <> 0) Or (D1 <> 0 And L <> 0)

        If bSim Then
            _D3dOp = IIf(bOnlyShow, If(a <> 0 And b <> 0 And L <> 0, D3dOperation.D3dOperation_ShowBlok, D3dOperation.D3dOperation_ShowValec),
                                    If(a <> 0 And b <> 0 And L <> 0, D3dOperation.D3dOperation_SimBlok, D3dOperation.D3dOperation_SimValec))
        End If

        'setRbnLabels(sCaption, bSim, a + D1, b + d2, L, PocetKusov)

        colSNKeys = New Collection
        colSNObjectShapes = New Collection
    End Sub

    Public Sub PrepareBlokData(bOrientedL As Boolean, fetch As D3DSource())
        Dim CS As BlokSourceObj = Nothing
        Dim f As D3DSource
        Dim bl As Blok

        Dim sorted = fetch.OrderBy(Function(x) x.SN)

        For Each f In sorted
            If f.Type.ToLower() = "blok" Then

                Dim sortedblok = f.Blok.
                    OrderBy(Function(x) x.a1).ThenBy(Function(x) x.a2).
                    ThenBy(Function(x) x.L1).ThenBy(Function(x) x.L2).
                    ThenBy(Function(x) x.b1).ThenBy(Function(x) x.b2)

                CS = New BlokSourceObj(f.SN, f.Sarza, f.Location, f.SklCena, bOrientedL, f.SN, sortedblok)

                'Ak už také rozmery existujú, tak ich nepridám, iba rozšírim SN
                Dim sRef = CS.CoordToString()
                Dim s As String
                Dim bEx As Boolean = False

                For Each s In colSNKeys
                    Dim v As BlokSourceObj = colSNObjectShapes(s)
                    If sRef = v.CoordToString() Then
                        bEx = True
                        v.SNs = v.SNs & "; " & f.SN
                        v.ColSNs.Add(f.SN, f.SN)
                    End If
                Next

                If Not bEx Then
                    For Each bl In sortedblok
                        If bl.Add Then
                            CS.CoordToAdd(New CoordinatesBlok(bl.a1, bl.a2, bl.L1, bl.L2, bl.b1, bl.b2))
                        Else
                            CS.CoordToRemove(New CoordinatesBlok(bl.a1, bl.a2, bl.L1, bl.L2, bl.b1, bl.b2))
                        End If
                    Next
                    CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu

                    If Not IsExistKey(colSNKeys, f.SN) Then
                        colSNKeys.Add(f.SN, f.SN)
                        colSNObjectShapes.Add(CS, f.SN)
                    End If

                End If

            End If
        Next
    End Sub

    Public Function SimulateBlok(ByVal DesiredBlok As Coord_bod, ByRef sims As List(Of D3DReturn), ByVal pocKs As Integer, maxRez As Integer, threshold As Integer, maxSim As Integer) As Boolean
        Dim bms As MultiSimulationBlok2
        Dim iCount As Integer
        Dim colMs As New Collection
        Dim i As Integer, h As Integer
        Dim bRes As Boolean

        For i = 1 To colSNKeys.Count
            bms = New MultiSimulationBlok2
            If bms.GraphicsCompute(colSNObjectShapes(i), pocKs, DesiredBlok, False, maxRez, threshold) Then
                If bms.CountMoznosti > 0 Then
                    iCount += bms.CountMoznosti
                    bms.Key = CStr(i)
                    colMs.Add(bms, bms.Key)
                End If
            End If
        Next

        'lboxSN.Items.Clear()
        If iCount > 0 Then

            '****************************** 
            '22.1.2012  Optimalizacia vykreslenia
            'Zotriedit Computations aby sa vobec negeneroval SS pre tie co sa nebudu zobrazovat

            Dim colAllCmps As New Collection
            Dim j As Integer
            For i = 1 To colMs.Count
                bms = colMs(i)
                For j = 1 To bms.colComputations.Count
                    colAllCmps.Add(bms.colComputations(j))
                Next
            Next

            If colAllCmps.Count > CInt(maxSim * 1.3) Then  'Pridam este 1,3 koeficient - aby bolo dost simulacii na dobeh
                SortComputations(colAllCmps) ' Zotried Computations v MSs
                For j = CInt(maxSim * 1.3) + 1 To colAllCmps.Count
                    DirectCast(colAllCmps(j), ComputationBlok).bDoNotDraw = True
                Next
            End If
            '*********************************

            h = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "MS_Height", "-1")
            For i = 1 To colMs.Count
                bms = colMs(i)
                bms.GraphicsCombine(False)
                SortSingleSimulations(bms.colSSimulations) ' Zotried SS v MS
            Next

            SortToMaxSim(colMs, maxSim) ' Zotried vsetky SS zo vsetkych MS na prvych MaxSim a potom aj samotne MS

            ' Zobraz
            For i = 1 To colMs.Count
                bms = colMs(i)
                'bms.AA = rbn_AA.Checked
                'bms.SVP = rbn_SVP.Checked
                'bms.rotationSpeed = 9000 - TBCitlivost.Value * 600

                Dim s As BlokSourceObj = bms.SourceObj

                Dim simulacia As D3DReturn = New D3DReturn(s.SN, s.Sarza, s.Location, s.SklCena, D3dOperation.D3dOperation_SimBlok)
                sims.Add(simulacia)

                Dim cv As ComputationBlok = bms.colComputations(1)

                For j = 0 To cv.FoundRezList.Count - 1
                    Dim coord As CoordinatesBlok = cv.FoundRezList.Item(j)
                    simulacia.Blok.Add(New ReturnBlok(New CoordinatesBlok(coord.a1, coord.a2, coord.L1, coord.L2, coord.b1, coord.b2), ReturnObject.D3dType.D3dType_FoundRezList))
                Next

                For j = 0 To cv.CelyRez.Count - 1
                    Dim coord As CoordinatesBlok = cv.CelyRez.Item(j)
                    simulacia.Blok.Add(New ReturnBlok(New CoordinatesBlok(coord.a1, coord.a2, coord.L1, coord.L2, coord.b1, coord.b2), ReturnObject.D3dType.D3dType_CelyRez))
                Next

                'For j = 0 To cv.NedotknutaCast.Count - 1
                '    Dim coord As CoordinatesBlok = cv.NedotknutaCast.Item(j)
                '    simulacia.Blok.Add(New ReturnBlok(New CoordinatesBlok(coord.a1, coord.a2, coord.L1, coord.L2, coord.b1, coord.b2), ReturnObject.D3dType.D3dType_NedotknutaCast))
                'Next

                'If h <> -1 Then bms.Height = h
                'bms.Width = FLP.Width - 25
                bRes = True
                'FLP.Controls.Add(bms)
                'lboxSN.Items.Add(New ValueDescriptionPair(bms.SourceObj.SizeDescription & " - " & bms.SourceObj.SNs, bms))
                'bms.iListItemID = lboxSN.Items.Count - 1
                If Not bRes Then Exit For
            Next
        End If

        Return iCount > 0

    End Function

    Public Function SimulateValec(ByVal DesiredValec As Coord_bod, ByRef sims As List(Of D3DReturn), ByVal pocKs As Integer, maxSim As Integer) As Boolean
        Dim vms As MultiSimulationValec2
        Dim iCount As Integer
        Dim colMs As New Collection
        Dim i As Integer
        Dim bRes As Boolean
        Dim colEx As New Collection

        For i = 1 To colSNKeys.Count
            'Sprava
            vms = New MultiSimulationValec2
            Dim s As ValecSourceObj = colSNObjectShapes(i)

            If vms.GraphicsCompute(s, pocKs, DesiredValec, ValecType.ValecType_Right) Then
                If vms.CountMoznosti > 0 Then
                    iCount += vms.CountMoznosti
                    vms.Key = CStr(i) + "_RIGHT"
                    colMs.Add(vms, vms.Key)
                End If
            End If

            If vms.SourceObj.coordList.Count > 1 Then

                'Zlava
                vms = New MultiSimulationValec2
                If vms.GraphicsCompute(colSNObjectShapes(i), pocKs, DesiredValec, ValecType.ValecType_Left) Then
                    If vms.CountMoznosti > 0 Then
                        iCount += vms.CountMoznosti
                        vms.Key = CStr(i) + "_LEFT"
                        colMs.Add(vms, vms.Key)
                    End If
                End If

            End If

            'Zvnutra
            vms = New MultiSimulationValec2
            If vms.GraphicsCompute(colSNObjectShapes(i), pocKs, DesiredValec, ValecType.ValecType_Inner) Then
                If vms.CountMoznosti > 0 Then
                    iCount += vms.CountMoznosti
                    vms.Key = CStr(i) + "_INNER"
                    colMs.Add(vms, vms.Key)
                End If
            End If

        Next

        'lboxSN.Items.Clear()
        If iCount > 0 Then

            For i = 1 To colMs.Count
                vms = colMs(i)
                vms.GraphicsCombine()
            Next

            SortToMaxSim(colMs, maxSim) ' Zotried vsetky SS zo vsetkych MS na prvych MaxSim a potom aj samotne MS

            ' Zobraz
            For i = 1 To colMs.Count
                vms = colMs(i)
                'vms.AA = rbn_AA.Checked
                'vms.SVP = rbn_SVP.Checked
                'vms.rotationSpeed = 9000 - TBCitlivost.Value * 600

                Dim s As ValecSourceObj = vms.SourceObj

                Dim simulacia As D3DReturn = New D3DReturn(s.SN, s.Sarza, s.Location, s.SklCena, D3dOperation.D3dOperation_SimValec)
                sims.Add(simulacia)

                Dim cv As ComputationValec = vms.colComputations(1)

                For j As Integer = 0 To cv.FoundRezList.Count - 1
                    Dim coord As CoordinatesValec = cv.FoundRezList.Item(j)
                    simulacia.Valec.Add(New ReturnValec(New CoordinatesValec(coord.D1, coord.d2, coord.L1, coord.L2), ReturnValec.D3dType.D3dType_FoundRezList))
                Next

                For j As Integer = 0 To cv.CelyRez.Count - 1
                    Dim coord As CoordinatesValec = cv.CelyRez.Item(j)
                    simulacia.Valec.Add(New ReturnValec(New CoordinatesValec(coord.D1, coord.d2, coord.L1, coord.L2), ReturnValec.D3dType.D3dType_CelyRez))
                Next

                For j As Integer = 0 To cv.NedotknutaCast.Count - 1
                    Dim coord As CoordinatesValec = cv.NedotknutaCast.Item(j)
                    simulacia.Valec.Add(New ReturnValec(New CoordinatesValec(coord.D1, coord.d2, coord.L1, coord.L2), ReturnValec.D3dType.D3dType_NedotknutaCast))
                Next

                bRes = True
                'FLP.Controls.Add(vms)
                'If Not colEx.Contains(vms.SourceObj.SN) Then  'Robim to cez kolekciu, kedze mozu byt dve simulacie s rovnakym SN

                '    If vms.SSV.cmpCurrent.iValecType = ValecType.ValecType_Left Then
                '        lboxSN.Items.Add(New ValueDescriptionPair(vms.SourceObj.SizeDescription & " (L) " & vms.SourceObj.SNs, vms))
                '    ElseIf vms.SSV.cmpCurrent.iValecType = ValecType.ValecType_Right Then
                '        lboxSN.Items.Add(New ValueDescriptionPair(vms.SourceObj.SizeDescription & " (R) " & vms.SourceObj.SNs, vms))
                '    ElseIf vms.SSV.cmpCurrent.iValecType = ValecType.ValecType_Inner Then
                '        lboxSN.Items.Add(New ValueDescriptionPair(vms.SourceObj.SizeDescription & " (I) " & vms.SourceObj.SNs, vms))
                '    End If

                '    vms.iListItemID = lboxSN.Items.Count - 1
                'End If
                If Not bRes Then Exit For
            Next
        End If

        Return iCount > 0

    End Function

    Private Sub SortToMaxSim(ByRef colMs As Collection, MaxSim As Integer) ' Sortne vsetky SS z MS a oznaci ich kt sa maju zobrazit
        Dim colAllSS As New Collection
        Dim msb As MultiSimulationBlok2
        Dim msv As MultiSimulationValec2

        ' Urob kolekciu so vsetkymi SS
        For Each o As Object In colMs
            If TypeOf o Is MultiSimulationBlok2 Then
                msb = DirectCast(o, MultiSimulationBlok2)
                For Each ss As SingleSimulationBlok2 In msb.colSSimulations
                    colAllSS.Add(ss) ' SingleSimBlok
                Next
            ElseIf TypeOf o Is MultiSimulationValec2 Then
                msv = DirectCast(o, MultiSimulationValec2)
                colAllSS.Add(msv.SSV) ' SingleSimValec
            End If
        Next
        If colAllSS.Count > 0 Then
            ' Zoradime vsetky SS
            SortSingleSimulations(colAllSS)
            ' Zmazeme vsetko za MaxSim (nehavame len posledne alternativy od MaxSim)
            Dim Zvysok1 As Decimal, Zvysok2 As Decimal
            Dim Qty1 As Integer, Qty2 As Integer
            Dim OuterSizeValue1 As Integer, OuterSizeValue2 As Integer
            Dim sTmp As String = ""

            If colAllSS.Count > MaxSim Then
                GetSortDataFromSS(colAllSS(MaxSim), Zvysok1, Qty1, sTmp, OuterSizeValue1)  ' init udajov poslednej alternativy

                Dim M As Integer = colAllSS.Count
                Dim N As Integer = MaxSim + 1
                Dim i As Integer
                For i = M To N Step -1 ' odzadu musime
                    GetSortDataFromSS(colAllSS(i), Zvysok2, Qty2, sTmp, OuterSizeValue2)
                    If Not (Zvysok2 = Zvysok1 And Qty2 = Qty1 And OuterSizeValue1 = OuterSizeValue2) Then colAllSS.Remove(i) ' rovnake alternativy nehame
                Next
            End If
            ' Oznacime vsetky tie co chceme zobrazit + zotriedime MS
            Dim ssb As SingleSimulationBlok2
            Dim ssv As SingleSimulationValec2
            Dim colSortedMS As New Collection
            For Each o As Object In colAllSS
                If TypeOf o Is SingleSimulationBlok2 Then
                    ssb = DirectCast(o, SingleSimulationBlok2)
                    ssb.ShowSimulation = True
                    msb = DirectCast(colMs(ssb.MSKey), MultiSimulationBlok2)
                    If Not colSortedMS.Contains(msb.Key) Then colSortedMS.Add(msb, msb.Key) ' MS collection zoradena podla najlepsich vysledkov (zle sa sem nedostanu)
                ElseIf TypeOf o Is SingleSimulationValec2 Then
                    ssv = DirectCast(o, SingleSimulationValec2)
                    ssv.ShowSimulation = True
                    msv = DirectCast(colMs(ssv.MSKey), MultiSimulationValec2)
                    If Not colSortedMS.Contains(msv.Key) Then colSortedMS.Add(msv, msv.Key)
                End If
            Next
            If colSortedMS.Count > 0 Then colMs = colSortedMS
        End If
    End Sub

    Private Sub Run()
        Select Case _D3dOp
            Case D3dOperation.D3dOperation_ShowBlok
                'showBlok(1000, 10)
            Case D3dOperation.D3dOperation_ShowValec
                'showValec()
            Case D3dOperation.D3dOperation_SimBlok
                'SimulateBlok()
            Case D3dOperation.D3dOperation_SimValec
                'SimulateValec()
        End Select

    End Sub

End Class

' Riesi Item data pre Combobox, lstBox
Friend Class ValueDescriptionPair
    Friend ValueObj As Object
    Friend ValueText As String

    Friend Sub New(ByVal NewText As String, ByVal NewObj As Object)
        ValueText = NewText
        ValueObj = NewObj
    End Sub
    Public Overrides Function ToString() As String
        Return ValueText
    End Function

End Class
