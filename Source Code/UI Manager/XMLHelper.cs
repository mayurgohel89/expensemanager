using System;
using System.Data;
using System.Xml;
using System.Collections;

using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ExpenseManager
{

    public sealed class XMLHelper : IDataHelper 
    {
        #region Member Variables
        public static XMLHelper m_XMLHelper = null ;
        private string xmlWorkPath = string.Empty;
        #endregion

		#region constuctors
        private XMLHelper()
		{
            xmlWorkPath = Settings.Default.XMLFilesPath ;   
        }
		#endregion

		#region Public Methods
        public static XMLHelper Get()
        {
            if (m_XMLHelper == null )
            {
                m_XMLHelper = new XMLHelper(); 
            }
            return m_XMLHelper;
        }

        public bool AddUser(string strUserName, ref string strMessage)
        {
            XDocument xmlDB ;
            bool bResult = false;
            try
            {
                xmlDB = XDocument.Load(xmlWorkPath + "User.xml");
                var query = from xNode in xmlDB.Element("XMLDB").Elements("USER")
                            where (string)xNode.Attribute("UserName") == strUserName
                            select xNode;

                if (query.Count() > 0)
                {
                    strMessage = "Add USER failed : User with this name already exists.";
                    return false;
                }

                string strNewUser;
                XElement xNewUser;
                int newID = 1;
                int iUsers = xmlDB.Element("XMLDB").Elements("USER").Count() ;
                if (iUsers > 0)
                {
                    //Atlest 1 user exists in the system, increment ID by 1.
                    XElement xmlLastUser = xmlDB.Element("XMLDB").Elements("USER").Last();
                    newID = Int16.Parse(xmlLastUser.Attribute("ID").Value) + 1;
                    strNewUser = String.Format("<USER ID=\"{0}\" UserName=\"{1}\" IsActive=\"1\" StartDate=\"{2}\" EndDate=\"\" />", newID, strUserName, DateTime.Now.ToShortDateString());
                }
                else
                {
                    //No user exist in the system, newID will use default value of 1.
                    strNewUser = String.Format("<USER ID=\"{0}\" UserName=\"{1}\" IsActive=\"1\" StartDate=\"{2}\" EndDate=\"\" />", newID, strUserName, DateTime.Now.ToShortDateString());
                }

                xNewUser = XElement.Parse(strNewUser, LoadOptions.None);
                xmlDB.Element("XMLDB").Add(xNewUser);
                xmlDB.Save(xmlWorkPath + "User.xml");

                //Reuse xmlDB to load UserBalance.xml now
                xmlDB = XDocument.Load(xmlWorkPath + "UserBalance.xml");
                string strUserBal = String.Format("<USERBALANCE User_ID=\"{0}\" InBal=\"0\" OutBal=\"0\" TotalBal=\"0\" />", newID) ;
                XElement xUserBal = XElement.Parse (strUserBal, LoadOptions.None); 
                xmlDB.Element("XMLDB").Add(xUserBal);
                xmlDB.Save(xmlWorkPath + "UserBalance.xml");

                bResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xmlDB = null;
            }
            return bResult;
        }
        public bool CanRemoveUser(int iUserId)
        {
            XDocument xmlDB;
            bool bResult = false;
            try
            {
                string strUserId = iUserId.ToString();
                xmlDB = XDocument.Load(xmlWorkPath + "UserBalance.xml");
                var query = from xNode in xmlDB.Element("XMLDB").Elements("USERBALANCE")
                            where (string)xNode.Attribute("User_ID") == strUserId
                            select xNode.Attribute("TotalBal") ;

                if (query.Count() > 0)
                {
                        double dTotalBal = Double.Parse(query.First().Value);
                        bResult = (dTotalBal == 0) ? true : false;                        
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xmlDB = null;
            }
            return bResult;
        }

        public bool RemoveUser( int iUserId )
        {
            XDocument xmlDB;
            bool bResult = false;
            try
            {
                xmlDB = XDocument.Load(xmlWorkPath + "User.xml");
                var query = from xNode in xmlDB.Element("XMLDB").Elements("USER")
                            where (string)xNode.Attribute("ID") == iUserId.ToString ()
                            select xNode;
                //TODO: Try to correct above query to update data in query itself.

                if (query.Count() > 0)
                {
                    XElement xUser = query.First();
                    xUser.Attribute("IsActive").Value = "0";
                    xUser.Attribute("EndDate").Value = DateTime.Now.ToShortDateString();   
                    xmlDB.Save(xmlWorkPath + "User.xml");
                    bResult = true;
                }

            }
            catch (Exception ex)
            {
                bResult = false;
                throw ex;
            }
            finally
            {
                xmlDB = null;
            }
            return bResult;
        }

		public DataSet GetTransactionsByUserId( int userId, bool bShowPositiveTransactions)
		{
            DataSet ds = new DataSet("DEFAULT_TABLE");
            XDocument xmlDB_t, xmlDB_tb;
            XElement xResults = new XElement("XMLDB");

            xmlDB_t = XDocument.Load(xmlWorkPath + "Transaction.xml");
            xmlDB_tb = XDocument.Load(xmlWorkPath + "TransactionBreakup.xml");
            if (bShowPositiveTransactions)
            {   
                var query = from t in xmlDB_t.Element("XMLDB").Elements("TRANSACTION")
                            join tb in xmlDB_tb.Element("XMLDB").Elements("TRANSACTIONBREAKUP")
                            on (string)t.Attribute("ID") equals (string)tb.Attribute("Transaction_ID")
                            where (string)tb.Attribute("User_ID") == userId.ToString()
                            where Double.Parse((string)tb.Attribute("Amount")) > 0
                            select new XElement("PositiveTrans", t.Attribute("DateTime"), tb.Attribute("Amount"), t.Attribute("Details"));

                foreach (XElement row in query)
                {
                    xResults.Add(row);
                }
            }
            else
            {
                XDocument xmlDB_u = XDocument.Load(xmlWorkPath + "User.xml");
                var query = from t in xmlDB_t.Element("XMLDB").Elements("TRANSACTION")
                            join tb in xmlDB_tb.Element("XMLDB").Elements("TRANSACTIONBREAKUP")
                            on (string)t.Attribute("ID") equals (string)tb.Attribute("Transaction_ID")
                            join u in xmlDB_u.Element("XMLDB").Elements("USER")
                            on (string)t.Attribute("Payee_ID") equals (string)u.Attribute("ID")  
                            where (string)tb.Attribute("User_ID") == userId.ToString()
                            where Double.Parse((string)tb.Attribute("Amount")) < 0
                            select new XElement("PositiveTrans", t.Attribute("DateTime"), tb.Attribute("Amount"), u.Attribute("UserName"),  t.Attribute("Details"));

                foreach (XElement row in query)
                {
                    xResults.Add(row);
                }
            }

            ds.ReadXml(xResults.CreateReader());

            if (ds.Tables.Count > 0)
            {
                if (bShowPositiveTransactions)
                {
                    ds.Tables[0].Columns["Amount"].ColumnName = "Amount CREDITED to your Account";
                }
                else
                {
                    ds.Tables[0].Columns["Amount"].ColumnName = "Amount DEBITED from your Account";
                    ds.Tables[0].Columns["UserName"].ColumnName = "PAID BY";
                }
                ds.Tables[0].Columns["DateTime"].ColumnName = "DATE";
                ds.Tables[0].Columns["Details"].ColumnName = "DESCRIPTION";
            }
            else
            {
                //Create an empty table to show headers.
                DataTable dt = new DataTable();
                dt.Columns.Add("DATE");
                if (bShowPositiveTransactions)
                {
                   dt.Columns.Add( "Amount CREDITED to your Account");
                }
                else
                {
                    dt.Columns.Add( "Amount DEBITED from your Account");
                    dt.Columns.Add( "PAID BY");
                }
                dt.Columns.Add("DESCTIPTION");
                ds.Tables.Add(dt); 
            }
            return ds;
		}

		public DataSet GetTransactionsSumary()
		{
            DataSet ds = new DataSet("DEFAULT_TABLE");
            XDocument xmlDB_User, xmlDB_Balance;

            xmlDB_User = XDocument.Load(xmlWorkPath + "User.xml");
            xmlDB_Balance = XDocument.Load(xmlWorkPath + "UserBalance.xml");

            XElement xResults = new XElement("XMLDB");
            var query = from userRows in xmlDB_User.Element("XMLDB").Elements("USER")
                        join  balanceRows in xmlDB_Balance.Element("XMLDB").Elements("USERBALANCE") 
                        on (string)userRows.Attribute("ID") equals (string)balanceRows.Attribute("User_ID")
                        where (string)userRows.Attribute ("IsActive") == "1"
                        select new XElement("UserBalance", userRows.Attribute("UserName"), balanceRows.Attribute("InBal"), balanceRows.Attribute("OutBal"), balanceRows.Attribute("TotalBal"), userRows.Attribute("ID")); 

            foreach (XElement row in query)
            {
                xResults.Add(row); 
            }             

            ds.ReadXml(xResults.CreateReader());
            ds.Tables[0].Columns["UserName"].ColumnName = "User";
            ds.Tables[0].Columns["InBal"].ColumnName = "POSITIVE DEPOSIT";
            ds.Tables[0].Columns["OutBal"].ColumnName = "CREDIT TAKEN";
            ds.Tables[0].Columns["TotalBal"].ColumnName = "BALANCE";
            ds.Tables[0].Columns["ID"].ColumnName = "USER ID"; 
            return ds;
		}        

        public DataSet GetActiveUsers()
		{
            DataSet ds = new DataSet("DEFAULT_TABLE");
            ds.ReadXml(xmlWorkPath + "User.xml", XmlReadMode.InferSchema);  
            //return ds;


            DataTable dt = ds.Tables[0];
            ds.Tables.Remove(dt);

            var query = from row in dt.AsEnumerable()
                        where row.Field<string>("IsActive") == "1"
                        //select new { ID = row.Field<string>("ID"), UserName = row.Field<string>("UserName") };
                        select row;
              
            DataTable dtquery = query.CopyToDataTable();
            ds.Tables.Add(dtquery); 
            return ds;
		}
        
		public bool addRecordsToDB( int iPayeeId, Hashtable userCostMap , string strDetails)
		{
			bool bSuccess = false;
			return bSuccess;
		}
		#endregion

		#region Private Methods
	    // No Private methods till now.	
		#endregion
    }

   
}