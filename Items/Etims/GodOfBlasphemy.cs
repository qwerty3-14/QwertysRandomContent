using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    
    public class GodOfBlasphemy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shifte: God of Blasphemy");
            Tooltip.SetDefault("");

        }
        public const int dmg = 48;
        public const int crt = 0;
        public const float kb = 1f;
        public const int def = 13;
        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 50;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 200;
            item.rare = 1;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.mountType = mod.MountType("GodOfBlasphemyShift");
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;

        }
        public override bool CanUseItem(Player player)
        {
            

            return base.CanUseItem(player);
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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("Etims"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    public class GodOfBlasphemyB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("God of Blasphemy");
            Description.SetDefault("");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("GodOfBlasphemyShift"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
    public class GodOfBlasphemyShift : ModMountData
    {
        public override void SetDefaults()
        {

            mountData.buff = mod.BuffType("GodOfBlasphemyB");
            mountData.spawnDust = 15;

            mountData.heightBoost = 86;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 0f;
            mountData.dashSpeed = 0f;
            mountData.acceleration = 0f;
            mountData.jumpHeight = 0;
            mountData.jumpSpeed = 0f;
            mountData.totalFrames = 1;
            mountData.constantJump = false;

            int[] array = new int[mountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 0;
            }
            mountData.playerYOffsets = array;
            mountData.xOffset = 0;
            mountData.bodyFrame = 1;
            mountData.yOffset = 2;
            mountData.playerHeadOffset = 0;
            mountData.standingFrameCount = 1;
            mountData.standingFrameDelay = 10;
            mountData.standingFrameStart = 0;
            mountData.runningFrameCount = 1;
            mountData.runningFrameDelay = 10;
            mountData.runningFrameStart = 0;
            mountData.flyingFrameCount = 1;
            mountData.flyingFrameDelay = 0;
            mountData.flyingFrameStart = 0;
            mountData.inAirFrameCount = 1;
            mountData.inAirFrameDelay = 12;
            mountData.inAirFrameStart = 0;
            mountData.idleFrameCount = 1;
            mountData.idleFrameDelay = 12;
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
            player.GetModPlayer<MorphFlightControl>().controlled = true;
            
        }

    }
    
    public class MorphFlightControl : ModPlayer
    {
        float flySpeed = 6.2f;
        int shotCooldown = 0;
        public bool controlled = false;
        public override void ResetEffects()
        {
            controlled = false;
        }
        float pupilDirection = 0f;
        float greaterPupilRadius = 18;
        float lesserPupilRadius = 6;
        public float scale = 1f;
        public Vector2 pupilPosition;
        public override void PostUpdateMiscEffects()
        {

            float pupilStareOutAmount = (QwertysRandomContent.LocalCursor[player.whoAmI] - player.Center).Length() / 300f;
            if (pupilStareOutAmount > 1f)
            {
                pupilStareOutAmount = 1f;
            }
            scale = 1f + .05f * (float)Math.Sin(player.GetModPlayer<ShapeShifterPlayer>().pulseCounter);
            pupilDirection = (QwertysRandomContent.LocalCursor[player.whoAmI] - player.Center).ToRotation();
            pupilPosition = new Vector2((float)Math.Cos(pupilDirection) * greaterPupilRadius * pupilStareOutAmount, (float)Math.Sin(pupilDirection) * lesserPupilRadius) * scale;
            if (controlled)
            {
                player.nightVision = true;
                player.GetModPlayer<ShapeShifterPlayer>().drawGodOfBlasphemy = true;
                player.noFallDmg = true;
                player.gravity = 0;
                player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
                Mount mount = player.mount;
                player.GetModPlayer<ShapeShifterPlayer>().morphed = true;
                player.GetModPlayer<ShapeShifterPlayer>().overrideWidth = 166;
                player.noItems = true;
                player.statDefense = GodOfBlasphemy.def + player.GetModPlayer<ShapeShifterPlayer>().morphDef;
                player.velocity = Vector2.Zero;
                if(player.controlUp)
                {
                    player.velocity.Y += -1;
                }
                if (player.controlDown)
                {
                    player.velocity.Y += 1;
                }
                if (player.controlLeft)
                {
                    player.velocity.X += -1;
                }
                if (player.controlRight)
                {
                    player.velocity.X += 1;
                }
                if (shotCooldown > 0)
                {
                    shotCooldown--;
                }
                if (player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
                {
                    shotCooldown = 20;
                    Projectile p = Main.projectile[Projectile.NewProjectile(player.Center + pupilPosition, QwertyMethods.PolarVector(10, (QwertysRandomContent.LocalCursor[player.whoAmI]-player.Center).ToRotation()), mod.ProjectileType("EtimsicRayFreindly"), (int)(GodOfBlasphemy.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), GodOfBlasphemy.kb, player.whoAmI)];

                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PewPew").WithVolume(3f).WithPitchVariance(.5f), player.Center);
                }
                if (player.velocity.Length() > 0)
                {
                    player.velocity = player.velocity.SafeNormalize(-Vector2.UnitY);
                    player.velocity *= flySpeed;
                }

            }
        }
    }
    public class EtimsicRayFreindly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etimsic Ray");
        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.light = 1f;
            projectile.penetrate = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
            projectile.GetGlobalProjectile<Etims>().effect = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }


    }

}
