using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
	public class AggroScroll : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aggro Scroll");
			Tooltip.SetDefault("Killing enemies with magic will summon aggro runes");
		}

		public override void SetDefaults()
		{
			item.value = 500000;
			item.rare = 9;
			item.magic = true;
			item.damage = 420;

			item.width = 54;
			item.height = 56;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			var modPlayer = player.GetModPlayer<QwertyPlayer>();

			modPlayer.aggroScroll = true;
		}
	}

	internal class AggroRuneFreindly : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.aiStyle = -1;

			projectile.width = 62;
			projectile.height = 62;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.alpha = 0;
			projectile.tileCollide = false;
			projectile.timeLeft = 120;
			projectile.magic = true;
		}

		public int runeTimer;
		public float startDistance = 200f;
		public float direction;
		public float runeSpeed = 10;
		public bool runOnce = true;
		public float aim;

		public override void AI()
		{
			if (projectile.alpha > 0)
				projectile.alpha--;
			else
				projectile.alpha = 0;

			if (projectile.timeLeft <= 2)
			{
				projectile.alpha = 255;

				projectile.friendly = true;
				for (int d = 0; d <= 100; d++)
				{
					Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AggroRuneLash"));
				}
			}
		}
	}
}