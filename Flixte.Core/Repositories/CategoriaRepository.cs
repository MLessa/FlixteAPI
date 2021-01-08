using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class CategoriaRepository : DBDapperComponent
    {
        #region Build
        private static CategoriaRepository categoriaRepository = null;
        private const string cTableName = "categoria";
        private static string columnList = "nome,idGrupo";
        /// <summary>
        /// Constructor of CategoriaRepository
        /// </summary>
        private CategoriaRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of CategoriaRepository
        /// </summary>
        /// <returns>CategoriaRepository Object</returns>
        public static CategoriaRepository GetInstance()
        {
            if (categoriaRepository == null)
            {
                categoriaRepository = new CategoriaRepository();
            }
            return categoriaRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity Categoria
        /// </summary>
        /// <param name="Categoria">Model of Categoria</param>
        /// <returns>true if successfull</returns>
        public bool Insert(Categoria categoria)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, categoria);
        }

        /// <summary>
        /// Update the entity Categoria in DB
        /// </summary>
        /// <param name="Categoria">Model of Categoria</param>
        /// <returns>true if successfull</returns>
        public bool Update(Categoria categoria)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set nome=@nome,idGrupo=@idGrupo where id=@id;";

            return Execute(commandText, categoria);
        }

        /// <summary>
        /// Delete the entity Categoria in DB
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
        /// Find all activated Categoria
        /// </summary>
        /// <returns>List of Categoria</returns>
        public List<Categoria> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where 1 = 1 ";
            return QueryList<Categoria>(commandText);
        }

        /// <summary>
        /// Find all Categoria
        /// </summary>
        /// <returns>List of Categoria</returns>
        public List<Categoria> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<Categoria>(commandText);
        }
        /// <summary>
        /// Find Categoria By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>Categoria Model</returns>
        public Categoria FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<Categoria>(commandText, new { id = id });
        }

        /// <summary>
        /// Find Categoria By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of Categoria</returns>
        public List<Categoria> FindFilter(string nome)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where 1 = 1 ";

            if (!string.IsNullOrEmpty(nome))
                commandText += " and nome like @nome ";           
            return QueryList<Categoria>(commandText, new { nome = "%" + nome + "%"});
        }
        #endregion
    }
}
