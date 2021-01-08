using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class SubCategoriaRepository : DBDapperComponent
    {
        #region Build
        private static SubCategoriaRepository subCategoriaRepository = null;
        private const string cTableName = "subcategoria";
        private static string columnList = "nome,idCategoria,idGrupo";
        /// <summary>
        /// Constructor of SubCategoriaRepository
        /// </summary>
        private SubCategoriaRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of SubCategoriaRepository
        /// </summary>
        /// <returns>SubCategoriaRepository Object</returns>
        public static SubCategoriaRepository GetInstance()
        {
            if (subCategoriaRepository == null)
            {
                subCategoriaRepository = new SubCategoriaRepository();
            }
            return subCategoriaRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity SubCategoria
        /// </summary>
        /// <param name="SubCategoria">Model of SubCategoria</param>
        /// <returns>true if successfull</returns>
        public bool Insert(SubCategoria subCategoria)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, subCategoria);
        }

        /// <summary>
        /// Update the entity SubCategoria in DB
        /// </summary>
        /// <param name="SubCategoria">Model of SubCategoria</param>
        /// <returns>true if successfull</returns>
        public bool Update(SubCategoria subCategoria)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set nome=@nome,idCategoria=@idCategoria,idGrupo=@idGrupo where id=@id;";

            return Execute(commandText, subCategoria);
        }

        /// <summary>
        /// Delete the entity SubCategoria in DB
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
        /// Find all activated SubCategoria
        /// </summary>
        /// <returns>List of SubCategoria</returns>
        public List<SubCategoria> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where 1 = 1 ";
            return QueryList<SubCategoria>(commandText);
        }

        /// <summary>
        /// Find all SubCategoria
        /// </summary>
        /// <returns>List of SubCategoria</returns>
        public List<SubCategoria> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<SubCategoria>(commandText);
        }
        /// <summary>
        /// Find SubCategoria By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>SubCategoria Model</returns>
        public SubCategoria FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<SubCategoria>(commandText, new { id = id });
        }

        /// <summary>
        /// Find SubCategoria By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of SubCategoria</returns>
        public List<SubCategoria> FindFilter(string nome)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where 1 = 1 ";

            if (!string.IsNullOrEmpty(nome))
                commandText += " and nome like @nome ";           
            return QueryList<SubCategoria>(commandText, new { nome = "%" + nome + "%"});
        }
        #endregion
    }
}
