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
    public class Message
    {
        #region Properties

        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public int RecordsAffected { get; set; }
        [DataMember]
        public string ExceptionMessage { get; set; }
        [DataMember]
        public string InnerExceptionMessage { get; set; }
        [DataMember]
        public string InnerInnerExceptionMessage { get; set; }
        [DataMember]
        public string InnerInnerExceptionStackTrace { get; set; }
        [DataMember]
        public string StackTrace { get; set; }
        [DataMember]
        public string UserMessage { get; set; }
        [DataMember]
        public bool ShowErrorDetail { get; set; }
        [DataMember]
        public string ErrorDetailWithLineBreaks { get; set; }
        [DataMember]
        public string ErrorDetail { get; set; }
        [DataMember]
        public string ErrorDetailInHTML { get; set; }
        //Message type icons 
        //"/Images/icon-error.gif
        //"/Images/icon-info.gif
        //"/Images/icon-question.gif
        [DataMember]
        public string MessageType { get; set; }
        
        #endregion

        #region Methods
        public string CreateDetail()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b><u>User Message Displayed </u></b></br>");
            sb.AppendLine(this.UserMessage + "</br>");
            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b><u>Exception Message</u></b></br>");
            sb.AppendLine(this.ExceptionMessage + "</br>");
            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b><u>Inner Exception Message</u></b></br>");
            sb.AppendLine(this.InnerExceptionMessage + "</br>");
            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b><u>Inner Inner Exception Message</u></b></br>");
            sb.AppendLine(this.InnerInnerExceptionMessage + "</br>");
            sb.AppendLine("----------------------------</br>");
            sb.AppendLine("<b></u>Stack Trace</u></b></br>");
            sb.AppendLine(this.StackTrace + "</br>");
            sb.AppendLine("----------------------------</br>");

            return sb.ToString();
        }

        public string CreateDetailNoHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.UserMessage);
            sb.AppendLine(this.ExceptionMessage);
            sb.AppendLine(this.InnerExceptionMessage);
            sb.AppendLine(this.InnerInnerExceptionMessage);
            sb.AppendLine(this.StackTrace);

            return sb.ToString();
        }

        #endregion
    }
}
