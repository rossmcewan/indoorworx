using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using IndoorWorx.NHibernate.Serialization;

namespace IndoorWorx.NHibernate.ServiceModel
{
    public class DataContractSurrogateExtensionElement : BehaviorExtensionElement, IEndpointBehavior
    {
        public override Type BehaviorType
        {
            get { return typeof(DataContractSurrogateExtensionElement); }
        }

        protected override object CreateBehavior()
        {
            return new DataContractSurrogateExtensionElement();
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            foreach (var operation in endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior =
                operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                dataContractSerializerOperationBehavior.DataContractSurrogate = new DataContractSurrogate();
            }
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            foreach (var operation in endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior =
                operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
                dataContractSerializerOperationBehavior.DataContractSurrogate = new DataContractSurrogate();
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

}
