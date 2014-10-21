using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.SMS
{
    public class MyDetail_Session : MyBase
    {
       
        public DataTable mTable = new DataTable();
        public MyDetail_Session(DataTable tblSession)
        {
            mTable = tblSession;
            mTemplatePath = "~/Templates/SMS/SMSDetail_Session.htm";

            Init();
        }
        protected override string BuildHTML()
        {
            try
            {
                MyAuction.SMS.SMSSession mSession = new MyAuction.SMS.SMSSession();
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
              

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }

                DataRow mRow = mTable.Rows[0];
                    string SessionID = mRow["SessionID"].ToString();
                    string SessionNameURL = MyCommon.CreateRewriteURL(mRow["SessionName"].ToString());
                    string ProductName = mRow["ProductName"].ToString();
                    string StepPrice = mRow["StepPrice"] != DBNull.Value ? ((double)mRow["StepPrice"]).ToString(MyUtility.MyConfig.DoubleFormat) : string.Empty;
                    string OriginalPrice = mRow["OriginalPrice"] != DBNull.Value ? ((double)mRow["OriginalPrice"]).ToString(MyUtility.MyConfig.DoubleFormat) : string.Empty;
                    
                    int Hour = 0, Minute = 0, Second = 0;
                    TimeSpan RemainTime = (DateTime)mRow["StopDate"] - DateTime.Now;
                    Hour = RemainTime.Hours + RemainTime.Days * 24;
                    Minute = RemainTime.Minutes;
                    Second = RemainTime.Seconds;

                    Hour = Hour < 0 ? 0 : Hour;
                    Minute = Minute < 0 ? 0 : Minute;
                    Second = Second < 0 ? 0 : Second;

                    string SMSCode = mRow["SMSCode"].ToString();
                    string ServiceID = mRow["ServiceID"].ToString();


                    MyAuction.Product mProduct = new MyAuction.Product();

                    DataTable tblProduct = mProduct.Select(1, mRow["ProductID"].ToString());
                    string Description = string.Empty;
                    if (tblProduct != null && tblProduct.Rows.Count > 0)
                    {
                        Description = tblProduct.Rows[0]["Description"].ToString();
                    }
                    string[] Arr_Value = {  ProductName,Hour.ToString(),Minute.ToString(),Second.ToString(),
                                            OriginalPrice,SMSCode,SessionID,ServiceID,Description,mRow["ProductID"].ToString()};

                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath, Arr_Value));

                    return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
