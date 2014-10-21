using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyUtility;
namespace MyAdmin.Admin
{
    public partial class Ad_Alert : System.Web.UI.Page
    {
        public string str_Alert = string.Empty;
        public int iAlertType = 0;

       public enum AlertType
        {
            NotAccessRule = 1,
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            ((MyAdmin.MasterPages.Admin)Page.Master).ShowToolBox = false;
            ((MyAdmin.MasterPages.Admin)Page.Master).str_TitleSearchBox = "Thông báo:";

            if (Request.QueryString["ID"] != null)
            {
                int.TryParse(Request.QueryString["ID"], out iAlertType);
            }

            switch ((AlertType)iAlertType)
            {
                case AlertType.NotAccessRule:
                    str_Alert = "HIỆN TẠI BẠN KHÔNG CÓ QUYỀN TRUY CẬP VÀO TRANG NÀY, XIN HÃY LIÊN HỆ VỚI ADMIN!";
                    break;
                default:
                    str_Alert = "HIỆN TẠI BẠN KHÔNG CÓ QUYỀN TRUY CẬP VÀO TRANG NÀY, XIN HÃY LIÊN HỆ VỚI ADMIN!";
                    break;
            }
        }
    }
}
