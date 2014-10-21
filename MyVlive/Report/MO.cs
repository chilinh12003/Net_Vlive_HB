using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using MyConnect.Oracle;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace MyVlive.Report
{
    public class MO
    {
        MyOracleExecuteData mExec;
        MyOracleGetData mGet;

        public MO()
        {
            mExec = new MyOracleExecuteData();
            mGet = new MyOracleGetData();
        }

        public MO(string ConfigKey)
        {
            mExec = new MyOracleExecuteData(ConfigKey);
            mGet = new MyOracleGetData(ConfigKey);
        }


        public DataTable Search(int? Type, DateTime BeginDate, DateTime EndDate, string OrderByColumn)
        {
            try
            {
                string Where = string.Empty;
                string Query = string.Empty;
                string OrderBy = string.Empty;
                string GroupBy = string.Empty;


                Where = " WHERE  MO.MO_RXTIME BETWEEN TO_DATE('" + BeginDate.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') AND TO_DATE('" + EndDate.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') ";

                if (!string.IsNullOrEmpty(OrderByColumn))
                {
                    OrderBy = " ORDER BY " + OrderByColumn + " ";
                }
                GroupBy = " GROUP BY MO_Keyword,MO_ShortCode ";

                Query = "   SELECT MO_Keyword,MO_ShortCode, COUNT(MO_Keyword) AS TOTAL,TO_DATE('" + BeginDate.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') AS ReportDate " +
                        "   FROM    MO " +
                         Where + GroupBy +
                         OrderBy;

                return mGet.GetDataTable_Query(Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
