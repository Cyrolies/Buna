using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DSDBLL
{

		[XmlRoot(ElementName = "students")]
		public class Students
		{
			[XmlElement(ElementName = "ID")]
			public string ID { get; set; }
			[XmlElement(ElementName = "LastName")]
			public string LastName { get; set; }
			[XmlElement(ElementName = "FirstName")]
			public string FirstName { get; set; }
			[XmlElement(ElementName = "MiddleName")]
			public string MiddleName { get; set; }
			[XmlElement(ElementName = "BirthDate")]
			public string BirthDate { get; set; }
			[XmlElement(ElementName = "PassNo")]
			public string PassNo { get; set; }
			[XmlElement(ElementName = "Gender")]
			public string Gender { get; set; }
			[XmlElement(ElementName = "Photo")]
			public string Photo { get; set; }
			[XmlElement(ElementName = "ParentID")]
			public string ParentID { get; set; }
			[XmlElement(ElementName = "Email")]
			public string Email { get; set; }
			[XmlElement(ElementName = "Cell")]
			public string Cell { get; set; }
			[XmlElement(ElementName = "AdmNo")]
			public string AdmNo { get; set; }
			[XmlElement(ElementName = "StatusID")]
			public string StatusID { get; set; }
			[XmlElement(ElementName = "NationalityID")]
			public string NationalityID { get; set; }
			[XmlElement(ElementName = "EnrollDate")]
			public string EnrollDate { get; set; }
			[XmlElement(ElementName = "TransfFrom")]
			public string TransfFrom { get; set; }
			[XmlElement(ElementName = "TransInstNo")]
			public string TransInstNo { get; set; }
			[XmlElement(ElementName = "LURNo")]
			public string LURNo { get; set; }
			[XmlElement(ElementName = "State")]
			public string State { get; set; }
			[XmlElement(ElementName = "FirstTimeState")]
			public string FirstTimeState { get; set; }
			[XmlElement(ElementName = "PrevState")]
			public string PrevState { get; set; }
			[XmlElement(ElementName = "Immigrant")]
			public string Immigrant { get; set; }
			[XmlElement(ElementName = "PopulationGroup")]
			public string PopulationGroup { get; set; }
			[XmlElement(ElementName = "LangHome")]
			public string LangHome { get; set; }
			[XmlElement(ElementName = "BoardingStatus")]
			public string BoardingStatus { get; set; }
			[XmlElement(ElementName = "LangTeach")]
			public string LangTeach { get; set; }
			[XmlElement(ElementName = "LangPref")]
			public string LangPref { get; set; }
			[XmlElement(ElementName = "PSNPBenefit")]
			public string PSNPBenefit { get; set; }
			[XmlElement(ElementName = "EarlyChildhoodDev")]
			public string EarlyChildhoodDev { get; set; }
			[XmlElement(ElementName = "Foundation")]
			public string Foundation { get; set; }
			[XmlElement(ElementName = "Interesen")]
			public string Interesen { get; set; }
			[XmlElement(ElementName = "GET")]
			public string GET { get; set; }
			[XmlElement(ElementName = "FET")]
			public string FET { get; set; }
			[XmlElement(ElementName = "Insurance")]
			public string Insurance { get; set; }
			[XmlElement(ElementName = "InsuranceNo")]
			public string InsuranceNo { get; set; }
			[XmlElement(ElementName = "FamilyDoctor")]
			public string FamilyDoctor { get; set; }
			[XmlElement(ElementName = "FamilyDocTel")]
			public string FamilyDocTel { get; set; }
			[XmlElement(ElementName = "EyesProblems")]
			public string EyesProblems { get; set; }
			[XmlElement(ElementName = "Glasses")]
			public string Glasses { get; set; }
			[XmlElement(ElementName = "Lenses")]
			public string Lenses { get; set; }
			[XmlElement(ElementName = "WearGlasses")]
			public string WearGlasses { get; set; }
			[XmlElement(ElementName = "EyesSeating")]
			public string EyesSeating { get; set; }
			[XmlElement(ElementName = "EyesDoctor")]
			public string EyesDoctor { get; set; }
			[XmlElement(ElementName = "EyesDocTel")]
			public string EyesDocTel { get; set; }
			[XmlElement(ElementName = "EarsProblems")]
			public string EarsProblems { get; set; }
			[XmlElement(ElementName = "HearingAid")]
			public string HearingAid { get; set; }
			[XmlElement(ElementName = "Grommets")]
			public string Grommets { get; set; }
			[XmlElement(ElementName = "GlueEar")]
			public string GlueEar { get; set; }
			[XmlElement(ElementName = "EarSeating")]
			public string EarSeating { get; set; }
			[XmlElement(ElementName = "EarDoctor")]
			public string EarDoctor { get; set; }
			[XmlElement(ElementName = "EarDocTel")]
			public string EarDocTel { get; set; }
			[XmlElement(ElementName = "SpeechProblems")]
			public string SpeechProblems { get; set; }
			[XmlElement(ElementName = "SpeechTherapy")]
			public string SpeechTherapy { get; set; }
			[XmlElement(ElementName = "OrthopProblem")]
			public string OrthopProblem { get; set; }
			[XmlElement(ElementName = "OrthopLimitations")]
			public string OrthopLimitations { get; set; }
			[XmlElement(ElementName = "HeartProblems")]
			public string HeartProblems { get; set; }
			[XmlElement(ElementName = "HeartLimitations")]
			public string HeartLimitations { get; set; }
			[XmlElement(ElementName = "Allergy1")]
			public string Allergy1 { get; set; }
			[XmlElement(ElementName = "Allergy1Med")]
			public string Allergy1Med { get; set; }
			[XmlElement(ElementName = "Allergy2")]
			public string Allergy2 { get; set; }
			[XmlElement(ElementName = "Allergy2Med")]
			public string Allergy2Med { get; set; }
			[XmlElement(ElementName = "Allergy3")]
			public string Allergy3 { get; set; }
			[XmlElement(ElementName = "Allergy3Med")]
			public string Allergy3Med { get; set; }
			[XmlElement(ElementName = "MedInstr1")]
			public string MedInstr1 { get; set; }
			[XmlElement(ElementName = "MedInstr1Desc")]
			public string MedInstr1Desc { get; set; }
			[XmlElement(ElementName = "MedInstr2")]
			public string MedInstr2 { get; set; }
			[XmlElement(ElementName = "MedInstr2Desc")]
			public string MedInstr2Desc { get; set; }
			[XmlElement(ElementName = "MedInstr3")]
			public string MedInstr3 { get; set; }
			[XmlElement(ElementName = "MedInstr3Desc")]
			public string MedInstr3Desc { get; set; }
			[XmlElement(ElementName = "MedInstr4")]
			public string MedInstr4 { get; set; }
			[XmlElement(ElementName = "MedInstr4Desc")]
			public string MedInstr4Desc { get; set; }
			[XmlElement(ElementName = "MedicalOther")]
			public string MedicalOther { get; set; }
			[XmlElement(ElementName = "MemberName")]
			public string MemberName { get; set; }
			[XmlElement(ElementName = "MemberID")]
			public string MemberID { get; set; }
			[XmlElement(ElementName = "MedicalConsitions")]
			public string MedicalConsitions { get; set; }
			[XmlElement(ElementName = "CampusName")]
			public string CampusName { get; set; }
			[XmlElement(ElementName = "InGrade")]
			public string InGrade { get; set; }
			[XmlElement(ElementName = "InPhase")]
			public string InPhase { get; set; }
		}

		[XmlRoot(ElementName = "root")]
		public class Root
		{
			[XmlElement(ElementName = "students")]
			public List<Students> Students { get; set; }
		}

}

