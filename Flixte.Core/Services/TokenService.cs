using Flixte.Core.Models;
using Flixte.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Services
{
    public class TokenService
    {
        public static bool SaveToken(Token token)
        {
            return TokenRepository.GetInstance().SaveToken(token);
        }

        public static Token FindByUser(int id)
        {
            return TokenRepository.GetInstance().FindByUser(id);
        }
        public static Usuario FindByToken(string token)
        {
            return TokenRepository.GetInstance().FindByToken(token);
        }
        public static bool Update(Token model)
        {
            return TokenRepository.GetInstance().Update(model);
        }
    }
}
