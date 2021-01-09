Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D
Imports Microsoft.DirectX.Direct3D.D3DX
Imports System.Drawing.Imaging
Imports System.Collections.Generic

Friend Class SingleSimulationValec : Inherits SingleSimulation
  Private valecDetail As Integer = 64
  Friend cmpCurrent As ComputationValec
  Friend ShowSimulation As Boolean
  Friend MSKey As String ' Key MS collekcie do ktorej patri
  Friend Event MaxSimulationValec()

#Region "D3D Setup Code"
  ' Data variables.

  Friend Function GraphicsDraw(ByVal iSimulationIndex As Integer, _
                               ByVal aaEnabled As Boolean,
                               ByVal svpEnabled As Boolean,
                               ByVal rotSpeed As Decimal) As Boolean
    ' Initialize the graphics device. Return True if successful.

    SimulationIndex = iSimulationIndex
    aaEnable = aaEnabled
    svpEnable = svpEnabled
    rotateSpeed = rotSpeed

    angleY = Math.PI / 6
    angleX = Math.PI / 6

    m_MaxPocKS = cmpCurrent.FoundRezList.Count
    iudQTY.Maximum = cmpCurrent.FoundRezList.Count
    iudQTY.Value = cmpCurrent.FoundRezList.Count

    velkostObj = cmpCurrent.SourceObjShape.Size

    lblPopis.Text = ""
    If cmpCurrent.iValecType = ValecType.ValecType_Inner Then
      chb_bd.Checked = True
      chb_bd.Enabled = False
      chb_aD.Checked = False
      chb_aD.Enabled = False
    ElseIf cmpCurrent.Rozmery.bd = 0 Then
      chb_bd.Checked = True
      chb_bd.Enabled = False
    End If

    defW = Me.Width
    defH = Me.Height

    Dim sPopis As String = ""

    MyBase.FillLogInfo(cmpCurrent.ObjemVyrezu, cmpCurrent.ObjemOdrezku)

    'Debug.Print(lblZvysok.Text & "-" & lblVyrez.Text)


    Return MyBase.GraphicsDrawFinish()

  End Function

  ' Create a vertex buffer for the device.
  Protected Overrides Sub CreateVertexBuffer()
    ' Create the points.
    m_Points = New List(Of CustomVertex.PositionNormalTextured)

    ' Make boxes.
    Dim u As Integer
    Dim c As Integer
    Dim b As New CoordinatesValec()

    Dim d As Decimal = velkostObj.aD
    If d < velkostObj.L Then d = velkostObj.L

    Debug.Print("var iMierka = " & CInt(d / 2) & ";")

    d = d / 5
    MakeCube(-d, -d, -d, d, -d + d / 6, -d + d / 6)
    MakeCube(-d, -d, -d, -d + d / 6, -d + d / 6, d)
    MakeCube(-d, -d, -d, -d + d / 6, d, -d + d / 6)


    u = cmpCurrent.NedotknutaCast.Count()
    For c = 0 To u - 1
      b = cmpCurrent.NedotknutaCast.Item(c)
      MakeCone(b.d2, b.D1, b.L1, b.L2)
      Debug.Print("addValec(group, " & b.D1 & ", " & b.d2 & ", " & b.L1 & ", " & b.L2 & ", 0x808080, 0, 0.8); //Gray")
    Next

    u = cmpCurrent.CelyRez.Count()
    For c = 0 To u - 1
      b = cmpCurrent.CelyRez.Item(c)
      MakeCone(b.d2, b.D1, b.L1, b.L2)
      Debug.Print("addValec(group, " & b.D1 - CInt(IIf(b.D1 / 100 < 5, 5, b.D1 / 100)) & ", " & b.d2 & ", " & b.L1 & ", " & b.L2 & ", 0x008000, 0, 0.4); //Olive")
    Next

    u = cmpCurrent.FoundRezList.Count()
    For c = 0 To u - 1
      b = cmpCurrent.FoundRezList.Item(c)
      MakeCone(b.d2, b.D1, b.L1, b.L2)
      Debug.Print("addValec(group, " & b.D1 & ", " & b.d2 & ", " & b.L1 & ", " & b.L2 & ", 0xff0000, " & CInt((b.L2 - b.L1) / 20) & ", 0.8, material); //Red")
    Next

    MyBase.CreateVertexBufferFinish()

  End Sub

  Private Sub MakeCone(ByVal r0 As Decimal, ByVal r1 As Decimal, ByVal ymin As Single, ByVal ymax As Single)
    ' Top.
    Dim umin As Single = 0
    Dim wmin As Single = 0
    Dim vmin As Single = 0
    Dim umax As Single = 1
    Dim wmax As Single = 1
    Dim vmax As Single = 1
    Dim i As Integer

    ymin = ymin - velkostObj.L / 2
    ymax = ymax - velkostObj.L / 2

    Dim vertx As New ArrayList
    Dim vertz As New ArrayList

    Dim casti As Integer
    casti = valecDetail

    For i = 0 To casti - 1
      Dim angle As Decimal = Math.PI * 2 * (i / casti)
      Dim s As Decimal = Math.Sin(angle)
      Dim c As Decimal = Math.Cos(angle)
      vertx.Add(s)
      vertz.Add(c)
    Next

    'vonku
    For i = casti - 1 To 0 Step -1

      If i = 0 Then
        MakeRectanglePNT( _
             vertx.Item(i) * r1, ymin, vertz.Item(i) * r1, umin, vmin, _
             vertx.Item(i) * r1, ymax, vertz.Item(i) * r1, umin, vmax, _
             vertx.Item(casti - 1) * r1, ymax, vertz.Item(casti - 1) * r1, umax, vmax, _
             vertx.Item(casti - 1) * r1, ymin, vertz.Item(casti - 1) * r1, umax, vmin)
      Else
        MakeRectanglePNT( _
       vertx.Item(i) * r1, ymin, vertz.Item(i) * r1, umin, vmin, _
       vertx.Item(i) * r1, ymax, vertz.Item(i) * r1, umin, vmax, _
       vertx.Item(i - 1) * r1, ymax, vertz.Item(i - 1) * r1, umax, vmax, _
       vertx.Item(i - 1) * r1, ymin, vertz.Item(i - 1) * r1, umax, vmin)

      End If
    Next

    'vnutro
    For i = 0 To casti - 1

      If i = casti - 1 Then
        MakeRectanglePNT( _
             vertx.Item(i) * r0, ymin, vertz.Item(i) * r0, umin, vmin, _
             vertx.Item(i) * r0, ymax, vertz.Item(i) * r0, umin, vmax, _
             vertx.Item(0) * r0, ymax, vertz.Item(0) * r0, umax, vmax, _
             vertx.Item(0) * r0, ymin, vertz.Item(0) * r0, umax, vmin)
      Else
        MakeRectanglePNT( _
       vertx.Item(i) * r0, ymin, vertz.Item(i) * r0, umin, vmin, _
       vertx.Item(i) * r0, ymax, vertz.Item(i) * r0, umin, vmax, _
       vertx.Item(i + 1) * r0, ymax, vertz.Item(i + 1) * r0, umax, vmax, _
       vertx.Item(i + 1) * r0, ymin, vertz.Item(i + 1) * r0, umax, vmin)

      End If
    Next


    'vrch
    For i = 0 To casti - 1

      If i = casti - 1 Then
        MakeRectanglePNT( _
             vertx.Item(i) * r0, ymax, vertz.Item(i) * r0, umin, vmin, _
             vertx.Item(i) * r1, ymax, vertz.Item(i) * r1, umin, vmax, _
             vertx.Item(0) * r1, ymax, vertz.Item(0) * r1, umax, vmax, _
             vertx.Item(0) * r0, ymax, vertz.Item(0) * r0, umax, vmin)
      Else
        MakeRectanglePNT( _
       vertx.Item(i) * r0, ymax, vertz.Item(i) * r0, umin, vmin, _
       vertx.Item(i) * r1, ymax, vertz.Item(i) * r1, umin, vmax, _
       vertx.Item(i + 1) * r1, ymax, vertz.Item(i + 1) * r1, umax, vmax, _
       vertx.Item(i + 1) * r0, ymax, vertz.Item(i + 1) * r0, umax, vmin)

      End If
    Next


    'Zlava
    For i = casti - 1 To 0 Step -1

      If i = 0 Then
        MakeRectanglePNT( _
             vertx.Item(i) * r0, ymin, vertz.Item(i) * r0, umin, vmin, _
             vertx.Item(i) * r1, ymin, vertz.Item(i) * r1, umin, vmax, _
             vertx.Item(casti - 1) * r1, ymin, vertz.Item(casti - 1) * r1, umax, vmax, _
             vertx.Item(casti - 1) * r0, ymin, vertz.Item(casti - 1) * r0, umax, vmin)
      Else
        MakeRectanglePNT( _
       vertx.Item(i) * r0, ymin, vertz.Item(i) * r0, umin, vmin, _
       vertx.Item(i) * r1, ymin, vertz.Item(i) * r1, umin, vmax, _
       vertx.Item(i - 1) * r1, ymin, vertz.Item(i - 1) * r1, umax, vmax, _
       vertx.Item(i - 1) * r0, ymin, vertz.Item(i - 1) * r0, umax, vmin)

      End If
    Next
  End Sub


