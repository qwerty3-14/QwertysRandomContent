using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TundraBossItems
{
    public class PenguinLauncher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penguin Launcher");
            Tooltip.SetDefault("Uses penguins as ammo");

        }
        public override void SetDefaults()
        {
            item.damage = 30;
            item.ranged = true;

            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = 100000;
            item.rare = 1;
            item.UseSound = SoundID.Item11;

            item.width = 82;
            item.height = 34;

            item.shoot = mod.ProjectileType("SlidingPenguin");
            item.useAmmo = ItemID.Penguin;
            item.shootSpeed = 6;
            item.noMelee = true;
            item.autoReuse = true;


        }


        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-18, -1);
        }







    }
    public class PenguinAmmo : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Penguin)
            {
                item.ammo = ItemID.Penguin;
                item.shoot = mod.ProjectileType("SlidingPenguin");

            }
        }
    }
    public class SlidingPenguin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cryonic BoltP");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            //aiType = ProjectileID.Bullet;
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;



        }
        bool runOnce = true;
        float initVel;
        bool hitGround;
        int timer;
        public override void AI()
        {
            
            
            projectile.spriteDirection = -(int)(projectile.velocity.X * Math.Abs(1f / projectile.velocity.X));
            if (runOnce)
            {
                initVel = (float)Math.Abs(projectile.velocity.Length());
                
                runOnce = false;
            }
            if (hitGround)
            {
                timer++;
                if (timer > 120)
                {
                    initVel -= .3f;
                    if (Math.Abs(projectile.velocity.X) < 1f)
                    {
                        projectile.friendly = false;
                        NPC Penguin = Main.npc[NPC.NewNPC((int)projectile.Top.X, (int)projectile.Top.Y, NPCID.Penguin)];
                        if(projectile.ai[1]==1)
                        {
                            Penguin.SpawnedFromStatue = true;
                        }
                        projectile.Kill();
                    }

                }
                if (projectile.velocity.X < 0)
                {
                    projectile.velocity.X = -initVel;

                }
                else
                {
                    projectile.velocity.X = initVel;
                }
            }
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].type == NPCID.Penguin || Main.npc[n].type == NPCID.PenguinBlack)
                {
                    Main.npc[n].immune[projectile.owner] = 0;
                    projectile.localNPCImmunity[n] = 10;
                }
            }
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[projectile.owner] = 0;
            

        }
        public override bool OnTileCollide(Vector2 velocityChange)
        {
            hitGround = true;
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }

            return false;
        }
    }

}

