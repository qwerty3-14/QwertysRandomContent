using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles.FortressFurniture
{
    public class FortressBookcase : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;

            //Main.tileContainer[Type] = true;
            Main.tileLavaDeath[Type] = true;
            //TileID.Sets.HasOutlines[Type] = true;

            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(1, 3);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = true;

            TileObjectData.addTile(Type);
            //AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Fortress Bookcase");
            AddMapEntry(new Color(162, 184, 185), name);
            dustType = mod.DustType("FortressDust");
            //disableSmartCursor = true;
            adjTiles = new int[] { TileID.Bookcases };
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 32, mod.ItemType("FortressBookcase"));
            Chest.DestroyChest(i, j);
        }
    }
}