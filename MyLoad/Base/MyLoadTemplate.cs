using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyUtility;

namespace MyLoad.Base
{
    public class MyLoadTemplate
    {
        /// <summary>
        /// Đọc template từ file
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <returns></returns>
        public string LoadTemplate(string TemplatePath)
        {
            try
            {
                TemplatePath = MyFile.GetFullPathFile(TemplatePath);
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                return strTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Template theo dữ liệu truyền vào là DataTable (load tất cả các Column)
        /// </summary>
        /// <param name="TemplatePath">Tên file Template</param>
        /// <param name="Data">DataTable dữ liệu</param>
        /// <returns></returns>
        public string LoadTemplate(string TemplatePath, DataTable Data)
        {
            try
            {
                TemplatePath = MyFile.GetFullPathFile(TemplatePath);
                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < Data.Columns.Count; i++)
                    {
                        //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                        if (Data.Columns[i].DataType == typeof(DateTime))
                        {
                            arr_ListPara.Add(((DateTime)mRow[i]).ToString(MyConfig.ViewDateFormat));
                        }
                        else
                        {
                            arr_ListPara.Add(mRow[i].ToString());
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));

                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Template với 1 tham số truyền vào
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string LoadTemplateByString(string TemplatePath, string Value)
        {
            try
            {
                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Ứng với mỗi dòng sẽ load 1 template
                mBuilder.Append(string.Format(strTemplate, Value));

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load template dựa các mảng các giá trị được truyền vào
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <param name="ArrayValue"></param>
        /// <returns></returns>
        public string LoadTemplateByArray(string TemplatePath, string[] ArrayValue)
        {
            try
            {
                if (ArrayValue.Length < 1)
                    throw new Exception("Số lượng item trong ArrayValue truyền vào là không phù hợp");

                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Ứng với mỗi dòng sẽ load 1 template
                mBuilder.Append(string.Format(strTemplate, ArrayValue));

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Template theo Index của Column
        /// </summary>
        /// <param name="TemplatePath">Tên file Template</param>
        /// <param name="Data">DataTable chứa dữ liệu</param>
        /// <param name="ColumnIndex">Index của Column cần lấy dữ liệu</param>
        /// <returns></returns>
        public string LoadTemplateByColumIndex(string TemplatePath, DataTable Data, int[] ColumnIndex)
        {
            try
            {
                if (ColumnIndex.Length < 1 || Data == null || Data.Columns.Count < ColumnIndex.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");

                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnIndex.Length; i++)
                    {
                        //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                        if (Data.Columns[ColumnIndex[i]].DataType == typeof(DateTime))
                        {
                            arr_ListPara.Add(((DateTime)mRow[ColumnIndex[i]]).ToString(MyConfig.ViewDateFormat));
                        }
                        else
                        {
                            arr_ListPara.Add(mRow[ColumnIndex[i]].ToString());
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));

                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Tempate theo tên Column
        /// </summary>
        /// <param name="TemplatePath">Tên file Template</param>
        /// <param name="Data"></param>
        /// <param name="ColumnName">Mảng các tên Column cần lấy dữ liệu</param>
        /// <returns></returns>
        public string LoadTemplateByColumnName(string TemplatePath, DataTable Data, string[] ColumnName)
        {
            try
            {
                if (ColumnName.Length < 1 || Data == null || Data.Columns.Count < ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");

                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnName.Length; i++)
                    {

                        //if (ColumnName[i].ToLower() == "imagepath")
                        //{
                        //    arr_ListPara.Add(MyFile.GetFullResourceLink(mRow[ColumnName[i]].ToString()));
                        //    continue;
                        //}
                        // Kien DT -> Nếu total down là DBNull thì trả về 0
                        if (ColumnName[i].ToLower() == "totaldown")
                        {
                            if (mRow[ColumnName[i]].ToString() == "")
                            {
                                arr_ListPara.Add("0");
                                continue;
                            }
                        }
                        //Nếu Data không chứa Column này thì lấy chính tên Column thay giá trị
                        if (Data.Columns.Contains(ColumnName[i]))
                        {
                            //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                            if (Data.Columns[ColumnName[i]].DataType == typeof(DateTime))
                            {
                                arr_ListPara.Add(((DateTime)mRow[ColumnName[i]]).ToString(MyConfig.ViewDateFormat));
                            }
                            else
                            {
                                arr_ListPara.Add(mRow[ColumnName[i]].ToString());
                            }
                        }
                        else
                        {
                            arr_ListPara.Add(ColumnName[i]);
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));
                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string LoadTemplateByColumnName(string TemplatePath, DataTable Data, string[] ColumnName, string[] ColumnPrefix)
        {
            try
            {
                if (ColumnName.Length < 1 || Data == null || Data.Columns.Count < ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");
                if (ColumnPrefix.Length != ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex,ColumnPrefix truyền vào là không phù hợp");

                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnName.Length; i++)
                    {
                        //if (ColumnName[i].ToLower() == "imagepath")
                        //{
                        //    arr_ListPara.Add(MyFile.GetFullResourceLink(mRow[ColumnName[i]].ToString()));
                        //    continue;
                        //}
                        // Kien DT -> Nếu total down là DBNull thì trả về 0
                        if (ColumnName[i].ToLower() == "totaldown")
                        {
                            if (mRow[ColumnName[i]].ToString() == "")
                            {
                                arr_ListPara.Add("0");
                                continue;
                            }
                        }
                        //Nếu Data không chứa Column này thì lấy chính tên Column thay giá trị
                        if (Data.Columns.Contains(ColumnName[i]))
                        {
                            //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                            if (Data.Columns[ColumnName[i]].DataType == typeof(DateTime))
                            {
                                arr_ListPara.Add(ColumnPrefix[i] + ((DateTime)mRow[ColumnName[i]]).ToString(MyConfig.ViewDateFormat));
                            }
                            else
                            {
                                arr_ListPara.Add(ColumnPrefix[i] + mRow[ColumnName[i]].ToString());
                            }

                        }
                        else
                        {
                            arr_ListPara.Add(ColumnPrefix[i] + ColumnName[i]);
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));
                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
