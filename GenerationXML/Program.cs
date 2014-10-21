using System;
using System.Collections.Generic;
using System.Text;
using MyUtility;
using MyVlive;
using System.Data;
using System.IO;
namespace GenerationXML
{
    class Program
    {
        static DataSet mSet_Update = new DataSet("Parent");
        
           

        static MyVlive.Vlive.GenerationXML mGen = new MyVlive.Vlive.GenerationXML();
        static MyVlive.Vlive.News mNews = new MyVlive.Vlive.News();

        static string Format_Vlive = MyFile.ReadFile(MyFile.GetFullPathFile("Templates/XML/Vlive.txt"));
        static string Format_BeforeTheClickMessage = MyFile.ReadFile(MyFile.GetFullPathFile("Templates/XML/BeforeTheClickMessage.txt"));
        static string Format_Menu = MyFile.ReadFile(MyFile.GetFullPathFile("Templates/XML/Menu.txt"));
        static string Format_MenuItem_Text = MyFile.ReadFile(MyFile.GetFullPathFile("Templates/XML/MenuItem_Text.txt"));
        static string Format_MenuItem_Download = MyFile.ReadFile(MyFile.GetFullPathFile("Templates/XML/MenuItem_Download.txt"));

        static int BeginDurationTime = 10;
        static int EndDurationTime = 10;

        static int TimeDelay = 60;
        static string ExportXMLPath = MyConfig.GetKeyInConfigFile("ExportXMLPath");

        public static void Sleep()
        {
            Console.WriteLine("--DUNG " + 60 + "(giay) VA SE CHAY TIEP--");
            System.Threading.Thread.Sleep(TimeDelay * 1000);
        }
        public static bool GenFileXML(string NewsID)
        {
            try
            {
                int CateID = 0;

                string VliveXML = mNews.GenerationXMLContent(ref CateID, NewsID, Format_Vlive, Format_BeforeTheClickMessage, Format_Menu, Format_MenuItem_Text, Format_MenuItem_Download);

                if (string.IsNullOrEmpty(VliveXML))
                {
                    Console.WriteLine("Tao XML cho NewsID:" + NewsID + " khong thanh cong!");
                    return false;
                }

                string ConfigFileName = MyVlive.Vlive.News.GetFileNameXML(CateID);

                string FileName = ExportXMLPath + ConfigFileName + ".xml";
                string FileName_ByDay = ExportXMLPath + ConfigFileName + "_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + Member.LoginName() + ".xml";
                int Name_Index = 1;

                while (File.Exists(FileName_ByDay))
                {
                    FileName_ByDay = ExportXMLPath + ConfigFileName + "_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + Member.LoginName() + "_" + Name_Index++.ToString() + ".xml";
                }

                if (File.Exists(FileName))
                {
                    File.Copy(FileName, FileName_ByDay);
                    File.Delete(FileName);
                }
                StreamWriter mTextWriter;
                mTextWriter = new StreamWriter(FileName, true, System.Text.Encoding.ASCII);

                mTextWriter.WriteLine(VliveXML);

                mTextWriter.Flush();
                mTextWriter.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateTableID()
        {
            try
            {
                DataTable tbl_ListID = new DataTable("Child");
                DataColumn col_1 = new DataColumn("ID", typeof(int));
                tbl_ListID.Columns.Add(col_1);

                mSet_Update.Tables.Add(tbl_ListID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GetConfig()
        {
            try
            {
                int.TryParse(MyConfig.GetKeyInConfigFile("TimeDelay"), out TimeDelay);
                int.TryParse(MyConfig.GetKeyInConfigFile("BeginDurationTime"), out BeginDurationTime);
                int.TryParse(MyConfig.GetKeyInConfigFile("EndDurationTime"), out EndDurationTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateIsCreateXML()
        {
            try
            {
                if (mSet_Update.Tables[0].Rows.Count < 1)
                    return;

                if (mGen.UpdateCreateXML(1, true, mSet_Update.GetXml()))
                {
                    Console.WriteLine("Cap nhat IsCreateXML vao DB thanh cong");
                }
                else
                {
                    Console.WriteLine("Cap nhat IsCreateXML vao DB KHONG thanh cong");
                }

                mSet_Update.Tables[0].Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void Main(string[] args)
        {
            try
            {
                GetConfig();

                CreateTableID();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);
                Console.WriteLine("----CO LOI KIA ADMIN, KIEM TRA LOG NHANH LEN NAO --> KHONG CHET CA LU BAY GIO");
            }

            Console.Clear();

        BEGIN:
            Console.WriteLine("--BAT DAU NAO ANH EM OI HO..HO..HO..");

            try
            {
                

            

                if (string.IsNullOrEmpty(ExportXMLPath))
                {
                    Console.WriteLine("Duong dan xuat file XML khong chinh xac");

                    Sleep();
                    goto BEGIN;
                }

                DataTable mTable = mGen.Select(2, "100");

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    Console.WriteLine("Khong co du lieu trong database");
                    Sleep();
                    goto BEGIN;
                }

                DateTime BeginDate = DateTime.Now.AddMinutes(-1*BeginDurationTime);
                DateTime EndDate = DateTime.Now.AddMinutes(EndDurationTime);

                foreach (DataRow mRow in mTable.Rows)
                {
                    string NewsID = mRow["NewsID"].ToString();
                    if (mRow["GenDate"] == DBNull.Value)
                        continue;

                    DateTime GenDate = (DateTime)mRow["GenDate"];

                    if (GenDate < BeginDate || GenDate > EndDate)
                    {
                        Console.WriteLine("NewsID:" + NewsID +"Co ngay generationXML khong nam trong khoang thoi gian duoc xuat");
                        continue;
                    }
                    

                    if (GenFileXML(NewsID))
                    {
                        Console.WriteLine("Tao XML thanh cong cho NewsID:" + NewsID);
                        DataRow mNewsRow = mSet_Update.Tables[0].NewRow();
                        mNewsRow["ID"] = NewsID;
                        mSet_Update.Tables[0].Rows.Add(mNewsRow);
                    }
                    else
                    {
                        Console.WriteLine("Tao XML KHONG thanh cong cho NewsID:" + NewsID);
                    }
                }
                
                UpdateIsCreateXML();

                Sleep();
                goto BEGIN;
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex);

                Console.WriteLine("----CO LOI KIA ADMIN, KIEM TRA LOG NHANH LEN NAO --> KHONG CHET CA LU BAY GIO");
                Sleep();
                goto BEGIN;
            }
            
        }
    }
}
