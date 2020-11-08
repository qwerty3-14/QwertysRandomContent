using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
    public class NanoDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nano Dart");
            Tooltip.SetDefault("Breaks into a cluster of homing nanites");
        }
        public override void SetDefaults()
        {
            item.damage = 13;
            item.width = 10;
            item.height = 20;
            item.ranged = true;
            item.ammo = AmmoID.Dart;
            item.shoot = mod.ProjectileType("NanoDartP");
            item.shootSpeed = 3;
            item.knockBack = 1;
            item.rare = 3;
            item.consumable = true;
            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Nanites, 1);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
    public class NanoDartP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.ranged = true;
            projectile.width = projectile.height = 14;
            projectile.extraUpdates = 1;
            projectile.friendly = true;
        }
        public override void AI()
        {
            Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(-6, projectile.rotation + (float)Math.PI / 2), 135, Vector2.Zero, 100);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            target.AddBuff(BuffID.Confused, 60 * 10);
        }
        public override void Kill(int timeLeft)
        {
            for (int r = 0; r < 5; r++)
            {
                Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(1f, (r / 5f) * (float)Math.PI * 2 + projectile.velocity.ToRotation()), mod.ProjectileType("Nanoprobe"), (int)(.5f * projectile.damage), 0, projectile.owner);
            }
        }
    }
    public class Nanoprobe : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.ranged = true;
            projectile.width = projectile.height = 6;
            projectile.extraUpdates = 5;
            projectile.friendly = true;
            projectile.timeLeft = 1200;
        }
        float dir;
        bool runOnce = true;
        int counter = 0;
        NPC target;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(counter < 40)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void AI()
        {
            if(runOnce)
            {
                dir = projectile.velocity.ToRotation();
                runOnce = false;
            }
            projectile.velocity = QwertyMethods.PolarVector(4.5f, dir);
            counter++;
            if(counter > 40)
            {
                if(QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center))
                {
                    dir.SlowRotation((target.Center - projectile.Center).ToRotation(), (float)Math.PI / 60f);
                }
            }
            Dust d = Dust.NewDustPerfect(projectile.Center, 135, Vector2.Zero, 100);
            d.noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Confused, 60 * 10);
        }
    }
}
