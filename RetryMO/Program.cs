using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using MyUtility;
namespace RetryMO
{
    class Program
    {
        public static void LogMO( string moid, string moseq, string src, string dest, string cmdcode, string msgbody, string username, string password, string Result, string ResultDesc)
        {
            try
            {
                string format_log = "RetryMO--> moid:{0} || moseq:{1} || src:{2} || dest:{3} || cmdcode:{4} || msgbody:{5} || username:{6} || password:{7} || Result:{8} || ResultDesc:{9}";
                MyLogfile.WriteLogData("_RetryMO", string.Format(format_log, moid, moseq, src, dest, cmdcode, msgbody, username, password, Result, ResultDesc));

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);
            }
        }

        public struct MOProcess
        {
            public string moid, moseq, src, dest, cmdcode, msgbody, username, password;
            public string Result;

            public bool RetryGame()
            {
                try
                {
                    WS_Game.Game mWSGame = new RetryMO.WS_Game.Game();
                    Result = mWSGame.ReceiveMO(moid, moseq, src, dest, cmdcode, msgbody, username, password).ToString();

                    LogMO(moid, moseq, src, dest, cmdcode, msgbody, username, password, Result.ToString(), "");
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError("_Error", ex);
                }

                if (Result == "200")
                    return true;
                else 
                    return false;
            }
            public bool RetryVideo()
            {
                try
                {
                    WS_Video.Video mVideo = new RetryMO.WS_Video.Video();
                    Result = mVideo.ReceiveMO(moid, moseq, src, dest, cmdcode, msgbody, username, password).ToString();

                    LogMO(moid, moseq, src, dest, cmdcode, msgbody, username, password, Result.ToString(), "");
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError("_Error", ex);
                }

                if (Result == "200")
                    return true;
                else
                    return false;
            }

            public bool RetryImage()
            {
                try
                {
                    WS_Image.Image mImage = new RetryMO.WS_Image.Image();
                    Result = mImage.ReceiveMO(moid, moseq, src, dest, cmdcode, msgbody, username, password).ToString();

                    LogMO(moid, moseq, src, dest, cmdcode, msgbody, username, password, Result.ToString(), "");
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError("_Error", ex);
                }

                if (Result == "200")
                    return true;
                else
                    return false;
            }
            public bool RetryRingtone()
            {
                try
                {
                    WS_Ringtone.Ringtone mRingtone = new RetryMO.WS_Ringtone.Ringtone();
                    Result = mRingtone.ReceiveMO(moid, moseq, src, dest, cmdcode, msgbody, username, password).ToString();

                    LogMO(moid, moseq, src, dest, cmdcode, msgbody, username, password, Result.ToString(), "");
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError("_Error", ex);
                }

                if (Result == "200")
                    return true;
                else
                    return false;
            }
            public bool RetryTextbase()
            {
                try
                {
                    WS_TextBase.TextBase mTextBase = new RetryMO.WS_TextBase.TextBase();
                    Result = mTextBase.ReceiveMO(moid, moseq, src, dest, cmdcode, msgbody, username, password).ToString();

                    LogMO(moid, moseq, src, dest, cmdcode, msgbody, username, password, Result.ToString(), "");
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError("_Error", ex);
                }

                if (Result == "200")
                    return true;
                else
                    return false;
            }

            public bool RetryPacket()
            {
                try
                {
                    WS_Packet.Packet mWSPacket = new RetryMO.WS_Packet.Packet();
                    Result = mWSPacket.ReceiveMO(moid, moseq, src, dest, cmdcode, msgbody, username, password).ToString();

                    LogMO(moid, moseq, src, dest, cmdcode, msgbody, username, password, Result.ToString(), "");
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError("_Error", ex);
                }

                if (Result == "200")
                    return true;
                else
                    return false;
            }
        }
        enum KeywordType 
        {
            Nothing = 0,
            Game = 1,
            Image,
            Ringtone,
            Video,
            TextBase,
            Packet
        }


        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Bat dau chay");
                MORetry mMORetry = new MORetry("Oracle_Connection");

