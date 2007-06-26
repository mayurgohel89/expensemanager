using System;
using System.Data;
using System.Xml;
using System.Collections;


namespace ExpenseManager
{
    public sealed class XMLHelper : IDataHelper 
    {
        #region Member Variables
        public static XMLHelper m_XMLHelper = null ;
        #endregion

		#region constuctors
        private XMLHelper()
		{
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
            // No Logic Implemented Yet.
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
