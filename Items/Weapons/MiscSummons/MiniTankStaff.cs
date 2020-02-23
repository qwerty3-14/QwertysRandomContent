using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
    public class MiniTankStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Tank Staff");
            Tooltip.SetDefault("Summons a Mini Tank!");


        }

        public override void SetDefaults()
        {

            item.damage = 24;
            item.mana = 20;
            item.width = 38;
            item.height = 38;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 5f;
            item.value = 200000;
            item.rare = 3;
            item.UseSound = SoundID.Item44;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("MiniTank");
            item.summon = true;
            item.buffType = mod.BuffType("MiniTank");
            item.buffTime = 3600;
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

    public class MiniTank : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Tank");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting

        }

        public override void SetDefaults()
        {


            projectile.width = 60;
            projectile.height = 24;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 1;
            projectile.knockBack = 10f;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            //projectile.usesLocalprojectileImmunity = true;
        }
        int tankCount = 0;
        int identity = 0;
        const float terminalVelocity = 10;
        const float garvityAcceleration = .2f;
        float gotoX;
        const float spacing = 70;
        const float maxSpeedX = 8;
        bool returnToPlayer;
        float gunRotation = 0;
        NPC target;
        float aim;
        int shootCounter = 0;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.miniTank)
            {
                projectile.timeLeft = 2;
            }
            tankCount = player.ownedProjectileCounts[mod.ProjectileType("MiniTank")];
            identity = 0;
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == mod.ProjectileType("MiniTank") && Main.projectile[p].active)
                {
                    if (p == projectile.whoAmI)
                    {
                        break;
                    }
                    else
                    {
                        identity++;
                    }
                }
            }
            if (returnToPlayer)
            {
                projectile.velocity = (player.Top - projectile.Center) * .1f;
                if ((player.Top - projectile.Center).Length() < 200)
                {
                    returnToPlayer = false;
                }
                projectile.tileCollide = false;
            }
            else
            {
                if ((player.Center - projectile.Center).Length() > 1500)
                {
                    returnToPlayer = true;
                }
                projectile.tileCollide = true;


                if (projectile.velocity.Y < terminalVelocity)
                {
                    projectile.velocity.Y += garvityAcceleration;
                }
                gotoX = player.Center.X + -player.direction * (player.width / 2 + spacing + (identity * spacing));
                projectile.velocity.X = (gotoX - projectile.Center.X) * .1f;
                if (Math.Abs(projectile.velocity.X) > maxSpeedX)
                {
                    projectile.velocity.X = projectile.velocity.X > 0 ? maxSpeedX : -maxSpeedX;
                }

                projectile.spriteDirection = projectile.velocity.X > 0 ? 1 : -1;
                if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Top, false, player.MinionAttackTargetNPC))
                {
                    if (target.Center.Y > projectile.Top.Y)
                    {
                        if (target.Center.X > projectile.Top.X)
                        {
                            aim = 0;
                        }
                        else
                        {
                            aim = (float)Math.PI;
                        }
                    }
                    else
                    {
                        aim = (target.Center - projectile.Top).ToRotation();
                    }
                    if (shootCounter >= 60)
                    {
                        shootCounter = 0;
                        Projectile.NewProjectile(projectile.Top + QwertyMethods.PolarVector(30, gunRotation), QwertyMethods.PolarVector(10, gunRotation), mod.ProjectileType("MiniTankCannonBallFreindly"), projectile.damage, projectile.knockBack, player.whoAmI);
                    }
                }
                else
                {
                    aim = projectile.spriteDirection == 1 ? 0 : (float)Math.PI;
                }
                shootCounter++;
                gunRotation = QwertyMethods.SlowRotation(gunRotation, aim, 3);
                if (gunRotation > 0)
                {
                    if (gunRotation > (float)Math.PI / 2)
                    {
                        gunRotation = (float)Math.PI;
                    }
                    else
                    {
                        gunRotation = 0;
                    }
                }
                //projectile.velocity = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, player.Center.Y > projectile.Bottom.Y, player.Center.Y > projectile.Bottom.Y);
            }
        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Point origin = new Vector2(projectile.Center.X + (projectile.width / 2 - 6) * projectile.spriteDirection, projectile.Bottom.Y).ToTileCoordinates();
            Point point;
            if ((oldVelocity.X != projectile.velocity.X) && WorldUtils.Find(origin, Searches.Chain(new Searches.Down(1), new GenCondition[]
                                            {
                                            new Conditions.IsSolid()
                                            }), out point))
            {
                projectile.velocity.Y = -6;
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = Main.player[projectile.owner].Center.Y - projectile.Center.Y > 64;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscSummons/MiniTankGun");
            spriteBatch.Draw(texture, projectile.Top - Main.screenPosition,
                        new Rectangle(0, 0, 40, 20), lightColor, gunRotation,
                        new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
            return true;
        }



    }
    public class MiniTankCannonBallFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank!!");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 600;
            projectile.usesLocalNPCImmunity = true;


        }
        public bool runOnce = true;

        public override void AI()
        {
            //Main.NewText(projectile.damage);
            if (runOnce)
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
                for (int i = 0; i < 4; i++)
                {
                    int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndex].velocity *= .6f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 8; i++)
                {
                    int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 2f;
                    dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndex].velocity *= 1f;
                }
                runOnce = false;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MiniTankBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("MiniTankBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];


            Main.PlaySound(SoundID.Item62, projectile.position);
            for (int i = 0; i < 10; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 31, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta), Scale: .5f);

            }
            // Fire Dust spawn
            for (int i = 0; i < 20; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta), Scale: .5f);
                dustIndex.noGravity = true;
                theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 2f, theta), Scale: 1f);
            }
        }
    }
    public class MiniTankBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank!!");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.minion = true;
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

    }

}