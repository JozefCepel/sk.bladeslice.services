Option Explicit On

Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.IO
Imports Janus.Windows
Imports Newtonsoft.Json
Imports System.Linq

Public Class FormD3dGraphic

    Private colSNKeys As Collection
    Private colSNObjectShapes As Collection
    Private grd As New clsGrid

    Friend Enum D3dOperation
        D3dOperation_SimBlok = 1
        D3dOperation_SimValec = 2
        D3dOperation_ShowBlok = 3
        D3dOperation_ShowValec = 4
    End Enum

    Private _D3dOp As D3dOperation

    Private Sub frmImport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Koniec(False, False)
    End Sub

    Private Sub Koniec(ByVal bSaveSettings As Boolean, Optional ByVal bExitApp As Boolean = True)
        Static Dim bAlreadyExiting As Boolean
        '  Nezabudni za volanim dat Exit Sub lebo po vykonani bude kod bezat este do konca
        If bAlreadyExiting Then Exit Sub ' Aby sa nespustalo 2x (Application.Exit spusta zase Form.Close)
        If bSaveSettings Then
            If Me.WindowState = FormWindowState.Minimized Then Me.WindowState = FormWindowState.Normal
            If Me.WindowState = FormWindowState.Maximized Then
                SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Max", "1")
            Else
                SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Width", Me.Width.ToString)
                SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Height", Me.Height.ToString)
                SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Left", Me.Left.ToString)
                SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Top", Me.Top.ToString)
                SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Max", "0")
            End If

            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "maxRez", Me.rbn_max_rez.Value)
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "Threshold", Me.rbn_threshold.Value)
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "MaxSim", Me.rbn_MaxSim.Value)
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "chbAA", Me.rbn_AA.Checked)
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "chbSVP", Me.rbn_SVP.Checked)
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "RunOnStart", Me.rbn_RunOnStart.Checked)
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "TBSize", Me.TBSize.Value)
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "TBCitlivost", Me.TBCitlivost.Value)
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "SplitterResult", Me.splSim.SplitterDistance)
        End If
        Clean()
        ' RemoveHandler lboxTmp.SelectedIndexChanged  - App si to vycisti sama
        If bExitApp Then
            bAlreadyExiting = True
            Application.Exit()
        End If
    End Sub

    Private Sub FormD3dGraphic_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Nacitanie zakladnych objektov

        Dim sTmp As String
        Dim i As Integer

        Try
            sTmp = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Max")
            If sTmp <> "" Then
                If sTmp = "1" Then
                    Me.WindowState = FormWindowState.Maximized
                Else
                    Me.Width = CInt(GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Width", Me.Width.ToString))
                    Me.Height = CInt(GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Height", Me.Height.ToString))
                    i = CInt(GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Left", Me.Left.ToString))
                    Me.Left = i
                    i = CInt(GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Top", Me.Top.ToString))
                    Me.Top = i
                End If

                Me.rbn_max_rez.Value = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "maxRez", Me.rbn_max_rez.Value)
                Me.rbn_threshold.Value = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "Threshold", Me.rbn_threshold.Value)
                Me.rbn_MaxSim.Value = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "MaxSim", Me.rbn_MaxSim.Value)

                Me.rbn_AA.Checked = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "chbAA", Me.rbn_AA.Checked)
                Me.rbn_SVP.Checked = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "chbSVP", Me.rbn_SVP.Checked)
                Me.rbn_RunOnStart.Checked = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "RunOnStart", Me.rbn_RunOnStart.Checked)
                Me.TBSize.Value = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "TBSize", Me.TBSize.Value)
                Me.TBCitlivost.Value = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "TBCitlivost", Me.TBCitlivost.Value)
                Me.splSim.SplitterDistance = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "SplitterResult", "800")

                sTmp = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Schema")
                If sTmp <> "" Then DoColors(sTmp)
            End If
        Catch ex As Exception
            ' Nieco zblblo, zmaz nastavenia 
            MsgBox("Nastala chyba pri naËÌtanÌ konfigur·cie," & vbCrLf & "bud˙ obnovenÈ z·kladnÈ nastavenia!" & vbCrLf & vbCrLf & "(" & ex.Message & ")", MsgBoxStyle.Exclamation, "Kontrola")
            DeleteSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME)
        End Try

        Try
            grd.CreateMyTable()
            grd.InitControlGrid(Me, MJG, String.Empty, String.Empty, _D3dOp)
            'Me.memJG.Enabled = False
            If Me.rbn_RunOnStart.Checked Then Run()
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
        sbrOperacia.Text = ""
        PGB.Visible = False
    End Sub

    Private Sub lboxSN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lboxSN.SelectedIndexChanged
        Dim lb As ListBox = CType(sender, ListBox)

        If lb.SelectedItem Is Nothing Then Exit Sub
        Dim o As Object = CType(lb.SelectedItem, ValueDescriptionPair).ValueObj
        If TypeOf o Is MultiSimulationBlok Then
            Dim ms As MultiSimulationBlok = DirectCast(o, MultiSimulationBlok)
            FLP.ScrollControlIntoView(ms)
        ElseIf TypeOf o Is MultiSimulationValec Then
            Dim ms As MultiSimulationValec = DirectCast(o, MultiSimulationValec)
            FLP.ScrollControlIntoView(ms)
        End If
    End Sub

    ' ********************* Dynamicke context menu *********************
    Private Sub rbnMnuDyn_CommandClick(ByVal sender As System.Object, ByVal e As Janus.Windows.Ribbon.CommandEventArgs) Handles rbnMnuDyn.CommandClick
        Select Case e.Command.Key
            Case "xx"

            Case "yy"

            Case "zz"

            Case Else
                MsgBox("Nezn·ma akcia: " & e.Command.Key & vbCrLf & "Kontaktujte dod·vateæa aplik·cie", MsgBoxStyle.Exclamation)
        End Select
    End Sub

    ' MENU CLICK
    Private Sub MenuCommandClick(ByVal sCommand As String)
        If sCommand.StartsWith("rbnBtn") Then sCommand = Replace(sCommand, "rbnBtn", "", , , CompareMethod.Binary)
        Select Case sCommand
            Case "VS_Black", "VS_Blue", "VS_Silver"
                DoColors(sCommand)
            Case "Reset"
                If MsgBox("Naozaj chcete zmazaù nastavenia aplik·cie?" & vbCrLf &
                          "Aplik·cia bude ukonËen· a bude ju treba nanovo spustiù.",
                          MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Vymazaù") = MsgBoxResult.Yes Then
                    Dim rk As Microsoft.Win32.RegistryKey
                    DeleteSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME)
                    On Error Resume Next
                    DeleteSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME & "_" & CStr(D3dOperation.D3dOperation_SimBlok))
                    DeleteSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME & "_" & CStr(D3dOperation.D3dOperation_SimValec))
                    DeleteSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME & "_" & CStr(D3dOperation.D3dOperation_ShowBlok))
                    DeleteSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME & "_" & CStr(D3dOperation.D3dOperation_ShowValec))
                    On Error GoTo 0
                    Koniec(False)
                End If
            Case "Run"
                Run()
            Case "Ok"
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Koniec(True, False)
            Case "Exit", "Close"
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Koniec(False, False)
        End Select
    End Sub

    Private Sub DoColors(ByVal sSchema As String)
        Dim Farba As System.Drawing.Color, Schema As Common.Office2007ColorScheme
        rbnBtnVS_Blue.Checked = False
        rbnBtnVS_Silver.Checked = False
        rbnBtnVS_Black.Checked = False
        Select Case sSchema
            Case "VS_Black"
                rbnBtnVS_Black.Checked = True
                Farba = Color.FromArgb(&HF0, &HF1, &HF2)
                Schema = Common.Office2007ColorScheme.Black
            Case "VS_Silver"
                rbnBtnVS_Silver.Checked = True
                Farba = Color.FromArgb(&HF0, &HF1, &HF2)
                Schema = Common.Office2007ColorScheme.Silver
            Case Else ' Ak sa nenajde daj default
                sSchema = "VS_Blue"
                rbnBtnVS_Blue.Checked = True
                Farba = Color.FromArgb(227, 239, 255)
                Schema = Common.Office2007ColorScheme.Blue
        End Select
        Me.BackColor = Farba
        Common.VisualStyleManager.DefaultOffice2007ColorScheme = Schema
        SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME, "Schema", sSchema)
    End Sub

    Private Sub rbnMAIN_CommandClick(ByVal sender As System.Object, ByVal e As Janus.Windows.Ribbon.CommandEventArgs) Handles rbnMAIN.CommandClick
        MenuCommandClick(e.Command.Key)
    End Sub

    Private Sub setRbnLabels(ByVal sCaption As String, ByVal bSim As Boolean, ByVal aD As Integer, ByVal bd As Integer, ByVal L As Integer, ByVal PocetKusov As Integer)

        uiDetail.Text = sCaption
        rbnGrpInput.Visible = bSim
        rbn_cont_max_rez.Visible = bSim And Not _D3dOp = D3dOperation.D3dOperation_SimValec
        rbn_cont_threshold.Visible = bSim And Not _D3dOp = D3dOperation.D3dOperation_SimValec

        If bSim Then
            If _D3dOp = D3dOperation.D3dOperation_SimValec Then
                cont_rbn_aD.Text = "D [" & sMjRozmery & "]"
                cont_rbn_bd.Text = "d [" & sMjRozmery & "]"
                rbn_bd.Minimum = 0
            Else
                cont_rbn_aD.Text = "a [" & sMjRozmery & "]"
                cont_rbn_bd.Text = "b [" & sMjRozmery & "]"
                rbn_bd.Minimum = 1
            End If
            cont_rbn_L.Text = "L [" & sMjRozmery & "]"

            rbn_aD.Value = aD
            rbn_bd.Value = bd
            rbn_L.Value = L
            rbn_poc_ks.Value = PocetKusov
        End If
        uiSN.Closed = Not bSim

        HKEY_SECTION_NAME_TYP = HKEY_SECTION_NAME & "_" & CStr(_D3dOp)

    End Sub

    Private Sub DebugLog(ByVal sTxt As String) ' Log to file debug info
        Dim sFile As String = ""
        Try
            'sFile = Application.StartupPath & "\Log.txt"
            sFile = "C:\Log.txt"
            File.AppendAllText(sFile, sTxt & vbCrLf, System.Text.Encoding.Default)
        Catch ex As Exception
            MsgBox("Error saving '" & sFile & "'." & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Function RunValec(ByVal sCaption As String, ByVal bOnlyShow As Boolean,
                          ByVal strMjObjem As String, ByVal strMjRozmery As String,
                          ByVal intPocetMiestObjem As Integer, ByVal intKoefMjObjemRozmery As Integer,
                          Optional ByVal PocetKusov As Integer = 0,
                          Optional ByRef D1 As Integer = 0, Optional ByRef d2 As Integer = 0, Optional ByRef L As Integer = 0,
                          Optional ByVal Input3DData As String = "", Optional ByRef Output3DData As String = "",
                          Optional ByVal bOnlyReturnObjem As Boolean = False, Optional ByVal bOrientedL As Boolean = False) As Decimal

        sMjObjem = strMjObjem
        sMjRozmery = strMjRozmery
        iPocetMiestObjem = intPocetMiestObjem
        iKoefMjObjemRozmery = intKoefMjObjemRozmery

        Dim CS As ValecSourceObj = Nothing
        Dim bSim As Boolean = (D1 <> 0 And L <> 0)

        _D3dOp = IIf(bOnlyShow, D3dOperation.D3dOperation_ShowValec, D3dOperation.D3dOperation_SimValec)
        If bSim Then
            If D1 <= d2 Then
                MsgBox("Vn˙torn˝ priemer (" & D1 & ") musÌ byù v‰ËöÌ ako vonkajöÌ (" & d2 & ")!", vbExclamation, "Kontrola")
                Exit Function
            End If
        End If
        setRbnLabels(sCaption, bSim, D1, d2, L, PocetKusov)

        colSNKeys = New Collection
        colSNObjectShapes = New Collection
        '*********************************
        If String.IsNullOrEmpty(Input3DData) Then
            Exit Function
        Else

            Dim fetch As D3DSource()
            Dim f As D3DSource
            Dim va As Valec

            fetch = JsonConvert.DeserializeObject(Of D3DSource())(Input3DData)
            Dim sorted = fetch.OrderBy(Function(x) x.SN)

            For Each f In sorted
                If f.Type.ToLower() = "valec" Then

                    Dim sortedvalec = f.Valec.
                        OrderBy(Function(x) x.D1).ThenBy(Function(x) x.d2).
                        ThenBy(Function(x) x.L1).ThenBy(Function(x) x.L2)

                    CS = New ValecSourceObj(f.SN, f.Sarza, f.Location, f.SklCena, f.SN, sortedvalec)

                    'Ak uû takÈ rozmey existuj˙, tak ich neprid·m, iba rozöÌrim SN
                    Dim sRef = CS.CoordToString()
                    Dim s As String
                    Dim bEx As Boolean = False

                    For Each s In colSNKeys
                        Dim v As ValecSourceObj = colSNObjectShapes(s)
                        If sRef = v.CoordToString() Then
                            bEx = True
                            v.SNs = v.SNs & "; " & f.SN
                            v.ColSNs.Add(f.SN, f.SN)
                        End If
                    Next

                    If Not bEx Then

                        For Each va In sortedvalec
                            If va.Add Then
                                CS.CoordToAdd(New CoordinatesValec(va.D1, va.d2, va.L1, va.L2))
                            Else
                                CS.CoordToRemove(New CoordinatesValec(va.D1, va.d2, va.L1, va.L2))
                            End If
                        Next

                        CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu
                        If Not IsExistKey(colSNKeys, f.SN) Then
                            colSNKeys.Add(f.SN, f.SN)
                            colSNObjectShapes.Add(CS, f.SN)
                        End If

                    End If
                End If
            Next
        End If

        If bOnlyReturnObjem Then
            If colSNObjectShapes.Count = 0 Then
                Return 0
            Else
                'Return GetValecObjem(CS.coordList)
                Return GetValecObjem(colSNObjectShapes.Item(colSNObjectShapes.Count).coordList)
            End If
        Else
            Dim ret As DialogResult = Me.ShowDialog()
            If ret = DialogResult.OK Then
                Output3DData = grd.GetOutput3DData
                D1 = rbn_aD.Value
                d2 = rbn_bd.Value
                L = rbn_L.Value
            Else
                Output3DData = ""
            End If
            Return 0
        End If

    End Function

    Public Function RunValecOLD(ByVal sCaption As String, ByVal bOnlyShow As Boolean,
                            ByVal strMjObjem As String, ByVal strMjRozmery As String,
                            ByVal intPocetMiestObjem As Integer, ByVal intKoefMjObjemRozmery As Integer,
                            Optional ByVal PocetKusov As Integer = 0, Optional ByRef D1 As Integer = 0, Optional ByRef d2 As Integer = 0, Optional ByRef L As Integer = 0,
                            Optional ByVal rsIN As ADODB.Recordset = Nothing, Optional ByRef rsOUT As ADODB.Recordset = Nothing,
                            Optional ByVal bOnlyReturnObjem As Boolean = False) As Decimal

        sMjObjem = strMjObjem
        sMjRozmery = strMjRozmery
        iPocetMiestObjem = intPocetMiestObjem
        iKoefMjObjemRozmery = intKoefMjObjemRozmery

        Dim sSN As String = "", sOldSN As String = "$#$#$#"
        Dim CS As ValecSourceObj = Nothing
        Dim bSim As Boolean = (D1 <> 0 And L <> 0)
        Dim decCena As Decimal
        Dim colSNDuplic As Collection
        Dim colSNRef As Collection
        Dim colSNs As Collection
        Dim sRef As String = ""
        Dim sOldRef As String = ""
        Dim sBefore As String
        Dim sKey As String = ""

        _D3dOp = IIf(bOnlyShow, D3dOperation.D3dOperation_ShowValec, D3dOperation.D3dOperation_SimValec)
        If bSim Then
            If D1 <= d2 Then
                MsgBox("Vn˙torn˝ priemer (" & D1 & ") musÌ byù v‰ËöÌ ako vonkajöÌ (" & d2 & ")!", vbExclamation, "Kontrola")
                Exit Function
            End If
        End If
        setRbnLabels(sCaption, bSim, D1, d2, L, PocetKusov)
        colSNKeys = New Collection
        colSNObjectShapes = New Collection
        '*********************************
        If rsIN Is Nothing Then
            Exit Function
        Else

            If rsIN.RecordCount > 0 Then ' z VB6 poslem len pokial je RecordCount > 0 

                colSNDuplic = New Collection
                colSNRef = New Collection
                colSNs = New Collection
                rsIN.Sort = "SN, D1, d2, L1, L2"
                rsIN.MoveFirst()
                sRef = ""
                sOldRef = ""
                sOldSN = "$#$#$#"
                Do Until rsIN.EOF
                    sSN = rsIN("SN").Value
                    If sSN <> sOldSN And sOldSN <> "$#$#$#" Then
                        If IsExistKey(colSNRef, sRef) Then
                            'Duplicitne
                            sKey = colSNRef(sRef)
                            colSNDuplic.Add(sKey, sOldSN)
                            sBefore = colSNs(sKey)
                            colSNs.Remove(sKey)
                            colSNs.Add(sBefore & "; " & sOldSN, sKey)
                        Else
                            colSNRef.Add(sOldSN, sRef)
                            colSNs.Add(sOldSN, sOldSN)
                        End If
                        sOldRef = sRef
                        sRef = ""
                    End If
                    sRef = sRef & CStr(rsIN("D1").Value) & "_" &
                      CStr(rsIN("D2").Value) & "_" &
                      CStr(rsIN("L1").Value) & "_" &
                      CStr(rsIN("L2").Value)
                    rsIN.MoveNext()
                    sOldSN = sSN
                Loop

                If IsExistKey(colSNRef, sRef) Then
                    'Duplicitne
                    colSNDuplic.Add(colSNRef(sRef), sOldSN)
                    sBefore = colSNs(sKey)
                    colSNs.Remove(sKey)
                    colSNs.Add(sBefore & "; " & sOldSN, sKey)
                Else
                    colSNRef.Add(sOldSN, sRef)
                    colSNs.Add(sOldSN, sOldSN)
                End If

                sOldSN = "$#$#$#"
                rsIN.Sort = "SN"
                rsIN.MoveFirst()
                Do Until rsIN.EOF
                    sSN = rsIN("SN").Value

                    If Not IsExistKey(colSNDuplic, sSN) Then

                        If sSN <> sOldSN Then
                            If CS IsNot Nothing Then ' uzavri predchadzajuci
                                CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu
                                If Not IsExistKey(colSNKeys, sOldSN) Then
                                    colSNKeys.Add(sOldSN, sOldSN)
                                    colSNObjectShapes.Add(CS, sOldSN)
                                End If
                            End If
                            If ExistFieldInRs(rsIN, "D_CENA") Then decCena = rsIN("D_CENA").Value Else decCena = 0
                            CS = New ValecSourceObj(sSN, rsIN("SARZA").Value, rsIN("LOCATION").Value, decCena, colSNs(CStr(sSN)), Nothing) 'Nothing - aby nekrical
                        End If
                        If CInt(rsIN("PV").Value) = 1 Then ' + 
                            CS.CoordToAdd(New CoordinatesValec(rsIN("D1").Value, rsIN("D2").Value, rsIN("L1").Value, rsIN("L2").Value))
                        Else ' 2 -
                            CS.CoordToRemove(New CoordinatesValec(rsIN("D1").Value, rsIN("D2").Value, rsIN("L1").Value, rsIN("L2").Value))
                        End If
                        sOldSN = sSN
                    End If

                    rsIN.MoveNext()
                Loop
                ' uzavri predchadzajuci
                CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu
                If Not IsExistKey(colSNKeys, sSN) Then
                    colSNKeys.Add(sSN, sSN)
                    colSNObjectShapes.Add(CS, sSN)
                End If
            Else
                Exit Function
            End If
        End If

        If bOnlyReturnObjem Then
            If CS Is Nothing Then
                Return 0
            Else
                Return GetValecObjem(CS.coordList)
            End If
        Else
            Dim ret As DialogResult = Me.ShowDialog()
            If ret = DialogResult.OK Then
                rsOUT = grd.FillRS
                D1 = rbn_aD.Value
                d2 = rbn_bd.Value
                L = rbn_L.Value
            Else
                rsOUT = Nothing
            End If
            Return 0
        End If
    End Function

    Public Function RunBlok(ByVal sCaption As String, ByVal bOnlyShow As Boolean,
                          ByVal strMjObjem As String, ByVal strMjRozmery As String,
                          ByVal intPocetMiestObjem As Integer, ByVal intKoefMjObjemRozmery As Integer,
                          Optional ByVal PocetKusov As Integer = 0,
                          Optional ByRef a As Integer = 0, Optional ByRef b As Integer = 0, Optional ByRef L As Integer = 0,
                          Optional ByVal Input3DData As String = "", Optional ByRef Output3DData As String = "",
                          Optional ByVal bOnlyReturnObjem As Boolean = False, Optional ByVal bOrientedL As Boolean = False) As Decimal

        sMjObjem = strMjObjem
        sMjRozmery = strMjRozmery
        iPocetMiestObjem = intPocetMiestObjem
        iKoefMjObjemRozmery = intKoefMjObjemRozmery

        Dim CS As BlokSourceObj = Nothing
        Dim bSim As Boolean = (a <> 0 And b <> 0 And L <> 0)

        _D3dOp = IIf(bOnlyShow, D3dOperation.D3dOperation_ShowBlok, D3dOperation.D3dOperation_SimBlok)
        setRbnLabels(sCaption, bSim, a, b, L, PocetKusov)

        colSNKeys = New Collection
        colSNObjectShapes = New Collection
        '*********************************
        If String.IsNullOrEmpty(Input3DData) Then
            Exit Function
        Else

            Dim fetch As D3DSource()
            Dim f As D3DSource
            Dim bl As Blok

            fetch = JsonConvert.DeserializeObject(Of D3DSource())(Input3DData)
            Dim sorted = fetch.OrderBy(Function(x) x.SN)

            For Each f In sorted
                If f.Type.ToLower() = "blok" Then

                    Dim sortedblok = f.Blok.
                        OrderBy(Function(x) x.a1).ThenBy(Function(x) x.a2).
                        ThenBy(Function(x) x.L1).ThenBy(Function(x) x.L2).
                        ThenBy(Function(x) x.b1).ThenBy(Function(x) x.b2)

                    CS = New BlokSourceObj(f.SN, f.Sarza, f.Location, f.SklCena, bOrientedL, f.SN, sortedblok)

                    'Ak uû takÈ rozmey existuj˙, tak ich neprid·m, iba rozöÌrim SN
                    Dim sRef = CS.CoordToString()
                    Dim s As String
                    Dim bEx As Boolean = False

                    For Each s In colSNKeys
                        Dim v As BlokSourceObj = colSNObjectShapes(s)
                        If sRef = v.CoordToString() Then
                            bEx = True
                            v.SNs = v.SNs & "; " & f.SN
                            v.ColSNs.Add(f.SN, f.SN)
                        End If
                    Next

                    If Not bEx Then
                        For Each bl In sortedblok
                            If bl.Add Then
                                CS.CoordToAdd(New CoordinatesBlok(bl.a1, bl.a2, bl.L1, bl.L2, bl.b1, bl.b2))
                            Else
                                CS.CoordToRemove(New CoordinatesBlok(bl.a1, bl.a2, bl.L1, bl.L2, bl.b1, bl.b2))
                            End If
                        Next
                        CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu

                        If Not IsExistKey(colSNKeys, f.SN) Then
                            colSNKeys.Add(f.SN, f.SN)
                            colSNObjectShapes.Add(CS, f.SN)
                        End If

                    End If

                End If
            Next
        End If

        If bOnlyReturnObjem Then
            If colSNObjectShapes.Count = 0 Then
                Return 0
            Else
                'Return GetValecObjem(CS.coordList)
                Return GetBlokObjem(colSNObjectShapes.Item(colSNObjectShapes.Count).coordList)
            End If
        Else
            Dim ret As DialogResult = Me.ShowDialog()
            If ret = DialogResult.OK Then
                Output3DData = grd.GetOutput3DData
                a = rbn_aD.Value
                b = rbn_bd.Value
                L = rbn_L.Value
            Else
                Output3DData = ""
            End If
            Return 0
        End If

    End Function

    Private Function RunBlokOLD(ByVal sCaption As String, ByVal bOnlyShow As Boolean,
                          ByVal strMjObjem As String, ByVal strMjRozmery As String,
                          ByVal intPocetMiestObjem As Integer, ByVal intKoefMjObjemRozmery As Integer,
                          Optional ByVal PocetKusov As Integer = 0,
                          Optional ByRef a As Integer = 0, Optional ByRef b As Integer = 0, Optional ByRef L As Integer = 0,
                          Optional ByVal rsIN As ADODB.Recordset = Nothing, Optional ByRef rsOUT As ADODB.Recordset = Nothing,
                          Optional ByVal bOnlyReturnObjem As Boolean = False, Optional ByVal bOrientedL As Boolean = False) As Decimal

        sMjObjem = strMjObjem
        sMjRozmery = strMjRozmery
        iPocetMiestObjem = intPocetMiestObjem
        iKoefMjObjemRozmery = intKoefMjObjemRozmery

        Dim sSN As String = "", sOldSN As String = "$#$#$#"
        Dim CS As BlokSourceObj = Nothing
        Dim bSim As Boolean = (a <> 0 And b <> 0 And L <> 0)
        Dim decCena As Decimal
        Dim colSNDuplic As Collection
        Dim colSNRef As Collection
        Dim colSNs As Collection
        Dim sRef As String = ""
        Dim sOldRef As String = ""
        Dim sBefore As String
        Dim sKey As String = ""

        _D3dOp = IIf(bOnlyShow, D3dOperation.D3dOperation_ShowBlok, D3dOperation.D3dOperation_SimBlok)
        setRbnLabels(sCaption, bSim, a, b, L, PocetKusov)

        colSNKeys = New Collection
        colSNObjectShapes = New Collection
        '*********************************
        If rsIN Is Nothing Then
            Exit Function
        Else

            If rsIN.RecordCount > 0 Then ' z VB6 poslem len pokial je RecordCount > 0 

                colSNDuplic = New Collection
                colSNRef = New Collection
                colSNs = New Collection
                rsIN.Sort = "SN, A1, A2, L1, L2, B1, B2"
                rsIN.MoveFirst()
                sRef = ""
                sOldRef = ""
                sOldSN = "$#$#$#"
                Do Until rsIN.EOF
                    sSN = rsIN("SN").Value
                    If sSN <> sOldSN And sOldSN <> "$#$#$#" Then
                        If IsExistKey(colSNRef, sRef) Then
                            'Duplicitne
                            sKey = colSNRef(sRef)
                            colSNDuplic.Add(sKey, sOldSN)
                            sBefore = colSNs(sKey)
                            colSNs.Remove(sKey)
                            colSNs.Add(sBefore & "; " & sOldSN, sKey)
                        Else
                            colSNRef.Add(sOldSN, sRef)
                            colSNs.Add(sOldSN, sOldSN)
                        End If
                        sOldRef = sRef
                        sRef = ""
                    End If
                    sRef = sRef & CStr(rsIN("A1").Value) & "_" &
                        CStr(rsIN("A2").Value) & "_" &
                        CStr(rsIN("L1").Value) & "_" &
                        CStr(rsIN("L2").Value) & "_" &
                        CStr(rsIN("B1").Value) & "_" &
                        CStr(rsIN("B2").Value)
                    rsIN.MoveNext()
                    sOldSN = sSN
                Loop

                If IsExistKey(colSNRef, sRef) Then
                    'Duplicitne
                    sKey = colSNRef(sRef)
                    colSNDuplic.Add(sKey, sOldSN)
                    sBefore = colSNs(sKey)
                    colSNs.Remove(sKey)
                    colSNs.Add(sBefore & "; " & sOldSN, sKey)
                Else
                    colSNRef.Add(sOldSN, sRef)
                    colSNs.Add(sOldSN, sOldSN)
                End If

                sOldSN = "$#$#$#"
                rsIN.Sort = "SN"
                rsIN.MoveFirst()
                Do Until rsIN.EOF
                    sSN = rsIN("SN").Value

                    If Not IsExistKey(colSNDuplic, sSN) Then
                        If sSN <> sOldSN Then
                            If CS IsNot Nothing Then ' uzavri predchadzajuci
                                CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu
                                If Not IsExistKey(colSNKeys, sOldSN) Then
                                    colSNKeys.Add(sOldSN, sOldSN)
                                    colSNObjectShapes.Add(CS, sOldSN)
                                End If
                            End If
                            If ExistFieldInRs(rsIN, "D_CENA") Then decCena = rsIN("D_CENA").Value Else decCena = 0
                            CS = New BlokSourceObj(sSN, rsIN("SARZA").Value, rsIN("LOCATION").Value, decCena, bOrientedL, colSNs(sSN), Nothing) 'Nothing - aby nekrical
                        End If
                        If CInt(rsIN("PV").Value) = 1 Then ' + 
                            CS.CoordToAdd(New CoordinatesBlok(rsIN("A1").Value, rsIN("A2").Value, rsIN("L1").Value, rsIN("L2").Value, rsIN("B1").Value, rsIN("B2").Value))
                        Else ' 2 -
                            CS.CoordToRemove(New CoordinatesBlok(rsIN("A1").Value, rsIN("A2").Value, rsIN("L1").Value, rsIN("L2").Value, rsIN("B1").Value, rsIN("B2").Value))
                        End If
                        'DebugLog("(""" & sSN & """, " & IIf(rsIN("PV").Value = 1, "True", "False") & ", " & rsIN("A1").Value & ", " & rsIN("A2").Value & ", " & rsIN("L1").Value & ", " & rsIN("L2").Value & ", " & rsIN("B1").Value & ", " & rsIN("B2").Value & ")")
                        sOldSN = sSN
                    End If

                    rsIN.MoveNext()
                Loop
                ' uzavri predchadzajuci
                CS.Process() 'Vytvori kolekciu Inv a vypocita delenie zakladneho objektu
                If Not IsExistKey(colSNKeys, sSN) Then
                    colSNKeys.Add(sSN, sSN)
                    colSNObjectShapes.Add(CS, sSN)
                End If
            Else
                Exit Function
            End If
        End If

        If bOnlyReturnObjem Then
            If CS Is Nothing Then
                Return 0
            Else
                Return GetBlokObjem(CS.coordList)
            End If
        Else
            Dim ret As DialogResult = Me.ShowDialog()
            If ret = DialogResult.OK Then
                rsOUT = grd.FillRS
                a = rbn_aD.Value
                b = rbn_bd.Value
                L = rbn_L.Value
            Else
                rsOUT = Nothing
            End If
            Return 0
        End If

    End Function

    Private Sub simulateBlok()
        Dim bms As MultiSimulationBlok
        Dim iCount As Integer
        Dim colMs As New Collection
        Dim i As Integer, h As Integer
        Dim DesiredBlok As New Coord_bod(rbn_aD.Value, rbn_L.Value, rbn_bd.Value)
        Dim bRes As Boolean

        If DesiredBlok.aD <= 0 Then
            MsgBox("Rozmer 'a' musÌ byù nenulovÈ ËÌslo!", vbExclamation, "Kontrola")
            Exit Sub
        End If

        If DesiredBlok.bd <= 0 Then
            MsgBox("Rozmer 'b' musÌ byù nenulovÈ ËÌslo!", vbExclamation, "Kontrola")
            Exit Sub
        End If

        If DesiredBlok.L <= 0 Then
            MsgBox("Rozmer 'L' musÌ byù nenulovÈ ËÌslo!", vbExclamation, "Kontrola")
            Exit Sub
        End If

        Clean()

        Dim iMax As Integer  'NEW

        iMax = colSNKeys.Count
        PGBInitialize(iMax, "1/5 - v˝poËet")

        For i = 1 To iMax
            PGB.Value += 1
            bms = New MultiSimulationBlok
            If bms.GraphicsCompute(Me.SS_Size, colSNObjectShapes(i), rbn_poc_ks.Value, DesiredBlok, False, rbn_max_rez.Value, rbn_threshold.Value) Then
                If bms.CountMoznosti > 0 Then
                    AddHandler bms.SelectedSimulation, AddressOf ms_SelectedSimulation
                    AddHandler bms.StartNewTocenie, AddressOf ms_StartNewTocenie
                    AddHandler bms.Resized, AddressOf ms_Resized
                    iCount += bms.CountMoznosti
                    bms.Key = CStr(i)
                    colMs.Add(bms, bms.Key)
                End If
            End If
        Next

        'Debug.Print("trvalo to:" & DateDiff(DateInterval.Second, dBefore, DateTime.Now))
        'dBefore = DateTime.Now

        lboxSN.Items.Clear()
        If iCount > 0 Then

            '****************************** 
            '22.1.2012  Optimalizacia vykreslenia
            'Zotriedit Computations aby sa vobec negeneroval SS pre tie co sa nebudu zobrazovat
            PGBInitialize(2, "2/5 - zoradenie")
            PGB.Value = 1

            Dim colAllCmps As New Collection
            Dim j As Integer
            For i = 1 To colMs.Count
                bms = colMs(i)
                For j = 1 To bms.colComputations.Count
                    colAllCmps.Add(bms.colComputations(j))
                Next
            Next

            If colAllCmps.Count > CInt(rbn_MaxSim.Value * 1.3) Then  'Pridam este 1,3 koeficient - aby bolo dost simulacii na dobeh
                SortComputations(colAllCmps) ' Zotried Computations v MSs
                For j = CInt(rbn_MaxSim.Value * 1.3) + 1 To colAllCmps.Count
                    DirectCast(colAllCmps(j), ComputationBlok).bDoNotDraw = True
                Next
            End If
            '*********************************
            'MsgBox("Nenacita sa " & colAllCmps.Count - (rbn_MaxSim.Value + 20) & "  ss. Vsetkych je " & colAllCmps.Count)

            iMax = colMs.Count
            PGBInitialize(iMax, "3/5 - zluËovanie")

            h = GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "MS_Height", "-1")
            For i = 1 To iMax
                PGB.Value += 1
                bms = colMs(i)
                bms.GraphicsCombine(False)
                SortSingleSimulations(bms.colSSimulations) ' Zotried SS v MS
            Next

            PGBInitialize(2, "4/5 - zoradenie")
            PGB.Value = 2
            SortToMaxSim(colMs) ' Zotried vsetky SS zo vsetkych MS na prvych MaxSim a potom aj samotne MS

            iMax = colMs.Count
            PGBInitialize(iMax, "5/5 - zobrazenie")

            ' Zobraz
            For i = 1 To colMs.Count
                PGB.Value += 1
                bms = colMs(i)
                bms.AA = rbn_AA.Checked
                bms.SVP = rbn_SVP.Checked
                bms.rotationSpeed = 9000 - TBCitlivost.Value * 600
                If h <> -1 Then bms.Height = h
                bms.Width = FLP.Width - 25
                bRes = bms.GraphicsDraw()
                FLP.Controls.Add(bms)
                lboxSN.Items.Add(New ValueDescriptionPair(bms.SourceObj.SizeDescription & " - " & bms.SourceObj.SNs, bms))
                bms.iListItemID = lboxSN.Items.Count - 1
                If Not bRes Then Exit For
            Next

        Else
            MsgBox("Nenaöli sa ûiadne moûnosti", MsgBoxStyle.Information)
        End If
        PGBFinish()

        'Debug.Print("Nakonie to trvalo:" & DateDiff(DateInterval.Second, dBefore, DateTime.Now))
        'dBefore = DateTime.Now

        FLP_Resize(Nothing, Nothing)
    End Sub

    Private Sub simulateValec()
        Dim vms As MultiSimulationValec
        Dim iCount As Integer
        Dim colMs As New Collection
        Dim i As Integer
        Dim DesiredValec As New Coord_bod(rbn_aD.Value, rbn_L.Value, rbn_bd.Value)
        Dim bRes As Boolean
        Dim colEx As New Collection

        If DesiredValec.aD - DesiredValec.bd <= 0 Then
            MsgBox("Rozdiel D-d musÌ byù kladnÈ ËÌslo!", vbExclamation, "Kontrola")
            Exit Sub
        End If

        If DesiredValec.L <= 0 Then
            MsgBox("DÂûka musÌ byù nenulov· ËÌslo!", vbExclamation, "Kontrola")
            Exit Sub
        End If

        Clean()
        PGBInitialize(colSNKeys.Count * 3 + 3, "1/4 - v˝poËet")

        For i = 1 To colSNKeys.Count
            PGB.Value += 1
            'Sprava
            vms = New MultiSimulationValec
            If vms.GraphicsCompute(Me.SS_Size, colSNObjectShapes(i), rbn_poc_ks.Value, DesiredValec, ValecType.ValecType_Right) Then
                If vms.CountMoznosti > 0 Then
                    AddHandler vms.SelectedSimulation, AddressOf ms_SelectedSimulation
                    AddHandler vms.StartNewTocenie, AddressOf ms_StartNewTocenie
                    iCount += vms.CountMoznosti
                    vms.Key = CStr(i) + "_RIGHT"
                    colMs.Add(vms, vms.Key)
                End If
            End If

            PGB.Value += 1
            If vms.SourceObj.coordList.Count > 1 Then

                'Zlava
                vms = New MultiSimulationValec
                If vms.GraphicsCompute(Me.SS_Size, colSNObjectShapes(i), rbn_poc_ks.Value, DesiredValec, ValecType.ValecType_Left) Then
                    If vms.CountMoznosti > 0 Then
                        AddHandler vms.SelectedSimulation, AddressOf ms_SelectedSimulation
                        AddHandler vms.StartNewTocenie, AddressOf ms_StartNewTocenie
                        iCount += vms.CountMoznosti
                        vms.Key = CStr(i) + "_LEFT"
                        colMs.Add(vms, vms.Key)
                    End If
                End If

            End If

            PGB.Value += 1
            'Zvnutra
            vms = New MultiSimulationValec
            If vms.GraphicsCompute(Me.SS_Size, colSNObjectShapes(i), rbn_poc_ks.Value, DesiredValec, ValecType.ValecType_Inner) Then
                If vms.CountMoznosti > 0 Then
                    AddHandler vms.SelectedSimulation, AddressOf ms_SelectedSimulation
                    AddHandler vms.StartNewTocenie, AddressOf ms_StartNewTocenie
                    iCount += vms.CountMoznosti
                    vms.Key = CStr(i) + "_INNER"
                    colMs.Add(vms, vms.Key)
                End If
            End If

        Next

        lboxSN.Items.Clear()
        If iCount > 0 Then

            PGBInitialize(colMs.Count * 3 + 3, "2/4 - zluËovanie")
            For i = 1 To colMs.Count
                PGB.Value += 1
                vms = colMs(i)
                vms.GraphicsCombine()
            Next

            PGBInitialize(2, "3/4 - zoradenie")
            PGB.Value = 1
            SortToMaxSim(colMs) ' Zotried vsetky SS zo vsetkych MS na prvych MaxSim a potom aj samotne MS

            PGBInitialize(colMs.Count, "3/4 - vykreslenie")
            ' Zobraz
            For i = 1 To colMs.Count
                PGB.Value += 1
                vms = colMs(i)
                vms.AA = rbn_AA.Checked
                vms.SVP = rbn_SVP.Checked
                vms.rotationSpeed = 9000 - TBCitlivost.Value * 600
                bRes = vms.GraphicsDraw()
                FLP.Controls.Add(vms)
                If Not colEx.Contains(vms.SourceObj.SN) Then  'Robim to cez kolekciu, kedze mozu byt dve simulacie s rovnakym SN

                    If vms.SSV.cmpCurrent.iValecType = ValecType.ValecType_Left Then
                        lboxSN.Items.Add(New ValueDescriptionPair(vms.SourceObj.SizeDescription & " (L) " & vms.SourceObj.SNs, vms))
                    ElseIf vms.SSV.cmpCurrent.iValecType = ValecType.ValecType_Right Then
                        lboxSN.Items.Add(New ValueDescriptionPair(vms.SourceObj.SizeDescription & " (R) " & vms.SourceObj.SNs, vms))
                    ElseIf vms.SSV.cmpCurrent.iValecType = ValecType.ValecType_Inner Then
                        lboxSN.Items.Add(New ValueDescriptionPair(vms.SourceObj.SizeDescription & " (I) " & vms.SourceObj.SNs, vms))
                    End If

                    vms.iListItemID = lboxSN.Items.Count - 1
                End If
                If Not bRes Then Exit For
            Next
        Else
            MsgBox("Nenaöli sa ûiadne moûnosti", MsgBoxStyle.Information)
        End If
        PGBFinish()

        FLP_Resize(Nothing, Nothing)

    End Sub

    Private Sub SortToMaxSim(ByRef colMs As Collection) ' Sortne vsetky SS z MS a oznaci ich kt sa maju zobrazit
        Dim colAllSS As New Collection
        Dim msb As MultiSimulationBlok
        Dim msv As MultiSimulationValec

        ' Urob kolekciu so vsetkymi SS
        For Each o As Object In colMs
            If TypeOf o Is MultiSimulationBlok Then
                msb = DirectCast(o, MultiSimulationBlok)
                For Each ss As SingleSimulationBlok In msb.colSSimulations
                    colAllSS.Add(ss) ' SingleSimBlok
                Next
            ElseIf TypeOf o Is MultiSimulationValec Then
                msv = DirectCast(o, MultiSimulationValec)
                colAllSS.Add(msv.SSV) ' SingleSimValec
            End If
        Next
        If colAllSS.Count > 0 Then
            ' Zoradime vsetky SS
            SortSingleSimulations(colAllSS)
            ' Zmazeme vsetko za MaxSim (nehavame len posledne alternativy od MaxSim)
            Dim MaxSim As Integer = rbn_MaxSim.Value
            Dim Zvysok1 As Decimal, Zvysok2 As Decimal
            Dim Qty1 As Integer, Qty2 As Integer
            Dim OuterSizeValue1 As Integer, OuterSizeValue2 As Integer
            Dim sTmp As String = ""

            If colAllSS.Count > MaxSim Then
                GetSortDataFromSS(colAllSS(MaxSim), Zvysok1, Qty1, sTmp, OuterSizeValue1)  ' init udajov poslednej alternativy

                Dim M As Integer = colAllSS.Count
                Dim N As Integer = MaxSim + 1
                Dim i As Integer
                For i = M To N Step -1 ' odzadu musime
                    GetSortDataFromSS(colAllSS(i), Zvysok2, Qty2, sTmp, OuterSizeValue2)
                    If Not (Zvysok2 = Zvysok1 And Qty2 = Qty1 And OuterSizeValue1 = OuterSizeValue2) Then colAllSS.Remove(i) ' rovnake alternativy nehame
                Next
            End If
            ' Oznacime vsetky tie co chceme zobrazit + zotriedime MS
            Dim ssb As SingleSimulationBlok
            Dim ssv As SingleSimulationValec
            Dim colSortedMS As New Collection
            For Each o As Object In colAllSS
                If TypeOf o Is SingleSimulationBlok Then
                    ssb = DirectCast(o, SingleSimulationBlok)
                    ssb.ShowSimulation = True
                    msb = DirectCast(colMs(ssb.MSKey), MultiSimulationBlok)
                    If Not colSortedMS.Contains(msb.Key) Then colSortedMS.Add(msb, msb.Key) ' MS collection zoradena podla najlepsich vysledkov (zle sa sem nedostanu)
                ElseIf TypeOf o Is SingleSimulationValec Then
                    ssv = DirectCast(o, SingleSimulationValec)
                    ssv.ShowSimulation = True
                    msv = DirectCast(colMs(ssv.MSKey), MultiSimulationValec)
                    If Not colSortedMS.Contains(msv.Key) Then colSortedMS.Add(msv, msv.Key)
                End If
            Next
            If colSortedMS.Count > 0 Then colMs = colSortedMS
        End If
    End Sub

    Private Sub showBlok()
        Dim bms As MultiSimulationBlok
        Dim colMs As New Collection
        Dim i As Integer
        Dim DesiredBlok As New Coord_bod(1, 1, 1)

        Clean()

        For i = 1 To colSNKeys.Count
            bms = New MultiSimulationBlok
            If bms.GraphicsCompute(Me.SS_Size, colSNObjectShapes(i), 0, DesiredBlok, False, rbn_max_rez.Value, rbn_threshold.Value) Then
                colMs.Add(bms, CStr(i))
                bms.AA = rbn_AA.Checked
                bms.SVP = rbn_SVP.Checked
                bms.rotationSpeed = 9000 - TBCitlivost.Value * 600
                FLP.Controls.Add(bms)
                bms.RawGraphicsDraw()

            End If
        Next
        FLP_Resize(Nothing, Nothing)
    End Sub

    Private Sub showValec()
        ' Este nedokoncene
        Clean()
        FLP_Resize(Nothing, Nothing)
    End Sub

    Private Sub PGBInitialize(ByVal iCount As Integer, ByVal sPopis As String)
        PGB.Visible = False
        sbrOperacia.Visible = False
        PGB.Value = 0
        sbrOperacia.Text = sPopis
        PGB.Maximum = iCount
        PGB.Visible = True
        sbrOperacia.Visible = True
        Me.Refresh()
    End Sub

    Private Sub PGBFinish()
        PGB.Value = PGB.Maximum
        PGB.Visible = False
        sbrOperacia.Text = ""
    End Sub

    Private Sub ms_SelectedSimulation(ByRef SingleSim As SingleSimulation, ByRef MultiSim As Object)
        grd.FillTable(Me, SingleSim, MultiSim, iPocetMiestObjem)
        Me.rbnBtnOk.Enabled = True

        For Each o As Object In FLP.Controls
            If TypeOf o Is MultiSimulationBlok Then
                Dim ms As MultiSimulationBlok
                ms = DirectCast(o, MultiSimulationBlok)
                For Each ss As SingleSimulationBlok In ms.FLP.Controls
                    ss.HighLightSelected(ss Is SingleSim)
                Next
            ElseIf TypeOf o Is MultiSimulationValec Then
                Dim ms As MultiSimulationValec
                ms = DirectCast(o, MultiSimulationValec)
                ms.SSV.HighLightSelected(ms.SSV Is SingleSim)
            End If
        Next

    End Sub

    Private Sub ms_StartNewTocenie(ByRef SingleSim As SingleSimulation)
        For Each o As Object In FLP.Controls
            If TypeOf o Is MultiSimulationBlok Then
                Dim ms As MultiSimulationBlok
                ms = DirectCast(o, MultiSimulationBlok)
                For Each ss As SingleSimulationBlok In ms.FLP.Controls
                    If ss.bTociSa AndAlso (ss IsNot SingleSim) Then ss.StartStopTocenie(False)
                Next
            ElseIf TypeOf o Is MultiSimulationValec Then
                Dim ms As MultiSimulationValec
                ms = DirectCast(o, MultiSimulationValec)
                If ms.SSV.bTociSa AndAlso (ms.SSV IsNot SingleSim) Then ms.SSV.StartStopTocenie(False)
            End If
        Next
    End Sub

    Private Sub TB_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBSize.Scroll
        RedrawMS()
    End Sub

    Private Sub RedrawMS()
        For Each o As Object In FLP.Controls
            If TypeOf o Is MultiSimulationBlok Then
                DirectCast(o, MultiSimulationBlok).RedrawSS(Me.SS_Size, rbn_AA.Checked, rbn_SVP.Checked)
            ElseIf TypeOf o Is MultiSimulationValec Then
                DirectCast(o, MultiSimulationValec).RedrawSS(Me.SS_Size, rbn_AA.Checked, rbn_SVP.Checked)
            End If
        Next
    End Sub

    Private Sub Run()
        Select Case _D3dOp
            Case D3dOperation.D3dOperation_ShowBlok
                showBlok()
            Case D3dOperation.D3dOperation_ShowValec
                showValec()
            Case D3dOperation.D3dOperation_SimBlok
                simulateBlok()
            Case D3dOperation.D3dOperation_SimValec
                simulateValec()
        End Select

    End Sub

    Private Sub Clean()
        Dim msb As MultiSimulationBlok
        Dim msv As MultiSimulationValec
        Dim o As Object

        For i As Integer = FLP.Controls.Count - 1 To 0 Step -1
            o = FLP.Controls(i)

            If TypeOf o Is MultiSimulationBlok Then
                msb = DirectCast(o, MultiSimulationBlok)
                msb.RemoveAllSSPaintEvents()    ' Odstranime paint evanty aby sa to zbytocne neprekreslovalo pri vypinani
                msb.Dispose()

            ElseIf TypeOf o Is MultiSimulationValec Then
                msv = DirectCast(o, MultiSimulationValec)
                msv.SSV.RemovePaintEvent()    ' Odstranime paint evanty aby sa to zbytocne neprekreslovalo pri vypinani
                msv.Dispose()

            End If
        Next

    End Sub

    Private Sub FLP_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles FLP.Resize
        For Each o As Object In FLP.Controls
            If TypeOf o Is MultiSimulationBlok Then
                resizeBMS(o)
            ElseIf TypeOf o Is MultiSimulationValec Then
                resizeVMS(o)
            End If
        Next
    End Sub

    Private Sub resizeBMS(ByVal ms As MultiSimulationBlok)
        ms.Width = FLP.Width - IIf(FLP.VerticalScroll.Visible, 25, 10)
        If FLP.Controls.Count = 1 Then
            ms.Height = FLP.Height - IIf(FLP.HorizontalScroll.Visible, 25, 10)
        End If
    End Sub

    Private Sub resizeVMS(ByVal ms As MultiSimulationValec)
        'If FLP.Controls.Count = 1 Then
        '  ms.Width = FLP.Width - IIf(FLP.VerticalScroll.Visible, 25, 10)
        '  ms.Height = FLP.Height - IIf(FLP.HorizontalScroll.Visible, 25, 10)
        'End If
    End Sub

    Private Sub TBCitlivost_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TBCitlivost.ValueChanged
        For Each o As Object In FLP.Controls
            If TypeOf o Is MultiSimulationBlok Then
                DirectCast(o, MultiSimulationBlok).changeSpeed(9000 - TBCitlivost.Value * 600)
            ElseIf TypeOf o Is MultiSimulationValec Then
                DirectCast(o, MultiSimulationValec).changeSpeed(9000 - TBCitlivost.Value * 600)
            End If
        Next
    End Sub

    Friend ReadOnly Property SS_Size() As Integer
        Get
            Return 150 + TBSize.Value * 50
        End Get
    End Property

    Private Sub ms_Resized(ByRef MultiSimBlok As MultiSimulationBlok)
        If MultiSimBlok.iListItemID = 0 And FLP.Controls.Count > 1 Then ' Zaujima nas len zmena velkosti prvej
            If GetSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "MS_Height", "-1") = MultiSimBlok.Height Then Exit Sub
            SaveSetting(HKEY_MEMPHIS_NAME, HKEY_SECTION_NAME_TYP, "MS_Height", MultiSimBlok.Height)
        End If

    End Sub

End Class

' Riesi Item data pre Combobox, lstBox
Friend Class ValueDescriptionPair
    Friend ValueObj As Object
    Friend ValueText As String

    Friend Sub New(ByVal NewText As String, ByVal NewObj As Object)
        ValueText = NewText
        ValueObj = NewObj
    End Sub
    Public Overrides Function ToString() As String
        Return ValueText
    End Function

End Class
