using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using MyConnect.Oracle;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace MyVlive.CCare
{
    public class V_Member
    {
        MyOracleExecuteData mExec;
        MyOracleGetData mGet;

        public V_Member()
        {
            mExec = new MyOracleExecuteData();
            mGet = new MyOracleGetData();
        }

        public V_Member(string ConfigKey)
        {
            mExec = new MyOracleExecuteData(ConfigKey);
            mGet = new MyOracleGetData(ConfigKey);
        }

         public DataTable Search(int? Type,string MSISDN, int Week)
        {
            try
            {
                string Where = string.Empty;
                string Query = string.Empty;
                string OrderBy = string.Empty;
                string Limit = string.Empty;

                if (!string.IsNullOrEmpty(MSISDN))
                {
                    Where += " userid LIKE '%" + MSISDN + "%' ";
                }


                Query = "   SELECT  * " +
                           "FROM    v_members " +
                           "WHERE   SIMNUMBER LIKE '%" + MSISDN + "%' AND WEEK = " + Week.ToString() + " ";

                return mGet.GetDataTable_Query(Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
