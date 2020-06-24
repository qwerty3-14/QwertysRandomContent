using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles
{
    public class Mirrors : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16
            };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.addTile(Type);

            Main.tileFrameImportant[Type] = true;

            Main.tileLavaDeath[Type] = true;

            ModTranslation name = CreateMapEntryName();

            soundType = 21;
            soundStyle = 2;

            AddMapEntry(new Color(162, 184, 185));
            name.SetDefault("Fortress Carving");
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int style = frameX / 18;
            switch (style)
            {
                case 0:
                case 1:
                    Item.NewItem(i * 16, j * 16, 16, 32, mod.ItemType("WoodenMirror"));
                    break;

                case 2:
                case 3:
                    Item.NewItem(i * 16, j * 16, 16, 32, mod.ItemType("FortressMirror"));
                    break;
            }
        }
    }
}