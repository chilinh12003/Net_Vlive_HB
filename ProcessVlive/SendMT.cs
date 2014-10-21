using System;
using System.Collections.Generic;
using System.Web;
using MyUtility;
namespace ProcessVlive
{
    public class SendMT
    {
        public string PrefixFileName = "_MO_MT_Log";
        public enum MessageType
        {
            /// <summary>
            /// tin nhắn dạng text
            /// </summary>
            text,
            /// <summary>
            /// nếu tin nhắn dạng Waplink
            /// </summary>
            bookmark,
            /// <summary>
            /// nếu tin nhắn có tiếng việt
            /// </summary>
            ucs2,
            /// <summary>
            /// Nếu tin nhắn hình
            /// </summary>
            picture,
            /// <summary>
            /// Nếu tinh nhắn là Logo
            /// </summary>
            logo,
            /// <summary>
            /// Nếu tin nhắn là dạng binary
            /// </summary>
            binary
        }

        public static string UserName
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("SendMT_UserName");
                if (string.IsNullOrEmpty(Temp))
                    return "hbgame";
                else
                    return Temp;
            }
            set { }
        }

        public static string Password
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("SendMT_Password");
                if (string.IsNullOrEmpty(Temp))
                    return "bf88d111114e02e8273b063f7bfc7ce1";
                else
                    return Temp;
            }
            set { }
        }

        public static string CPID
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("SendMT_CPID");
                if (string.IsNullOrEmpty(Temp))
                    return "10009";
                else
                    return Temp;
            }
            set { }
        }

        public string mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, serviceid, reqtime, procresult;

        public int Result = -1;

        /// <summary>
        /// Send MT, nếu là MT dài thì gửi theo MT dài
        /// </summary>
        /// <returns></returns>
        public int Send()
        {
            try
            {
                if (msgbody.Length > 160)
                    return SendSplitMT();

                WS_SendMT.Service mService = new ProcessVlive.WS_SendMT.Service();
                Result = mService.SendMT(mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, CPID, serviceid, reqtime, procresult, UserName, Password);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Send MT bao gồm các MT quảng cáo cheo (đã được thiết lập trên cms)
        /// </summary>
        /// <returns></returns>
        public int SendAdvertise()
        {
            try
            {
                //Cho biết bản tin text đã được gửi đi chưa
                bool HasSent_MTText = false;
                bool IsWappust = true;

                #region MyRegion
                MyVlive.Vlive.SMSAdvertise mSMSAdv = new MyVlive.Vlive.SMSAdvertise("SQlConnection_Vlive");

                System.Data.DataTable mTable = mSMSAdv.Select(3, cmdcode, src);
                if (mTable == null || mTable.Rows.Count < 1)
                {
                    return Send();
                }

                string Content = mTable.Rows[0]["Content"].ToString();
                string Wappush = mTable.Rows[0]["Wappush"].ToString(); 
                #endregion

                #region MyRegion
                if (string.IsNullOrEmpty(Wappush) && string.IsNullOrEmpty(Content))
                {
                    return Send();
                }

                //Kiểm tra  bản tin là wappush hay là text
                if(string.IsNullOrEmpty(Wappush) || Wappush.IndexOf("http:") < 0)
                {
                    IsWappust = false;
                }
                
                #endregion

                #region MyRegion
                List<string> mList = new List<string>();

                string FullMessage = msgbody;

                while (true)
                {
                    if (FullMessage.Length < 160)
                    {
                        mList.Add(FullMessage);
                        break;
                    }
                    int index = FullMessage.LastIndexOf(" ", 160);
                    mList.Add(FullMessage.Substring(0, index));
                    FullMessage = FullMessage.Substring(index);

                }
                //Nếu bản tin cuối cùng mà + MT quảng cáo (Dạng text) mà <160 ký tự thì thêm quảng cáo vào.
                if (IsWappust == false && mList[mList.Count - 1].Length + Content.Length < 159)
                {
                    mList[mList.Count - 1] = mList[mList.Count - 1] + " " + Content;
                    HasSent_MTText = true;
                }

                int i = 1;
                string Temp = msgbody;

                WS_SendMT.Service mService = new ProcessVlive.WS_SendMT.Service();

                foreach (string item in mList)
                {
                    if (HasSent_MTText)
                    {
                        mttotalseg = (mList.Count).ToString();
                    }
                    else
                    {
                        mttotalseg = (mList.Count + 1).ToString();
                    }
                    mtseqref = i.ToString();
                    reqtime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    msgbody = item;

                    Result = mService.SendMT(mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, CPID, serviceid, reqtime, procresult, UserName, Password);

                    MyLocalConfig.LogMT("_SplitSendMT", this);

                    //nếu như 1 MO có nhiều MT thì MT đầu tiên sẽ là tính cước, còn lại không tính cước.
                    procresult = "1";
                    i++;
                }

                if (IsWappust == false)
                {
                    //Gửi bản tin text cuối cùng
                    if (HasSent_MTText == false)
                    {
                        msgbody = Content;
                        this.msgtype = SendMT.MessageType.text.ToString();
                        mtseqref = i.ToString();
                        reqtime = DateTime.Now.ToString("yyyyMMddHHmmss");

                        //nếu MT chưa Được gửi ở trên thì gửi ở đây hehe
                        Result = mService.SendMT(mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, CPID, serviceid, reqtime, procresult, UserName, Password);
                    }
                }
                else
                {
                    //Gửi bản tin wappush cuối cùng
                    Wappush = Wappush.Replace(@"http://", "");
                    msgtitle = Content;
                    msgbody = Wappush;
                    this.msgtype = SendMT.MessageType.bookmark.ToString();
                    mtseqref = i.ToString();
                    reqtime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    Result = mService.SendMT(mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, CPID, serviceid, reqtime, procresult, UserName, Password);
                }

                MyLocalConfig.LogMT("_SplitSendMT", this);
                

                msgbody = Temp; 
                #endregion

                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gửi MT Dài
        /// </summary>
        /// <returns></returns>
        public int SendSplitMT()
        {
            try
            {
                List<string> mList = new List<string>();

                string FullMessage = msgbody;

                while (true)
                {
                   if(FullMessage.Length <160)
                   {
                       mList.Add(FullMessage);
                       break;
                   }
                   int index = FullMessage.LastIndexOf(" ", 160);
                   mList.Add(FullMessage.Substring(0,index));
                   FullMessage = FullMessage.Substring(index);

                }
                int i = 1;
                string Temp = msgbody;

                foreach (string item in mList)
                {
                    WS_SendMT.Service mService = new ProcessVlive.WS_SendMT.Service();
                    mttotalseg = mList.Count.ToString();
                    mtseqref = i.ToString();
                    reqtime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    msgbody = item;

                    Result = mService.SendMT(mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg,mtseqref, CPID, serviceid,reqtime, procresult, UserName, Password);

                    //Nếu như 1 MO có nhiều MT thì MT đầu tiên sẽ là tính cước, còn lại không tính cước.
                    procresult = "1";

                    i++;

                    MyLocalConfig.LogMT("_SplitSendMT", this);
                }

                msgbody = Temp;

                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
    }
}
