using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;

namespace MyLoad.User
{
    public class MyRegisterSuccess:MyBase
    {
        public MyRegisterSuccess()
        {
            mTemplatePath = "~/Templates/User/RegisterSuccess.htm";
            Init();
        }
        
        // Hàm trả về chuỗi có chứa mã HTML
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
