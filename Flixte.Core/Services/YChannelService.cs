using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class YChannelService
    {
        public static YChannel GetYChannel(int id)
        {
            return YChannelRepository.GetInstance().FindByPK(id);
        }

        /// <summary>
        /// Insert into DB a Entity YChannel
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Insert(YChannel model)
        {            
            return YChannelRepository.GetInstance().Insert(model);
        }

        /// <summary>
        /// Update the entity YChannel in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Update(YChannel model)
        {
            return YChannelRepository.GetInstance().Update(model);
        }

        /// <summary>
        /// Delete the entity YChannel in DB
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public static bool Delete(int id)
        {
            return YChannelRepository.GetInstance().Delete(id);
        }

        /// <summary>
        /// Find all YChannel
        /// </summary>
        /// <returns>List of YChannel</returns>
        public static List<YChannel> FindAll()
        {
            return YChannelRepository.GetInstance().FindAll();
        }
        /// <summary>
        /// Find all YChannel
        /// </summary>
        /// <returns>List of YChannel</returns>
        public static List<YChannel> FindAllWithInactive()
        {
            return YChannelRepository.GetInstance().FindAllWithInactive();
        }
        /// <summary>
        /// Find YChannel
        /// </summary>
        /// <param name="nome">string</param>
        /// <param name="login">string</param>
        /// <returns>List of YChannel DataSet</returns>
        public static List<YChannel> FindByFilter(string nome)
        {
            return YChannelRepository.GetInstance().FindFilter(nome);
        }
    }
}
