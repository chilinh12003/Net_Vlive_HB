using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.News
{
    public class MyHot : MyBase
    {
        public MyHot()
        {
            mTemplatePath = "~/Templates/News/Hot.htm";
            mTemplatePath_Repeat = "~/Templates/News/Hot_Repeat.htm";

            Init();
        }

        protected override string BuildHTML()
        {
            try
            {
                StringBuilder mBuilder_News = new StringBuilder(string.Empty);

                MyAuction.News mNews = new MyAuction.News();
                DataTable mTable = mNews.Search(0, 0, 5, string.Empty, MyAuction.News.HotNews_CateID, true, " NewsID DESC, Priority DESC");
               
                foreach (DataRow mRow in mTable.Rows)
                {
                    string[] Arr = { mRow["NewsID"].ToString(), MyCommon.CreateRewriteURL(mRow["NewsName"].ToString()), mRow["NewsName"].ToString() };
                    mBuilder_News.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr));
                }               
                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { mBuilder_News.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
