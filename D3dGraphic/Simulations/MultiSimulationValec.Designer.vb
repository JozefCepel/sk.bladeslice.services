<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MultiSimulationValec
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
    Me.components = New System.ComponentModel.Container()
    Me.groupBox = New Janus.Windows.EditControls.UIGroupBox()
        Me.SSV = New D3dGraphic2.SingleSimulationValec()
        Me.lblSN = New System.Windows.Forms.Label()
    Me.TMReset = New System.Windows.Forms.Timer(Me.components)
    Me.TMResetAA = New System.Windows.Forms.Timer(Me.components)
    Me.lblPopis = New System.Windows.Forms.Label()
    Me.cbSNs = New Janus.Windows.EditControls.UIComboBox()
    CType(Me.groupBox, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.groupBox.SuspendLayout()
    Me.SuspendLayout()
    '
    'groupBox
    '
    Me.groupBox.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.ExplorerBarBackground
    Me.groupBox.Controls.Add(Me.lblPopis)
    Me.groupBox.Controls.Add(Me.cbSNs)
    Me.groupBox.Controls.Add(Me.SSV)
    Me.groupBox.Controls.Add(Me.lblSN)
    Me.groupBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.groupBox.FrameStyle = Janus.Windows.EditControls.FrameStyle.None
    Me.groupBox.Location = New System.Drawing.Point(0, 0)
    Me.groupBox.Name = "groupBox"
    Me.groupBox.Size = New System.Drawing.Size(251, 270)
    Me.groupBox.TabIndex = 21
    Me.groupBox.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007
    '
    'SSV
    '
    Me.SSV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.SSV.Location = New System.Drawing.Point(0, 25)
    Me.SSV.Name = "SSV"
    Me.SSV.Size = New System.Drawing.Size(251, 245)
    Me.SSV.TabIndex = 26
        Me.SSV.Type = D3dGraphic2.SingleSimulation.D3dSim.D3dSim_Valec
        '
        'lblSN
        '
        Me.lblSN.AutoSize = True
    Me.lblSN.BackColor = System.Drawing.Color.Transparent
    Me.lblSN.Location = New System.Drawing.Point(78, 8)
    Me.lblSN.Name = "lblSN"
    Me.lblSN.Size = New System.Drawing.Size(64, 13)
    Me.lblSN.TabIndex = 25
    Me.lblSN.Text = "Materiál: ##"
    '
    'TMReset
    '
    Me.TMReset.Interval = 500
    '
    'TMResetAA
    '
    Me.TMResetAA.Interval = 500
    '
    'lblPopis
    '
    Me.lblPopis.AutoSize = True
    Me.lblPopis.BackColor = System.Drawing.Color.Transparent
    Me.lblPopis.Location = New System.Drawing.Point(3, 9)
    Me.lblPopis.Name = "lblPopis"
    Me.lblPopis.Size = New System.Drawing.Size(22, 13)
    Me.lblPopis.TabIndex = 29
    Me.lblPopis.Text = "SN"
    '
    'cbSNs
    '
    Me.cbSNs.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList
    Me.cbSNs.Location = New System.Drawing.Point(3, 4)
    Me.cbSNs.Name = "cbSNs"
    Me.cbSNs.Size = New System.Drawing.Size(69, 20)
    Me.cbSNs.TabIndex = 28
    Me.cbSNs.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'MultiSimulationValec
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.groupBox)
    Me.Name = "MultiSimulationValec"
    Me.Size = New System.Drawing.Size(251, 270)
    CType(Me.groupBox, System.ComponentModel.ISupportInitialize).EndInit()
    Me.groupBox.ResumeLayout(False)
    Me.groupBox.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents groupBox As Janus.Windows.EditControls.UIGroupBox
  Friend WithEvents TMReset As System.Windows.Forms.Timer
  Friend WithEvents TMResetAA As System.Windows.Forms.Timer
  Friend WithEvents lblSN As System.Windows.Forms.Label
    Friend WithEvents SSV As D3dGraphic2.SingleSimulationValec
    Friend WithEvents lblPopis As System.Windows.Forms.Label
  Friend WithEvents cbSNs As Janus.Windows.EditControls.UIComboBox

End Class
