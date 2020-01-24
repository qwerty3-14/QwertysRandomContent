using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Dungeon
{
    public class Hydrospear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrospear");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.useStyle = 5;
            item.useAnimation = 40;
            item.useTime = 40;
            item.shootSpeed = 37f;
            item.knockBack = 6f;
            item.width = 70;
            item.height = 70;
            item.scale = 1f;
            item.value = Item.sellPrice(silver: 54);
            item.rare = 2;


            item.melee = true;
            item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
            item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
            //item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()
            item.channel = true;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("HydrospearP");
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }

    public class HydrospearP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrospear");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1.3f;
            projectile.usesLocalNPCImmunity = true;


            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }

        // In here the AI uses this example, to make the code more organized and readable
        // Also showcased in ExampleJavelinProjectile.cs
        public float movementFactor // Change this value to alter how fast the spear moves
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        public int debugTimer;
        public float movefactSpeed = 1f;
        public float maxDistance = 750;
        public float vel;
        public bool runOnce = true;
        int streamCounter = 0;
        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {

            // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            // Sadly, Projectile/ModProjectile does not have its own
            Player projOwner = Main.player[projectile.owner];
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            vel = maxDistance / projOwner.itemAnimationMax / 2;
            projectile.velocity = new Vector2((float)Math.Cos(projectile.velocity.ToRotation()) * vel, (float)Math.Sin(projectile.velocity.ToRotation()) * vel);
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);

            // As long as the player isn't frozen, the spear can move
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
                {
                    movementFactor = 0; // Make sure the spear moves forward when initially thrown out
                    projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 2) // Somewhere along the item animation, make sure the spear moves back
                {
                    if (projOwner.channel)
                    {
                        projectile.friendly = false;
                        projOwner.itemAnimation++;
                        if (Collision.CanHit(projOwner.Center, 0, 0, projectile.Center, 0, 0))
                        {
                            streamCounter++;
                            if (streamCounter % 16 == 0)
                            {
                                //if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(180, projectile.rotation - (3 * (float)Math.PI / 4)) + QwertyMethods.PolarVector(5, projectile.rotation - (1 * (float)Math.PI / 4)), QwertyMethods.PolarVector(1, projectile.rotation - (3 * (float)Math.PI / 4)), mod.ProjectileType("HydrospearStream"), projectile.damage, projectile.knockBack, projectile.owner);
                                }
                            }
                        }
                    }
                    else
                    {
                        projectile.friendly = true;
                        movementFactor -= movefactSpeed;
                    }

                    //Main.NewText("Hi");
                }
                else // Otherwise, increase the movement factor
                {
                    projectile.friendly = true;
                    movementFactor += movefactSpeed;
                }

            }
            // Change the spear position based off of the velocity and the movementFactor
            projectile.position += projectile.velocity * movementFactor;
            // When we reach the end of the animation, we can kill the spear projectile
            if (projOwner.itemAnimation == 0)
            {
                projectile.Kill();
            }
            // Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
            // MathHelper.ToRadians(xx degrees here)
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            // Offset by 90 degrees here
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }

            Dust k = Dust.NewDustPerfect(projectile.Center + QwertyMethods.PolarVector(-4, projectile.rotation - (3 * (float)Math.PI / 4)) + QwertyMethods.PolarVector(4, projectile.rotation - (1 * (float)Math.PI / 4)), 172);
            k.velocity = Vector2.Zero;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[projectile.owner] = 0;
        }
    }
    public class HydrospearStream : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.extraUpdates = 99;
            projectile.timeLeft = 1200;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.melee = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            if (Main.rand.Next(8) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(projectile.Center, 0, 0, 172)];
                d.velocity *= .1f;
                d.noGravity = true;
                d.position = projectile.Center;
            }

        }
    }
}
