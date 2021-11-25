using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.Entity.Validation;



namespace Common
{
    public static class ExceptionHandler
    {
        private static bool showDetailError = false;
        private static bool logToDB = false;
        public static Message Handle(Exception ex)
        {
            
            //TODO Log error to Log File or Db etc
            //and return appropriate message to user
            if (ConfigurationManager.AppSettings["DisplayErrorDetail"] != null)
            {
                if (ConfigurationManager.AppSettings["DisplayErrorDetail"].Equals("true"))
                {
                    showDetailError = true;
                }
            }
            if (ConfigurationManager.AppSettings["logExceptionsToDb"] != null)
            {
                if (ConfigurationManager.AppSettings["logExceptionsToDb"].Equals("true"))
                {
                    logToDB = true;
                }
            }
            
            Message mess =  new Message();
            mess.ExceptionMessage = ex.Message + ex.GetType().ToString();
            mess.StackTrace = ex.StackTrace;
            if (ex is DbEntityValidationException)
            {
                DbEntityValidationException dbex = (DbEntityValidationException)ex;
                foreach (var validationErrors in dbex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                      //  Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        mess.ErrorDetailWithLineBreaks = mess.ErrorDetail + "Validation Failed for " + validationError.PropertyName + Environment.NewLine + validationError.ErrorMessage + Environment.NewLine;
                        mess.ErrorDetail = mess.ErrorDetail + " Validation Failed for " + validationError.PropertyName + " " + validationError.ErrorMessage;

                        mess.ExceptionMessage = mess.ExceptionMessage + Environment.NewLine + "Validation Failed for " + validationError.PropertyName + Environment.NewLine + validationError.ErrorMessage + Environment.NewLine;
                    }
                }
            }

            if (ex.InnerException != null)
            {
                mess.InnerExceptionMessage = ex.InnerException.Message;
                mess.ExceptionMessage = mess.ExceptionMessage + Environment.NewLine + "InnerException = " + ex.InnerException.Message;
                if (ex.InnerException.InnerException != null)
                {
                    mess.InnerInnerExceptionMessage = ex.InnerException.InnerException.Message;
                    mess.ExceptionMessage = mess.ExceptionMessage + Environment.NewLine + "InnerException.InnerException = " + ex.InnerException.InnerException.Message;
                }
                else
                {
                    mess.InnerInnerExceptionMessage = "null";
                }
            }
            else
            {
                mess.InnerExceptionMessage = "null";
            }                     
            mess.MessageType = "/Images/icon-question.gif";
            mess.ShowErrorDetail = showDetailError;
            
            if (mess.ShowErrorDetail)
            {
                mess.ErrorDetailInHTML = mess.CreateDetail();
                mess.ErrorDetail = mess.CreateDetailNoHtml();
            }
            if (logToDB)
            {
                //LOG exceptions to DB ezException table
            }
            return mess;
        }
        /// <summary>
        /// Get a substring of the first N characters.
        /// </summary>
        public static string Truncate(string source, int length)
        {
            if (source != null && source.Length > length)
            {
                source = source.Substring(0, length);
            }
            else
            {
                return String.Empty;
            }
            return source;
        }
    }
}
