using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class LaserSharkShift : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Laser shark!");
            Tooltip.SetDefault("I have one simple request and that is sharks with fricken' laser beams attached to their heads!");

        }
        public const int dmg = 100;
        public const int crt = 0;
        public const float kb = 2f;
        public const int def = 2;
        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = 200;
            item.rare = 1;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.mountType = mod.MountType("LaserSharkMorph");
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

            return base.UseItem(player);
        }
    }
    public class LaserSharkShiftB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Laser Shark");
            Description.SetDefault("One simple request...");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("LaserSharkMorph"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
    public class LaserSharkMorph : ModMountData
    {
        public override void SetDefaults()
        {

            mountData.buff = mod.BuffType("LaserSharkShiftB");
            mountData.spawnDust = 15;

            mountData.heightBoost = 0;
            mountData.constantJump = false;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 1f;
            mountData.runSpeed = 2f;
            mountData.dashSpeed = 2f;
            mountData.swimSpeed = 6f;
            mountData.acceleration = 0.08f;
            mountData.jumpHeight = 10;
            mountData.jumpSpeed = 3.15f;

            mountData.swimSpeed = 10f;
            mountData.totalFrames = 4;
            int[] array = new int[mountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 0;
            }
            mountData.playerYOffsets = array;
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
            mountData.swimFrameCount = 4;
            mountData.swimFrameDelay = 30;
            mountData.swimFrameStart = 0;
            mountData.idleFrameLoop = true;

            if (Main.netMode != 2)
            {

                mountData.textureWidth = mountData.backTexture.Width;
                mountData.textureHeight = mountData.backTexture.Height;
            }
        }

        public override void UpdateEffects(Player player)
        {
            player.GetModPlayer<SharkControl>().controlled = true;
            if(player.mount._frameState==4)
            {
                mountData.fallDamage = 0f;
                mountData.runSpeed = mountData.swimSpeed;
                mountData.dashSpeed = 4f;
                mountData.jumpHeight = 10;
                mountData.jumpSpeed = 5f;
                //player.jump = 100;
            }
            else
            {
                mountData.fallDamage = 1f;
                mountData.runSpeed = 0;
                mountData.dashSpeed = 0;
                mountData.jumpHeight = 10;
                mountData.jumpSpeed = 5f;
            }
        }
    }
    public class SharkControl : ModPlayer
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
                player.accFlipper = true;
                player.gills = true;
                player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
                Mount mount = player.mount;
                player.GetModPlayer<ShapeShifterPlayer>().morphed = true;
                player.GetModPlayer<ShapeShifterPlayer>().overrideWidth = 120;
                player.noItems = true;
                player.statDefense = 2 + player.GetModPlayer<ShapeShifterPlayer>().morphDef;
                if (player.whoAmI == Main.myPlayer && player.wet && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
                {
                    shotCooldown = 60;
                    Projectile.NewProjectile(player.Center + Vector2.UnitX * 58 *player.direction, Vector2.UnitX * 12f * player.direction, mod.ProjectileType("SharkLaser"), (int)(LaserSharkShift.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), LaserSharkShift.kb, player.whoAmI);
                }
                else if(shotCooldown>0)
                {
                    shotCooldown--;
                }
            }
        }
    }
    public class SharkLaser : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.light = 0.75f;
            projectile.alpha = 255;
            projectile.extraUpdates = 2;
            projectile.scale = 1.2f;
            projectile.timeLeft = 600;
            aiType = ProjectileID.PinkLaser;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
        }
        
    }
}
