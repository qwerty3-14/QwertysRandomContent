using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
    public class AncientGemstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Gemstone");
            Tooltip.SetDefault("Halves you max health but avoiding damge for 5 sec after being hit will fully heal you");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicAncient ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;

            item.expert = true;
            item.width = 32;
            item.height = 32;
            item.value = 150000;
            item.rare = 3;
            item.accessory = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/AncientGemstone_Glow_Old") : mod.GetTexture("Items/AncientItems/AncientGemstone_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 /= 2;
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.gemRegen = true;
        }
    }
}