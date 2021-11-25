using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MySql.Data.MySqlClient;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;


namespace CyroGenerateTool
{
    public partial class frmGenerator : Form
    {
        private Server _server = new Server();
        private Database _database = null;
        
        private bool isBinding = false;
        string defaultDb = string.Empty;

        public string _defaultApplicationName = string.Empty;
        private string _defaultPathMetaData = string.Empty;
        private string _defaultPathService = string.Empty;
        private string _defaultPathManager = string.Empty;
        private string _defaultPathController = string.Empty;

        public frmGenerator()
        {
            InitializeComponent();            
        }

        private void frmGenerator_Load(object sender, EventArgs e)
        {
            isBinding = true;

            Initialize();

            if (tbConnString.Text.Trim().Length > 0)
            {
                string connectionString = tbConnString.Text.Trim();
                _server.ConnectionContext.ConnectionString = tbConnString.Text;                
                PopulateDatabases();
            }

            isBinding = false;            
        }

        private void Initialize()
        {
          //  string drive =  ConfigurationSettings.AppSettings["defaultDrive"];
            _defaultPathMetaData = ConfigurationSettings.AppSettings["ouputPathMetaData"];
            _defaultPathService = ConfigurationSettings.AppSettings["ouputPathServices"];

            _defaultPathManager =  ConfigurationSettings.AppSettings["ouputPathManagers"];
            _defaultPathController =  ConfigurationSettings.AppSettings["ouputPathControllers"];

            tbMetaDataFolder.Text = _defaultPathMetaData;
            tbServiceDirectory.Text = _defaultPathService;
            tbManagerDirectory.Text = _defaultPathManager;
            tbControllerPath.Text = _defaultPathController;

            tbModelsFolder.Text = "";


            tbConnString.Text = ConfigurationSettings.AppSettings["connectionString"];

            defaultDb = ConfigurationSettings.AppSettings["defaultDB"];
        }

        private void PopulateDatabases()
        {
            cmbDatabase.Items.Clear();
            //Adding ONLY sql databases
            //foreach (Database oDB in _server.Databases)
            //{
            //    cmbDatabase.Items.Add(oDB.Name);
            //}

            //Adding Sql compact databases here as well
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                cmbDatabase.Items.Add(ConfigurationManager.ConnectionStrings[i].Name);
            }

                //if (cmbDatabase.Items.Contains(defaultDb))
                //{
                //    cmbDatabase.SelectedItem = defaultDb;

                //    if (_server.Databases.Contains(defaultDb))
                //        _database = _server.Databases[defaultDb];


                //}

            //    PopulateTablesList();
        }

