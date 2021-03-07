Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D
Imports Microsoft.DirectX.Direct3D.D3DX
Imports System.Drawing.Imaging
Imports System.Collections.Generic

Friend Class SingleSimulationValec2
    Inherits SingleSimulation2

    Private valecDetail As Integer = 64
    Friend cmpCurrent As ComputationValec
    Friend ShowSimulation As Boolean
    Friend MSKey As String ' Key MS collekcie do ktorej patri
    Friend Event MaxSimulationValec()

    ' Data variables.

    Friend Function GraphicsDraw(ByVal iSimulationIndex As Integer,
                                 ByVal aaEnabled As Boolean,
                                 ByVal svpEnabled As Boolean,
                                 ByVal rotSpeed As Decimal) As Boolean
        ' Initialize the graphics device. Return True if successful.

        SimulationIndex = iSimulationIndex
        aaEnable = aaEnabled
        svpEnable = svpEnabled
        'rotateSpeed = rotSpeed

        angleY = Math.PI / 6
        angleX = Math.PI / 6

        m_MaxPocKS = cmpCurrent.FoundRezList.Count
        'iudQTY.Maximum = cmpCurrent.FoundRezList.Count
        'iudQTY.Value = cmpCurrent.FoundRezList.Count

        velkostObj = cmpCurrent.SourceObjShape.Size

        'lblPopis.Text = ""
        'If cmpCurrent.iValecType = ValecType.ValecType_Inner Then
        '    chb_bd.Checked = True
        '    chb_bd.Enabled = False
        '    chb_aD.Checked = False
        '    chb_aD.Enabled = False
        'ElseIf cmpCurrent.Rozmery.bd = 0 Then
        '    chb_bd.Checked = True
        '    chb_bd.Enabled = False
        'End If

        'defW = Me.Width
        'defH = Me.Height

        Dim sPopis As String = ""

        'MyBase.FillLogInfo(cmpCurrent.ObjemVyrezu, cmpCurrent.ObjemOdrezku)

        'Return MyBase.GraphicsDrawFinish()

    End Function

    'Protected Overrides Sub ShowRezInfo()
    '    Dim rez As CoordinatesValec = cmpCurrent.rezPilou
    '    Dim ft As System.Drawing.Font
    '    Dim myFont As Direct3D.Font

    '    ft = New System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold)
    '    myFont = New Direct3D.Font(m_Device, ft)
    '    myFont.DrawText(Nothing, "D/d: " & CInt(rez.D1) & "/" & CInt(rez.d2), 10, 10, Color.Blue) ' System.Drawing.Color.Yellow.ToArgb()
    '    myFont.DrawText(Nothing, "L:     " & CInt(rez.L1) & "-" & CInt(rez.L2), 10, 25, Color.Orange)
    '    myFont.Dispose()
    '    myFont = Nothing
    '    ft.Dispose()
    '    ft = Nothing
    '    rez = Nothing
    'End Sub

    'Protected Overrides Sub RecomputeCore()
    '    If Not m_Device Is Nothing Then
    '        Dim i As Integer = iudQTY.Value
    '        If (Not i > iudQTY.Maximum) And (i > 0) Then
    '            cmpCurrent.Recompute(iudQTY.Value, chb_aD.Checked, chb_bd.Checked, chb_L.Checked)
    '            MyBase.FillLogInfo(cmpCurrent.ObjemVyrezu, cmpCurrent.ObjemOdrezku)
    '            CreateVertexBuffer()
    '        End If

    '    End If

    'End Sub

End Class