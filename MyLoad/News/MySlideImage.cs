using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.News
{
    public class MySlideImage : MyBase
    { 

        public MySlideImage()
        {
            mTemplatePath = "~/Templates/News/SlideImage.htm";
            mTemplatePath_Repeat = "~/Templates/News/SlideImage_Repeat.htm";
            mTemplatePath_Repeat_1 = "~/Templates/News/SlideImage_Repeat_1.htm";

            Init();
        }

        protected override string BuildHTML()
        {
            try
            {
                StringBuilder mBuilder_Image = new StringBuilder(string.Empty);
                StringBuilder mBuilder_Button = new StringBuilder(string.Empty);
                MyAuction.News mNews = new MyAuction.News();
                DataTable mTable = mNews.Search(0, 0, 5, string.Empty, MyAuction.News.Adv_Image_CateID, true, " NewsID DESC, Priority DESC");

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }

                int i = 1;
                foreach (DataRow mRow in mTable.Rows)
                {
                    string Link = string.Empty;
                    if (mRow["Link"] != DBNull.Value && mRow["Link"].ToString().Trim() != string.Empty)
                        Link = "onclick = \"return OpenNewWindow('" + mRow["Link"].ToString() + "');\"";
                    string NewsName = string.IsNullOrEmpty(mRow["NewsName"].ToString().Trim()) ? string.Empty : "<p>" + mRow["NewsName"].ToString() + "</p>";
                    string[] Arr_img = { mRow["NewsID"].ToString(), MyCommon.CreateRewriteURL(mRow["NewsName"].ToString()), Link, MyCommon.GetFullPathImage(mRow, 3), NewsName };
                    mBuilder_Image.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr_img));
                    mBuilder_Button.Append(mLoadTempLate.LoadTemplateByString(mTemplatePath_Repeat_1, i++.ToString()));
                }

                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { mBuilder_Image.ToString(), mBuilder_Button.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
