using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneSpectorBoss
{
    class QuitRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            
            projectile.width = 200;
            projectile.height = 200;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.timeLeft = 120;


        }
        public override void AI()
        {
            projectile.velocity = new Vector2(0, 0);
            if(projectile.alpha >0)
                projectile.alpha-=10;
            else
                projectile.alpha=0;


        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("QuitRuneDeath"));
            }
        }
    }
}
