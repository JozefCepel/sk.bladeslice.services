using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Bds.Dto;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public partial interface IBdsRepository : IRepositoryBase
    {

        #region Nastavenie

        object GetParameterTypeBds(GetParameterType data);

        /// <summary>
        /// Update nastavenie
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        long UpdateNastavenieBds(UpdateNastavenie data);

        #endregion


    }
}
