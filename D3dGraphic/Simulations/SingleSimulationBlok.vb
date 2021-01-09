Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D
Imports Microsoft.DirectX.Direct3D.D3DX
Imports System.Drawing.Imaging
Imports System.Collections.Generic

Friend Class SingleSimulationBlok : Inherits SingleSimulation
  Friend cmpCurrent As ComputationBlok
  Friend Computations As New Collection
  Friend ShowSimulation As Boolean
  Friend MSKey As String ' Key MS collekcie do ktorej patri

#Region "D3D Setup Code"
  ' Data variables.

  'Friend ReadOnly Property BestComputation() As ComputationBlok
  '  Get
  '    Dim cmpBest As ComputationBlok
  '    If Computations.Count > 0 Then
  '      cmpBest = Computations(1)

  '      For Each cmp As ComputationBlok In Computations
  '        'Najst najlepsiu moznost>
  '        If cmp.ObjemZvysku < cmpBest.ObjemZvysku Then
  '          cmpBest = cmp
  '        End If
  '      Next
  '    Else
  '      cmpBest = Nothing
  '    End If
  '    Return cmpBest
  '  End Get
  'End Property

  Friend ReadOnly Property Popis() As ComputationBlok
    Get
      Dim cmpBest As ComputationBlok
      If Computations.Count > 0 Then
        cmpBest = Computations(1)

        For Each cmp As ComputationBlok In Computations
          'Najst najlepsiu moznost>
          If cmp.ObjemOdrezku < cmpBest.ObjemOdrezku Then
            cmpBest = cmp
          End If
        Next
      Else
        cmpBest = Nothing
      End If
      Return cmpBest
    End Get
  End Property

  Private Sub changeCombination()
    cmpCurrent = Computations(cbComputations.SelectedItem.Index + 1)
    MyBase.FillLogInfo(cmpCurrent.ObjemVyrezu, cmpCurrent.ObjemOdrezku)
    CreateVertexBuffer()

    If Not bTociSa Then MyBase.Render()
  End Sub

  Friend Function GraphicsDrawLite(ByVal iSimulationIndex As Integer, _
                                   ByVal aaEnabled As Boolean,
                                   ByVal svpEnabled As Boolean,
                                   ByVal rotSpeed As Decimal) As Boolean
    liteVersion = True
    lbl_aD.Visible = False
    lbl_bd.Visible = False
    lbl_L.Visible = False
    chb_aD.Visible = False
    chb_bd.Visible = False
    chb_L.Visible = False
    lblIndex.Visible = False
    lblVyrez.Visible = False
    lblZvysok.Visible = False
    cbComputations.Visible = False
    lblPopis.Visible = False
    tblLog.Visible = False
    pic3d.Dock = DockStyle.Fill

    Return GraphicsDraw(iSimulationIndex, aaEnabled, svpEnabled, rotSpeed)
    ' Initialize the graphics device. Return True if successful.

  End Function

  Friend Function GraphicsDraw(ByVal iSimulationIndex As Integer, _
                               ByVal aaEnabled As Boolean,
                               ByVal svpEnabled As Boolean,
                               ByVal rotSpeed As Decimal) As Boolean
    ' Initialize the graphics device. Return True if successful.

    SimulationIndex = iSimulationIndex
    aaEnable = aaEnabled
    svpEnable = svpEnabled
    rotateSpeed = rotSpeed

    lblIndex.Text = SimulationIndex.ToString
    defW = Me.Width
    defH = Me.Height

    If Computations.Count > 0 And Not liteVersion Then

      Dim rez As Integer = DirectCast(Computations.Item(1), ComputationBlok).defaultRezSmer
      Select Case rez
        Case 1
          chb_aD.Checked = True
          chb_aD.Enabled = False
        Case 2
          chb_bd.Checked = True
          chb_bd.Enabled = False
        Case 3
          chb_L.Checked = True
          chb_L.Enabled = False
      End Select


      Dim cmp As ComputationBlok
      Dim sPopis As String = ""
      Dim i As Integer
      Dim colTmp As New Collection
      Dim cmpCompare As ComputationBlok
      Dim bAdded As Boolean

      For Each cmp In Computations
        bAdded = False
        If colTmp.Count = 0 Then
          colTmp.Add(cmp)
          bAdded = True
        Else
          For i = 1 To colTmp.Count
            cmpCompare = colTmp(i)
            If cmp.ObjemOdrezku < cmpCompare.ObjemOdrezku Then
              colTmp.Add(cmp, cmp.Popis, i)
              bAdded = True
              Exit For
            End If
          Next
        End If
        If Not bAdded Then colTmp.Add(cmp)
      Next
      Computations = colTmp

      cmpCurrent = Computations(1)

      Select Case cmpCurrent.roh
        Case 1
          Select Case cmpCurrent.strana
            Case 3 'pred
              angleY = Math.PI / 8
              angleX = -Math.PI / 8
            Case 2 'lavo
              angleY = Math.PI / 8
              angleX = -Math.PI / 3
            Case 1 'spod
              angleX = -Math.PI / 8
              angleY = Math.PI / 3

          End Select
        Case 2
          Select Case cmpCurrent.strana
            Case 3  'pravo
              angleY = Math.PI / 8
              angleX = Math.PI / 3
            Case 1 'spod
              angleY = Math.PI / 3
              angleX = Math.PI / 8
            Case 2 'pred
              angleY = Math.PI / 8
              angleX = Math.PI / 8
          End Select
        Case 3
          Select Case cmpCurrent.strana
            Case 3 'zad
              angleX = Math.PI - Math.PI / 8
              angleY = -Math.PI / 8
            Case 2 'pravo
              angleX = Math.PI - Math.PI / 3
              angleY = -Math.PI / 8
            Case 1 'spod
              angleX = Math.PI - Math.PI / 8
              angleY = -Math.PI / 3

          End Select
        Case 4
          Select Case cmpCurrent.strana
            Case 3  'lavo
              angleX = Math.PI + Math.PI / 3
              angleY = -Math.PI / 8
            Case 2  'zad
              angleX = Math.PI + Math.PI / 8
              angleY = -Math.PI / 8
            Case 1 'spod
              angleX = Math.PI + Math.PI / 8
              angleY = -Math.PI / 3

          End Select
        Case 5
          Select Case cmpCurrent.strana
            Case 1  'vrch
              angleY = -Math.PI / 3
              angleX = -Math.PI / 8
            Case 3  'lavo
              angleY = -Math.PI / 8
              angleX = -Math.PI / 3
            Case 2 'pred
              angleY = -Math.PI / 8
              angleX = -Math.PI / 8
          End Select
        Case 6
          Select Case cmpCurrent.strana
            Case 3 'pred
              angleY = -Math.PI / 8
              angleX = Math.PI / 8
            Case 2  'pravo
              angleY = -Math.PI / 8
              angleX = Math.PI / 3
            Case 1  'vrch
              angleY = -Math.PI / 3
              angleX = Math.PI / 8
          End Select
        Case 7
          Select Case cmpCurrent.strana
            Case 3  'pravo
              angleX = Math.PI - Math.PI / 3
              angleY = Math.PI / 8
            Case 2  'zad
              angleX = Math.PI - Math.PI / 8
              angleY = Math.PI / 8
            Case 1  'vrch
              angleX = Math.PI - Math.PI / 8
              angleY = Math.PI / 3

          End Select
        Case 8
          Select Case cmpCurrent.strana
            Case 3   'zad
              angleX = Math.PI + Math.PI / 8
              angleY = Math.PI / 8
            Case 2  'lavo
              angleX = Math.PI + Math.PI / 3
              angleY = Math.PI / 8
            Case 1  'vrch
              angleX = Math.PI + Math.PI / 8
              angleY = Math.PI / 3

          End Select
      End Select

      For Each cmp In Computations
        cbComputations.Items.Add(cmp.Popis)
        sPopis += IIf(sPopis <> "", vbCrLf, "") & cmp.Popis
        cbComputations.SelectedItem = cbComputations.Items(0)
        lblPopis.Text = cmp.Popis
      Next

      If (Computations.Count = 1) Then
        cbComputations.Hide()
        ToolTip1.SetToolTip(lblPopis, sPopis)
      Else
        lblPopis.Hide()
        ToolTip1.SetToolTip(cbComputations, sPopis)
      End If

      MyBase.FillLogInfo(cmpCurrent.ObjemVyrezu, cmpCurrent.ObjemOdrezku)

      m_MaxPocKS = cmpCurrent.maxPocet
      iudQTY.Maximum = cmpCurrent.maxPocet
      iudQTY.Value = cmpCurrent.Pocet

    Else
      cmpCurrent = Computations(1)
      iudQTY.Visible = False
    End If

    velkostObj = cmpCurrent.SourceObjShape.Size(True)

    Return MyBase.GraphicsDrawFinish()

  End Function

  ' Create a vertex buffer for the device.
  Protected Overrides Sub CreateVertexBuffer()
    ' Create the points.
    m_Points = New List(Of CustomVertex.PositionNormalTextured)

    ' Make boxes.
    Dim u As Integer
    Dim c As Integer
    Dim b As New CoordinatesBlok()

    Dim d As Decimal = velkostObj.aD
    If velkostObj.bd > d Then d = velkostObj.bd
    If velkostObj.L > d Then d = velkostObj.L

    d = d / 5
    MakeCube(-d, -d, -d, d, -d + d / 6, -d + d / 6)
    MakeCube(-d, -d, -d, -d + d / 6, -d + d / 6, d)
    MakeCube(-d, -d, -d, -d + d / 6, d, -d + d / 6)

    u = cmpCurrent.SourceObjShape.coordList.Count()
    For c = 0 To u - 1
      b = cmpCurrent.SourceObjShape.coordList.Item(c)
      MakeCube(b.a1, b.L1, b.b1, b.a2, b.L2, b.b2)
    Next

    u = cmpCurrent.CelyRez.Count()
    For c = 0 To u - 1
      b = cmpCurrent.CelyRez.Item(c)

      MakeCube(b.a1, b.L1, b.b1, b.a2, b.L2, b.b2)
    Next

    u = cmpCurrent.FoundRezList.Count()
    For c = 0 To u - 1
      b = cmpCurrent.FoundRezList.Item(c)

      MakeCube(b.a1, b.L1, b.b1, b.a2, b.L2, b.b2)
    Next

    MyBase.CreateVertexBufferFinish()

  End Sub

