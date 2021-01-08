using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class GrupoService
    {
        public static Grupo GetGrupo(int id)
        {
            return GrupoRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity Grupo
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(Grupo model)
        {            
            return GrupoRepository.GetInstance().Insert(model);
        }

        /// <summary>
        /// Update the entity Grupo in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Update(Grupo model)
        {
            return GrupoRepository.GetInstance().Update(model);
        }

        /// <summary>
        /// Delete the entity Grupo in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return GrupoRepository.GetInstance().Delete(id);
        }

        /// <summary>
        /// Find all Grupo
        /// </summary>
        /// <returns>List of Grupo</returns>
        public static List<Grupo> FindAll()
        {
            return GrupoRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all Grupo
        /// </summary>
        /// <returns>List of Grupo</returns>
        public static List<Grupo> FindAllWithInactive()
        {
            return GrupoRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find Grupo
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of Grupo DataSet</returns>
        public static List<Grupo> FindByFilter(string nome)
        {
            return GrupoRepository.GetInstance().FindFilter(nome);
        }
    }
}
