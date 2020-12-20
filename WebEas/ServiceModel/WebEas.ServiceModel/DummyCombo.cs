using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Testovacie data
    /// </summary>
    [PfeDataModel(Name = "Dummy combo")]
    [DataContract]
    public class DummyCombo : BaseTenantEntity
    {
        /// <summary>
        /// Gets or sets the dummy combo_ id.
        /// </summary>
        /// <value>The dummy combo_ id.</value>
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int DummyCombo_Id { get; set; }

        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        [DataMember]
        [PfeValueColumn]
        public string Nazov { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Nazov;
        }
    }
}