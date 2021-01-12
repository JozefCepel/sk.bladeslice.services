<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormD3dGraphic
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormD3dGraphic))
        Me.rbnMAIN = New Janus.Windows.Ribbon.Ribbon()
        Me.rbnBtnExit = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbn_aD = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
        Me.VSM = New Janus.Windows.Common.VisualStyleManager(Me.components)
        Me.rbn_bd = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
        Me.rbn_L = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
        Me.rbn_poc_ks = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
        Me.rbn_max_rez = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
        Me.rbn_threshold = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
        Me.rbn_MaxSim = New Janus.Windows.GridEX.EditControls.IntegerUpDown()
        Me.TBSize = New Janus.Windows.EditControls.UITrackBar()
        Me.TBCitlivost = New Janus.Windows.EditControls.UITrackBar()
        Me.rbnTabDomov = New Janus.Windows.Ribbon.RibbonTab()
        Me.rbnGrpOperations = New Janus.Windows.Ribbon.RibbonGroup()
        Me.rbnBtnCalc = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbnBtnOk = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbnBtnClose = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbnGrpInput = New Janus.Windows.Ribbon.RibbonGroup()
        Me.cont_rbn_aD = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.cont_rbn_bd = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.cont_rbn_L = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.cont_rbn_poc_ks = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.rbnGrpSimSet = New Janus.Windows.Ribbon.RibbonGroup()
        Me.rbn_cont_max_rez = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.rbn_cont_threshold = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.rbn_cont_MaxSim = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.rbnSeparator1 = New Janus.Windows.Ribbon.SeparatorCommand()
        Me.rbn_AA = New Janus.Windows.Ribbon.CheckBoxCommand()
        Me.rbn_RunOnStart = New Janus.Windows.Ribbon.CheckBoxCommand()
        Me.rbn_SVP = New Janus.Windows.Ribbon.CheckBoxCommand()
        Me.rbnSeparator2 = New Janus.Windows.Ribbon.SeparatorCommand()
        Me.rbn_cont_velkost = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.rbn_cont_citlivost = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.rbnBtnReset = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbnTabOstatne = New Janus.Windows.Ribbon.RibbonTab()
        Me.rbnGrpColors = New Janus.Windows.Ribbon.RibbonGroup()
        Me.rbnBtnVS_Blue = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbnBtnVS_Silver = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbnBtnVS_Black = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbnGrpPomoc = New Janus.Windows.Ribbon.RibbonGroup()
        Me.rbnBtnAbout = New Janus.Windows.Ribbon.ButtonCommand()
        Me.rbnPopupBackGrid = New Janus.Windows.Ribbon.RibbonContextMenu(Me.components)
        Me.rbnPopupGrid = New Janus.Windows.Ribbon.RibbonContextMenu(Me.components)
        Me.rbnPopupHeaderGrid = New Janus.Windows.Ribbon.RibbonContextMenu(Me.components)
        Me.rbnListBoxMenu = New Janus.Windows.Ribbon.RibbonContextMenu(Me.components)
        Me.rbnMnuDyn = New Janus.Windows.Ribbon.RibbonContextMenu(Me.components)
        Me.panelManager = New Janus.Windows.UI.Dock.UIPanelManager(Me.components)
        Me.uiSN = New Janus.Windows.UI.Dock.UIPanel()
        Me.uiSNContainer = New Janus.Windows.UI.Dock.UIPanelInnerContainer()
        Me.lboxSN = New System.Windows.Forms.ListBox()
        Me.uiGrid = New Janus.Windows.UI.Dock.UIPanel()
        Me.uiGridContainer = New Janus.Windows.UI.Dock.UIPanelInnerContainer()
        Me.splSim = New System.Windows.Forms.SplitContainer()
        Me.MJG = New MemControls.MemJG()
        Me.grpSelSim = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtVyrez = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtOdrezok = New System.Windows.Forms.Label()
        Me.labObjemCapt = New System.Windows.Forms.Label()
        Me.labPocKsCapt = New System.Windows.Forms.Label()
        Me.txtPolotovar = New System.Windows.Forms.Label()
        Me.labPocKs = New System.Windows.Forms.Label()
        Me.uiDetail = New Janus.Windows.UI.Dock.UIPanel()
        Me.uiDetailContainer = New Janus.Windows.UI.Dock.UIPanelInnerContainer()
        Me.uiGrpBox = New Janus.Windows.EditControls.UIGroupBox()
        Me.FLP = New System.Windows.Forms.FlowLayoutPanel()
        Me.RSB = New Janus.Windows.Ribbon.RibbonStatusBar()
        Me.PGB = New Janus.Windows.EditControls.UIProgressBar()
        Me.sbrOperacia = New Janus.Windows.Ribbon.ContainerControlCommand()
        Me.CheckBoxCommand1 = New Janus.Windows.Ribbon.CheckBoxCommand()
        CType(Me.rbnMAIN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.rbnMAIN.SuspendLayout()
        CType(Me.panelManager, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uiSN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uiSN.SuspendLayout()
        Me.uiSNContainer.SuspendLayout()
        CType(Me.uiGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uiGrid.SuspendLayout()
        Me.uiGridContainer.SuspendLayout()
        CType(Me.splSim, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splSim.Panel1.SuspendLayout()
        Me.splSim.Panel2.SuspendLayout()
        Me.splSim.SuspendLayout()
        Me.grpSelSim.SuspendLayout()
        CType(Me.uiDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uiDetail.SuspendLayout()
        Me.uiDetailContainer.SuspendLayout()
        CType(Me.uiGrpBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uiGrpBox.SuspendLayout()
        Me.RSB.SuspendLayout()
        Me.SuspendLayout()
        '
        'rbnMAIN
        '
        Me.rbnMAIN.ControlBoxMenu.LeftCommands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.rbnBtnExit})
        Me.rbnMAIN.ControlBoxMenu.SuperTipSettings.ImageListProvider = Me.rbnMAIN.ControlBoxMenu
        Me.rbnMAIN.Controls.Add(Me.rbn_aD)
        Me.rbnMAIN.Controls.Add(Me.rbn_bd)
        Me.rbnMAIN.Controls.Add(Me.rbn_L)
        Me.rbnMAIN.Controls.Add(Me.rbn_poc_ks)
        Me.rbnMAIN.Controls.Add(Me.rbn_max_rez)
        Me.rbnMAIN.Controls.Add(Me.rbn_threshold)
        Me.rbnMAIN.Controls.Add(Me.rbn_MaxSim)
        Me.rbnMAIN.Controls.Add(Me.TBSize)
        Me.rbnMAIN.Controls.Add(Me.TBCitlivost)
        Me.rbnMAIN.Location = New System.Drawing.Point(0, 0)
        Me.rbnMAIN.Name = "rbnMAIN"
        Me.rbnMAIN.ShowCustomizeButton = False
        Me.rbnMAIN.Size = New System.Drawing.Size(1035, 146)
        '
        '
        '
        Me.rbnMAIN.SuperTipComponent.AutoPopDelay = 2000
        Me.rbnMAIN.SuperTipComponent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.rbnMAIN.SuperTipComponent.ImageList = Nothing
        Me.rbnMAIN.TabIndex = 0
        Me.rbnMAIN.Tabs.AddRange(New Janus.Windows.Ribbon.RibbonTab() {Me.rbnTabDomov, Me.rbnTabOstatne})
        '
        'rbnBtnExit
        '
        Me.rbnBtnExit.Image = CType(resources.GetObject("rbnBtnExit.Image"), System.Drawing.Image)
        Me.rbnBtnExit.Key = "rbnBtnExit"
        Me.rbnBtnExit.Name = "rbnBtnExit"
        Me.rbnBtnExit.Style = Janus.Windows.Ribbon.CommandStyle.ImageAndText
        Me.rbnBtnExit.SuperTipSettings.ImageListProvider = Me.rbnBtnExit
        Me.rbnBtnExit.Text = "Zavrieù"
        '
        'rbn_aD
        '
        Me.rbn_aD.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.rbn_aD.Location = New System.Drawing.Point(183, 60)
        Me.rbn_aD.Maximum = 10000
        Me.rbn_aD.Minimum = 1
        Me.rbn_aD.Name = "rbn_aD"
        Me.rbn_aD.Size = New System.Drawing.Size(50, 20)
        Me.rbn_aD.TabIndex = 0
        Me.rbn_aD.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
        Me.rbn_aD.Value = 1
        Me.rbn_aD.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        Me.rbn_aD.VisualStyleManager = Me.VSM
        '
        'VSM
        '
        Me.VSM.DefaultColorScheme = "Schema1"
        '
        'rbn_bd
        '
        Me.rbn_bd.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.rbn_bd.Location = New System.Drawing.Point(183, 82)
        Me.rbn_bd.Maximum = 10000
        Me.rbn_bd.Minimum = 1
        Me.rbn_bd.Name = "rbn_bd"
        Me.rbn_bd.Size = New System.Drawing.Size(50, 20)
        Me.rbn_bd.TabIndex = 1
        Me.rbn_bd.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
        Me.rbn_bd.Value = 1
        Me.rbn_bd.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        Me.rbn_bd.VisualStyleManager = Me.VSM
        '
        'rbn_L
        '
        Me.rbn_L.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.rbn_L.Location = New System.Drawing.Point(183, 104)
        Me.rbn_L.Maximum = 10000
        Me.rbn_L.Minimum = 1
        Me.rbn_L.Name = "rbn_L"
        Me.rbn_L.Size = New System.Drawing.Size(50, 20)
        Me.rbn_L.TabIndex = 2
        Me.rbn_L.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
        Me.rbn_L.Value = 1
        Me.rbn_L.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        Me.rbn_L.VisualStyleManager = Me.VSM
        '
        'rbn_poc_ks
        '
        Me.rbn_poc_ks.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.rbn_poc_ks.Location = New System.Drawing.Point(305, 60)
        Me.rbn_poc_ks.Maximum = 1000
        Me.rbn_poc_ks.Minimum = 1
        Me.rbn_poc_ks.Name = "rbn_poc_ks"
        Me.rbn_poc_ks.Size = New System.Drawing.Size(50, 20)
        Me.rbn_poc_ks.TabIndex = 3
        Me.rbn_poc_ks.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
        Me.rbn_poc_ks.Value = 1
        Me.rbn_poc_ks.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        Me.rbn_poc_ks.VisualStyleManager = Me.VSM
        '
        'rbn_max_rez
        '
        Me.rbn_max_rez.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.rbn_max_rez.Increment = 10
        Me.rbn_max_rez.Location = New System.Drawing.Point(471, 60)
        Me.rbn_max_rez.Maximum = 100000
        Me.rbn_max_rez.Minimum = 1
        Me.rbn_max_rez.Name = "rbn_max_rez"
        Me.rbn_max_rez.Size = New System.Drawing.Size(50, 20)
        Me.rbn_max_rez.TabIndex = 4
        Me.rbn_max_rez.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
        Me.rbn_max_rez.Value = 10
        Me.rbn_max_rez.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        Me.rbn_max_rez.VisualStyleManager = Me.VSM
        '
        'rbn_threshold
        '
        Me.rbn_threshold.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.rbn_threshold.Increment = 10
        Me.rbn_threshold.Location = New System.Drawing.Point(471, 82)
        Me.rbn_threshold.Name = "rbn_threshold"
        Me.rbn_threshold.Size = New System.Drawing.Size(50, 20)
        Me.rbn_threshold.TabIndex = 5
        Me.rbn_threshold.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
        Me.rbn_threshold.Value = 10
        Me.rbn_threshold.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        Me.rbn_threshold.VisualStyleManager = Me.VSM
        '
        'rbn_MaxSim
        '
        Me.rbn_MaxSim.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.rbn_MaxSim.Increment = 10
        Me.rbn_MaxSim.Location = New System.Drawing.Point(471, 104)
        Me.rbn_MaxSim.Maximum = 1000
        Me.rbn_MaxSim.Name = "rbn_MaxSim"
        Me.rbn_MaxSim.Size = New System.Drawing.Size(50, 20)
        Me.rbn_MaxSim.TabIndex = 6
        Me.rbn_MaxSim.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
        Me.rbn_MaxSim.Value = 50
        Me.rbn_MaxSim.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        Me.rbn_MaxSim.VisualStyleManager = Me.VSM
        '
        'TBSize
        '
        Me.TBSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TBSize.BackColor = System.Drawing.Color.Transparent
        Me.TBSize.LargeChange = 3
        Me.TBSize.Location = New System.Drawing.Point(767, 59)
        Me.TBSize.Name = "TBSize"
        Me.TBSize.Size = New System.Drawing.Size(100, 23)
        Me.TBSize.TabIndex = 7
        Me.TBSize.ThumbColor = System.Drawing.SystemColors.Control
        Me.TBSize.Value = 2
        '
        'TBCitlivost
        '
        Me.TBCitlivost.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TBCitlivost.BackColor = System.Drawing.Color.Transparent
        Me.TBCitlivost.Location = New System.Drawing.Point(767, 82)
        Me.TBCitlivost.Name = "TBCitlivost"
        Me.TBCitlivost.Size = New System.Drawing.Size(100, 23)
        Me.TBCitlivost.TabIndex = 8
        Me.TBCitlivost.ThumbColor = System.Drawing.SystemColors.Control
        Me.TBCitlivost.Value = 2
        '
        'rbnTabDomov
        '
        Me.rbnTabDomov.Groups.AddRange(New Janus.Windows.Ribbon.RibbonGroup() {Me.rbnGrpOperations, Me.rbnGrpInput, Me.rbnGrpSimSet})
        Me.rbnTabDomov.Key = "rbnTabDomov"
        Me.rbnTabDomov.Name = "rbnTabDomov"
        Me.rbnTabDomov.Text = "Domov"
        '
        'rbnGrpOperations
        '
        Me.rbnGrpOperations.Commands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.rbnBtnCalc, Me.rbnBtnOk, Me.rbnBtnClose})
        Me.rbnGrpOperations.DialogButtonSuperTipSettings.ImageListProvider = Me.rbnGrpOperations
        Me.rbnGrpOperations.ImageKey = ""
        Me.rbnGrpOperations.Key = "rbnGrpEkonomika"
        Me.rbnGrpOperations.Name = "rbnGrpOperations"
        Me.rbnGrpOperations.SuperTipSettings.ImageListProvider = Me.rbnGrpOperations
        Me.rbnGrpOperations.Text = "Spustiù"
        '
        'rbnBtnCalc
        '
        Me.rbnBtnCalc.Key = "rbnBtnRun"
        Me.rbnBtnCalc.LargeImage = CType(resources.GetObject("rbnBtnCalc.LargeImage"), System.Drawing.Image)
        Me.rbnBtnCalc.Name = "rbnBtnCalc"
        Me.rbnBtnCalc.SizeStyle = Janus.Windows.Ribbon.CommandSizeStyle.Large
        Me.rbnBtnCalc.Style = Janus.Windows.Ribbon.CommandStyle.ImageAndText
        Me.rbnBtnCalc.SuperTipSettings.ImageListProvider = Me.rbnBtnCalc
        Me.rbnBtnCalc.Text = "Simulovaù"
        '
        'rbnBtnOk
        '
        Me.rbnBtnOk.Enabled = False
        Me.rbnBtnOk.Image = CType(resources.GetObject("rbnBtnOk.Image"), System.Drawing.Image)
        Me.rbnBtnOk.Key = "rbnBtnOk"
        Me.rbnBtnOk.Name = "rbnBtnOk"
        Me.rbnBtnOk.SuperTipSettings.ImageListProvider = Me.rbnBtnOk
        Me.rbnBtnOk.Text = "Vybraù"
        '
        'rbnBtnClose
        '
        Me.rbnBtnClose.Image = CType(resources.GetObject("rbnBtnClose.Image"), System.Drawing.Image)
        Me.rbnBtnClose.Key = "rbnBtnClose"
        Me.rbnBtnClose.Name = "rbnBtnClose"
        Me.rbnBtnClose.SuperTipSettings.ImageListProvider = Me.rbnBtnClose
        Me.rbnBtnClose.Text = "Zavrieù"
        '
        'rbnGrpInput
        '
        Me.rbnGrpInput.Commands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.cont_rbn_aD, Me.cont_rbn_bd, Me.cont_rbn_L, Me.cont_rbn_poc_ks})
        Me.rbnGrpInput.DialogButtonSuperTipSettings.ImageListProvider = Me.rbnGrpInput
        Me.rbnGrpInput.ImageKey = ""
        Me.rbnGrpInput.Key = "rbnGrpInput"
        Me.rbnGrpInput.Name = "rbnGrpInput"
        Me.rbnGrpInput.SuperTipSettings.ImageListProvider = Me.rbnGrpInput
        Me.rbnGrpInput.Text = "VstupnÈ ˙daje"
        '
        'cont_rbn_aD
        '
        Me.cont_rbn_aD.Control = Me.rbn_aD
        Me.cont_rbn_aD.ControlWidth = 50
        Me.cont_rbn_aD.Key = "cont_rbn_aD"
        Me.cont_rbn_aD.Name = "cont_rbn_aD"
        Me.cont_rbn_aD.SuperTipSettings.ImageListProvider = Me.cont_rbn_aD
        Me.cont_rbn_aD.Text = "aD"
        '
        'cont_rbn_bd
        '
        Me.cont_rbn_bd.Control = Me.rbn_bd
        Me.cont_rbn_bd.ControlWidth = 50
        Me.cont_rbn_bd.Key = "cont_rbn_bd"
        Me.cont_rbn_bd.Name = "cont_rbn_bd"
        Me.cont_rbn_bd.SuperTipSettings.ImageListProvider = Me.cont_rbn_bd
        Me.cont_rbn_bd.Text = "bd"
        '
        'cont_rbn_L
        '
        Me.cont_rbn_L.Control = Me.rbn_L
        Me.cont_rbn_L.ControlWidth = 50
        Me.cont_rbn_L.Key = "cont_rbn_L"
        Me.cont_rbn_L.Name = "cont_rbn_L"
        Me.cont_rbn_L.SuperTipSettings.ImageListProvider = Me.cont_rbn_L
        Me.cont_rbn_L.Text = "L"
        '
        'cont_rbn_poc_ks
        '
        Me.cont_rbn_poc_ks.Control = Me.rbn_poc_ks
        Me.cont_rbn_poc_ks.ControlWidth = 50
        Me.cont_rbn_poc_ks.Key = "cont_rbn_poc_ks"
        Me.cont_rbn_poc_ks.Name = "cont_rbn_poc_ks"
        Me.cont_rbn_poc_ks.SuperTipSettings.ImageListProvider = Me.cont_rbn_poc_ks
        Me.cont_rbn_poc_ks.Text = "PoËet kusov"
        '
        'rbnGrpSimSet
        '
        Me.rbnGrpSimSet.Commands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.rbn_cont_max_rez, Me.rbn_cont_threshold, Me.rbn_cont_MaxSim, Me.rbnSeparator1, Me.rbn_AA, Me.rbn_RunOnStart, Me.rbn_SVP, Me.rbnSeparator2, Me.rbn_cont_velkost, Me.rbn_cont_citlivost, Me.rbnBtnReset})
        Me.rbnGrpSimSet.DialogButtonSuperTipSettings.ImageListProvider = Me.rbnGrpSimSet
        Me.rbnGrpSimSet.ImageKey = ""
        Me.rbnGrpSimSet.Key = "rbnGrpSimSet"
        Me.rbnGrpSimSet.Name = "rbnGrpSimSet"
        Me.rbnGrpSimSet.SuperTipSettings.ImageListProvider = Me.rbnGrpSimSet
        Me.rbnGrpSimSet.Text = "Nastavenia simul·ciÌ"
        '
        'rbn_cont_max_rez
        '
        Me.rbn_cont_max_rez.Control = Me.rbn_max_rez
        Me.rbn_cont_max_rez.ControlWidth = 50
        Me.rbn_cont_max_rez.Key = "rbn_cont_max_rez"
        Me.rbn_cont_max_rez.Name = "rbn_cont_max_rez"
        Me.rbn_cont_max_rez.SuperTipSettings.HeaderText = "Maxim·lny rez"
        Me.rbn_cont_max_rez.SuperTipSettings.ImageListProvider = Me.rbn_cont_max_rez
        Me.rbn_cont_max_rez.SuperTipSettings.Text = "Maxim·lna dÂûka rezu ak˙ je moûnÈ rezaù (zodpoved· dÂûke pÌlovÈho listu)"
        Me.rbn_cont_max_rez.Text = "Maxim·lny rez"
        '
        'rbn_cont_threshold
        '
        Me.rbn_cont_threshold.Control = Me.rbn_threshold
        Me.rbn_cont_threshold.ControlWidth = 50
        Me.rbn_cont_threshold.Key = "rbn_cont_threshold"
        Me.rbn_cont_threshold.Name = "rbn_cont_threshold"
        Me.rbn_cont_threshold.SuperTipSettings.HeaderText = "Threshold"
        Me.rbn_cont_threshold.SuperTipSettings.ImageListProvider = Me.rbn_cont_threshold
        Me.rbn_cont_threshold.SuperTipSettings.Text = resources.GetString("rbn_cont_threshold.SuperTipSettings.Text")
        Me.rbn_cont_threshold.Text = "Threshold %"
        '
        'rbn_cont_MaxSim
        '
        Me.rbn_cont_MaxSim.Control = Me.rbn_MaxSim
        Me.rbn_cont_MaxSim.ControlWidth = 50
        Me.rbn_cont_MaxSim.Key = "ContainerControlCommand1"
        Me.rbn_cont_MaxSim.Name = "rbn_cont_MaxSim"
        Me.rbn_cont_MaxSim.SuperTipSettings.HeaderText = "Maxim·lny poËet simul·ciÌ"
        Me.rbn_cont_MaxSim.SuperTipSettings.ImageListProvider = Me.rbn_cont_MaxSim
        Me.rbn_cont_MaxSim.SuperTipSettings.Text = "Koæko najlepöÌch simul·cii zo vöetkych multisimul·cii (ak s˙ pouûitÈ) bude zobraz" &
    "en˝ch. V prÌpade viacer˝ch multisimul·ciÌ sa nemusia zobraziù tie multisimul·cie" &
    ", ktorÈ nespadn˙ do danÈho poËtu."
        Me.rbn_cont_MaxSim.Text = "Max.  poË. simul·ciÌ"
        '
        'rbnSeparator1
        '
        Me.rbnSeparator1.Key = "rbnSeparator1"
        Me.rbnSeparator1.Name = "rbnSeparator1"
        '
        'rbn_AA
        '
        '
        '
        '
        Me.rbn_AA.CheckBox.BackColor = System.Drawing.Color.Transparent
        Me.rbn_AA.CheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.rbn_AA.CheckBox.Location = New System.Drawing.Point(669, 59)
        Me.rbn_AA.CheckBox.Name = ""
        Me.rbn_AA.CheckBox.ShowFocusRectangle = False
        Me.rbn_AA.CheckBox.Size = New System.Drawing.Size(18, 22)
        Me.rbn_AA.CheckBox.TabIndex = 75
        Me.rbn_AA.CheckBox.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
        Me.rbn_AA.ControlWidth = 18
        Me.rbn_AA.Key = "rbn_AA"
        Me.rbn_AA.Name = "rbn_AA"
        Me.rbn_AA.SuperTipSettings.HeaderText = "Anti-aliasing"
        Me.rbn_AA.SuperTipSettings.ImageListProvider = Me.rbn_AA
        Me.rbn_AA.SuperTipSettings.Text = "Vyhladenie hr·n zobrazovanÈho modelu"
        Me.rbn_AA.Text = "Anti-aliasing"
        '
        'rbn_RunOnStart
        '
        '
        '
        '
        Me.rbn_RunOnStart.CheckBox.BackColor = System.Drawing.Color.Transparent
        Me.rbn_RunOnStart.CheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.rbn_RunOnStart.CheckBox.Location = New System.Drawing.Point(669, 81)
        Me.rbn_RunOnStart.CheckBox.Name = ""
        Me.rbn_RunOnStart.CheckBox.ShowFocusRectangle = False
        Me.rbn_RunOnStart.CheckBox.Size = New System.Drawing.Size(18, 22)
        Me.rbn_RunOnStart.CheckBox.TabIndex = 76
        Me.rbn_RunOnStart.CheckBox.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
        Me.rbn_RunOnStart.ControlWidth = 18
        Me.rbn_RunOnStart.Key = "rbn_RunOnStart"
        Me.rbn_RunOnStart.Name = "rbn_RunOnStart"
        Me.rbn_RunOnStart.SuperTipSettings.HeaderText = "Autorun"
        Me.rbn_RunOnStart.SuperTipSettings.ImageListProvider = Me.rbn_RunOnStart
        Me.rbn_RunOnStart.SuperTipSettings.Text = "Po ötarte sa hneÔ spustÌ simul·cia (nie je nutnÈ klikn˙ù na tlaËidlo 'Simulovaù')" &
    ""
        Me.rbn_RunOnStart.Text = "Autorun"
        '
        'rbn_SVP
        '
        '
        '
        '
        Me.rbn_SVP.CheckBox.BackColor = System.Drawing.Color.Transparent
        Me.rbn_SVP.CheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.rbn_SVP.CheckBox.Location = New System.Drawing.Point(669, 103)
        Me.rbn_SVP.CheckBox.Name = ""
        Me.rbn_SVP.CheckBox.ShowFocusRectangle = False
        Me.rbn_SVP.CheckBox.Size = New System.Drawing.Size(18, 22)
        Me.rbn_SVP.CheckBox.TabIndex = 77
        Me.rbn_SVP.CheckBox.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
        Me.rbn_SVP.ControlWidth = 18
        Me.rbn_SVP.Key = "rbn_SVP"
        Me.rbn_SVP.Name = "rbn_SVP"
        Me.rbn_SVP.SuperTipSettings.ImageListProvider = Me.rbn_SVP
        Me.rbn_SVP.Text = "Software vertex processing"
        '
        'rbnSeparator2
        '
        Me.rbnSeparator2.Key = "rbnSeparator2"
        Me.rbnSeparator2.Name = "rbnSeparator2"
        '
        'rbn_cont_velkost
        '
        Me.rbn_cont_velkost.Control = Me.TBSize
        Me.rbn_cont_velkost.Key = "rbn_cont_velkost"
        Me.rbn_cont_velkost.Name = "rbn_cont_velkost"
        Me.rbn_cont_velkost.SuperTipSettings.HeaderText = "Veækosù"
        Me.rbn_cont_velkost.SuperTipSettings.ImageListProvider = Me.rbn_cont_velkost
        Me.rbn_cont_velkost.SuperTipSettings.Text = "Moûnosù meniù veækosù zobrazovan˝ch objektov"
        Me.rbn_cont_velkost.Text = "Veækosù"
        '
        'rbn_cont_citlivost
        '
        Me.rbn_cont_citlivost.Control = Me.TBCitlivost
        Me.rbn_cont_citlivost.Key = "rbn_cont_citlivost"
        Me.rbn_cont_citlivost.Name = "rbn_cont_citlivost"
        Me.rbn_cont_citlivost.SuperTipSettings.HeaderText = "Citlivosù myöi"
        Me.rbn_cont_citlivost.SuperTipSettings.ImageListProvider = Me.rbn_cont_citlivost
        Me.rbn_cont_citlivost.SuperTipSettings.Text = "Moûnosù meniù citlivosù myöi pri ot·ËanÌ  zobrazovan˝ch objektov"
        Me.rbn_cont_citlivost.Text = "Citlivosù myöi"
        '
        'rbnBtnReset
        '
        Me.rbnBtnReset.Image = CType(resources.GetObject("rbnBtnReset.Image"), System.Drawing.Image)
        Me.rbnBtnReset.Key = "rbnBtnReset"
        Me.rbnBtnReset.Name = "rbnBtnReset"
        Me.rbnBtnReset.SizeStyle = Janus.Windows.Ribbon.CommandSizeStyle.Small
        Me.rbnBtnReset.Style = Janus.Windows.Ribbon.CommandStyle.ImageAndText
        Me.rbnBtnReset.SuperTipSettings.HeaderText = "Reset nastavenÌ"
        Me.rbnBtnReset.SuperTipSettings.ImageListProvider = Me.rbnBtnReset
        Me.rbnBtnReset.SuperTipSettings.Text = "Z registrov sa zmaû˙ vöetky nastavenia ktorÈ sa tam uloûili poËas pr·ce s program" &
    "om. Po reötarte aplik·cie bude pouûitÈ prednastavenÈ prostredie"
        Me.rbnBtnReset.Text = "Reset nastavenÌ"
        '
        'rbnTabOstatne
        '
        Me.rbnTabOstatne.Groups.AddRange(New Janus.Windows.Ribbon.RibbonGroup() {Me.rbnGrpColors, Me.rbnGrpPomoc})
        Me.rbnTabOstatne.Key = "rbnTabOstatne"
        Me.rbnTabOstatne.Name = "rbnTabOstatne"
        Me.rbnTabOstatne.Text = "OstatnÈ"
        '
        'rbnGrpColors
        '
        Me.rbnGrpColors.Commands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.rbnBtnVS_Blue, Me.rbnBtnVS_Silver, Me.rbnBtnVS_Black})
        Me.rbnGrpColors.DialogButtonSuperTipSettings.ImageListProvider = Me.rbnGrpColors
        Me.rbnGrpColors.ImageKey = ""
        Me.rbnGrpColors.Key = "rbnGrpColors"
        Me.rbnGrpColors.Name = "rbnGrpColors"
        Me.rbnGrpColors.SuperTipSettings.ImageListProvider = Me.rbnGrpColors
        Me.rbnGrpColors.Text = "Farby"
        '
        'rbnBtnVS_Blue
        '
        Me.rbnBtnVS_Blue.Icon = CType(resources.GetObject("rbnBtnVS_Blue.Icon"), System.Drawing.Icon)
        Me.rbnBtnVS_Blue.Key = "rbnBtnVS_Blue"
        Me.rbnBtnVS_Blue.Name = "rbnBtnVS_Blue"
        Me.rbnBtnVS_Blue.SizeStyle = Janus.Windows.Ribbon.CommandSizeStyle.Small
        Me.rbnBtnVS_Blue.Style = Janus.Windows.Ribbon.CommandStyle.ImageAndText
        Me.rbnBtnVS_Blue.SuperTipSettings.ImageListProvider = Me.rbnBtnVS_Blue
        Me.rbnBtnVS_Blue.Text = "Modr·"
        '
        'rbnBtnVS_Silver
        '
        Me.rbnBtnVS_Silver.Icon = CType(resources.GetObject("rbnBtnVS_Silver.Icon"), System.Drawing.Icon)
        Me.rbnBtnVS_Silver.Key = "rbnBtnVS_Silver"
        Me.rbnBtnVS_Silver.Name = "rbnBtnVS_Silver"
        Me.rbnBtnVS_Silver.SizeStyle = Janus.Windows.Ribbon.CommandSizeStyle.Small
        Me.rbnBtnVS_Silver.Style = Janus.Windows.Ribbon.CommandStyle.ImageAndText
        Me.rbnBtnVS_Silver.SuperTipSettings.ImageListProvider = Me.rbnBtnVS_Silver
        Me.rbnBtnVS_Silver.Text = "Strieborn·"
        '
        'rbnBtnVS_Black
        '
        Me.rbnBtnVS_Black.Icon = CType(resources.GetObject("rbnBtnVS_Black.Icon"), System.Drawing.Icon)
        Me.rbnBtnVS_Black.Key = "rbnBtnVS_Black"
        Me.rbnBtnVS_Black.Name = "rbnBtnVS_Black"
        Me.rbnBtnVS_Black.SizeStyle = Janus.Windows.Ribbon.CommandSizeStyle.Small
        Me.rbnBtnVS_Black.Style = Janus.Windows.Ribbon.CommandStyle.ImageAndText
        Me.rbnBtnVS_Black.SuperTipSettings.ImageListProvider = Me.rbnBtnVS_Black
        Me.rbnBtnVS_Black.Text = "»ierna"
        '
        'rbnGrpPomoc
        '
        Me.rbnGrpPomoc.Commands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.rbnBtnAbout})
        Me.rbnGrpPomoc.DialogButtonSuperTipSettings.ImageListProvider = Me.rbnGrpPomoc
        Me.rbnGrpPomoc.ImageKey = ""
        Me.rbnGrpPomoc.Key = "rbnGrpPomoc"
        Me.rbnGrpPomoc.Name = "rbnGrpPomoc"
        Me.rbnGrpPomoc.SuperTipSettings.ImageListProvider = Me.rbnGrpPomoc
        '
        'rbnBtnAbout
        '
        Me.rbnBtnAbout.Image = CType(resources.GetObject("rbnBtnAbout.Image"), System.Drawing.Image)
        Me.rbnBtnAbout.Key = "rbnBtnAbout"
        Me.rbnBtnAbout.Name = "rbnBtnAbout"
        Me.rbnBtnAbout.Style = Janus.Windows.Ribbon.CommandStyle.ImageAndText
        Me.rbnBtnAbout.SuperTipSettings.ImageListProvider = Me.rbnBtnAbout
        Me.rbnBtnAbout.Text = "O aplik·cii"
        '
        'rbnPopupBackGrid
        '
        Me.rbnPopupBackGrid.Name = "rbnPopupBackGrid"
        '
        'rbnPopupGrid
        '
        Me.rbnPopupGrid.Name = "rbnPopupGrid"
        '
        'rbnPopupHeaderGrid
        '
        Me.rbnPopupHeaderGrid.Name = "rbnPopupHeaderGrid"
        '
        'rbnListBoxMenu
        '
        Me.rbnListBoxMenu.Name = "rbnListBoxMenu"
        '
        'rbnMnuDyn
        '
        Me.rbnMnuDyn.Name = "rbnMnuDyn"
        '
        'panelManager
        '
        Me.panelManager.ContainerControl = Me
        Me.panelManager.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007
        Me.panelManager.VisualStyleManager = Me.VSM
        Me.uiSN.Id = New System.Guid("68b6b267-34d8-46f6-993f-5631aa0f7530")
        Me.panelManager.Panels.Add(Me.uiSN)
        Me.uiGrid.Id = New System.Guid("64ee3ff7-b94b-4ac6-b5b0-79663952775d")
        Me.panelManager.Panels.Add(Me.uiGrid)
        Me.uiDetail.Id = New System.Guid("51b96c11-d862-45fd-b4de-80b64bbc50b5")
        Me.panelManager.Panels.Add(Me.uiDetail)
        '
        'Design Time Panel Info:
        '
        Me.panelManager.BeginPanelInfo()
        Me.panelManager.AddDockPanelInfo(New System.Guid("68b6b267-34d8-46f6-993f-5631aa0f7530"), Janus.Windows.UI.Dock.PanelDockStyle.Left, New System.Drawing.Size(200, 326), True)
        Me.panelManager.AddDockPanelInfo(New System.Guid("51b96c11-d862-45fd-b4de-80b64bbc50b5"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, New System.Drawing.Size(829, 172), True)
        Me.panelManager.AddDockPanelInfo(New System.Guid("64ee3ff7-b94b-4ac6-b5b0-79663952775d"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, New System.Drawing.Size(829, 154), True)
        Me.panelManager.AddFloatingPanelInfo(New System.Guid("68b6b267-34d8-46f6-993f-5631aa0f7530"), New System.Drawing.Point(-1, -1), New System.Drawing.Size(-1, -1), False)
        Me.panelManager.AddFloatingPanelInfo(New System.Guid("51b96c11-d862-45fd-b4de-80b64bbc50b5"), New System.Drawing.Point(-1, -1), New System.Drawing.Size(-1, -1), False)
        Me.panelManager.AddFloatingPanelInfo(New System.Guid("64ee3ff7-b94b-4ac6-b5b0-79663952775d"), New System.Drawing.Point(-1, -1), New System.Drawing.Size(-1, -1), False)
        Me.panelManager.EndPanelInfo()
        '
        'uiSN
        '
        Me.uiSN.AllowPanelDrag = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiSN.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiSN.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiSN.InnerContainer = Me.uiSNContainer
        Me.uiSN.Location = New System.Drawing.Point(3, 149)
        Me.uiSN.Name = "uiSN"
        Me.uiSN.Size = New System.Drawing.Size(200, 326)
        Me.uiSN.TabIndex = 1
        Me.uiSN.Text = "V˝robnÈ ËÌsla"
        '
        'uiSNContainer
        '
        Me.uiSNContainer.Controls.Add(Me.lboxSN)
        Me.uiSNContainer.Location = New System.Drawing.Point(1, 23)
        Me.uiSNContainer.Name = "uiSNContainer"
        Me.uiSNContainer.Size = New System.Drawing.Size(194, 302)
        Me.uiSNContainer.TabIndex = 0
        '
        'lboxSN
        '
        Me.lboxSN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lboxSN.FormattingEnabled = True
        Me.lboxSN.Location = New System.Drawing.Point(0, 0)
        Me.lboxSN.Name = "lboxSN"
        Me.lboxSN.Size = New System.Drawing.Size(194, 302)
        Me.lboxSN.TabIndex = 0
        Me.lboxSN.Tag = "CISEL"
        '
        'uiGrid
        '
        Me.uiGrid.AllowPanelDrag = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiGrid.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiGrid.AutoHideButtonVisible = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiGrid.CaptionDoubleClickAction = Janus.Windows.UI.Dock.CaptionDoubleClickAction.None
        Me.uiGrid.CaptionVisible = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiGrid.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiGrid.InnerContainer = Me.uiGridContainer
        Me.uiGrid.Location = New System.Drawing.Point(203, 321)
        Me.uiGrid.Name = "uiGrid"
        Me.uiGrid.Size = New System.Drawing.Size(829, 154)
        Me.uiGrid.TabIndex = 3
        Me.uiGrid.Text = "⁄daje o vybranej simul·cii"
        '
        'uiGridContainer
        '
        Me.uiGridContainer.Controls.Add(Me.splSim)
        Me.uiGridContainer.Location = New System.Drawing.Point(1, 5)
        Me.uiGridContainer.Name = "uiGridContainer"
        Me.uiGridContainer.Size = New System.Drawing.Size(827, 148)
        Me.uiGridContainer.TabIndex = 0
        '
        'splSim
        '
        Me.splSim.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splSim.Location = New System.Drawing.Point(0, 0)
        Me.splSim.Name = "splSim"
        '
        'splSim.Panel1
        '
        Me.splSim.Panel1.Controls.Add(Me.MJG)
        '
        'splSim.Panel2
        '
        Me.splSim.Panel2.Controls.Add(Me.grpSelSim)
        Me.splSim.Size = New System.Drawing.Size(827, 148)
        Me.splSim.SplitterDistance = 589
        Me.splSim.TabIndex = 2
        '
        'MJG
        '
        Me.MJG.AbrNavigate = ""
        Me.MJG.AbrPopupBackGrid = ""
        Me.MJG.AbrPopupGrid = ""
        Me.MJG.AbrPopupHeaderGrid = ""
        Me.MJG.AbrStandard = ""
        Me.MJG.AbrSubor = ""
        Me.MJG.AbrUpravy = ""
        Me.MJG.AbrZobrazit = ""
        Me.MJG.AcceptedGridNotInListValueCurrent = False
        Me.MJG.AllowAddNew = False
        Me.MJG.AllowDelete = False
        Me.MJG.AllowEdit = True
        Me.MJG.ApplicationName = "MEMPHIS"
        Me.MJG.AutoGridFilter = False
        Me.MJG.AutomaticSort = True
        Me.MJG.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(239, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.MJG.BackgroundColor = 16773091
        Me.MJG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.MJG.CardShow = False
        Me.MJG.DataChanged = False
        Me.MJG.Description = ""
        Me.MJG.DetectRowDrag = False
        Me.MJG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MJG.FilterPanel = False
        Me.MJG.ForeColor = System.Drawing.SystemColors.ControlText
        Me.MJG.ForegroundColor = -2147483630
        Me.MJG.FrozenColumns = 0
        Me.MJG.GroupByBoxVisible = False
        Me.MJG.GroupFieldsVisible = False
        Me.MJG.Location = New System.Drawing.Point(0, 0)
        Me.MJG.Menu_AllowGroupByBoxVisible = True
        Me.MJG.Menu_ManageReportsToStandardBand = False
        Me.MJG.MultiSelect = False
        Me.MJG.Name = "MJG"
        Me.MJG.NewRowPositionTop = False
        Me.MJG.RbnCBM = True
        Me.MJG.RbnHome = ""
        Me.MJG.RbnNavigate = ""
        Me.MJG.RbnOperacie = ""
        Me.MJG.RbnSchranka = ""
        Me.MJG.RbnUpravy = ""
        Me.MJG.RbnZoskupovanie = ""
        Me.MJG.RecordNavigator = False
        Me.MJG.RibbonBar = Nothing
        Me.MJG.RibbonPopupBackGrid = Nothing
        Me.MJG.RibbonPopupGrid = Nothing
        Me.MJG.RibbonPopupHeaderGrid = Nothing
        Me.MJG.RowHeaders = False
        Me.MJG.SingleCardView = False
        Me.MJG.Size = New System.Drawing.Size(589, 148)
        Me.MJG.TabIndex = 0
        '
        'grpSelSim
        '
        Me.grpSelSim.Controls.Add(Me.Label3)
        Me.grpSelSim.Controls.Add(Me.txtVyrez)
        Me.grpSelSim.Controls.Add(Me.Label1)
        Me.grpSelSim.Controls.Add(Me.txtOdrezok)
        Me.grpSelSim.Controls.Add(Me.labObjemCapt)
        Me.grpSelSim.Controls.Add(Me.labPocKsCapt)
        Me.grpSelSim.Controls.Add(Me.txtPolotovar)
        Me.grpSelSim.Controls.Add(Me.labPocKs)
        Me.grpSelSim.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpSelSim.Location = New System.Drawing.Point(0, 0)
        Me.grpSelSim.Name = "grpSelSim"
        Me.grpSelSim.Size = New System.Drawing.Size(234, 148)
        Me.grpSelSim.TabIndex = 1
        Me.grpSelSim.TabStop = False
        Me.grpSelSim.Text = "Zvolen· simul·cia"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(50, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Odrezok:"
        '
        'txtVyrez
        '
        Me.txtVyrez.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVyrez.Location = New System.Drawing.Point(85, 80)
        Me.txtVyrez.Name = "txtVyrez"
        Me.txtVyrez.Size = New System.Drawing.Size(143, 13)
        Me.txtVyrez.TabIndex = 7
        Me.txtVyrez.Text = "0"
        Me.txtVyrez.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Polotovar:"
        '
        'txtOdrezok
        '
        Me.txtOdrezok.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOdrezok.Location = New System.Drawing.Point(85, 67)
        Me.txtOdrezok.Name = "txtOdrezok"
        Me.txtOdrezok.Size = New System.Drawing.Size(143, 13)
        Me.txtOdrezok.TabIndex = 5
        Me.txtOdrezok.Text = "0"
        Me.txtOdrezok.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'labObjemCapt
        '
        Me.labObjemCapt.AutoSize = True
        Me.labObjemCapt.Location = New System.Drawing.Point(9, 80)
        Me.labObjemCapt.Name = "labObjemCapt"
        Me.labObjemCapt.Size = New System.Drawing.Size(36, 13)
        Me.labObjemCapt.TabIndex = 2
        Me.labObjemCapt.Text = "V˝rez:"
        '
        'labPocKsCapt
        '
        Me.labPocKsCapt.AutoSize = True
        Me.labPocKsCapt.Location = New System.Drawing.Point(9, 30)
        Me.labPocKsCapt.Name = "labPocKsCapt"
        Me.labPocKsCapt.Size = New System.Drawing.Size(70, 13)
        Me.labPocKsCapt.TabIndex = 0
        Me.labPocKsCapt.Text = "PoËet kusov:"
        '
        'txtPolotovar
        '
        Me.txtPolotovar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPolotovar.Location = New System.Drawing.Point(85, 54)
        Me.txtPolotovar.Name = "txtPolotovar"
        Me.txtPolotovar.Size = New System.Drawing.Size(143, 13)
        Me.txtPolotovar.TabIndex = 3
        Me.txtPolotovar.Text = "0"
        Me.txtPolotovar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'labPocKs
        '
        Me.labPocKs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.labPocKs.Location = New System.Drawing.Point(85, 30)
        Me.labPocKs.Name = "labPocKs"
        Me.labPocKs.Size = New System.Drawing.Size(143, 13)
        Me.labPocKs.TabIndex = 1
        Me.labPocKs.Text = "0"
        Me.labPocKs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uiDetail
        '
        Me.uiDetail.AllowPanelDrag = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiDetail.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiDetail.AutoHideButtonVisible = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiDetail.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.[False]
        Me.uiDetail.InnerContainer = Me.uiDetailContainer
        Me.uiDetail.Location = New System.Drawing.Point(203, 149)
        Me.uiDetail.Name = "uiDetail"
        Me.uiDetail.Size = New System.Drawing.Size(829, 172)
        Me.uiDetail.TabIndex = 2
        Me.uiDetail.Text = "## Skupina, KÛd, N·zov, MJ"
        '
        'uiDetailContainer
        '
        Me.uiDetailContainer.Controls.Add(Me.uiGrpBox)
        Me.uiDetailContainer.Location = New System.Drawing.Point(1, 23)
        Me.uiDetailContainer.Name = "uiDetailContainer"
        Me.uiDetailContainer.Size = New System.Drawing.Size(827, 148)
        Me.uiDetailContainer.TabIndex = 0
        '
        'uiGrpBox
        '
        Me.uiGrpBox.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.ExplorerBarGroupBackground
        Me.uiGrpBox.Controls.Add(Me.FLP)
        Me.uiGrpBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uiGrpBox.FrameStyle = Janus.Windows.EditControls.FrameStyle.None
        Me.uiGrpBox.Location = New System.Drawing.Point(0, 0)
        Me.uiGrpBox.Name = "uiGrpBox"
        Me.uiGrpBox.Size = New System.Drawing.Size(827, 148)
        Me.uiGrpBox.TabIndex = 33
        Me.uiGrpBox.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007
        '
        'FLP
        '
        Me.FLP.AutoScroll = True
        Me.FLP.BackColor = System.Drawing.Color.Transparent
        Me.FLP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FLP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FLP.Location = New System.Drawing.Point(0, 0)
        Me.FLP.Name = "FLP"
        Me.FLP.Size = New System.Drawing.Size(827, 148)
        Me.FLP.TabIndex = 0
        '
        'RSB
        '
        Me.RSB.Controls.Add(Me.PGB)
        Me.RSB.ImageSize = New System.Drawing.Size(16, 16)
        Me.RSB.Location = New System.Drawing.Point(0, 478)
        Me.RSB.Name = "RSB"
        Me.RSB.Office2007CustomColor = System.Drawing.Color.Empty
        Me.RSB.RightPanelCommands.AddRange(New Janus.Windows.Ribbon.CommandBase() {Me.sbrOperacia})
        Me.RSB.Size = New System.Drawing.Size(1035, 23)
        '
        '
        '
        Me.RSB.SuperTipComponent.AutoPopDelay = 2000
        Me.RSB.SuperTipComponent.ImageList = Nothing
        Me.RSB.TabIndex = 4
        Me.RSB.Text = "RSB"
        Me.RSB.UseCompatibleTextRendering = False
        '
        'PGB
        '
        Me.PGB.Location = New System.Drawing.Point(817, 1)
        Me.PGB.Name = "PGB"
        Me.PGB.ShowPercentage = True
        Me.PGB.Size = New System.Drawing.Size(200, 25)
        Me.PGB.TabIndex = 0
        Me.PGB.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
        Me.PGB.VisualStyleManager = Me.VSM
        '
        'sbrOperacia
        '
        Me.sbrOperacia.Control = Me.PGB
        Me.sbrOperacia.ControlWidth = 200
        Me.sbrOperacia.Key = "sbrOperacia"
        Me.sbrOperacia.Name = "sbrOperacia"
        Me.sbrOperacia.SuperTipSettings.ImageListProvider = Me.sbrOperacia
        Me.sbrOperacia.Text = "##Oper·cia"
        '
        'CheckBoxCommand1
        '
        '
        '
        '
        Me.CheckBoxCommand1.CheckBox.BackColor = System.Drawing.Color.Transparent
        Me.CheckBoxCommand1.CheckBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.CheckBoxCommand1.CheckBox.Location = New System.Drawing.Point(617, 59)
        Me.CheckBoxCommand1.CheckBox.Name = ""
        Me.CheckBoxCommand1.CheckBox.ShowFocusRectangle = False
        Me.CheckBoxCommand1.CheckBox.Size = New System.Drawing.Size(20, 22)
        Me.CheckBoxCommand1.CheckBox.TabIndex = 75
        Me.CheckBoxCommand1.CheckBox.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007
        Me.CheckBoxCommand1.ControlWidth = 20
        Me.CheckBoxCommand1.Key = "rbn_AA"
        Me.CheckBoxCommand1.Name = "rbn_AA"
        Me.CheckBoxCommand1.SuperTipSettings.ImageListProvider = Me.CheckBoxCommand1
        Me.CheckBoxCommand1.Text = "Anti aliasing"
        '
        'FormD3dGraphic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(239, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1035, 501)
        Me.Controls.Add(Me.uiDetail)
        Me.Controls.Add(Me.uiGrid)
        Me.Controls.Add(Me.uiSN)
        Me.Controls.Add(Me.RSB)
        Me.Controls.Add(Me.rbnMAIN)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormD3dGraphic"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Simul·cie 3D sklad"
        CType(Me.rbnMAIN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.rbnMAIN.ResumeLayout(False)
        Me.rbnMAIN.PerformLayout()
        CType(Me.panelManager, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uiSN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uiSN.ResumeLayout(False)
        Me.uiSNContainer.ResumeLayout(False)
        CType(Me.uiGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uiGrid.ResumeLayout(False)
        Me.uiGridContainer.ResumeLayout(False)
        Me.splSim.Panel1.ResumeLayout(False)
        Me.splSim.Panel2.ResumeLayout(False)
        CType(Me.splSim, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splSim.ResumeLayout(False)
        Me.grpSelSim.ResumeLayout(False)
        Me.grpSelSim.PerformLayout()
        CType(Me.uiDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uiDetail.ResumeLayout(False)
        Me.uiDetailContainer.ResumeLayout(False)
        CType(Me.uiGrpBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uiGrpBox.ResumeLayout(False)
        Me.RSB.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uiControl As Janus.Windows.UI.Dock.UIPanel
  Friend WithEvents VSM As Janus.Windows.Common.VisualStyleManager
  Friend WithEvents rbnMAIN As Janus.Windows.Ribbon.Ribbon
  Friend WithEvents rbnTabOstatne As Janus.Windows.Ribbon.RibbonTab
  Friend WithEvents rbnGrpColors As Janus.Windows.Ribbon.RibbonGroup
  Friend WithEvents rbnTabDomov As Janus.Windows.Ribbon.RibbonTab
  Friend WithEvents rbnBtnReset As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents rbnGrpPomoc As Janus.Windows.Ribbon.RibbonGroup
  Friend WithEvents rbnBtnAbout As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents rbnBtnVS_Blue As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents rbnBtnVS_Silver As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents rbnBtnVS_Black As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents rbnPopupBackGrid As Janus.Windows.Ribbon.RibbonContextMenu
  Friend WithEvents rbnPopupGrid As Janus.Windows.Ribbon.RibbonContextMenu
  Friend WithEvents rbnPopupHeaderGrid As Janus.Windows.Ribbon.RibbonContextMenu
  Friend WithEvents rbnListBoxMenu As Janus.Windows.Ribbon.RibbonContextMenu
  Friend WithEvents rbnMnuDyn As Janus.Windows.Ribbon.RibbonContextMenu
  Friend WithEvents rbnGrpOperations As Janus.Windows.Ribbon.RibbonGroup
  Friend WithEvents rbnBtnCalc As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents rbnBtnExit As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents rbnGrpInput As Janus.Windows.Ribbon.RibbonGroup
  Friend WithEvents panelManager As Janus.Windows.UI.Dock.UIPanelManager
  Friend WithEvents uiDetail As Janus.Windows.UI.Dock.UIPanel
  Friend WithEvents uiDetailContainer As Janus.Windows.UI.Dock.UIPanelInnerContainer
  Friend WithEvents uiSN As Janus.Windows.UI.Dock.UIPanel
  Friend WithEvents uiSNContainer As Janus.Windows.UI.Dock.UIPanelInnerContainer
  Friend WithEvents lboxSN As System.Windows.Forms.ListBox
  Friend WithEvents rbn_aD As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents cont_rbn_aD As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents rbn_L As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents rbn_bd As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents cont_rbn_bd As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents cont_rbn_L As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents rbn_poc_ks As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents cont_rbn_poc_ks As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents rbn_threshold As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents rbn_max_rez As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents TBSize As Janus.Windows.EditControls.UITrackBar
  Friend WithEvents TBCitlivost As Janus.Windows.EditControls.UITrackBar
  Friend WithEvents rbnGrpSimSet As Janus.Windows.Ribbon.RibbonGroup
  Friend WithEvents rbn_cont_max_rez As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents rbn_cont_threshold As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents rbnSeparator1 As Janus.Windows.Ribbon.SeparatorCommand
  Friend WithEvents rbn_AA As Janus.Windows.Ribbon.CheckBoxCommand
  Friend WithEvents rbnSeparator2 As Janus.Windows.Ribbon.SeparatorCommand
  Friend WithEvents rbn_cont_velkost As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents rbn_cont_citlivost As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents uiGrpBox As Janus.Windows.EditControls.UIGroupBox
  Friend WithEvents FLP As System.Windows.Forms.FlowLayoutPanel
  Friend WithEvents RSB As Janus.Windows.Ribbon.RibbonStatusBar
  Friend WithEvents PGB As Janus.Windows.EditControls.UIProgressBar
  Friend WithEvents sbrOperacia As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents rbnBtnOk As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents rbnBtnClose As Janus.Windows.Ribbon.ButtonCommand
  Friend WithEvents uiGrid As Janus.Windows.UI.Dock.UIPanel
  Friend WithEvents uiGridContainer As Janus.Windows.UI.Dock.UIPanelInnerContainer
  Friend WithEvents MJG As MemControls.MemJG
  Friend WithEvents grpSelSim As System.Windows.Forms.GroupBox
  Friend WithEvents txtPolotovar As System.Windows.Forms.Label
  Friend WithEvents labPocKs As System.Windows.Forms.Label
  Friend WithEvents labObjemCapt As System.Windows.Forms.Label
  Friend WithEvents labPocKsCapt As System.Windows.Forms.Label
  Friend WithEvents rbn_MaxSim As Janus.Windows.GridEX.EditControls.IntegerUpDown
  Friend WithEvents rbn_cont_MaxSim As Janus.Windows.Ribbon.ContainerControlCommand
  Friend WithEvents rbn_RunOnStart As Janus.Windows.Ribbon.CheckBoxCommand
  Friend WithEvents CheckBoxCommand1 As Janus.Windows.Ribbon.CheckBoxCommand
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents txtVyrez As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents txtOdrezok As System.Windows.Forms.Label
  Friend WithEvents splSim As System.Windows.Forms.SplitContainer
  Friend WithEvents rbn_SVP As Janus.Windows.Ribbon.CheckBoxCommand
End Class
