using WebEas.ServiceInterface;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceInterface.Office.Bds
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
