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
            if (Modules.FindNode(request.ItemCode).ModelType == typeof(NavrhBdsView))
            {
                //Repository.ChangeStateNavrh(request);
            }
            else if (Modules.FindNode(request.ItemCode).ModelType == typeof(ZmenyBdsView))
            {
                //Repository.ChangeStateZmena(request);
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
