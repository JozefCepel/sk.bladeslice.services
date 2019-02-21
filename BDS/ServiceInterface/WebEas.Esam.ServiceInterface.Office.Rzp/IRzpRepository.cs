using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Rzp.Dto;
using WebEas.Esam.ServiceModel.Office.Rzp.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office.Rzp
{
    public partial interface IRzpRepository : IRepositoryBase,
        ICreate<IntDokladView, CreateIntDoklad>,
        ICreate<NavrhyRzpValView, CreateNavrhyRzpVal>,
        ICreate<ZmenyRzpValView, CreateZmenyRzpVal>,
        IUpdate<IntDokladView, UpdateIntDoklad>,
        IUpdate<NavrhyRzpValView, UpdateNavrhyRzpVal>,
        IUpdate<ZmenyRzpValView, UpdateZmenyRzpVal>
    {
        #region Program

        ProgramView CreateDefPrrProgramovyRozpocet(CreateDefPrrProgramovyRozpocet request);
        ProgramView UpdateDefPrrProgramovyRozpocet(UpdateDefPrrProgramovyRozpocet request);

        #endregion

        #region Návrhy rozpočtu

        void ChangeStateNavrh(ChangeStateDto state);
        void PrevziatNavrhRozpoctu(PrevziatNavrhRozpoctuDto request);

        #endregion

        #region Zmeny rozpočtu

        void ChangeStateZmena(ChangeStateDto state);
        long PocetOdoslanychZaznamovKDatumu(PocetOdoslanychZaznamovKDatumu request);

        #endregion

        #region Nastavenie

        object GetParameterTypeRzp(GetParameterType data);

        /// <summary>
        /// Update nastavenie
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        long UpdateNastavenieRzp(UpdateNastavenie data);

        #endregion


    }
}
