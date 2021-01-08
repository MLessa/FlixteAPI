using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class LogService
    {
        public static bool SaveLog(string ip, int? userid, string parameters, string formValues, string controller, string action)
        {
            return LogRepository.GetInstance().SaveLog(ip, userid, parameters, formValues, controller, action);
        }
    }
}
