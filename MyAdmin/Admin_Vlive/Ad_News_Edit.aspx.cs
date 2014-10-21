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
using MyVlive.Vlive;

namespace MyAdmin.Admin_Vlive
{
    public partial class Ad_News_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        News mNews = new News();

        public int EditID = 1;
        DataSet mSet = new DataSet();
        public string ParentPath = "../Admin_Vlive/Ad_News.aspx";

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
                        ddl_CateID_1.DataValueField = "CateID_1";
                        ddl_CateID_1.DataBind();
                        ddl_CateID_1.SelectedIndex = ddl_CateID_1.Items.IndexOf(ddl_CateID_1.Items.FindByValue(News.News_CateID_1.ToString()));
                        ddl_CateID_1.Enabled = false;
                        break;
                    case 2: //Thể loại mức 2
                        ddl_CateID_2.DataSource = mCate.Select(3, ddl_CateID_1.SelectedValue);
                        ddl_CateID_2.DataTextField = "CateName";
                        ddl_CateID_2.DataValueField = "CateID_2";
                        ddl_CateID_2.DataBind();
                        ddl_CateID_2.Items.Insert(0, new ListItem("--Thể loại mức 2--", "0"));
                        break;
                    case 3: //Thể loại mức 3
                        sel_CateID_3.DataSource = mCate.Select(4, ddl_CateID_2.SelectedValue);
                        sel_CateID_3.DataTextField = "CateName";
                        sel_CateID_3.DataValueField = "CateID_3";
                        sel_CateID_3.DataBind();
                        sel_CateID_3.Items.Insert(0, new ListItem("--Thể loại mức 3--", "0"));
                        break;
                    case 4: // Bind dữ liệu về giờ
                        sel_StartHour.DataSource = MyEnum.GetDataFromTime(3, string.Empty, string.Empty);
                        sel_StartHour.DataValueField = "ID";
                        sel_StartHour.DataTextField = "Text";
                        sel_StartHour.DataBind();
                        sel_StartHour.Items.Insert(0, new ListItem("--Giờ--", "0"));

                        sel_StartMinute.DataSource = MyEnum.GetDataFromTime(4, string.Empty, string.Empty);
                        sel_StartMinute.DataValueField = "ID";
                        sel_StartMinute.DataTextField = "Text";
                        sel_StartMinute.DataBind();
                        sel_StartMinute.Items.Insert(0, new ListItem("--Phút--", "0"));

