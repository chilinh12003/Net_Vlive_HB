using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MyVlive;
using MyUtility;
namespace MyAdmin.Admin
{
    public partial class Ad_Login : System.Web.UI.Page
    {
        string PrevURL = string.Empty;
        public string AlertMessage = "Xin hãy kiểm tra lại Tăng đăng nhập hoặc Mật khẩu.";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Member.IsLogined())
            {
                Response.Redirect(MyConfig.URLNotice + "?ID=" + ((int)MyAdmin.Admin.Ad_Alert.AlertType.NotAccessRule).ToString(), false);
                return;
            }
            try
            {
                PrevURL = HttpContext.Current.Server.UrlDecode(Request.QueryString["PrevURL"]);
               
            }
            catch
            {
            }
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Remove("Menu_1");
                Session.Remove("Menu_2");

                if (Member.Login(tbx_LoginName.Value, tbx_Password.Value, false, true, true, ref AlertMessage))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Login;
                    mLog.Insert("Member", string.Empty, string.Empty, Action, true, tbx_LoginName.Value);
                    #endregion

                    if (!string.IsNullOrEmpty(PrevURL))
                        Response.Redirect(PrevURL, false);
                    else
                        Response.Redirect(MyConfig.URLLoginSuccess, false);
                }
                else
                {
                    div_Loi.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoginError, "Chilinh");
            }
        }
    }
}
