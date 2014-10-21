using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.SMS
{
    public class MySMSFinish : MyBase
    {
        public int? CateID = 0;
        public int RowCount = 8;
        public MySMSFinish()
        {
            mTemplatePath = "~/Templates/SMS/SMSFinish.htm";
            mTemplatePath_Repeat = "~/Templates/SMS/SMSFinish_Repeat.htm";

            Init();
        }

        public MySMSFinish(int? CateID, int RowCount)
            : this()
        {           
            this.CateID = CateID;
            this.RowCount = RowCount;
        }

        protected override string BuildHTML()
        {
            try
            {
                MyAuction.SMS.SMSFinish mFinish = new MyAuction.SMS.SMSFinish();
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = mFinish.Select(4, CateID.ToString(), RowCount.ToString());

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

                    string FinishDate = ((DateTime)mRow["FinishDate"]).ToString(LocalConfig.AuctionDateFormat);

                    
                    string BuyPrice = ((double)mRow["CurrentPrice"]).ToString(MyUtility.MyConfig.DoubleFormat);
                    string UserName = mRow["UserName"] != DBNull.Value ? mRow["UserName"].ToString() : string.Empty;

                    string[] Arr_Value = {  SessionID,SessionNameURL,ProductName,
                                         ImagePath,OriginalPrice,FinishDate,
                                         BuyPrice,UserName};
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
