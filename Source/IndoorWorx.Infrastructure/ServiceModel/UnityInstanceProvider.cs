using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace IndoorWorx.Infrastructure.ServiceModel
{
    /// <summary>
    /// Declares methods that provide a service object or recycle a service object for a Windows Communication Foundation (WCF) service.
    /// </summary>
    public class UnityInstanceProvider : IInstanceProvider
    {
        /// <summary>
        /// The <see cref="IUnityContainer"/>.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// The type of the instance.
        /// </summary>
        private readonly Type instanceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityInstanceProvider"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="instanceType">Type of the instance.</param>
        public UnityInstanceProvider(IUnityContainer container, Type instanceType)
        {
            this.container = container;
            this.instanceType = instanceType;
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext" /> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext" /> object.</param>
        /// <returns> A user-defined service object.</returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext" /> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext" /> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.container.Resolve(this.instanceType);
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext" /> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            this.container.Teardown(instance);
        }
    }
}
