using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using MyConnect.Oracle;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace RetryMO
{
    public class MORetry
    {
        MyOracleExecuteData mExec;
        MyOracleGetData mGet;

        public MORetry()
        {
            mExec = new MyOracleExecuteData();
            mGet = new MyOracleGetData();
        }

        public MORetry(string ConfigKey)
        {
            mExec = new MyOracleExecuteData(ConfigKey);
            mGet = new MyOracleGetData(ConfigKey);
        }

        public DataTable Search(int? Type)
        {
            try
            {
                string Where = string.Empty;
                string Query = string.Empty;
                string OrderBy = string.Empty;
                string Limit = string.Empty;
                Where += " WHERE MT_Receive = 0 AND ROWNUM <= 10 ";
                Query = "   SELECT  * " +
                        "   FROM    moretry " + Where;

                return mGet.GetDataTable_Query(Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete(string ListID)
        {
            try
            {
                if (string.IsNullOrEmpty(ListID))
                    return true;

                string Where = string.Empty;
                string Query = string.Empty;
                Query = " DELETE FROM moretry WHERE MO_ID IN ( " + ListID + " ) ";

                if (mExec.ExecQuery(Query) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
