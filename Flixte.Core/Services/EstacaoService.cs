using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class EstacaoService
    {
        public static Estacao GetEstacao(int id)
        {
            return EstacaoRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity Estacao
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(Estacao model)
        {            
            return EstacaoRepository.GetInstance().Insert(model);
        }

        /// <summary>
        /// Update the entity Estacao in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Update(Estacao model)
        {
            return EstacaoRepository.GetInstance().Update(model);
        }

        /// <summary>
        /// Delete the entity Estacao in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return EstacaoRepository.GetInstance().Delete(id);
        }

        public static bool Rate(int id, int rate, int idUsuario)
        {
            return EstacaoRepository.GetInstance().Rate(id, rate, idUsuario);
        }

        public static bool Follow(int id, int idUsuario)
        {
            return EstacaoRepository.GetInstance().Follow(id, idUsuario);
        }

        /// <summary>
        /// Find all Estacao
        /// </summary>
        /// <returns>List of Estacao</returns>
        public static List<Estacao> FindAll()
        {
            return EstacaoRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all Estacao
        /// </summary>
        /// <returns>List of Estacao</returns>
        public static List<Estacao> FindAllWithInactive()
        {
            return EstacaoRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find Estacao
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of Estacao DataSet</returns>
        public static List<Estacao> FindByFilter(string nome, bool onlyDestaque, string categoryID)
        {
            return EstacaoRepository.GetInstance().FindFilter(nome, onlyDestaque, categoryID);
        }
    }
}
