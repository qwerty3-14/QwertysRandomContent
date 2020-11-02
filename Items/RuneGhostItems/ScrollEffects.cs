using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
    public class ScrollEffects : ModPlayer
    {
        public bool aggro = false;
        public bool ice = false;
        public bool leech = false;
        int leechCooldown = 0;
        public bool pursuit = false;
        public override void ResetEffects()
        {
            aggro = false;
            ice = false;
            leech = false;
            pursuit = false;
        }
        public override void PreUpdate()
        {
            if (aggro && player.ownedProjectileCounts[mod.ProjectileType("AggroRuneFriendly")] < 1)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("AggroRuneFriendly"), (int)(500f * player.magicDamage), 0, player.whoAmI);
            }
            if (ice && player.ownedProjectileCounts[mod.ProjectileType("IceRuneFreindly")] < 1)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("IceRuneFreindly"), (int)(300f * player.meleeDamage), 0, player.whoAmI);
            }
            if(leechCooldown > 0)
            {
                leechCooldown--;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (leech && proj.ranged && proj.type != mod.ProjectileType("LeechRuneFreindly") && leechCooldown == 0)
            {
                leechCooldown = 30;
                float theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                Projectile.NewProjectile(target.Center + QwertyMethods.PolarVector(150, theta), QwertyMethods.PolarVector(-10, theta), mod.ProjectileType("LeechRuneFreindly"), (int)(50 * player.rangedDamage), 3f, Main.myPlayer);

            }
        }
    }
}
