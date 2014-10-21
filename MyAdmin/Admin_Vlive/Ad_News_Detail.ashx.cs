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
    public class Ad_News_Detail : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            try
            {

                int NewsID = 0;
                if (context.Request["id"] != null)
                    int.TryParse(context.Request["id"], out NewsID);

                if (NewsID < 1)
                {
                    context.Response.Write("<div class='InsertLetter_Error'>KHÔNG CÓ DỮ LIỆU!</div>");
                    return;
                }

                News mNews = new News();
                DataSet mSet = mNews.SelectDataSet(3, NewsID.ToString());
                if (mSet == null || mSet.Tables.Count != 3 || mSet.Tables[0].Rows.Count < 1)
                {
                    context.Response.Write("<div class='InsertLetter_Error'>KHÔNG CÓ DỮ LIỆU!</div>");
                    return;
                }

                string Format_News = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/View/News.htm"));
                string Format_Letter = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/View/Letter_Repeat.htm"));
                string Format_Record_Text = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/View/Record_Child_Repeat_Text.htm"));
                string Format_Record_Download = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/View/Record_Child_Repeat_Download.htm"));
                DataTable tbl_Letter = mSet.Tables["Letter"];
                DataTable tbl_Record = mSet.Tables["Record"];

                StringBuilder mBuilder = new StringBuilder(string.Empty);
                int index = 1;
                foreach (DataRow mRow_Letter in tbl_Letter.Rows)
                {
                    StringBuilder mBuilder_Parent = new StringBuilder(string.Empty);
                    StringBuilder mBuilder_Child = new StringBuilder(string.Empty);
                    tbl_Record.DefaultView.RowFilter = "LetterID = " + mRow_Letter["LetterID"].ToString() + " AND IsParent = 1";
                    for (int i = 0; i<tbl_Record.DefaultView.Count; i++)
                    {
                        if ((Record.RecordType)(int)tbl_Record.DefaultView[i]["RecordTypeID"] == Record.RecordType.SMSText)
                        {
                            mBuilder_Parent.Append(string.Format(Format_Record_Text, tbl_Record.DefaultView[i]["Introduction"].ToString(), tbl_Record.DefaultView[i]["Content"].ToString()));
                        }
                        else
                        {
                            mBuilder_Parent.Append(string.Format(Format_Record_Download, tbl_Record.DefaultView[i]["Introduction"].ToString(), tbl_Record.DefaultView[i]["ServiceID"].ToString(), tbl_Record.DefaultView[i]["Keyword"].ToString()));
                        }
                    }

                    tbl_Record.DefaultView.RowFilter = "LetterID = " + mRow_Letter["LetterID"].ToString() + " AND IsParent = 0";
                    for (int i = 0; i < tbl_Record.DefaultView.Count; i++)
                    {
                        if ((Record.RecordType)(int)tbl_Record.DefaultView[i]["RecordTypeID"] == Record.RecordType.SMSText)
                        {
                            mBuilder_Child.Append(string.Format(Format_Record_Text, tbl_Record.DefaultView[i]["Introduction"].ToString(), tbl_Record.DefaultView[i]["Content"].ToString()));
                        }
                        else
                        {
                            mBuilder_Child.Append(string.Format(Format_Record_Download, tbl_Record.DefaultView[i]["Introduction"].ToString(), tbl_Record.DefaultView[i]["ServiceID"].ToString(), tbl_Record.DefaultView[i]["Keyword"].ToString()));
                        }
                    }

                    mBuilder.Append(string.Format(Format_Letter,
                                                    index++.ToString(),
                                                    mRow_Letter["LetterName"].ToString(),
                                                    ((DateTime)mRow_Letter["PushTime"]).ToString(MyConfig.LongDateFormat),
                                                    mBuilder_Parent.ToString(),
                                                    mBuilder_Child.ToString()));
                }

                context.Response.Write(string.Format(Format_News, mBuilder.ToString()));
            }
            catch (Exception ex)
            {
                context.Response.Write("<div class='InsertLetter_Error'>Có lỗi trong quá trình tải dữ liệu!</div>");
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
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
