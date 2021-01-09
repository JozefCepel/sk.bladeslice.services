Friend Class ComputationBlok

  Friend SourceObjShape As BlokSourceObj
  Private velkostObj As New Coord_bod
  Friend CelyRez As ArrayList
  Friend FoundRezList As ArrayList

  Friend bDoNotDraw As Boolean = False
  Friend roh As Byte
  Private maxRez As Integer
  Private riadky As Byte
  Private otocenie As Byte
  Friend strana As Byte
  Private Rozmery As Coord_bod
  Private _Popis As String = ""
  Private _ObjemVyrez As Decimal = 0
  Friend defaultRezSmer As Integer
  Friend maxPocet As Integer = 0
  Private thres As Decimal
  Private vyrezInfo As CoordinatesBlok
  Private _OuterSizeFinalBod As Coord_bod

  Friend Sub Recompute(ByVal iPocKS As Integer, ByVal stranaA As Boolean, ByVal stranaB As Boolean, ByVal stranaL As Boolean)
    Compute(SourceObjShape, roh, riadky, otocenie, strana, Rozmery, maxRez, iPocKS, thres, 0, stranaA, stranaB, stranaL)
  End Sub

  Friend Sub Compute(ByRef oSourceObjShape As BlokSourceObj, _
                     ByVal iRoh As Byte, ByVal iRiadky As Byte, ByVal iOtocenie As Byte, ByVal iStrana As Byte, _
                     ByVal abLRozmery As Coord_bod, ByVal mRez As Decimal, ByVal iPocKS As Integer, ByVal thr As Decimal, Optional ByVal first As Boolean = 1, _
                     Optional ByVal stranaA As Boolean = False, Optional ByVal stranaB As Boolean = False, Optional ByVal stranaL As Boolean = False)
    Dim b As CoordinatesBlok

    thres = thr
    maxRez = mRez
    roh = iRoh
    riadky = iriadky
    otocenie = iotocenie
    strana = iStrana
    Rozmery = abLRozmery
    Dim alternativy As New ArrayList

    SourceObjShape = oSourceObjShape.Clone
    CelyRez = New ArrayList
    FoundRezList = New ArrayList

    Dim offsetSize As Coord_bod = SourceObjShape.getBlokOffset()
    otocOffset(offsetSize, roh)

    otoc(SourceObjShape.coordListInv, roh)
    otoc(SourceObjShape.coordList, roh)
    velkostObj = SourceObjShape.Size(True)

    Select Case strana
      Case 1
        b = FindStartAtMinY(SourceObjShape.coordList, abLRozmery, 0, riadky)
      Case 2
        b = FindStartAtMinX(SourceObjShape.coordList, abLRozmery, 0, riadky)
      Case Else 'iba 3 pripada do uvahy
        b = FindStartAtMinZ(SourceObjShape.coordList, abLRozmery, 0, riadky)
    End Select

    If (b.a1 < velkostObj.aD And b.L1 < velkostObj.L And b.b1 < velkostObj.bd) Then
      Dim c As New CoordinatesBlok(b.a1, b.a1 + abLRozmery.aD, _
                                   b.L1, b.L1 + abLRozmery.L, _
                                   b.b1, b.b1 + abLRozmery.bd)

      FoundRezList.Add(c)
      Dim more As Boolean = False

      If iPocKS - 1 > 0 Then
        Select Case strana
          Case 1
            chod_po_xz(c, iPocKS - 1, riadky, alternativy, more)
          Case 2
            chod_po_yz(c, iPocKS - 1, riadky, alternativy, more)
          Case 3
            chod_po_xy(c, iPocKS - 1, riadky, alternativy, more)
        End Select
      End If

    End If
    If Me.Pocet > 0 Then

      Dim rez As CoordinatesBlok
      If first Then

        maxPocet = FoundRezList.Count
        rez = vyrobRez(FoundRezList, riadky, strana)
        vyrezInfo = New CoordinatesBlok(rez)
        Dim tempA As New ArrayList
        tempA.Add(vyrezInfo)
        otoc(tempA, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)
        CelyRez.Add(rez)
        vycistiRez(CelyRez)
        otoc(CelyRez, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)
        Dim porovnanie As Decimal
        Dim objKvadra As Decimal
        objKvadra = Rozmery.aD * Rozmery.bd * Rozmery.L
        porovnanie = (GetBlokObjem(CelyRez) + objKvadra) / (objKvadra * FoundRezList.Count)
        Dim trez As New CoordinatesBlok
        Dim trezy As New ArrayList
        Dim tcoordlist As New ArrayList
        tcoordlist = FoundRezList.Clone

        If roh = 7 And strana = 2 And otocenie = 2 And riadky = 1 Then
          Dim sdda As Integer = 1
        End If

        Dim threshold As Decimal = thr / 100

        If (alternativy.Count > 0) Then

          Dim i1 As Integer = 0
          If (alternativy.Count > 1) Then
            i1 = (alternativy.Item(alternativy.Count - 2) - alternativy.Item(alternativy.Count - 1))
          Else
            If (alternativy.Count > 0) Then
              i1 = (iPocKS - alternativy.Item(alternativy.Count - 1))
            End If
          End If

          Dim i2 As Integer = (FoundRezList.Count - (iPocKS - alternativy.Item(alternativy.Count - 1)))

          If (i2 > 0) Then
            If (i1 * threshold > i2) Then
              FoundRezList.RemoveRange(iPocKS - alternativy.Item(alternativy.Count - 1), FoundRezList.Count - iPocKS + alternativy.Item(alternativy.Count - 1))
              trez = vyrobRez(FoundRezList, riadky, strana)
              vyrezInfo = New CoordinatesBlok(trez)
              Dim tempB As New ArrayList
              tempB.Add(vyrezInfo)
              otoc(tempB, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)
              trezy.Clear()
              trezy.Add(trez)
              vycistiRez(trezy)
              otoc(trezy, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)
              CelyRez = trezy
            End If
          End If
        End If


        otoc(FoundRezList, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)
      Else
        rez = recomputeRez(FoundRezList, riadky, strana, stranaA, stranaB, stranaL)
        vyrezInfo = New CoordinatesBlok(rez)
        Dim tempA As New ArrayList
        tempA.Add(vyrezInfo)
        otoc(tempA, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)
        CelyRez.Add(rez)
        vycistiRez(CelyRez)
        otoc(CelyRez, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)
        otoc(FoundRezList, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)

      End If

      _ObjemVyrez = GetBlokObjem(CelyRez)
      _Popis = "roh " + roh.ToString + " | " + IIf(riadky = 1, "stĺpce", "riadky") + " | strana " + strana.ToString + " | otočenie " + otocenie.ToString

      'Blade ******** povodne od MT 17.5.2012
      'If (rez.a2 - rez.a1) > mRez OR (rez.b2 - rez.b1) > mRez Or (rez.L2 - rez.L1) > mRez Then
      '  FoundRezList.Clear()
      'End If

      'Blade ******** aktualne 17.5.2012 - potrebujem zistit ci nie je rez vacsi ako MAX      
      Dim bRezA As Boolean = (stranaA Or defaultRezSmer = 1) And (vyrezInfo.a2 - vyrezInfo.a1) <= maxRez
      Dim bRezB As Boolean = (stranaB Or defaultRezSmer = 2) And (vyrezInfo.b2 - vyrezInfo.b1) <= maxRez
      Dim bRezL As Boolean = (stranaL Or defaultRezSmer = 3) And (vyrezInfo.L2 - vyrezInfo.L1) <= maxRez

      If Not bRezA And Not bRezB And Not bRezL Then
        FoundRezList.Clear()
      End If

    End If

    otoc(SourceObjShape.coordListInv, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)
    otoc(SourceObjShape.coordList, -roh, offsetSize.aD, offsetSize.bd, offsetSize.L)

    If Me.Pocet > 0 Then

    End If

    Dim remove As ArrayList = SourceObjShape.coordList.Clone
    For i As Integer = 0 To CelyRez.Count - 1
      vymazCustom(remove, CelyRez(i))
    Next
    _OuterSizeFinalBod = getBlokSizeMinimum(remove)

    SourceObjShape = oSourceObjShape.Clone

  End Sub

  Friend ReadOnly Property OuterSizeFinalBod() As Coord_bod
    Get
      Return _OuterSizeFinalBod
    End Get
  End Property

  Friend ReadOnly Property OuterSizeFinal() As String
    Get
      Return SizeDescriptionBlok(OuterSizeFinalBod)
    End Get
  End Property

  Friend ReadOnly Property Popis() As String
    Get
      Return _Popis
    End Get
  End Property

  Friend ReadOnly Property rezPilou() As CoordinatesBlok
    Get
      Return vyrezInfo
    End Get
  End Property

  Friend ReadOnly Property ObjemVyrezu() As Decimal
    Get
      Return _ObjemVyrez
    End Get
  End Property

  Friend ReadOnly Property ObjemOdrezku() As Decimal
    Get
      Return Me.ObjemVyrezu - Me.ObjemPolotovaru
    End Get
  End Property

  Friend ReadOnly Property ObjemPolotovaru() As Decimal
    Get
      Return Math.Round(Me.Pocet * Rozmery.aD * Rozmery.L * Rozmery.bd / iKoefMjObjemRozmery, iPocetMiestObjem)
    End Get
  End Property

  Friend ReadOnly Property Pocet() As Integer
    Get
      Return FoundRezList.Count
    End Get
  End Property

  Private Sub vycistiRez(ByRef co As ArrayList)
    For i As Integer = 0 To SourceObjShape.coordListInv.Count - 1
      vymazCustom(co, SourceObjShape.coordListInv(i))
    Next
  End Sub

  Friend Sub vymazCustom(ByRef co As ArrayList, ByVal b As CoordinatesBlok)
    Dim i As Integer = co.Count()
    Dim u As Integer = 0

    Dim b2 As New CoordinatesBlok()
    For c As Integer = 0 To i - 1
      b2 = co.Item(u)
      If Not testuj(b, b2) Then
        del(b2, b, co)
        co.RemoveAt(u)
      Else : u = u + 1
      End If
    Next

  End Sub

  Private Function otocTypStrany(ByVal typ0 As Integer) As Integer
    Dim index As Integer = typ0
    Select Case roh
      Case 1

      Case 2
        Select Case typ0
          Case 1
            index = 2
          Case 2
            index = 1
        End Select
      Case 3

      Case 4
        Select Case typ0
          Case 1
            index = 2
          Case 2
            index = 1
        End Select

      Case 5
        Select Case typ0
          Case 1
            index = 2
          Case 2
            index = 1
        End Select

      Case 6


      Case 7
        Select Case typ0
          Case 1
            index = 2
          Case 2
            index = 1
        End Select


      Case 8

    End Select

    Return index
  End Function

  Private Sub otocBooleanStran(ByRef typA As Boolean, ByRef typB As Boolean)

    Select Case roh
      Case 1

      Case 2
        Dim t As Boolean = typA
        typA = typB
        typB = t

      Case 3

      Case 4
        Dim t As Boolean = typA
        typA = typB
        typB = t

      Case 5
        Dim t As Boolean = typA
        typA = typB
        typB = t

      Case 6


      Case 7
        Dim t As Boolean = typA
        typA = typB
        typB = t


      Case 8

    End Select

  End Sub

  Private Function vyrobRez(ByVal coordList As ArrayList, ByVal riadok As Integer, ByVal strana As Byte) As CoordinatesBlok

    Dim rez As New CoordinatesBlok(coordList.Item(0))
    Dim temp As New CoordinatesBlok

    For i As Integer = 1 To coordList.Count - 1

      temp = coordList.Item(i)

      If rez.a1 > temp.a1 Then rez.a1 = temp.a1
      If rez.L1 > temp.L1 Then rez.L1 = temp.L1
      If rez.b1 > temp.b1 Then rez.b1 = temp.b1
      If rez.a2 < temp.a2 Then rez.a2 = temp.a2
      If rez.L2 < temp.L2 Then rez.L2 = temp.L2
      If rez.b2 < temp.b2 Then rez.b2 = temp.b2

    Next

    Select Case strana
      Case 1
        Select Case riadok
          Case 1
            rez.b1 = 0
            rez.b2 = velkostObj.bd
            defaultRezSmer = otocTypStrany(2)
            rez.a1 = 0
            rez.L1 = 0
          Case 2
            rez.a1 = 0
            rez.a2 = velkostObj.aD
            defaultRezSmer = otocTypStrany(1)
            rez.L1 = 0
            rez.b1 = 0
        End Select
      Case 2
        Select Case riadok
          Case 1
            rez.L1 = 0
            rez.L2 = velkostObj.L
            defaultRezSmer = otocTypStrany(3)
            rez.a1 = 0
            rez.b1 = 0
          Case 2
            rez.b1 = 0
            rez.b2 = velkostObj.bd
            defaultRezSmer = otocTypStrany(2)
            rez.a1 = 0
            rez.L1 = 0
        End Select
      Case 3
        Select Case riadok
          Case 1
            rez.L1 = 0
            rez.L2 = velkostObj.L
            defaultRezSmer = otocTypStrany(3)
            rez.b1 = 0
            rez.a1 = 0
          Case 2
            rez.a1 = 0
            rez.a2 = velkostObj.aD
            defaultRezSmer = otocTypStrany(1)
            rez.b1 = 0
            rez.L1 = 0
        End Select
    End Select

    Return rez

  End Function

  Private Function recomputeRez(ByVal coordList As ArrayList, ByVal riadok As Integer, ByVal strana As Byte, _
                        ByVal stranaA As Boolean, ByVal stranaB As Boolean, ByVal stranaL As Boolean) As CoordinatesBlok

    otocBooleanStran(stranaA, stranaB)

    Dim rez As New CoordinatesBlok(coordList.Item(0))
    Dim temp As New CoordinatesBlok

    For i As Integer = 1 To coordList.Count - 1

      temp = coordList.Item(i)

      If rez.a1 > temp.a1 Then rez.a1 = temp.a1
      If rez.L1 > temp.L1 Then rez.L1 = temp.L1
      If rez.b1 > temp.b1 Then rez.b1 = temp.b1
      If rez.a2 < temp.a2 Then rez.a2 = temp.a2
      If rez.L2 < temp.L2 Then rez.L2 = temp.L2
      If rez.b2 < temp.b2 Then rez.b2 = temp.b2

    Next

    Select Case strana
      Case 1
        Select Case riadok
          Case 1 : rez.b1 = 0
            rez.b2 = velkostObj.bd
            rez.a1 = 0
            rez.L1 = 0
            If stranaA Then rez.a2 = velkostObj.aD
            If stranaL Then rez.L2 = velkostObj.L
          Case 2 : rez.a1 = 0
            rez.a2 = velkostObj.aD
            rez.L1 = 0
            rez.b1 = 0
            If stranaB Then rez.b2 = velkostObj.bd
            If stranaL Then rez.L2 = velkostObj.L
        End Select
      Case 2
        Select Case riadok
          Case 1 : rez.L1 = 0
            rez.a1 = 0
            rez.L2 = velkostObj.L
            rez.b1 = 0
            If stranaA Then rez.a2 = velkostObj.aD
            If stranaB Then rez.b2 = velkostObj.bd
          Case 2 : rez.b1 = 0
            rez.a1 = 0
            rez.b2 = velkostObj.bd
            rez.L1 = 0
            If stranaA Then rez.a2 = velkostObj.aD
            If stranaL Then rez.L2 = velkostObj.L
        End Select
      Case 3
        Select Case riadok
          Case 1 : rez.L1 = 0
            rez.b1 = 0
            rez.L2 = velkostObj.L
            rez.a1 = 0
            If stranaA Then rez.a2 = velkostObj.aD
            If stranaB Then rez.b2 = velkostObj.bd
          Case 2 : rez.a1 = 0
            rez.b1 = 0
            rez.a2 = velkostObj.aD
            rez.L1 = 0
            If stranaB Then rez.b2 = velkostObj.bd
            If stranaL Then rez.L2 = velkostObj.L
        End Select
    End Select

    Return rez

  End Function

  Private Function FindStartAtMinX(ByVal array As ArrayList, ByVal velkost As Coord_bod, ByVal minX As Decimal, ByVal bX2 As Integer) As CoordinatesBlok

    Dim tmp As New Coord_bod(velkostObj)
    Dim c As Integer
    Dim i As Integer = array.Count

    Dim tmp2 As CoordinatesBlok
    Dim testcoord As New CoordinatesBlok()

    For c = 0 To i - 1
      tmp2 = array.Item(c)
      If tmp2.a1 <= minX Then

        If (tmp2.b1 <= tmp.bd And tmp2.L1 = tmp.L Or tmp2.L1 < tmp.L) And bX2 = 2 Or _
           (tmp2.L1 <= tmp.L And tmp2.b1 = tmp.bd Or tmp2.b1 < tmp.bd) And bX2 = 1 Then

          testcoord.a1 = minX
          testcoord.a2 = minX + velkost.aD
          testcoord.L1 = tmp2.L1
          testcoord.L2 = tmp2.L1 + velkost.L
          testcoord.b1 = tmp2.b1
          testcoord.b2 = tmp2.b1 + velkost.bd

          If Not je_von_start(testcoord, SourceObjShape.coordListInv) Then
            tmp.aD = testcoord.a1
            tmp.L = testcoord.L1
            tmp.bd = testcoord.b1
          End If

        End If
      End If
    Next

    If tmp.aD = velkostObj.aD Then

      For c = 0 To i - 1
        tmp2 = array.Item(c)
        If (tmp2.a1 >= minX And (tmp2.b1 <= tmp.bd And tmp2.L1 = tmp.L Or tmp2.L1 < tmp.L) And bX2 = 2) Or _
           (tmp2.a1 >= minX And (tmp2.L1 <= tmp.L And tmp2.b1 = tmp.bd Or tmp2.b1 < tmp.bd) And bX2 = 1) Then
          testcoord.a1 = tmp2.a1
          testcoord.a2 = tmp2.a1 + velkost.aD
          testcoord.L1 = tmp2.L1
          testcoord.L2 = tmp2.L1 + velkost.L
          testcoord.b1 = tmp2.b1
          testcoord.b2 = tmp2.b1 + velkost.bd

          If Not je_von_start(testcoord, SourceObjShape.coordListInv) Then
            tmp.aD = testcoord.a1
            tmp.L = testcoord.L1
            tmp.bd = testcoord.b1
          End If

        End If
      Next

    End If

    Return New CoordinatesBlok(tmp.aD, tmp.aD + velkost.aD, tmp.L, tmp.L + velkost.L, tmp.bd, tmp.bd + velkost.bd)

  End Function

  Private Function FindStartAtMinY(ByVal array As ArrayList, ByVal velkost As Coord_bod, ByVal minY As Decimal, ByVal bY2 As Integer) As CoordinatesBlok

    Dim tmp As New Coord_bod(velkostObj)
    Dim c As Integer
    Dim i As Integer
    i = array.Count
    Dim tmp2 As CoordinatesBlok
    Dim testcoord As New CoordinatesBlok()

    For c = 0 To i - 1
      tmp2 = array.Item(c)
      If tmp2.L1 <= minY Then

        If (tmp2.a1 <= tmp.aD And tmp2.b1 = tmp.bd Or tmp2.b1 < tmp.bd) And bY2 = 2 Or _
           (tmp2.b1 <= tmp.bd And tmp2.a1 = tmp.aD Or tmp2.a1 < tmp.aD) And bY2 = 1 Then
          testcoord.a1 = tmp2.a1
          testcoord.a2 = tmp2.a1 + velkost.aD
          testcoord.L1 = minY
          testcoord.L2 = minY + velkost.L
          testcoord.b1 = tmp2.b1
          testcoord.b2 = tmp2.b1 + velkost.bd

          If Not je_von_start(testcoord, SourceObjShape.coordListInv) Then
            tmp.aD = testcoord.a1
            tmp.L = testcoord.L1
            tmp.bd = testcoord.b1
          End If

        End If
      End If
    Next

    If tmp.aD = velkostObj.aD Then

      For c = 0 To i - 1
        tmp2 = array.Item(c)
        If (tmp2.L1 >= minY And (tmp2.a1 <= tmp.aD And tmp2.b1 = tmp.bd Or tmp2.b1 < tmp.bd) And bY2 = 2) Or _
           (tmp2.L1 >= minY And (tmp2.b1 <= tmp.bd And tmp2.a1 = tmp.aD Or tmp2.a1 < tmp.aD) And bY2 = 1) Then
          testcoord.a1 = tmp2.a1
          testcoord.a2 = tmp2.a1 + velkost.aD
          testcoord.L1 = tmp2.L1
          testcoord.L2 = tmp2.L1 + velkost.L
          testcoord.b1 = tmp2.b1
          testcoord.b2 = tmp2.b1 + velkost.bd

          If Not je_von_start(testcoord, SourceObjShape.coordListInv) Then
            tmp.aD = testcoord.a1
            tmp.L = testcoord.L1
            tmp.bd = testcoord.b1
          End If

        End If
      Next

    End If

    Return New CoordinatesBlok(tmp.aD, tmp.aD + velkost.aD, tmp.L, tmp.L + velkost.L, tmp.bd, tmp.bd + velkost.bd)

  End Function

  Private Function FindStartAtMinZ(ByVal array As ArrayList, ByVal velkost As Coord_bod, ByVal minZ As Decimal, ByVal bZ2 As Integer) As CoordinatesBlok

    Dim tmp As New Coord_bod(velkostObj)
    Dim tmp2 As CoordinatesBlok
    Dim c As Integer
    Dim i As Integer
    i = array.Count
    Dim testcoord As New CoordinatesBlok()

    For c = 0 To i - 1
      tmp2 = array.Item(c)
      If tmp2.b1 <= minZ Then

        If (tmp2.a1 <= tmp.aD And tmp2.L1 = tmp.L Or tmp2.L1 < tmp.L) And bZ2 = 2 Or _
           (tmp2.L1 <= tmp.L And tmp2.a1 = tmp.aD Or tmp2.a1 < tmp.aD) And bZ2 = 1 Then
          testcoord.a1 = tmp2.a1
          testcoord.a2 = tmp2.a1 + velkost.aD
          testcoord.L1 = tmp2.L1
          testcoord.L2 = tmp2.L1 + velkost.L
          testcoord.b1 = minZ
          testcoord.b2 = minZ + velkost.bd

          If Not je_von_start(testcoord, SourceObjShape.coordListInv) Then
            tmp.aD = testcoord.a1
            tmp.L = testcoord.L1
            tmp.bd = testcoord.b1
          End If

        End If
      End If
    Next

    If tmp.aD = velkostObj.aD Then

      For c = 0 To i - 1
        tmp2 = array.Item(c)
        If (tmp2.b1 >= minZ And (tmp2.a1 <= tmp.aD And tmp2.L1 = tmp.L Or tmp2.L1 < tmp.L) And bZ2 = 2) Or _
           (tmp2.b1 >= minZ And (tmp2.L1 <= tmp.L And tmp2.a1 = tmp.aD Or tmp2.a1 < tmp.aD) And bZ2 = 1) Then
          testcoord.a1 = tmp2.a1
          testcoord.a2 = tmp2.a1 + velkost.aD
          testcoord.L1 = tmp2.L1
          testcoord.L2 = tmp2.L1 + velkost.L
          testcoord.b1 = tmp2.b1
          testcoord.b2 = tmp2.b1 + velkost.bd

          If Not je_von_start(testcoord, SourceObjShape.coordListInv) Then
            tmp.aD = testcoord.a1
            tmp.L = testcoord.L1
            tmp.bd = testcoord.b1
          End If

        End If
      Next

    End If

    Return New CoordinatesBlok(tmp.aD, tmp.aD + velkost.aD, tmp.L, tmp.L + velkost.L, tmp.bd, tmp.bd + velkost.bd)

  End Function

  Private Sub chod_po_xy(ByVal predch As CoordinatesBlok, ByVal este As Integer, ByVal Os As Integer, ByRef alternativy As ArrayList, ByRef more As Boolean)

    Dim velkost As New Coord_bod(predch.a2 - predch.a1, _
                           predch.L2 - predch.L1, _
                           predch.b2 - predch.b1)

    Dim teraz As New CoordinatesBlok(predch)

    Select Case Os
      Case 2
        teraz.a1 = predch.a2
        teraz.a2 = predch.a2 + velkost.aD
      Case 1
        teraz.L1 = predch.L2
        teraz.L2 = predch.L2 + velkost.L
    End Select

    If Not je_von(teraz, SourceObjShape.coordListInv, FoundRezList) Then
      FoundRezList.Add(teraz)
      este = este - 1
      If este > 0 Then chod_po_xy(teraz, este, Os, alternativy, more)

    Else

      Dim tmp As New CoordinatesBlok()
      Select Case Os
        Case 2
          tmp = FindStartAtMin_abL(SourceObjShape.coordList, velkost, teraz.L2, teraz.b1, Os_bod.Os_bd, Os_bod.Os_L)
        Case 1
          tmp = FindStartAtMin_abL(SourceObjShape.coordList, velkost, teraz.a2, teraz.b1, Os_bod.Os_aD, Os_bod.Os_L)
      End Select

      If Not tmp.a1 = velkostObj.aD Then
        FoundRezList.Add(tmp)
        este = este - 1
        If Not more Then alternativy.Add(este + 1)
        If este > 0 Then
          chod_po_xy(tmp, este, Os, alternativy, more)
        End If


      Else

        tmp = FindStartAtMinZ(SourceObjShape.coordList, velkost, teraz.b1 + velkost.bd, Os)

        If Not tmp.a1 = velkostObj.aD Then
          FoundRezList.Add(tmp)
          este = este - 1
          If Not more Then
            more = True
            alternativy.Clear()
          End If
          alternativy.Add(este + 1)

          If este > 0 Then
            chod_po_xy(tmp, este, Os, alternativy, more)
          End If
        End If

      End If
    End If

  End Sub

  Private Sub chod_po_xz(ByVal predch As CoordinatesBlok, ByVal este As Integer, ByVal Os As Integer, ByRef alternativy As ArrayList, ByRef more As Boolean)

    Dim velkost As New Coord_bod(predch.a2 - predch.a1, _
                           predch.L2 - predch.L1, _
                           predch.b2 - predch.b1)

    Dim teraz As New CoordinatesBlok(predch)

    Select Case Os
      Case 2
        teraz.a1 = predch.a2
        teraz.a2 = predch.a2 + velkost.aD

      Case 1
        teraz.b1 = predch.b2
        teraz.b2 = predch.b2 + velkost.bd
    End Select

    If Not je_von(teraz, SourceObjShape.coordListInv, FoundRezList) Then
      FoundRezList.Add(teraz)
      este = este - 1
      If este > 0 Then chod_po_xz(teraz, este, Os, alternativy, more)

    Else

      Dim tmp As New CoordinatesBlok()

      Select Case Os
        Case 2
          tmp = FindStartAtMin_abL(SourceObjShape.coordList, velkost, teraz.b2, teraz.L1, Os_bod.Os_L, Os_bod.Os_bd)
        Case 1
          tmp = FindStartAtMin_abL(SourceObjShape.coordList, velkost, teraz.a2, teraz.L1, Os_bod.Os_aD, Os_bod.Os_bd)
      End Select

      If Not tmp.a1 = velkostObj.aD Then
        FoundRezList.Add(tmp)
        este = este - 1
        If Not more Then alternativy.Add(este + 1)
        If este > 0 Then
          chod_po_xz(tmp, este, Os, alternativy, more)
        End If
      Else


        tmp = FindStartAtMinY(SourceObjShape.coordList, velkost, teraz.L1 + velkost.L, Os)

        If Not tmp.a1 = velkostObj.aD Then
          FoundRezList.Add(tmp)
          este = este - 1

          If Not more Then
            more = True
            alternativy.Clear()
          End If
          alternativy.Add(este + 1)

          If este > 0 Then
            chod_po_xz(tmp, este, Os, alternativy, more)
          End If
        End If

      End If
    End If

  End Sub

  Private Sub chod_po_yz(ByVal predch As CoordinatesBlok, ByVal este As Integer, ByVal Os As Integer, ByRef alternativy As ArrayList, ByRef more As Boolean)

    Dim velkost As New Coord_bod(predch.a2 - predch.a1, _
                           predch.L2 - predch.L1, _
                           predch.b2 - predch.b1)

    Dim teraz As New CoordinatesBlok(predch)

    Select Case Os
      Case 1
        teraz.L1 = predch.L2
        teraz.L2 = predch.L2 + velkost.L
      Case 2
        teraz.b1 = predch.b2
        teraz.b2 = predch.b2 + velkost.bd
    End Select

    If Not je_von(teraz, SourceObjShape.coordListInv, FoundRezList) Then
      FoundRezList.Add(teraz)
      este = este - 1
      If este > 0 Then chod_po_yz(teraz, este, Os, alternativy, more)


    Else

      Dim tmp As New CoordinatesBlok()
      Select Case Os
        Case 1
          tmp = FindStartAtMin_abL(SourceObjShape.coordList, velkost, teraz.b2, teraz.a1, Os_bod.Os_L, Os_bod.Os_aD)
        Case 2
          tmp = FindStartAtMin_abL(SourceObjShape.coordList, velkost, teraz.L2, teraz.a1, Os_bod.Os_bd, Os_bod.Os_aD)
      End Select

      If Not tmp.a1 = velkostObj.aD Then
        FoundRezList.Add(tmp)
        este = este - 1
        If Not more Then alternativy.Add(este + 1)

        If este > 0 Then
          chod_po_yz(tmp, este, Os, alternativy, more)
        End If

      Else

        tmp = FindStartAtMinX(SourceObjShape.coordList, velkost, teraz.a1 + velkost.aD, Os)

        If Not tmp.a1 = velkostObj.aD Then
          FoundRezList.Add(tmp)
          este = este - 1
          If Not more Then
            more = True
            alternativy.Clear()
          End If
          alternativy.Add(este + 1)

          If este > 0 Then
            chod_po_yz(tmp, este, Os, alternativy, more)
          End If
        End If

      End If
    End If

  End Sub

  Private Function FindStartAtMin_abL(ByVal array As ArrayList, ByVal velkost As Coord_bod, _
                                  ByVal min1 As Decimal, ByVal min2 As Decimal, _
                                  ByVal iOs1 As Os_bod, ByVal iOs2 As Os_bod) As CoordinatesBlok

    Dim tmp As New Coord_bod(velkostObj)
    Dim c As Integer
    Dim i As Integer
    i = array.Count
    Dim tmpCoord As CoordinatesBlok
    Dim testcoord As New CoordinatesBlok()
    Dim iOs1Value As Integer
    Dim iOs2Value As Integer
    Dim iThirdOs As Os_bod = GetThirdOs(iOs1, iOs2)

    For c = 0 To i - 1
      tmpCoord = array.Item(c)
      iOs1Value = tmpCoord.GetOsValue(iOs1, CoordRank.CoordRank1)
      iOs2Value = tmpCoord.GetOsValue(iOs2, CoordRank.CoordRank1)

      If iOs2Value <= min2 And _
         tmpCoord.GetOsValue(iThirdOs, CoordRank.CoordRank1) < tmp.GetOsValue(iThirdOs) Then

        testcoord.a1 = tmpCoord.a1
        testcoord.L1 = tmpCoord.L1
        testcoord.b1 = tmpCoord.b1
        Select Case iOs1
          Case Os_bod.Os_aD : testcoord.a1 = min1
          Case Os_bod.Os_bd : testcoord.L1 = min1
          Case Os_bod.Os_L : testcoord.b1 = min1
        End Select

        Select Case iOs2
          Case Os_bod.Os_aD : testcoord.a1 = min2
          Case Os_bod.Os_bd : testcoord.L1 = min2
          Case Os_bod.Os_L : testcoord.b1 = min2
        End Select

        testcoord.a2 = testcoord.a1 + velkost.aD
        testcoord.L2 = testcoord.L1 + velkost.L
        testcoord.b2 = testcoord.b1 + velkost.bd

        If Not je_von_start(testcoord, SourceObjShape.coordListInv) Then
          tmp.aD = testcoord.a1
          tmp.L = testcoord.L1
          tmp.bd = testcoord.b1
        End If
      End If


    Next

    If tmp.aD = velkostObj.aD Then
      For c = 0 To i - 1
        tmpCoord = array.Item(c)
        iOs1Value = tmpCoord.GetOsValue(iOs1, CoordRank.CoordRank1)
        iOs2Value = tmpCoord.GetOsValue(iOs2, CoordRank.CoordRank1)

        If iOs1Value > min1 And iOs2Value <= min2 And _
           tmp.GetOsValue(iOs1) > min1 And iOs1Value < tmp.GetOsValue(iOs1) And _
           tmpCoord.GetOsValue(iThirdOs, CoordRank.CoordRank1) < tmp.GetOsValue(iThirdOs) Then

          testcoord.a1 = IIf(iOs2 = Os_bod.Os_aD, min2, tmpCoord.a1)
          testcoord.L1 = IIf(iOs2 = Os_bod.Os_bd, min2, tmpCoord.L1)
          testcoord.b1 = IIf(iOs2 = Os_bod.Os_L, min2, tmpCoord.b1)

          testcoord.a2 = testcoord.a1 + velkost.aD
          testcoord.L2 = testcoord.L1 + velkost.L
          testcoord.b2 = testcoord.b1 + velkost.bd

          If Not je_von_start(testcoord, SourceObjShape.coordListInv) Then
            tmp.aD = testcoord.a1
            tmp.L = testcoord.L1
            tmp.bd = testcoord.b1
          End If
        End If

      Next

    End If

    Return New CoordinatesBlok(tmp.aD, tmp.aD + velkost.aD, tmp.L, tmp.L + velkost.L, tmp.bd, tmp.bd + velkost.bd)

  End Function

  Private Function je_von(ByVal b As CoordinatesBlok, ByVal array As ArrayList, ByVal array2 As ArrayList) As Boolean

    Dim c As Integer
    Dim i As Integer
    Dim von As Boolean
    Dim b2 As New CoordinatesBlok()

    If ((b.a1 < 0) OrElse (b.L1 < 0) OrElse (b.b1 < 0) OrElse (b.a2 > velkostObj.aD) OrElse (b.L2 > velkostObj.L) OrElse (b.b2 > velkostObj.bd)) Then
      von = True
    End If

    i = array.Count()
    If Not von Then
      For c = 0 To i - 1
        b2 = array.Item(c)
        If Not testuj(b, b2) Then
          von = True
        End If
      Next
    End If


    i = array2.Count()
    If Not von Then
      For c = 0 To i - 1
        b2 = array2.Item(c)
        If Not testuj(b, b2) Then
          von = True
        End If
      Next
    End If

    Return von

  End Function

  Private Function je_von_start(ByVal utvar As CoordinatesBlok, ByVal array As ArrayList) As Boolean


    Dim c As Integer
    Dim i As Integer
    Dim von As Boolean
    Dim b2 As New CoordinatesBlok()
    Dim b As New CoordinatesBlok()
    b = utvar

    If ((b.a1 < 0) OrElse (b.L1 < 0) OrElse (b.b1 < 0) OrElse (b.a2 > velkostObj.aD) OrElse (b.L2 > velkostObj.L) OrElse (b.b2 > velkostObj.bd)) Then
      von = True
    End If

    i = array.Count()
    If Not von Then
      For c = 0 To i - 1
        b2 = array.Item(c)
        If Not testuj(b, b2) Then
          von = True
        End If
      Next
    End If


    Return von

  End Function



  Friend Sub otocOffset(ByRef offset As Coord_bod, ByVal index As Integer)


    Select Case index
      Case 1
        offset.aD = 0
        offset.bd = 0
        offset.L = 0

        '*****
      Case 2
        offset.aD = offset.aD
        offset.bd = 0
        offset.L = 0

        '*****
      Case 3
        offset.aD = offset.aD
        offset.bd = offset.bd
        offset.L = 0

        '*****
      Case 4
        offset.aD = 0
        offset.bd = offset.bd
        offset.L = 0

        '*****
      Case 5
        offset.aD = 0
        offset.L = offset.L
        offset.bd = offset.bd

        '*****
      Case 6
        offset.aD = offset.aD
        offset.L = offset.L
        offset.bd = 0

        '*****
      Case 7
        offset.aD = offset.aD
        offset.L = offset.L
        offset.bd = offset.bd

        '*****
      Case 8
        offset.aD = 0
        offset.L = offset.L
        offset.bd = offset.bd


    End Select

  End Sub

  Friend Sub otoc(ByRef coordList As ArrayList, ByVal index As Integer, Optional ByVal offsetA As Integer = 0 _
                  , Optional ByVal offsetB As Integer = 0, Optional ByVal offsetL As Integer = 0)
    Select Case index
      Case 1, -1

        '*****
      Case 2
        otoc_yplus(coordList)
      Case -2
        otoc_yminus(coordList)

        '*****
      Case 3
        otoc_2yminus(coordList)
      Case -3
        otoc_2yplus(coordList)

        '*****
      Case 4
        otoc_yminus(coordList)
      Case -4
        otoc_yplus(coordList)

        '*****
      Case 5
        otoc_dolehlavou(coordList)
        otoc_yminus(coordList)
      Case -5
        otoc_dolehlavou(coordList)
        otoc_yminus(coordList)

        '*****
      Case 6
        otoc_dolehlavou2(coordList)
      Case -6
        otoc_dolehlavou2(coordList)

        '*****
      Case 7
        otoc_dolehlavou(coordList)
        otoc_yplus(coordList)
      Case -7
        otoc_dolehlavou(coordList)
        otoc_yplus(coordList)

        '*****
      Case 8
        otoc_dolehlavou(coordList)
      Case -8
        otoc_dolehlavou(coordList)

    End Select


    Dim i As Integer = coordList.Count()
    Dim b As New CoordinatesBlok()
    Dim c As Integer

    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 + offsetA
      b.a2 = b.a2 + offsetA
      b.b1 = b.b1 + offsetB
      b.b2 = b.b2 + offsetB
      b.L1 = b.L1 + offsetL
      b.L2 = b.L2 + offsetL

    Next

  End Sub

  Private Function getPolVelkosti() As Coord_bod
    Dim vo As New Coord_bod
    vo = SourceObjShape.Size(True)
    Return New Coord_bod(vo.aD / 2, vo.L / 2, vo.bd / 2)
  End Function

  Private Sub otoc_yplus(ByRef coordList As ArrayList)
    Dim polVelkosti As Coord_bod = getPolVelkosti()

    Dim i As Integer = coordList.Count()
    Dim b As New CoordinatesBlok()
    Dim c As Integer

    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 - polVelkosti.aD
      b.a2 = b.a2 - polVelkosti.aD
      b.L1 = b.L1 - polVelkosti.L
      b.L2 = b.L2 - polVelkosti.L
      b.b1 = b.b1 - polVelkosti.bd
      b.b2 = b.b2 - polVelkosti.bd

      Dim temp2 As Decimal

      Dim temp As Decimal
      temp = b.a1
      b.a1 = b.b1
      b.b1 = -temp

      temp = b.a2
      b.a2 = b.b2
      b.b2 = -temp

      temp2 = b.b2 - b.b1
      b.b1 = b.b1 + temp2
      b.b2 = b.b2 - temp2

      b.a1 = b.a1 + polVelkosti.bd
      b.a2 = b.a2 + polVelkosti.bd
      b.L1 = b.L1 + polVelkosti.L
      b.L2 = b.L2 + polVelkosti.L
      b.b1 = b.b1 + polVelkosti.aD
      b.b2 = b.b2 + polVelkosti.aD
    Next
  End Sub

  Private Sub otoc_2yplus(ByRef coordList As ArrayList)
    Dim polVelkosti As Coord_bod = getPolVelkosti()

    Dim i As Integer = coordList.Count()
    Dim b As New CoordinatesBlok()
    Dim c As Integer

    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 - polVelkosti.aD
      b.a2 = b.a2 - polVelkosti.aD
      b.L1 = b.L1 - polVelkosti.L
      b.L2 = b.L2 - polVelkosti.L
      b.b1 = b.b1 - polVelkosti.bd
      b.b2 = b.b2 - polVelkosti.bd

      Dim temp2 As Decimal

      Dim temp As Decimal
      temp = b.a1
      b.a1 = b.b1
      b.b1 = -temp

      temp = b.a2
      b.a2 = b.b2
      b.b2 = -temp

      temp2 = b.b2 - b.b1
      b.b1 = b.b1 + temp2
      b.b2 = b.b2 - temp2

      b.a1 = b.a1 + polVelkosti.bd
      b.a2 = b.a2 + polVelkosti.bd
      b.L1 = b.L1 + polVelkosti.L
      b.L2 = b.L2 + polVelkosti.L
      b.b1 = b.b1 + polVelkosti.aD
      b.b2 = b.b2 + polVelkosti.aD
    Next

    polVelkosti.aD = velkostObj.bd / 2
    polVelkosti.L = velkostObj.L / 2
    polVelkosti.bd = velkostObj.aD / 2

    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 - polVelkosti.aD
      b.a2 = b.a2 - polVelkosti.aD
      b.L1 = b.L1 - polVelkosti.L
      b.L2 = b.L2 - polVelkosti.L
      b.b1 = b.b1 - polVelkosti.bd
      b.b2 = b.b2 - polVelkosti.bd

      Dim temp2 As Decimal

      Dim temp As Decimal
      temp = b.a1
      b.a1 = b.b1
      b.b1 = -temp

      temp = b.a2
      b.a2 = b.b2
      b.b2 = -temp

      temp2 = b.b2 - b.b1
      b.b1 = b.b1 + temp2
      b.b2 = b.b2 - temp2

      b.a1 = b.a1 + polVelkosti.bd
      b.a2 = b.a2 + polVelkosti.bd
      b.L1 = b.L1 + polVelkosti.L
      b.L2 = b.L2 + polVelkosti.L
      b.b1 = b.b1 + polVelkosti.aD
      b.b2 = b.b2 + polVelkosti.aD
    Next
  End Sub

  Private Sub otoc_yminus(ByRef coordList As ArrayList)
    Dim polVelkosti As Coord_bod = getPolVelkosti()

    Dim i As Integer = coordList.Count()
    Dim b As New CoordinatesBlok()
    Dim c As Integer

    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 - polVelkosti.aD
      b.a2 = b.a2 - polVelkosti.aD
      b.L1 = b.L1 - polVelkosti.L
      b.L2 = b.L2 - polVelkosti.L
      b.b1 = b.b1 - polVelkosti.bd
      b.b2 = b.b2 - polVelkosti.bd

      Dim temp2 As Decimal

      Dim temp As Decimal
      temp = b.b1
      b.b1 = b.a1
      b.a1 = -temp

      temp = b.b2
      b.b2 = b.a2
      b.a2 = -temp

      temp2 = b.a2 - b.a1
      b.a1 = b.a1 + temp2
      b.a2 = b.a2 - temp2

      b.a1 = b.a1 + polVelkosti.bd
      b.a2 = b.a2 + polVelkosti.bd
      b.L1 = b.L1 + polVelkosti.L
      b.L2 = b.L2 + polVelkosti.L
      b.b1 = b.b1 + polVelkosti.aD
      b.b2 = b.b2 + polVelkosti.aD
    Next
  End Sub

  Private Sub otoc_2yminus(ByRef coordList As ArrayList)
    Dim polVelkosti As Coord_bod = getPolVelkosti()

    Dim i As Integer = coordList.Count()
    Dim b As New CoordinatesBlok()
    Dim c As Integer

    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 - polVelkosti.aD
      b.a2 = b.a2 - polVelkosti.aD
      b.L1 = b.L1 - polVelkosti.L
      b.L2 = b.L2 - polVelkosti.L
      b.b1 = b.b1 - polVelkosti.bd
      b.b2 = b.b2 - polVelkosti.bd

      Dim temp2 As Decimal

      Dim temp As Decimal
      temp = b.b1
      b.b1 = b.a1
      b.a1 = -temp

      temp = b.b2
      b.b2 = b.a2
      b.a2 = -temp

      temp2 = b.a2 - b.a1
      b.a1 = b.a1 + temp2
      b.a2 = b.a2 - temp2

      b.a1 = b.a1 + polVelkosti.bd
      b.a2 = b.a2 + polVelkosti.bd
      b.L1 = b.L1 + polVelkosti.L
      b.L2 = b.L2 + polVelkosti.L
      b.b1 = b.b1 + polVelkosti.aD
      b.b2 = b.b2 + polVelkosti.aD
    Next

    polVelkosti.aD = velkostObj.bd / 2
    polVelkosti.L = velkostObj.L / 2
    polVelkosti.bd = velkostObj.aD / 2


    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 - polVelkosti.aD
      b.a2 = b.a2 - polVelkosti.aD
      b.L1 = b.L1 - polVelkosti.L
      b.L2 = b.L2 - polVelkosti.L
      b.b1 = b.b1 - polVelkosti.bd
      b.b2 = b.b2 - polVelkosti.bd

      Dim temp2 As Decimal

      Dim temp As Decimal
      temp = b.b1
      b.b1 = b.a1
      b.a1 = -temp

      temp = b.b2
      b.b2 = b.a2
      b.a2 = -temp

      temp2 = b.a2 - b.a1
      b.a1 = b.a1 + temp2
      b.a2 = b.a2 - temp2

      b.a1 = b.a1 + polVelkosti.bd
      b.a2 = b.a2 + polVelkosti.bd
      b.L1 = b.L1 + polVelkosti.L
      b.L2 = b.L2 + polVelkosti.L
      b.b1 = b.b1 + polVelkosti.aD
      b.b2 = b.b2 + polVelkosti.aD
    Next
  End Sub

  Private Sub otoc_dolehlavou(ByRef coordList As ArrayList)

    Dim polVelkosti As Coord_bod = getPolVelkosti()

    Dim i As Integer = coordList.Count()
    Dim b As New CoordinatesBlok()
    Dim c As Integer

    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 - polVelkosti.aD
      b.a2 = b.a2 - polVelkosti.aD
      b.L1 = b.L1 - polVelkosti.L
      b.L2 = b.L2 - polVelkosti.L
      b.b1 = b.b1 - polVelkosti.bd
      b.b2 = b.b2 - polVelkosti.bd

      Dim temp As Decimal
      temp = b.L1
      b.L1 = -b.L2
      b.L2 = -temp

      temp = b.b1
      b.b1 = -b.b2
      b.b2 = -temp

      b.a1 = b.a1 + polVelkosti.aD
      b.a2 = b.a2 + polVelkosti.aD
      b.L1 = b.L1 + polVelkosti.L
      b.L2 = b.L2 + polVelkosti.L
      b.b1 = b.b1 + polVelkosti.bd
      b.b2 = b.b2 + polVelkosti.bd
    Next
  End Sub

  Private Sub otoc_dolehlavou2(ByRef coordList As ArrayList)
    Dim polVelkosti As Coord_bod = getPolVelkosti()

    Dim i As Integer = coordList.Count()
    Dim b As New CoordinatesBlok()
    Dim c As Integer

    For c = 0 To i - 1
      b = coordList.Item(c)

      b.a1 = b.a1 - polVelkosti.aD
      b.a2 = b.a2 - polVelkosti.aD
      b.L1 = b.L1 - polVelkosti.L
      b.L2 = b.L2 - polVelkosti.L
      b.b1 = b.b1 - polVelkosti.bd
      b.b2 = b.b2 - polVelkosti.bd

      Dim temp As Decimal
      temp = b.L1
      b.L1 = -b.L2
      b.L2 = -temp

      temp = b.a1
      b.a1 = -b.a2
      b.a2 = -temp

      b.a1 = b.a1 + polVelkosti.aD
      b.a2 = b.a2 + polVelkosti.aD
      b.L1 = b.L1 + polVelkosti.L
      b.L2 = b.L2 + polVelkosti.L
      b.b1 = b.b1 + polVelkosti.bd
      b.b2 = b.b2 + polVelkosti.bd
    Next
  End Sub

  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub
End Class
