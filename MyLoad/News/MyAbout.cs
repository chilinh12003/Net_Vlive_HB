using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using System.Data;


namespace MyLoad.News
{
    public class MyAbout : MyBase
    {
         /// <summary>
        /// CateID để lấy dữ liệu về Thể lệ
        /// </summary>
        public MyAbout()
        {
            mTemplatePath = "~/Templates/News/About.htm";

            Init();
        }
       
        protected override string BuildHTML()
        {
            try
            {
                // Lấy template từ file HTML
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplate(mTemplatePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
