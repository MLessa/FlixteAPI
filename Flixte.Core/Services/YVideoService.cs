using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class YVideoService
    {
        public static YVideo GetYVideo(int id)
        {
            return YVideoRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity YVideo
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(YVideo model)
        {            
            return YVideoRepository.GetInstance().Insert(model);
        }

        /// <summary>
        /// Update the entity YVideo in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Update(YVideo model)
        {
            return YVideoRepository.GetInstance().Update(model);
        }

        /// <summary>
        /// Delete the entity YVideo in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return YVideoRepository.GetInstance().Delete(id);
        }

        /// <summary>
        /// Find all YVideo
        /// </summary>
        /// <returns>List of YVideo</returns>
        public static List<YVideo> FindAll()
        {
            return YVideoRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all YVideo
        /// </summary>
        /// <returns>List of YVideo</returns>
        public static List<YVideo> FindAllWithInactive()
        {
            return YVideoRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find YVideo
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of YVideo DataSet</returns>
        public static List<YVideo> FindByFilter(string nome, int yChannelID)
        {
            return YVideoRepository.GetInstance().FindFilter(nome, yChannelID);
        }
    }
}
