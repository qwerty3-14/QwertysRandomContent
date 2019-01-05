using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Consumables
{
	
	public class CephalopodSlurry : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cephalopod Slurry");
			//Tooltip.SetDefault("30 minute duration");
		}

		public override void SetDefaults()
		{
            item.UseSound = SoundID.Item2;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.maxStack = 30;
            item.consumable = true;
            item.width = 10;
            item.height = 10;
            item.buffType = 26;
            item.buffTime = 72000;
            item.rare = 1;
            item.value = Item.sellPrice(0, 0, 5, 0);

        }

		
		
		
		
	}
    public class SlurryDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
           if(npc.type == NPCID.Squid)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CephalopodSlurry"), Main.rand.Next(4) + (Main.expertMode ? 1: 0));
            }
        }
    }
}