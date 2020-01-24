using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.RuneGhostItems
{


    public class PursuitScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pursuit Scroll");
            Tooltip.SetDefault("Minions ocasionaly shoot pursuit runes");

        }

        public override void SetDefaults()
        {

            item.value = 500000;
            item.rare = 9;
            item.summon = true;
            item.damage = 50;

            item.width = 54;
            item.height = 56;

            item.accessory = true;



        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();

            modPlayer.pursuitScroll = true;


        }



    }
    class PursuitRuneFreindly : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 20;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 0;
            projectile.tileCollide = true;
            projectile.timeLeft = 720;
            projectile.minion = true;


        }
        public int runeTimer;
        public NPC target;

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
                runOnce = false;
            }
            if (projectile.alpha > 0)
                projectile.alpha -= 5;
            else
                projectile.alpha = 0;

            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {



                    if (Math.Abs(projectile.rotation - runeTargetDirection) > Math.PI)
                    {
                        f = -1;
                    }
                    else
                    {
                        f = 1;
                    }
                    runeTargetDirection = (Main.npc[k].Center - projectile.Center).ToRotation();

                }
                else
                {
                    runeTargetDirection = (Main.MouseWorld - projectile.Center).ToRotation();
                }

            }
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 1200);
        }
        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("PursuitRuneDeath"));
            }
        }

    }
    public class MinionPursuit : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public int runeCounter;
        public float runeSpeed = 10;

        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (projectile.minion && projectile.minionSlots > 0 && modPlayer.pursuitScroll)
            {

                runeCounter++;
                if (runeCounter >= 180 / projectile.minionSlots)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((1 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((1 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRuneFreindly"), (int)(50 * player.minionDamage), 3f, Main.myPlayer);
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((2 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((2 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRuneFreindly"), (int)(50 * player.minionDamage), 3f, Main.myPlayer);
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((3 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((3 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRuneFreindly"), (int)(50 * player.minionDamage), 3f, Main.myPlayer);
                    runeCounter = 0;
                }

            }
            else if (projectile.minion && projectile.sentry && modPlayer.pursuitScroll)
            {
                runeCounter++;
                if (runeCounter >= 90)
                {
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((1 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((1 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRuneFreindly"), (int)(50 * player.minionDamage), 3f, Main.myPlayer);
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((2 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((2 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRuneFreindly"), (int)(50 * player.minionDamage), 3f, Main.myPlayer);
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)Math.Cos((3 * 2 * Math.PI) / 3) * runeSpeed, (float)Math.Sin((3 * 2 * Math.PI) / 3) * runeSpeed, mod.ProjectileType("PursuitRuneFreindly"), (int)(50 * player.minionDamage), 3f, Main.myPlayer);
                    runeCounter = 0;
                }
            }

        }

    }



}

