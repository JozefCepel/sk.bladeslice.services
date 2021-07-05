using ServiceStack;
using ServiceStack.OrmLite;
using System.Linq;
using WebEas.Esam.ServiceModel.Office.Reg.Dto;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office.Reg
{
    /// <summary>
    ///
    /// </summary>
    public partial class RegService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public RegService(IRegRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new IRegRepository Repository
        {
            get
            {
                return (IRegRepository)this.repository;
            }
        }

        #region Globalne, ale musia byt v moduloch

        public object Any(ListDto request)
        {
            return base.GetList(request);
        }

        public object Get(ListComboDto request)
        {
            return this.Repository.GetListCombo(request);
        }

        public void Post(ChangeStateDto request)
        {
            this.Repository.ChangeState(request);
        }

        public object Post(GetTreeCountsDto request)
        {
            return base.GetTreeCounts(request);
        }

        #endregion

        #region Globalne, Long operations

        public object Post(RegLongOperationStartDto request)
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
            return Repository.LongOperationList(request);
        }

        #endregion

        #region StavovyPriestor

        public StavovyPriestorView Post(CreateStavovyPriestor request)
        {
            return Repository.Create<StavovyPriestorView>(request);
        }

        public StavovyPriestorView Any(UpdateStavovyPriestor request)
        {
            return Repository.Update<StavovyPriestorView>(request);
        }

        public void Any(DeleteStavovyPriestor request)
        {
            this.Repository.Delete<StavovyPriestor>(request.C_StavovyPriestor_Id);
        }

        public object Any(ListStavovyPriestor request)
        {
            return this.Repository.GetList<StavovyPriestorView>(request);
        }

        #endregion

        #region StavEntity

        public StavEntityView Post(CreateStavEntity request)
        {
            return Repository.Create<StavEntityView>(request);
        }

        public StavEntityView Any(UpdateStavEntity request)
        {
            return Repository.Update<StavEntityView>(request);
        }

        public void Any(DeleteStavEntity request)
        {
            this.Repository.Delete<StavEntity>(request.C_StavEntity_Id);
        }

        public object Any(ListStavEntity request)
        {
            return this.Repository.GetList<StavEntityView>(request);
        }

        #endregion

        #region StavEntityStavEntity

        public StavEntityStavEntityView Post(CreateStavEntityStavEntity request)
        {
            return this.Repository.Create<StavEntityStavEntityView>(request);
        }

        public StavEntityStavEntityView Any(UpdateStavEntityStavEntity request)
        {
            return this.Repository.Update<StavEntityStavEntityView>(request);
        }

        public void Any(DeleteStavEntityStavEntity request)
        {
            this.Repository.Delete<StavEntityStavEntity>(request.C_StavEntity_StavEntity_Id);
        }

        public object Any(ListStavEntityStavEntity request)
        {
            return this.Repository.GetList<StavEntityStavEntityView>(request);
        }

        #endregion

        #region TypBiznisEntity

        public object Any(ListTypBiznisEntity request)
        {
            return this.Repository.GetList<TypBiznisEntityView>(request);
        }

        #endregion

        #region TypBiznisEntityNastav
        public TypBiznisEntityNastavView Any(UpdateTypBiznisEntityNastav request)
        {
            return Repository.UpdateTypBiznisEntityNastav(request);
        }

        public void Any(RefreshDefault request)
        {
            this.Repository.RefreshDefaultTypBiznisEntityNastav(request);
        }

        public object Get(GetTypBiznisEntityNastavViewDto request)
        {
            if (request.C_TypBiznisEntity_Id.HasValue)
            {
                return Repository.GetTypBiznisEntityNastavView().Where(x => x.C_TypBiznisEntity_Id == request.C_TypBiznisEntity_Id);
            }

            return Repository.GetTypBiznisEntityNastavView();
        }

        #endregion

        public object Get(WebEas.ServiceModel.Dto.SessionInfo request)
        {
            return this.GetSessionInfo();
        }

        #region Nasledovny stav

        public object Post(CreateNasledovnyStavEntity request)
        {
            StavEntityView stav = request.ConvertTo<StavEntityView>();
            var naslStav = new NasledovnyStavEntity
            {
                C_StavovyPriestor_Id = request.C_StavovyPriestor_Id,
                C_StavEntity_Id_Parent = request.C_StavEntity_Id_Predchadzajuci,
                NasledovnyStav = stav
            };
            naslStav = this.Repository.Save(naslStav);
            NasledovnyStavEntityDto response = naslStav.NasledovnyStav.ConvertTo<NasledovnyStavEntityDto>();
            response.C_StavEntity_Id_Predchadzajuci = naslStav.C_StavEntity_Id_Parent;
            return response;
        }

        public object Any(ListNaslStavEntity request)
        {
            return new ListNaslStavEntityResponse
            {
                Result = this.Repository.GetListNaslStavEntity(request.C_StavEntity_Id).Select(x => x.ConvertTo<StavEntityDto>()).ToList()
            };
        }

        #endregion

        #region Translations

        public TranslationDictionary Any(CreateTranslationDictionary request)
        {
            return this.Repository.Create<TranslationDictionary>(request);
        }

        public TranslationDictionary Any(UpdateTranslationDictionary request)
        {
            TranslationDictionary data;
            if (!request.D_PrekladovySlovnik_Id.HasValue || request.D_PrekladovySlovnik_Id.Value == 0)
            {
                data = this.Repository.Create<TranslationDictionary>(request);
            }
            else
            {
                data = this.Repository.Update<TranslationDictionary>(request);
            }

            return new TranslationDictionary
            {
                D_PrekladovySlovnik_Id = data.D_PrekladovySlovnik_Id,
                ColumnName = data.ColumnName,
                Cs = data.Cs,
                De = data.De,
                En = data.En,
                Hu = data.Hu,
                Identifier = data.Identifier,
                ModulName = data.ModulName,
                Pl = data.Pl,
                PovodnaHodnota = request.PovodnaHodnota,
                Rom = data.Rom,
                Rue = data.Rue,
                TableName = data.TableName,
                Uk = data.Uk,
                UniqueIdentifier = request.UniqueIdentifier
            };
        }

        #endregion

        #region Logging

        public LoggingConfig Any(CreateLoggingConfig request)
        {
            var res = Repository.Save(request.ConvertToEntity());
            //invalidate cache
            this.Repository.ResetLoggingCache();
            return res;
        }

        public LoggingConfig Any(UpdateLoggingConfig request)
        {
            var res = Repository.Save(request.ConvertToEntity());
            //invalidate cache
            this.Repository.ResetLoggingCache();
            return res;
        }

        public object Any(DeleteLoggingConfig request)
        {
            var res = this.Repository.Delete<LoggingConfig>(request.C_LoggingConfig_Id);
            //invalidate cache
            this.Repository.ResetLoggingCache();
            return res;
        }

        #endregion

        #region Default row values

        public object Any(RowDefaultValues request)
        {
            return Repository.GetRowDefaultValues(request.code, request.masterCode, request.masterRowId);
        }

        #endregion

        #region Nastavenie

        public WebEas.ServiceModel.Types.NastavenieView Any(UpdateNastavenie request)
        {
            return Repository.UpdateNastavenie(request);
        }

        public object Any(GetNastavenieB request)
        {
            return new ResultResponse<bool> { Result = Repository.GetNastavenieB(request.Modul, request.Kod) };
        }

        public object Any(GetNastavenieI request)
        {
            return new ResultResponse<long> { Result = Repository.GetNastavenieI(request.Modul, request.Kod) };
        }

        public object Any(GetNastavenieS request)
        {
            return new ResultResponse<string> { Result = Repository.GetNastavenieS(request.Modul, request.Kod) };
        }

        public object Any(GetNastavenieD request)
        {
            return new ResultResponse<System.DateTime?> { Result = Repository.GetNastavenieD(request.Modul, request.Kod) };
        }

        #endregion

        #region Stredisko

        public StrediskoView Any(CreateStredisko request)
        {
            return Repository.CreateStredisko(request);
        }

        public StrediskoResult Any(UpdateStredisko request)
        {
            return Repository.UpdateStredisko(request);
        }

        public StrediskoResult Any(DeleteStredisko request)
        {
            return Repository.DeleteStredisko(request);
        }

        #endregion

        #region TypBiznisEntity_Kniha

        public TypBiznisEntity_KnihaView Any(CreateTypBiznisEntity_Kniha request)
        {
            return Repository.CreateTypBiznisEntity_Kniha(request);
        }

        public TypBiznisEntity_KnihaView Any(UpdateTypBiznisEntity_Kniha request)
        {
            return Repository.UpdateTypBiznisEntity_Kniha(request);
        }

        public void Any(DeleteTypBiznisEntity_Kniha request)
        {
            Repository.Delete<TypBiznisEntity_Kniha>(request.C_TypBiznisEntity_Kniha_Id);
        }

        #endregion

        #region TypBiznisEntity_ParovanieDef

        public TypBiznisEntity_ParovanieDefView Any(CreateTypBiznisEntity_ParovanieDef request)
        {
            return Repository.CreateTypBiznisEntity_ParovanieDef(request);
        }

        public TypBiznisEntity_ParovanieDefView Any(UpdateTypBiznisEntity_ParovanieDef request)
        {
            return Repository.UpdateTypBiznisEntity_ParovanieDef(request);
        }

        public void Any(DeleteTypBiznisEntity_ParovanieDef request)
        {
            Repository.Delete<TypBiznisEntity_ParovanieDef>(request.C_TypBiznisEntity_ParovanieDef_Id);
        }

        #endregion

        #region BiznisEntita_Parovanie

        public BiznisEntita_ParovanieView Any(CreateBiznisEntita_Parovanie request)
        {
            return Repository.CreateBiznisEntita_Parovanie(request);
        }

        public BiznisEntita_ParovanieView Any(UpdateBiznisEntita_Parovanie request)
        {
            return Repository.UpdateBiznisEntita_Parovanie(request);
        }

        public void Any(DeleteBiznisEntita_Parovanie request)
        {
            foreach (var id in request.D_BiznisEntita_Parovanie_Id)
            {   // Chceme HardDelete
                Repository.Db.Delete<BiznisEntita_Parovanie>(e => e.D_BiznisEntita_Parovanie_Id == id);
            }
        }

        #endregion

        #region BiznisEntita_Zaloha

        public BiznisEntita_ZalohaView Any(CreateBiznisEntita_Zaloha request)
        {
            return Repository.CreateBiznisEntita_Zaloha(request);
        }

        public BiznisEntita_ZalohaView Any(UpdateBiznisEntita_Zaloha request)
        {
            return Repository.UpdateBiznisEntita_Zaloha(request);
        }

        public void Any(DeleteBiznisEntita_Zaloha request)
        {
            foreach (var id in request.D_BiznisEntita_Zaloha_Id)
            {
                var zal = Repository.Db.Select(Repository.Db.From<BiznisEntita_ZalohaView>().Select(x => new {x.D_BiznisEntita_Id_FA, x.C_TypBiznisEntity_Id_FA, x.Rok}).Where(x => x.D_BiznisEntita_Zaloha_Id == id)).FirstOrDefault();
                // Chceme HardDelete
                Repository.Db.Delete<BiznisEntita_Zaloha>(e => e.D_BiznisEntita_Zaloha_Id == id);
                Repository.UpdateZalohaInHead(zal.D_BiznisEntita_Id_FA, zal.C_TypBiznisEntity_Id_FA ?? 0, zal.Rok);
            }
        }

        #endregion

        #region Pokladnica
        public PokladnicaView Any(CreatePokladnica request)
        {
            return Repository.CreatePokladnica(request);
        }

        public PokladnicaResult Any(UpdatePokladnica request)
        {
            return Repository.UpdatePokladnica(request);
        }

        public PokladnicaResult Any(DeletePokladnica request)
        {
            return Repository.DeletePokladnica(request);
        }
        #endregion

        #region BankaUcet
        public BankaUcetView Any(CreateBankaUcet request)
        {
            return Repository.CreateBankaUcet(request);
        }

        public BankaUcetResult Any(UpdateBankaUcet request)
        {
            return Repository.UpdateBankaUcet(request);
        }

        public BankaUcetResult Any(DeleteBankaUcet request)
        {
            return Repository.DeleteBankaUcet(request);
        }

        #endregion

        #region Projekt

        public ProjektView Any(CreateProjekt request)
        {
            var res = Repository.Create<ProjektView>(request);
            Repository.InvalidateTreeCountsForPath("reg-cis-prj");
            return res;
        }

        public ProjektView Any(UpdateProjekt request)
        {
            return Repository.Update<ProjektView>(request);
        }

        public void Any(DeleteProjekt request)
        {
            Repository.Delete<Projekt>(request.C_Projekt_Id);
            Repository.InvalidateTreeCountsForPath("reg-cis-prj");
        }

        #endregion

        #region Banka
        public BankaView Any(CreateBanka request)
        {
            return Repository.Create<BankaView>(request);
        }

        public BankaView Any(UpdateBanka request)
        {
            return Repository.Update<BankaView>(request);
        }

        public void Any(DeleteBanka request)
        {
            Repository.Delete<Banka>(request.C_Banka_Id);
        }
        #endregion

        #region MenaKurz
        public MenaKurzView Any(CreateMenaKurz request)
        {
            return Repository.Create<MenaKurzView>(request);
        }

        public MenaKurzView Any(UpdateMenaKurz request)
        {
            return Repository.Update<MenaKurzView>(request);
        }

        public void Any(DeleteMenaKurz request)
        {
            Repository.Delete<MenaKurz>(request.C_MenaKurz_Id);
        }
        #endregion

        #region Cislovanie
        public CislovanieView Any(UpdateCislovanie request)
        {
            return Repository.UpdateCislovanie(request);
        }

        public void Any(DeleteCislovanie request)
        {
            Repository.DeleteCislovanie(request.C_Cislovanie_Id);
        }
        #endregion

        public object Any(GetListStredisko request)
        {
            return this.Repository.GetListStredisko();
        }

        #region Typ
        public TypView Any(CreateTyp request)
        {
            return Repository.Create<TypView>(request);
        }

        public TypView Any(UpdateTyp request)
        {
            return Repository.Update<TypView>(request);
        }

        public void Any(DeleteTyp request)
        {
            Repository.Delete<Typ>(request.C_Typ_Id);
        }
        #endregion

        #region TypBiznisEntityTyp
        public TypBiznisEntityTypView Any(CreateTypBiznisEntityTyp request)
        {
            return Repository.Create<TypBiznisEntityTypView>(request);
        }

        public TypBiznisEntityTypView Any(UpdateTypBiznisEntityTyp request)
        {
            return Repository.Update<TypBiznisEntityTypView>(request);
        }

        public void Any(DeleteTypBiznisEntityTyp request)
        {
            Repository.Delete<TypBiznisEntityTyp>(request.C_TypBiznisEntity_Typ_Id);
        }
        #endregion

        #region Textacia
        public TextaciaView Any(CreateTextacia request)
        {
            if (request.RokOd > request.RokDo)
                throw new WebEasValidationException(null, "'Rok do' nesmie byť nižší ako 'Rok od'.");
            return Repository.Create<TextaciaView>(request);
        }

        public TextaciaView Any(UpdateTextacia request)
        {
            if (request.RokOd > request.RokDo)
                throw new WebEasValidationException(null, "'Rok do' nesmie byť nižší ako 'Rok od'.");
            return Repository.Update<TextaciaView>(request);
        }

        public void Any(DeleteTextacia request)
        {
            Repository.Delete<Textacia>(request.C_Textacia_Id);
        }
        #endregion

        #region TextaciaPol
        public TextaciaPolView Any(CreateTextaciaPol request)
        {
            return Repository.Create<TextaciaPolView>(request);
        }

        public TextaciaPolView Any(UpdateTextaciaPol request)
        {
            return Repository.Update<TextaciaPolView>(request);
        }

        public void Any(DeleteTextaciaPol request)
        {
            Repository.Delete<TextaciaPol>(request.C_TextaciaPol_Id);
        }
        #endregion

        #region DphSadzba
        public DphSadzbaView Any(CreateDphSadzba request)
        {
            return Repository.Create<DphSadzbaView>(request);
        }

        public DphSadzbaView Any(UpdateDphSadzba request)
        {
            return Repository.Update<DphSadzbaView>(request);
        }

        public void Any(DeleteDphSadzba request)
        {
            Repository.Delete<DphSadzba>(request.C_DphSadzba_Id);
        }
        #endregion

        public object Any(DKLVratCisloDto request)
        {
            return Repository.DKLGetCislo(request);
        }
    }
}
