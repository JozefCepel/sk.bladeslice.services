using ServiceStack;
using System.Linq;
using WebEas.Esam.ServiceModel.Office.Reg.Dto;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Dto;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Office.Egov.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office
{
    /// <summary>
    /// 
    /// </summary>    
    public partial class OfficeService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public OfficeService(IOfficeRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new IOfficeRepository Repository
        {
            get
            {
                return (IOfficeRepository)this.repository;
            }
        }

        #region Globalne, ale musia byt v moduloch

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
            this.Repository.ChangeState(request);
        }

        #endregion

        #region StavovyPriestor

        public object Post(CreateStavovyPriestor request)
        {
            return this.Repository.Create<StavovyPriestorView>(request);
        }

        public object Any(UpdateStavovyPriestor request)
        {
            return this.Repository.Update<StavovyPriestorView>(request);
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

        public object Post(CreateStavEntity request)
        {
            return this.Repository.Create<StavEntityView>(request);
        }

        public object Any(UpdateStavEntity request)
        {
            return this.Repository.Update<StavEntityView>(request);
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

        public object Post(CreateStavEntityStavEntity request)
        {
            return this.Repository.Create<StavEntityStavEntityView>(request);
        }

        public object Any(UpdateStavEntityStavEntity request)
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

        public object Get(WebEas.ServiceModel.Dto.SessionInfo request)
        {
            return this.GetSessionInfo();
        }

        public object Get(AppStatus.HealthCheckDto request)
        {
            return GetHealthCheck(request);
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

        public object Any(CreateTranslationDictionary request)
        {
            return this.Repository.Create<ColumnTranslation>(request);
        }

        public object Any(UpdateTranslationDictionary request)
        {
            ColumnTranslation data;
            if (!request.D_PrekladovySlovnik_Id.HasValue || request.D_PrekladovySlovnik_Id.Value == 0)
            {
                data = this.Repository.Create<ColumnTranslation>(request);
            }
            else
            {
                data = this.Repository.Update<ColumnTranslation>(request);
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

        public object Any(CreateLoggingConfig request)
        {
            var res = this.Repository.Save(request.ConvertToEntity());
            //invalidate cache
            this.Repository.ResetLoggingCache();
            return res;
        }

        public object Any(UpdateLoggingConfig request)
        {
            var res = this.Repository.Save(request.ConvertToEntity());
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


        #region Stredisko

        public object Any(CreateStredisko request)
        {
            return Repository.Create<StrediskoView>(request);
        }

        public object Any(UpdateStredisko request)
        {
            return Repository.Update<StrediskoView>(request);
        }

        public void Any(DeleteStredisko request)
        {
            Repository.Delete<Stredisko>(request.C_Stredisko_Id);
        }

        #endregion

        #region Projekt

        public object Any(CreateProjekt request)
        {
            return Repository.Create<ProjektView>(request);
        }

        public object Any(UpdateProjekt request)
        {
            return Repository.Update<ProjektView>(request);
        }

        public void Any(DeleteProjekt request)
        {
            Repository.Delete<Projekt>(request.C_Projekt_Id);
        }

        #endregion
    }
}
