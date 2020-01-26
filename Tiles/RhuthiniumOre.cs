using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace QwertysRandomContent.Tiles
{
    public class RhuthiniumOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 550;

            ModTranslation name = CreateMapEntryName();
            dustType = mod.DustType("RhuthiniumDust");
            soundType = 21;
            soundStyle = 2;

            AddMapEntry(new Color(39, 129, 129), name);
            name.SetDefault("Rhuthinium Ore");
            drop = mod.ItemType("RhuthiniumOre");


            minPick = 1;

        }


        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }
    }
}