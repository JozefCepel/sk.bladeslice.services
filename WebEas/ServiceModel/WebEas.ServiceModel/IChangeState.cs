namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChangeState
    { 
        /// <summary>
        /// Gets or sets the item code.
        /// </summary>
        /// <value>The item code.</value>
        string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        long Id { get; set; }

        /// <summary>
        /// Gets or sets the new state of the id.
        /// </summary>
        /// <value>The new state of the id.</value>
        int IdNewState { get; set; }

        /// <summary>
        /// Vyjadrenie spracovatela
        /// </summary>
        string VyjadrenieSpracovatela { get; set; }
    }
}