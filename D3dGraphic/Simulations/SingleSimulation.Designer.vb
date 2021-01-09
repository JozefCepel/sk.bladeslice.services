<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SingleSimulation
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SingleSimulation))
    Me.UiCM = New Janus.Windows.UI.CommandBars.UICommandManager(Me.components)
    Me.BottomRebar1 = New Janus.Windows.UI.CommandBars.UIRebar()
    Me.Close = New Janus.Windows.UI.CommandBars.UICommand("Close")
    Me.Pocet = New Janus.Windows.UI.CommandBars.UICommand("Pocet")
    Me.Sep = New Janus.Windows.UI.CommandBars.UICommand("Sep")
    Me.PopisPocet = New Janus.Windows.UI.CommandBars.UICommand("PopisPocet")
    Me.LeftRebar1 = New Janus.Windows.UI.CommandBars.UIRebar()
    Me.RightRebar1 = New Janus.Windows.UI.CommandBars.UIRebar()
    Me.TopRebar1 = New Janus.Windows.UI.CommandBars.UIRebar()
    Me.gbSS = New Janus.Windows.EditControls.UIGroupBox()
    Me.tblLog = New System.Windows.Forms.TableLayoutPanel()
    Me.lblVyrez = New System.Windows.Forms.Label()
    Me.lblZvysok = New System.Windows.Forms.Label()
    Me.lblMax = New System.Windows.Forms.Label()
    Me.lblPopis = New System.Windows.Forms.Label()
    Me.lblIndex = New System.Windows.Forms.Label()
    Me.lbl_L = New System.Windows.Forms.Label()
    Me.lbl_bd = New System.Windows.Forms.Label()
    Me.lbl_aD = New System.Windows.Forms.Label()
    Me.btnMaximize = New Janus.Windows.EditControls.UIButton()
    Me.cbComputations = New Janus.Windows.EditControls.UIComboBox()
    Me.btnOk = New Janus.Windows.EditControls.UIButton()
    Me.iudQTY = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
    Me.btnCancel = New Janus.Windows.EditControls.UIButton()
    Me.pic3d = New System.Windows.Forms.PictureBox()
    Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
    Me.chb_L = New System.Windows.Forms.CheckBox()
    Me.chb_bd = New System.Windows.Forms.CheckBox()
    Me.chb_aD = New System.Windows.Forms.CheckBox()
    CType(Me.UiCM, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.BottomRebar1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.LeftRebar1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.RightRebar1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.TopRebar1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.gbSS, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.gbSS.SuspendLayout()
    Me.tblLog.SuspendLayout()
    CType(Me.pic3d, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'UiCM
    '
    Me.UiCM.BottomRebar = Me.BottomRebar1
    Me.UiCM.Commands.AddRange(New Janus.Windows.UI.CommandBars.UICommand() {Me.Close, Me.Pocet, Me.Sep, Me.PopisPocet})
    Me.UiCM.ContainerControl = Me
    Me.UiCM.Id = New System.Guid("3756768f-141c-4d99-a3e4-ae0c643c18d1")
    Me.UiCM.LeftRebar = Me.LeftRebar1
    Me.UiCM.LockCommandBars = True
    Me.UiCM.RightRebar = Me.RightRebar1
    Me.UiCM.ShowCustomizeButton = Janus.Windows.UI.InheritableBoolean.[False]
    Me.UiCM.TopRebar = Me.TopRebar1
    Me.UiCM.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'BottomRebar1
    '
    Me.BottomRebar1.CommandManager = Me.UiCM
    Me.BottomRebar1.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.BottomRebar1.Location = New System.Drawing.Point(0, 0)
    Me.BottomRebar1.Name = "BottomRebar1"
    Me.BottomRebar1.Size = New System.Drawing.Size(0, 0)
    '
    'Close
    '
    Me.Close.CommandStyle = Janus.Windows.UI.CommandBars.CommandStyle.TextImage
    Me.Close.Key = "Close"
    Me.Close.Name = "Close"
    Me.Close.ShowTextInContainers = Janus.Windows.UI.InheritableBoolean.[False]
    Me.Close.Text = "Zrušiť"
    '
    'Pocet
    '
    Me.Pocet.CommandType = Janus.Windows.UI.CommandBars.CommandType.ComboBoxCommand
    Me.Pocet.IsEditableControl = Janus.Windows.UI.InheritableBoolean.[True]
    Me.Pocet.Key = "Pocet"
    Me.Pocet.Name = "Pocet"
    Me.Pocet.Text = "Počet"
    '
    'Sep
    '
    Me.Sep.CommandType = Janus.Windows.UI.CommandBars.CommandType.Separator
    Me.Sep.Key = "Sep"
    Me.Sep.Name = "Sep"
    Me.Sep.Text = "Sep"
    '
    'PopisPocet
    '
    Me.PopisPocet.CommandType = Janus.Windows.UI.CommandBars.CommandType.Label
    Me.PopisPocet.Key = "PopisPocet"
    Me.PopisPocet.Name = "PopisPocet"
    Me.PopisPocet.Text = "Počet"
    '
    'LeftRebar1
    '
    Me.LeftRebar1.CommandManager = Me.UiCM
    Me.LeftRebar1.Dock = System.Windows.Forms.DockStyle.Left
    Me.LeftRebar1.Location = New System.Drawing.Point(0, 0)
    Me.LeftRebar1.Name = "LeftRebar1"
    Me.LeftRebar1.Size = New System.Drawing.Size(0, 0)
    '
    'RightRebar1
    '
    Me.RightRebar1.CommandManager = Me.UiCM
    Me.RightRebar1.Dock = System.Windows.Forms.DockStyle.Right
    Me.RightRebar1.Location = New System.Drawing.Point(0, 0)
    Me.RightRebar1.Name = "RightRebar1"
    Me.RightRebar1.Size = New System.Drawing.Size(0, 0)
    '
    'TopRebar1
    '
    Me.TopRebar1.CommandManager = Me.UiCM
    Me.TopRebar1.Dock = System.Windows.Forms.DockStyle.Top
    Me.TopRebar1.Location = New System.Drawing.Point(0, 0)
    Me.TopRebar1.Name = "TopRebar1"
    Me.TopRebar1.Size = New System.Drawing.Size(263, 0)
    '
    'gbSS
    '
    Me.gbSS.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.gbSS.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.Rebar
    Me.gbSS.Controls.Add(Me.chb_aD)
    Me.gbSS.Controls.Add(Me.chb_bd)
    Me.gbSS.Controls.Add(Me.chb_L)
    Me.gbSS.Controls.Add(Me.tblLog)
    Me.gbSS.Controls.Add(Me.lblMax)
    Me.gbSS.Controls.Add(Me.lblPopis)
    Me.gbSS.Controls.Add(Me.lblIndex)
    Me.gbSS.Controls.Add(Me.lbl_L)
    Me.gbSS.Controls.Add(Me.lbl_bd)
    Me.gbSS.Controls.Add(Me.lbl_aD)
    Me.gbSS.Controls.Add(Me.btnMaximize)
    Me.gbSS.Controls.Add(Me.cbComputations)
    Me.gbSS.Controls.Add(Me.btnOk)
    Me.gbSS.Controls.Add(Me.iudQTY)
    Me.gbSS.Controls.Add(Me.btnCancel)
    Me.gbSS.Controls.Add(Me.pic3d)
    Me.gbSS.FrameStyle = Janus.Windows.EditControls.FrameStyle.None
    Me.gbSS.Location = New System.Drawing.Point(0, 0)
    Me.gbSS.Name = "gbSS"
    Me.gbSS.Size = New System.Drawing.Size(263, 243)
    Me.gbSS.TabIndex = 4
    Me.gbSS.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007
    '
    'tblLog
    '
    Me.tblLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.tblLog.BackColor = System.Drawing.Color.Transparent
    Me.tblLog.ColumnCount = 2
    Me.tblLog.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.tblLog.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.tblLog.Controls.Add(Me.lblVyrez, 0, 0)
    Me.tblLog.Controls.Add(Me.lblZvysok, 1, 0)
    Me.tblLog.Location = New System.Drawing.Point(66, 7)
    Me.tblLog.Margin = New System.Windows.Forms.Padding(0)
    Me.tblLog.Name = "tblLog"
    Me.tblLog.RowCount = 1
    Me.tblLog.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.tblLog.Size = New System.Drawing.Size(129, 16)
    Me.tblLog.TabIndex = 23
    '
    'lblVyrez
    '
    Me.lblVyrez.BackColor = System.Drawing.Color.Transparent
    Me.lblVyrez.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lblVyrez.ForeColor = System.Drawing.Color.Black
    Me.lblVyrez.Location = New System.Drawing.Point(3, 0)
    Me.lblVyrez.Name = "lblVyrez"
    Me.lblVyrez.Size = New System.Drawing.Size(58, 16)
    Me.lblVyrez.TabIndex = 10
    Me.lblVyrez.Text = "## /"
    Me.lblVyrez.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblZvysok
    '
    Me.lblZvysok.BackColor = System.Drawing.Color.Transparent
    Me.lblZvysok.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lblZvysok.ForeColor = System.Drawing.Color.ForestGreen
    Me.lblZvysok.Location = New System.Drawing.Point(67, 0)
    Me.lblZvysok.Name = "lblZvysok"
    Me.lblZvysok.Size = New System.Drawing.Size(59, 16)
    Me.lblZvysok.TabIndex = 11
    Me.lblZvysok.Text = "##"
    Me.lblZvysok.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'lblMax
    '
    Me.lblMax.AutoSize = True
    Me.lblMax.BackColor = System.Drawing.Color.Transparent
    Me.lblMax.ForeColor = System.Drawing.Color.Black
    Me.lblMax.Location = New System.Drawing.Point(38, 7)
    Me.lblMax.Name = "lblMax"
    Me.lblMax.Size = New System.Drawing.Size(25, 13)
    Me.lblMax.TabIndex = 22
    Me.lblMax.Text = "z xx"
    '
    'lblPopis
    '
    Me.lblPopis.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblPopis.AutoSize = True
    Me.lblPopis.BackColor = System.Drawing.Color.Transparent
    Me.lblPopis.Location = New System.Drawing.Point(3, 218)
    Me.lblPopis.Name = "lblPopis"
    Me.lblPopis.Size = New System.Drawing.Size(24, 13)
    Me.lblPopis.TabIndex = 21
    Me.lblPopis.Text = "text"
    '
    'lblIndex
    '
    Me.lblIndex.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblIndex.AutoSize = True
    Me.lblIndex.BackColor = System.Drawing.Color.Transparent
    Me.lblIndex.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
    Me.lblIndex.Location = New System.Drawing.Point(239, 30)
    Me.lblIndex.Name = "lblIndex"
    Me.lblIndex.Size = New System.Drawing.Size(27, 20)
    Me.lblIndex.TabIndex = 20
    Me.lblIndex.Text = "12"
    '
    'lbl_L
    '
    Me.lbl_L.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lbl_L.AutoSize = True
    Me.lbl_L.BackColor = System.Drawing.Color.Transparent
    Me.lbl_L.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.lbl_L.Location = New System.Drawing.Point(243, 150)
    Me.lbl_L.Name = "lbl_L"
    Me.lbl_L.Size = New System.Drawing.Size(13, 13)
    Me.lbl_L.TabIndex = 19
    Me.lbl_L.Text = "L"
    '
    'lbl_bd
    '
    Me.lbl_bd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lbl_bd.AutoSize = True
    Me.lbl_bd.BackColor = System.Drawing.Color.Transparent
    Me.lbl_bd.ForeColor = System.Drawing.Color.Green
    Me.lbl_bd.Location = New System.Drawing.Point(243, 111)
    Me.lbl_bd.Name = "lbl_bd"
    Me.lbl_bd.Size = New System.Drawing.Size(19, 13)
    Me.lbl_bd.TabIndex = 18
    Me.lbl_bd.Text = "bd"
    '
    'lbl_aD
    '
    Me.lbl_aD.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lbl_aD.AutoSize = True
    Me.lbl_aD.BackColor = System.Drawing.Color.Transparent
    Me.lbl_aD.ForeColor = System.Drawing.Color.Blue
    Me.lbl_aD.Location = New System.Drawing.Point(243, 72)
    Me.lbl_aD.Name = "lbl_aD"
    Me.lbl_aD.Size = New System.Drawing.Size(21, 13)
    Me.lbl_aD.TabIndex = 17
    Me.lbl_aD.Text = "aD"
    '
    'btnMaximize
    '
    Me.btnMaximize.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnMaximize.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button
    Me.btnMaximize.Icon = CType(resources.GetObject("btnMaximize.Icon"), System.Drawing.Icon)
    Me.btnMaximize.ImageHorizontalAlignment = Janus.Windows.EditControls.ImageHorizontalAlignment.Center
    Me.btnMaximize.Location = New System.Drawing.Point(219, 3)
    Me.btnMaximize.Name = "btnMaximize"
    Me.btnMaximize.Size = New System.Drawing.Size(20, 20)
    Me.btnMaximize.TabIndex = 13
    Me.btnMaximize.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'cbComputations
    '
    Me.cbComputations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cbComputations.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList
    Me.cbComputations.Location = New System.Drawing.Point(3, 214)
    Me.cbComputations.Name = "cbComputations"
    Me.cbComputations.Size = New System.Drawing.Size(257, 20)
    Me.cbComputations.TabIndex = 12
    Me.cbComputations.Text = "UiComboBox1"
    Me.cbComputations.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'btnOk
    '
    Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnOk.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button
    Me.btnOk.Icon = CType(resources.GetObject("btnOk.Icon"), System.Drawing.Icon)
    Me.btnOk.ImageHorizontalAlignment = Janus.Windows.EditControls.ImageHorizontalAlignment.Center
    Me.btnOk.Location = New System.Drawing.Point(198, 3)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(20, 20)
    Me.btnOk.TabIndex = 7
    Me.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'iudQTY
    '
    Me.iudQTY.Location = New System.Drawing.Point(3, 4)
    Me.iudQTY.Minimum = 1
    Me.iudQTY.Name = "iudQTY"
    Me.iudQTY.Size = New System.Drawing.Size(33, 20)
    Me.iudQTY.TabIndex = 6
    Me.iudQTY.Value = 1
    Me.iudQTY.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
    '
    'btnCancel
    '
    Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnCancel.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button
        Me.btnCancel.Image = Global.D3dGraphic2.My.Resources.Resources.End32
        Me.btnCancel.ImageHorizontalAlignment = Janus.Windows.EditControls.ImageHorizontalAlignment.Center
    Me.btnCancel.Location = New System.Drawing.Point(240, 3)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(20, 20)
    Me.btnCancel.TabIndex = 5
    Me.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
    '
    'pic3d
    '
    Me.pic3d.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pic3d.Location = New System.Drawing.Point(5, 29)
    Me.pic3d.Name = "pic3d"
    Me.pic3d.Size = New System.Drawing.Size(234, 179)
    Me.pic3d.TabIndex = 3
    Me.pic3d.TabStop = False
    '
    'chb_L
    '
    Me.chb_L.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chb_L.AutoSize = True
    Me.chb_L.BackColor = System.Drawing.Color.Transparent
    Me.chb_L.Location = New System.Drawing.Point(243, 166)
    Me.chb_L.Name = "chb_L"
    Me.chb_L.Size = New System.Drawing.Size(15, 14)
    Me.chb_L.TabIndex = 16
    Me.chb_L.UseVisualStyleBackColor = False
    '
    'chb_bd
    '
    Me.chb_bd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chb_bd.AutoSize = True
    Me.chb_bd.BackColor = System.Drawing.Color.Transparent
    Me.chb_bd.Location = New System.Drawing.Point(243, 127)
    Me.chb_bd.Name = "chb_bd"
    Me.chb_bd.Size = New System.Drawing.Size(15, 14)
    Me.chb_bd.TabIndex = 15
    Me.chb_bd.UseVisualStyleBackColor = False
    '
    'chb_aD
    '
    Me.chb_aD.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.chb_aD.AutoSize = True
    Me.chb_aD.BackColor = System.Drawing.Color.Transparent
    Me.chb_aD.Location = New System.Drawing.Point(243, 88)
    Me.chb_aD.Name = "chb_aD"
    Me.chb_aD.Size = New System.Drawing.Size(15, 14)
    Me.chb_aD.TabIndex = 14
    Me.chb_aD.UseVisualStyleBackColor = False
    '
    'SingleSimulation
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.gbSS)
    Me.Controls.Add(Me.TopRebar1)
    Me.Name = "SingleSimulation"
    Me.Size = New System.Drawing.Size(263, 243)
    CType(Me.UiCM, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.BottomRebar1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.LeftRebar1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.RightRebar1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.TopRebar1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.gbSS, System.ComponentModel.ISupportInitialize).EndInit()
    Me.gbSS.ResumeLayout(False)
    Me.gbSS.PerformLayout()
    Me.tblLog.ResumeLayout(False)
    CType(Me.pic3d, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents UiCM As Janus.Windows.UI.CommandBars.UICommandManager
  Friend WithEvents Close As Janus.Windows.UI.CommandBars.UICommand
  Friend WithEvents TopRebar1 As Janus.Windows.UI.CommandBars.UIRebar
  Friend WithEvents BottomRebar1 As Janus.Windows.UI.CommandBars.UIRebar
  Friend WithEvents pic3d As System.Windows.Forms.PictureBox
  Friend WithEvents LeftRebar1 As Janus.Windows.UI.CommandBars.UIRebar
  Friend WithEvents RightRebar1 As Janus.Windows.UI.CommandBars.UIRebar
  Friend WithEvents gbSS As Janus.Windows.EditControls.UIGroupBox
  Friend WithEvents Pocet As Janus.Windows.UI.CommandBars.UICommand
  Friend WithEvents Sep As Janus.Windows.UI.CommandBars.UICommand
  Friend WithEvents PopisPocet As Janus.Windows.UI.CommandBars.UICommand
  Friend WithEvents btnCancel As Janus.Windows.EditControls.UIButton
  Friend WithEvents iudQTY As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents btnOk As Janus.Windows.EditControls.UIButton
  Friend WithEvents lblZvysok As System.Windows.Forms.Label
  Friend WithEvents lblVyrez As System.Windows.Forms.Label
  Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
  Friend WithEvents btnMaximize As Janus.Windows.EditControls.UIButton
  Friend WithEvents lbl_aD As System.Windows.Forms.Label
  Friend WithEvents lblIndex As System.Windows.Forms.Label
  Friend WithEvents lbl_L As System.Windows.Forms.Label
  Friend WithEvents lbl_bd As System.Windows.Forms.Label
  Friend WithEvents lblPopis As System.Windows.Forms.Label
  Friend WithEvents cbComputations As Janus.Windows.EditControls.UIComboBox

  Public Sub New()

    ' This call is required by the designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
  End Sub
  Friend WithEvents lblMax As System.Windows.Forms.Label
  Friend WithEvents tblLog As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents chb_aD As System.Windows.Forms.CheckBox
  Friend WithEvents chb_bd As System.Windows.Forms.CheckBox
  Friend WithEvents chb_L As System.Windows.Forms.CheckBox

End Class
