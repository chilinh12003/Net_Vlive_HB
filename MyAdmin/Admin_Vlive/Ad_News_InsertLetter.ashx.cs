using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Data;
using MyUtility;
using MyVlive;
using MyVlive.Vlive;

namespace MyAdmin.Admin_Vlive
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class Ad_News_InsertLetter : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            try
            {
                int RecordID = 0;
                if (context.Request.QueryString["RecordID"] != null)
                    int.TryParse(context.Request.QueryString["RecordID"], out RecordID);

                string strFormat = MyUtility.MyFile.ReadFile("~/Templates/InsertLetter.htm");
                MyVlive.Vlive.News mNews = new News();

                DataSet mSet = mNews.GetDataSet_Session();
                DataTable tbl_Record = mSet.Tables["Record"];

                tbl_Record.DefaultView.RowFilter = " RecordID =" + RecordID.ToString();

                string RecordTypeSMS = string.Empty;
                string RecordTypeDownload = string.Empty;
                string Method_SMSDD = string.Empty;
                string Method_SMS = string.Empty;
                string Introduction = string.Empty;
                string Content = string.Empty;
                string Keyword = string.Empty;

                string ServiceID_9093= string.Empty;
                string ServiceID_9193 = string.Empty;
                string ServiceID_9293 = string.Empty;
                string ServiceID_9393 = string.Empty;
                string ServiceID_9493 = string.Empty;
                string ServiceID_9593 = string.Empty;
                string ServiceID_9693 = string.Empty;
                string ServiceID_9793 = string.Empty;
                string ServiceID_9194 = string.Empty;


                string Priority = string.Empty;

                if (tbl_Record.DefaultView.Count > 0)
                {
                    DataRowView mView = tbl_Record.DefaultView[0];
                    if (Record.RecordType.SMSText == (Record.RecordType)int.Parse(mView["RecordTypeID"].ToString()))
                    {
                        RecordTypeSMS = "selected=\"selected\"";
                    }
                    else
                        RecordTypeDownload = "selected=\"selected\"";

                    if (Record.Method.sms == (Record.Method)int.Parse(mView["MethodID"].ToString()))
                    {
                        Method_SMS = "selected=\"selected\"";
                    }
                    else
                    {
                        Method_SMSDD = "selected=\"selected\"";
                    }

                    Content = mView["Content"].ToString();
                    Keyword = mView["Keyword"].ToString();
                    Introduction = mView["Introduction"].ToString();

                    if (mView["ServiceID"].ToString() == "9093")
                        ServiceID_9093 = "selected=\"selected\"";
                    if (mView["ServiceID"].ToString() == "9193")
                        ServiceID_9193 = "selected=\"selected\"";
                    if (mView["ServiceID"].ToString() == "9293")
                        ServiceID_9293 = "selected=\"selected\"";
                    if (mView["ServiceID"].ToString() == "9393")
                        ServiceID_9393 = "selected=\"selected\"";
                    if (mView["ServiceID"].ToString() == "9493")
                        ServiceID_9493 = "selected=\"selected\"";
                    if (mView["ServiceID"].ToString() == "9593")
                        ServiceID_9593 = "selected=\"selected\"";
                    if (mView["ServiceID"].ToString() == "9693")
                        ServiceID_9693 = "selected=\"selected\"";
                    if (mView["ServiceID"].ToString() == "9793")
                        ServiceID_9793 = "selected=\"selected\"";

                    if (mView["ServiceID"].ToString() == "9194")
                        ServiceID_9194 = "selected=\"selected\"";
                    Priority = mView["Priority"].ToString();
                }

                string[] arr = { RecordTypeSMS, RecordTypeDownload, Method_SMSDD, Method_SMS, Introduction, Content, 
                                   Keyword, ServiceID_9093, ServiceID_9193, ServiceID_9293, ServiceID_9393,
                                   ServiceID_9493, ServiceID_9593, ServiceID_9693, ServiceID_9793,ServiceID_9194,Priority,RecordID.ToString() };
               
                string HTML = string.Format(strFormat, arr);



                context.Response.Write(HTML);
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
