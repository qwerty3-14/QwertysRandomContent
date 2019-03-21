using Microsoft.Xna.Framework;
using QwertysRandomContent.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Cactus
{
    public class BarrelCactus : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrel Cactus");
            Tooltip.SetDefault("Roll like a barrel! \nBounces off walls and enemies");
        }
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 34;
            item.thrown = true;
            
            item.shoot = mod.ProjectileType("BarrelCactusP");
            item.shootSpeed = 6f;
            item.useTime = 20;
            item.useAnimation = 20;
            item.damage = 6;
            item.maxStack = 999;
            item.useStyle = 1;
            item.knockBack = 0f;
            item.consumable = true;
            item.rare = 1;
            
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            speedX = new Vector2(speedX, speedY).Length() * (speedX>0 ? 1:-1);
            speedY = 0;
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
    public class BarrelCactusP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrel Cactus");


        }
        public override void SetDefaults()
        {
            
            projectile.width = 34;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.penetrate = 4;
            projectile.thrown = true;
            
            projectile.usesLocalNPCImmunity = true;


        }
        public override void AI()
        {
            if(projectile.velocity.Y < 6f)
            {
                projectile.velocity.Y += .1f;
            }
            
            projectile.rotation += (projectile.velocity.X * 2) / 17f;
        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            
            for (int k = 0; k < 200; k++)
            {
                projectile.localNPCImmunity[k] = 0;
            }
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.penetrate--;
                projectile.velocity.X = -velocityChange.X;
            }
            
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int k = 0; k < 200; k++)
            {
                projectile.localNPCImmunity[k] = 0;
            }

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            //Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, -projectile.velocity.X, -projectile.velocity.Y, mod.ProjectileType("BouncyArrowP"), projectile.damage, projectile.knockBack, Main.myPlayer);
            projectile.velocity.X = -projectile.velocity.X;
            
        }
        public override void Kill(int timeLeft)
        {
            
        }


    }


} 
