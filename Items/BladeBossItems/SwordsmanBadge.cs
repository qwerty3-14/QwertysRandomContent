using QwertysRandomContent.Config;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
	public class SwordsmanBadge : ModItem
	{
		public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicImperious ? base.Texture + "_Old" : base.Texture;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swordsman Badge");
			Tooltip.SetDefault("Swing swords faster when standing still");
		}

		public override void SetDefaults()
		{
			item.rare = 7;
			item.value = Item.sellPrice(0, 10, 0, 0);

			item.width = item.height = 20;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<AttackSpeedPlayer>().swordBadge = true;
		}
	}
}