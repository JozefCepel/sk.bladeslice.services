using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Types
{
    [DataContract]
    public class PossibleStatesResult
    {
        /// <summary>
        /// Gets or sets the old state.
        /// </summary>
        /// <value>The old state.</value>
        [DataMember]        
        public string OldState { get; set; }

        /// <summary>
        /// Gets or sets the old state_ id.
        /// </summary>
        /// <value>The old state_ id.</value>
        [DataMember]
        public int? OldState_Id { get; set; }

        /// <summary>
        /// Gets or sets the new state.
        /// </summary>
        /// <value>The new state.</value>
        [DataMember]
        public string NewState { get; set; }

        /// <summary>
        /// Gets or sets the new state_ id.
        /// </summary>
        /// <value>The new state_ id.</value>
        [DataMember]
        public int? NewState_Id { get; set; }

        /// <summary>
        /// Gets or sets entity space.
        /// </summary>
        /// <value>The entity space.</value>
        [DataMember]
        public int EntitySpace { get; set; }
    }
}