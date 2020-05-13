using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons
{
	public class ChaosRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Rifle");
			Tooltip.SetDefault("Picks a random bullet to shoot");
		}

		public override void SetDefaults()
		{
			item.damage = 179;
			item.ranged = true;

			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 500000;
			item.rare = 9;
			item.UseSound = SoundID.Item11;

			item.width = 82;
			item.height = 34;

			item.shoot = 97;
			item.useAmmo = 97;
			item.shootSpeed = 6;
			item.noMelee = true;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CraftingRune"), 20);

			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-28, -1);
		}

		public override bool ConsumeAmmo(Player player)
		{
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//reset damage and knockback to avoid ammo modifing
			damage = (int)(item.damage * player.meleeDamage);
			knockBack = item.knockBack * knockBack;

			QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
			Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(1));

			bool yes = true;
			float anotherSpeedVariable = trueSpeed.Length();

			modPlayer.PickRandomAmmo(item, ref type, ref anotherSpeedVariable, ref yes, ref damage, ref knockBack, Main.rand.Next(2) == 0);
			Projectile.NewProjectile(position.X, position.Y, trueSpeed.X, trueSpeed.Y, type, damage, knockBack, player.whoAmI);

			return false;
		}
	}
}