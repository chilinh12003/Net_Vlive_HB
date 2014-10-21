using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.SMS
{
    public class MyDetail_Product : MyBase
    {
        public int? ProductID = 0;
        public MyDetail_Product()
        {
            mTemplatePath = "~/Templates/SMS/SMSDetail_Product.htm";

            Init();
        }

        public MyDetail_Product(int? ProductID)
            : this()
        {
            this.ProductID = ProductID;
        }

        protected override string BuildHTML()
        {
            try
            {
                MyAuction.Product mProduct = new MyAuction.Product();

                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = mProduct.Select(1, ProductID.ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }
                
                // Lấy template từ file HTML
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByString(mTemplatePath, mTable.Rows[0]["Content"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
