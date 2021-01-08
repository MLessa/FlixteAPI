using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class TipoUsuarioService
    {
        public static TipoUsuario GetTipoUsuario(int id)
        {
            return TipoUsuarioRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity TipoUsuario
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(TipoUsuario model)
        {            
            return TipoUsuarioRepository.GetInstance().Insert(model);
        }

        /// <summary>
        /// Update the entity TipoUsuario in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Update(TipoUsuario model)
        {
            return TipoUsuarioRepository.GetInstance().Update(model);
        }

        /// <summary>
        /// Delete the entity TipoUsuario in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return TipoUsuarioRepository.GetInstance().Delete(id);
        }

        /// <summary>
        /// Find all TipoUsuario
        /// </summary>
        /// <returns>List of TipoUsuario</returns>
        public static List<TipoUsuario> FindAll()
        {
            return TipoUsuarioRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all TipoUsuario
        /// </summary>
        /// <returns>List of TipoUsuario</returns>
        public static List<TipoUsuario> FindAllWithInactive()
        {
            return TipoUsuarioRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find TipoUsuario
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of TipoUsuario DataSet</returns>
        public static List<TipoUsuario> FindByFilter(string nome)
        {
            return TipoUsuarioRepository.GetInstance().FindFilter(nome);
        }
    }
}