#End Region ' D3D Setup Code

  Protected Overrides Function GetFirstTriangle1() As Integer
    Return cmpCurrent.NedotknutaCast.Count * 8 * valecDetail + 36
  End Function

  Protected Overrides Function GetFirstTriangle2() As Integer
    Return cmpCurrent.NedotknutaCast.Count * 8 * valecDetail + cmpCurrent.CelyRez.Count * 8 * valecDetail + 36
  End Function

  Protected Overrides Sub ShowRezInfo()
    Dim rez As CoordinatesValec = cmpCurrent.rezPilou
    Dim ft As System.Drawing.Font
    Dim myFont As Direct3D.Font

    ft = New System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold)
    myFont = New Direct3D.Font(m_Device, ft)
    myFont.DrawText(Nothing, "D/d: " & CInt(rez.D1) & "/" & CInt(rez.d2), 10, 10, Color.Blue) ' System.Drawing.Color.Yellow.ToArgb()
    myFont.DrawText(Nothing, "L:     " & CInt(rez.L1) & "-" & CInt(rez.L2), 10, 25, Color.Orange)
    myFont.Dispose()
    myFont = Nothing
    ft.Dispose()
    ft = Nothing
    rez = Nothing
  End Sub

  Protected Overrides Sub RecomputeCore()
    If Not m_Device Is Nothing Then
      Dim i As Integer = iudQTY.Value
      If (Not i > iudQTY.Maximum) And (i > 0) Then
        cmpCurrent.Recompute(iudQTY.Value, chb_aD.Checked, chb_bd.Checked, chb_L.Checked)
        MyBase.FillLogInfo(cmpCurrent.ObjemVyrezu, cmpCurrent.ObjemOdrezku)
        CreateVertexBuffer()
      End If

    End If

  End Sub

End Class