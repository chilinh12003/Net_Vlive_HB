using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;
using MyVlive;

namespace MyAdmin.Admin_CCare
{
    public partial class Ad_CustomerCare : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;
      
        private bool CheckPermission()
        {
            try
            {
                if (mGetRole.ViewRole == false)
                {
                    Response.Redirect(mGetRole.URLNotView, false);
                    return false;
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.CheckPermissionError, "Chilinh");
                return false;
            }
            return true;
        }

        private void BindCombo(int type)
        {
            try
            {
                switch (type)
                {
                    case 1: //Bind dữ liệu về giờ
                        sel_StartHour.DataSource = sel_StopHour.DataSource = MyEnum.GetDataFromTime(3, string.Empty, string.Empty);
                        sel_StartHour.DataValueField = sel_StopHour.DataValueField = "ID";
                        sel_StartHour.DataTextField = sel_StopHour.DataTextField = "Text";
                        sel_StartHour.DataBind();
                        sel_StopHour.DataBind();
                        //sel_StartHour.Items.Insert(0, new ListItem("--Giờ--", "-1"));
                        //sel_StopHour.Items.Insert(0, new ListItem("--Giờ--", "-1"));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            bool IsRedirect = false;
            try
            {
                //Phân quyền
                if (ViewState["Role"] == null)
                {
                    mGetRole = new GetRole(LocalConfig.ListPage.CustomerCare, Member.MemberGroupID());
                }
                else
                {
                    mGetRole = (GetRole)ViewState["Role"];
                }

                if (!CheckPermission())
                {
                    IsRedirect = true;
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
            if (IsRedirect)
            {
                Response.End();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = MyEnum.StringValueOf(LocalConfig.ListPage.CustomerCare);

                mMaster.ShowToolBox = false;
                mMaster.ShowSearchBox = true;

                if (!IsPostBack)
                {
                    BindCombo(1);
                    tbx_StartDate.Value = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
                    tbx_StopDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }
    }
}
