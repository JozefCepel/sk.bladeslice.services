using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOfficeRepository : IRepositoryBase,
        ISave<NasledovnyStavEntity>,
        ISave<ColumnTranslation>,
        ISave<LoggingConfig>
    {
        List<StavEntityView> GetListNaslStavEntity(int idStavEntity);
        void ResetLoggingCache();
    }
}