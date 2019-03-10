using System;
using System.Linq;
using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public partial class BdsRepository
    {
        public List<DatabaseHierarchyNode> RenderProgramovyRozpocet(DatabaseHierarchyNode staticData)
        {
            List<ProgramView> programy = GetCacheOptimizedTenant($"pfe:ProgramovyRozpocet", () =>
            {
                var f = new Filter(FilterElement.Custom("((PlatnostOd IS NULL OR CONVERT(date, PlatnostOd) <= CONVERT(date, getdate())) AND (PlatnostDo IS NULL OR CONVERT(date, PlatnostDo) >= CONVERT(date, getdate())))"));
                return GetList<ProgramView>(f);
            }, new TimeSpan(24, 0, 0));

            if (staticData.ModelType == typeof(ProgramovyRozpocetProgramView))
            {
                var result = new List<DatabaseHierarchyNode>();
                foreach (var program in programy.Where(x => x.PRTyp == 1).OrderBy(x => x.program))
                {
                    var node = staticData.Clone();
                    node.Parameter = program.program;
                    node.Nazov = $" {program.PRKod} - {program.PRNazov}";

                    result.Add(node);
                }

                return result;
            }

            if (staticData.ModelType == typeof(ProgramovyRozpocetPodprogramView))
            {
                var result = new List<DatabaseHierarchyNode>();
                if (staticData.Parent.Parameter != null)
                {
                    long parameterValue = long.Parse(staticData.Parent?.Parameter?.ToString());
                    foreach (var program in programy.Where(x => x.program == parameterValue && x.PRTyp == 2).OrderBy(x => x.podprogram))
                    {
                        var node = staticData.Clone();
                        node.Parameter = program.podprogram;
                        node.Nazov = $" {program.PRKod} - {program.PRNazov}";

                        result.Add(node);
                    }
                }

                return result;
            }

            if (staticData.ModelType == typeof(ProgramovyRozpocetPrvokView))
            {
                var result = new List<DatabaseHierarchyNode>();
                if (staticData.Parent.Parent.Parameter != null)
                {
                    long parentParameterValue = long.Parse(staticData.Parent.Parent.Parameter.ToString());
                    long parameterValue = long.Parse(staticData.Parent.Parameter.ToString());
                    foreach (var program in programy.Where(x => x.program == parentParameterValue && x.podprogram == parameterValue && x.PRTyp == 3).OrderBy(x => x.prvok))
                    {
                        var node = staticData.Clone();
                        node.Parameter = program.prvok;
                        node.Nazov = $" {program.PRKod} - {program.PRNazov}";

                        result.Add(node);
                    }
                }
                

                return result;
            }

            throw new WebEasNotFoundException("Neznámy typ položky");

        }
    }
}
