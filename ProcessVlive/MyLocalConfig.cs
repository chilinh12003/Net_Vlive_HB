using System;
using System.Collections.Generic;
using System.Web;
using MyUtility;
using System.Text;
namespace ProcessVlive
{
    public class MyLocalConfig
    {
        public static string GameKeyWord
        {
            get
            {
                return MyConfig.GetKeyInConfigFile("GameKeyword");
            }
            set { }
        }
        public static string VideoKeyword
        {
            get
            {
                return MyConfig.GetKeyInConfigFile("VideoKeyword");
            }
            set { }
        }
        public static string ImageKeyword
        {
            get
            {
                return MyConfig.GetKeyInConfigFile("ImageKeyword");
            }
            set { }
        }
        public static string RingtoneKeyword
        {
            get
            {
                return MyConfig.GetKeyInConfigFile("RingtoneKeyword");
            }
            set { }
        }
        public static string TextBaseKeyword
        {
            get
            {
                return MyConfig.GetKeyInConfigFile("TextBaseKeyword");
            }
            set { }
        }
        public static string PacketKeyword
        {
            get
            {
                return MyConfig.GetKeyInConfigFile("PacketKeyword");
            }
            set { }
        }

        /// <summary>
        /// Lấy giá theo đầu số
        /// </summary>
        /// <param name="ShortCode"></param>
        /// <returns></returns>
        public static double GetPriceByShortCode(string ShortCode)
        {
            try
            {
                ShortCode = ShortCode.Trim();

                if (ShortCode == "9093")
                    return 500;
                if (ShortCode == "9193")
                    return 1000;
                if (ShortCode == "9293")
                    return 2000;
                if (ShortCode == "9393")
                    return 3000;
                if (ShortCode == "9493")
                    return 4000;
                if (ShortCode == "9593")
                    return 5000;
                if (ShortCode == "9693")
                    return 10000;
                if (ShortCode == "9793")
                    return 15000;

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Xử lý MO để lấy MediaID
        /// </summary>
        /// <param name="Info"></param>
        /// <param name="MediaID"></param>
        /// <returns></returns>
        public static bool AnalyseMO(string Info, ref string MediaID,string KeywordConfig)
        {
            Info = Info.ToUpper().Trim();
            Info = MyText.RemoveSpecialCharacters(Info);

            bool Result = false;
            try
            {
                string[] arr_keyword = KeywordConfig.Split('|');

                StringBuilder mBuilder_Number = new StringBuilder(string.Empty);
                foreach (string key in arr_keyword)
                {
                    if (Info.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Info = Info.Replace(key.ToUpper(), string.Empty);

                        foreach (char c in Info)
                        {
                            if ((c >= '0' && c <= '9'))
                            {
                                //Chỉ lấy các ký tự là số, vì MeidaID là số
                                mBuilder_Number.Append(c);
                            }
                        }
                        break;
                    }
                }

                if (mBuilder_Number.Length < 1)
                    return false;
                else
                {
                    MediaID = mBuilder_Number.ToString();
                }

                return true;
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);
            }
            return Result;
        }

        public static void LogMO(string PrefixFileName, string moid, string moseq, string src, string dest, string cmdcode, string msgbody, string username, string password, string Result, string ResultDesc)
        {
            try
            {
                string format_log = "MO--> moid:{0} || moseq:{1} || src:{2} || dest:{3} || cmdcode:{4} || msgbody:{5} || username:{6} || password:{7} || Result:{8} || ResultDesc:{9}";
                MyLogfile.WriteLogData(PrefixFileName, string.Format(format_log, moid, moseq, src, dest, cmdcode, msgbody, username, password, Result, ResultDesc));

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);
            }
        }
        public static void LogMT(string PrefixFileName, string mtseq, string moid, string moseq, string src, string dest, string cmdcode, string msgbody, string msgtype, string msgtitle, string mttotalseg, string mtseqref, string CPID, string serviceid, string reqtime, string procresult, string UserName, string password, string Result, string ResultDesc)
        {
            try
            {
                string format = "MT--> mtseq:{0} || moid:{1} || moseq:{2} || src:{3} || dest:{4} || cmdcode:{5} || msgbody:{6} || msgtype:{7} || msgtitle:{8} || mttotalseg:{9} || mtseqref:{10} || CPID:{11} || serviceid:{12} || reqtime:{13} || procresult:{14} || UserName:{15} || Password:{16} || Result:{17}";
                MyLogfile.WriteLogData(PrefixFileName, string.Format(format, mtseq, moid, moseq, src, dest, cmdcode, msgbody, msgtype, msgtitle, mttotalseg, mtseqref, CPID, serviceid, reqtime, procresult, UserName, string.Empty, Result));

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);
            }
        }
        public static void LogMT(string PrefixFileName, SendMT mSendMT)
        {
            try
            {
                LogMT(PrefixFileName,mSendMT.mtseq, mSendMT.moid, mSendMT.moseq, mSendMT.src, mSendMT.dest, mSendMT.cmdcode, mSendMT.msgbody, mSendMT.msgtype, mSendMT.msgtitle, mSendMT.mttotalseg, mSendMT.mtseqref, SendMT.CPID, mSendMT.serviceid, mSendMT.reqtime, mSendMT.procresult, SendMT.UserName, string.Empty, mSendMT.Result.ToString(), string.Empty);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);
            }
        }
    }
}
