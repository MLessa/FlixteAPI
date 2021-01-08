using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class SubCategoriaService
    {
        public static SubCategoria GetSubCategoria(int id)
        {
            return SubCategoriaRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity SubCategoria
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(SubCategoria model)
        {            
            return SubCategoriaRepository.GetInstance().Insert(model);
        }

        /// <summary>
        /// Update the entity SubCategoria in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Update(SubCategoria model)
        {
            return SubCategoriaRepository.GetInstance().Update(model);
        }

        /// <summary>
        /// Delete the entity SubCategoria in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return SubCategoriaRepository.GetInstance().Delete(id);
        }

        /// <summary>
        /// Find all SubCategoria
        /// </summary>
        /// <returns>List of SubCategoria</returns>
        public static List<SubCategoria> FindAll()
        {
            return SubCategoriaRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all SubCategoria
        /// </summary>
        /// <returns>List of SubCategoria</returns>
        public static List<SubCategoria> FindAllWithInactive()
        {
            return SubCategoriaRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find SubCategoria
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of SubCategoria DataSet</returns>
        public static List<SubCategoria> FindByFilter(string nome)
        {
            return SubCategoriaRepository.GetInstance().FindFilter(nome);
        }
    }
}
