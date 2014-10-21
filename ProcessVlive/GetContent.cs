using System;
using System.Collections.Generic;
using System.Web;
using MyUtility;
namespace ProcessVlive
{
    public class GetContent
    {
        public string PrefixFileName_Log = "_GetContent";
        public static string UserName
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("GetMedia_UserName");
                if (string.IsNullOrEmpty(Temp))
                    return "vlive";
                else
                    return Temp;

            }
            set { }
        }

        public static string Password
        {
            get
            {

                string Temp = MyConfig.GetKeyInConfigFile("GetMedia_Password");
                if (string.IsNullOrEmpty(Temp))
                    return "vlivee3d1adascss12";
                else
                    return Temp;
            }
            set { }
        }

        public string MSISDN
        {
            get;
            set;
        }

        public int MediaType
        {
            get;
            set;
        }

        public string MediaID
        {
            get;
            set;
        }

        public int ChannelType
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public string RequestTime
        {
            get;
            set;
        }

        public string MOID
        {
            get;
            set;
        }

        public string GetLink()
        {
            string Result = string.Empty;
            string LinkDownload = string.Empty;
            try
            {
                WS_GetMedia.GetMedia mGetMedia = new ProcessVlive.WS_GetMedia.GetMedia();
                Result = mGetMedia.GetLinkMedia(UserName, Password, MSISDN, MediaType, MediaID, ChannelType, Price, RequestTime);

                if (Result.StartsWith("0"))
                {
                    string[] arr = Result.Split('|');
                    if (arr.Length == 2)
                        return arr[1];
                    else
                        return string.Empty;
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                string format = "UserName:{0} || Password:{1} || MSISDN:{2} || MediaType:{3} || MediaID:{4} || ChannelType:{5} || Price:{6} || RequestTime:{7} || Resutl:{8} || MOID:{9}";
                MyLogfile.WriteLogData(PrefixFileName_Log, string.Format(format, UserName, string.Empty, MSISDN, MediaType.ToString(), MediaID, ChannelType.ToString(), Price.ToString(), RequestTime, Result, MOID));
            }
        }

     

        public string GetTextBase()
        {
            try
            {
                MyConnect.SQLServer.MyGetData mGet = new MyConnect.SQLServer.MyGetData();
                string[] para = { "Type", "Para_1" };
                string[] value = { "2", MediaID };
                System.Data.DataTable mTable = mGet.GetDataTable("Sp_STKText_Select", para, value);
                if (mTable == null || mTable.Rows.Count < 1)
                    return string.Empty;
                return mTable.Rows[0]["Content"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
