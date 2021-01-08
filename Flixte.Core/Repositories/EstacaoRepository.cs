using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class EstacaoRepository : DBDapperComponent
    {
        #region Build
        private static EstacaoRepository estacaoRepository = null;
        private const string cTableName = "estacao";
        private static string columnList = "nome,idUsuario,descricao,tags,urlLogo,ativo,destaque,data,users,views,excluido,privado,senha";
        /// <summary>
        /// Constructor of EstacaoRepository
        /// </summary>
        private EstacaoRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of EstacaoRepository
        /// </summary>
        /// <returns>EstacaoRepository Object</returns>
        public static EstacaoRepository GetInstance()
        {
            if (estacaoRepository == null)
            {
                estacaoRepository = new EstacaoRepository();
            }
            return estacaoRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity Estacao
        /// </summary>
        /// <param name="Estacao">Model of Estacao</param>
        /// <returns>true if successfull</returns>
        public bool Insert(Estacao estacao)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, estacao);
        }

        /// <summary>
        /// Update the entity Estacao in DB
        /// </summary>
        /// <param name="Estacao">Model of Estacao</param>
        /// <returns>true if successfull</returns>
        public bool Update(Estacao estacao)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set nome=@nome,descricao=@descricao,tags=@tags,urlLogo=@urlLogo,ativo=@ativo,destaque=@destaque,users=@users,views=@views,excluido=@excluido,privado=@privado,senha=@senha where id=@id;";

            return Execute(commandText, estacao);
        }

        /// <summary>
        /// Delete the entity Estacao in DB
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>true if successfull</returns>
        public bool Delete(int id)
        {
            // buildding a command T-SQL
            string commandText = "delete from " + cTableName + " where id=@id";
            return Execute(commandText, new { id = id });
        }

        public bool Rate(int id, int rate, int idUsuario)
        {
            // buildding a command T-SQL
            string commandText = "insert into avaliacao_estacao (idEstacao, idUsuario,dataCriacao, avaliacao) values (@id,@idUsuario,@rate,now())";
            return Execute(commandText, new { id = id, idUsuario = idUsuario, rate = rate });
        }

        public bool Follow(int id, int idUsuario)
        {
            // buildding a command T-SQL
            string commandText = "insert into estacao_usuario (idEstacao, idUsuario,data) values (@id,@idUsuario,now())";
            return Execute(commandText, new { id = id, idUsuario = idUsuario });
        }

        /// <summary>
        /// Find all activated Estacao
        /// </summary>
        /// <returns>List of Estacao</returns>
        public List<Estacao> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where ativo = 1 ";
            return QueryList<Estacao>(commandText);
        }

        /// <summary>
        /// Find all Estacao
        /// </summary>
        /// <returns>List of Estacao</returns>
        public List<Estacao> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<Estacao>(commandText);
        }
        /// <summary>
        /// Find Estacao By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>Estacao Model</returns>
        public Estacao FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<Estacao>(commandText, new { id = id });
        }

        /// <summary>
        /// Find Estacao By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of Estacao</returns>
        public List<Estacao> FindFilter(string nome, bool onlyDestaque, string tag)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where ativo = 1 ";

            if (!string.IsNullOrEmpty(nome))
                commandText += " and nome like @nome ";
            if (onlyDestaque)
                commandText += " and destaque = 1 ";
            if (!string.IsNullOrEmpty(tag))
                commandText += " and tags like @tag ";
            return QueryList<Estacao>(commandText, new { nome = "%" + nome + "%", tag = "%" + tag + "%" });
        }
        #endregion
    }
}
