using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IListComboDto
    {
        /// <summary>
        /// Gets or sets the kod polozky.
        /// </summary>
        /// <value>The kod polozky.</value>
        string KodPolozky { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        string Column { get; set; }

        /// <summary>
        /// Gets or sets the required field.
        /// </summary>
        /// <value>The required field.</value>
        string RequiredField { get; set; }
    }

    /// <summary>
    /// Interface for 'static data' combo support (can not be retrieved from DB)
    /// </summary>
    public interface IStaticCombo
    {
        System.Collections.Generic.List<WebEas.ServiceModel.Types.ComboResult> GetComboList(string[] requestFileds);
        IWebEasRepositoryBase Repository { get; set; }
    }
}