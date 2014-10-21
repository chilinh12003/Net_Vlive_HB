using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace MyVlive
{
    /// <summary>
    /// Cấu hình riêng cho dự án
    /// </summary>
    public class LocalConfig
    {
        /// <summary>
        /// Tỉ giá giữa Xeng trên VND
        /// </summary>
        public static double RateExchange = 1;
        /// <summary>
        /// Lấy thời gian hiện tại từ DB.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static DateTime GetDateFromDB(int Type)
        {
            try
            {
                MyConnect.SQLServer.MyExecuteData mExec = new MyConnect.SQLServer.MyExecuteData();
                MyConnect.SQLServer.MyGetData mGet = new MyConnect.SQLServer.MyGetData();
           
                string[] mpara = { "Type" };
                string[] mValue = { Type.ToString() };
                return (DateTime)mGet.GetExecuteScalar("Sp_SelectCurrentDate", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string AuctionDateFormat = "dd/MM-HH:mm";

        /// <summary>
        /// Danh sách các trang cần phân trang
        /// </summary>
        public enum ListPage
        {
            [DescriptionAttribute("Quản trị thể loại")]
            Categories,
            [DescriptionAttribute("Quản trị Menu")]
            MenuAdmin,
            [DescriptionAttribute("Cấu hình hệ thống")]
            SystemConfig,         
            [DescriptionAttribute("Nhóm thành viên")]
            MemberGroup,
            [DescriptionAttribute("Quản trị tài khoản")]
            Member,
            [DescriptionAttribute("Phần quyền")]
            Permission,
            [DescriptionAttribute("Log thành viên")]
            MemberLog,
            [DescriptionAttribute("Đổi mật khẩu")]
            ChangePass,
            [DescriptionAttribute("Quản trị Đối tác")]
            Partner,          
            [DescriptionAttribute("Quản trị Thể loại")]
            Category,
            [DescriptionAttribute("Quản trị Vlive")]
            News,
            [DescriptionAttribute("Xuất Excel")]
            ExportExcel,
            [DescriptionAttribute("Chăm sóc khách hàng")]
            CustomerCare,
            [DescriptionAttribute("Thiếp lập thời gian tạo XML")]
            GenerationXML,
            [DescriptionAttribute("Quảng cáo SMS")]
            SMSAdvertise,
        }

     
    }
}
