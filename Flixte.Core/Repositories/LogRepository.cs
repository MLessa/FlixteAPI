using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    internal class LogRepository : Database.Common.DBDapperComponent
    {
        #region Build
        private static LogRepository logRepository = null;
        private const string cTableName = "log";

        /// <summary>
        /// Constructor of LogRepository
        /// </summary>
        private LogRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of LogRepository
        /// </summary>
        /// <returns>LogRepository Object</returns>
        public static LogRepository GetInstance()
        {
            if (logRepository == null)
            {
                logRepository = new LogRepository();
            }
            return logRepository;
        }

        #endregion

        #region Methods        
        /// <summary>
        /// Save Log in DB
        /// </summary>
        /// <param name="ip">User IPAddress</param>
        /// <param name="userid">UserID</param>
        /// <param name="parameters">URL Parameters</param>
        /// <param name="formValues">Post Parameters</param>
        /// <param name="controller">Controller</param>
        /// <param name="action">View </param>
        /// <returns>True if insert was successful</returns>
        public bool SaveLog(string ip, int? userid, string parameters, string formValues, string controller, string action)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + "(`LogDate`,`Controller`,`Action`,`FormValues` ,`UserID`,`ActionParameters`,`IpAddress`) values( now(),@controller,@action,@formValues,@userid, @parameters,@ip)";
            return Execute(commandText, new { controller = controller, action = action, formValues = formValues, userid = userid, parameters = parameters, ip = ip });
        }
        #endregion
    }
}
