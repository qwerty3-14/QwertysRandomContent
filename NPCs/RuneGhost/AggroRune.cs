using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.NPCs.RuneGhost.RuneBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneGhost
{
    public class AggroRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 62;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 720;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        int timer;
        bool runOnce = true;
        Vector2 middle;
        public override void AI()
        {
            timer++;
            if (runOnce)
            {
                middle = projectile.Center;
                runOnce = false;
                projectile.position += QwertyMethods.PolarVector(200, projectile.rotation);
            }
            if (timer % 120 == 29)
            {
                projectile.velocity = Vector2.Zero;
                if(Main.netMode != 1)
                {
                    projectile.netUpdate = true;
                }
            }
            if(timer % 120 == 90 && Main.netMode != 1)
            {
                Projectile.NewProjectile(middle, QwertyMethods.PolarVector(1, projectile.rotation), mod.ProjectileType("AggroStrike"), projectile.damage, 0);
            }
            if(timer % 120 == 119)
            {
                if(Main.netMode != 1)
                {
                    Vector2 goTo = middle + QwertyMethods.PolarVector(200, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI));
                    projectile.velocity = (goTo - projectile.Center) / 30f;
                    projectile.netUpdate = true;
                }
            }
            projectile.rotation = (projectile.Center - middle).ToRotation();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
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
            spriteBatch.Draw(RuneSprites.runeTransition[(int)Runes.Aggro][frame], projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(15.5f, 15.5f), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(timer % 120 > 30 && timer % 120 < 90 && middle != null)
            {
                Texture2D texture = mod.GetTexture("NPCs/RuneGhost/AggroLaser");
                spriteBatch.Draw(texture, middle - Main.screenPosition, null, Color.White, projectile.rotation, Vector2.UnitY, new Vector2(1500, 1), 0, 0);
            }
        }
        
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(projectile.velocity);
            writer.WriteVector2(projectile.position);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {

            projectile.velocity = reader.ReadVector2();
            projectile.position = reader.ReadVector2();
        }
    }
    public class AggroStrike : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 2;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 30;
        }
        bool runOnce = true;
        int timer;
        public override void AI()
        {
            timer++;
            if(runOnce)
            {
                runOnce = false;
                projectile.rotation = projectile.velocity.ToRotation();
                projectile.velocity = Vector2.Zero;
            }

        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(timer < 5)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + QwertyMethods.PolarVector(1000, projectile.rotation));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int frame = timer / 2;
            if(timer > 22)
            {
                frame = (30 - timer) / 2;
            }
            if (frame > 3)
            {
                frame = 3;
            }
            float c = (float)frame / 3f;
            for(int i = 0; i < 3000; i+=8)
            {
                spriteBatch.Draw(RuneSprites.aggroStrike[frame], projectile.Center + QwertyMethods.PolarVector(i, projectile.rotation) - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(0, 3), Vector2.One * 2, 0, 0);
            }
            
            return false;
        }
    }
}
