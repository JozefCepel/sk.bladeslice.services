namespace WebEas.ServiceModel
{
    public interface IPfeCustomize
    {
        /// <summary>
        /// Customizes the column.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        /// <param name="property">The property.</param>
        /// <param name="repository">The repository.</param>
        void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey);
    }
}