using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class CategoriaService
    {
        public static Categoria GetCategoria(int id)
        {
            return CategoriaRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity Categoria
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(Categoria model)
        {            
            return CategoriaRepository.GetInstance().Insert(model);
        }

        /// <summary>
        /// Update the entity Categoria in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Update(Categoria model)
        {
            return CategoriaRepository.GetInstance().Update(model);
        }

        /// <summary>
        /// Delete the entity Categoria in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return CategoriaRepository.GetInstance().Delete(id);
        }

        /// <summary>
        /// Find all Categoria
        /// </summary>
        /// <returns>List of Categoria</returns>
        public static List<Categoria> FindAll()
        {
            return CategoriaRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all Categoria
        /// </summary>
        /// <returns>List of Categoria</returns>
        public static List<Categoria> FindAllWithInactive()
        {
            return CategoriaRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find Categoria
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of Categoria DataSet</returns>
        public static List<Categoria> FindByFilter(string nome)
        {
            return CategoriaRepository.GetInstance().FindFilter(nome);
        }
    }
}
