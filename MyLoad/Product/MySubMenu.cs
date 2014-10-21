using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;

namespace MyLoad.Product
{
    public class MySubMenu:MyBase
    {

        private int _CateID = MyAuction.Product.Product_CateID;
        public int CateID
        {
            get;
            set;
        }
        private int _CurrentCateID = 0;
        public int CurrentCateID
        {
            get;
            set;
        }
        public string FolderURL = "p";
        public string FileNameURL = "l_product";

         /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="mCateID">CateID của thể loại muốn build ListCate</param>
        public MySubMenu(int mCateID)
        {
            CateID = mCateID;
            mTemplatePath = "~/Templates/Product/SubMenu.htm";
            mTemplatePath_Repeat = "~/Templates/Product/SubMenu_Repeat.htm";

            Init();

        }

        public MySubMenu(int mCateID, int CurrentCateID)
            : this(mCateID)
        {
            this.CurrentCateID = CurrentCateID;
        }

        protected override void Begin()
        {
            //Nếu mà đường dẫn thay đổi thì phải xóa bỏ cache để đảm bảo đường dẫn của cate menu đúng với từng Folter
            if (!string.IsNullOrEmpty(PreviousKey) && !PreviousKey.Equals(FolderURL, StringComparison.CurrentCultureIgnoreCase))
                RemoveCache();
        }

        protected override string BuildHTML()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                Category mCate = new Category();
                DataTable mTable = mCate.Select(12, CateID.ToString(), "7");

                if (mTable == null || mTable.Rows.Count < 1)
                    return string.Empty;

                DataRow row_1 = mTable.NewRow();
                row_1["CateID"] = MyAuction.Product.Product_CateID;
                row_1["CateID_1"] = MyAuction.Product.Product_CateID_1;
                row_1["CateName"] = "Tất cả danh mục";
                mTable.Rows.InsertAt(row_1, 0);

                //Thêm 1 column chứa cateName dành cho rewrite URL
                DataColumn col_1 = new DataColumn("RewriteURL", typeof(string));
                mTable.Columns.Add(col_1);
                foreach (DataRow mRow in mTable.Rows)
                {
                    mRow["RewriteURL"] = MyCommon.CreateRewriteURL(mRow["CateName"].ToString());
                }

                string[] arr_column = { FolderURL, FileNameURL, "CateID", "RewriteURL", "CateName" };
                mBuilder.Append(mLoadTempLate.LoadTemplateByColumnName(mTemplatePath_Repeat, mTable, arr_column));

                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByString(mTemplatePath, mBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        protected override void Finish()
        {
            PreviousKey = FolderURL;
        }
    }
}
