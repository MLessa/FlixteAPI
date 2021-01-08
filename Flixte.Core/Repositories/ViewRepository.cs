using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class ViewRepository : DBDapperComponent
    {
        #region Build
        private static ViewRepository viewRepository = null;
        private const string cTableName = "view";
        private static string columnList = "idUsuario,idEstacao,idYChannel,data";
        /// <summary>
        /// Constructor of ViewRepository
        /// </summary>
        private ViewRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of ViewRepository
        /// </summary>
        /// <returns>ViewRepository Object</returns>
        public static ViewRepository GetInstance()
        {
            if (viewRepository == null)
            {
                viewRepository = new ViewRepository();
            }
            return viewRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity View
        /// </summary>
        /// <param name="View">Model of View</param>
        /// <returns>true if successfull</returns>
        public bool Insert(View view)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, view);
        }
                
        /// <summary>
        /// Delete the entity View in DB
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>true if successfull</returns>
        public bool Delete(int id)
        {
            // buildding a command T-SQL
            string commandText = "delete from " + cTableName + " where id=@id";
            return Execute(commandText, new { id = id });
        }

        /// <summary>
        /// Find all activated View
        /// </summary>
        /// <returns>List of View</returns>
        public List<View> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where 1 = 1 ";
            return QueryList<View>(commandText);
        }

        /// <summary>
        /// Find all View
        /// </summary>
        /// <returns>List of View</returns>
        public List<View> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<View>(commandText);
        }
        /// <summary>
        /// Find View By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>View Model</returns>
        public View FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<View>(commandText, new { id = id });
        }

        /// <summary>
        /// Find View By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of View</returns>
        public List<View> FindFilter(int idEstacao)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where 1 = 1 ";

            
                commandText += " and idEstacao = @idEstacao ";           
            return QueryList<View>(commandText, new { idEstacao = idEstacao });
        }
        #endregion
    }
}
