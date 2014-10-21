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
    public class V_MO
    {
        MyOracleExecuteData mExec;
        MyOracleGetData mGet;

        public V_MO()
        {
            mExec = new MyOracleExecuteData();
            mGet = new MyOracleGetData();
        }

        public V_MO(string ConfigKey)
        {
            mExec = new MyOracleExecuteData(ConfigKey);
            mGet = new MyOracleGetData(ConfigKey);
        }

        
        public int TotalRow(int? Type, string MSISDN, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string Where = string.Empty;
                string Query = string.Empty;
                string OrderBy = string.Empty;

                Where = " WHERE   V_mo.requesttime BETWEEN TO_DATE('" + BeginDate.ToString("yyyy-MM-dd h") + "','yyyy-MM-dd HH') AND TO_DATE('" + EndDate.ToString("yyyy-MM-dd h") + "','yyyy-MM-dd HH') ";
                if (!string.IsNullOrEmpty(MSISDN))
                {
                    Where += " AND v_mo.userid LIKE '%" + MSISDN + "%' ";
                }
                Query = "    SELECT  COUNT(v_mo.row_id) AS TotalRow " +
                                "   FROM    v_mo LEFT JOIN v_mt ON v_mo.mo_id = v_mt.mo_id ";
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

        public DataTable Search(int? Type, int BeginRow, int EndRow, string MSISDN, DateTime BeginDate, DateTime EndDate, string OrderByColumn)
        {
            try
            {
                string Where = string.Empty;
                string Query = string.Empty;
                string OrderBy = string.Empty;
                string Limit = string.Empty;


                Where = " WHERE   V_mo.requesttime BETWEEN TO_DATE('" + BeginDate.ToString("yyyy-MM-dd h") + "','yyyy-MM-dd HH') AND TO_DATE('" + EndDate.ToString("yyyy-MM-dd h") + "','yyyy-MM-dd HH') ";

                if (!string.IsNullOrEmpty(MSISDN))
                {
                    Where += " and v_mo.userid LIKE '%" + MSISDN + "%' ";
                }
                if (!string.IsNullOrEmpty(OrderByColumn))
                {
                    OrderBy = " ORDER BY " + OrderByColumn + " ";
                }
                Limit = " WHERE RowNumber BETWEEN " + BeginRow.ToString() + " and " + EndRow.ToString() + "  ";

                Query = "   SELECT * "+
                        "   FROM (  SELECT  v_mo.row_id,v_mo.mo_id,v_mo.userid,v_mo.message AS MO,v_mo.requesttime, v_mo.SERVICEID, " +
                        "                    v_mt.message AS MT, v_mt.responsetime, "+
                         "                   ROWNUM AS RowNumber "+
                         "           FROM    v_mo LEFT JOIN v_mt ON v_mo.mo_id = v_mt.mo_id "+
                         Where+
                         OrderBy+
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
