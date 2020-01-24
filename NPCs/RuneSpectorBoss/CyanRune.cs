using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneSpectorBoss
{
    class CyanRune : ModProjectile
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
            projectile.timeLeft = 128 + 720;
            projectile.light = 1f;


        }
        public int runeTimer;
        public float startDistance = 200f;
        public float theta;
        public Projectile IceA;
        public Projectile IceB;
        public Projectile IceC;
        public Projectile IceD;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.velocity = new Vector2(0, 0);
            if (projectile.alpha > 0)
                projectile.alpha -= 2;
            else
                projectile.alpha = 0;
            runeTimer++;
            if (runeTimer == 128)
            {


                if (Main.netMode == 0)
                {
                    theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                    Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance, 0, 0, mod.ProjectileType("IceRune"), projectile.damage, 3f, Main.myPlayer);
                    Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta + Math.PI / 2) * startDistance, player.Center.Y + (float)Math.Sin(theta + Math.PI / 2) * startDistance, 0, 0, mod.ProjectileType("IceRune"), projectile.damage, 3f, Main.myPlayer);
                    Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta + Math.PI) * startDistance, player.Center.Y + (float)Math.Sin(theta + Math.PI) * startDistance, 0, 0, mod.ProjectileType("IceRune"), projectile.damage, 3f, Main.myPlayer);
                    Projectile.NewProjectile(player.Center.X + (float)Math.Cos(theta + 3 * Math.PI / 2) * startDistance, player.Center.Y + (float)Math.Sin(theta + 3 * Math.PI / 2) * startDistance, 0, 0, mod.ProjectileType("IceRune"), projectile.damage, 3f, Main.myPlayer);
                }
                else
                {

                    theta = MathHelper.ToRadians(Main.rand.Next(0, 360));
                    IceA = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance, mod.ProjectileType("IceRune"), projectile.damage, 3f, Main.myPlayer)];
                    IceB = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("IceRune"), projectile.damage, 3f, Main.myPlayer)];
                    IceC = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("IceRune"), projectile.damage, 3f, Main.myPlayer)];
                    IceD = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("IceRune"), projectile.damage, 3f, Main.myPlayer)];


                    IceA.scale = theta;
                    IceB.scale = (float)(theta + Math.PI / 2);
                    IceC.scale = (float)(theta + Math.PI);
                    IceD.scale = (float)(theta + 3 * Math.PI / 2);
                    //IceA.position = new Vector2(player.Center.X + (float)Math.Cos(theta) * startDistance, player.Center.Y + (float)Math.Sin(theta) * startDistance);
                    //IceB.position = new Vector2(player.Center.X + (float)Math.Cos(theta + Math.PI / 2) * startDistance, player.Center.Y + (float)Math.Sin(theta + Math.PI / 2) * startDistance);
                    //IceC.position = new Vector2(player.Center.X + (float)Math.Cos(theta + Math.PI) * startDistance, player.Center.Y + (float)Math.Sin(theta + Math.PI) * startDistance);
                    //IceD.position = new Vector2(player.Center.X + (float)Math.Cos(theta + 3 * Math.PI / 2) * startDistance, player.Center.Y + (float)Math.Sin(theta + 3 * Math.PI / 2) * startDistance);

                }


            }
            if (runeTimer == 129)
            {

            }


        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("IceRuneDeath"));
            }
        }
    }
    class IceRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.timeLeft = 720;
            projectile.light = 1f;
            projectile.coldDamage = true;

        }
        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;
        public float theta;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                if (Main.netMode != 0)
                {
                    projectile.Center = new Vector2(player.Center.X + (float)Math.Cos(projectile.scale) * startDistance, player.Center.Y + (float)Math.Sin(projectile.scale) * startDistance);
                    projectile.scale = 1;
                }
                projectile.rotation = (player.Center - projectile.Center).ToRotation() - (float)Math.PI / 2;
                runOnce = false;
            }
            if (projectile.alpha > 0)
                projectile.alpha--;
            else
                projectile.alpha = 0;

            if (projectile.timeLeft <= 60)
            {
                projectile.velocity.X = (float)Math.Cos(projectile.rotation + Math.PI / 2) * runeSpeed;
                projectile.velocity.Y = (float)Math.Sin(projectile.rotation + Math.PI / 2) * runeSpeed;
            }
            else if (projectile.timeLeft <= 120)
            {
                projectile.velocity = new Vector2(0, 0);

            }
            else
            {
                projectile.rotation += (float)((2 * Math.PI) / (Math.PI * 2 * 200 / runeSpeed));
                projectile.velocity.X = runeSpeed * (float)Math.Cos(projectile.rotation) + player.velocity.X;
                projectile.velocity.Y = runeSpeed * (float)Math.Sin(projectile.rotation) + player.velocity.Y;
            }


        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frozen, 60);
        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("IceRuneDeath"));
            }
        }
    }
}
