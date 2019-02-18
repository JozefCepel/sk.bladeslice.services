using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office.Reg
{
    public class ServiceInterface : IWebEasServiceInterface
    {
        /// <summary>
        /// Gets the role list.
        /// </summary>
        /// <value>The role list.</value>
        public HierarchyNode RootNode
        {
            get
            {
                return Modul.HierarchyTree;
            }
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code
        {
            get
            {
                return Modul.Code;
            }
        }
    }
}
