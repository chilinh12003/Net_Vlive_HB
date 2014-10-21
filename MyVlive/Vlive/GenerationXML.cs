using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace MyVlive.Vlive
{
    public class GenerationXML
    {
         MyExecuteData mExec;
        MyGetData mGet;
        public enum Status
        {
            [DescriptionAttribute("Đã tạo xml")]
            CreatedXML = 1,
            [DescriptionAttribute("Chưa tạo xml")]
            NotCreateXML = 2
        }
        public GenerationXML()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public DataSet CreateDataSet()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                DataSet mSet = mGet.GetDataSet("Sp_GenerationXML_Select", mPara, mValue);
                if (mSet != null && mSet.Tables.Count >= 1)
                {
                    mSet.DataSetName = "Parent";
                    mSet.Tables[0].TableName = "Child";
                }
                return mSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 1: Lấy tất cả dữ liệu</para>
        /// <para>Type = 2: Lấy TOP (Para_1 = TOP) các Record chưa Tạo XML</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mpara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_GenerationXML_Select", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        /// <summary>
        /// Insert dữ liệu
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Insert(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedureTran("Sp_GenerationXML_Insert", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

      
        public bool Delete(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedureTran("Sp_GenerationXML_Delete", mpara, mValue) > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
       
        public bool Active(int? Type, bool IsActive, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsActive", "XMLContent", };
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent };
                if (mExec.ExecProcedureTran("Sp_GenerationXML_Active", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public bool UpdateCreateXML(int? Type, bool IsCreateXML, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsCreateXML", "XMLContent", };
                string[] mValue = { Type.ToString(), IsCreateXML.ToString(), XMLContent };
                if (mExec.ExecProcedureTran("Sp_GenerationXML_CreateXML", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int TotalRow(int? Type, string SearchContent, int? CateID, bool? IsActive)
        {
            try
            {
                string[] mpara = { "Type", "SearchContent", "CateID", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, CateID.ToString(), (IsActive == null ? null : IsActive.ToString()), true.ToString() };
                return (int)mGet.GetExecuteScalar("Sp_GenerationXML_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int? CateID, bool? IsActive, string OrderByColumn)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "CateID", "IsActive", "OrderByColumn", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, CateID.ToString(), (IsActive == null ? null : IsActive.ToString()), OrderByColumn, false.ToString() };
                return mGet.GetDataTable("Sp_GenerationXML_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
