using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Terraria.DataStructures;

namespace QwertysRandomContent.Tiles.Banners
{
	public class Banners : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			string item;
			switch (style)
			{
				case 0:
					item = "HopperBanner";
					break;
				case 1:
					item = "CrawlerBanner";
					break;
                case 2:
                    item = "GuardTileBanner";
                    break;
                case 3:
                    item = "FortressFlierBanner";
                    break;
                case 4:
                    item = "CasterBanner";
                    break;
                case 5:
                    item = "SpectorBanner";
                    break;
                case 6:
                    item = "TriceratankBanner";
                    break;
                case 7:
                    item = "UtahBanner";
                    break;
                case 8:
                    item = "VelocichopperBanner";
                    break;
                case 9:
                    item = "AntiAirBanner";
                    break;
                case 10:
                    item = "SwarmerBanner";
                    break;
                default:
					return;
			}
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item));
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				int style = Main.tile[i, j].frameX / 18;
				string type;
				switch (style)
				{
					case 0:
						type = "Hopper" + (Config.classicFortress ? "_Classic":"");
						break;
					case 1:
						type = "Crawler";
						break;
                    case 2:
                        type = "GuardTile" + (Config.classicFortress ? "_Classic" : "");
                        break;
                    case 3:
                        type = "FortressFlier";
                        break;
                    case 4:
                        type = "Caster";
                        break;
                    case 5:
                        type = "Spector";
                        break;
                    case 6:
                        type = "Triceratank";
                        break;
                    case 7:
                        type = "Utah";
                        break;
                    case 8:
                        type = "Velocichopper";
                        break;
                    case 9:
                        type = "AntiAir";
                        break;
                    case 10:
                        type = "Swarmer";
                        break;
                    default:
						return;
				}
				player.NPCBannerBuff[mod.NPCType(type)] = true;
				player.hasBanner = true;
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
}
