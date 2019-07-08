using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons
{
	public class ColdFront : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cold Front");
			Tooltip.SetDefault("");
			
		}
		public override void SetDefaults()
		{
			item.damage = 30;
			item.magic = true;
			
			item.useTime = 5;
			item.useAnimation = 45;
			item.useStyle = 5;
			item.knockBack = 0;
			item.value = 54000;
			item.rare = 3;
			
			item.autoReuse = true;
			item.width = 28;
			item.height = 30;
			
			item.mana =10;
			item.shoot = mod.ProjectileType("Icicle");
			item.shootSpeed =20;
			item.noMelee=true;
            
			
			
			
			
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemonScythe);
            recipe.AddIngredient(ItemID.WaterBolt);
            recipe.AddIngredient(mod.ItemType("AncientWave"));
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public int shotNumber = 0;
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float angle = (new Vector2(speedX, speedY)).ToRotation();
            float trueSpeed = (new Vector2(speedX, speedY)).Length();
            position = player.Center;
            if (shotNumber == 8)
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(60)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(60)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-60)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-60)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                shotNumber =0;
                return false;
            }
            if (shotNumber == 7)
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(52.5f)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(52.5f)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-52.5f)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-52.5f)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                shotNumber++;
                return false;
            }
            if (shotNumber == 6)
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(45)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(45)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-45)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-45)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                shotNumber++;
                return false;
            }
            if (shotNumber == 5)
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(37.5f)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(37.5f)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-37.5f)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-37.5f)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                shotNumber++;
                return false;
            }
            if (shotNumber == 4)
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(30)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(30)) * trueSpeed, type, (int)(damage*.8f), knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-30)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-30)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                shotNumber++;
                return false;
            }
            if (shotNumber == 3)
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(22.5f)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(22.5f)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-22.5f)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-22.5f)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                shotNumber++;
                return false;
            }
            if (shotNumber == 2)
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(15)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(15)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-15)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-15)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                shotNumber++;
                return false;
            }
            else if (shotNumber == 1)
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(7.5f)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(7.5f)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-7.5f)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-7.5f)) * trueSpeed, type, (int)(damage * .8f), knockBack, Main.myPlayer, 0f, 0f);
                shotNumber++;
                return false;
            }
            else
            {
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(0)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(0)) * trueSpeed, mod.ProjectileType("LargeIcicle"), damage, knockBack, Main.myPlayer, 0f, 0f);
                shotNumber=1;
                return false;
            }



            
            
        }


    }
    public class Icicle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Icicle");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.magic = true;
            projectile.tileCollide = true;




        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 120);

        }
        
        


    }
    public class LargeIcicle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Icicle");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 22;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.magic = true;
            projectile.tileCollide = true;




        }
       
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 120);

        }
        


    }

}

