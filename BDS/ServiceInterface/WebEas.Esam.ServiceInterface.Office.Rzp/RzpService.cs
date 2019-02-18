using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Rzp.Dto;
using WebEas.Esam.ServiceModel.Office.Rzp.Types;
using WebEas.ServiceInterface;

namespace WebEas.Esam.ServiceInterface.Office.Rzp
{
    public partial class RzpService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RzpService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public RzpService(IRzpRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new IRzpRepository Repository
        {
            get
            {
                return (IRzpRepository)repository;
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
            if (Modules.FindNode(request.ItemCode).ModelType == typeof(NavrhRzpView))
            {
                Repository.ChangeStateNavrh(request);
            }
            else if (Modules.FindNode(request.ItemCode).ModelType == typeof(ZmenyRzpView))
            {
                Repository.ChangeStateZmena(request);
            }
            else
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
            return Repository.LongOperationStart(request.OperationName, request.OperationParameters, request.OperationInfo);
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

        #region DefPrrProgramovyRozpocetView

        private void RemoveProgramFromLocalCache()
        {
            var key = $"ten:{Repository.Session.TenantId}:pfe:ProgramovyRozpocet";
            Repository.Cache.Remove(key);
        }

        public object Any(CreateDefPrrProgramovyRozpocet request)
        {
            RemoveProgramFromLocalCache();
            return Repository.CreateDefPrrProgramovyRozpocet(request);
        }

        public object Any(UpdateDefPrrProgramovyRozpocet request)
        {
            RemoveProgramFromLocalCache();
            return Repository.UpdateDefPrrProgramovyRozpocet(request);
        }
        public void Any(DeleteDefPrrProgramovyRozpocet request)
        {
            RemoveProgramFromLocalCache();
            Repository.Delete<Program>(request.D_Program_Id);
        }

        #endregion

        #region Ciele

        public object Any(CreateCiele request)
        {
            return Repository.Create<CieleView>(request);
        }

        public object Any(UpdateCiele request)
        {
            return Repository.Update<CieleView>(request);
        }

        public void Any(DeleteCiele request)
        {
            Repository.Delete<Ciele>(request.D_PRCiele_Id);
        }

        #endregion

        #region CieleUkaz

        public object Any(CreateCieleUkaz request)
        {
            return Repository.Create<CieleUkazView>(request);
        }

        public object Any(UpdateCieleUkaz request)
        {
            return Repository.Update<CieleUkazView>(request);
        }

        public void Any(DeleteCieleUkaz request)
        {
            Repository.Delete<CieleUkaz>(request.D_PRCieleUkaz_Id);
        }

        #endregion

        #region Rozpočtové položky

        public object Any(CreateRzpPolozky request)
        {
            return Repository.Create<RzpPolozkyView>(request);
        }

        public object Any(UpdateRzpPolozky request)
        {
            return Repository.Update<RzpPolozkyView>(request);
        }

        public void Any(DeleteRzpPolozky request)
        {
            Repository.Delete<RzpPolozky>(request.C_RzpPolozky_Id);
        }

        #endregion

        #region Návrhy rozpočtu

        public object Any(CreateNavrhZmenyRzp request)
        {
            return Repository.Create<ZmenyRzpView>(request);
        }

        public object Any(UpdateNavrhZmenyRzp request)
        {
            return Repository.Update<ZmenyRzpView>(request);
        }

        public void Any(DeleteNavrhZmenyRzp request)
        {
            Repository.Delete<NavrhZmenyRzp>(request.D_NavrhZmenyRzp_Id);
        }

        public object Any(PocetOdoslanychZaznamovKDatumu request)
        {
            return Repository.PocetOdoslanychZaznamovKDatumu(request);
        }

        public void Post(PrevziatNavrhRozpoctuDto request)
        {
            Repository.PrevziatNavrhRozpoctu(request);
        }

        #endregion

        #region Položky návrhov

        public object Any(CreateNavrhyRzpVal request)
        {
            return Repository.Create(request);
        }

        public object Any(UpdateNavrhyRzpVal request)
        {
            return Repository.Update(request);
        }

        public void Any(DeleteNavrhyRzpVal request)
        {
            Repository.Delete<NavrhyRzpVal>(request.D_NavrhyRzpVal_Id);
        }

        #endregion

        #region Položky zmien

        public object Any(CreateZmenyRzpVal request)
        {
            return Repository.Create(request);
        }

        public object Any(UpdateZmenyRzpVal request)
        {
            return Repository.Update(request);
        }

        public void Any(DeleteZmenyRzpVal request)
        {
            Repository.Delete<ZmenyRzpVal>(request.D_ZmenyRzpVal_Id);
        }

        #endregion

        #region IntDoklad

        public object Any(CreateIntDoklad request)
        {
            return Repository.Create(request);
        }

        public object Any(UpdateIntDoklad request)
        {
            return Repository.Update(request);
        }

        public void Any(DeleteIntDoklad request)
        {
            Repository.Delete<IntDoklad>(request.D_IntDoklad_Id);
        }

        #endregion

        #region DennikRzp

        public object Any(CreateDennikRzp request)
        {
            return Repository.Create<DennikRzpView>(request);
        }

        public object Any(UpdateDennikRzp request)
        {
            return Repository.Update<DennikRzpView>(request);
        }

        public void Any(DeleteDennikRzp request)
        {
            Repository.Delete<DennikRzp>(request.D_DennikRzp_Id);
        }

        #endregion

        #region Nastavenie

        public object Any(GetParameterType request)
        {
            return Repository.GetParameterTypeRzp(request);
        }

        public object Any(UpdateNastavenie request)
        {
            return Repository.UpdateNastavenieRzp(request);
        }

        #endregion
    }
}
