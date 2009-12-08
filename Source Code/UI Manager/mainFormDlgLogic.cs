using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using CalculateLib; 

namespace ExpenseManager
{
   	public partial class mainFormDlg : System.Windows.Forms.Form
    {
        #region Member_Variables
        CalculateLogicClass myCalculator = new CalculateLogicClass();
        private IDataHelper m_dbObj;
        private ExpenseManager.ExpenseLayout[] userControlArr = new ExpenseLayout[Settings.Default.maxUsers];

        private bool m_bResetSharing = false;
        
        private double dTotalAmount = 0;
        private int iCostSharinUsers = 0;
        private int m_iUserIdPayee = -1;

        private long m_lActiveUsers = 0;
        private long m_lTotalUsers = 0;

        private static Point clrBtnOrgLocation;
        private static Point nxtBtnOrgLocation;
        #endregion

        #region Private_Methods
        private void txtUser_Click(object sender, System.EventArgs e)
        {
            txtUser.Text = Constants.NULL_STRING;
        }

        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabControl.SelectedTab == tabPayment)
            {
                this.btnClrPrev.Location = clrBtnOrgLocation;
                this.btnClrPrev.Text = "CLEAR";
                this.btnClrPrev.Show();
                this.btnNextDone.Enabled = true;
                this.btnNextDone.Text = "NEXT";
                this.btnNextDone.Show();
            }
            else if (tabControl.SelectedTab == tabSharing)
            {
                this.btnClrPrev.Location = clrBtnOrgLocation;
                this.btnClrPrev.Text = "BACK";
                this.btnClrPrev.Show();
                this.btnNextDone.Enabled = false;
                this.btnNextDone.Text = "DONE";
                this.btnNextDone.Show();
            }
            else if (tabControl.SelectedTab == tabUsers)
            {
                txtUser.Text = Constants.MU_INITIAL_MSG;
                this.btnClrPrev.Hide();
                this.btnNextDone.Hide();
            }
            else if (tabControl.SelectedTab == tabDetails)
            {
                this.btnNextDone.Hide();
                this.btnClrPrev.Hide();
                this.btnClrPrev.Location = nxtBtnOrgLocation;
                this.pnlDetails.Show();
                this.populateGridSumary();
            }
            else
            {
                MessageBox.Show("ERROR: No Source code present for this event !");
            }
        }

        private void populateGridSumary()
        {
            DataSet ds = m_dbObj.GetTransactionsSumary();
            this.gridSumary.DataSource = ds.Tables[0].DefaultView;
            m_lTotalUsers = ds.Tables[0].Rows.Count;
        }

        private void mainFormDlg_Load(object sender, System.EventArgs e)
        {
            clrBtnOrgLocation = this.btnClrPrev.Location;
            nxtBtnOrgLocation = this.btnNextDone.Location;

            try
            {
                DataSet ds = m_dbObj.GetActiveUsers();
                lstMembers.DataSource = ds.Tables[0];
                lstMembers.DisplayMember = "UserName";
                lstMembers.ValueMember = "ID";
                lblInstructions.Text = "1. Enter the amount spent by Payee." + Environment.NewLine
                                     + "2. Enter a valid description." + Environment.NewLine
                                     + "3. Select the name of Payee." + Environment.NewLine
                                     + "4.  Click 'Next'";


                lstUsers.DataSource = ds.Tables[0];
                lstUsers.DisplayMember = "UserName";
                lstUsers.ValueMember = "ID";


                m_lActiveUsers = ds.Tables[0].Rows.Count;
                //In case of no active users, Remove Users button shall be disabled.
                this.btnRemove.Enabled = m_lActiveUsers > Constants.ZERO ? true : false;
                DataColumn colUserId = ds.Tables[0].Columns["ID"];
                DataColumn colUserName = ds.Tables[0].Columns["UserName"];
                Point ctrlLocation = new Point(Constants.X_CORDINATE, Constants.Y_CORDINATE);


                this.tabSharing.SuspendLayout();

                for (int iControlNum = 0; iControlNum < m_lActiveUsers; ++iControlNum)
                {
                    userControlArr[iControlNum] = new ExpenseLayout();
                    // Set the user ID for this userControl
                    userControlArr[iControlNum].UserId = Int32.Parse(ds.Tables[0].Rows[iControlNum][colUserId].ToString());
                    // Set the userName here
                    userControlArr[iControlNum].strUserName = ds.Tables[0].Rows[iControlNum][colUserName].ToString();
                    // Set the checkbox events
                    userControlArr[iControlNum].ChkBoxChecked += new EventHandler(mainFormDlg_ChkBoxChecked);
                    userControlArr[iControlNum].ChkBoxUnChecked += new EventHandler(mainFormDlg_ChkBoxUnChecked);
                    //Set the location of this TserControl
                    userControlArr[iControlNum].Location = ctrlLocation;
                    ctrlLocation.Offset(0, Constants.Y_INCREMENT);
                    //Add this control to group box
                    this.grpBoxSharing.Controls.Add(this.userControlArr[iControlNum]);
                }
                this.tabSharing.ResumeLayout();
            }
            catch (DataException ex)
            {
                MessageBox.Show(null, ex.Message.ToString(), "DataBase Error Caught !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(null, ex.Message.ToString(), "Error Caught !");
            }
        }


        private void button1_Click(object sender, System.EventArgs e)
        {
            txtPaidBy.Text = lstMembers.Text.ToString();
            m_iUserIdPayee = Int16.Parse(lstMembers.SelectedValue.ToString());
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            if (this.tabControl.SelectedTab == this.tabPayment)
            {
                if (txtAmount.Text == Constants.NULL_STRING)
                {
                    errorProvider.SetError(this.txtAmount, "Please enter some amount !");
                    errorProvider.SetError(this.txtDescription, Constants.NULL_STRING);
                    errorProvider.SetError(this.txtPaidBy, Constants.NULL_STRING);
                }
                else if (txtDescription.Text == Constants.NULL_STRING)
                {
                    errorProvider.SetError(this.txtAmount, Constants.NULL_STRING);
                    errorProvider.SetError(this.txtDescription, "Please enter some descrition !");
                    errorProvider.SetError(this.txtPaidBy, Constants.NULL_STRING);
                }
                else if (m_iUserIdPayee == -1)
                {
                    errorProvider.SetError(this.txtAmount, Constants.NULL_STRING);
                    errorProvider.SetError(this.txtDescription, Constants.NULL_STRING);
                    errorProvider.SetError(this.txtPaidBy, "Please select the user who paid the money!");
                }
                else
                {
                    errorProvider.SetError(this.txtAmount, Constants.NULL_STRING);
                    errorProvider.SetError(this.txtDescription, Constants.NULL_STRING);
                    errorProvider.SetError(this.txtPaidBy, Constants.NULL_STRING);

                    try
                    {
                        ResetSharing();
                        dTotalAmount = Double.Parse(txtAmount.Text);
                        tabControl.SelectedTab = tabSharing;

                        for (int iControlNum = 0; iControlNum < m_lActiveUsers; ++iControlNum)
                        {
                            if (userControlArr[iControlNum].UserId == m_iUserIdPayee)
                            {
                                userControlArr[iControlNum].strPlusMinus = Constants.SYMBOL_POSITIVE;

                                // Set the check box properties here.
                                userControlArr[iControlNum].chkShareAmt.Checked = true;
                            }
                            else
                            {
                                userControlArr[iControlNum].strPlusMinus = Constants.SYMBOL_NEGATIVE;
                            }
                        }
                    }
                    catch (OverflowException)
                    {
                        errorProvider.SetError(this.txtAmount, "Amount entered seems to be an exceptionaly LARGE Value !");
                    }
                    catch (FormatException)
                    {
                        errorProvider.SetError(this.txtAmount, "Amount entered seems to be in INVALID Format ! !");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString());
                    }
                }
            }
            else if (this.tabControl.SelectedTab == this.tabSharing)
            {
                this.addRecords();
            }
            else
            {
                MessageBox.Show("ERROR: No Code written for this Block");
            }
        }

        private void addRecords()
        {
            Hashtable hshUserCostMap = new Hashtable(6);
            for (int iControlNum = 0; iControlNum < m_lActiveUsers; ++iControlNum)
            {
                double myShare = 0;
                myShare = userControlArr[iControlNum].CostPerHead;
                if (myShare == 0)
                {
                    continue;
                }
                if (userControlArr[iControlNum].strPlusMinus == Constants.SYMBOL_NEGATIVE)
                {
                    myShare *= -1;
                }
                hshUserCostMap.Add(userControlArr[iControlNum].UserId, myShare);
            }
            if (hshUserCostMap.Values.Count > 0)
            {
                bool bSuccess = false;
                bSuccess = m_dbObj.addRecordsToDB(m_iUserIdPayee, hshUserCostMap, txtDescription.Text);
                if (bSuccess)
                {
                    MessageBox.Show(this, "Amount spent on other users has been Credited to Payee's Account.", this.Text);
                    ResetSharing();
                    this.btnNextDone.Enabled = false;
                }
            }
        }

        private void btnPrev_Click(object sender, System.EventArgs e)
        {
            if (this.tabControl.SelectedTab == this.tabPayment)
            {
                txtAmount.Text = "";
                txtDescription.Text = "";
                txtPaidBy.Text = "Select a Name";
                this.m_iUserIdPayee = Constants.NO_SELECTION;
                this.btnNextDone.Text = "NEXT";
            }
            else if (this.tabControl.SelectedTab == this.tabSharing)
            {
                ResetSharing();
                tabControl.SelectedTab = tabPayment;
            }
            else if (this.tabControl.SelectedTab == this.tabDetails)
            {
                this.btnClrPrev.Hide();
                this.pnlDetails.Show();
            }
        }

        private void mainFormDlg_ChkBoxChecked(object sender, EventArgs e)
        {
            bool bEnableCheckedItem = false;

            ++iCostSharinUsers;
            bEnableCheckedItem = (1 == iCostSharinUsers) ? false : true;
            updateSharing(bEnableCheckedItem);
        }



        private void mainFormDlg_ChkBoxUnChecked(object sender, EventArgs e)
        {
            if (!m_bResetSharing)
            {
                bool bEnableCheckedItem = false;

                --iCostSharinUsers;
                bEnableCheckedItem = (1 == iCostSharinUsers) ? false : true;
                updateSharing(bEnableCheckedItem);
            }
        }


        private void updateSharing(bool bEnableCheckedItem)
        {
            double dCostPerHead = myCalculator.CostPerHead(dTotalAmount, iCostSharinUsers);
            for (int iControlNum = 0; iControlNum < m_lActiveUsers; ++iControlNum)
            {
                if (userControlArr[iControlNum].UserId == m_iUserIdPayee)
                {
                    userControlArr[iControlNum].strPlusMinus = Constants.SYMBOL_POSITIVE;
                    if (userControlArr[iControlNum].chkShareAmt.Checked)
                    {
                        //Payee also spent the equal share on himself too 
                        userControlArr[iControlNum].CostPerHead = (dTotalAmount) - dCostPerHead;
                        userControlArr[iControlNum].chkShareAmt.Enabled = bEnableCheckedItem;
                    }
                    else
                    {
                        //Payee spent only on others, Will be reimbursed full amount
                        userControlArr[iControlNum].CostPerHead = (dTotalAmount);
                        userControlArr[iControlNum].chkShareAmt.Enabled = true;
                    }
                    if (userControlArr[iControlNum].CostPerHead == 0)
                    {
                        this.btnNextDone.Enabled = false;
                    }
                    else
                    {
                        this.btnNextDone.Enabled = true;
                    }
                }
                else
                {
                    if (userControlArr[iControlNum].chkShareAmt.Checked)
                    {
                        userControlArr[iControlNum].CostPerHead = dCostPerHead;
                        userControlArr[iControlNum].strPlusMinus = "-";
                        userControlArr[iControlNum].chkShareAmt.Enabled = bEnableCheckedItem;
                    }
                    else
                    {
                        userControlArr[iControlNum].CostPerHead = 0;
                        userControlArr[iControlNum].strPlusMinus = Constants.SYMBOL_POSITIVE;
                        userControlArr[iControlNum].chkShareAmt.Enabled = true;
                    }
                }
            }

        }


        private void ShowTransactionsByUserId(bool bShowPositiveTransactions)
        {
            int iUserIdForDetails = Constants.NO_SELECTION;
            errorProvider.SetError(this.gridSumary, Constants.NULL_STRING);
            for (int iRow = 0; iRow < m_lTotalUsers; ++iRow)
            {
                if (this.gridSumary.IsSelected(iRow))
                {
                    this.pnlDetails.Hide();
                    this.btnClrPrev.Visible = true;
                    this.btnClrPrev.Text = "BACK";
                    DataGridCell dgCell;
                    if (Settings.Default.MODE == Constants.MODE_XML)
                    {
                        dgCell = new DataGridCell(iRow, 5);
                    }
                    else
                    {
                        dgCell = new DataGridCell(iRow, 7);
                    }
                    iUserIdForDetails = Int16.Parse(gridSumary[dgCell].ToString());

                    DataSet ds = m_dbObj.GetTransactionsByUserId(iUserIdForDetails, bShowPositiveTransactions);
                    this.gridDetails.DataSource = ds.Tables[0].DefaultView;
                }
            }
            if (iUserIdForDetails == Constants.NO_SELECTION)
            {
                errorProvider.SetError(this.gridSumary, "Please select a ROW to view details !");
            }
        }


        private void ResetSharing()
        {
            this.m_bResetSharing = true;
            this.iCostSharinUsers = 0;

            for (int iControlNum = 0; iControlNum < m_lActiveUsers; ++iControlNum)
            {
                userControlArr[iControlNum].CostPerHead = 0;
                userControlArr[iControlNum].strPlusMinus = Constants.SYMBOL_POSITIVE;
                userControlArr[iControlNum].chkShareAmt.Checked = false;
                userControlArr[iControlNum].chkShareAmt.Enabled = false;
            }
            this.m_bResetSharing = false;
        }

        private void lnkPositive_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            gridDetails.PreferredColumnWidth = (Constants.GRID_DETAILS_WIDTH / 3) - Constants.GRID_SCROLL_FACTOR;
            ShowTransactionsByUserId(true);
        }

        private void lnkNegative_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            gridDetails.PreferredColumnWidth = (Constants.GRID_DETAILS_WIDTH / 4) - Constants.GRID_SCROLL_FACTOR;
            ShowTransactionsByUserId(false);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                Int16 iUserId = Int16.Parse(lstUsers.SelectedValue.ToString());
                if (m_dbObj.CanRemoveUser(iUserId))
                {
                    DialogResult dgResult = MessageBox.Show(
                        this,
                        "Application has to be restarted once you confirm to Add or Remove Users." +
                        Environment.NewLine +
                        "Do you want to continue ?",
                        "WARNING",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (dgResult.Equals(DialogResult.Yes))
                    {
                        m_dbObj.RemoveUser(iUserId);
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("This user can't be removed due unsettled Account balance !" +
                        Environment.NewLine +
                        "Please see account details section and verify that TOTAL BALANCE should be ZERO to continue.",
                        "Can't Proceed !",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("General System Error !", "Error in Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == Constants.MU_INITIAL_MSG ||
                txtUser.Text == Constants.MU_INCORRECT_LENGTH)
            {
                return;
            }
            if (txtUser.Text.Length >= Constants.MU_MIN_LENGTH &&
                txtUser.Text.Length <= Constants.MU_MAX_LENGTH)
            {
                if (m_lActiveUsers < Settings.Default.maxUsers)
                {
                    DialogResult dgResult = MessageBox.Show(
                                            this,
                                            "Application has to be restarted once you confirm to Add or Remove Users." +
                                            Environment.NewLine +
                                            "Do you want to continue ?",
                                            "WARNING",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);

                    if (dgResult.Equals(DialogResult.Yes))
                    {
                        string strMessage = string.Empty;
                        bool bSuccess = false;
                        bSuccess = m_dbObj.AddUser(txtUser.Text, ref strMessage);
                        if (bSuccess)
                        {
                            Application.Exit();
                        }
                        else
                        {
                            MessageBox.Show(this, strMessage, "Add User", MessageBoxButtons.OK, MessageBoxIcon.Error);   
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Can't add more users !" +
                        Environment.NewLine +
                        "Plz update the configuration if you wish to have more than " +
                        Settings.Default.maxUsers.ToString() +
                        " active users !", "All users Acive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                txtUser.Text = Constants.MU_INCORRECT_LENGTH;
            }
        }
        #endregion
    }
}
