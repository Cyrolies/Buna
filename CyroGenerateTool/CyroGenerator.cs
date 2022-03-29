using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;
using DALEFModel;
using DSDBLL;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;
using CyroCommon;


namespace CyroGenerateTool
{
    public class CyroGenerator
    {
        private string _connString = string.Empty;
        private string _defaultDB = string.Empty;
        private string modelName = string.Empty;
        private Database _database = null;        
        private List<string> _lstTables = null;

        private string _tbCtrlMngSvcName = string.Empty;

        public string CtrlMngSvcName
        {
            get { return _tbCtrlMngSvcName; }
            set { _tbCtrlMngSvcName = value; }
        }

        private string _folderToSaveMetaData = string.Empty;

        private string _applicationName = string.Empty;

        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        public string FolderToSaveMetaData
        {
            get;
            set;
        }

        public string FolderToSavePartialClass
        {
            get ;
            set ;
        }
        private string _folderToSaveEditTemplates = string.Empty;

        public string FolderToSaveEditTemplates
        {
            get { return _folderToSaveEditTemplates; }
            set { _folderToSaveEditTemplates = value; }
        }

        private string _folderToSaveSetupForms = string.Empty;
        public string FolderToSaveSetupForms
        {
            get { return _folderToSaveSetupForms; }
            set { _folderToSaveSetupForms = value; }
        }

        private string _FolderPathUIController = string.Empty;
        public string FolderPathUIController
        {
            get { return _FolderPathUIController; }
            set { _FolderPathUIController = value; }
        }
        private string _FolderPathAPIManager = string.Empty;

        public string FolderPathAPIManager
        {
            get { return _FolderPathAPIManager; }
            set { _FolderPathAPIManager = value; }
        }
        private string _FolderPathAPIController = string.Empty;

        public string FolderPathAPIController
        {
            get { return _FolderPathAPIController; }
            set { _FolderPathAPIController = value; }
        }
        private string _FolderPathAPIControllerInterface = string.Empty;

        public string FolderPathAPIControllerInterface
        {
            get { return _FolderPathAPIControllerInterface; }
            set { _FolderPathAPIControllerInterface = value; }
        }

        private bool _generateMetadataFile = false;

        public bool GenerateMetadataFile
        {
            get { return _generateMetadataFile; }
            set { _generateMetadataFile = value; }
        }
        private bool _generateSvcFile = false;
        public bool GeneratePartialClass
        {
            get ;
            set; 
        }
        public bool GenerateSvcFile
        {
            get { return _generateSvcFile; }
            set { _generateSvcFile = value; }
        }
        private bool _generateEditTemplates = false;

        public bool GenerateEditTemplates
        {
            get { return _generateEditTemplates; }
            set { _generateEditTemplates = value; }
        }
        private bool _generateSetupForms = false;

        public bool GenerateSetupForms
        {
            get { return _generateSetupForms; }
            set { _generateSetupForms = value; }
        }

        private bool _generateManagerClass = false;
        public bool GenerateManagerClass
        {
            get { return _generateManagerClass; }
            set { _generateManagerClass = value; }
        }

        private bool _generateControllers = false;
        public bool GenerateControllers
        {
            get { return _generateControllers; }
            set { _generateControllers = value; }
        }

        private string _modelNamespace = string.Empty;

        public string ModelNamespace
        {
            get { return _modelNamespace; }
            set { _modelNamespace = value; }
        }
               

        private Dictionary<string, string> dctFriendlyNames = null;        
        //private Dictionary<string, string> dctOrderByFields = null;

        /// <summary>
        /// Constructor for the BLL code generation
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tables"></param>
        /// <param name="strOutputPathBLL"></param>
        public CyroGenerator(Database database, List<string> tables)
        {
            _database = database;
            _lstTables = tables;
            
            _connString = ConfigurationSettings.AppSettings["connectionString"];
            _defaultDB = ConfigurationSettings.AppSettings["defaultDB"];

         //   this.GenerateFriendlyNames();            
   
        }

        public void GenerateFriendlyNames()
        {
            dctFriendlyNames = new Dictionary<string, string>();

            foreach (string strTableName in _lstTables)
            {
                Table objTable = _database.Tables[strTableName];

                //List<string> lstPieces = objTable.Name.Split('_').ToList();
                //string friendlyName = lstPieces.Last().ToLower();

                string friendlyName = objTable.Name;

               // friendlyName = friendlyName.First().ToString().ToUpper() + friendlyName.Substring(1);

                dctFriendlyNames.Add(objTable.Name, friendlyName);
            }
        }

        public void Generate()
        {
            try
            {
                string _className = string.Empty;
                string _friendlyName = string.Empty;
                modelName = this.ModelNamespace.Substring(this.ModelNamespace.LastIndexOf('\\') + 1).Replace(".edmx", "");

                //Create a file for each entity selected
                foreach (string strTableName in _lstTables)
                {
                    //Table objTable = _database.Tables[strTableName];                
                    //_className = objTable.Name;

                    _className = strTableName;

                    if (this.GeneratePartialClass)
                        this.GeneratePartialClasses(strTableName);

                    if (this.GenerateMetadataFile)
                        this.GenerateMetadataClasses(strTableName);

                    if (GenerateEditTemplates)
                        this.GenerateEditTemplateViewsJqueryDataTable(strTableName);
                    //this.GenerateEditTemplateViews(strTableName);
                    if (this.GenerateSetupForms)
                        this.GenerateSetupFormJqueryDataTable(strTableName);
                    // this.GenerateSetupForm(strTableName);

                }
                //Creates methods for all entities selected into the same file below
                if (GenerateControllers)
                    //  GenerateControllerApiClasses(_lstTables);
                    GenerateControllerForPortalToApiClasses(_lstTables);
                //GenerateControllerClasses(_lstTables);
                if (GenerateManagerClass)
                    GenerateManagerClasses(_lstTables);
                if (GenerateSvcFile)
                    this.GenerateControllerApiClasses(_lstTables);
                //GenerateServiceClasses(_lstTables);
                //GenerateServiceInterfaceClasses(_lstTables);

            }catch(Exception ex)
			{
                throw ex;
            }

        }

