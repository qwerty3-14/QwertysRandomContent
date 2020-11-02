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
    public class BigRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = projectile.height = 200;
            projectile.timeLeft = 1400;
        }
        int frame = 0;
        float timer = 0;
        int atackTimer = 0;
        bool madeRunes = false;
        public override void AI()
        {
            projectile.netUpdate = true;
            timer += projectile.ai[1];
            frame = (int)(20f * (timer / 240f));
            if(frame > 19)
            {
                frame = 19;
            }
            if(timer > 240f)
            {
                atackTimer++;
                switch ((int)projectile.ai[0])
                {
                    case (int)Runes.Aggro:
                        if (!madeRunes)
                        {
                            madeRunes = true;
                            if (Main.netMode != 1)
                            {
                                for(int i = 0; i < 8; i++)
                                {
                                    Projectile p = Main.projectile[Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("AggroRune"), (int)(projectile.damage * 1.3f), 0, Main.myPlayer)];
                                    p.rotation = (i / 8f) * (float)Math.PI * 2;
                                }
                            }
                        }
                        break;
                    case (int)Runes.Leech:
                        if(atackTimer % 120 == 1)
                        {
                            if (Main.netMode != 1)
                            {
                                float closest = 100000;
                                Player closestPlayer = null;
                                for (int i = 0; i < 255; i++)
                                {
                                    if (Main.player[i].active && (projectile.Center - Main.player[i].Center).Length() < closest)
                                    {
                                        closest = (projectile.Center - Main.player[i].Center).Length();
                                        closestPlayer = Main.player[i];
                                    }
                                }
                                if (closestPlayer != null)
                                {
                                    float r = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                                    Projectile p = Main.projectile[Projectile.NewProjectile(closestPlayer.Center + QwertyMethods.PolarVector(160f, r), Vector2.Zero, mod.ProjectileType("LeechRune"), (int)(projectile.damage), 0, Main.myPlayer)];
                                }
                            }
                        }
                        break;
                    case (int)Runes.IceRune:
                        if(!madeRunes)
                        {
                            madeRunes = true;
                            if(Main.netMode != 2)
                            {
                                Projectile p = Main.projectile[Projectile.NewProjectile(Main.LocalPlayer.Center, Vector2.Zero, mod.ProjectileType("IceRune"), (int)(projectile.damage * 1.6f), 0, Main.myPlayer)];
                                p.rotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                            }
                            projectile.Kill();
                        }
                        break;
                    case (int)Runes.Pursuit:
                        if (!madeRunes)
                        {
                            madeRunes = true;
                            if (Main.netMode != 1)
                            {
                                for(int i =0; i < 4; i++)
                                {
                                    Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(20f, (i /4f) * (float)Math.PI * 2f), mod.ProjectileType("PursuitRune"), (int)(projectile.damage * 1f), 0, Main.myPlayer);
                                }
                            }
                            projectile.Kill();
                        }
                        break;
                }
                if(atackTimer >=720)
                {
                    projectile.Kill();
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float c = (timer / 240f);
            if(c > 1f)
            {
                c = 1f;
            }
            spriteBatch.Draw(RuneSprites.bigRuneTransition[(int)projectile.ai[0]][frame], projectile.Center - Main.screenPosition, null, new Color(c, c, c, c), projectile.rotation, new Vector2(50, 50), Vector2.One * 2, 0, 0);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            string dustName = "";

            switch ((int)projectile.ai[0])
            {
                case 0:
                    dustName = "AggroRuneLash";
                    break;
                case 1:
                    dustName = "LeechRuneDeath";
                    break;
                case 2:
                    dustName = "IceRuneDeath";
                    break;
                case 3:
                    dustName = "PursuitRuneDeath";
                    break;
            }
            for(int d = 0; d < 300; d++)
            {
                //Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType(dustName));
                Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(Main.rand.Next(100), Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)), mod.DustType(dustName));
            }
        }
    }
}
