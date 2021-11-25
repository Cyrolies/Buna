using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// Summary description for Enumerations.
    /// </summary>
    public class Enumerations
    {
        public static string GetEnumerationText(Type type, object enumValue)
        {
            return Enum.Parse(type, enumValue.ToString()).ToString();
        }
        public static int GetEnumerationValue(Type type, object enumValue)
        {
            return Convert.ToInt32(Enum.Parse(type, enumValue.ToString()));
        }
        public static Array GetEnumerationValue(Type type)
        {
            return Enum.GetValues(type);
        }

		public enum Permissions //Based on StcData table
		{
			None = 24,
			View = 11,
			Add = 10,
			Edit = 9,
			Delete = 8,
		}
		
		//public enum Database
		//{
		//    DsdDB = 0,
		//    DsdManagerDB = 1
		//}
		//public enum EventStatus
		//{
		//    ERRO,
		//    DONE,
		//    RELE,
		//    WARN
		//}

		//public enum ControlType
		//{
		//    TextBox = 12,	
		//    CheckBox = 13,
		//    NumericBox = 14,
		//    CurrencyBox = 15,
		//    PercentageBox = 16,
		//    ComboBox = 17,
		//    Calendar = 18,
		//    RangeSlider = 19,
		//    TreeViewCheckBox = 20,
		//    UploadFileBox = 21,
		//    Grid = 22,
		//    TextArea = 23

		//}

		//public enum FieldDataType
		//{
		//    nvarchar = 11,
		//    bit = 12,
		//    integer = 14,
		//    dec = 15,
		//    date_time = 16,
		//    smalldatetime = 17,
		//    List = 481
		//}

		//public enum  EntityFieldDataType
		//{

		// List =1,               
		// nvarchar =2,            
		// bit = 3,                 
		// integer = 4,                
		// decimals = 5,             
		// datetime = 6,            
		// smalldatetime = 7,       
		// timestamp =8,           
		//}

		public enum ModalContext
        {
            EzFloAglEntities = 1,
            DeltaEntities = 2,
            BMOBILEEntities = 3,
            EzFloDSDEntities = 4,
            EzFloManagerEntities =5,
            EzFloOTIEntities = 6, 
            EzFloAGLModel = 7,
            DALEFModel = 8,
           
        }
		public enum SetupDataType
		{
			Language = 1,
			Theme = 2,
			UserAction = 3,
			ActivityGroup = 4,
			ClientType = 5,
			ProjectType = 6,
			Currency = 7,
			ProjectPhase = 8,
			Version = 9,
			BugStatus = 10,
			BugSeverity = 11,
			BugType = 12,
			ContactType = 13,
			Title = 14,
			PreferredContactMethod = 15,
			PreferredCorresspondenceMethod = 16,
			IdentityType = 17,
			ProjectStatus = 18,
			TaskStatus = 19,
			Departments = 20,
		}

		public enum StaticDataType
		{
			Status = 1,
			Permission = 2,
			EntityFieldDataType = 16,
			ControlType = 3,

		}

		public enum SupervisionStatus
        {
            Approved = 1,
            Pending = 2,
            PendingUsable = 3,
            PendingDeletion = 4,
            RejectedApplication = 5,
            Rejected = 6,
        }

        //public enum FormMode
        //{
        //    View = 1,
        //    Add = 2,
        //    Edit = 3,
        //}

	
		public enum MessageType
		{
			Error,
			Information,
			ConcurrencyError,
			Question,
		}

		public enum RepositoryAction
		{
			Add = 1,
			Update = 2,
			Delete = 3,
			Get = 4,
			Gets = 5,

		}

		////public enum Language
		//{
		//    English = 420,
		//    Afrikaanse = 421,

		//}
	}
}
