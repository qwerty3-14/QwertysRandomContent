using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
    public class Hydrent : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Hydrent");
            Tooltip.SetDefault("Shoots hydra breath");
        }

        public override void SetDefaults()
        {
            item.damage = 38;
            item.useStyle = 5;
            item.useAnimation = 30;
            item.useTime = 30;
            item.shootSpeed = 3.7f;
            item.knockBack = 6.5f;
            item.width = 104;
            item.height = 104;
            item.scale = 1f;
            item.value = 250000;
            item.rare = 5;

            item.melee = true;
            item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
            item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
            item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("HydrentP");
        }

        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            ////

            Vector2 muzzleOffset = new Vector2(speedX, speedY).SafeNormalize(-Vector2.UnitY);
            position += new Vector2(muzzleOffset.Y * player.direction, muzzleOffset.X * -player.direction) * 1f; // change to change the height
            muzzleOffset *= 0f; // change to change the offset from the player
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
                Vector2 speed = new Vector2(speedX, speedY) * 5;
                float rot = speed.ToRotation();
                Projectile.NewProjectile(position + QwertyMethods.PolarVector(5f, rot), speed, mod.ProjectileType("HydrentBreath"), damage, knockBack, player.whoAmI);
                Projectile.NewProjectile(position + QwertyMethods.PolarVector(7f, rot + (float)Math.PI / 2), speed * 5f, mod.ProjectileType("HydrentBreath"), damage, knockBack, player.whoAmI);
                Projectile.NewProjectile(position + QwertyMethods.PolarVector(7f, rot - (float)Math.PI / 2), speed * 5f, mod.ProjectileType("HydrentBreath"), damage, knockBack, player.whoAmI);
            }

            return true;
        }
    }

    public class HydrentP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Hydrent");
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

        // In here the AI uses this example, to make the code more organized and readable
        // Also showcased in ExampleJavelinProjectile.cs
        public float movementFactor // Change this value to alter how fast the spear moves
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        // It appears that for this AI, only the ai0 field is used!
        public override void AI()
        {
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = -1;
            }
            else
            {
                projectile.spriteDirection = 1;
            }
            // Since we access the owner player instance so much, it's useful to create a helper local variable for this
            // Sadly, Projectile/ModProjectile does not have its own
            Player projOwner = Main.player[projectile.owner];
            // Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
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
                    movementFactor = 3f; // Make sure the spear moves forward when initially thrown out
                    projectile.netUpdate = true; // Make sure to netUpdate this spear
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
                {
                    movementFactor -= 2.4f;
                }
                else // Otherwise, increase the movement factor
                {
                    movementFactor += 2.1f;
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

    public class HydrentBreath : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrent Breath");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 14;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.melee = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            CreateDust();
        }

        public virtual void CreateDust()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("HydraBreathGlow"));
        }
    }
}