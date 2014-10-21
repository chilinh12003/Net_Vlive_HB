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
    public class News
    {
        MyExecuteData mExec;
        MyGetData mGet;      

        public News()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }      

        #region MyRegion
        public static int News_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("News_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("News_CateID_1"));
                else return 0;
            }
        }

        public static int News_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("News_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("News_CateID"));
                else return 0;
            }
        }

        public static int TinTuc_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("TinTuc_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("TinTuc_CateID_1"));
                else return 0;
            }
        }

        public static int TinTuc_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("TinTuc_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("TinTuc_CateID"));
                else return 0;
            }
        }

        public static int TTHuuIch_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("TTHuuIch_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("TTHuuIch_CateID_1"));
                else return 0;
            }
        }

        public static int TTHuuIch_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("TTHuuIch_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("TTHuuIch_CateID"));
                else return 0;
            }
        }

        public static int GiaiTri_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("GiaiTri_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("GiaiTri_CateID_1"));
                else return 0;
            }
        }

        public static int GiaiTri_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("GiaiTri_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("GiaiTri_CateID"));
                else return 0;
            }
        }

        public static int Game_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("Game_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("Game_CateID_1"));
                else return 0;
            }
        }

        public static int Game_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("Game_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("Game_CateID"));
                else return 0;
            }
        }

        public static int Nhac_HinhAnh_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("Nhac_HinhAnh_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("Nhac_HinhAnh_CateID_1"));
                else return 0;
            }
        }

        public static int Nhac_HinhAnh_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("Nhac_HinhAnh_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("Nhac_HinhAnh_CateID"));
                else return 0;
            }
        }

        public static int XoSo_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("XoSo_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("XoSo_CateID_1"));
                else return 0;
            }
        }

        public static int XoSo_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("XoSo_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("XoSo_CateID"));
                else return 0;
            }
        }

        public static int TheThao_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("TheThao_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("TheThao_CateID_1"));
                else return 0;
            }
        }

        public static int TheThao_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("TheThao_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("TheThao_CateID"));
                else return 0;
            }
        } 
        #endregion

        /// <summary>
        /// Lấy tên file XML cần xuất ra theo CateID
        /// </summary>
        /// <param name="CateID"></param>
        /// <returns></returns>
        public static string GetFileNameXML(int CateID)
        {
            string fileName = "";
            int LichVanSu_CateID = 2;
            int YTe_CateID = 3;
            int Clip_CateID = 4;
            int Game_CateID = 5;
            int TinTuc_CateID = 6;
            int TuVan_CateID = 7;
            int GiaiTri_CateID = 8;
            int NhacCho_CateID = 9;
            int GameVlive_CateID = 10;
            int NhacChuong_CateID = 11;
            int TheThao_CateID = 12;

            int TongHop_CateID = 13;

            string LichVanSu_FileName = "HB_VLive_LVS";
            string YTe_FileName = "HB_VLive_YTe";
            string Clip_FileName = "HB_VLive_Clip";
            string Game_FileName = "HB_VLive_Game";
            string TinTuc_FileName = "HB_VLive_TinTuc";
            string TuVan_FileName = "HB_VLive_TuVan";
            string GiaiTri_FileName = "HB_VLive_GiaiTri";
            string NhacCho_FileName = "HB_VLive_NhacCho";
            string GameVlive_FileName = "HB_VLive_GameVlive";
            string NhacChuong_FileName = "HB_VLive_NhacChuong";
            string TheThao_FileName = "HB_VLive_TheThao";

            string TongHop_FileName = "HB_VLive_TongHop";

            #region MyRegion
            int.TryParse(MyConfig.GetKeyInConfigFile("LichVanSu_CateID"), out LichVanSu_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("YTe_CateID"), out YTe_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("Clip_CateID"), out Clip_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("Game_CateID"), out Game_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("TinTuc_CateID"), out TinTuc_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("TuVan_CateID"), out TuVan_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("GiaiTri_CateID"), out GiaiTri_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("NhacCho_CateID"), out NhacCho_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("NhacChuong_CateID"), out NhacChuong_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("GameVlive_CateID"), out GameVlive_CateID);
            int.TryParse(MyConfig.GetKeyInConfigFile("TheThao_CateID"), out TheThao_CateID);

            int.TryParse(MyConfig.GetKeyInConfigFile("TongHop_CateID"), out TongHop_CateID);

            LichVanSu_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("LichVanSu_FileName")) ? LichVanSu_FileName : MyConfig.GetKeyInConfigFile("LichVanSu_FileName");
            YTe_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("YTe_FileName")) ? YTe_FileName : MyConfig.GetKeyInConfigFile("YTe_FileName");
            Clip_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("Clip_FileName")) ? Clip_FileName : MyConfig.GetKeyInConfigFile("Clip_FileName");
            Game_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("Game_FileName")) ? Game_FileName : MyConfig.GetKeyInConfigFile("Game_FileName");
            TinTuc_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("TinTuc_FileName")) ? TinTuc_FileName : MyConfig.GetKeyInConfigFile("TinTuc_FileName");
            TuVan_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("TuVan_FileName")) ? TuVan_FileName : MyConfig.GetKeyInConfigFile("TuVan_FileName");
            GiaiTri_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("GiaiTri_FileName")) ? GiaiTri_FileName : MyConfig.GetKeyInConfigFile("GiaiTri_FileName");
            NhacCho_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("NhacCho_FileName")) ? NhacCho_FileName : MyConfig.GetKeyInConfigFile("NhacCho_FileName");
            GameVlive_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("GameVlive_FileName")) ? GameVlive_FileName : MyConfig.GetKeyInConfigFile("GameVlive_FileName");
            NhacChuong_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("NhacChuong_FileName")) ? NhacChuong_FileName : MyConfig.GetKeyInConfigFile("NhacChuong_FileName");

            TheThao_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("TheThao_FileName")) ? TheThao_FileName : MyConfig.GetKeyInConfigFile("TheThao_FileName");

            TongHop_FileName = string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("TongHop_FileName")) ? TongHop_FileName : MyConfig.GetKeyInConfigFile("TongHop_FileName");

            #endregion

            if (CateID == LichVanSu_CateID)
                fileName = LichVanSu_FileName;

            else if (CateID == YTe_CateID)
                fileName = YTe_FileName;

            else if (CateID == Clip_CateID)
                fileName = Clip_FileName;

            else if (CateID == Game_CateID)
                fileName = Game_FileName;

            else if (CateID == TinTuc_CateID)
                fileName = TinTuc_FileName;

            else if (CateID == TuVan_CateID)
                fileName = TuVan_FileName;

            else if (CateID == GiaiTri_CateID)
                fileName = GiaiTri_FileName;

            else if (CateID == NhacCho_CateID)
                fileName = NhacCho_FileName;

            else if (CateID == GameVlive_CateID)
                fileName = GameVlive_FileName;

            else if (CateID == NhacChuong_CateID)
                fileName = NhacChuong_FileName;

            else if (CateID == TheThao_CateID)
                fileName = TheThao_FileName;
            else if (CateID == TongHop_CateID)
                fileName = TongHop_FileName;
            return fileName;
        }

        public string GenerationXMLContent(ref int CateID, string NewsID, string Format_Vlive, string Format_BeforeTheClickMessage, string Format_Menu, string Format_MenuItem_Text, string Format_MenuItem_Download)
        {
            try
            {
                string VliveXML = String.Empty;
                DataSet mSet = SelectDataSet(3, NewsID);
                if (mSet == null || mSet.Tables.Count != 3)
                {
                    return VliveXML;
                }

                DataTable tbl_News = mSet.Tables["News"];
                DataTable tbl_Letter = mSet.Tables["Letter"];
                DataTable tbl_Record = mSet.Tables["Record"];

                StringBuilder mBuilder_BeforeTheClickMessage = new StringBuilder(string.Empty);
                int index = 1;
                foreach (DataRow mRow_Letter in mSet.Tables["Letter"].Rows)
                {
                    tbl_Record.DefaultView.RowFilter = " LetterID = " + mRow_Letter["LetterID"].ToString();

                    DataTable tbl_Record_Current = tbl_Record.DefaultView.ToTable();

                    if (tbl_Record_Current.Rows.Count < 1)
                        continue;

                    StringBuilder mBuilder_MenuItem_Parent = new StringBuilder(string.Empty);

                    foreach (DataRow mRow_Record in tbl_Record_Current.Rows)
                    {
                        StringBuilder mBuilder_MenuItem_Child = new StringBuilder(string.Empty);

                        //Nếu là Record cha
                        if ((bool)mRow_Record["IsParent"])
                        {
                            #region Tao menuitem cap 2
                            tbl_Record_Current.DefaultView.RowFilter = " ParentID = " + mRow_Record["RecordID"].ToString();

                            //Bắt đầu build các MenuItem cấp 2, các menuitem này chỉ được phép là dạng Download
                            for (int i = 0; i < tbl_Record_Current.DefaultView.Count; i++)
                            {
                                string[] arr = {    tbl_Record_Current.DefaultView[i]["Introduction"].ToString(),
                                                    ((Record.Method)(int)tbl_Record_Current.DefaultView[i]["MethodID"]).ToString(),
                                                    tbl_Record_Current.DefaultView[i]["ServiceID"].ToString(),
                                                    tbl_Record_Current.DefaultView[i]["Keyword"].ToString()
                                                };
                                mBuilder_MenuItem_Child.Append(string.Format(Format_MenuItem_Download, arr));
                            }

                            tbl_Record_Current.DefaultView.RowFilter = string.Empty;
                            #endregion
                        }
                        else
                        {
                            continue;
                        }

                        if ((Record.RecordType)(int)mRow_Record["RecordTypeID"] == Record.RecordType.SMSText)
                        {
                            #region Tao menuitem cap 1 dang Text
                            string Menu = string.Empty;
                            if (mBuilder_MenuItem_Child.Length > 0)
                                Menu = string.Format(Format_Menu, mBuilder_MenuItem_Child.ToString());
                            string[] arr = {    mRow_Record["Introduction"].ToString(),
                                                 ((Record.Method)(int)mRow_Record["MethodID"]).ToString(),
                                                 mRow_Record["Content"].ToString(),
                                                 Menu
                                                };
                            mBuilder_MenuItem_Parent.Append(string.Format(Format_MenuItem_Text, arr));
                            #endregion
                        }
                        else
                        {
                            #region Tạo MenuItem cap 1 dang download
                            string[] arr = {   mRow_Record["Introduction"].ToString(),
                                                    ((Record.Method)(int)mRow_Record["MethodID"]).ToString(),
                                                    mRow_Record["ServiceID"].ToString(),
                                                    mRow_Record["Keyword"].ToString()
                                                };
                            mBuilder_MenuItem_Parent.Append(string.Format(Format_MenuItem_Download, arr));
                            #endregion
                        }
                    }
                    string[] add_1 = { ((DateTime)mRow_Letter["PushTime"]).ToString("dd-MM-yyyyTHH:mm:ss"), index++.ToString(), mRow_Letter["LetterName"].ToString(), mBuilder_MenuItem_Parent.ToString() };

                    mBuilder_BeforeTheClickMessage.Append(string.Format(Format_BeforeTheClickMessage, add_1));
                }

                CateID = (int)tbl_News.Rows[0]["CateID"];
                VliveXML = string.Format(Format_Vlive, mBuilder_BeforeTheClickMessage.ToString());




                return VliveXML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public News(string KeyConnect_InConfig)
        {
            mExec = new MyExecuteData(KeyConnect_InConfig);
            mGet = new MyGetData(KeyConnect_InConfig);
        }

        public int TotalRow(int? Type, string SearchContent, int? CateID, bool? IsPublish)
        {
            try
            {
                return TotalRow(Type, SearchContent, CateID, IsPublish, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int TotalRow(int? Type, string SearchContent, int? CateID, bool? IsPublish, bool? IsCreateXML)
        {
            try
            {
                return TotalRow(Type, SearchContent, CateID, IsPublish, IsCreateXML, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int TotalRow(int? Type, string SearchContent, int? CateID, bool? IsPublish, bool? IsCreateXML,bool? IsSchedule)
        {
            try
            {
                string[] mpara = { "Type", "SearchContent", "CateID", "IsPublish", "IsCreateXML","IsSchedule", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, CateID.ToString(), (IsPublish == null ? null : IsPublish.ToString()), (IsCreateXML == null ? null : IsCreateXML.ToString()), (IsSchedule == null ? null : IsSchedule.ToString()), true.ToString() };
                return (int)mGet.GetExecuteScalar("Sp_News_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int? CateID, bool? IsPublish, string OrderByColumn)
        {
            try
            {
                return Search(Type, BeginRow, EndRow, SearchContent, CateID, IsPublish, null, OrderByColumn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int? CateID, bool? IsPublish,bool? IsCreateXML, string OrderByColumn)
        {
            try
            {
                return Search(Type, BeginRow, EndRow, SearchContent, CateID, IsPublish, IsCreateXML, null, OrderByColumn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int? CateID, bool? IsPublish, bool? IsCreateXML,bool? IsSchedule, string OrderByColumn)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "CateID", "IsPublish", "IsCreateXML","IsSchedule", "OrderByColumn", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, CateID.ToString(), (IsPublish == null ? null : IsPublish.ToString()), (IsCreateXML == null ? null : IsCreateXML.ToString()), (IsSchedule == null ? null : IsSchedule.ToString()), OrderByColumn, false.ToString() };
                return mGet.GetDataTable("Sp_News_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Tạo 1 Dataset mẫu
        /// </summary>
        /// <returns></returns>
        public DataSet CreateDataSet()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                DataSet mSet = mGet.GetDataSet("Sp_News_Select", mPara, mValue);
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
        /// tạo dataset
        /// </summary>
        /// <param name="Type">
        /// Cách thức lấy
        /// <para>Type = 2: lấy tất cả các table liên quan</para>
        /// </param>
        /// <returns></returns>
        public DataSet CreateDataSet(int Type)
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { Type.ToString() };
                DataSet mSet = mGet.GetDataSet("Sp_News_Select", mPara, mValue);
                if (mSet != null && mSet.Tables.Count >= 1)
                {
                    mSet.DataSetName = "Parent";
                    mSet.Tables[0].TableName = "News";
                    mSet.Tables[1].TableName = "Letter";
                    mSet.Tables[2].TableName = "Record";
                }
                return mSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Type">Các thức lấy
        /// <para>Type = 3: Lấy 3 table News, Letter, Record theo NewsID = Para_1</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataSet CreateDataSet(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type","Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                DataSet mSet = mGet.GetDataSet("Sp_News_Select", mPara, mValue);
                if (mSet != null && mSet.Tables.Count >= 1)
                {
                    mSet.DataSetName = "Parent";
                    mSet.Tables[0].TableName = "News";
                    mSet.Tables[1].TableName = "Letter";
                    mSet.Tables[2].TableName = "Record";
                }
                return mSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu News
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 3: Lấy thông tin tất cả các table liên quan tới 1 bản ghi của News</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataSet SelectDataSet(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1"};
                string[] mValue = { Type.ToString(), Para_1 };
                DataSet mSet = mGet.GetDataSet("Sp_News_Select", mPara, mValue);
                if (mSet != null && mSet.Tables.Count >= 1)
                {
                    mSet.DataSetName = "Parent";
                    mSet.Tables[0].TableName = "News";
                    mSet.Tables[1].TableName = "Letter";
                    mSet.Tables[2].TableName = "Record";
                }
                return mSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        /// <summary>
        /// Lấy dữ liệu về News
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Lấy dữ liệu mẫu</para>
        /// <para>Type = 1: Lấy thông tin chi tiết 1 record (Para_1 = NewsID)</para>
        /// <para>Type = 4: Lấy dữ liệu của 1 listID (ListID = Para_1) chưa tạo XML và đã được kích hoạt: VD 1,2,3,4</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mpara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_News_Select", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu News
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 3: Lấy các Record cùng thể loại (Para_1 = NewsID, Para_2 = RowCount)</para>
        /// <para>Type = 5: Lấy tất cả tin của 1 ngày, theo ngày phát (Para_1 = BeginTime, Para_2= EndTime)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1, string Para_2)
        {
            try
            {
                string[] mpara = { "Type", "Para_1", "Para_2" };
                string[] mValue = { Type.ToString(), Para_1, Para_2 };
                return mGet.GetDataTable("Sp_News_Select", mpara, mValue);
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
                if (mExec.ExecProcedure("Sp_News_Insert", mpara, mValue) > 0)
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

        public bool Insert_Vlive(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };

                mExec.UseTransction = true;
                if (mExec.ExecProcedure("Sp_Vlive_Insert", mpara, mValue) > 0)
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

        public bool Update_Vlive(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };

                mExec.UseTransction = true;
                if (mExec.ExecProcedure("Sp_Vlive_Update", mpara, mValue) > 0)
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
        /// <param name="Type">Type = 0: </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Update(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_News_Update", mpara, mValue) > 0)
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

        public bool Update_CreateXML(int NewsID)
        {
            try
            {
                string[] mpara = { "NewsID" };
                string[] mValue = { NewsID.ToString() };
                if (mExec.ExecProcedure("Sp_News_Update_CreateXML", mpara, mValue) > 0)
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
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Delete(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_News_Delete", mpara, mValue) > 0)
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
        public bool Publish(int? Type, bool IsPublish, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsPublish", "XMLContent", };
                string[] mValue = { Type.ToString(), IsPublish.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_News_Publish", mpara, mValue) > 0)
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
        public bool CurrentXML(int? Type, bool IsCurrentXML, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsCurrentXML", "XMLContent", };
                string[] mValue = { Type.ToString(), IsCurrentXML.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_News_CurrentXML", mpara, mValue) > 0)
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

        public void SetDataSet_Session(DataSet mSet)
        {
            try
            {
                if (mSet != null)
                {
                    MyConvert.ConvertDateColumnToStringColumn(ref mSet);
                    MyUtility.MyCurrent.CurrentPage.Session["VliveSchema"] = mSet.GetXmlSchema();
                    MyUtility.MyCurrent.CurrentPage.Session["Vlive"] = mSet.GetXml();
                }
                else
                {
                    MyUtility.MyCurrent.CurrentPage.Session["Vlive"] = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lấy dataset trong session
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet_Session()
        {
            
            DataSet mSet = new DataSet();
            if (MyUtility.MyCurrent.CurrentPage.Session["VliveSchema"] != null)
            {
                if (MyUtility.MyCurrent.CurrentPage.Session["Vlive"] != null && !string.IsNullOrEmpty(MyUtility.MyCurrent.CurrentPage.Session["Vlive"].ToString()))
                {
                    mSet = MyXML.GetDataSetFromXMLString(MyUtility.MyCurrent.CurrentPage.Session["Vlive"].ToString());
                }
                if (mSet.Tables.Count == 3)
                {
                }
                else if (mSet.Tables.Count == 0)
                {
                    //Nếu chưa có dữ liệu thì tạo dataset mấu
                    mSet = MyXML.GetDataSetFromXMLSchemaString(MyUtility.MyCurrent.CurrentPage.Session["VliveSchema"].ToString());
                }
                else
                {
                    //nếu có table nhưng chưa đủ 3 table. thì thêm các table chưa có vào dataset
                    DataSet mSet_Temp = MyXML.GetDataSetFromXMLSchemaString(MyUtility.MyCurrent.CurrentPage.Session["VliveSchema"].ToString());
                    if (!mSet.Tables.Contains("News"))
                    {
                        DataTable mTable = mSet_Temp.Tables["News"].Copy();
                        mSet.Tables.Add(mTable);
                    }
                    if (!mSet.Tables.Contains("Letter"))
                    {
                        DataTable mTable = mSet_Temp.Tables["Letter"].Copy();
                        mSet.Tables.Add(mTable);
                    }
                    if (!mSet.Tables.Contains("Record"))
                    {
                        DataTable mTable = mSet_Temp.Tables["Record"].Copy();
                        mSet.Tables.Add(mTable);
                    }
                }

                //kiểm tra table có bị mất column nào không (vì trường hợp Clumn == null thì trong file xml sẽ không có)
                DataSet mSet_full = MyXML.GetDataSetFromXMLSchemaString(MyUtility.MyCurrent.CurrentPage.Session["VliveSchema"].ToString());

                foreach (DataColumn mCol in mSet_full.Tables["News"].Columns)
                {
                    if (!mSet.Tables["News"].Columns.Contains(mCol.ColumnName))
                    {
                        DataColumn mNewCol = new DataColumn(mCol.ColumnName, typeof(string));
                        mSet.Tables["News"].Columns.Add(mNewCol);
                    }
                }

                foreach (DataColumn mCol in mSet_full.Tables["Letter"].Columns)
                {
                    if (!mSet.Tables["Letter"].Columns.Contains(mCol.ColumnName))
                    {
                        DataColumn mNewCol = new DataColumn(mCol.ColumnName, typeof(string));
                        mSet.Tables["Letter"].Columns.Add(mNewCol);
                    }
                }

              
                foreach (DataColumn mCol in mSet_full.Tables["Record"].Columns)
                {
                    if (!mSet.Tables["Record"].Columns.Contains(mCol.ColumnName))
                    {
                        DataColumn mNewCol = new DataColumn(mCol.ColumnName, typeof(string));
                        mSet.Tables["Record"].Columns.Add(mNewCol);
                    }
                }
            }
            else
            {
                mSet = CreateDataSet(2);

                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                MyUtility.MyCurrent.CurrentPage.Session["VliveSchema"] = mSet.GetXmlSchema();
            }
            MyUtility.MyCurrent.CurrentPage.Session["Vlive"] = mSet.GetXml();
            return mSet;
        }
    }
}
