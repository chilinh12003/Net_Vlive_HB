using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyUtility;
using MyVlive.Vlive;
namespace MyAjax.Ajax
{
    public class MyVlive : MyAjaxBase
    {
        News mNews = new News();

        public void EditLetter()
        {
            try
            {
                DataSet mSet = mNews.GetDataSet_Session();
                DataTable mTable = mSet.Tables["Letter"];

                int LetterID = 0;
                GetParemeter<int>(ref LetterID, "LetterID");

                if (LetterID < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Warring, "Xin hãy chọn chính xác bản tin cần sửa"));
                    return;
                }

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Warring, "Không có dữ liệu về nội dung bản tin"));
                    return;
                }
                string Format_Letter = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/Letter_Update.htm"));

                mTable.DefaultView.RowFilter = " LetterID = " + LetterID.ToString();
                if (mTable.DefaultView.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Warring, "Không có dữ liệu về nội dung bản tin"));
                    return;
                }
                DataRowView mView = mTable.DefaultView[0];
                DateTime PushTime = DateTime.MinValue;

                if (!DateTime.TryParseExact(mView["PushTime"].ToString(), MyConfig.DateFormat_InsertToDB, null, System.Globalization.DateTimeStyles.None, out PushTime))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Warring, "Xin hãy nhập chính xác ngày phát tin."));
                    return;
                }

                int Hour = PushTime.Hour;
                int Minute = PushTime.Minute;
                int Second = PushTime.Second;

                string Format_Option = "<option value=\"{0}\" {1}>{2}</option>";
                StringBuilder mBuilder_Hour = new StringBuilder("<option value=\"0\">--Giờ--</option>");
                StringBuilder mBuilder_Minute = new StringBuilder("<option value=\"0\">--Phút--</option>");
                StringBuilder mBuilder_Second = new StringBuilder("<option value=\"0\">--Giây--</option>");
                
                string Selected = string.Empty;

                for (int i = 1; i <= 23; i++)
                {
                    
                    if(Hour == i)
                        Selected = "selected=\"selected\"";
                    
                    mBuilder_Hour.Append(string.Format(Format_Option, i.ToString(),Selected, i.ToString()));
                    Selected = string.Empty;
                }
                for (int i = 1; i <= 60; i++)
                {
                    if (Minute == i)
                        Selected = "selected=\"selected\"";
                    mBuilder_Minute.Append(string.Format(Format_Option, i.ToString(), Selected, i.ToString()));
                    Selected = string.Empty;

                    if (Second == i)
                        Selected = "selected=\"selected\"";
                    mBuilder_Second.Append(string.Format(Format_Option, i.ToString(), Selected, i.ToString()));
                    Selected = string.Empty;
                }
               

                string HMTL = string.Format(Format_Letter, mView["LetterName"].ToString(),
                                                            PushTime.ToString("dd/MM/yyyy"),
                                                            mBuilder_Hour.ToString(),
                                                            mBuilder_Minute.ToString(),
                                                            mBuilder_Second.ToString(),
                                                            mView["Priority"].ToString(),
                                                            LetterID.ToString());

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, HMTL, MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }
        public void SaveLetter()
        {
            try
            {
                bool IsEdit = false;

                DataSet mSet = mNews.GetDataSet_Session();
                DataTable mTable = mSet.Tables["Letter"];
                DataRow mRow = mTable.NewRow();

                int LetterID = 0;
                GetParemeter<int>(ref LetterID, "LetterID");

                int NewsID = 0;
                GetParemeter<int>(ref NewsID, "NewsID");

                string LetterName = string.Empty;
                GetParemeter<string>(ref LetterName, "LetterName");

                int Priority = 0;
                GetParemeter<int>(ref Priority, "Priority");

                string PushTime = string.Empty;
                GetParemeter<string>(ref PushTime, "PushTime");

                if (string.IsNullOrEmpty(LetterName))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Warring, "Xin hãy nhập tên bản tin."));
                    return;
                }
                if (string.IsNullOrEmpty(PushTime))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Warring, "Xin hãy nhập ngày phát tin."));
                    return;
                }
                DateTime TempDate = DateTime.MinValue;

                if (!DateTime.TryParseExact(PushTime, "dd/MM/yyyyTH:m:s", null, System.Globalization.DateTimeStyles.None, out TempDate))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Warring, "Xin hãy nhập chính xác ngày phát tin."));
                    return;
                }

                if (LetterID < 1)
                {
                    //Nếu là thêm mới thì tạo tạm 1 id, ID không được lưu vào db
                    LetterID = mTable.Rows.Count + 1;
                }
                else
                {
                    IsEdit = true;
                    DataRow[] Arr_Row = mTable.Select(" LetterID = " + LetterID.ToString());
                    if (Arr_Row.Length > 0)
                        mRow = Arr_Row[0];
                }
              
                mRow["LetterID"] = LetterID;
                mRow["NewsID"] = NewsID;
                mRow["LetterName"] = LetterName;
                mRow["Priority"] = Priority;
                mRow["PushTime"] = TempDate.ToString(MyConfig.DateFormat_InsertToDB);

                if (!IsEdit)
                    mTable.Rows.Add(mRow);

                mSet.AcceptChanges();

                mNews.SetDataSet_Session(mSet);

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void SaveRecord()
        {
            try
            {
                bool IsEdit = false;
                DataSet mSet = mNews.GetDataSet_Session();
                DataTable mTable = mSet.Tables["Record"];
                DataRow mRow = mTable.NewRow();

                #region MyRegion
                int RecordID = 0;
                GetParemeter<int>(ref RecordID, "RecordID");

                int LetterID = 0;
                GetParemeter<int>(ref LetterID, "LetterID");

                int RecordTypeID = 0;
                GetParemeter<int>(ref RecordTypeID, "RecordTypeID");

                int Priority = 0;
                GetParemeter<int>(ref Priority, "Priority");

                string RecordName = string.Empty;
                GetParemeter<string>(ref RecordName, "RecordName");

                string Introduction = string.Empty;
                GetParemeter<string>(ref Introduction, "Introduction");

                string Content = string.Empty;
                GetParemeter<string>(ref Content, "Content");

                int MethodID = 0;
                GetParemeter<int>(ref MethodID, "MethodID");

                string ServiceID = string.Empty;
                GetParemeter<string>(ref ServiceID, "ServiceID");

                string Keyword = string.Empty;
                GetParemeter<string>(ref Keyword, "Keyword");

                bool IsParent = false;
                GetParemeter<bool>(ref IsParent, "IsParent");

                int ParentID = 0;
                GetParemeter<int>(ref ParentID, "ParentID");

                #endregion

                if (RecordID < 1)
                {
                    //Nếu là thêm mới thì tạo tạm 1 id, ID không được lưu vào db
                    RecordID = mTable.Rows.Count + 1;
                }
                else
                {
                    IsEdit = true;

                    DataRow[] Arr_Row = mTable.Select(" RecordID = " + RecordID.ToString());

                    if (Arr_Row.Length > 0)
                        mRow = Arr_Row[0];
                }
                if (Introduction.Length > 15)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Lời dẫn không được vượt quá 15 ký tự. xin hãy nhập lại"));
                    return;
                }
                if (Content.Length > 450)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Nội dung không được vượt quá 450 ký tự. xin hãy nhập lại"));
                    return;
                }
                if (ParentID > 0)
                    IsParent = false;

                mRow["RecordID"] = RecordID;
                mRow["LetterID"] = LetterID;
                mRow["IsParent"] = IsParent;
                mRow["ParentID"] = ParentID;
                mRow["RecordTypeID"] = RecordTypeID;
                mRow["Priority"] = Priority;
                mRow["RecordName"] = RecordName;
                mRow["Introduction"] = Introduction;
                mRow["Content"] = Content;
                mRow["MethodID"] = MethodID;
                mRow["ServiceID"] = ServiceID;
                mRow["Keyword"] = Keyword;

                if (!IsEdit)
                    mTable.Rows.Add(mRow);

                mSet.AcceptChanges();

                mNews.SetDataSet_Session(mSet);

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void ViewRecord()
        {
            try
            {
                DataSet mSet = mNews.GetDataSet_Session();
                DataTable mTable = mSet.Tables["Record"];

                int LetterID = 0;
                GetParemeter<int>(ref LetterID, "LetterID");

                int ParentID = 0;
                GetParemeter<int>(ref ParentID, "ParentID");

                bool IsParent = false;
                GetParemeter<bool>(ref IsParent, "IsParent");

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.NoData));
                    return;
                }
                mTable.DefaultView.Sort = " Priority DESC, LetterID ASC ";
                mTable.DefaultView.RowFilter = " LetterID = " + LetterID.ToString();

                //Nếu là mẩu tin con
                if (ParentID > 0)
                {
                    mTable.DefaultView.RowFilter += " AND ParentID = " + ParentID.ToString();
                }
                else
                {
                    mTable.DefaultView.RowFilter += " AND (IsParent = true OR IsParent ='1' OR ParentID='0')";
                }
                if (mTable.DefaultView.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.NoData));
                    return;
                }
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                string Format_Download = string.Empty;
                string Format_Text = string.Empty;

                if (ParentID > 0)
                {
                    Format_Download = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/Record_Child_Repeat_Download.htm"));
                    Format_Text = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/Record_Child_Repeat_Text.htm"));

                    for (int i = 0; i < mTable.DefaultView.Count; i++)
                    {
                        if ((Record.RecordType)int.Parse(mTable.DefaultView[i]["RecordTypeID"].ToString()) == Record.RecordType.SMSDownload)
                        {
                            mBuilder.Append(string.Format(Format_Download, mTable.DefaultView[i]["RecordName"].ToString(),
                                                                        mTable.DefaultView[i]["RecordID"].ToString(),
                                                                        mTable.DefaultView[i]["ServiceID"].ToString(),
                                                                        mTable.DefaultView[i]["Keyword"].ToString(),
                                                                        ParentID.ToString()));
                        }
                        else
                        {
                            mBuilder.Append(string.Format(Format_Text, mTable.DefaultView[i]["RecordName"].ToString(),
                                                                        mTable.DefaultView[i]["RecordID"].ToString(),
                                                                        mTable.DefaultView[i]["Content"].ToString(),
                                                                        ParentID.ToString()));
                        }
                    }
                }
                else
                {
                    Format_Download = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/Record_Parent_Repeat_Download.htm"));
                    Format_Text = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/Record_Parent_Repeat_Text.htm"));

                    for (int i = 0; i < mTable.DefaultView.Count; i++)
                    {
                        if ((Record.RecordType)int.Parse(mTable.DefaultView[i]["RecordTypeID"].ToString()) == Record.RecordType.SMSDownload)
                        {
                            mBuilder.Append(string.Format(Format_Download, mTable.DefaultView[i]["RecordName"].ToString(),
                                                                        mTable.DefaultView[i]["RecordID"].ToString(),
                                                                        mTable.DefaultView[i]["ServiceID"].ToString(),
                                                                        mTable.DefaultView[i]["Keyword"].ToString(),
                                                                        LetterID.ToString()));
                        }
                        else
                        {
                            mBuilder.Append(string.Format(Format_Text, mTable.DefaultView[i]["RecordName"].ToString(),
                                                                        mTable.DefaultView[i]["RecordID"].ToString(),
                                                                        mTable.DefaultView[i]["Content"].ToString(),
                                                                        LetterID.ToString()));
                        }
                    }
                }

               

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success,mBuilder.ToString(), MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void ViewLetter()
        {
            try
            {
                DataSet mSet = mNews.GetDataSet_Session();
                DataTable mTable = mSet.Tables["Letter"];
                mTable.DefaultView.Sort = " Priority DESC ";
               
                if (mTable.DefaultView.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.NoData));
                    return;
                }
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                string Format_Letter = MyFile.ReadFile(MyFile.GetFullPathFile("~/Templates/Letter_Repeat.htm"));
                    
                for (int i = 0; i < mTable.DefaultView.Count; i++)
                {

                    mBuilder.Append(string.Format(Format_Letter, mTable.DefaultView[i]["LetterName"].ToString(),
                                                                mTable.DefaultView[i]["LetterID"].ToString()));
                   
                }

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, mBuilder.ToString(), MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        /// <summary>
        /// Xóa letter
        /// </summary>
        public void DeleteLetter()
        {
            try
            {
                DataSet mSet = mNews.GetDataSet_Session();
                DataTable tbl_Letter = mSet.Tables["Letter"];
                DataTable tbl_Record = mSet.Tables["Record"];

                int LetterID = 0;
                GetParemeter<int>(ref LetterID, "LetterID");

                if (tbl_Letter == null || tbl_Letter.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.NoData));
                    return;
                }

                tbl_Letter.DefaultView.RowFilter = " LetterID = " + LetterID.ToString();

                if (tbl_Letter.DefaultView.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.NoData));
                    return;
                }

                tbl_Letter.DefaultView.Delete(0);

                if (tbl_Record != null && tbl_Record.Rows.Count > 0)
                {
                    tbl_Record.DefaultView.RowFilter = " LetterID = " + LetterID.ToString();
                    if (tbl_Record.DefaultView.Count > 0)
                    {
                        for (int i = tbl_Record.DefaultView.Count-1; i >= 0; i--)
                        {
                            tbl_Record.DefaultView[i].Delete();
                        }
                    }
                }

                mSet.AcceptChanges();
                mNews.SetDataSet_Session(mSet);

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        /// <summary>
        /// Xóa Record
        /// </summary>
        public void DeleteRecord()
        {
            try
            {
                DataSet mSet = mNews.GetDataSet_Session();
                DataTable tbl_Record = mSet.Tables["Record"];

                int RecordID = 0;
                GetParemeter<int>(ref RecordID, "RecordID");

                if (tbl_Record == null || tbl_Record.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.NoData));
                    return;
                }

                tbl_Record.DefaultView.RowFilter = " RecordID = " + RecordID.ToString() + " OR ParentID = " + RecordID.ToString();

                if (tbl_Record.DefaultView.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.NoData));
                    return;
                }

                if (tbl_Record.DefaultView.Count > 0)
                {
                    for (int i = tbl_Record.DefaultView.Count - 1; i >= 0; i--)
                    {
                        tbl_Record.DefaultView[i].Delete();
                    }
                }

                mSet.AcceptChanges();
                mNews.SetDataSet_Session(mSet);

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, MyMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void ViewAll()
        {

        }
    }
}
