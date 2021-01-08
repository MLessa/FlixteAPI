using Database.Common;
using Flixte.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Repositories
{
    public class TokenRepository : DBDapperComponent
    {
        #region Build
        private static TokenRepository tokenRepository = null;
        private const string cTableName = "token";

        /// <summary>
        /// Constructor of TokenRepository
        /// </summary>
        private TokenRepository()
        {
            this.DefaultConnectionStringKey = "FlixteCS";
        }

        /// <summary>
        /// Return a Instance of TokenRepository
        /// </summary>
        /// <returns>TokenRepository Object</returns>
        public static TokenRepository GetInstance()
        {
            if (tokenRepository == null)
            {
                tokenRepository = new TokenRepository();
            }
            return tokenRepository;
        }

        #endregion

        #region Methods         
        public bool SaveToken(Token token)
        {
            // buildding a command T-SQL
            string commandText = "insert into " + cTableName + "(`tokenValue`,`idUsuario`,`data` ,`ativo`,`deviceID`,`app`,`UltimoAcesso`) values( @tokenValue,@idUsuario,now(),1,@deviceID,@app,now())";
            return Execute(commandText, new { tokenValue = token.TokenValue, idUsuario = token.IDUsuario, deviceID = token.DeviceID, app = token.App });
        }
        public Usuario FindByToken(string tokenValue)
        {
            // buildding a command T-SQL
            string commandText = "select u.id, u.nome ,u.ativo,u.login,u.password,u.admin,u.ultimoacesso,u.data from " + cTableName + " t, usuario u   where u.id=t.idUsuario and tokenValue = @tokenValue ";

            return QuerySingle<Usuario>(commandText, new { tokenValue = tokenValue });
        }
        public Token FindByUser(int idUsuario)
        {
            // buildding a command T-SQL
            string commandText = "select id,`tokenValue`,`idUsuario`,`data` ,`ativo`, `deviceID`,`app`,`UltimoAcesso` from " + cTableName + "  where idUsuario = @idUsuario and ativo=1 ";

            return QuerySingle<Token>(commandText, new { idUsuario = idUsuario });
        }
        public bool Update(Token token)
        {
            // buildding a command T-SQL
            string commandText = "update " + cTableName + " set ativo=@ativo,deviceID =@deviceID, app=@app,UltimoAcesso=@UltimoAcesso where id=@id;";

            return Execute(commandText, token);
        }
        #endregion
    }
}
