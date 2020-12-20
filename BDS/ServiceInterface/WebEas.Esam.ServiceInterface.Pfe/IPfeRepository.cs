using System.Collections.Generic;
using System.IO;
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
        IList<PohladList> GetPohlady(string kodPolozky, bool browser);

        object GetPohlad(GetPohlad request);

        PohladView GetPohladModel(int id);

        PohladView SavePohlad(Pohlad pohlad);

        PohladView CreatePohlad(Pohlad pohlad);

        PohladView CreatePohlad(CreatePohlad request);

        PohladView UpdatePohlad(Pohlad pohlad);

        void DeletePohlad(int[] ids);

        PohladView UpdatePohladCustom(PohladDto pohladCustom);

        PohladView DeletePohladCustom(int D_Pohlad_Id);

        List<PossibleStateResponse> ListPossibleStates(int idPriestor, int idState, bool uctovanie, string ItemCode);

        List<PohladView> UnLockPohlad(int id, bool zamknut);

        List<PohladItem> SelectedViewItems(string kodPolozky);

        //string GetBussinessXml(int textationId);

        List<FileUploadResponse> FileUpload(Dictionary<string, Stream> fileList, FileUpload fileUpload);

        ContextUser GetContextUser(string moduleShortcut);

        PfeLayout FillLayoutPagesTitle(PfeLayout layout);

        RendererResult ExportGrid(string title, string xml);

        List<TranslationColumnEntity> GetTranslationColumns(string module);

        List<TranslationDictionary> GetTraslatedExpressions(string uniqueKey);        

        RendererResult GenerateMergeScriptGlobalViews(MergeScriptGlobalViews request);

        List<LogView> PreviewLog(string identifier);

        List<LogView> PreviewLogCorId(string identifier);

        PollerReceiveResponse PollerReceive(string tenantId);

        void LogRequestDuration(LogRequestDurationReq req);

        List<ListAllModulesResponse> ListAllModules(ListAllModules request);

        object GetModuleTreeView(GetTreeView request);

        HierarchyNode GetHierarchyNodeForModule(string kodPolozky);
        string GetModuleCode(string itemCode);
        RendererResult GetReport(GetReportDto request);
    }
}