namespace CyroGenerateTool
{
    partial class frmGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.tbConnString = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbMetaDataFolder = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ckbPartialClass = new System.Windows.Forms.CheckBox();
			this.lblPartialClass = new System.Windows.Forms.Label();
			this.txtPartialClassPath = new System.Windows.Forms.TextBox();
			this.lblFullAppPath = new System.Windows.Forms.Label();
			this.txtFullAppPath = new System.Windows.Forms.TextBox();
			this.chkGenSrv = new System.Windows.Forms.CheckBox();
			this.chkGenMng = new System.Windows.Forms.CheckBox();
			this.chkGenMVCCntrl = new System.Windows.Forms.CheckBox();
			this.tbEditTemplatePath = new System.Windows.Forms.TextBox();
			this.chkSetupForm = new System.Windows.Forms.CheckBox();
			this.chkGenEditTemplates = new System.Windows.Forms.CheckBox();
			this.tbSetupFormPath = new System.Windows.Forms.TextBox();
			this.chkGenMetaFile = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tbControllerPath = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbModelsFolder = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbManagerDirectory = new System.Windows.Forms.TextBox();
			this.tbServiceDirectory = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.cmbDatabase = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.lstTables = new System.Windows.Forms.ListBox();
			this.errValidation = new System.Windows.Forms.ErrorProvider(this.components);
			this.tbCtrlMngSvcName = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errValidation)).BeginInit();
			this.SuspendLayout();
			// 
			// btnGenerate
			// 
			this.btnGenerate.Location = new System.Drawing.Point(892, 589);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(82, 23);
			this.btnGenerate.TabIndex = 0;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// tbConnString
			// 
			this.tbConnString.Location = new System.Drawing.Point(95, 23);
			this.tbConnString.Name = "tbConnString";
			this.tbConnString.Size = new System.Drawing.Size(289, 20);
			this.tbConnString.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Conn Str:";
			// 
			// tbMetaDataFolder
			// 
			this.tbMetaDataFolder.Location = new System.Drawing.Point(62, 42);
			this.tbMetaDataFolder.Name = "tbMetaDataFolder";
			this.tbMetaDataFolder.Size = new System.Drawing.Size(280, 20);
			this.tbMetaDataFolder.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 46);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(50, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Meta Dir:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ckbPartialClass);
			this.groupBox1.Controls.Add(this.lblPartialClass);
			this.groupBox1.Controls.Add(this.txtPartialClassPath);
			this.groupBox1.Controls.Add(this.lblFullAppPath);
			this.groupBox1.Controls.Add(this.txtFullAppPath);
			this.groupBox1.Controls.Add(this.chkGenSrv);
			this.groupBox1.Controls.Add(this.chkGenMng);
			this.groupBox1.Controls.Add(this.chkGenMVCCntrl);
			this.groupBox1.Controls.Add(this.tbEditTemplatePath);
			this.groupBox1.Controls.Add(this.chkSetupForm);
			this.groupBox1.Controls.Add(this.chkGenEditTemplates);
			this.groupBox1.Controls.Add(this.tbSetupFormPath);
			this.groupBox1.Controls.Add(this.chkGenMetaFile);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.tbControllerPath);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.tbMetaDataFolder);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.tbModelsFolder);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.tbManagerDirectory);
			this.groupBox1.Controls.Add(this.tbServiceDirectory);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(12, 11);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(977, 147);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File Paths";
			// 
			// ckbPartialClass
			// 
			this.ckbPartialClass.AutoSize = true;
			this.ckbPartialClass.Location = new System.Drawing.Point(353, 126);
			this.ckbPartialClass.Name = "ckbPartialClass";
			this.ckbPartialClass.Size = new System.Drawing.Size(80, 17);
			this.ckbPartialClass.TabIndex = 37;
			this.ckbPartialClass.Text = "PartialClass";
			this.ckbPartialClass.UseVisualStyleBackColor = true;
			// 
			// lblPartialClass
			// 
			this.lblPartialClass.AutoSize = true;
			this.lblPartialClass.Location = new System.Drawing.Point(3, 124);
			this.lblPartialClass.Name = "lblPartialClass";
			this.lblPartialClass.Size = new System.Drawing.Size(80, 13);
			this.lblPartialClass.TabIndex = 36;
			this.lblPartialClass.Text = "PartialClass Dir:";
			// 
			// txtPartialClassPath
			// 
			this.txtPartialClassPath.CausesValidation = false;
			this.txtPartialClassPath.Location = new System.Drawing.Point(89, 124);
			this.txtPartialClassPath.Name = "txtPartialClassPath";
			this.txtPartialClassPath.Size = new System.Drawing.Size(253, 20);
			this.txtPartialClassPath.TabIndex = 35;
			// 
			// lblFullAppPath
			// 
			this.lblFullAppPath.AutoSize = true;
			this.lblFullAppPath.Location = new System.Drawing.Point(7, 19);
			this.lblFullAppPath.Name = "lblFullAppPath";
			this.lblFullAppPath.Size = new System.Drawing.Size(140, 13);
			this.lblFullAppPath.TabIndex = 34;
			this.lblFullAppPath.Text = "Full Path & Application Name:";
			// 
			// txtFullAppPath
			// 
			this.txtFullAppPath.Location = new System.Drawing.Point(149, 16);
			this.txtFullAppPath.Name = "txtFullAppPath";
			this.txtFullAppPath.Size = new System.Drawing.Size(312, 20);
			this.txtFullAppPath.TabIndex = 33;
			this.txtFullAppPath.Text = "CyroTechPortal";
			this.txtFullAppPath.TextChanged += new System.EventHandler(this.txtFullAppPath_TextChanged);
			// 
			// chkGenSrv
			// 
			this.chkGenSrv.AutoSize = true;
			this.chkGenSrv.Location = new System.Drawing.Point(352, 73);
			this.chkGenSrv.Name = "chkGenSrv";
			this.chkGenSrv.Size = new System.Drawing.Size(107, 17);
			this.chkGenSrv.TabIndex = 20;
			this.chkGenSrv.Text = "GenAPIController";
			this.chkGenSrv.UseVisualStyleBackColor = true;
			// 
			// chkGenMng
			// 
			this.chkGenMng.AutoSize = true;
			this.chkGenMng.Location = new System.Drawing.Point(353, 99);
			this.chkGenMng.Name = "chkGenMng";
			this.chkGenMng.Size = new System.Drawing.Size(70, 17);
			this.chkGenMng.TabIndex = 19;
			this.chkGenMng.Text = "GenMngr";
			this.chkGenMng.UseVisualStyleBackColor = true;
			// 
			// chkGenMVCCntrl
			// 
			this.chkGenMVCCntrl.AutoSize = true;
			this.chkGenMVCCntrl.Location = new System.Drawing.Point(837, 47);
			this.chkGenMVCCntrl.Name = "chkGenMVCCntrl";
			this.chkGenMVCCntrl.Size = new System.Drawing.Size(90, 17);
			this.chkGenMVCCntrl.TabIndex = 32;
			this.chkGenMVCCntrl.Text = "GenMVCCtrlr ";
			this.chkGenMVCCntrl.UseVisualStyleBackColor = true;
			// 
			// tbEditTemplatePath
			// 
			this.tbEditTemplatePath.CausesValidation = false;
			this.tbEditTemplatePath.Location = new System.Drawing.Point(545, 97);
			this.tbEditTemplatePath.Name = "tbEditTemplatePath";
			this.tbEditTemplatePath.Size = new System.Drawing.Size(280, 20);
			this.tbEditTemplatePath.TabIndex = 28;
			// 
			// chkSetupForm
			// 
			this.chkSetupForm.AutoSize = true;
			this.chkSetupForm.Location = new System.Drawing.Point(837, 71);
			this.chkSetupForm.Name = "chkSetupForm";
			this.chkSetupForm.Size = new System.Drawing.Size(111, 17);
			this.chkSetupForm.TabIndex = 31;
			this.chkSetupForm.Text = "Gen Setup Forms ";
			this.chkSetupForm.UseVisualStyleBackColor = true;
			// 
			// chkGenEditTemplates
			// 
			this.chkGenEditTemplates.AutoSize = true;
			this.chkGenEditTemplates.Location = new System.Drawing.Point(837, 99);
			this.chkGenEditTemplates.Name = "chkGenEditTemplates";
			this.chkGenEditTemplates.Size = new System.Drawing.Size(122, 17);
			this.chkGenEditTemplates.TabIndex = 30;
			this.chkGenEditTemplates.Text = "Gen Edit Templates ";
			this.chkGenEditTemplates.UseVisualStyleBackColor = true;
			// 
			// tbSetupFormPath
			// 
			this.tbSetupFormPath.CausesValidation = false;
			this.tbSetupFormPath.Location = new System.Drawing.Point(545, 72);
			this.tbSetupFormPath.Name = "tbSetupFormPath";
			this.tbSetupFormPath.Size = new System.Drawing.Size(280, 20);
			this.tbSetupFormPath.TabIndex = 29;
			// 
			// chkGenMetaFile
			// 
			this.chkGenMetaFile.AutoSize = true;
			this.chkGenMetaFile.Location = new System.Drawing.Point(352, 44);
			this.chkGenMetaFile.Name = "chkGenMetaFile";
			this.chkGenMetaFile.Size = new System.Drawing.Size(95, 17);
			this.chkGenMetaFile.TabIndex = 18;
			this.chkGenMetaFile.Text = "GenMetaClass";
			this.chkGenMetaFile.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(478, 101);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(62, 13);
			this.label7.TabIndex = 26;
			this.label7.Text = "EditTplt Dir:";
			// 
			// tbControllerPath
			// 
			this.tbControllerPath.CausesValidation = false;
			this.tbControllerPath.Location = new System.Drawing.Point(545, 46);
			this.tbControllerPath.Name = "tbControllerPath";
			this.tbControllerPath.Size = new System.Drawing.Size(280, 20);
			this.tbControllerPath.TabIndex = 31;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(475, 76);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 13);
			this.label8.TabIndex = 27;
			this.label8.Text = "StpForm Dir:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(491, 51);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(47, 13);
			this.label9.TabIndex = 30;
			this.label9.Text = "Cntrl Dir:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(494, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 13);
			this.label2.TabIndex = 20;
			this.label2.Text = "Mdl Dir:";
			// 
			// tbModelsFolder
			// 
			this.tbModelsFolder.Location = new System.Drawing.Point(545, 19);
			this.tbModelsFolder.Name = "tbModelsFolder";
			this.tbModelsFolder.Size = new System.Drawing.Size(280, 20);
			this.tbModelsFolder.TabIndex = 21;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(61, 13);
			this.label6.TabIndex = 25;
			this.label6.Text = "API Ctrl Dir:";
			// 
			// tbManagerDirectory
			// 
			this.tbManagerDirectory.CausesValidation = false;
			this.tbManagerDirectory.Location = new System.Drawing.Point(74, 98);
			this.tbManagerDirectory.Name = "tbManagerDirectory";
			this.tbManagerDirectory.Size = new System.Drawing.Size(268, 20);
			this.tbManagerDirectory.TabIndex = 23;
			// 
			// tbServiceDirectory
			// 
			this.tbServiceDirectory.CausesValidation = false;
			this.tbServiceDirectory.Location = new System.Drawing.Point(73, 72);
			this.tbServiceDirectory.Name = "tbServiceDirectory";
			this.tbServiceDirectory.Size = new System.Drawing.Size(269, 20);
			this.tbServiceDirectory.TabIndex = 24;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 101);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 13);
			this.label3.TabIndex = 22;
			this.label3.Text = "API Mngs Dir:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.button1);
			this.groupBox2.Controls.Add(this.cmbDatabase);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.tbConnString);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Location = new System.Drawing.Point(12, 161);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(771, 51);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Databases";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(0, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			// 
			// cmbDatabase
			// 
			this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDatabase.FormattingEnabled = true;
			this.cmbDatabase.Location = new System.Drawing.Point(521, 21);
			this.cmbDatabase.Name = "cmbDatabase";
			this.cmbDatabase.Size = new System.Drawing.Size(244, 21);
			this.cmbDatabase.TabIndex = 4;
			this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.cmbDatabase_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(459, 26);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 13);
			this.label5.TabIndex = 3;
			this.label5.Text = "Database:";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.lstTables);
			this.groupBox3.Location = new System.Drawing.Point(12, 242);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(977, 340);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Select tables";
			// 
			// lstTables
			// 
			this.lstTables.ColumnWidth = 220;
			this.lstTables.FormattingEnabled = true;
			this.lstTables.Location = new System.Drawing.Point(6, -33);
			this.lstTables.MultiColumn = true;
			this.lstTables.Name = "lstTables";
			this.lstTables.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lstTables.Size = new System.Drawing.Size(965, 407);
			this.lstTables.TabIndex = 0;
			this.lstTables.SelectedIndexChanged += new System.EventHandler(this.lstTables_SelectedIndexChanged);
			// 
			// errValidation
			// 
			this.errValidation.ContainerControl = this;
			// 
			// tbCtrlMngSvcName
			// 
			this.tbCtrlMngSvcName.Location = new System.Drawing.Point(789, 180);
			this.tbCtrlMngSvcName.Name = "tbCtrlMngSvcName";
			this.tbCtrlMngSvcName.Size = new System.Drawing.Size(179, 20);
			this.tbCtrlMngSvcName.TabIndex = 29;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(789, 161);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(135, 13);
			this.label10.TabIndex = 30;
			this.label10.Text = "Cntrl/Mng/Svc PrefixName";
			// 
			// frmGenerator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1001, 620);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.tbCtrlMngSvcName);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnGenerate);
			this.Name = "frmGenerator";
			this.Text = "frmGenerator";
			this.Load += new System.EventHandler(this.frmGenerator_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errValidation)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox tbConnString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMetaDataFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lstTables;
        private System.Windows.Forms.ErrorProvider errValidation;
        private System.Windows.Forms.CheckBox chkGenMng;
        private System.Windows.Forms.CheckBox chkGenMetaFile;
        private System.Windows.Forms.TextBox tbModelsFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbManagerDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbServiceDirectory;
        private System.Windows.Forms.CheckBox chkSetupForm;
        private System.Windows.Forms.CheckBox chkGenEditTemplates;
        private System.Windows.Forms.TextBox tbSetupFormPath;
        private System.Windows.Forms.TextBox tbEditTemplatePath;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbControllerPath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkGenMVCCntrl;
        private System.Windows.Forms.CheckBox chkGenSrv;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbCtrlMngSvcName;
        private System.Windows.Forms.Label lblFullAppPath;
        private System.Windows.Forms.TextBox txtFullAppPath;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox ckbPartialClass;
		private System.Windows.Forms.Label lblPartialClass;
		private System.Windows.Forms.TextBox txtPartialClassPath;
	}
}