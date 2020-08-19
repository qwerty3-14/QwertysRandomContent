using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace QwertysRandomContent
{
    public class FrozenDen : ModWorld
    {
        public int denLength = 101;
        public int denLowerHeight = 7;
        public int denUpperHeight = 40;
        public static Vector2 BearSpawn = new Vector2(-1, -1);
        public static bool activeSleeper = false;
        public static FrozenDen instance;

        public override void Initialize()
        {
            BearSpawn = new Vector2(-1, -1);
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"BearSpawnX", BearSpawn.X},
                {"BearSpawnY", BearSpawn.Y},
                {"activeSleeper", activeSleeper }
            };
        }

        public override void Load(TagCompound tag)
        {
            BearSpawn.X = tag.GetFloat("BearSpawnX");
            BearSpawn.Y = tag.GetFloat("BearSpawnY");
            activeSleeper = tag.GetBool("activeSleeper");
            instance = this;
        }

        public void GenerateDen(int x, int y)
        {
            //QwertyMethods.BreakTiles(x - (denLength-1)/2, y, denLength, denLowerHeight);

            for (int l = 0; l < denLength; l++)
            {
                for (int h = 0; h < denUpperHeight; h++)
                {
                    if (!Main.tile[(x - ((denLength - 1) / 2)) + l, y - h].active())
                    {
                        WorldGen.PlaceTile((x - ((denLength - 1) / 2)) + l, y - h, TileID.IceBlock);
                    }
                    Main.tile[x - ((denLength - 1) / 2) + l, y - h].liquid = 0;
                }
                int ceilingHeight = (int)((float)Math.Sin(((float)l / (float)denLength) * (float)Math.PI) * (float)denUpperHeight);
                for (int h = 0; h < ceilingHeight; h++)
                {
                    WorldGen.KillTile(x - ((denLength - 1) / 2) + l, y - h, false, false, true);
                    WorldGen.KillWall(x - ((denLength - 1) / 2) + l, y - h, false);

                    WorldGen.PlaceWall(x - ((denLength - 1) / 2) + l, y - h, WallID.IceUnsafe);
                    if (l % 4 == 0 && h == ceilingHeight - 5)
                    {
                        WorldGen.PlaceTile(x - ((denLength - 1) / 2) + l, y - h, TileID.Torches, false, false, -1, 9);
                    }
                    if (Math.Abs((x - ((denLength - 1) / 2) + l) - x) >= 10 && Math.Abs((x - ((denLength - 1) / 2) + l) - x) <= 19 && h == 15)
                    {
                        WorldGen.PlaceTile(x - ((denLength - 1) / 2) + l, y - h, TileID.Platforms, style: 35);
                    }
                }
                for (int h = 0; h < denLowerHeight; h++)
                {
                    WorldGen.PlaceTile(x - ((denLength - 1) / 2) + l, y + h, TileID.IceBlock);
                }
            }
            for (int l = 0; l < denLength; l++)
            {
                int ceilingHeight = (int)((float)Math.Sin(((float)l / (float)denLength) * (float)Math.PI) * (float)denUpperHeight);
                for (int h = 0; h < ceilingHeight; h++)
                {
                    if (l == 35 && h == 16)
                    {
                        Chest chest = Main.chest[WorldGen.PlaceChest(x - ((denLength - 1) / 2) + l, y - h, style: 11)];
                        int slot = 0;

                        chest.item[slot].SetDefaults(ItemID.SnowballCannon, false);
                        slot++;
                        chest.item[slot].SetDefaults(ItemID.Snowball, false);
                        chest.item[slot].stack = Main.rand.Next(500, 1000);
                        slot++;

                        chest.item[slot].SetDefaults(ItemID.IceTorch, false);
                        chest.item[slot].stack = Main.rand.Next(20, 100);
                        slot++;
                        chest.item[slot].SetDefaults(ItemID.LesserHealingPotion, false);
                        chest.item[slot].stack = Main.rand.Next(4, 11);
                        slot++;
                        if (Main.rand.Next(5) == 0)
                        {
                            chest.item[slot].SetDefaults(ItemID.IceMirror, false);
                            slot++;
                        }
                        slot++;
                    }
                    if (l == 64 && h == 16)
                    {
                        Chest chest = Main.chest[WorldGen.PlaceChest(x - ((denLength - 1) / 2) + l, y - h, style: 11)];

                        int slot = 0;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                chest.item[slot].SetDefaults(ItemID.IceSkates, false);
                                slot++;
                                break;

                            case 1:
                                chest.item[slot].SetDefaults(ItemID.FlurryBoots, false);
                                slot++;
                                break;

                            case 2:
                                chest.item[slot].SetDefaults(ItemID.BlizzardinaBottle, false);
                                slot++;
                                break;
                        }
                        chest.item[slot].SetDefaults(ItemID.IceTorch, false);
                        chest.item[slot].stack = Main.rand.Next(20, 100);
                        slot++;
                        chest.item[slot].SetDefaults(ItemID.LesserHealingPotion, false);
                        chest.item[slot].stack = Main.rand.Next(4, 11);
                        slot++;
                        if (Main.rand.Next(5) == 0)
                        {
                            chest.item[slot].SetDefaults(ItemID.IceMirror, false);
                            slot++;
                        }
                    }
                }
            }
            //int[] rocks = new int[] { 97, 92, 81, 60 };
            /*
            for (int i = 0; i < 10; i++)
            {
                if (Main.rand.Next(2) == 0)
                {
                    WorldGen.PlaceSmallPile(x - ((denLength - 1) / 2) + Main.rand.Next(denLength), y - 1, Main.rand.Next(42, 48), 0);
                }
                else
                {
                    WorldGen.PlaceSmallPile(x - ((denLength - 1) / 2) + Main.rand.Next(denLength), y - 1, Main.rand.Next(25, 31), 1);
                }
            }*/
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Tile Cleanup"));

            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Hiding true compasses!", delegate (GenerationProgress progress)
                {
                    for (int c = 0; c < Main.chest.Length; c++)
                    {
                        if (Main.chest[c] != null)
                        {
                            if ((Main.chest[c].item[0].type == ItemID.IceBlade || Main.chest[c].item[0].type == ItemID.SnowballCannon || Main.chest[c].item[0].type == ItemID.IceBoomerang || Main.chest[c].item[0].type == ItemID.IceSkates || Main.chest[c].item[0].type == ItemID.FlurryBoots || Main.chest[c].item[0].type == ItemID.BlizzardinaBottle) && Main.rand.Next(4) == 0)
                            {
                                for (int i = 0; i < Main.chest[c].item.Length; i++)
                                {
                                    if (Main.chest[c].item[i].type == 0)
                                    {
                                        Main.chest[c].item[i].SetDefaults(mod.ItemType("FrostCompass"), false);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }));
            }
        }

        public override void PostWorldGen()
        {
            for (int i = 0; i < 1000; i++)
            {
                int x = WorldGen.genRand.Next(Main.maxTilesX);
                int y = WorldGen.genRand.Next(Main.maxTilesY);
                if ((Main.tile[x, y].type == TileID.SnowBlock || Main.tile[x, y].type == TileID.IceBlock) && y > WorldGen.rockLayer)
                {
                    BearSpawn = new Vector2(x * 16, (y - 2) * 16);
                    activeSleeper = true;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                    GenerateDen(x, y);
                    break;
                }
            }
        }

        public override void PreUpdate()
        {
            if (!NPC.AnyNPCs(mod.NPCType("PolarBear")) && !NPC.AnyNPCs(mod.NPCType("Sleeping")) && Main.dayTime && Main.time == 1 && BearSpawn.X != -1 && BearSpawn.Y != -1)
            {
                activeSleeper = true;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                NPC.NewNPC((int)BearSpawn.X, (int)BearSpawn.Y, mod.NPCType("Sleeping"));
            }
            else if (activeSleeper && !NPC.AnyNPCs(mod.NPCType("PolarBear")) && !NPC.AnyNPCs(mod.NPCType("Sleeping")) && BearSpawn.X != -1 && BearSpawn.Y != -1)
            {
                activeSleeper = true;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                NPC.NewNPC((int)BearSpawn.X, (int)BearSpawn.Y, mod.NPCType("Sleeping"));
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WritePackedVector2(BearSpawn);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BearSpawn = reader.ReadPackedVector2();
        }
    }

    internal class CreateDen : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "createDen"; }
        }

        public override string Description
        {
            get { return "Create's a den in the underground tundra and places the polar exterminator in it"; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            for (int i = 0; i < 1000; i++)
            {
                int x = WorldGen.genRand.Next(Main.maxTilesX);
                int y = WorldGen.genRand.Next(Main.maxTilesY);
                if ((Main.tile[x, y].type == TileID.SnowBlock || Main.tile[x, y].type == TileID.IceBlock) && y > WorldGen.rockLayer)
                {
                    FrozenDen.BearSpawn = new Vector2(x * 16, (y - 2) * 16);
                    FrozenDen.activeSleeper = true;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                    FrozenDen.instance.GenerateDen(x, y);
                    break;
                }
            }
        }
    }
}