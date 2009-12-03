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
            xmlWorkPath = @"..\..\..\..\XMLFiles\";   
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
            XDocument xUsers ;
            bool bResult = false;
            try
            {
                xUsers = XDocument.Load(xmlWorkPath + "User.xml");
                var query = from xNode in xUsers.Element("XMLDB").Elements("USER")
                            where (string)xNode.Attribute("UserName") == strUserName
                            select xNode;

                if (query.Count() > 0)
                {
                    strMessage = "Add USER failed : User with this name already exists.";
                    return false;
                }

                string strNewUser;
                XElement xNewUser;
                int iUsers = xUsers.Element("XMLDB").Elements("USER").Count() ;
                if (iUsers > 0)
                {
                    //Atlest 1 user exists in the system, increment ID and insert new row as last sibling of <USER>.
                    XElement xmlLastUser = xUsers.Element("XMLDB").Elements("USER").Last();
                    int newID = Int16.Parse(xmlLastUser.Attribute("ID").Value) + 1;
                    strNewUser = String.Format("<USER ID=\"{0}\" UserName=\"{1}\" IsActive=\"1\" StartDate=\"{2}\" EndDate=\"\" />", newID, strUserName, DateTime.Now.ToShortDateString());
                    xNewUser = XElement.Parse(strNewUser, LoadOptions.None);
                    xmlLastUser.AddAfterSelf(xNewUser);
                }
                else
                {
                    //No user exist in the system, create new row with ID = 1 as child of <XMLDB>.
                    strNewUser = String.Format("<USER ID=\"1\" UserName=\"{0}\" IsActive=\"1\" StartDate=\"{1}\" EndDate=\"\" />", strUserName, DateTime.Now.ToShortDateString());
                    xNewUser = XElement.Parse(strNewUser, LoadOptions.None);
                    xUsers.Element("XMLDB").Add(xNewUser);
                }
                
                xUsers.Save(xmlWorkPath + "User.xml");
                bResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xUsers = null;
            }
            return bResult;
        }
        public bool CanRemoveUser(int iUserId)
        {
            return false;
        }

        public bool RemoveUser( int iUserId )
        {
            return false ;
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
