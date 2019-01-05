using Terraria;
using Terraria.ModLoader;
using QwertysRandomContent.NPCs;
using QwertysRandomContent;

namespace QwertysRandomContent.Buffs
{
	public class Grabbed : ModBuff
	{
		
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Grabbed");
			Description.SetDefault("If you can read this you're hacking!");
			Main.debuff[Type] = true;
			
			
			longerExpertDebuff = false;
		}
        public override void Update(NPC npc, ref int buffIndex)
        {
            
        }




    }
}