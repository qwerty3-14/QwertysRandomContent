using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class TeleportationArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Teleportation Arrow (Jump)");
			Tooltip.SetDefault("Teleports you where it lands");
		}

		public override void SetDefaults()
		{
			item.damage = 6;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 500;
			item.rare = 8;
			item.width = 12;
			item.height = 50;

			item.shootSpeed = 6;

			item.consumable = true;
			item.shoot = mod.ProjectileType("TeleportationArrowP");
			item.ammo = 40;
			item.maxStack = 999;
		}

		/*
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 200);
			recipe.AddIngredient(ItemID.PinkGel, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 200);
			recipe.AddRecipe();
		}
		*/
	}

	public class TeleportationArrowP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Teleportation Arrow");
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
			Player player = Main.player[projectile.owner];
		}

		public override bool OnTileCollide(Vector2 velocityChange)
		{
			Player player = Main.player[projectile.owner];
			player.position.X = projectile.position.X;
			player.position.Y = projectile.position.Y - 30;
			Main.PlaySound(25, player.position, 0);
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			player.position.X = target.Center.X;
			player.position.Y = target.Center.Y - target.height;
			Main.PlaySound(25, player.position, 0);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			Player player = Main.player[projectile.owner];
			player.position.X = target.Center.X;
			player.position.Y = target.Center.Y - target.height;
			Main.PlaySound(25, player.position, 0);
		}

		public override void Kill(int timeLeft)
		{
		}
	}
}