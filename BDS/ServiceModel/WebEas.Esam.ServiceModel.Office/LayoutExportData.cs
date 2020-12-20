using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.Reports;

namespace WebEas.Esam.ServiceModel.Office
{
    /// <summary>
    /// 
    /// </summary>
    public class LayoutExportData : IReportData
    {
        /// <summary>
        /// Gets or sets the document title.
        /// </summary>
        /// <value>The document title.</value>
        public string DocumentTitle { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets the city logo.
        /// </summary>
        /// <value>The city logo.</value>
        public byte[] CityLogo { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<LayoutExportItem> Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class LayoutExportItem
        {
            /// <summary>
            /// Gets or sets the column data.
            /// </summary>
            /// <value>The column data.</value>
            public List<LayoutExportColumnData> ColumnData { get; set; }

            /// <summary>
            /// Gets or sets the data.
            /// </summary>
            /// <value>The data.</value>
            public List<Dictionary<string, object>> Data { get; set; }

            /// <summary>
            /// Gets or sets the name of the view.
            /// </summary>
            /// <value>The name of the view.</value>
            public string ViewName { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the filter.
            /// </summary>
            /// <value>The filter.</value>
            public string Filter { get; set; }

            /// <summary>
            /// Gets or sets the filter.
            /// </summary>
            /// <value>The filter.</value>
            public string FilterText { get; set; }

            /// <summary>
            /// Gets or sets the page number.
            /// </summary>
            /// <value>The page number.</value>
            public string PageNumber { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class LayoutExportColumnData
        {
            public LayoutExportColumnData()
            {
                Align = AligmentType.Unknown;
            }

            /// <summary>
            /// 
            /// </summary>
            public enum AligmentType
            {
                Unknown = -1,
                Right = 1,
                Left = 2,
                Center = 3,
                Justify = 4
            }

            /// <summary>
            /// 
            /// </summary>
            public enum DataType
            {
                Default = 0,
                Boolean = 1,
                Text = 2,
                Number = 3,
                Date = 4,
                Time = 5,
                DateTime = 6,
                Unknown = -1
            }

            /// <summary>
            /// Gets or sets the caption.
            /// </summary>
            /// <value>The caption.</value>
            public string Caption { get; set; }

            /// <summary>
            /// Gets or sets the width.
            /// </summary>
            /// <value>The width.</value>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets the name of the column.
            /// </summary>
            /// <value>The name of the column.</value>
            public string ColumnName { get; set; }

            /// <summary>
            /// Gets or sets the align.
            /// </summary>
            /// <value>The align.</value>
            public AligmentType Align { get; set; }

            /// <summary>
            /// Gets or sets the type of the col.
            /// </summary>
            /// <value>The type of the col.</value>
            public DataType ColType { get; set; }
        }
    }
}
