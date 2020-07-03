using LeagueBot.DesignPattern;
using LeagueBot.Game;
using LeagueBot.Api;
using LeagueBot.Game.Enums;
using LeagueBot.Game.Misc;
using LeagueBot.Image;
using LeagueBot.IO;
using LeagueBot.Patterns;
using LeagueBot.Texts;
using LeagueBot.Windows;
using System;
using System.Globalization;
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
            GameApi game = new GameApi();

            Random r = new Random();
            /* Process.Start("C:\\Riot Games\\League of Legends\\LeagueClient.exe");
            bot.waitProcessOpen("RiotClientServices");

            BotHelper.Wait(3000);

            string username = "testperi";
            string password = "testperi1";
            LoginHandler.Login(username, password);
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

            Logger.Write("CLIENT IS OPEN!");
            BotHelper.Wait(10000);

            if (TextHelper.TextExists(725, 425, 500, 50, "What name will you go by in the game?"))
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                string nUsername = myTI.ToTitleCase(username);
                Logger.Write("Set Name to " + nUsername + "!");
                InputHelper.LeftClick(750, 540);
                InputHelper.InputWords(nUsername);
                BotHelper.Wait(3000);

            }

                // 421 220testperitestperi
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
            BotHelper.Wait(r.Next(1000, 1700));

            InputHelper.LeftClick(824, 870);
            Logger.Write("SEARCH MATCH...");
            BotHelper.Wait(r.Next(800, 2000));

            while(true)
            {
                TextHelper.WaitForText(827, 555, 267, 36, "MATCH FOUND");

                InputHelper.LeftClick(960, 741);
                Logger.Write("MATCH ACCEPTED!");
                BotHelper.Wait(3000);

                if (TextHelper.TextExists(758, 192, 441, 35, "CHOOSE YOUR CHAMPION!"))
                    break;
            }

            // Search Champion
            InputHelper.LeftClick(1095, 258);
            BotHelper.Wait(r.Next(800, 1300));

            InputHelper.InputWords("ezreal");
            BotHelper.Wait(r.Next(800, 1300));

            // Select Champion
            InputHelper.LeftClick(702, 349);
            BotHelper.Wait(r.Next(800, 1300));

            InputHelper.LeftClick(963, 787);
            BotHelper.Wait(r.Next(800, 1300));

            Logger.Write("PICKED CHAMP!"); */
            // BotHelper.Wait(95000);

            string GAME_PROCESS_NAME = "League of Legends";
            Logger.Write("process done^TEST");
            bot.waitProcessOpen(GAME_PROCESS_NAME);
            bot.waitUntilProcessBounds(GAME_PROCESS_NAME, 1030, 797);
            bot.wait(200);

            bot.bringProcessToFront(GAME_PROCESS_NAME);
            bot.centerProcess(GAME_PROCESS_NAME);

            Logger.Write("process done");

            game.waitUntilGameStart();

            bot.log("We are in game!");

            bot.bringProcessToFront(GAME_PROCESS_NAME);
            bot.centerProcess(GAME_PROCESS_NAME);

            bot.wait(1000);

            game.detectSide();

            Point CastTargetPoint;

            if (game.getSide() == SideEnum.Blue)
            {
                CastTargetPoint = new Point(1084, 398);
                bot.log("We are blue side!");
            }
            else
            {
                CastTargetPoint = new Point(644, 761);
                bot.log("We are red side!");
            }

            int allyIndex = 2;
            bool CreepHasBeenFound = false;

            bot.wait(1000);

            game.player.setLevel(0);

            #region items

            Item[] items = {
                            new Item("Vampiric Scepter",900,false,false, 0, game.shop.getItemPosition(ShopItemTypeEnum.Early,1)),
                            new Item("Long Sword", 350, false, false, 0, game.shop.getItemPosition(ShopItemTypeEnum.Early,0)),
                            new Item("Berserker's Greaves", 1100, false, false, 0, game.shop.getItemPosition(ShopItemTypeEnum.Essential,2)),
                            new Item("B.F. Sword", 1300, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Offensive,1)),
                            new Item("Bloodthirster", 3500, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Offensive,2)),
                            new Item("B.F. Sword", 1300, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Offensive,1)),
                            new Item("Infinity Edge", 3400, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Defensive,2)),
                            new Item("Statikk Shiv", 2600, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Defensive,0)),
                            new Item("Frozen Mallet", 3100, false, false, 0,  game.shop.getItemPosition(ShopItemTypeEnum.Defensive,1)),
                };

            //if want another itemset, just copy and paste and change SELECTED_CHAMPION_SET value

            #endregion

            List<Item> itemsToBuy = new List<Item>(items);

            game.shop.setItemBuild(itemsToBuy);

            game.shop.toogle();

            bot.wait(1500);

            bot.wait(1000);
            game.player.fixItemsInShop();
            bot.wait(1000);
            game.shop.buyItem(1);
            game.shop.toogle();

            bot.wait(3000);

            game.player.moveNearestBotlaneAllyTower();

            while (bot.isProcessOpen(GAME_PROCESS_NAME)) // Game loop
            {

                bot.bringProcessToFront(GAME_PROCESS_NAME);
                bot.centerProcess(GAME_PROCESS_NAME);


                if (game.player.getCharacterLeveled())
                {
                    game.player.increaseLevel();
                    game.player.upSpells(); //Change order on MainPlayer.cs
                }

                int health = game.player.getHealthPercent();

                //back base/buy
                if (health <= 50)
                {
                    //heal usage if is available
                    if (game.player.isThereAnEnemy())
                        game.player.tryCastSpellToCreep(6);
                }

                if (health <= 88)
                {
                    //heal usage if is available
                    if (game.player.isThereAnEnemy())
                        game.player.tryCastSpellToCreep(5);
                }

                if (health <= 25)
                {
                    //low hp.
                    game.player.moveNearestBotlaneAllyTower();
                    bot.wait(8000);
                    game.player.backBaseRegenerateAndBuy();
                    // read gold.
                    game.shop.toogle();
                    game.shop.tryBuyItem();
                    game.shop.toogle();
                    bot.wait(200);

                    game.player.moveNearestBotlaneAllyTower();
                    bot.wait(6000);
                    //prevent getting stucked by doing it again
                    game.player.moveNearestBotlaneAllyTower();
                }

                //getting attacked by enemy, tower or creep.

                if (game.player.isThereAnAllyCreep())
                {
                    //attack enemy and run away
                    if (game.player.isThereAnEnemy())
                    {
                        game.player.processSpellToEnemyChampions();
                        game.player.moveAwayFromEnemy();
                    }
                    else
                    {
                        if (game.player.isThereAnEnemyCreep())
                        {
                            game.player.processSpellToEnemyCreeps();
                            game.player.moveAwayFromCreep();
                        }
                        bot.wait(100);
                        game.player.allyCreepPosition();
                        CreepHasBeenFound = true;
                    }
                }
                else
                {
                    // Just run away, no allies to find.

                    if (game.player.isThereAnEnemy())
                    {
                        game.player.processSpellToEnemyChampions();
                        game.player.moveAwayFromEnemy();
                    }

                    if (game.player.isThereAnEnemyCreep())
                    {
                        game.player.processSpellToEnemyCreeps();
                        game.player.moveAwayFromCreep();
                    }

                    if (!game.player.isThereAnAllyCreep() && !game.player.isThereAnEnemy() && !game.player.nearTowerStructure() && !game.player.isThereAnEnemyCreep())
                    {
                        bot.log("im lost help!");
                        if (game.player.tryMoveLightArea(1397, 683, "#65898F")) { }
                        else if (game.player.tryMoveLightArea(966, 630, "#65898F")) { }
                        else if (game.player.tryMoveLightArea(1444, 813, "#919970")) { }
                        else
                        {
                            if (CreepHasBeenFound)
                                game.camera.lockAlly(allyIndex);
                            else
                            {
                                allyIndex = incAllyIndex(allyIndex);

                                bot.wait(5000);
                            }


                            game.moveCenterScreen();
                            if (!game.player.isThereAnAllyCreep() || !game.player.isThereAnEnemyCreep()) //if player just afks, change index.
                            {
                                allyIndex = incAllyIndex(allyIndex);
                            }

                            bot.wait(500);

                        }
                    }

                }

            }


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

        static int incAllyIndex(int allyIndex)
        {
            return (allyIndex < 5) ? ++allyIndex : 1;
        }

        static void cameraFix()
        {
            GameApi game = new GameApi();
            BotApi bot = new BotApi();

            string img = Directory.GetCurrentDirectory() + @"\Images\Game\unfixedcamera.png";
            if (File.Exists(img))
            {
                game.camera.toggleIfUnlocked();
            }
            else
            {
                bot.log("cameraFix error: Image not found.");
            }
        }
    }
}
