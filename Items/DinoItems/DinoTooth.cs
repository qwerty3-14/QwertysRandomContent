using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.DinoItems
{
	[AutoloadEquip(EquipType.Neck)]
	public class DinoTooth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dino Tooth");
			Tooltip.SetDefault("Increases armor penetration by 18");
		}

		public override void SetDefaults()
		{
			item.value = 10000;
			item.rare = 6;
			item.width = 14;
			item.height = 20;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.armorPenetration += 18;
		}
	}
}