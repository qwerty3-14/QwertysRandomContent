using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneGhost
{
    public class PursuitRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 20;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 360;
        }
        private int timer;
        private float closest = 10000;
        bool runOnce = true;

        public override void AI()
        {
            if(runOnce)
            {
                runOnce = false;
                projectile.rotation = projectile.velocity.ToRotation();
            }
            timer++;
            if(timer > 119 && timer % 120 == 0)
            {
                projectile.extraUpdates++;
                for (int d = 0; d <= 40; d++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("PursuitRuneDeath"));
                }
            }
            if (Main.netMode != 1)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && (projectile.Center - Main.player[i].Center).Length() < closest)
                    {
                        closest = (projectile.Center - Main.player[i].Center).Length();
                        projectile.ai[0] = (Main.player[i].Center - projectile.Center).ToRotation();
                        projectile.netUpdate = true;
                    }
                }
            }
            projectile.rotation.SlowRotation(projectile.ai[0], (float)Math.PI / 240f);
            projectile.velocity = QwertyMethods.PolarVector(12, projectile.rotation);
            closest = 10000;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float c = (timer / 40f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = timer / 2;
            if (frame > 19)
            {
                frame = 19;
            }
            spriteBatch.Draw(RuneSprites.runeTransition[(int)Runes.Pursuit][frame], projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(10, 5), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 40; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("PursuitRuneDeath"));
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 180);
        }
    }
}
