using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class YVideoRepository : DBDapperComponent
    {
        #region Build
        private static YVideoRepository yVideoRepository = null;
        private const string cTableName = "yvideo";
        private static string columnList = "ychannelID,data,nome,descricao,idExterno,url,urlLogo,ativo";
        /// <summary>
        /// Constructor of YVideoRepository
        /// </summary>
        private YVideoRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of YVideoRepository
        /// </summary>
        /// <returns>YVideoRepository Object</returns>
        public static YVideoRepository GetInstance()
        {
            if (yVideoRepository == null)
            {
                yVideoRepository = new YVideoRepository();
            }
            return yVideoRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity YVideo
        /// </summary>
        /// <param name="YVideo">Model of YVideo</param>
        /// <returns>true if successfull</returns>
        public bool Insert(YVideo yVideo)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, yVideo);
        }

        /// <summary>
        /// Update the entity YVideo in DB
        /// </summary>
        /// <param name="YVideo">Model of YVideo</param>
        /// <returns>true if successfull</returns>
        public bool Update(YVideo yVideo)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set ychannelID=@ychannelID,nome=@nome,descricao=@descricao,idExterno=@idExterno,url=@url,urlLogo=@urlLogo,ativo=@ativo where id=@id;";

            return Execute(commandText, yVideo);
        }

        /// <summary>
        /// Delete the entity YVideo in DB
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
        /// Find all activated YVideo
        /// </summary>
        /// <returns>List of YVideo</returns>
        public List<YVideo> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where ativo = 1 ";
            return QueryList<YVideo>(commandText);
        }

        /// <summary>
        /// Find all YVideo
        /// </summary>
        /// <returns>List of YVideo</returns>
        public List<YVideo> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<YVideo>(commandText);
        }
        /// <summary>
        /// Find YVideo By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>YVideo Model</returns>
        public YVideo FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<YVideo>(commandText, new { id = id });
        }

        /// <summary>
        /// Find YVideo By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of YVideo</returns>
        public List<YVideo> FindFilter(string nome, int ychannelID)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where ativo = 1 ";

            if (!string.IsNullOrEmpty(nome))
                commandText += " and nome like @nome "; 
            if (ychannelID!=-1)
                commandText += " and ychannelID = @ychannelID ";
            return QueryList<YVideo>(commandText, new { nome = "%" + nome + "%", ychannelID = ychannelID });
        }
        #endregion
    }
}
