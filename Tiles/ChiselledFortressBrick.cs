using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace QwertysRandomContent.Tiles
{
    public class ChiselledFortressBrick : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            if (Config.classicFortress)
            {
                texture += "_Classic";
            }
            return base.Autoload(ref name, ref texture);
        }
        public override void SetDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            //Main.tileSolidTop[Type] = true;
            Main.tileSolid[Type] = true;
            //Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.Platforms[Type] = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleMultiplier = 27;
            TileObjectData.newTile.StyleWrapLimit = 27;
            TileObjectData.newTile.UsesCustomCanPlace = false;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);
            dustType = mod.DustType("FortressDust");
            soundType = 21;
            soundStyle = 2;
            minPick = 50;
            AddMapEntry(new Color(162, 184, 185));
            mineResist = 1;
            drop = mod.ItemType("ChiselledFortressBrick");

        }


        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }
        public override bool CanExplode(int i, int j)
        {


            return true;




        }


    }
}