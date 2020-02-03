using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
    public class CaeliteBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angelic Tracker");
            Tooltip.SetDefault("Higher beings will guide your boomerang!");


        }
        public override void SetDefaults()
        {
            item.damage = 38;
            item.melee = true;
            item.noMelee = true;

            item.useTime = 42;
            item.useAnimation = 42;
            item.useStyle = 5;
            item.knockBack = 0;
            item.value = 50000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.width = 18;
            item.height = 32;

            item.autoReuse = true;
            item.shoot = mod.ProjectileType("CaeliteBoomerangP");
            item.shootSpeed = 15;
            item.channel = true;




        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class CaeliteBoomerangP : ModProjectile
    {
        public override void SetDefaults()
        {
            //projectile.aiStyle = ProjectileID.WoodenBoomerang;
            //aiType = 52;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 18;
            projectile.height = 32;
            projectile.melee = true;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caelite Boomerang");

        }
        float speed;
        float maxSpeed;
        bool runOnce = true;
        float decceleration = 1f / 3f;
        int spinDirection;
        bool returnToPlayer;
        NPC ConfirmedTarget;
        NPC possibleTarget;
        float distance;
        float maxDistance = 300;
        bool foundTarget;
        int timerAfterReturning;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                spinDirection = player.direction;
                speed = projectile.velocity.Length();
                maxSpeed = speed;
                runOnce = false;
            }
            projectile.rotation += MathHelper.ToRadians(maxSpeed * spinDirection);
            if (returnToPlayer)
            {
                timerAfterReturning++;
                if (timerAfterReturning == 30)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        projectile.localNPCImmunity[k] = 0;
                    }
                }

                if (Collision.CheckAABBvAABBCollision(player.position, player.Size, projectile.position, projectile.Size))
                {
                    projectile.Kill();
                }
                projectile.tileCollide = false;
                //projectile.friendly = false;
                projectile.velocity = QwertyMethods.PolarVector(speed, (player.Center - projectile.Center).ToRotation());
                speed += decceleration;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            else
            {

                projectile.velocity = projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                speed -= decceleration;
                if (speed < 1f)
                {
                    returnToPlayer = true;
                }
            }
            //Main.NewText("MaxSpeed: " + maxSpeed);
            //Main.NewText("speed: " + speed);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0)
            {
                target.AddBuff(mod.BuffType("PowerDown"), 120);
            }
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            if (!returnToPlayer)
            {

                Player player = Main.player[projectile.owner];
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && projectile.localNPCImmunity[k] >= 0 && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && Collision.CanHit(projectile.Center, 0, 0, possibleTarget.Center, 0, 0))
                    {
                        ConfirmedTarget = Main.npc[k];
                        foundTarget = true;


                        maxDistance = (ConfirmedTarget.Center - projectile.Center).Length();
                    }

                }
                if (foundTarget)
                {
                    projectile.velocity = QwertyMethods.PolarVector(maxSpeed, (ConfirmedTarget.Center - projectile.Center).ToRotation());
                    speed = maxSpeed;
                }
                else
                {
                    returnToPlayer = true;
                }
                foundTarget = false;
                maxDistance = 300;
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            returnToPlayer = true;
            return false;
        }
    }


}

