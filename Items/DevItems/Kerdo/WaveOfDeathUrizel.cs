using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.Kerdo
{
    public class WaveOfDeathUrizel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wave Of Death Urizel");
            Tooltip.SetDefault("This magical rune contains the ancient power sword of URIZEL \nDev item");
        }

        public override void SetDefaults()
        {
            item.damage = 300;
            item.magic = true;

            item.useTime = 1;
            item.useAnimation = 60;

            item.useStyle = 102;
            item.noUseGraphic = true;
            item.knockBack = 1;
            item.value = 0;
            item.rare = 10;
            item.UseSound = SoundID.Item1;
            item.noMelee = true;
            item.width = 68;
            item.height = 68;

            item.shoot = mod.ProjectileType("WaveOfDeathUrizelP");
            item.autoReuse = true;

            item.shootSpeed = 3;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return player.itemAnimation == 1;
        }
    }

    public class WaveOfDeathUrizelP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 5;
            projectile.height = 5;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = 120;
            projectile.friendly = true;
        }

        private bool runOnce = true;
        private float ringSpeed = 0;
        private float radius;

        public override void AI()
        {
            if (runOnce)
            {
                ringSpeed = projectile.velocity.Length();
                projectile.velocity = Vector2.Zero;
                runOnce = false;
            }
            projectile.ai[0]++;
            radius = projectile.ai[0] * ringSpeed;
            for (int d = 0; d < projectile.ai[0] / 2f; d++)
            {
                Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(radius, Main.rand.NextFloat(-1, 1) * (float)Math.PI), mod.DustType("DeathWaveSmoke"), Vector2.Zero);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float dist = (targetHitbox.Center.ToVector2() - projectile.Center).Length();
            return (dist > radius - ringSpeed && dist < radius + ringSpeed);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }

    public class FlailArmsSpell : ModPlayer
    {
        public override void PostItemCheck()
        {
            if (!player.inventory[player.selectedItem].IsAir)
            {
                Item item = player.inventory[player.selectedItem];

                if (item.useStyle == 102 && player.itemAnimation > 0)
                {
                    if (player.itemAnimation < player.itemAnimationMax * .2f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 1;
                    }
                    else if (player.itemAnimation < player.itemAnimationMax * .4f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                    }
                    else if (player.itemAnimation < player.itemAnimationMax * .6f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 1;
                    }
                    else if (player.itemAnimation < player.itemAnimationMax * .8f)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                    }
                    else
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 1;
                    }
                    Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
                    if (player.direction != 1)
                    {
                        vector24.X = (float)player.bodyFrame.Width - vector24.X;
                    }
                    if (player.gravDir != 1f)
                    {
                        vector24.Y = (float)player.bodyFrame.Height - vector24.Y;
                    }
                    vector24 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
                    player.itemLocation = player.position + vector24;
                    Dust.NewDustPerfect(player.itemLocation, mod.DustType("DeathWaveSmoke"));
                }
            }
        }
    }
}