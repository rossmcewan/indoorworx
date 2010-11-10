using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace IndoorWorx.Infrastructure.ServiceModel
{
    /// <summary>
    /// A message inspector that change the response status code from 500 to 200 in order to enable Silverlight to read the body of the messages.
    /// Implementation extracted from: http://msdn.microsoft.com/en-us/library/dd470096(VS.96).aspx.
    /// </summary>
    public class SilverlightFaultMessageInspector : IDispatchMessageInspector
    {
        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param><param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply.IsFault)
            {
                HttpResponseMessageProperty property = new HttpResponseMessageProperty();

                // Here the response code is changed to 200.
                property.StatusCode = System.Net.HttpStatusCode.OK;

                reply.Properties[HttpResponseMessageProperty.Name] = property;
            }
        }

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/> method.
        /// </returns>
        /// <param name="request">The request message.</param><param name="channel">The incoming channel.</param><param name="instanceContext">The current service instance.</param>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // Do nothing to the incoming message.
            return null;
        }
    }
}
