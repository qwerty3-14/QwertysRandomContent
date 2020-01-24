using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneSpectorBoss
{
    class PurpleRune : ModProjectile
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
            projectile.timeLeft = 720 + 128;
            projectile.light = 1f;

        }
        public int runeTimer;
        public float runeSpeed = 10;
        public override void AI()
        {
            projectile.velocity = new Vector2(0, 0);
            if (projectile.alpha > 0)
                projectile.alpha -= 2;
            else
                projectile.alpha = 0;

            runeTimer++;
            if (runeTimer == 128 && Main.netMode != 2)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((1 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((1 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRune"), projectile.damage, 3f, Main.myPlayer);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((2 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((2 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRune"), projectile.damage, 3f, Main.myPlayer);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((3 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((3 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRune"), projectile.damage, 3f, Main.myPlayer);
                //Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((4 * 2 * Math.PI) / 5) * runeSpeed, (float)Math.Sin((4 * 2 * Math.PI) / 5) * runeSpeed, mod.ProjectileType("PursuitRune"), projectile.damage, 3f, Main.myPlayer);
                //Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((5 * 2 * Math.PI) / 5) * runeSpeed, (float)Math.Sin((5 * 2 * Math.PI) / 5) * runeSpeed, mod.ProjectileType("PursuitRune"), projectile.damage, 3f, Main.myPlayer);

            }


        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("PursuitRuneDeath"));
            }
        }
    }
    class PursuitRune : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 40;
            projectile.height = 20;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = 720;
            projectile.light = 1f;

        }
        public int runeTimer;
        public float runeSpeed = 10;
        public float runeDirection;
        public float runeTargetDirection;
        public bool runOnce = true;
        public int f;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                projectile.rotation = (projectile.velocity).ToRotation();
            }
            if (projectile.alpha > 0)
                projectile.alpha -= 5;
            else
                projectile.alpha = 0;


            if (Math.Abs(projectile.rotation - runeTargetDirection) > Math.PI)
            {
                f = -1;
            }
            else
            {
                f = 1;
            }
            runeTargetDirection = (player.Center - projectile.Center).ToRotation();
            if (projectile.rotation <= runeTargetDirection + MathHelper.ToRadians(1) && projectile.rotation >= runeTargetDirection - MathHelper.ToRadians(1))
            {
                projectile.rotation = runeTargetDirection;
            }
            else if (projectile.rotation <= runeTargetDirection)
            {
                projectile.rotation += MathHelper.ToRadians(1) * f;
            }
            else if (projectile.rotation >= runeTargetDirection)
            {
                projectile.rotation -= MathHelper.ToRadians(1) * f;
            }
            projectile.velocity = new Vector2((float)(Math.Cos(projectile.rotation) * runeSpeed), (float)(Math.Sin(projectile.rotation) * runeSpeed));




        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 360);
        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("PursuitRuneDeath"));
            }
        }
    }
}
