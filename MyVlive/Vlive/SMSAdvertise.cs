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
    public class SMSAdvertise
    {
        public enum SMSTypeID
        {
            [DescriptionAttribute("Tin nhắn text")]
            SMSText = 1,
            [DescriptionAttribute("Tin nhắn Wappush")]
            Wappush = 2,
        }

        public enum MediaType
        {
            Nothing = 0,
            [DescriptionAttribute("Game")]
            Game = 1,
            [DescriptionAttribute("Ứng dụng")]
            Soft = 2,
            [DescriptionAttribute("Video-Clip")]
            Video = 3,
            [DescriptionAttribute("Nhạc chuông")]
            Ringtone = 4,
            [DescriptionAttribute("Hình Ảnh")]
            Image = 5,
            [DescriptionAttribute("Theme")]
            Theme = 6,
            [DescriptionAttribute("TextBase")]
            TextBase = 7,
            [DescriptionAttribute("Nhạc chờ")]
            Ringback = 8,
            [DescriptionAttribute("Truyện audio")]
            Audio = 9,
            [DescriptionAttribute("Gói Media")]
            Packet = 10,
        }
        MyExecuteData mExec;
        MyGetData mGet;      

        public SMSAdvertise()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }
        public SMSAdvertise(string KeyConnect_InConfig)
        {
            mExec = new MyExecuteData(KeyConnect_InConfig);
            mGet = new MyGetData(KeyConnect_InConfig);
        }

        public int TotalRow(int? Type, string SearchContent, int? SMSTypeID, int? MediaTypeID, bool? IsActive)
        {
            try
            {
                string[] mpara = { "Type", "SearchContent", "SMSTypeID", "MediaTypeID", "IsActive","IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, SMSTypeID.ToString(), MediaTypeID.ToString(), (IsActive == null ? null : IsActive.ToString()), true.ToString() };
                return (int)mGet.GetExecuteScalar("Sp_SMSAdvertise_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int? SMSTypeID, int? MediaTypeID, bool? IsActive, string OrderByColumn)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "SMSTypeID", "MediaTypeID", "IsActive", "OrderByColumn", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, SMSTypeID.ToString(), MediaTypeID.ToString(), (IsActive == null ? null : IsActive.ToString()), OrderByColumn, false.ToString() };
                return mGet.GetDataTable("Sp_SMSAdvertise_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CreateDataSet()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                DataSet mSet = mGet.GetDataSet("Sp_SMSAdvertise_Select", mPara, mValue);
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
        public DataTable Select(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type","Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_SMSAdvertise_Select", mPara, mValue);             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 3: Lấy chi tiết 1 rows theo keyword và shortcode đang được kích hoạt</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1, string Para_2)
        {
            try
            {
                string[] mPara = { "Type","Para_1","Para_2" };
                string[] mValue = { Type.ToString(), Para_1, Para_2 };
                return mGet.GetDataTable("Sp_SMSAdvertise_Select", mPara, mValue);             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_SMSAdvertise_Update", mpara, mValue) > 0)
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

        public bool Insert(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_SMSAdvertise_Insert", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_SMSAdvertise_Delete", mpara, mValue) > 0)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="IsActive"></param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Active(int? Type, bool IsActive, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsActive", "XMLContent", };
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_SMSAdvertise_Active", mpara, mValue) > 0)
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
    }
}
