using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class PowerDown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Caelite Wrath");
            Description.SetDefault("You deal 20% less damage!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }
        int timer;
        public override void Update(NPC npc, ref int buffIndex)
        {
            timer++;
            if (timer > 30)
            {
                for (int d = 0; d < 30; d++)
                {
                    Dust dust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("CaeliteDust"))];
                    dust.velocity *= 3;
                }
                timer = 0;
            }

        }
        public override void Update(Player player, ref int buffIndex)
        {
            timer++;
            if (timer > 30)
            {
                for (int d = 0; d < 30; d++)
                {
                    Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, mod.DustType("CaeliteDust"))];
                    dust.velocity *= 3;
                }
                timer = 0;
            }
        }


    }
    public class PowerDownNPC : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].HasBuff(mod.BuffType("PowerDown")))
            {
                damage = (int)(damage * .8f);
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (player.HasBuff(mod.BuffType("PowerDown")))
            {
                damage = (int)(damage * .8f);
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            if (npc.HasBuff(mod.BuffType("PowerDown")))
            {
                damage = (int)(damage * .8f);
            }
        }

    }
}