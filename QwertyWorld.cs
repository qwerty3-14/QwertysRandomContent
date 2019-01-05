using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using System.Collections.Generic;

using System;
using Terraria.ModLoader.IO;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace QwertysRandomContent
{
    public class QwertyWorld : ModWorld
    {
        public bool DinoEvent = false;
        public int DinoKillCount = 0;
        public int MaxDinoKillCount;
        public static int AbstractiveBlock = 0;
        public bool hasGeneratedRhuthinium;
        public static bool downedAncient;
        public static bool downedhydra;
        public static bool downedRuneGhost;
        public static bool downedB4;
        public static bool downedDinoMilitia;
        public static bool downedDinoMilitiaHard;
        public static bool downedTyrant;
        public static bool hasSummonedFortressBoss;
        public static bool downedFortressBoss;
        public static bool downedBear;
        public static bool downedBlade;
        public override void Initialize()
        {
            hasGeneratedRhuthinium = false;
            downedAncient = false;
            downedhydra = false;
            downedRuneGhost = false;
            downedB4 = false;
            downedDinoMilitia = false;
            downedDinoMilitiaHard = false;
            downedTyrant = false;
            hasSummonedFortressBoss=false;
            downedFortressBoss = false;
            downedBear = false;
            downedBlade = false;
        }
        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"genned", hasGeneratedRhuthinium},
                {"downedAncient", downedAncient},
                {"downedHydra", downedhydra},
                {"downedRuneGhost", downedRuneGhost},
                {"downedB4", downedB4},
                {"downedDinoMilitia", downedDinoMilitia},
                {"downedDinoMilitiaHard", downedDinoMilitiaHard},
                {"downedTyrant", downedTyrant},
                {"summonedFortressBoss",  hasSummonedFortressBoss},
                {"downedFortressBoss", downedFortressBoss },
                {"downedBear", downedBear},
                {"downedBlade", downedBlade }
            };
        }
        public override void Load(TagCompound tag)
        {
            hasGeneratedRhuthinium = tag.GetBool("genned");
            downedAncient = tag.GetBool("downedAncient");
            downedhydra= tag.GetBool("downedHydra");
            downedRuneGhost = tag.GetBool("downedRuneGhost");
            downedB4 = tag.GetBool("downedB4");
            downedDinoMilitia = tag.GetBool("downedDinoMilitia");
            downedDinoMilitiaHard = tag.GetBool("downedDinoMilitiaHard");
            downedTyrant = tag.GetBool("downedTyrant");
            hasSummonedFortressBoss = tag.GetBool("summonedFortressBoss");
            downedFortressBoss = tag.GetBool("downedFortressBoss");
            downedBear = tag.GetBool("downedBear");
            downedBlade = tag.GetBool("downedBlade");
        }
        public static void FortressBossQuotes()
        {
            string key;
            if (hasSummonedFortressBoss)
            {
                key = "Mods.QwertysRandomContent.DivineRage";
            }
            else
            {
                key = "Mods.QwertysRandomContent.DivineIntro";
            }
           
            Color messageColor = Color.Orange;
            if (Main.netMode == 2) // Server
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
            }
            else if (Main.netMode == 0) // Single Player
            {
                Main.NewText(Language.GetTextValue(key), messageColor);
            }
        }
        public override void ResetNearbyTileEffects()
        {
            QwertyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<QwertyPlayer>(mod);

            AbstractiveBlock = 0;
        }
        public override void TileCountsAvailable(int[] tileCounts)
        {
            AbstractiveBlock = tileCounts[mod.TileType("Abstractive")];
        }
        
        public override void PreUpdate()
        {
            
           
            if (DinoEvent)
            {

            }
            if (NPC.downedMoonlord)
            {
                MaxDinoKillCount = 300;
                if (DinoKillCount >= MaxDinoKillCount)
                {
                    DinoEvent = false;
                    downedDinoMilitia = true;
                    downedDinoMilitiaHard = true;
                    string key = "Mods.QwertysRandomContent.DinoDefeat";
                    Color messageColor = Color.Orange;
                    if (Main.netMode == 2) // Server
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                    }
                    else if (Main.netMode == 0) // Single Player
                    {
                        Main.NewText(Language.GetTextValue(key), messageColor);
                    }
                    DinoKillCount = 0;

                }
            }
            else
            {
                MaxDinoKillCount = 150;
                if (DinoKillCount >= MaxDinoKillCount)
                {
                    DinoEvent = false;
                    downedDinoMilitia = true;
                    string key = "Mods.QwertysRandomContent.DinoDefeat";
                    Color messageColor = Color.Orange;
                    if (Main.netMode == 2) // Server
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                    }
                    else if (Main.netMode == 0) // Single Player
                    {
                        Main.NewText(Language.GetTextValue(key), messageColor);
                    }
                    DinoKillCount = 0;

                }
            }
           
            if (!hasGeneratedRhuthinium && NPC.downedBoss3)
            {
                
                for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-06); i++)
                {
                    WorldGen.OreRunner(
                        WorldGen.genRand.Next(0, Main.maxTilesX), // X Coord of the tile
                        WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY-200), // Y Coord of the tile
                        (double)WorldGen.genRand.Next(40, 40), // Strength (High = more)
                        WorldGen.genRand.Next(2, 6), // Steps 
                        (ushort)mod.TileType("RhuthiniumOre") // The tile type that will be spawned
                       ); // Overrides existing tiles
                }
                string key = "Mods.QwertysRandomContent.RhuthiniumGenerates";
                Color messageColor = Color.Cyan;
                if (Main.netMode == 2) // Server
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == 0) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                hasGeneratedRhuthinium = true;
            }
        }
        
        
    }


}
