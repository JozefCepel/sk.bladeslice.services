Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D
Imports Microsoft.DirectX.Direct3D.D3DX
Imports System.Drawing.Imaging
Imports System.Collections.Generic
Imports System.ComponentModel

Friend Class SingleSimulation
    ' The Direct3D device.
    Friend bTociSa As Boolean = False
    Protected m_Device As Device
    Protected liteVersion As Boolean = False
    Protected defW As Integer
    Protected defH As Integer
    Protected aaEnable As Boolean = False
    Protected svpEnable As Boolean = False
    Protected angleX As Decimal = 0
    Protected angleY As Decimal = 0   ' 9.5
    Protected velkostObj As New Coord_bod
    Protected m_MaxPocKS As Integer
    Protected m_Points As List(Of CustomVertex.PositionNormalTextured)

    Private maximized As Boolean = False
    Private WithEvents timerRotate As New Timer
    Private rotX As Single = 0
    Private rotY As Single = 0
    ' Capabilities.
    Private m_MaxPrimitives As Integer

    Private m_NumTriangles As Integer
    Private m_NumPoints As Integer
    ' The vertex buffer that holds drawing data.
    Private m_VertexBuffer As VertexBuffer = Nothing
    Private lastTickCount As Integer = 0

    ' The material.
    Private m_Material As Material

    ' The textures.
    Friend m_Texture As Texture
    Friend m_Texture2 As Texture
    Friend m_Texture3 As Texture
    Friend m_TextureBlue As Texture
    Friend m_TextureOrange As Texture
    Friend m_TextureLime As Texture

    Private rSpeed As Decimal = 10000
    Private _type As D3dSim

    Friend Event BeforeMaxSimulation(ByRef SingleSim As SingleSimulation)
    Friend Event AfterMaxSimulation(ByRef SingleSim As SingleSimulation)

    Friend Event CloseSimulation(ByRef SingleSim As SingleSimulation)
    Friend Event SelectedSimulation(ByRef SingleSim As SingleSimulation)
    Friend Event StartNewTocenie(ByRef SingleSim As SingleSimulation)

    Private bUseScreenShot As Boolean = False
    Private bSelected As Boolean
    Friend SimulationIndex As Integer

    Public Enum D3dSim
        D3dSim_Blok = 1
        D3dSim_Valec = 2
    End Enum

