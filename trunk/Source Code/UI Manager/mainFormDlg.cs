using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using CalculateLib; 

namespace ExpenseManager
{ 
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public partial class mainFormDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabUsers;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListBox lstUsers;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.TextBox txtUser;
        private IContainer components;
		private System.Windows.Forms.Label lblAmount;
		private System.Windows.Forms.TextBox txtAmount;
		private System.Windows.Forms.Label lblDescrition;
		
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnPaidBy;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPaidBy;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox lstMembers;

		
		private System.Windows.Forms.ErrorProvider errorProvider;
		
		private System.Windows.Forms.TabPage tabPayment;
		private System.Windows.Forms.TabPage tabSharing;
		private System.Windows.Forms.Button btnClrPrev;
		private System.Windows.Forms.Button btnNextDone;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Panel pnlDetails;
		private System.Windows.Forms.TabPage tabDetails;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel lnkPositive;
		private System.Windows.Forms.LinkLabel lnkNegative;
		private System.Windows.Forms.DataGrid gridSumary;
        private System.Windows.Forms.DataGrid gridDetails;
        private Label lblInstructions;
		private System.Windows.Forms.GroupBox grpBoxSharing;
		

		public mainFormDlg()
		{
            if (Constants.MODE_XML == Settings.Default.MODE)
            {
                m_dbObj = XMLHelper.Get();
            }
            else
            {
                // Use static method "Get" to create object of Single Ton Class DBHelper
                m_dbObj = DBHelper.Get();
            }

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainFormDlg));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPayment = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.txtPaidBy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstMembers = new System.Windows.Forms.ListBox();
            this.btnPaidBy = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescrition = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.tabSharing = new System.Windows.Forms.TabPage();
            this.grpBoxSharing = new System.Windows.Forms.GroupBox();
            this.tabDetails = new System.Windows.Forms.TabPage();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.lnkNegative = new System.Windows.Forms.LinkLabel();
            this.lnkPositive = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.gridSumary = new System.Windows.Forms.DataGrid();
            this.gridDetails = new System.Windows.Forms.DataGrid();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.lblUsers = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.btnClrPrev = new System.Windows.Forms.Button();
            this.btnNextDone = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl.SuspendLayout();
            this.tabPayment.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabSharing.SuspendLayout();
            this.tabDetails.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSumary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetails)).BeginInit();
            this.tabUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPayment);
            this.tabControl.Controls.Add(this.tabSharing);
            this.tabControl.Controls.Add(this.tabDetails);
            this.tabControl.Controls.Add(this.tabUsers);
            this.tabControl.Location = new System.Drawing.Point(20, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(664, 288);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPayment
            // 
            this.tabPayment.Controls.Add(this.groupBox1);
            this.tabPayment.Controls.Add(this.txtPaidBy);
            this.tabPayment.Controls.Add(this.label1);
            this.tabPayment.Controls.Add(this.lstMembers);
            this.tabPayment.Controls.Add(this.btnPaidBy);
            this.tabPayment.Controls.Add(this.label2);
            this.tabPayment.Controls.Add(this.txtDescription);
            this.tabPayment.Controls.Add(this.lblDescrition);
            this.tabPayment.Controls.Add(this.txtAmount);
            this.tabPayment.Controls.Add(this.lblAmount);
            this.tabPayment.Location = new System.Drawing.Point(4, 22);
            this.tabPayment.Name = "tabPayment";
            this.tabPayment.Size = new System.Drawing.Size(656, 262);
            this.tabPayment.TabIndex = 0;
            this.tabPayment.Text = "Payment Details";
            this.tabPayment.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblInstructions);
            this.groupBox1.Location = new System.Drawing.Point(248, 158);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 88);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Instructions";
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(17, 23);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(121, 13);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = "To Be Filled at RunTime";
            // 
            // txtPaidBy
            // 
            this.txtPaidBy.Location = new System.Drawing.Point(312, 112);
            this.txtPaidBy.Name = "txtPaidBy";
            this.txtPaidBy.ReadOnly = true;
            this.txtPaidBy.Size = new System.Drawing.Size(208, 20);
            this.txtPaidBy.TabIndex = 12;
            this.txtPaidBy.Text = "Select a Name";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(120, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Members:";
            // 
            // lstMembers
            // 
            this.lstMembers.Location = new System.Drawing.Point(120, 112);
            this.lstMembers.Name = "lstMembers";
            this.lstMembers.Size = new System.Drawing.Size(112, 134);
            this.lstMembers.TabIndex = 10;
            // 
            // btnPaidBy
            // 
            this.btnPaidBy.Location = new System.Drawing.Point(248, 112);
            this.btnPaidBy.Name = "btnPaidBy";
            this.btnPaidBy.Size = new System.Drawing.Size(40, 24);
            this.btnPaidBy.TabIndex = 9;
            this.btnPaidBy.Text = ">>";
            this.btnPaidBy.Click += new System.EventHandler(this.btnPaidBy_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(312, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Paid By:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(120, 56);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(400, 20);
            this.txtDescription.TabIndex = 3;
            // 
            // lblDescrition
            // 
            this.lblDescrition.Location = new System.Drawing.Point(24, 56);
            this.lblDescrition.Name = "lblDescrition";
            this.lblDescrition.Size = new System.Drawing.Size(80, 16);
            this.lblDescrition.TabIndex = 2;
            this.lblDescrition.Text = "Description:";
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(120, 24);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(72, 20);
            this.txtAmount.TabIndex = 1;
            // 
            // lblAmount
            // 
            this.lblAmount.Location = new System.Drawing.Point(24, 24);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(80, 16);
            this.lblAmount.TabIndex = 0;
            this.lblAmount.Text = "Amount Spent:  ";
            // 
            // tabSharing
            // 
            this.tabSharing.Controls.Add(this.grpBoxSharing);
            this.tabSharing.Location = new System.Drawing.Point(4, 22);
            this.tabSharing.Name = "tabSharing";
            this.tabSharing.Size = new System.Drawing.Size(656, 262);
            this.tabSharing.TabIndex = 3;
            this.tabSharing.Text = "Sharing Details";
            this.tabSharing.UseVisualStyleBackColor = true;
            // 
            // grpBoxSharing
            // 
            this.grpBoxSharing.Location = new System.Drawing.Point(8, 8);
            this.grpBoxSharing.Name = "grpBoxSharing";
            this.grpBoxSharing.Size = new System.Drawing.Size(640, 248);
            this.grpBoxSharing.TabIndex = 3;
            this.grpBoxSharing.TabStop = false;
            // 
            // tabDetails
            // 
            this.tabDetails.Controls.Add(this.pnlDetails);
            this.tabDetails.Controls.Add(this.gridDetails);
            this.tabDetails.Location = new System.Drawing.Point(4, 22);
            this.tabDetails.Name = "tabDetails";
            this.tabDetails.Size = new System.Drawing.Size(656, 262);
            this.tabDetails.TabIndex = 2;
            this.tabDetails.Text = "Your Details";
            this.tabDetails.UseVisualStyleBackColor = true;
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.lnkNegative);
            this.pnlDetails.Controls.Add(this.lnkPositive);
            this.pnlDetails.Controls.Add(this.label3);
            this.pnlDetails.Controls.Add(this.gridSumary);
            this.pnlDetails.Location = new System.Drawing.Point(16, 24);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(608, 208);
            this.pnlDetails.TabIndex = 7;
            // 
            // lnkNegative
            // 
            this.lnkNegative.Location = new System.Drawing.Point(288, 176);
            this.lnkNegative.Name = "lnkNegative";
            this.lnkNegative.Size = new System.Drawing.Size(64, 23);
            this.lnkNegative.TabIndex = 3;
            this.lnkNegative.TabStop = true;
            this.lnkNegative.Text = "NEGATIVE";
            this.lnkNegative.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNegative_LinkClicked);
            // 
            // lnkPositive
            // 
            this.lnkPositive.Location = new System.Drawing.Point(208, 176);
            this.lnkPositive.Name = "lnkPositive";
            this.lnkPositive.Size = new System.Drawing.Size(56, 23);
            this.lnkPositive.TabIndex = 2;
            this.lnkPositive.TabStop = true;
            this.lnkPositive.Text = "POSITIVE";
            this.lnkPositive.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPositive_LinkClicked);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(456, 24);
            this.label3.TabIndex = 1;
            this.label3.Text = "Select a user above to see details of                         and                " +
                "      Transactions.";
            // 
            // gridSumary
            // 
            this.gridSumary.DataMember = "";
            this.gridSumary.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridSumary.Location = new System.Drawing.Point(16, 16);
            this.gridSumary.Name = "gridSumary";
            this.gridSumary.PreferredColumnWidth = 111;
            this.gridSumary.ReadOnly = true;
            this.gridSumary.Size = new System.Drawing.Size(562, 152);
            this.gridSumary.TabIndex = 0;
            // 
            // gridDetails
            // 
            this.gridDetails.AlternatingBackColor = System.Drawing.Color.Thistle;
            this.gridDetails.DataMember = "";
            this.gridDetails.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridDetails.Location = new System.Drawing.Point(16, 24);
            this.gridDetails.Name = "gridDetails";
            this.gridDetails.ReadOnly = true;
            this.gridDetails.Size = new System.Drawing.Size(608, 208);
            this.gridDetails.TabIndex = 6;
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.lblUsers);
            this.tabUsers.Controls.Add(this.btnRemove);
            this.tabUsers.Controls.Add(this.lstUsers);
            this.tabUsers.Controls.Add(this.btnAdd);
            this.tabUsers.Controls.Add(this.txtUser);
            this.tabUsers.Location = new System.Drawing.Point(4, 22);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Size = new System.Drawing.Size(656, 262);
            this.tabUsers.TabIndex = 1;
            this.tabUsers.Text = "Manage Users";
            // 
            // lblUsers
            // 
            this.lblUsers.Location = new System.Drawing.Point(144, 80);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(288, 16);
            this.lblUsers.TabIndex = 4;
            this.lblUsers.Text = "Current Users:";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(448, 104);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(88, 24);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lstUsers
            // 
            this.lstUsers.Location = new System.Drawing.Point(144, 104);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(288, 95);
            this.lstUsers.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(448, 40);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 24);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(144, 40);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(288, 20);
            this.txtUser.TabIndex = 0;
            this.txtUser.Text = "Please Enter New User\'s Display Name Here ...";
            this.txtUser.Click += new System.EventHandler(this.txtUser_Click);
            // 
            // btnClrPrev
            // 
            this.btnClrPrev.Location = new System.Drawing.Point(511, 304);
            this.btnClrPrev.Name = "btnClrPrev";
            this.btnClrPrev.Size = new System.Drawing.Size(75, 23);
            this.btnClrPrev.TabIndex = 1;
            this.btnClrPrev.Text = "CLEAR";
            this.btnClrPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNextDone
            // 
            this.btnNextDone.Location = new System.Drawing.Point(607, 304);
            this.btnNextDone.Name = "btnNextDone";
            this.btnNextDone.Size = new System.Drawing.Size(75, 23);
            this.btnNextDone.TabIndex = 2;
            this.btnNextDone.Text = "NEXT";
            this.btnNextDone.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // mainFormDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(704, 337);
            this.Controls.Add(this.btnNextDone);
            this.Controls.Add(this.btnClrPrev);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "mainFormDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expense Manager";
            this.Load += new System.EventHandler(this.mainFormDlg_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPayment.ResumeLayout(false);
            this.tabPayment.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabSharing.ResumeLayout(false);
            this.tabDetails.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSumary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetails)).EndInit();
            this.tabUsers.ResumeLayout(false);
            this.tabUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new mainFormDlg());
		}	
	}
}

