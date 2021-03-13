Imports System.Linq

Friend Class ValecSourceObj
    Private _coordListAdd As ArrayList
    Private _coordList As ArrayList  'Kopia _coordListAdd ale uz s odstranenymi Remove


    Friend Sub New(ByVal sSN As String, ByVal sSarza As String, ByVal sLocation As String, ByVal dblSklCena As Decimal,
                   ByVal sSNs As String, ByVal valec As IOrderedEnumerable(Of Valec))
        SN = sSN
        SNs = sSNs
        Sarza = sSarza
        Location = sLocation
        SklCena = dblSklCena
        Me.valec = valec

        If _coordListAdd Is Nothing Then
            _coordListAdd = New ArrayList()
        Else
            _coordListAdd.Clear()
        End If
        If coordListRemove Is Nothing Then
            coordListRemove = New ArrayList()
        Else
            coordListRemove.Clear()
        End If

        Dim arraySNs() As String
        Dim sOneSN As String

        arraySNs = Split(sSNs, ";", , CompareMethod.Text)

        If UBound(arraySNs) > 0 Then
            Debug.Print(UBound(arraySNs))
        End If

        For i As Integer = 0 To UBound(arraySNs)
            sOneSN = Trim(arraySNs(i))
            If Not IsExistKey(ColSNs, sOneSN) Then
                ColSNs.Add(sOneSN, sOneSN)
            End If
        Next

    End Sub

    Friend Sub CoordToRemove(ByVal iCoord As CoordinatesValec)
        coordListRemove.Add(iCoord)
    End Sub

    Friend Sub CoordToAdd(ByVal iCoord As CoordinatesValec)
        _coordListAdd.Add(iCoord)
    End Sub

    Friend Sub Process()
        'Naplnenie CoordList-u zo zadania
        Dim CoordToRemove As CoordinatesValec
        Dim i As Integer

        _coordList = New ArrayList

        For i = 0 To _coordListAdd.Count() - 1
            _coordList.Add(New CoordinatesValec(_coordListAdd.Item(i)))
        Next i

        For i = 0 To coordListRemove.Count - 1
            CoordToRemove = coordListRemove(i)
            RemoveFromCoordList(CoordToRemove)
        Next

        OptimiseValec(_coordList)


    End Sub

    Private Sub RemoveFromCoordList(ByVal remove As CoordinatesValec)

        Dim done As Boolean = False
        Dim process As Boolean = False
        Dim temp As CoordinatesValec
        Dim i As Integer
        Dim novy As New ArrayList

        sortValce(_coordList, True)


        For i = 0 To _coordList.Count - 1
            temp = _coordList.Item(i)

            If done Then
                novy.Add(temp)
            Else

                If Not process Then
                    If (temp.L2 > remove.L1) Then
                        process = True

                        If (temp.L1 < remove.L1) Then
                            novy.Add(New CoordinatesValec(temp.d2, temp.D1, temp.L1, remove.L1))
                        End If

                        If (temp.D1 > remove.D1) Then
                            If (temp.L2 > remove.L2) Then
                                novy.Add(New CoordinatesValec(remove.D1, temp.D1, remove.L1, remove.L2))
                            Else
                                novy.Add(New CoordinatesValec(remove.D1, temp.D1, remove.L1, temp.L2))
                            End If
                        End If

                        If (temp.d2 < remove.d2) Then
                            If (temp.L2 > remove.L2) Then
                                novy.Add(New CoordinatesValec(temp.d2, remove.d2, remove.L1, remove.L2))
                            Else
                                novy.Add(New CoordinatesValec(temp.d2, remove.d2, remove.L1, temp.L2))
                            End If
                        End If

                        If (temp.L2 = remove.L2) Then
                            done = True
                        End If

                        If (temp.L2 > remove.L2) Then
                            novy.Add(New CoordinatesValec(temp.d2, temp.D1, remove.L2, temp.L2))
                            done = True
                        End If
                    Else
                        novy.Add(temp)
                    End If

                Else

                    If (temp.L2 >= remove.L2) Then
                        done = True

                        If (temp.D1 > remove.D1) Then
                            If (temp.L2 > remove.L2) Then
                                novy.Add(New CoordinatesValec(remove.D1, temp.D1, remove.L1, remove.L2))
                            Else
                                novy.Add(New CoordinatesValec(remove.D1, temp.D1, remove.L1, temp.L2))
                            End If
                        End If

                        If (temp.d2 < remove.d2) Then
                            If (temp.L2 > remove.L2) Then
                                novy.Add(New CoordinatesValec(temp.d2, remove.d2, remove.L1, remove.L2))
                            Else
                                novy.Add(New CoordinatesValec(temp.d2, remove.d2, remove.L1, temp.L2))
                            End If
                        End If

                        If (temp.L2 > remove.L2) Then
                            novy.Add(New CoordinatesValec(temp.d2, temp.D1, remove.L2, temp.L2))
                            done = True
                        End If

                    Else

                        If (temp.D1 > remove.D1) Then
                            novy.Add(New CoordinatesValec(remove.D1, temp.D1, remove.L1, temp.L2))
                        End If

                        If (temp.d2 < remove.d2) Then
                            novy.Add(New CoordinatesValec(temp.d2, remove.d2, remove.L1, temp.L2))
                        End If

                    End If

                End If
            End If

        Next
        _coordList = novy
    End Sub

    Friend ReadOnly Property SN() As String

    Friend Property SNs() As String

    Friend ReadOnly Property ColSNs() As New Collection

    Friend ReadOnly Property Sarza() As String

    Friend ReadOnly Property Location() As String

    Friend ReadOnly Property SklCena() As Decimal

    Friend ReadOnly Property valec() As IOrderedEnumerable(Of Valec)

    Friend ReadOnly Property CoordToString() As String
        Get
            Dim sRef = ""
            Dim va As Valec
            For Each va In valec
                sRef = sRef & CStr(va.D1) & "_" &
                                CStr(va.d2) & "_" &
                                CStr(va.L1) & "_" &
                                CStr(va.L2) & "_" &
                                CStr(va.Add)
            Next

            Return sRef
        End Get
    End Property

    Public ReadOnly Property coordList() As ArrayList
        Get
            Return _coordList
        End Get
    End Property

    Friend ReadOnly Property coordListRemove() As ArrayList

    Friend ReadOnly Property Size() As Coord_bod
        Get
            Return GetValecSize(coordList)
        End Get
    End Property

    Friend ReadOnly Property SizeDescription() As String
        Get
            Return SizeDescriptionValec(Size)
        End Get
    End Property

    Friend ReadOnly Property Clone() As ValecSourceObj
        Get
            'Dal by sa prepisat tak, ze sa iba okopiruju kolekcie a nie standardnym vypocitanim cez Process
            Dim CS As New ValecSourceObj(Me.SN, Me.Sarza, Me.Location, Me.SklCena, Me.SNs, Me.valec)

            For i As Integer = 0 To _coordListAdd.Count - 1
                CS.CoordToAdd(New CoordinatesValec(_coordListAdd(i)))
            Next

            For i As Integer = 0 To coordListRemove.Count - 1
                CS.CoordToRemove(New CoordinatesValec(coordListRemove(i)))
            Next
            CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu

            Return CS
        End Get
    End Property

End Class
