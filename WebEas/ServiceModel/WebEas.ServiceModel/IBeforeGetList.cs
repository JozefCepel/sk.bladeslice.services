using System.Collections.Generic;
using System.Text;

namespace WebEas.ServiceModel
{
    public interface IBeforeGetList
    {
        /// <summary>
        /// Executed Before GetList
        /// </summary>
        /// <param name="repository">WebEasRepositoryBase</param>
        /// <param name="node">polozka v strome</param>
        /// <param name="sql">ak sa tu naplni, pouzije sa dany select, inac default</param>
        /// <param name="filter">moznost upravy filtra pri selecte</param>
        /// <param name="sqlOrderPart">informacia o strankovani a sortovani</param>
        void BeforeGetList(IWebEasRepositoryBase repository, HierarchyNode node, ref string sql, ref Filter filter, ref string sqlFromAlias, string sqlOrderPart);
    }
}
