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
    public class IceRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 36;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 720;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < 4; i++)
            {
                if(Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + QwertyMethods.PolarVector(dist, projectile.rotation + i * (float)Math.PI/2f) + new Vector2(-18, -18), new Vector2(36, 36)))
                {
                    return true;
                }
            }
            return false;
        }
        float dist = 200;
        int timer;
        public override void AI()
        {
            timer++;
            if(projectile.timeLeft > 150)
            {
                projectile.rotation += (float)Math.PI / 60f;
            }
            if(projectile.timeLeft > 120)
            {
                projectile.Center = Main.LocalPlayer.Center;
            }
            else if(projectile.timeLeft < 60)
            {
                dist -= 8;
            }
            
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = projectile.Center + QwertyMethods.PolarVector(dist, projectile.rotation + i * (float)Math.PI / 2f) + new Vector2(-18, -18);
                for (int d = 0; d <= 40; d++)
                {
                    Dust.NewDust(pos, 36, 36, mod.DustType("IceRuneDeath"));
                }
            }
            
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int i = 0; i < 4; i++)
            {
                float c = (timer / 60f);
                if (c > 1f)
                {
                    c = 1f;
                }
                int frame = timer / 3;
                if (frame > 19)
                {
                    frame = 19;
                }
                spriteBatch.Draw(RuneSprites.runeTransition[(int)Runes.IceRune][frame], projectile.Center + QwertyMethods.PolarVector(dist, projectile.rotation + i * (float)Math.PI / 2f) - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(9, 9), Vector2.One * 2, 0, 0);
            }
            
            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frozen, 60);
        }
    }
}
