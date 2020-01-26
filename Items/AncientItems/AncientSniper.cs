using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
    public class AncientSniper : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Sniper");
            Tooltip.SetDefault("Harness the ancient power of sniping" + "\nRight click to zoom");
            if (ModContent.GetInstance<SpriteSettings>().ClassicAncient)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/AncientItems/Old/AncientSniper_Old");
            }

        }
        public override void SetDefaults()
        {
            item.damage = 48;
            item.ranged = true;

            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = 150000;
            item.rare = 3;
            item.UseSound = SoundID.Item11;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/Old/AncientSniper_Old_Glow") : mod.GetTexture("Items/AncientItems/AncientSniper_Glow");
            }
            item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -26;
            item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -2;
            item.width = 92;
            item.height = 30;
            item.crit = 25;
            item.shoot = 97;
            item.useAmmo = 97;
            item.shootSpeed = 36;
            item.noMelee = true;
            //item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/AncientItems/AncientSniper_Glow");


        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/Old/AncientSniper_Old_Glow") : mod.GetTexture("Items/AncientItems/AncientSniper_Glow");
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

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(item.GetGlobalItem<ItemUseGlow>().glowOffsetX, item.GetGlobalItem<ItemUseGlow>().glowOffsetY);
        }
        public override void HoldItem(Player player)
        {
            player.scope = true;
        }


    }


}

