using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WebEas.Esam.ServiceModel.Office.Bds.Defaults;
using WebEas.Esam.ServiceModel.Office.Bds.Dto;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.Esam.ServiceModel.Office.Dto;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public partial class BdsRepository : RepositoryBase, IBdsRepository
    {
        //public List<long> GetOdvybavDoklady(GetOdvybavDokladyReq request)
        //{
        //    return GetVybavOdvybavDoklady(request.IDs.ToList(), false, request.IdField);
        //}

        //public List<long> GetVybavDoklady(GetVybavDokladyReq request)
        //{
        //    return GetVybavOdvybavDoklady(request.IDs.ToList(), true, request.IdField);
        //}

        public List<long> GetVybavOdvybavDoklady(long[] Ids, bool pri)
        {
            // Vybavi doklady. V zozname by mali byt iba V jedněho typu

            var result = new List<long>();

            System.Data.IDbTransaction transaction = this.BeginTransaction();

            try
            {
                if (pri)
                {
                    var doklady = GetList(Db.From<tblD_PRI_0>().Where(x => Sql.In(x.D_PRI_0, Ids)));
                    if (!doklady.Any())
                    {
                        throw new WebEasValidationException(null, "No receipts found");
                    }

                    foreach (var dkl in doklady)
                    {
                        dkl.V = !dkl.V;
                        UpdateData(dkl);
                    }

                }
                else
                {
                    var doklady = GetList(Db.From<tblD_VYD_0>().Where(x => Sql.In(x.D_VYD_0, Ids)));
                    if (!doklady.Any())
                    {
                        throw new WebEasValidationException(null, "No expenses found");
                    }
                    foreach (var dkl in doklady)
                    {
                        dkl.V = !dkl.V;
                        UpdateData(dkl);
                    }
                }

                transaction.Commit();
            }
            catch (WebEasException ex)
            {
                transaction.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new WebEasException("Nastala chyba pri vybavovaní dokladu", "Nastala chyba pri vybavovaní dokladu", ex);
            }
            
            return result;

            //return result;
        }

        #region Warehouse

        #endregion

        #region Long Operations

        protected override void LongOperationProcess(WebEas.ServiceModel.Dto.LongOperationStartDtoBase request)
        {
            string operationParametersDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(request.OperationParameters));
            string operationInfoDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(request.OperationInfo));

            if (Enum.TryParse(request.OperationName, out OperationsList operation))
            {
                SpracovatDokladDto par = operationParametersDecoded.FromJson<SpracovatDokladDto>();
                LongOperationInfo inf = operationInfoDecoded.FromJson<LongOperationInfo>();
                switch (operation)
                {
                    // Operacie
                    case OperationsList.SpracovatDoklad:
                        GetVybavOdvybavDoklady(par.Ids, inf.Druh == "Receipts");
                        //SpracujDoklad(par, request.ProcessKey, out string spracovatDokladReportId);
                        break;
                }
            }
            else
            {
                throw new WebEasException($"Long operation with the name {request.OperationName} is not implemented", "Operácia nie je implementovaná!");
            }
        }

        private void InternyPoplatok(string processKey, string parameters)
        {
            string actionName = "Vyrubenie dane/poplatku";

            LongOperationStarted(processKey, actionName);
            LongOperationReportProgress(processKey, actionName, 0, 1);
            LongOperationSetStateFinished(processKey, string.Empty, "Hotovo", state: LongOperationState.Done);
        }

        #endregion

        #region GetRowDefaultValues

        public override object GetRowDefaultValues(string code, string masterCode, string masterRowId)
        {
            //Odkomentovať keď to chcem použiť
            var root = RenderModuleRootNode(code);
            var node = root.TryFindNode(code);
            HierarchyNode masternode = null;
            if (!masterCode.IsNullOrEmpty())
            {
                masternode = root.TryFindNode(masterCode);
            }

            #region D_PRI_0, D_VYD_0

            if (node != null && (node.ModelType == typeof(V_PRI_0View) || node.ModelType == typeof(V_VYD_0View)))
            {
                var FirstOrj = GetList<tblK_ORJ_0>().OrderBy(x => x.Serial_No).FirstOrDefault();
                var FirstSkl = GetList<tblK_SKL_0>().OrderBy(x => x.Serial_No).FirstOrDefault();

                return new Pri0Vyd0Defaults()
                {
                    DAT_DKL = DateTime.Now.Date,
                    K_ORJ_0 = (int)(FirstOrj?.K_ORJ_0 ?? null),
                    K_SKL_0 = (int)(FirstSkl?.K_SKL_0 ?? null)
                };
            }

            #endregion

            #region D_SIM_0

            if (node != null && node.ModelType == typeof(V_SIM_0View))
            {
                short r = 1;
                byte t = 1;

                if (masternode == null || string.IsNullOrEmpty(masterRowId))
                {
                    //nic
                }
                else if (masternode.ModelType == typeof(V_PRI_0View))
                {
                    var poslednaPolozka = GetList<tblD_SIM_0>(x => x.D_PRI_0 == Convert.ToInt16(masterRowId)).OrderByDescending(x => x.RANK).FirstOrDefault();
                    r = (short)((poslednaPolozka?.RANK ?? 0) + 1);
                    t = 1; //"+" - PV3DCombo.GetText
                }
                else if (masternode.ModelType == typeof(V_VYD_0View))
                {
                    var poslednaPolozka = GetList<tblD_SIM_0>(x => x.D_VYD_0 == Convert.ToInt16(masterRowId)).OrderByDescending(x => x.RANK).FirstOrDefault();
                    r = (short)((poslednaPolozka?.RANK ?? 0) + 1);
                    t = 2; //"-" - PV3DCombo.GetText
                }
                return new SimDefaults()
                {
                    RANK = r,
                    PV = t
                };
            }

            #endregion

            #region D_PRI_1, D_VYD_1

            if (node != null && masternode != null && !string.IsNullOrEmpty(masterRowId) && (node.ModelType == typeof(V_PRI_1View) || node.ModelType == typeof(V_VYD_1View)))
            {
                Pri1Vyd1Defaults def = new Pri1Vyd1Defaults() { RANK = 1};

                if (masternode.ModelType == typeof(V_PRI_0View))
                {
                    var poslednaPolozka = GetList<tblD_PRI_1>(x => x.D_PRI_0 == Convert.ToInt16(masterRowId)).OrderByDescending(x => x.RANK).FirstOrDefault();
                    if (poslednaPolozka != null) { def.RANK = (short)((poslednaPolozka?.RANK ?? 0) + 1); }

                    var masterRow = GetById<V_PRI_0View>(masterRowId);
                    if (masterRow != null) { def.K_SKL_0 = masterRow.K_SKL_0; }
                }
                else if (masternode.ModelType == typeof(V_VYD_0View))
                {
                    var poslednaPolozka = GetList<tblD_VYD_1>(x => x.D_VYD_0 == Convert.ToInt16(masterRowId)).OrderByDescending(x => x.RANK).FirstOrDefault();
                    if (poslednaPolozka != null) { def.RANK = (short)((poslednaPolozka?.RANK ?? 0) + 1); }

                    var masterRow = GetById<V_VYD_0View>(masterRowId);
                    if (masterRow != null) {def.K_SKL_0 = masterRow.K_SKL_0; }
                }
                return def;
            }

            #endregion



            return new object();
        }

        #endregion

        #region Nastavenie

        public object GetParameterTypeBds(GetParameterType data)
        {
            Nastavenie typ;

            var filter = new Filter();
            filter.AndEq("Nazov", data.Nazov);
            filter.AndEq("Modul", "bds");

            typ = GetList<Nastavenie>(filter).FirstOrDefault();
            if (typ == null)
            {
                throw new WebEasException(null, "Parameter nenájdený!");
            }

            switch (typ.Typ)
            {
                case "S":
                    return new { typ.Typ, hodnota = typ.Hodn };
                case "I":
                    return new { typ.Typ, hodnota = Convert.ToInt64(typ.Hodn) };
                case "B":
                    bool byt = typ.Hodn == "1";
                    return new { typ.Typ, hodnota = byt };
                case "D":
                    return new { typ.Typ, hodnota = Convert.ToDateTime(typ.Hodn) };
                case "T":
                    return new { typ.Typ, hodnota = Convert.ToDateTime(typ.Hodn) };
                case "N":
                    return new { typ.Typ, hodnota = Convert.ToDecimal(typ.Hodn) };
                default:
                    return null;
            }
        }

        public long UpdateNastavenieBds(UpdateNastavenie data)
        {
            System.Data.IDbTransaction transaction = this.BeginTransaction();

            try
            {
                string typ = Db.Scalar<string>("Select Typ from [reg].[V_Nastavenie] where Nazov = @nazov and Modul = 'rzp'", new { nazov = data.Nazov });
                var p = new DynamicParameters();
                p.Add("@tenant", Session.TenantIdGuid, dbType: System.Data.DbType.Guid);
                p.Add("@modul", "bds", dbType: System.Data.DbType.String);
                p.Add("@nazov", data.Nazov, dbType: System.Data.DbType.String);
                p.Add("@pouzivatel", null, dbType: System.Data.DbType.String);
                switch (typ)
                {
                    case "I":
                        p.Add("@bigint", data.iHodn, dbType: System.Data.DbType.Int64);
                        break;
                    case "S":
                        p.Add("@string", data.sHodn, dbType: System.Data.DbType.String);
                        break;
                    case "B":
                        p.Add("@bit", data.bHodn, dbType: System.Data.DbType.Boolean);
                        break;
                    case "D":
                        p.Add("@date", data.dHodn, dbType: System.Data.DbType.Date);
                        break;
                    case "T":
                        p.Add("@datetime", data.tHodn, dbType: System.Data.DbType.DateTime);
                        break;
                    case "N":
                        p.Add("@numeric", data.nHodn, dbType: System.Data.DbType.Decimal);
                        break;
                }

                SqlProcedure("[reg].[PR_Nastavenie]", p);

                transaction.Commit();
            }
            catch (WebEasValidationException ex)
            {
                transaction.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new WebEasException("Nastala chyba pri volaní SQL procedúry [reg].[PR_Nastavenie]", "Parameter sa nepodarilo upraviť kvôli internej chybe", ex);
            }

            RemoveFromCacheByRegex(string.Concat("ten:", Session.TenantId, ":GetNastavenie:*"));
            return 1;
        }

        #endregion

        #region AccessFlag

        public override void SetAccessFlag(object viewData)
        {
            base.SetAccessFlag(viewData);

            // flagy specificke pre modul
            if (viewData is System.Collections.IEnumerable)
            {
                var e = viewData as System.Collections.IEnumerable;
                var baseEntityList = e.OfType<BaseEntity>();

                var entityType = e.GetType().GetProperty("Item").PropertyType;
                var props = new List<PropertyInfo>(entityType.GetProperties());

                //PropertyInfo piTenantId = props.FirstOrDefault(p => p.Name.ToUpper() == "D_TENANT_ID");
                //PropertyInfo piStavEntity = props.FirstOrDefault(p => p.Name.ToUpper() == "C_STAVENTITY_ID");
                PropertyInfo piV = props.FirstOrDefault(p => p.Name.ToUpper() == "V");

                foreach (var baseEntity in baseEntityList)
                {
                    //var tenantId = piTenantId == null ? null : (Guid?)piTenantId.GetValue(baseEntity);
                    //var stavEntity = piStavEntity == null ? null : (int?)piStavEntity.GetValue(baseEntity);
                    bool jeVybavene = piV == null ? false : (bool)piV.GetValue(baseEntity);


                    //nastavenie flagov podla vybavenia
                    if (piV != null)
                    {
                        baseEntity.AccessFlag |= (long)NodeActionFlag.SpracovatDoklad; //Nemám žiaden vyšší stav

                        if (jeVybavene)
                        {
                            if ((baseEntity.AccessFlag & (long)NodeActionFlag.Create) > 0) baseEntity.AccessFlag -= (long)(NodeActionFlag.Create);
                            if ((baseEntity.AccessFlag & (long)NodeActionFlag.Update) > 0) baseEntity.AccessFlag -= (long)(NodeActionFlag.Update);
                            if ((baseEntity.AccessFlag & (long)NodeActionFlag.Delete) > 0) baseEntity.AccessFlag -= (long)(NodeActionFlag.Delete);
                        }
                    }
                }
            }
        }

        #endregion

        #region V_PRI_0View

        public V_PRI_0View CreateD_PRI_0(CreateD_PRI_0 data)
        {
            int i = GetCisloDokladuSkl(data.DAT_DKL, data.K_SKL_0, "bds.V_PRI_0", out string interneCisloDokladu);
            data.DKL_C = interneCisloDokladu;
            var dkl = Create<V_PRI_0View>(data);
            return dkl;
        }

        #endregion

        #region V_VYD_0View

        public V_VYD_0View CreateD_VYD_0(CreateD_VYD_0 data)
        {
            int i = GetCisloDokladuSkl(data.DAT_DKL, data.K_SKL_0, "bds.V_VYD_0", out string interneCisloDokladu);
            data.DKL_C = interneCisloDokladu;
            var dkl = Create<V_VYD_0View>(data);
            return dkl;
        }

        #endregion


        private int GetCisloDokladuSkl(DateTime datumDokladu, int sklId, string tableName, out string newCisloInterne)
        {
            bool ok = GetDefCisloDokladu(datumDokladu, sklId, out newCisloInterne, out string likeSql);
            string txt;
            char paddingChar;
            string newCounter;

            likeSql = likeSql.Replace(Regex.Match(likeSql, @"<C,(\d*)>").Groups[0].Value,
                "_".PadLeft(int.Parse(Regex.Match(likeSql, @"<C,(\d*)>").Groups[1].Value), '_'));

            newCounter = Db.Scalar<string>($@"SELECT TOP(1) DKL_C FROM {tableName}
                                              WHERE CisloInterne like '{likeSql}' 
                                              ORDER BY DKL_C DESC");

            paddingChar = '0';
            txt = newCounter.ToString();
            newCisloInterne = newCisloInterne.Replace(Regex.Match(newCisloInterne, @"<.,(\d*)>").Groups[0].Value, txt.PadLeft(int.Parse(Regex.Match(newCisloInterne, @"<.,(\d*)>").Groups[1].Value), paddingChar));
            return 1;
            //return newCounter;
        }

        private bool GetDefCisloDokladu(DateTime datumDokladu, int sklId, out string newCisloInterne, out string likeSql)
        {
            // Zistenie masky
            bool raiseValidityError = true;

            newCisloInterne = "[SKL3]-[RR].{MM}/<C,4>";

            string skl = Db.Scalar<string>("SELECT KOD FROM [bds].[V_SKL_0] WHERE K_SKL_0 = @K_SKL_0", new { K_SKL_0 = sklId });

            likeSql = newCisloInterne.Replace("_", "[_]");

            //ORJ SKL POK VBU ZAM
            newCisloInterne = OneMatch(newCisloInterne, raiseValidityError, @"[[{](ORJ|SKL|POK|VBU|ZAM)(\d*)[]}]", "", "", "", skl, "", ref likeSql, "Nepodarilo sa vygenerovať číslo dokladu. Skontrolujte nastavenie číselného radu (kód ORŠ - strediska, bank.účtu alebo pokladnice)!");
            //Mesiac
            newCisloInterne = OneMatch(newCisloInterne, raiseValidityError, @"[[{](MM)(\d*)[]}]", "MM", "", "", "", datumDokladu.Month.ToString().PadLeft(2, '0'), ref likeSql, "Nepodarilo sa vygenerovať číslo dokladu. Skontrolujte nastavenie číselného radu (mesiac)!");
            //Rok
            newCisloInterne = OneMatch(newCisloInterne, raiseValidityError, @"[[{](R|RR|RRRR)(\d*)[]}]", "", "", "", "", datumDokladu.Year.ToString(), ref likeSql, "Nepodarilo sa vygenerovať číslo dokladu. Skontrolujte nastavenie číselného radu (rok)!");

            return true;
        }
    }
}
