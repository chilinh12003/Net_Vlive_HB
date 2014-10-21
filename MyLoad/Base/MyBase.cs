using System;
using System.Collections.Generic;
using System.Text;
using MyUtility;
namespace MyLoad.Base
{
    public abstract class MyBase
    {
        /// <summary>
        /// chứa danh sách các module được phép cache, và được khởi tạo khi application bắt đấu chạy
        /// <para>
        /// VD: "Myload.Static.MyBanner",true,5
        /// </para>
        /// </summary>
        public static System.Data.DataTable ModuleCacheList = new System.Data.DataTable();

        // Lấy đường dẫn file HTML
        // Tham số {0} sẽ được truyền vào sau.

        public string mTemplatePath;
        public string mTemplatePath_Repeat;
        public string mTemplatePath_Repeat_1;

        /// <summary>
        /// Cho phép Module này ẩn hay hiện (Disable = true thì sẽ trả về chuỗi HTML rỗng) nghĩa là không hiển thị gì cả
        /// </summary>
        public bool Disable = false;

        public MyLoadTemplate mLoadTempLate;

        public MyCommon.MenuPage CurrentPage = MyCommon.MenuPage.Home;

        /// <summary>
        /// Key để lưu dữ liệu cate của HMTL, nhằm  mục đích không phải load lại nhiều lần
        /// </summary>
        public string CacheKey;

        /// <summary>
        /// Cho phép đối tượng này được phép catching HTML sau khi build ra hay không
        /// </summary>
        public bool AllowCache = false;

        /// <summary>
        /// Khởi tạo các giá trị đầu tiên
        /// </summary>
        public void Init()
        {
            mLoadTempLate = new MyLoadTemplate();
            CacheKey = this.GetType().Namespace + "." + this.GetType().Name;
        }

        public MyBase()
        {
            Init();
        }

        /// <summary>
        /// Thời gian cho phép catching trên class được tính bằng Giây
        /// </summary>
        public int CacheTime = 300;


        /// <summary>
        /// Lưu giữ một dữ liệu của đối tượng được tạo (lần kề trước). để phục vụ cho kiểm tra xem có được phép removeCache hay không
        /// 
        /// </summary>
        public string PreviousKey
        {
            get
            {
                if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Session != null && MyCurrent.CurrentPage.Session[CacheKey] != null)
                {
                    return MyCurrent.CurrentPage.Session[CacheKey].ToString();
                }
                return string.Empty;
            }
            set
            {
                if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Session != null)
                {
                    MyCurrent.CurrentPage.Session[CacheKey] = value;
                }
            }
        }

        /// <summary>
        /// Hàm được chạy trước khi thực hiện trả về chuỗi HMTL
        /// </summary>
        protected virtual void Begin()
        {

        }

        /// <summary>
        /// Hàm chạy cuối cùng của đối tượng
        /// </summary>
        protected virtual void Finish()
        {

        }

        /// <summary>
        /// Trả về chuổi HTML cần Build của lớp này đồng thời trước khi trả thì cache lại
        /// </summary>
        /// <returns></returns>
        public string GetHTML()
        {
            Begin();

            if (Disable)
                return string.Empty;

            try
            {
                //Lấy thông tin về cache cho từng module.
                #region MyRegion
                if (ModuleCacheList != null && ModuleCacheList.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow mRow in ModuleCacheList.Rows)
                    {
                        if (mRow["ClassName"].ToString().Equals(this.GetType().FullName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (mRow["IsCache"] != DBNull.Value)
                            {
                                bool.TryParse(mRow["IsCache"].ToString(), out AllowCache);
                            }
                            if (mRow["CacheTime"] != DBNull.Value)
                                int.TryParse(mRow["CacheTime"].ToString(), out CacheTime);
                        }
                    }
                } 
                #endregion

                //nếu module cho phép cache thì tiến hành cache với key và tên namepace.Classname của module này
                if (AllowCache)
                {
                    #region MyRegion
                    if (MyUtility.MyCurrent.CurrentPage != null && MyUtility.MyCurrent.CurrentPage.Cache != null && MyUtility.MyCurrent.CurrentPage.Cache[CacheKey] != null)
                    {
                        try
                        {
                            return MyUtility.MyCurrent.CurrentPage.Cache[CacheKey].ToString();
                        }
                        catch (Exception ex)
                        {
                            MyLogfile.WriteLogError("Cache_Error", ex);
                            return BuildHTML();
                        }
                    }
                    else
                    {
                        string HTML = BuildHTML();
                        MyUtility.MyCurrent.CurrentPage.Cache.Add(CacheKey, HTML, null, DateTime.Now.AddSeconds(CacheTime), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                        return HTML;
                    } 
                    #endregion
                }
                else
                {
                    return BuildHTML();
                }
            }
            catch (Exception ex)
            {
                MyUtility.MyLogfile.WriteLogError("MyLoad", ex);
                throw ex;
                
            }
            finally
            {
                Finish();
            }
        }

        /// <summary>
        /// Xóa bảo cache cho một module
        /// </summary>
        /// <returns></returns>
        public bool RemoveCache()
        {
            try
            {
                if (MyUtility.MyCurrent.CurrentPage != null && MyUtility.MyCurrent.CurrentPage.Cache != null && MyUtility.MyCurrent.CurrentPage.Cache[CacheKey] != null)
                {

                    MyUtility.MyCurrent.CurrentPage.Cache.Remove(CacheKey);

                    if (MyUtility.MyCurrent.CurrentPage.Cache[CacheKey] != null)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Build lên chuỗi HTML của lớp
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildHTML()
        {
            return string.Empty;
        }
       
    }
}
