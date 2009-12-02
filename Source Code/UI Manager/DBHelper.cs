using System;
using System.Data; 
using System.Data.SqlClient;
using System.Collections ; 
   

namespace ExpenseManager
{
	/// <summary>
	/// Description: 
	/// </summary>
	public sealed class DBHelper : IDataHelper 
	{
        #region Member Variables
        private static DBHelper m_DBHelper = null;
        private System.Data.SqlClient.SqlConnection m_conn;
        private System.Data.SqlClient.SqlDataAdapter m_da;
        private System.Data.SqlClient.SqlCommand m_sqlCmd;
        #endregion

		#region constuctors
        private DBHelper()
		{
			this.m_conn   = new System.Data.SqlClient.SqlConnection();
            this.m_conn.ConnectionString = Settings.Default.DBStringLocalHost ; 		 
        }
		#endregion

		#region Public Methods
        public static DBHelper Get()
        {
            if (m_DBHelper == null )
            {
                m_DBHelper = new DBHelper(); 
            }
            return m_DBHelper;
        }

        public bool AddUser(string strUserName)
        {
            m_conn.Open();
            m_sqlCmd = new SqlCommand("sp_AddNewUser", m_conn);
            m_sqlCmd.CommandType = CommandType.StoredProcedure;
            m_sqlCmd.Parameters.Add("@userName", SqlDbType.VarChar).Value = strUserName;
            m_sqlCmd.ExecuteNonQuery();
            m_conn.Close();
            return true;
        }
        public bool CanRemoveUser(int iUserId)
        {
            bool bCanRemoveUser = false;
            decimal dTotalBalance = 0;
            m_conn.Open();
            m_sqlCmd = new SqlCommand("sp_GetUserBalance", m_conn);
            m_sqlCmd.CommandType = CommandType.StoredProcedure;
            m_sqlCmd.Parameters.Add("@userId", SqlDbType.Int).Value = iUserId;
            dTotalBalance = (decimal) m_sqlCmd.ExecuteScalar();
            m_conn.Close();
            if( dTotalBalance == 0   )
            {
                bCanRemoveUser = true;
            }
            return bCanRemoveUser;
        }

        public bool RemoveUser(int iUserId)
        {
            bool bSuccess = false;
            int iRowsEffected = 0;
            m_conn.Open();
            m_sqlCmd = new SqlCommand("sp_DeactivateUser", m_conn);
            m_sqlCmd.CommandType = CommandType.StoredProcedure;
            m_sqlCmd.Parameters.Add("@userId", SqlDbType.Int).Value = iUserId;
            iRowsEffected = m_sqlCmd.ExecuteNonQuery();
            bSuccess = (iRowsEffected > Constants.ZERO) ? true : false;
            return bSuccess;
        }

		public DataSet GetTransactionsByUserId( int iUserId, bool bShowPositiveTransactions)
		{
			m_conn.Open(); 
			m_sqlCmd = new SqlCommand( "sp_TransactionDetails" , m_conn );
			m_sqlCmd.CommandType = CommandType.StoredProcedure ;
            m_sqlCmd.Parameters.Add( "@userId",SqlDbType.Int ).Value =  iUserId;
            m_sqlCmd.Parameters.Add( "@showPositiveTransactions", SqlDbType.Bit ).Value =  bShowPositiveTransactions;
			m_da = new System.Data.SqlClient.SqlDataAdapter( m_sqlCmd );
			DataSet ds = new DataSet( "DEFAULT_TABLE");
			m_da.Fill(ds);
			m_conn.Close();
			return ds;
		}

		public DataSet GetTransactionsSumary()
		{
			m_conn.Open(); 
			m_sqlCmd = new SqlCommand ("sp_PopulateGridSumary" , m_conn);
			m_sqlCmd.CommandType = CommandType.StoredProcedure;  
			m_da = new System.Data.SqlClient.SqlDataAdapter(m_sqlCmd);
			DataSet ds = new DataSet("DEFAULT_TABLE");
			m_da.Fill(ds);
			m_conn.Close();
			return ds;
		}
		
        public DataSet GetActiveUsers()
		{
			m_conn.Open();
            m_sqlCmd = new SqlCommand("sp_GetActiveUsers", m_conn);
            m_sqlCmd.CommandType = CommandType.StoredProcedure;   
			m_da = new System.Data.SqlClient.SqlDataAdapter(m_sqlCmd);
			DataSet ds = new DataSet("DEFAULT_TABLE");
			m_da.Fill(ds);
			m_conn.Close();
			return ds;
		}
        
		public bool addRecordsToDB( int iPayeeId, Hashtable userCostMap , string strDetails)
		{
			bool bSuccess = false;
			int iTransactionId = 0;
			m_conn.Open();
			SqlTransaction transInsertData = m_conn.BeginTransaction() ;
			try
			{
				m_sqlCmd = new SqlCommand( "sp_AddTransaction", m_conn ); 
				m_sqlCmd.Transaction =  transInsertData;
				m_sqlCmd.CommandType = CommandType.StoredProcedure ;

				SqlParameter sqlParaPId = new SqlParameter("@payeeId", SqlDbType.Int );
				sqlParaPId.Value = iPayeeId;
				SqlParameter sqlParaDetails = new SqlParameter("@details", SqlDbType.NVarChar, strDetails.Length  );
				sqlParaDetails.Value = strDetails;
				SqlParameter sqlParaTId = new SqlParameter("@newTransactionId", SqlDbType.Int );
				sqlParaTId.Direction = ParameterDirection.Output;
				
				m_sqlCmd.Parameters.Add (sqlParaPId);
				m_sqlCmd.Parameters.Add (sqlParaDetails);
                m_sqlCmd.Parameters.Add (sqlParaTId);
				
				m_sqlCmd.ExecuteNonQuery(); 

				iTransactionId = (Int32) m_sqlCmd.Parameters["@newTransactionId"].Value; 
    			IEnumerator iterKeys = userCostMap.Keys.GetEnumerator() ;
				while( iterKeys.MoveNext() )
				{
					m_sqlCmd = new SqlCommand( "sp_AddSharedCost", m_conn );
					m_sqlCmd.CommandType = CommandType.StoredProcedure ;
					m_sqlCmd.Transaction = transInsertData;
					m_sqlCmd.Parameters.Add("@transactionId",SqlDbType.Int).Value = iTransactionId;
					m_sqlCmd.Parameters.Add("@userId",SqlDbType.Int).Value = Int16.Parse( iterKeys.Current.ToString() )  ;
					m_sqlCmd.Parameters.Add("@amount",SqlDbType.Float).Value = (float) Double.Parse( userCostMap[iterKeys.Current].ToString(), System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign );
					m_sqlCmd.ExecuteNonQuery();
				}		
				transInsertData.Commit(); 
				bSuccess = true;
			}
			catch( Exception ex)
			{
				System.Windows.Forms.MessageBox.Show ( ex.Message );   
				transInsertData.Rollback(); 
			}
			finally
			{
				m_conn.Close();
			}
              
			return bSuccess;
		}

		#endregion

		#region Private Methods
	    // No Private methods till now.	
		#endregion
	}
}
