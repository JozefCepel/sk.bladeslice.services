using System.Collections.Generic;

namespace WebEas.ServiceModel
{
    public interface IAfterGetList
    {
        /// <summary>
        /// Modify data after GetList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">IWebEasRepositoryBase</param>
        /// <param name="data">data returned from GetList</param>
        void AfterGetList<T>(IWebEasRepositoryBase repository, ref List<T> data, Filter filter);
    }
}
