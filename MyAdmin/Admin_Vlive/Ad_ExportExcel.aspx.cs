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
using System.IO;
namespace MyAdmin.Admin_Vlive
{
    public partial class Ad_ExportExcel : System.Web.UI.Page
    {
        public GetRole mGetRole;
        News mNews = new News();
        public int PageIndex = 1;
        public int EditID = 1;

        DateTime ReportDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        string ExportExcel_Path
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("ExportExcel_Path");
                if(string.IsNullOrEmpty(Temp))
                {
                    return "~/u/ExportExcel/";
                }
                else
                {
                    return Temp;
                }

            }
        }

        string ExportExcel_CPName
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("ExportExcel_CPName");
                if(string.IsNullOrEmpty(Temp))
                {
                    return "CPName";
                }
                else
                {
                    return Temp;
                }

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
                    mGetRole = new GetRole(LocalConfig.ListPage.ExportExcel, Member.MemberGroupID());
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
                mMaster.str_PageTitle = MyEnum.StringValueOf(LocalConfig.ListPage.ExportExcel);
                mMaster.str_TitleSearchBox = "Thông tin về " + MyEnum.StringValueOf(LocalConfig.ListPage.ExportExcel);

                if (!IsPostBack)
                {
                    BindData();
                    tbx_ReportDay.Value = ReportDay.ToString("dd/MM/yyyy");
                }
              
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }

        }
        private void BindData()
        {
            try
            {

                rpt_Data.DataSource = GetListFile();
                rpt_Data.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataTable GetListFile()
        {
            try
            {
                if (!Directory.Exists(MyFile.GetFullPathFile(ExportExcel_Path)))
                {
                    Directory.CreateDirectory(MyFile.GetFullPathFile(ExportExcel_Path));
                }

                string[] array2 = Directory.GetFiles(MyFile.GetFullPathFile(ExportExcel_Path));

                DataTable mTable = new DataTable();
                DataColumn col_Name = new DataColumn("FileName", typeof(string));
                DataColumn col_CreateDate = new DataColumn("CreateDate", typeof(DateTime));
                DataColumn col_DownloadLink = new DataColumn("DownloadLink", typeof(string));

                mTable.Columns.AddRange(new DataColumn[] {col_Name,col_CreateDate,col_DownloadLink });

                foreach (string FileName in array2)
                {
                    FileInfo mInfo = new FileInfo(FileName);
                    DataRow mRow = mTable.NewRow();
                    mRow["FileName"] = mInfo.Name;
                    mRow["CreateDate"] = mInfo.LastWriteTime;
                    mRow["DownloadLink"] = MyConfig.ResourceLink + ExportExcel_Path.Replace("~", "").Replace("..", "") + mInfo.Name;
                    mTable.Rows.Add(mRow);
                }

                mTable.DefaultView.Sort= "CreateDate DESC";
                return mTable.DefaultView.ToTable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ExportToExcel()
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    int Name_Index = 1;

                    string FileName = MyFile.GetFullPathFile(ExportExcel_Path) + ExportExcel_CPName + "_" + ReportDay.ToString("dd.MM.yyyy") + ".xls";
                    string FileName_ByDay = MyFile.GetFullPathFile(ExportExcel_Path) + ExportExcel_CPName + "_" + ReportDay.ToString("dd.MM.yyyy") +"_"+ Member.LoginName() + ".xls";

                    if (!Directory.Exists(MyFile.GetFullPathFile(ExportExcel_Path)))
                    {
                        Directory.CreateDirectory(MyFile.GetFullPathFile(ExportExcel_Path));
                    }

                    while (File.Exists(FileName_ByDay))
                    {
                        FileName_ByDay = MyFile.GetFullPathFile(ExportExcel_Path) + ExportExcel_CPName + "_" + ReportDay.ToString("dd.MM.yyyy") + "_" + Member.LoginName() + "_" + Name_Index++.ToString() + ".xls";
                    }

                    if (File.Exists(FileName))
                    {

                        File.Copy(FileName, FileName_ByDay);
                        File.Delete(FileName);
                    }

                    StreamWriter writer = File.AppendText(FileName);
                    grv_Data.RenderControl(hw);
                    writer.WriteLine(sw.ToString());
                    writer.Close();
                }
            }
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn_Delete = (Button)sender;
                string FileName = MyFile.GetFullPathFile(ExportExcel_Path) + btn_Delete.CommandArgument;
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                    BindData();
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }
        protected void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DateTime.TryParseExact(tbx_ReportDay.Value, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ReportDay))
                {
                    MyMessage.ShowError("Xin vui lòng kiểm tra lại định dạng Ngày/Tháng/Năm của Ngày cần xuất dữ liệu.");
                        return;
                }
                DateTime EndDate = ReportDay.AddDays(1);

                DataTable mTable = mNews.Select(5, ReportDay.ToString(MyConfig.DateFormat_InsertToDB), EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                if (mTable == null || mTable.Rows.Count < 1)
                {
                    MyMessage.ShowError("Không có dữ lệu cho ngày " + tbx_ReportDay.Value);
                    return;
                }

                DataColumn col_Price = new DataColumn("Price", typeof(double));
                DataColumn col_RowCount = new DataColumn("RowCount", typeof(int));

                if (!mTable.Columns.Contains("Price"))
                {
                    mTable.Columns.Add(col_Price);
                }
                if (!mTable.Columns.Contains("RowCount"))
                {
                    mTable.Columns.Add(col_RowCount);
                }
                DataTable mTable_Temp = mTable.Copy();

                foreach (DataRow mRow in mTable.Rows)
                {
                    if (mRow["ServiceID"].ToString().Trim().StartsWith("90"))
                        mRow["Price"] = 500;
                    else if (mRow["ServiceID"].ToString().Trim().StartsWith("91"))
                        mRow["Price"] = 1000;
                    else if (mRow["ServiceID"].ToString().Trim().StartsWith("92"))
                        mRow["Price"] = 2000;
                    else if (mRow["ServiceID"].ToString().Trim().StartsWith("93"))
                        mRow["Price"] = 3000;
                    else if (mRow["ServiceID"].ToString().Trim().StartsWith("94"))
                        mRow["Price"] = 4000;
                    else if (mRow["ServiceID"].ToString().Trim().StartsWith("95"))
                        mRow["Price"] = 5000;
                    else if (mRow["ServiceID"].ToString().Trim().StartsWith("96"))
                        mRow["Price"] = 10000;
                    else if (mRow["ServiceID"].ToString().Trim().StartsWith("97"))
                        mRow["Price"] = 15000;

                    if (mRow["MethodID"].ToString() == "1")
                    {
                        mRow["Price"] = 500;
                    }
                    mTable_Temp.DefaultView.RowFilter = "LetterID=" + mRow["LetterID"].ToString();

                    mRow["RowCount"] = mTable_Temp.DefaultView.Count;
                }

                grv_Data.DataSource = mTable;
                grv_Data.DataBind();

                ExportToExcel();
                BindData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        private int CurrentLetterID = 0;
        protected void grv_Data_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Control lbl_LetterID = e.Row.FindControl("lbl_LetterID");

                    if (lbl_LetterID == null)
                    {
                        return;
                    }

                    int LetterID = 0;
                    if (!int.TryParse(((Label)lbl_LetterID).Text, out LetterID))
                    {
                        return;
                    }
                    int RowCount = 0;
                    if (!int.TryParse(((Label)lbl_LetterID).ToolTip, out RowCount))
                    {
                        return;
                    }

                    string S1 = e.Row.Cells[1].Text;
                    string S2 = e.Row.Cells[2].Text;
                    string S3 = e.Row.Cells[3].Text;

                    if (LetterID != 0 && LetterID != CurrentLetterID)
                    {
                        e.Row.Cells[1].RowSpan=RowCount;
                        e.Row.Cells[2].RowSpan = RowCount;
                        e.Row.Cells[3].RowSpan = RowCount;

                    }
                    else if (CurrentLetterID != 0 && LetterID != 0 && LetterID == CurrentLetterID)
                    {
                        
                        e.Row.Cells.RemoveAt(1);
                        e.Row.Cells.RemoveAt(1);//cellindex = 2
                        e.Row.Cells.RemoveAt(1);//cellindex = 3
                        //e.Row.Cells[1].Visible = false;
                        //e.Row.Cells[2].Visible = false;
                        //e.Row.Cells[3].Visible = false;
                        
                    }
                    CurrentLetterID = LetterID;
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


           
        }
    }
}