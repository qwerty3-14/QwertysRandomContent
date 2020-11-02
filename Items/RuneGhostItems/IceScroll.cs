using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;

namespace QwertysRandomContent.Items.RuneGhostItems
{
    public class IceScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Scroll");
            Tooltip.SetDefault("Summons two ice runes to orbit you");
        }

        public override void SetDefaults()
        {
            item.value = 500000;
            item.rare = 9;
            item.melee = true;
            item.damage = 300;

            item.width = 48;
            item.height = 60;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ScrollEffects>().ice = true;
        }
    }

    internal class IceRuneFreindly : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 36;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = 30;
            target.immune[projectile.owner] = 0;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < 2; i++)
            {
                if (Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + QwertyMethods.PolarVector(dist, projectile.rotation + i * (float)Math.PI) + new Vector2(-18, -18), new Vector2(36, 36)))
                {
                    return true;
                }
            }
            return false;
        }
        float dist = 50;
        int timer;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<ScrollEffects>().ice)
            {
                projectile.timeLeft = 2;
            }
            timer++;
            projectile.rotation += (float)Math.PI / 30f;
            projectile.Center = player.Center;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 pos = projectile.Center + QwertyMethods.PolarVector(dist, projectile.rotation + i * (float)Math.PI) + new Vector2(-18, -18);
                for (int d = 0; d <= 40; d++)
                {
                    Dust.NewDust(pos, 36, 36, mod.DustType("IceRuneDeath"));
                }
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int i = 0; i < 2; i++)
            {
                float c = (timer / 60f);
                if (c > 1f)
                {
                    c = 1f;
                }
                int frame = timer / 3;
                if (frame > 19)
                {
                    frame = 19;
                }
                spriteBatch.Draw(RuneSprites.runeTransition[(int)Runes.IceRune][frame], projectile.Center + QwertyMethods.PolarVector(dist, projectile.rotation + i * (float)Math.PI) - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(9, 9), Vector2.One * 2, 0, 0);
            }

            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 120);
        }
    }
}