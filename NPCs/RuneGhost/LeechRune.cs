using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneGhost
{
    public class LeechRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 40;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 180;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(timer < 60)
            {
                return false;
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        int timer = 0;
        public override void AI()
        {
            timer++;
            if(timer == 60)
            {
                if (Main.netMode != 1)
                {
                    float closest = 10000;
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
            }
            if(timer > 60)
            {
                projectile.velocity = QwertyMethods.PolarVector(10, projectile.ai[0]);
            }
            projectile.rotation += Math.Sign(projectile.velocity.X) * (float)Math.PI / 60f;
        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 40; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));
            }
        }
        public NPC runeGhost;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            foreach (NPC npcSearch in Main.npc)
            {
                if (npcSearch.type == mod.NPCType("RuneGhost"))
                {
                    runeGhost = npcSearch;
                    break;
                }
            }
            if (runeGhost != null && runeGhost.active)
            {
                runeGhost.life += damage * 40;
                runeGhost.HealEffect(damage * 40, true);

            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float c = (timer / 60f);
            if (c > 1f)
            {
                c = 1f;
            }
            int frame = timer / 3;
            if(frame >19)
            {
                frame = 19;
            }
            spriteBatch.Draw(RuneSprites.runeTransition[(int)Runes.Leech][frame], projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(10, 10), Vector2.One * 2, 0, 0);
            return false;
        }
    }
}
