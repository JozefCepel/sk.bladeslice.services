<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MultiSimulationBlok
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
    Me.grpBox = New Janus.Windows.EditControls.UIGroupBox()
    Me.lblPopis = New System.Windows.Forms.Label()
    Me.lblSN = New System.Windows.Forms.Label()
    Me.SplitMSB = New System.Windows.Forms.Splitter()
    Me.btnClose = New Janus.Windows.EditControls.UIButton()
    Me.lblSimsCount = New System.Windows.Forms.Label()
    Me.btnVsetkyRohy = New Janus.Windows.EditControls.UIButton()
    Me.FLP = New System.Windows.Forms.FlowLayoutPanel()
    Me.cbSNs = New Janus.Windows.EditControls.UIComboBox()
    Me.TMReset = New System.Windows.Forms.Timer(Me.components)
    Me.TMResetAA = New System.Windows.Forms.Timer(Me.components)
    CType(Me.grpBox, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpBox.SuspendLayout()
    Me.SuspendLayout()
    '
    'grpBox
    '
    Me.grpBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpBox.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.ExplorerBarBackground
    Me.grpBox.Controls.Add(Me.lblPopis)
    Me.grpBox.Controls.Add(Me.lblSN)
    Me.grpBox.Controls.Add(Me.SplitMSB)
    Me.grpBox.Controls.Add(Me.btnClose)
    Me.grpBox.Controls.Add(Me.lblSimsCount)
    Me.grpBox.Controls.Add(Me.btnVsetkyRohy)
    Me.grpBox.Controls.Add(Me.FLP)
    Me.grpBox.Controls.Add(Me.cbSNs)
    Me.grpBox.FrameStyle = Janus.Windows.EditControls.FrameStyle.None
    Me.grpBox.Location = New System.Drawing.Point(0, 0)
    Me.grpBox.Name = "grpBox"
    Me.grpBox.Size = New System.Drawing.Size(762, 326)
    Me.grpBox.TabIndex = 21
    Me.grpBox.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007
    '
    'lblPopis
    '
    Me.lblPopis.AutoSize = True
    Me.lblPopis.BackColor = System.Drawing.Color.Transparent
    Me.lblPopis.Location = New System.Drawing.Point(3, 10)
    Me.lblPopis.Name = "lblPopis"
    Me.lblPopis.Size = New System.Drawing.Size(22, 13)
    Me.lblPopis.TabIndex = 27
    Me.lblPopis.Text = "SN"
    '
    'lblSN
    '
    Me.lblSN.AutoSize = True
    Me.lblSN.BackColor = System.Drawing.Color.Transparent
    Me.lblSN.Location = New System.Drawing.Point(84, 10)
    Me.lblSN.Name = "lblSN"
    Me.lblSN.Size = New System.Drawing.Size(64, 13)
    Me.lblSN.TabIndex = 22
    Me.lblSN.Text = "Materiál: ##"
    '
    'SplitMSB
    '
    Me.SplitMSB.Cursor = System.Windows.Forms.Cursors.HSplit
    Me.SplitMSB.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.SplitMSB.Location = New System.Drawing.Point(0, 321)
    Me.SplitMSB.Name = "SplitMSB"
    Me.SplitMSB.Size = New System.Drawing.Size(762, 5)
    Me.SplitMSB.TabIndex = 25
    Me.SplitMSB.TabStop = False
    '
    'btnClose
    '
    Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnClose.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button
        Me.btnClose.Image = Global.D3dGraphic2.My.Resources.Resources.End32
        Me.btnClose.ImageHorizontalAlignment = Janus.Windows.EditControls.ImageHorizontalAlignment.Center
    Me.btnClose.Location = New System.Drawing.Point(737, 5)
    Me.btnClose.Name = "btnClose"
    Me.btnClose.Size = New System.Drawing.Size(20, 20)
    Me.btnClose.TabIndex = 24
    Me.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'lblSimsCount
    '
    Me.lblSimsCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblSimsCount.BackColor = System.Drawing.Color.Transparent
    Me.lblSimsCount.Location = New System.Drawing.Point(263, 10)
    Me.lblSimsCount.Name = "lblSimsCount"
    Me.lblSimsCount.Size = New System.Drawing.Size(367, 13)
    Me.lblSimsCount.TabIndex = 23
    Me.lblSimsCount.Text = "Počet simulácii: ##"
    Me.lblSimsCount.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'btnVsetkyRohy
    '
    Me.btnVsetkyRohy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnVsetkyRohy.Location = New System.Drawing.Point(638, 5)
    Me.btnVsetkyRohy.Name = "btnVsetkyRohy"
    Me.btnVsetkyRohy.Size = New System.Drawing.Size(86, 23)
    Me.btnVsetkyRohy.TabIndex = 0
    Me.btnVsetkyRohy.Text = "Všetky rohy (8)"
    Me.btnVsetkyRohy.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'FLP
    '
    Me.FLP.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.FLP.AutoScroll = True
    Me.FLP.BackColor = System.Drawing.Color.Transparent
    Me.FLP.Location = New System.Drawing.Point(5, 31)
    Me.FLP.Name = "FLP"
    Me.FLP.Size = New System.Drawing.Size(752, 284)
    Me.FLP.TabIndex = 21
    '
    'cbSNs
    '
    Me.cbSNs.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList
    Me.cbSNs.Location = New System.Drawing.Point(3, 5)
    Me.cbSNs.Name = "cbSNs"
    Me.cbSNs.Size = New System.Drawing.Size(75, 20)
    Me.cbSNs.TabIndex = 26
    Me.cbSNs.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'TMReset
    '
    Me.TMReset.Interval = 500
    '
    'TMResetAA
    '
    Me.TMResetAA.Interval = 500
    '
    'MultiSimulationBlok
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.grpBox)
    Me.Name = "MultiSimulationBlok"
    Me.Size = New System.Drawing.Size(762, 326)
    CType(Me.grpBox, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpBox.ResumeLayout(False)
    Me.grpBox.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents grpBox As Janus.Windows.EditControls.UIGroupBox
  Friend WithEvents lblSN As System.Windows.Forms.Label
  Friend WithEvents FLP As System.Windows.Forms.FlowLayoutPanel
  Friend WithEvents btnVsetkyRohy As Janus.Windows.EditControls.UIButton
  Friend WithEvents TMReset As System.Windows.Forms.Timer
  Friend WithEvents lblSimsCount As System.Windows.Forms.Label
  Friend WithEvents TMResetAA As System.Windows.Forms.Timer
  Friend WithEvents btnClose As Janus.Windows.EditControls.UIButton
  Friend WithEvents SplitMSB As System.Windows.Forms.Splitter
  Friend WithEvents cbSNs As Janus.Windows.EditControls.UIComboBox
  Friend WithEvents lblPopis As System.Windows.Forms.Label

End Class
