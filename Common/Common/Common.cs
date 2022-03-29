using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using System.IO.Compression;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Drawing;
using System.Configuration;
using System.Net.Mail;

namespace Common
{
    public static class Common
    {
        public static void SendEmail(string fromAddress, string emailTo, string subject, string body)
        {
            try
            {
                //Send Email to User


                if (ConfigurationManager.AppSettings["MailSmtp"].ToLower() == "gmail")
                {
                    //GMAIL smtp
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.UseDefaultCredentials = true;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["GmailAddress"], ConfigurationManager.AppSettings["GmailPassword"]);
                    smtp.Send(mail);
                }
                if (ConfigurationManager.AppSettings["MailSmtp"].ToLower() == "cov")
                {
                    //cov mail.cov19screen.com
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("info@buna.africa"); 
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    SmtpClient smtp = new SmtpClient("mail5013.site4now.net");
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = 8889;
                    smtp.EnableSsl = false;
                    smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["CovAddress"], ConfigurationManager.AppSettings["CovPassword"]);

                    //smtp.Credentials = new System.Net.NetworkCredential("sdpschool@cov19screen.com", "ThornFarm1!");
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<FilterField> GetFilters(ReadOnlyCollection<string> sSearch_, ReadOnlyCollection<string> mDataProp_)
        {
            var filteredColumns = new List<FilterField>();
            for (int i = 0; i < sSearch_.Count; i++)
            {
                if (sSearch_[i].Length > 0)
                {
                    //Any date range filter
                    if (sSearch_[i].ToString().Contains("-yadcf_delim-"))
                    {
                        string datefilter = sSearch_[i].ToString().Replace("-yadcf_delim-", "|");
                        string fromDate = datefilter.Substring(0, datefilter.IndexOf("|")).Replace("/", "-");
                        string toDate = datefilter.Substring(datefilter.IndexOf("|") + 1).Replace("/", "-");
                        if (fromDate.Length > 0)
                        {
                            filteredColumns.Add(new FilterField
                            {
                                Property = mDataProp_[i],
                                Operator = ">=",
                                Value = fromDate
                            });
                        }
                        if (toDate.Length > 0)
                        {
                            filteredColumns.Add(new FilterField { Property = mDataProp_[i], Operator = "<=", Value = toDate });
                        }
                    }
                    else
                    {
                        //var reg = @"^(-?[1-9]+\\d*([.]\\d+)?)$|^(-?0[.]\\d*[1-9]+)$|^0$"; //check for numbers and decimals
                        //if(Regex.Match(sSearch_[i], reg, RegexOptions.IgnoreCase).Success)
                        Regex regex = new Regex(@"[-0-9]");
                        
                        //All boolean searches
                        if (sSearch_[i].Equals("False") || sSearch_[i].Equals("True"))
                        {
                            filteredColumns.Add(new FilterField { Property = mDataProp_[i].Replace("CheckBox", ""), Operator = "==", Value = sSearch_[i] });
                        }
                        else if (mDataProp_[i].Substring(mDataProp_[i].Length - 2) == "ID")//All foregin key field names must end with ID 
                        {
                            filteredColumns.Add(new FilterField { Property = mDataProp_[i], Operator = "=", Value = sSearch_[i] });
                        }
                        //Took this out number could also be in a string field filter
                        //Treat all numbers as string fields
                        //else if (regex.IsMatch(sSearch_[i]))
                        //{
                        //    filteredColumns.Add(new FilterField { Property = mDataProp_[i], Operator = ">=", Value = sSearch_[i] });
                        //}
                        else//If text then like search
                        {
                           filteredColumns.Add(new FilterField { Property = mDataProp_[i], Operator = "like", Value = sSearch_[i] });
                        }
                    }
                }
            }

            return filteredColumns;
        }

        public static Bitmap Resize_Image(Stream streamImage, int maxWidth, int maxHeight)
        {
            Bitmap originalImage = new Bitmap(streamImage);
            int newWidth = originalImage.Width;
            int newHeight = originalImage.Height;
            double aspectRatio = Convert.ToDouble(originalImage.Width) / Convert.ToDouble(originalImage.Height);

            if (aspectRatio <= 1 && originalImage.Width > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = Convert.ToInt32(Math.Round(newWidth / aspectRatio));
            }
            else if (aspectRatio > 1 && originalImage.Height > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = Convert.ToInt32(Math.Round(newHeight * aspectRatio));
            }
            return new Bitmap(originalImage, newWidth, newHeight);
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string Compress(byte[] bytes)
        {            
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(bytes, 0, bytes.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }
        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static List<T> ToList2<T>(this object Input)
        {
            if (Input is IEnumerable)
                return ((IEnumerable)Input).Cast<T>().ToList();
            return new List<T>() { (T)Input };
        }

        public static string AddSpace(string text)
            {
                switch (text)
                {
                    case "PendingUsable": return "Pending Usable";
                    case "PendingDeletion": return "Pending Deletion";
                    case "RejectedApplication": return "Pending Application";
                    default: return text;
                }
            }

        public static string ClearEntityFieldName(string fieldname)
            {
                string returnStr = string.Empty;
                //TODO generate a list of fields which are isforeignkey=true and suffix ends with ID. to loop through 
                switch (fieldname)
                {
				//case "Org":
				//returnStr = "Organization";
				//break;
				case "OrgID":
					returnStr = "Organization";
					break;
				case "BugCreatedByContactID":
                    returnStr = "Contact";
                    break;
                    case "AssignedContactID":
                    returnStr = "Contact";
                    break;
                    case "CreatedByID":
                    returnStr = "User";
                    break;
                    case "ChangedByID":
                    returnStr = "User";
                    break;
                    case "SupervisorID":
                    returnStr = "User";
                    break;
                    case "ShippingPoint":
                    returnStr = "CodeDescription";
                    break;
                    case "ParentID":
                    returnStr =  "Parent";
                    break;
                    case "ParentAssetID":
                    returnStr = "Asset";
                    break;
                    case "AssignedToID":
                    returnStr = "Person";
                    break;
                    case "UserRoleID":
                    returnStr = "UserRole";
                    break;
                    case "ActivityID":
                    returnStr = "Activity";
                    break;
                    case "EntityFieldDataTypeID":
                    returnStr = "EntityFieldDataType";
                    break;
                    case "ComboBoxDisplayFieldID":
                    returnStr = "ComboBoxDisplayField";
                    break;
                    case "UserID":
                    returnStr = "User";
                    break;
                    case "VisitListID":
                    returnStr = "VisitList";
                    break;
                    case "CustomerID":
                    returnStr = "Customer";
                    break;
                    case "PriceListID":
                    returnStr = "PriceList";
                    break;
                    case "ProductID":
                    returnStr = "Product";
                    break;
                    case "PlantID":
                    returnStr = "Plant";
                    break;
                    case "SurveyID":
                    returnStr = "Survey";
                    break;
                    case "DiscountID":
                    returnStr = "Discount";
                    break;
                    case "DiscountId":
                    returnStr = "Discount";
                    break;
                    case "MobileUserID":
                    returnStr = "MobileUser";
                    break;
                    case "EntityMenuParentID":
                    returnStr = "EntityMenu";
                    break;
                    case "EntityID":
                    returnStr = "Entity";
                    break;
                    case "TechnicianID":
                    returnStr = "User";
                    break;
                    case "PartID":
                        returnStr = "Part";
                        break;
                    case "WorkOrderID":
                        returnStr = "WorkOrder";
                        break;
                    case "UsedOnAssetID":
                        returnStr = "Asset";
                        break;
                    case "AssetID":
                        returnStr = "Asset";
                        break;
                    case "PersonID":
                        returnStr = "Person";
                        break;
                case "SupplierID":
                    returnStr = "Supplier";
                    break;
                case "ID":
                        returnStr = "";
                        break;
                        default:
                        break;
                }

                return returnStr;
            }
           
        public static DataSet ToDataSet<T>(this IEnumerable<T> collection, string dataTableName)
            {
                if (collection == null)
                {
                    throw new ArgumentNullException("collection");
                }

                if (string.IsNullOrEmpty(dataTableName))
                {
                    throw new ArgumentNullException("dataTableName");
                }

                DataSet data = new DataSet("NewDataSet");
                data.Tables.Add(FillDataTable(dataTableName, collection));
                return data;
            }

        private static DataTable FillDataTable<T>(string tableName,
            IEnumerable<T> collection)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();

                DataTable dt = CreateDataTable<T>(tableName,
                collection, properties);

                IEnumerator<T> enumerator = collection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    dt.Rows.Add(FillDataRow<T>(dt.NewRow(),
                   enumerator.Current, properties));
                }

                return dt;
            }

        private static DataRow FillDataRow<T>(DataRow dataRow,
            T item, PropertyInfo[] properties)
            {
                foreach (PropertyInfo property in properties)
                {
                   // if(property.PropertyType is System.Web.Web )
                    dataRow[property.Name.ToString()] = property.GetValue(item, null);
                }

                return dataRow;
            }

        private static DataTable CreateDataTable<T>(string tableName,
            IEnumerable<T> collection, PropertyInfo[] properties)
            {
                DataTable dt = new DataTable(tableName);

                foreach (PropertyInfo property in properties)
                {
                    dt.Columns.Add(property.Name.ToString());
                }

                return dt;
            }

        private static DataSet CreateDataSet<T>(List<T> list)
            {
                //list is nothing or has nothing, return nothing (or add exception handling)
                if (list == null || list.Count == 0) { return null; }

                //get the type of the first obj in the list
                var obj = list[0].GetType();

                //now grab all properties
                var properties = obj.GetProperties();

                //make sure the obj has properties, return nothing (or add exception handling)
                if (properties.Length == 0) { return null; }

                //it does so create the dataset and table
                var dataSet = new DataSet();
                var dataTable = new DataTable();

                //now build the columns from the properties
                var columns = new DataColumn[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    columns[i] = new DataColumn(properties[i].Name, properties[i].PropertyType);
                }

                //add columns to table
                dataTable.Columns.AddRange(columns);

                //now add the list values to the table
                foreach (var item in list)
                {
                    //create a new row from table
                    var dataRow = dataTable.NewRow();

                    //now we have to iterate thru each property of the item and retrieve it's value for the corresponding row's cell
                    var itemProperties = item.GetType().GetProperties();

                    for (int i = 0; i < itemProperties.Length; i++)
                    {
                        dataRow[i] = itemProperties[i].GetValue(item, null);
                    }

                    //now add the populated row to the table
                    dataTable.Rows.Add(dataRow);
                }

                //add table to dataset
                dataSet.Tables.Add(dataTable);

                //return dataset
                return dataSet;
            }
    }
    
}
