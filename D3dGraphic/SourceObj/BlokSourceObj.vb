Imports System.Linq

Friend Class BlokSourceObj
    Private _coordListAdd As ArrayList
    Private _coordListInv As ArrayList
    Private _coordList As ArrayList  'Kopia _coordListAdd ale uz s odstranenymi Remove


    Friend Sub New(ByVal sSN As String, ByVal sSarza As String, ByVal sLocation As String, ByVal dblSklCena As Decimal, ByVal bOrientedL As Boolean,
                   ByVal sSNs As String, ByVal blok As IOrderedEnumerable(Of Blok))
        SN = sSN
        SNs = sSNs
        Sarza = sSarza
        Location = sLocation
        SklCena = dblSklCena
        OrientedL = bOrientedL
        Me.blok = blok

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

    Friend Sub CoordToRemove(ByVal iCoord As CoordinatesBlok)
        coordListRemove.Add(iCoord)
    End Sub

    Friend Sub CoordToAdd(ByVal iCoord As CoordinatesBlok)
        _coordListAdd.Add(iCoord)
    End Sub

    Friend Sub Process()
        'Naplnenie CoordList-u a CoordListInv-u zo zadania
        Dim CoordToRemove As CoordinatesBlok
        Dim i As Integer

        _coordList = New ArrayList
        _coordListInv = New ArrayList

        If _coordListAdd.Count > 1 Then

            Dim size As Coord_bod = getBlokSizeFromZero(_coordListAdd)
            Dim delete As New CoordinatesBlok(0, size.aD, 0, size.L, 0, size.bd)
            Dim okolieArray As New ArrayList
            okolieArray.Add(delete)

            For i = 0 To _coordListAdd.Count - 1
                CoordToRemove = _coordListAdd(i)
                RemoveFromCoordList(okolieArray, CoordToRemove)
            Next

            For i = 0 To okolieArray.Count - 1
                _coordListInv.Add(okolieArray.Item(i))
            Next
        End If

        For i = 0 To _coordListAdd.Count() - 1
            _coordList.Add(New CoordinatesBlok(_coordListAdd.Item(i)))
        Next i

        For i = 0 To coordListRemove.Count - 1
            CoordToRemove = coordListRemove(i)
            _coordListInv.Add(New CoordinatesBlok(CoordToRemove))
            RemoveFromCoordList(_coordList, CoordToRemove)
        Next

    End Sub

    Private Sub RemoveFromCoordList(ByRef cl As ArrayList, ByVal coordToRemove As CoordinatesBlok)

        Dim u As Integer = 0
        Dim j As Integer
        Dim i As Integer = cl.Count()
        Dim b1 As New CoordinatesBlok()
        Dim b2 As New CoordinatesBlok(coordToRemove)

        For j = 0 To i - 1
            b1 = cl.Item(u)
            If Not testuj(b2, b1) Then
                del(b1, b2, cl)
                cl.RemoveAt(u)
            Else : u += 1
            End If
        Next j

    End Sub

    Friend ReadOnly Property SN() As String

    Friend Property SNs() As String

    Friend ReadOnly Property ColSNs() As New Collection

    Friend ReadOnly Property Sarza() As String

    Friend ReadOnly Property Location() As String

    Friend ReadOnly Property SklCena() As Decimal

    Friend ReadOnly Property OrientedL() As Boolean

    Friend ReadOnly Property blok() As IOrderedEnumerable(Of Blok)

    Friend ReadOnly Property CoordToString() As String
        Get
            Dim sRef = ""
            Dim bl As Blok
            For Each bl In blok
                sRef = sRef & CStr(bl.a1) & "_" &
                        CStr(bl.a2) & "_" &
                        CStr(bl.L1) & "_" &
                        CStr(bl.L2) & "_" &
                        CStr(bl.b1) & "_" &
                        CStr(bl.b2) & "_" &
                        CStr(bl.Add)
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

    Friend ReadOnly Property coordListInv() As ArrayList
        Get
            Return _coordListInv
        End Get
    End Property

    Friend Function Size(ByVal bFromZero As Boolean) As Coord_bod
        If bFromZero Then
            Return getBlokSizeFromZero(_coordList)
        Else
            Return getBlokSizeMinimum(_coordList)
        End If
    End Function

    Friend Function getBlokOffset() As Coord_bod

        If _coordList.Count > 0 Then
            Dim i As Integer
            Dim v As Coord_bod
            Dim temp As CoordinatesBlok

            temp = New CoordinatesBlok(_coordList.Item(0))
            v = New Coord_bod(temp.a1, temp.L1, temp.b1)

            For i = 1 To _coordList.Count - 1
                temp = _coordList.Item(i)
                If temp.a1 < v.aD Then v.aD = temp.a1
                If temp.L1 < v.L Then v.L = temp.L1
                If temp.b1 < v.bd Then v.bd = temp.b1
            Next
            Return v
        Else
            Return New Coord_bod(0, 0, 0)
        End If

    End Function

    Friend ReadOnly Property SizeDescription() As String
        Get
            Return SizeDescriptionBlok(Size(False))
        End Get
    End Property

    Friend ReadOnly Property Clone() As BlokSourceObj
        Get
            'Dal by sa prepisat tak, ze sa iba okopiruju kolekcie a nie standardnym vypocitanim cez Process
            Dim CS As New BlokSourceObj(Me.SN, Me.Sarza, Me.Location, Me.SklCena, Me.OrientedL, Me.SNs, Me.blok)

            For i As Integer = 0 To _coordListAdd.Count - 1
                CS.CoordToAdd(New CoordinatesBlok(_coordListAdd(i)))
            Next

            For i As Integer = 0 To coordListRemove.Count - 1
                CS.CoordToRemove(New CoordinatesBlok(coordListRemove(i)))
            Next
            CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu

            Return CS
        End Get
    End Property

End Class
