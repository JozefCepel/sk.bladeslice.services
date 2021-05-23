using ServiceStack;

namespace WebEas.Esam.ServiceInterface.Pfe
{
    public class MaintenanceService : Service
    {
        public IServerEvents ServerEvents { get; set; }

        public void Post(WebEas.Esam.ServiceModel.Pfe.Dto.AppStatusDto request)
        {
#if !DEBUG
            ServerEvents.NotifyAll(request);
#endif
        }
    }
}
