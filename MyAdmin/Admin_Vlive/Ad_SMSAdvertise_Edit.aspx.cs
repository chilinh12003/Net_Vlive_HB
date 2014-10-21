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
    public partial class Ad_SMSAdvertise_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        SMSAdvertise mSMSAdvertise = new SMSAdvertise();

        int EditID = 0;

        public string ParentPath = "../Admin_Vlive/Ad_SMSAdvertise.aspx";

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
                        break;
                    case 2: //Thể loại mức 1
                        sel_MediaType.DataSource = MyEnum.CrateDatasourceFromEnum(typeof(SMSAdvertise.MediaType));
                        sel_MediaType.DataTextField = "Text";
                        sel_MediaType.DataValueField = "ID";
                        sel_MediaType.DataBind();
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
                chk_Active.Disabled = !mGetRole.PublishRole;

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.CheckPermissionError, "Chilinh");
                return false;
            }
            return true;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            bool IsRedirect = false;
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
                //Lấy AdvID nếu là trước hợp Sửa
                EditID = Request.QueryString["ID"] == null ? 0 : int.Parse(Request.QueryString["ID"]);

                if (!CheckPermission())
                    return;

                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_TitleSearchBox = "Thông tin tài khoản:";

                if (!IsPostBack)
                {
                    BindCombo(1);
                    BindCombo(2);

                    //Nếu là Edit
                    if (EditID > 0)
                    {
                        DataTable mTable = mSMSAdvertise.Select(1, EditID.ToString());

                        //Lưu lại thông tin OldData để lưu vào MemberLog
                        ViewState["OldData"] = MyXML.GetXML(mTable);

                        if (mTable != null && mTable.Rows.Count > 0)
                        {
                            #region MyRegion
                            DataRow mRow = mTable.Rows[0];
                   
                            tbx_AdvName.Value = mRow["AdvName"].ToString();
                            tbx_Content.Value = mRow["Content"].ToString();
                            tbx_Wappush.Value = mRow["Wappush"].ToString();
                            tbx_Keyword.Value = mRow["Keyword"].ToString();
                        
                            chk_Active.Checked = (bool)mRow["IsActive"];

                            sel_MediaType.SelectedIndex = sel_MediaType.Items.IndexOf(sel_MediaType.Items.FindByValue(mRow["MediaTypeID"].ToString()));
                            sel_SMSType.SelectedIndex = sel_SMSType.Items.IndexOf(sel_SMSType.Items.FindByValue(mRow["SMSTypeID"].ToString()));
                            sel_ShortCode.SelectedIndex = sel_ShortCode.Items.IndexOf(sel_ShortCode.Items.FindByValue(mRow["ShortCode"].ToString()));


                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }

        }

    
     

        private void AddNewRow(ref DataSet mSet)
        {
            MyConvert.ConvertDateColumnToStringColumn(ref mSet);
            DataRow mNewRow = mSet.Tables["Child"].NewRow();

            if (EditID > 0)
                mNewRow["AdvID"] = EditID;        

            mNewRow["AdvName"] = tbx_AdvName.Value;
            mNewRow["Content"] = tbx_Content.Value;
            mNewRow["Wappush"] = tbx_Wappush.Value;
            mNewRow["Keyword"] = tbx_Keyword.Value;

       
            mNewRow["SMSTypeID"] = int.Parse(sel_SMSType.Value);
            mNewRow["SMSTypeName"] = sel_SMSType.Items[sel_SMSType.SelectedIndex].Text;

            mNewRow["MediaTypeID"] = int.Parse(sel_MediaType.Value);
            mNewRow["MediaTypeName"] = sel_MediaType.Items[sel_MediaType.SelectedIndex].Text;

            mNewRow["ShortCode"] = sel_ShortCode.Value;

            mNewRow["CreateDate"] = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);
            mNewRow["IsActive"] = chk_Active.Checked;

            mSet.Tables["Child"].Rows.Add(mNewRow);
            mSet.AcceptChanges();
        }

        private void Save(bool IsApply)
        {
            try
            {
             

                DataSet mSet = mSMSAdvertise.CreateDataSet();
                AddNewRow(ref mSet);
                //Nếu là Edit
                if (EditID > 0)
                {
                    if (mSMSAdvertise.Update(0, mSet.GetXml()))
                    {
                        #region Log SMSAdvertise
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Update;
                        mLog.Insert("SMSAdvertise", ViewState["OldData"].ToString(), mSet.GetXml(), Action, true, string.Empty);
                        #endregion

                        if (IsApply)
                            MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                        else
                        {
                            Response.Redirect(ParentPath, false);
                        }
                    }
                    else
                    {
                        MyMessage.ShowMessage("Cập nhật dữ liệu (KHÔNG) thành công!");
                    }
                }
                else
                {
                    if (mSMSAdvertise.Insert(0, mSet.GetXml()))
                    {
                        #region Log SMSAdvertise
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                        mLog.Insert("SMSAdvertise", string.Empty, mSet.GetXml(), Action, true, string.Empty);
                        #endregion

                        if (IsApply)
                            MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                        else
                        {
                            Response.Redirect(ParentPath, false);
                        }
                    }
                    else
                    {
                        MyMessage.ShowMessage("Cập nhật dữ liệu (KHÔNG) thành công!");
                    }
                }
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
       
    }
}
