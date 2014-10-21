using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyUtility;
namespace MyLoad
{
    public class MyCommon
    {
        public enum MenuPage
        {
            Home =1,
            About = 2,
            Guide =3,
            Rule = 4,
            Winner = 5,
            FAQ = 6,
            Contact = 7,
        }
        public static string DefaultImagePath = "../Images/NoImage.png";

        public static string GetFullPathImage(DataRow mProductRow, int GetImageOrder)
        {
            try
            {

                string FullPath = DefaultImagePath;

                switch (GetImageOrder)
                {
                    case 1: //Lấy hình ảnh từ Column ImagePath_1
                        if (mProductRow["ImagePath_1"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_1"].ToString();
                        else if (mProductRow["ImagePath_2"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_2"].ToString();
                        else if (mProductRow["ImagePath_3"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_3"].ToString();
                        break;
                    case 2: //Lấy hình ảnh từ Column ImagePath_2
                        if (mProductRow["ImagePath_2"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_2"].ToString();
                        else if (mProductRow["ImagePath_3"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_3"].ToString();
                        else if (mProductRow["ImagePath_1"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_1"].ToString();
                        break;
                    case 3: //Lấy hình ảnh từ Column ImagePath_3
                        if (mProductRow["ImagePath_3"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_3"].ToString();
                        else if (mProductRow["ImagePath_2"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_2"].ToString();
                        else if (mProductRow["ImagePath_1"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_1"].ToString();
                        break;
                    default:
                        if (mProductRow["ImagePath_3"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_3"].ToString();
                        else if (mProductRow["ImagePath_2"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_2"].ToString();
                        else if (mProductRow["ImagePath_1"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_1"].ToString();
                        break;
                }

                FullPath = MyConfig.ResourceLink + FullPath.Replace("~", "").Replace("..", "");
                return FullPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GetAdvantagePriceDistance(double CurrentPrice, ref double MinValue, ref double MaxValue)
        {
            try
            {
                Random mRandom = new Random();
                MinValue = 0;
                MaxValue = CurrentPrice;
                if (CurrentPrice < 10)
                {
                    MinValue = CurrentPrice;
                    MaxValue = mRandom.Next(50, 100);

                }
                else
                {
                    MinValue = mRandom.Next(1, 50);
                    if (MinValue < CurrentPrice)
                        MinValue = CurrentPrice - MinValue;
                    MaxValue = CurrentPrice + mRandom.Next(50, 100);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xóa bỏ tiếng việt và khoảng trắng
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string CreateRewriteURL(string Content)
        {
            try
            {
                Content = MyUtility.MyText.RemoveSignVietnameseString(Content);
                
                Content = Content.TrimEnd().TrimStart().Replace(" ", "-");
               // Content = Content.Replace("--", "-").Replace("..", ".");
                Content = MyUtility.MyText.RemoveSpecialChar(Content, "-");
                while (true)
                {
                    if (Content.IndexOf("--") < 0 )
                        break;
                    Content = Content.Replace("--", "-");
                }
                return Content;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
