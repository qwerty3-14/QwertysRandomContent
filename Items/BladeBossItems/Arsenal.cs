using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class Arsenal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arsenal");
            Tooltip.SetDefault("Creates lingering blades!");
            ItemID.Sets.Yoyo[item.type] = true;
            ItemID.Sets.GamepadExtraRange[item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 30;
            item.height = 26;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 16f;
            item.knockBack = 2.5f;
            item.damage = 18;
            item.value = Item.sellPrice(gold: 10);
            item.rare = 7;
            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;

            item.shoot = mod.ProjectileType("ArsenalP");
        }

        private Projectile yoyo;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int n = 0; n < 6; n++)
            {
                yoyo = Main.projectile[Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI)];
                yoyo.localAI[1] = n;
            }

            return false;
        }
    }

    public class ArsenalP : QwertyYoyo
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 1;
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
            projectile.localNPCHitCooldown = 20;
            projectile.usesLocalNPCImmunity = true;
            yoyoCount = 6;
            time = 2f;
            range = 160;
            speed = 11f;
            //counterWeightId = mod.ProjectileType("SpiderCounterweight");
        }

        public override void YoyoHit(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            target.immune[projectile.owner] = 0;
        }

        public override void PostYoyoAI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter % 20 == 0)
            {
                Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(4f + Main.rand.NextFloat(2f), (float)Math.PI * 2f * Main.rand.NextFloat()), mod.ProjectileType("ArsenalSword"), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }
    }

    public class ArsenalSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
            projectile.usesLocalNPCImmunity = true;
            projectile.tileCollide = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }

        private bool redirect = false;
        private NPC target;

        public override void AI()
        {
            if (!redirect)
            {
                projectile.velocity *= .823f;
                projectile.rotation += (projectile.velocity.Length() * (float)Math.PI * .4f + (float)Math.PI / 60) * Math.Sign(projectile.velocity.X);
                if (QwertyMethods.ClosestNPC(ref target, 300, projectile.Center))
                {
                    redirect = true;
                    projectile.velocity = QwertyMethods.PolarVector(6f, (target.Center - projectile.Center).ToRotation());
                    projectile.extraUpdates++;
                }
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation();
            }

            if (projectile.velocity.Length() < .1f || redirect)
            {
                if (projectile.timeLeft > 60)
                {
                    projectile.timeLeft = 60;
                }
                projectile.alpha = (int)(255f * (1f - (projectile.timeLeft / 60f)));
            }
        }
    }
}