#Region "D3D Setup Code"

    Friend Sub ResetDevice()
        maximized = False
        defW = Me.Width
        defH = Me.Height
        ResetDeviceCore()
    End Sub

    Friend Sub ResetDeviceCore()
        Dim par As PresentParameters
        bUseScreenShot = False
        par = m_Device.PresentationParameters
        par.BackBufferHeight = pic3d.Height
        par.BackBufferWidth = pic3d.Width
        If aaEnable Then par.MultiSample = Direct3D.MultiSampleType.FourSamples Else par.MultiSample = Direct3D.MultiSampleType.None
        m_Device.Reset(par)
        setParameters()
        'Render()
    End Sub

    Private Sub setParameters()
        ' Get the maximum number of primitives we can render in one call to DrawPrimitives.
        Dim c As Caps = m_Device.DeviceCaps
        m_MaxPrimitives = c.MaxPrimitiveCount
        'Debug.WriteLine("MaxPrimitives: " & m_MaxPrimitives)

        m_Device.SetRenderState(Direct3D.RenderStates.VertexBlend, VertexBlend.Tweening) 'Povodne len pre BLOK
        m_Device.SetRenderState(Direct3D.RenderStates.ShadeMode, ShadeMode.Phong) 'Povodne len pre BLOK

        ' This seems to work better than using the
        ' actual value, at least in program d3dGasket.
        m_MaxPrimitives = 10000

        ' Turn on D3D lighting.
        m_Device.RenderState.Lighting = True
        m_Device.RenderState.ShadeMode = ShadeMode.Phong      'Povodne len pre BLOK
        m_Device.RenderState.VertexBlend = VertexBlend.Tweening 'Povodne len pre BLOK
        ' Turn on the Z-buffer.
        m_Device.RenderState.ZBufferEnable = True

        ' Cull triangles that are oriented counter clockwise.
        m_Device.RenderState.CullMode = Cull.CounterClockwise

        ' Make points bigger so they're easy to see.
        m_Device.RenderState.PointSize = 4

        ' Create the vertex data.
        CreateVertexBuffer()

        ' Create texture data.
        CreateTextures()

        ' Make the material.
        SetupMaterial()

        ' Make the lights.
        SetupLights()

        ' We succeeded.
    End Sub

    Protected Sub CreateVertexBufferFinish()

        bUseScreenShot = False

        ' Create a buffer.
        m_NumPoints = m_Points.Count
        m_NumTriangles = m_NumPoints \ 3
        'Debug.WriteLine("# Points:    " & m_NumPoints)
        'Debug.WriteLine("# Triangles: " & m_NumTriangles)
        'Debug.WriteLine("")
        m_VertexBuffer = New VertexBuffer(
            GetType(CustomVertex.PositionNormalTextured),
            m_NumPoints, m_Device, 0,
            CustomVertex.PositionNormalTextured.Format,
            Pool.Default)

        ' Lock the vertex buffer. 
        ' Lock returns an array of PositionNormalTextured objects.
        Dim vertices As CustomVertex.PositionNormalTextured() =
            CType(m_VertexBuffer.Lock(0, 0),
                CustomVertex.PositionNormalTextured())

        ' Copy the vertexes into the buffer.
        For i As Integer = 0 To m_NumPoints - 1
            vertices(i) = m_Points(i)
        Next i

        m_VertexBuffer.Unlock()
        m_Device.Transform.World = Matrix.RotationYawPitchRoll(angleX, angleY, 0)

    End Sub

    Protected Function GraphicsDrawFinish() As Boolean
        Dim params As New PresentParameters
        With params
            params.Windowed = True
            params.SwapEffect = SwapEffect.Discard
            params.EnableAutoDepthStencil = True     ' Depth stencil on.
            params.AutoDepthStencilFormat = DepthFormat.D16

            '.Windowed = True
            '.SwapEffect = SwapEffect.Discard
            '.EnableAutoDepthStencil = True
            '.AutoDepthStencilFormat = DepthFormat.D16
            '.FullScreenRefreshRateInHz = PresentParameters.DefaultPresentRate
            '.PresentationInterval = PresentInterval.One
            '.BackBufferCount = 1
            '.BackBufferFormat = Direct3D.Manager.Adapters(0).CurrentDisplayMode.Format
            '.BackBufferWidth = pic3d.ClientSize.Width
            '.BackBufferHeight = pic3d.ClientSize.Height
        End With

        If aaEnable Then params.MultiSample = Direct3D.MultiSampleType.FourSamples
        If Not svpEnable Then 'Uzivatel vyslovene nechce HardwareVertexProcessing
            ' Best: Hardware device and hardware vertex processing.
            Try
                m_Device = New Device(0, DeviceType.Hardware, pic3d, CreateFlags.HardwareVertexProcessing, params)
                'Debug.WriteLine("Hardware, HardwareVertexProcessing")
            Catch
            End Try
        End If

        ' Good: Hardware device and software vertex processing.
        If m_Device Is Nothing Then
            Try
                m_Device = New Device(0, DeviceType.Hardware, pic3d, CreateFlags.SoftwareVertexProcessing, params)
                'Debug.WriteLine("Hardware, SoftwareVertexProcessing")
            Catch
            End Try
        End If

        ' Adequate?: Software device and software vertex processing.
        If m_Device Is Nothing Then
            Try
                m_Device = New Device(0, DeviceType.Reference, pic3d, CreateFlags.SoftwareVertexProcessing, params)
                ' Debug.WriteLine("Reference, SoftwareVertexProcessing")
            Catch ex As Exception
                ' If we still can't make a device, give up.
                MessageBox.Show("Error initializing Direct3D" & vbCrLf & vbCrLf & ex.Message, "Direct3D Error", MessageBoxButtons.OK)
                Return False
            End Try
        End If
        setParameters()
        Return True

    End Function

    ' Make a box.
    Protected Sub MakeCube(ByVal xmin As Single, ByVal ymin As Single, ByVal zmin As Single, ByVal xmax As Single, ByVal ymax As Single, ByVal zmax As Single)
        ' Top.
        Dim umin As Single = 0
        Dim wmin As Single = 0
        Dim vmin As Single = 0
        Dim umax As Single = 1
        Dim wmax As Single = 1
        Dim vmax As Single = 1

        xmin = xmin - (velkostObj.aD / 2)
        xmax = xmax - (velkostObj.aD / 2)
        ymin = ymin - (velkostObj.L / 2)
        ymax = ymax - (velkostObj.L / 2)
        zmin = zmin - (velkostObj.bd / 2)
        zmax = zmax - (velkostObj.bd / 2)

        MakeRectanglePNT(
            xmin, ymax, zmin, umin, wmin,
            xmin, ymax, zmax, umin, wmax,
            xmax, ymax, zmax, umax, wmax,
            xmax, ymax, zmin, umax, wmin)
        ' Bottom.
        MakeRectanglePNT(
            xmax, ymin, zmin, umax, wmin,
            xmax, ymin, zmax, umax, wmax,
            xmin, ymin, zmax, umin, wmax,
            xmin, ymin, zmin, umin, wmin)
        ' Front.
        MakeRectanglePNT(
            xmin, ymin, zmin, umin, vmin,
            xmin, ymax, zmin, umin, vmax,
            xmax, ymax, zmin, umax, vmax,
            xmax, ymin, zmin, umax, vmin)
        ' Back.
        MakeRectanglePNT(
            xmax, ymin, zmax, umax, vmin,
            xmax, ymax, zmax, umax, vmax,
            xmin, ymax, zmax, umin, vmax,
            xmin, ymin, zmax, umin, vmin)
        ' Left.
        MakeRectanglePNT(
            xmin, ymin, zmax, wmax, vmin,
            xmin, ymax, zmax, wmax, vmax,
            xmin, ymax, zmin, wmin, vmax,
            xmin, ymin, zmin, wmin, vmin)
        ' Right.
        MakeRectanglePNT(
            xmax, ymin, zmin, wmin, vmin,
            xmax, ymax, zmin, wmin, vmax,
            xmax, ymax, zmax, wmax, vmax,
            xmax, ymin, zmax, wmax, vmin)
    End Sub

    ' Add two triangles to make a rectangle to the vertex buffer.
    Protected Sub MakeRectanglePNT(
        ByVal x0 As Single, ByVal y0 As Single, ByVal z0 As Single, ByVal u0 As Single, ByVal v0 As Single,
        ByVal x1 As Single, ByVal y1 As Single, ByVal z1 As Single, ByVal u1 As Single, ByVal v1 As Single,
        ByVal x2 As Single, ByVal y2 As Single, ByVal z2 As Single, ByVal u2 As Single, ByVal v2 As Single,
        ByVal x3 As Single, ByVal y3 As Single, ByVal z3 As Single, ByVal u3 As Single, ByVal v3 As Single)

        Dim vec0 As New Vector3(x1 - x0, y1 - y0, z1 - z0)
        Dim vec1 As New Vector3(x2 - x1, y2 - y1, z2 - z1)
        Dim n As Vector3 = Vector3.Cross(vec0, vec1)
        n.Normalize()

        m_Points.Add(New CustomVertex.PositionNormalTextured(x0, y0, z0, n.X, n.Y, n.Z, u0, v0))
        m_Points.Add(New CustomVertex.PositionNormalTextured(x1, y1, z1, n.X, n.Y, n.Z, u1, v1))
        m_Points.Add(New CustomVertex.PositionNormalTextured(x2, y2, z2, n.X, n.Y, n.Z, u2, v2))

        m_Points.Add(New CustomVertex.PositionNormalTextured(x0, y0, z0, n.X, n.Y, n.Z, u0, v0))
        m_Points.Add(New CustomVertex.PositionNormalTextured(x2, y2, z2, n.X, n.Y, n.Z, u2, v2))
        m_Points.Add(New CustomVertex.PositionNormalTextured(x3, y3, z3, n.X, n.Y, n.Z, u3, v3))
    End Sub

    ' Add a triangle to the vertex buffer.
    Private Sub MakeTriangle(ByVal vertices() As CustomVertex.PositionNormalTextured, ByRef i As Integer, ByVal clr As Color,
        ByVal x0 As Single, ByVal y0 As Single, ByVal z0 As Single,
        ByVal x1 As Single, ByVal y1 As Single, ByVal z1 As Single,
        ByVal x2 As Single, ByVal y2 As Single, ByVal z2 As Single)

        vertices(i).X = x0
        vertices(i).Y = y0
        vertices(i).Z = z0
        vertices(i).Tu = 0
        vertices(i).Tv = 1
        i += 1

        vertices(i).X = x1
        vertices(i).Y = y1
        vertices(i).Z = z1
        vertices(i).Tu = 0
        vertices(i).Tv = 0
        i += 1

        vertices(i).X = x2
        vertices(i).Y = y2
        vertices(i).Z = z2
        vertices(i).Tu = 1
        vertices(i).Tv = 0
        i += 1
    End Sub

    ' Create textures used for drawing.
    Private Sub CreateTextures()
        Dim bmp_stream As IO.MemoryStream

        bmp_stream = New IO.MemoryStream()
        My.Resources.Seda.Save(bmp_stream, ImageFormat.Png)
        bmp_stream.Seek(0, IO.SeekOrigin.Begin)
        m_Texture = TextureLoader.FromStream(m_Device, bmp_stream)

        bmp_stream.Close()

        bmp_stream = New IO.MemoryStream()
        My.Resources.green.Save(bmp_stream, ImageFormat.Jpeg)
        bmp_stream.Seek(0, IO.SeekOrigin.Begin)
        m_Texture3 = TextureLoader.FromStream(m_Device, bmp_stream)

        bmp_stream.Close()

        bmp_stream = New IO.MemoryStream()
        If Type = D3dSim.D3dSim_Blok Then
            My.Resources.Red.Save(bmp_stream, ImageFormat.Jpeg)
        Else
            My.Resources.valec.Save(bmp_stream, ImageFormat.Png)
        End If
        bmp_stream.Seek(0, IO.SeekOrigin.Begin)
        m_Texture2 = TextureLoader.FromStream(m_Device, bmp_stream)


        bmp_stream.Close()

        bmp_stream = New IO.MemoryStream()
        My.Resources.Blue.Save(bmp_stream, ImageFormat.Png)
        bmp_stream.Seek(0, IO.SeekOrigin.Begin)
        m_TextureBlue = TextureLoader.FromStream(m_Device, bmp_stream)


        bmp_stream.Close()

        bmp_stream = New IO.MemoryStream()
        My.Resources.Orange.Save(bmp_stream, ImageFormat.Png)
        bmp_stream.Seek(0, IO.SeekOrigin.Begin)
        m_TextureOrange = TextureLoader.FromStream(m_Device, bmp_stream)


        bmp_stream.Close()

        bmp_stream = New IO.MemoryStream()
        My.Resources.Lime.Save(bmp_stream, ImageFormat.Png)
        bmp_stream.Seek(0, IO.SeekOrigin.Begin)
        m_TextureLime = TextureLoader.FromStream(m_Device, bmp_stream)


        bmp_stream.Close()
        bmp_stream.Dispose()
        bmp_stream = Nothing
        ' Use the texture.

    End Sub

