using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DevItems.Phantom
{
	public class Genisis : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Genisis");
			Tooltip.SetDefault("Phantom454545 wanted this to be a staff, but NO it's totally a pickaxe");
		}

		public override void SetDefaults()
		{
			item.damage = 999;
			item.melee = true;

			item.useTime = 1;
			item.useAnimation = 10;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;

			item.width = 10;
			item.height = 20;
			item.crit = 5;
			item.autoReuse = true;
			item.pick = 1000;
			item.tileBoost = 100;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("HolyGlow"));
		}
	}
}