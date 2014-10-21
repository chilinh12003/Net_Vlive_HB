using System;
using System.Collections.Generic;
using System.Web;
using MyLoad.Base;
using MyLoad.Static;
using MyUtility;
using System.Text;
namespace MyWeb.p
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class home : MyASHXBase
    {
        public override void WriteHTML()
        {
            try
            {
                MyHeader mHeader = new MyHeader();
                Write(mHeader.GetHTML());

                MyBanner mBanner = new MyBanner();
                Write(mBanner.GetHTML());

                MyMenu mMenu = new MyMenu(MyLoad.MyCommon.MenuPage.About);
                Write(mMenu.GetHTML());

                MyContent mContent = new MyContent();

                //Tạo 2 hàm để trả về nội dung của bên trái và bên phải của trang
                mContent.InsertHTML_Left_Change += new MyContent.InsertHTML_Left(mContent_InsertHTML_Left_Change);
                mContent.InsertHTML_Right_Change += new MyContent.InsertHTML_Right(mContent_InsertHTML_Right_Change);
                Write(mContent.GetHTML());
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex, false, MyNotice.EndUserError.LoadDataError, "Chilinh");
                Write(MyNotice.EndUserError.LoadDataError);
            }
            finally
            {
                MyFooter mFooter = new MyFooter();
                Write(mFooter.GetHTML());
            }
        }

        private string mContent_InsertHTML_Right_Change()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
               
                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string mContent_InsertHTML_Left_Change()
        {
            try
            {
                
                return string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