        private void GenerateEditTemplateViewsJqueryDataTable(string objName)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder scriptInsert = new StringBuilder();
            AdminManager manager = new AdminManager();
            OperationStatus status = new OperationStatus();
            Entity entity = manager.GetEntity(objName, true);
            if (entity == null || entity.EntityField.Count() == 0)
                return;
            try
            {
                //LOOP entity properties from entity table
                //TODO add application UI name from application table 

                sb.AppendLine("@using " + ApplicationName + ";");
                //TODO add EF model name from application table
                sb.AppendLine("@model DALEFModel." + objName);
                sb.AppendLine("<script src = \"@Url.Content(\"~/Scripts/jquery.validate.unobtrusive.min.js\")\" ></script>");
                sb.AppendLine(Tab(1) + "<div class=\"modal fade\" id=\"modal1\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"myModalLabel\" aria-hidden=\"true\">");
                sb.AppendLine(Tab(2) + "<div class=\"modal-dialog\">");
                sb.AppendLine(Tab(3) + "<div class=\"content\">");
                sb.AppendLine(Tab(4) + "<div class=\"box-header\">");
                sb.AppendLine(Tab(5) + "<button type = \"button\" name=\"btnclose\" id=\"btnclose\" class=\"close\" data-dismiss=\"modal\">x</button>");
                sb.AppendLine(Tab(5) + "<h4 class=\"modal-title\">@Html.Label(@Html.Encode(Localizer.Current.GetString(\"" + objName + "\")), new { @class = \"col-md-2 control-label\" })</h4>");
                sb.AppendLine(Tab(4) + "</div>");
                sb.AppendLine(string.Empty);
               // sb.AppendLine(Tab(4) + "<div class=\"box-default\">");
                sb.AppendLine(Tab(4) + "<div id = \"result\" ></div>");
                sb.AppendLine(Tab(4) + "@using(Ajax.BeginForm(\"Edit" + objName + "\", \"" + entity.MngCtlrName + "\", new AjaxOptions");
                sb.AppendLine(Tab(5) + "{");
                sb.AppendLine(Tab(6) + "HttpMethod = \"POST\",");
                sb.AppendLine(Tab(6) + "InsertionMode = InsertionMode.Replace,");
                sb.AppendLine(Tab(6) + "UpdateTargetId = \"result\"");
                sb.AppendLine(Tab(5) + "}))");
                sb.AppendLine(Tab(5) + "{");
                sb.AppendLine(Tab(6) + "try {");
                //Start of Main Panel
                sb.AppendLine(Tab(7) + "@* @Html.AntiForgeryToken()*@");
                sb.AppendLine(Tab(7) + "@Html.ValidationSummary(true)");
                sb.AppendLine(string.Empty);

                if (entity.IsTabbedForm)
                {
                    //Refetched EntityField on entity because using include on GetEntity gives issues with related entityField on entitytfields 
                    
                    List<EntityField> fields = manager.ListEntityField(entity.Name).Where(o => o.TabName.Length > 0 && o.IsActive == true && o.EntityFieldDataTypeID != 12).OrderByDescending(o => o.TabName).ToList();
                    IEnumerable<IGrouping<string, EntityField>> tabs = fields.OrderBy(n => n.TabOrderNo).GroupBy(o => o.TabOrderNo.ToString());

                  //  sb.AppendLine(Tab(4) + "<div class=\"panel panel-default\">");
                    sb.AppendLine(Tab(8) + "<div is=\"Tabs\" role=\"tabpanel\">");
                    sb.AppendLine(Tab(8) + "<ul class=\"nav nav-tabs\" role=\"tablist\">");
                    int countTab = 0;
                        foreach (IGrouping<string, EntityField> tab in tabs)
                        {
                            if (countTab == 0)//Make first tab active tab
                            {
                                sb.AppendLine(Tab(9) + "<li class=\"active\" ><a href = \"#" + tab.ToList()[0].TabName.Trim() + "\" aria-controls = \"#" + tab.ToList()[0].TabName.Trim() + "\" role=\"tab\" data-toggle=\"tab\">" + tab.ToList()[0].TabName.Trim() + "</a></li>");

                            }
                            else
                            {
                                sb.AppendLine(Tab(9) + "<li><a href = \"#" + tab.ToList()[0].TabName.Trim() + "\" aria-controls = \"#" + tab.ToList()[0].TabName.Trim() + "\" role=\"tab\" data-toggle=\"tab\">" + tab.ToList()[0].TabName.Trim() + "</a></li>");
                            }
                            countTab++;
                        }
                        sb.AppendLine(Tab(8) + "</ul>");
                        sb.AppendLine(Tab(9) + "<div class=\"tab-content\">");
                        countTab = 0;//set zero
                        foreach (IGrouping<string, EntityField> tab in tabs)
                            {
                                if (countTab == 0)//Make first tab active tab
                                {
                                    //Groupbox can't span more than one tab
                                    sb.AppendLine(Tab(10) + "<div class=\"tab-pane active\" role=\"tabpanel\" id=\"" + tab.ToList()[0].TabName.Trim() + "\">");
						        }
                                else
						        {
                                    //Groupbox can't span more than one tab
                                    sb.AppendLine(Tab(10) + "<div class=\"tab-pane\" role=\"tabpanel\" id=\"" + tab.ToList()[0].TabName.Trim() + "\">");
                                }
                                //Creates a list of EntityField in each groupbox
                                //IEnumerable<EntityField> groupedOrderedBy = tab.OrderByDescending(o => o.GroupBoxOrderNo);
                                IEnumerable<IGrouping<string, EntityField>> grouped = tab.OrderBy(o => o.GroupBoxOrder).GroupBy(o => o.GroupBoxName);
						        foreach (IGrouping<string, EntityField> group in grouped)
						        {
							        if (group.ToList()[0].GroupBoxName != null)
							        {
								        //Creates fields in GroupBox
								        sb.AppendLine(GenerateGroupBoxForJqueryNew(group.OrderBy(o => o.ControlOrderNo).ToList()));
							        }
							        else
							        {
								        //Creates fields with out GroupBox one below the other
								        foreach (EntityField ent in group.OrderBy(o => o.ControlOrderNo).ToList())
								        {
									        sb.AppendLine(this.GenerateEditJqueryControl(ent));

								        }
							        }
							        //Get any scripts for controls
							        foreach (EntityField ent in group.OrderBy(o => o.ControlOrderNo).ToList())
							        {
                                        if (ent.ScriptInsert != null && ent.IsForeignKey == false)
                                        {
                                            scriptInsert.Append(ent.ScriptInsert?.Trim() + Environment.NewLine);
                                            scriptInsert.Append("    ");
                                        }
							        }
						        }

						        sb.AppendLine(Tab(10) + "</div>");
                                countTab++;
                            }

                        sb.AppendLine(Tab(9) + "</div>");
                        sb.AppendLine(Tab(8) + "</div>");

                    // sb.AppendLine(Tab(4) + "</div>");
                    //sb.AppendLine(Tab(4) + "</div>");
                }
                else
                {
                    //Creates a list of EntityField in each groupbox
                    //Refetched EntityField on entity because using include on GetEntity gives issues with related entityField on entitytfields 
                    List<EntityField> fields = manager.ListEntityField(entity.Name).Where(o => o.IsActive == true && o.EntityFieldDataTypeID != 12).ToList();
                    IEnumerable<IGrouping<string, EntityField>> grouped = fields.OrderBy(o => o.GroupBoxOrder).GroupBy(o => o.GroupBoxName);
                    foreach (IGrouping<string, EntityField> group in grouped)
                    {
                        if (group.ToList()[0].GroupBoxName != null && group.ToList()[0].GroupBoxName.Length > 0)
                        {
                            //Creates fields in GroupBox
                            sb.AppendLine(GenerateGroupBoxForJqueryNew(group.OrderBy(o => o.ControlOrderNo).ToList()));
                        }
                        else
                        {
                            //Creates fields with out GroupBox one below the other
                            foreach (EntityField ent in group.OrderBy(o => o.ControlOrderNo).ToList())
                            {
                                sb.AppendLine(GenerateEditJqueryControl(ent));

                            }
                        }
                        //Get any scripts for controls
                        foreach (EntityField ent in group.OrderBy(o => o.ControlOrderNo).ToList())
                        {
                            scriptInsert.Append(ent.ScriptInsert?.Trim() + Environment.NewLine);
                        }
                    }

                }

            
            sb.AppendLine(Tab(6) + "<div class=\"modal-footer\">");
            sb.AppendLine(Tab(6) + "<button type = \"submit\" class=\"btn btn-default\">@Localizer.Current.GetString(\"Save\")</button>");
            sb.AppendLine(Tab(6) + "</div>");
           
           
            sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(Tab(6) + "catch (Exception ex)");
            sb.AppendLine(Tab(6) + "{");
            sb.AppendLine(Tab(6) + "<script type = \"text/javascript\" >");
            sb.AppendLine(Tab(7) + "ShowNotification(false, '@ex.Message.Replace(\"'\",\"\\'\").Replace(\"\\r\\n\",\" <br/> \")');");
            sb.AppendLine(Tab(6) + "</script>");
            sb.AppendLine(Tab(6) + "}");

            sb.AppendLine(Tab(5) + "}");

            sb.AppendLine(Tab(3) + "</div>");
            sb.AppendLine(Tab(2) + "</div>");
            sb.AppendLine(Tab(1) + "</div>");

            sb.AppendLine(Tab(1) + "<script>");
            sb.AppendLine(Tab(2) + "$(document).ready(function () {");
            sb.AppendLine(Tab(2) + "$('#modal1').attr('class', 'modal fade').attr('aria-labelledby', 'myModalLabel');");
            sb.AppendLine(Tab(2) + "$('#modal1').modal('show');");
            sb.AppendLine(Tab(2) + "document.getElementById(\"btnclose\").onclick = function () { $('#tblGrid" + entity.Name + "').DataTable().ajax.reload(); };");
            sb.AppendLine(Tab(2) +  scriptInsert.ToString());
            sb.AppendLine(Tab(2) + "});");
            sb.AppendLine(Tab(1) + "</script>");
           // sb.AppendLine(Tab(1) + "</div>");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                //TODO add custom code parts
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(this.FolderToSaveEditTemplates + "\\" + entity.PathEditTemplate, "Edit" + objName + ".cshtml")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateEditJqueryControl(EntityField field)
        {
            try
            {
                if (!field.IsActive)
                { return ""; }
                StringBuilder str = new StringBuilder();

                if (field.IsHidden)//Just hidden entry created
                {
                    str.AppendLine(Tab(16) + "@Html.HiddenFor(o => o." + field.EntityFieldName + ")");
                    return str.ToString();
                }
                if (field.IsDisabled)//Hidden and normal disabled control created because disabled control does not send binded data back in form so hidden is added
                {
                    str.AppendLine(Tab(16) + "@Html.HiddenFor(o => o." + field.EntityFieldName + ")");
                }
                //if (!field.IsHidden)
                //{
                str.AppendLine(Tab(16) + "<div class=\"" + field.ControlScreenColumns + "\">");
                str.AppendLine(Tab(17) + "<div class=\"form-group\">");
				if (field.IsPrimaryKey)
				{
					str.AppendLine(Tab(18) + "@Html.HiddenFor(o => o." + field.EntityFieldName + ")");
				}
				if(field.StcControlTypeID != (int)Enumerations.ControlType.CopyCheckBox)//Add Label if not CopyCheckBox
				{
					str.AppendLine(Tab(18) + "@Html.CustomLabelFor(o => o." + field.EntityFieldName + ")");
                }
                if (field.StcControlTypeID == (int)Enumerations.ControlType.HeaderTextLabel)
                {
                    str.AppendLine(Tab(18) + "@Html.Label(@Html.Encode(Localizer.Current.GetString(\"" + field.DisplayName + "\")))");
                }

                // }

                if (field.StcControlTypeID == (int)Enumerations.ControlType.ComboBox)
                {
                    if (field.ComboBoxDisplayFieldID == null)
                    {
                        throw new Exception("No ComboBoxDisplayFieldID specified for = " + field.DisplayName);
                    }
                    
                    str.AppendLine(Tab(18) + "@Html.CustomDropDownListFor(st => st." + field.EntityFieldName + ")");
                    str.AppendLine(Tab(18) + "@Html.CustomValidationMessageFor(o => o." + field.EntityFieldName + ")");
                    
                }
                else if (field.StcControlTypeID == (int)Enumerations.ControlType.AutoCompleteTextBox || field.StcControlTypeID == (int)Enumerations.ControlType.CurrencyTextBox || field.StcControlTypeID == (int)Enumerations.ControlType.PercentageTextBox || field.StcControlTypeID == (int)Enumerations.ControlType.NumericTextBox || field.StcControlTypeID == (int)Enumerations.ControlType.TextBox || field.StcControlTypeID == (int)Enumerations.ControlType.Calendar || field.StcControlTypeID == (int)Enumerations.ControlType.TimePicker)
                {
                    str.AppendLine(Tab(18) + "@Html.CustomTextBoxFor(o => o." + field.EntityFieldName + ")");
                    str.AppendLine(Tab(18) + "@Html.CustomValidationMessageFor(o => o." + field.EntityFieldName + ")");
                }
                else if (field.StcControlTypeID == (int)Enumerations.ControlType.TextArea)
                {

                    str.AppendLine(Tab(18) + "@Html.CustomTextBoxAreaFor(o => o." + field.EntityFieldName + ")");
                    str.AppendLine(Tab(18) + "@Html.CustomValidationMessageFor(o => o." + field.EntityFieldName + ")");
                }
                else if (field.StcControlTypeID == (int)Enumerations.ControlType.RangeSlider)
                {
                    //TODO Add rangeslider text how to bind
                    //  str.AppendLine("<br/>");
                    //control
                    //        str.AppendLine("<%= Html.Telerik().RangeSlider<double>().Name("RangeSlider")");
                    //        str.AppendLine("<br/>");
                    //        str.AppendLine("<br/>");
                    //        str.AppendLine("<br/>");
                    //        str.AppendLine("<br/>");
                    //        str.AppendLine("<br/>");


                    //        .Min(Model.RangeSliderAttributes.MinValue.Value)
                    //        .Max(Model.RangeSliderAttributes.MaxValue.Value)
                    //        .Values(Model.RangeSliderAttributes.SelectionStart.Value,
                    //            Model.RangeSliderAttributes.SelectionEnd.Value)
                    //        .SmallStep(Model.RangeSliderAttributes.SmallStep.Value)
                    //        .LargeStep(Model.RangeSliderAttributes.LargeStep.Value)
                    //        .TickPlacement(Model.RangeSliderAttributes.TickPlacement.Value)
                    //        .Orientation(Model.RangeSliderAttributes.SliderOrientation.Value)
                    //%>
                }
                else if (field.StcControlTypeID == (int)Enumerations.ControlType.UploadFileBox)
                {
                    str.AppendLine(Tab(18) + "<%= Html.Telerik().Upload().Name(\"attachments\").Multiple((bool) ViewData[\"multiple\"]).Async(async => async " +
                                " .Save(\"Save\", \"Upload\") " +
                                " .Remove(\"Remove\", \"Upload\") " +
                                " .AutoUpload((bool) ViewData[\"autoUpload\"]))%>");
                }
                else if (field.StcControlTypeID == (int)Enumerations.ControlType.TreeViewCheckBox)
                {
                    
                }
                else if (field.StcControlTypeID == (int)Enumerations.ControlType.CheckBox)
                {
                   str.AppendLine(Tab(18) + "@Html.CustomCheckBoxFor(o => o." + field.EntityFieldName + ")");
                }
                else if (field.StcControlTypeID == (int)Enumerations.ControlType.CopyCheckBox)
                {
                    if(string.IsNullOrEmpty(field.ScriptInsert))
					{
                        throw new Exception("No script specified on CopyCheckBox control : " + field.EntityFieldName);
					}
                    str.AppendLine(Tab(18) + "@Html.Label(@Html.Encode(Localizer.Current.GetString(\"" + field.DisplayName + "\")))");
                    str.AppendLine(Tab(18) + "@Html.CheckBox(\"" + field.EntityFieldName + "\",false,new { @id = \""+ field.EntityFieldName + "\"})");
                    

                }

                str.AppendLine(Tab(17) + "</div>");
                str.AppendLine(Tab(16) + "</div>");
               
                return str.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateEditTemplateViews(string objName)
        {
            StringBuilder sb = new StringBuilder();
            BaseManager manager = new BaseManager();
            OperationStatus status = new OperationStatus();
            Entity entity = manager.GetEntity(objName, true);
            if (entity == null || entity.EntityField.Count() == 0)
                return;
            try
            {
               
                //LOOP entity properties from entity table

                sb.AppendLine("<%@ Control Language=\"C#\" Inherits=\"System.Web.Mvc.ViewUserControl<"+modelName+"." + entity.Name + ">\" %>");
                sb.AppendLine("<%@ Import Namespace=\"" + modelName + "\" %>");
                sb.AppendLine("<%@ Import Namespace=\"" + ApplicationName + "BLL\" %>");
                sb.AppendLine("<%@ Import Namespace=\"MVCLocalization\" %>");
                sb.AppendLine("<%@ Import Namespace=\"Common\" %>");
                 sb.AppendLine(string.Empty);
                //Start of Main Panel
                sb.AppendLine(Tab(1) + "<%= Html.ValidationSummary() %>");
                sb.AppendLine(string.Empty);
               
                if (entity.IsTabbedForm)
                {
                    //Refetched EntityField on entity becuase using includ eon GetEntity gives issues with related entityField on entitytfields 
                    List<EntityField> fields = manager.ListEntityField(entity.Name).Where(o => o.TabName.Length > 0).OrderByDescending(o => o.TabName).ToList();
                    IEnumerable<IGrouping<string, EntityField>> tabs = fields.OrderBy(n => n.TabOrderNo).GroupBy(o => o.TabOrderNo.ToString());
                    
                    sb.AppendLine(Tab(1) + "<% Html.Telerik().TabStrip()");
                    sb.AppendLine(Tab(1) + ".Name(\"TabStrip\")");
                    sb.AppendLine(Tab(1) + ".Items(tabstrip =>");
                    sb.AppendLine(Tab(1) + "{");

                    foreach (IGrouping<string, EntityField> tab in tabs)
                    {
                        //Create tab form controls stuff
                        //Groupbox can't span more than one tab

                        sb.AppendLine(Tab(2) + "tabstrip.Add()");
                        sb.AppendLine(Tab(2) + ".Text(Localizer.Current.GetString(\"" + tab.ToList()[0].TabName + "\"))");
                        sb.AppendLine(Tab(2) + ".Content(() =>");
                        sb.AppendLine(Tab(2) + "{%>");

                        //Creates a list of EntityField in each groupbox
                        //IEnumerable<EntityField> groupedOrderedBy = tab.OrderByDescending(o => o.GroupBoxOrderNo);
                        IEnumerable<IGrouping<string, EntityField>> grouped = tab.GroupBy(o => o.GroupBoxName);
                        foreach (IGrouping<string, EntityField> group in grouped)
                        {
                            if (group.ToList()[0].GroupBoxName != null)
                            {
                                //Creates fields in GroupBox
                                sb.AppendLine(GenerateGroupBox(group.OrderBy(o => o.ControlOrderNo).ToList(), entity.MaxNoFieldsInGrpBox));
                            }
                            else
                            {
                                //Creates fields with out GroupBox one below the other
                                foreach (EntityField ent in group.OrderBy(o => o.ControlOrderNo).ToList())
                                {
                                    sb.AppendLine(GenerateEditControl(ent));

                                }
                            }
                       }

                        sb.AppendLine(Tab(2) + " <%});");
                    }

                    sb.AppendLine(Tab(1) + " })");
                    sb.AppendLine(Tab(1) + ".SelectedIndex(0)");
                    sb.AppendLine(Tab(1) + ".Render();");
                    sb.AppendLine(Tab(1) + "%>");
            
                }
                else
                {
                    //Creates a list of EntityField in each groupbox
                    //Refetched EntityField on entity because using include on GetEntity gives issues with related entityField on entitytfields 
                    List<EntityField> fields = manager.ListEntityField(entity.Name).ToList();
                    IEnumerable<IGrouping<string, EntityField>> grouped = fields.OrderBy(o=>o.GroupBoxName).GroupBy(o => o.GroupBoxName);
                    foreach (IGrouping<string, EntityField> group in grouped)
                    {
                        if (group.ToList()[0].GroupBoxName != null)
                        {
                            //Creates fields in GroupBox
                            sb.AppendLine(GenerateGroupBox(group.OrderBy(o => o.ControlOrderNo).ToList(), entity.MaxNoFieldsInGrpBox));
                        }
                        else
                        {
                            //Creates fields with out GroupBox one below the other
                            foreach (EntityField ent in group.OrderBy(o => o.ControlOrderNo).ToList())
                            {
                                sb.AppendLine(GenerateEditControl(ent));

                            }
                        }
                    }

                }
                         
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
               //TODO add custom code parts
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(this.FolderToSaveEditTemplates + "\\"+entity.PathEditTemplate, "Edit"+ objName + ".ascx")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateGroupBoxForJquery(List<EntityField> fields, int amtControlsHeightInDiv)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                amtControlsHeightInDiv++;
                //Start of GroupBox
                str.AppendLine(Tab(6) + "<div id=\"" + fields[0].GroupBoxName + "Groupbox\" dir=\"ltr\" style=\"text-align:left;\"><fieldset><legend>@Html.Encode(Localizer.Current.GetString(\"GroupBox" + fields[0].GroupBoxName + "\"))</legend>");
                str.AppendLine(string.Empty);
                List<EntityField> firstDivFields = fields.Take(amtControlsHeightInDiv).ToList();

                //Get max Column width
                if (fields[0].GroupBoxWidth == null)
                {
                    str.AppendLine(Tab(6) + "<div class=\"col-md-6\"> ");
                }
                else
                {
                    str.AppendLine(Tab(6) + "<div class=\"" + fields[0].GroupBoxWidth + "\" > ");
                }
                //LOOP entity fields and add only as many as specified in entity.MaxNoFieldsInGroupBox to this left div column
                foreach (EntityField entField in firstDivFields.Take(amtControlsHeightInDiv))//NOTE WHAT HAPPENS WHEN LIST IS LESS THAN amt
                {
                    str.AppendLine(this.GenerateEditJqueryControl(entField));
                }

                str.AppendLine(Tab(6) + "</div>");
                //End of Left Div in GroupBox

                str.AppendLine(Tab(6) + "<div id=\"Divider1\" style=\"width: 30px;height:100%;float: right;  top: 5px; \" ></div>");

                //Remove fields already added in first div
                if (fields.Count() >= amtControlsHeightInDiv)
                {
                    fields.RemoveRange(0, amtControlsHeightInDiv);
                }
                else
                {
                    str.AppendLine(Tab(6) + "</fieldset></div>");
                    return str.ToString();
                }
                //if more fields exist continue with next div
                if (fields.Count() > 0)
                {
                    //Start of second div
                    int nextAmt = fields.Count();
                    string alignDiv = "right";
                    //If there more fields for third div then only add the next max field amt to second div
                    if (fields.Count() > amtControlsHeightInDiv)
                    {
                        nextAmt = amtControlsHeightInDiv;
                        alignDiv = "center";
                    }

                    //Start second Div in GroupBox
                    str.AppendLine(Tab(6) + "<div style=\"width: 45%;height:100%; float: " + alignDiv + "; top: 5px; \">");
                    //LOOP entity fields and add only as many as specified in entity.MaxNoFieldsInGroupBox to this rightt div column
                    foreach (EntityField entField in fields.Take(nextAmt))
                    {

                        str.AppendLine(GenerateEditJqueryControl(entField));
                        //  str.AppendLine(Tab(3) + "<%: Html.ValidationMessageFor(o => o." + entField.EntityFieldName + ")%>");

                    }
                    str.AppendLine(Tab(6) + "</div>");
                }



                //Remove fields already added in second div
                if (fields.Count() > amtControlsHeightInDiv)
                {
                    fields.RemoveRange(0, amtControlsHeightInDiv);
                }
                else
                {
                    str.AppendLine(Tab(6) + "</fieldset></div>");
                    return str.ToString();
                }
                //if more fields exist continue with last div
                if (fields.Count() > 0)
                {
                    //We only cater for 3 column rows in a groupbox
                    //so add remining fields to the last div
                    //Start of last Div
                    //End of second Div in GroupBox
                    str.AppendLine(Tab(6) + "<div id=\"Divider2\" style=\"width: 30px;height:100%;float: right;  top: 5px; \" ></div>");

                    string alignDiv = "right";
                    //Start Right Div in GroupBox
                    str.AppendLine(Tab(6) + "<div style=\"width: 45%;height:100%; float: " + alignDiv + "; top: 5px; \">");
                    //LOOP entity fields and add only as many as specified in entity.MaxNoFieldsInGroupBox to this rightt div column
                    foreach (EntityField entField in fields)
                    {

                        str.AppendLine(GenerateEditJqueryControl(entField));
                        //   str.AppendLine(Tab(3) + "<%: Html.ValidationMessageFor(o => o." + entField.EntityFieldName + ")%><br />");

                    }
                    str.AppendLine(Tab(6) + "</div>");
                    //End of Right Div in GroupBox



                    str.AppendLine(Tab(6) + "</fieldset></div>");
                    //End of GroupBox
                }


                return str.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateGroupBoxForJqueryNew(List<EntityField> fields)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                // amtControlsHeightInDiv++;
                //Get max Column width
                if (fields[0].GroupBoxWidth == null)
                {
                    str.AppendLine(Tab(10) + "<div class=\"col-md-12\"> ");
                }
                else
                {
                    str.AppendLine(Tab(10) + "<div class=\"" + fields[0].GroupBoxWidth + "\" > ");
                }
                str.AppendLine(Tab(12) + "<div class=\"row\">");
                str.AppendLine(Tab(13) + "<div class=\"box-default\">");
                str.AppendLine(Tab(14) + "<div class=\"box-header with-border\">");
                str.AppendLine(Tab(15) + "<h3 class=\"box-title\">@Html.Encode(Localizer.Current.GetString(\"GroupBox" + fields[0].GroupBoxName + "\"))</h3>");

                //str.AppendLine(Tab(6) + "<div id=\"" + fields[0].GroupBoxName + "Groupbox\" dir=\"ltr\" style=\"text-align:left;\"><fieldset><legend>@Html.Encode(Localizer.Current.GetString(\"GroupBox" + fields[0].GroupBoxName + "\"))</legend>");
                str.AppendLine(string.Empty);
                // List<EntityField> firstDivFields = fields.Take(amtControlsHeightInDiv).ToList();
                str.AppendLine(Tab(15) + "<div class=\"box-body\">");
                foreach (EntityField entField in fields)//NOTE WHAT HAPPENS WHEN LIST IS LESS THAN amt
                {
                    str.AppendLine(this.GenerateEditJqueryControl(entField));
                }
                str.AppendLine(Tab(15) + "</div>");//end box-title class
                str.AppendLine(Tab(14) + "</div>");//end box header class
                str.AppendLine(Tab(13) + "</div>");//end box-default class
                str.AppendLine(Tab(12) + "</div>");//end row class
               
                str.AppendLine(Tab(10) + "</div>");//end col-md class
                return str.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateGroupBox(List<EntityField> fields,int amtControlsHeightInDiv)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                amtControlsHeightInDiv ++;
                //Start of GroupBox
                str.AppendLine(Tab(2) + "<div id=\"" + fields[0].GroupBoxName + "Groupbox\" dir=\"ltr\" style=\"text-align:left;\"><fieldset><legend><%= @Html.Encode(Localizer.Current.GetString(\"GroupBox" + fields[0].GroupBoxName + "\")) %></legend>");
                str.AppendLine(string.Empty);
                List<EntityField> firstDivFields = fields.Take(amtControlsHeightInDiv).ToList();

                //Start of left Div in GroupBox
                str.AppendLine(Tab(2) + "<div style=\"width: 45%;height:100%; float: left; top: 5px;\" >");

                //LOOP entity fields and add only as many as specified in entity.MaxNoFieldsInGroupBox to this left div column
                foreach (EntityField entField in firstDivFields.Take(amtControlsHeightInDiv))//NOTE WHAT HAPPENS WHEN LIST IS LESS THAN amt
                {
                    str.AppendLine(GenerateEditControl(entField));
                }

                str.AppendLine(Tab(2) + "</div>");
                //End of Left Div in GroupBox

                str.AppendLine(Tab(2) + "<div id=\"Divider1\" style=\"width: 30px;height:100%;float: right;  top: 5px; \" ></div>");

                //Remove fields already added in first div
                if (fields.Count() >= amtControlsHeightInDiv)
                {
                    fields.RemoveRange(0, amtControlsHeightInDiv);
                }
                else
                {
                    str.AppendLine(Tab(2) + "</fieldset></div>");
                    return str.ToString();
                }
                //if more fields exist continue with next div
                if (fields.Count() > 0)
                {
                    //Start of second div
                    int nextAmt = fields.Count();
                    string alignDiv = "right";
                    //If there more fields for third div then only add the next max field amt to second div
                    if (fields.Count() > amtControlsHeightInDiv)
                    {
                        nextAmt = amtControlsHeightInDiv;
                        alignDiv = "center";
                    }

                    //Start second Div in GroupBox
                    str.AppendLine(Tab(2) + "<div style=\"width: 45%;height:100%; float: " + alignDiv + "; top: 5px; \">");
                    //LOOP entity fields and add only as many as specified in entity.MaxNoFieldsInGroupBox to this rightt div column
                    foreach (EntityField entField in fields.Take(nextAmt))
                    {

                        str.AppendLine(GenerateEditControl(entField));
                      //  str.AppendLine(Tab(3) + "<%: Html.ValidationMessageFor(o => o." + entField.EntityFieldName + ")%>");

                    }
                    str.AppendLine(Tab(2) + "</div>");
                }



                //Remove fields already added in second div
                if (fields.Count() > amtControlsHeightInDiv)
                {
                    fields.RemoveRange(0, amtControlsHeightInDiv);
                }
                else
                {
                    str.AppendLine(Tab(2) + "</fieldset></div>");
                    return str.ToString();
                }
                //if more fields exist continue with last div
                if (fields.Count() > 0)
                {
                    //We only cater for 3 column rows in a groupbox
                    //so add remining fields to the last div
                    //Start of last Div
                    //End of second Div in GroupBox
                    str.AppendLine(Tab(2) + "<div id=\"Divider2\" style=\"width: 30px;height:100%;float: right;  top: 5px; \" ></div>");

                    string alignDiv = "right";
                    //Start Right Div in GroupBox
                    str.AppendLine(Tab(2) + "<div style=\"width: 45%;height:100%; float: " + alignDiv + "; top: 5px; \">");
                    //LOOP entity fields and add only as many as specified in entity.MaxNoFieldsInGroupBox to this rightt div column
                    foreach (EntityField entField in fields)
                    {

                        str.AppendLine(GenerateEditControl(entField));
                     //   str.AppendLine(Tab(3) + "<%: Html.ValidationMessageFor(o => o." + entField.EntityFieldName + ")%><br />");

                    }
                    str.AppendLine(Tab(2) + "</div>");
                    //End of Right Div in GroupBox



                    str.AppendLine(Tab(2) + "</fieldset></div>");
                    //End of GroupBox
                }
            
            
            return str.ToString();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateSetupFormJqueryDataTable(string objName)
        {
            StringBuilder sb = new StringBuilder();
            AdminManager manager = new AdminManager();
            OperationStatus status = new OperationStatus { Status = true };
            Entity entity = manager.GetEntity(objName, true);
            if (entity == null || entity.EntityField.Count() == 0)
                return;
            try
            {

                //LOOP entity properties from entity table
                sb.AppendLine("@using "+ApplicationName+";");
                sb.AppendLine("@{");
                sb.AppendLine(" ViewBag.Title = Localizer.Current.GetString(\""+ entity.Name +"\");");
                sb.AppendLine("}");
                sb.AppendLine(String.Empty);
                sb.AppendLine("<div class=\"row\">");
                sb.AppendLine("<div class=\"col-md-12\">");
                sb.AppendLine("<div class=\"box box-primary\">");
                sb.AppendLine(String.Empty);
                //sb.AppendLine(Tab(1) + "<div id = \"table\" class=\"box-header with-border\">");
                //sb.AppendLine(Tab(1) + "<div class=\"box-tools pull-right\">");
                //sb.AppendLine(Tab(2) + "<button type = \"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\">");
                //sb.AppendLine(Tab(2) + "<i class=\"fa fa-minus\"></i>");
                //sb.AppendLine(Tab(2) + "</button>");
                //sb.AppendLine(Tab(2) + "<button type = \"button\" class=\"btn btn-box-tool\" data-widget=\"remove\"><i class=\"fa fa-times\"></i></button>");
                //sb.AppendLine(Tab(1) + "</div>");
                //sb.AppendLine(Tab(1) + "</div>");
                sb.AppendLine("<div id=\"viewModal" + entity.Name + "\" ></div>");
                sb.AppendLine(Tab(1) + "<div class=\"box-body\">");
                sb.AppendLine(Tab(2) + "<div class=\"row\">");
                sb.AppendLine(Tab(3) + "<div class=\"col-md-12\">");
                sb.AppendLine(Tab(4) + "<div id=\"responseMessage" + entity.Name +"\" ></div>");
                sb.AppendLine(Tab(3) + "</div>");
                sb.AppendLine(Tab(2) + "</div>");

                sb.AppendLine(Tab(2) + "<div id=\"table\" class=\"table-responsive\">");
                sb.AppendLine(Tab(2) + "<table id= \"tblGrid" + entity.Name + "\" class=\"table no-margin\">");
                sb.AppendLine(Tab(2) + "<thead>");
                sb.AppendLine(Tab(3) + "<tr>");

                //Adding header columns
                foreach (EntityField field in entity.EntityField.Where(o => o.IsActive == true).OrderBy(o => o.ControlOrderNo).OrderByDescending(o => o.IsPrimaryKey))
                {
                    if (field.IsPrimaryKey)
                    {
                        sb.AppendLine(Tab(2) + "<th>" + field.EntityFieldName + "</th>");
                    }
                    else
                    {
                        if (field.IsInGridDisplay)
                        {
                            if (!field.IsHidden)
                            {
                                sb.AppendLine(Tab(2) + "<th>@Localizer.Current.GetString(\"" + field.DisplayName + "\")</th>");
                            }
                        }
                    }
                }
                sb.AppendLine(Tab(2) + "<th></th>");
                sb.AppendLine(Tab(2) + "<th></th>");
                sb.AppendLine(Tab(3) + "</tr>");
                sb.AppendLine(Tab(2) + "</thead>");
                sb.AppendLine(Tab(2) + "</table>");
                sb.AppendLine(Tab(2) + "</div>");
                sb.AppendLine(Tab(1) + "</div>");
                sb.AppendLine("</div>");
                sb.AppendLine("</div>");
                //TODO multi grid master detail
                //if (entity.IsMultiGrid)
                //{
                //    sb.AppendLine(Tab(1) + "<div class=\"row\">");
                //    sb.AppendLine(Tab(2) + "<div class=\"col-xs-12\">");
                //    sb.AppendLine(Tab(3) + " <div class=\"nav-tabs-custom\">");
                //    sb.AppendLine(Tab(4) + " <ul class=\"nav nav-tabs\">");
                //        //TODO loop build based on entityfield tabnames
                //        //Add detail grid name
                //        sb.AppendLine(Tab(5) + "<li class=\"active\"><a data-toggle=\"tab\" href=\"#Reps\">Reps</a></li>");
                //        sb.AppendLine(Tab(5) + "<li><a data-toggle=\"tab\" href=\"#Customers\">Customers</a></li>");
                //        //-----
                //    sb.AppendLine(Tab(4) + "</ul>");
                //    sb.AppendLine(Tab(4) + "<div class=\"tab-content\">");
                //        //TODO loop build based on entityfield tabnames
                //        //Add detail grid name
                //        sb.AppendLine(Tab(5) + "<div id =\"Reps\" class=\"tab-pane fade in active\">");
                //        sb.AppendLine(Tab(5) + "</div>");

                //        sb.AppendLine(Tab(5) + "<div id =\"Customers\" class=\"tab-pane fade\">");
                //        sb.AppendLine(Tab(5) + "</div>");
                //        //-----
                //    sb.AppendLine(Tab(4) + "</div>");
                //    sb.AppendLine(Tab(3) + "</div>");

                //    sb.AppendLine(Tab(2) + "</div>");
                //    sb.AppendLine(Tab(1) + "</div>");
                    
                //}
                      
                sb.AppendLine(Tab(1) + "@section Header{");
                sb.AppendLine(Tab(1) + "<h1>");
                sb.AppendLine(Tab(1) + "@Localizer.Current.GetString(\"" + entity.Name + "\")");
               // sb.AppendLine(Tab(1) + "<small> @Localizer.Current.GetString(\"Maintain activity descriptions\") </small>");
                sb.AppendLine(Tab(1) + "</h1>");
                sb.AppendLine(Tab(1) + "<ol class=\"breadcrumb\">");
                sb.AppendLine(Tab(1) + "<li><a href = \"/\" ><i class=\"fa fa-home\"></i>@Localizer.Current.GetString(\"Home\")</a></li>");
                sb.AppendLine(Tab(1) + "<li><a href = \"#\" class=\"active\">@Localizer.Current.GetString(\"" + entity.Name + "\")</a></li>");
                sb.AppendLine(Tab(1) + "</ol>");
                sb.AppendLine("}");
                sb.AppendLine(String.Empty);
                sb.AppendLine("@section scripts");
                sb.AppendLine("{");
                sb.AppendLine(String.Empty);
                sb.AppendLine(Tab(1) + "<script type=\"text/javascript\">");
                sb.AppendLine(String.Empty);
                sb.AppendLine(Tab(2) + "$(document).ready(function() {");
                sb.AppendLine(String.Empty);
                sb.AppendLine(Tab(3) + "//Filter drop down data");
                int dropDowncount = 1;
                foreach (EntityField field in entity.EntityField.Where(o => o.IsActive == true && o.IsForeignKey == true && o.IsInGridDisplay == true && o.IsDisabled == false).OrderBy(o => o.ControlOrderNo))
                    {
                        if (field.IsForeignKey)
                        {
                            sb.AppendLine(Tab(3) + "var filterList" + dropDowncount.ToString() + " = [];");
                            sb.AppendLine(Tab(3) + field.ScriptInsert + Environment.NewLine);
                            dropDowncount++;
                        }
                    }
                sb.AppendLine(Tab(3) + Environment.NewLine);
                sb.AppendLine(Tab(3) + "$('#responseMessage" + entity.Name + "').html(\"\");");
                sb.AppendLine(String.Empty);
                sb.AppendLine(Tab(3) + "var datatable" + entity.Name + " = $('#tblGrid" + entity.Name + "').dataTable({");
                sb.AppendLine(Tab(3) + "\"bServerSide\": true,");
                sb.AppendLine(Tab(3) + "\"bAutoWidth\": true,");
                sb.AppendLine(Tab(3) + "\"sAjaxSource\": '@Url.Action(\"Get" + entity.Name + "List\", \"" + entity.MngCtlrName + "\")',");
                sb.AppendLine(Tab(3) + "\"fnServerData\": function(sSource, aoData, fnCallback) {");
                sb.AppendLine(Tab(3) + "$('#responseMessage" + entity.Name + "').html(\"\");"); 
                sb.AppendLine(Tab(3) + "$.ajax({");
                sb.AppendLine(Tab(4) + "\"dataType\": 'json',");
                sb.AppendLine(Tab(4) + "\"contentType\": \"application/json; charset = utf-8\",");
                sb.AppendLine(Tab(4) + "\"type\": \"GET\",");
                sb.AppendLine(Tab(4) + "\"url\": sSource,");
                sb.AppendLine(Tab(4) + "\"data\": aoData,");
                sb.AppendLine(Tab(4) + "\"success\": fnCallback,");
                sb.AppendLine(Tab(4) + "\"error\": function(xhr, textStatus, error){");
                sb.AppendLine(Tab(5) + "$('#responseMessage" + entity.Name + "').html(xhr.responseText);");
                sb.AppendLine(Tab(4) + "}");
                sb.AppendLine(Tab(3) + "});");
                sb.AppendLine(Tab(3) + "},");
                sb.AppendLine(Tab(3) + "\"bProcessing\":true,"); 
                sb.AppendLine(Tab(3) + "\"dom\": 'T<\"clear\"><Blp>rtip',");
                sb.AppendLine(Tab(3) + "\"lengthMenu\": [[10, 100, 500, 1000, -1], [10, 100, 500, 1000, \"All\"]],");
               
                sb.AppendLine(Tab(2) + "\"aoColumns\": [");
                List<KeyValuePair<string,string>> foreignKeyNameFields = new List<KeyValuePair<string, string>>();
                
                int countField = 0;
                //Adding header columns
                foreach (EntityField field in entity.EntityField.Where(o => o.IsActive == true).OrderBy(o => o.ControlOrderNo).OrderByDescending(o => o.IsPrimaryKey))
                {
                    if (field.IsPrimaryKey)
                    {
                        sb.AppendLine(Tab(3) + " { \"mDataProp\": \"" + field.EntityFieldName + "\" },");
                    }
                    else
                    {
                        if (field.IsInGridDisplay && field.IsPrimaryKey == false)
                        {
                            if (field.IsHidden == false && field.IsDisabled == false)
                            {
                                countField++;
                                if (field.IsForeignKey)
                                {
                                    sb.AppendLine(Tab(3) + " { \"mDataProp\": \"" + field.EntityFieldName + "\" },");
                                    if (field.EntityFieldName.Substring(0,3).ToLower() == "stp")
                                    {
                                        foreignKeyNameFields.Add(new KeyValuePair<string, string>(countField.ToString(), field.EntityFieldName.Substring(3, field.EntityFieldName.Length-5)));
                                    }
                                    else if (field.EntityFieldName.Substring(0,3).ToLower() == "stc")
                                    {
                                        foreignKeyNameFields.Add(new KeyValuePair<string, string>(countField.ToString(), field.EntityFieldName.Substring(3, field.EntityFieldName.Length - 5)));
                                    }
                                    else
                                    {
                                        foreignKeyNameFields.Add(new KeyValuePair<string, string>(countField.ToString(), field.EntityFieldName.Replace("ID", "").Replace("id", "")));
                                    }
                                }
                                else
                                {
                                    if (field.EntityFieldName == "IsActive")
                                    {
                                        sb.AppendLine(Tab(3) + " {\"mDataProp\": \"IsActiveCheckBox\" },");
                                    }
                                    else
                                    {
                                        sb.AppendLine(Tab(3) + " { \"mDataProp\": \"" + field.EntityFieldName + "\" },");
                                    }
                                }
                            }
                        }
                    }
                }
                sb.AppendLine(Tab(3) + " {\"mDataProp\": \"EditButton\" },");
                sb.AppendLine(Tab(3) + " {\"mDataProp\": \"DeleteButton\" },");
                
                                
                //Adding columns formatting for dates
                StringBuilder dateColumnsNo = new StringBuilder();
                int countformat = 0;
                foreach (EntityField field in entity.EntityField.Where(o => o.EntityFieldDataTypeID != null && o.IsInGridDisplay == true && o.IsActive == true).OrderBy(o => o.ControlOrderNo).OrderByDescending(o => o.IsPrimaryKey))
                {
                    countformat++;
                    //Date formatting in column
                    if (field.IsInGridDisplay && field.EntityFieldDataTypeID == (int)Enumerations.EntityFieldDataType.datetime)
                    {
                        if (!field.IsHidden)
                        {
                            dateColumnsNo.Append(countformat.ToString() + ",");
                        }
                    }
                }

                StringBuilder foreignKeyNamecount = new StringBuilder();
                int counter = 0;
                counter = countformat + 2;//Adding 2 for the edit and delete buttons
                foreach (KeyValuePair<string, string> forKey in foreignKeyNameFields)
                {
                    sb.AppendLine(Tab(3) + " { \"mDataProp\": \"" + forKey.Value + "Desc\" },");
                    foreignKeyNamecount = foreignKeyNamecount.Append("," + counter.ToString());
                    counter++;
                }

                sb.AppendLine(Tab(2) + "],");

                StringBuilder foreignKeyNames = new StringBuilder();
                foreach(KeyValuePair<string, string> foreignKey in foreignKeyNameFields)
                {
                    foreignKeyNames.Append(",{\"render\": function(data, type, row) {  return row."+ foreignKey.Value + "Desc;},\"targets\": [" + foreignKey.Key + "]}");
                }
                string formatDates = string.Empty;
                if (dateColumnsNo.Length > 0)
                {
                  //  sb.AppendLine(Tab(2) + "\"columns\": [");
                    formatDates = ",{\"render\": function(data, type, row) { if (data == null) { return \"\"; } else { var date = new Date(parseInt(data.substr(6)));return formatDate(date.toDateString()); }},\"targets\": [" + dateColumnsNo.ToString().Substring(0, dateColumnsNo.ToString().LastIndexOf(",")) + "]}";
                }
                string disableOrdering = String.Empty;
               
             //   int countExtra = entity.EntityField.Where(o => o.IsPrimaryKey == true && o.IsInGridDisplay == true).Count();
                disableOrdering = ",{ orderable: false, targets: [" + (countformat).ToString() + "," + (countformat + 1).ToString() + "] }";
                
                sb.AppendLine(Tab(2) + "\"columnDefs\": [{ visible: false, targets: [0"+ foreignKeyNamecount.ToString() + "] }"+ disableOrdering + formatDates + foreignKeyNames.ToString() +" ],");//disable primary key id field column/ formatting date columns/ disable edit and delete columns
                sb.AppendLine(Tab(2) + "\"buttons\": [");
                sb.AppendLine(Tab(3) + "{");
                sb.AppendLine(Tab(4) + "text: '<i class=\"fa fa-plus\" aria-hidden=\"true\"></i>',");
              //  sb.AppendLine(Tab(5) + "className: 'btn btn-primary',");
                sb.AppendLine(Tab(4) + "action: function(e, dt, node, config) {");
                sb.AppendLine(Tab(4) + "$('#progressIcon').show();");

                sb.AppendLine(Tab(4) + "$.ajax({");
                sb.AppendLine(Tab(5) + "type: 'POST',");
                sb.AppendLine(Tab(5) + "url: '@Url.Action(\"Get" + entity.Name + "\", \"" + entity.MngCtlrName + "\")',");
                sb.AppendLine(Tab(5) + "data: { id: '' },");
                sb.AppendLine(Tab(5) + "dataType: \"text\",");
                sb.AppendLine(Tab(5) + "success: function(result) {");
                sb.AppendLine(Tab(5) + "if (result)");
                sb.AppendLine(Tab(5) + "{");
                sb.AppendLine(Tab(6) + "$('#viewModal" + entity.Name + "').html(result);");
                sb.AppendLine(Tab(5) + "}");
                sb.AppendLine(Tab(5) + "else {");
                sb.AppendLine(Tab(6) + "ShowNotification(false, data.errorMessage)");
                sb.AppendLine(Tab(5) + "}");
                sb.AppendLine(Tab(4) + "$('#progressIcon').hide();");
                sb.AppendLine(Tab(5) + "}");
                sb.AppendLine(Tab(4) + "});");
                sb.AppendLine(Tab(4) + "},");
                sb.AppendLine(Tab(5) + "titleAttr: '@Localizer.Current.GetString(\"Add\")'");
                sb.AppendLine(Tab(4) + "},");
                sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(5) + "text: '<i class=\"fa fa-file-excel-o\"></i>',");
               // sb.AppendLine(Tab(5) + "className: 'btn btn-primary',");
                sb.AppendLine(Tab(5) + "action: function(e, dt, node, config)");
                sb.AppendLine(Tab(4) + "{");

                sb.AppendLine(Tab(4) + "$.ajax({");
                sb.AppendLine(Tab(5) + "type: 'POST',");
                sb.AppendLine(Tab(5) + "url: '@Url.Action(\"ExportForm\", \"" + entity.MngCtlrName + "\")',");
                sb.AppendLine(Tab(5) + "data: { dataList: JSON.stringify(dt.ajax.params()) },");
                sb.AppendLine(Tab(5) + "success: function(result) {");
                sb.AppendLine(Tab(5) + "if (result)");
                sb.AppendLine(Tab(5) + "{");
                sb.AppendLine(Tab(6) + "$('#viewModal" + entity.Name + "').html(result);");
                sb.AppendLine(Tab(5) + "}");
                sb.AppendLine(Tab(5) + "else {");
                sb.AppendLine(Tab(6) + "ShowNotification(false, data.errorMessage)");
                sb.AppendLine(Tab(5) + "}");
                sb.AppendLine(Tab(5) + "}");
                sb.AppendLine(Tab(4) + "});");
                sb.AppendLine(Tab(4) + "},");
                sb.AppendLine(Tab(5) + "titleAttr: '@Localizer.Current.GetString(\"Export\")'");
                sb.AppendLine(Tab(4) + "},");
                sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(5) + "extend: 'copyHtml5',");
                sb.AppendLine(Tab(5) + "text: '<i class=\"fa fa-files-o\"></i>',");
               // sb.AppendLine(Tab(5) + "className: 'btn btn-primary',");
                sb.AppendLine(Tab(5) + "titleAttr: 'Copy'");
                sb.AppendLine(Tab(4) + "},");
                sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(5) + "extend: 'print',");
                sb.AppendLine(Tab(5) + "text: '<i class=\"fa fa-print\" aria-hidden=\"true\"></i>',");
                sb.AppendLine(Tab(5) + "titleAttr: '@Localizer.Current.GetString(\"Print\")',");
              //  sb.AppendLine(Tab(5) + "className: 'btn btn-primary',");
                sb.AppendLine(Tab(5) + "customize: function(win)");
                sb.AppendLine(Tab(5) + "{");
                sb.AppendLine(Tab(6) + "$(win.document.body)");
                sb.AppendLine(Tab(6) + ".css('font-size', '10pt')");
                sb.AppendLine(Tab(6) + ".prepend(");
                sb.AppendLine(Tab(6) + " '<img src=\"http://datatables.net/media/images/logo-fade.png\" style=\"position:absolute; top:0; left:0;\" />'");
                sb.AppendLine(Tab(6) + ");");
                sb.AppendLine(Tab(6) + "$(win.document.body).find('table')");
                sb.AppendLine(Tab(6) + ".addClass('compact')");
                sb.AppendLine(Tab(6) + ".css('font-size', 'inherit');");
                sb.AppendLine(Tab(5) + "}");
                sb.AppendLine(Tab(4) + "}");
                sb.AppendLine(Tab(2) + "],");
               
                sb.AppendLine(Tab(2) + "\"language\": {");
                sb.AppendLine(Tab(3) + "\"lengthMenu\":\"  \" + display.value + \" _MENU_ \" + records.value,");
                sb.AppendLine(Tab(3) + "\"zeroRecords\": zeroRecords.value,");
                sb.AppendLine(Tab(3) + "\"info\":  showing.value + \" _PAGE_ \" + \" of _PAGES_ from _TOTAL_ entries\", ");
                sb.AppendLine(Tab(3) + "\"infoEmpty\": infoEmpty.value, ");
                sb.AppendLine(Tab(3) + "\"infoFiltered\":  \"(\" + filtered.value + \" _MAX_ \" + totalrecords.value + \")\", ");
                sb.AppendLine(Tab(3) + "\"loadingRecords\": loadingRecords.value, ");
                sb.AppendLine(Tab(3) + "\"processing\": '<div class=\"loader\"> <i class=\"fa fa-spinner fa-spin fa-3x fa-fw\"></i></div> ', ");
                sb.AppendLine(Tab(3) + "\"paginate\": {");
                sb.AppendLine(Tab(4) + "\"first\": first.value, ");
                sb.AppendLine(Tab(4) + "\"last\": last.value, ");
                sb.AppendLine(Tab(4) + "\"next\": next.vaue, ");
                sb.AppendLine(Tab(4) + "\"previous\": previous.value ");
                sb.AppendLine(Tab(3) + "},");
                sb.AppendLine(Tab(2) + "}");
               
                sb.AppendLine(Tab(2) + "}).yadcf([");
                
                //Get columns and add filters
                int count = 0;
                int selectCount = 1;
                foreach (EntityField field in entity.EntityField.Where(o => o.IsActive == true).OrderBy(o => o.ControlOrderNo).OrderByDescending(o => o.IsPrimaryKey))
                {
                    if(field.IsPrimaryKey == true)
                    {
                        count++;
                        continue;
                    }   
                    if (field.IsInGridDisplay)
                    {
                        if (!field.IsHidden)
                        {
                            sb.AppendLine(Tab(3) + "{");
                            if (field.IsForeignKey)
                            {
                                sb.AppendLine(Tab(4) + "column_number: "+count+",");
                                sb.AppendLine(Tab(4) + "filter_type: \"select\",");
                                sb.AppendLine(Tab(4) + "data: filterList" + selectCount.ToString()+",");
                                sb.AppendLine(Tab(4) + "filter_delay: 500");
                                selectCount++;
                            }
                            if(field.StcControlTypeID == (int)Enumerations.ControlType.TextBox || field.StcControlTypeID == (int)Enumerations.ControlType.AutoCompleteTextBox)
                            {
                                sb.AppendLine(Tab(4) + " column_number: " + count + ",");
                                sb.AppendLine(Tab(4) + "filter_type: \"text\",");
                                sb.AppendLine(Tab(4) + "filter_delay: 500");
                            }
                            if (field.StcControlTypeID == (int)Enumerations.ControlType.Calendar)
                            {
                                sb.AppendLine(Tab(4) + " column_number: " + count + ",");
                                sb.AppendLine(Tab(4) + "filter_type: \"range_date\",");
                                sb.AppendLine(Tab(4) + "date_format: \"yyyy-mm-dd\",");
                                sb.AppendLine(Tab(4) + "filter_delay: 500");
                            }
                            if (field.StcControlTypeID == (int)Enumerations.ControlType.CheckBox)
                            {
                                sb.AppendLine(Tab(4) + " column_number: " + count + ",");
                                sb.AppendLine(Tab(4) + "filter_type: \"select\",");
                                sb.AppendLine(Tab(4) + "data: [\"True\", \"False\"]");
                                
                            }
                            count++;
                            sb.AppendLine(Tab(3) + "},");
                        }
                    }
                }

                sb.AppendLine(Tab(2) + "], { cumulative_filtering: true });");
                sb.AppendLine(String.Empty);
                sb.AppendLine(Tab(2) + "$('#tblGrid" + entity.Name + "').on('click', '.btnEdit', function(e)");
                sb.AppendLine(Tab(2) + "{");
                sb.AppendLine(Tab(3) + "e.preventDefault();");
                sb.AppendLine(Tab(3) + "$('#progressIcon').show();");
                sb.AppendLine(Tab(3) + "var value = $(this).attr(\"data-id\");");
                sb.AppendLine(Tab(3) + "$.ajax({");
                sb.AppendLine(Tab(4) + "type: 'POST',");
                sb.AppendLine(Tab(4) + "url: '@Url.Action(\"Get"+entity.Name+"\", \""+entity.MngCtlrName+"\")',");
                sb.AppendLine(Tab(4) + "data: { id: value },");
                sb.AppendLine(Tab(4) + "dataType: \"text\",");
                sb.AppendLine(Tab(4) + "success: function(result)");
                sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(4) + "$('#progressIcon').hide();");
                    sb.AppendLine(Tab(4) + "if (result)");
                    sb.AppendLine(Tab(4) + "{");
                    sb.AppendLine(Tab(4) + "$('#viewModal" + entity.Name + "').html(result);");
                    sb.AppendLine(Tab(4) + "}");
                    sb.AppendLine(Tab(4) + "else");
                    sb.AppendLine(Tab(4) + "{");
                    sb.AppendLine(Tab(5) + "ShowNotification(false, data.errorMessage)");
                    sb.AppendLine(Tab(4) + "}");
                sb.AppendLine(Tab(3) + "}");
                sb.AppendLine(Tab(3) + "});");
                sb.AppendLine(Tab(2) + "});");
                sb.AppendLine(String.Empty);

                sb.AppendLine(Tab(2) + "$('#tblGrid" + entity.Name + "').on('dblclick', 'tr', function(e)");
                sb.AppendLine(Tab(2) + "{");
                sb.AppendLine(Tab(3) + "e.preventDefault();");
                sb.AppendLine(Tab(3) + "$('#progressIcon').show();");
                sb.AppendLine(Tab(3) + "var value = $('#tblGrid" + entity.Name + "').DataTable().row(this).data()[\"" + entity.EntityField.Where(o=>o.IsPrimaryKey==true).FirstOrDefault().DisplayName + "\"];"); 
                sb.AppendLine(Tab(3) + "$.ajax({");
                sb.AppendLine(Tab(4) + "type: 'POST',");
                sb.AppendLine(Tab(4) + "url: '@Url.Action(\"Get" + entity.Name + "\", \"" + entity.MngCtlrName + "\")',");
                sb.AppendLine(Tab(4) + "data: { id: value },");
                sb.AppendLine(Tab(4) + "dataType: \"text\",");
                sb.AppendLine(Tab(4) + "success: function(result)");
                sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(4) + "$('#progressIcon').hide();");
                sb.AppendLine(Tab(4) + "if (result)");
                sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(4) + "$('#viewModal" + entity.Name + "').html(result);");
                sb.AppendLine(Tab(4) + "}");
                sb.AppendLine(Tab(4) + "else");
                sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(5) + "ShowNotification(false, data.errorMessage)");
                sb.AppendLine(Tab(4) + "}");
                sb.AppendLine(Tab(3) + "}");
                sb.AppendLine(Tab(3) + "});");
                sb.AppendLine(Tab(2) + "});");
                sb.AppendLine(String.Empty);

