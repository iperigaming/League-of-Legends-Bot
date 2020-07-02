using LeagueBot.DesignPattern;
using LeagueBot.Game;
using LeagueBot.Api;
using LeagueBot.Game.Enums;
using LeagueBot.Image;
using LeagueBot.IO;
using LeagueBot.Patterns;
using LeagueBot.Texts;
using LeagueBot.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static LeagueBot.Windows.Interop;
using LeagueBot.ApiHelpers;

namespace LeagueBot
{
    /* 
     * The user input settings (game mode, champion , champion behaviour)
     * -> We create SessionParameters.cs that we pass to each scripts.
     * Or
     * 
     * The user input settings (game mode, champion , champion behaviour)
     * -> We create Session.cs that call scripts.
     */
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Logger.OnStartup();

            StartupManager.Initialize(Assembly.GetExecutingAssembly());

            checkChampList();

            /* if (args.Length == 0)
                HandleCommand(string.Empty);
            else
                HandleCommand(args[0]); */

            BotApi bot = new BotApi();
            Random r = new Random();
            Process.Start("C:\\Riot Games\\League of Legends\\LeagueClient.exe");
            bot.waitProcessOpen("RiotClientServices");

            BotHelper.Wait(3000);
            LoginHandler.Login("elighnothy", "test1234");
            BotHelper.Wait(3000);
            if (TextHelper.TextExists(769, 271, 262, 41, "TERMS OF SERVICE"))
            {
                Logger.Write("Accept the Terms of Service!");
                for (int i = 0; i < 100; i++)
                {
                    if (ImageHelper.GetColor(816, 739) == "#BC252A")
                        break;

                    InputHelper.LeftClick(1168, 642);
                }
                InputHelper.LeftClick(816, 739);
            }

            bot.waitProcessOpen("LeagueClientUX");
            bot.bringProcessToFront("LeagueClientUX");
            bot.centerProcess("LeagueClientUX");

            Logger.Write("Bot is ready!");
            BotHelper.Wait(10000);

            // 421 220
            InputHelper.LeftClick(421, 220);
            BotHelper.Wait(r.Next(800, 1300));
            // 464 285
            InputHelper.LeftClick(464, 285);
            BotHelper.Wait(r.Next(800, 1300));
            // 765 676
            InputHelper.LeftClick(765, 676);
            BotHelper.Wait(r.Next(800, 1300));
            // 824 870
            InputHelper.LeftClick(824, 870);
            BotHelper.Wait(r.Next(800, 1300));

            InputHelper.LeftClick(824, 870);

            TextHelper.WaitForText(827, 555, 267, 36, "MATCH FOUND");
            Logger.Write("MATCH FOUND!");
            BotHelper.Wait(r.Next(800, 2000));
            // InputHelper.LeftClick(960, 741);
            Logger.Write("MATCH ACCEPTED!");

            Console.Read();
        }

        static void HandleCommand(string restart)
        {
            string line;

            if (restart == string.Empty)
            {
                Logger.Write("Enter a pattern filename, type 'help' for help.", MessageState.INFO);
                line = Console.ReadLine();
            } else { line = restart; }
            
            
            // PatternsManager.Execute(line);

            HandleCommand(string.Empty);
        }

        private static void checkChampList()
        {
            string path = Directory.GetCurrentDirectory() + "\\champlist.txt";
            if (!File.Exists(path))
            {
                Logger.WriteColor1("champlist.txt not found. Creating file...\nPlease fill the list that has been generated with champion names.\nNOTE: One champion per line.");
                File.Create(path);
            }
        }
    }
}
