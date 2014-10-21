using System;
using System.Collections.Generic;
using System.Web;
using MyLoad.Base;
using MyLoad.Static;
using MyUtility;
using System.Text;
using MyLoad.News;
namespace MyWeb.p
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class rule : MyASHXBase
    {

        public override void WriteHTML()
        {
            try
            {
                MyHeader mHeader = new MyHeader();
                Write(mHeader.GetHTML());

                MyBanner mBanner = new MyBanner();
                Write(mBanner.GetHTML());

                MyMenu mMenu = new MyMenu(MyLoad.MyCommon.MenuPage.Rule);
                Write(mMenu.GetHTML());

                MyRule mRule = new MyRule();
                Write(mRule.GetHTML());
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

      
    }
}
