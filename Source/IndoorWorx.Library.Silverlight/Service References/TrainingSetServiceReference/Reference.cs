﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.60310.0
// 
namespace IndoorWorx.Library.TrainingSetServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TrainingSetServiceReference.ITrainingSetService")]
    public interface ITrainingSetService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ITrainingSetService/CreateTrainingSet", ReplyAction="http://tempuri.org/ITrainingSetService/CreateTrainingSetResponse")]
        System.IAsyncResult BeginCreateTrainingSet(IndoorWorx.Infrastructure.Requests.CreateTrainingSetRequest request, System.AsyncCallback callback, object asyncState);
        
        IndoorWorx.Infrastructure.Responses.CreateTrainingSetResponse EndCreateTrainingSet(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITrainingSetServiceChannel : IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CreateTrainingSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public CreateTrainingSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public IndoorWorx.Infrastructure.Responses.CreateTrainingSetResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((IndoorWorx.Infrastructure.Responses.CreateTrainingSetResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TrainingSetServiceClient : System.ServiceModel.ClientBase<IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService>, IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService {
        
        private BeginOperationDelegate onBeginCreateTrainingSetDelegate;
        
        private EndOperationDelegate onEndCreateTrainingSetDelegate;
        
        private System.Threading.SendOrPostCallback onCreateTrainingSetCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public TrainingSetServiceClient() {
        }
        
        public TrainingSetServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TrainingSetServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TrainingSetServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TrainingSetServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<CreateTrainingSetCompletedEventArgs> CreateTrainingSetCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService.BeginCreateTrainingSet(IndoorWorx.Infrastructure.Requests.CreateTrainingSetRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCreateTrainingSet(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        IndoorWorx.Infrastructure.Responses.CreateTrainingSetResponse IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService.EndCreateTrainingSet(System.IAsyncResult result) {
            return base.Channel.EndCreateTrainingSet(result);
        }
        
        private System.IAsyncResult OnBeginCreateTrainingSet(object[] inValues, System.AsyncCallback callback, object asyncState) {
            IndoorWorx.Infrastructure.Requests.CreateTrainingSetRequest request = ((IndoorWorx.Infrastructure.Requests.CreateTrainingSetRequest)(inValues[0]));
            return ((IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService)(this)).BeginCreateTrainingSet(request, callback, asyncState);
        }
        
        private object[] OnEndCreateTrainingSet(System.IAsyncResult result) {
            IndoorWorx.Infrastructure.Responses.CreateTrainingSetResponse retVal = ((IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService)(this)).EndCreateTrainingSet(result);
            return new object[] {
                    retVal};
        }
        
        private void OnCreateTrainingSetCompleted(object state) {
            if ((this.CreateTrainingSetCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CreateTrainingSetCompleted(this, new CreateTrainingSetCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CreateTrainingSetAsync(IndoorWorx.Infrastructure.Requests.CreateTrainingSetRequest request) {
            this.CreateTrainingSetAsync(request, null);
        }
        
        public void CreateTrainingSetAsync(IndoorWorx.Infrastructure.Requests.CreateTrainingSetRequest request, object userState) {
            if ((this.onBeginCreateTrainingSetDelegate == null)) {
                this.onBeginCreateTrainingSetDelegate = new BeginOperationDelegate(this.OnBeginCreateTrainingSet);
            }
            if ((this.onEndCreateTrainingSetDelegate == null)) {
                this.onEndCreateTrainingSetDelegate = new EndOperationDelegate(this.OnEndCreateTrainingSet);
            }
            if ((this.onCreateTrainingSetCompletedDelegate == null)) {
                this.onCreateTrainingSetCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCreateTrainingSetCompleted);
            }
            base.InvokeAsync(this.onBeginCreateTrainingSetDelegate, new object[] {
                        request}, this.onEndCreateTrainingSetDelegate, this.onCreateTrainingSetCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService CreateChannel() {
            return new TrainingSetServiceClientChannel(this);
        }
        
        private class TrainingSetServiceClientChannel : ChannelBase<IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService>, IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService {
            
            public TrainingSetServiceClientChannel(System.ServiceModel.ClientBase<IndoorWorx.Library.TrainingSetServiceReference.ITrainingSetService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginCreateTrainingSet(IndoorWorx.Infrastructure.Requests.CreateTrainingSetRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("CreateTrainingSet", _args, callback, asyncState);
                return _result;
            }
            
            public IndoorWorx.Infrastructure.Responses.CreateTrainingSetResponse EndCreateTrainingSet(System.IAsyncResult result) {
                object[] _args = new object[0];
                IndoorWorx.Infrastructure.Responses.CreateTrainingSetResponse _result = ((IndoorWorx.Infrastructure.Responses.CreateTrainingSetResponse)(base.EndInvoke("CreateTrainingSet", _args, result)));
                return _result;
            }
        }
    }
}
