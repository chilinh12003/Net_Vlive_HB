using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;

namespace MyLoad.Static
{
    public class MyPath:MyBase
    {
        public MyPath(string PageCode)
        {
            mTemplatePath = "~/Templates/Static/Path.htm";           

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
