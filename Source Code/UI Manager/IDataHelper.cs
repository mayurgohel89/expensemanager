using System.Data;
using System.Collections;


namespace ExpenseManager
{
    interface IDataHelper
    {
        DataSet GetActiveUsers();
		bool addRecordsToDB( int iPayeeId, Hashtable userCostMap , string strDetails );
        DataSet GetTransactionsSumary();
        DataSet GetTransactionsByUserId( int iUserId, bool bShowPositiveTransactions );
        bool AddUser(string strUserName, ref string strMessage);
        bool CanRemoveUser(int iUserId);
        bool RemoveUser( int iUserId );
    }        
}
