using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial class CfeRepository 
    {
        public List<DatabaseHierarchyNode> RenderPouzivateliaModuly(DatabaseHierarchyNode staticData)
        {
            List<WebEas.Esam.ServiceModel.Office.Cfe.Types.Modul> moduly = GetCacheOptimized($"pfe:Moduly", () =>
            {
                
                return GetList<WebEas.Esam.ServiceModel.Office.Cfe.Types.Modul>();
            }, new TimeSpan(24, 0, 0));

            var result = new List<DatabaseHierarchyNode>();
            foreach (var modul in moduly)
            {
                var node = staticData.Clone();
                node.Parameter = modul.C_Modul_Id;
                node.Nazov = $" {modul.Kod} - {modul.Nazov}";

                result.Add(node);
            }

            return result;

            //throw new WebEasNotFoundException("Neznámy typ položky");

        }

    }
}
