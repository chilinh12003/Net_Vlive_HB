using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyUtility;
using MyAuction.User;
using MyAuction;
namespace MyAjax.Ajax
{
    public class MyUser : MyAjaxBase
    {
       
        User mUser = new User();
        public void Login()
        {
            try
            {
                string UserName = string.Empty;
                string Password = string.Empty;
                string Message = string.Empty;
                Parameters.TryGetValue("username", out UserName);
                Parameters.TryGetValue("password", out Password);
                if (MyAuction.User.User.Login(UserName, Password, false, true, true, ref Message))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, "Thành công"));
                }
                else
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, Message));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyMessage.CommonError.Error));
            }
            finally
            {
                MyContent.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
    }

        /// <summary>
        /// Kiểm tra tình trạng đăng nhập
        /// </summary>
        public void IsLogin()
        {
            try
            {
                if (MyAuction.User.User.IsLogined())
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, "Đã đăng nhập"));
                }
                else
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Chưa đăng nhập"));
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Có lỗi xảy ra"));
            }
            finally
            {
                MyContent.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void CheckUserName()
        {
            try
            {
                string UserName = string.Empty;
                string Message = string.Empty;
                Parameters.TryGetValue("UserName", out UserName);

                if (mUser.CheckExistsUserName(UserName))
                {
                    //Tài khoản đã được sử dụng
                    MyContent.Response.Write("0");
                }
                else
                {
                    MyContent.Response.Write("1");
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                MyContent.Response.Write("-1");
            }
        }

        public void CheckPhoneNumber()
        {
            try
            {
                string MSISDN = string.Empty;
                string Message = string.Empty;
                Parameters.TryGetValue("MSISDN", out MSISDN);

                MyUtility.MyConfig.Telco mTelco = MyConfig.Telco.Nothing;

                if (!MyUtility.MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, string.Empty))
                {
                    MyContent.Response.Write("0");
                }
                else
                {
                    if (mUser.CheckExistsPhoneNumber(MSISDN))
                        //Số điện thoại đã được sử dụng
                        MyContent.Response.Write("2");
                    else
                        MyContent.Response.Write("1");
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                MyContent.Response.Write("-1");
            }
        }

        public void CheckCaptcha()
        {
            try
            {
                string Code = string.Empty;
                string Message = string.Empty;
                Parameters.TryGetValue("CaptchaCode", out Code);

                if (CheckCaptcha(Code))
                {
                    MyContent.Response.Write("1");
                }
                else
                {
                    MyContent.Response.Write("0");
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                MyContent.Response.Write("-1");
            }
        }

        private bool CheckCaptcha(string CaptchaCode)
        {
            try
            {
                if (MyContent.Session["CaptchaCode"] == null)
                {
                    return false;
                }

                if (CaptchaCode.Equals(MyContent.Session["CaptchaCode"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Register()
        {
            try
            {

                string UserName = string.Empty;
                Parameters.TryGetValue("UserName", out UserName);
                string Password = string.Empty;
                Parameters.TryGetValue("Password", out Password);
                string RepeatPassword = string.Empty;
                Parameters.TryGetValue("RepeatPassword", out RepeatPassword);
                string MSISDN = string.Empty;
                Parameters.TryGetValue("MSISDN", out MSISDN);
                string CMND = string.Empty;
                Parameters.TryGetValue("CMND", out CMND);
                string Email = string.Empty;
                Parameters.TryGetValue("Email", out Email);
                string FullName = string.Empty;
                Parameters.TryGetValue("FullName", out FullName);
                string Birthday = string.Empty;
                Parameters.TryGetValue("Birthday", out Birthday);
                string CaptchaCode = string.Empty;
                Parameters.TryGetValue("CaptchaCode", out CaptchaCode);
                string IsReceiveSMS = string.Empty;
                Parameters.TryGetValue("IsReceiveSMS", out IsReceiveSMS);
                string Sex = string.Empty;
                Parameters.TryGetValue("Sex", out Sex);

                bool IsValid = true;
                //Kiểm tra thông tin
                if (string.IsNullOrEmpty(UserName))
                {
                    ListAjaxResult.Add(new AjaxResult("UserName", "Tên đăng nhập không được để trống"));
                    IsValid = false;
                }
                else if (mUser.CheckExistsUserName(UserName))
                {
                    ListAjaxResult.Add(new AjaxResult("UserName", "Tên đăng nhập đã được sử dụng"));
                    IsValid = false;
                }
                if (string.IsNullOrEmpty(MSISDN))
                {
                    ListAjaxResult.Add(new AjaxResult("MSISDN", "Số điện thoại không được để trống"));
                    IsValid = false;
                }
                else
                {
                    MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                    if (!MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, string.Empty))
                    {
                        ListAjaxResult.Add(new AjaxResult("MSISDN", "Số điện thoại không đúng"));
                        IsValid = false;
                    }
                    else if (mUser.CheckExistsPhoneNumber(MSISDN))
                    {
                        ListAjaxResult.Add(new AjaxResult("MSISDN", "Số điện thoại đã được sử dụng"));
                        IsValid = false;
                    }
                }
                if (string.IsNullOrEmpty(Password))
                {
                    ListAjaxResult.Add(new AjaxResult("Password", "Mật khẩu không được để trống"));
                    IsValid = false;
                }
                else if (Password.Length < 6)
                {
                    ListAjaxResult.Add(new AjaxResult("Password", "Mật khẩu phải lớn hơn hoặc bằng 6 ký tự"));
                    IsValid = false;
                }
                if (string.IsNullOrEmpty(RepeatPassword))
                {
                    ListAjaxResult.Add(new AjaxResult("RepeatPassword", "Nhắc lại Mật khẩu không được để trống"));
                    IsValid = false;
                }
                else if (Password != RepeatPassword)
                {
                    ListAjaxResult.Add(new AjaxResult("RepeatPassword", "Mật khẩu nhắc lại và Mật khẩu là không giống nhau"));
                    IsValid = false;
                }

                if (!CheckCaptcha(CaptchaCode))
                {
                    ListAjaxResult.Add(new AjaxResult("CaptchaCode", "Mã bảo mật là không chính xác"));
                    IsValid = false;
                }

                DateTime dBirthday = DateTime.MinValue;
                if (!string.IsNullOrEmpty(Birthday) && !DateTime.TryParseExact(Birthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dBirthday))
                {
                    ListAjaxResult.Add(new AjaxResult("Birthday", "Ngày sinh nhập vào không đúng định dạng (Ngày/Tháng/Năm)"));
                    IsValid = false;
                }
                if (!IsValid)
                {

                    return;
                }


                bool bIsReceiveSMS = false;
                if (IsReceiveSMS == "checked")
                    bIsReceiveSMS = true;

                DataSet mSet = mUser.CreateDataSet();
                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                DataRow mNewRow = mSet.Tables[0].NewRow();

                mNewRow["UserName"] = UserName;
                mNewRow["Password"] = MySecurity.Encrypt_MD5(Password);
                mNewRow["MSISDN"] = MSISDN;
                mNewRow["CMND"] = CMND;
                mNewRow["Email"] = Email;
                if (dBirthday != DateTime.MinValue)
                    mNewRow["Birthday"] = dBirthday.ToString(MyConfig.DateFormat_InsertToDB);
                mNewRow["FullName"] = FullName;

                if (!string.IsNullOrEmpty(Sex))
                {
                    if (Sex == "1")
                        mNewRow["Sex"] = true;
                    else
                        mNewRow["Sex"] = false;

                }
                mNewRow["CreateDate"] = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);
                mNewRow["StatusID"] = (int)User.Status.News;
                mNewRow["StatusName"] = MyEnum.StringValueOf(User.Status.News);

                mNewRow["UserTypeID"] = (int)User.UserType.Normal;
                mNewRow["UserTypeName"] = MyEnum.StringValueOf(User.UserType.Normal);
                mNewRow["SearchContent"] = MyConvert.ConvertDataToSearchText(mNewRow, new string[] { "UserName", "MSISDN", "CMND", "Email", "FullName" });

                mNewRow["IsReceiveSMS"] = bIsReceiveSMS;

                mSet.Tables[0].Rows.Add(mNewRow);

                if (mUser.Insert(1, mSet.GetXml()))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, "Đăng ký tài khoản thành công."));
                }
                else
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Đăng ký tài khoản Không thành công."));
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Có lỗi xảy ra trong quá trình đăng ký"));
            }
            finally
            {
                MyContent.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void GetBalance()
        {
            try
            {
                User mUser = new User();
                if (User.IsLogined())
                {
                    double Balance = mUser.Balance(User.UserID());
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, Balance.ToString(MyConfig.DoubleFormat), "Thành công"));
                }
                else
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Chưa đăng nhập"));
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Có lỗi xảy ra trong lấy dữ liệu"));
            }
            finally
            {
                MyContent.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void Logout()
        {
            try
            {
                if (User.LogOut())
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success,"Thành công"));
                }
                else
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Đăng xuất không thành công"));
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, "Có lỗi xảy ra"));
            }
            finally
            {
                MyContent.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

    }
}
