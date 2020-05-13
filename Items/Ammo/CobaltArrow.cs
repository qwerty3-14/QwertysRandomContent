using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Ammo
{
	public class CobaltArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Arrow");
			Tooltip.SetDefault("Remains stationarry until you right click, which sends it flying at your cursor");
		}

		public override void SetDefaults()
		{
			item.damage = 10;
			item.ranged = true;
			item.knockBack = 2;
			item.value = 5;
			item.rare = 3;
			item.width = 14;
			item.height = 32;

			item.shootSpeed = 40;

			item.consumable = true;
			item.shoot = mod.ProjectileType("CobaltArrowP");
			item.ammo = 40;
			item.maxStack = 999;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CobaltBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}

	public class CobaltArrowP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Arrow");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.ranged = true;
			projectile.arrow = true;
			projectile.timeLeft = 300;
			projectile.tileCollide = true;
			projectile.alpha = 0;
		}

		public bool HasRightClicked = false;
		public bool runOnce = true;
		public float targetRotation;

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if ((Main.mouseRight && projectile.timeLeft <= 290 && Main.myPlayer == projectile.owner || HasRightClicked))
			{
				projectile.alpha = 0;
				if (runOnce)
				{
					HasRightClicked = true;

					projectile.timeLeft = 3600;
					runOnce = false;
					projectile.netUpdate = true;
				}

				projectile.velocity.X = (float)Math.Cos(targetRotation + MathHelper.ToRadians(-90)) * 10f;
				projectile.velocity.Y = (float)Math.Sin(targetRotation + MathHelper.ToRadians(-90)) * 10f;
			}
			else
			{
				projectile.alpha = (int)(255f - ((float)projectile.timeLeft / 300f) * 255f);
				projectile.velocity.X = 0;
				projectile.velocity.Y = 0;
				if (Main.LocalPlayer == player)
				{
					projectile.ai[0] = (Main.MouseWorld - projectile.Center).ToRotation();
					if (projectile.ai[1] == 1)
					{
						projectile.ai[0] += (float)Math.PI;
					}
					projectile.netUpdate = true;

					//projectile.netUpdate = true;
				}
				targetRotation = projectile.ai[0] + (float)Math.PI / 2;
				projectile.rotation = targetRotation;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(HasRightClicked);
			writer.Write(runOnce);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			HasRightClicked = reader.ReadBoolean();
			runOnce = reader.ReadBoolean();
		}
	}
}