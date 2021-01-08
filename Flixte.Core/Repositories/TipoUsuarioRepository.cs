using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class TipoUsuarioRepository : DBDapperComponent
    {
        #region Build
        private static TipoUsuarioRepository tipoUsuarioRepository = null;
        private const string cTableName = "tipousuario";
        private static string columnList = "nome";
        /// <summary>
        /// Constructor of TipoUsuarioRepository
        /// </summary>
        private TipoUsuarioRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of TipoUsuarioRepository
        /// </summary>
        /// <returns>TipoUsuarioRepository Object</returns>
        public static TipoUsuarioRepository GetInstance()
        {
            if (tipoUsuarioRepository == null)
            {
                tipoUsuarioRepository = new TipoUsuarioRepository();
            }
            return tipoUsuarioRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity TipoUsuario
        /// </summary>
        /// <param name="TipoUsuario">Model of TipoUsuario</param>
        /// <returns>true if successfull</returns>
        public bool Insert(TipoUsuario tipoUsuario)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, tipoUsuario);
        }

        /// <summary>
        /// Update the entity TipoUsuario in DB
        /// </summary>
        /// <param name="TipoUsuario">Model of TipoUsuario</param>
        /// <returns>true if successfull</returns>
        public bool Update(TipoUsuario tipoUsuario)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set nome=@nome where id=@id;";

            return Execute(commandText, tipoUsuario);
        }

        /// <summary>
        /// Delete the entity TipoUsuario in DB
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
        /// Find all activated TipoUsuario
        /// </summary>
        /// <returns>List of TipoUsuario</returns>
        public List<TipoUsuario> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where 1 = 1 ";
            return QueryList<TipoUsuario>(commandText);
        }

        /// <summary>
        /// Find all TipoUsuario
        /// </summary>
        /// <returns>List of TipoUsuario</returns>
        public List<TipoUsuario> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<TipoUsuario>(commandText);
        }
        /// <summary>
        /// Find TipoUsuario By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>TipoUsuario Model</returns>
        public TipoUsuario FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<TipoUsuario>(commandText, new { id = id });
        }

        /// <summary>
        /// Find TipoUsuario By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of TipoUsuario</returns>
        public List<TipoUsuario> FindFilter(string nome)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where 1 = 1 ";

            if (!string.IsNullOrEmpty(nome))
                commandText += " and nome like @nome ";           
            return QueryList<TipoUsuario>(commandText, new { nome = "%" + nome + "%"});
        }
        #endregion
    }
}