        private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isBinding) return;

            //if (cmbDatabase.SelectedItem != null)
            //{
            //    _database = _server.Databases[cmbDatabase.SelectedItem.ToString()];

                PopulateTablesList();
            //}
        }

        private void PopulateTablesList()
        {
            //if (cmbDatabase.SelectedItem == null)
            //{
            //    return;
            //}
            lstTables.Items.Clear();
            if (cmbDatabase.SelectedItem.ToString().Contains("My"))
            {

                try
                {
                    using (MySqlConnection sqlCon = new MySqlConnection(ConfigurationManager.ConnectionStrings[cmbDatabase.SelectedItem.ToString()].ConnectionString))
                    {
                        sqlCon.Open();
                        // Read in all values in the table.
                        using (MySqlCommand com = new MySqlCommand("select * from information_Schema.tables order by table_name", sqlCon))
                        {
                            MySqlDataReader reader = com.ExecuteReader();
                            lstTables.Items.Clear();
                            while (reader.Read())
                            {
                                if (reader["TABLE_SCHEMA"].ToString() == sqlCon.Database)
                                {
                                    if (reader["TABLE_TYPE"].ToString() == "BASE TABLE")
                                    {
                                        lstTables.Items.Add(reader["TABLE_NAME"]);
                                    }
                                }

                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (cmbDatabase.SelectedItem.ToString().Substring(0, 5) == "SQLCE")
            {

                try
                {
                    using (SqlCeConnection sqlCon = new SqlCeConnection(ConfigurationManager.ConnectionStrings[cmbDatabase.SelectedItem.ToString()].ConnectionString))
                    {
                        sqlCon.Open();
                        // Read in all values in the table.
                        using (SqlCeCommand com = new SqlCeCommand("select * from information_Schema.tables order by table_name", sqlCon))
                        {
                            SqlCeDataReader reader = com.ExecuteReader();
                            lstTables.Items.Clear();
                            while (reader.Read())
                            {
                                lstTables.Items.Add(reader[2]);

                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                try
                {

                    //using (SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings[cmbDatabase.SelectedItem.ToString()].ConnectionString))
                   // using (SqlConnection sqlCon = new SqlConnection("Data Source =ZA-PLZ-L-ROBINC; Database = CyroTechManager; User Id = sa; Password = ThornFarm1!"))
                    using (SqlConnection sqlCon = new SqlConnection(this.tbConnString.Text))
                    {
                        sqlCon.Open();
                        // Read in all values in the table.
                        using (SqlCommand com = new SqlCommand("select * from information_Schema.tables order by table_name", sqlCon))
                        {
                            SqlDataReader reader = com.ExecuteReader();
                            lstTables.Items.Clear();
                            while (reader.Read())
                            {
                                lstTables.Items.Add(reader[2]);

                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                //if (_database == null) return;

                //foreach (Table sqlTable in _database.Tables)
                //{
                //    lstTables.Items.Add(sqlTable.Name);
                //}
            }
        }
         
        public override bool ValidateChildren()
        {
            bool valid = true;

            if (this.chkGenMetaFile.Checked && tbMetaDataFolder.Text.Trim().Length == 0)
                valid = false;
            if (!valid)
                errValidation.SetError(tbMetaDataFolder, "Valid output path required.");
            else
                errValidation.SetError(tbMetaDataFolder, "");

            if (this.chkGenMng.Checked && this.tbManagerDirectory.Text.Trim().Length == 0)
                valid = false;
            if (!valid)
                errValidation.SetError(tbManagerDirectory, "Valid output path required.");
            else
                errValidation.SetError(tbManagerDirectory, "");
            
            if (chkGenMng.Checked || chkGenMVCCntrl.Checked || chkGenSrv.Checked)
            {
                if (this.tbCtrlMngSvcName.Text.Trim().Length == 0)
                    valid = false;
                if (!valid)
                    errValidation.SetError(tbCtrlMngSvcName, "Valid name required.");
                else
                    errValidation.SetError(tbCtrlMngSvcName, "");
            }

            if (cmbDatabase.SelectedIndex < 0)
            {
                errValidation.SetError(cmbDatabase, "Database selection required.");
                valid = false;
            }
            else
                errValidation.SetError(cmbDatabase, "");


            if (lstTables.SelectedItems.Count == 0)
            {
                errValidation.SetError(lstTables, "Select at least one table.");
                valid = false;
            }
            else
                errValidation.SetError(lstTables, "");

            if (string.IsNullOrWhiteSpace(tbModelsFolder.Text))
            {
                errValidation.SetError(tbModelsFolder, "Enter the sub-folder name of the .edmx in the DAL Models folder");
                valid = false;
            }
            else
                errValidation.SetError(tbModelsFolder, "");


            //if (string.IsNullOrWhiteSpace(tbEntityContainerName.Text))
            //{
            //    errValidation.SetError(tbEntityContainerName, "Enter the entity container name of the .edmx model (check properties of the model diagram)");
            //    valid = false;
            //}
            //else
            //    errValidation.SetError(tbEntityContainerName, "");


            return valid;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;

            string strFolderToSaveMetaData = string.Empty;

            List<string> lstStrTables = new List<string>();

            strFolderToSaveMetaData = tbMetaDataFolder.Text;
            if (!strFolderToSaveMetaData.EndsWith(@"\")) strFolderToSaveMetaData += @"\";
            
            foreach (string tableName in lstTables.SelectedItems)
            {
                lstStrTables.Add(tableName);
            }

            CyroGenerator generator = new CyroGenerator(_database, lstStrTables);

            generator.FolderToSaveMetaData = strFolderToSaveMetaData;
            generator.GenerateMetadataFile = this.chkGenMetaFile.Checked;

            generator.FolderToSavePartialClass = txtPartialClassPath.Text;
            generator.GeneratePartialClass = this.ckbPartialClass.Checked;

            generator.FolderPathAPIManager = tbManagerDirectory.Text;
            generator.GenerateManagerClass = this.chkGenMng.Checked;

            generator.FolderPathAPIController = tbServiceDirectory.Text;
            generator.GenerateSvcFile = chkGenSrv.Checked;
            
            generator.FolderToSaveEditTemplates = tbEditTemplatePath.Text;
            generator.GenerateEditTemplates = chkGenEditTemplates.Checked;

            generator.FolderToSaveSetupForms = tbSetupFormPath.Text;
            generator.GenerateSetupForms = chkSetupForm.Checked;

            generator.FolderPathUIController = tbControllerPath.Text;
            generator.GenerateControllers = chkGenMVCCntrl.Checked;

            generator.ModelNamespace = tbModelsFolder.Text;

            generator.CtrlMngSvcName = tbCtrlMngSvcName.Text;

            generator.ApplicationName = txtFullAppPath.Text;// _defaultApplicationName;//.Substring(_defaultApplicationName.LastIndexOf('\\')+1);
            try
            {
                generator.Generate();
            }catch(Exception ex)
			{
                MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            MessageBox.Show(this, "Generation complete!", "GenerateMe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtFullAppPath_TextChanged(object sender, EventArgs e)
        {
			if (txtFullAppPath.Text.Length > 0)
			{
				//    this.tbModelsFolder.Text = ConfigurationSettings.AppSettings["ouputPathModel"];
				//    this.tbMetaDataFolder.Text = txtFullAppPath.Text + ConfigurationSettings.AppSettings["ouputPathMetaData"];
				//    this.tbControllerPath.Text = txtFullAppPath.Text + ConfigurationSettings.AppSettings["ouputPathControllers"];
				//    this.tbManagerDirectory.Text = txtFullAppPath.Text + ConfigurationSettings.AppSettings["ouputPathManagers"];

				//    this.tbSetupFormPath.Text = txtFullAppPath.Text + ConfigurationSettings.AppSettings["ouputPathSetup"];
				//    this.tbEditTemplatePath.Text = txtFullAppPath.Text + ConfigurationSettings.AppSettings["ouputPathEdit"];

				//    tbServiceDirectory.Text = txtFullAppPath.Text + ConfigurationSettings.AppSettings["ouputPathServices"];

				this._defaultApplicationName = txtFullAppPath.Text;
                   
                
            }
        }

        private void lstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tbCtrlMngSvcName.Text = lstTables.SelectedItems[0].ToString();
        }

		
	}
}
