using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class UsuarioRepository : DBDapperComponent
    {
        #region Build
        private static UsuarioRepository usuarioRepository = null;
        private const string cTableName = "usuario";
        private static string columnList = "nome,login,password,ativo,admin,idTipoUsuario,Data,UltimoAcesso,fbEmailAddress,fbAccountID,fbToken,ggEmailAddress,ggAccountID,ggToken,username,ImageURL";
        /// <summary>
        /// Constructor of UsuarioRepository
        /// </summary>
        private UsuarioRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of UsuarioRepository
        /// </summary>
        /// <returns>UsuarioRepository Object</returns>
        public static UsuarioRepository GetInstance()
        {
            if (usuarioRepository == null)
            {
                usuarioRepository = new UsuarioRepository();
            }
            return usuarioRepository;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Insert into DB a Entity Usuario
        /// </summary>
        /// <param name="Usuario">Model of Usuario</param>
        /// <returns>true if successfull</returns>
        public bool Insert(Usuario usuario)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + " (" + columnList + ") values (@" + columnList.Replace(",", ",@") + ")";

            return this.Execute(commandText, usuario);
        }

        /// <summary>
        /// Update the entity Usuario in DB
        /// </summary>
        /// <param name="Usuario">Model of Usuario</param>
        /// <returns>true if successfull</returns>
        public bool Update(Usuario usuario)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set nome=@nome ,ativo=@ativo,password=@password,admin=@admin,idTipoUsuario=@idTipoUsuario,UltimoAcesso=@UltimoAcesso,fbEmailAddress=@fbEmailAddress,fbAccountID=@fbAccountID,fbToken=@fbToken,ggEmailAddress=@ggEmailAddress,ggAccountID=@ggAccountID,ggToken=@ggToken,username=@username,ImageURL=@ImageURL where id=@id;";

            return Execute(commandText, usuario);
        }

        /// <summary>
        /// Delete the entity Usuario in DB
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
        /// Find all activated Usuario
        /// </summary>
        /// <returns>List of Usuario</returns>
        public List<Usuario> FindAll()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + " where ativo = 1 ";
            return QueryList<Usuario>(commandText);
        }

        /// <summary>
        /// Find all Usuario
        /// </summary>
        /// <returns>List of Usuario</returns>
        public List<Usuario> FindAllWithInactive()
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  ";
            return QueryList<Usuario>(commandText);
        }
        /// <summary>
        /// Find Usuario By PK
        /// </summary>
        /// <param name="id">Integer</param>
        /// <returns>Usuario Model</returns>
        public Usuario FindByPK(int id)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "  where id = @id ";

            return QuerySingle<Usuario>(commandText, new { id = id });
        }

        /// <summary>
        /// Find Usuario By Filter
        /// </summary>
        /// <param name="nome">string</param>
        /// <returns>List of Usuario</returns>
        public List<Usuario> FindFilter(string nome, string login, string googleID, string username, string ggToken)
        {
            // buildding a command T-SQL
            string commandText = "select id," + columnList + " from " + cTableName + "    where ativo = 1 ";

            if (!string.IsNullOrEmpty(nome))
                commandText += " and nome like @nome ";
            if (!string.IsNullOrEmpty(login))
                commandText += " and login = @login ";
            if (!string.IsNullOrEmpty(username))
                commandText += " and username = @username ";
            if (!string.IsNullOrEmpty(googleID))
                commandText += " and ggAccountID = @googleID ";
            if (!string.IsNullOrEmpty(ggToken))
                commandText += " and ggToken = @ggToken ";
            return QueryList<Usuario>(commandText, new { nome = "%" + nome + "%", login = login, googleID = googleID, username = username, ggToken= ggToken });
        }
        #endregion
    }
}
