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
    public class Fan : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            //Main.tileNoAttach[Type] = true;
            //Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.AnchorTop = default(AnchorData);
            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.newTile.AnchorLeft = default(AnchorData);
            TileObjectData.newTile.AnchorRight = default(AnchorData);
            
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<FanE>().Hook_AfterPlacement, 0, 0, true);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            //TileObjectData.newTile.StyleWrapLimit = 2; //not really necessary but allows me to add more subtypes of chairs below the example chair texture
            //TileObjectData.newTile.StyleMultiplier = 2; //same as above
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //allows me to place example chairs facing the same way as the player
            TileObjectData.addAlternate(1); //facing right will use the second texture style
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Fan");
            AddMapEntry(new Color(200, 200, 200), name);
            //dustType = mod.DustType("Sparkle");
            disableSmartCursor = true;
            //adjTiles = new int[]{ TileID.Chairs };
            animationFrameHeight = 38;
        }
        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i + 1, j].active() || Main.tile[i - 1, j].active() || Main.tile[i, j + 1].active() || Main.tile[i + 1, j - 1].active() || Main.tile[i - 1, j - 1].active() || Main.tile[i, j - 2].active();
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        //public bool blowing;


        //public int counter;
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            /*
            int left = i;
            int top = j - (Main.tile[i, j].frameY / 18) % 2;

            int index = mod.GetTileEntity<FanE>().Find(left, top);
            if (index == -1)
            {
                return;
            }
            FanE FanE = (FanE)TileEntity.ByID[index];
            */


        }
        
        public override void HitWire(int i, int j)
        {
            int left = i;
            int top = j - (Main.tile[i, j].frameY / 18) % 2;

            int index = mod.GetTileEntity<FanE>().Find(left, top);
            if (index == -1)
            {
                return;
            }
            FanE fanE = (FanE)TileEntity.ByID[index];
            fanE.switchBlow = true;
        }
        
        public override void RightClick(int i, int j)
        {
            int left = i;
            int top = j - (Main.tile[i, j].frameY / 18) % 2;

            int index = mod.GetTileEntity<FanE>().Find(left, top);
            if (index == -1)
            {
                return;
            }
            FanE fanE = (FanE)TileEntity.ByID[index];
            fanE.switchBlow = true;
        }
        
        /*
        public override void RightClick(int i, int j)
        {
            Player player = player;
            
            for (int b = 0; b < 58; b++) // this searches every invintory slot
            {
                if (player.inventory[b].type == mod.ItemType("Abstractive") && player.inventory[b].stack > 0) //this checks if the slot has the valid item
                {
                    Main.PlaySound(SoundID.Roar, player.position, 0);
                    NPC.NewNPC(i*16, j*16, NPCID.EyeofCthulhu);
                    player.inventory[b].stack--;
                    break;
                    


                }
            }
        }
        */
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType("Fan");
        }


        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("Fan"));
            mod.GetTileEntity<FanE>().Kill(i, j);
        }
    }
    public class FanE : ModTileEntity
    {

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active();
        }
        public int frameTimer;
        public bool blowing;
        public bool switchBlow;
        public override void Update()
        {
            
            if (switchBlow)
            {
                blowing = !blowing;
            }
            if (blowing)
            {
                frameTimer++;
                if (frameTimer >= 6)
                {
                    frameTimer = 0;
                }
                else if (frameTimer >= 3)
                {
                    Main.tile[Position.X, Position.Y].frameY = 76;
                    Main.tile[Position.X, Position.Y + 1].frameY = (short)(76 + 18);
                }
                else
                {
                    Main.tile[Position.X, Position.Y].frameY = 38;
                    Main.tile[Position.X, Position.Y + 1].frameY = (short)(38 + 18);
                }
            }
            else
            {
                Main.tile[Position.X, Position.Y].frameY = 0;
                Main.tile[Position.X, Position.Y + 1].frameY = (short)(0 + 18);
            }
            if (Main.tile[Position.X, Position.Y].frameY != 0)
            {
                float fanSpeed = .2f;
                Vector2 blowArea = new Vector2(20, 2) * 16;
                int edge = Position.X + 1;
                Vector2 blowOrigin = new Vector2(edge, Position.Y) * 16;
                int subtractFrom = 320;
                if (Main.tile[Position.X, Position.Y].frameX == 0)
                {
                    subtractFrom = 0;
                    fanSpeed = -.2f;
                    blowArea = new Vector2(20, 2) * 16;
                    //edge = Position.X;
                    //blowOrigin = new Vector2(edge, Position.Y) * 16;
                    //Dust.NewDust(blowOrigin, 0, (int)blowArea.Y, mod.DustType("StormArrowDust"), fanSpeed, 0);
                    edge = Position.X - 20;
                    blowOrigin = new Vector2(edge, Position.Y) * 16;

                    //Main.NewText("I'm a big fan of the left!");
                }
                else
                {
                    subtractFrom = 320;
                    fanSpeed = .2f;
                    blowArea = new Vector2(20, 2) * 16;
                    //edge = Position.X + 1;
                    //blowOrigin = new Vector2(edge, Position.Y) * 16;
                    //Dust.NewDust(blowOrigin, 0, (int)blowArea.Y, mod.DustType("StormArrowDust"), fanSpeed, 0);
                    edge = Position.X + 1;
                    blowOrigin = new Vector2(edge, Position.Y) * 16;
                    // Main.NewText("I'm a big fan of the right!");
                    
                }


                //Main.NewText("I'm a big fan!");

                for (int i = 0; i < 200; i++)
                {
                    NPC blownObj = Main.npc[i];
                    if (blownObj.active && !blownObj.boss && !blownObj.immortal && Collision.CheckAABBvAABBCollision(blownObj.position, new Vector2(blownObj.width, blownObj.height), blowOrigin, blowArea))
                    {
                        blownObj.velocity.X += fanSpeed;
                        //blownObj.velocity.X += Math.Abs(subtractFrom - Math.Abs((blownObj.Center.X - (edge) *16))) * fanSpeed;
                        //Main.NewText((320 - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed);
                    }
                }
                for (int i = 0; i < 200; i++)
                {
                    Projectile blownObj = Main.projectile[i];
                    if (blownObj.active && Collision.CheckAABBvAABBCollision(blownObj.position, new Vector2(blownObj.width, blownObj.height), blowOrigin, blowArea))
                    {
                        blownObj.velocity.X += fanSpeed;
                        //blownObj.velocity.X += Math.Abs(subtractFrom - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed;
                        //Main.NewText((320 - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed);
                    }
                }

                for (int i = 0; i < 200; i++)
                {
                    Player blownObj = Main.player[i];
                    if (blownObj.active && Collision.CheckAABBvAABBCollision(blownObj.position, new Vector2(blownObj.width, blownObj.height), blowOrigin, blowArea))
                    {
                        blownObj.velocity.X += fanSpeed;
                        //blownObj.velocity.X += Math.Abs(subtractFrom - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed;
                        //Main.NewText((320 - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed);
                    }
                }
                for (int i = 0; i < 200; i++)
                {
                    Dust blownObj = Main.dust[i];
                    if (blownObj.active && Collision.CheckAABBvAABBCollision(blownObj.position, new Vector2(1,1), blowOrigin, blowArea))
                    {
                        blownObj.velocity.X += fanSpeed;
                        //blownObj.velocity.X += Math.Abs(subtractFrom - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed;
                        //Main.NewText((320 - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed);
                    }
                }
                for (int i = 0; i < 200; i++)
                {
                    Item blownObj = Main.item[i];
                    if (blownObj.active && Collision.CheckAABBvAABBCollision(blownObj.position, new Vector2(blownObj.width, blownObj.height), blowOrigin, blowArea))
                    {
                        blownObj.velocity.X += fanSpeed;
                        //blownObj.velocity.X += Math.Abs(subtractFrom - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed;
                        //Main.NewText((320 - Math.Abs((blownObj.Center.X - (edge) * 16))) * fanSpeed);
                    }
                }
                //Dust.NewDust(blowOrigin, 0, 0, 0);

                Dust dust = Main.dust[Dust.NewDust(blowOrigin, (int)blowArea.X, (int)blowArea.Y, mod.DustType("StormArrowDust"), fanSpeed*3, .0001f, 0, default(Color), .2f)];
                dust.velocity.Y = 0;
            }
            // Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            switchBlow = false;
        }
        
        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            writer.Write(blowing);
        }
        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            blowing = reader.ReadBoolean();
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