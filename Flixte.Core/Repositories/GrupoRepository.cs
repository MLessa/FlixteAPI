using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class GrupoRepository : DBDapperComponent
    {
        #region Build
        private static GrupoRepository grupoRepository = null;
        private const string cTableName = "grupo";
        private static string columnList = "nome";
        /// <summary>
        /// Constructor of GrupoRepository
        /// </summary>
        private GrupoRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of GrupoRepository
        /// </summary>
        /// <returns>GrupoRepository Object</returns>
        public static GrupoRepository GetInstance()
        {
            if (grupoRepository == null)
            {
                grupoRepository = new GrupoRepository();
            }
            return grupoRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity Grupo
        /// </summary>
        /// <param name="Grupo">Model of Grupo</param>
        /// <returns>true if successfull</returns>
        public bool Insert(Grupo grupo)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, grupo);
        }

        /// <summary>
        /// Update the entity Grupo in DB
        /// </summary>
        /// <param name="Grupo">Model of Grupo</param>
        /// <returns>true if successfull</returns>
        public bool Update(Grupo grupo)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set nome=@nome where id=@id;";

            return Execute(commandText, grupo);
        }

        /// <summary>
        /// Delete the entity Grupo in DB
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
        /// Find all activated Grupo
        /// </summary>
        /// <returns>List of Grupo</returns>
        public List<Grupo> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where 1 = 1 ";
            return QueryList<Grupo>(commandText);
        }

        /// <summary>
        /// Find all Grupo
        /// </summary>
        /// <returns>List of Grupo</returns>
        public List<Grupo> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<Grupo>(commandText);
        }
        /// <summary>
        /// Find Grupo By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>Grupo Model</returns>
        public Grupo FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<Grupo>(commandText, new { id = id });
        }

        /// <summary>
        /// Find Grupo By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of Grupo</returns>
        public List<Grupo> FindFilter(string nome)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where 1 = 1 ";

            if (!string.IsNullOrEmpty(nome))
                commandText += " and nome like @nome ";           
            return QueryList<Grupo>(commandText, new { nome = "%" + nome + "%"});
        }
        #endregion
    }
}
