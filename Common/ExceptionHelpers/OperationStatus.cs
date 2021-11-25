// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;

namespace Common
{
    [DataContract]
    public class OperationStatus 
    {

        //public  OperationStatus (CultureInfo _culture)
        //{
        //    Culture = _culture;
        //}

        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public int RecordsAffected { get; set; }

        private string usermessage = string.Empty;
        /// <summary>
        /// Gets or sets the user message.
        /// </summary>
        /// <value>
        /// The user message is the only Message that is Culture language specific.
        /// </value>
        [DataMember]
        public string UserMessage 
        {
            get
            {
                if(Culture != null)
                {
                    switch(this.Action)
                    {
                        case Enumerations.RepositoryAction.Add:
                            usermessage = resManager.GetString("Add", Culture) + " " + resManager.GetString("of", Culture) + " " + Entity + " " + (Status ? resManager.GetString("SavedSuccessfull", Culture) : resManager.GetString("failed", Culture));
                            break;
                        case Enumerations.RepositoryAction.Update:
                            usermessage = resManager.GetString("Update", Culture) + " " + resManager.GetString("of", Culture) + " " + Entity + " " + (Status ? resManager.GetString("SavedSuccessfull", Culture) : resManager.GetString("failed", Culture));
                            break;
                        case Enumerations.RepositoryAction.Delete:
                            usermessage = resManager.GetString("Delete", Culture) + " " + resManager.GetString("of", Culture) + " " + Entity + " " + (Status ? resManager.GetString("SavedSuccessfull", Culture) : resManager.GetString("failed", Culture));
                            break;
                        case Enumerations.RepositoryAction.Gets:
                            usermessage = resManager.GetString("Retrieved", Culture) + " " + Entity + "s " + (Status ? resManager.GetString("Successfully", Culture) : resManager.GetString("failed", Culture));
                            break;
                    }
                    //if (this.MessageType == Enumerations.MessageType.Error && this.Action == Enumerations.RepositoryAction.Gets)
                    //{
                    //    usermessage = resManager.GetString("ErrorOccured", Culture) + " " + resManager.GetString("Retrieved", Culture) + " " + Entity;
                    //}
                    
                }
                return usermessage;
            }
            set
            {
                usermessage = value;
            }
        }
        [DataMember]
        public string ExceptionMessage { get; set; }
        [DataMember]
        public string InnerExceptionMessage { get; set; }
        [DataMember]
        public string InnerInnerExceptionMessage { get; set; }
        [DataMember]
        public string StackTrace { get; set; }
        [DataMember]
        public Enumerations.RepositoryAction Action { get; set; }
        [DataMember]
        public string Entity { get; set; }
        
        private ResourceManager resManager { get; set;}

        private CultureInfo culture = null;
        public CultureInfo Culture
        {
            get
            {
                return culture;
            }
            set
            {
                culture = value;
                System.Reflection.Assembly resourceAssembly;
                resourceAssembly = System.Reflection.Assembly.Load("Common");
                if (resourceAssembly != null)
                {
                    resManager = new ResourceManager("Common.Resource.Messages", resourceAssembly);
                }
            }
        }
        
        [DataMember]
        public Enumerations.MessageType? MessageType { get; set; }

        [DataMember]
        public string MessageDetail { get; set; }
        [DataMember]
        public string ErrorDetail { get; set; }

        public static OperationStatus CreateFromException(OperationStatus opStatus, Exception ex)
        {
            //TODO add exception manager to get more detail logic to get exact error that occurred etc
            
            //OperationStatus opStatus = new OperationStatus
            //{
            //    //Entity = this.Entity,
            //    Status = false,
            //    ExceptionMessage = ex.Message,
            //    MessageType = Enumerations.MessageType.Error,
            //    Action = action,
            //};
            opStatus.Status = false;
            opStatus.MessageType = Enumerations.MessageType.Error;
            opStatus.ExceptionMessage = ex.Message;
            
            if(ex.InnerException != null)
            {
                if (ex.InnerException.InnerException != null)
                {
                    opStatus.InnerInnerExceptionMessage = ex.InnerException.InnerException.Message;
                }
                opStatus.InnerExceptionMessage = ex.InnerException.Message; 
              opStatus.StackTrace = ex.InnerException.StackTrace;
            }
            //TODO add error logging here
            //Create Default setting to switch on error logging in application load
            //if (logError)
            //{
            //    //Add opStatus in log info to show what was returned
            //    //Log error here 
            //    //Create setting in config to say where to log to File db or event log 
            //}
            opStatus.MessageDetail = CreateDetail(opStatus);
            opStatus.ErrorDetail = CreateDetailNoHtml(opStatus);
            return opStatus;
        }

        private static string CreateDetail(OperationStatus status)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b><u>Message Type</u></b></br>");
            sb.AppendLine(status.MessageType + "</br>");
            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b><u>Exception Message</u></b></br>");
            sb.AppendLine(status.ExceptionMessage + "</br>");
            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b><u>Inner Exception</u></b></br>");
            sb.AppendLine(status.InnerExceptionMessage + "</br>");
            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b><u>Inner InnerException</u></b></br>");
            sb.AppendLine(status.InnerInnerExceptionMessage + "</br>");
            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b></u>Stack Trace</u></b></br>");
            sb.AppendLine(status.StackTrace + "</br>");
            sb.AppendLine("----------------------------</br>");

            return sb.ToString();
        }

        private static string CreateDetailNoHtml(OperationStatus status)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(status.InnerExceptionMessage);
                        
            sb.AppendLine(status.UserMessage );
           
            sb.AppendLine(status.MessageType.ToString());
            
            sb.AppendLine(status.ExceptionMessage );
           
            sb.AppendLine(status.InnerExceptionMessage );
            
            return sb.ToString();
        }
        
    }
}