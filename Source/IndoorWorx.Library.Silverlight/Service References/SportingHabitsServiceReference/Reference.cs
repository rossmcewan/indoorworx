﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.50401.0
// 
namespace IndoorWorx.Library.SportingHabitsServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SportingHabitsServiceReference.ISportingHabitsService")]
    public interface ISportingHabitsService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISportingHabitsService/RetrieveTrainingVolumeOptions", ReplyAction="http://tempuri.org/ISportingHabitsService/RetrieveTrainingVolumeOptionsResponse")]
        System.IAsyncResult BeginRetrieveTrainingVolumeOptions(System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.TrainingVolume> EndRetrieveTrainingVolumeOptions(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISportingHabitsService/RetrieveIndoorTrainingFrequency", ReplyAction="http://tempuri.org/ISportingHabitsService/RetrieveIndoorTrainingFrequencyResponse" +
            "")]
        System.IAsyncResult BeginRetrieveIndoorTrainingFrequency(System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.IndoorTrainingFrequency> EndRetrieveIndoorTrainingFrequency(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ISportingHabitsService/RetrieveCompetitiveLevels", ReplyAction="http://tempuri.org/ISportingHabitsService/RetrieveCompetitiveLevelsResponse")]
        System.IAsyncResult BeginRetrieveCompetitiveLevels(System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.CompetitiveLevel> EndRetrieveCompetitiveLevels(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISportingHabitsServiceChannel : IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RetrieveTrainingVolumeOptionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public RetrieveTrainingVolumeOptionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.TrainingVolume> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.TrainingVolume>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RetrieveIndoorTrainingFrequencyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public RetrieveIndoorTrainingFrequencyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.IndoorTrainingFrequency> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.IndoorTrainingFrequency>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RetrieveCompetitiveLevelsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public RetrieveCompetitiveLevelsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.CompetitiveLevel> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.CompetitiveLevel>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SportingHabitsServiceClient : System.ServiceModel.ClientBase<IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService>, IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService {
        
        private BeginOperationDelegate onBeginRetrieveTrainingVolumeOptionsDelegate;
        
        private EndOperationDelegate onEndRetrieveTrainingVolumeOptionsDelegate;
        
        private System.Threading.SendOrPostCallback onRetrieveTrainingVolumeOptionsCompletedDelegate;
        
        private BeginOperationDelegate onBeginRetrieveIndoorTrainingFrequencyDelegate;
        
        private EndOperationDelegate onEndRetrieveIndoorTrainingFrequencyDelegate;
        
        private System.Threading.SendOrPostCallback onRetrieveIndoorTrainingFrequencyCompletedDelegate;
        
        private BeginOperationDelegate onBeginRetrieveCompetitiveLevelsDelegate;
        
        private EndOperationDelegate onEndRetrieveCompetitiveLevelsDelegate;
        
        private System.Threading.SendOrPostCallback onRetrieveCompetitiveLevelsCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public SportingHabitsServiceClient() {
        }
        
        public SportingHabitsServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SportingHabitsServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SportingHabitsServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SportingHabitsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<RetrieveTrainingVolumeOptionsCompletedEventArgs> RetrieveTrainingVolumeOptionsCompleted;
        
        public event System.EventHandler<RetrieveIndoorTrainingFrequencyCompletedEventArgs> RetrieveIndoorTrainingFrequencyCompleted;
        
        public event System.EventHandler<RetrieveCompetitiveLevelsCompletedEventArgs> RetrieveCompetitiveLevelsCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService.BeginRetrieveTrainingVolumeOptions(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRetrieveTrainingVolumeOptions(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.TrainingVolume> IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService.EndRetrieveTrainingVolumeOptions(System.IAsyncResult result) {
            return base.Channel.EndRetrieveTrainingVolumeOptions(result);
        }
        
        private System.IAsyncResult OnBeginRetrieveTrainingVolumeOptions(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService)(this)).BeginRetrieveTrainingVolumeOptions(callback, asyncState);
        }
        
        private object[] OnEndRetrieveTrainingVolumeOptions(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.TrainingVolume> retVal = ((IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService)(this)).EndRetrieveTrainingVolumeOptions(result);
            return new object[] {
                    retVal};
        }
        
        private void OnRetrieveTrainingVolumeOptionsCompleted(object state) {
            if ((this.RetrieveTrainingVolumeOptionsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RetrieveTrainingVolumeOptionsCompleted(this, new RetrieveTrainingVolumeOptionsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RetrieveTrainingVolumeOptionsAsync() {
            this.RetrieveTrainingVolumeOptionsAsync(null);
        }
        
        public void RetrieveTrainingVolumeOptionsAsync(object userState) {
            if ((this.onBeginRetrieveTrainingVolumeOptionsDelegate == null)) {
                this.onBeginRetrieveTrainingVolumeOptionsDelegate = new BeginOperationDelegate(this.OnBeginRetrieveTrainingVolumeOptions);
            }
            if ((this.onEndRetrieveTrainingVolumeOptionsDelegate == null)) {
                this.onEndRetrieveTrainingVolumeOptionsDelegate = new EndOperationDelegate(this.OnEndRetrieveTrainingVolumeOptions);
            }
            if ((this.onRetrieveTrainingVolumeOptionsCompletedDelegate == null)) {
                this.onRetrieveTrainingVolumeOptionsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRetrieveTrainingVolumeOptionsCompleted);
            }
            base.InvokeAsync(this.onBeginRetrieveTrainingVolumeOptionsDelegate, null, this.onEndRetrieveTrainingVolumeOptionsDelegate, this.onRetrieveTrainingVolumeOptionsCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService.BeginRetrieveIndoorTrainingFrequency(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRetrieveIndoorTrainingFrequency(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.IndoorTrainingFrequency> IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService.EndRetrieveIndoorTrainingFrequency(System.IAsyncResult result) {
            return base.Channel.EndRetrieveIndoorTrainingFrequency(result);
        }
        
        private System.IAsyncResult OnBeginRetrieveIndoorTrainingFrequency(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService)(this)).BeginRetrieveIndoorTrainingFrequency(callback, asyncState);
        }
        
        private object[] OnEndRetrieveIndoorTrainingFrequency(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.IndoorTrainingFrequency> retVal = ((IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService)(this)).EndRetrieveIndoorTrainingFrequency(result);
            return new object[] {
                    retVal};
        }
        
        private void OnRetrieveIndoorTrainingFrequencyCompleted(object state) {
            if ((this.RetrieveIndoorTrainingFrequencyCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RetrieveIndoorTrainingFrequencyCompleted(this, new RetrieveIndoorTrainingFrequencyCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RetrieveIndoorTrainingFrequencyAsync() {
            this.RetrieveIndoorTrainingFrequencyAsync(null);
        }
        
        public void RetrieveIndoorTrainingFrequencyAsync(object userState) {
            if ((this.onBeginRetrieveIndoorTrainingFrequencyDelegate == null)) {
                this.onBeginRetrieveIndoorTrainingFrequencyDelegate = new BeginOperationDelegate(this.OnBeginRetrieveIndoorTrainingFrequency);
            }
            if ((this.onEndRetrieveIndoorTrainingFrequencyDelegate == null)) {
                this.onEndRetrieveIndoorTrainingFrequencyDelegate = new EndOperationDelegate(this.OnEndRetrieveIndoorTrainingFrequency);
            }
            if ((this.onRetrieveIndoorTrainingFrequencyCompletedDelegate == null)) {
                this.onRetrieveIndoorTrainingFrequencyCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRetrieveIndoorTrainingFrequencyCompleted);
            }
            base.InvokeAsync(this.onBeginRetrieveIndoorTrainingFrequencyDelegate, null, this.onEndRetrieveIndoorTrainingFrequencyDelegate, this.onRetrieveIndoorTrainingFrequencyCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService.BeginRetrieveCompetitiveLevels(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRetrieveCompetitiveLevels(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.CompetitiveLevel> IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService.EndRetrieveCompetitiveLevels(System.IAsyncResult result) {
            return base.Channel.EndRetrieveCompetitiveLevels(result);
        }
        
        private System.IAsyncResult OnBeginRetrieveCompetitiveLevels(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService)(this)).BeginRetrieveCompetitiveLevels(callback, asyncState);
        }
        
        private object[] OnEndRetrieveCompetitiveLevels(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.CompetitiveLevel> retVal = ((IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService)(this)).EndRetrieveCompetitiveLevels(result);
            return new object[] {
                    retVal};
        }
        
        private void OnRetrieveCompetitiveLevelsCompleted(object state) {
            if ((this.RetrieveCompetitiveLevelsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RetrieveCompetitiveLevelsCompleted(this, new RetrieveCompetitiveLevelsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RetrieveCompetitiveLevelsAsync() {
            this.RetrieveCompetitiveLevelsAsync(null);
        }
        
        public void RetrieveCompetitiveLevelsAsync(object userState) {
            if ((this.onBeginRetrieveCompetitiveLevelsDelegate == null)) {
                this.onBeginRetrieveCompetitiveLevelsDelegate = new BeginOperationDelegate(this.OnBeginRetrieveCompetitiveLevels);
            }
            if ((this.onEndRetrieveCompetitiveLevelsDelegate == null)) {
                this.onEndRetrieveCompetitiveLevelsDelegate = new EndOperationDelegate(this.OnEndRetrieveCompetitiveLevels);
            }
            if ((this.onRetrieveCompetitiveLevelsCompletedDelegate == null)) {
                this.onRetrieveCompetitiveLevelsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRetrieveCompetitiveLevelsCompleted);
            }
            base.InvokeAsync(this.onBeginRetrieveCompetitiveLevelsDelegate, null, this.onEndRetrieveCompetitiveLevelsDelegate, this.onRetrieveCompetitiveLevelsCompletedDelegate, userState);
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
        
        protected override IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService CreateChannel() {
            return new SportingHabitsServiceClientChannel(this);
        }
        
        private class SportingHabitsServiceClientChannel : ChannelBase<IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService>, IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService {
            
            public SportingHabitsServiceClientChannel(System.ServiceModel.ClientBase<IndoorWorx.Library.SportingHabitsServiceReference.ISportingHabitsService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginRetrieveTrainingVolumeOptions(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("RetrieveTrainingVolumeOptions", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.TrainingVolume> EndRetrieveTrainingVolumeOptions(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.TrainingVolume> _result = ((System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.TrainingVolume>)(base.EndInvoke("RetrieveTrainingVolumeOptions", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginRetrieveIndoorTrainingFrequency(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("RetrieveIndoorTrainingFrequency", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.IndoorTrainingFrequency> EndRetrieveIndoorTrainingFrequency(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.IndoorTrainingFrequency> _result = ((System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.IndoorTrainingFrequency>)(base.EndInvoke("RetrieveIndoorTrainingFrequency", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginRetrieveCompetitiveLevels(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("RetrieveCompetitiveLevels", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.CompetitiveLevel> EndRetrieveCompetitiveLevels(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.CompetitiveLevel> _result = ((System.Collections.ObjectModel.ObservableCollection<IndoorWorx.Infrastructure.Models.CompetitiveLevel>)(base.EndInvoke("RetrieveCompetitiveLevels", _args, result)));
                return _result;
            }
        }
    }
}
