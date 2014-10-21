using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Text;
using MyUtility;
using MyVlive;
using MyVlive.Vlive;
using System.Data;
namespace MyAdmin.Admin_Vlive
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class Ad_News_Action : IHttpHandler, IRequiresSessionState
    {
        public enum Action
        {
            SaveLetter = 1,
        }

        struct Message
        {
            public static string Error = "<div> Có lỗi xảy ra trong quá trình xử lý.<div>";
            public static string NoData = "<div> Không có dữ liệu.<div>";
        }
        HttpContext MyContext;
        DataSet mSet = new DataSet();
        News mNews = new News();

        StringBuilder mBuilder = new StringBuilder(string.Empty);

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                MyContext = context;
                context.Response.ContentType = "text/html";

                if (MyContext.Session["Vlive"] != null)
                {
                    mSet = MyXML.GetDataSetFromXMLString(MyContext.Session["Vlive"].ToString());
                }
                else
                {
                    mSet = mNews.CreateDataSet(2);
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
                mBuilder = new StringBuilder(Message.Error);
            }
            finally
            {
                MyContext.Session["Vlive"] = mSet.GetXml();
                MyContext.Response.Write(mBuilder.ToString());
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool SaveLetter()
        {
            try
            {
                int LetterID = 0;
                int NewsID = 0;
                string LetterName = string.Empty;
                int Priority = 0;

                DataTable mTable = mSet.Tables["Letter"];
                DataRow mRow = mTable.NewRow();

                LetterName = MyContext.Request.QueryString["LetterName"];

                if (MyContext.Request.QueryString["Priority"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["Priority"], out Priority);
                }

                if (MyContext.Request.QueryString["NewsID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["NewsID"], out NewsID);
                }

                if (MyContext.Request.QueryString["LetterID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["LetterID"], out LetterID);
                }
                else
                {
                    //Nếu là thêm mới thì tạo tạm 1 id, ID không được lưu vào db
                    LetterID = mTable.Rows.Count + 1;
                }

                mRow["NewsID"] = NewsID;
                mRow["LetterName"] = LetterName;
                mRow["Priority"] = Priority;

                mTable.Rows.Add(mRow);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SaveRecord()
        {
            try
            {
                int RecordID = 0;
                int LetterID = 0;
                int RecordTypeID = 0;
                int Priority = 0;
                string RecordName = string.Empty;
                string Introduction = string.Empty;
                string Content = string.Empty;
                int MethodID = 0;
                string ServiceID = string.Empty;
                string Keyword = string.Empty;
                bool IsChild = false;
                int ParentID = 0;

                DataTable mTable = mSet.Tables["Record"];
                DataRow mRow = mTable.NewRow();


                #region MyRegion

                if (MyContext.Request.QueryString["RecordID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["RecordID"], out RecordID);
                }
                else
                {
                    //Nếu là thêm mới thì tạo tạm 1 id, ID không được lưu vào db
                    RecordID = mTable.Rows.Count + 1;
                }
              
                if (MyContext.Request.QueryString["LetterID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["LetterID"], out LetterID);
                }
                if (MyContext.Request.QueryString["RecordTypeID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["RecordTypeID"], out RecordTypeID);
                }
                if (MyContext.Request.QueryString["Priority"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["Priority"], out Priority);
                }
                if (MyContext.Request.QueryString["RecordName"] != null)
                {
                    RecordName = MyContext.Request.QueryString["RecordName"];
                }
                if (MyContext.Request.QueryString["Introduction"] != null)
                {
                    Introduction = MyContext.Request.QueryString["Introduction"];
                }
                if (MyContext.Request.QueryString["Content"] != null)
                {
                    Introduction = MyContext.Request.QueryString["Content"];
                }
                if (MyContext.Request.QueryString["MethodID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["MethodID"], out MethodID);
                }
                if (MyContext.Request.QueryString["ServiceID"] != null)
                {
                    ServiceID = MyContext.Request.QueryString["ServiceID"];
                }
                if (MyContext.Request.QueryString["Keyword"] != null)
                {
                    Keyword = MyContext.Request.QueryString["Keyword"];
                }
                if (MyContext.Request.QueryString["IsChild"] != null)
                {
                    bool.TryParse(MyContext.Request.QueryString["IsChild"], out IsChild);
                }
                if (MyContext.Request.QueryString["ParentID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["ParentID"], out ParentID);
                }
                
                #endregion

              
                mRow["LetterID"] = LetterID;
                mRow["RecordTypeID"] = RecordTypeID;
                mRow["Priority"] = Priority;
                mRow["RecordName"] = RecordName;
                mRow["Introduction"] = Introduction;
                mRow["Content"] = Content;
                mRow["MethodID"] = MethodID;
                mRow["ServiceID"] = ServiceID;
                mRow["Keyword"] = Keyword;
                mRow["IsChild"] = IsChild;
                mRow["ParentID"] = ParentID;

                mTable.Rows.Add(mRow);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ViewRecord()
        {
            try
            {
                int LetterID = 0;
                int ParentID = 0;
                bool IsParent = false;

                if (MyContext.Request.QueryString["LetterID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["LetterID"], out LetterID);
                }
                if (MyContext.Request.QueryString["ParentID"] != null)
                {
                    int.TryParse(MyContext.Request.QueryString["ParentID"], out ParentID);
                }
                if (MyContext.Request.QueryString["IsParent"] != null)
                {
                    bool.TryParse(MyContext.Request.QueryString["IsParent"], out IsParent);
                }
                DataTable mTable = mSet.Tables["Record"];
                if (mTable == null || mTable.Rows.Count < 1)
                {
                    mBuilder.Append(Message.NoData);
                    return;
                }

                mTable.DefaultView.RowFilter = " LetterID = " + LetterID.ToString();

                if (ParentID > 0)
                {
                    mTable.DefaultView.RowFilter += " AND ParentID = " + ParentID.ToString();
                }
               
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadLetter()
        {

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
