using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Pumpkin
{
    public class PumpGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pumpgun");
            Tooltip.SetDefault("Turns seeds into pumpkin seeds" + "\nPumpkin seeds will stick to enemies then grow into pumkin rockets" + "\nAllows for the collection of seeds");
        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.autoReuse = true;
            item.useAnimation = 25;
            item.useTime = 25;
            item.width = 38;
            item.height = 6;
            item.shoot = 10;
            item.useAmmo = AmmoID.Dart;
            item.UseSound = SoundID.Item63;
            item.damage = 11;
            item.shootSpeed = 11f;
            item.noMelee = true;
            item.value = 1000;
            item.rare = 1;
            item.knockBack = 3.5f;
            item.ranged = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pumpkin, 30);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, -4);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.Seed)
            {
                type = mod.ProjectileType("PumpkinSeed");
            }
            return true;
        }
    }

    public class PumpkinSeed : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;

            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.light = 0.5f;
            //projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 600;
            projectile.ranged = true;
            projectile.extraUpdates = 1;
        }

        // Here's an example on how you could make your AI even more readable, by giving AI fields more descriptive names
        // These are not used in AI, but it is good practice to apply some form like this to keep things organized

        // Are we sticking to a target?
        public bool isStickingToTarget
        {
            get { return projectile.ai[0] == 1f; }
            set { projectile.ai[0] = value ? 1f : 0f; }
        }

        // WhoAmI of the current target
        public float targetWhoAmI
        {
            get { return projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }

        private int d;

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit,
            ref int hitDirection)
        {
            // If you'd use the example above, you'd do: isStickingToTarget = 1f;
            // and: targetWhoAmI = (float)target.whoAmI;
            isStickingToTarget = true; // we are sticking to a target
            targetWhoAmI = (float)target.whoAmI; // Set the target whoAmI
            projectile.velocity =
                (target.Center - projectile.Center) *
                0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            projectile.netUpdate = true; // netUpdate this javelin

            projectile.penetrate = -1;
            projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore

            // The following code handles the javelin sticking to the enemy hit.
            int maxStickingJavelins = 300; // This is the max. amount of javelins being able to attach
            Point[] stickingJavelins = new Point[maxStickingJavelins]; // The point array holding for sticking javelins
            int javelinIndex = 0; // The javelin index
            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != projectile.whoAmI // Make sure the looped projectile is not the current javelin
                    && currentProjectile.active // Make sure the projectile is active
                    && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                    && currentProjectile.type == projectile.type // Make sure the projectile is of the same type as this javelin
                    && currentProjectile.ai[0] == 1f // Make sure ai0 state is set to 1f (set earlier in ModifyHitNPC)
                    && currentProjectile.ai[1] == (float)target.whoAmI
                ) // Make sure ai1 is set to the target whoAmI (set earlier in ModifyHitNPC)
                {
                    stickingJavelins[javelinIndex++] =
                        new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                    if (javelinIndex >= stickingJavelins.Length
                    ) // If the javelin's index is bigger than or equal to the point array's length, break
                    {
                        break;
                    }
                }
            }
            // Here we loop the other javelins if new javelin needs to take an older javelin's place.
            if (javelinIndex >= stickingJavelins.Length)
            {
                int oldJavelinIndex = 0;
                // Loop our point array
                for (int i = 1; i < stickingJavelins.Length; i++)
                {
                    // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                    if (stickingJavelins[i].Y < stickingJavelins[oldJavelinIndex].Y)
                    {
                        oldJavelinIndex = i; // Remember the index of the removed javelin
                    }
                }
                // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                Main.projectile[stickingJavelins[oldJavelinIndex].X].Kill();
            }
        }

        // Added these 2 constant to showcase how you could make AI code cleaner by doing this
        // Change this number if you want to alter how long the javelin can travel at a constant speed
        private const float maxTicks = 45f;

        // Change this number if you want to alter how the alpha changes
        private const int alphaReduction = 25;

        public override void AI()
        {
            if (d == 0)
            {
                d = projectile.damage;
            }
            // Slowly remove alpha as it is present
            if (projectile.alpha > 0)
            {
                projectile.alpha -= alphaReduction;
            }
            // If alpha gets lower than 0, set it to 0
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            // If ai0 is 0f, run this code. This is the 'movement' code for the javelin as long as it isn't sticking to a target
            if (!isStickingToTarget)
            {
                targetWhoAmI += 1f;
                // For a little while, the javelin will travel with the same speed, but after this, the javelin drops velocity very quickly.
                if (targetWhoAmI >= maxTicks)
                {
                    // Change these multiplication factors to alter the javelin's movement change after reaching maxTicks
                    float velXmult = 0.98f; // x velocity factor, every AI update the x velocity will be 98% of the original speed
                    float
                        velYmult = 0.35f; // y velocity factor, every AI update the y velocity will be be 0.35f bigger of the original speed, causing the javelin to drop to the ground
                    targetWhoAmI = maxTicks; // set ai1 to maxTicks continuously
                    projectile.velocity.X = projectile.velocity.X * velXmult;
                    projectile.velocity.Y = projectile.velocity.Y + velYmult;
                }
                // Make sure to set the rotation accordingly to the velocity, and add some to work around the sprite's rotation
                projectile.rotation =
                    projectile.velocity.ToRotation() +
                    MathHelper.ToRadians(
                        90f); // Please notice the MathHelper usage, offset the rotation by 135 degrees (to radians because rotation uses radians) because the sprite's rotation is not aligned!
            }
            // This code is ran when the javelin is sticking to a target
            if (isStickingToTarget)
            {
                // These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI
                projectile.ignoreWater = true; // Make sure the projectile ignores water
                projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
                int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javelin
                bool killProj = false; // if true, kill projectile at the end

                projectile.localAI[0] += 1f;

                int projTargetIndex = (int)targetWhoAmI;
                if (projectile.localAI[0] >= (float)(60 * aiFactor)// If it's time for this javelin to die, kill it
                    || (projTargetIndex < 0 || projTargetIndex >= 200)) // If the index is past its limits, kill it
                {
                    killProj = true;
                }
                else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage) // If the target is active and can take damage
                {
                    // Set the projectile's position relative to the target's center
                    projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                }
                else // Otherwise, kill the projectile
                {
                    killProj = true;
                }

                if (killProj) // Kill the projectile
                {
                    projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            if (isStickingToTarget)
            {
                Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(8, projectile.rotation + (float)Math.PI / 2), mod.ProjectileType("PumpkinRocket"), d, projectile.knockBack, projectile.owner);
            }
        }
    }

    public class PumpkinRocket : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.light = 0.5f;
            //projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 600;
            projectile.ranged = true;
            //projectile.extraUpdates = 1;
        }

        private NPC target;
        private NPC possibleTarget;
        private bool foundTarget;
        private float maxDistance = 10000f;
        private float distance;
        private int timer;
        private float speed = 8;
        private bool runOnce = true;
        private float direction;

        public override void AI()
        {
            if (runOnce)
            {
                direction = projectile.velocity.ToRotation();
                projectile.rotation = direction + ((float)Math.PI / 2);
                runOnce = false;
            }
            timer++;
            Player player = Main.player[projectile.owner];
            if (timer > 20)
            {
                projectile.friendly = true;
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                    {
                        target = Main.npc[k];
                        foundTarget = true;

                        maxDistance = (target.Center - projectile.Center).Length();
                    }
                }
                if (foundTarget)
                {
                    direction = QwertyMethods.SlowRotation(direction, (target.Center - projectile.Center).ToRotation(), 3f);
                }
                projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
                foundTarget = false;
                maxDistance = 10000f;
            }

            projectile.rotation = direction + ((float)Math.PI / 2);
        }
    }

    public class DropSeed : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Tile tile = Main.tile[i, j];
            if (tile.type == 3 || tile.type == 73)
            {
                if (WorldGen.genRand.Next(2) == 0 && (Main.player[(int)Player.FindClosest(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16)].HasItem(mod.ItemType("PumpGun"))))
                {
                    Item.NewItem(i * 16, j * 16, 16, 16, 283, 12, false, -1, false, false);
                }
            }
        }
    }
}