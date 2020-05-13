using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
	public class HydraScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Scale");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 30;
			item.maxStack = 999;
			item.value = 100;
			item.rare = 3;
			item.value = 500;
			item.rare = 5;
		}
	}
}