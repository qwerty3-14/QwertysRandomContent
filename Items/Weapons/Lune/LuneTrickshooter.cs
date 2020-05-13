using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Lune
{
	public class LuneTrickshooter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Trickshooter");
			Tooltip.SetDefault("Musket balls are converted to Lune trick shots!" + "\nTrick shots can bounce off walls 3 times and gain significant damage if they do so");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.ranged = true;

			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = 5;
			item.knockBack = 1;
			item.value = 10000;
			item.rare = 1;
			item.UseSound = SoundID.Item11;

			item.width = 54;
			item.height = 30;

			item.shoot = 97;
			item.useAmmo = 97;
			item.shootSpeed = 9f;
			item.noMelee = true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.Bullet)
			{
				type = mod.ProjectileType("Trickshot");
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);

			recipe.AddIngredient(mod.ItemType("LuneBar"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class Trickshot : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.light = 0.5f;
			//projectile.alpha = 255;
			projectile.scale = 1.2f;
			projectile.timeLeft = 600;
			projectile.ranged = true;
			projectile.extraUpdates = 1;
		}

		private int bounceCounter = 3;

		public override bool OnTileCollide(Vector2 velocityChange)
		{
			if (bounceCounter > 0)
			{
				if (projectile.velocity.X != velocityChange.X)
				{
					projectile.velocity.X = -velocityChange.X;
				}
				if (projectile.velocity.Y != velocityChange.Y)
				{
					projectile.velocity.Y = -velocityChange.Y;
				}
				projectile.damage = (int)(projectile.damage * 1.5f);
				bounceCounter--;
				return false;
			}

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bounceCounter = 0;
		}
	}
}