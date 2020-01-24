using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace QwertysRandomContent.Tiles
{
    public class LaunchPad : ModTile
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
            Main.tileSolid[Type] = true;
            Main.tileFrameImportant[Type] = true;
            //Main.tileNoAttach[Type] = true;
            //Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 0, 0);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            dustType = mod.DustType("CaeliteDust");
            soundType = 21;
            soundStyle = 2;
            minPick = 1;
            AddMapEntry(new Color(162, 184, 185));
            name.SetDefault("Launchpad");


        }
        public override bool CanPlace(int i, int j)
        {
            return Main.tile[i, j + 1].active();
        }
        public override void FloorVisuals(Player player)
        {
            //Main.NewText("Hi");


            player.velocity.Y = -20;

        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("Launchpad"));

        }


    }
}