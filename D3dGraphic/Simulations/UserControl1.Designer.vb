<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserControl1
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserControl1))
    Me.UiGroupBox1 = New Janus.Windows.EditControls.UIGroupBox()
    Me.RibbonStatusBar1 = New Janus.Windows.Ribbon.RibbonStatusBar()
    Me.iudQTY = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
    Me.ContainerControlCommand1 = New Janus.Windows.Ribbon.ContainerControlCommand()
    Me.LabelCommand1 = New Janus.Windows.Ribbon.LabelCommand()
    Me.LabelCommand2 = New Janus.Windows.Ribbon.LabelCommand()
    Me.LabelCommand3 = New Janus.Windows.Ribbon.LabelCommand()
    Me.ButtonGroup1 = New Janus.Windows.Ribbon.ButtonGroup()
    Me.ButtonCommand3 = New Janus.Windows.Ribbon.ButtonCommand()
    Me.ButtonCommand4 = New Janus.Windows.Ribbon.ButtonCommand()
    Me.ButtonCommand5 = New Janus.Windows.Ribbon.ButtonCommand()
    Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
    Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox()
    Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
    Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
    Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
    Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
    Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
    Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
    CType(Me.UiGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.UiGroupBox1.SuspendLayout()
    Me.RibbonStatusBar1.SuspendLayout()
    Me.ToolStrip1.SuspendLayout()
    Me.SuspendLayout()
    '
    'UiGroupBox1
    '
    Me.UiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.Rebar
    Me.UiGroupBox1.Controls.Add(Me.RibbonStatusBar1)
    Me.UiGroupBox1.Controls.Add(Me.ToolStrip1)
    Me.UiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.UiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None
    Me.UiGroupBox1.Location = New System.Drawing.Point(0, 0)
    Me.UiGroupBox1.Name = "UiGroupBox1"
    Me.UiGroupBox1.Size = New System.Drawing.Size(402, 253)
    Me.UiGroupBox1.TabIndex = 5
    Me.UiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007
    '
    'RibbonStatusBar1
    '
    Me.RibbonStatusBar1.Controls.Add(Me.iudQTY)
    Me.RibbonStatusBar1.Dock = System.Windows.Forms.DockStyle.Top
    Me.RibbonStatusBar1.ImageSize = New System.Drawing.Size(16, 16)
    Me.RibbonStatusBar1.LeftPanelCommands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.ContainerControlCommand1, Me.LabelCommand1, Me.LabelCommand2, Me.LabelCommand3})
    Me.RibbonStatusBar1.Location = New System.Drawing.Point(0, 25)
    Me.RibbonStatusBar1.Name = "RibbonStatusBar1"
    Me.RibbonStatusBar1.Office2007CustomColor = System.Drawing.Color.Empty
    Me.RibbonStatusBar1.RightPanelCommands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.ButtonGroup1})
    Me.RibbonStatusBar1.Size = New System.Drawing.Size(402, 23)
    '
    '
    '
    Me.RibbonStatusBar1.SuperTipComponent.AutoPopDelay = 2000
    Me.RibbonStatusBar1.SuperTipComponent.ImageList = Nothing
    Me.RibbonStatusBar1.TabIndex = 2
    Me.RibbonStatusBar1.Text = "RibbonStatusBar1"
    Me.RibbonStatusBar1.UseCompatibleTextRendering = False
    '
    'iudQTY
    '
    Me.iudQTY.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(251, Byte), Integer))
    Me.iudQTY.Location = New System.Drawing.Point(0, 2)
    Me.iudQTY.Minimum = 1
    Me.iudQTY.Name = "iudQTY"
    Me.iudQTY.Size = New System.Drawing.Size(100, 20)
    Me.iudQTY.TabIndex = 7
    Me.iudQTY.Value = 1
    Me.iudQTY.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
    '
    'ContainerControlCommand1
    '
    Me.ContainerControlCommand1.Control = Me.iudQTY
    Me.ContainerControlCommand1.Key = "ContainerControlCommand1"
    Me.ContainerControlCommand1.Name = "ContainerControlCommand1"
    Me.ContainerControlCommand1.SuperTipSettings.ImageListProvider = Me.ContainerControlCommand1
    Me.ContainerControlCommand1.Text = ""
    '
    'LabelCommand1
    '
    Me.LabelCommand1.Key = "LabelCommand1"
    Me.LabelCommand1.Name = "LabelCommand1"
    Me.LabelCommand1.SuperTipSettings.ImageListProvider = Me.LabelCommand1
    Me.LabelCommand1.Text = "z 30"
    '
    'LabelCommand2
    '
    Me.LabelCommand2.Key = "LabelCommand2"
    Me.LabelCommand2.Name = "LabelCommand2"
    Me.LabelCommand2.SuperTipSettings.ImageListProvider = Me.LabelCommand2
    Me.LabelCommand2.Text = "rez"
    '
    'LabelCommand3
    '
    Me.LabelCommand3.Key = "LabelCommand3"
    Me.LabelCommand3.Name = "LabelCommand3"
    Me.LabelCommand3.SuperTipSettings.ImageListProvider = Me.LabelCommand3
    Me.LabelCommand3.Text = "zvysok"
    '
    'ButtonGroup1
    '
    Me.ButtonGroup1.Commands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.ButtonCommand3, Me.ButtonCommand4, Me.ButtonCommand5})
    Me.ButtonGroup1.Key = "ButtonGroup1"
    Me.ButtonGroup1.Name = "ButtonGroup1"
    '
    'ButtonCommand3
    '
    Me.ButtonCommand3.Image = CType(resources.GetObject("ButtonCommand3.Image"), System.Drawing.Image)
    Me.ButtonCommand3.Key = "ButtonCommand1"
    Me.ButtonCommand3.Name = "ButtonCommand3"
    Me.ButtonCommand3.SuperTipSettings.ImageListProvider = Me.ButtonCommand3
    Me.ButtonCommand3.Text = ""
    '
    'ButtonCommand4
    '
    Me.ButtonCommand4.Checked = True
    Me.ButtonCommand4.CheckOnClick = True
    Me.ButtonCommand4.Image = CType(resources.GetObject("ButtonCommand4.Image"), System.Drawing.Image)
    Me.ButtonCommand4.Key = "ButtonCommand2"
    Me.ButtonCommand4.Name = "ButtonCommand4"
    Me.ButtonCommand4.SuperTipSettings.ImageListProvider = Me.ButtonCommand4
    Me.ButtonCommand4.Text = ""
    '
    'ButtonCommand5
    '
    Me.ButtonCommand5.Key = "ButtonCommand3"
    Me.ButtonCommand5.Name = "ButtonCommand5"
    Me.ButtonCommand5.SuperTipSettings.ImageListProvider = Me.ButtonCommand5
    Me.ButtonCommand5.Text = ""
    '
    'ToolStrip1
    '
    Me.ToolStrip1.BackColor = System.Drawing.Color.Transparent
    Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBox1, Me.ToolStripLabel3, Me.ToolStripLabel1, Me.ToolStripLabel2, Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripButton3})
    Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
    Me.ToolStrip1.Name = "ToolStrip1"
    Me.ToolStrip1.Size = New System.Drawing.Size(402, 25)
    Me.ToolStrip1.Stretch = True
    Me.ToolStrip1.TabIndex = 1
    Me.ToolStrip1.Text = "ToolStrip1"
    '
    'ToolStripComboBox1
    '
    Me.ToolStripComboBox1.DropDownWidth = 75
    Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
    Me.ToolStripComboBox1.Size = New System.Drawing.Size(75, 25)
    '
    'ToolStripLabel3
    '
    Me.ToolStripLabel3.Name = "ToolStripLabel3"
    Me.ToolStripLabel3.Size = New System.Drawing.Size(38, 22)
    Me.ToolStripLabel3.Text = "( z 30)"
    '
    'ToolStripLabel1
    '
    Me.ToolStripLabel1.Name = "ToolStripLabel1"
    Me.ToolStripLabel1.Size = New System.Drawing.Size(22, 22)
    Me.ToolStripLabel1.Text = "rez"
    '
    'ToolStripLabel2
    '
    Me.ToolStripLabel2.Name = "ToolStripLabel2"
    Me.ToolStripLabel2.Size = New System.Drawing.Size(42, 22)
    Me.ToolStripLabel2.Text = "zvysok"
    '
    'ToolStripButton1
    '
    Me.ToolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
    Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
    Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
    Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.ToolStripButton1.Name = "ToolStripButton1"
    Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
    Me.ToolStripButton1.Text = "ToolStripButton1"
    '
    'ToolStripButton2
    '
    Me.ToolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
    Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
    Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
    Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.ToolStripButton2.Name = "ToolStripButton2"
    Me.ToolStripButton2.Size = New System.Drawing.Size(23, 22)
    Me.ToolStripButton2.Text = "ToolStripButton2"
    '
    'ToolStripButton3
    '
    Me.ToolStripButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
    Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
    Me.ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), System.Drawing.Image)
    Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.ToolStripButton3.Name = "ToolStripButton3"
    Me.ToolStripButton3.Size = New System.Drawing.Size(23, 22)
    Me.ToolStripButton3.Text = "ToolStripButton3"
    '
    'UserControl1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.UiGroupBox1)
    Me.Name = "UserControl1"
    Me.Size = New System.Drawing.Size(402, 253)
    CType(Me.UiGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.UiGroupBox1.ResumeLayout(False)
    Me.UiGroupBox1.PerformLayout()
    Me.RibbonStatusBar1.ResumeLayout(False)
    Me.RibbonStatusBar1.PerformLayout()
    Me.ToolStrip1.ResumeLayout(False)
    Me.ToolStrip1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents UiGroupBox1 As Janus.Windows.EditControls.UIGroupBox
  Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
  Friend WithEvents ToolStripComboBox1 As System.Windows.Forms.ToolStripComboBox
  Friend WithEvents ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
  Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
  Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
  Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
  Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
  Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
  Friend WithEvents RibbonStatusBar1 As Janus.Windows.Ribbon.RibbonStatusBar
  Friend WithEvents ContainerControlCommand1 As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents ButtonGroup1 As Janus.Windows.Ribbon.ButtonGroup
  Friend WithEvents ButtonCommand3 As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents ButtonCommand4 As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents ButtonCommand5 As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents iudQTY As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents LabelCommand1 As Janus.Windows.Ribbon.LabelCommand
  Friend WithEvents LabelCommand2 As Janus.Windows.Ribbon.LabelCommand
  Friend WithEvents LabelCommand3 As Janus.Windows.Ribbon.LabelCommand

End Class