                DataTable mTable = mMORetry.Search(0);

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    Console.WriteLine("Khong co du lieu trong db");
                    Console.WriteLine("Chuong trinh se tat trong 1 phut toi");
                    System.Threading.Thread.Sleep(60000);
                    return;
                }

                Dictionary<string, string> ListMOID_Send = new Dictionary<string, string>();

                int Count = 0;
                while (mTable!= null&& mTable.Rows.Count > 0)
                {
                    if (Count > 10)
                    {
                        Console.WriteLine("Co hien tuong lap tin");
                        break;
                    }
                    string ListMOID = string.Empty;

                    foreach (DataRow mRow in mTable.Rows)
                    {
                        MOProcess mMOProcess = new MOProcess();
                        mMOProcess.moid = mRow["MO_ID"].ToString();
                        mMOProcess.moseq = mRow["MO_ID_ORIGIN"].ToString();
                        mMOProcess.cmdcode = mRow["MO_KEYWORD"].ToString();
                        mMOProcess.dest = mRow["MO_SHORTCODE"].ToString();
                        mMOProcess.msgbody = mRow["MO_MSG"].ToString();
                        mMOProcess.password = string.Empty;
                        mMOProcess.src = mRow["MO_MSISDN"].ToString();
                        mMOProcess.username = string.Empty;

                        if (ListMOID_Send.ContainsKey(mMOProcess.moid))
                        {
                            Count++;
                            continue;
                        }

                        ListMOID_Send.Add(mMOProcess.moid, mMOProcess.src);

                        KeywordType CurrentType = CheckKeyword(mMOProcess.cmdcode);
                        switch (CurrentType)
                        {
                            #region MyRegion
                            case KeywordType.Game:
                                mMOProcess.RetryGame();
                                break;
                            case KeywordType.Video:
                                mMOProcess.RetryVideo();
                                break;
                            case KeywordType.Image:
                                mMOProcess.RetryImage();
                                break;
                            case KeywordType.Ringtone:
                                mMOProcess.RetryRingtone();
                                break;
                            case KeywordType.TextBase:
                                mMOProcess.RetryTextbase();
                                break;
                            case KeywordType.Packet:
                                mMOProcess.RetryPacket();
                                break;
                            default:
                                break;
                            #endregion
                        }
                        if (CurrentType == KeywordType.Nothing || mMOProcess.Result != "200")
                        {
                            //Nếu keyword không đúng thì sendMT Invalid
                            SendMT mSendMT = new SendMT();
                            mSendMT.cmdcode = mMOProcess.cmdcode;
                            mSendMT.dest = mMOProcess.src;
                            mSendMT.moid = mMOProcess.moid;
                            mSendMT.moseq = mMOProcess.moseq;
                            mSendMT.msgbody = MyLocalConfig.MT_Invalid;
                            mSendMT.msgtitle = string.Empty;
                            mSendMT.msgtype = SendMT.MessageType.text.ToString();

                            mSendMT.mttotalseg = "1";
                            mSendMT.mtseqref = "1";

                            mSendMT.serviceid = string.Empty;
                            mSendMT.reqtime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            mSendMT.procresult = "0";
                            mSendMT.Send();
                            MyLocalConfig.LogMT("_MTLog", mSendMT);
                        }

                        Console.WriteLine("Xu ly xong MOID:" + mMOProcess.moid + "||USERID:" + mMOProcess.src);

                        if (string.IsNullOrEmpty(ListMOID))
                            ListMOID = mMOProcess.moid;
                        else
                            ListMOID += ", " + mMOProcess.moid;
                    }

                    if (mMORetry.Delete(ListMOID))
                    {
                        Console.WriteLine("Xoa du lieu thanh cong cac ID:" + ListMOID);
                        MyLogfile.WriteLogData("_DELETE_ROW", "Xoa thanh cong LisMOID:" + ListMOID);
                    }
                    else
                    {
                        Console.WriteLine("Xoa du lieu KHONG thanh cong cac ID:" + ListMOID);
                        MyLogfile.WriteLogData("_DELETE_ROW", "Xoa KHONG thanh cong LisMOID:" + ListMOID);
                    }
                    Console.WriteLine("Ket thuc xu ly 10 row");
                    Console.WriteLine("----------------------------------------------------");
                    Console.WriteLine("Bat dau lay du lieu moi");
                    mTable = mMORetry.Search(0);
                }
                Console.WriteLine("Da Retry xong MO");
                Console.WriteLine("Chuong trinh se tat trong 1 phut toi");
                System.Threading.Thread.Sleep(60000);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);
            }
        }

        static KeywordType CheckKeyword(string Keyword)
        {
            try
            {
                Keyword = Keyword.ToUpper();
                if (MyLocalConfig.GameKeyWord.ToUpper().IndexOf(Keyword) >= 0)
                {
                    return CheckCorrectKeyword(Keyword, KeywordType.Game);
                }
                else if (MyLocalConfig.VideoKeyword.ToUpper().IndexOf(Keyword) >= 0)
                {
                    return CheckCorrectKeyword(Keyword, KeywordType.Video);
                }
                else if (MyLocalConfig.ImageKeyword.ToUpper().IndexOf(Keyword) >= 0)
                {
                    return CheckCorrectKeyword(Keyword, KeywordType.Image);
                }
                else if (MyLocalConfig.RingtoneKeyword.ToUpper().IndexOf(Keyword) >= 0)
                {
                    return CheckCorrectKeyword(Keyword, KeywordType.Ringtone);
                }
                else if (MyLocalConfig.TextBaseKeyword.ToUpper().IndexOf(Keyword) >= 0)
                {
                    return CheckCorrectKeyword(Keyword, KeywordType.TextBase);
                }
                else if (MyLocalConfig.PacketKeyword.ToUpper().IndexOf(Keyword) >= 0)
                {
                    return CheckCorrectKeyword(Keyword, KeywordType.Packet);
                }
                else
                {
                    return CheckCorrectKeyword(Keyword, KeywordType.Nothing);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static KeywordType CheckCorrectKeyword(string Keyword, KeywordType TemType)
        {
            try
            {
                KeywordType CorrectType = KeywordType.Nothing;

                string[] ListKeyword_Game = MyLocalConfig.GameKeyWord.ToUpper().Split('|');
                string[] ListKeyword_Video = MyLocalConfig.VideoKeyword.ToUpper().Split('|');
                string[] ListKeyword_Image = MyLocalConfig.ImageKeyword.ToUpper().Split('|');
                string[] ListKeyword_Ringtone = MyLocalConfig.RingtoneKeyword.ToUpper().Split('|');
                string[] ListKeyword_TextBase = MyLocalConfig.TextBaseKeyword.ToUpper().Split('|');
                string[] ListKeyword_Packet = MyLocalConfig.PacketKeyword.ToUpper().Split('|');
                switch (TemType)
                {
                    case KeywordType.Game:
                        #region MyRegion
                        foreach (string item in ListKeyword_Game)
                        {
                            if (item == Keyword)
                            {
                                return KeywordType.Game;
                            }
                        }
                        #endregion
                        break;
                    case KeywordType.Image:
                        #region MyRegion
                        foreach (string item in ListKeyword_Image)
                        {
                            if (item == Keyword)
                            {
                                return KeywordType.Image;
                            }
                        }
                        #endregion
                        break;
                    case KeywordType.Ringtone:
                        #region MyRegion
                        foreach (string item in ListKeyword_Ringtone)
                        {
                            if (item == Keyword)
                            {
                                return KeywordType.Ringtone;
                            }
                        }
                        #endregion
                        break;
                    case KeywordType.TextBase:
                        #region MyRegion
                        foreach (string item in ListKeyword_TextBase)
                        {
                            if (item == Keyword)
                            {
                                return KeywordType.TextBase;
                            }
                        }
                        #endregion
                        break;
                    case KeywordType.Video:
                        #region MyRegion
                        foreach (string item in ListKeyword_Video)
                        {
                            if (item == Keyword)
                            {
                                return KeywordType.Video;
                            }
                        }
                        #endregion
                        break;
                    case KeywordType.Packet:
                        #region MyRegion
                        foreach (string item in ListKeyword_Packet)
                        {
                            if (item == Keyword)
                            {
                                return KeywordType.Packet;
                            }
                        }
                        #endregion
                        break;

                }

                //nếu là không có trong keyword nào thì kiểm tra full
                #region MyRegion
                foreach (string item in ListKeyword_Game)
                {
                    if (item == Keyword)
                    {
                        return KeywordType.Game;
                    }
                }
                foreach (string item in ListKeyword_Image)
                {
                    if (item == Keyword)
                    {
                        return KeywordType.Image;
                    }
                }
                foreach (string item in ListKeyword_Ringtone)
                {
                    if (item == Keyword)
                    {
                        return KeywordType.Ringtone;
                    }
                }
                foreach (string item in ListKeyword_TextBase)
                {
                    if (item == Keyword)
                    {
                        return KeywordType.TextBase;
                    }
                }
                foreach (string item in ListKeyword_Video)
                {
                    if (item == Keyword)
                    {
                        return KeywordType.Video;
                    }
                }
                foreach (string item in ListKeyword_Packet)
                {
                    if (item == Keyword)
                    {
                        return KeywordType.Packet;
                    }
                }
                #endregion

                return CorrectType;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
   
}

