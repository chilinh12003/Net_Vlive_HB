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
    public partial class Ad_News : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;

        News mNews = new News();

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
                lbtn_Active.Visible = mGetRole.PublishRole;
                lbtn_UnActive.Visible = mGetRole.PublishRole;
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
                    mGetRole = new GetRole(LocalConfig.ListPage.News, Member.MemberGroupID());
                }
                else
                {
                    mGetRole = (GetRole)ViewState["Role"];
                }
                if (!CheckPermission())
                    return;
                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = MyEnum.StringValueOf(LocalConfig.ListPage.News);

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


                    if (rad_Active.Checked)
                        IsActive = true;
                    if (rad_UnActive.Checked)
                        IsActive = false;
                }
                return mNews.TotalRow(SearchType, str_SearchContent, CateID, IsActive);
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


                    if (rad_Active.Checked)
                        IsActive = true;
                    if (rad_UnActive.Checked)
                        IsActive = false;
                }

                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

                return mNews.Search(SearchType, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, str_SearchContent, CateID, IsActive, SortBy);
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

                if (mNews.Delete(0, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Delete;
                    mLog.Insert("News", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
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

        private void Publish(bool IsPublish)
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

                if (mNews.Publish(0, IsPublish, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = IsPublish ? MemberLog.ActionType.Publish : MemberLog.ActionType.UnPublish;
                    mLog.Insert("News", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
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
                Publish(true);
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
                Publish(false);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.ActiveError, "Chilinh");
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

        protected void lbtn_CreateXML_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn_CreateXML = (LinkButton)sender;

                #region MyRegion
                string Format_Vlive = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/XML/Vlive.txt"));
                string Format_BeforeTheClickMessage = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/XML/BeforeTheClickMessage.txt"));
                string Format_Menu = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/XML/Menu.txt"));
                string Format_MenuItem_Text = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/XML/MenuItem_Text.txt"));
                string Format_MenuItem_Download = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/XML/MenuItem_Download.txt"));
                
                int CateID = 0;
                string NewsID = lbtn_CreateXML.CommandArgument;

                string Vlive = mNews.GenerationXMLContent(ref CateID, NewsID, Format_Vlive, Format_BeforeTheClickMessage, Format_Menu, Format_MenuItem_Text, Format_MenuItem_Download);
                if (string.IsNullOrEmpty(Vlive))
                {
                    MyMessage.ShowError("Tạo XML không thành công!");
                    return;
                }
                string ConfigFileName = News.GetFileNameXML(CateID);

                string FileName = MyFile.GetFullPathFile("~/u/" + ConfigFileName + ".xml");
                string FileName_ByDay = MyFile.GetFullPathFile("~/u/" + ConfigFileName +"_"+ DateTime.Now.ToString("dd_MM_yyyy") + "_" + Member.LoginName() + ".xml");
                int Name_Index = 1;

                while(File.Exists(FileName_ByDay))
                {
                    FileName_ByDay = MyFile.GetFullPathFile("~/u/" + ConfigFileName + "_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + Member.LoginName() + "_" + Name_Index++.ToString() + ".xml");
                }

                if (File.Exists(FileName))
                {
                    
                    File.Copy(FileName, FileName_ByDay);
                    File.Delete(FileName);
                }
                StreamWriter mTextWriter;
                mTextWriter = new StreamWriter(FileName, true, System.Text.Encoding.ASCII);   
            
                mTextWriter.WriteLine(Vlive);
              
                mTextWriter.Flush();
                mTextWriter.Close();

                #endregion

                if (!string.IsNullOrEmpty(Vlive))
                {
                    mNews.Update_CreateXML(int.Parse(lbtn_CreateXML.CommandArgument));
                    BindData();
                    MyMessage.ShowMessage("Cập nhật dữ liệu thành công");
                }
                else
                {
                    MyMessage.ShowMessage("Cập nhật dữ liệu không thành công");
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
        }

     

        public string GetFullFileXML(int CateID)
        {
            return "http://" + MyCurrent.GetDomainName() + ":" + MyCurrent.GetURLPort() + "/u/" + News.GetFileNameXML(CateID) + ".xml";
        }
    }
}
