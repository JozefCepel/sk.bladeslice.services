using System.Collections.Generic;
using System.IO;
using WebEas.Egov.Reports;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Pfe.Dto;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Pfe.Types;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Pfe
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IPfeRepository : IRepositoryBase
    {
        IList<PohladList> GetPohlady(string kodPolozky);

        PohladView GetPohlad(int id);

        object GetPohlad(GetPohlad request);

        PohladView GetPohladModel(int id);

        PohladView SavePohlad(Pohlad pohlad);

        PohladView CreatePohlad(Pohlad pohlad);

        PohladView CreatePohlad(CreatePohlad request, PohladActions source = null);

        PohladView UpdatePohlad(Pohlad pohlad);

        void DeletePohlad(int id);

        List<PossibleStateResponse> ListPossibleStates(int idState);

        List<PohladView> UnLockPohlad(int id, bool zamknut);

        List<PohladItem> SelectedViewItems(string kodPolozky);

        //string GetBussinessXml(int textationId);

        //Zastarale, uz sa nepouziva
        //string GetSubmissionFormUrl(long dPodanieId);
        //string GetDecisionFormUrl(long dEntitaHistoriaStavovId);

        List<FileUploadResponse> FileUpload(Dictionary<string, Stream> fileList, FileUpload fileUpload);

        ContextUser GetContextUser(string moduleShortcut);

        PfeLayout FillLayoutPagesTitle(PfeLayout layout);

        RendererResult ExportGrid(string title, string xml);

        List<TranslationColumnEntity> GetTranslationColumns(string module);

        List<TranslationDictionary> GetTraslatedExpressions(string uniqueKey);        

        RendererResult GenerateMergeScriptGlobalViews();

        List<LogView> PreviewLog(string identifier);

        List<LogView> PreviewLogCorId(string identifier);

        PollerReceiveResponse PollerReceive(string tenantId);

        void LogRequestDuration(LogRequestDurationReq req);
    }
}