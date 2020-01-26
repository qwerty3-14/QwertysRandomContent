using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace QwertysRandomContent.Tiles
{
    public class FortressCarving1 : ModTile
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
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16
            };
            TileObjectData.addTile(Type);
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            ModTranslation name = CreateMapEntryName();
            dustType = mod.DustType("FortressDust");
            soundType = 21;
            soundStyle = 2;

            AddMapEntry(new Color(162, 184, 185));
            name.SetDefault("Fortress Carving");





        }


        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, mod.ItemType("FortressCarving1"));

        }
        public override bool CanExplode(int i, int j)
        {


            return false;




        }


    }
}