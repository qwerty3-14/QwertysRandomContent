using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
    public class GoldDaggerStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Dagger Staff");
            Tooltip.SetDefault("Summons a Golden Dagger to fight for you!");


        }

        public override void SetDefaults()
        {

            item.damage = 7;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = 9000;
            item.rare = 1;
            item.UseSound = SoundID.Item44;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GoldDagger");
            item.summon = true;
            item.buffType = mod.BuffType("GoldDagger");
            item.buffTime = 3600;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);   //this make so the projectile will spawn at the mouse cursor position
            position = SPos;

            return true;
        }


        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }

    }

    public class GoldDagger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Dagger");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting

        }

        public override void SetDefaults()
        {


            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 1;
            projectile.knockBack = 10f;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            projectile.usesLocalNPCImmunity = true;
        }

        int AttackMode = 1;
        const int idle = 1;
        const int charging = 2;
        const int returning = 3;

        Vector2 moveTo;
        float acceleration = .3f;
        float maxSpeed = 6f;
        int timer = 61;
        float distanceFromPlayer;
        float maxDistanceFromPlayer = 500;
        int attackTimer;
        int attackCooldown = 30;
        NPC target;

        float targetDistanceFromPlayer;
        float targetMaxDistanceFromPlayer = 400;

        float chargeSpeed = 14f;
        int chargeTime = 30;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            //Main.NewText(moveTo);
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.GoldDagger)
            {
                projectile.timeLeft = 2;
            }
            distanceFromPlayer = (player.Center - projectile.Center).Length();
            timer++;
            switch (AttackMode)
            {

                case idle:
                    projectile.tileCollide = true;
                    attackTimer++;
                    if (timer > 60)
                    {
                        if (Main.netMode != 2 && projectile.owner == Main.myPlayer)
                        {
                            projectile.ai[0] = Main.rand.Next(0, 80);
                            projectile.ai[1] = Main.rand.Next(-80, 80);
                            if (Main.netMode == 1)
                            {
                                QwertysRandomContent.ProjectileAIUpdate(projectile);
                            }
                            projectile.netUpdate = true;

                        }
                        timer = 0;
                    }
                    moveTo = new Vector2(player.Center.X + projectile.ai[1], player.Center.Y - projectile.ai[0]);
                    if (attackTimer > attackCooldown)
                    {


                        if (QwertyMethods.ClosestNPC(ref target, targetMaxDistanceFromPlayer, player.Center, false, player.MinionAttackTargetNPC))
                        {

                            chargeTime = (int)((targetMaxDistanceFromPlayer + 100) / chargeSpeed);
                            for (int k = 0; k < 200; k++)
                            {
                                projectile.localNPCImmunity[k] = 0;
                            }
                            projectile.velocity = (target.Center - projectile.Center).SafeNormalize(-Vector2.UnitY) * chargeSpeed;
                            attackTimer = 0;
                            AttackMode = charging;
                        }
                    }
                    if (distanceFromPlayer > maxDistanceFromPlayer)
                    {
                        AttackMode = returning;
                    }

                    break;
                case charging:
                    projectile.tileCollide = true;
                    attackTimer++;
                    if (attackTimer > chargeTime)
                    {
                        AttackMode = returning;
                        attackTimer = 0;
                        //projectile.velocity = Vector2.Zero;
                    }
                    break;
                case returning:
                    projectile.tileCollide = false;
                    moveTo = player.Center;
                    if (Collision.CheckAABBvAABBCollision(player.position, player.Size, projectile.position, projectile.Size))
                    {
                        AttackMode = idle;
                    }
                    break;



            }
            if (AttackMode == charging)
            {
                projectile.friendly = true;
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
            }
            else
            {
                projectile.friendly = false;
                projectile.velocity += (moveTo - projectile.Center).SafeNormalize(-Vector2.UnitY) * acceleration;
                if (projectile.velocity.Length() > maxSpeed)
                {
                    projectile.velocity = (moveTo - projectile.Center).SafeNormalize(-Vector2.UnitY) * maxSpeed;
                }
                projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, projectile.velocity.ToRotation() + (float)Math.PI / 2, 3);
            }


            //projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;

        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            for (int k = 0; k < 200; k++)
            {
                projectile.localNPCImmunity[k] = 0;
            }
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            for (int k = 0; k < 200; k++)
            {
                projectile.localNPCImmunity[k] = 0;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }


    }

}