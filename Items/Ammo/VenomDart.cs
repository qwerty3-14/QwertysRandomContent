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
    public class VenomDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Venom Dart");
            Tooltip.SetDefault("Creates venom clouds when hitting enemies");
        }
        public override void SetDefaults()
        {
            item.damage = 20;
            item.width = 10;
            item.height = 20;
            item.ranged = true;
            item.ammo = AmmoID.Dart;
            item.shoot = mod.ProjectileType("VenomDartP");
            item.shootSpeed = 3;
            item.knockBack = 1;
            item.rare = 3;
            item.consumable = true;
            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.VialofVenom, 1);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
    public class VenomDartP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.ranged = true;
            projectile.width = projectile.height = 10;
            projectile.extraUpdates = 1;
            projectile.penetrate = 7;
            projectile.usesLocalNPCImmunity = true;
            projectile.friendly = true;
        }
        public override void AI()
        {
            int num67 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 171, 0f, 0f, 100);
            Main.dust[num67].scale = (float)Main.rand.Next(1, 10) * 0.1f;
            Main.dust[num67].noGravity = true;
            Main.dust[num67].fadeIn = 1.5f;
            Main.dust[num67].velocity *= 0.25f;
            Main.dust[num67].velocity += projectile.velocity * 0.25f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            target.AddBuff(BuffID.Venom, 60 * 30);
            Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(Main.rand.NextFloat(), Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)), mod.ProjectileType("VenomCloud"), (int)(.5f * projectile.damage), projectile.knockBack, projectile.owner);
        }
    }
    public class VenomCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 30;
            projectile.height = 30;
            //projectile.aiStyle = 44;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.scale = 1.1f;
            projectile.penetrate = -1;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.timeLeft = 30;
        }
        public override void AI()
        {
            int num322 = 6;

            projectile.velocity *= 0.96f;
            projectile.alpha += 4;
            if (projectile.alpha > 255)
            {
                projectile.Kill();
            }

            if (++projectile.frameCounter >= num322)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.perIDStaticNPCImmunity[projectile.type][target.whoAmI] = Main.GameUpdateCount + 10;
            target.immune[projectile.owner] = 0;
            target.AddBuff(BuffID.Venom, 60 * 30);
        }
    }
}
