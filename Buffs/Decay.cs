using Terraria;
using Terraria.ModLoader;
using QwertysRandomContent.NPCs;
using QwertysRandomContent;
using Microsoft.Xna.Framework;

namespace QwertysRandomContent.Buffs
{
	public class Decay : ModBuff
	{
		
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Decay");
			Description.SetDefault("Deal 10% less damage, defense reduced by 10, life slowly depleting");
			Main.debuff[Type] = true;
			
			
			longerExpertDebuff = false;
		}
        public override void Update(NPC npc, ref int buffIndex)
        {
            

        }




    }
    public class DecayEffect : GlobalNPC
    {
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            //drawColor.A = (byte)(drawColor.A *.8f);
            if(npc.HasBuff(mod.BuffType("Decay")))
            {
                drawColor.R = (byte)(drawColor.R * .5f);
                drawColor.G = (byte)(drawColor.G * .5f);
                drawColor.B = (byte)(drawColor.B * .5f);
            }
            
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if(npc.HasBuff(mod.BuffType("Decay")))
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 4;
            }
            
        }
        public override void AI(NPC npc)
        {
            if (npc.HasBuff(mod.BuffType("Decay")))
            {
                if (npc.defense >= 0 && npc.defense < 10)
                {
                    npc.defense = 0;
                }
                else
                {
                    npc.defense -= 10;
                }
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].HasBuff(mod.BuffType("Decay")))
            {
                damage = (int)(damage * .9f);
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (player.HasBuff(mod.BuffType("Decay")))
            {
                damage = (int)(damage * .9f);
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            if (npc.HasBuff(mod.BuffType("Decay")))
            {
                damage = (int)(damage * .9f);
            }
        }
    }

}