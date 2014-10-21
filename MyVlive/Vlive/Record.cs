using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace MyVlive.Vlive
{
    public class Record
    {
        public enum Method
        {
            smsdd = 1,
            sms = 2,
        }

        public enum RecordType
        {
            [DescriptionAttribute("Bản tin text")]
            SMSText =1,
            [DescriptionAttribute("Bản tin download")]
            SMSDownload = 2
        }
    }
}
