using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.HelpTypes
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class StavovyPriestorStav
    {
        /// <summary>
        /// Gets or sets the c_ stav entity_ id.
        /// </summary>
        /// <value>The c_ stav entity_ id.</value>
        [DataMember]
        public int C_StavEntity_Id { get; set; }

        /// <summary>
        /// Gets or sets the c_ stavovy priestor_ id.
        /// </summary>
        /// <value>The c_ stavovy priestor_ id.</value>
        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }
    }
}