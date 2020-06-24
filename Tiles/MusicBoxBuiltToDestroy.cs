using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace QwertysRandomContent.Tiles
{
    internal class MusicBoxBuiltToDestroy : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            if (ModContent.GetInstance<SpriteSettings>().ClassicAncient)
            {
                texture += "_Old";
            }
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Music Box");
            AddMapEntry(new Color(139, 62, 40), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType("AncientMusicBox"));
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.showItemIcon = true;
            player.showItemIcon2 = mod.ItemType("AncientMusicBox");
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Main.spriteBatch.Draw(mod.GetTexture("Tiles/MusicBoxBuiltToDestroy_Glow" + (ModContent.GetInstance<SpriteSettings>().ClassicAncient ? "_Old" : "")), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, 72 / 2), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}