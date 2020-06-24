using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.BossFour
{
    public class TurretShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 720;
            projectile.tileCollide = false;
            //projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }

        /*
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }*/

        public override bool PreAI()
        {
            return base.PreAI();
        }

        public override void AI()
        {
            projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
        }
    }

    public class BurstShot : ModProjectile
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
            projectile.timeLeft = 720;
            projectile.tileCollide = false;
        }

        public float distance;
        private float closest = 250;
        private Player player;

        public override void AI()
        {
            if (Main.netMode != 1)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && (projectile.Center - Main.player[i].Center).Length() < closest)
                    {
                        projectile.Kill();
                    }
                }
            }
            projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
        }

        public float shotSpeed = 3;

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1 && timeLeft > 1)
            {
                QwertyMethods.ProjectileSpread(projectile.Center, 4, shotSpeed, mod.ProjectileType("TurretShot"), projectile.damage, projectile.knockBack, Main.myPlayer);
                QwertyMethods.ProjectileSpread(projectile.Center, 4, shotSpeed * 1.5f, mod.ProjectileType("TurretShot"), projectile.damage, projectile.knockBack, Main.myPlayer, rotation: (float)Math.PI / 4);
            }
        }
    }

    public class TurretGrav : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pew Pew");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicOLORD ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 360;
            projectile.tileCollide = false;
        }

        public float horiSpeed;
        public float horiAccCon = .075f;
        public float vertSpeed;
        public float vertAccCon = .075f;
        public float direction;
        public float maxSpeed = 12f;
        private float closest = 10000;

        public override void AI()
        {
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

            horiSpeed += (float)Math.Cos(projectile.ai[0]) * horiAccCon;
            vertSpeed += (float)Math.Sin(projectile.ai[0]) * vertAccCon;
            projectile.velocity = new Vector2(horiSpeed, vertSpeed);

            if (projectile.velocity.Length() > maxSpeed)
            {
                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * maxSpeed;
            }
            projectile.rotation += (float)Math.PI / 30;
            for (int i = 0; i < 1; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("B4PDust"));
            }
            closest = 10000;
        }
    }

    public class MagicMineLayer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Mine");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
        }

        public float horiSpeed;
        public float vertSpeed;
        public float direction;
        public float pullSpeed = .5f;
        public float dustSpeed = 20f;
        public NPC origin;

        public int frameTimer;
        public float distance;

        public override void AI()
        {
            origin = Main.npc[(int)projectile.ai[0]];

            Player player = Main.player[projectile.owner];

            if ((origin.Center - projectile.Center).Length() > projectile.ai[1])
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MagicMine"), projectile.damage, 0, Main.myPlayer);
            }
        }
    }

    public class MagicMine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Mine");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
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
        public float distance;

        public override void AI()
        {
            projectile.velocity = new Vector2(0, 0);

            Player player = Main.player[projectile.owner];

            frameTimer++;
            if (frameTimer % 5 == 0)
            {
                projectile.frame++;
                if (projectile.frame > 4)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}