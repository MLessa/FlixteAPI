using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class YChannelRepository : DBDapperComponent
    {
        #region Build
        private static YChannelRepository yChannelRepository = null;
        private const string cTableName = "ychannel";
        private static string columnList = "nome,descricao,ativo,refreshed,urlLogo,idExterno,idCategoria,idSubCategoria";
        /// <summary>
        /// Constructor of YChannelRepository
        /// </summary>
        private YChannelRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of YChannelRepository
        /// </summary>
        /// <returns>YChannelRepository Object</returns>
        public static YChannelRepository GetInstance()
        {
            if (yChannelRepository == null)
            {
                yChannelRepository = new YChannelRepository();
            }
            return yChannelRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity YChannel
        /// </summary>
        /// <param name="YChannel">Model of YChannel</param>
        /// <returns>true if successfull</returns>
        public bool Insert(YChannel yChannel)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, yChannel);
        }

        /// <summary>
        /// Update the entity YChannel in DB
        /// </summary>
        /// <param name="YChannel">Model of YChannel</param>
        /// <returns>true if successfull</returns>
        public bool Update(YChannel yChannel)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set nome=@nome,descricao=@descricao,ativo=@ativo,refreshed=@refreshed,urlLogo=@urlLogo,idExterno=@idExterno,idCategoria=@idCategoria,idSubCategoria=@idSubCategoria  where id=@id;";

            return Execute(commandText, yChannel);
        }

        /// <summary>
        /// Delete the entity YChannel in DB
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
        /// Find all activated YChannel
        /// </summary>
        /// <returns>List of YChannel</returns>
        public List<YChannel> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where ativo = 1 ";
            return QueryList<YChannel>(commandText);
        }

        /// <summary>
        /// Find all YChannel
        /// </summary>
        /// <returns>List of YChannel</returns>
        public List<YChannel> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<YChannel>(commandText);
        }
        /// <summary>
        /// Find YChannel By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>YChannel Model</returns>
        public YChannel FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<YChannel>(commandText, new { id = id });
        }

        /// <summary>
        /// Find YChannel By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of YChannel</returns>
        public List<YChannel> FindFilter(string nome)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where ativo = 1 ";

            if (!string.IsNullOrEmpty(nome))
                commandText += " and nome like @nome ";           
            return QueryList<YChannel>(commandText, new { nome = "%" + nome + "%"});
        }
        #endregion
    }
}
