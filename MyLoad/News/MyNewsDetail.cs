using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;


namespace MyLoad.News
{
    public class MyNewsDetail:MyBase
    {
         /// <summary>
        /// CateID để lấy dữ liệu về Thể lệ
        /// </summary>
        public int NewsID = 0;
        public MyNewsDetail()
        {
            mTemplatePath = "~/Templates/News/NewsDetail.htm";

            Init();
        }
        public MyNewsDetail(int NewsID)
            : this()
        {
            this.NewsID = NewsID;
        }
        protected override string BuildHTML()
        {
            try
            {
                string Guide_HTML = string.Empty;
                string Rule_HTML = string.Empty;

                MyAuction.News mNews = new MyAuction.News();

                DataTable mTable = mNews.Select(1, NewsID.ToString(), "1");
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
