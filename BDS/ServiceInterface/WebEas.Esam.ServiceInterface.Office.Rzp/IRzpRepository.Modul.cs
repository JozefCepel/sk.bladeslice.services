using System.Collections.Generic;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office.Rzp
{
    public partial interface IRzpRepository
    {
        List<DatabaseHierarchyNode> RenderProgramovyRozpocet(DatabaseHierarchyNode staticData);
    }
}
