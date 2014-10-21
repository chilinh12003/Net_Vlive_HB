using System;
using System.Collections.Generic;
using System.Text;
using MyLoad.Base;
using MyAuction;
using System.Data;


namespace MyLoad.SMS
{
    public class MyDetail:MyBase
    {
        public MyAuction.SMS.SMSSession.SessionType SesstionTypeID = Session.SessionType.UpsideDown;

        MyAuction.SMS.SMSSession mSession = new MyAuction.SMS.SMSSession();

        public string SessionID = string.Empty;
        private int ProductID = 0;

        public MyDetail()
        {
            mTemplatePath = "~/Templates/SMS/SMSDetail.htm";

            Init();
        }

        public MyDetail(string SessionID)
            : this()
        {
            this.SessionID = SessionID;
        }

        public MyDetail(MyAuction.SMS.SMSSession.SessionType mSessionType)
            : this()
        {
            SesstionTypeID = mSessionType;
        }

        protected override string BuildHTML()
        {
            try
            {
                DataTable mTable = new DataTable();

                if (string.IsNullOrEmpty(SessionID))
                {
                    mTable = mSession.Select(4, "1", ((int)SesstionTypeID).ToString(), ((int)MyAuction.SMS.SMSSession.Status.Ongoing).ToString());
                }
                else
                {
                    mTable = mSession.Select(1, SessionID);
                }

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return string.Empty;
                }
                SessionID = mTable.Rows[0]["SessionID"].ToString();
                ProductID = (int)mTable.Rows[0]["ProductID"];

                MyDetail_Image mDetailImage = new MyDetail_Image(ProductID);
                MyDetail_Session mDetailSession = new MyDetail_Session(mTable);

                int CateNewsID = MyAuction.News.Rule_News_SMS_CateID;
                if (SesstionTypeID == Session.SessionType.Down)
                    CateNewsID = MyAuction.News.Rule_News_Down_CateID;
                else if (SesstionTypeID == Session.SessionType.Mark)
                    CateNewsID = MyAuction.News.Rule_News_Mark_CateID;

                MyLoad.News.MyDetail_Guide mDetailGuide = new MyLoad.News.MyDetail_Guide(CateNewsID);

                MyLoad.SMS.MyFinishSession mFinish = new MyLoad.SMS.MyFinishSession(SesstionTypeID);

                MyLoad.Shop.MyHomeShop mShop = new MyLoad.Shop.MyHomeShop(MyAuction.Shop.Shop.ShopStatus.Auction);
               

                string[] Arr_Value = {MyUtility.MyEnum.StringValueOf(SesstionTypeID).ToUpper(),
                                         mDetailImage.GetHTML(),
                                         mDetailSession.GetHTML(),
                                         mDetailGuide.GetHTML(),mFinish.GetHTML(),
                                         mShop.GetHTML()};
                // Lấy template từ file HTML
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, Arr_Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
