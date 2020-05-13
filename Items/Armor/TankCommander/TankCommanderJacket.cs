using Terraria;

using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.TankCommander
{
	[AutoloadEquip(EquipType.Body)]
	public class TankCommanderJacket : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tank Commander Uniform");
			Tooltip.SetDefault("+1 max minions\n7% increased morph damage and critical strike chance");
		}

		public override void SetDefaults()
		{
			item.value = 100000;
			item.rare = 1;

			item.width = 22;
			item.height = 12;
			item.defense = 2;
			item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
			player.GetModPlayer<ShapeShifterPlayer>().morphDamage += .07f;
			player.GetModPlayer<ShapeShifterPlayer>().morphCrit += 7;
		}

		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}
	}
}