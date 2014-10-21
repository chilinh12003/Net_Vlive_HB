using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
namespace MyLoad.Static
{
    public class MyBanner : MyBase
    {
        public MyBanner()
        {
            mTemplatePath = "~/Templates/Static/Banner.htm";           

            Init();
        }
        
        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                //MyLoginBox mLoginBox = new MyLoginBox();
                
                return mLoadTempLate.LoadTemplateByString(mTemplatePath, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
