Imports MemControls.MemJG
Imports MemControlsSharedLibrary
Imports Janus.Windows
Imports Newtonsoft.Json

Public Class clsGrid
    Private dsSim As DataSet
    Private _sTableName As String = "SIMDATA"
    Private MJG As MemControls.MemJG
    Private _bIsBlokSim As Boolean

    Friend Sub FillTable(ByRef frm As FormD3dGraphic, ByRef SingleSim As SingleSimulation, ByRef MultiSim As Object, ByVal iPocetMiestObjem As Integer)
        Dim dRow As DataRow, dTab As DataTable, i As Integer
        Dim decObjemVyrezu As Decimal
        Dim decObjemOdrezku As Decimal
        Dim decObjemPolotovaru As Decimal
        Dim sOuterSizeFinal As String = ""
        Dim iPocKs As Integer
        Dim coB As New CoordinatesBlok()
        Dim coV As New CoordinatesValec()
        Dim msBlok As MultiSimulationBlok = Nothing
        Dim msVal As MultiSimulationValec = Nothing

        dTab = dsSim.Tables(_sTableName)
        dTab.Clear()

        Dim Rez As ArrayList
        If _bIsBlokSim Then
            Dim ssBlok As SingleSimulationBlok
            ssBlok = CType(SingleSim, SingleSimulationBlok)
            msBlok = CType(MultiSim, MultiSimulationBlok)
            Rez = ssBlok.cmpCurrent.CelyRez
            decObjemVyrezu = ssBlok.cmpCurrent.ObjemVyrezu
            decObjemOdrezku = ssBlok.cmpCurrent.ObjemOdrezku
            decObjemPolotovaru = ssBlok.cmpCurrent.ObjemPolotovaru
            iPocKs = ssBlok.cmpCurrent.Pocet
            sOuterSizeFinal = ssBlok.cmpCurrent.OuterSizeFinal
        Else
            Dim ssVal As SingleSimulationValec
            ssVal = CType(SingleSim, SingleSimulationValec)
            msVal = CType(MultiSim, MultiSimulationValec)
            Rez = MyArrayClone(ssVal.cmpCurrent.CelyRez)
            OptimiseValec(Rez)
            decObjemVyrezu = ssVal.cmpCurrent.ObjemVyrezu
            decObjemOdrezku = ssVal.cmpCurrent.ObjemOdrezku
            decObjemPolotovaru = ssVal.cmpCurrent.ObjemPolotovaru
            iPocKs = ssVal.cmpCurrent.Pocet
            sOuterSizeFinal = ssVal.cmpCurrent.OuterSizeFinal
        End If

        frm.txtPolotovar.Text = RoundItNice(decObjemPolotovaru) & " " & sMjObjem
        frm.txtOdrezok.Text = RoundItNice(decObjemOdrezku) & " " & sMjObjem
        frm.txtVyrez.Text = RoundItNice(decObjemVyrezu) & " " & sMjObjem

        frm.labPocKs.Text = iPocKs

        If Rez Is Nothing Then Exit Sub

        For i = 0 To Rez.Count - 1
            dRow = dTab.NewRow()
            dRow.Item("ID") = i
            dRow.Item("OBJEM_VYREZ") = Math.Round(decObjemVyrezu, iPocetMiestObjem)
            dRow.Item("OBJEM_ZVYSOK") = Math.Round(decObjemOdrezku, iPocetMiestObjem)
            dRow.Item("OBJEM_PLT") = Math.Round(decObjemPolotovaru, iPocetMiestObjem)

            dRow.Item("POC_KS") = iPocKs

            If _bIsBlokSim Then
                coB = Rez.Item(i)
                dRow.Item("SN") = msBlok.SelectedSN 'msBlok.SourceObj.SN
                dRow.Item("SARZA") = msBlok.SourceObj.Sarza
                dRow.Item("LOCATION") = msBlok.SourceObj.Location
                dRow.Item("OUTER_SIZE") = msBlok.SourceObj.SizeDescription
                dRow.Item("OUTER_SIZE_FINAL") = sOuterSizeFinal

                dRow.Item("D_CENA") = msBlok.SourceObj.SklCena

                dRow.Item("A1") = coB.a1
                dRow.Item("A2") = coB.a2
                dRow.Item("B1") = coB.b1
                dRow.Item("B2") = coB.b2
                dRow.Item("L1") = coB.L1
                dRow.Item("L2") = coB.L2
            Else
                coV = Rez.Item(i)
                dRow.Item("SN") = msVal.SelectedSN 'msVal.SourceObj.SN
                dRow.Item("SARZA") = msVal.SourceObj.Sarza
                dRow.Item("LOCATION") = msVal.SourceObj.Location
                dRow.Item("OUTER_SIZE") = msVal.SourceObj.SizeDescription
                dRow.Item("OUTER_SIZE_FINAL") = sOuterSizeFinal
                dRow.Item("D_CENA") = msVal.SourceObj.SklCena

                dRow.Item("D1") = coV.D1
                dRow.Item("D2") = coV.d2
                dRow.Item("L1") = coV.L1
                dRow.Item("L2") = coV.L2
            End If
            dTab.Rows.Add(dRow)
        Next
        MJG.Rebind()
    End Sub

    Friend Sub InitControlGrid(ByRef Frm As Form, ByRef jg As MemControls.MemJG,
                             ByVal sLayout As String, ByVal sGrpColapsed As String, ByVal oType As FormD3dGraphic.D3dOperation)

        Dim sTmp As String = "", sHide As String = "_" ' Default hide
        Dim iIco As memColumnIco
        iIco = memColumnIco.memColumnImage_Katalog
        _bIsBlokSim = (oType = FormD3dGraphic.D3dOperation.D3dOperation_SimBlok Or oType = FormD3dGraphic.D3dOperation.D3dOperation_ShowBlok)
        MJG = jg
        MJG.InitializeCN("")
        MJG.InitializeJG()
        MJG.AddColumn("ID", "_ID")
        MJG.AddColumn("A1", IIf(_bIsBlokSim, "a1", "_A1") & " [" & sMjRozmery & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("A2", IIf(_bIsBlokSim, "a2", "_A2") & " [" & sMjRozmery & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("B1", IIf(_bIsBlokSim, "b1", "_B1") & " [" & sMjRozmery & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("B2", IIf(_bIsBlokSim, "b2", "_B2") & " [" & sMjRozmery & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("D1", IIf(_bIsBlokSim, "_D1", "D") & " [" & sMjRozmery & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("D2", IIf(_bIsBlokSim, "_D2", "d") & " [" & sMjRozmery & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("L1", "L1" & " [" & sMjRozmery & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("L2", "L2" & " [" & sMjRozmery & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("SN", "Výrobné číslo", GridEX.TextAlignment.Near, "", , -1)
        MJG.AddColumn("SARZA", "Šarža", GridEX.TextAlignment.Near, "", , -1)
        MJG.AddColumn("LOCATION", "Umiestnenie", GridEX.TextAlignment.Near, "", , -1)
        MJG.AddColumn("OUTER_SIZE", "Vstupné rozmery", GridEX.TextAlignment.Near, "", , -1)
        MJG.AddColumn("OUTER_SIZE_FINAL", "Výstupné rozmery", GridEX.TextAlignment.Near, "", , -1)

        MJG.AddColumn("D_CENA", "Skladová cena", GridEX.TextAlignment.Far, 0, CS_DBL_FMT2, -1)
        MJG.AddColumn("POC_KS", "_Počet kusov", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("OBJEM_VYREZ", "_Objem výrezu" & " [" & sMjObjem & "]", GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("OBJEM_ZVYSOK", "_Objem odrezku" & " [" & sMjObjem & "]", , GridEX.TextAlignment.Far, 0, , -1)
        MJG.AddColumn("OBJEM_PLT", "_Objem polotovaru" & " [" & sMjObjem & "]", , GridEX.TextAlignment.Far, 0, , -1)
        MJG.InitDS(dsSim, _sTableName, Frm, MJG.Name, True, , True, , , , sLayout, , sGrpColapsed, iIco, , , MJG.Name, _sTableName)

        MJG.Visible = True
    End Sub

    Friend Sub CreateMyTable()
        Dim table As DataTable
        table = Me.CreateTable()
        ' Naplnim
        dsSim = New DataSet()
        dsSim.Tables.Add(table)
    End Sub

    Public Shared Function CreateRs() As ADODB.Recordset ' table OUT ale vyuzijeme ho aj ako table IN z Test projektu
        Dim rs As New ADODB.Recordset
        rs.Fields.Append("SN", ADODB.DataTypeEnum.adVarChar, 100)
        rs.Fields.Append("SARZA", ADODB.DataTypeEnum.adVarChar, 100)
        rs.Fields.Append("LOCATION", ADODB.DataTypeEnum.adVarChar, 100)
        rs.Fields.Append("OUTER_SIZE", ADODB.DataTypeEnum.adVarChar, 255)
        rs.Fields.Append("OUTER_SIZE_FINAL", ADODB.DataTypeEnum.adVarChar, 255)
        rs.Fields.Append("POC_KS", ADODB.DataTypeEnum.adInteger)
        rs.Fields.Append("D_CENA", ADODB.DataTypeEnum.adDouble)

        rs.Fields.Append("OBJEM_VYREZ", ADODB.DataTypeEnum.adDecimal)
        rs.Fields.Append("OBJEM_ZVYSOK", ADODB.DataTypeEnum.adDecimal)
        rs.Fields.Append("OBJEM_PLT", ADODB.DataTypeEnum.adDecimal)

        rs.Fields("OBJEM_VYREZ").NumericScale = 4
        rs.Fields("OBJEM_ZVYSOK").NumericScale = 4
        rs.Fields("OBJEM_PLT").NumericScale = 4

        rs.Fields.Append("PV", ADODB.DataTypeEnum.adTinyInt)

        rs.Fields.Append("A1", ADODB.DataTypeEnum.adInteger)
        rs.Fields.Append("A2", ADODB.DataTypeEnum.adInteger)
        rs.Fields.Append("B1", ADODB.DataTypeEnum.adInteger)
        rs.Fields.Append("B2", ADODB.DataTypeEnum.adInteger)
        rs.Fields.Append("D1", ADODB.DataTypeEnum.adInteger)
        rs.Fields.Append("D2", ADODB.DataTypeEnum.adInteger)
        rs.Fields.Append("L1", ADODB.DataTypeEnum.adInteger)
        rs.Fields.Append("L2", ADODB.DataTypeEnum.adInteger)
        Return rs
    End Function

    Friend Function FillRS() As ADODB.Recordset
        Dim dRow As DataRow, dTab As DataTable
        Dim rs As ADODB.Recordset

        dTab = dsSim.Tables(_sTableName)
        rs = CreateRs()
        rs.Open()
        For Each dRow In dTab.Rows
            rs.AddNew()
            rs("SN").Value = dRow.Item("SN")
            rs("SARZA").Value = dRow.Item("SARZA")
            rs("POC_KS").Value = dRow.Item("POC_KS") ' Vybranych (aj menej moze byt)
            rs("D_CENA").Value = dRow.Item("D_CENA")
            rs("OBJEM_VYREZ").Value = dRow.Item("OBJEM_VYREZ")
            rs("OBJEM_ZVYSOK").Value = dRow.Item("OBJEM_ZVYSOK")
            rs("OBJEM_PLT").Value = dRow.Item("OBJEM_PLT")
            rs("LOCATION").Value = dRow.Item("LOCATION")
            rs("OUTER_SIZE").Value = dRow.Item("OUTER_SIZE")
            rs("OUTER_SIZE_FINAL").Value = dRow.Item("OUTER_SIZE_FINAL")
            rs("PV").Value = 2 ' vsetko je Minus

            rs("A1").Value = dRow.Item("A1")
            rs("A2").Value = dRow.Item("A2")
            rs("L1").Value = dRow.Item("L1")
            rs("L2").Value = dRow.Item("L2")
            rs("B1").Value = dRow.Item("B1")
            rs("B2").Value = dRow.Item("B2")
            rs("D1").Value = dRow.Item("D1")
            rs("D2").Value = dRow.Item("D2")
            rs.Update()
        Next
        Return rs
    End Function

    Friend Function GetOutput3DData() As String
        Dim dRow As DataRow
        Dim dTab As DataTable

        dTab = dsSim.Tables(_sTableName)

        If dTab.Rows.Count = 0 Then Return ""

        dRow = dTab.Rows(0)
        Dim f As New D3DSource With {
            .SN = dRow.Item("SN"),
            .Sarza = dRow.Item("SARZA"),
            .Location = dRow.Item("LOCATION"),
            .SklCena = dRow.Item("D_CENA"),
            .PocKs = dRow.Item("POC_KS"), ' Vybranych (aj menej moze byt)
            .ObjemVyrez = dRow.Item("OBJEM_VYREZ"),
            .ObjemZvysok = dRow.Item("OBJEM_ZVYSOK"),
            .ObjemPlt = dRow.Item("OBJEM_PLT"),
            .OuterSize = dRow.Item("OUTER_SIZE"),
            .OuterSizeFinal = dRow.Item("OUTER_SIZE_FINAL")
        }

        If _bIsBlokSim Then
            f.Type = "blok"
            f.Blok = New List(Of Blok)
        Else
            f.Type = "valec"
            f.Valec = New List(Of Valec)
        End If

        For Each dRow In dTab.Rows
            If _bIsBlokSim Then
                Dim itm As New Blok With {
                    .Add = False,
                    .a1 = dRow.Item("A1"),
                    .a2 = dRow.Item("A2"),
                    .L1 = dRow.Item("L1"),
                    .L2 = dRow.Item("L2"),
                    .b1 = dRow.Item("B1"),
                    .b2 = dRow.Item("B2")
                }
                f.Blok.Add(itm)
            Else
                Dim itm As New Valec With {
                    .Add = False,
                    .L1 = dRow.Item("L1"),
                    .L2 = dRow.Item("L2"),
                    .D1 = dRow.Item("D1"),
                    .d2 = dRow.Item("D2")
                }
                f.Valec.Add(itm)
            End If
        Next
        Dim s As String = JsonConvert.SerializeObject(f)
        Return s

    End Function

    Private Function CreateTable() As DataTable
    Dim table As DataTable
    Dim dcTmp As DataColumn

    table = New DataTable(_sTableName)
        dcTmp = New DataColumn("_ICON") With {
            .DataType = GetType(String),
            .DefaultValue = ""
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("ID") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0,
            .Unique = True
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("A1") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("A2") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("B1") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("B2") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("L1") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("L2") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("D1") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("D2") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("SN") With {
            .DataType = GetType(String),
            .DefaultValue = ""
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("SARZA") With {
            .DataType = GetType(String),
            .DefaultValue = ""
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("LOCATION") With {
            .DataType = GetType(String),
            .DefaultValue = ""
        }
        table.Columns.Add(dcTmp)

        dcTmp = New DataColumn("OUTER_SIZE") With {
            .DataType = GetType(String),
            .DefaultValue = ""
        }
        table.Columns.Add(dcTmp)

        dcTmp = New DataColumn("OUTER_SIZE_FINAL") With {
            .DataType = GetType(String),
            .DefaultValue = ""
        }
        table.Columns.Add(dcTmp)


        dcTmp = New DataColumn("D_CENA") With {
            .DataType = GetType(Decimal),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("OBJEM_VYREZ") With {
            .DataType = GetType(Decimal),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("OBJEM_ZVYSOK") With {
            .DataType = GetType(Decimal),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("OBJEM_PLT") With {
            .DataType = GetType(Decimal),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)
        dcTmp = New DataColumn("POC_KS") With {
            .DataType = GetType(Integer),
            .DefaultValue = 0
        }
        table.Columns.Add(dcTmp)

        Return table
    End Function

End Class
