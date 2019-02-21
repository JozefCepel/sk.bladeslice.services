using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Rzp.Dto;
using WebEas.Esam.ServiceModel.Office.Rzp.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office.Rzp
{
    public partial class RzpRepository : RepositoryBase, IRzpRepository
    {

        #region Program

        private void CheckProgram(short? program, short? podprogram, short? prvok, long? D_Program_Id = null)
        {
            if (!program.HasValue)
            {
                throw new WebEasValidationException(null, "Nesprávne vyplnený kód programu!");
            }

            if (podprogram.HasValue && !program.HasValue)
            {
                throw new WebEasValidationException(null, "Nesprávne vyplnený kód programu. Chýba program!");
            }

            if (prvok.HasValue && !podprogram.HasValue)
            {
                throw new WebEasValidationException(null, "Nesprávne vyplnený kód programu. Chýba podprogram!");
            }

            if (prvok.HasValue && !program.HasValue)
            {
                throw new WebEasValidationException(null, "Nesprávne vyplnený kód programu. Chýba program!");
            }

            if (podprogram != null && !GetList<ProgramView>(x => x.program == program && x.podprogram == null).Any())
            {
                throw new WebEasValidationException(null, "Nedodržaná štruktúra. Zadajte najprv program!");
            }

            if (podprogram != null && prvok != null && !GetList<ProgramView>(x => x.program == program && x.podprogram == podprogram).Any())
            {
                throw new WebEasValidationException(null, "Nedodržaná štruktúra. Zadajte najprv podprogram!");
            }

            if (D_Program_Id.HasValue)
            {
                if (GetList<ProgramView>(x => x.program == program && x.podprogram == podprogram && x.prvok == prvok && x.D_Program_Id != D_Program_Id).Any())
                {
                    throw new WebEasValidationException(null, "Položka s rovnakým kódom programu už existuje!");
                }
            }
            else
            {
                if (GetList<ProgramView>(x => x.program == program && x.podprogram == podprogram && x.prvok == prvok).Any())
                {
                    throw new WebEasValidationException(null, "Položka s rovnakým kódom programu už existuje!");
                }    
            }
        }

        public ProgramView CreateDefPrrProgramovyRozpocet(CreateDefPrrProgramovyRozpocet request)
        {
            CheckProgram(request.program, request.podprogram, request.prvok);
            return Create<ProgramView>(request);
        }

        public ProgramView UpdateDefPrrProgramovyRozpocet(UpdateDefPrrProgramovyRozpocet request)
        {
            CheckProgram(request.program, request.podprogram, request.prvok, request.D_Program_Id);
            return Update<ProgramView>(request);
        }

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

            if (node != null && node.ModelType == typeof(RzpPolozkyPrijView))
            {
                return new RzpPolozkyView()
                {
                    PlatnostOd = DateTime.Now,
                    PrijemVydaj = 1
                };
            }

            if (node != null && node.ModelType == typeof(RzpPolozkyVydView))
            {
                return new RzpPolozkyView()
                {
                    PlatnostOd = DateTime.Now,
                    PrijemVydaj = 2
                };
            }

            #endregion

            #region Návrhy rozpočtu

            if (node != null && node.ModelType == typeof(NavrhRzpView))
            {
                return new NavrhRzpView()
                {
                    Datum = DateTime.Now.Date,
                    Rok = DateTime.Now.Year,
                    Nazov = string.Concat("Návrh rozpočtu na rok ", DateTime.Now.Year),
                    D_User_Id_Zodp = Session.DcomIdGuid.Value,
                    Typ = false,
                    C_StavEntity_Id = 1
                };
            }

            if (node != null && node.ModelType == typeof(ZmenyRzpView))
            {
                return new ZmenyRzpView()
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

            if (node != null && node.ModelType == typeof(NavrhyRzpValView) && masternode != null && masternode.ModelType == typeof(NavrhRzpView) && masterRowId != null)
            {
                var masterRow = GetById<NavrhRzpView>(masterRowId);
                if (masterRow != null)
                {
                    return new NavrhyRzpValView()
                    {
                        D_NavrhZmenyRzp_Id = masterRow.D_NavrhZmenyRzp_Id,
                        Rok = masterRow.Rok,
                        NavrhZmenyRzpNazov = masterRow.Nazov
                    };
                }
            }

            #endregion

            #region Položky zmien

            if (node != null && node.ModelType == typeof(ZmenyRzpValView) && masternode != null && masternode.ModelType == typeof(ZmenyRzpView) && masterRowId != null)
            {
                var masterRow = GetById<ZmenyRzpView>(masterRowId);
                if (masterRow != null)
                {
                    return new ZmenyRzpValView()
                    {
                        D_NavrhZmenyRzp_Id = masterRow.D_NavrhZmenyRzp_Id,
                        Datum = masterRow.Datum,
                        NavrhZmenyRzpNazov = masterRow.Nazov
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

            #region Dennik Rzp.

            if (node != null && node.ModelType == typeof(DennikRzpView))
            {
                if (masternode != null && masternode.ModelType == typeof(IntDokladView) && !string.IsNullOrEmpty(masterRowId))
                {
                    var intDoklad = GetById<IntDokladView>(masterRowId);

                    if (intDoklad != null)
                    {
                        return new DennikRzpView()
                        {
                            DatumDokladu = intDoklad.DatumDokladu,
                            CisloDokladu = intDoklad.CisloDokladu,
                            D_IntDoklad_Id = intDoklad.D_IntDoklad_Id,
                            UO = intDoklad.UO,
                            Rok = intDoklad.Rok,
                            PrijemVydaj = 1,
                            Suma = 0
                        };
                    }
                }
            }

            #endregion

            return new object();
        }

        #endregion

        #region Nastavenie

        public object GetParameterTypeRzp(GetParameterType data)
        {
            Nastavenie typ;

            var filter = new Filter();
            filter.AndEq("Nazov", data.Nazov);
            filter.AndEq("Modul", "rzp");

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

        public long UpdateNastavenieRzp(UpdateNastavenie data)
        {
            System.Data.IDbTransaction transaction = this.BeginTransaction();

            try
            {
                string typ = Db.Scalar<string>("Select Typ from [rzp].[V_Nastavenie] where Nazov = @nazov and Modul = 'rzp'", new { nazov = data.Nazov });
                var p = new DynamicParameters();
                p.Add("@tenant", Session.TenantIdGuid, dbType: System.Data.DbType.Guid);
                p.Add("@modul", "rzp", dbType: System.Data.DbType.String);
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

        #region Int. doklad

        public IntDokladView Create(CreateIntDoklad data)
        {
            var transaction = BeginTransaction();
            long id = 0;
            try
            {
                var be = new BiznisEntita
                {
                    CisloDokladu = data.CisloDokladu,
                    C_Stredisko_Id = data.C_Stredisko_Id,
                    DatumDokladu = data.DatumDokladu,
                    Rok = data.DatumDokladu.Year,
                    UO = data.UO,
                    VS = data.VS,
                    C_TypBiznisEntity_Id = (int)TypBiznisEntityEnum.RZP_IntDoklad,
                    C_StavovyPriestor_Id = 1,
                    C_StavEntity_Id = 1,
                    PolozkaStromu = "rzp-evi-intd"
                };

                var beId = InsertData(be);
                var intDokl = data.ConvertToEntity();
                intDokl.D_BiznisEntita_Id = beId;
                intDokl.Rok = be.Rok;
                id = InsertData(intDokl);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                EndTransaction(transaction);
            }

            return GetById<IntDokladView>(id);
        }

        public IntDokladView Update(UpdateIntDoklad data)
        {
            var transaction = BeginTransaction();
            long id = 0;
            try
            {
                var intDokl = GetById<IntDoklad>(data.D_IntDoklad_Id);
                var be = GetById<BiznisEntita>(intDokl.D_BiznisEntita_Id);

                be.CisloDokladu = data.CisloDokladu;
                be.C_Stredisko_Id = data.C_Stredisko_Id;
                be.DatumDokladu = data.DatumDokladu;
                be.Rok = data.DatumDokladu.Year;
                be.UO = data.UO;
                be.VS = data.VS;
                UpdateData(be);
                data.UpdateEntity(intDokl);
                intDokl.Rok = be.Rok;
                UpdateData(intDokl);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                EndTransaction(transaction);
            }

            return GetById<IntDokladView>(id);
        }

        #endregion

        #region Návrhy rozpočtu

        public NavrhyRzpValView Create(CreateNavrhyRzpVal data)
        {
            if (data.C_RzpPolozky_Id.HasValue)
            {
                var rzpUcet = GetById<RzpPolozkyView>(data.C_RzpPolozky_Id);
                if (rzpUcet.PrijemVydaj == 1 && data.D_Program_Id.HasValue)
                {
                    throw new WebEasValidationException(null, "Pre príjmový účet nie je možné zapísať program.");
                }
            }
            
            return GetById<NavrhyRzpValView>(Create(data.ConvertToEntity()));
        }

        public NavrhyRzpValView Update(UpdateNavrhyRzpVal data)
        {
            if (data.C_RzpPolozky_Id.HasValue)
            {
                var rzpUcet = GetById<RzpPolozkyView>(data.C_RzpPolozky_Id);
                if (rzpUcet.PrijemVydaj == 1 && data.D_Program_Id.HasValue)
                {
                    throw new WebEasValidationException(null, "Pre príjmový účet nie je možné zapísať program.");
                }
            }

            return GetById<NavrhyRzpValView>(Update(data.ConvertToEntity()));
        }

        public void ChangeStateNavrh(ChangeStateDto state)
        {
            var rec = GetList<NavrhRzpView>(x => x.D_NavrhZmenyRzp_Id == state.Id).SingleOrDefault();
            if (rec != null)
            {
                var existedRec = GetList<NavrhRzpView>(x => x.D_NavrhZmenyRzp_Id != rec.D_NavrhZmenyRzp_Id && x.Rok == rec.Rok && x.Typ == false && (x.C_StavEntity_Id == 2 || x.C_StavEntity_Id == 5));
                if (existedRec.Any())
                {
                    throw new WebEasValidationException(null, $"Pre rok {rec.Rok} existuje {existedRec.First().StavNazov} návrh rozpočtu!");
                }

                using (var transaction = BeginTransaction())
                {
                    try
                    {
                        ChangeState<NavrhZmenyRzp>(state.Id, state.IdNewState);

                        var historia = new EntitaHistoriaStavov
                        {
                            ZmenaStavuDatum = DateTime.Now,
                            C_StavEntity_Id_New = state.IdNewState,
                            C_StavEntity_Id_Old = rec.C_StavEntity_Id,
                            C_StavovyPriestor_Id = (int)StavovyPriestorEnum.RZP_NavrhUprava,
                            D_NavrhZmenyRzp_Id = rec.D_NavrhZmenyRzp_Id,
                            VyjadrenieSpracovatela = state.VyjadrenieSpracovatela
                        };

                        InsertData(historia);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new WebEasException("Nastala chyba pri zmene stavu", ex);
                    }
                }
            }
            else
                throw new WebEasNotFoundException("Neexistujuci zaznam");
        }

        public void PrevziatNavrhRozpoctu(PrevziatNavrhRozpoctuDto request)
        {
            var transaction = BeginTransaction();
            try
            {
                if (request.D_NavrhZmenyRzp_Id != 0)
                {
                    // nova hlavicka
                    var h_new = GetList<NavrhRzpView>(x => x.D_NavrhZmenyRzp_Id == request.D_NavrhZmenyRzp_Id).SingleOrDefault();
                    if (h_new != null) // if (existedRec.Any())
                    {
                        // stara hlavicka
                        var filt = new Filter();
                        filt.AndEq("Typ", false);
                        filt.AndEq("Rok", h_new.Rok - 1);
                        filt.And(FilterElement.In("C_StavEntity_Id", new[] {(int)StavEntityEnumRzp.SCHVALENY, (int)StavEntityEnumRzp.ODOSLANY} ));

                        var h_old = GetList<NavrhRzpView>(filt);
                        if (h_old.Count == 1)
                        {
                            // zmazat stare
                            if (request.OdstranitPolozkyNavyse)
                            {
                                var p_new = GetList<NavrhyRzpValView>(x => x.D_NavrhZmenyRzp_Id == h_new.D_NavrhZmenyRzp_Id);
                                p_new.ForEach(x =>
                                {
                                    DeleteData<NavrhyRzpVal>(x.D_NavrhyRzpVal_Id);
                                });
                            }
                            // polozky
                            var p_old = GetList<NavrhyRzpValView>(x => x.D_NavrhZmenyRzp_Id == h_old[0].D_NavrhZmenyRzp_Id);
                            if (request.AktualizovatHodnoty) // aktualizujeme tie co najdeme
                            {
                                var p_upd = GetList<NavrhyRzpVal>(x => x.D_NavrhZmenyRzp_Id == h_new.D_NavrhZmenyRzp_Id);
                                p_upd.ForEach(x =>
                                {
                                    foreach (NavrhyRzpValView y in p_old)
                                    {
                                        if (y.C_RzpPolozky_Id == x.C_RzpPolozky_Id && y.D_Program_Id == x.D_Program_Id && y.C_Stredisko_Id == x.C_Stredisko_Id & y.C_Projekt_Id == x.C_Projekt_Id)
                                        {
                                            x.SchvalenyRzp = GetRightValue(request.Navrh, y);
                                            x.NavrhRzp1 = GetRightValue(request.Rok1, y);
                                            x.NavrhRzp2 = GetRightValue(request.Rok2, y);
                                            // zapis zmeny
                                            UpdateData(x);
                                            break;
                                        }
                                    }
                                });
                                transaction.Commit();
                            }
                            else {  // vytvarame nove
                                if (p_old.Count != 0) 
                                {
                                    p_old.ForEach(x =>
                                    {
                                        if ((!request.VynechatNulove) || (request.VynechatNulove && (x.SchvalenyRzp > 0) && (x.NavrhRzp1 > 0) && (x.NavrhRzp2 > 0)))
                                        {
                                            var p_new = new NavrhyRzpVal { };
                                            p_new.D_NavrhZmenyRzp_Id = h_new.D_NavrhZmenyRzp_Id;
                                            p_new.C_RzpPolozky_Id = x.C_RzpPolozky_Id;
                                            p_new.D_Program_Id = x.D_Program_Id;
                                            p_new.C_Stredisko_Id = x.C_Stredisko_Id;
                                            p_new.C_Projekt_Id = x.C_Projekt_Id;
                                            p_new.SchvalenyRzp = GetRightValue(request.Navrh, x);
                                            p_new.NavrhRzp1 = GetRightValue(request.Rok1, x);
                                            p_new.NavrhRzp2 = GetRightValue(request.Rok2, x);
                                            p_new.Poznamka = x.Poznamka;
                                            // zapis
                                            InsertData(p_new);
                                        }
                                    });
                                    transaction.Commit();
                                }
                                else
                                    throw new WebEasNotFoundException(string.Format("Nenašli sa žiadne položky na prenos!", h_old.Count, h_new.Rok - 1));
                            }
                        }
                        else if (h_old.Count == 0)
                            throw new WebEasNotFoundException(string.Format("Nenašiel sa záznam z predch. roku {0}!", h_new.Rok - 1));
                        else
                            throw new WebEasNotFoundException(string.Format("Našlo sa viac záznamov ({0}) v roku {1}!", h_old.Count, h_new.Rok - 1));
                    }
                    else
                        throw new WebEasNotFoundException("Neexistujúci záznam!");
                }
                else
                    throw new WebEasNotFoundException("Nie je vybranný záznam!");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                EndTransaction(transaction);
            }
        }

        private static decimal GetRightValue(byte typ, NavrhyRzpValView rec)
        {
            decimal retVal;
            switch (typ)
            {
                case 1:    // 1=Schválený rozpočet (@Rok-1)
                    retVal = rec.SchvalenyRzp;
                    break;
                case 2:    // 2=Upravený rozpočet (@Rok-1)
                    retVal = rec.SchvalenyRzp + rec.SumaZmeny;
                    break;
                case 3:    // 3=Čerpanie/Plnenie (@Rok-1)
                    retVal = rec.SumaCerpanie;
                    break;
                case 4:    // 4=Návrh (@Rok)
                    retVal = rec.NavrhRzp1 ;
                    break;
                case 5:    //  5=Návrh (@Rok+1)
                    retVal = rec.NavrhRzp2;
                    break;
                case 6:    // 6=Nulové hodnoty
                    retVal = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("GetRightValue.typ");
            }
            return retVal;
        }

        #endregion

        #region Zmeny rozpočtu

        public ZmenyRzpValView Create(CreateZmenyRzpVal data)
        {
            if (data.C_RzpPolozky_Id.HasValue)
            {
                var rzpUcet = GetById<RzpPolozkyView>(data.C_RzpPolozky_Id);
                if (rzpUcet.PrijemVydaj == 1 && data.D_Program_Id.HasValue)
                {
                    throw new WebEasValidationException(null, "Pre príjmový účet nie je možné zapísať program.");
                }
            }

            return GetById<ZmenyRzpValView>(Create(data.ConvertToEntity()));
        }

        public ZmenyRzpValView Update(UpdateZmenyRzpVal data)
        {
            if (data.C_RzpPolozky_Id.HasValue)
            {
                var rzpUcet = GetById<RzpPolozkyView>(data.C_RzpPolozky_Id);
                if (rzpUcet.PrijemVydaj == 1 && data.D_Program_Id.HasValue)
                {
                    throw new WebEasValidationException(null, "Pre príjmový účet nie je možné zapísať program.");
                }
            }

            return GetById<ZmenyRzpValView>(Update(data.ConvertToEntity()));
        }

        public void ChangeStateZmena(ChangeStateDto state)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    var rec = GetList<ZmenyRzpView>(x => x.D_NavrhZmenyRzp_Id == state.Id).SingleOrDefault();
                    ChangeState<NavrhZmenyRzp>(state.Id, state.IdNewState);

                    var historia = new EntitaHistoriaStavov
                    {
                        ZmenaStavuDatum = DateTime.Now,
                        C_StavEntity_Id_New = state.IdNewState,
                        C_StavEntity_Id_Old = rec.C_StavEntity_Id,
                        C_StavovyPriestor_Id = (int)StavovyPriestorEnum.RZP_NavrhUprava,
                        D_NavrhZmenyRzp_Id = rec.D_NavrhZmenyRzp_Id,
                        VyjadrenieSpracovatela = state.VyjadrenieSpracovatela
                    };

                    InsertData(historia);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new WebEasException("Nastala chyba pri zmene stavu", ex);
                }
            }
        }

        public long PocetOdoslanychZaznamovKDatumu(PocetOdoslanychZaznamovKDatumu request)
        {
            return Db.Scalar<long>("SELECT count(d_navrhzmenyrzp_id) FROM [rzp].[V_NavrhZmenyRzp] where Typ = 1 and C_StavEntity_Id = @stav and month(Datum) = @mesiac AND year(Datum) = @rok", new
                {
                    stav = StavEntityEnumRzp.ODOSLANY,
                    mesiac = request.Datum.Month,
                    rok = request.Datum.Year
                });
        }

        #endregion

    }
}
