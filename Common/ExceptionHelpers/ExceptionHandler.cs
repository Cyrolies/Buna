using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

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
                if (ex.InnerException.InnerException != null)
                {
                    if (ex.InnerException.InnerException is SqlException)
                    {
                        SqlException sqlEx = (SqlException)ex.InnerException.InnerException;
                        if(sqlEx.Number == 547) //Referentail integrity error on deletion
						{
                            var errMsg = sqlEx.Errors[0].Message;
                            string start = errMsg.Substring(errMsg.IndexOf("_")+1);
                            string foreignKey = start.Substring(0, start.IndexOf("_"));
                            mess.ExceptionMessage = "Cannot be deleted because it still has a link to a " + foreignKey.Replace("Asset", "Farm").Replace("Person", "Farmer");
                        }
                        if (sqlEx.Number == 2627)//Unique Constraint on saving
                        {
                            var errMsg = sqlEx.Errors[0].Message;
                            string start = errMsg.Substring(errMsg.IndexOf("(") + 1);
                            string foreignKey = start.Substring(0, start.IndexOf(")"));
                            mess.ExceptionMessage = "Already exists must be unique " + foreignKey.Replace("Asset", "Farm").Replace("Person", "Farmer");

                        }
                    }
                }
                else 
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
            }
            else
            {
                mess.ExceptionMessage = ex.Message;
                if (mess.ShowErrorDetail)
                {
                    mess.StackTrace = ex.StackTrace;
                }

            }
            mess.MessageType = "/Images/icon-question.gif";
            mess.ShowErrorDetail = showDetailError;
            
            if (mess.ShowErrorDetail)
            {
                mess.StackTrace = ex.StackTrace;
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
