﻿
using System;
using System.Collections.Generic;
using System.Drawing;
using LeagueBot.Patterns;

namespace LeagueBot
{
    public class EndCoop : PatternScript
    {
        
        public override void Execute()
        {
             bot.log("Match ended.");

            bot.waitProcessOpen(CLIENT_PROCESS_NAME);

            bot.bringProcessToFront(CLIENT_PROCESS_NAME);
            bot.centerProcess(CLIENT_PROCESS_NAME);

            bot.wait(5000);

            client.skipHonor();

            bot.wait(4000);

            if (client.levelUp())
            {
                bot.log("level up!");
                client.skipLevelRewards();
            }

            bot.wait(4000);

            while (client.questCompleted())
            {
                bot.log("quest completed!");
                client.skipLevelRewards();
            }

            bot.wait(4000);

            client.closeGameRecap();

            bot.wait(4000);

            bot.executePattern("StartCoop");
        }
    }
}
