using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.AppStatus
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [Route("/app-status", "GET")]
    [Api("Stav aplikácie")]
    public class HealthCheckDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckDto" /> class.
        /// </summary>
        public HealthCheckDto()
        {
        }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>The scope.</value>
        [ApiMember(Name = "scope")]
        [DataMember(Name = "scope")]
        public string Scope { get; set; }
    }
}