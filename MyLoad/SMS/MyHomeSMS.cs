using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;
using MyAuction.SMS;
namespace MyLoad.SMS
{
    public class MyHomeSMS : MyBase
    {
        public SMSSession.SessionType SessionTypeID = Session.SessionType.UpsideDown;

        public MyHomeSMS(SMSSession.SessionType mSessionType)
        {
            SessionTypeID = mSessionType;
            mTemplatePath = "~/Templates/SMS/HomeSMS.htm";

            Init();
        }

        protected override string BuildHTML()
        {
            try
            {
                MyAuction.SMS.SMSCurrent mCurrent = new MyAuction.SMS.SMSCurrent();
                MyAuction.SMS.SMSSession mSession = new SMSSession();
                
                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = mSession.Select(4,"1", ((int)SessionTypeID).ToString(),((int)SMSSession.Status.Ongoing).ToString());
                if (mTable == null || mTable.Rows.Count < 1)
                    return string.Empty;
                DataRow mRow = mTable.Rows[0];

                string ImagePath = string.Empty;
                if (mRow["ImagePath_3"] != DBNull.Value)
                    ImagePath = mRow["ImagePath_3"].ToString();
                else if (mRow["ImagePath_2"] != DBNull.Value)
                    ImagePath = mRow["ImagePath_2"].ToString();
                else if (mRow["ImagePath_1"] != DBNull.Value)
                    ImagePath = mRow["ImagePath_1"].ToString();
               ImagePath= ImagePath.Replace("..", "").Replace("~", "");

                string SessionID = mRow["SessionID"].ToString();
                string StartDate = mRow["StartDate"] != DBNull.Value ? ((DateTime)mRow["StartDate"]).ToString(MyAuction.LocalConfig.AuctionDateFormat) : string.Empty;
                string StopDate = mRow["StopDate"] != DBNull.Value ? ((DateTime)mRow["StopDate"]).ToString(MyAuction.LocalConfig.AuctionDateFormat) : string.Empty;
                string ProductName = mRow["ProductName"].ToString();
                string Description = mRow["Description"].ToString();
                string OriginalPrice = mRow["OriginalPrice"] != DBNull.Value ? ((double)mRow["OriginalPrice"]).ToString(MyUtility.MyConfig.DoubleFormat) : string.Empty;
                string SessionURL = "sms";
                if (SessionTypeID == Session.SessionType.Down)
                    SessionURL = "down";
                else if (SessionTypeID == Session.SessionType.Mark)
                {
                    SessionURL = "mark";
                }

               
                string SMSCode = mRow["SMSCode"].ToString();
                string ServiceID = mRow["ServiceID"].ToString();
                int Hour = 0, Minute = 0, Second = 0;
                TimeSpan RemainTime = (DateTime)mRow["StopDate"] - DateTime.Now;
                Hour = RemainTime.Hours + RemainTime.Days * 24;;
                Minute = RemainTime.Minutes;
                Second = RemainTime.Seconds;

                Hour = Hour < 0 ? 0 : Hour;
                Minute = Minute < 0 ? 0 : Minute;
                Second = Second < 0 ? 0 : Second;

                string[] Arr_Value = {  ImagePath,SessionID,StartDate,StopDate,
                                        ProductName,Description,OriginalPrice,SessionURL,
                                        SMSCode,ServiceID,
                                        Hour.ToString(),Minute.ToString(),Second.ToString(),
                                        MyUtility.MyEnum.StringValueOf(SessionTypeID).ToUpper()};

                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, Arr_Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
