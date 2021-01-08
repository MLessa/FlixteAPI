using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core
{
    public class TokenUserCache
    {
        public string TokenID;
        public Models.Usuario User;
        public DateTime LastUse;
    }
    
    public class Util
    {
        private static string[] caracteres = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "z" ,
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "z" ,
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "z" ,
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "z" ,
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "z" ,
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "z" ,
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "z" };
        private static string[] numbers = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private const int DefaultPassSize = 6;
        public static string GeneratePassword()
        {
            Random rand = new Random();
            string pass = "";
            for (int i = 0; i < DefaultPassSize; i++)
            {
                if (i % 2 == 0)
                    pass += caracteres[rand.Next(0, caracteres.Length - 1)];
                else
                    pass += numbers[rand.Next(0, numbers.Length - 1)];
            }
            return pass;
        }
        public static Models.Usuario GetUser(string tokenID, int? usuarioID)
        {
            Models.Usuario user = null;
            lock (ListUsers)
            {
                int totalMinutesToCache = 10;                
                if (usuarioID.HasValue)
                {
                    var cachedUser = ListUsers.FirstOrDefault(x => x.User.ID == usuarioID.Value);
                    if (cachedUser == null)
                    {
                        user = Services.UsuarioService.GetUsuario(usuarioID.Value);
                        if (user != null)
                            ListUsers.Add(new TokenUserCache() { LastUse = DateTime.Now.AddMinutes(totalMinutesToCache), TokenID = null, User = user });
                    }
                    else
                    {
                        user = cachedUser.User;
                        cachedUser.LastUse = DateTime.Now.AddMinutes(totalMinutesToCache);
                    }
                }
                else
                {
                    var cachedUser = ListUsers.FirstOrDefault(x => x.TokenID == tokenID);
                    if (cachedUser == null)
                    {
                        user = Services.TokenService.FindByToken(tokenID);
                        if (user != null)
                        {
                            var cachedUserByID = ListUsers.FirstOrDefault(x => x.User.ID == usuarioID.Value);
                            if (cachedUserByID != null)
                                ListUsers.RemoveAll(x => x.User.ID == usuarioID.Value);
                            ListUsers.Add(new TokenUserCache() { LastUse = DateTime.Now.AddMinutes(totalMinutesToCache), TokenID = tokenID, User = user });
                        }
                    }
                    else
                    {
                        cachedUser.LastUse = DateTime.Now.AddMinutes(totalMinutesToCache);
                        user = cachedUser.User;
                    }
                }
                ListUsers.RemoveAll(x => x.LastUse <= DateTime.Now);
            }
            return user;
        }
        public static List<TokenUserCache> ListUsers = new List<TokenUserCache>();
        public const string CookieTokenName = "TkFx";
        public static string MD5Encode(string input)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            return ByteToHexString(data);
        }
     
        private static string ByteToHexString(byte[] data)
        {
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            return sBuilder.ToString();
        }

        public enum UserType
        {
            Padrao = 1,
            Expert = 2
        }

        public static string GetURL(string url)
        {
            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(url);

            // *** Set Properties            
            loHttp.Timeout = int.MaxValue;
            loHttp.AllowAutoRedirect = true;
            loHttp.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36";
            
            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();

            //Encoding enc = System.Text.Encoding.GetEncoding(1252);
            Encoding enc = Encoding.UTF8;

            StreamReader loResponseStream =
               new StreamReader(loWebResponse.GetResponseStream(), enc);

            string lcHtml = loResponseStream.ReadToEnd();
            
            loWebResponse.Close();
            loResponseStream.Close();

            return lcHtml;
        }
    }
}
