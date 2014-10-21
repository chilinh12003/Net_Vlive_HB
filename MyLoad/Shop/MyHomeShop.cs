using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;


namespace MyLoad.Shop
{
    public class MyHomeShop:MyBase
    {
        public MyAuction.Shop.Shop.ShopStatus ShopStatusID = MyAuction.Shop.Shop.ShopStatus.Auction;

        public MyHomeShop(MyAuction.Shop.Shop.ShopStatus mShopStatus)
        {
            ShopStatusID = mShopStatus;
            mTemplatePath = "~/Templates/Shop/HomeShop.htm";
            mTemplatePath_Repeat = "~/Templates/Shop/ShopItem_Repeat.htm";

            Init();
        }

        protected override string BuildHTML()
        {
            try
            {
                MyAuction.Shop.Shop mShop = new MyAuction.Shop.Shop();
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = mShop.Select(3, "4", ((int)ShopStatusID).ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }

                foreach (DataRow mRow in mTable.Rows)
                {
                    string ShopID = mRow["ShopID"].ToString();
                    string ProductNameURL = MyCommon.CreateRewriteURL(mRow["ProductName"].ToString());
                    string ProductName = mRow["ProductName"].ToString();
                    string ImagePath = MyCommon.GetFullPathImage(mRow, 2);
                   

                    string Price = mRow["Price"] != DBNull.Value ? ((double)mRow["Price"]).ToString(MyUtility.MyConfig.DoubleFormat) : string.Empty;

                   
                    string[] Arr_Value = {  ShopID,ProductNameURL,ProductName,
                                         ImagePath,Price};
                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr_Value));
                }

                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByString(mTemplatePath, mBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
