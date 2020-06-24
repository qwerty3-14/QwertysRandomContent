using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent
{
    internal class QwertyGlobalProjectile : GlobalProjectile
    {
        public bool ignoresArmor = false;
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == 195 || projectile.type == 374 || projectile.type == 376 || projectile.type == 389 || projectile.type == 408 || projectile.type == 478 || projectile.type == 479 || projectile.type == 309 || projectile.type == 664 || projectile.type == 666 || projectile.type == 668 || projectile.type == 680)
            {
                projectile.minion = true;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (ignoresArmor)
            {
                Player player = Main.player[projectile.owner];
                int finalDefense = target.defense - player.armorPenetration;
                target.ichor = false;
                target.betsysCurse = false;
                if (finalDefense < 0)
                {
                    finalDefense = 0;
                }
                damage += finalDefense / 2;
            }
        }
    }
}