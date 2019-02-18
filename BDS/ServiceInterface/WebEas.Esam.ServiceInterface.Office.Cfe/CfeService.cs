using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial class CfeService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CfeService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CfeService(ICfeRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new ICfeRepository Repository
        {
            get
            {
                return (ICfeRepository)repository;
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
            this.Repository.ChangeState(request);
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
    }
}