                sb.AppendLine(Tab(2) + "$('#tblGrid" + entity.Name + "').on('click', '.btnDelete', function(e)");
                sb.AppendLine(Tab(2) + "{");
                sb.AppendLine(Tab(3) + "if (confirm('Are you sure you want to delete'))");
                sb.AppendLine(Tab(3) + "{");
                sb.AppendLine(Tab(3) + "e.preventDefault();");
                sb.AppendLine(Tab(3) + "$('#progressIcon').show();");
                sb.AppendLine(Tab(3) + "var value = $(this).attr(\"data-id\");");
                sb.AppendLine(Tab(3) + "$.ajax({");
                sb.AppendLine(Tab(4) + "type: 'POST',");
                sb.AppendLine(Tab(4) + "url: '@Url.Action(\"Delete" + entity.Name + "\", \"" + entity.MngCtlrName + "\")',");
                sb.AppendLine(Tab(4) + "data: { id: value },");
                sb.AppendLine(Tab(4) + "dataType: \"text\",");
                sb.AppendLine(Tab(4) + "success: function(result) {");
                sb.AppendLine(Tab(4) + "$('#progressIcon').hide();");
                sb.AppendLine(Tab(4) + "$('#viewModal" + entity.Name + "').html(result);");
                sb.AppendLine(Tab(4) + "$('#tblGrid" + entity.Name + "').DataTable().ajax.reload();");
                sb.AppendLine(Tab(4) + "},");
                sb.AppendLine(Tab(3) + " });");
                sb.AppendLine(Tab(3) + " }");
                sb.AppendLine(Tab(2) + " });");
                sb.AppendLine(String.Empty);

