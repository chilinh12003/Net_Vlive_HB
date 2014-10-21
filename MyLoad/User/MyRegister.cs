using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
namespace MyLoad.User
{
    public class MyRegister:MyBase
    {
        public MyRegister()
        {
            mTemplatePath = "~/Templates/User/Register.htm";           

            Init();
        }
        
        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                return mLoadTempLate.LoadTemplate(mTemplatePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
