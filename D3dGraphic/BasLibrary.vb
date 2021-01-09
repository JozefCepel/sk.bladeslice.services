Imports System.Data.SqlClient

Module BasLibrary
    Friend Const HKEY_MEMPHIS_NAME As String = "MemphisWin32_3"  ' HKCU\Software\VB and VBA Program Settings
    Friend Const HKEY_SECTION_NAME As String = "D3dGraphic"      ' Spolocne nastavenia
    Friend HKEY_SECTION_NAME_TYP As String                       ' Spolocne nastavenia pre dany typ
    Friend Const CS_DBL_FMT2 As String = "### ### ##0.00"

    Public sMjObjem As String = "dm3"
    Public sMjRozmery As String = "mm"
    Public iPocetMiestObjem As Integer = 4
    Public iKoefMjObjemRozmery As Integer = 1000

    Friend Enum Os_bod
        Os_aD = 1
        Os_bd = 2
        Os_L = 3
    End Enum

    Friend Enum CoordRank
        CoordRank1 = 1
        CoordRank2 = 2
    End Enum

    Friend Enum ValecType
        ValecType_Left = 1
        ValecType_Right = 2
        ValecType_Inner = 3
    End Enum

    Friend Function GetMaterialName(ByVal sSN As String, ByVal sSarza As String, ByVal dblSklCena As Decimal,
                                      ByVal sLocation As String, ByVal bDetailed As Boolean, ByVal sSizeDescription As String) As String
        Dim sTmp As String = ""

        If Trim(sSizeDescription) <> "" Then
            If sTmp <> "" Then sTmp &= ";  "
            If bDetailed Then sTmp &= "Vonkajšie vstupné rozmery: "
            sTmp &= sSizeDescription
        End If

        If Trim(sSarza) <> "" Then
            If sTmp <> "" Then sTmp &= ";  "
            If bDetailed Then sTmp &= "Šarža: "
            sTmp &= sSarza
        End If

        If bDetailed Then
            If sTmp <> "" Then sTmp &= ";  "
            If bDetailed Then sTmp &= "Skladová cena: "
            sTmp &= Format(dblSklCena, CS_DBL_FMT2) & " €"
        End If

        If Trim(sLocation) <> "" Then
            If sTmp <> "" Then sTmp &= ";  "
            If bDetailed Then sTmp &= "Umiestnenie: "
            sTmp &= sLocation
        End If

        'If sTmp <> "" Then sTmp &= ";  "
        'If bDetailed Then sTmp &= "Výrobné číslo/a: "
        'sTmp &= sSN

        Return sTmp
    End Function

    Friend Function testuj(ByVal b1 As CoordinatesBlok, ByVal b2 As CoordinatesBlok) As Boolean
        Return (((b1.a1 <= b2.a1 AndAlso b1.a2 <= b2.a1) OrElse (b1.a1 >= b2.a2 AndAlso b1.a2 >= b2.a2)) OrElse ((b1.L1 <= b2.L1 AndAlso b1.L2 <= b2.L1) _
                OrElse (b1.L1 >= b2.L2 AndAlso b1.L2 >= b2.L2)) OrElse ((b1.b1 <= b2.b1 AndAlso b1.b2 <= b2.b1) OrElse (b1.b1 >= b2.b2 AndAlso b1.b2 >= b2.b2)))
    End Function

    Friend Function IsExistKey(ByRef col As Collection, ByRef sKey As String) As Boolean
        Return col.Contains(sKey)
    End Function

    Friend Function GetThirdOs(ByVal FirstOs As Os_bod, ByVal SecondOs As Os_bod) As Os_bod
        If FirstOs = Os_bod.Os_aD Then
            Return IIf(SecondOs = Os_bod.Os_bd, Os_bod.Os_L, Os_bod.Os_bd)

        ElseIf FirstOs = Os_bod.Os_bd Then
            Return IIf(SecondOs = Os_bod.Os_L, Os_bod.Os_aD, Os_bod.Os_L)

        ElseIf FirstOs = Os_bod.Os_L Then
            Return IIf(SecondOs = Os_bod.Os_aD, Os_bod.Os_bd, Os_bod.Os_aD)
        End If

    End Function

    Friend Sub sortValce(ByRef shapes As ArrayList, ByVal zlava As Boolean)
        Dim i As Integer
        Dim i2 As Integer
        Dim ss As CoordinatesValec
        Dim ss2 As CoordinatesValec
        Dim temp As CoordinatesValec

        If zlava Then
            If (shapes.Count > 1) Then
                For i = 1 To shapes.Count - 1
                    For i2 = 1 To shapes.Count - i
                        ss = shapes.Item(i2)
                        ss2 = shapes.Item(i2 - 1)
                        If ss.L1 < ss2.L1 Then
                            temp = shapes.Item(i2)
                            shapes.Item(i2) = shapes.Item(i2 - 1)
                            shapes.Item(i2 - 1) = temp
                        End If
                    Next
                Next
            End If
        Else

            If (shapes.Count > 1) Then
                For i = 1 To shapes.Count - 1
                    For i2 = 1 To shapes.Count - i
                        ss = shapes.Item(i2)
                        ss2 = shapes.Item(i2 - 1)
                        If ss.L1 > ss2.L1 Then
                            temp = shapes.Item(i2)
                            shapes.Item(i2) = shapes.Item(i2 - 1)
                            shapes.Item(i2 - 1) = temp
                        End If
                    Next
                Next
            End If
        End If

    End Sub

    Friend Sub SortComputations(ByRef colAllCmps As Collection)
        Dim Zvysok1 As Decimal, Zvysok2 As Decimal
        Dim Qty1 As Integer, Qty2 As Integer
        Dim Popis1 As String = "", Popis2 As String = ""
        Dim OuterSizeValue1 As Integer, OuterSizeValue2 As Integer
        Dim colTmp As New Collection
        Dim bAdded As Boolean, i As Integer

        For Each o As Object In colAllCmps
            bAdded = False
            If colTmp.Count = 0 Then
                colTmp.Add(o)
                bAdded = True
            Else
                For i = 1 To colTmp.Count
                    If Not sortComputationGetData(o, Zvysok1, Qty1, Popis1, OuterSizeValue1) Then Exit For
                    If Not sortComputationGetData(colTmp(i), Zvysok2, Qty2, Popis2, OuterSizeValue2) Then Exit For
                    If (Zvysok1 = Zvysok2 And Qty1 = Qty2 And OuterSizeValue1 < OuterSizeValue2) Then
                        'Debug.Print("a")
                    End If
                    If (Zvysok1 < Zvysok2) Or (Zvysok1 = Zvysok2 And Qty1 > Qty2) Or (Zvysok1 = Zvysok2 And Qty1 = Qty2 And OuterSizeValue1 < OuterSizeValue2) Then
                        colTmp.Add(o, Popis1, i)
                        bAdded = True
                        Exit For
                    End If
                Next
            End If
            If Not bAdded Then colTmp.Add(o)
        Next
        colAllCmps = colTmp
    End Sub

    Private Function sortComputationGetData(ByRef oCmp As Object, ByRef Zvysok As Decimal, ByRef Qty As Integer, ByRef Popis As String, ByRef OuterSizeValue As Integer) As Boolean
        Dim cmpb As ComputationBlok
        Dim cmpv As ComputationValec

        If TypeOf oCmp Is ComputationBlok Then
            cmpb = DirectCast(oCmp, ComputationBlok)
            Zvysok = cmpb.ObjemOdrezku ' Vsetky computation su rovnake tak mozem pouzit current
            Qty = cmpb.Pocet
            Popis = cmpb.SourceObjShape.SN & cmpb.Popis
            OuterSizeValue = cmpb.OuterSizeFinalBod.aD + cmpb.OuterSizeFinalBod.bd + cmpb.OuterSizeFinalBod.L
            Return True
        ElseIf TypeOf oCmp Is ComputationValec Then
            cmpv = DirectCast(oCmp, ComputationValec)
            Zvysok = cmpv.ObjemOdrezku
            Qty = cmpv.Pocet
            Popis = cmpv.SourceObjShape.SN & cmpv.Popis
            OuterSizeValue = cmpv.OuterSizeFinalBod.aD + cmpv.OuterSizeFinalBod.bd + cmpv.OuterSizeFinalBod.L
            Return True
        End If
    End Function

    Friend Sub SortSingleSimulations(ByRef colAllSS As Collection)
        Dim Zvysok1 As Decimal, Zvysok2 As Decimal
        Dim Qty1 As Integer, Qty2 As Integer
        Dim Popis1 As String = "", Popis2 As String = ""
        Dim OuterSizeValue1 As Integer, OuterSizeValue2 As Integer
        Dim colTmp As New Collection
        Dim bAdded As Boolean, i As Integer

        For Each o As Object In colAllSS
            bAdded = False
            If colTmp.Count = 0 Then
                colTmp.Add(o)
                bAdded = True
            Else
                For i = 1 To colTmp.Count
                    If Not GetSortDataFromSS(o, Zvysok1, Qty1, Popis1, OuterSizeValue1) Then Exit For
                    If Not GetSortDataFromSS(colTmp(i), Zvysok2, Qty2, Popis2, OuterSizeValue2) Then Exit For
                    If (Zvysok1 = Zvysok2 And Qty1 = Qty2 And OuterSizeValue1 < OuterSizeValue2) Then
                        'Debug.Print("a")
                    End If
                    If (Zvysok1 < Zvysok2) Or (Zvysok1 = Zvysok2 And Qty1 > Qty2) Or (Zvysok1 = Zvysok2 And Qty1 = Qty2 And OuterSizeValue1 < OuterSizeValue2) Then
                        colTmp.Add(o, Popis1, i)
                        bAdded = True
                        Exit For
                    End If
                Next
            End If
            If Not bAdded Then colTmp.Add(o)
        Next
        colAllSS = colTmp
    End Sub

    Friend Function GetSortDataFromSS(ByRef oSS As Object, ByRef Zvysok As Decimal, ByRef Qty As Integer, ByRef Popis As String, ByRef OuterSizeValue As Integer) As Boolean
        Dim ssb As SingleSimulationBlok
        Dim ssv As SingleSimulationValec

        If TypeOf oSS Is SingleSimulationBlok Then
            ssb = DirectCast(oSS, SingleSimulationBlok)
            Zvysok = ssb.cmpCurrent.ObjemOdrezku ' Vsetky computation su rovnake tak mozem pouzit current
            Qty = ssb.cmpCurrent.Pocet
            Popis = ssb.cmpCurrent.SourceObjShape.SN & ssb.cmpCurrent.Popis
            OuterSizeValue = ssb.cmpCurrent.OuterSizeFinalBod.aD + ssb.cmpCurrent.OuterSizeFinalBod.bd + ssb.cmpCurrent.OuterSizeFinalBod.L
            Return True
        ElseIf TypeOf oSS Is SingleSimulationValec Then
            ssv = DirectCast(oSS, SingleSimulationValec)
            Zvysok = ssv.cmpCurrent.ObjemOdrezku
            Qty = ssv.cmpCurrent.Pocet
            Popis = ssv.cmpCurrent.SourceObjShape.SN & ssv.cmpCurrent.Popis
            OuterSizeValue = ssv.cmpCurrent.OuterSizeFinalBod.aD + ssv.cmpCurrent.OuterSizeFinalBod.bd + ssv.cmpCurrent.OuterSizeFinalBod.L
            Return True
        End If
    End Function

    Friend Sub del(ByVal b1 As CoordinatesBlok, ByVal b2 As CoordinatesBlok, ByRef _coordList As ArrayList)

        '/////////////////////XXXXXXXXXX
        If (b1.a1 < b2.a1) Then
            _coordList.Add(New CoordinatesBlok(b1.a1, b2.a1,
                                        b1.L1, b1.L2,
                                        b1.b1, b1.b2))
        End If

        If (b1.a2 > b2.a2) Then
            _coordList.Add(New CoordinatesBlok(b2.a2, b1.a2,
                                        b1.L1, b1.L2,
                                        b1.b1, b1.b2))
        End If

        '//zvysna cast kvadra s odstranenou vyseknutou castou
        Dim b4 As New CoordinatesBlok()
        If (b1.a1 > b2.a1) Then b4.a1 = b1.a1 Else b4.a1 = b2.a1
        If (b1.a2 < b2.a2) Then b4.a2 = b1.a2 Else b4.a2 = b2.a2
        b4.L1 = b1.L1
        b4.L2 = b1.L2
        b4.b1 = b1.b1
        b4.b2 = b1.b2
        b1 = b4

        '/////////////////////YYYYYYYY
        If (b1.L1 < b2.L1) Then
            _coordList.Add(New CoordinatesBlok(b1.a1, b1.a2,
                                        b1.L1, b2.L1,
                                        b1.b1, b1.b2))
        End If

        If (b1.L2 > b2.L2) Then
            _coordList.Add(New CoordinatesBlok(b1.a1, b1.a2,
                                        b2.L2, b1.L2,
                                        b1.b1, b1.b2))
        End If

        '//zvysna cast kvadra s odstranenou vyseknutou castou
        If (b1.L1 > b2.L1) Then b4.L1 = b1.L1 Else b4.L1 = b2.L1
        If (b1.L2 < b2.L2) Then b4.L2 = b1.L2 Else b4.L2 = b2.L2

        b1 = b4

        '////////////////////ZZZZZZZZZZZ
        If (b1.b1 < b2.b1) Then
            _coordList.Add(New CoordinatesBlok(b1.a1, b1.a2,
                                        b1.L1, b1.L2,
                                        b1.b1, b2.b1))
        End If

        If (b1.b2 > b2.b2) Then
            _coordList.Add(New CoordinatesBlok(b1.a1, b1.a2,
                                        b1.L1, b1.L2,
                                        b2.b2, b1.b2))
        End If

        '//zvysna cast kvadra s odstranenou vyseknutou castou, po konci=to co sekame
        If (b1.b1 > b2.b1) Then b4.b1 = b1.b1 Else b4.b1 = b2.b1
        If (b1.b2 < b2.b2) Then b4.b2 = b1.b2 Else b4.b2 = b2.b2

    End Sub

    Friend Sub otoc_abL(ByRef velkost As Coord_bod, ByVal index As Integer)
        Dim t As Integer
        Select Case index
            Case 1

            Case 2
                t = velkost.L
                velkost.L = velkost.aD
                velkost.aD = t

            Case 3
                t = velkost.bd
                velkost.bd = velkost.aD
                velkost.aD = t

            Case 4
                t = velkost.L
                velkost.L = velkost.bd
                velkost.bd = t

            Case 5
                t = velkost.L
                velkost.L = velkost.aD
                velkost.aD = t

                t = velkost.L
                velkost.L = velkost.bd
                velkost.bd = t

            Case 6
                t = velkost.L
                velkost.L = velkost.bd
                velkost.bd = t

                t = velkost.L
                velkost.L = velkost.aD
                velkost.aD = t

            Case -1

            Case -2
                t = velkost.L
                velkost.L = velkost.aD
                velkost.aD = t

            Case -3
                t = velkost.bd
                velkost.bd = velkost.aD
                velkost.aD = t

            Case -4
                t = velkost.L
                velkost.L = velkost.bd
                velkost.bd = t

            Case -5
                t = velkost.L
                velkost.L = velkost.bd
                velkost.bd = t

                t = velkost.L
                velkost.L = velkost.aD
                velkost.aD = t

            Case -6
                t = velkost.L
                velkost.L = velkost.aD
                velkost.aD = t

                t = velkost.L
                velkost.L = velkost.bd
                velkost.bd = t

        End Select


    End Sub

    Friend Sub CloseConnection(ByRef cnTmp As SqlConnection)
        If Not cnTmp Is Nothing Then
            If cnTmp.State = ConnectionState.Open Then cnTmp.Close()
            cnTmp.Dispose()
        End If
    End Sub

    Public Function GetValecObjem(ByVal array As ArrayList) As Decimal
        Dim i As Integer

        Dim temp As New CoordinatesValec
        Dim objem As Decimal = 0

        For i = 0 To array.Count - 1

            temp = array.Item(i)
            objem = objem + (Math.PI * (temp.D1 / 2) * (temp.D1 / 2) * (temp.L2 - temp.L1)) -
                          (Math.PI * (temp.d2 / 2) * (temp.d2 / 2) * (temp.L2 - temp.L1))

        Next

        Return Math.Round(objem / iKoefMjObjemRozmery, iPocetMiestObjem)

    End Function

    Public Function GetBlokObjem(ByVal array As ArrayList) As Decimal

        Dim Objem As Decimal = 0
        Dim temp As CoordinatesBlok
        For i As Integer = 0 To array.Count - 1
            temp = array.Item(i)
            Objem += (Math.Abs(temp.a2 - temp.a1) * Math.Abs(temp.L2 - temp.L1) * Math.Abs(temp.b2 - temp.b1))
        Next

        Return Math.Round(Objem / iKoefMjObjemRozmery, iPocetMiestObjem)

    End Function

    Public Sub OptimiseValec(ByRef coordL As ArrayList)

        If coordL.Count = 0 Then Exit Sub
        sortValce(coordL, True)

        Dim temp As CoordinatesValec = coordL.Item(0)
        Dim temp0 As CoordinatesValec
        Dim i As Integer = 1

        If (coordL.Count > 1) Then
            While i < coordL.Count
                temp0 = coordL.Item(i)
                If (temp0.D1 = temp.D1 And temp0.d2 = temp.d2) Then
                    temp.L2 = temp0.L2
                    coordL.RemoveAt(i)
                Else
                    temp = coordL.Item(i)
                    i += 1
                End If
            End While
        End If

    End Sub

    Public Function MyArrayClone(ByRef SourceArray As ArrayList) As ArrayList
        Dim cl As New ArrayList
        Dim i As Integer
        For i = 0 To SourceArray.Count - 1
            If TypeOf SourceArray(i) Is CoordinatesBlok Then
                cl.Add(New CoordinatesBlok(CType(SourceArray(i), CoordinatesBlok)))
            ElseIf TypeOf SourceArray(i) Is CoordinatesValec Then
                cl.Add(New CoordinatesValec(CType(SourceArray(i), CoordinatesValec)))
            End If
        Next
        Return cl
    End Function

    Friend Function RoundItNice(ByVal decValue As Decimal) As String
        Dim sRoundFmt As String
        If iPocetMiestObjem = 0 Then
            sRoundFmt = "0"
        Else
            sRoundFmt = "0." & StrDup(iPocetMiestObjem, "0")
        End If

        decValue = CDec(Format(decValue, sRoundFmt))
        If CLng(decValue) = decValue Then
            RoundItNice = LTrim(Format(decValue, "### ### ##0"))
        Else
            RoundItNice = LTrim(Format(decValue, "### ### ##0.00##"))
        End If

    End Function

    Friend Function ExistFieldInRs(ByRef rsCheck As ADODB.Recordset, ByRef strFieldName As String) As Boolean
        Dim oField As Object
        strFieldName = strFieldName.ToLower
        For Each oField In rsCheck.Fields
            If oField.Name.ToLower = strFieldName Then Return True
        Next
    End Function

    Friend Function SizeDescriptionBlok(bod As Coord_bod) As String

        If bod.aD = 0 And bod.bd = 0 And bod.L = 0 Then
            Return " - "
        Else
            Return CInt(bod.aD) & " x " & CInt(bod.bd) & " x " & CInt(bod.L)
        End If

    End Function

    Friend Function SizeDescriptionValec(bod As Coord_bod) As String
        If bod.aD = 0 And bod.bd = 0 And bod.L = 0 Then
            Return " - "
        Else
            Return CInt(bod.aD / 2) & IIf(bod.bd <> 0, " / " & CInt(bod.bd / 2), "") & " x " & CInt(bod.L)
        End If
    End Function

    Friend Function GetValecSize(coordList As ArrayList) As Coord_bod
        Dim i As Integer
        Dim v As New Coord_bod With {
                .aD = 0,
                .bd = 0,
                .L = 0
            }

        Dim temp As New CoordinatesValec

        For i = 0 To coordList.Count - 1
            temp = coordList.Item(i)

            If i = 0 Then
                v.bd = temp.d2 * 2  'Inicializacia
            End If


            If temp.D1 * 2 > v.aD Then v.aD = temp.D1 * 2
            If temp.L2 > v.L Then v.L = temp.L2
            If temp.d2 * 2 < v.bd Then v.bd = temp.d2 * 2

        Next

        Return v
    End Function

    Friend Function getBlokSizeFromZero(ByRef cl As ArrayList) As Coord_bod
        Dim i As Integer
        Dim v As New Coord_bod With {
                .aD = 0,
                .L = 0,
                .bd = 0
            }

        Dim temp As New CoordinatesBlok

        For i = 0 To cl.Count - 1

            temp = cl.Item(i)

            If temp.a2 > v.aD Then v.aD = temp.a2
            If temp.L2 > v.L Then v.L = temp.L2
            If temp.b2 > v.bd Then v.bd = temp.b2

        Next

        Return v
    End Function

    Friend Function getBlokSizeMinimum(ByRef cl As ArrayList) As Coord_bod
        Dim i As Integer
        Dim temp As CoordinatesBlok
        Dim v2 As New Coord_bod(0, 0, 0)
        Dim v1 As New Coord_bod(0, 0, 0)

        If cl.Count > 0 Then
            temp = New CoordinatesBlok(cl.Item(0))
            v1 = New Coord_bod(temp.a1, temp.L1, temp.b1)
            v2 = New Coord_bod(temp.a2, temp.L2, temp.b2)

            For i = 1 To cl.Count - 1
                temp = cl.Item(i)

                If temp.a2 > v2.aD Then v2.aD = temp.a2
                If temp.L2 > v2.L Then v2.L = temp.L2
                If temp.b2 > v2.bd Then v2.bd = temp.b2

                If temp.a1 < v1.aD Then v1.aD = temp.a1
                If temp.L1 < v1.L Then v1.L = temp.L1
                If temp.b1 < v1.bd Then v1.bd = temp.b1

            Next

        End If

        Return New Coord_bod(v2.aD - v1.aD, v2.L - v1.L, v2.bd - v1.bd)
    End Function

End Module
