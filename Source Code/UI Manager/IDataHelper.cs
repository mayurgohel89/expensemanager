using System.Data;
using System.Collections;


namespace ExpenseManager
{
    interface IDataHelper
    {
        DataSet GetActiveUsers();
        bool AddTransaction(int iPayeeId, Hashtable userCostMap, string strDetails);
        DataSet GetUserBalance();
        DataSet GetTransactionSummary();
        DataSet GetTransactionsByUserId(int iUserId, bool bShowPositiveTransactions);
        bool AddUser(string strUserName, ref string strMessage);
        bool CanRemoveUser(int iUserId);
        bool RemoveUser(int iUserId);
        bool VoidTransaction(int iTransactionId);
    }
}
