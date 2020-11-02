using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
    public class CraftingRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rune");
            Tooltip.SetDefault("");
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            int f = (Main.LocalPlayer.GetModPlayer<QwertyPlayer>().ArmorFrameCounter / 4) % 80;
            spriteBatch.Draw
                (
                    RuneSprites.runeCycle[f],
                    position - new Vector2(2f, 2f),
                    null,
                    Color.White,
                    0,
                    origin,
                    scale * 2,
                    SpriteEffects.None,
                    0f
                );
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            int f = (Main.LocalPlayer.GetModPlayer<QwertyPlayer>().ArmorFrameCounter / 4) % 80;
            spriteBatch.Draw
                (
                    RuneSprites.runeCycle[f],
                    item.position - Main.screenPosition + Vector2.One * 54,
                    null,
                    Color.White,
                    0,
                    new Vector2(27, 27),
                    scale * 2,
                    SpriteEffects.None,
                    0f
                );
            return false;
        }

        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 54;
            item.maxStack = 999;
            item.value = 100;
            item.rare = 3;
            item.value = 500;
            item.rare = 9;
        }
    }
}