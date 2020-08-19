using Microsoft.Xna.Framework;
using QwertysRandomContent.Items.B4Items;
using QwertysRandomContent.Items.Fortress.CaeliteWeapons;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    public class HydraArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Arrow");
            Tooltip.SetDefault("Splits into 3 arrows each doing 60% damage");
        }

        public override void SetDefaults()
        {
            item.damage = 7;
            item.ranged = true;
            item.knockBack = 2;
            item.value = 5;
            item.rare = 5;
            item.width = 14;
            item.height = 32;

            item.shootSpeed = 6;

            item.consumable = true;
            item.shoot = mod.ProjectileType("HydraArrowP");
            item.ammo = 40;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("HydraScale"), 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }

    public class HydraArrowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Arrow");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.arrow = true;
            projectile.timeLeft = 1;
        }

        public override void AI()
        {
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.owner == Main.myPlayer)
            {
                float V = 30;
                Projectile arrow1 = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-90)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-90)) * V, mod.ProjectileType("HydraArrowP2"), (int)(projectile.damage * .6f), projectile.knockBack, Main.myPlayer)];
                Projectile arrow2 = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-80)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-80)) * V, mod.ProjectileType("HydraArrowP2"), (int)(projectile.damage * .6f), projectile.knockBack, Main.myPlayer)];
                Projectile arrow3 = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(projectile.rotation + MathHelper.ToRadians(-100)) * V, (float)Math.Sin(projectile.rotation + MathHelper.ToRadians(-100)) * V, mod.ProjectileType("HydraArrowP2"), (int)(projectile.damage * .6f), projectile.knockBack, Main.myPlayer)];
                if (projectile.GetGlobalProjectile<arrowHoming>().B4HomingArrow)
                {
                    arrow1.GetGlobalProjectile<arrowHoming>().B4HomingArrow = true;
                    arrow2.GetGlobalProjectile<arrowHoming>().B4HomingArrow = true;
                    arrow3.GetGlobalProjectile<arrowHoming>().B4HomingArrow = true;
                }
                if (projectile.GetGlobalProjectile<arrowgigantism>().GiganticArrow)
                {
                    arrow1.scale *= 3;
                    arrow2.scale *= 3;
                    arrow3.scale *= 3;
                    arrow1.GetGlobalProjectile<arrowgigantism>().GiganticArrow = true;
                    arrow2.GetGlobalProjectile<arrowgigantism>().GiganticArrow = true;
                    arrow3.GetGlobalProjectile<arrowgigantism>().GiganticArrow = true;
                    projectile.GetGlobalProjectile<arrowgigantism>().GiganticArrow = false;
                }
                arrow1.GetGlobalProjectile<ArrowWarping>().warpedArrow = projectile.GetGlobalProjectile<ArrowWarping>().warpedArrow;
                arrow2.GetGlobalProjectile<ArrowWarping>().warpedArrow = projectile.GetGlobalProjectile<ArrowWarping>().warpedArrow;
                arrow3.GetGlobalProjectile<ArrowWarping>().warpedArrow = projectile.GetGlobalProjectile<ArrowWarping>().warpedArrow;
            }
        }
    }

    public class HydraArrowP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydra Arrow");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.arrow = true;
        }

        public override void AI()
        {
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
        }
    }
}