                 //hide/show columns
                sb.AppendLine(Tab(2) + "$('a.toggle-vis').on('click', function(e)");
                sb.AppendLine(Tab(2) + "{");
                sb.AppendLine(Tab(3) + "e.preventDefault();");
                sb.AppendLine(Tab(3) + "var column = $('#tblGrid" + entity.Name + "').DataTable().column($('#tblGrid" + entity.Name + "').attr('data-column'));");
                sb.AppendLine(Tab(3) + "column.visible(!column.visible());");
                sb.AppendLine(Tab(2) + " });");
                sb.AppendLine(String.Empty);



                if (entity.IsMultiGrid)
                {
                    //      yadcf.exFilterColumn(datatable" + entity.Name +", [
                    //        [1, $('#tblGrid" + entity.Name + "').DataTable().cell('.selected', 0).data()]
                    //]);

                    //$('#yadcf-filter--tblGrid" + entity.Name +"-1').attr('disabled', 'disabled');
                    //$('#yadcf-filter--tblGrid" + entity.Name +"-1-reset').attr('disabled', 'disabled');

                    sb.AppendLine(Tab(2) + "$('#tblGrid" + entity.Name + "').on('click', 'tr', function() {");
                    sb.AppendLine(Tab(2) + "{");
                    sb.AppendLine(Tab(3) + "if ($(this).hasClass('selected')) {");
                    sb.AppendLine(Tab(3) + "{");
                    sb.AppendLine(Tab(4) + "$(this).removeClass('selected');");
                    //Add detail grid name
                    sb.AppendLine(Tab(4) + "$('#Reps').html();");
                    sb.AppendLine(Tab(3) + "}");
                    sb.AppendLine(Tab(3) + "else");
                    sb.AppendLine(Tab(3) + "{");
                    sb.AppendLine(Tab(4) + "datatable.$('tr.selected').removeClass('selected');");
                    sb.AppendLine(Tab(4) + "$(this).addClass('selected');");
                    //TODO loop and add every detail grid
                    sb.AppendLine(Tab(4) + "$.ajax({");
                    sb.AppendLine(Tab(5) + "url: '@Url.Action(\"Get" + entity.Name + "Partial\", \"" + entity.MngCtlrName + "\")',");
                    sb.AppendLine(Tab(5) + "success: function(result) {");
                    sb.AppendLine(Tab(5) + "if (result)");
                    sb.AppendLine(Tab(5) + "{");
                    //Add detail grid name
                    sb.AppendLine(Tab(6) + "$('#Reps').html(result);");
                    sb.AppendLine(Tab(5) + "}");
                    sb.AppendLine(Tab(5) + "else");
                    sb.AppendLine(Tab(5) + "{");
                    sb.AppendLine(Tab(6) + "ShowNotification(false, data.errorMessage)");
                    sb.AppendLine(Tab(5) + "}");
                    sb.AppendLine(Tab(5) + "}");
                    sb.AppendLine(Tab(4) + "});");
                    sb.AppendLine(Tab(3) + " }");
                    sb.AppendLine(Tab(2) + "}");
                    sb.AppendLine(Tab(2) + "});");



                    // $('#tblGrid" + entity.Name + "').on('click', 'tr', function() {
                    //    if ($(this).hasClass('selected')) {
                    //                $(this).removeClass('selected');
                    //                $('#Reps').html();
                    //                $('#Customers').html();
                    //    }
                    //            else {
                    //        datatable.$('tr.selected').removeClass('selected');
                    //                $(this).addClass('selected');

                    //                $.ajax({
                    //            url: '@Url.Action("GetVisitListMobileUserPartial", "VisitListMobileUser")',
                    //                    success: function(result) {
                    //                if (result)
                    //                {
                    //                            $('#Reps').html(result);
                    //                }
                    //                else {
                    //                    ShowNotification(false, data.errorMessage)
                    //                        }
                    //            }

                    //        });
                    //                 $.ajax({
                    //            url: '@Url.Action("GetVisitListCustomerPartial", "VisitListCustomer")',
                    //                    success: function(result) {
                    //                if (result)
                    //                {
                    //                            $('#Customers').html(result);
                    //                }
                    //                else {
                    //                    ShowNotification(false, data.errorMessage)
                    //                        }
                    //            }

                    //        });

                    //    }
                    //});

                }

                sb.AppendLine(String.Empty);

                sb.AppendLine(Tab(2) + " });");//End of Ready 

                //sb.AppendLine(Tab(2) + "function ShowNotification(success, message) {");
                //sb.AppendLine(Tab(3) + "var icon = '';");
                //sb.AppendLine(Tab(3) + "var header = '';");
                //sb.AppendLine(Tab(3) + "var alertClass = '';");
                //sb.AppendLine(Tab(3) + "if (success)");
                //sb.AppendLine(Tab(3) + "{");
                //sb.AppendLine(Tab(4) + "header = 'Success';");
                //sb.AppendLine(Tab(4) + "alertClass = 'alert-success';");
                //sb.AppendLine(Tab(4) + "icon = 'fa-check';");
                //sb.AppendLine(Tab(3) + "}");
                //sb.AppendLine(Tab(3) + "else");
                //sb.AppendLine(Tab(3) + "{");
                //sb.AppendLine(Tab(4) + "header = 'Error';");
                //sb.AppendLine(Tab(4) + "alertClass = 'alert-danger';");
                //sb.AppendLine(Tab(4) + "icon = 'fa-ban';");
                //sb.AppendLine(Tab(3) + "}");

                //sb.AppendLine(Tab(3) + " var html = '<div class=\"alert ' + alertClass + ' alert - dismissible\">' +");
                //sb.AppendLine(Tab(3) + "'<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button>' +");
                //sb.AppendLine(Tab(3) + "'<h4><i class=\"icon fa ' + icon + '\"></i> ' + header + '</h4>' + message +");
                //sb.AppendLine(Tab(3) + "'</div>';");
                //sb.AppendLine(Tab(3) + "$(\"#responseMessage\").html(html);");
                //sb.AppendLine(Tab(3) + " };");
                sb.AppendLine(String.Empty);
                sb.AppendLine(Tab(1) + "</script>");

