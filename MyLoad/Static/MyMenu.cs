using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
namespace MyLoad.Static
{
    public class MyMenu: MyBase
    {

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="mCrrentPage"></param>
        public MyMenu(MyCommon.MenuPage mCrrentPage)
        {
            mTemplatePath = "~/Templates/Static/Menu.htm";
            Init();
            CurrentPage = mCrrentPage;
        }

        protected override void Begin()
        {
            if(!string.IsNullOrEmpty( PreviousKey) && !PreviousKey.Equals(CurrentPage.ToString(),StringComparison.CurrentCultureIgnoreCase))
            {
                RemoveCache();
            }
        }
        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                string css_0 = "item";
                string css_1 = "item";
                string css_2 = "item";
                string css_3= "item";
                string css_4 = "item";
                string css_5 = "item";
                string css_6 = "item";
                switch (CurrentPage)
                {
                    case MyCommon.MenuPage.Home:
                        css_0 = "active";
                        break;
                    case MyCommon.MenuPage.About:
                        css_1 = "active";
                        break;
                    case MyCommon.MenuPage.Guide:
                        css_2 = "active";
                        break;
                    case MyCommon.MenuPage.Rule:
                        css_3 = "active";
                        break;
                    case MyCommon.MenuPage.Winner:
                        css_4 = "active";
                        break;
                    case MyCommon.MenuPage.FAQ:
                        css_5 = "active";
                        break;
                    case MyCommon.MenuPage.Contact:
                        css_6 = "active";
                        break;
                }
                string[] arr_css = { css_0, css_1, css_2, css_3, css_4, css_5 };

                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath,arr_css);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected override void Finish()
        {
            PreviousKey = CurrentPage.ToString();
        }
    }
}
