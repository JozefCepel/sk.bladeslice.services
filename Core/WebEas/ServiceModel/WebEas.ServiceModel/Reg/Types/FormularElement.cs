using System;
using System.Linq;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    public class FormularElement
    {
        /// <summary>
        /// Gets or sets the modul.
        /// </summary>
        /// <value>The modul.</value>
        public string Modul { get; set; }

        /// <summary>
        /// Gets or sets the root element.
        /// </summary>
        /// <value>The root element.</value>
        public string RootElement { get; set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; set; }
    }
}