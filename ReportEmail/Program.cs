using System;
using System.Collections.Generic;
using System.Text;
using MyUtility;
using System.Data;
using System.Text;
using System.IO;

namespace ReportEmail
{
    class Program
    {
        static MO mMO = new MO("Oracle_Connection_Report");

        static string EmailAccount = string.Empty;
        static string Password = string.Empty;
        static string EmailReceiveList = string.Empty;

        static string Format = string.Empty;
        static string Format_TR_Repeat = string.Empty;
        static string Format_TR_Total = string.Empty;
        static string Format_TD_Repeat = string.Empty;
        static string Format_TH_Repeat_1 = string.Empty;
        static string Format_TR_Repeat_2 = string.Empty;

        static string ListKeyword = "GAME5|CLIP5|IMG5|NEWS5|MUSIC5|BD5|XS5";
        static string[] Arr_Keyword = { };
        static DataTable TableTemplate = new DataTable();
        static private void Init()
        {
            try
            {
                EmailAccount = MyConfig.GetKeyInConfigFile("EmailAccount");
                Password = MyConfig.GetKeyInConfigFile("Password");
                EmailReceiveList = MyConfig.GetKeyInConfigFile("EmailReceiveList");

                string Temp = MyConfig.GetKeyInConfigFile("ListKeyword");
                if (!string.IsNullOrEmpty(Temp))
                {
                    ListKeyword = Temp;
                }

                Arr_Keyword = ListKeyword.Split('|');

                Format = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay.htm"));
                Format_TR_Repeat = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay_TR_Repeat.htm"));
                Format_TR_Total = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay_TR_Total.htm"));
                Format_TD_Repeat = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay_TD_Repeat.htm"));
                Format_TH_Repeat_1 = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay_TH_Repeat_1.htm"));
                Format_TR_Repeat_2 = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay_TH_Repeat_2.htm"));

                TableTemplate = CreateTableTemplate();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void Main(string[] args)
        {
        Begin:
            try
            {
                Console.WriteLine("Bat dau chay chuong trinh");
                Console.WriteLine("Lay thong tin config, va khoi tai");
                Init();

                string Subject = MyConfig.GetKeyInConfigFile("Subject") + DateTime.Now.ToString("dd-MM-yyyy");

                Console.WriteLine("Build Noi dung Email");
                string Body = BuildEmail(DateTime.Now.AddDays(-1));
                Console.WriteLine("Bat dau gui email...");

                bool Result = MySendEmail.SendEmail_Google(EmailAccount, Password, EmailReceiveList, Subject, Body, System.Web.Mail.MailFormat.Html, string.Empty);

                MyLogfile.LogEmail(Subject, Body);

                if (!Result)
                {
                    Console.WriteLine("---GUI EMAIL KHONG THANH CONG, CHO 1 PHUT SE CHAY LAI----");
                    System.Threading.Thread.Sleep(60000);
                    goto Begin;
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex, false);
                Console.WriteLine("---CO LOI XAY RA, CHO 1 PHUT SE CHAY LAI----");
                System.Threading.Thread.Sleep(60000);
                goto Begin;
            }

            Console.WriteLine("Gui email report thanh cong - cho 1 phut chuong trinh se tu dong TAT");
            System.Threading.Thread.Sleep(60000);
        }


        public static string BuildEmail(DateTime mDate)
        {
            try
            {
                StringBuilder mBuild_TH_1 = new StringBuilder(string.Empty);
                StringBuilder mBuild_TH_2 = new StringBuilder(string.Empty);
                StringBuilder mBuild_TR = new StringBuilder(string.Empty);

                foreach (string item in Arr_Keyword)
                {
                    mBuild_TH_1.Append(string.Format(Format_TH_Repeat_1, item));
                    mBuild_TH_2.Append(Format_TR_Repeat_2);
                }
                DataTable mFullData = GetFullData(mDate);
                if (mFullData == null || mFullData.Rows.Count < 1)
                {
                    return string.Empty;
                }

                DataRow mTotalRow = mFullData.NewRow();
                mTotalRow["ReportDate"] = "TỔNG";

                foreach (DataRow mRow in mFullData.Rows)
                {
                    StringBuilder mBuild_TD = new StringBuilder(string.Empty);

                    #region MyRegion
                    foreach (DataColumn mCol in mFullData.Columns)
                    {
                        if (mCol.ColumnName.Equals("ReportDate", StringComparison.InvariantCultureIgnoreCase))
                        {
                            mBuild_TD.Append(string.Format(Format_TD_Repeat, mRow[mCol.ColumnName]));
                        }
                        else
                        {
                            double Temp = 0;
                            double.TryParse(mRow[mCol.ColumnName].ToString(), out Temp);
                            mBuild_TD.Append(string.Format(Format_TD_Repeat, Temp.ToString(MyConfig.DoubleFormat)));

                            if (mTotalRow[mCol.ColumnName] == DBNull.Value || mTotalRow[mCol.ColumnName].ToString() == string.Empty)
                            {
                                mTotalRow[mCol.ColumnName] = mRow[mCol.ColumnName];
                            }
                            else
                            {
                                mTotalRow[mCol.ColumnName] = double.Parse(mTotalRow[mCol.ColumnName].ToString()) + Temp;
                            }
                        }
                    } 
                    #endregion

                    mBuild_TR.Append(string.Format(Format_TR_Repeat, mBuild_TD.ToString()));
                }

                //Thêm dòng total
                #region MyRegion
                StringBuilder mBuilt_TD_Total = new StringBuilder(string.Empty);

                foreach (DataColumn mCol in mFullData.Columns)
                {
                    if (mCol.ColumnName.Equals("ReportDate", StringComparison.InvariantCultureIgnoreCase))
                    {
                        mBuilt_TD_Total.Append(string.Format(Format_TD_Repeat, mTotalRow[mCol.ColumnName]));
                    }
                    else
                    {
                        double Temp = 0;
                        double.TryParse(mTotalRow[mCol.ColumnName].ToString(), out Temp);
                        mBuilt_TD_Total.Append(string.Format(Format_TD_Repeat, Temp.ToString(MyConfig.DoubleFormat)));

                        if (mTotalRow[mCol.ColumnName] == DBNull.Value || mTotalRow[mCol.ColumnName].ToString() == string.Empty)
                        {
                            mTotalRow[mCol.ColumnName] = mTotalRow[mCol.ColumnName];
                        }
                        else
                        {
                            mTotalRow[mCol.ColumnName] = double.Parse(mTotalRow[mCol.ColumnName].ToString()) + Temp;
                        }
                    }
                }
                mBuild_TR.Append(string.Format(Format_TR_Total, mBuilt_TD_Total.ToString())); 
                #endregion

                int TotalColumn = mFullData.Columns.Count;

                return string.Format(Format, TotalColumn.ToString(), mDate.ToString("dd/MM/yyyy"), mBuild_TH_1.ToString(), mBuild_TH_2.ToString(), mBuild_TR.ToString());
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Tạo table mẫu
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateTableTemplate()
        {
            try
            {
                DataTable mTable = new DataTable("Child");
                DataColumn col_day = new DataColumn("ReportDate", typeof(string));
                DataColumn col_Total_Count = new DataColumn("Total_Count", typeof(string));
                DataColumn col_Total_Money = new DataColumn("Total_Money", typeof(string));
                DataColumn col_Other_Count = new DataColumn("Other_Count", typeof(string));
                DataColumn col_Other_Money = new DataColumn("Other_Money", typeof(string));
                mTable.Columns.Add(col_day);
                foreach (string item in Arr_Keyword)
                {
                    DataColumn mCol_Count = new DataColumn(item+"_Count", typeof(string));
                    DataColumn mCol_Money = new DataColumn(item + "_Money", typeof(string));
                    mTable.Columns.Add(mCol_Count);
                    mTable.Columns.Add(mCol_Money);
                }

                mTable.Columns.Add(col_Other_Count);
                mTable.Columns.Add(col_Other_Money);

                mTable.Columns.Add(col_Total_Count);
                mTable.Columns.Add(col_Total_Money);
                return mTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ file XML
        /// </summary>
        /// <param name="mDate"></param>
        /// <returns></returns>
        public static DataSet GetDataFromXML(DateTime mDate)
        {
            try
            {
                DataSet mSet = new DataSet();
                string FileName = "ByMonth_" + mDate.ToString("yyyyMM") + ".xml";
                string FullPath = MyFile.GetFullPathFile("\\App_Data\\" + FileName);
                FileInfo mFile = new FileInfo(FullPath);

                DataSet mSet_Return = new DataSet("Parent");
                DataTable mTable = TableTemplate.Clone();
                mSet_Return.Tables.Add(mTable);

                if (!Directory.Exists(mFile.DirectoryName))
                {
                    System.IO.Directory.CreateDirectory(mFile.DirectoryName);
                }

                if (!File.Exists(FullPath))
                {
                    //File.Create(FullPath);
                    return mSet_Return;
                }

                mSet = MyXML.GetXMLData(FullPath);

                if (mSet == null || mSet.Tables.Count < 1)
                    return mSet_Return;

                foreach (DataRow mRow in mSet.Tables[0].Rows)
                {
                    DataRow mNewRow = mTable.NewRow();

                    foreach (DataColumn mCol in mTable.Columns)
                    {
                        if (!mSet.Tables[0].Columns.Contains(mCol.ColumnName))
                        {
                            continue;
                        }
                        mNewRow[mCol.ColumnName] = mRow[mCol.ColumnName].ToString();
                    }
                    mTable.Rows.Add(mNewRow);
                }

                return mSet_Return;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ database
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataFromDB(DateTime mDate)
        {
            try
            {
                #region MyRegion
                DataTable Return_Table = TableTemplate.Clone();

                DateTime ToDate = new DateTime(mDate.Year, mDate.Month, mDate.Day);
                DateTime FromDate = ToDate.AddDays(1);
                DataTable mTable = mMO.Search(0, ToDate, FromDate, string.Empty);

                if (mTable == null || mTable.Rows.Count < 1)
                    return Return_Table;

                string ExistKeyword = string.Empty;
                string ReportDate = ToDate.ToString("dd/MM/yyyy");
                double TotalCount = 0;
                double TotalMoney = 0;

                DataRow mNewRow = Return_Table.NewRow(); 

                #endregion

                foreach (string item in Arr_Keyword)
                {
                    #region MyRegion
                    mTable.DefaultView.RowFilter = " MO_Keyword = '" + item + "' ";

                    if (mTable.DefaultView.Count < 1)
                        continue;

                    ExistKeyword += ",'" + item + "'";

                    double MOCount = 0;
                    double MOMoney = 0;

                    foreach (DataRowView mView in mTable.DefaultView)
                    {
                        MOCount += double.Parse(mView["TOTAL"].ToString());
                        MOMoney += double.Parse(mView["TOTAL"].ToString()) * GetPrice(mView["MO_ShortCode"].ToString());
                    }
                    mNewRow[item + "_Count"] = MOCount.ToString();
                    mNewRow[item + "_Money"] = MOMoney.ToString();
                    TotalCount += MOCount;
                    TotalMoney += MOMoney; 
                    #endregion
                }

                #region MyRegion
                ExistKeyword = ExistKeyword.Substring(1, ExistKeyword.Length - 1);
                mTable.DefaultView.RowFilter = " MO_Keyword NOT IN (" + ExistKeyword + " )";

                double OtherCount = 0;
                double OtherMoney = 0;

                foreach (DataRowView mView in mTable.DefaultView)
                {
                    OtherCount += double.Parse(mView["TOTAL"].ToString());
                    OtherMoney += double.Parse(mView["TOTAL"].ToString()) * GetPrice(mView["MO_ShortCode"].ToString());
                }
                mNewRow["Other_Count"] = OtherCount.ToString();
                mNewRow["Other_Money"] = OtherMoney.ToString();

                TotalCount += OtherCount;
                TotalMoney += OtherMoney; 
                #endregion

                mNewRow["ReportDate"] = ReportDate;
                mNewRow["Total_Count"] = TotalCount.ToString();
                mNewRow["Total_Money"] = TotalMoney.ToString();

                Return_Table.Rows.Add(mNewRow);

                return Return_Table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu trong file XML và trong DB, đồng thời lưu data đã lấy xuống XML
        /// </summary>
        /// <param name="mDate"></param>
        /// <returns></returns>
        private static DataTable GetFullData(DateTime mDate)
        {
            try
            {
                DataSet mSet = GetDataFromXML(mDate);
                DataTable mTable = GetDataFromDB(mDate);

                if (mSet == null || mSet.Tables.Count < 1 || mSet.Tables[0].Rows.Count < 1)
                {
                    mSet = new DataSet();

                    mSet.DataSetName = "Parent";
                    mSet.Tables.Add(mTable);
                }
                else
                {
                    for (int i = mSet.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow mRow_Old = mSet.Tables[0].Rows[i];

                        mTable.DefaultView.RowFilter = " ReportDate = '" + mRow_Old["ReportDate"].ToString() + "'";
                        //nếu dòng đã tồn tại thì cập nhật thêm vào.
                        if (mTable.DefaultView.Count > 0)
                        {
                            foreach (DataColumn mCol in mTable.Columns)
                            {
                                if (!mSet.Tables[0].Columns.Contains(mCol.ColumnName))
                                {
                                    continue;
                                }
                                mTable.DefaultView[0][mCol.ColumnName] = mRow_Old[mCol.ColumnName].ToString();
                            }
                        }
                        else
                        {
                            DataRow mNewRow = mTable.NewRow();
                            foreach (DataColumn mCol in mTable.Columns)
                            {
                                if (!mSet.Tables[0].Columns.Contains(mCol.ColumnName))
                                {
                                    continue;
                                }
                                mNewRow[mCol.ColumnName] = mRow_Old[mCol.ColumnName].ToString();
                            }
                            int index = 0;
                            if (mTable.Rows.Count > 0)
                                index = mTable.Rows.Count;

                            mTable.Rows.InsertAt(mNewRow, index);
                        }
                    }
                    mTable.DefaultView.RowFilter = string.Empty;
                    mSet = new DataSet("Parent");
                    DataTable tbl_Save = mTable.Clone();

                    for (int j = mTable.Rows.Count - 1; j >= 0; j--)
                    {
                        tbl_Save.ImportRow(mTable.Rows[j]);
                    }

                    mSet.Tables.Add(tbl_Save);
                }


                SaveDataFromXML(mSet, mDate);

                return mTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Lấy giá từ shortcode
        /// </summary>
        /// <param name="ShortCode"></param>
        /// <returns></returns>
        private static double GetPrice(string ShortCode)
        {
            try
            {
                if (ShortCode.Equals("9093", StringComparison.InvariantCultureIgnoreCase))
                {
                    return 500;
                }
                else if (ShortCode.Equals("9193", StringComparison.InvariantCultureIgnoreCase))
                    return 1000;
                else if (ShortCode.Equals("9293", StringComparison.InvariantCultureIgnoreCase))
                    return 2000;
                else if (ShortCode.Equals("9393", StringComparison.InvariantCultureIgnoreCase))
                    return 3000;
                else if (ShortCode.Equals("9493", StringComparison.InvariantCultureIgnoreCase))
                    return 4000;
                else if (ShortCode.Equals("9593", StringComparison.InvariantCultureIgnoreCase))
                    return 5000;
                else if (ShortCode.Equals("9693", StringComparison.InvariantCultureIgnoreCase))
                    return 10000;
                else if (ShortCode.Equals("9793", StringComparison.InvariantCultureIgnoreCase))
                    return 15000;
                else
                    return 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lưu dữ liệu đã gửi email xuống file xml
        /// </summary>
        /// <param name="mSet"></param>
        /// <param name="mDate"></param>
        public static void SaveDataFromXML(DataSet mSet, DateTime mDate)
        {
            try
            {
                string FileName = "ByMonth_" + mDate.ToString("yyyyMM") + ".xml";
                string FullPath = MyFile.GetFullPathFile("\\App_Data\\" + FileName);

                FileInfo mFile = new FileInfo(FullPath);
                if (!Directory.Exists(mFile.DirectoryName))
                {
                    System.IO.Directory.CreateDirectory(mFile.DirectoryName);
                }
                //if (!File.Exists(FullPath))
                //{
                //    File.Create(FullPath);                   
                //}
                System.Security.AccessControl.FileSecurity mSec = new System.Security.AccessControl.FileSecurity();

                mSet.WriteXml(FullPath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
