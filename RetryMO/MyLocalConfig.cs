using System;
using System.Collections.Generic;
using System.Web;
using MyUtility;
using System.Text;

namespace RetryMO
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
        public static string MT_Invalid
        {
            get
            {
                return MyConfig.GetKeyInConfigFile("MT_Invalid");
            }
            set { }
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
