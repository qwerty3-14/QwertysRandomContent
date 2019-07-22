using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace QwertysRandomContent.Items.Weapons.Meteor
{
    public class HovercraftShift : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Hovercraft!");
            Tooltip.SetDefault("Turn into a hovercraft with decent mobility and firepower");

        }
        public const int dmg = 22;
        public const int crt = 0;
        public const float kb = 1f;
        public const int def = 14;
        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.value = Item.sellPrice(silver: 40);
            item.rare = 1;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.mountType = mod.MountType("HovercraftMorph");
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
            if (player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing)
            {
                player.GetModPlayer<ShapeShifterPlayer>().EyeBlessing = false;
            }
            else
            {
                player.AddBuff(mod.BuffType("MorphSickness"), 180);
            }
            return base.CanUseItem(player);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
    public class HovercraftMorphB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hovercraft shift");
            Description.SetDefault("");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType<HovercraftMorph>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
    public class HovercraftMorph : ModMountData
    {
        public override void SetDefaults()
        {

            mountData.buff = mod.BuffType("HovercraftMorphB");
            mountData.spawnDust = 15;

            mountData.heightBoost = -20;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 4f;
            mountData.dashSpeed = 6.2f;
            mountData.acceleration = 2f;
            mountData.jumpHeight = 0;
            mountData.jumpSpeed = 0f;
            mountData.totalFrames = 2;
            mountData.constantJump = false;

            mountData.playerYOffsets = new int[] { 0, 0 };
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
            player.GetModPlayer<HovercraftControl>().controlled = true;
        }
        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
            if(Math.Abs(velocity.X)<2f )
            {
                mountData.standingFrameStart = 0;
                mountData.runningFrameStart = 0;
                mountData.flyingFrameStart = 0;
                mountData.inAirFrameStart = 0;
                mountData.idleFrameStart = 0;
            }
            else
            {
                mountData.standingFrameStart = 1;
                mountData.runningFrameStart = 1;
                mountData.flyingFrameStart = 1;
                mountData.inAirFrameStart = 1;
                mountData.idleFrameStart = 1;
                Dust d = Dust.NewDustPerfect(mountedPlayer.Center + new Vector2(-mountedPlayer.width / 2 * mountedPlayer.direction, 2+ Main.rand.Next(-3,3)), 6);
                d.noGravity = true;
                d.noLight = true;
            }
        
            return true;
        }

    }
    public class HovercraftControl : ModPlayer
    {
        public bool controlled = false;
        public override void ResetEffects()
        {
            controlled = false;
        }
        int shotCooldown = 0;
        float hoverHeight = 80;
        float hoverTo = 0;
        float hoverDrift = 0f;
        float currentHeight = 0;
        const int maxHoverHeight = 160;
        const int minHoverHeight = 20;
        float hoverSpeed = 3f;
        public override void PostUpdateMiscEffects()
        {
            if (controlled)
            {
                player.noFallDmg = true;

                player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;

                player.GetModPlayer<ShapeShifterPlayer>().hovercraft = true;
                Mount mount = player.mount;
                player.GetModPlayer<ShapeShifterPlayer>().morphed = true;
                player.GetModPlayer<ShapeShifterPlayer>().overrideWidth = 40;
                //player.height = 30;
                player.noItems = true;
                hoverDrift += (float)Math.PI / 60;
                player.statDefense = 14 + player.GetModPlayer<ShapeShifterPlayer>().morphDef;
                if ((player.controlUp || player.controlJump) && hoverHeight < maxHoverHeight)
                {
                    hoverHeight++;
                }
                else if (player.controlDown && hoverHeight > minHoverHeight)
                {
                    hoverHeight--;
                }
                for (; currentHeight < (maxHoverHeight + 10); currentHeight++)
                {
                    if (!Collision.CanHit(player.Center, 0, 0, player.Center + new Vector2(0, currentHeight), 0, 0))
                    {
                        break;
                    }
                }
                hoverTo = hoverHeight + (float)Math.Sin(hoverDrift) * 16;
                //player.velocity.Y = ( currentHeight-hoverHeight) * .1f;
                if (Math.Abs(currentHeight - hoverTo) > hoverSpeed * 4)
                {
                    if (currentHeight - hoverTo > 0)
                    {
                        player.velocity.Y = hoverSpeed;
                        hoverSpeed = 6f;
                    }
                    if (currentHeight - hoverTo < 0)
                    {
                        player.velocity.Y = -hoverSpeed;
                        hoverSpeed = 6f;
                    }
                }
                else
                {
                    player.velocity.Y = (currentHeight - hoverTo) * .1f;
                }

                Vector2 shootFrom = player.Top;
                shootFrom.Y += 8;
                float pointAt = (QwertysRandomContent.LocalCursor[player.whoAmI] - shootFrom).ToRotation();


                player.GetModPlayer<ShapeShifterPlayer>(mod).tankCannonRotation = QwertyMethods.SlowRotation(player.GetModPlayer<ShapeShifterPlayer>(mod).tankCannonRotation, pointAt, 3);
                //Main.NewText(player.GetModPlayer<ShapeShifterPlayer>(mod).tankCannonRotation);

                if (shotCooldown > 0)
                {
                    shotCooldown--;
                }
                if (player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
                {
                    shotCooldown = 12;
                    Projectile p = Main.projectile[Projectile.NewProjectile(shootFrom + QwertyMethods.PolarVector(19, player.GetModPlayer<ShapeShifterPlayer>(mod).tankCannonRotation), QwertyMethods.PolarVector(12, player.GetModPlayer<ShapeShifterPlayer>(mod).tankCannonRotation), ProjectileID.GreenLaser, (int)(HovercraftShift.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), HovercraftShift.kb, player.whoAmI)];
                    p.magic = false;
                    p.GetGlobalProjectile<MorphProjectile>().morph = true;
                    p.penetrate = 1;
                    p.alpha = 0;
                    if (Main.netMode == 1)
                    {
                        QwertysRandomContent.UpdateProjectileClass(p);
                    }
                    Main.PlaySound(SoundID.Item12, player.Center);
                }

                currentHeight = 0; //reset
                hoverTo = 0;
                hoverSpeed = 3f;
            }
        }
    }

}