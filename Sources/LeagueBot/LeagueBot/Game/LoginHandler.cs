using System;
using LeagueBot.ApiHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBot.Game
{
    public class LoginHandler
    {
        public static void Login(string username, string password)
        {
            InputHelper.LeftClick(407, 461);
            InputHelper.InputWords(username);
            BotHelper.Wait(2000);
            InputHelper.LeftClick(427, 528);
            InputHelper.InputWords(password);
            BotHelper.Wait(1000);
            InputHelper.LeftClick(518, 677);
        }
    }
}
