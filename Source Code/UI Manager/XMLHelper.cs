using System;
using System.Data;
using System.Xml;
using System.Collections;

using System.Linq;
 

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

        public bool AddUser(string strUserName)
        {
            return false;
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
