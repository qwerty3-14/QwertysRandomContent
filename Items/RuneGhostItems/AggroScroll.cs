using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;

namespace QwertysRandomContent.Items.RuneGhostItems
{
    public class AggroScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aggro Scroll");
            Tooltip.SetDefault("An aggro rune occasionally fires");
        }

        public override void SetDefaults()
        {
            item.value = 500000;
            item.rare = 9;
            item.magic = true;
            item.damage = 500;

            item.width = 54;
            item.height = 56;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ScrollEffects>().aggro = true;
        }
    }

    internal class AggroRuneFriendly : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 62;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.magic = true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        int timer;
        bool runOnce = true;
        Vector2 relativeVelocity = Vector2.Zero;
        Vector2 relativePosition = Vector2.Zero;
        public override void AI()
        {
            projectile.velocity = Vector2.Zero;
            Player player = Main.player[projectile.owner];
            if(player.GetModPlayer<ScrollEffects>().aggro)
            {
                projectile.timeLeft = 2;
            }
            timer++;
            if (runOnce)
            {
                relativePosition = QwertyMethods.PolarVector(50, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
                runOnce = false;
            }
            if (timer % 120 == 29)
            {
                relativeVelocity = Vector2.Zero;
            }
            if (timer % 120 == 90 && Main.netMode != 1)
            {
                Projectile.NewProjectile(player.Center, QwertyMethods.PolarVector(1, projectile.rotation), mod.ProjectileType("AggroStrikeFriendly"), projectile.damage, 0, projectile.owner);
            }
            if (timer % 120 == 119)
            {
                Vector2 goTo = QwertyMethods.PolarVector(50, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
                relativeVelocity = (goTo - relativePosition) / 30f;
            }
            relativePosition += relativeVelocity;
            projectile.Center = player.Center + relativePosition;
            projectile.rotation = (projectile.Center - player.Center).ToRotation();
            
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
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
            spriteBatch.Draw(RuneSprites.runeTransition[(int)Runes.Aggro][frame], projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(15.5f, 15.5f), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (timer % 120 > 30 && timer % 120 < 90)
            {
                Texture2D texture = mod.GetTexture("NPCs/RuneGhost/AggroLaser");
                spriteBatch.Draw(texture, Main.player[projectile.owner].Center - Main.screenPosition, null, Color.White, projectile.rotation, Vector2.UnitY, new Vector2(1500, 1), 0, 0);
            }
        }
    }
    public class AggroStrikeFriendly : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 2;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 30;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        bool runOnce = true;
        int timer;
        public override void AI()
        {
            timer++;
            if (runOnce)
            {
                runOnce = false;
                projectile.rotation = projectile.velocity.ToRotation();
                projectile.velocity = Vector2.Zero;
            }
            Player player = Main.player[projectile.owner];
            projectile.Center = player.Center;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (timer < 5)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + QwertyMethods.PolarVector(1000, projectile.rotation));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int frame = timer / 2;
            if (timer > 22)
            {
                frame = (30 - timer) / 2;
            }
            if (frame > 3)
            {
                frame = 3;
            }
            float c = (float)frame / 3f;
            for (int i = 0; i < 3000; i += 8)
            {
                spriteBatch.Draw(RuneSprites.aggroStrike[frame], projectile.Center + QwertyMethods.PolarVector(i, projectile.rotation) - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(0, 3), Vector2.One * 2, 0, 0);
            }

            return false;
        }
    }
}