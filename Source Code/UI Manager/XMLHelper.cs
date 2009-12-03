using System;
using System.Data;
using System.Xml;
using System.Collections;

using System.Linq;
using System.Xml.Linq;

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
                //TODO: Calculate correct value for USER.ID  
                string strNewUser = String.Format("<USER ID=\"{0}\" UserName=\"{1}\" IsActive=\"1\" StartDate=\"{2}\" EndDate=\"\" />", 99, strUserName, DateTime.Now.ToShortDateString() );
                XElement xNewUser = XElement.Parse(strNewUser, LoadOptions.None );

                XElement xmlLastUser = xUsers.Element("XMLDB").Elements("USER").Last();
                xmlLastUser.AddAfterSelf(xNewUser);
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
