using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
    public class HolyExiler : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holy Exiler");
            Tooltip.SetDefault("Higher beings will help you shoot your enemies!");

        }
        public override void SetDefaults()
        {
            item.damage = 16;
            item.ranged = true;

            item.useTime = 34;
            item.useAnimation = 34;

            item.useStyle = 5;
            item.knockBack = 2f;
            item.value = 50000;
            item.rare = 3;
            item.UseSound = SoundID.Item5;

            item.width = 32;
            item.height = 62;

            item.shoot = 40;
            item.useAmmo = 40;
            item.shootSpeed = 12f;
            item.noMelee = true;
            item.autoReuse = true;


        }

        public Projectile arrow;
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            //Vector2 trueSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
            arrow = Main.projectile[Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI)];
            arrow.GetGlobalProjectile<ArrowWarping>().warpedArrow = true;
            return false;
        }


    }

    public class ArrowWarping : GlobalProjectile
    {
        public bool warpedArrow;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        NPC target;
        NPC possibleTarget;
        List<int> targets = new List<int>();
        float maxDistance = 300;
        Projectile portal1;
        Projectile portal2;
        float teleportDistance = 80;
        int teleportTries = 100;
        public override void Kill(Projectile projectile, int timeLeft)
        {
            if (warpedArrow && projectile.type != mod.ProjectileType("HydraArrowP"))
            {
                for (int n = 0; n < 200; n++)
                {
                    possibleTarget = Main.npc[n];
                    float distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal)
                    {
                        targets.Add(n); //save valid possibletarget's id to the targets list
                    }
                }
                if (targets.Count > 0) //only run if a vallid target has been found
                {
                    target = Main.npc[targets[Main.rand.Next(targets.Count)]]; // pick a random value in the targets list and use that to pick a target
                    for (int c = 0; c < teleportTries; c++)
                    {

                        if (Main.netMode != 1) // don't run on client
                        {
                            //Use the npc.ai[0] variable as it's easy to sync in multiplayer
                            //server sync whenever randomizing so the client and server won't disagree
                            projectile.ai[0] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI); // sets the npc.ai[0] variable to a random radian angle
                            projectile.netUpdate = true; // update the client's npc.ai[0] variable  to be equal to the server's
                        }
                        Vector2 teleTo = new Vector2(target.Center.X + (float)Math.Cos(projectile.ai[0]) * teleportDistance, target.Center.Y + (float)Math.Sin(projectile.ai[0]) * teleportDistance);
                        if (Collision.CanHit(new Vector2(teleTo.X - projectile.width / 2, teleTo.Y - projectile.height / 2), projectile.width, projectile.height, target.position, target.width, target.height))// checks if there are no tiles between player and potential teleport spot
                        {
                            portal1 = Main.projectile[Projectile.NewProjectile(teleTo, Vector2.Zero, mod.ProjectileType("ArrowPortal"), projectile.damage, projectile.knockBack, projectile.owner, projectile.type, projectile.velocity.Length())];
                            portal1.rotation = (target.Center - teleTo).ToRotation();
                            portal1.timeLeft = 30;
                            break; //end for loop

                        }
                    }
                    target = Main.npc[targets[Main.rand.Next(targets.Count)]]; // pick a random value in the targets list and use that to pick a target
                    for (int c = 0; c < teleportTries; c++)
                    {

                        if (Main.netMode != 1) // don't run on client
                        {
                            //Use the npc.ai[0] variable as it's easy to sync in multiplayer
                            //server sync whenever randomizing so the client and server won't disagree
                            projectile.ai[0] = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI); // sets the npc.ai[0] variable to a random radian angle
                            projectile.netUpdate = true; // update the client's npc.ai[0] variable  to be equal to the server's
                        }
                        Vector2 teleTo = new Vector2(target.Center.X + (float)Math.Cos(projectile.ai[0]) * teleportDistance, target.Center.Y + (float)Math.Sin(projectile.ai[0]) * teleportDistance);
                        if (Collision.CanHit(new Vector2(teleTo.X - projectile.width / 2, teleTo.Y - projectile.height / 2), projectile.width, projectile.height, target.position, target.width, target.height))// checks if there are no tiles between player and potential teleport spot
                        {
                            portal2 = Main.projectile[Projectile.NewProjectile(teleTo, Vector2.Zero, mod.ProjectileType("ArrowPortal"), projectile.damage, projectile.knockBack, projectile.owner, projectile.type, projectile.velocity.Length())];
                            portal2.rotation = (target.Center - teleTo).ToRotation();
                            portal2.timeLeft = 45;
                            break; //end for loop

                        }
                    }
                }
            }
        }


    }
    public class ArrowPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.light = .5f;
        }
        int activeTime = 30;
        public override void AI()
        {

            if (projectile.timeLeft < activeTime)
            {
                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
                dust.scale = .5f;
                //dust.velocity =Vector2.Zero;
                //dust.frame.Y = 0;
                if (projectile.timeLeft < activeTime / 5)
                {
                    projectile.frame = 0;
                }
                else if (projectile.timeLeft < 2 * (activeTime / 5))
                {
                    projectile.frame = 1;
                }
                else if (projectile.timeLeft < 3 * (activeTime / 5))
                {
                    projectile.frame = 2;
                }
                else if (projectile.timeLeft < 4 * (activeTime / 5))
                {
                    projectile.frame = 1;
                }
                else
                {
                    projectile.frame = 0;
                }
                if (projectile.timeLeft == activeTime / 2)
                {
                    Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(projectile.ai[1], projectile.rotation), (int)projectile.ai[0], projectile.damage, projectile.knockBack, projectile.owner);
                }
                projectile.alpha = 0;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                /*
                Dust dust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, mod.DustType("CaeliteDust"))];
                dust.velocity *= 3;
                */
            }
        }
    }



}

