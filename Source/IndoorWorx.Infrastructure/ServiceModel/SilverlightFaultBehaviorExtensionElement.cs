using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace IndoorWorx.Infrastructure.ServiceModel
{
    /// <summary>
    /// Endpoint behavior for Silverlight Faults.
    /// Implementation extracted from: http://msdn.microsoft.com/en-us/library/dd470096(VS.96).aspx.
    /// </summary>
    public class SilverlightFaultBehaviorExtensionElement : BehaviorExtensionElement, IEndpointBehavior
    {
        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/>.
        /// </returns>
        /// <value>A <see cref="SilverlightFaultBehaviorExtensionElement"/> type.</value>
        public override System.Type BehaviorType
        {
            get { return typeof(SilverlightFaultBehaviorExtensionElement); }
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param><param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            SilverlightFaultMessageInspector inspector = new SilverlightFaultMessageInspector();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param><param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param><param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        /// The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new SilverlightFaultBehaviorExtensionElement();
        }
    }
}
