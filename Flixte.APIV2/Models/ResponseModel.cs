using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flixte.APIV2.Models
{

    public enum ResponseCodeType
    {
        USER_NOT_FOUND = -4,
        OPERATION_FAULT = -3,
        EMPTY_FIELDS = -2,
        GENERAL_ERROR = -1,
        SUCCESS = 1
    }

    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ResponseCodeType Code { get; set; }
    }
}