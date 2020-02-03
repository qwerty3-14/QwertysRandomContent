using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            item.mountType = mod.MountType("TankMorph");
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;

        }
        public override bool UseItem(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().justStableMorphed();
            if (player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing)
            {

                player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing = false;
            }
            else
            {
                player.AddBuff(mod.BuffType("MorphSickness"), 180);
            }
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
            player.mount.SetMount(mod.MountType("TankMorph"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
    public class TankMorph : ModMountData
    {
        public override void SetDefaults()
        {

            mountData.buff = mod.BuffType("TankMorphB");
            mountData.spawnDust = 15;

            mountData.heightBoost = 0;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 4f;
            mountData.dashSpeed = 6.2f;
            mountData.acceleration = 0.06f;
            mountData.jumpHeight = 0;
            mountData.jumpSpeed = 0f;
            mountData.totalFrames = 1;
            mountData.constantJump = false;

            mountData.playerYOffsets = new int[] { 0 };
            mountData.xOffset = 0;
            mountData.bodyFrame = 1;
            mountData.yOffset = 0;
            mountData.playerHeadOffset = 0;
            mountData.standingFrameCount = 1;
            mountData.standingFrameDelay = 1;
            mountData.standingFrameStart = 0;
            mountData.runningFrameCount = 1;
            mountData.runningFrameDelay = 1;
            mountData.runningFrameStart = 0;
            mountData.flyingFrameCount = 0;
            mountData.flyingFrameDelay = 0;
            mountData.flyingFrameStart = 0;
            mountData.inAirFrameCount = 1;
            mountData.inAirFrameDelay = 1;
            mountData.inAirFrameStart = 0;
            mountData.idleFrameCount = 1;
            mountData.idleFrameDelay = 1;
            mountData.idleFrameStart = 0;
            mountData.idleFrameLoop = true;
            mountData.swimFrameCount = mountData.inAirFrameCount;
            mountData.swimFrameDelay = mountData.inAirFrameDelay;
            mountData.swimFrameStart = mountData.inAirFrameStart;

            if (Main.netMode != 2)
            {

                mountData.textureWidth = mountData.backTexture.Width;
                mountData.textureHeight = mountData.backTexture.Height;
            }
        }

        public override void UpdateEffects(Player player)
        {
            player.GetModPlayer<TankControl>().controlled = true;

        }
    }
    public class TankControl : ModPlayer
    {
        public bool controlled = false;
        public override void ResetEffects()
        {
            controlled = false;
        }
        int shotCooldown = 0;
        float flightTime = 0;
        public override void PostUpdateMiscEffects()
        {
            if (controlled)
            {
                player.noKnockback = true;
                //player.Hitbox.Height = 30;
                player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
                player.GetModPlayer<ShapeShifterPlayer>().drawTankCannon = true;
                Mount mount = player.mount;
                player.GetModPlayer<ShapeShifterPlayer>().morphed = true;
                player.GetModPlayer<ShapeShifterPlayer>().overrideWidth = 150;
                //player.height = 30;
                player.noItems = true;
                player.statDefense = 40 + player.GetModPlayer<ShapeShifterPlayer>().morphDef;

                Vector2 shootFrom = player.Top;
                shootFrom.Y -= 4;
                float pointAt = (QwertysRandomContent.LocalCursor[player.whoAmI] - shootFrom).ToRotation();
                if (QwertysRandomContent.LocalCursor[player.whoAmI].Y > player.Top.Y)
                {
                    if (QwertysRandomContent.LocalCursor[player.whoAmI].X > player.Top.X)
                    {
                        pointAt = 0;
                    }
                    else
                    {
                        pointAt = (float)Math.PI;
                    }
                }

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
                    Projectile.NewProjectile(shootFrom + QwertyMethods.PolarVector(112, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), QwertyMethods.PolarVector(16, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), mod.ProjectileType("TankCannonBallFreindly"), (int)(TankShift.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), TankShift.kb, player.whoAmI);
                }

                if (player.controlJump && player.GetModPlayer<TankComPantsEffects>().effect && flightTime < 120)
                {
                    Main.PlaySound(SoundID.Item24, player.position);
                    if (player.velocity.Y > -2)
                    {
                        player.velocity.Y = -2f;
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

                if (player.velocity.Y == 0 && player.oldVelocity.Y == 0)
                {
                    flightTime = 0;
                }
            }
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
                }
                // Fire Dust spawn
                for (int i = 0; i < 20; i++)
                {
                    int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 2f;
                    dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1f;
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
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 31, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta));

            }
            // Fire Dust spawn
            for (int i = 0; i < 20; i++)
            {
                float theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                Dust dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta));
                dustIndex.noGravity = true;
                theta = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                dustIndex = Dust.NewDustPerfect(projectile.Center, 6, QwertyMethods.PolarVector(Main.rand.NextFloat() * 4f, theta), Scale: 2f);
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