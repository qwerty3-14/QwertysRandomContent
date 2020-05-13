using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles
{
	public class FortressPillar : ModTile
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
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 0, 0);

			TileObjectData.newTile.Height = 1;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(162, 184, 185));

			dustType = mod.DustType("FortressDust");
			drop = mod.ItemType("FortressPillar");
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
		}

		public override bool CanPlace(int i, int j)
		{
			return Main.tile[i + 1, j].active() || Main.tile[i - 1, j].active() || Main.tile[i, j + 1].active() || Main.tile[i, j - 1].active(); ;
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			if (Main.tile[i, j + 1].type == mod.TileType("FortressPillar"))
			{
				if (Main.tile[i, j - 1].type == mod.TileType("FortressPillar"))
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
					}
				}
			}
			else if (Main.tile[i, j - 1].type == mod.TileType("FortressPillar"))
			{
				Main.tile[i, j].frameY = 54;
				Main.tile[i, j].frameX = 0;
				//bottom
			}
			else
			{
				Main.tile[i, j].frameY = 0;
				//solo
			}
		}
	}
}