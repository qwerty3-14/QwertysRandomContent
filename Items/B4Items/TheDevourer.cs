using QwertysRandomContent.Items.Fortress.CaeliteWeapons;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
	public class TheDevourer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Devourer");
			Tooltip.SetDefault("Mines a 9x9 area!");
		}

		public override void SetDefaults()
		{
			item.damage = 170;
			item.melee = true;

			item.useTime = 52;
			item.useAnimation = 52;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 750000;
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.scale = 2;
			item.width = 85;
			item.height = 82;
			//item.crit = 5;
			item.autoReuse = true;
			item.pick = 250;
			item.tileBoost = 6;
			item.GetGlobalItem<AoePick>().miningRadius = 4;
		}
	}
}