#End Region ' D3D Setup Code

  Protected Overrides Function GetFirstTriangle1() As Integer
    Return cmpCurrent.SourceObjShape.coordList.Count * 12 + 36
  End Function
  Protected Overrides Function GetFirstTriangle2() As Integer
    Return cmpCurrent.CelyRez.Count * 12 + cmpCurrent.SourceObjShape.coordList.Count * 12 + 36
  End Function

  Protected Overrides Sub ShowRezInfo()
    Dim rez As CoordinatesBlok = cmpCurrent.rezPilou
    Dim ft As System.Drawing.Font
    Dim myFont As Direct3D.Font

    ft = New System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold)
    myFont = New Direct3D.Font(m_Device, ft)
    myFont.DrawText(Nothing, "a: " & CInt(rez.a1) & "-" & CInt(rez.a2), 10, 10, Color.Blue) ' System.Drawing.Color.Yellow.ToArgb()
    myFont.DrawText(Nothing, "b: " & CInt(rez.b1) & "-" & CInt(rez.b2), 10, 25, Color.Green)
    myFont.DrawText(Nothing, "L: " & CInt(rez.L1) & "-" & CInt(rez.L2), 10, 40, Color.Orange)
    myFont.Dispose()
    myFont = Nothing
    ft.Dispose()
    ft = Nothing
    rez = Nothing
  End Sub

  Protected Overrides Sub cbComputations_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Not m_Device Is Nothing Then changeCombination()
  End Sub

  Protected Overrides Sub RecomputeCore()
    If Not m_Device Is Nothing Then
      Dim i As Integer = iudQTY.Value
      If (Not i > iudQTY.Maximum) And (i > 0) Then
        For i = 1 To Computations.Count
          DirectCast(Computations.Item(i), ComputationBlok).Recompute(iudQTY.Value, chb_aD.Checked, chb_bd.Checked, chb_L.Checked)
        Next

        changeCombination()
      End If

    End If
  End Sub

End Class

