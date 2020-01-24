using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace QwertysRandomContent.Tiles.Decorations
{
    public class DecorativePlant : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.addTile(Type);
            Main.tileFrameImportant[Type] = true;
            ModTranslation name = CreateMapEntryName();
            dustType = 0;


            AddMapEntry(new Color(55, 95, 62));
            name.SetDefault("Decorative Plant");
            //drop = mod.ItemType("DecorativePlant");




        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 48, mod.ItemType("DecorativePlant"));
        }


    }
}