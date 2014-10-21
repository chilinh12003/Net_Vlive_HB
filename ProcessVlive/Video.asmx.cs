using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using MyUtility;
using System.Text;

namespace ProcessVlive
{
    /// <summary>
    /// Summary description for Video
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Video : System.Web.Services.WebService
    {

        [WebMethod]
        public int ReceiveMO(string moid, string moseq, string src, string dest, string cmdcode, string msgbody, string username, string password)
        {
            string PrefixFileName_Log = "_Clip_MO_MT_Log";
            int Result = 400;
            string ResultDesc = string.Empty;
            string MediaID = string.Empty;
            string LinkDownload = string.Empty;
            string Message = "Vlive Link Tai Clip ";
            int ResultSendMT = 0;

            SendMT mSendMT = new SendMT();
            GetContent mGetContent = new GetContent();

            try
            {
                #region Check parameter
                if (string.IsNullOrEmpty(moid) ||
                            string.IsNullOrEmpty(moseq) ||
                            string.IsNullOrEmpty(src) ||
                            string.IsNullOrEmpty(msgbody))
                {
                    ResultDesc = "Tham số truyền vào không chính xác.";
                    Result = 400;
                    return Result;
                }

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (!MyCheck.CheckPhoneNumber(ref src, ref mTelco, "84"))
                {
                    ResultDesc = "Số điện thoại không chính xác";
                    Result = 400;
                    return Result;
                }

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    ResultDesc = "Số điện thoại không thuộc mạng VNP";
                    Result = 400;
                    return Result;
                }

                #endregion

                //Xử lý MO lấy Mã Game
                #region MyRegion
                if (!MyLocalConfig.AnalyseMO(msgbody, ref MediaID, MyLocalConfig.VideoKeyword))
                {
                    ResultDesc = "Keyword không chính xác";
                    Result = 400;
                    return Result;
                }
                #endregion

                //Lấy link download
                #region MyRegion


                mGetContent.MOID = moid + "-" + moseq;
                mGetContent.MSISDN = src;
                mGetContent.MediaType = 3;
                mGetContent.ChannelType = 3;
                mGetContent.Price = MyLocalConfig.GetPriceByShortCode(dest);
                mGetContent.MediaID = MediaID;
                mGetContent.RequestTime = DateTime.Now.ToString("yyyyMMddhhmmssff");

                LinkDownload = mGetContent.GetLink();
                if (string.IsNullOrEmpty(LinkDownload))
                {
                    ResultDesc = "Không lấy được Link Download";
                    Result = 400;
                    return Result;
                }
                #endregion

                LinkDownload = LinkDownload.Replace(@"http://", "");

                //Gửi MT
                #region MyRegion
                mSendMT.mtseq = MySecurity.GenerateString(10);
                mSendMT.moid = moid;
                mSendMT.moseq = moseq;
                mSendMT.src = dest;
                mSendMT.dest = src;
                mSendMT.cmdcode = cmdcode;
                mSendMT.msgbody = LinkDownload;
                mSendMT.msgtype = SendMT.MessageType.bookmark.ToString();

                mSendMT.msgtitle = Message;
                mSendMT.mttotalseg = "1";
                mSendMT.mtseqref = "1";

                mSendMT.serviceid = string.Empty;
                mSendMT.reqtime = DateTime.Now.ToString("yyyyMMddHHmmss");
                mSendMT.procresult = "0";
                ResultSendMT = mSendMT.SendAdvertise();

                if (ResultSendMT != 200)
                {
                    ResultDesc = "Gửi MT không thành công ResultSendMT:" + ResultSendMT.ToString();
                    Result = 400;
                    return Result;
                } 
                #endregion

                Result = 200;
                ResultDesc = "Xử lý MO thành công";

                return Result;
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);
            }
            finally
            {
                //Ghi log thông tin MO nhận được
                MyLocalConfig.LogMO(PrefixFileName_Log, moid, moseq, src, dest, cmdcode, msgbody, username, password, Result.ToString(), ResultDesc);

                MyLocalConfig.LogMT(PrefixFileName_Log, mSendMT);
            }

            return Result;
        }

    }
}
