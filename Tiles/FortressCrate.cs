using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles
{
	public class FortressCrate : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileTable[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				18
			};
			TileObjectData.newTile.CoordinateWidth = 18;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.addTile(Type);
			dustType = mod.DustType("FortressDust");
			AddMapEntry(new Color(162, 184, 185));

			drop = mod.ItemType("FortressCrate");
		}
	}
}