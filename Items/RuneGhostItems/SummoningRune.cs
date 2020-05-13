using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
	public class SummoningRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Summoning Rune");
			Tooltip.SetDefault("Summons The Rune Ghost");
			ItemID.Sets.ItemNoGravity[item.type] = true;
			ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13; // This helps sort inventory know this is a boss summoning item.
		}

		public override void SetDefaults()
		{
			item.width = 54;
			item.height = 54;
			item.maxStack = 20;
			item.rare = 3;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
			item.noUseGraphic = true;
		}

		public override bool CanUseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("RuneSpector"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			item.stack--;
			return true;
		}
	}
}