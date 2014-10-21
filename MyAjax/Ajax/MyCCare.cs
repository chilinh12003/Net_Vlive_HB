using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyUtility;
using MyVlive.CCare;

namespace MyAjax.Ajax
{
    public class MyCCare:MyAjaxBase
    {
        string ConnectionKeyConfig = "Oracle_Connection";

        public void ViewInfo()
        {
            try
            {
                V_Member mMember = new V_Member(ConnectionKeyConfig);

                int Week = 0;
                GetParemeter<int>(ref Week, "Week");

                string MSISDN = string.Empty;
                GetParemeter<string>(ref MSISDN, "MSISDN");

                if (string.IsNullOrEmpty(MSISDN))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy nhập chính xác số điện thoại"));
                    return;
                }

                if (Week < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy chọn tuần thi đấu."));
                    return;
                }

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Số điện thoại nhập vào không thuộc mạng Vinaphone."));
                    return;
                }

                DataTable mTable = mMember.Search(0, MSISDN, Week);

                if (mTable.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.UnSuccess, MyMessage.CommonSuccess.NoData, MyMessage.CommonSuccess.NoData));
                    return;
                }
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                string Format_Info = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/CCare/Info.htm"));

                DataRow mRow = mTable.Rows[0];
                double Duration = 0;
                double.TryParse(mRow["Duration"].ToString(), out Duration);

                mBuilder.Append(string.Format(Format_Info, mRow["SimNumber"].ToString(),
                                                            mRow["Week"].ToString(),
                                                            mRow["Score"].ToString(),
                                                            Duration.ToString(MyConfig.DoubleFormat),
                                                            ((DateTime)mRow["StartDate"]).ToString(MyConfig.LongDateFormat),
                                                            ((DateTime)mRow["EndTime"]).ToString(MyConfig.LongDateFormat)
                                                            ));

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, mBuilder.ToString(), MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void ViewAnswerHistory()
        {
            try
            {
                V_MemberLog mMember_Log = new V_MemberLog(ConnectionKeyConfig);

                int Week = 0;
                GetParemeter<int>(ref Week, "Week");
                
                int PageIndex = 1;
                GetParemeter<int>(ref PageIndex, "PageIndex");

                string MSISDN = string.Empty;
                GetParemeter<string>(ref MSISDN, "MSISDN");

                string p_BeginDate = string.Empty;
                GetParemeter<string>(ref p_BeginDate, "BeginDate");

                string p_EndDate = string.Empty;
                GetParemeter<string>(ref p_EndDate, "EndDate");

                DateTime BeginDate = DateTime.MinValue;
                DateTime EndDate = DateTime.MinValue;

                if (!DateTime.TryParseExact(p_BeginDate, "dd/MM/yyyyTH", null, System.Globalization.DateTimeStyles.None, out BeginDate))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy nhập chính xác ngày bắt đầu."));
                    return;
                }
                if (!DateTime.TryParseExact(p_EndDate, "dd/MM/yyyyTH", null, System.Globalization.DateTimeStyles.None, out EndDate))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy nhập chính xác ngày kết thúc."));
                    return;
                }

                if (string.IsNullOrEmpty(MSISDN))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy nhập chính xác số điện thoại"));
                    return;
                }

                if (Week < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy chọn tuần thi đấu."));
                    return;
                }

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Số điện thoại nhập vào không thuộc mạng Vinaphone."));
                    return;
                }
                

                MyPaging mPaging = new MyPaging();
                mPaging.Onclick = " ViewHistoryAnswer(this,{0});";
                mPaging.PageSize = 5;
                mPaging.CurrentPageIndex = PageIndex;

                mPaging.TotalRow = mMember_Log.TotalRow(0, MSISDN, Week, BeginDate, EndDate);

                if (mPaging.TotalRow < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.UnSuccess, MyMessage.CommonSuccess.NoData, MyMessage.CommonSuccess.NoData));
                    return;
                }

                DataTable mTable = mMember_Log.Search(0,mPaging.BeginRow,mPaging.EndRow, MSISDN, Week, BeginDate, EndDate, string.Empty);

                if (mTable.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.UnSuccess, MyMessage.CommonSuccess.NoData, MyMessage.CommonSuccess.NoData));
                    return;
                }

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                string Format_MemberLog = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/CCare/MemberLog.htm"));
                string Format_MemberLog_Repeat_1 = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/CCare/MemberLog_Repeat_1.htm"));
                string Format_MemberLog_Repeat_2 = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/CCare/MemberLog_Repeat_2.htm"));

                int Count = 0;

                

                foreach (DataRow mRow in mTable.Rows)
                {
                    string Format_Repeat = Format_MemberLog_Repeat_1;
                    if (++Count % 2 == 0)
                    {
                        Format_Repeat = Format_MemberLog_Repeat_2;
                    }

                    int STT = Count + (mPaging.CurrentPageIndex -1)* mPaging.PageSize;

                    string[] arr = {STT.ToString(),
                                    mRow["SimNumber"].ToString(),
                                    mRow["Question"].ToString(),
                                    mRow["Answer"].ToString(),
                                    ((DateTime)mRow["CreatedTime"]).ToString(MyConfig.LongDateFormat),
                                    mRow["IsCorrect"].ToString()
                                   };

                    mBuilder.Append(string.Format(Format_Repeat, arr));
                }


                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, string.Format(Format_MemberLog, new string[] { mBuilder.ToString(), mPaging.BuildHTML() }), MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void ViewMOHistory()
        {
            try
            {
                V_MO mMO = new V_MO(ConnectionKeyConfig);
              
                int PageIndex = 1;
                GetParemeter<int>(ref PageIndex, "PageIndex");

                string MSISDN = string.Empty;
                GetParemeter<string>(ref MSISDN, "MSISDN");

                string p_BeginDate = string.Empty;
                GetParemeter<string>(ref p_BeginDate, "BeginDate");

                string p_EndDate = string.Empty;
                GetParemeter<string>(ref p_EndDate, "EndDate");

                DateTime BeginDate = DateTime.MinValue;
                DateTime EndDate = DateTime.MinValue;

                if (!DateTime.TryParseExact(p_BeginDate, "dd/MM/yyyyTH", null, System.Globalization.DateTimeStyles.None, out BeginDate))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy nhập chính xác ngày bắt đầu."));
                    return;
                }
                if (!DateTime.TryParseExact(p_EndDate, "dd/MM/yyyyTH", null, System.Globalization.DateTimeStyles.None, out EndDate))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy nhập chính xác ngày kết thúc."));
                    return;
                }

                if (string.IsNullOrEmpty(MSISDN))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Xin hãy nhập chính xác số điện thoại"));
                    return;
                }
             

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Số điện thoại nhập vào không thuộc mạng Vinaphone."));
                    return;
                }


                MyPaging mPaging = new MyPaging();
                mPaging.Onclick = " ViewMOAnswer(this,{0});";
                mPaging.PageSize = 5;
                mPaging.CurrentPageIndex = PageIndex;

                mPaging.TotalRow = mMO.TotalRow(0, MSISDN, BeginDate, EndDate);

                if (mPaging.TotalRow < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.UnSuccess, MyMessage.CommonSuccess.NoData, MyMessage.CommonSuccess.NoData));
                    return;
                }

                DataTable mTable = mMO.Search(0, mPaging.BeginRow, mPaging.EndRow, MSISDN, BeginDate, EndDate, string.Empty);

                if (mTable.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.UnSuccess, MyMessage.CommonSuccess.NoData, MyMessage.CommonSuccess.NoData));
                    return;
                }

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                string Format_MemberLog = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/CCare/MOLog.htm"));
                string Format_MemberLog_Repeat_1 = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/CCare/MOLog_Repeat_1.htm"));
                string Format_MemberLog_Repeat_2 = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/CCare/MOLog_Repeat_2.htm"));

                int Count = 0;
                foreach (DataRow mRow in mTable.Rows)
                {
                    string Format_Repeat = Format_MemberLog_Repeat_1;
                    if (++Count % 2 == 0)
                    {
                        Format_Repeat = Format_MemberLog_Repeat_2;
                    }
                    int STT = Count + (mPaging.CurrentPageIndex - 1) * mPaging.PageSize;

                    string[] arr = {STT.ToString(),
                                    mRow["UserID"].ToString(),
                                    mRow["MO"].ToString(),
                                    mRow["ServiceID"].ToString(),
                                    mRow["MT"].ToString(),
                                    ((DateTime)mRow["requesttime"]).ToString(MyConfig.LongDateFormat)
                                   };

                    mBuilder.Append(string.Format(Format_Repeat, arr));
                }


                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, string.Format(Format_MemberLog, new string[] { mBuilder.ToString(), mPaging.BuildHTML() }), MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }
    }
}
