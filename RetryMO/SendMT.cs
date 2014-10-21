using System;
using System.Collections.Generic;
using System.Web;
using MyUtility;

namespace RetryMO
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
        public int Send()
        {
            try
            {
                if (msgbody.Length > 160)
                    return SendSplitMT();

                WS_SendMT.Service mService = new WS_SendMT.Service();
                Result = mService.SendMT(mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, CPID, serviceid, reqtime, procresult, UserName, Password);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SendSplitMT()
        {
            try
            {
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
                int i = 1;
                string Temp = msgbody;

                foreach (string item in mList)
                {
                    WS_SendMT.Service mService = new WS_SendMT.Service();
                    mttotalseg = mList.Count.ToString();
                    mtseqref = i.ToString();
                    reqtime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    msgbody = item;

                    Result = mService.SendMT(mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, CPID, serviceid, reqtime, procresult, UserName, Password);

                    //nếu như 1 MO có nhiều MT thì MT đầu tiên sẽ là tính cước, còn lại không tính cước.
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
