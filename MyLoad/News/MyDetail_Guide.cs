using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.News
{
    public class MyDetail_Guide : MyBase
    {

        /// <summary>
        /// CateID để lấy dữ liệu về Thể lệ
        /// </summary>
        public int Rule_CateID = 0;
        public MyDetail_Guide()
        {
            mTemplatePath = "~/Templates/News/SMSDetail_Guide.htm";

            Init();
        }
        public MyDetail_Guide( int Rule_CateID)
            : this()
        {
            this.Rule_CateID = Rule_CateID;
        }
        protected override string BuildHTML()
        {
            try
            {
                string Guide_HTML = string.Empty;
                string Rule_HTML = string.Empty;

                MyAuction.News mNews = new MyAuction.News();


                DataTable mTable = mNews.Select(2, Rule_CateID.ToString(), "1");
                if (mTable != null && mTable.Rows.Count > 0)
                {
                    Guide_HTML = mTable.Rows[0]["Content"].ToString();
                }            

                // Lấy template từ file HTML
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { Guide_HTML });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
