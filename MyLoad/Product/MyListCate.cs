using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;
using MyAuction.SMS;
namespace MyLoad.Product
{
    public class MyListCate : MyBase
    {

        string mTemplatePath_Empty = string.Empty;
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        public MyListCate()
        {
            
            mTemplatePath = "~/Templates/Product/Cate.htm";
            mTemplatePath_Repeat = "~/Templates/Product/CateRepeat.htm";
            mTemplatePath_Repeat_1 = "~/Templates/Product/CateRepeat_1.htm";
            mTemplatePath_Empty = "~/Templates/Product/Cate_Empty.htm";
            Init();
        }       
      
        protected override string BuildHTML()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                Winner mWinner = new Winner();

                DataSet mSet = mWinner.Select_DataSet(3);

                if (mSet == null || mSet.Tables.Count != 2)
                    return string.Empty;

                DataTable tbl_Week = mSet.Tables[0];
                DataTable tbl_Winner = mSet.Tables[1];

                int count_1 = 1;
                string div_Current_ID = string.Empty;
                string CurentCount = "0";

                foreach (DataRow mRow_1 in tbl_Week.Rows)
                {
                    tbl_Winner.DefaultView.RowFilter = " Week = " + mRow_1["Week"].ToString();


                    string[] Arr_1 = { count_1.ToString(), tbl_Winner.DefaultView.Count.ToString(), "TUẦN " + mRow_1["Week"].ToString() };
                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr_1));
                    if (tbl_Winner.DefaultView.Count > 0)
                    {
                        int count_2 = 1;
                        foreach (DataRowView mView in tbl_Winner.DefaultView)
                        {
                            string[] Arr_2 = { count_1.ToString(), count_2.ToString(), MyUtility.MyEnum.StringValueOf(((SMSSession.SessionType)(int)mView["SessionTypeID"])), mView["MSISDN"].ToString() };
                            mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat_1, Arr_2));
                            count_2++;
                        }
                    }
                    count_1++;
                }
                if (tbl_Week == null || tbl_Week.Rows.Count < 1)
                {
                    return mLoadTempLate.LoadTemplate(mTemplatePath_Empty);
                }
                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { mBuilder.ToString(), div_Current_ID, CurentCount });
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        protected override void Finish()
        {
           
        }
    }
}
