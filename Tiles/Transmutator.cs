using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles
{
    public class Transmutator : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorTop = default(AnchorData);
            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.newTile.AnchorLeft = default(AnchorData);
            TileObjectData.newTile.AnchorRight = default(AnchorData);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 0, 0);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity("TransmutatorE").Hook_AfterPlacement, 0, 0, true);
            TileObjectData.newTile.Height = 1;
            TileObjectData.addTile(Type);



            dustType = 1;
            drop = mod.ItemType("Transmutator");
        }
        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i + 1, j].active() || Main.tile[i - 1, j].active() || Main.tile[i, j + 1].active() || Main.tile[i, j - 1].active(); ;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            //Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("Fan"));
            mod.GetTileEntity("TransmutatorE").Kill(i, j);
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {

        }



    }
    public class TransmutatorE : ModTileEntity
    {

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active();
        }
        public override void Update()
        {
            int i = Position.X;
            int j = Position.Y;
            if (!Main.dayTime)
            {
                for (int g = 1; g < 6; g++)
                {
                    if (Main.tile[i, j - g].active())
                    {
                        if (Main.tile[i, j - g].type == TileID.Crimtane || Main.tile[i, j - g].type == TileID.Demonite)
                        {
                            if (Main.rand.Next((int)Main.nightLength / 2) == 0)
                            {
                                //Main.tile[i, j - g].type = (ushort)mod.TileType("LuneOre");
                                WorldGen.PlaceTile(i, j - g, (ushort)mod.TileType("LuneOre"), false, true);
                                for (int d = 0; d < 40; d++)
                                {
                                    Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                                }
                            }
                            if (Main.rand.Next(60) == 0)
                            {
                                Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                            }
                            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y - g);
                        }

                        if (Main.tile[i, j - g].type == TileID.Dirt)
                        {
                            if (Main.rand.Next((int)Main.nightLength / 20) == 0)
                            {
                                //Main.tile[i, j - g].type = TileID.ClayBlock;
                                WorldGen.PlaceTile(i, j - g, TileID.ClayBlock, false, true);
                                for (int d = 0; d < 40; d++)
                                {
                                    Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                                }
                            }
                            if (Main.rand.Next(60) == 0)
                            {
                                Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                            }
                            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y - g);
                        }

                        if (Main.tile[i, j - g].type == TileID.HardenedSand)
                        {
                            if (Main.rand.Next((int)Main.nightLength / 4) == 0)
                            {
                                WorldGen.PlaceTile(i, j - g, TileID.DesertFossil, false, true);
                                //Main.tile[i, j - g].type = TileID.DesertFossil;
                                for (int d = 0; d < 40; d++)
                                {
                                    Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                                }
                            }
                            if (Main.rand.Next(60) == 0)
                            {
                                Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                            }
                            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y - g);
                        }
                        if (Main.tile[i, j - g].type == TileID.HoneyBlock)
                        {
                            if (Main.rand.Next((int)Main.nightLength / 4) == 0)
                            {
                                WorldGen.PlaceTile(i, j - g, TileID.Hive, false, true);
                                //Main.tile[i, j - g].type = TileID.Hive;
                                for (int d = 0; d < 40; d++)
                                {
                                    Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                                }
                            }
                            if (Main.rand.Next(60) == 0)
                            {
                                Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                            }
                            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y - g);
                        }
                        if (Main.tile[i, j - g].type == TileID.Copper || Main.tile[i, j - g].type == TileID.Tin)
                        {
                            if (Main.rand.Next((int)Main.nightLength / 4) == 0)
                            {
                                if (Main.rand.Next(2) == 0)
                                {
                                    WorldGen.PlaceTile(i, j - g, TileID.Iron, false, true);
                                    //Main.tile[i, j - g].type = TileID.Iron;
                                }
                                else
                                {
                                    WorldGen.PlaceTile(i, j - g, TileID.Lead, false, true);
                                    // Main.tile[i, j - g].type = TileID.Lead;
                                }
                                for (int d = 0; d < 40; d++)
                                {
                                    Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                                }
                            }
                            if (Main.rand.Next(60) == 0)
                            {
                                Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                            }
                            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y - g);
                        }
                        if (Main.tile[i, j - g].type == TileID.Silver || Main.tile[i, j - g].type == TileID.Tungsten)
                        {
                            if (Main.rand.Next((int)Main.nightLength / 2) == 0)
                            {
                                if (Main.rand.Next(2) == 0)
                                {
                                    WorldGen.PlaceTile(i, j - g, TileID.Gold, false, true);
                                    // Main.tile[i, j - g].type = TileID.Gold;
                                }
                                else
                                {
                                    WorldGen.PlaceTile(i, j - g, TileID.Platinum, false, true);
                                    //Main.tile[i, j - g].type = TileID.Platinum;
                                }
                                for (int d = 0; d < 40; d++)
                                {
                                    Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                                }
                            }
                            if (Main.rand.Next(60) == 0)
                            {
                                Dust.NewDust(new Vector2(i, j - g) * 16, 16, 16, mod.DustType("LuneDust"));
                            }
                            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y - g);
                        }

                    }

                }

            }
            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);

        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            for (int g = 1; g < 6; g++)
            {
                //NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y - g);
                writer.Write(Main.tile[Position.X, Position.Y - g].type);

            }

        }
        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            for (int g = 1; g < 6; g++)
            {
                //NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y - g);
                //writer.Write(Main.tile[Position.X, Position.Y - g].type);
                Main.tile[Position.X, Position.Y - g].type = reader.ReadUInt16();
            }
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            //Main.NewText("I'm a big fan!");
            //Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            return Place(i, j);
        }
    }
}