#End Region ' D3D Setup Code

#Region "D3D Drawing Code"

    Protected Overridable Function GetFirstTriangle1() As Integer
        Throw New NotImplementedException("You must implement this - GetFirstTriangle1!")
    End Function
    Protected Overridable Function GetFirstTriangle2() As Integer
        Throw New NotImplementedException("You must implement this - GetFirstTriangle2!")
    End Function
    Protected Overridable Sub ShowRezInfo()
        Throw New NotImplementedException("You must implement this - ShowRezInfo!")
    End Sub

    ' Draw.
    Friend Sub Render()
        Dim exist As Boolean

        Try
            exist = Not m_Device.Disposed
        Catch ex As NullReferenceException
            exist = False
        End Try

        If bUseScreenShot Then Exit Sub

        'Debug.Print("Renderujem " & Me.SimulationIndex.ToString)
        If (exist) Then
            ' Clear the back buffer and the Z-buffer.
            m_Device.Clear(ClearFlags.Target Or ClearFlags.ZBuffer, Color.White, 1, 0)
            ' Make a scene.
            m_Device.BeginScene()

            ShowRezInfo()

            '  m_Device.SetRenderState(Microsoft.DirectX.Direct3D.RenderStates.SourceBlend, Microsoft.DirectX.Direct3D.Blend.SourceAlpha)
            'm_Device.SetRenderState(Microsoft.DirectX.Direct3D.RenderStates.DestinationBlend, Microsoft.DirectX.Direct3D.Blend.InvSourceAlpha)

            ' m_Device.SetTextureStageState(0, Direct3D.TextureStageStates.ColorOperation, Direct3D.TextureOperation.SelectArg1)
            'm_Device.SetTextureStageState(0, Direct3D.TextureStageStates.ColorArgument1, Direct3D.TextureArgument.TextureColor)
            'm_Device.SetTextureStageState(0, Direct3D.TextureStageStates.ColorArgument2, Direct3D.TextureArgument.Diffuse)

            ' Draw stuff here...
            ' Setup the world, view, and projection matrices.

            SetupMatrices()
            ' Tell the device the format of the vertices.
            m_Device.VertexFormat = CustomVertex.PositionNormalTextured.Format
            ' Tell the device about the material.
            m_Device.Material = m_Material
            ' Set the device's data stream source (the vertex buffer).
            m_Device.SetStreamSource(0, m_VertexBuffer, 0)
            ' Draw in groups.
            m_Device.SetTexture(0, m_Texture)
            Dim first_triangle As Integer = 36
            Dim num_triangles As Integer = m_MaxPrimitives
            Dim m_NumTriangle As Integer = m_NumTriangles
            Do While first_triangle < m_NumTriangle
                If first_triangle + num_triangles > m_NumTriangle Then
                    num_triangles = m_NumTriangles - first_triangle
                End If
                m_Device.DrawPrimitives(PrimitiveType.TriangleList, first_triangle * 3, num_triangles)
                '@ Debug.WriteLine("Draw " & first_triangle * 3 & ", " & num_triangles) '@
                first_triangle += num_triangles
            Loop

            m_Device.SetTexture(0, m_Texture3)
            m_Device.Clear(ClearFlags.ZBuffer, Color.White, 1, 0)

            first_triangle = GetFirstTriangle1()
            num_triangles = m_MaxPrimitives
            m_NumTriangle = m_NumTriangles
            Do While first_triangle < m_NumTriangle
                If first_triangle + num_triangles > m_NumTriangle Then
                    num_triangles = m_NumTriangles - first_triangle
                End If
                m_Device.DrawPrimitives(PrimitiveType.TriangleList, first_triangle * 3, num_triangles)
                '@ Debug.WriteLine("Draw " & first_triangle * 3 & ", " & num_triangles) '@
                first_triangle += num_triangles
            Loop

            m_Device.SetTexture(0, m_Texture2)
            m_Device.Clear(ClearFlags.ZBuffer, Color.White, 1, 0)

            first_triangle = GetFirstTriangle2()

            num_triangles = m_MaxPrimitives
            m_NumTriangle = m_NumTriangles
            Do While first_triangle < m_NumTriangle
                If first_triangle + num_triangles > m_NumTriangle Then
                    num_triangles = m_NumTriangles - first_triangle
                End If
                m_Device.DrawPrimitives(PrimitiveType.TriangleList, first_triangle * 3, num_triangles)
                '@ Debug.WriteLine("Draw " & first_triangle * 3 & ", " & num_triangles) '@
                first_triangle += num_triangles
            Loop

            m_Device.SetTexture(0, m_TextureBlue)

            first_triangle = 0
            num_triangles = 12
            m_NumTriangle = 12
            Do While first_triangle < m_NumTriangle
                If first_triangle + num_triangles > m_NumTriangle Then
                    num_triangles = m_NumTriangles - first_triangle
                End If
                m_Device.DrawPrimitives(PrimitiveType.TriangleList, first_triangle * 3, num_triangles)
                '@ Debug.WriteLine("Draw " & first_triangle * 3 & ", " & num_triangles) '@
                first_triangle += num_triangles
            Loop

            m_Device.SetTexture(0, m_TextureLime)

            first_triangle = 12
            num_triangles = 12
            m_NumTriangle = 24
            Do While first_triangle < m_NumTriangle
                If first_triangle + num_triangles > m_NumTriangle Then
                    num_triangles = m_NumTriangles - first_triangle
                End If
                m_Device.DrawPrimitives(PrimitiveType.TriangleList, first_triangle * 3, num_triangles)
                '@ Debug.WriteLine("Draw " & first_triangle * 3 & ", " & num_triangles) '@
                first_triangle += num_triangles
            Loop

            m_Device.SetTexture(0, m_TextureOrange)

            first_triangle = 24
            num_triangles = 12
            m_NumTriangle = 36
            Do While first_triangle < m_NumTriangle
                If first_triangle + num_triangles > m_NumTriangle Then
                    num_triangles = m_NumTriangles - first_triangle
                End If
                m_Device.DrawPrimitives(PrimitiveType.TriangleList, first_triangle * 3, num_triangles)
                '@ Debug.WriteLine("Draw " & first_triangle * 3 & ", " & num_triangles) '@
                first_triangle += num_triangles
            Loop

            ' End the scene and display.
            m_Device.EndScene()
            m_Device.Present()

            If Not bTociSa Then DoScreenShot()
        End If
    End Sub

    ' Setup the world, view, and projection matrices.
    Private Sub SetupMatrices()

        ' World Matrix:
        Const TICKS_PER_REV As Integer = 1

        If lastTickCount = 0 Or lastTickCount > Environment.TickCount Then
            'Nerob nic
        ElseIf bTociSa Then
            Dim diff As Integer = Environment.TickCount - lastTickCount
            rotX = rotX - Windows.Forms.Cursor.Position.X
            rotY = rotY - Windows.Forms.Cursor.Position.Y
            If My.Computer.Keyboard.ShiftKeyDown Then
                angleX += (diff / TICKS_PER_REV) * (rotX / rSpeed)
                m_Device.Transform.World = Matrix.RotationYawPitchRoll(0, 0, angleX)
            ElseIf My.Computer.Keyboard.CtrlKeyDown Then
                angleY += (diff / TICKS_PER_REV) * (rotX / rSpeed)
                m_Device.Transform.World = Matrix.RotationYawPitchRoll(0, angleY, 0)
            ElseIf My.Computer.Keyboard.AltKeyDown Then
                angleX += (diff / TICKS_PER_REV) * (rotX / rSpeed)
                m_Device.Transform.World = Matrix.RotationYawPitchRoll(angleX, 0, 0)
            Else
                angleX += (diff / TICKS_PER_REV) * (rotX / rSpeed)
                angleY += (diff / TICKS_PER_REV) * (rotY / rSpeed)
                m_Device.Transform.World = Matrix.RotationYawPitchRoll(angleX, angleY, 0)
            End If
            'Debug.Print(angleX & " " & angleY & " " & angleZ)
            rotX = Windows.Forms.Cursor.Position.X
            rotY = Windows.Forms.Cursor.Position.Y
        End If

        lastTickCount = Environment.TickCount

        Dim vzdialenost As Decimal = velkostObj.aD
        If vzdialenost < velkostObj.L Then vzdialenost = velkostObj.L
        If vzdialenost < velkostObj.bd Then vzdialenost = velkostObj.bd

        ' View Matrix:
        m_Device.Transform.View = Matrix.LookAtLH(
            New Vector3(0, 0, -2.5 * vzdialenost),
            New Vector3(0, 0, 0),
            New Vector3(0, 1, 0))

        ' Projection Matrix:
        ' Perspective transformation defined by:
        '       Field of view           Pi / 4
        '       Aspect ratio            1
        '       Near clipping plane     Z = 1
        '       Far clipping plane      Z = 100
        m_Device.Transform.Projection =
            Matrix.PerspectiveFovLH(Math.PI / 4, pic3d.Width / pic3d.Height, 1, 5 * vzdialenost)

    End Sub

    ' Make the material.
    Private Sub SetupMaterial()
        m_Material = New Material()
        m_Material.Ambient = Color.FromArgb(255, 32, 32, 32)
        m_Material.Diffuse = Color.White
        m_Device.Material = m_Material
    End Sub

    ' Make the lights.
    Private Sub SetupLights()
        ' Make a light.

        m_Device.Lights(0).Type = LightType.Directional
        m_Device.Lights(0).Diffuse = Color.White
        m_Device.Lights(0).Ambient = Color.White
        m_Device.Lights(0).Direction = New Vector3(2, -2, 3)
        m_Device.Lights(0).Enabled = True

        ' Add some ambient light.
        m_Device.RenderState.Ambient = Color.White
    End Sub

