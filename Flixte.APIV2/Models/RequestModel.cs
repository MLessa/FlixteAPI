using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flixte.APIV2.Models
{
    public class RequestModel<T>
    {
        public T Data { get; set; }
        public string AppVersion { get; set; }
        public bool IsAppRequest { get; set; }
        public string ResponseLocale { get; set; }
        public string DeviceID { get; set; }
    }
}