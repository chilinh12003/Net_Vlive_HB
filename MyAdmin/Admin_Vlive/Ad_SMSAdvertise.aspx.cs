using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;
using MyVlive.Vlive;
using MyVlive;
using System.Text;
using System.IO;

namespace MyAdmin.Admin_Vlive
{
    public partial class Ad_SMSAdvertise : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;

        SMSAdvertise mSMSAdvertise = new SMSAdvertise();

        private void BindCombo(int type)
        {
            try
            {
                switch (type)
                {
                    case 1: //Thể loại mức 1
                        sel_SMSType.DataSource = MyEnum.CrateDatasourceFromEnum(typeof(SMSAdvertise.SMSTypeID));
                        sel_SMSType.DataTextField = "Text";
                        sel_SMSType.DataValueField = "ID";
                        sel_SMSType.DataBind();
                        sel_SMSType.Items.Insert(0, new ListItem("--Loại SMS--", "0"));
                        break;
                    case 2: //Thể loại mức 1
                        sel_MediaType.DataSource = MyEnum.CrateDatasourceFromEnum(typeof(SMSAdvertise.MediaType));
                        sel_MediaType.DataTextField = "Text";
                        sel_MediaType.DataValueField = "ID";
                        sel_MediaType.DataBind();
                        sel_MediaType.Items.Insert(0, new ListItem("--Loại nội dung--", "0"));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindData()
        {
            Admin_Paging1.ResetLoadData();
        }

        private bool CheckPermission()
        {
            try
            {
                if (mGetRole.ViewRole == false)
                {
                    Response.Redirect(mGetRole.URLNotView, false);
                    return false;
                }

                link_Add.Visible = mGetRole.AddRole;
                link_Edit.Visible = mGetRole.EditRole;
                lbtn_Active.Visible = mGetRole.ActiveRole;
                lbtn_UnActive.Visible = mGetRole.ActiveRole;
                lbtn_Delete.Visible = mGetRole.DeleteRole;

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.CheckPermissionError, "Chilinh");
                return false;
            }
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //Phân quyền
                if (ViewState["Role"] == null)
                {
                    mGetRole = new GetRole(LocalConfig.ListPage.SMSAdvertise, Member.MemberGroupID());
                }
                else
                {
                    mGetRole = (GetRole)ViewState["Role"];
                }
                if (!CheckPermission())
                    return;
                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = MyEnum.StringValueOf(LocalConfig.ListPage.SMSAdvertise);

                if (!IsPostBack)
                {
                    ViewState["SortBy"] = string.Empty;
                    BindCombo(1);
                    BindCombo(2);
                }

                Admin_Paging1.rpt_Data = rpt_Data;
                Admin_Paging1.GetData_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetData_Callback(Admin_Paging1_GetData_Callback_Change);
                Admin_Paging1.GetTotalPage_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetTotalPage_Callback(Admin_Paging1_GetTotalPage_Callback_Change);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

        int Admin_Paging1_GetTotalPage_Callback_Change()
        {
            try
            {
                int? SearchType = null;
                string str_SearchContent = null;
                bool? IsActive = null;                
                int? SMSTypeID = null;
                int? MediaTypeID = null;

                string SortBy = ViewState["SortBy"].ToString();
               
                if (btn_CancelSearch.Visible)
                {
                    SearchType = int.Parse(sel_SearchType.Value);

                    if (sel_SMSType.Value != "0")
                    {
                        SMSTypeID = int.Parse(sel_SMSType.Value);
                    }
                    if (sel_MediaType.Value != "0")
                    {
                        MediaTypeID = int.Parse(sel_MediaType.Value);
                    }
                    str_SearchContent = tbx_Search.Value.Length < 1 ? null : MyText.ValidSearchContent(tbx_Search.Value);


                    if (rad_Active.Checked)
                        IsActive = true;
                    if (rad_UnActive.Checked)
                        IsActive = false;
                }
                return mSMSAdvertise.TotalRow(SearchType, str_SearchContent, SMSTypeID,MediaTypeID, IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging1_GetData_Callback_Change()
        {
            try
            {
                int? SearchType = null;
                string str_SearchContent = null;
                bool? IsActive = null;
                int? SMSTypeID = null;
                int? MediaTypeID = null;

                string SortBy = ViewState["SortBy"].ToString();

                if (btn_CancelSearch.Visible)
                {
                    SearchType = int.Parse(sel_SearchType.Value);

                    if (sel_SMSType.Value != "0")
                    {
                        SMSTypeID = int.Parse(sel_SMSType.Value);
                    }
                    if (sel_MediaType.Value != "0")
                    {
                        MediaTypeID = int.Parse(sel_MediaType.Value);
                    }
                    str_SearchContent = tbx_Search.Value.Length < 1 ? null : MyText.ValidSearchContent(tbx_Search.Value);


                    if (rad_Active.Checked)
                        IsActive = true;
                    if (rad_UnActive.Checked)
                        IsActive = false;
                }

                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

                return mSMSAdvertise.Search(SearchType, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, str_SearchContent, SMSTypeID,MediaTypeID, IsActive, SortBy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtn_Sort_Click(object sender, EventArgs e)
        {
            try
            {
                lbtn_Sort_1.CssClass = "Sort";
                lbtn_Sort_2.CssClass = "Sort";
                lbtn_Sort_3.CssClass = "Sort";
                //lbtn_Sort_4.CssClass = "Sort";
                //lbtn_Sort_5.CssClass = "Sort";
                lbtn_Sort_6.CssClass = "Sort";

                LinkButton mLinkButton = (LinkButton)sender;
                ViewState["SortBy"] = mLinkButton.CommandArgument;

                if (mLinkButton.CommandArgument.IndexOf(" ASC") >= 0)
                {
                    mLinkButton.CssClass = "SortActive_Up";
                    mLinkButton.CommandArgument = mLinkButton.CommandArgument.Replace(" ASC", " DESC");
                }
                else
                {
                    mLinkButton.CssClass = "SortActive_Down";
                    mLinkButton.CommandArgument = mLinkButton.CommandArgument.Replace(" DESC", " ASC");
                }

                BindData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SortError, "Chilinh");
            }
        }

        protected void lbtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                char[] key_1 = { '|' };

                string[] arr_1 = hid_ListCheckAll.Value.Split(key_1);

                DataSet dds_Parent = new DataSet("Parent");
                DataTable tbl_Child = new DataTable("Child");
                DataColumn col_1 = new DataColumn("ID", typeof(int));
                tbl_Child.Columns.Add(col_1);

                for (int i = 0; i < arr_1.Length; i++)
                {
                    DataRow mRow = tbl_Child.NewRow();

                    mRow["ID"] = int.Parse(arr_1[i]);

                    tbl_Child.Rows.Add(mRow);
                }
                tbl_Child.AcceptChanges();

                dds_Parent.Tables.Add(tbl_Child);
                dds_Parent.AcceptChanges();

                if (mSMSAdvertise.Delete(0, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Delete;
                    mLog.Insert("SMSAdvertise", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
                    #endregion

                    MyMessage.ShowMessage("Xóa dữ liệu thành công.");
                    BindData();
                }
                else
                {
                    MyMessage.ShowMessage("Xóa dữ liệu KHÔNG thành công!");
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.DeleteDataError, "Chilinh");
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                btn_CancelSearch.Visible = true;
                BindData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SeachError, "Chilinh");
            }
        }

        protected void btn_CancelSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btn_CancelSearch.Visible = false;
                BindData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SeachError, "Chilinh");
            }
        }

        private void Active(bool IsActive)
        {
            try
            {
                char[] key_1 = { '|' };

                string[] arr_1 = hid_ListCheckAll.Value.Split(key_1);


                DataSet dds_Parent = new DataSet("Parent");
                DataTable tbl_Child = new DataTable("Child");
                DataColumn col_1 = new DataColumn("ID", typeof(int));
                tbl_Child.Columns.Add(col_1);

                for (int i = 0; i < arr_1.Length; i++)
                {
                    DataRow mRow = tbl_Child.NewRow();

                    mRow["ID"] = int.Parse(arr_1[i]);

                    tbl_Child.Rows.Add(mRow);
                }
                tbl_Child.AcceptChanges();

                dds_Parent.Tables.Add(tbl_Child);
                dds_Parent.AcceptChanges();

                if (mSMSAdvertise.Active(0, IsActive, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = IsActive ? MemberLog.ActionType.Active : MemberLog.ActionType.InActive;
                    mLog.Insert("SMSAdvertise", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
                    #endregion
                    MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                    BindData();
                }
                else
                {
                    MyMessage.ShowMessage("Cập nhật dữ liệu KHÔNG thành công!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtn_Active_Click(object sender, EventArgs e)
        {

            try
            {
                Active(true);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.ActiveError, "Chilinh");
            }
        }

        protected void lbtn_UnActive_Click(object sender, EventArgs e)
        {
            try
            {
                Active(false);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.ActiveError, "Chilinh");
            }
        }

    }
}
