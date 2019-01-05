using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Tiles
{
	public class FortressWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = mod.DustType("FortressDust");
            drop = mod.ItemType("FortressWall");
            AddMapEntry(new Color(76, 80, 92));
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.4f;
			g = 0.4f;
			b = 0.4f;
		}
	}
}