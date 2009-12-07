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
            // No Logic Implemented Yet.
            return ds;
		}

		public DataSet GetTransactionsSumary()
		{
            DataSet ds = new DataSet("DEFAULT_TABLE");
            // No Logic Implemented Yet.
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
