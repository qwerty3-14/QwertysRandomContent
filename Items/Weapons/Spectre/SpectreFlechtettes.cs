using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Spectre
{
	public class SpectreFlechettes : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Flechettes");
			Tooltip.SetDefault("Flechettes do more damage as they pick up speed from gravity\nPierces tiles and enemies");
		}

		public override void SetDefaults()
		{
			item.damage = 61;
			item.thrown = true;
			item.knockBack = 5;
			item.value = Item.sellPrice(copper: 40);
			item.rare = 8;
			item.width = 14;
			item.height = 26;
			item.useStyle = 1;
			item.shootSpeed = 8f;
			item.useTime = 9;
			item.useAnimation = 18;
			item.consumable = true;
			item.shoot = mod.ProjectileType("SpectreFlechetteP");
			item.noUseGraphic = true;
			item.noMelee = true;
			item.maxStack = 999;
			item.autoReuse = true;
			item.UseSound = SoundID.Item39;
		}

		public override bool ConsumeItem(Player player)
		{
			return Main.rand.Next(2) == 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpectreBar, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 200);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float speed = new Vector2(speedX, speedY).Length();
			int numberOfProjectiles = 3 + Main.rand.Next(4);

			for (int p = 0; p < numberOfProjectiles; p++)
			{
				float direction = Main.rand.NextFloat(3 * (float)Math.PI / 4, 1 * (float)Math.PI / 4);
				Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}

	public class SpectreFlechetteP : Flechette
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Flechette");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.thrown = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.tileCollide = false;
			acceleration = .1f;
			maxVerticalSpeed = 10f;
			projectile.timeLeft = 180;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
	}
}