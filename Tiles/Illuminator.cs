using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles
{
    public class Illuminator : ModTile
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
            //TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity("IlluminatorE").Hook_AfterPlacement, 0, 0, true);
            TileObjectData.newTile.Height = 1;
            TileObjectData.addTile(Type);

            dustType = 1;
            drop = mod.ItemType("Illuminator");
        }

        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i + 1, j].active() || Main.tile[i - 1, j].active() || Main.tile[i, j + 1].active() || Main.tile[i, j - 1].active(); ;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            //Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("Fan"));
            mod.GetTileEntity("IlluminatorE").Kill(i, j);
        }

        public override void HitWire(int i, int j)
        {
            Main.tile[i, j].frameX += 18;
            if (Main.tile[i, j].frameX > 18)
            {
                Main.tile[i, j].frameX = 0;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            if (Main.tile[i, j + 1].type == mod.TileType("Illuminator"))
            {
                if (Main.tile[i, j - 1].type == mod.TileType("Illuminator"))
                {
                    Main.tile[i, j].frameY = 36;
                    Main.tile[i, j].frameX = 0;
                    //middle
                }
                else
                {
                    Main.tile[i, j].frameY = 18;
                    //top
                    if (Main.tile[i, j].frameX == 0)
                    {
                        /*
                        for (int g = 0; Main.tile[i, j + g].type == mod.TileType("Illuminator"); g++)
                        {
                            Lighting.AddLight(new Vector2(i, j - 1 - (g * 4)) * 16, 1.0f, 1.0f, 1.0f);
                            Lighting.AddLight(new Vector2(i, j - 2 - (g * 4)) * 16, 1.0f, 1.0f, 1.0f);
                            Lighting.AddLight(new Vector2(i, j - 3 - (g * 4)) * 16, 1.0f, 1.0f, 1.0f);
                            Lighting.AddLight(new Vector2(i, j - 4 - (g * 4)) * 16, 1.0f, 1.0f, 1.0f);
                        }
                        */
                    }
                }
            }
            else if (Main.tile[i, j - 1].type == mod.TileType("Illuminator"))
            {
                Main.tile[i, j].frameY = 54;
                Main.tile[i, j].frameX = 0;
                //bottom
            }
            else
            {
                Main.tile[i, j].frameY = 0;
                //solo
                /*
                if (Main.tile[i, j].frameX == 0)
                {
                    Lighting.AddLight(new Vector2(i, j - 1) * 16, 1.0f, 1.0f, 1.0f);
                    Lighting.AddLight(new Vector2(i, j - 2) * 16, 1.0f, 1.0f, 1.0f);
                    Lighting.AddLight(new Vector2(i, j - 3) * 16, 1.0f, 1.0f, 1.0f);
                    Lighting.AddLight(new Vector2(i, j - 4) * 16, 1.0f, 1.0f, 1.0f);
                }
                */
            }
        }
    }

    public class IlluminatorE : ModTileEntity
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
            if (Main.tile[i, j + 1].type == mod.TileType("Illuminator"))
            {
                if (Main.tile[i, j - 1].type == mod.TileType("Illuminator"))
                {
                    //Main.tile[i, j].frameY = 36;
                    //Main.tile[i, j].frameX = 0;
                    //middle
                }
                else
                {
                    //Main.tile[i, j].frameY = 18;
                    //top
                    if (Main.tile[i, j].frameX == 0)
                    {
                        for (int g = 0; Main.tile[i, j + g].type == mod.TileType("Illuminator"); g++)
                        {
                            Lighting.AddLight(new Vector2(i, j - 1 - (g * 4)) * 16, 1.0f, 1.0f, 1.0f);
                            Lighting.AddLight(new Vector2(i, j - 2 - (g * 4)) * 16, 1.0f, 1.0f, 1.0f);
                            Lighting.AddLight(new Vector2(i, j - 3 - (g * 4)) * 16, 1.0f, 1.0f, 1.0f);
                            Lighting.AddLight(new Vector2(i, j - 4 - (g * 4)) * 16, 1.0f, 1.0f, 1.0f);
                            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
                            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
                        }
                    }
                }
            }
            else if (Main.tile[i, j - 1].type == mod.TileType("Illuminator"))
            {
                //Main.tile[i, j].frameY = 54;
                //Main.tile[i, j].frameX = 0;
                //bottom
            }
            else
            {
                //Main.tile[i, j].frameY = 0;
                //solo
                if (Main.tile[i, j].frameX == 0)
                {
                    Lighting.AddLight(new Vector2(i, j - 1) * 16, 1.0f, 1.0f, 1.0f);
                    Lighting.AddLight(new Vector2(i, j - 2) * 16, 1.0f, 1.0f, 1.0f);
                    Lighting.AddLight(new Vector2(i, j - 3) * 16, 1.0f, 1.0f, 1.0f);
                    Lighting.AddLight(new Vector2(i, j - 4) * 16, 1.0f, 1.0f, 1.0f);
                    // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
                }
            }
        }

        /*
        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            int i = Position.X;
            int j = Position.Y;
            if (Main.tile[i, j + 1].type == mod.TileType("Illuminator"))
            {
                if (Main.tile[i, j - 1].type == mod.TileType("Illuminator"))
                {
                    //Main.tile[i, j].frameY = 36;
                    //Main.tile[i, j].frameX = 0;
                    //middle
                }
                else
                {
                    //Main.tile[i, j].frameY = 18;
                    //top
                    if (Main.tile[i, j].frameX == 0)
                    {
                        for (int g = 0; Main.tile[i, j + g].type == mod.TileType("Illuminator"); g++)
                        {
                            writer.Write(g);
                            writer.WriteVector2(new Vector2(i, j - 1 - (g * 4))*16);
                            writer.WriteVector2(new Vector2(i, j - 2 - (g * 4)) * 16);
                            writer.WriteVector2(new Vector2(i, j - 3 - (g * 4)) * 16);
                            writer.WriteVector2(new Vector2(i, j - 4 - (g * 4)) * 16);
                        }
                    }
                }
            }
            else if (Main.tile[i, j - 1].type == mod.TileType("Illuminator"))
            {
                //Main.tile[i, j].frameY = 54;
                //Main.tile[i, j].frameX = 0;
                //bottom
            }
            else
            {
                //Main.tile[i, j].frameY = 0;
                //solo
                if (Main.tile[i, j].frameX == 0)
                {
                    writer.WriteVector2(new Vector2(i, j - 1) * 16);
                    writer.WriteVector2(new Vector2(i, j - 2) * 16);
                    writer.WriteVector2(new Vector2(i, j - 3) * 16);
                    writer.WriteVector2(new Vector2(i, j - 4 ) * 16);
                }
            }
        }
        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            int i = Position.X;
            int j = Position.Y;
            if (Main.tile[i, j + 1].type == mod.TileType("Illuminator"))
            {
                if (Main.tile[i, j - 1].type == mod.TileType("Illuminator"))
                {
                    //Main.tile[i, j].frameY = 36;
                    //Main.tile[i, j].frameX = 0;
                    //middle
                }
                else
                {
                    //Main.tile[i, j].frameY = 18;
                    //top
                    if (Main.tile[i, j].frameX == 0)
                    {
                        int count =reader.ReadInt32();
                        for (int g = 1; g < count+1; g++)
                        {
                            Lighting.AddLight(reader.ReadVector2(), 1f, 1f, 1f);
                            Lighting.AddLight(reader.ReadVector2(), 1f, 1f, 1f);
                            Lighting.AddLight(reader.ReadVector2(), 1f, 1f, 1f);
                            Lighting.AddLight(reader.ReadVector2(), 1f, 1f, 1f);
                        }
                    }
                }
            }
            else if (Main.tile[i, j - 1].type == mod.TileType("Illuminator"))
            {
                //Main.tile[i, j].frameY = 54;
                //Main.tile[i, j].frameX = 0;
                //bottom
            }
            else
            {
                //Main.tile[i, j].frameY = 0;
                //solo
                if (Main.tile[i, j].frameX == 0)
                {
                    Lighting.AddLight(reader.ReadVector2(), 1f, 1f, 1f);
                    Lighting.AddLight(reader.ReadVector2(), 1f, 1f, 1f);
                    Lighting.AddLight(reader.ReadVector2(), 1f, 1f, 1f);
                    Lighting.AddLight(reader.ReadVector2(), 1f, 1f, 1f);
                }
            }
        }
*/

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