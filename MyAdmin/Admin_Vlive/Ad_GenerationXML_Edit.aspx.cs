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
    public partial class Ad_GenerationXML_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;
        public string ParentPath = "../Admin_Vlive/Ad_GenerationXML.aspx";
        DataSet mSet = new DataSet("Parent");
        News mNews = new News();
        GenerationXML mGen = new GenerationXML();
        private void BindCombo(int type)
        {
            try
            {
                Category mCate = new Category();
                switch (type)
                {
                    case 1: //Thể loại mức 1
                        ddl_CateID_1.DataSource = mCate.Select(2, null);
                        ddl_CateID_1.DataTextField = "CateName";
                        ddl_CateID_1.DataValueField = "CateID";
                        ddl_CateID_1.DataBind();
                        ddl_CateID_1.SelectedIndex = ddl_CateID_1.Items.IndexOf(ddl_CateID_1.Items.FindByValue(News.News_CateID.ToString()));
                        ddl_CateID_1.Enabled = false;
                        break;
                    case 2: //Thể loại mức 2

                        ddl_CateID_2.DataSource = mCate.Select(6, ddl_CateID_1.SelectedValue);
                        ddl_CateID_2.DataTextField = "CateName";
                        ddl_CateID_2.DataValueField = "CateID";
                        ddl_CateID_2.DataBind();
                        ddl_CateID_2.Items.Insert(0, new ListItem("--Thể loại mức 2--", "0"));
                        break;
                    case 3: //Thể loại mức 3
                        sel_CateID_3.DataSource = mCate.Select(6, ddl_CateID_2.SelectedValue);
                        sel_CateID_3.DataTextField = "CateName";
                        sel_CateID_3.DataValueField = "CateID";
                        sel_CateID_3.DataBind();
                        sel_CateID_3.Items.Insert(0, new ListItem("--Thể loại mức 3--", "0"));
                        break;
                    case 4: // Bind dữ liệu về giờ
                        sel_Hour.DataSource = MyEnum.GetDataFromTime(3, string.Empty, string.Empty);
                        sel_Hour.DataValueField = "ID";
                        sel_Hour.DataTextField =  "Text";
                        sel_Hour.DataBind();
                        sel_Hour.Items.Insert(0, new ListItem("--Giờ--", "-1"));
                        break;
                    case 5: // Bind dữ liệu về Phút
                        sel_Minute.DataSource = MyEnum.GetDataFromTime(4, string.Empty, string.Empty);
                        sel_Minute.DataValueField = "ID";
                        sel_Minute.DataTextField = "Text";
                        sel_Minute.DataBind();
                        sel_Minute.Items.Insert(0, new ListItem("--Phút--", "0"));
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

        private void BindData_Grid()
        {
            try
            {
                rpt_Gen.DataSource = mSet.Tables[0];
                rpt_Gen.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                //link_Edit.Visible = mGetRole.EditRole;
                //lbtn_Active.Visible = mGetRole.PublishRole;
                //lbtn_UnActive.Visible = mGetRole.PublishRole;
                //lbtn_Delete.Visible = mGetRole.DeleteRole;

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
                    mGetRole = new GetRole(LocalConfig.ListPage.GenerationXML, Member.MemberGroupID());
                }
                else
                {
                    mGetRole = (GetRole)ViewState["Role"];
                }
                if (!CheckPermission())
                    return;
                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = MyEnum.StringValueOf(LocalConfig.ListPage.GenerationXML);

                if (!IsPostBack)
                {
                    ViewState["SortBy"] = string.Empty;
                    BindCombo(1);
                    BindCombo(2);
                    BindCombo(4);
                    BindCombo(5);
                    tbx_Day.Value = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

                    mSet = mGen.CreateDataSet();
                    ViewState["Data"] = mSet;

                    BindData_Grid();
                }

                mSet = (DataSet)ViewState["Data"];

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
                bool? IsActive = true;
                int? CateID = null;
                string SortBy = ViewState["SortBy"].ToString();
                if (ddl_CateID_1.SelectedIndex >= 0 && ddl_CateID_1.Items.Count > 0)
                {
                    CateID = int.Parse(ddl_CateID_1.SelectedValue);
                }
                if (btn_CancelSearch.Visible)
                {
                    SearchType = int.Parse(sel_SearchType.Value);

                    if (ddl_CateID_2.SelectedValue != "0")
                    {
                        CateID = int.Parse(ddl_CateID_2.SelectedValue);
                    }
                    if (sel_CateID_3.Value != "0")
                    {
                        CateID = int.Parse(sel_CateID_3.Value);
                    }
                    str_SearchContent = tbx_Search.Value.Length < 1 ? null : MyText.ValidSearchContent(tbx_Search.Value);
                    
                }
                return mNews.TotalRow(SearchType, str_SearchContent, CateID, IsActive, false, false);
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
                bool? IsActive = true;
                int? CateID = null;
                string SortBy = ViewState["SortBy"].ToString();
                CateID = int.Parse(ddl_CateID_1.SelectedValue);

                if (btn_CancelSearch.Visible)
                {
                    SearchType = int.Parse(sel_SearchType.Value);

                    if (ddl_CateID_2.SelectedValue != "0")
                    {
                        CateID = int.Parse(ddl_CateID_2.SelectedValue);
                    }
                    if (sel_CateID_3.Value != "0")
                    {
                        CateID = int.Parse(sel_CateID_3.Value);
                    }
                    str_SearchContent = tbx_Search.Value.Length < 1 ? null : MyText.ValidSearchContent(tbx_Search.Value);
                }

                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

                return mNews.Search(SearchType, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, str_SearchContent, CateID, IsActive, false,false,SortBy);
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

        protected void lbtn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Save(false);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
        }

        protected void lbtn_Apply_Click(object sender, EventArgs e)
        {
            try
            {
                Save(true);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
        }

        private void Save(bool IsApply)
        {
            try
            {
                if (mSet == null || mSet.Tables.Count < 1 || mSet.Tables[0].Rows.Count < 1)
                {
                    MyMessage.ShowError("Xin hãy nhập liệu trước khi lưu.");
                }

                MyConvert.ConvertDateColumnToStringColumn(ref mSet);
                if (mGen.Insert(0, mSet.GetXml()))
                {
                    if (IsApply)
                    {
                        MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                        mSet = mGen.CreateDataSet();
                        ViewState["Data"] = mSet;
                        BindData_Grid();
                    }
                    else
                    {
                        Response.Redirect(ParentPath, false);
                        return;
                    }
                }
                else
                {
                    MyMessage.ShowMessage("Cập nhật dữ liệu KHÔNG thành công.");
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void AddRowToDataSet(DataTable mTable_News, DateTime GenDate)
        {
            try
            {
                if (mSet == null)
                    mSet = mGen.CreateDataSet();

                DataRow mNewRow = mSet.Tables[0].NewRow();
              
                foreach (DataRow mRow in mTable_News.Rows)
                {
                    if (mSet.Tables[0].Select("NewsID = " + mRow["NewsID"].ToString()).Length > 0)
                        continue;

                    mNewRow = mSet.Tables[0].NewRow();
                    mNewRow["NewsID"] = mRow["NewsID"];
                    mNewRow["NewsName"] = mRow["NewsName"];
                    mNewRow["CateID"] = mRow["CateID"];
                    mNewRow["CateName"] = mRow["CateName"];
                    mNewRow["GenDate"] = GenDate;
                    mNewRow["Createdate"] = DateTime.Now;
                    mNewRow["Note"] = string.Empty;
                    mNewRow["StatusID"] = (int)GenerationXML.Status.NotCreateXML;
                    mNewRow["StatusName"] = MyEnum.StringValueOf(GenerationXML.Status.NotCreateXML);
                    mNewRow["IsActive"] = true;

                    mSet.Tables[0].Rows.Add(mNewRow);
                }

                ViewState["Data"] = mSet;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btn_Save_Temp_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbx_Day.Value))
                {
                    MyMessage.ShowError("Xin hãy chọn ngày cần xuất bản tin XML.");
                    return;
                }

                string ListID = hid_ListCheckAll.Value.Trim();

                if (string.IsNullOrEmpty(ListID))
                {
                    MyMessage.ShowError("Xin hãy chọn ít nhất 1 bản tin.");
                    return;
                }

                ListID = ListID.Replace("|", ",");
                DataTable mTable = mNews.Select(4, ListID);
                if (mTable == null || mTable.Rows.Count < 1)
                {
                    MyMessage.ShowError("Không có bản tin nào phù hợp trong danh sách bản tin.");
                    return;
                }


                DateTime GenDate = DateTime.MinValue;
                if (tbx_Day.Value.Length > 0)
                {
                    int Hour = 0;
                    int Minute = 0;
                    int Second = 0;
                    DateTime TempDate = DateTime.ParseExact(tbx_Day.Value, "dd/MM/yyyy", null);

                    if (sel_Hour.SelectedIndex > 0)
                        int.TryParse(sel_Hour.Value, out Hour);
                    if (sel_Minute.SelectedIndex > 0)
                        int.TryParse(sel_Minute.Value, out Minute);

                    GenDate = new DateTime(TempDate.Year, TempDate.Month, TempDate.Day, Hour, Minute, Second);
                    if (GenDate < DateTime.Now.AddMinutes(30))
                    {
                        MyMessage.ShowError("Thời gian hẹn giờ phải lớn hơn thời gian hiện tại 30 phút");
                        return;
                    }
                }
                if (GenDate == DateTime.MinValue)
                {
                    MyMessage.ShowError("Ngày tháng bạn chọn là không chính xác.");
                    return;
                }

                AddRowToDataSet(mTable, GenDate);

                BindData_Grid();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
        }

        protected void btn_Delete_Item_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn_Delete_Item = (Button)sender;
                if (btn_Delete_Item == null)
                {
                    MyMessage.ShowError("Xóa không thành công.");
                    return;
                }
                string NewsID = btn_Delete_Item.CommandArgument;
                if (string.IsNullOrEmpty(NewsID))
                {
                    MyMessage.ShowError("Không tìm thấy bản tin cần xóa");
                    return;
                }

                mSet.Tables[0].DefaultView.RowFilter = "NewsID = " + NewsID;
                if (mSet.Tables[0].DefaultView.Count < 1)
                {
                    MyMessage.ShowError("Không tìm thấy bản tin cần xóa");
                    return;
                }
                mSet.Tables[0].DefaultView[0].Delete();

                mSet.Tables[0].DefaultView.RowFilter = string.Empty;

                ViewState["Data"] = mSet;
                BindData_Grid();

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.DeleteDataError, "Chilinh");
            }
            finally
            {
                mSet.Tables[0].DefaultView.RowFilter = string.Empty;
                ViewState["Data"] = mSet;
                BindData_Grid();
            }
        }

        protected void ddl_CateID_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCombo(2);
                BindCombo(3);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

        protected void ddl_CateID_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCombo(3);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

    }
}
