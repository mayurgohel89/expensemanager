using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;

namespace ExpenseManager
{
    /// <summary>
    /// This class contains implementation of those methods which uses Linq2Sql approach
    /// </summary>
    public sealed partial class DBHelper : IDataHelper
    {
        /// <summary>
        /// This example shows how to call a stored procedure using LINQ2SQL
        /// </summary>
        /// <returns></returns>
        public DataSet GetActiveUsers()
        {
            Linq2SqlDataContext db = new Linq2SqlDataContext();
            ISingleResult<sp_GetActiveUsersResult> rslt = db.sp_GetActiveUsers();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("UserName"); 
            foreach (sp_GetActiveUsersResult row in rslt)
            {
                dt.Rows.Add (row.ID, row.UserName);   
            }
              
            DataSet ds = new DataSet("DEFAULT_TABLE");
            ds.Tables.Add(dt); 
            return ds;
        }

        /// <summary>
        /// This example shows how to update a table using LINQ2SQL
        /// </summary>
        /// <param name="iTransactionId"></param>
        /// <returns></returns>
        public bool VoidTransaction(int iTransactionId)
        {
            Linq2SqlDataContext db = new Linq2SqlDataContext();
            tabTransaction trans = db.tabTransactions.Single(t => t.ID == iTransactionId);
            trans.IsVoid = 1;
            db.SubmitChanges();
            //TODO: Add a new SP to correct the amount from UserBalance.
            return true;
        }
    }
}