                        sel_StartSecond.DataSource = MyEnum.GetDataFromTime(5, string.Empty, string.Empty);
                        sel_StartSecond.DataValueField = "ID";
                        sel_StartSecond.DataTextField = "Text";
                        sel_StartSecond.DataBind();
                        sel_StartSecond.Items.Insert(0, new ListItem("--Giây--", "0"));
                        break;
                }
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
                if (EditID > 0)
                {
                    lbtn_Save.Visible = lbtn_Accept.Visible = mGetRole.EditRole;
                    link_Add.Visible = mGetRole.AddRole;
                }
                else
                {
                    lbtn_Save.Visible = lbtn_Accept.Visible = link_Add.Visible = mGetRole.AddRole;
                }


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

                //Lấy memberID nếu là trước hợp Sửa
                EditID = Request.QueryString["ID"] == null ? 0 : int.Parse(Request.QueryString["ID"]);

                if (!CheckPermission())
                    return;

                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = MyEnum.StringValueOf(LocalConfig.ListPage.News);
                mMaster.str_TitleSearchBox = "Thông tin về " + MyEnum.StringValueOf(LocalConfig.ListPage.News);

                if (!IsPostBack)
                {
                    tbx_StartDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                    BindCombo(1);
                    BindCombo(2);
                    BindCombo(4);

                    //Nếu là Edit
                    if (EditID > 0)
                    {
                        mSet = mNews.SelectDataSet(3, EditID.ToString());

                        mNews.SetDataSet_Session(mSet);

                        //Lưu lại thông tin OldData để lưu vào MemberLog
                        ViewState["OldData"] = mSet.GetXml();

                        if (mSet != null && mSet.Tables.Count > 0)
                        {
                            #region MyRegion
                            DataTable tbl_News = mSet.Tables["News"];
                            DataTable tbl_Letter = mSet.Tables["Letter"];
                            DataTable tbl_Record = mSet.Tables["Record"];

                            DataRow mRow_News = tbl_News.Rows[0];

                            ddl_CateID_1.SelectedIndex = ddl_CateID_1.Items.IndexOf(ddl_CateID_1.Items.FindByValue(mRow_News["CateID_1"].ToString()));

                            if (mRow_News["CateID_2"] != null)
                            {
                                BindCombo(2);
                                ddl_CateID_2.SelectedIndex = ddl_CateID_2.Items.IndexOf(ddl_CateID_2.Items.FindByValue(mRow_News["CateID_2"].ToString()));
                                if (mRow_News["CateID_3"] != null)
                                {
                                    BindCombo(3);
                                    sel_CateID_3.SelectedIndex = sel_CateID_3.Items.IndexOf(sel_CateID_3.Items.FindByValue(mRow_News["CateID_3"].ToString()));
                                }
                            }

                            tbx_NewsName.Value = mRow_News["NewsName"].ToString();

                            #endregion
                        }
                    }

                }
                mSet = mNews.GetDataSet_Session();

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
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

        private void AddNewRow()
        {
            
            MyConvert.ConvertDateColumnToStringColumn(ref mSet);
            DataRow mNewRow = mSet.Tables["News"].NewRow();

            if (EditID > 0 && mSet.Tables["News"].Rows.Count > 0)
                mNewRow = mSet.Tables["News"].Rows[0];

            Category mCate = new Category();
            DataTable tblCate = mCate.Select(10, ddl_CateID_1.SelectedValue, ddl_CateID_2.SelectedValue, sel_CateID_3.Value, string.Empty);
            if (tblCate != null && tblCate.Rows.Count > 0)
            {
                mNewRow["CateID"] = tblCate.Rows[0]["CateID"];
                mNewRow["CateName"] = tblCate.Rows[0]["CateName"];
            }

            if (EditID > 0)
            {
                mNewRow["NewsID"] = EditID;
            }
            else
            {
                mNewRow["CreateDate"] = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);
            }
            
            mNewRow["NewsName"] = string.IsNullOrEmpty(tbx_NewsName.Value) ? tblCate.Rows[0]["CateName"].ToString() + " -- Ngày "+DateTime.Now.ToString("dd-MM-yy HH:mm") : tbx_NewsName.Value;

            mNewRow["IsPublish"] = false;
            mNewRow["IsCreateXML"] = false;
            mNewRow["IsComplete"] = true;

            if (!(EditID > 0))
                mSet.Tables["News"].Rows.Add(mNewRow);

            mSet.AcceptChanges();
        }

        private void Save(bool IsApply)
        {
            try
            {
                AddNewRow();

                if (SaveVlive(mSet))
                {

                    mNews.SetDataSet_Session(null);
                    if (IsApply)
                        MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                    else
                    {
                        Response.Redirect(ParentPath, false);
                    }
                }
                else
                {
                    if (IsApply)
                        MyMessage.ShowMessage("Cập nhật dữ liệu KHÔNG thành công.");
                    else
                    {
                        Response.Redirect(ParentPath, false);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SaveVlive(DataSet mSet_Save)
        {
            try
            {

                //Nếu là Edit
                if (EditID > 0)
                {
                    if (mNews.Update_Vlive(0, mSet_Save.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Update;
                        mLog.Insert("News", ViewState["OldData"].ToString(), mSet_Save.GetXml(), Action, true, string.Empty);
                        #endregion
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (mNews.Insert_Vlive(0, mSet_Save.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                        mLog.Insert("News", string.Empty, mSet_Save.GetXml(), Action, true, string.Empty);
                        #endregion
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
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

        protected void btn_DeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                mNews.SetDataSet_Session(null);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.DeleteDataError, "Chilinh");
            }
        }
    }
}
