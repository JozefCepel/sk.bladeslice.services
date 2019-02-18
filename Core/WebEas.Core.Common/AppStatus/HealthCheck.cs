using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.AppStatus
{
    /// <summary>
    /// 
    /// </summary>
    public enum NodeStatusType
    {
        ok,
        maintenance
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AppStatusType
    {
        ok,
        warn,
        error
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class HealthCheck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        public HealthCheck()
        {
            this.NodeId = System.Environment.MachineName;
            this.NodeStatus = NodeStatusType.ok;
            this.AppStatus = AppStatusType.ok;
        }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>The node id.</value>
        [DataMember(Name = "nodeId", Order = 1)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the node status.
        /// </summary>
        /// <value>The node status.</value>
        [DataMember(Name = "nodeStatus", Order = 2)]
        public NodeStatusType NodeStatus { get; set; }

        /// <summary>
        /// Gets or sets the app status.
        /// </summary>
        /// <value>The app status.</value>
        [DataMember(Name = "appStatus", Order = 3)]
        public AppStatusType AppStatus { get; set; }
    }
}