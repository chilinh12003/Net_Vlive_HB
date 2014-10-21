using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.SMS
{
    public class MyFinishSession:MyBase
    {
        public MyAuction.SMS.SMSSession.Status StatusID = MyAuction.SMS.SMSSession.Status.Ended;
        public MyAuction.SMS.SMSSession.SessionType TypeID = Session.SessionType.UpsideDown;

        public MyFinishSession(Session.SessionType mTypeID)
        {
            TypeID = mTypeID;
            mTemplatePath = "~/Templates/SMS/FinishSession.htm";
            mTemplatePath_Repeat = "~/Templates/SMS/FinishSession_Repeat.htm";

            Init();
        }

        protected override string BuildHTML()
        {
            try
            {
                MyAuction.SMS.SMSSession mSession = new MyAuction.SMS.SMSSession();

                StringBuilder mBuilder = new StringBuilder(string.Empty);
                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = mSession.Select(4, "4", ((int)TypeID).ToString(), ((int)StatusID).ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }
                int i = 1;
                foreach (DataRow mRow in mTable.Rows)
                {
                    string ProductName = mRow["ProductName"].ToString();
                    string ImagePath = MyCommon.GetFullPathImage(mRow, 2);
                    string Price = mRow["OriginalPrice"] != DBNull.Value ? ((double)mRow["OriginalPrice"]).ToString(MyUtility.MyConfig.DoubleFormat) : string.Empty;
                    string[] Arr_Value = {  ProductName,ImagePath,Price, i++.ToString()};
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
