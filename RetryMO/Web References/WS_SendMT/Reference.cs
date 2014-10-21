﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18034.
// 
#pragma warning disable 1591

namespace RetryMO.WS_SendMT {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="http://tempuri.org/")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SendMTOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendMT2OperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::RetryMO.Properties.Settings.Default.RetryMO_WS_SendMT_Service;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SendMTCompletedEventHandler SendMTCompleted;
        
        /// <remarks/>
        public event SendMT2CompletedEventHandler SendMT2Completed;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendMT", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int SendMT(
                    string mtseq, 
                    string moid, 
                    string moseq, 
                    string src, 
                    string dest, 
                    string cmdcode, 
                    string msgbody, 
                    string msgtype, 
                    string msgtitle, 
                    string mttotalseg, 
                    string mtseqref, 
                    string cpid, 
                    string serviceid, 
                    string reqtime, 
                    string procresult, 
                    string username, 
                    string password) {
            object[] results = this.Invoke("SendMT", new object[] {
                        mtseq,
                        moid,
                        moseq,
                        src,
                        dest,
                        cmdcode,
                        msgbody,
                        msgtype,
                        msgtitle,
                        mttotalseg,
                        mtseqref,
                        cpid,
                        serviceid,
                        reqtime,
                        procresult,
                        username,
                        password});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void SendMTAsync(
                    string mtseq, 
                    string moid, 
                    string moseq, 
                    string src, 
                    string dest, 
                    string cmdcode, 
                    string msgbody, 
                    string msgtype, 
                    string msgtitle, 
                    string mttotalseg, 
                    string mtseqref, 
                    string cpid, 
                    string serviceid, 
                    string reqtime, 
                    string procresult, 
                    string username, 
                    string password) {
            this.SendMTAsync(mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, cpid, serviceid, reqtime, procresult, username, password, null);
        }
        
        /// <remarks/>
        public void SendMTAsync(
                    string mtseq, 
                    string moid, 
                    string moseq, 
                    string src, 
                    string dest, 
                    string cmdcode, 
                    string msgbody, 
                    string msgtype, 
                    string msgtitle, 
                    string mttotalseg, 
                    string mtseqref, 
                    string cpid, 
                    string serviceid, 
                    string reqtime, 
                    string procresult, 
                    string username, 
                    string password, 
                    object userState) {
            if ((this.SendMTOperationCompleted == null)) {
                this.SendMTOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendMTOperationCompleted);
            }
            this.InvokeAsync("SendMT", new object[] {
                        mtseq,
                        moid,
                        moseq,
                        src,
                        dest,
                        cmdcode,
                        msgbody,
                        msgtype,
                        msgtitle,
                        mttotalseg,
                        mtseqref,
                        cpid,
                        serviceid,
                        reqtime,
                        procresult,
                        username,
                        password}, this.SendMTOperationCompleted, userState);
        }
        
        private void OnSendMTOperationCompleted(object arg) {
            if ((this.SendMTCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendMTCompleted(this, new SendMTCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendMT2", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int SendMT2(string moseq, string src, string dest, string cmdcode, string msgbody, string msgtype, string msgtitle, string mttotalseg, string mtseqref, string cpid, string procresult, string username, string password) {
            object[] results = this.Invoke("SendMT2", new object[] {
                        moseq,
                        src,
                        dest,
                        cmdcode,
                        msgbody,
                        msgtype,
                        msgtitle,
                        mttotalseg,
                        mtseqref,
                        cpid,
                        procresult,
                        username,
                        password});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void SendMT2Async(string moseq, string src, string dest, string cmdcode, string msgbody, string msgtype, string msgtitle, string mttotalseg, string mtseqref, string cpid, string procresult, string username, string password) {
            this.SendMT2Async(moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, cpid, procresult, username, password, null);
        }
        
        /// <remarks/>
        public void SendMT2Async(string moseq, string src, string dest, string cmdcode, string msgbody, string msgtype, string msgtitle, string mttotalseg, string mtseqref, string cpid, string procresult, string username, string password, object userState) {
            if ((this.SendMT2OperationCompleted == null)) {
                this.SendMT2OperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendMT2OperationCompleted);
            }
            this.InvokeAsync("SendMT2", new object[] {
                        moseq,
                        src,
                        dest,
                        cmdcode,
                        msgbody,
                        msgtype,
                        msgtitle,
                        mttotalseg,
                        mtseqref,
                        cpid,
                        procresult,
                        username,
                        password}, this.SendMT2OperationCompleted, userState);
        }
        
        private void OnSendMT2OperationCompleted(object arg) {
            if ((this.SendMT2Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendMT2Completed(this, new SendMT2CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void SendMTCompletedEventHandler(object sender, SendMTCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendMTCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendMTCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void SendMT2CompletedEventHandler(object sender, SendMT2CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendMT2CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendMT2CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591