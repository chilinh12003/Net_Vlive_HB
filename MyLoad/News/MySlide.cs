using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;
namespace MyLoad.News
{
    /// <summary>
    /// Build 2 SLide Text và Slide Image
    /// </summary>
    public class MySlide : MyBase
    {

        public MySlide()
        {
            mTemplatePath = "~/Templates/News/Slide.htm";

            Init();
        }

        protected override string BuildHTML()
        {
            try
            {
                MySlideText mSlideText = new MySlideText();
                MySlideImage mSlideImage = new MySlideImage();

                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { mSlideText.GetHTML(), mSlideImage.GetHTML() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
