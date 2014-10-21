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
    public class V_MemberLog
    {
        MyOracleExecuteData mExec;
        MyOracleGetData mGet;

        public V_MemberLog()
        {
            mExec = new MyOracleExecuteData();
            mGet = new MyOracleGetData();
        }

        public V_MemberLog(string ConfigKey)
        {
            mExec = new MyOracleExecuteData(ConfigKey);
            mGet = new MyOracleGetData(ConfigKey);
        }

        
        public int TotalRow(int? Type, string MSISDN,int Week, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string Where = string.Empty;
                string Query = string.Empty;
                string OrderBy = string.Empty;

                Where = " WHERE   v_members_log.createdtime BETWEEN TO_DATE('" + BeginDate.ToString("yyyy-MM-dd h") + "','yyyy-MM-dd HH') AND TO_DATE('" + EndDate.ToString("yyyy-MM-dd h") + "','yyyy-MM-dd HH') ";

                if (!string.IsNullOrEmpty(MSISDN))
                {
                    Where += " AND v_members_log.simnumber LIKE '%" + MSISDN + "%' ";
                }
                if(Week > 0)
                {
                       Where += " AND v_members_log.week = " + Week.ToString() + " ";
                }
                Query = "    SELECT  count(v_members_log.id) AS TotalRow "+
                            "FROM    v_members_log LEFT JOIN v_questions ON v_members_log.question_id = v_questions.id ";

                Query += Where;

                int TotalRow = 0;
                int.TryParse(mGet.GetExecuteScalar_Query(Query).ToString(), out TotalRow);

                return TotalRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string MSISDN,int Week, DateTime BeginDate, DateTime EndDate, string OrderByColumn)
        {
            try
            {
                string Where = string.Empty;
                string Query = string.Empty;
                string OrderBy = string.Empty;
                string Limit = string.Empty;


                Where = " WHERE   v_members_log.createdtime BETWEEN TO_DATE('" + BeginDate.ToString("yyyy-MM-dd h") + "','yyyy-MM-dd HH') AND TO_DATE('" + EndDate.ToString("yyyy-MM-dd h") + "','yyyy-MM-dd HH') ";

                if (!string.IsNullOrEmpty(MSISDN))
                {
                    Where += " AND v_members_log.simnumber LIKE '%" + MSISDN + "%' ";
                }
                if (Week > 0)
                {
                    Where += " AND v_members_log.week = " + Week.ToString() + " ";
                }

                if (!string.IsNullOrEmpty(OrderByColumn))
                {
                    OrderBy = " ORDER BY " + OrderByColumn + " ";
                }
                Limit = " WHERE RowNumber BETWEEN " + BeginRow.ToString() + " and " + EndRow.ToString() + "  ";

                Query = "   SELECT * " +
                           "FROM (  SELECT  v_members_log.*, " +
                           "                v_questions.question, " +
                           "                ROWNUM AS RowNumber " +
                           "        FROM    v_members_log LEFT JOIN v_questions ON v_members_log.question_id = v_questions.id " +
                         Where +
                         OrderBy +
                         "       ) " +
                         Limit;

                return mGet.GetDataTable_Query(Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
