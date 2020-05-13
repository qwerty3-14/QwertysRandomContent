using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles
{
	public class DnasLantern : ModTile
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			if (ModContent.GetInstance<SpriteSettings>().ClassicFortress)
			{
				texture += "_Classic";
			}
			return base.Autoload(ref name, ref texture);
		}

		public override void SetDefaults()
		{
			//Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			//Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity("DnasLanternE").Hook_AfterPlacement, 0, 0, true);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.addTile(Type);
			//AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Dnas Lantern");
			AddMapEntry(new Color(162, 184, 185), name);
			dustType = mod.DustType("FortressDust");
			//disableSmartCursor = true;
			//adjTiles = new int[]{ TileID.Sinks };
		}

		public override void RightClick(int i, int j)
		{
			Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
		}

		public override void HitWire(int i, int j)
		{
			int left = i - (Main.tile[i, j].frameX / 18) % 2;
			int top = j - (Main.tile[i, j].frameY / 18) % 2;
			/*
            for(int x= left; x< left + 2; x++)
            {
                for (int y = top; y < top + 2; y++)
                {
                    if(Main.tile[x, y].frameX>= 36)
                    {
                        Main.tile[x, y].frameX -= 36;
                    }
                    else
                    {
                        Main.tile[x, y].frameX += 36;
                    }
                }
            }
            if (Wiring.running)
            {
                Wiring.SkipWire(left, top);
                Wiring.SkipWire(left, top + 1);
                Wiring.SkipWire(left + 1, top);
                Wiring.SkipWire(left + 1, top + 1);
            }
            NetMessage.SendTileSquare(-1, left, top + 1, 2);
            */
			int index = mod.GetTileEntity("DnasLanternE").Find(left, top);
			if (index == -1)
			{
				return;
			}
			DnasLanternE DnasLanternE = (DnasLanternE)TileEntity.ByID[index];
			DnasLanternE.toggle = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color = new Color(100, 100, 255, 0);
			int frameX = Main.tile[i, j].frameX;
			int frameY = Main.tile[i, j].frameY;
			int width = 20;
			int offsetY = 2;
			int height = 20;
			int offsetX = 2;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 7; k++)
			{
				float x = (float)Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float y = (float)Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(mod.GetTexture("Tiles/DnasLantern_Flame"), new Vector2((float)(i * 16 - (int)Main.screenPosition.X + offsetX) - (width - 16f) / 2f + x, (float)(j * 16 - (int)Main.screenPosition.Y + offsetY) + y) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
			if (Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
			{
				Vector2 Center = new Vector2(i + 1, j + 1) * 16 - Main.screenPosition + zero;
				Texture2D texture = mod.GetTexture("Tiles/LanternAura");
				Main.spriteBatch.Draw(texture, Center, new Rectangle(0, 0, 598, 598), new Color(255, 255, 255, 100), 0f, new Vector2(299, 299), 1f, SpriteEffects.None, 0f);
				//texture = mod.GetTexture("Tiles/LanternCross");
				//Main.spriteBatch.Draw(texture, Center, new Rectangle(0, 0, 20, 20), new Color(255, 255, 255, 100), 0f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
				//spriteBatch.Draw(texture, Center + new Vector2(499, 499) - Main.screenPosition, new Rectangle(0, 0, 998, 998), Color.White/*new Color(255, 255, 255, 100)*/);
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("DnasLantern");
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("DnasLantern"));
			mod.GetTileEntity("DnasLanternE").Kill(i, j);
		}
	}

	public class DnasLanternE : ModTileEntity
	{
		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return tile.active();
		}

		public bool off = false;
		public bool toggle = false;

		public override void Update()
		{
			if (toggle)
			{
				off = !off;
			}
			if (!off)
			{
				Main.tile[Position.X, Position.Y].frameX = 0;
				Main.tile[Position.X, Position.Y].frameY = 0;
				Main.tile[Position.X + 1, Position.Y].frameX = 18;
				Main.tile[Position.X + 1, Position.Y].frameY = 0;
				Main.tile[Position.X, Position.Y + 1].frameX = 0;
				Main.tile[Position.X, Position.Y + 1].frameY = 18;
				Main.tile[Position.X + 1, Position.Y + 1].frameX = 18;
				Main.tile[Position.X + 1, Position.Y + 1].frameY = 18;
				if ((Main.LocalPlayer.Center - ((Position.ToVector2() * 16) + new Vector2(16, 16))).Length() < 300)
				{
					if (Main.LocalPlayer.GetModPlayer<QwertyPlayer>().forcedAntiGravity == 0)
					{
						Main.LocalPlayer.velocity.Y = 0;
					}
					Main.LocalPlayer.GetModPlayer<QwertyPlayer>().forcedAntiGravity = 10;
				}
			}
			else
			{
				Main.tile[Position.X, Position.Y].frameX = 0 + 36;

				Main.tile[Position.X + 1, Position.Y].frameX = 18 + 36;

				Main.tile[Position.X, Position.Y + 1].frameX = 0 + 36;

				Main.tile[Position.X + 1, Position.Y + 1].frameX = 18 + 36;
			}
			toggle = false;
		}

		public override void NetSend(BinaryWriter writer, bool lightSend)
		{
			writer.Write(off);
		}

		public override void NetReceive(BinaryReader reader, bool lightReceive)
		{
			off = reader.ReadBoolean();
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"off", off}
			};
		}

		public override void Load(TagCompound tag)
		{
			off = tag.GetBool("off");
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
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