using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    public class HydraBarrage : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape shift: Hydra Barrage");
            Tooltip.SetDefault("Launches a barrage of hydra breath");
        }

        public const int dmg = 142;
        public const float kb = 0f;
        public const int def = -1;

        public override void SetDefaults()
        {
            item.damage = dmg;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.QuickShiftType;
            item.GetGlobalItem<ShapeShifterItem>().morphCooldown = 24;
            item.noMelee = true;

            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 5;

            item.value = 250000;
            item.rare = 5;
            item.noUseGraphic = true;
            item.width = 18;
            item.height = 32;
            item.shoot = mod.ProjectileType("HydraBarrageBase");
            item.shootSpeed = 0f;
            item.channel = true;
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if ((Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) || player.HasBuff(mod.BuffType("MorphCooldown")))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class HydraBarrageBase : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HydraBarrage Barrage Base");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 90;
        }

        private bool runOnce = true;

        public override void AI()
        {
            if (runOnce)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("HydraBarrageHead"), projectile.damage, projectile.knockBack, projectile.owner, 1f, 2);
                }
                runOnce = false;
            }

            Player player = Main.player[projectile.owner];
            player.Center = projectile.Center;
            player.immune = true;
            player.immuneTime = 2;
            player.statDefense = 0;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
            //projectile.rotation = (QwertysRandomContent.GetLocalCursor(projectile.owner) - projectile.Center).ToRotation();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }

    public class HydraBarrageHead : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.width = projectile.height = 10;
            projectile.tileCollide = false;
            projectile.timeLeft = 30;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
        }

        private bool runOnce = true;
        private Vector2 offset;

        public override void AI()
        {
            if (runOnce)
            {
                offset = QwertyMethods.PolarVector(100, Main.rand.NextFloat() * (float)Math.PI * 2f);
                runOnce = false;
            }
            projectile.scale = projectile.ai[0];
            Player player = Main.player[projectile.owner];
            Vector2 diff = ((player.Center + offset) - projectile.Center);
            projectile.velocity = diff;
            if (projectile.velocity.Length() > 16f)
            {
                projectile.velocity = projectile.velocity.SafeNormalize(Vector2.UnitY) * 16f;
            }
            projectile.rotation = (QwertysRandomContent.GetLocalCursor(player.whoAmI) - projectile.Center).ToRotation();
            if (projectile.timeLeft == 10)
            {
                Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(57 * projectile.scale, projectile.rotation), QwertyMethods.PolarVector(10, projectile.rotation), mod.ProjectileType("HydraBarrageBreath"), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], 0f);
            }
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[1] > 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("HydraBarrageHead"), (int)(projectile.damage * .8f), projectile.knockBack * .8f, projectile.owner, projectile.ai[0] * .8f, projectile.ai[1] - 1);
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D neck = mod.GetTexture("Items/HydraItems/HydraBarrageNeck");
            Texture2D neckBase = mod.GetTexture("Items/HydraItems/HydraBarrageBase");
            for (float f = 0; f < (projectile.Center - Main.player[projectile.owner].Center).Length(); f += neck.Height * projectile.scale)
            {
                spriteBatch.Draw(f == 0 ? neckBase : neck, Main.player[projectile.owner].Center - Main.screenPosition + QwertyMethods.PolarVector(f, (projectile.Center - Main.player[projectile.owner].Center).ToRotation()), null, lightColor, (projectile.Center - Main.player[projectile.owner].Center).ToRotation() + (float)Math.PI / 2, neck.Size() * .5f, Vector2.One * projectile.scale, 0, 0);
            }
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, Vector2.One * 36f, Vector2.One * projectile.scale, 0, 0);
            return false;
        }
    }

    public class HydraBarrageBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrage Breath");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 14;
            projectile.height = 8;

            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            projectile.scale = projectile.ai[0];
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBreathGlow"));
        }
    }
}