using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using QwertysRandomContent.Items.Armor.TankCommander;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class TankShift : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Tank!");
            Tooltip.SetDefault("Turn into a tank with superior offense and defense!");
        }

        public const int dmg = 89;
        public const int crt = 0;
        public const float kb = 7f;
        public const int def = 40;

        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 200000;
            item.rare = 1;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.shoot = mod.ProjectileType("TankMorph");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("TankMorphB"), 2);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool UseItem(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().justStableMorphed();

            return base.UseItem(player);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
    }

    public class TankMorphB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tank Shift");
            Description.SetDefault("You're a tank!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<ShapeShifterPlayer>().delayThing <= 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }

    public class TankMorph : StableMorph
    {
        private int shotCooldown = 0;
        private float flightTime = 0;

        public override void SetSafeDefaults()
        {
            projectile.width = 150;
            projectile.height = 42;
            buffName = "TankMorphB";
            itemName = "TankShift";
        }

        public override void Effects(Player player)
        {
            player.noKnockback = true;
        }

        public override void Movement(Player player)
        {
            Vector2 shootFrom = projectile.Top;
            shootFrom.Y -= 4;
            Vector2 LocalCursor = QwertysRandomContent.GetLocalCursor(player.whoAmI);
            float pointAt = (LocalCursor - shootFrom).ToRotation();
            if (LocalCursor.Y > projectile.Top.Y)
            {
                if (LocalCursor.X > projectile.Top.X)
                {
                    pointAt = 0;
                }
                else
                {
                    pointAt = (float)Math.PI;
                }
            }
            player.GetModPlayer<ShapeShifterPlayer>().drawTankCannon = true;
            player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation = QwertyMethods.SlowRotation(player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation, pointAt, 3);
            //Main.NewText(player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation);
            if (player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation > 0)
            {
                if (player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation > (float)Math.PI / 2)
                {
                    player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation = (float)Math.PI;
                }
                else
                {
                    player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation = 0;
                }
            }
            if (shotCooldown > 0)
            {
                shotCooldown--;
            }
            if (player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
            {
                shotCooldown = 26;
                Projectile.NewProjectile(shootFrom + QwertyMethods.PolarVector(112, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), QwertyMethods.PolarVector(16, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), mod.ProjectileType("TankCannonBallFreindly"), (int)projectile.damage, projectile.knockBack, player.whoAmI);
            }
            if (projectile.velocity.Y == .4f && projectile.oldVelocity.Y == 0)
            {
                flightTime = 0;
            }
            projectile.velocity.Y += .4f;
            if (projectile.velocity.Y > 10)
            {
                projectile.velocity.Y = 10;
            }
            if (player.controlJump && player.GetModPlayer<TankComPantsEffects>().effect && flightTime < 120)
            {
                Main.PlaySound(SoundID.Item24, player.position);
                if (projectile.velocity.Y > -2)
                {
                    projectile.velocity.Y = -2f;
                    flightTime++;
                }
                for (int num104 = 0; num104 < 2; num104++)
                {
                    int type3 = 6;
                    float scale2 = 2.5f;
                    int alpha2 = 100;

                    if (num104 == 0)
                    {
                        int num105 = Dust.NewDust(new Vector2(player.Center.X + ((player.width / 2 - 15) * player.direction), player.position.Y + (float)player.height - 10f), 8, 8, type3, 0f, 0f, alpha2, default(Color), scale2);
                        Main.dust[num105].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);
                        Main.dust[num105].noGravity = true;

                        Main.dust[num105].velocity.Y = Main.dust[num105].velocity.Y * 1f + 2f * player.gravDir - player.velocity.Y * 0.3f;
                    }
                    else
                    {
                        int num106 = Dust.NewDust(new Vector2(player.Center.X + ((player.width / 2 - 15) * -player.direction), player.position.Y + (float)player.height - 10f), 8, 8, type3, 0f, 0f, alpha2, default(Color), scale2);
                        Main.dust[num106].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, player);

                        Main.dust[num106].noGravity = true;

                        Main.dust[num106].velocity.Y = Main.dust[num106].velocity.Y * 1f + 2f * player.gravDir - player.velocity.Y * 0.3f;
                    }
                }
            }

            if (player.controlRight)
            {
                projectile.velocity.X += 0.1f;
            }
            else if (player.controlLeft)
            {
                projectile.velocity.X -= 0.1f;
            }
            else
            {
                projectile.velocity.X *= .9f;
            }
            if (projectile.velocity.X > 6)
            {
                projectile.velocity.X = 6;
            }
            if (projectile.velocity.X < -6)
            {
                projectile.velocity.X = -6;
            }
        }

        public override bool Running()
        {
            speed = 6;
            acceleration = .1f;
            jumpHeight = 0;
            jumpSpeed = 0f;

            return true;
        }

        public override bool DrawMorphExtras(SpriteBatch spriteBatch, Color lightColor)
        {
            Player drawPlayer = Main.player[projectile.owner];

            //Main.NewText("Tank!!");
            Texture2D texture = mod.GetTexture("Items/Weapons/ShapeShifter/TankMorph_Cannon");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White), 0f);
            spriteBatch.Draw(texture,
                new Vector2(projectile.position.X + 75, projectile.position.Y - 4) - Main.screenPosition,
                new Rectangle(0, 0, 130, 34),
                color12,
                drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation,
                new Vector2(18, 18),
                1f,
                0,
                0);

            return true;
        }
    }

    public class TankCannonBallFreindly : ModProjectile
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
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
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
                for (int i = 0; i < 10; i++)
                {
                    int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= .6f;
                    Main.dust[dustIndex].noGravity = true;
                }
                // Fire Dust spawn
                for (int i = 0; i < 20; i++)
                {
                    int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 2f;
                    dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1f;
                    Main.dust[dustIndex].noGravity = true;
                }
                runOnce = false;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("TankBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            e.localNPCImmunity[target.whoAmI] = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("TankBlast"), projectile.damage, projectile.knockBack, projectile.owner)];
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];

            Main.PlaySound(SoundID.Item62, projectile.position);
            for (int i = 0; i < 10; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dust = Dust.NewDustPerfect(projectile.Center, 31, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta));
                dust.noGravity = true;
            }
            // Fire Dust spawn
            for (int i = 0; i < 20; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta));
                dustIndex.noGravity = true;
                theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta), Scale: 2f);
                dustIndex.noGravity = true;
            }
        }
    }

    public class TankBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank!!");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 100;
            projectile.height = 100;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
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