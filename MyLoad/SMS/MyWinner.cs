using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;
using MyAuction.SMS;
namespace MyLoad.SMS
{
    public class MyWinner:MyBase
    {
        public MyWinner()
        {
            mTemplatePath = "~/Templates/SMS/Winner.htm";
            mTemplatePath_Repeat = "~/Templates/SMS/Winner_Repeat.htm";
            mTemplatePath_Repeat_1 = "~/Templates/SMS/Winner_Repeat_1.htm";

            Init();
        }
      
        protected override string BuildHTML()
        {
            try
            {
                MyAuction.SMS.Winner mWinner = new MyAuction.SMS.Winner();
                DataTable mTable = mWinner.Select(2,string.Empty);
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                string Title = "DS TRÚNG THƯỞNG ĐẤU GIÁ NGƯỢC";
                mTable.DefaultView.RowFilter = " SessionTypeID = " + ((int)SMSSession.SessionType.UpsideDown).ToString();
                mBuilder.Append(BuildItem(mTable, Title));

                Title = "DS TRÚNG THƯỞNG ĐẤU GIÁ XUÔI";
                mTable.DefaultView.RowFilter = " SessionTypeID = " + ((int)SMSSession.SessionType.Down).ToString();
                mBuilder.Append(BuildItem(mTable, Title));

                Title = "DS TRÚNG THƯỞNG ĐẤU GIÁ TÍCH ĐIỂM";
                mTable.DefaultView.RowFilter = " SessionTypeID = " + ((int)SMSSession.SessionType.Mark).ToString();
                mBuilder.Append(BuildItem(mTable, Title));

                return mLoadTempLate.LoadTemplateByString(mTemplatePath, mBuilder.ToString());
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string BuildItem(DataTable mTable, string Title)
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                foreach (DataRowView mView in mTable.DefaultView)
                {
                    string[] arr = {mView["FullName"].ToString(),
                                    mView["MSISDN"].ToString(),
                                    mView["SessionName"].ToString(),
                                    mView["Price"].ToString(),
                                    mView["ProductName"].ToString()
                                    };

                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat_1, arr));
                }
                if (mTable.DefaultView.Count > 0)
                    return mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, new string[] { Title, mBuilder.ToString() });
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
