using Microsoft.Xna.Framework;
using QwertysRandomContent.NPCs.Fortress;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
	public class ShotgunOfExcessiveForce : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shotgun of Excessive Force");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.ranged = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.damage = 17;
			item.knockBack = 5f;
			item.width = 46;
			item.height = 22;
			item.useAmmo = AmmoID.Bullet;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 8f;
			item.useTime = 24;
			item.useAnimation = 24;
			item.UseSound = SoundID.Item38;
			item.rare = 3;
			item.value = 120000;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-7, -1);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int bulletCount = 5 + Main.rand.Next(2);
			float dir = new Vector2(speedX, speedY).ToRotation();
			float speed = new Vector2(speedX, speedY).Length();
			for (int b = 0; b < bulletCount; b++)
			{
				Projectile p = Main.projectile[Projectile.NewProjectile(position + QwertyMethods.PolarVector(24, dir), QwertyMethods.PolarVector(speed, dir + Main.rand.NextFloat(-1, 1) * (float)Math.PI / 18), type, damage, knockBack, player.whoAmI)];
				p.extraUpdates++;
				p.GetGlobalProjectile<Etims>().effect = true;
			}

			player.velocity = QwertyMethods.PolarVector(12f, dir + (float)Math.PI);

			return false;
		}
	}

	public class Etims : GlobalProjectile
	{
		public bool effect = false;
		public override bool InstancePerEntity => true;

		public override void AI(Projectile projectile)
		{
			if (effect)
			{
				Dust.NewDustPerfect(projectile.Center, mod.DustType("BloodforceDust"), Vector2.Zero);
			}
		}

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (effect)
			{
				if (target.GetGlobalNPC<FortressNPCGeneral>().fortressNPC)
				{
					for (int i = 0; i < damage / 3; i++)
					{
						Dust d = Dust.NewDustPerfect(projectile.Center, mod.DustType("BloodforceDust"));
						d.velocity *= 5f;
					}
					damage *= 2;
				}
			}
		}
	}
}