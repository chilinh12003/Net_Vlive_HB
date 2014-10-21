using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.SMS
{
    public class MyListSMS : MyBase
    {
        public int? CateID = 0;
        public int RowCount = 8;

        public MyListSMS()
        {
            mTemplatePath = "~/Templates/SMS/ListSMS.htm";
            mTemplatePath_Repeat = "~/Templates/SMS/SMSItem_Repeat.htm";

            Init();
        }
        public MyListSMS(int? CateID,int RowCount):this()
        {
            this.CateID = CateID;
            this.RowCount = RowCount;
        }
        protected override string BuildHTML()
        {
            try
            {
                MyAuction.SMS.SMSCurrent mCurrent = new MyAuction.SMS.SMSCurrent();
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = mCurrent.Select(4, CateID.ToString(), RowCount.ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }

                foreach (DataRow mRow in mTable.Rows)
                {
                    string SessionID = mRow["SessionID"].ToString();
                    string SessionNameURL = MyCommon.CreateRewriteURL(mRow["SessionName"].ToString());
                    string ProductName = mRow["ProductName"].ToString();
                    string ImagePath = string.Empty;
                    if (mRow["ImagePath_2"] != DBNull.Value)
                        ImagePath = mRow["ImagePath_2"].ToString();
                    else if (mRow["ImagePath_3"] != DBNull.Value)
                        ImagePath = mRow["ImagePath_3"].ToString();
                    else if (mRow["ImagePath_1"] != DBNull.Value)
                        ImagePath = mRow["ImagePath_1"].ToString();
                    ImagePath = ImagePath.Replace("..", "").Replace("~", "");
                    string OriginalPrice = mRow["OriginalPrice"] != DBNull.Value ? ((double)mRow["OriginalPrice"]).ToString(MyUtility.MyConfig.DoubleFormat) : string.Empty;

                    int Hour = 0, Minute = 0, Second = 0;
                    TimeSpan RemainTime = (DateTime)mRow["StopDate"] - DateTime.Now;
                    Hour = RemainTime.Hours + RemainTime.Days * 24;;
                    Minute = RemainTime.Minutes;
                    Second = RemainTime.Seconds;

                    Hour = Hour < 0 ? 0 : Hour;
                    Minute = Minute < 0 ? 0 : Minute;
                    Second = Second < 0 ? 0 : Second;

                    string SMSCode = mRow["SMSCode"].ToString();
                    string ServiceID = mRow["ServiceID"].ToString();
                    
                    string UserName = mRow["UserName"] != DBNull.Value ? mRow["UserName"].ToString() : string.Empty;

                    string[] Arr_Value = {  SessionID,SessionNameURL,ProductName,
                                         ImagePath,OriginalPrice,Hour.ToString(),Minute.ToString(),Second.ToString(),
                                         SMSCode,SessionID,ServiceID,UserName};
                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr_Value));
                }

                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByString(mTemplatePath, mBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
