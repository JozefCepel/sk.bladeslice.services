using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;
using ServiceStack.OrmLite.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using WebEas.Esam.DcomWs.IsoPla;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Crm.Types;
using WebEas.Esam.ServiceModel.Office.Dto;
using WebEas.Esam.ServiceModel.Office.Reg.Dto;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.Esam.ServiceModel.Urbis.Types;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office.Reg
{
    public partial class RegRepository : RepositoryBase, IRegRepository
    {
        #region Long Operations

        protected override void LongOperationProcess(WebEas.ServiceModel.Dto.LongOperationStartDtoBase request)
        {
            string operationParametersDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(request.OperationParameters));
            if (Enum.TryParse(request.OperationName, out OperationsList operation))
            {
                switch (operation)
                {
                    // Operacie
                    case OperationsList.SpracovatDoklad:
                        SpracujDoklad(operationParametersDecoded.FromJson<SpracovatDokladDto>(), request.ProcessKey, out string spracovatDokladReportId);
                        break;
                    case OperationsList.DoposlanieUhradDoDcomu:
                        //tu iba zneuzijem class na zistenie kodu polozky
                        DoposlanieUhrad(operationParametersDecoded.FromJson<ListComboDto>().KodPolozky, request.ProcessKey);
                        break;
                    case OperationsList.PredkontovatDoklad:
                        PredkontujDoklad(operationParametersDecoded.FromJson<PredkontovatDokladDto>(), request.ProcessKey, out string predkontovatDokladReportId); ;
                        break;
                    case OperationsList.SkontrolovatZauctovanie:
                        SkontrolujZauctovanie(operationParametersDecoded.FromJson<SkontrolovatZauctovanieDto>(), request.ProcessKey); ;
                        break;
                    case OperationsList.ZauctovatDoklad:
                        ZauctujDoklad(operationParametersDecoded.FromJson<ZauctovatDokladDto>(), request.ProcessKey, out string zauctujDokladReportId); ;
                        break;
                    case OperationsList.MigraciaPociatocnehoStavu:
                        MigraciaPociatocnehoStavu(operationParametersDecoded.FromJson<MigraciaStavovDto>(), request.ProcessKey); ;
                        break;

                    //Zostavy
                }
            }
            else
            {
                throw new WebEasException($"Long operation with the name {request.OperationName} is not implemented", "Operácia nie je implementovaná!");
            }
        }

        #endregion

        #region NasledovnyStavEntity

        public NasledovnyStavEntity Save(NasledovnyStavEntity entity)
        {
            using (var txScope = new TransactionScope())
            {
                long stavEntityId = InsertData(entity.NasledovnyStav);
                entity.C_StavEntity_Id_Child = (int)stavEntityId;
                InsertData(entity);

                txScope.Complete();
            }

            return entity;
        }

        public List<StavEntityView> GetListNaslStavEntity(int idStavEntity)
        {
            var filter = new Filter("C_StavEntity_Id_Parent", idStavEntity);
            List<NasledovnyStavEntity> naslStavy = this.GetList<NasledovnyStavEntity>(filter);

            filter = new Filter(FilterElement.In("C_StavEntity_Id", naslStavy.Select(nav => nav.C_StavEntity_Id_Child)));
            return this.GetList<StavEntityView>(filter);
        }

        #endregion

        #region Translations

        public ColumnTranslation Save(ColumnTranslation entity)
        {
            this.InsertData<ColumnTranslation>(entity);
            return entity;
        }

        #endregion

        #region Logging

        public LoggingConfig Save(LoggingConfig entity)
        {
            if (entity.C_LoggingConfig_Id == default(int))
            {
                entity.C_LoggingConfig_Id = (int)this.InsertData<LoggingConfig>(entity);
            }
            else
            {
                this.UpdateData<LoggingConfig>(entity);
            }
            return entity;
        }

        public void ResetLoggingCache()
        {
            base.RemoveFromCacheOptimizedTenant("db:columnslog");
        }

        #endregion // Logging

        #region GetRowDefaultValues
        public override object GetRowDefaultValues(string code, string masterCode, string masterRowId)
        {
            //Odkomentovať keď to chcem použiť
            var root = RenderModuleRootNode(code);
            var node = root.TryFindNode(code);
            //HierarchyNode masternode = null;
            //if (!masterCode.IsNullOrEmpty()) //Používať iba ak je modul z code a mastercode rovnaký
            //{
            //    masternode = root.TryFindNode(masterCode);
            //}

            #region Stredisko
            if (node != null && node.ModelType == typeof(StrediskoView))
            {
                var poradieMax = Db.Scalar<short>(Db.From<StrediskoView>().Select(x => new { Poradie = Sql.Max(x.Poradie) }));
                return new // StrediskoView()
                {
                    Poradie = (short)(poradieMax + 1)
                };
            }
            #endregion

            #region BankaUcet
            if (node != null && node.ModelType == typeof(BankaUcetView))
            {
                var poradieMax = Db.Scalar<byte>(Db.From<BankaUcetView>().Select(x => new { Poradie = Sql.Max(x.Poradie) }));
                return new // BankaUcetView()
                {
                    Poradie = (byte)(poradieMax + 1)
                };
            }
            #endregion

            #region Pokladnica
            if (node != null && node.ModelType == typeof(PokladnicaView))
            {
                var poradieMax = Db.Scalar<byte>(Db.From<PokladnicaView>().Select(x => new { Poradie = Sql.Max(x.Poradie) }));
                return new // PokladnicaView()
                {
                    Poradie = (byte)(poradieMax + 1)
                };
            }
            #endregion

            #region PredkontaciaUctView
            if (code == "uct-def-kont-konf-uct" && masterCode == "reg-ors-vbu" && !string.IsNullOrEmpty(masterRowId))
            {
                return new
                {
                    C_BankaUcet_Id = masterRowId,
                    SkupinaPredkont_Id = (SkupinaPredkontEnum.Bankove_vypisy),
                    Poradie = (byte)(1),
                    C_Typ_Id = (int)TypEnum.SumaKredit
                };
            }
            else if (code == "uct-def-kont-konf-uct" && masterCode == "reg-ors-pok" && !string.IsNullOrEmpty(masterRowId))
            {
                return new
                {
                    C_Pokladnica_Id = masterRowId,
                    SkupinaPredkont_Id = (SkupinaPredkontEnum.Pokladnicne_doklady),
                    Poradie = (byte)(1),
                    C_Typ_Id = (int)TypEnum.SumaDokladu
                };
            }
            else if (code == "uct-def-kont-konf-uct" && masterCode == "reg-ors-orj" && !string.IsNullOrEmpty(masterRowId))
            {
                return new
                {
                    C_Stredisko_Id = masterRowId,
                    SkupinaPredkont_Id = (SkupinaPredkontEnum.Odberatelia),
                    Poradie = (byte)(1)
                };
            }
            #endregion

            #region TextaciaPol
            if (node != null && node.ModelType == typeof(TextaciaPolView))
            {
                var poradieMax = Db.Scalar<short>(Db.From<TextaciaPolView>().Select(x => new { Poradie = Sql.Max(x.Poradie) }));
                return new // TextaciaPolView()
                {
                    C_Textacia_Id = masterRowId,
                    Poradie = (short)(poradieMax + 1)
                };
            }
            #endregion

            return base.GetRowDefaultValues(code, masterCode, masterRowId);
        }
        #endregion

        #region AccessFlag

        public override void SetAccessFlag(object viewData)
        {
            base.SetAccessFlag(viewData);

            if (viewData is IEnumerable<IOrsPravo>)
            {
                foreach (IOrsPravo be in viewData as IEnumerable)
                {
                    be.ApplyOrsPravoToAccesFlags();
                }
            }
        }

        #endregion

        #region UpdateTypBiznisEntityNastav
        public TypBiznisEntityNastavView UpdateTypBiznisEntityNastav(UpdateTypBiznisEntityNastav data)
        {
            UpdateTypBiznisEntityNastav(data.C_TypBiznisEntity_Id, (bool)data.StrediskoNaPolozke, (bool)data.ProjektNaPolozke,
                                        (bool)data.UctKluc1NaPolozke, (bool)data.UctKluc2NaPolozke, (bool)data.UctKluc3NaPolozke,
                                        (bool)data.EvidenciaDMS, (bool)data.EvidenciaSystem, (bool)data.CislovanieJedno,
                                        data.DatumDokladuTU, data.DatumDokladuEU, data.DatumDokladuDV, (bool)data.UctovatPolozkovite);
            SetCislovanie();
            SetPredkontacia();

            return GetList<TypBiznisEntityNastavView>(t => t.C_TypBiznisEntity_Id == data.C_TypBiznisEntity_Id).FirstOrDefault();

        }
        #endregion

        #region RenderCisTree
        public List<DatabaseHierarchyNode> RenderCisTree(string code, DatabaseHierarchyNode staticData)
        {
            var result = new List<DatabaseHierarchyNode>();
            var reader = Db.ExecuteReader($@"SELECT tbek.Nazov AS Item1, tbek.Kod as Item2, tbek.C_TypBiznisEntity_Kniha_Id as Item3
                                                FROM reg.V_TypBiznisEntity_Kniha tbek
                                                    LEFT JOIN reg.V_TypBiznisEntity tbe ON tbe.C_TypBiznisEntity_Id = tbek.C_TypBiznisEntity_Id
                                                    LEFT JOIN reg.V_TypBiznisEntityNastav tben ON tben.C_TypBiznisEntity_Id = tbek.C_TypBiznisEntity_Id
                                                Where tben.EvidenciaSystem = 1 AND tben.CislovanieJedno = 0 AND tbe.KodORS = '{code}'
                                                ORDER BY tbek.Poradie");
            var data = reader.Parse<Tuple<string, string, int>>().ToList();
            reader.Close();

            foreach (var item in data)
            {
                var node = staticData.Clone();
                node.Parameter = item.Item3; // (item.Item3 < 0)? (item.Item3 * -1) : item.Item3 + 1000;
                node.Nazov = $" Číslovanie - {item.Item1}";

                result.Add(node);
            }

            return result;
        }
        #endregion

        #region RefreshDefaultTypBiznisEntityNastav
        public void RefreshDefaultTypBiznisEntityNastav(RefreshDefault data)
        {
            RefreshDefaultTypBiznisEntityNastav(data.IDs);
            SetCislovanie();
            SetPredkontacia();
        }
        #endregion

        #region Kniha
        public TypBiznisEntity_KnihaView CreateTypBiznisEntity_Kniha(CreateTypBiznisEntity_Kniha data)
        {
            TypBiznisEntity_KnihaView result = Create<TypBiznisEntity_KnihaView>(data);
            SetCislovanie();
            SetPredkontacia();
            return result;
        }

        public TypBiznisEntity_KnihaView UpdateTypBiznisEntity_Kniha(UpdateTypBiznisEntity_Kniha data)
        {
            TypBiznisEntity_KnihaView result = Update<TypBiznisEntity_KnihaView>(data);
            SetCislovanie();
            SetPredkontacia();
            return result;
        }
        #endregion

        #region TypBiznisEntity_ParovanieDef
        public TypBiznisEntity_ParovanieDefView CreateTypBiznisEntity_ParovanieDef(CreateTypBiznisEntity_ParovanieDef data)
        {
            TypBiznisEntity_ParovanieDefView result = Create<TypBiznisEntity_ParovanieDefView>(data);
            return result;
        }

        public TypBiznisEntity_ParovanieDefView UpdateTypBiznisEntity_ParovanieDef(UpdateTypBiznisEntity_ParovanieDef data)
        {
            TypBiznisEntity_ParovanieDefView result = Update<TypBiznisEntity_ParovanieDefView>(data);
            return result;
        }
        #endregion

        #region BiznisEntita_Parovanie
        public BiznisEntita_ParovanieView CreateBiznisEntita_Parovanie(CreateBiznisEntita_Parovanie data)
        {
            BiznisEntita_Parovanie par;
            par = data.ConvertToEntity();
            SetParovanieIdentifikatorRok(par);
            InsertData(par);
            var result = GetById<BiznisEntita_ParovanieView>(par.D_BiznisEntita_Parovanie_Id);
            return result;
        }

        public BiznisEntita_ParovanieView UpdateBiznisEntita_Parovanie(UpdateBiznisEntita_Parovanie data)
        {
            BiznisEntita_ParovanieView result = Update<BiznisEntita_ParovanieView>(data);
            return result;
        }
        #endregion

        #region BiznisEntita_Zaloha
        public BiznisEntita_ZalohaView CreateBiznisEntita_Zaloha(CreateBiznisEntita_Zaloha data)
        {
            BiznisEntita_Zaloha par;
            par = data.ConvertToEntity();
            var be = GetById<BiznisEntita>(data.D_BiznisEntita_Id_FA);
            if (be != null) par.Rok = be.Rok; // zober rok z dokladu
            InsertData(par);
            UpdateZalohaInHead(be.D_BiznisEntita_Id, be.C_TypBiznisEntity_Id, be?.Rok ?? par.Rok);
            var result = GetById<BiznisEntita_ZalohaView>(par.D_BiznisEntita_Zaloha_Id);
            return result;
        }

        public BiznisEntita_ZalohaView UpdateBiznisEntita_Zaloha(UpdateBiznisEntita_Zaloha data)
        {
            BiznisEntita_Zaloha par;
            par = data.ConvertToEntity();
            var be = GetById<BiznisEntita>(data.D_BiznisEntita_Id_FA);
            if (be != null) par.Rok = be.Rok; // zober rok z dokladu
            UpdateData(par);
            UpdateZalohaInHead(be.D_BiznisEntita_Id, be.C_TypBiznisEntity_Id, be?.Rok ?? par.Rok);
            var result = GetById<BiznisEntita_ZalohaView>(par.D_BiznisEntita_Zaloha_Id);
            return result;
        }

        public void UpdateZalohaInHead(long id, int typ, short rok)
        {
            var zalPol = GetList(Db.From<BiznisEntita_ZalohaView>().Select(x => new { x.DM_Cena, x.VS }). //View  berie iba platne zaznamy
                Where(e => e.D_BiznisEntita_Id_FA == id && e.Rok == rok));   //Rok je filtrovaný iba kvôli performance na partícii

            //foreach (var zal in zalPol)
            //{
            //    suma += zal.DM_Cena;
            //    if (cislo != "") cislo += ", ";
            //    cislo += zal.VS;
            //}

            decimal suma = zalPol.Sum(x => x.DM_Cena);
            string cislo = zalPol.OrderBy(o => o.VS).Select(x => x.VS).Distinct().Join(", ");

            if (typ == (int)TypBiznisEntityEnum.DFA)
            {
                var dfa = GetById<CrmDokladDFA>(id);
                if (dfa != null)
                {
                    dfa.DM_SumaZal = suma;
                    dfa.CisloFAK = cislo;
                    Update(dfa);
                }
            }
            else if (typ == (int)TypBiznisEntityEnum.OFA)
            {
                var ofa = GetById<CrmDokladOFA>(id);
                if (ofa != null)
                {
                    ofa.DM_SumaZal = suma;
                    ofa.CisloFAK = cislo;
                    Update(ofa);
                }
            }

            // na FE sa potom refreshne master grid
        }

        #endregion

        #region Stredisko

        public StrediskoView CreateStredisko(CreateStredisko data)
        {
            var dcomRezim = GetNastavenieI("reg", "eSAMRezim") == 1;
            KontrolaStredisko(data.Kod, dcomRezim);
            StrediskoView result = Create<StrediskoView>(data);
            SetCislovanie();
            SetPredkontacia(); //V tento moment sa pri pridaní strediska setnú aj predkontácie. Ide hlavne o prvý záznam.
            InvalidateTreeCountsForPath("reg-ors-orj");
            return result;
        }

        public StrediskoView UpdateStredisko(UpdateStredisko data)
        {
            bool dcomRezim = GetNastavenieI("reg", "eSAMRezim") == 1;
            KontrolaStredisko(data.Kod, dcomRezim);
            var result = Update<StrediskoView>(data);
            SetCislovanie();

            if (result.DCOM.GetValueOrDefault() && dcomRezim)
            {
                using var client = new PlatbyClient();
                var dcmHeader = new DcmHeader
                {
                    tenantId = Session.D_Tenant_Id_Externe?.ToString() ?? throw new ArgumentException("nie je zadefinovane D_Tenant_Id_Externe"),
                    isoId = Session.IsoId,
                    requestId = WebEas.Context.Current.CurrentCorrelationID.ToString()
                };
                UpdateStrediskoDcom(null, new List<StrediskoView> { result }, client, ref dcmHeader);
            }

            return result;
        }

        public void DeleteStredisko(DeleteStredisko data)
        {
            var eSamRezim = GetNastavenieI("reg", "eSAMRezim");

            if (eSamRezim == 1)
            {
                var strediska = GetList(Db.From<StrediskoView>().Where(x => data.C_Stredisko_Id.Contains(x.C_Stredisko_Id)));
                strediska.RemoveAll(x => !x.DCOM.GetValueOrDefault());
                using var client = new PlatbyClient();
                var dcmHeader = new DcmHeader
                {
                    tenantId = Session.D_Tenant_Id_Externe?.ToString() ?? throw new ArgumentException("nie je zadefinovane D_Tenant_Id_Externe"),
                    isoId = Session.IsoId,
                    requestId = WebEas.Context.Current.CurrentCorrelationID.ToString()
                };
                UpdateStrediskoDcom(null, strediska, client, ref dcmHeader, delete: true);
            }

            Delete<StrediskoCis>(data.C_Stredisko_Id);
            InvalidateTreeCountsForPath("reg-ors-orj");
        }

        #endregion

        #region Pokladnica


        public PokladnicaView CreatePokladnica(CreatePokladnica data)
        {
            var dcomRezim = GetNastavenieI("reg", "eSAMRezim") == 1;
            KontrolaPokladnica(data.Kod, dcomRezim);
            PokladnicaView result = Create<PokladnicaView>(data);
            SetCislovanie();
            InvalidateTreeCountsForPath("reg-ors-pok");
            return result;
        }

        public PokladnicaView UpdatePokladnica(UpdatePokladnica data)
        {
            var dcomRezim = GetNastavenieI("reg", "eSAMRezim") == 1;
            KontrolaPokladnica(data.Kod, dcomRezim);
            PokladnicaView result = Update<PokladnicaView>(data);
            SetCislovanie();

            if (result.DCOM.GetValueOrDefault() && dcomRezim)
            {
                using var client = new PlatbyClient();
                var dcmHeader = new DcmHeader
                {
                    tenantId = Session.D_Tenant_Id_Externe?.ToString() ?? throw new ArgumentException("nie je zadefinovane D_Tenant_Id_Externe"),
                    isoId = Session.IsoId,
                    requestId = WebEas.Context.Current.CurrentCorrelationID.ToString()
                };
                UpdatePokladnicaDcom(null, new List<PokladnicaView> { result }, client, ref dcmHeader);
            }

            return result;
        }

        public void DeletePokladnica(DeletePokladnica data)
        {
            var eSamRezim = GetNastavenieI("reg", "eSAMRezim");

            if (eSamRezim == 1)
            {
                var pokladnice = GetList(Db.From<PokladnicaView>().Where(x => data.C_Pokladnica_Id.Contains(x.C_Pokladnica_Id)));
                pokladnice.RemoveAll(x => !x.DCOM.GetValueOrDefault());
                using var client = new PlatbyClient();
                var dcmHeader = new DcmHeader
                {
                    tenantId = Session.D_Tenant_Id_Externe?.ToString() ?? throw new ArgumentException("nie je zadefinovane D_Tenant_Id_Externe"),
                    isoId = Session.IsoId,
                    requestId = WebEas.Context.Current.CurrentCorrelationID.ToString()
                };
                UpdatePokladnicaDcom(null, pokladnice, client, ref dcmHeader, delete: true);
            }

            Delete<Pokladnica>(data.C_Pokladnica_Id);
            InvalidateTreeCountsForPath("reg-ors-pok");
        }

        #endregion

        #region BankaUcet
        public BankaUcetView CreateBankaUcet(CreateBankaUcet data)
        {
            data.IBAN = string.Join("", data.IBAN.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

            BankaUcetView result = Create<BankaUcetView>(data);
            SetCislovanie();
            InvalidateTreeCountsForPath("reg-ors-vbu");
            return result;
        }

        public BankaUcetView UpdateBankaUcet(UpdateBankaUcet data)
        {
            data.IBAN = string.Join("", data.IBAN.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            var result = Update<BankaUcetView>(data);
            SetCislovanie();

            if (result.DCOM.GetValueOrDefault() && GetNastavenieI("reg", "eSAMRezim") == 1)
            {
                using var client = new PlatbyClient();
                var dcmHeader = new DcmHeader
                {
                    tenantId = Session.D_Tenant_Id_Externe?.ToString() ?? throw new ArgumentException("nie je zadefinovane D_Tenant_Id_Externe"),
                    isoId = Session.IsoId,
                    requestId = WebEas.Context.Current.CurrentCorrelationID.ToString()
                };
                UpdateBankaUcetDcom(null, new List<BankaUcetView> { result }, client, ref dcmHeader);
            }

            return result;
        }

        public void DeleteBankaUcet(DeleteBankaUcet data)
        {
            var eSamRezim = GetNastavenieI("reg", "eSAMRezim");

            if (eSamRezim == 1)
            {
                var bankaUcty = GetList(Db.From<BankaUcetView>().Where(x => data.C_BankaUcet_Id.Contains(x.C_BankaUcet_Id)));
                bankaUcty.RemoveAll(x => !x.DCOM.GetValueOrDefault());
                using var client = new PlatbyClient();
                var dcmHeader = new DcmHeader
                {
                    tenantId = Session.D_Tenant_Id_Externe?.ToString() ?? throw new ArgumentException("nie je zadefinovane D_Tenant_Id_Externe"),
                    isoId = Session.IsoId,
                    requestId = WebEas.Context.Current.CurrentCorrelationID.ToString()
                };
                UpdateBankaUcetDcom(null, bankaUcty, client, ref dcmHeader, delete: true);
            }

            Delete<BankaUcetCis>(data.C_BankaUcet_Id);
            InvalidateTreeCountsForPath("reg-ors-vbu");
        }

        #endregion

        #region Cislovanie
        public CislovanieView UpdateCislovanie(UpdateCislovanie data)
        {
            CislovanieView result = Update<CislovanieView>(data);
            SetCislovanie();
            return result;
        }

        public void DeleteCislovanie(int[] C_Cislovanie_Id)
        {
            Delete<CislovanieCis>(C_Cislovanie_Id);
            SetCislovanie();
        }

        #endregion

        #region InicialnyImport

        private void MigraciaPociatocnehoStavu(MigraciaStavovDto request, string processKey)
        {
            request.DatumDo = CheckESAMStartDate(request.DatumDo);

            int nastavenieISOZdroj = (int)GetNastavenieI("reg", "ISOZdroj");
            string sConnString = GetNastavenieS("reg", "ISOZdrojDatabaza");
            if (request.TypImportu.Contains("reg-str"))
            {
                LongOperationSetStateMessage(processKey, "Prebieha migrácia stredísk (1/4)");
                GetCiselnikStrediskoExt(request, nastavenieISOZdroj, sConnString);
            }
            if (request.TypImportu.Contains("reg-ban"))
            {
                LongOperationSetStateMessage(processKey, "Prebieha migrácia bankových účtov (2/4)");
                GetCiselnikBankaUcetExt(request, nastavenieISOZdroj, sConnString);
            }
            if (request.TypImportu.Contains("reg-pdk"))
            {
                LongOperationSetStateMessage(processKey, "Prebieha migrácia pokladníc (3/4)");
                GetCiselnikPokladnicaExt(request, nastavenieISOZdroj, sConnString);
            }
            if (request.TypImportu.Contains("reg-proj"))
            {
                LongOperationSetStateMessage(processKey, "Prebieha migrácia projektov (4/4)");
                GetCiselnikProjektExt(request, nastavenieISOZdroj, sConnString);
            }
            SetCislovanie();
            SetPredkontacia(); // pregeneruj/vygeneruj uct.C_Predkontacia
            InvalidateTreeCountsForPath("reg-.*");
            LongOperationSetStateFinished(processKey, string.Empty, "Migrácia modulu 'Registre' prebehla úspešne.", state: LongOperationState.Done);
        }

        private void GetCiselnikStrediskoExt(MigraciaStavovDto data, int nastavenieISOZdroj, string sConnString)
        {
            string sSql;
            DateTime datumK = (DateTime)data.DatumDo;

            if (nastavenieISOZdroj == 1)
            {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        using (IDbConnection cn = GetNewDb(sConnString, nastavenieISOZdroj))
                        {
                            sSql = Properties.Resources.Korwin_STR.Replace("DB_FROM", cn.Database);
                            sSql = sSql.Replace("2019", datumK.Year.ToString());
                            Db.ExecuteNonQuery(sSql);
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new WebEasException(null, "Nastala chyba pri importe stredísk", ex);
                    }
                }
            }
            else if (nastavenieISOZdroj == 2)
            {
                var roky = Enumerable.Range(datumK.Year, DateTime.Now.Year - datumK.Year + 1).ToList();
                foreach (int rok in roky)
                {
                    using (var tran = BeginTransaction())
                    {
                        try
                        {
                            using (IDbConnection cn = GetNewDb(sConnString, nastavenieISOZdroj))
                            {
                                if (cn.TableExists("uco_stred" + rok.ToString()))
                                {
                                    sSql = Properties.Resources.Urbis_STR.Replace("DB_FROM", cn.Database).Replace("@YEAR", rok.ToString());
                                    Db.ExecuteNonQuery(sSql);
                                }
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new WebEasException(null, "Nastala chyba pri importe stredísk za rok " + rok.ToString(), ex);
                        }
                    }
                }
            }
            SetCislovanie();
        }

        private void GetCiselnikPokladnicaExt(MigraciaStavovDto data, int nastavenieISOZdroj, string sConnString)
        {
            string sSql;
            DateTime datumK = (DateTime)data.DatumDo;

            if (nastavenieISOZdroj == 1)
            {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        using (IDbConnection cn = GetNewDb(sConnString, nastavenieISOZdroj))
                        {
                            sSql = Properties.Resources.Korwin_POK.Replace("DB_FROM", cn.Database);
                            sSql = sSql.Replace("2019", datumK.Year.ToString());
                            Db.ExecuteNonQuery(sSql);
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new WebEasException(null, "Nastala chyba pri importe pokladníc za rok " + datumK.Year.ToString(), ex);
                    }
                 }
            }
            else if (nastavenieISOZdroj == 2)
            {
                var roky = Enumerable.Range(datumK.Year, DateTime.Now.Year - datumK.Year + 1).ToList();
                foreach (int rok in roky)
                {
                    using (var tran = BeginTransaction())
                    {
                        try
                        {
                            using (IDbConnection cn = GetNewDb(sConnString, nastavenieISOZdroj))
                            {
                                if (cn.TableExists("uco_pokdef" + rok.ToString()))
                                {
                                sSql = Properties.Resources.Urbis_POK.Replace("DB_FROM", cn.Database).Replace("@YEAR", rok.ToString());
                                Db.ExecuteNonQuery(sSql);
                                }
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new WebEasException(null, "Nastala chyba pri importe pokladníc za rok " + rok.ToString(), ex);
                        }
                    }
                }
            }
            SetCislovanie();
        }

        private void GetCiselnikBankaUcetExt(MigraciaStavovDto data, int nastavenieISOZdroj, string sConnString)
        {
            DateTime datumK = (DateTime)data.DatumDo;

            if (nastavenieISOZdroj == 1)
            {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        using (IDbConnection cn = GetNewDb(sConnString, nastavenieISOZdroj))
                        {
                            string sSql = Properties.Resources.Korwin_VBU.Replace("DB_FROM", cn.Database);
                            sSql = sSql.Replace("2019", datumK.Year.ToString());
                            Db.ExecuteNonQuery(sSql);
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new WebEasException(null, "Nastala chyba pri importe bankových účtov za rok " + datumK.Year.ToString(), ex);
                    }
                }
            }
            else if (nastavenieISOZdroj == 2)
            {
                var roky = Enumerable.Range(datumK.Year, DateTime.Now.Year - datumK.Year + 1).ToList();
                foreach (int rok in roky)
                {
                    using (var tran = BeginTransaction())
                    {
                        try
                        {
                            using (IDbConnection cn = GetNewDb(sConnString, nastavenieISOZdroj))
                            {
                                if (cn.TableExists("uco_bandef" + rok.ToString()))
                                {
                                string sSql = Properties.Resources.Urbis_VBU.Replace("DB_FROM", cn.Database).Replace("@YEAR", rok.ToString());
                                Db.ExecuteNonQuery(sSql);
                            }
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new WebEasException(null, "Nastala chyba pri importe bankových účtov za rok " + rok.ToString(), ex);
                        }
                    }
                }
            }
            SetCislovanie();
        }

        private void GetCiselnikProjektExt(MigraciaStavovDto data, int nastavenieISOZdroj, string sConnString)
        {
            DateTime datumK = (DateTime)data.DatumDo;

            if (nastavenieISOZdroj == 1)
            {
                using (var tran = BeginTransaction())
                {
                    try
                    {
                        using (IDbConnection cn = GetNewDb(sConnString, nastavenieISOZdroj))
                        {
                            string sSql = Properties.Resources.Korwin_PRJ.Replace("DB_FROM", cn.Database);
                            sSql = sSql.Replace("2019", datumK.Year.ToString());
                            Db.ExecuteNonQuery(sSql);
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new WebEasException(null, "Nastala chyba pri importe projektov za rok " + datumK.Year.ToString(), ex);
                    }
                }
            }
            else if (nastavenieISOZdroj == 2)
            {
                throw new WebEasException(null, "Funkcia migrácie projektov sa pripravuje.");
            }
        }

        #endregion

        public List<StrediskoView> GetListStredisko()
        {
            List<StrediskoView> list = new List<StrediskoView>();
            list = Db.Select<StrediskoView>().ToList();
            return list;
        }

        public List<DKLVratCisloResponseDto> DKLGetCislo(DKLVratCisloDto request)
        {
            if (request.NumberChar == "0" || request.NumberChar == "X")
            {
                int? counter = GetCisloDokladu(request.Be.DatumDokladu, request.Be, request.NumberChar, out string cisloDokladu, out string vs);

                return new List<DKLVratCisloResponseDto>()
                {
                    new DKLVratCisloResponseDto
                    {
                        CisloDokladu = cisloDokladu,
                        Cislo = request.NumberChar == "X" ? null : counter,
                        VS = vs
                    }
                };
            }
            return GetChybajuceCisloDokladu(request.Be.DatumDokladu, request.Be).Select(x => new DKLVratCisloResponseDto { Cislo = x.Cislo, CisloDokladu = x.CisloDokladu, VS = x.VS }).ToList();
        }
    }
}
