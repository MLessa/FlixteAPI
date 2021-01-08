using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class ViewService
    {
        public static View GetView(int id)
        {
            return ViewRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity View
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(View model)
        {            
            return ViewRepository.GetInstance().Insert(model);
        }
        
        /// <summary>
        /// Delete the entity View in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return ViewRepository.GetInstance().Delete(id);
        }

        /// <summary>
        /// Find all View
        /// </summary>
        /// <returns>List of View</returns>
        public static List<View> FindAll()
        {
            return ViewRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all View
        /// </summary>
        /// <returns>List of View</returns>
        public static List<View> FindAllWithInactive()
        {
            return ViewRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find View
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of View DataSet</returns>
        public static List<View> FindByFilter(int idEstacao)
        {
            return ViewRepository.GetInstance().FindFilter(idEstacao);
        }
    }
}