                sb.AppendLine(Tab(1) + "}");

            
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                //TODO add custom code parts
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(this.FolderToSaveSetupForms + "\\" + entity.PathSetupForm, objName + ".cshtml")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateSetupForm(string objName)
        {
            StringBuilder sb = new StringBuilder();
            BaseManager manager = new BaseManager();
            OperationStatus status = new OperationStatus { Status = true};
            Entity entity = manager.GetEntity(objName, true);
            if (entity == null || entity.EntityField.Count() == 0)
                return;
            try
            {

                //LOOP entity properties from entity table

                sb.AppendLine("<%@ Page Language=\"C#\"  MasterPageFile=\"~/Views/Shared/Site.Master\" Inherits=\"System.Web.Mvc.ViewPage<IEnumerable<" + modelName + "." + entity.Name + ">>\" %>");
                sb.AppendLine("<%@ Import Namespace=\"Common.ExportHelpers\" %>");
                sb.AppendLine("<%@ Import Namespace=\"MVCLocalization\" %>");
                sb.AppendLine("<%@ Import Namespace=\"" + ApplicationName + "BLL\" %>");
                sb.AppendLine(String.Empty);
                sb.AppendLine("<asp:content ID=\"Content\" contentPlaceHolderID=\"MainContent\" runat=\"server\">");
                sb.AppendLine(String.Empty);
                sb.AppendLine("<% using(Html.BeginForm(\"Setup" + entity.Name + "\", \"" + entity.PathSetupForm + "\"))");
                sb.AppendLine("{");
                sb.AppendLine(" %>");
                sb.AppendLine("<input class=\"t-button\" style = \"visibility:visible;float: right\" type=\"submit\" value=\"Update\" />");
                sb.AppendLine("&nbsp;&nbsp;");
                sb.AppendLine("<%=  Html.Telerik().IntegerTextBox()");
                sb.AppendLine(".Name(\"pagesize\")");
                sb.AppendLine(".MinValue(1)");
                sb.AppendLine(".MaxValue(10000)");
                sb.AppendLine(".Value(10)");
                sb.AppendLine(".HtmlAttributes(new { style = \"visibility:visable;float: right;padding:1px 2px 1px 1px\" })");
                sb.AppendLine(" %>");
                sb.AppendLine("&nbsp;&nbsp;");
                sb.AppendLine("<label style = \"visibility:visible;float: right; padding:1px 2px 1px 1px\" >Page size:</label>");
                sb.AppendLine("<br/><br />");
                sb.AppendLine("<%");
                sb.AppendLine("}");
                sb.AppendLine(" %>");

                sb.AppendLine("<%  ");
                sb.AppendLine(String.Empty);
                sb.AppendLine("BaseManager manager = new BaseManager();");
                sb.AppendLine(String.Empty);
                sb.AppendLine(Tab(1) + "Html.Telerik().Grid(Model).Name(\"" + entity.Name + "\")");
                sb.AppendLine(Tab(1) + ".HtmlAttributes(new { @class = \"Font\" })");
                sb.AppendLine(Tab(1) + ".DataKeys(dataKeys => dataKeys.Add(c => c." + entity.EntityField.Single(o => o.IsPrimaryKey == true).EntityFieldName + "))");
                sb.AppendLine(Tab(1) + ".ToolBar(commands => commands.Insert().ButtonType(GridButtonType.BareImage).ImageHtmlAttributes(new{style = \"margin-left:0\"}).HtmlAttributes(manager.getButtonHtmlAttributes(\"Add\", \"" + entity.Name + "\", Session[\"culture\"]==null?\"en-US\" : Session[\"culture\"].ToString())))");
                sb.AppendLine(Tab(1) + ".ToolBar(commands => commands.Custom().Text(Localizer.Current.GetString(\"Export\")).HtmlAttributes(new { id = \"export-open-button\" }).Action(\"ExportForm\",\" " + entity.MngCtlrName + "\"))");
                sb.AppendLine(Tab(1) + ".ToolBar(commands => commands.Custom().Text(TempData[\"pageTotalDisplay\"].ToString()).HtmlAttributes(new { style = \"float: right;background: none repeat scroll 0 0;cursor: default\" }))");
                sb.AppendLine(Tab(1) + ".ToolBar(commands => commands.Custom().Text(Localizer.Current.GetString(\"Next\")).HtmlAttributes(new { id = \"next-button\", style = TempData[\"visableNext\"] }).Action(\"Setup" + entity.Name + "\", \"" + entity.PathSetupForm + "\", new { page = TempData[\"page\"], pageUp = true, paging = true, pagesize = TempData[\"defaultPageSize\"] }))");
                sb.AppendLine(Tab(1) + ".ToolBar(commands => commands.Custom().Text(Localizer.Current.GetString(\"Prev\")).HtmlAttributes(new { id = \"prev-button\", style = TempData[\"visablePrev\"] }).Action(\"Setup" + entity.Name + "\", \"" + entity.PathSetupForm + "\", new { page = TempData[\"page\"], pageUp = false, paging = true, pagesize = TempData[\"defaultPageSize\"] }))");
                 
                sb.AppendLine(Tab(1) + ".DataBinding(dataBinding => dataBinding.Server()");
                sb.AppendLine(Tab(2) + ".Select(\"Setup" + entity.Name + "\", \"" + entity.PathSetupForm + "\", new { pagesize = TempData[\"defaultPageSize\"] })");
                sb.AppendLine(Tab(2) + ".Insert(\"Create" + entity.Name + "\", \"" + entity.PathSetupForm + "\")");
                sb.AppendLine(Tab(2) + ".Update(\"Edit" + entity.Name + "\",  \"" + entity.PathSetupForm + "\")");
                sb.AppendLine(Tab(2) + ".Delete(\"Delete" + entity.Name + "\",  \"" + entity.PathSetupForm + "\"))");
                                
                sb.AppendLine(Tab(1) + ".Columns(columns => {");
              
                foreach(EntityField field in entity.EntityField.OrderBy(o => o.ControlOrderNo))
                {

                    if (field.IsInGridDisplay)
                    {
                        if (field.IsHidden)
                        {
                            sb.AppendLine(Tab(2) + "columns.Bound(o => o." + field.EntityFieldName + ").Hidden();");
                     
                        }
                        else
                        {
                            if (field.IsForeignKey)
                            {
                                
                                if (field.EntityFieldName == "StcStatusID")
                                {
                                    sb.AppendLine(Tab(2) + "columns.Bound(o => o.Status).Width(100).Title(Localizer.Current.GetString(\"Status\"));");
                                }
                                else
                                {
                                    string relatedName = string.Empty;
                                    string colName = string.Empty;
                                    if (field.EntityFieldName.Substring(0, 3) == "Stp")
                                    {
                                        relatedName = "StpData";
                                        colName = "DataDescription";
                                    }
                                    else if (field.EntityFieldName.Substring(0, 3) == "Stc")
                                    {
                                        relatedName = "StcData";
                                        colName = "Description";
                                    }
                                    else
                                    {
                                        relatedName = Common.ClearEntityFieldName(field.EntityFieldName);
                                        //relatedName = field.EntityFieldName.Replace("ParentID", "").Replace("SupervisorID", "User").Replace("CreatedByID", "User").Replace("ID", "").Replace("Org", "Organization");
                                        colName = manager.GetEntityField((int)field.ComboBoxDisplayFieldID, false).EntityFieldName; 
                                    }

                                    sb.AppendLine(Tab(2) + "columns.Bound(o => o." + relatedName + "." + colName + ").Width(" + field.Max + ").Title(Localizer.Current.GetString(\"" + field.EntityFieldName + "\")).Sortable(false).Filterable(false).Groupable(false);");
                                }
                               
                            }
                            else
                            {
                                sb.AppendLine(Tab(2) + "columns.Bound(o => o." + field.EntityFieldName + ").Width(" + field.Max + ").Title(Localizer.Current.GetString(\"" + field.EntityFieldName + "\"));");
                            }
                        }
                    }

                }

                sb.AppendLine(Tab(1) + "columns.Command(commands =>{commands.Edit().ButtonType(GridButtonType.BareImage).HtmlAttributes(manager.getButtonHtmlAttributes(\"Edit\",  \"" + entity.Name + "\", Session[\"culture\"] == null ? \"en-US\" : Session[\"culture\"].ToString()));commands.Delete().ButtonType(GridButtonType.BareImage);}).HtmlAttributes(manager.getButtonHtmlAttributes(\"Delete\", \"" + entity.Name + "\", Session[\"culture\"] == null ? \"en-US\" : Session[\"culture\"].ToString())).Width(30).Title(Localizer.Current.GetString(\"Commands\"));");
                
                
                sb.AppendLine(Tab(1) + "})");
                sb.AppendLine(Tab(1) + ".Editable(editing => editing.Mode(GridEditMode.PopUp).TemplateName(\"" + entity.PathEditTemplate + "/Edit" + entity.Name + "\").FormHtmlAttributes(manager.getButtonHtmlAttributes(\"EditForm\", \"" + entity.Name + "\", Session[\"culture\"] == null ? \"en-US\" : Session[\"culture\"].ToString())))");
                sb.AppendLine(Tab(1) + ".KeyboardNavigation()");
                sb.AppendLine(Tab(1) + ".Sortable()");
                sb.AppendLine(Tab(1) + ".Groupable()");
                sb.AppendLine(Tab(1) + ".Filterable()");
                sb.AppendLine(Tab(1) + ".Render();");
         
                sb.AppendLine("%>");
         
                sb.AppendLine("<% ExportModel exp = new ExportModel { ControllerName = \""+entity.MngCtlrName+"\", ModelName = \""+entity.Name+"\" };%>");
                sb.AppendLine("<%= Html.Partial(\"ExportControl\", exp)%>");

                sb.AppendLine("<div id=\"bottom\"  style = \"width: 100%; height:100px;\"></div>");
                
                sb.AppendLine("</asp:content>");
                

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                //TODO add custom code parts
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(this.FolderToSaveSetupForms + "\\" + entity.PathSetupForm, "Setup" + objName + ".aspx")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        private string GenerateEditControl(EntityField field)
        {
            try
            {
            StringBuilder str = new StringBuilder();
            if (!field.IsHidden)
            {
                str.AppendLine(Tab(3) + "<%: Html.Label(@Html.Encode(Localizer.Current.GetString(\"" + field.EntityFieldName + "\")))%>");
                str.AppendLine(Tab(3) + "<br/>");
            }
            if (field.IsHidden)
            {
                //str.AppendLine(Tab(3) + "<%: Html.HiddenFor(o => o." + field.EntityFieldName + ")%>");
                return str.ToString();
            }
            if (field.StcControlTypeID ==(int)Enumerations.ControlType.Calendar)
            {
                if (field.IsToolTip)
                {
                    str.AppendLine(Tab(3) + "<%=  Html.Telerik().DatePickerFor(o => o." + field.EntityFieldName + ").Name(\"" + field.EntityFieldName + "\").HtmlAttributes(new { title = @Html.Encode(Localizer.Current.GetString(\"ToolTip" + field.EntityFieldName.Trim() + "\") }).Enable(Html.enableDatePicker(o => o." + field.EntityFieldName + "))%>");
                }
                else
                {
                    str.AppendLine(Tab(3) + "<%= Html.Telerik().DatePickerFor(o => o." + field.EntityFieldName + ").Name(\"" + field.EntityFieldName + "\").Enable(Html.enableControl(o => o." + field.EntityFieldName + "))%>");
                }
                str.AppendLine(Tab(3) + "<%: Html.CustomValidationMessageFor(o => o." + field.EntityFieldName + ")%>");
                str.AppendLine(Tab(3) + "<br/>");
            }
            else if (field.StcControlTypeID ==(int)Enumerations.ControlType.ComboBox)
            {
                if (field.ComboBoxDisplayFieldID != null)
                {
                    //BaseManager manager = new BaseManager();
                    //OperationStatus status = new OperationStatus { Status = true };
                    //str.AppendLine(Tab(3) + "<% if(manager == null){manager = new BaseManager();} %>");//TODO if theres more than one drop down this is repeated
                    //str.AppendLine(Tab(3) + "<% if(status == null){status = new OperationStatus();} %>");//TODO if theres more than one drop down this is repeated
                    ////control
                    //string dataName = string.Empty;
                    //string bindNameID = string.Empty;

                    //string bindname = manager.GetEntityField((int)field.ComboBoxDisplayFieldID, false).EntityFieldName;

                    //if (field.EntityFieldName.Substring(0, 3) == "Stp")
                    //{
                    //    dataName = field.EntityFieldName.Remove(0, 3).Replace("ID", "");
                    //    bindNameID = "StpDataID";
                    //    str.AppendLine(Tab(3) + "<% IQueryable<StpData> List" + dataName + " = manager.GetList<StpData,string>(null,o => o.StpDataTypeID == (int)Enumerations.SetupDataType." + dataName + " && o.IsActive == true && statuslist.Contains((int)o.StcStatusID),o => o." + bindname + " ,out status);  %> <br/>");
                    //}
                    //else if (field.EntityFieldName.Substring(0, 3) == "Stc")
                    //{
                    //    dataName = field.EntityFieldName.Remove(0, 3).Replace("ID", "");
                    //    bindNameID = "StcDataID";
                    //    str.AppendLine(Tab(3) + "<% IQueryable<StcData> List" + dataName + " = manager.GetList<StcData,string>(null,o => o.StcDataTypeID == (int)Enumerations.StaticDataType." + dataName + " && o.IsActive == true ,o => o." + bindname + " ,out status);  %> <br/>");
                    //}
                    //else
                    //{
                    //    //TODO Robin create a list of foreign key names that dont match the tablename to replace with proper name 
                    //    dataName = field.EntityFieldName.Replace("ID", "").Replace("Supervisor", "User");
                    //    bindNameID = field.EntityFieldName;
                    //    str.AppendLine(Tab(3) + "<% IQueryable<" + dataName + "> List" + dataName + " = manager.GetList<" + dataName + ",string>(null,o => o.IsActive == true && statuslist.Contains((int)o.StcStatusID) ,o => o." + bindname + " ,out status);  %> <br/>");
                    //}
                    //Robin : This telerik combo box does not work when a grid has filters on it
                    //str.AppendLine(Tab(3) + "<%= Html.Telerik().ComboBoxFor(o => o." + field.EntityFieldName + ")");
                    //str.AppendLine(Tab(4) + ".AutoFill(true)");
                    //str.AppendLine(Tab(4) + ".BindTo(new SelectList(List" + dataName + ".ToList(), \"" + bindNameID + "\", \"" + bindname + "\"))");
                    //str.AppendLine(Tab(4) + ".Filterable(filtering =>{filtering.FilterMode(AutoCompleteFilterMode.StartsWith);})");
                    //str.AppendLine(Tab(4) + ".HighlightFirstMatch(true)");
                    //str.AppendLine(Tab(4) + ".OpenOnFocus(true)");

                    str.AppendLine(Tab(3) + "<%= Html.CustomDropDownListFor(st => st." + field.EntityFieldName + ")%>");
                    
                    //str.AppendLine(Tab(3) + "<%= Html.DropDownListFor(st => st." + field.EntityFieldName + ", new SelectList(List" + dataName + ".ToList(), \"" + bindNameID + "\", \"" + bindname + "\"), \"Localizer.Current.GetString(\"Select +\" \"+ Localizer.Current.GetString(" + dataName + " \", new { @class = \"t-widget t-input\", style = \"width:" + field.Max.Trim() + "px;\" ");
                    //if (field.IsToolTip)
                    //{
                    //    str.AppendLine(Tab(4) + ", title = @Html.Encode(Localizer.Current.GetString(\"ToolTip" + dataName + ") ");
                    //}
                    //if (field.EntityFieldName == "StcStatusID")
                    //{
                    //    str.AppendLine(Tab(4) + ", Enabled = \"false\"");
                    //}
                    //str.AppendLine(Tab(4) + " })");
                    //str.AppendLine(Tab(4) + "%>");
                    //str.AppendLine(Tab(4) + "<br/>");

                    str.AppendLine(Tab(3) + "<%: Html.CustomValidationMessageFor(o => o." + field.EntityFieldName + ")%>");
                    str.AppendLine(Tab(3) + "<br/>");
                }
            }

            if (field.StcControlTypeID == (int)Enumerations.ControlType.CurrencyTextBox || field.StcControlTypeID == (int)Enumerations.ControlType.PercentageTextBox || field.StcControlTypeID == (int)Enumerations.ControlType.NumericTextBox || field.StcControlTypeID == (int)Enumerations.ControlType.TextBox)
            {
                str.AppendLine(Tab(3) + "<%: Html.CustomTextBoxFor(o => o." + field.EntityFieldName + ") %>");
                str.AppendLine(Tab(3) + "<%: Html.CustomValidationMessageFor(o => o." + field.EntityFieldName + ")%>");
                str.AppendLine(Tab(3) + "<br/>");
            }
            else if (field.StcControlTypeID == (int)Enumerations.ControlType.TextArea)
            {

                str.AppendLine(Tab(3) + "<%: Html.CustomTextBoxAreaFor(o => o." + field.EntityFieldName + ") %>");
                str.AppendLine(Tab(3) + "<%: Html.CustomValidationMessageFor(o => o." + field.EntityFieldName + ")%>");
                str.AppendLine(Tab(3) + "<br/>");
            }
            else if (field.StcControlTypeID == (int)Enumerations.ControlType.RangeSlider)
            {
                //TODO Add rangeslider text how to bind
                //  str.AppendLine("<br/>");
                //control
                //        str.AppendLine("<%= Html.Telerik().RangeSlider<double>().Name("RangeSlider")");
                //        str.AppendLine("<br/>");
                //        str.AppendLine("<br/>");
                //        str.AppendLine("<br/>");
                //        str.AppendLine("<br/>");
                //        str.AppendLine("<br/>");


                //        .Min(Model.RangeSliderAttributes.MinValue.Value)
                //        .Max(Model.RangeSliderAttributes.MaxValue.Value)
                //        .Values(Model.RangeSliderAttributes.SelectionStart.Value,
                //            Model.RangeSliderAttributes.SelectionEnd.Value)
                //        .SmallStep(Model.RangeSliderAttributes.SmallStep.Value)
                //        .LargeStep(Model.RangeSliderAttributes.LargeStep.Value)
                //        .TickPlacement(Model.RangeSliderAttributes.TickPlacement.Value)
                //        .Orientation(Model.RangeSliderAttributes.SliderOrientation.Value)
                //%>
            }
            else if (field.StcControlTypeID == (int)Enumerations.ControlType.UploadFileBox)
            {
                //TODO Add uploaderfunctionality text how to bind
                // str.AppendLine("<br/>");
                //control

                return "<%= Html.Telerik().Upload().Name(\"attachments\").Multiple((bool) ViewData[\"multiple\"]).Async(async => async " +
                            " .Save(\"Save\", \"Upload\") " +
                            " .Remove(\"Remove\", \"Upload\") " +
                            " .AutoUpload((bool) ViewData[\"autoUpload\"]))%>";
            }
            else if (field.StcControlTypeID == (int)Enumerations.ControlType.TreeViewCheckBox)
            {
                //TODO find out how selected values will be saved back
                //  str.AppendLine("<br/>");
                //control
                //<%= Html.Telerik().TreeView()
                //        .Name("TreeView")
                //        .ShowLines(true)
                //        .ShowCheckBox(true)
                //        .HtmlAttributes(new { style = string.Format("width:{0}px", 300) })
                //        .BindTo(Model.EntityField, mappings => 
                //        {
                //            mappings.For<EntityField>(binding => binding
                //                    .ItemDataBound((item, ent) =>
                //                    {
                //                        item.Text = ent.EntityFieldName;
                //                    })
                //                    );
                //        })
                //%>
            }
            else if (field.StcControlTypeID == (int)Enumerations.ControlType.CheckBox)
            {

                str.AppendLine(Tab(3) + " <%: Html.CustomCheckBoxFor(o => o." + field.EntityFieldName + ")%>");
                str.AppendLine(Tab(3) + "<br/>");

            }
            
            return str.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateControllerForPortalToApiClasses(List<string> _lstTables)
        {
            StringBuilder sb = new StringBuilder();
            AdminManager manager = new AdminManager();
            OperationStatus status = new OperationStatus();

            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using System.IO;");
                sb.AppendLine("using System.Text;");
                sb.AppendLine("using System.Collections;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Collections.ObjectModel;");
                sb.AppendLine("using System.Configuration;");
                sb.AppendLine("using System.ComponentModel;");
                sb.AppendLine("using System.Web.Routing;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using System.Web;");
                sb.AppendLine("using System.Web.Mvc;");
                sb.AppendLine("using System.Web.Caching;");
                sb.AppendLine("using System.Net.Http;");
                sb.AppendLine("using System.Net.Http.Headers;");
                sb.AppendLine("using Newtonsoft.Json;");
                sb.AppendLine("using DALEFModel;");
                sb.AppendLine("using Common;");
                sb.AppendLine("using Models;");
                sb.AppendLine("using CyroTechPortal.HTMLHelpers;");
                sb.AppendLine(string.Empty);
                int index = this.ApplicationName.LastIndexOf("\\");

                sb.AppendLine(Tab(0) + "namespace "+ this.ApplicationName + "");

                sb.AppendLine(Tab(0) + "{");

                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(1) + "public class " + CtrlMngSvcName + "Controller : BaseController");
                sb.AppendLine(Tab(1) + "{");
                //busy here-------------------------------------------------------------------------------------------              
                sb.AppendLine(string.Empty);

                foreach (string strTableName in _lstTables)
                {
                    string _className = string.Empty;
                    Entity entity = manager.GetEntity(strTableName, true);
                    if (entity == null || entity.EntityField.Count() == 0)
                        return;


                    //string orderByField = "";
                    string pkFieldName = "";
                    string pkFieldType = "";

                    //if (dctFriendlyNames.ContainsKey(strTableName))
                    //    friendlyName = dctFriendlyNames[strTableName];

                    //if (dctOrderByFields.ContainsKey(_className))
                    //    orderByField = dctOrderByFields[_className];
                    foreach (EntityField col in entity.EntityField)
                    {
                        if (col.IsPrimaryKey)
                        {
                            pkFieldName = col.EntityFieldName;
                            pkFieldType = "int";//this.ConvertDataType(col.EntityFieldDataType.Type, false);
                            break;
                        }
                    }
                    
                    sb.Append(this.GenerateControllerForPortalToApiCRUD(strTableName, pkFieldType, pkFieldName));
                }

                //sb.AppendLine(string.Empty);
                //sb.AppendLine(Tab(2) + "#region Dispose");
                //sb.AppendLine(Tab(2) + "public void Dispose()");
                //sb.AppendLine(Tab(2) + "{");
                //sb.AppendLine(Tab(2) + "}");
                //sb.AppendLine(Tab(2) + "#endregion");

                sb.AppendLine(Tab(1) + "}"); // end class

                sb.AppendLine(Tab(0) + "}"); // end namespace
            }

            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                //TODO apend all code in the custom region from old file to new one
                //if(File Exists)
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(this.FolderPathUIController, CtrlMngSvcName + "Controller.cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GenerateControllerClasses(List<string> _lstTables)
        {
            StringBuilder sb = new StringBuilder();
            BaseManager manager = new BaseManager();
            OperationStatus status = new OperationStatus();
            
            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using System.IO;");
                sb.AppendLine("using System.Text;");
                sb.AppendLine("using System.Collections;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Configuration;");
                sb.AppendLine("using System.ComponentModel;");
                sb.AppendLine("using System.Web.Routing;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using System.Web;");
                sb.AppendLine("using System.Web.Mvc;");
                sb.AppendLine("using Telerik.Web.Mvc;");
                sb.AppendLine("using Telerik.Web.Mvc.UI;");
                sb.AppendLine("using Telerik.Web.Mvc.Extensions;");
                sb.AppendLine("using " + ApplicationName + "BLL;");
                sb.AppendLine("using " + modelName + ";");
                sb.AppendLine("using MVCLocalization;");
                sb.AppendLine("using Common;");
                sb.AppendLine(string.Empty);
                int index = this.ApplicationName.LastIndexOf("\\");
                
                sb.AppendLine(Tab(0) + "namespace " + this.ApplicationName.Substring(index+1) + "UI.Controllers");

                sb.AppendLine(Tab(0) + "{");

                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(1) + "public class " + CtrlMngSvcName + "Controller : BaseController");
                sb.AppendLine(Tab(1) + "{");

                sb.AppendLine(Tab(3) + "private " + CtrlMngSvcName + "Manager manager = new " + CtrlMngSvcName + "Manager();");
                //busy here-------------------------------------------------------------------------------------------              
                sb.AppendLine(string.Empty);

                foreach (string strTableName in _lstTables)
                {
                    string _className = string.Empty;
                    Entity entity = manager.GetEntity(strTableName, true);
                    if (entity == null || entity.EntityField.Count() == 0)
                        return;

                   
                    //string orderByField = "";
                    string pkFieldName = "";
                    string pkFieldType = "";

                    //if (dctFriendlyNames.ContainsKey(strTableName))
                    //    friendlyName = dctFriendlyNames[strTableName];

                    //if (dctOrderByFields.ContainsKey(_className))
                    //    orderByField = dctOrderByFields[_className];
                    foreach (EntityField col in entity.EntityField)
                    {
                        if (col.IsPrimaryKey)
                        {
                            pkFieldName = col.EntityFieldName;
                            pkFieldType = "int";//this.ConvertDataType(col.EntityFieldDataType.Type, false);
                            break;
                        }
                    }
                    sb.Append(this.GenerateControllerCRUD(strTableName, pkFieldType, pkFieldName));
                }

                //sb.AppendLine(string.Empty);
                //sb.AppendLine(Tab(2) + "#region Dispose");
                //sb.AppendLine(Tab(2) + "public void Dispose()");
                //sb.AppendLine(Tab(2) + "{");
                //sb.AppendLine(Tab(2) + "}");
                //sb.AppendLine(Tab(2) + "#endregion");

                sb.AppendLine(Tab(1) + "}"); // end class

                sb.AppendLine(Tab(0) + "}"); // end namespace
            }

            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                //TODO apend all code in the custom region from old file to new one
                //if(File Exists)
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(this.FolderPathUIController, CtrlMngSvcName + "Controller.cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateControllerCRUD(string className, string pkFieldType, string pkFieldName)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(2) + "#region " + className + " Methods");
            sb.AppendLine(string.Empty);
            //----------------Setup data action-----------------------------------------
            sb.AppendLine(Tab(2) + "[GridAction(GridName = \"" + className + "\")]");
            sb.AppendLine(Tab(2) + "public ActionResult Setup" + className + "(GridCommand command)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "IQueryable list = null;");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "if (command.Page == 1)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + " command = new GridCommand();");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "int currentPage = 0;");
            sb.AppendLine(Tab(4) + "int pageSize = 0;");
            sb.AppendLine(Tab(4) + "int rowTotal = manager.Get" + className + "(false, ExpressionBuilder.Expression<" + className + ">(command.FilterDescriptors)).Count();");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "GridPaging(rowTotal, out currentPage, out  pageSize);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "Dictionary<string, string> listOrderBy = GetSortOrderDictionary(command.SortDescriptors);");
            sb.AppendLine(Tab(4) + "list = manager.Get" + className + "(false, ExpressionBuilder.Expression<" + className + ">(command.FilterDescriptors), listOrderBy, currentPage, pageSize);");
                       
            sb.AppendLine(string.Empty); 
            sb.AppendLine(Tab(3) + "}catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "Session[\"Error\"]  = ExceptionHandler.Handle(ex);");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(4) + "return View(list);");
            sb.AppendLine(Tab(2) + "}");
                        
            sb.AppendLine(string.Empty);


            //----------------Export data action-----------------------------------------
            sb.AppendLine(Tab(2) + "[GridAction]");
            sb.AppendLine(Tab(2) + "public ActionResult ExportData" + className + "(FormCollection form, bool chkIncludeDetails, bool chkPdf, bool chkExcel, bool chkCsv,bool chkCurrentPage, bool chkExportBlank)");
            sb.AppendLine(Tab(2) + "{");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(3) + "List<" + className + "> list = new List<" + className + ">();");
                sb.AppendLine(Tab(3) + "try");
                sb.AppendLine(Tab(3) + "{");

                    sb.AppendLine(Tab(4) + "if (chkExportBlank)");
                    sb.AppendLine(Tab(5) + "{");
                        sb.AppendLine(Tab(6) + "FileContentResult file = base.ExportTemplateBase<" + className + ">(form, false, false, true, false, false);");
                        sb.AppendLine(Tab(6) + "if (file != null)");
                        sb.AppendLine(Tab(7) + "{");
                            sb.AppendLine(Tab(8) + "return file;");
                        sb.AppendLine(Tab(7) + "}");
                        sb.AppendLine(Tab(6) + "else");        
                        sb.AppendLine(Tab(7) + "{");
                            sb.AppendLine(Tab(6) + "Session[\"Error\"] = ExceptionHandler.Handle(new Exception(Localizer.Current.GetString(\"ExportError\")));");
                        sb.AppendLine(Tab(7) + "}");
                    sb.AppendLine(Tab(6) + "}");
                    sb.AppendLine(Tab(5) + "else");
                    sb.AppendLine(Tab(6) + "{");

                        sb.AppendLine(Tab(3) + "GridModel data = manager.Get" + className + "(true).ToGridModel(Convert.ToInt32(form.GetValue(\"currentPage\").AttemptedValue), Convert.ToInt32(form.GetValue(\"pageSize\").AttemptedValue), form.GetValue(\"sortOrders\").AttemptedValue,string.Empty, form.GetValue(\"filters\").AttemptedValue);");
                        sb.AppendLine(Tab(4) + "list = data.Data.Cast<" + className + ">().ToList();");
                        sb.AppendLine(Tab(3) + "if (list.Count > 0)");
                        sb.AppendLine(Tab(3) + "{");
                         sb.AppendLine(Tab(4) + "FileContentResult file = base.ExportBase<" + className + ">(form, list, chkIncludeDetails, chkPdf, chkExcel, chkCsv, chkCurrentPage);");
                            sb.AppendLine(Tab(4) + "if (file != null)");
                            sb.AppendLine(Tab(4) + "{");
                             sb.AppendLine(Tab(5) + "return file;");
                            sb.AppendLine(Tab(4) + "}");
                            sb.AppendLine(Tab(4) + "else");
                            sb.AppendLine(Tab(4) + "{");
                             sb.AppendLine(Tab(5) + "Session[\"Error\"] = ExceptionHandler.Handle(new Exception(Localizer.Current.GetString(\"ExportError\")));");
                            sb.AppendLine(Tab(4) + "}");
                        sb.AppendLine(Tab(3) + "}");

                    sb.AppendLine(Tab(3) + "}");
                sb.AppendLine(Tab(3) + "}catch(Exception ex)");
                sb.AppendLine(Tab(3) + "{");
                 sb.AppendLine(Tab(4) + "Session[\"Error\"] = ExceptionHandler.Handle(ex);");
                sb.AppendLine(Tab(3) + "}");
            
                sb.AppendLine(Tab(2) + "return RedirectToAction(\"Setup" + className + "\");");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);
            
            //---------------Export Empty Template---------------------------------
            sb.AppendLine(Tab(2) + "[GridAction]");
            sb.AppendLine(Tab(2) + "public FileContentResult ExportTemplateBase<T>(FormCollection form, bool chkIncludeDetails, bool chkPdf, bool chkExcel, bool chkCsv, bool chkCurrentPage) where T : class,new()");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "StringBuilder sb = new StringBuilder();");       
            sb.AppendLine(Tab(4) + "MemoryStream output = new MemoryStream();");
            sb.AppendLine(Tab(4) + "output = mng.ExportDataHeaderToExcel<T>(chkIncludeDetails, sb.ToString());");
            sb.AppendLine(Tab(4) + "if (output == null)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + "return null;");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "output.Close();");
            sb.AppendLine(Tab(4) + "output.Dispose();");        

            sb.AppendLine(Tab(4) + "return File(output.ToArray(), \"application/vnd.ms-excel\", form.GetValue(\"txtFilename\").AttemptedValue + \".xls\");");
            sb.AppendLine(Tab(3) + "}");    
            sb.AppendLine(Tab(3) + "catch (Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "throw ex;");
            sb.AppendLine(Tab(3) + "}");

            sb.AppendLine(Tab(3) + "}");

            //---------------Import Action-----------------------------------------
            sb.AppendLine(Tab(2) + "[GridAction]");
            sb.AppendLine(Tab(2) + "public ActionResult ImportData" + className + "(HttpPostedFileBase file)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "if (file.ContentLength > 0)");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(6) + "base.UploadFile<"+className+">(file);");
            sb.AppendLine(Tab(5) + "}");

            sb.AppendLine(Tab(4) + "Session[\"AlertDialog\"] = Localizer.Current.GetString(\"Import Complete\");");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch (Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "Session[\"Error\"] = ExceptionHandler.Handle(ex);");
            sb.AppendLine(Tab(3) + "}");

            sb.AppendLine(Tab(3) + " return RedirectToAction(\"Setup"+className+"\");");
            sb.AppendLine(Tab(2) + "}");


            //----------------Create action-----------------------------------------
            sb.AppendLine(Tab(2) + "[AcceptVerbs(HttpVerbs.Post)]");
            sb.AppendLine(Tab(2) + "[GridAction]");
            sb.AppendLine(Tab(2) + "public ActionResult Create" + className + "()");
            sb.AppendLine(Tab(2) + "{");
           
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + ""+className+" data = new "+className+"();");
            sb.AppendLine(Tab(4) + "data.OrgID = Convert.ToInt32(Session[\"defaultOrgID\"]);");
            sb.AppendLine(Tab(4) + "//Perform model binding (fill the product properties and validate it).");
            sb.AppendLine(Tab(4) + "if (TryUpdateModel(data))");
            sb.AppendLine(Tab(4) + "{");
          //  sb.AppendLine(Tab(5) + " //Add any extra logic or fields here");
          //  sb.AppendLine(Tab(5) + " data.OrgID = (int)Session[\"OrgID\"];");
            sb.AppendLine(Tab(5) + " manager.Add" + className + "(data);");
           
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + " Session[\"Error\"]  = ExceptionHandler.Handle(ex);");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "RouteValueDictionary routeValues = this.GridRouteValues();");
            sb.AppendLine(Tab(3) + "return RedirectToAction(\"Setup"+className+"\", routeValues);");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);

