using ServiceStack;
using WebEas.AppStatus;

namespace WebEas.Esam.ServiceInterface.Office
{
    public class PublicService : Service
    {
        /// <summary>
        /// Gets the health check.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public object Get(HealthCheckDto request)
        {
            HealthCheck result;
            switch (request.Scope)
            {
                case "monitoring":
                    result = new Monitoring(); break;
                case "full":
                    result = new Full(); break;
                default:
                    result = new HealthCheck(); break;
            }

            Response.AddHeader("x-node-id", result.NodeId);
            Response.AddHeader("x-node-status", result.NodeStatus.ToString());

            return result;
        }
    }
}
