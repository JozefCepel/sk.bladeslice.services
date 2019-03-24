using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Bds.Dto;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public partial class BdsRepository : RepositoryBase, IBdsRepository
    {

        #region Warehouse

        #endregion

        #region Long Operations

        protected override void LongOperationProcess(string processKey, string operationName, string operationParameters)
        {
            switch (operationName)
            {
                case OperationsList.InternyPoplatok:
                    this.InternyPoplatok(processKey, operationParameters);
                    break;
                default:
                    throw new WebEasException(string.Format("Long operation with the name {0} is not implemented", operationName), "Operácia nie je implementovaná!");
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
            var node = Modules.TryFindNode(code);
            var masternode = Modules.TryFindNode(masterCode);

            /*
            #region Programový rozpočet

            var aktDatum = DateTime.Now;

            if (node != null && node.ModelType == typeof(ProgramView))
            {
                var poslednaPolozka = GetList<ProgramView>(x => x.PRTyp == 1).OrderByDescending(x => x.program).FirstOrDefault();
                return new ProgramView()
                {
                    PRTyp = 1,
                    PRTypMeno = "Program",
                    program = (short)((poslednaPolozka?.program ?? 0) + 1),
                    PlatnostOd = aktDatum
                };
            }

            if (node != null && node.ModelType == typeof(ProgramovyRozpocetProgramView))
            {
                var poslednaPolozka = GetList<ProgramView>(x => x.PRTyp == 2 && x.program == (short)node.Parameter).OrderByDescending(x => x.podprogram).FirstOrDefault();
                return new ProgramView()
                {
                    PRTyp = 2,
                    PRTypMeno = "Podprogram",
                    program = short.Parse(node.Parameter.ToString()),
                    podprogram = (short)((poslednaPolozka?.podprogram ?? 0) + 1),
                    PlatnostOd = aktDatum
                };
            }

            if (node != null && node.ModelType == typeof(ProgramovyRozpocetPodprogramView))
            {
                var poslednaPolozka = GetList<ProgramView>(x => x.PRTyp == 3 && x.program == (short)node.Parent.Parameter && x.podprogram == (short)node.Parameter).OrderByDescending(x => x.prvok).FirstOrDefault();
                return new ProgramView()
                {
                    PRTyp = 3,
                    PRTypMeno = "Prvok",
                    program = short.Parse(node.Parent.Parameter.ToString()),
                    podprogram = short.Parse(node.Parameter.ToString()),
                    prvok = (short)((poslednaPolozka?.prvok ?? 0) + 1),
                    PlatnostOd = aktDatum
                };
            }

            #endregion

            #region Ciele

            if (node != null && node.ModelType == typeof(CieleView) && masternode.ModelType == typeof(ProgramovyRozpocetProgramView) && !string.IsNullOrEmpty(masterRowId))
            {
                var masterRow = GetById<ProgramView>(masterRowId);
                return new CieleView()
                {
                    D_User_Id_Zodp = Session.DcomIdGuid,
                    D_Program_Id = masterRow.D_Program_Id,
                    PRFull = masterRow.PRFull,
                    ProgramKod = masterRow.PRKod,
                    ProgramNazov = masterRow.PRNazov
                };
            }

            if (node != null && node.ModelType == typeof(CieleView))
            {
                return new CieleView()
                {
                    D_User_Id_Zodp = Session.DcomIdGuid
                };
            }

            #endregion

            #region Ciele / Ukazovatele

            if (node != null && node.ModelType == typeof(CieleUkazView) && masternode.ModelType == typeof(CieleView) && !string.IsNullOrEmpty(masterRowId))
            {
                var masterRow = GetById<CieleView>(masterRowId);
                var program = GetById<ProgramView>(masterRow.D_Program_Id);
                return new CieleUkazView()
                {
                    D_User_Id_Zodp = Session.DcomIdGuid,
                    D_Program_Id = program.D_Program_Id,
                    ProgramKod = program.PRKod,
                    ProgramNazov = program.PRNazov,
                    PRFull = program.PRFull,
                    D_PRCiele_Id = masterRow.D_PRCiele_Id,
                    CielNazov = masterRow.Nazov
                };
            }

            #endregion

            #region Rozpočtové položky

            if (node != null && node.ModelType == typeof(BdsPolozkyPrijView))
            {
                return new BdsPolozkyView()
                {
                    PlatnostOd = DateTime.Now,
                    PrijemVydaj = 1
                };
            }

            if (node != null && node.ModelType == typeof(BdsPolozkyVydView))
            {
                return new BdsPolozkyView()
                {
                    PlatnostOd = DateTime.Now,
                    PrijemVydaj = 2
                };
            }

            #endregion

            #region Návrhy rozpočtu

            if (node != null && node.ModelType == typeof(NavrhBdsView))
            {
                return new NavrhBdsView()
                {
                    Datum = DateTime.Now.Date,
                    Rok = DateTime.Now.Year,
                    Nazov = string.Concat("Návrh rozpočtu na rok ", DateTime.Now.Year),
                    D_User_Id_Zodp = Session.DcomIdGuid.Value,
                    Typ = false,
                    C_StavEntity_Id = 1
                };
            }

            if (node != null && node.ModelType == typeof(ZmenyBdsView))
            {
                return new ZmenyBdsView()
                {
                    Datum = DateTime.Now.Date,
                    Rok = DateTime.Now.Year,
                    Nazov = string.Concat("Úprava rozpočtu k ", DateTime.Now.ToShortDateString()),
                    D_User_Id_Zodp = Session.DcomIdGuid.Value,
                    Typ = true,
                    C_StavEntity_Id = 1
                };
            }

            #endregion

            #region Položky návrhov

            if (node != null && node.ModelType == typeof(NavrhyBdsValView) && masternode != null && masternode.ModelType == typeof(NavrhBdsView) && masterRowId != null)
            {
                var masterRow = GetById<NavrhBdsView>(masterRowId);
                if (masterRow != null)
                {
                    return new NavrhyBdsValView()
                    {
                        D_NavrhZmenyBds_Id = masterRow.D_NavrhZmenyBds_Id,
                        Rok = masterRow.Rok,
                        NavrhZmenyBdsNazov = masterRow.Nazov
                    };
                }
            }

            #endregion

            #region Položky zmien

            if (node != null && node.ModelType == typeof(ZmenyBdsValView) && masternode != null && masternode.ModelType == typeof(ZmenyBdsView) && masterRowId != null)
            {
                var masterRow = GetById<ZmenyBdsView>(masterRowId);
                if (masterRow != null)
                {
                    return new ZmenyBdsValView()
                    {
                        D_NavrhZmenyBds_Id = masterRow.D_NavrhZmenyBds_Id,
                        Datum = masterRow.Datum,
                        NavrhZmenyBdsNazov = masterRow.Nazov
                    };
                }
            }

            #endregion

            #region Int. doklad

            if (node != null && node.ModelType == typeof(IntDokladView))
            {
                var maxCisloDokladu = Db.Scalar<int>(Db.From<IntDokladView>().Select(x => Sql.Max(x.CisloDokladu))) + 1;
                return new IntDokladView()
                {
                    DatumDokladu = DateTime.Now,
                    Rok = DateTime.Now.Year,
                    UO = (byte)DateTime.Now.Month,
                    CisloDokladu = maxCisloDokladu.ToString()
                };
            }

            #endregion
           */

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
                string typ = Db.Scalar<string>("Select Typ from [rzp].[V_Nastavenie] where Nazov = @nazov and Modul = 'rzp'", new { nazov = data.Nazov });
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
            finally
            {
                EndTransaction(transaction);
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

                var piTenantId = props.FirstOrDefault(p => p.Name.ToUpper() == "D_TENANT_ID");
                var piStavEntity = props.FirstOrDefault(p => p.Name.ToUpper() == "C_STAVENTITY_ID");

                foreach (var baseEntity in baseEntityList)
                {
                    var tenantId = piTenantId == null ? null : (Guid?)piTenantId.GetValue(baseEntity);
                    var stavEntity = piStavEntity == null ? null : (int?)piStavEntity.GetValue(baseEntity);

                    //nastavenie flagov podla stavu
                    if (stavEntity.HasValue)
                    {
                        switch (stavEntity.Value)
                        {
                            // ---------------------------- RZP Návrhy
                            case 1: // NOVY
                                baseEntity.AccessFlag |= (long)NodeActionFlag.ZmenaStavuPodania;
                                baseEntity.AccessFlag |= (long)NodeActionFlag.PrevziatNavrhRozpoctu;
                                break;
                            case 2: // SCHVALENY
                                baseEntity.AccessFlag |= (long)NodeActionFlag.ZmenaStavuPodania;
                                break;
                            case 3: // NESCHVALENY
                                baseEntity.AccessFlag |= (long)NodeActionFlag.ZmenaStavuPodania;
                                break;
                            case 4: // SCHVALOVANIE_ZRUSENE
                                baseEntity.AccessFlag |= (long)NodeActionFlag.PrevziatNavrhRozpoctu;
                                break;
                            case 5: // ODOSLANY
                                break;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
