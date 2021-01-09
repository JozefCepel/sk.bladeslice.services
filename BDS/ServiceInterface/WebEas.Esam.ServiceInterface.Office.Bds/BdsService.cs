using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Bds.Dto;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceInterface;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public partial class BdsService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BdsService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public BdsService(IBdsRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new IBdsRepository Repository
        {
            get
            {
                return (IBdsRepository)repository;
            }
        }

        #region Globalne, ale musia byt v moduloch

        public object Get(AppStatus.HealthCheckDto request)
        {
            return GetHealthCheck(request);
        }

        public object Get(ListDto request)
        {
            return base.GetList(request);
        }

        public object Get(ListComboDto request)
        {
            return this.Repository.GetListCombo(request);
        }

        public void Post(ChangeStateDto request)
        {            
            Repository.ChangeState(request);
        }


        public object Post(GetTreeCountsDto request)
        {
            return base.GetTreeCounts(request);
        }

        public object Get(DataChangesRequest request)
        {
            return Repository.GetTableLogging(request.Code, request.RowId);
        }

        #endregion

        #region Globalne, Long operations

        public object Post(LongOperationStartDto request)
        {
            return Repository.LongOperationStart(request);
        }

        public object Post(LongOperationRestartDto request)
        {
            return Repository.LongOperationRestart(request.ProcessKey);
        }

        public object Get(LongOperationProgressDto request)
        {
            return Repository.LongOperationStatus(request.ProcessKey);
        }

        public object Get(LongOperationResultDto request)
        {
            return Repository.LongOperationResult(request.ProcessKey);
        }

        public object Post(LongOperationCancelDto request)
        {
            return Repository.LongOperationCancel(request.ProcessKey, request.NotRemove);
        }

        public object Get(LongOperationListDto request)
        {
            return Repository.LongOperationList(request.PerTenant, request.Skip, request.Take);
        }

        #endregion

        #region Default row values

        public object Any(RowDefaultValues request)
        {
            return Repository.GetRowDefaultValues(request.code, request.masterCode, request.masterRowId);
        }

        #endregion

        #region D_PRI_0

        public object Any(CreateD_PRI_0 request)
        {
            return Repository.Create<V_PRI_0View>(request);
        }

        public object Any(UpdateD_PRI_0 request)
        {
            return Repository.Update<V_PRI_0View>(request);
        }

        public void Any(DeleteD_PRI_0 request)
        {
            Repository.Delete<tblD_PRI_0>(request.D_PRI_0);
        }

        #endregion

        #region D_PRI_1

        public object Any(CreateD_PRI_1 request)
        {
            return Repository.Create<V_PRI_1View>(request);
        }

        public object Any(UpdateD_PRI_1 request)
        {
            return Repository.Update<V_PRI_1View>(request);
        }

        public void Any(DeleteD_PRI_1 request)
        {
            Repository.Delete<tblD_PRI_1>(request.D_PRI_1);
        }

        public object Any(GetVybavDokladyReq request)
        {
            return Repository.GetVybavDoklady(request);
        }

        public object Any(GetOdvybavDokladyReq request)
        {
            return Repository.GetOdvybavDoklady(request);
        }

        #endregion

        #region D_SIM_0

        public object Get(GetSimData request)
        {
            V_VYD_1View vyd1 = Repository.GetList(Db.From<V_VYD_1View>().Where(e => e.D_VYD_1 == request.D_VYD_1)).First();
            var tsk = vyd1.K_TSK_0;
            var lTyp = vyd1.K_TYP_0;
            var sKod = vyd1.KOD;
            var lPocKs = vyd1.D3D_POC_KS;
            var simType = vyd1.SKL_SIMULATION;

            if (lPocKs == 0)
            {
                throw new WebEasException(null, "Number of pieces cannot be zero!");
            }
            else if (simType != 1 && simType != 2 && simType != 3)
            {
                throw new WebEasException(null, "Material group without simulation definition!");
                //case 0: return "-";
                //case 1: return "Cube";
                //case 2: return "Cube - oriented";
                //case 3: return "Cylinder";
                //default: return id + " (?)";
            }

            var sim = Repository.GetList(Db.From<V_SIM_0View>().Where(e => e.V && e.K_TSK_0 == tsk && e.KOD == sKod));



            return 1;

            /*
                   Set rsSim = New ADODB.Recordset
      rsSim.Open "SELECT SN, SARZA, LOCATION, D_CENA, PV, A1, A2, B1, B2, D1, D2, L1, L2" & vbCrLf & _
                 "FROM V_SIM_0              WHERE K_TSK_0 = " & lTSK & " AND KOD = '" & sKod & "' AND " & vbCrLf & _
                 "SN IN (SELECT SN FROM STS WHERE K_TSK_0 = " & lTSK & " AND KOD = '" & sKod & "' AND K_SKL_0 = " & lSkl & " GROUP BY SN HAVING SUM(KS) > 0)", _
                 clsMngAll.cn, adOpenForwardOnly, adLockReadOnly
      
      If Not bInitSim Then ReadJgSim
      Set rsIN = CombineRsInWithRsSim(rsSim, lID, sKod, lTSK, lTyp)
      DeactivateRs rsSim
      If rsIN.RecordCount > 0 Then
        
        Dim rsOUT As ADODB.Recordset
        ' Skupina, Kód, Názov, MJ
        If sKod <> "" Then sCapt = Trim(sKod)
        If Not IsNull(.Value(.Columns("NAZOV").Index)) Then sCapt = sCapt & ", " & Trim(.Value(.Columns("NAZOV").Index))
        If Not IsNull(.Value(.Columns("MJ").Index)) Then sCapt = sCapt & ", " & Trim(.Value(.Columns("MJ").Index))
        
        On Error Resume Next
        Set f = CreateObject("D3dGraphic.FormD3DGraphic")
        On Error GoTo 0
        
        If f Is Nothing Then
          MsgBox "Systému sa nepodarilo spustiť simulačný modul!", vbExclamation, "Kontrola"
        Else
          lL = .Value(.Columns("D3D_L").Index)
          If lIco = 1 Or lIco = 3 Then
            la = .Value(.Columns("D3D_A").Index)
            lB = .Value(.Columns("D3D_B").Index)
            
            If la = 0 Or lB = 0 Or lL = 0 Or lPocKs = 0 Then
              MsgBox "Chýbajú údaje potrebné pre simuláciu!" & vbCrLf & "Skontrolujte polia 'a', 'b', 'L', 'Počet kusov'.", vbExclamation, "Kontrola"
            Else
              f.RunBlok sCapt, False, _
                        clsMngAll.Helper.GetSystemSettingValues(memSystemSetting_SKL_3D_MJ_OBJEM), _
                        clsMngAll.Helper.GetSystemSettingValues(memSystemSetting_SKL_3D_MJ_ROZMERY), _
                        clsMngAll.Helper.GetSystemSettingValues(memSystemSetting_SKL_3D_POCET_MIEST_OBJEM), _
                        clsMngAll.Helper.GetSystemSettingValues(memSystemSetting_SKL_3D_KOEF_MJ_OBJ_ROZ), _
                        lPocKs, la, lB, lL, rsIN, rsOUT, , lIco = 3
            End If
          Else
            lD1 = .Value(.Columns("D3D_D1").Index)
            ld2 = .Value(.Columns("D3D_D2").Index)
            If lD1 = 0 Or lL = 0 Or lPocKs = 0 Then
              MsgBox "Chýbajú údaje potrebné pre simuláciu!" & vbCrLf & "Skontrolujte polia 'D', 'L', 'Počet kusov'.", vbExclamation, "Kontrola"
            Else
              f.RunValec sCapt, False, _
                         clsMngAll.Helper.GetSystemSettingValues(memSystemSetting_SKL_3D_MJ_OBJEM), _
                         clsMngAll.Helper.GetSystemSettingValues(memSystemSetting_SKL_3D_MJ_ROZMERY), _
                         clsMngAll.Helper.GetSystemSettingValues(memSystemSetting_SKL_3D_POCET_MIEST_OBJEM), _
                         clsMngAll.Helper.GetSystemSettingValues(memSystemSetting_SKL_3D_KOEF_MJ_OBJ_ROZ), _
                         lPocKs, lD1, ld2, lL, rsIN, rsOUT
            End If
            
          End If
          Set f = Nothing
        End If
        
        If Not rsOUT Is Nothing Then
          If rsOUT.RecordCount > 0 Then
            DeleteRowsInJgSim lID  ' Potvrdili sme tak zmaz stare zaznamy
            rsOUT.MoveFirst
            Set jsRow = .GetRowData(.Row) ' kopia riadku
            ' updatni udaje na riadku s kt sme to volali
            .Value(.Columns("POC_KS").Index) = CDbl(Format(rsOUT("OBJEM_VYREZ").Value, CS_FMT4)) '+ CDbl(Format(rsOut("OBJEM_ZVYSOK").Value, CS_FMT4))
            .Value(.Columns("D3D_POC_KS").Index) = rsOUT("POC_KS").Value
            .Value(.Columns("Z_CENA").Index) = CDbl(Format(rsOUT("D_CENA").Value, clsMngAll.Helper.CS_DBL_FMT_N))
            .Value(.Columns("SN").Index) = rsOUT("SN").Value
            .Value(.Columns("SARZA").Index) = rsOUT("SARZA").Value
            .Value(.Columns("LOCATION").Index) = rsOUT("LOCATION").Value
            .Value(.Columns("CUST_STR_1").Index) = rsOUT("OUTER_SIZE").Value
            .Update
            ' v pripade ze este menil rozmery
            If lIco = 1 Or lIco = 3 Then
              .Value(.Columns("D3D_A").Index) = la
              .Value(.Columns("D3D_B").Index) = lB
            Else
              .Value(.Columns("D3D_D1").Index) = lD1
              .Value(.Columns("D3D_D2").Index) = ld2
            End If
            .Value(.Columns("D3D_L").Index) = lL
            ' ak sme vratili mensi pocet tak predvytvor novy riadok s tym istym materialom
            If lPocKs > rsOUT("POC_KS").Value Then
              AddNewRowToGrid jgPolDkl
              ' copy riadky
              clsMngAll.clsJgPol.InsertRowFromJSRow jsRow, False
              .Value(.Columns("D3D_POC_KS").Index) = lPocKs - rsOUT("POC_KS").Value
              .Value(.Columns("POC_KS").Index) = 0
              If DKL = "MNF" Then .Value(.Columns("V").Index) = False
              .Value(.Columns("Z_CENA").Index) = 0
              .Value(.Columns("SN").Index) = ""
              .Value(.Columns("SARZA").Index) = ""
              .Value(.Columns("LOCATION").Index) = ""
              .Value(.Columns("CUST_STR_1").Index) = ""
              .Update
            End If
            ' udaje do jgSIM gridu
            Do While Not rsOUT.EOF
              AddNewRowToGrid jgSimDkl
              ' -------
              jgSimDkl.Value(jgSimDkl.Columns("ID_D").Index) = lID
              jgSimDkl.Value(jgSimDkl.Columns("SN").Index) = rsOUT("SN").Value
              jgSimDkl.Value(jgSimDkl.Columns("POC_KS").Index) = rsOUT("POC_KS").Value
              jgSimDkl.Value(jgSimDkl.Columns("POC_KS_VYREZ").Index) = CDbl(Format(rsOUT("OBJEM_VYREZ").Value, CS_FMT4))
              jgSimDkl.Value(jgSimDkl.Columns("POC_KS_ZVYSOK").Index) = CDbl(Format(rsOUT("OBJEM_ZVYSOK").Value, CS_FMT4))
              jgSimDkl.Value(jgSimDkl.Columns("POC_KS_PLT").Index) = CDbl(Format(rsOUT("OBJEM_PLT").Value, CS_FMT4))
              jgSimDkl.Value(jgSimDkl.Columns("SARZA").Index) = rsOUT("SARZA").Value
              jgSimDkl.Value(jgSimDkl.Columns("SKL_CENA").Index) = CDbl(Format(rsOUT("D_CENA").Value, clsMngAll.Helper.CS_DBL_FMT_N))
              jgSimDkl.Value(jgSimDkl.Columns("LOCATION").Index) = rsOUT("LOCATION").Value
              jgSimDkl.Value(jgSimDkl.Columns("OUTER_SIZE").Index) = rsOUT("OUTER_SIZE").Value
              jgSimDkl.Value(jgSimDkl.Columns("OUTER_SIZE_FINAL").Index) = rsOUT("OUTER_SIZE_FINAL").Value
              
              jgSimDkl.Value(jgSimDkl.Columns("PV").Index) = rsOUT("PV").Value ' Znamienko
              jgSimDkl.Value(jgSimDkl.Columns("A1").Index) = rsOUT("A1").Value
              jgSimDkl.Value(jgSimDkl.Columns("A2").Index) = rsOUT("A2").Value
              jgSimDkl.Value(jgSimDkl.Columns("B1").Index) = rsOUT("B1").Value
              jgSimDkl.Value(jgSimDkl.Columns("B2").Index) = rsOUT("B2").Value
              jgSimDkl.Value(jgSimDkl.Columns("L1").Index) = rsOUT("L1").Value
              jgSimDkl.Value(jgSimDkl.Columns("L2").Index) = rsOUT("L2").Value
              jgSimDkl.Value(jgSimDkl.Columns("D1").Index) = rsOUT("D1").Value
              jgSimDkl.Value(jgSimDkl.Columns("D2").Index) = rsOUT("D2").Value
              ' -------
              jgSimDkl.Update ' uloz row
              If jgSimDkl.DataChanged Then jgSimDkl.DataChanged = False
              
              rsOUT.MoveNext
            Loop
            DeactivateRs rsOUT
          
            'Refresni ComboDD
            If Not clsJgSim Is Nothing Then clsJgSim.RefillCombo_DD GetRsSimPol, "ID_D", JGMngDropPol
          End If
          
          Dim blnBefore As Boolean
          blnBefore = clsMngAll.clsJgPol.LockGrid
          clsMngAll.clsJgPol.LockGrid = False
          jgPolDkl_RowFormat .GetRowData(.Row)
          clsMngAll.clsJgPol.LockGrid = blnBefore
      
        End If
      Else
        MsgBox "Nie sú k dispozícii vstupné údaje pre simuláciu!", vbExclamation, sAppName
      End If
      DeactivateRs rsIN
    End If

            */



        }

        public object Any(CreateD_SIM_0 request)
        {
            return Repository.Create<V_SIM_0View>(request);
        }

        public object Any(UpdateD_SIM_0 request)
        {
            return Repository.Update<V_SIM_0View>(request);
        }

        public void Any(DeleteD_SIM_0 request)
        {
            Repository.Delete<tblD_SIM_0>(request.D_SIM_0);
        }


        GetSimData


        #endregion

        #region D_VYD_0

        public object Any(CreateD_VYD_0 request)
        {
            return Repository.Create<V_VYD_0View>(request);
        }

        public object Any(UpdateD_VYD_0 request)
        {
            return Repository.Update<V_VYD_0View>(request);
        }

        public void Any(DeleteD_VYD_0 request)
        {
            Repository.Delete<tblD_VYD_0>(request.D_VYD_0);
        }

        #endregion

        #region D_VYD_1

        public object Any(CreateD_VYD_1 request)
        {
            return Repository.Create<V_VYD_1View>(request);
        }

        public object Any(UpdateD_VYD_1 request)
        {
            return Repository.Update<V_VYD_1View>(request);
        }

        public void Any(DeleteD_VYD_1 request)
        {
            Repository.Delete<tblD_VYD_1>(request.D_VYD_1);
        }

        #endregion

        #region K_MAT_0

        public object Any(CreateK_MAT_0 request)
        {
            return Repository.Create<V_MAT_0View>(request);
        }

        public object Any(UpdateK_MAT_0 request)
        {
            return Repository.Update<V_MAT_0View>(request);
        }

        public void Any(DeleteK_MAT_0 request)
        {
            Repository.Delete<tblK_MAT_0>(request.K_MAT_0);
        }

        #endregion

        #region K_OBP_0

        public object Any(CreateK_OBP_0 request)
        {
            return Repository.Create<V_OBP_0View>(request);
        }

        public object Any(UpdateK_OBP_0 request)
        {
            return Repository.Update<V_OBP_0View>(request);
        }

        public void Any(DeleteK_OBP_0 request)
        {
            Repository.Delete<tblK_OBP_0>(request.K_OBP_0);
        }

        #endregion

        #region K_OWN_0
        public object Any(UpdateK_OWN_0 request)
        {
            return Repository.Update<V_OWN_0View>(request);
        }
        #endregion

        #region K_ORJ_0

        public object Any(CreateK_ORJ_0 request)
        {
            return Repository.Create<V_ORJ_0View>(request);
        }

        public object Any(UpdateK_ORJ_0 request)
        {
            return Repository.Update<V_ORJ_0View>(request);
        }

        public void Any(DeleteK_ORJ_0 request)
        {
            Repository.Delete<tblK_ORJ_0>(request.K_ORJ_0);
        }

        #endregion

        #region K_ORJ_1

        public object Any(CreateK_ORJ_1 request)
        {
            return Repository.Create<V_ORJ_1View>(request);
        }

        public object Any(UpdateK_ORJ_1 request)
        {
            return Repository.Update<V_ORJ_1View>(request);
        }

        public void Any(DeleteK_ORJ_1 request)
        {
            Repository.Delete<tblK_ORJ_1>(request.K_ORJ_1);
        }

        #endregion

        #region K_SKL_0

        public object Any(CreateK_SKL_0 request)
        {
            return Repository.Create<V_SKL_0View>(request);
        }

        public object Any(UpdateK_SKL_0 request)
        {
            return Repository.Update<V_SKL_0View>(request);
        }

        public void Any(DeleteK_SKL_0 request)
        {
            Repository.Delete<tblK_SKL_0>(request.K_SKL_0);
        }

        #endregion

        #region K_SKL_1

        public object Any(CreateK_SKL_1 request)
        {
            return Repository.Create<V_SKL_1View>(request);
        }

        public object Any(UpdateK_SKL_1 request)
        {
            return Repository.Update<V_SKL_1View>(request);
        }

        public void Any(DeleteK_SKL_1 request)
        {
            Repository.Delete<tblK_SKL_1>(request.K_SKL_1);
        }

        #endregion

        #region K_SKL_2

        public object Any(CreateK_SKL_2 request)
        {
            return Repository.Create<V_SKL_2View>(request);
        }

        public object Any(UpdateK_SKL_2 request)
        {
            return Repository.Update<V_SKL_2View>(request);
        }

        public void Any(DeleteK_SKL_2 request)
        {
            Repository.Delete<tblK_SKL_2>(request.K_SKL_2);
        }

        #endregion

        #region K_TSK_0

        public object Any(CreateK_TSK_0 request)
        {
            return Repository.Create<V_TSK_0View>(request);
        }

        public object Any(UpdateK_TSK_0 request)
        {
            return Repository.Update<V_TSK_0View>(request);
        }

        public void Any(DeleteK_TSK_0 request)
        {
            Repository.Delete<tblK_TSK_0>(request.K_TSK_0);
        }

        #endregion

        #region K_TYP_0

        public object Any(CreateK_TYP_0 request)
        {
            return Repository.Create<V_TYP_0View>(request);
        }

        public object Any(UpdateK_TYP_0 request)
        {
            return Repository.Update<V_TYP_0View>(request);
        }

        public void Any(DeleteK_TYP_0 request)
        {
            Repository.Delete<tblK_TYP_0>(request.K_TYP_0);
        }

        #endregion

        #region Nastavenie

        public object Any(GetParameterType request)
        {
            return Repository.GetParameterTypeBds(request);
        }

        public object Any(UpdateNastavenie request)
        {
            return Repository.UpdateNastavenieBds(request);
        }

        #endregion
    }
}
