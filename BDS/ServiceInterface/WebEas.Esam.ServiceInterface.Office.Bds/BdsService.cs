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

        #region D_PRI_0

        public object Any(CreateD_PRI_0 request)
        {
            return Repository.Create<D_PRI_0View>(request);
        }

        public object Any(UpdateD_PRI_0 request)
        {
            return Repository.Update<D_PRI_0View>(request);
        }

        public void Any(DeleteD_PRI_0 request)
        {
            Repository.Delete<D_PRI_0>(request.D_PRD_PRI_0_Id);
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
