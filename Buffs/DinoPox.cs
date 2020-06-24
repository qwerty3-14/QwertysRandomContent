using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class DinoPox : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Dino Pox");
            Description.SetDefault("Deadly enough to kill a dinosaur");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<QwertyPlayer>().DinoPox = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            npc.lifeRegen -= 20;
        }
    }
}