using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
    public class RhuthiniumArmorEfffects : ModPlayer
    {
        public bool meleeSet = false;
        public bool magicSet = false;
        public bool rangedSet = false;
        public bool summonSet = false;
        private int rangedCounter = 0;
        private int summonCounter = 0;

        public override void ResetEffects()
        {
            meleeSet = magicSet = rangedSet = summonSet = false;
        }

        private void RhuthimisWraith(Vector2 position)
        {
            if (summonSet && summonCounter > 60)
            {
                summonCounter = 0;
                float rot = Main.rand.NextFloat() * 2f * (float)Math.PI;
                Projectile.NewProjectile(position + QwertyMethods.PolarVector(240, rot), QwertyMethods.PolarVector(4f, rot + (float)Math.PI), mod.ProjectileType("RhuthimisWraith"), (int)(30 * player.minionDamage), (int)(5f * player.minionKB), player.whoAmI);
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (item.melee && meleeSet && target.life < damage)
            {
                player.AddBuff(mod.BuffType("RhuthiniumMight"), 300);
            }
            RhuthimisWraith(target.Center);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.melee && meleeSet && target.life < damage)
            {
                player.AddBuff(mod.BuffType("RhuthiniumMight"), 300);
            }
            if (proj.magic && magicSet && crit)
            {
                player.statMana += damage / 2;
                player.ManaEffect(damage / 2);
                player.AddBuff(mod.BuffType("RhuthiniumMagic"), 300);
                for (int num71 = 0; num71 < 5; num71++)
                {
                    int num72 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                    Main.dust[num72].noLight = true;
                    Main.dust[num72].noGravity = true;
                    Main.dust[num72].velocity *= 0.5f;
                }
            }
            RhuthimisWraith(target.Center);
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            rangedCounter = 0;
        }

        public override void PreUpdate()
        {
            summonCounter++;
            rangedCounter++;
            if (rangedCounter > 300 && rangedSet)
            {
                player.AddBuff(mod.BuffType("RhuthiniumFocus"), 2);
            }
        }

        public override void PostUpdateEquips()
        {
            if (player.HasBuff(mod.BuffType("RhuthiniumMight")))
            {
                player.accRunSpeed *= 1.2f;
            }
        }
    }

    public class RhuthimisWraith : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RhuthimisWraith");
        }

        public override void SetDefaults()
        {
            projectile.minion = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.width = projectile.height = 14;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = 120;
            projectile.extraUpdates = 3;
            projectile.friendly = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
            if (projectile.timeLeft > 80)
            {
                projectile.alpha = (int)(255f * (40f - (120f - projectile.timeLeft)) / 40f);
            }
            if (projectile.timeLeft < 40)
            {
                projectile.alpha = (int)(255f * (40f - projectile.timeLeft) / 40f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha), projectile.rotation, new Vector2(21, 7), Vector2.One, SpriteEffects.None, 0);
            return false;
        }
    }
}