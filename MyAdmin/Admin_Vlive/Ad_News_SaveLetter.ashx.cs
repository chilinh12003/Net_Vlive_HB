using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Text;
using MyUtility;
using MyVlive;
using MyVlive.Vlive;
using System.Web.UI;
using System.Data;

namespace MyAdmin.Admin_Vlive
{
    public class Ad_News_SaveLetter : IHttpHandler, IRequiresSessionState
    {
        int LetterID = 0;
        string LetterName = string.Empty;
        int Priority = 0;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            try
            {
                LetterID = context.Request.QueryString["LetterID"] == null ? 0 : int.Parse(context.Request.QueryString["LetterID"]);
                DataTable mTable = new DataTable();
                DataSet mSet = MyXML.GetDataSetFromXMLString(context.Session["Vlive"].ToString());
                mTable = mSet.Tables["Letter"];
                for (int i = 0; i < mTable.Rows.Count; i++)
                {
                    if (int.Parse(mTable.Rows[i]["LetterID"].ToString())==LetterID)
                    {
                        LetterName = mTable.Rows[i]["LetterName"].ToString();
                        Priority = int.Parse(mTable.Rows[i]["Priority"].ToString());
                    }
                }
                string strFormat = MyUtility.MyFile.ReadFile("~/Templates/SaveLetter.htm");

                StringBuilder mBuilder = new StringBuilder();
                mBuilder.Append(string.Format(strFormat,LetterID,LetterName,Priority));

                context.Response.Write(mBuilder.ToString());
            }
            catch (Exception ex)
            {
                context.Response.Write("<div class='SaveLetter_Error'>Có lỗi trong quá trình tải dữ liệu!</div>");
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
