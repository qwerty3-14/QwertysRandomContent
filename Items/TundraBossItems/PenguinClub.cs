using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TundraBossItems
{
	public class PenguinClub : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Penguin Club");
			Tooltip.SetDefault("Launches penguins upon hitting an enemy");
		}

		public override void SetDefaults()
		{
			item.damage = 17;
			item.melee = true;

			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 1;
			item.knockBack = 2;
			item.value = 100000;
			item.rare = 1;
			item.UseSound = SoundID.Item1;

			item.width = 48;
			item.height = 48;

			item.noMelee = false;
			item.autoReuse = true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			Projectile penguin = Main.projectile[Projectile.NewProjectile(player.Center, (target.Center - player.Center).SafeNormalize(-Vector2.UnitY) * 6, mod.ProjectileType("SlidingPenguin"), item.damage, item.knockBack, player.whoAmI, ai1: 1)];
			penguin.melee = true;
			penguin.ranged = false;
		}
	}
}