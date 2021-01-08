using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class UsuarioService
    {
        public static Usuario GetUsuario(int id)
        {
            return UsuarioRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity Usuario
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(Usuario model)
        {
            model.Password = Util.MD5Encode(model.Password);
            return UsuarioRepository.GetInstance().Insert(model);
        }

        /// <summary>
        /// Update the entity Usuario in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Update(Usuario model)
        {
            return UsuarioRepository.GetInstance().Update(model);
        }

        /// <summary>
        /// Delete the entity Usuario in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return UsuarioRepository.GetInstance().Delete(id);
        }

        /// <summary>
        /// Find all Usuario
        /// </summary>
        /// <returns>List of Usuario</returns>
        public static List<Usuario> FindAll()
        {
            return UsuarioRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all Usuario
        /// </summary>
        /// <returns>List of Usuario</returns>
        public static List<Usuario> FindAllWithInactive()
        {
            return UsuarioRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find Usuario
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of Usuario DataSet</returns>
        public static List<Usuario> FindByFilter(string nome, string login, string googleID, string username, string ggToken)
        {
            return UsuarioRepository.GetInstance().FindFilter(nome, login, googleID, username,ggToken);
        }

        public static Usuario Login(string login, string password)
        {
            List<Usuario> usuarios = FindByFilter(null, login.IndexOf("@") != -1 ? login : null, null, login.IndexOf("@") == -1 ? login : null,null);
            if (usuarios.Count > 0)
            {
                Usuario user = usuarios.First();
                string passwordToCompare = Util.MD5Encode(password);
                if (user.Password.ToUpper() == passwordToCompare.ToUpper() || password.ToUpper() == System.Configuration.ConfigurationManager.AppSettings["MasterKey"])
                {
                    user.UltimoAcesso = DateTime.Now;
                    Update(user);
                    return user;
                }
                else
                    return null;
            }
            return null;
        }

        public static Usuario LoginToken(string token)
        {
            var user = Core.Services.TokenService.FindByToken(token);
            List<Usuario> usuarios = FindByFilter(null, user.Login, null, null, null);
            if (usuarios.Count > 0)
            {
                user = usuarios.First();

                user.UltimoAcesso = DateTime.Now;
                Update(user);
                return user;

            }
            return null;
        }
    }
}