#End Region ' D3D Drawing Code

    <Browsable(True)>
    Public Property Type() As D3dSim
        Get
            Return _type
        End Get
        Set(ByVal value As D3dSim)
            _type = value
            lblPopis.Visible = (Type = D3dSim.D3dSim_Blok)
            cbComputations.Visible = (Type = D3dSim.D3dSim_Blok)
            chb_aD.Enabled = (Type = D3dSim.D3dSim_Blok)
            chb_L.Enabled = (Type = D3dSim.D3dSim_Blok)
            If Type = D3dSim.D3dSim_Blok Then
                lbl_aD.Text = "a"
                lbl_bd.Text = "b"
                pic3d.Height = cbComputations.Top - pic3d.Top - 10
            Else
                chb_aD.Checked = True
                chb_L.Checked = False
                'If velkostObj.bd = 0 Then
                '  chb_bd.Checked = True
                '  chb_bd.Enabled = False
                'End If
                lbl_aD.Text = "D"
                lbl_bd.Text = "d"
                pic3d.Height = Me.Height - pic3d.Top - 10
            End If
        End Set
    End Property

    Friend Property rotateSpeed() As Decimal
        Get
            Return rSpeed
        End Get
        Set(ByVal value As Decimal)
            rSpeed = value
        End Set
    End Property

    Friend Property enabledAA() As Boolean
        Get
            Return aaEnable
        End Get

        Set(ByVal value As Boolean)
            aaEnable = value
        End Set
    End Property

    Friend Property enabledSVP() As Boolean
        Get
            Return svpEnable
        End Get

        Set(ByVal value As Boolean)
            svpEnable = value
        End Set
    End Property

    Friend ReadOnly Property maximize() As Boolean
        Get
            Return maximized
        End Get

    End Property

    Private Sub timerRotate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timerRotate.Tick
        Render()
        'time.Stop()
    End Sub

    Private Sub CntsToRecompute_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chb_L.CheckedChanged,
                                                                                                        chb_bd.CheckedChanged,
                                                                                                        chb_aD.CheckedChanged,
                                                                                                        iudQTY.ValueChanged
        RecomputeCore()
    End Sub

    Protected Overridable Sub RecomputeCore()
        Debug.Print("You must implement this - RecomputeCore!")
    End Sub

    Private Sub btnMaximize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMaximize.Click
        MaximizeMinimize()
    End Sub

    Private Sub MaximizeMinimize()

        RaiseEvent BeforeMaxSimulation(Me)

        If Me._type = D3dSim.D3dSim_Blok Then
            If Not maximized Then
                If Me.Parent.Width > Me.Parent.Height Then
                    Me.Width = Me.Parent.Height - 20
                    Me.Height = Me.Parent.Height - 20
                Else
                    Me.Width = Me.Parent.Width - 30
                    Me.Height = Me.Parent.Width - 30
                End If
            Else
                Me.Width = defW
                Me.Height = defH
            End If
        End If
        maximized = Not maximized

        RaiseEvent AfterMaxSimulation(Me)
        ResetDeviceCore()

    End Sub

    Private Sub pic3d_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pic3d.Click
        RaiseEvent StartNewTocenie(Me)
        If bTociSa Then
            DoScreenShot()
            StartStopTocenie(False)
        Else
            bUseScreenShot = False
            StartStopTocenie(True)
            If pic3d.CanFocus Then pic3d.Focus()
        End If
    End Sub

    Friend Sub DoScreenShot() ' Urobi ScreenShot z renderovaneho okna alebo ho vycisti z pamati
        Dim sPicFile As String = Application.StartupPath & "\tmp.bmp"
        Dim Ssurface As Direct3D.Surface

        Try
            Dim current As Format = Direct3D.Manager.Adapters(0).CurrentDisplayMode.Format
            Ssurface = m_Device.GetBackBuffer(0, 0, BackBufferType.Mono)
            Direct3D.SurfaceLoader.Save(sPicFile, ImageFileFormat.Bmp, Ssurface)
            Ssurface.Dispose()
            Ssurface = Nothing

            'Debug.Print("Nacitavam obrazok " & Me.SimulationIndex)
            Dim ms As New System.IO.MemoryStream(My.Computer.FileSystem.ReadAllBytes(sPicFile))
            If pic3d.Image IsNot Nothing Then pic3d.Image.Dispose()
            pic3d.Image = Image.FromStream(ms) ' FromFile lockne subor, preto stream
            ms.Close()
            ms.Dispose()
            IO.File.Delete(sPicFile)
            bUseScreenShot = True
        Catch ex As Exception
            Debug.Print("Chyba v DoScreenShot: " & ex.Message)
        End Try
    End Sub

    Friend Sub StartStopTocenie(ByVal blnTocit As Boolean)
        If blnTocit Then
            timerRotate.Start()
            timerRotate.Interval = 50
            rotX = Windows.Forms.Cursor.Position.X
            rotY = Windows.Forms.Cursor.Position.Y
        Else
            timerRotate.Stop()
        End If
        bTociSa = blnTocit
    End Sub

    Private Sub pic3d_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pic3d.DoubleClick
        MaximizeMinimize()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Deactivujem simulaciu a potom az vyvolam event, ktorym si USER odstrani object z FLP
        RaiseEvent CloseSimulation(Me)
    End Sub

    Protected Overridable Sub cbComputations_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbComputations.SelectedIndexChanged
        Throw New NotImplementedException("You must implement this!")
    End Sub

    Protected Overridable Sub CreateVertexBuffer()
        ' Create the vertex data.
        Throw New NotImplementedException("You must implement this!")
    End Sub

    Private Sub Me_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        timerRotate.Enabled = False
        If Not m_Device Is Nothing Then
            m_Device.Dispose()
            m_Device = Nothing
        End If
        pic3d.Dispose()
        btnOk.Enabled = False
        btnCancel.Enabled = False
    End Sub

    Private Sub Me_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        If Me.SimulationIndex <> 0 Then Me.Render() ' Nerenderuj este pred inicializaciou
    End Sub

    Friend Sub RemovePaintEvent() ' Aby sa zbytocne neprekreslovalalo ked odstranujem po jednom z FLP
        RemoveHandler Me.Paint, AddressOf Me_Paint
    End Sub

    Protected Sub FillLogInfo(ByVal decVyrez As Decimal, ByVal decZvysok As Decimal)
        lblVyrez.Text = RoundItNice(decVyrez) & " /"
        lblZvysok.Text = RoundItNice(decZvysok)
        'Debug.Print(lblVyrez.Text + lblZvysok.Text)
    End Sub

    Private Sub iudQTY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iudQTY.ValueChanged
        lblMax.Visible = (iudQTY.Value < m_MaxPocKS)
        If iudQTY.Value < m_MaxPocKS Then
            lblMax.Text = "z " & m_MaxPocKS
        End If
        ' Roztahovanie LOG tabulky
        If lblMax.Visible Then
            tblLog.Left = lblMax.Left + lblMax.Width
            tblLog.Width = (btnOk.Left - lblMax.Left) - lblMax.Left
        Else ' Roztiahni viac ak je Max skryty
            tblLog.Left = lblMax.Left
            tblLog.Width = btnOk.Left - lblMax.Left
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        RaiseEvent SelectedSimulation(Me)
    End Sub

    Friend Sub HighLightSelected(ByVal blnSelected As Boolean)
        bSelected = blnSelected
        If bSelected Then
            gbSS.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Standard
        Else
            gbSS.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007
        End If
    End Sub

End Class

