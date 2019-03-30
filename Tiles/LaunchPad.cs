using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
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
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<VFanE>().Hook_AfterPlacement, 0, 0, true);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            dustType = mod.DustType("CaeliteDust");
            soundType = 21;
            soundStyle = 2;
            minPick = 1;
            AddMapEntry(new Color(162, 184, 185));
            name.SetDefault("Launchpad");
            

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