using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.Shop
{
    public class MyProductBKC : MyBase
    {
        public int CateID = 0;
        public string CateName = "";

        public MyProductBKC(int CateID)
        {
            this.CateID = CateID;

            mTemplatePath = "~/Templates/Shop/TechProduct.htm";
            mTemplatePath_Repeat = "~/Templates/Shop/TechProduct_Repeat.htm";
            Init();
        }
        public MyProductBKC(int CateID,string CateName)
        {
            this.CateID = CateID;
            this.CateName = CateName;

            mTemplatePath = "~/Templates/Shop/TechProduct.htm";
            mTemplatePath_Repeat = "~/Templates/Shop/TechProduct_Repeat.htm";
            Init();
        }
        protected override string BuildHTML()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                //Lấy 1 phiên đấu giá Ngược (SMS) đang diễn ra, với độ ưu tiên cao nhất
                DataTable mTable = MyCommon.GetProductBKC(CateID);

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }

                foreach (DataRow mRow in mTable.Rows)
                {

                    string Price = mRow["Price"] != DBNull.Value ? ((double)mRow["Price"]).ToString(MyUtility.MyConfig.DoubleFormat) : string.Empty;
                    string[] Arr_Value = {  mRow["ProductName"].ToString(),
                                         mRow["ImagePath"].ToString(),
                                         Price};

                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, Arr_Value));
                }

                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { CateName, mBuilder.ToString() });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
