using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Enums;
using System.IO;

namespace QwertysRandomContent.Tiles
{
    public class DnasTransmutator : ModTile
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
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<DnasTransmutatorE>().Hook_AfterPlacement, 0, 0, true);
            TileObjectData.newTile.Height = 1;
            TileObjectData.addTile(Type);



            dustType = mod.DustType("FortressDust");
            drop = mod.ItemType("DnasTransmutator");
        }
        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i + 1, j].active() || Main.tile[i - 1, j].active() || Main.tile[i, j + 1].active() || Main.tile[i, j - 1].active(); ;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            //Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("Fan"));
            mod.GetTileEntity<DnasTransmutatorE>().Kill(i, j);
        }

        
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            if (Main.tile[i + 1, j].type == mod.TileType("DnasTransmutator"))
            {
                if(Main.tile[i - 1, j].type == mod.TileType("DnasTransmutator"))
                {
                    Main.tile[i, j].frameX = 36;
                }
                else
                {
                    Main.tile[i, j].frameX = 18;
                }
            }
            else if (Main.tile[i - 1, j].type == mod.TileType("DnasTransmutator"))
            {
                Main.tile[i, j].frameX = 54;
            }
            else
            {
                Main.tile[i, j].frameX = 0;
                //solo

            }
        }


    }
    public class DnasTransmutatorE : ModTileEntity
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
            if (Main.dayTime)
            {
                int k = 1;
                while(Main.tile[i + k, j].type == mod.TileType("DnasTransmutator"))
                {
                    k++;
                }
                if(Main.tile[i, j - 1].type == TileID.Sand && Main.tile[i,j-1].active() && !Main.tile[i+ k, j].active())
                {
                    if (Main.rand.Next(180) == 0)
                    {
                        WorldGen.KillTile(i, j - 1, noItem: true);
                        Projectile.NewProjectile(new Vector2(i+ k, j) *16+ new Vector2(8,8), Vector2.Zero, mod.ProjectileType("ReverseSandBall"), 50, 0f, Main.myPlayer);
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        Dust.NewDustPerfect(new Vector2(i, j - 1) * 16 + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), 32, Vector2.UnitY *-1);
                    }
;                }

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