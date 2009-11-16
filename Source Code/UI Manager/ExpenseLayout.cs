using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ExpenseManager
{
	/// <summary>
	/// Summary description for ExpenseLayout.
	/// </summary>
	public class ExpenseLayout : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblPlusMinus;
		private System.Windows.Forms.TextBox txtPerHeadCost;
		public System.Windows.Forms.CheckBox chkShareAmt;
		private System.Windows.Forms.Label lblUserName;

		private long lUserId = 0 ;
		// An event that clients can use to be notified whenever the 
		// elements of the list change: 
		public event EventHandler ChkBoxChecked; 
		public event EventHandler ChkBoxUnChecked;

		public ExpenseLayout()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblPlusMinus = new System.Windows.Forms.Label();
			this.lblUserName = new System.Windows.Forms.Label();
			this.txtPerHeadCost = new System.Windows.Forms.TextBox();
			this.chkShareAmt = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// lblPlusMinus
			// 
			this.lblPlusMinus.Location = new System.Drawing.Point(248, 15);
			this.lblPlusMinus.Name = "lblPlusMinus";
			this.lblPlusMinus.Size = new System.Drawing.Size(32, 16);
			this.lblPlusMinus.TabIndex = 4;
            this.lblPlusMinus.Text = "[ " + Constants.SYMBOL_POSITIVE + " ]";
			// 
			// lblUserName
			// 
			this.lblUserName.Location = new System.Drawing.Point(16, 8);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(136, 23);
			this.lblUserName.TabIndex = 3;
			// 
			// txtPerHeadCost
			// 
			this.txtPerHeadCost.Location = new System.Drawing.Point(288, 11);
			this.txtPerHeadCost.Name = "txtPerHeadCost";
			this.txtPerHeadCost.ReadOnly = true;
			this.txtPerHeadCost.TabIndex = 2;
			this.txtPerHeadCost.Text = "Rs 00.00";
			// 
			// chkShareAmt
			// 
			this.chkShareAmt.Enabled = false;
			this.chkShareAmt.Location = new System.Drawing.Point(168, 7);
			this.chkShareAmt.Name = "chkShareAmt";
			this.chkShareAmt.Size = new System.Drawing.Size(32, 24);
			this.chkShareAmt.TabIndex = 1;
			this.chkShareAmt.CheckedChanged += new System.EventHandler(this.chkShareAmt_CheckedChanged);
			// 
			// ExpenseLayout
			// 
			this.Controls.Add(this.chkShareAmt);
			this.Controls.Add(this.lblUserName);
			this.Controls.Add(this.txtPerHeadCost);
			this.Controls.Add(this.lblPlusMinus);
			this.Name = "ExpenseLayout";
			this.Size = new System.Drawing.Size(408, 40);
			this.ResumeLayout(false);

		}
		#endregion

		private void chkShareAmt_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.chkShareAmt.Checked == true)
			{
				ChkBoxChecked( this , e );
			}
			else
			{
				ChkBoxUnChecked( this , e );
			}			
		}


        public long UserId
        {
            get
            {
                return lUserId;
            }
            set
            {
                lUserId = value ;
            }
        }

		public string strUserName 
		{
			get 
			{ 
				return lblUserName.Text   ; 
			}
			set 
			{ 
				lblUserName.Text = value;
			}
		}

		public string strPlusMinus 
		{
			get 
			{ 
				return lblPlusMinus.Text.Substring(2,1) ; 
			}
			set 
			{ 
				lblPlusMinus.Text = "[ " + value + " ]";
			}
		}

		public double CostPerHead 
		{
			
			get 
			{	
				string strCostPerHead;			
				strCostPerHead = txtPerHeadCost.Text.Substring(3);  
				return Double.Parse( strCostPerHead, System.Globalization.NumberStyles.AllowDecimalPoint );
 			}
			set 
			{ 
				string strCostPerHead = value.ToString() ;
				txtPerHeadCost.Text = "Rs " + strCostPerHead ; 
			}
		}


	}
}
