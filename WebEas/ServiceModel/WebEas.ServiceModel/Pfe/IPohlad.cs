using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPohlad
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        string Data { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; set; }
     
        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        string Nazov { get; set; }

        /// <summary>
        /// Gets or sets the typ.
        /// </summary>
        /// <value>The typ.</value>
        string Typ { get; set; }

        /// <summary>
        /// Gets or sets the show in actions.
        /// </summary>
        /// <value>The typshow in actions.</value>
        bool ShowInActions { get; set; }
    }
}