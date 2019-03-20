using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial interface ICfeRepository
    {
        List<DatabaseHierarchyNode> RenderPouzivateliaModuly(DatabaseHierarchyNode staticData);
    }
}
