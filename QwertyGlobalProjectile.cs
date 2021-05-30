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
        /*
        int timer = 0;
        int bulletsPerRocket = 7; //making this 7 will make it just like vanilla
        int fireRate = 5; //making this 5 will make it just like vanilla
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == 615)
            {
                timer++;
                //determine fire rate
                if(timer % fireRate == 0)
                {
                    projectile.ai[1] = -1; //forces VBeater to fire
                }
                else
                {
                    projectile.ai[1] = 3; //forces VBeater to not fire
                }

                projectile.ai[0] = 5; //forces VBeater not to fire a rocket
                if((timer / fireRate) % bulletsPerRocket == 0)
                {
                    projectile.ai[0] = -1; //forces VBeater to fire a rocket
                }

                //this code preserves the buildup animation
                if (timer >= 40f)
                {
                    projectile.ai[0] += 35 * 2;
                }
                if (timer >= 80f)
                {
                    projectile.ai[0] += 35;
                }
                if (timer >= 120f)
                {
                    projectile.ai[0] += 35;
                }
            }
            return base.PreAI(projectile);
        }
        */
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