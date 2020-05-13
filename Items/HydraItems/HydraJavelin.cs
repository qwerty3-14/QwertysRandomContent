using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
using QwertysRandomContent.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//copied from example javelin forom example mod
namespace QwertysRandomContent.Items.HydraItems
{
	public class HydraJavelin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Javelin");
			Tooltip.SetDefault("Throws three at once!");
		}

		public override void SetDefaults()
		{
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
			item.shootSpeed = 10f;
			item.damage = 40;
			item.knockBack = 5f;
			item.useStyle = 1;
			item.useAnimation = 24;
			item.useTime = 24;
			item.width = 58;
			item.height = 58;
			item.maxStack = 999;
			item.rare = 5;

			item.value = 30;
			item.consumable = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.thrown = true;

			item.UseSound = SoundID.Item1;

			item.shoot = mod.ProjectileType("HydraJavelinP");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("HydraScale"), 2);

			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 111);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float angle = (new Vector2(speedX, speedY)).ToRotation();
			float trueSpeed = (new Vector2(speedX, speedY)).Length();
			Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(-5)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(-5)) * trueSpeed, type, damage, knockBack, Main.myPlayer, 0f, 0f);
			Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(0)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(0)) * trueSpeed, type, damage, knockBack, Main.myPlayer, 0f, 0f);
			Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(5)) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(5)) * trueSpeed, type, damage, knockBack, Main.myPlayer, 0f, 0f);
			return false;
		}
	}

	public class HydraJavelinP : Javelin
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Javelin");
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.penetrate = 1;
			projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
			projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 1;
			maxStickingJavelins = 100;
			dropItem = mod.ItemType("HydraJavelin");
		}

		public override void ExtraAI()
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBeamGlow"));
		}
	}
}