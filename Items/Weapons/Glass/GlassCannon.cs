using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Glass
{
    public class GlassCannonShift : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Glass cannon");
            Tooltip.SetDefault("Do I even need to explain?");

        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicGlass ? base.Texture + "_Old" : base.Texture;
        public const int dmg = 69;
        public const int crt = 0;
        public const float kb = 7f;
        public const int def = 0;
        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 42;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 10000;
            item.rare = 1;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.mountType = mod.MountType("GlassCannonMorph");
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;

        }
        public override bool CanUseItem(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().justStableMorphed();
            return base.CanUseItem(player);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 30);
            recipe.AddRecipeGroup("QwertysrandomContent:SilverBar", 6);
            recipe.AddTile(TileID.GlassKiln);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
    public class GlassCannonMorphB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Glass cannon");
            Description.SetDefault("You break easily");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("GlassCannonMorph"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
    public class GlassCannonMorph : ModMountData
    {
        
        public override void SetDefaults()
        {

            mountData.buff = mod.BuffType("GlassCannonMorphB");
            mountData.spawnDust = 15;

            mountData.heightBoost = -24;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 0f;
            mountData.dashSpeed = 0f;
            mountData.acceleration = 0f;
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
            player.GetModPlayer<GlassCannonControl>().controlled = true;

        }
    }
    public class GlassCannonControl : ModPlayer
    {
        public bool controlled = false;
        public override void ResetEffects()
        {
            controlled = false;
        }
        int shotCooldown = 0;
        public override void PostUpdateMiscEffects()
        {
            if (controlled)
            {
                player.noKnockback = true;
                //player.Hitbox.Height = 30;
                player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
                player.GetModPlayer<ShapeShifterPlayer>().glassCannon = true;
                Mount mount = player.mount;
                player.GetModPlayer<ShapeShifterPlayer>().morphed = true;
                player.GetModPlayer<ShapeShifterPlayer>().overrideWidth = 30;
                //player.height = 30;
                player.noItems = true;
                player.statDefense = 0 + player.GetModPlayer<ShapeShifterPlayer>().morphDef;

                Vector2 shootFrom = player.Top;
                //shootFrom.Y -= 4;
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
                    shotCooldown = 21;
                    Projectile.NewProjectile(shootFrom + QwertyMethods.PolarVector(30, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), QwertyMethods.PolarVector(16, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), mod.ProjectileType("GlassCannonball"), (int)(GlassCannonShift.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), GlassCannonShift.kb, player.whoAmI);
                }
            }
        }
    }
    public class GlassCannonball : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicGlass ? base.Texture + "_Old" : base.Texture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass cannon");


        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;

            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 600;



        }
        public bool runOnce = true;

        public override void AI()
        {
            //Main.NewText(projectile.damage);
            if (runOnce)
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
                for (int i = 0; i < 3; i++)
                {
                    int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= .6f;
                }

                runOnce = false;
            }
        }
        public override void Kill(int timeLeft)
        {
            int c = Main.rand.Next(2) + 2;
            for (int n = 0; n < c; n++)
            {

                Vector2 vel = QwertyMethods.PolarVector(8, Main.rand.NextFloat(-1, 1) * (float)Math.PI);
                Projectile e = Main.projectile[Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, mod.ProjectileType("GlassBulletShard"), (int)(projectile.damage * .7f), projectile.knockBack, projectile.owner)];
                e.GetGlobalProjectile<MorphProjectile>().morph = true;
                e.ranged = false;
            }
        }


    }

}