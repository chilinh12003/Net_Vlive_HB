using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using MyUtility;
using System.Reflection;
namespace MyAjax.Ajax
{
    public class MyAjaxHander : MyASHXBase
    {
        private Dictionary<string, Type> ClassList;

        public MyAjaxHander()
        {
            //Lấy tất cả các Type của class dành cho Ajax
            ClassList = new Dictionary<string, Type>();
            GetAllClass("MyAjax");

        }
        /// <summary>
        /// Lấy tất cả các class thuộc một namespace
        /// </summary>
        private void GetAllClass(string NamseSpace)
        {
            try
            {
                //Lấy tất cả namespace
                Assembly mAssem = Assembly.Load(NamseSpace);

                //Tìm namesapce cần lấy các class
                foreach (Type mType in mAssem.GetTypes())
                {
                    ClassList.Add(mType.Name.ToLower(), mType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void WriteHTML()
        {
            string NamePage = Request.Url.Segments[Request.Url.Segments.Length-1];
            string[] Arr = NamePage.Split('.');
            string ClassName = string.Empty;
            string MethodName = string.Empty;

            if (Arr.Length > 2)
            {
                ClassName = Arr[0];
                MethodName = Arr[1];
            }

            //Lấy class
            if (ClassList.ContainsKey(ClassName.ToLower()))
            {
                Type CurrentType;
                ClassList.TryGetValue(ClassName.ToLower(), out CurrentType);

                var CurrentClass = (MyAjaxBase)Activator.CreateInstance(CurrentType);
                CurrentClass.MyContext = MyContext;

                CurrentClass.RunMethod(MethodName);
            }
        }
    }
}
