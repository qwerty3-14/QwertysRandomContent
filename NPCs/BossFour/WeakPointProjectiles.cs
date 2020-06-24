using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.BossFour
{
    public class BlackHoleSeed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 30;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BlackHole"), projectile.damage, 3f, Main.myPlayer, projectile.ai[0]);
            }
        }
    }

    public class BlackHole : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BlackHole");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 480;
            projectile.tileCollide = false;
        }

        public float horiSpeed;
        public float vertSpeed;
        public float direction;
        public float pullSpeed = .5f;
        public float dustSpeed = 20f;
        public NPC mass;
        public Projectile proj;
        public int frameTimer;
        public Dust dust;
        public Item item;
        private int dustCounter;

        public override void AI()
        {
            projectile.velocity = new Vector2(0, 0);
            projectile.timeLeft -= (int)projectile.ai[0] - 1;
            //Player player = Main.player[projectile.owner];

            for (int p = 0; p < 255; p++)
            {
                direction = (projectile.Center - Main.player[p].Center).ToRotation();
                horiSpeed = (float)Math.Cos(direction) * pullSpeed / 2;
                vertSpeed = (float)Math.Sin(direction) * pullSpeed / 2;
                Main.player[p].velocity += new Vector2(horiSpeed, vertSpeed);

                for (int i = 0; i < 1; i++)
                {
                    int dust = Dust.NewDust(Main.player[p].position, Main.player[p].width, Main.player[p].height, mod.DustType("B4PDust"), 0, 0);
                }
            }
            /*
            for (int g = 0; g < 3; g++)
            {
                Dust blackEs = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"), 0, 0)];
                direction = (projectile.Center - blackEs.position).ToRotation();
                horiSpeed = (float)Math.Cos(direction) * pullSpeed * 50;
                vertSpeed = (float)Math.Sin(direction) * pullSpeed * 50;
                blackEs.velocity += new Vector2(horiSpeed, vertSpeed);
            }
            */

            for (int d = 0; d < 80; d++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(Main.rand.NextFloat(10, 200), theta), mod.DustType("BlackHoleMatter"), QwertyMethods.PolarVector(6, theta + (float)Math.PI / 2));
                dust.scale = 1f;
            }

            for (int i = 0; i < Main.dust.Length; i++)
            {
                dust = Main.dust[i];
                if (!dust.noGravity)
                {
                    direction = (projectile.Center - dust.position).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed * 5;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed * 5;
                    dust.velocity += new Vector2(horiSpeed, vertSpeed);
                }
                if (dust.type == mod.DustType("BlackHoleMatter"))
                {
                    direction = (projectile.Center - dust.position).ToRotation();
                    dust.velocity += QwertyMethods.PolarVector(.8f, direction);
                    if ((dust.position - projectile.Center).Length() < 10)
                    {
                        dust.scale = 0f;
                    }
                    else
                    {
                        dust.scale = .35f;
                    }
                }
            }
            for (int i = 0; i < 200; i++)
            {
                mass = Main.npc[i];
                if (!mass.boss && mass.active && mass.knockBackResist != 0f)
                {
                    direction = (projectile.Center - mass.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    mass.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(mass.position, mass.width, mass.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < Main.item.Length; i++)
            {
                item = Main.item[i];
                if (item.position != new Vector2(0, 0))
                {
                    direction = (projectile.Center - item.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    item.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(item.position, item.width, item.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                proj = Main.projectile[i];
                if (proj.active && proj.type != mod.ProjectileType("BlackHole") && proj.type != mod.ProjectileType("SideLaser"))
                {
                    direction = (projectile.Center - proj.Center).ToRotation();
                    horiSpeed = (float)Math.Cos(direction) * pullSpeed;
                    vertSpeed = (float)Math.Sin(direction) * pullSpeed;
                    proj.velocity += new Vector2(horiSpeed, vertSpeed);
                    for (int g = 0; g < 1; g++)
                    {
                        int dust = Dust.NewDust(proj.position, proj.width, proj.height, mod.DustType("B4PDust"), horiSpeed * dustSpeed, vertSpeed * dustSpeed);
                    }
                }
            }
        }
    }

    public class BurstShot2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            projectile.width = 102;
            projectile.height = 104;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 120;
            projectile.tileCollide = false;
        }

        public float distance;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
        }

        public float shotSpeed = 3;

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                QwertyMethods.ProjectileSpread(projectile.Center, 4, shotSpeed, mod.ProjectileType("TurretShot"), projectile.damage, projectile.knockBack, Main.myPlayer);
                QwertyMethods.ProjectileSpread(projectile.Center, 4, shotSpeed * 1.5f, mod.ProjectileType("TurretShot"), projectile.damage, projectile.knockBack, Main.myPlayer, rotation: (float)Math.PI / 4);
            }
        }
    }

    public class MegaBurst : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");
        }

        public override void SetDefaults()
        {
            projectile.width = 300;
            projectile.height = 300;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
        }

        public float distance;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
            projectile.timeLeft -= (int)projectile.ai[0] - 1;
        }

        public float shotSpeed = 3;

        public override void Kill(int timeLeft)
        {
            for (int r = 0; r < 6; r++)
            {
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos(r * (2 * Math.PI / 6)) * shotSpeed * 1.5f, (float)Math.Sin(r * (2 * Math.PI / 6)) * shotSpeed * 1.5f, mod.ProjectileType("BurstShot2"), projectile.damage, 0, Main.myPlayer);
                }
            }
        }
    }
}