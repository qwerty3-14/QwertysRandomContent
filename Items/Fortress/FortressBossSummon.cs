using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
	public class FortressBossSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pendant of the sky god");
			Tooltip.SetDefault("Can be used at the altar" + "\nIn multiplayer you have to use it like a regular boss summon, should still be used near the altar (couldn't get the altar working in multiplayer)");
		}
		
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 24;
			item.maxStack = 999;
			item.value = 10000;
			item.rare = 3;
			
			item.useTurn = true;
			
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 1;
			item.consumable = true;
		}
        public override bool CanUseItem(Player player)
        {
            return (Main.netMode != 0 && !NPC.AnyNPCs(mod.NPCType("FortressBoss")) && player.GetModPlayer<FortressBiome>(mod).TheFortress);
        }
        public override bool UseItem(Player player)
        {
            QwertyWorld.FortressBossQuotes();

            NPC.NewNPC((int)(player.Center.X + 400), (int)player.Center.Y, mod.NPCType("FortressBoss"));
            return true;
        }




    }
}
