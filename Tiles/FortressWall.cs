using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Tiles
{
	public class FortressWall : ModWall
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
			Main.wallHouse[Type] = true;
			dustType = mod.DustType("FortressDust");
			drop = mod.ItemType("FortressWall");
			AddMapEntry(new Color(76, 80, 92));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}