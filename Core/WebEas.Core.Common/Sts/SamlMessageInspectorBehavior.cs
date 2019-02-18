using System.ServiceModel.Description;

namespace WebEas.Core.Sts
{
    internal class SamlMessageInspectorBehavior : IEndpointBehavior
    {
        public SamlMessageInspector CurrentSamlInspector
        {
            get;
            private set;
        }

        public SamlMessageInspectorBehavior()
        {
            this.CurrentSamlInspector = new SamlMessageInspector();
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // No implementation necessary
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this.CurrentSamlInspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            // No implementation necessary
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            // No implementation necessary  
        }
    }
}