            //----------------Edit action-----------------------------------------
            sb.AppendLine(Tab(2) + "[AcceptVerbs(HttpVerbs.Post)]");
            sb.AppendLine(Tab(2) + "[GridAction]");
            sb.AppendLine(Tab(2) + "public ActionResult Edit" + className + "(int id)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "" + className + " data = manager.Get" + className + "(id, false);");
            sb.AppendLine(Tab(4) + "data.ChangeDateTime = DateTime.Now;");
            sb.AppendLine(Tab(4) + "if (TryUpdateModel(data))");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + " //Add any extra logic or fields here");
            sb.AppendLine(Tab(5) + " manager.Update" + className + "(data);");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + " Session[\"Error\"]  = ExceptionHandler.Handle(ex);");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "RouteValueDictionary routeValues = this.GridRouteValues();");
            sb.AppendLine(Tab(3) + "return RedirectToAction(\"Setup" + className + "\", routeValues);");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);
            //----------------Delete action-----------------------------------------
            sb.AppendLine(Tab(2) + "[AcceptVerbs(HttpVerbs.Post)]");
            sb.AppendLine(Tab(2) + "[GridAction]");
            sb.AppendLine(Tab(2) + "public ActionResult Delete" + className + "(int id)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "" + className + " data = manager.Get" + className + "(id, false);");
            sb.AppendLine(Tab(4) + "if (data != null)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + " //Add any extra logic or fields here");
            sb.AppendLine(Tab(5) + " manager.Delete" + className + "(data);");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + " Session[\"Error\"]  = ExceptionHandler.Handle(ex);");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "RouteValueDictionary routeValues = this.GridRouteValues();");
            sb.AppendLine(Tab(3) + "return RedirectToAction(\"Setup" + className + "\", routeValues);");
            sb.AppendLine(Tab(2) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(2) + "#endregion");
            //TODO - add other CRUD methods here:
            return sb.ToString();

        }
        private string GenerateControllerForPortalToApiCRUD(string className, string pkFieldType, string pkFieldName)
        {
            AdminManager manager = new AdminManager();
            Entity entity = manager.GetEntity(className, true);
            if (entity == null || entity.EntityField.Count() == 0)
            { 
                return "NO Entity or Entity fields defined for " + className;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Empty);

            //TODO ADD Detail grids
            //public ActionResult GetProductPriceListPartial()
            //{
            //    try
            //    {
            //        return PartialView("../PriceList/ProductPriceList");
            //    }
            //    catch (System.Exception ex)
            //    {
            //        return Content(ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
            //    }
            //}


            //Get page action call for mvc
            sb.AppendLine(Tab(2) + "#region " + className + " Methods");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// " + className +" this instance.");
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "[HttpGet]");
            sb.AppendLine(Tab(2) + "public ViewResult " + className + "()");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "return View();");
            sb.AppendLine(Tab(2) + "}");
            sb.AppendLine(string.Empty);
            //----------------Get data list action for datatables grid -----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// Gets the " + className + " list.");
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <param name=\"jQueryDataTablesModel\">The jquery data tables model.</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "public ActionResult Get" + className + "List(JQueryDataTablesModel jQueryDataTablesModel)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "string uri = CommonHelper.BaseUri + \"" + CtrlMngSvcName + "Controller/" + className + "\";");
            sb.AppendLine(Tab(3) + "jQueryDataTablesModel.uri = uri;");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(string.Empty);
            //sb.AppendLine(Tab(4) + "using (HttpClient httpClient = new HttpClient())");
            //sb.AppendLine(Tab(4) + "{");
            //sb.AppendLine(Tab(5) + "httpClient.DefaultRequestHeaders.Accept.Clear();");
            //sb.AppendLine(Tab(5) + "httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(\"application/json\"));");
            //sb.AppendLine(Tab(5) + "GridParam gridParams = new GridParam();");
            //sb.AppendLine(Tab(5) + "gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;");
            //sb.AppendLine(Tab(5) + "gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;");
            ////TODO add field in entity to specifiy if it should include relations
            //sb.AppendLine(Tab(5) + "gridParams.Includerelations = true;");
            //sb.AppendLine(Tab(5) + "gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;");
            //sb.AppendLine(Tab(5) + "gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;");
            //sb.AppendLine(Tab(5) + "StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, \"application/json\");");
            //sb.AppendLine(Tab(5) + "HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;");

            sb.AppendLine(Tab(5) + "HttpResponseMessage response = GetGridData(jQueryDataTablesModel, false);");
           
            sb.AppendLine(Tab(5) + "if (response.IsSuccessStatusCode)");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(6) + "var jsonString = response.Content.ReadAsStringAsync();");
            sb.AppendLine(Tab(6) + "jsonString.Wait();");
            sb.AppendLine(Tab(6) + "GridResult<" + className + "> result = JsonConvert.DeserializeObject<GridResult<" + className + ">>(jsonString.Result);");
            sb.AppendLine(Tab(6) + "List<" + className + "> retList = new List<" + className + ">();");
            sb.AppendLine(Tab(6) + "foreach (" + className + " item in result.Items)");
            sb.AppendLine(Tab(6) + "{");
            sb.AppendLine(string.Empty);
            foreach (EntityField field in entity.EntityField.OrderBy(o => o.ControlOrderNo))
            {
                int stpCount = 0;
                int stcCount = 0;
                if (field.IsInGridDisplay)
                {
                    if (field.IsForeignKey && field.EntityAndFieldName.Substring(field.EntityAndFieldName.Length - 2).Equals("ID",StringComparison.OrdinalIgnoreCase))
                    {
                        if (field.ForeignTable == null)
						{
                            throw new Exception("No Foreign Table defined " + field.EntityAndFieldName);
						}
                        sb.AppendLine(Tab(7) + "if (item."+ field.ForeignTable + " != null)");
                        sb.AppendLine(Tab(7) + "{");
                        if (field.EntityFieldName.Substring(0,3).ToLower() == "stp")
                        {
                            if (stpCount == 0)
                            {
                                sb.AppendLine(Tab(7) + "item." + field.EntityFieldName.Substring(3, field.EntityFieldName.Length - 5) + "Desc = item.StpData.DataDescription;");
                            }
                            else
							{
                                sb.AppendLine(Tab(7) + "item." + field.EntityFieldName.Substring(3, field.EntityFieldName.Length - 5) + "Desc = item.StpData"+ stpCount + ".DataDescription;");
                            }
                            stpCount++;
                            sb.AppendLine(Tab(7) + "item." + field.ForeignTable + " = null;");
                            sb.AppendLine(Tab(7) + "}");
                           
                            continue;
                        }
                        if (field.EntityFieldName.Substring(0,3).ToLower() == "stc")
                        {
                            if (stpCount == 0)
                            {
                                if (field.EntityFieldName != "StcStatus")
                                {
                                    sb.AppendLine(Tab(7) + "item." + field.EntityFieldName.Substring(3, field.EntityFieldName.Length - 5) + "Desc = item.StcData.Description;");
								}
								else
								{
                                    sb.AppendLine(Tab(7) + "item." + field.EntityFieldName.Substring(3) + "Desc = item.StcData.Description;");
                                }
                            }
                            else
                            {
                                if (field.EntityFieldName != "StcStatus")
                                {
                                    sb.AppendLine(Tab(7) + "item." + field.EntityFieldName.Substring(3, field.EntityFieldName.Length - 5) + "Desc = item.StcData" + stcCount + ".Description;");
                                }
                                else
                                {
                                    sb.AppendLine(Tab(7) + "item." + field.EntityFieldName.Substring(3) + "Desc = item.StcData" + stcCount + ".Description;");
                                }
                                
                            }
                            stcCount++;
                            sb.AppendLine(Tab(7) + "item." + field.ForeignTable + " = null;");
                            sb.AppendLine(Tab(7) + "}");
                            
                            continue;
                        }
                        if (field.ComboBoxDisplayFieldID != null)
                        {
                            EntityField displayField = manager.GetEntityField((int)field.ComboBoxDisplayFieldID,false);
                            if (displayField != null)
                            {
                                sb.AppendLine(Tab(7) + "item." + field.EntityFieldName.Substring(0, field.EntityFieldName.Length - 2) + "Desc = item." + field.ForeignTable + "." + displayField.EntityFieldName + ";");
                            }
                        }
                        sb.AppendLine(Tab(7) + "item." + field.ForeignTable + " = null;");
                        sb.AppendLine(Tab(7) + "}");
                        

                    }
                    if (field.EntityFieldName == "IsActive")
                    {
                        sb.AppendLine(Tab(7) + "item.IsActiveCheckBox = item.IsActive == true ? \" <span class='label label-success'>\" + Localizer.Current.GetString(\"True\") + \"</span></td>\" : \"<span class='label label-danger'>\" + Localizer.Current.GetString(\"False\") + \"</span></td>\";");
                    }
                }
            }
            
            sb.AppendLine(Tab(7) + "item.EditButton = \"<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='\" + item." + pkFieldName + " +\"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>\";");
            sb.AppendLine(Tab(7) + "item.DeleteButton = \"<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='\" + item." + pkFieldName + " +\"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>\";");
            sb.AppendLine(Tab(6) + "retList.Add(item);");
            sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(6) + "return Json(new");
            sb.AppendLine(Tab(6) + "{");
            sb.AppendLine(Tab(7) + "sEcho = jQueryDataTablesModel.sEcho,");
            sb.AppendLine(Tab(7) + "iTotalRecords = result.TotalCount,");
            sb.AppendLine(Tab(7) + "iTotalDisplayRecords = result.TotalFilteredCount,");
            sb.AppendLine(Tab(7) + "aaData = retList");
            sb.AppendLine(Tab(6) + "},");
            sb.AppendLine(Tab(6) + "JsonRequestBehavior.AllowGet);");

            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(5) + "else");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(6) + "var readAsStringAsync = response.Content.ReadAsStringAsync();");
            sb.AppendLine(Tab(6) + "return Content(CommonHelper.ShowNotification(false, readAsStringAsync.Result));");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(string.Empty);
         //   sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");
            sb.AppendLine(string.Empty);
         
            //----------------Get action-----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <param name=\"" + className + "ID\">The " + className + " identifier.</param>");
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");

            sb.AppendLine(Tab(2) + "public ActionResult Get" + className + "(int? id)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,");
            sb.AppendLine(Tab(5) + "TimeZoneInfo.FindSystemTimeZoneById(\"South Africa Standard Time\"));");
            sb.AppendLine(Tab(5) + "string uri = CommonHelper.BaseUri + \"" + CtrlMngSvcName + "Controller/"+ className + "\";");
            sb.AppendLine(Tab(5) + "if (id == null)");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(6) + "return PartialView(\"Edit" + className + "\", new " + className + "() ");  
            sb.AppendLine(Tab(7) + "{");
            sb.AppendLine(Tab(8) + "CreateDateTime = SATime,");
            sb.AppendLine(Tab(8) + "CreatedByID = ((User)Session[\"User\"]).UserID,");
            sb.AppendLine(Tab(8) + "OrgID = ((User)Session[\"User\"]).OrgID,");
            sb.AppendLine(Tab(8) + "IsActive = true,");
            sb.AppendLine(Tab(7) + "});");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(5) + "using (HttpClient httpClient = new HttpClient())");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(6) + "httpClient.DefaultRequestHeaders.Accept.Clear();");
            sb.AppendLine(Tab(6) + "httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(\"application/json\"));");
            sb.AppendLine(Tab(6) + "HttpResponseMessage response = httpClient.GetAsync(uri + \"/\" + id + \"/true\").Result;");
            sb.AppendLine(Tab(6) + "var content = response.Content.ReadAsStringAsync();");
            sb.AppendLine(Tab(6) + "if (response.IsSuccessStatusCode)");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(7) + "var settings = new JsonSerializerSettings");
            sb.AppendLine(Tab(7) + "{");
            sb.AppendLine(Tab(7) + "NullValueHandling = NullValueHandling.Ignore,");
            sb.AppendLine(Tab(7) + "MissingMemberHandling = MissingMemberHandling.Ignore");
            sb.AppendLine(Tab(7) + "};");
            sb.AppendLine(Tab(7) + "" + className + " item = JsonConvert.DeserializeObject<" + className + ">(content.Result,settings);");
            sb.AppendLine(Tab(7) + "item.ChangeDateTime = SATime;");
            sb.AppendLine(Tab(7) + "item.CreatedByID = ((User)Session[\"User\"]).UserID;");
            sb.AppendLine(Tab(7) + "item.OrgID = ((User)Session[\"User\"]).OrgID;");
            sb.AppendLine(Tab(7) + "return PartialView(\"Edit" + className + "\", item);");
            sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(Tab(5) + "else");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(7) + "return Content(CommonHelper.ShowNotification(false, content.Result));");
            sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + "return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(string.Empty);
            //----------------Edit-Update action-----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <param name=\"" + className + "\">The activity.</param>");
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "[HttpPost]");
            sb.AppendLine(Tab(2) + "public ActionResult Edit" + className + "(" + className + " newItem)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "string uri = CommonHelper.BaseUri + \"" + CtrlMngSvcName + "Controller/" + className + "\";");
            sb.AppendLine(Tab(4) + "string uriAdd = CommonHelper.BaseUri + \"" + CtrlMngSvcName + "Controller/" + className + "/add\";");
            sb.AppendLine(Tab(4) + "string uriUpdate = CommonHelper.BaseUri + \"" + CtrlMngSvcName + "Controller/" + className + "/update\";");
            sb.AppendLine(Tab(4) + "" + className + " itemExists = null;");
            sb.AppendLine(Tab(4) + "using (HttpClient httpClient = new HttpClient())");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "httpClient.DefaultRequestHeaders.Accept.Clear();");
            sb.AppendLine(Tab(5) + "httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(\"application/json\"));");
            sb.AppendLine(Tab(5) + "//Check exists");
            sb.AppendLine(Tab(5) + "HttpResponseMessage response = httpClient.GetAsync(uri + \"/\" + newItem." + pkFieldName + "+ \"/false\").Result;");
            sb.AppendLine(Tab(5) + "var content = response.Content.ReadAsStringAsync();");
            sb.AppendLine(Tab(5) + "if (response.IsSuccessStatusCode)");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(7) + "var settings = new JsonSerializerSettings");
            sb.AppendLine(Tab(7) + "{");
            sb.AppendLine(Tab(7) + "NullValueHandling = NullValueHandling.Ignore,");
            sb.AppendLine(Tab(7) + "MissingMemberHandling = MissingMemberHandling.Ignore");
            sb.AppendLine(Tab(7) + "};");
            sb.AppendLine(Tab(6) + "itemExists = JsonConvert.DeserializeObject<" + className + ">(content.Result,settings);");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(5) + "StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, \"application/json\");");
            sb.AppendLine(Tab(5) + "//Insert");
            sb.AppendLine(Tab(5) + "if (itemExists == null)");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(6) + "HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;");
            sb.AppendLine(Tab(6) + "var resultAdd = responseAdd.Content.ReadAsStringAsync();");
            sb.AppendLine(Tab(6) + "if (responseAdd.IsSuccessStatusCode)");
            sb.AppendLine(Tab(6) + "{");
            sb.AppendLine(Tab(7) + "return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString(\"Successfully Added\")));");
            sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(Tab(6) + "else");
            sb.AppendLine(Tab(6) + "{");
            sb.AppendLine(Tab(7) + "return Content(CommonHelper.ShowNotification(false, resultAdd.Result));");
            sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(5) + "else//Update");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(6) + "HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;");
            sb.AppendLine(Tab(6) + "var resultOut = responseOut.Content.ReadAsStringAsync();");
            sb.AppendLine(Tab(6) + "if (responseOut.IsSuccessStatusCode)");
            sb.AppendLine(Tab(6) + "{");
            sb.AppendLine(Tab(7) + "return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString(\"Successfully Updated\")));");
            sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(Tab(6) + "else");
            sb.AppendLine(Tab(6) + "{");
            sb.AppendLine(Tab(7) + "return Content(CommonHelper.ShowNotification(false, resultOut.Result));");
            sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");
                     
            sb.AppendLine(string.Empty);
            //----------------Delete action-----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// Deletes the " + className + ".");
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <param name=\"id\">The identifier.</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "public ActionResult Delete" + className + "(int? id)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "string uri = CommonHelper.BaseUri + \"" + CtrlMngSvcName + "Controller/" + className + "/delete\";");
            sb.AppendLine(Tab(4) + "if (id == null)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + "return View();");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "using (HttpClient httpClient = new HttpClient())");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "StringContent content1 = new StringContent(id.ToString());");
            sb.AppendLine(Tab(5) + "httpClient.DefaultRequestHeaders.Accept.Clear();");
            sb.AppendLine(Tab(5) + "httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(\"application/json\"));");
            sb.AppendLine(Tab(5) + "HttpResponseMessage response = httpClient.PostAsync(uri + \"/\" + id, content1).Result;");
            sb.AppendLine(Tab(5) + "var content = response.Content.ReadAsStringAsync();");
            sb.AppendLine(Tab(5) + "if (response.IsSuccessStatusCode)");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(6) + "return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString(\"Successfully Deleted\")));");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(5) + "else");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(6) + "return Content(CommonHelper.ShowNotification(false, content.Result));");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(2) + "}");
                     
            sb.AppendLine(string.Empty);

            //----------------Open Export form action-----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// Exports the form.");
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <param name=\"dtParams\">The data list.</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "public ActionResult ExportForm(string dataList)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(3) + "Export exportdata = new Export();");
            sb.AppendLine(Tab(3) + "exportdata.Controller =\"" + entity.MngCtlrName + "\";");
            sb.AppendLine(Tab(3) + "exportdata.Entity =\"" + className + "\";");
            sb.AppendLine(Tab(3) + "exportdata.DatatableParams = dataList;");
            sb.AppendLine(Tab(3) + "return PartialView(\"ExportControl\", exportdata);");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(3) + "return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");

            //----------------Export data action-----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// Exports the data.");
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// <param name=\"exportData\">The export data.</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "/// <exception cref=\"System.Exception\">");
            sb.AppendLine(Tab(2) + "/// </exception>");
            sb.AppendLine(Tab(2) + "[HttpPost]");
            sb.AppendLine(Tab(2) + "public FileResult ExportData(Export exportDetails)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "FileContentResult file = base.ExportBase<" + className + ">(exportDetails);");
            sb.AppendLine(Tab(4) + "if (file != null)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(4) + "return file;");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "else");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(4) + "throw new System.Exception(Localizer.Current.GetString(\"ExportError\"));");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(2) + "}");
            sb.AppendLine(Tab(2) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "throw ex;");
            sb.AppendLine(Tab(2) + "}");
            sb.AppendLine(Tab(2) + "}");


            //---------------Import Action-----------------------------------------
            //sb.AppendLine(Tab(2) + "[GridAction]");
            //sb.AppendLine(Tab(2) + "public ActionResult ImportData" + className + "(HttpPostedFileBase file)");
            //sb.AppendLine(Tab(2) + "{");
            //sb.AppendLine(Tab(3) + "try");
            //sb.AppendLine(Tab(3) + "{");
            //sb.AppendLine(Tab(4) + "if (file.ContentLength > 0)");
            //sb.AppendLine(Tab(5) + "{");
            //sb.AppendLine(Tab(6) + "base.UploadFile<" + className + ">(file);");
            //sb.AppendLine(Tab(5) + "}");

            //sb.AppendLine(Tab(4) + "Session[\"AlertDialog\"] = Localizer.Current.GetString(\"Import Complete\");");
            //sb.AppendLine(Tab(3) + "}");
            //sb.AppendLine(Tab(3) + "catch (Exception ex)");
            //sb.AppendLine(Tab(3) + "{");
            //sb.AppendLine(Tab(4) + "Session[\"Error\"] = ExceptionHandler.Handle(ex);");
            //sb.AppendLine(Tab(3) + "}");

            //sb.AppendLine(Tab(3) + " return RedirectToAction(\"Setup" + className + "\");");
            //sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(Tab(2) + "#endregion");
            //TODO - add other CRUD methods here:
            return sb.ToString();

        }
        
        private void GenerateMetadataClasses(string objName)
        {
            StringBuilder sb = new StringBuilder();
            AdminManager manager = new AdminManager();
            OperationStatus status = new OperationStatus();
            Entity entity = manager.GetEntity(objName, true);
            if (entity == null || entity.EntityField.Count() == 0)
                return;
            //BaseManager manager = new BaseManager();
            //OperationStatus status = new OperationStatus();
            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.ComponentModel;");
                sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using Common;");  
                
                sb.AppendLine(string.Empty);

                sb.AppendLine("namespace DALEFModel");
                sb.AppendLine(Tab(0) + "{");
                
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(1) + "/// <summary>");
                sb.AppendLine(Tab(1) + "/// Metadata class which has all the dataannotation attributes ");
                sb.AppendLine(Tab(1) + "/// Author: Robin Cyrolies");
                sb.AppendLine(Tab(1) + "/// </summary>");

                sb.AppendLine(Tab(1) + "[MetadataTypeAttribute(typeof(" + objName + "." + objName + "Metadata))]");
                sb.AppendLine(Tab(1) + "public partial class " + objName);
                sb.AppendLine(Tab(1) + "{");

                sb.AppendLine(Tab(2) + "internal sealed class " + objName + "Metadata");
                sb.AppendLine(Tab(2) + "{");

                sb.AppendLine(Tab(3) + "// Metadata classes are not meant to be instantiated.");
                sb.AppendLine(Tab(3) + "private " + objName + "Metadata()");
                sb.AppendLine(Tab(3) + "{");
                sb.AppendLine(Tab(3) + "}");

                sb.AppendLine(string.Empty);

                int i = 1;

                foreach (EntityField col in entity.EntityField)
                {
                                        
                    string colDescription = col.DisplayName;
                    EntityFieldDataType datatype = manager.Get<EntityFieldDataType>(o=>o.EntityFieldDataTypeID == col.EntityFieldDataTypeID);
                    if(datatype == null)
                    {
                        throw new Exception("No EntityFieldDataType defined on column : " + colDescription);
                    }
                    string dataType = datatype.Type.Trim();
                                       
                    //TODO put this on form so fields can be added manually to be ignored
                    if ((colDescription != "VersionNo") && (colDescription != "IsActive") && (colDescription != "OrgID") && (colDescription != "StcStatusID"))
                    {
                        //TODO EXPRESSION FOR EMAIL VALIDATION
                        //   [RegularExpression(".+@.+\\..+", ErrorMessageResourceType = typeof(MyResources.Resources),
                        //   ErrorMessageResourceName = "EmailInvalid")]  
                        if (col.RegularExpression != null)
                            sb.AppendLine(Tab(3) + "[RegularExpression(" + col.RegularExpression + ",ErrorMessage = \" Validation failed for " + colDescription.Replace("Stp", "").Replace("Stc", "").Replace("ID", "") + "\")]");
                        
                        if (col.Max > 0 && dataType.ToLower().Trim() == "nvarchar")
                            sb.AppendLine(Tab(3) + "[StringLength(" + col.Max.ToString() + ",MinimumLength = " + col.Min.ToString() + ",ErrorMessage = \" " + colDescription.Replace("Stp", "").Replace("Stc", "").Replace("ID", "") + " Maximum length is = " + col.Max.ToString() + " Minimum length is = " + col.Min.ToString() + "\")]");//TODO enter resource files
       
                        if (col.IsPrimaryKey)
                        {
                            sb.AppendLine(Tab(3) + "[Key]");
                        }
                        if (!col.IsPrimaryKey && col.IsMandatory && dataType.ToLower().Trim() != "bit")
                        {
                            sb.AppendLine(Tab(3) + "[Required(ErrorMessage = \" This field is required = " + colDescription.Replace("Stp", "").Replace("Stc", "").Replace("ID", "") + "\")]");
                        }

                        sb.AppendLine(Tab(3) + "[Display(Name = \"" + colDescription.Replace("Stp", "").Replace("Stc", "").Replace("ID", "") + "\", Order = " + i.ToString() + ")]");

                        //TODO - can exclude certain system fields with this:
                        //[Display(AutoGenerateField = false)]
                        if (dataType.ToLower().Trim() == "nvarchar" || dataType.ToLower().Trim() == "nchar" || dataType.ToLower().Trim() == "ConcatField")
                        {
                            sb.AppendLine(Tab(3) + "public string " + col.EntityFieldName + " { get; set; }");
                        }
                        else if(dataType.ToLower().Trim() == "bit")
                        {
                            sb.AppendLine(Tab(3) + "public bool " + col.EntityFieldName + " { get; set; }");
                        }
                        else if (dataType.ToLower().Trim() == "time")
                        {
                            sb.AppendLine(Tab(3) + "public TimeSpan " + col.EntityFieldName + " { get; set; }");
                        }
                        else if (dataType.ToLower().Trim() == "varbinary")
                        {
                            sb.AppendLine(Tab(3) + "public byte[] " + col.EntityFieldName + " { get; set; }");
                        }
                        else
                        {
                            sb.AppendLine(Tab(3) + "public " + dataType + " " + col.EntityFieldName + " { get; set; }");
                        }
                        sb.AppendLine("");

                        i++;
                    }
                }
                               

                sb.AppendLine(Tab(2) + "}"); // end internal sealed class
                sb.AppendLine(Tab(1) + "}"); // end class
                sb.AppendLine(Tab(0) + "}"); // end namespace
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(FolderToSaveMetaData, objName + ".cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GeneratePartialClasses(string objName)
        {
            StringBuilder sb = new StringBuilder();
            AdminManager manager = new AdminManager();
            OperationStatus status = new OperationStatus();
            Entity entity = manager.GetEntity(objName, true);
            if (entity == null || entity.EntityField.Count() == 0)
                return;
            //BaseManager manager = new BaseManager();
            //OperationStatus status = new OperationStatus();
            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.ComponentModel;");
                sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using Common;");

                sb.AppendLine(string.Empty);

                sb.AppendLine("namespace DALEFModel");
                sb.AppendLine(Tab(0) + "{");

                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(1) + "/// <summary>");
                sb.AppendLine(Tab(1) + "/// Partial class ");
                sb.AppendLine(Tab(1) + "/// Author: Robin Cyrolies");
                sb.AppendLine(Tab(1) + "/// </summary>");

                 sb.AppendLine(Tab(1) + "public partial class " + objName);
                sb.AppendLine(Tab(1) + "{");

                sb.AppendLine(string.Empty);

                foreach (EntityField col in entity.EntityField)
                {
           
                    if (col.IsForeignKey)
                    {
                        if (col.EntityFieldName.Substring(0, 3).ToLower() == "stp" || col.EntityFieldName.Substring(0, 3).ToLower() == "stc")
                        {
                            sb.AppendLine(Tab(3) + "public string " + col.EntityFieldName.Substring(3, col.EntityFieldName.Length - 5) + "Desc { get; set; }");
                        }
                        else
                        {
                            sb.AppendLine(Tab(3) + "public string " + col.EntityFieldName.Substring(0, col.EntityFieldName.Length - 2) + "Desc { get; set; }");
                        }

                    }
                    if(col.EntityFieldDataTypeID == 12) //Concat field
					{
                        sb.AppendLine(Tab(3) + "public string " + col.EntityFieldName + "{ get { return "+col.DisplayName+";}}");
                    }
                }
    
                sb.AppendLine(Tab(1) + "}"); // end class
                sb.AppendLine(Tab(0) + "}"); // end namespace
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(FolderToSavePartialClass, objName + ".cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GenerateServiceClasses(List<string> _lstTables)
        {
            //Create Service.svc file
            StringBuilder sbsvc = new StringBuilder();
            //Create Service.svc.cs file
            StringBuilder sb = new StringBuilder();
              
            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.ComponentModel;");
               // sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using System.ServiceModel.Activation;");
                sb.AppendLine("using " + ApplicationName + "BLL;");
                sb.AppendLine("using " + modelName + ";");
                sb.AppendLine("using Common;");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(0) + "namespace " + ApplicationName + "Services");

                sb.AppendLine(Tab(0) + "{");
                
                sb.AppendLine(string.Empty);
                //sb.AppendLine(" [EnableClientAccess()]");
                sb.AppendLine(Tab(1) + "[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]");
                sb.AppendLine(Tab(1) + "public class " + CtrlMngSvcName + "Service" + " : I" + CtrlMngSvcName + "Service");
                sb.AppendLine(Tab(1) + "{");

                string managername = CtrlMngSvcName + "Manager";

                sb.AppendLine(Tab(2) + "#region Property");
                sb.AppendLine(Tab(2) + CtrlMngSvcName + " manager = null;");
                sb.AppendLine(Tab(2) + "public " + CtrlMngSvcName + " Manager");
                sb.AppendLine(Tab(2) + "{");
                sb.AppendLine(Tab(3) + "get");
                sb.AppendLine(Tab(3) + "{");
                sb.AppendLine(Tab(3) + "if (manager == null)");
                sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(5) + "manager = new " + CtrlMngSvcName + "();");
                sb.AppendLine(Tab(4) + "}");
                sb.AppendLine(Tab(4) + "return manager;");
                sb.AppendLine(Tab(3) + "}");
                sb.AppendLine(Tab(2) + "}");
                sb.AppendLine(Tab(2) + "#endregion");
       


                foreach (string strTableName in _lstTables)
                {
                    string _className = string.Empty;

                    Table objTable = _database.Tables[strTableName];

                    _className = objTable.Name;

                    string friendlyName = _className;
                    string orderByField = "";
                    string pkFieldName = _className + "ID";
                    string pkFieldType = "int";

                    if (dctFriendlyNames.ContainsKey(_className))
                        friendlyName = dctFriendlyNames[_className];

                    //if (dctOrderByFields.ContainsKey(_className))
                    //    orderByField = dctOrderByFields[_className];
                    foreach (Column col in objTable.Columns)
                    {
                        if (col.InPrimaryKey)
                        {
                            pkFieldName = col.Name;
                            pkFieldType = this.ConvertDataType(col.DataType.Name, col.Nullable);
                            break;
                        }
                    }
                   sb.Append(this.GenerateServiceClassCRUD(friendlyName, _className,pkFieldType, pkFieldName, orderByField));
                }

                sb.AppendLine(string.Empty);
               // sb.AppendLine(Tab(2) + "#endregion");

                sb.AppendLine(Tab(1) + "}"); // end class

                sb.AppendLine(Tab(0) + "}"); // end namespace
            

            //Create Service.svc file
                sbsvc.AppendLine("<%@ ServiceHost Language=\"C#\" Debug=\"true\" Service=\"FJSBillingService." + CtrlMngSvcName + "Service\" CodeBehind=\"" + CtrlMngSvcName + "Service.svc.cs\" %>");


            }   
            catch (Exception)
            {
                throw;
            }

            //Save the code
            try
            {
                using (StreamWriter oStreamWriterSVC = new StreamWriter(Path.Combine(FolderPathAPIController, CtrlMngSvcName + "Service.svc")))
                {
                    oStreamWriterSVC.Write(sbsvc.ToString());
                    oStreamWriterSVC.Flush();
                    oStreamWriterSVC.Close();
                }
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(FolderPathAPIController, CtrlMngSvcName + "Service.svc.cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GenerateServiceClassCRUD(string friendlyName, string className, string pkFieldType, string pkFieldName, string orderByField)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Empty);

            sb.AppendLine(Tab(2) + "#region " + friendlyName + " Methods");


            sb.AppendLine(string.Empty);

            sb.AppendLine(Tab(2) + "public List<" + className + "> List" + friendlyName + "()");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "return Manager.Get" + friendlyName + "s();");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);

            sb.AppendLine(Tab(2) + "public "+ friendlyName +" Get" + friendlyName + "(" + pkFieldType + " id)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "return Manager.Get" + friendlyName + "(id);");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);

            sb.AppendLine(Tab(2) + "public OperationStatus Add" + friendlyName + "(" + className + " newEntity)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "return Manager.Add" + friendlyName + "(newEntity);");
            sb.AppendLine(Tab(2) + "}");
           
            sb.AppendLine(string.Empty);

            sb.AppendLine(Tab(2) + "public OperationStatus Update" + friendlyName + "(" + className + " editEntity)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "return Manager.Update" + friendlyName + "(editEntity);");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);

            sb.AppendLine(Tab(2) + "public OperationStatus Delete" + friendlyName + "(" + className + " deleteEntity)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "return Manager.Delete" + friendlyName + "(deleteEntity);");
            sb.AppendLine(Tab(2) + "}");

            

            sb.AppendLine(Tab(2) + "#endregion");
            //TODO - add other CRUD methods here:
            return sb.ToString();

        }

        private void GenerateServiceInterfaceClasses(List<string> _lstTables)
        {
            StringBuilder sb = new StringBuilder();
            string prefixname = this.ModelNamespace.Substring(this.ModelNamespace.LastIndexOf('\\') + 1).Replace("Model.edmx", "");

            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.ServiceModel;");
                sb.AppendLine("using DALEFModel;");
                sb.AppendLine("using Common;");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(0) + "namespace " + ApplicationName + "Services");

                sb.AppendLine(Tab(0) + "{");

                foreach (string strTableName in _lstTables)
                {
                    string _className = string.Empty;

                    Table objTable = _database.Tables[strTableName];

                    _className = objTable.Name;

                    string friendlyName = _className;
                    //string orderByField = "";
                    string pkFieldName = _className + "ID";
                    string pkFieldType = "int";

                    if (dctFriendlyNames.ContainsKey(_className))
                        friendlyName = dctFriendlyNames[_className];

                    //if (dctOrderByFields.ContainsKey(_className))
                    //    orderByField = dctOrderByFields[_className];
                    foreach (Column col in objTable.Columns)
                    {
                        if (col.InPrimaryKey)
                        {
                            pkFieldName = col.Name;
                            pkFieldType = this.ConvertDataType(col.DataType.Name, col.Nullable);
                            break;
                        }
                    }
                    sb.Append(this.GenerateServiceInterfaceClassCRUD(friendlyName, _className, pkFieldType, pkFieldName, prefixname));
                }

                sb.AppendLine(string.Empty);
                // sb.AppendLine(Tab(2) + "#endregion");

                //  sb.AppendLine(Tab(1) + "}"); // end class

                sb.AppendLine(Tab(0) + "}"); // end namespace
            }

            catch (Exception)
            {
                throw;
            }

            //Save the code
            try
            {
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(FolderPathAPIControllerInterface, "I" + CtrlMngSvcName + "Service.cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GenerateServiceInterfaceClassCRUD(string friendlyName, string className, string pkFieldType, string pkFieldName, string prefixname)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.AppendLine(Tab(1) + "[ServiceContract]");
                sb.AppendLine(Tab(1) + "public interface I" + friendlyName + "Service");
                sb.AppendLine(Tab(1) + "{");
                sb.AppendLine(Tab(2) + "[OperationContract]");
                sb.AppendLine(Tab(2) + "List<" + className + "> List" + friendlyName + "();");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(2) + "[OperationContract]");
                sb.AppendLine(Tab(2) +  friendlyName + " Get" + friendlyName + "(" + pkFieldType + " id);");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(2) + "[OperationContract]");
                sb.AppendLine(Tab(2) + "OperationStatus Add" + friendlyName + "(" + className + " newEntity);");
                sb.AppendLine(Tab(2) + "[OperationContract]");
                sb.AppendLine(Tab(2) + "OperationStatus Update" + friendlyName + "(" + className + " editEntity);");
                sb.AppendLine(Tab(2) + "[OperationContract]");
                sb.AppendLine(Tab(2) + "OperationStatus Delete" + friendlyName + "(" + className + " deleteEntity);");
               
                //TODO - add other CRUD methods here:

                sb.AppendLine(Tab(1) + "}");

                sb.AppendLine(string.Empty);
                // sb.AppendLine(Tab(2) + "#endregion");

                // sb.AppendLine(Tab(1) + "}"); // end class

                //  sb.AppendLine(Tab(0) + "}"); // end namespace
                return sb.ToString();
            }

            catch (Exception)
            {
                throw;
            }


        }
      
        private void GenerateServiceInterfaceClassesForSilverLightUI(List<string> _lstTables)
        {
            StringBuilder sb = new StringBuilder();
            string prefixname = this.ModelNamespace.Substring(this.ModelNamespace.LastIndexOf('\\') + 1).Replace("Model.edmx", "");

            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.ServiceModel;");
                sb.AppendLine("using DALEFModel;");
                sb.AppendLine("using Common;");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(0) + "namespace " + ApplicationName + "Services");

                sb.AppendLine(Tab(0) + "{");
                
                foreach (string strTableName in _lstTables)
                {
                    string _className = string.Empty;

                    Table objTable = _database.Tables[strTableName];

                    _className = objTable.Name;

                    string friendlyName = _className;
                    //string orderByField = "";
                    string pkFieldName = _className + "ID";
                    string pkFieldType = "int";

                    if (dctFriendlyNames.ContainsKey(_className))
                        friendlyName = dctFriendlyNames[_className];

                    //if (dctOrderByFields.ContainsKey(_className))
                    //    orderByField = dctOrderByFields[_className];
                    foreach (Column col in objTable.Columns)
                    {
                        if (col.InPrimaryKey)
                        {
                            pkFieldName = col.Name;
                            pkFieldType = this.ConvertDataType(col.DataType.Name, col.Nullable);
                            break;
                        }
                    }
                    sb.Append(this.GenerateServiceInterfaceClassCRUDForSilverLightUI(friendlyName, _className, pkFieldType, pkFieldName, prefixname));
                }

                sb.AppendLine(string.Empty);
                // sb.AppendLine(Tab(2) + "#endregion");

              //  sb.AppendLine(Tab(1) + "}"); // end class

                sb.AppendLine(Tab(0) + "}"); // end namespace
            }

            catch (Exception)
            {
                throw;
            }

            //Save the code
            try
            {
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(FolderPathAPIControllerInterface, "I" + prefixname + "Service.cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GenerateServiceInterfaceClassCRUDForSilverLightUI(string friendlyName, string className, string pkFieldType, string pkFieldName, string prefixname)
        {
            StringBuilder sb = new StringBuilder();
            
            try
            {
                sb.AppendLine(Tab(1) + "[ServiceContract]");
                sb.AppendLine(Tab(1) + "public interface I" + friendlyName + "Service");
                sb.AppendLine(Tab(1) + "{");
                sb.AppendLine(Tab(2) + "List<" + className + "> List" + friendlyName + "();");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(2) + "[OperationContract(AsyncPattern = true)]");
                sb.AppendLine(Tab(2) + "IAsyncResult BeginGet" + friendlyName + "(" + pkFieldType + " id, AsyncCallback callback, object state);");
                sb.AppendLine(string.Empty);            
                sb.AppendLine(Tab(2) + "[OperationContract(AsyncPattern = true)]");
                sb.AppendLine(Tab(2) + "IAsyncResult BeginAdd" + friendlyName + "(" + className + " newEntity, AsyncCallback callback, object state);");
                sb.AppendLine(Tab(2) + "[OperationContract(AsyncPattern = true)]");
                sb.AppendLine(Tab(2) + "IAsyncResult BeginUpdate" + friendlyName + "(" + className + " editEntity, AsyncCallback callback, object state);");
                sb.AppendLine(Tab(2) + "[OperationContract(AsyncPattern = true)]");
                sb.AppendLine(Tab(2) + "IAsyncResult BeginDelete" + friendlyName + "(" + className + " deleteEntity, AsyncCallback callback, object state);");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(2) + "OperationStatus EndAdd" + friendlyName + "(IAsyncResult result);");
                sb.AppendLine(Tab(2) + "OperationStatus EndUpdate" + friendlyName + "(IAsyncResult result);");
                sb.AppendLine(Tab(2) + "OperationStatus EndDelete" + friendlyName + "(IAsyncResult result);");
                sb.AppendLine(Tab(2) + "OperationStatus EndGet" + friendlyName + "(IAsyncResult result);");
                
                //TODO - add other CRUD methods here:

                sb.AppendLine(Tab(1) + "}");

                sb.AppendLine(string.Empty);
                // sb.AppendLine(Tab(2) + "#endregion");

               // sb.AppendLine(Tab(1) + "}"); // end class

              //  sb.AppendLine(Tab(0) + "}"); // end namespace
                return sb.ToString();
            }

            catch (Exception)
            {
                throw;
            }

            
        }

        private void GenerateControllerApiClasses(List<string> _lstTables)
        {
            StringBuilder sb = new StringBuilder();
            AdminManager manager = new AdminManager();
            OperationStatus status = new OperationStatus();

            try
            {
                sb.AppendLine("using System.Net;");
                sb.AppendLine("using System.Net.Http;");
                sb.AppendLine("using System.Web.Http;");
                sb.AppendLine("using Common;");
                sb.AppendLine("using DSDBLL;");
                sb.AppendLine("using DALEFModel;");
                                
                sb.AppendLine(string.Empty);
                // int index = this.ApplicationName.LastIndexOf("\\");
                  sb.AppendLine(Tab(0) + "namespace Controllers");

                sb.AppendLine(Tab(0) + "{");
                sb.AppendLine(Tab(0) + "[RoutePrefix(\"" + CtrlMngSvcName + "Controller\")]");

                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(1) + "public class " + CtrlMngSvcName + "Controller : BaseApiController");
                sb.AppendLine(Tab(1) + "{");
                //busy here-------------------------------------------------------------------------------------------              
                sb.AppendLine(string.Empty);

                sb.AppendLine(Tab(2) + "" + CtrlMngSvcName + "Manager manager = new " + CtrlMngSvcName + "Manager();");

                foreach (string strTableName in _lstTables)
                {
                    string _className = string.Empty;
                    Entity entity = manager.GetEntity(strTableName, true);
                    if (entity == null || entity.EntityField.Count() == 0)
                        return;


                    //string orderByField = "";
                    string pkFieldName = "";
                    string pkFieldType = "";

                    //if (dctFriendlyNames.ContainsKey(strTableName))
                    //    friendlyName = dctFriendlyNames[strTableName];

                    //if (dctOrderByFields.ContainsKey(_className))
                    //    orderByField = dctOrderByFields[_className];
                    foreach (EntityField col in entity.EntityField)
                    {
                        if (col.IsPrimaryKey)
                        {
                            pkFieldName = col.EntityFieldName;
                            pkFieldType = "int";//this.ConvertDataType(col.EntityFieldDataType.Type, false);
                            break;
                        }
                    }
                    sb.Append(this.GenerateControllerApiCRUD(strTableName, pkFieldType, pkFieldName));
                }

                //sb.AppendLine(string.Empty);
                //sb.AppendLine(Tab(2) + "#region Dispose");
                //sb.AppendLine(Tab(2) + "public void Dispose()");
                //sb.AppendLine(Tab(2) + "{");
                //sb.AppendLine(Tab(2) + "}");
                //sb.AppendLine(Tab(2) + "#endregion");

                sb.AppendLine(Tab(1) + "}"); // end class

                sb.AppendLine(Tab(0) + "}"); // end namespace
            }

            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                //TODO apend all code in the custom region from old file to new one
                //if(File Exists)
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(this.FolderPathAPIController, CtrlMngSvcName + "Controller.cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GenerateControllerApiCRUD(string className, string pkFieldType, string pkFieldName)
        {
            AdminManager manager = new AdminManager();
            Entity entity = manager.GetEntity(className, true);
            if (entity == null || entity.EntityField.Count() == 0)
            {
                return "NO Entity or Entity fields defined for " + className;
            }
            StringBuilder sb = new StringBuilder();
            
            //--Get List ------------------------
            sb.AppendLine(string.Empty);
            //Get page action call for mvc
            sb.AppendLine(Tab(2) + "#region " + className);
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// Gets all " + className + "s.");
            sb.AppendLine(Tab(2) + "/// <param name=\"filter\">The filter.</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "/// </summary>");
            //TODO add webapi name in UI
            sb.AppendLine(Tab(2) + "[Route(\"" + className + "\")]");
            sb.AppendLine(Tab(2) + "[HttpPost]");
            sb.AppendLine(Tab(2) + "public HttpResponseMessage Get" + className + "List([FromBody] GridParam filter)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "HttpResponseMessage response = new HttpResponseMessage();");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "GridResult<" + className + "> result = new GridResult<" + className + ">();");
            sb.AppendLine(Tab(4) + "result = manager.Get" + className + "(filter);");
            sb.AppendLine(Tab(4) + "response = Request.CreateResponse(HttpStatusCode.OK, result);");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "return response;");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);
            
            //----------------Get by id -----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// GET " + className);
            sb.AppendLine(Tab(2) + "/// <param name=\"id\">The identifier.</param>");
            sb.AppendLine(Tab(2) + "/// <param name=\"includerelations\">The includerelation.</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "/// </summary>");
            //TODO add webapi name in UI
            sb.AppendLine(Tab(2) + "[Route(\"" + className + "/{id?}/{includerelations?}\")]");
            sb.AppendLine(Tab(2) + "public HttpResponseMessage Get" + className + "(int id, string includerelations)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "HttpResponseMessage response = new HttpResponseMessage();");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "var returnItem = manager.Get" + className + "(id, System.Convert.ToBoolean(includerelations));");
            sb.AppendLine(Tab(4) + "if (returnItem == null)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + "response = Request.CreateResponse(HttpStatusCode.NotFound);");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "response = Request.CreateResponse(HttpStatusCode.OK, returnItem);");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "return response;");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);
            //----------------Add -----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// Puts the specified " + className + ".");
            sb.AppendLine(Tab(2) + "/// <param name = \"" + className + "\" > The " + className + ".</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "/// </summary>");
            //TODO add webapi name in UI
            sb.AppendLine(Tab(2) + "[Route(\"" + className + "/add\")]");
            sb.AppendLine(Tab(2) + "[HttpPost]");
            sb.AppendLine(Tab(2) + "public HttpResponseMessage Add" + className + "([FromBody]" + className + " newItem)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "HttpResponseMessage response = new HttpResponseMessage();");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "if (newItem != null)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + "" + className + " new" + className + " = manager.AddReturn" + className + "(newItem);");
            sb.AppendLine(Tab(5) + "response = Request.CreateResponse<" + className + ">(HttpStatusCode.Created, new" + className + ");");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "return response;");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);
            //----------------Update -----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// Updates " + className + ".");
            sb.AppendLine(Tab(2) + "/// <param name = \"" + className + "\" > The " + className + ".</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "/// </summary>");
            //TODO add webapi name in UI
            sb.AppendLine(Tab(2) + "[Route(\"" + className + "/update\")]");
            sb.AppendLine(Tab(2) + "[HttpPost]");
            sb.AppendLine(Tab(2) + "public HttpResponseMessage Update" + className + "([FromBody]" + className + " newItem)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "HttpResponseMessage response = new HttpResponseMessage();");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "if (newItem != null)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + "int new" + className + " = manager.Update" + className + "(newItem);");
            sb.AppendLine(Tab(5) + "response = Request.CreateResponse(HttpStatusCode.Created,newItem."+ className +"ID.ToString());");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "return response;");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);
            //----------------Delete -----------------------------------------
            sb.AppendLine(Tab(2) + "/// <summary>");
            sb.AppendLine(Tab(2) + "/// Deletes " + className + ".");
            sb.AppendLine(Tab(2) + "/// <param name=\"id\">The identifier.</param>");
            sb.AppendLine(Tab(2) + "/// <returns></returns>");
            sb.AppendLine(Tab(2) + "/// </summary>");
            //TODO add webapi name in UI
            sb.AppendLine(Tab(2) + "[Route(\"" + className + "/delete/{id?}\")]");
            sb.AppendLine(Tab(2) + "[HttpPost]");
            sb.AppendLine(Tab(2) + "public HttpResponseMessage Delete" + className + "(int id)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "HttpResponseMessage response = new HttpResponseMessage();");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(5) + " int row = manager.Delete" + className + "(id);");
            sb.AppendLine(Tab(5) + "response = Request.CreateResponse(HttpStatusCode.OK,row.ToString());");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "catch (System.Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "return response;");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(Tab(2) + "#endregion");
            //TODO - add other CRUD methods here:
            return sb.ToString();

        }
        private void GenerateManagerClasses(List<string> _lstTables)
        {
            StringBuilder sb = new StringBuilder();
            
            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using System.Text;");
                sb.AppendLine("using System.Linq.Expressions;");
                sb.AppendLine("using DALEFModel;");//Change to Bl folder 
                sb.AppendLine("using Common;");
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(0) + "namespace " + ApplicationName);

                sb.AppendLine(Tab(0) + "{");

                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(1) + "public sealed class " + CtrlMngSvcName + "Manager : BaseManager,IDisposable");
                sb.AppendLine(Tab(1) + "{");
                
                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(2) + "public " + CtrlMngSvcName + "Manager()");
                sb.AppendLine(Tab(2) + "{");
                sb.AppendLine(Tab(3) + "base.Model = Enumerations.ModalContext.EzFloManagerEntities;");
                sb.AppendLine(Tab(2) + "}");
                
                sb.AppendLine(string.Empty);
               
                foreach (string strTableName in _lstTables)
                {
                    //string friendlyName = strTableName;
                    //string orderByField = "";
                    string pkFieldName = strTableName + "ID";
                    string pkFieldType = "int";

                    //if (dctFriendlyNames.ContainsKey(strTableName))
                    //    friendlyName = dctFriendlyNames[strTableName];

                    //if (dctOrderByFields.ContainsKey(_className))
                    //    orderByField = dctOrderByFields[_className];
                    AdminManager manager = new AdminManager();
                    Entity entity = manager.GetEntity(strTableName, true);
                    if (entity != null || entity.EntityField.Count() > 0)
                    {
                        foreach (EntityField col in entity.EntityField)
                        {

                            if (col.IsPrimaryKey)
                            {
                                pkFieldName = col.EntityFieldName;
                                //pkFieldType = this.ConvertDataType(col.DataType.Name, col.Nullable);
                                break;
                            }
                        }
                    }
                    sb.Append(this.GenerateManagerCRUD(strTableName, pkFieldType, pkFieldName));
                }

                sb.AppendLine(string.Empty);
                sb.AppendLine(Tab(2) + "#region Dispose");
                sb.AppendLine(Tab(2) + "public void Dispose()");
                sb.AppendLine(Tab(2) + "{");
                sb.AppendLine(Tab(2) + "}");
                sb.AppendLine(Tab(2) + "#endregion");
        
                sb.AppendLine(Tab(1) + "}"); // end class

                sb.AppendLine(Tab(0) + "}"); // end namespace
            }

            catch (Exception ex)
            {
                throw ex;
            }

            //Save the code
            try
            {
                //TODO apend all code in the custom region from old file to new one
                //if(File Exists)
                using (StreamWriter oStreamWriter = new StreamWriter(Path.Combine(FolderPathAPIManager, CtrlMngSvcName + "Manager.cs")))
                {
                    oStreamWriter.Write(sb.ToString());
                    oStreamWriter.Flush();
                    oStreamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        private string GenerateManagerCRUD(string className, string pkFieldType, string pkFieldName)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(2) + "#region " + className + " Methods");
            sb.AppendLine(string.Empty);
            //----------------Getlist Method-----------------------------------------
            sb.AppendLine(Tab(2) + "public IQueryable<" + className + "> GetAll" + className + "()");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + " try");
            sb.AppendLine(Tab(3) + " {");
            sb.AppendLine(Tab(4) + "   return Repository.GetList<"+className+">();");
            sb.AppendLine(Tab(3) + " }catch(Exception ex)");
            sb.AppendLine(Tab(3) + " {");
            sb.AppendLine(Tab(4) + "   throw ex;");
            sb.AppendLine(Tab(3) + " }");
            sb.AppendLine(Tab(2) + "}");
            sb.AppendLine(string.Empty);
            //----------------Get Method-----------------------------------------
            sb.AppendLine(Tab(2) + "public " + className + " Get" + className + "(int id,bool includeRelations)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "if (includeRelations)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "//Include child objects");
            sb.AppendLine(Tab(5) + "List<Expression<Func<" + className + ", object>>> includepaths = new List<Expression<Func<" + className + ", object>>>();");
            sb.AppendLine(Tab(5) + "includepaths.Add(p => p.Organization);");
            sb.AppendLine(Tab(5) + "includepaths.Add(p => p.User);");
            sb.AppendLine(Tab(5) + "includepaths.Add(p => p.User1);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "// Add includepaths into method here if used and not null");
            sb.AppendLine(Tab(5) + "return Repository.Get<" + className + ">(includepaths, p => p." + pkFieldName + " == id);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "}");  
             sb.AppendLine(Tab(4) + "else");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "return Repository.Get<" + className + ">(null,p => p." + pkFieldName + " == id);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "}catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "throw ex;");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");
            
            sb.AppendLine(string.Empty);

            //----------------List Methods-----------------------------------------
            sb.AppendLine(Tab(2) + "public GridResult<" + className + "> Get" + className + "(GridParam filters)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{"); 
            sb.AppendLine(Tab(4) + "GridResult<" + className + "> result = new GridResult<" + className + ">();");
            sb.AppendLine(Tab(4) + "//Get total rows before filtering is applied");
            sb.AppendLine(Tab(4) + "result.TotalCount = this.GetList<" + className + ">().Count();");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "Expression <Func<" + className + ", bool>> where = null;");
            sb.AppendLine(Tab(4) + "if (filters.ListFilterBy != null)");
            sb.AppendLine(Tab(4) + "{");
              sb.AppendLine(Tab(5) + "foreach (FilterField field in filters.ListFilterBy)");
              sb.AppendLine(Tab(6) + "{");
                sb.AppendLine(Tab(7) + "if (field.Property.Length == 0 || field.Operator.Length == 0 || field.Value.Length == 0)");
                 sb.AppendLine(Tab(7) + "{");
                     sb.AppendLine(Tab(8) + "throw new Exception(\"A Filter field has not been specified properly.\");");
                 sb.AppendLine(Tab(7) + "}");
                 sb.AppendLine(Tab(7) + "if (where == null)");
                 sb.AppendLine(Tab(7) + "{");
                    sb.AppendLine(Tab(8) + "where = Common.QueryHelpers.BuildFilter.BuildWhereClause<" + className + ">(field.Property, field.Operator, field.Value);");
                 sb.AppendLine(Tab(7) + "}");
                 sb.AppendLine(Tab(7) + "else");
                 sb.AppendLine(Tab(7) + "{");
                    sb.AppendLine(Tab(7) + " where = Common.QueryHelpers.BuildFilter.BuildWhereClause<" + className + ">(field.Property, field.Operator, field.Value, where);");
                 sb.AppendLine(Tab(7) + "}");
              sb.AppendLine(Tab(6) + "}");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "IQueryable<" + className + "> list = null;");
            sb.AppendLine(Tab(4) + "if (filters.Includerelations)// Add includepaths into method ");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "//Add Includes here example below");
            //TODO add logic to add custom code back in here
            //Could add where and includes on entity table etc
            sb.AppendLine(Tab(5) + "//Include child objects");
            sb.AppendLine(Tab(5) + "List<Expression<Func<" + className + ", object>>> includepaths = new List<Expression<Func<" + className + ", object>>>();");
            sb.AppendLine(Tab(5) + "includepaths.Add(p => p.Organization);");
            sb.AppendLine(Tab(5) + "includepaths.Add(p => p.User);");
            sb.AppendLine(Tab(5) + "includepaths.Add(p => p.User1);");
           
            
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(5) + "//APPLY FILTERS");
            sb.AppendLine(Tab(5) + "if (where != null)");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(5) + "list = Repository.GetList<" + className + ", string>(includepaths, where);");
            sb.AppendLine(Tab(5) + "}");
            sb.AppendLine(Tab(5) + "else");
            sb.AppendLine(Tab(5) + "{");
            sb.AppendLine(Tab(5) + "list = Repository.GetList<" + className + ", string>(includepaths);");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(3) + "else");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "if (where != null)");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(4) + "list = Repository.GetList<" + className + ">(where).OrderBy(m => m." + pkFieldName + ");");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "else");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(4) + "list = Repository.GetList<" + className + ">().OrderBy(m => m." + pkFieldName + ");");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "//APPLY ALL SORTING");
            sb.AppendLine(Tab(3) + "if (filters.ListOrderBy != null && filters.ListOrderBy.Count() > 0)");
            sb.AppendLine(Tab(4) + "{");
                sb.AppendLine(Tab(5) + "foreach (var sort in filters.ListOrderBy)");
                sb.AppendLine(Tab(6) + "{");
                    sb.AppendLine(Tab(7) + "if (sort.Property.Length == 0 || sort.Value.Length == 0)");
                    sb.AppendLine(Tab(7) + "{");
                      sb.AppendLine(Tab(8) + " throw new Exception(\"A sort field has not been specified properly.\");");
                    sb.AppendLine(Tab(7) + "}");
                    sb.AppendLine(Tab(7) + "list = list.OrderBy(sort.Property, sort.Value);");
                sb.AppendLine(Tab(7) + "}");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(Tab(4) + "else");
            sb.AppendLine(Tab(4) + "{");
            sb.AppendLine(Tab(5) + "list = list.OrderBy(o => o." + pkFieldName + ");");
            sb.AppendLine(Tab(4) + "}");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(4) + "//Get total filtered rows before paging is applied ");
            sb.AppendLine(Tab(4) + "result.TotalFilteredCount = list.Count();");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "//APPLY PAGE SIZE");
            sb.AppendLine(Tab(3) + "list = list.Skip(filters.PageNo).Take(filters.PageSize);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "result.Items = list;");
            sb.AppendLine(Tab(3) + "return result;");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "}catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "throw ex;");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");
            


           // sb.AppendLine(Tab(2) + "public IQueryable<" + className + "> Get" + className + "(bool includeRelations, Expression<Func<" + className + ", bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)");
           // sb.AppendLine(Tab(2) + "{");
           // sb.AppendLine(Tab(3) + "try");
           // sb.AppendLine(Tab(3) + "{");
           // sb.AppendLine(string.Empty);
           // sb.AppendLine(Tab(4) + "IQueryable<" + className + "> list = null;");
           // sb.AppendLine(Tab(4) + "if (includeRelations)// Add includepaths into method ");   
           // sb.AppendLine(Tab(4) + "{");   
           // sb.AppendLine(string.Empty);
           //     sb.AppendLine(Tab(5) + "//Add Includes here example below");
           //     //TODO add logic to add custom code back in here
           //     //Could add where and includes on entity table etc
           //     sb.AppendLine(Tab(5) + "//STARTCUSTOMCODE");
           //     sb.AppendLine(Tab(5) + "//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();");
           //     sb.AppendLine(Tab(5) + "//includepaths.Add(p => p.StpDataType);");
           //     sb.AppendLine(string.Empty);
           //sb.AppendLine(Tab(5) + "//APPLY FILTERS");                                
           //sb.AppendLine(Tab(5) + "//if (where != null)");        
           //sb.AppendLine(Tab(5) + "//{");
           //sb.AppendLine(Tab(5) + "//list = Repository.GetList<" + className + ", string>(includepaths, where);");          
           //sb.AppendLine(Tab(5) + "//}");               
           //sb.AppendLine(Tab(5) + "//else");         
           //sb.AppendLine(Tab(5) + "//{");
           //sb.AppendLine(Tab(5) + "//list = Repository.GetList<" + className + ", string>(includepaths);");         
           //sb.AppendLine(Tab(4) + "//}");             
           //sb.AppendLine(Tab(3) + "}");          
           //sb.AppendLine(Tab(3) + "else");   
           //sb.AppendLine(Tab(3) + "{");     
           //sb.AppendLine(Tab(4) + "if (where != null)");     
           //sb.AppendLine(Tab(4) + "{");
           //sb.AppendLine(Tab(4) + "list = Repository.GetList<" + className + ">(where).OrderBy(m => m." + pkFieldName + ");");         
           //sb.AppendLine(Tab(4) + "}");          
           //sb.AppendLine(Tab(4) + "else");             
           //sb.AppendLine(Tab(4) + "{");
           //sb.AppendLine(Tab(4) + "list = Repository.GetList<" + className + ">().OrderBy(m => m." + pkFieldName + ");");            
           //sb.AppendLine(Tab(4) + "}");          
           //sb.AppendLine(Tab(3) + "}");             
           //sb.AppendLine(string.Empty);        
           //sb.AppendLine(Tab(3) + "//APPLY ALL SORTING");  
           //sb.AppendLine(Tab(3) + "if (listOrderBy != null && listOrderBy.Count() > 0)");    
           //sb.AppendLine(Tab(3) + "{");     
           //sb.AppendLine(Tab(3) + "foreach (var sort in listOrderBy)");      
           //sb.AppendLine(Tab(3) + "{");
           //sb.AppendLine(Tab(3) + "list = QueryFilters.Sort<" + className + ">(list.ToList(), sort.Key, sort.Value).AsQueryable();");     
           //sb.AppendLine(Tab(3) + "}");          
           //sb.AppendLine(Tab(3) + "}");         
           //sb.AppendLine(string.Empty); 
           //sb.AppendLine(Tab(3) + "//APPLY PAGE SIZE");     
           //sb.AppendLine(Tab(3) + "if (page > 0 && size > 0)");          
           //sb.AppendLine(Tab(3) + "{");      
           //sb.AppendLine(Tab(4) + "list = list.Skip((page - 1) * size).Take(size);");     
           //sb.AppendLine(Tab(3) + "}");     
           //sb.AppendLine(Tab(3) + "return list;");

           //sb.AppendLine(string.Empty);
           //sb.AppendLine(Tab(3) + "}catch(Exception ex)");
           //sb.AppendLine(Tab(3) + "{");
           //sb.AppendLine(Tab(4) + "throw ex;");
           //sb.AppendLine(Tab(3) + "}");
           //sb.AppendLine(Tab(2) + "}");   
           //------------------------------------------------------------------------------------------------------------ 


           
            //----------------Add Method-----------------------------------------
            sb.AppendLine(Tab(2) + "public int Add" + className + "(" + className + " newEntity)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off");
            sb.AppendLine(Tab(4) + "return Repository.UoW.Add<" + className + ">(newEntity);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "}catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "throw ex;");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(Tab(2) + "public " + className + " AddReturn" + className + "(" + className + " newEntity)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off");
            sb.AppendLine(Tab(4) + "return Repository.UoW.AddEntityReturnEntity<" + className + ">(newEntity);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "}catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "throw ex;");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");
            sb.AppendLine(string.Empty);

            //----------------Update Method-----------------------------------------
            sb.AppendLine(Tab(2) + "public int Update" + className + "(" + className + " editEntity)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off");
            sb.AppendLine(Tab(4) + "return Repository.UoW.Update<" + className + ">(editEntity);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "}catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "throw ex;");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");

            sb.AppendLine(string.Empty);
            
            //----------------Delete Method-----------------------------------------
            
            sb.AppendLine(Tab(2) + "public int Delete" + className + "(int id)");
            sb.AppendLine(Tab(2) + "{");
            sb.AppendLine(Tab(3) + "try");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + className + " item = this.Get" + className + "(id, false);");
            sb.AppendLine(Tab(4) + "return Repository.UoW.Delete<" + className + ">(item);");
            sb.AppendLine(string.Empty);
            sb.AppendLine(Tab(3) + "}catch(Exception ex)");
            sb.AppendLine(Tab(3) + "{");
            sb.AppendLine(Tab(4) + "throw ex;");
            sb.AppendLine(Tab(3) + "}");
            sb.AppendLine(Tab(2) + "}");
           
            sb.AppendLine(Tab(2) + "#endregion");
            //TODO - add other CRUD methods here:
            return sb.ToString();

        }

        private string StripForeignKey(string colName)
        {
            return colName.Replace("ID", "");
        }

        private string Tab(int numTabs)
        {
            string tabs = string.Empty;

            for (int i = 0; i < numTabs; i++)
                tabs += "\t";
            
            return tabs;
        }

        private string ConvertDataType(string sqlTypeName, bool isNullable)
        {            
            switch (sqlTypeName.ToLower())
            {                    
                case "datetime":
                case "smalldatetime":
                    if(isNullable)
                        return "DateTime?";
                    else
                        return "DateTime";                                  

                case "bit":
                    if (isNullable)
                        return "bool?";
                    else
                        return "bool";

                case "uniqueidentifier":
                    if (isNullable)
                        return "Guid?";
                    else
                        return "Guid";

                case "varchar":
                case "char":
                case "text":
                case "nvarchar":
                case "nchar":
                case "ntext":                
                    return "string";

                case "int":
                    if (isNullable)
                        return "int?";
                    else
                        return "int";

                case "bigint":
                    if (isNullable)
                        return "long?";
                    else
                        return "long";

                case "numeric":
                    if(isNullable)
                        return "decimal?";
                    else
                        return "decimal";

                case "sql_variant":
                    return "object";

                default: 
                    return sqlTypeName;                    
            }
        }

        //Checks if the table has at least one nullable FK field
        private bool HasNullableFK(Table objTable)
        {
            bool hasNullableFK = false;

            foreach (Column objColumn in objTable.Columns)
            {
                if (objColumn.IsForeignKey && !objColumn.InPrimaryKey && objColumn.Nullable)
                {
                    hasNullableFK = true;
                    break;
                }
            }

            return hasNullableFK;
        }

        private List<string> GetNullableFkFields(Table objTable)
        {
            List<string> lstFields = new List<string>();

            foreach (Column objColumn in objTable.Columns)
            {
                if (objColumn.IsForeignKey && !objColumn.InPrimaryKey && objColumn.Nullable)
                {
                    lstFields.Add(objColumn.Name);
                }
            }

            return lstFields;
        }
        
        //Checks that the column name supplied is in fact a field on the table
        private bool TableContainsColumn(Table objTable, string colName)
        {
            bool isInTable = false;

            foreach (Column objColumn in objTable.Columns)
            {
                if (objColumn.Name == colName)
                {
                    isInTable = true;
                    break;
                }
            }

            return isInTable;
        }

        //Checks that the column name supplied is in fact a field on the table
        private bool TableContainsColumn(string tableName, string colName)
        {
            Table objTable = _database.Tables[tableName];
            if (objTable != null)
                return this.TableContainsColumn(objTable, colName);
            else
                return false;
        }
    }
}
