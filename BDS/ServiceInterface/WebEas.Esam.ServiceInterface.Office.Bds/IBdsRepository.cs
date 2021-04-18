using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Bds.Dto;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public partial interface IBdsRepository : IRepositoryBase
    {


        /// <summary>
        /// Vybaví doklady príjemka
        /// </summary>
        /// <param name="request"></param>
        //List<long> GetVybavDoklady(GetVybavDokladyReq request);

        /// <summary>
        /// Odvybaví doklady príjemka
        /// </summary>
        /// <param name="request"></param>
        //List<long> GetOdvybavDoklady(GetOdvybavDokladyReq request);

        V_PRI_0View CreateD_PRI_0(CreateD_PRI_0 data);
        V_VYD_0View CreateD_VYD_0(CreateD_VYD_0 data);

    }
}
