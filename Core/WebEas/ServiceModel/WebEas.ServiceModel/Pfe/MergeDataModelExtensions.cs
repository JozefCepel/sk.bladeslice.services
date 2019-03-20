using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    public static class MergeDataModelExtensions
    {
        /// <summary>
        /// Zlucenie konfiguracie pohladu so vseobecnym datovym modelom pohladu
        /// </summary>
        /// <param name="dataModel">Vytvoreni vseobecny model pre danu polozku stromu</param>
        /// <param name="currentViewConfig">Konfiguracia pohladu</param>
        /// <returns></returns>
        public static PfeDataModel Merge(this PfeDataModel dataModel, PfeDataModel currentViewConfig)
        {
            if (currentViewConfig.Id.HasValue && currentViewConfig.Id != 0)
            {
                dataModel.Id = currentViewConfig.Id;
            }
            if (!String.IsNullOrEmpty(currentViewConfig.Name))
            {
                dataModel.Name = currentViewConfig.Name;
            }

            dataModel.RequiresFilter = currentViewConfig.RequiresFilter;
            dataModel.RowFilterEnabled = currentViewConfig.RowFilterEnabled;
            dataModel.SummaryRowEnabled = currentViewConfig.SummaryRowEnabled;
            dataModel.SummaryRow = currentViewConfig.SummaryRow;
            dataModel.MultiSortEnabled = currentViewConfig.MultiSortEnabled;

            if (dataModel.Type != PfeModelType.Layout)
            {
                foreach (PfeColumnAttribute col in dataModel.Fields)
                {
                    PfeColumnAttribute currCol = currentViewConfig.Fields == null ? null : currentViewConfig.Fields.Find(x => x.Name == col.Name);
                    if (currCol != null)
                    {
                        if (col.Hideable)
                        {
                            col.Width = currCol.Width;
                            col.Rank = currCol.Rank;
                            col.Hidden = currCol.Hidden;

                            if (col.Hidden && col.Rank != -1)
                            {
                                col.Hidden = false;
                            }
                        }
                    }
                    else
                    {
                        col.Hidden = true;
                        //col.Rank = -1;
                    }
                }
            }

            if (dataModel.Fields != null && dataModel.Fields.Count(nav => nav.Rank == 0) < 2)
            {
                dataModel.Fields = dataModel.Fields.OrderBy(nav => nav.Rank).ToList();
                for (int i = 0; i < dataModel.Fields.Count; i++)
                {
                    dataModel.Fields[i].Rank = i + 1;
                }
            }

            dataModel.FieldGroups = currentViewConfig.FieldGroups;
            dataModel.MultiSort = currentViewConfig.MultiSort;
            dataModel.Filters = currentViewConfig.Filters;
            dataModel.Pages = currentViewConfig.Pages;
            dataModel.Layout = currentViewConfig.Layout;
            dataModel.DoubleClickAction = currentViewConfig.DoubleClickAction;
            dataModel.WaitForInputData = currentViewConfig.WaitForInputData;
            dataModel.UseAsBrowser = currentViewConfig.UseAsBrowser;
            dataModel.UseAsBrowserRank = currentViewConfig.UseAsBrowserRank;

            return dataModel;
        }
    }
}