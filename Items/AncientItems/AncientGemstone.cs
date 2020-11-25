using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
    public class AncientGemstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Gemstone");
            Tooltip.SetDefault("Avoiding damage for 10 damage will heal half the health lost from the last attack");
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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GemEffect>().hasEffect = true;
            base.UpdateAccessory(player, hideVisual);
        }
    }
    public class GemEffect : ModPlayer
    {
        public bool hasEffect = false;
        int damageToRecover = -1;
        int timerToRecover = -1;
        public override void ResetEffects()
        {
            hasEffect = false;
        }
        public override void PreUpdate()
        {
            if(timerToRecover > -1)
            {
                timerToRecover--;
            }
            if(hasEffect && damageToRecover != -1 && timerToRecover == 0)
            {
                player.statLife += damageToRecover;
                player.HealEffect(damageToRecover, true);
                for (int i = 0; i < 200; i++)
                {
                    float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);

                    Dust dust = Dust.NewDustPerfect(player.Center + QwertyMethods.PolarVector(200, theta), mod.DustType("AncientGlow"), QwertyMethods.PolarVector(-200 / 10, theta));
                    dust.noGravity = true;
                }
            }
        }
        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            damageToRecover = (int)(damage / 2);
            timerToRecover = 600;
            base.PostHurt(pvp, quiet, damage, hitDirection, crit);
        }
    }
}