using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
	[AutoloadEquip(EquipType.Head)]
	public class RuneGhostMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune Ghost Mask");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.value = 0;
			item.rare = 1;

			item.vanity = true;
			item.width = 20;
			item.height = 20;
		}

		public override bool DrawHead()
		{
			return false;
		}
	}
}