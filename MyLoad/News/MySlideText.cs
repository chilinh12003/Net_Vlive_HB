using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.News
{
    public class MySlideText : MyBase
    {

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="mCateID">CateID của thể loại muốn build ListCate</param>
        public MySlideText()
        {
            mTemplatePath = "~/Templates/News/SlideText.htm";
            mTemplatePath_Repeat = "~/Templates/News/SlideText_Repeat.htm";

            Init();
        }

        protected override string BuildHTML()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                MyAuction.News mNews = new MyAuction.News();
                DataTable mTable = mNews.Search(0, 0, 2, string.Empty, MyAuction.News.Adv_Text_CateID, true, " NewsID DESC, Priority DESC");


                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }
                int i = 0;
                foreach (DataRow mRow in mTable.Rows)
                {
                    if (i++ != 0)
                    {
                        mBuilder.Append("&nbsp;&nbsp;|&nbsp;&nbsp;");
                    }
                    string[] arr = { mRow["NewsID"].ToString(), MyCommon.CreateRewriteURL(mRow["NewsName"].ToString()), mRow["NewsName"].ToString() };
                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, arr));

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
