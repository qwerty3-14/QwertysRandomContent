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
using QwertysRandomContent.NPCs.FortressBoss;

namespace QwertysRandomContent
{
    public class FortressBiome : ModPlayer
    {
        public bool TheFortress = false;
        public override void UpdateBiomes()
        {
            TheFortress = (SkyFortress.fortressBrick > 200) && ( ((Main.maxTilesX < 6000) && (player.Center.Y / 16) < 160)  || ((Main.maxTilesX < 8000 && Main.maxTilesX > 6000) && (player.Center.Y / 16) < 250)  || ((Main.maxTilesX >8000) &&(player.Center.Y/16)<350));
        }
        public override bool CustomBiomesMatch(Player other)
        {
            FortressBiome modOther = other.GetModPlayer<FortressBiome>(mod);
            return TheFortress == modOther.TheFortress;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            FortressBiome modOther = other.GetModPlayer<FortressBiome>(mod);
            modOther.TheFortress = TheFortress;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = TheFortress;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            TheFortress = flags[0];
        }
        public override void PostUpdate()
        {
           
        }
    }
    public class FortressSpawn : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.GetModPlayer<FortressBiome>(mod).TheFortress)
            {
                if (NPC.AnyNPCs(mod.NPCType<FortressBoss>()))
                {
                    spawnRate = 0;
                    maxSpawns = 0;
                }
                else
                {
                    if (Main.hardMode)
                    {
                        if (Main.dayTime)
                        {
                            spawnRate = 30;
                            maxSpawns = 12;
                        }
                        else
                        {
                            spawnRate = 34;
                            maxSpawns = 10;
                        }
                    }
                    else
                    {
                        if (Main.dayTime)
                        {
                            spawnRate = 34;
                            maxSpawns = 10;
                        }
                        else
                        {
                            spawnRate = 38;
                            maxSpawns = 8;
                        }
                    }
                }
                
                
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            
            if (spawnInfo.player.GetModPlayer<FortressBiome>(mod).TheFortress)
            {
                pool[0] = 0f;
            }
            //pool.Add(NPCID.Harpy, 0f);
        }

    }
    public class SkyFortress : ModWorld
    {
        public static int fortressBrick;
        public override void ResetNearbyTileEffects()
        {
            QwertyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<QwertyPlayer>(mod);

            fortressBrick = 0;
        }
        public override void TileCountsAvailable(int[] tileCounts)
        {
            fortressBrick = tileCounts[mod.TileType("FortressBrick")];
        }
        #region
        private static int numIslandHouses = 0;

        private static int houseCount = 0;
        private static bool[] skyLake = new bool[30];
        private static int[] fihX = new int[30];

        private static int[] fihY = new int[30];
        #endregion
        int tile1;
        int plat1 = TileID.Platforms;
        int wall1 = WallID.CobaltBrick;
        int pillar;
        
        public void Vertical(int i, int j, int type=0)
        {
            pillar = mod.TileType("FortressPillar");
            tile1 = mod.TileType("FortressBrick");
            if (type ==1) //basic vertical connection
            {
                int side = WorldGen.genRand.Next(2);
                for (int b = -1; b < 22; b++)
                {
                    for (int a = 0; a < 25; a++)
                    {
                        
                            if (side == 0)
                            {
                                if (a < 8 && b == 8)
                                {
                                    WorldGen.PlaceTile(i + a, j + b, tile1);
                                    if (a != 0 && a != 7)
                                    {
                                        WorldGen.PlaceTile(i + a, j + b + 1, tile1);
                                    }
                                }
                                else if (a > 16 && b == 12)
                                {

                                    WorldGen.PlaceTile(i + a, j + b, tile1);
                                    if (a != 17 && a != 24)
                                    {
                                        WorldGen.PlaceTile(i + a, j + b + 1, tile1);
                                    }
                                }
                            }
                            else
                            {
                                if (a < 8 && b ==12)
                                {
                                    WorldGen.PlaceTile(i + a, j + b , tile1);
                                    if (a != 0 && a != 7)
                                    {
                                        WorldGen.PlaceTile(i + a, j + b + 1, tile1);
                                    }
                                }
                                else if (a > 16 && b == 8)
                                {

                                    WorldGen.PlaceTile(i + a, j + b , tile1);
                                    if (a != 17 && a != 24)
                                    {
                                        WorldGen.PlaceTile(i + a, j + b + 1, tile1);
                                    }
                                }
                            }
                            
                        
                        if((a ==23 || a==1 || a ==18 || a==6) && !Main.tile[i + a, j + b].active())
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                    }
                }
            }
        }
        public void generateBridge(int i, int j, int type = 0)
        {
            tile1 = mod.TileType("FortressBrick");
            wall1 = mod.WallType("FortressWall");
            pillar = mod.TileType("FortressPillar");
            if (type ==0)
            {
                type = WorldGen.genRand.Next(1, 4);
            }
            if (type == 1) //basic bridge
            {
                for (int a = 0; a < 75; a++)
                {
                    for (int b = 0; b < 25; b++)
                    {
                        int extension = 28;
                        int upperExtension = 18;
                        if (b == 20 && (a < extension || a > 74 - extension))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 21 && (a < extension - 1 || a > 74 - extension + 1))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 22 && (a < extension - 2 || a > 74 - extension + 2))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 4 && (a < upperExtension || a > 74 - upperExtension))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 3 && (a < upperExtension - 1 || a > 74 - upperExtension + 1))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 2 && (a < upperExtension - 2 || a > 74 - upperExtension + 2))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (a%4 ==3 && !Main.tile[i + a, j + b].active() && (a < upperExtension - 1 || a > 74 - upperExtension + 1) && b>4 && b <20)
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                        #region walls
                        #endregion
                    }
                }

            }
            if (type == 2) //shorter bridge with island in center
            {
                for (int a = 0; a < 75; a++)
                {
                    for (int b = 0; b < 25; b++)
                    {
                        int extension = 8;
                        if (b == 20 && ((a < extension || a > 75 - extension) || (a > 25 && a < 49)))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 21 && ((a < extension - 1 || a > 75 - extension + 1) || (a > 26 && a < 48)))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 22 && ((a < extension - 2 || a > 75 - extension + 2) || (a > 27 && a < 47)))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if(b==12 && (a > 29 && a < 45))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                            if(a !=30 && a != 44)
                            {
                                WorldGen.PlaceTile(i + a, j + b-1, tile1);
                            }
                        }
                        if(b >12 && b <20 && (a == 31 || a == 43 || a == 35 || a == 39) && !Main.tile[i + a, j + b].active())
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                    }
                }
            }
            if (type == 3)
            {

                for (int b = 0; b < 25; b++)
                {
                    if (b >2 && b<22)
                    {
                        WorldGen.PlaceTile(i, j + b, tile1);
                        WorldGen.PlaceTile(i + 74, j + b, tile1);
                    }
                }

            }

        }
        public void generateRoom(int i, int j, int type = 0)
        {
            
            pillar = mod.TileType("FortressPillar");
            wall1 = mod.WallType("FortressWall");
            tile1 = mod.TileType("FortressBrick");
            plat1 = mod.TileType("FortressPlatform");
            if (type ==1) //Basic Room
            {
                #region tiles
                for(int b =0; b<25; b++)//Layers
                {
                    if (b ==0 || b== 24)
                    {
                        for(int a =0; a<25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {
                                
                                WorldGen.PlaceTile(i + a, j + b, plat1);
                            }
                            else if( a >=2 && a <= 22)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                            
                        }
                    }
                    else if (b == 1 || b == 23)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {
                                
                            }
                            else if(a >= 1 && a <= 23)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b < 5 || b > 19)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {
                               
                            }
                            else
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 11)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b==12)
                    {
                        for (int a = 0; a < 25; a++)
                        {

                            if (a >= 6 && a <= 18)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 13)
                    {
                        for (int a = 0; a < 25; a++)
                        {

                            if (a >= 7 && a <= 17)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }


                }
                #endregion
                #region walls
                for (int b = 0; b < 25; b++)//Layers
                {
                    for(int a= 0; a < 25; a++)
                    {
                        if(!Main.tile[i + a, j + b].active() && !(a ==0 && b==0) && !(a == 1 && b == 0) && !(a == 0 && b == 1) && !(a == 24 && b == 0) && !(a == 24 && b == 1) && !(a == 23 && b == 0) && !(a == 24 && b == 24) && !(a == 23 && b == 24) && !(a == 24 && b == 23) && !(a == 0 && b == 24) && !(a == 1 && b == 24) && !(a == 0 && b == 23))
                        {
                            WorldGen.PlaceWall(i + a, j + b, wall1);
                        }
                    }
                    
                }
                #endregion
                #region pillars
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if ((a == 8 || a == 16) && !Main.tile[i + a, j + b].active())
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                    }
                }
                #endregion
            }
            if (type == -1) //altar room
            {
                #region tiles
                for (int b = 0; b < 25; b++)//Layers
                {
                    if (b == 0 || b == 24)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b==0)
                            {

                                WorldGen.PlaceTile(i + a, j + b, plat1);
                            }
                            else if (a >= 2 && a <= 22)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }

                        }
                    }
                    else if (b == 1 || b == 23)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b == 1)
                            {

                            }
                            else if (a >= 1 && a <= 23)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b < 5 || b > 19)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b < 5)
                            {

                            }
                            else
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    if(b ==19)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 7 && a <= 17)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    if (b == 18)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 9 && a <= 15)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }


                }
                #endregion
                #region walls
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if (!Main.tile[i + a, j + b].active() && !(a == 0 && b == 0) && !(a == 1 && b == 0) && !(a == 0 && b == 1) && !(a == 24 && b == 0) && !(a == 24 && b == 1) && !(a == 23 && b == 0) && !(a == 24 && b == 24) && !(a == 23 && b == 24) && !(a == 24 && b == 23) && !(a == 0 && b == 24) && !(a == 1 && b == 24) && !(a == 0 && b == 23))
                        {
                            WorldGen.PlaceWall(i + a, j + b, wall1);
                        }
                    }

                }
                #endregion
                WorldGen.PlaceTile(i +12, j +17, mod.TileType("FortressAltar"));
                WorldGen.PlaceTile(i + 12, j + 12, mod.TileType("FortressCarving1"));
                WorldGen.PlaceTile(i + 18, j + 14, mod.TileType("FortressCarving2"));
                WorldGen.PlaceTile(i + 6, j + 14, mod.TileType("FortressCarving3"));
            }
            if (type == -2) //treausre room
            {
                #region tiles
                for (int b = 0; b < 25; b++)//Layers
                {
                    if (b == 0 || b == 24)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b == 0)
                            {

                                WorldGen.PlaceTile(i + a, j + b, plat1);
                            }
                            else if (a >= 2 && a <= 22)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }

                        }
                    }
                    else if (b == 1 || b == 23)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b == 1)
                            {

                            }
                            else if (a >= 1 && a <= 23)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b < 5 || b > 19)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b < 5)
                            {

                            }
                            else
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if(b >=5 && b<= 7)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if(a ==0 || a ==24)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b==8)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a <= 5 || a >= 19)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 11)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 12)
                    {
                        for (int a = 0; a < 25; a++)
                        {

                            if (a >= 6 && a <= 18)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 13)
                    {
                        for (int a = 0; a < 25; a++)
                        {

                            if (a >= 7 && a <= 17)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }


                }
                #endregion
                #region walls
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if (!Main.tile[i + a, j + b].active() && !(a == 0 && b == 0) && !(a == 1 && b == 0) && !(a == 0 && b == 1) && !(a == 24 && b == 0) && !(a == 24 && b == 1) && !(a == 23 && b == 0) && !(a == 24 && b == 24) && !(a == 23 && b == 24) && !(a == 24 && b == 23) && !(a == 0 && b == 24) && !(a == 1 && b == 24) && !(a == 0 && b == 23))
                        {
                            if (a != 0 && a != 24)
                            {
                                WorldGen.PlaceWall(i + a, j + b, wall1);
                            }
                        }
                    }

                }
                #endregion
                WorldGen.PlaceTile(i + 12, j + 19, TileID.Anvils);
                #region pillars
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if ((a == 8 || a == 16) && !Main.tile[i + a, j + b].active())
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                    }
                    if (b >= 5 && b <= 7)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a == 4 || a == 20)
                            {
                                WorldGen.PlaceTile(i + a, j + b, pillar);
                            }
                        }
                    }
                    if (b > 5 && b < 20)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if((a ==0 || a ==24 ) && !Main.tile[i + a, j + b].active())
                            {
                                WorldGen.PlaceTile(i + a, j + b, pillar);
                            }
                        }
                    }
                }

                #endregion
                #region treasure
                int gap = WorldGen.genRand.Next(0, 3);
                int gap2 = WorldGen.genRand.Next(0, 3);
                bool secondPlace = false;
                bool secondPlace2 = false;
                bool hasntPlaced = true;
                bool hasntPlaced2 = true;
                for (int b = 0; b < 25; b++)
                {
                    if (b == 7)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if ((a >= 21 && a <= 23))
                            {
                                WorldGen.PlaceTile(i + a, j + b, mod.TileType("CaeliteBar"));
                                if(21 + gap != a)
                                {
                                    WorldGen.PlaceTile(i + a, j + b - 1, mod.TileType("CaeliteBar"));
                                    if ((Main.rand.Next(1) == 0 || secondPlace) && hasntPlaced)
                                    {
                                        WorldGen.PlaceTile(i + a, j + b - 2, mod.TileType("CaeliteBar"));
                                        hasntPlaced = false;
                                    }
                                    else
                                    {
                                        secondPlace = true;
                                    }
                                }
                            }
                            if ((a >= 1 && a <= 3) )
                            {
                                WorldGen.PlaceTile(i + a, j + b, mod.TileType("CaeliteBar"));
                                if (1 + gap2 != a)
                                {
                                    WorldGen.PlaceTile(i + a, j + b - 1, mod.TileType("CaeliteBar"));
                                    if ((Main.rand.Next(1) == 0 || secondPlace2) && hasntPlaced2)
                                    {
                                        WorldGen.PlaceTile(i + a, j + b - 2, mod.TileType("CaeliteBar"));
                                        hasntPlaced2 = false;
                                    }
                                    else
                                    {
                                        secondPlace2 = true;
                                    }
                                }
                            }
                        }
                    }
                    /*
                    if (b == 6)
                    {
                        
                        for (int a = 0; a < 25; a++)
                        {
                            if ((a >= 1 && a <= 3 && 1+gap != a) )
                            {
                                
                                WorldGen.PlaceTile(i + a, j + b, mod.TileType("CaeliteBar"));
                                
                                if((Main.rand.Next(1) ==0|| secondPlace) && hasntPlaced)
                                {
                                    WorldGen.PlaceTile(i + a, j + b-1, mod.TileType("CaeliteBar"));
                                    hasntPlaced = false;
                                }
                                else
                                {
                                    secondPlace = true;
                                }
                                
                            }
                            if((a >= 21 && a <= 23 && 21 + gap != a))
                            {
                                WorldGen.PlaceTile(i + a, j + b, mod.TileType("CaeliteBar"));
                                
                                if ((Main.rand.Next(1) == 0 || secondPlace2) && hasntPlaced2)
                                {
                                    WorldGen.PlaceTile(i + a, j + b - 1, mod.TileType("CaeliteBar"));
                                    hasntPlaced2 = false;
                                }
                                else
                                {
                                    secondPlace2 = true;
                                }
                                
                            }
                        }
                        
                    }
                    */
                }
                #endregion

            }
        }
        public int startingX;
        
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            Mod Overrated = ModLoader.GetMod("CalamityMod");
            if (Overrated == null)
            {
                #region
                int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Islands"));
                if (ShiniesIndex != -1)
                {
                    tasks.RemoveAt(ShiniesIndex);
                }
                ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Island Houses"));
                if (ShiniesIndex != -1)
                {
                    tasks.RemoveAt(ShiniesIndex);
                }
                int skyLakes = 1;
                if (Main.maxTilesX > 8000)
                {
                    int skyLakes2 = skyLakes;
                    skyLakes = skyLakes2 + 1;
                }
                if (Main.maxTilesX > 6000)
                {
                    int skyLakes2 = skyLakes;
                    skyLakes = skyLakes2 + 1;
                }
                ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Full Desert"));
                if (ShiniesIndex != -1)
                {

                    tasks.Insert(ShiniesIndex + 1, new PassLegacy("Giving Sky Fortress space", delegate (GenerationProgress progress)
                    {

                        numIslandHouses = 0;
                        houseCount = 0;
                        progress.Message = Lang.gen[12].Value;
                        for (int k = 0; k < (int)((double)Main.maxTilesX * 0.0008) + skyLakes; k++)
                        {
                            int num = 1000;
                            int num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.3), (int)((double)Main.maxTilesX * 0.7));
                            if (WorldGen.dungeonX < Main.maxTilesX * .5f)
                            {
                                num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.3), (int)((double)Main.maxTilesX * 0.7));
                            }
                            else
                            {
                                num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.3), (int)((double)Main.maxTilesX * 0.7));
                            }

                            while (--num > 0)
                            {
                                bool flag2 = true;
                                while (num2 > Main.maxTilesX / 2 - 80 && num2 < Main.maxTilesX / 2 + 80)
                                {
                                    if (WorldGen.dungeonX < Main.maxTilesX * .5f)
                                    {
                                        num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.3), (int)((double)Main.maxTilesX * 0.7));
                                    }
                                    else
                                    {
                                        num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.3), (int)((double)Main.maxTilesX * 0.7));
                                    }
                                }
                                for (int l = 0; l < numIslandHouses; l++)
                                {
                                    if (num2 > fihX[l] - 180 && num2 < fihX[l] + 180)
                                    {
                                        flag2 = false;
                                        break;
                                    }
                                }
                                if (flag2)
                                {
                                    flag2 = false;
                                    int num3 = 0;
                                    int num4 = 200;
                                    while ((double)num4 < Main.worldSurface)
                                    {
                                        if (Main.tile[num2, num4].active())
                                        {
                                            num3 = num4;
                                            flag2 = true;
                                            break;
                                        }
                                        num4++;
                                    }
                                    if (flag2)
                                    {
                                        int num5 = WorldGen.genRand.Next(90, num3 - 100);
                                        num5 = Math.Min(num5, (int)WorldGen.worldSurfaceLow - 50);
                                        if (k < skyLakes)
                                        {
                                            skyLake[numIslandHouses] = true;
                                            WorldGen.CloudLake(num2, num5);
                                        }
                                        else
                                        {
                                            WorldGen.CloudIsland(num2, num5);
                                        }
                                        fihX[numIslandHouses] = num2;
                                        fihY[numIslandHouses] = num5;
                                        numIslandHouses++;
                                    }
                                }
                            }
                        }
                    }));
                }
                #endregion
                ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Jungle Trees"));
                if (ShiniesIndex != -1)
                {
                    #region

                    tasks.Insert(ShiniesIndex + 1, new PassLegacy("Giving Sky Fortress space", delegate (GenerationProgress progress)
                    {
                        for (int k = 0; k < numIslandHouses; k++)
                        {
                            if (!skyLake[k])
                            {
                                WorldGen.IslandHouse(fihX[k], fihY[k]);
                            }
                        }
                    }));

                    #endregion

                }
                ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Tile Cleanup"));

                if (ShiniesIndex != -1)
                {
                    tasks.Insert(ShiniesIndex + 1, new PassLegacy("Fortifying the Sky!!", delegate (GenerationProgress progress)
                    {

                        if (WorldGen.dungeonX < Main.maxTilesX * .5f)
                        {
                            startingX = (int)((double)Main.maxTilesX * 0.8);
                        }
                        else
                        {
                            startingX = (int)((double)Main.maxTilesX * 0.2);
                        }
                        int roomsPerLayer = 7;

                        if (Main.maxTilesX > 8000)
                        {
                            roomsPerLayer += 4;
                        }
                        if (Main.maxTilesX > 6000)
                        {
                            roomsPerLayer += 4;
                        }
                        int roomHeight = 25;
                        int roomWidth = 25;
                        int bridgeHeight = 25;
                        int bridgeWidth = 75;
                        int verticalHeight = 21;
                        int verticalWith = 25;
                        int HeightOffset = (roomHeight - 1) / 2;
                        int WidthOffset = (roomWidth - 1) / 2;
                        int startingY = (((int)WorldGen.worldSurfaceLow - 50) + (90)) / 2;
                        if (Main.maxTilesX > 8000)
                        {
                            startingY -= 25;
                        }
                        startingX -= WidthOffset + (((roomsPerLayer - 1) / 2) * (roomWidth + bridgeWidth));
                        startingY -= HeightOffset;
                    /*
                    generateRoom(startingX, startingY, 1);
                    generateBridge(startingX + roomWidth, startingY, 1);
                    generateRoom(startingX + roomWidth + bridgeWidth, startingY, 1);
                    */
                        int treasure = Main.rand.Next(roomsPerLayer - 1);
                        if (treasure >= (roomsPerLayer - 1) / 2)
                        {
                            treasure++;
                        }
                        for (int n = 0; n < roomsPerLayer; n++)
                        {
                            if (n == (roomsPerLayer - 1) / 2)
                            {
                                generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, -1);
                            }
                            else
                            {
                                if (n == treasure)
                                {
                                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, -2);
                                }
                                else
                                {
                                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, 1);
                                }

                            }

                            if (n != roomsPerLayer - 1)
                            {
                                generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY, 0);
                            }


                        }
                        treasure = Main.rand.Next(roomsPerLayer);
                        for (int n = 0; n < roomsPerLayer; n++)
                        {
                            if (n == treasure)
                            {
                                generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), -2);
                            }
                            else
                            {
                                generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), 1);
                            }
                            Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight) + roomHeight, 1);
                            if (n != roomsPerLayer - 1)
                            {
                                generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), 0);
                            }


                        }
                        if (Main.maxTilesX > 6000)
                        {
                            treasure = Main.rand.Next(roomsPerLayer);
                            for (int n = 0; n < roomsPerLayer; n++)
                            {
                                if (n == treasure)
                                {
                                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), -2);
                                }
                                else
                                {
                                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), 1);
                                }
                                Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight) - verticalHeight, 1);
                                if (n != roomsPerLayer - 1)
                                {
                                    generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), 0);
                                }


                            }
                            treasure = Main.rand.Next(roomsPerLayer);
                            for (int n = 0; n < roomsPerLayer; n++)
                            {
                                if (n == treasure)
                                {
                                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), -2);
                                }
                                else
                                {
                                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), 1);
                                }
                                Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight) + roomHeight, 1);
                                if (n != roomsPerLayer - 1)
                                {
                                    generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), 0);
                                }


                            }
                        }
                        if (Main.maxTilesX > 8000)
                        {
                            treasure = Main.rand.Next(roomsPerLayer);
                            for (int n = 0; n < roomsPerLayer; n++)
                            {
                                if (n == treasure)
                                {
                                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), -2);
                                }
                                else
                                {
                                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), 1);
                                }
                                Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight) - verticalHeight, 1);
                                if (n != roomsPerLayer - 1)
                                {
                                    generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), 0);
                                }


                            }

                        }

                    }));
                }
            }
        }
        public override void PostWorldGen()
        {

            Mod Overrated = ModLoader.GetMod("CalamityMod");
            if(Overrated != null)
            {
                 int startingX;
                float upperLimit = 60;
                float lowerLimit;
                int Center;
                int maxDistanceFromCenter;
                if (Main.dungeonX < Main.maxTilesX * .5f)
                {
                    Center = (int)((double)Main.maxTilesX * 0.8);
                }
                else
                {
                    Center = (int)((double)Main.maxTilesX * 0.2);
                }
                if (Main.maxTilesX > 8000)
                {
                    lowerLimit = 320;
                    maxDistanceFromCenter = 750;
                }
                else if (Main.maxTilesX > 6000)
                {
                    lowerLimit = 230;
                    maxDistanceFromCenter = 550;
                }
                else
                {
                    lowerLimit = 130;
                    maxDistanceFromCenter = 320;
                }
                QwertyMethods.BreakTiles(Center - maxDistanceFromCenter, 0, maxDistanceFromCenter * 2, (int)(lowerLimit));
                //Main.NewText(maxDistanceFromCenter);

                //Main.NewText(Main.maxTilesX);
                if (Main.dungeonX < Main.maxTilesX * .5f)
                {
                    startingX = (int)((double)Main.maxTilesX * 0.8);
                }
                else
                {
                    startingX = (int)((double)Main.maxTilesX * 0.2);
                }
                //Main.NewText(startingX);
                int roomsPerLayer = 7;

                if (Main.maxTilesX > 8000)
                {
                    roomsPerLayer += 4;

                }
                if (Main.maxTilesX > 6000)
                {
                    roomsPerLayer += 4;
                }
                else
                {

                }
                int roomHeight = 25;
                int roomWidth = 25;
                int bridgeHeight = 25;
                int bridgeWidth = 75;
                int verticalHeight = 21;
                int verticalWith = 25;
                int HeightOffset = (roomHeight - 1) / 2;
                int WidthOffset = (roomWidth - 1) / 2;
                int startingY = 110;
                if (Main.maxTilesX > 8000)
                {
                    startingY = 195;
                }
                else if (Main.maxTilesX > 6000)
                {
                    startingY = 145;
                }
                else
                {
                    startingY = 110;

                }
                if (Main.maxTilesX > 8000)
                {
                    startingY -= 25;
                }
                startingX -= WidthOffset + (((roomsPerLayer - 1) / 2) * (roomWidth + bridgeWidth));
                startingY -= HeightOffset;



                int treasure = Main.rand.Next(roomsPerLayer - 1);
                if (treasure >= (roomsPerLayer - 1) / 2)
                {
                    treasure++;
                }
                for (int n = 0; n < roomsPerLayer; n++)
                {
                    if (n == (roomsPerLayer - 1) / 2)
                    {
                        //Main.NewText(n + ", " + startingX + (n * (roomWidth + bridgeWidth)) + ", " + startingY);
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, -1);
                    }
                    else
                    {
                        if (n == treasure)
                        {
                            //Main.NewText(n + ", " + startingX + (n * (roomWidth + bridgeWidth)) + ", " + startingY);
                            generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, -2);
                        }
                        else
                        {
                            //Main.NewText(n + ", " + startingX + (n * (roomWidth + bridgeWidth)) + ", " + startingY);
                            generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, 1);
                        }

                    }

                    if (n != roomsPerLayer - 1)
                    {
                        generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY, 0);
                    }


                }
                treasure = Main.rand.Next(roomsPerLayer);
                for (int n = 0; n < roomsPerLayer; n++)
                {
                    if (n == treasure)
                    {
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), -2);
                    }
                    else
                    {
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), 1);
                    }
                    Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight) + roomHeight, 1);
                    if (n != roomsPerLayer - 1)
                    {
                        generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), 0);
                    }


                }
                if (Main.maxTilesX > 6000)
                {
                    treasure = Main.rand.Next(roomsPerLayer);
                    for (int n = 0; n < roomsPerLayer; n++)
                    {
                        if (n == treasure)
                        {
                            generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), -2);
                        }
                        else
                        {
                            generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), 1);
                        }
                        Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight) - verticalHeight, 1);
                        if (n != roomsPerLayer - 1)
                        {
                            generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), 0);
                        }


                    }
                    treasure = Main.rand.Next(roomsPerLayer);
                    for (int n = 0; n < roomsPerLayer; n++)
                    {
                        if (n == treasure)
                        {
                            generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), -2);
                        }
                        else
                        {
                            generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), 1);
                        }
                        Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight) + roomHeight, 1);
                        if (n != roomsPerLayer - 1)
                        {
                            generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), 0);
                        }


                    }
                }
                if (Main.maxTilesX > 8000)
                {
                    treasure = Main.rand.Next(roomsPerLayer);
                    for (int n = 0; n < roomsPerLayer; n++)
                    {
                        if (n == treasure)
                        {
                            generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), -2);
                        }
                        else
                        {
                            generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), 1);
                        }
                        Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight) - verticalHeight, 1);
                        if (n != roomsPerLayer - 1)
                        {
                            generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), 0);
                        }


                    }

                }
            }
        }

    }
    
    public class RegenFortress : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "RegenFortress"; }
        }

        public override string Description
        {
            get { return "Regenerates the sky fortress"; }
        }
       
        public void Vertical(int i, int j, int type = 0)
        {
            int pillar = mod.TileType("FortressPillar");
            int tile1 = mod.TileType("FortressBrick");
            if (type == 1) //basic vertical connection
            {
                int side = WorldGen.genRand.Next(2);
                for (int b = -1; b < 22; b++)
                {
                    for (int a = 0; a < 25; a++)
                    {

                        if (side == 0)
                        {
                            if (a < 8 && b == 8)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                                if (a != 0 && a != 7)
                                {
                                    WorldGen.PlaceTile(i + a, j + b + 1, tile1);
                                }
                            }
                            else if (a > 16 && b == 12)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                                if (a != 17 && a != 24)
                                {
                                    WorldGen.PlaceTile(i + a, j + b + 1, tile1);
                                }
                            }
                        }
                        else
                        {
                            if (a < 8 && b == 12)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                                if (a != 0 && a != 7)
                                {
                                    WorldGen.PlaceTile(i + a, j + b + 1, tile1);
                                }
                            }
                            else if (a > 16 && b == 8)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                                if (a != 17 && a != 24)
                                {
                                    WorldGen.PlaceTile(i + a, j + b + 1, tile1);
                                }
                            }
                        }


                        if ((a == 23 || a == 1 || a == 18 || a == 6) && !Main.tile[i + a, j + b].active())
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                    }
                }
            }
        }
        public void generateBridge(int i, int j, int type = 0)
        {
            int tile1 = mod.TileType("FortressBrick");
            int wall1 = mod.WallType("FortressWall");
            int pillar = mod.TileType("FortressPillar");
            if (type == 0)
            {
                type = WorldGen.genRand.Next(1, 4);
            }
            if (type == 1) //basic bridge
            {
                for (int a = 0; a < 75; a++)
                {
                    for (int b = 0; b < 25; b++)
                    {
                        int extension = 28;
                        int upperExtension = 18;
                        if (b == 20 && (a < extension || a > 74 - extension))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 21 && (a < extension - 1 || a > 74 - extension + 1))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 22 && (a < extension - 2 || a > 74 - extension + 2))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 4 && (a < upperExtension || a > 74 - upperExtension))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 3 && (a < upperExtension - 1 || a > 74 - upperExtension + 1))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 2 && (a < upperExtension - 2 || a > 74 - upperExtension + 2))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (a % 4 == 3 && !Main.tile[i + a, j + b].active() && (a < upperExtension - 1 || a > 74 - upperExtension + 1) && b > 4 && b < 20)
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                        #region walls
                        #endregion
                    }
                }

            }
            if (type == 2) //shorter bridge with island in center
            {
                for (int a = 0; a < 75; a++)
                {
                    for (int b = 0; b < 25; b++)
                    {
                        int extension = 8;
                        if (b == 20 && ((a < extension || a > 75 - extension) || (a > 25 && a < 49)))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 21 && ((a < extension - 1 || a > 75 - extension + 1) || (a > 26 && a < 48)))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 22 && ((a < extension - 2 || a > 75 - extension + 2) || (a > 27 && a < 47)))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                        }
                        if (b == 12 && (a > 29 && a < 45))
                        {
                            WorldGen.PlaceTile(i + a, j + b, tile1);
                            if (a != 30 && a != 44)
                            {
                                WorldGen.PlaceTile(i + a, j + b - 1, tile1);
                            }
                        }
                        if (b > 12 && b < 20 && (a == 31 || a == 43 || a == 35 || a == 39) && !Main.tile[i + a, j + b].active())
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                    }
                }
            }
            if (type == 3)
            {

                for (int b = 0; b < 25; b++)
                {
                    if (b > 2 && b < 22)
                    {
                        WorldGen.PlaceTile(i, j + b, tile1);
                        WorldGen.PlaceTile(i + 74, j + b, tile1);
                    }
                }

            }

        }
        public void generateRoom(int i, int j, int type = 0)
        {

            int pillar = mod.TileType("FortressPillar");
            int wall1 = mod.WallType("FortressWall");
            int tile1 = mod.TileType("FortressBrick");
            int plat1 = mod.TileType("FortressPlatform");
            if (type == 1) //Basic Room
            {
                #region tiles
                for (int b = 0; b < 25; b++)//Layers
                {
                    if (b == 0 || b == 24)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {

                                WorldGen.PlaceTile(i + a, j + b, plat1);
                            }
                            else if (a >= 2 && a <= 22)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }

                        }
                    }
                    else if (b == 1 || b == 23)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {

                            }
                            else if (a >= 1 && a <= 23)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b < 5 || b > 19)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {

                            }
                            else
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 11)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 12)
                    {
                        for (int a = 0; a < 25; a++)
                        {

                            if (a >= 6 && a <= 18)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 13)
                    {
                        for (int a = 0; a < 25; a++)
                        {

                            if (a >= 7 && a <= 17)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }


                }
                #endregion
                #region walls
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if (!Main.tile[i + a, j + b].active() && !(a == 0 && b == 0) && !(a == 1 && b == 0) && !(a == 0 && b == 1) && !(a == 24 && b == 0) && !(a == 24 && b == 1) && !(a == 23 && b == 0) && !(a == 24 && b == 24) && !(a == 23 && b == 24) && !(a == 24 && b == 23) && !(a == 0 && b == 24) && !(a == 1 && b == 24) && !(a == 0 && b == 23))
                        {
                            WorldGen.PlaceWall(i + a, j + b, wall1);
                        }
                    }

                }
                #endregion
                #region pillars
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if ((a == 8 || a == 16) && !Main.tile[i + a, j + b].active())
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                    }
                }
                #endregion
            }
            if (type == -1) //altar room
            {
                #region tiles
                for (int b = 0; b < 25; b++)//Layers
                {
                    if (b == 0 || b == 24)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b == 0)
                            {

                                WorldGen.PlaceTile(i + a, j + b, plat1);
                            }
                            else if (a >= 2 && a <= 22)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }

                        }
                    }
                    else if (b == 1 || b == 23)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b == 1)
                            {

                            }
                            else if (a >= 1 && a <= 23)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b < 5 || b > 19)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b < 5)
                            {

                            }
                            else
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    if (b == 19)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 7 && a <= 17)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    if (b == 18)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 9 && a <= 15)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }


                }
                #endregion
                #region walls
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if (!Main.tile[i + a, j + b].active() && !(a == 0 && b == 0) && !(a == 1 && b == 0) && !(a == 0 && b == 1) && !(a == 24 && b == 0) && !(a == 24 && b == 1) && !(a == 23 && b == 0) && !(a == 24 && b == 24) && !(a == 23 && b == 24) && !(a == 24 && b == 23) && !(a == 0 && b == 24) && !(a == 1 && b == 24) && !(a == 0 && b == 23))
                        {
                            WorldGen.PlaceWall(i + a, j + b, wall1);
                        }
                    }

                }
                #endregion
                WorldGen.PlaceTile(i + 12, j + 17, mod.TileType("FortressAltar"));
                WorldGen.PlaceTile(i + 12, j + 12, mod.TileType("FortressCarving1"));
                WorldGen.PlaceTile(i + 18, j + 14, mod.TileType("FortressCarving2"));
                WorldGen.PlaceTile(i + 6, j + 14, mod.TileType("FortressCarving3"));
            }
            if (type == -2) //treausre room
            {
                #region tiles
                for (int b = 0; b < 25; b++)//Layers
                {
                    if (b == 0 || b == 24)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b == 0)
                            {

                                WorldGen.PlaceTile(i + a, j + b, plat1);
                            }
                            else if (a >= 2 && a <= 22)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }

                        }
                    }
                    else if (b == 1 || b == 23)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b == 1)
                            {

                            }
                            else if (a >= 1 && a <= 23)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b < 5 || b > 19)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14 && b < 5)
                            {

                            }
                            else
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b >= 5 && b <= 7)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a == 0 || a == 24)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 8)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a <= 5 || a >= 19)
                            {
                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 11)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a >= 10 && a <= 14)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 12)
                    {
                        for (int a = 0; a < 25; a++)
                        {

                            if (a >= 6 && a <= 18)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }
                    else if (b == 13)
                    {
                        for (int a = 0; a < 25; a++)
                        {

                            if (a >= 7 && a <= 17)
                            {

                                WorldGen.PlaceTile(i + a, j + b, tile1);
                            }
                        }
                    }


                }
                #endregion
                #region walls
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if (!Main.tile[i + a, j + b].active() && !(a == 0 && b == 0) && !(a == 1 && b == 0) && !(a == 0 && b == 1) && !(a == 24 && b == 0) && !(a == 24 && b == 1) && !(a == 23 && b == 0) && !(a == 24 && b == 24) && !(a == 23 && b == 24) && !(a == 24 && b == 23) && !(a == 0 && b == 24) && !(a == 1 && b == 24) && !(a == 0 && b == 23))
                        {
                            if (a != 0 && a != 24)
                            {
                                WorldGen.PlaceWall(i + a, j + b, wall1);
                            }
                        }
                    }

                }
                #endregion
                WorldGen.PlaceTile(i + 12, j + 19, TileID.Anvils);
                #region pillars
                for (int b = 0; b < 25; b++)//Layers
                {
                    for (int a = 0; a < 25; a++)
                    {
                        if ((a == 8 || a == 16) && !Main.tile[i + a, j + b].active())
                        {
                            WorldGen.PlaceTile(i + a, j + b, pillar);
                        }
                    }
                    if (b >= 5 && b <= 7)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if (a == 4 || a == 20)
                            {
                                WorldGen.PlaceTile(i + a, j + b, pillar);
                            }
                        }
                    }
                    if (b > 5 && b < 20)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if ((a == 0 || a == 24) && !Main.tile[i + a, j + b].active())
                            {
                                WorldGen.PlaceTile(i + a, j + b, pillar);
                            }
                        }
                    }
                }

                #endregion
                #region treasure
                int gap = WorldGen.genRand.Next(0, 3);
                int gap2 = WorldGen.genRand.Next(0, 3);
                bool secondPlace = false;
                bool secondPlace2 = false;
                bool hasntPlaced = true;
                bool hasntPlaced2 = true;
                for (int b = 0; b < 25; b++)
                {
                    if (b == 7)
                    {
                        for (int a = 0; a < 25; a++)
                        {
                            if ((a >= 21 && a <= 23))
                            {
                                WorldGen.PlaceTile(i + a, j + b, mod.TileType("CaeliteBar"));
                                if (21 + gap != a)
                                {
                                    WorldGen.PlaceTile(i + a, j + b - 1, mod.TileType("CaeliteBar"));
                                    if ((Main.rand.Next(1) == 0 || secondPlace) && hasntPlaced)
                                    {
                                        WorldGen.PlaceTile(i + a, j + b - 2, mod.TileType("CaeliteBar"));
                                        hasntPlaced = false;
                                    }
                                    else
                                    {
                                        secondPlace = true;
                                    }
                                }
                            }
                            if ((a >= 1 && a <= 3))
                            {
                                WorldGen.PlaceTile(i + a, j + b, mod.TileType("CaeliteBar"));
                                if (1 + gap2 != a)
                                {
                                    WorldGen.PlaceTile(i + a, j + b - 1, mod.TileType("CaeliteBar"));
                                    if ((Main.rand.Next(1) == 0 || secondPlace2) && hasntPlaced2)
                                    {
                                        WorldGen.PlaceTile(i + a, j + b - 2, mod.TileType("CaeliteBar"));
                                        hasntPlaced2 = false;
                                    }
                                    else
                                    {
                                        secondPlace2 = true;
                                    }
                                }
                            }
                        }
                    }
                    /*
                    if (b == 6)
                    {

                        for (int a = 0; a < 25; a++)
                        {
                            if ((a >= 1 && a <= 3 && 1+gap != a) )
                            {

                                WorldGen.PlaceTile(i + a, j + b, mod.TileType("CaeliteBar"));

                                if((Main.rand.Next(1) ==0|| secondPlace) && hasntPlaced)
                                {
                                    WorldGen.PlaceTile(i + a, j + b-1, mod.TileType("CaeliteBar"));
                                    hasntPlaced = false;
                                }
                                else
                                {
                                    secondPlace = true;
                                }

                            }
                            if((a >= 21 && a <= 23 && 21 + gap != a))
                            {
                                WorldGen.PlaceTile(i + a, j + b, mod.TileType("CaeliteBar"));

                                if ((Main.rand.Next(1) == 0 || secondPlace2) && hasntPlaced2)
                                {
                                    WorldGen.PlaceTile(i + a, j + b - 1, mod.TileType("CaeliteBar"));
                                    hasntPlaced2 = false;
                                }
                                else
                                {
                                    secondPlace2 = true;
                                }

                            }
                        }

                    }
                    */
                }
                #endregion

            }
        }
        public int startingX;
        float upperLimit = 60;
        float lowerLimit;
        int Center;
        int maxDistanceFromCenter;
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Main.NewText("The sky has been Fortified!");

            if (Main.dungeonX < Main.maxTilesX * .5f)
            {
                Center = (int)((double)Main.maxTilesX * 0.8);
            }
            else
            {
                Center = (int)((double)Main.maxTilesX * 0.2);
            }
            if (Main.maxTilesX > 8000)
            {
                lowerLimit = 320 ;
                maxDistanceFromCenter = 750;
            }
            else if (Main.maxTilesX > 6000)
            {
                lowerLimit = 230 ;
                maxDistanceFromCenter = 550;
            }
            else
            {
                lowerLimit = 130 ;
                maxDistanceFromCenter = 320 ;
            }
            QwertyMethods.BreakTiles(Center- maxDistanceFromCenter, 0, maxDistanceFromCenter*2, (int)(lowerLimit));
            //Main.NewText(maxDistanceFromCenter);
            
            //Main.NewText(Main.maxTilesX);
            if (Main.dungeonX < Main.maxTilesX * .5f)
            {
                startingX = (int)((double)Main.maxTilesX * 0.8);
            }
            else
            {
                startingX = (int)((double)Main.maxTilesX * 0.2);
            }
            //Main.NewText(startingX);
            int roomsPerLayer = 7;

            if (Main.maxTilesX > 8000)
            {
                roomsPerLayer += 4;

            }
            if (Main.maxTilesX > 6000)
            {
                roomsPerLayer += 4;
            }
            else
            {

            }
            int roomHeight = 25;
            int roomWidth = 25;
            int bridgeHeight = 25;
            int bridgeWidth = 75;
            int verticalHeight = 21;
            int verticalWith = 25;
            int HeightOffset = (roomHeight - 1) / 2;
            int WidthOffset = (roomWidth - 1) / 2;
            int startingY = 110;
            if (Main.maxTilesX > 8000)
            {
                startingY =195;
            }
            else if(Main.maxTilesX > 6000)
            {
                startingY = 145;
            }
            else
            {
                startingY = 110;

            }
                if (Main.maxTilesX > 8000)
            {
                startingY -= 25;
            }
            startingX -= WidthOffset + (((roomsPerLayer - 1) / 2) * (roomWidth + bridgeWidth));
            startingY -= HeightOffset;

            

            int treasure = Main.rand.Next(roomsPerLayer - 1);
            if (treasure >= (roomsPerLayer - 1) / 2)
            {
                treasure++;
            }
            for (int n = 0; n < roomsPerLayer; n++)
            {
                if (n == (roomsPerLayer - 1) / 2)
                {
                    //Main.NewText(n + ", " + startingX + (n * (roomWidth + bridgeWidth)) + ", " + startingY);
                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, -1);
                }
                else
                {
                    if (n == treasure)
                    {
                        //Main.NewText(n + ", " + startingX + (n * (roomWidth + bridgeWidth)) + ", " + startingY);
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, -2);
                    }
                    else
                    {
                        //Main.NewText(n + ", " + startingX + (n * (roomWidth + bridgeWidth)) + ", " + startingY);
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY, 1);
                    }

                }

                if (n != roomsPerLayer - 1)
                {
                    generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY, 0);
                }


            }
            treasure = Main.rand.Next(roomsPerLayer);
            for (int n = 0; n < roomsPerLayer; n++)
            {
                if (n == treasure)
                {
                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), -2);
                }
                else
                {
                    generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), 1);
                }
                Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight) + roomHeight, 1);
                if (n != roomsPerLayer - 1)
                {
                    generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY - 1 * (roomHeight + verticalHeight), 0);
                }


            }
            if (Main.maxTilesX > 6000)
            {
                treasure = Main.rand.Next(roomsPerLayer);
                for (int n = 0; n < roomsPerLayer; n++)
                {
                    if (n == treasure)
                    {
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), -2);
                    }
                    else
                    {
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), 1);
                    }
                    Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight) - verticalHeight, 1);
                    if (n != roomsPerLayer - 1)
                    {
                        generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY + 1 * (roomHeight + verticalHeight), 0);
                    }


                }
                treasure = Main.rand.Next(roomsPerLayer);
                for (int n = 0; n < roomsPerLayer; n++)
                {
                    if (n == treasure)
                    {
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), -2);
                    }
                    else
                    {
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), 1);
                    }
                    Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight) + roomHeight, 1);
                    if (n != roomsPerLayer - 1)
                    {
                        generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY - 2 * (roomHeight + verticalHeight), 0);
                    }


                }
            }
            if (Main.maxTilesX > 8000)
            {
                treasure = Main.rand.Next(roomsPerLayer);
                for (int n = 0; n < roomsPerLayer; n++)
                {
                    if (n == treasure)
                    {
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), -2);
                    }
                    else
                    {
                        generateRoom(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), 1);
                    }
                    Vertical(startingX + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight) - verticalHeight, 1);
                    if (n != roomsPerLayer - 1)
                    {
                        generateBridge(startingX + roomWidth + (n * (roomWidth + bridgeWidth)), startingY + 2 * (roomHeight + verticalHeight), 0);
                    }


                }
                
            }
            
        }
    }
    public class SkyRNG : ModCommand
    {
        private static int numIslandHouses = 0;

        private static int houseCount = 0;
        private static bool[] skyLake = new bool[30];
        private static int[] fihX = new int[30];

        private static int[] fihY = new int[30];
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "skyrng"; }
        }

        public override string Description
        {
            get { return "Stimulates sky island generation and give the coordinates"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Main.NewText("player: " + Main.player[Main.myPlayer].Center.ToPoint16());
            numIslandHouses = 0;
            houseCount = 0;
            int skyLakes = 1;
            if (Main.maxTilesX > 8000)
            {
                int skyLakes2 = skyLakes;
                skyLakes = skyLakes2 + 1;
            }
            if (Main.maxTilesX > 6000)
            {
                int skyLakes2 = skyLakes;
                skyLakes = skyLakes2 + 1;
            }
            for (int k = 0; k < (int)((double)Main.maxTilesX * 0.0008) + skyLakes; k++)
            {
                int num = 1000;
                int num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.9));
                if (WorldGen.dungeonX < Main.maxTilesX * .5f)
                {
                    num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.7));
                }
                else
                {
                    num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.3), (int)((double)Main.maxTilesX * 0.9));
                }

                while (--num > 0)
                {
                    bool flag2 = true;
                    while (num2 > Main.maxTilesX / 2 - 80 && num2 < Main.maxTilesX / 2 + 80)
                    {
                        if (WorldGen.dungeonX < Main.maxTilesX * .5f)
                        {
                            num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.7));
                        }
                        else
                        {
                            num2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.3), (int)((double)Main.maxTilesX * 0.9));
                        }
                    }
                    for (int l = 0; l < numIslandHouses; l++)
                    {
                        if (num2 > fihX[l] - 180 && num2 < fihX[l] + 180)
                        {
                            flag2 = false;
                            break;
                        }
                    }
                    if (flag2)
                    {
                        flag2 = false;
                        int num3 = 0;
                        int num4 = 200;
                        while ((double)num4 < Main.worldSurface)
                        {
                            if (Main.tile[num2, num4].active())
                            {
                                num3 = num4;
                                flag2 = true;
                                break;
                            }
                            num4++;
                        }
                        if (flag2)
                        {
                            int num5 = WorldGen.genRand.Next(90, num3 - 100);
                            num5 = Math.Min(num5, (int)WorldGen.worldSurfaceLow - 50);
                            if (k < skyLakes)
                            {
                                skyLake[numIslandHouses] = true;
                                WorldGen.CloudLake(num2, num5);
                                Main.NewText("Lake: " + new Vector2(num2, num5));
                            }
                            else
                            {
                                WorldGen.CloudIsland(num2, num5);
                                //Main.NewText(new Vector2(num2, num5));
                                Main.NewText("Island: " + new Vector2(num2, num5));
                            }
                            fihX[numIslandHouses] = num2;
                            fihY[numIslandHouses] = num5;
                            numIslandHouses++;
                        }
                    }
                }
            }

        }
    }


}
