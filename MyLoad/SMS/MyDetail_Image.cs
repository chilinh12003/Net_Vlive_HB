using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.SMS
{
    public class MyDetail_Image : MyBase
    {
        public int? ProductID = 0;
        public MyDetail_Image()
        {
            mTemplatePath = "~/Templates/SMS/SMSDetail_Image.htm";
            mTemplatePath_Repeat = "~/Templates/SMS/SMSDetail_Image_Repeat.htm";

            Init();
        }

        public MyDetail_Image(int? ProductID)
            : this()
        {
            this.ProductID = ProductID;
        }

        protected override string BuildHTML()
        {
            try
            {
                MyAuction.ProductMedia mMedia = new MyAuction.ProductMedia();
                StringBuilder mBuilder_Image = new StringBuilder(string.Empty);
                StringBuilder mBuilder_ImageThumb = new StringBuilder(string.Empty);

                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = mMedia.Select(2, ProductID.ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }

                foreach (DataRow mRow in mTable.Rows)
                {
                    
                    string ImagePath = string.Empty;
                    string ImagePath_Thumb = string.Empty;
                    string ImageName = string.Empty;

                    ImagePath = mRow["ImagePath_2"].ToString();
                    ImagePath = ImagePath.Replace("..", "").Replace("~", "");

                    ImagePath_Thumb = mRow["ImagePath_1"].ToString();
                    ImagePath_Thumb = ImagePath_Thumb.Replace("..", "").Replace("~", "");

                  
                    string[] Arr_Value = {ImagePath,ImageName};
                    string[] Arr_Value_Thumb = { ImagePath_Thumb, ImageName };

                    mBuilder_Image.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr_Value));
                    mBuilder_ImageThumb.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr_Value_Thumb));
                }

                // Lấy template từ file HTML
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { mBuilder_Image.ToString(), mBuilder_ImageThumb.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
