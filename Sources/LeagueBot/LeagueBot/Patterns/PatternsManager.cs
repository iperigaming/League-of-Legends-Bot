using LeagueBot.Api;
using LeagueBot.DesignPattern;
using LeagueBot.IO;
using LeagueBot.Windows;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueBot.Patterns
{
    class PatternsManager
    {
        public const string PATH = "Patterns\\";

        public const string EXTENSION = ".cs";

        static Dictionary<string, Type> Scripts = new Dictionary<string, Type>();

        [StartupInvoke("Patterns", StartupInvokePriority.SecondPass)]
        public static void Initialize()
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.OutputAssembly = string.Empty;
            parameters.IncludeDebugInformation = false;

            parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");

            var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, PATH)).Where(x => Path.GetExtension(x) == EXTENSION).ToArray();

            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, files);

            if (results.Errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError err in results.Errors)
                {
                    sb.AppendLine(string.Format("{0}({1},{2}) : {3}", Path.GetFileName(err.FileName), err.Line, err.Column, err.ErrorText));
                }
                Logger.Write(sb.ToString(), MessageState.WARNING);
                Console.Read();
                Environment.Exit(0);
            }

            codeProvider.Dispose();

            foreach (var type in results.CompiledAssembly.GetTypes())
            {
                Scripts.Add(type.Name, type);
            }

        }
        public static bool Contains(string name)
        {
            return Scripts.ContainsKey(name);
        }
        public static void Execute(string name)
        {
            if (name == "help")
            {
                Logger.WriteColor1("The following scripts were found:");
                Logger.Write(PatternsManager.ToString());
                Logger.WriteColor1("Apply game settings? Type 'apply settings'");
            }
            else if(name.ToLower() == "apply settings")
            {
                try
                {
                    Game.Settings.LeagueManager.ApplySettings();
                }
                catch (UnauthorizedAccessException e)
                {
                    Logger.Write("<ERROR> - Permission denied.");
                }
                catch (Exception e)
                {
                    Logger.Write(e.ToString());
                }
            }
            else if (name == "Restart")
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("LeagueBot.exe");
                startInfo.Arguments = "StartCoop";
                Process.Start(startInfo);
                Environment.Exit(0);
            }
            else if (!Scripts.ContainsKey(name))
            {
                Logger.Write("Unable to execute " + name + EXTENSION + ". Script not found.", MessageState.WARNING);
            }
            else if (name == "start")
            {
               /*
                // Start Script

                Process.Start("C:\\Riot Games\\League of Legends\\LeagueClient.exe");
                BotApi.waitProcessOpen("RiotClientServices");

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

                BotApi.waitProcessOpen("LeagueClientUX");
                BotApi.bringProcessToFront("LeagueClientUX");
                BotApi.centerProcess("LeagueClientUX");

                Logger.Write("Bot is ready!");
                BotHelper.Wait(10000);

                // 421 220
                InputHelper.LeftClick(421, 220);
                BotHelper.Wait(1000);
                // 464 285
                InputHelper.LeftClick(464, 285);
                BotHelper.Wait(1000);
                // 765 676
                InputHelper.LeftClick(765, 676);
                BotHelper.Wait(1000);
                // 824 870
                InputHelper.LeftClick(824, 870);
                BotHelper.Wait(2000);
                InputHelper.LeftClick(824, 870);

                TextHelper.WaitForText(827, 555, 267, 36, "MATCH FOUND");
                Logger.Write("MATCH FOUND!");
                InputHelper.LeftClick(960, 741);
                Logger.Write("MATCH ACCEPTED!");
                */
            }
            else
            {
                PatternScript script = (PatternScript)Activator.CreateInstance(Scripts[name]);
                script.bot = new BotApi();
                script.client = new LCU();
                script.game = new GameApi();
                script.io = new FileIO(Directory.GetCurrentDirectory() + "\\champlist.txt");
                script.Execute();
            }
        }

        public static string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var script in Scripts)
            {
                sb.AppendLine("-" + script.Key);
            }

            return sb.ToString();
        }
    }
}
