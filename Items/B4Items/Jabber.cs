using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
    public class Jabber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jabber");
            Tooltip.SetDefault("The Darth Maul of spears!");
        }

        public override void SetDefaults()
        {
            item.damage = 240;
            item.useStyle = 5;
            item.useAnimation = 35;
            item.useTime = 35;
            item.shootSpeed = 37f;
            item.knockBack = 6.5f;
            item.width = 70;
            item.height = 70;
            item.scale = 1f;
            item.value = 750000;
            item.rare = 10;
            item.crit = 60;

            item.melee = true;
            item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
            item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
            item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("JabberP");
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }

    public class JabberP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jabber");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1.3f;



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
        public float maxDistance = 1500;
        public float vel;
        public bool runOnce = true;
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
            if (runOnce && Main.netMode != 2)
            {

                Projectile.NewProjectile(ownerMountedCenter.X - (float)(projectile.width / 2), ownerMountedCenter.Y - (float)(projectile.height / 2), projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("JabberP2"), projectile.damage, projectile.knockBack, projOwner.whoAmI);
                runOnce = false;
            }
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
                    movementFactor -= movefactSpeed;
                }
                else // Otherwise, increase the movement factor
                {
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



        }

    }
    public class JabberP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jabber");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1.3f;
            projectile.alpha = 0;

            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
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
        public float maxDistance = 1500;
        public float vel;
        public bool runOnce = true;
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
                if (runOnce) // When initially thrown out, the ai0 will be 0f
                {
                    movementFactor = -(projOwner.itemAnimationMax / 2 * movefactSpeed); // Make sure the spear moves forward when initially thrown out
                    projectile.netUpdate = true; // Make sure to netUpdate this spear
                    runOnce = false;
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 2) // Somewhere along the item animation, make sure the spear moves back
                {
                    movementFactor -= movefactSpeed;
                }
                else // Otherwise, increase the movement factor
                {
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
            /*
            debugTimer++;
            if(debugTimer % 10 ==0)
            {
                CombatText.NewText(projOwner.getRect(), new Color(38, 126, 126), projOwner.itemAnimation, true, false);
            }
            */


        }

    }
}
