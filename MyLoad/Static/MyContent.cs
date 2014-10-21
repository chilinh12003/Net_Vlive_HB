using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;

namespace MyLoad.Static
{
    public class MyContent : MyBase
    {
        public MyContent()
        {
            mTemplatePath = "~/Templates/Static/Content.htm";
            Init();
        }
        public delegate string InsertHTML_Left();
       
        /// <summary>
        /// Hàm này sẽ được định nghĩa sau khi khởi tạo 1 đối tượng (để insert vào phần Left của Content)
        /// </summary>
        public event InsertHTML_Left InsertHTML_Left_Change;

        public delegate string InsertHTML_Right();

        /// <summary>
        /// Hàm này sẽ được định nghĩa sau khi khởi tạo 1 đối tượng (để insert vào phần Right của Content)
        /// </summary>
        public event InsertHTML_Right InsertHTML_Right_Change;

        /// <summary>
        /// Hàm trả về chuỗi có chứa mã HTML
        /// </summary>
        /// <returns></returns>
        protected override string BuildHTML()
        {
            try
            {
                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { InsertHTML_Left_Change(), InsertHTML_Right_Change() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
