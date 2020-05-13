using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class BouncyArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bouncy Arrow");
			Tooltip.SetDefault("BOING");
		}

		public override void SetDefaults()
		{
			item.damage = 6;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 20;
			item.rare = 2;
			item.width = 14;
			item.height = 32;

			item.shootSpeed = 6;

			item.consumable = true;
			item.shoot = mod.ProjectileType("BouncyArrowP");
			item.ammo = 40;
			item.maxStack = 999;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 200);
			recipe.AddIngredient(ItemID.PinkGel, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 200);
			recipe.AddRecipe();
		}
	}

	public class BouncyArrowP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bouncy Arrow");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 4;
			projectile.ranged = true;
			projectile.arrow = true;
			projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
		}

		public override bool OnTileCollide(Vector2 velocityChange)
		{
			projectile.penetrate--;
			for (int k = 0; k < 200; k++)
			{
				projectile.localNPCImmunity[k] = 0;
			}
			if (projectile.velocity.X != velocityChange.X)
			{
				projectile.velocity.X = -velocityChange.X;
			}
			if (projectile.velocity.Y != velocityChange.Y)
			{
				projectile.velocity.Y = -velocityChange.Y;
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
			projectile.velocity.Y = -projectile.velocity.Y;
		}

		public override void Kill(int timeLeft)
		{
		}
	}
}