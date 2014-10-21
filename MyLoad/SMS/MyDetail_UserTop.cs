using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.SMS
{
    public class MyDetail_UserTop:MyBase
    {
         public string SessionID = string.Empty;
         public int RowCount = 10;
        public MyDetail_UserTop()
        {
            mTemplatePath = "~/Templates/SMS/SMSDetail_UserTop.htm";
            mTemplatePath_Repeat = "~/Templates/SMS/SMSDetail_UserTop_Repeat.htm";
            mTemplatePath_Repeat_1 = "~/Templates/SMS/SMSDetail_UserTop_Repeat_1.htm";
            Init();
        }

        public MyDetail_UserTop(string SessionID)
            : this()
        {
            this.SessionID = SessionID;
        }

        protected override string BuildHTML()
        {
            try
            {
                MyAuction.SMS.SMSTop mUserReport = new MyAuction.SMS.SMSTop();
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = mUserReport.Select(2, SessionID, RowCount.ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }

                int i = 0;
                foreach (DataRow mRow in mTable.Rows)
                {
                    string[] Arr_Value = { mRow["UserName"].ToString(),mRow["RowIndex"].ToString()};
                    if (++i % 2 == 0)
                    {
                        mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr_Value));
                    }
                    else
                    {
                        mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat_1, Arr_Value));
                    }
                }
                if (i % 2 == 0)
                {
                    mBuilder.Append("<div class=\"item_next_last\"></div>");
                }
                else
                {
                    mBuilder.Append("<div class=\"item_last\"></div>");
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
