using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class BunnyStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Bunny");
            Tooltip.SetDefault("Turns you into a cute BUT DEADLY bunny");
        }

        public const int dmg = 18;
        public const int crt = 0;
        public const float kb = 7f;
        public const int def = 6;

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
            item.mountType = mod.MountType("BunnyShift");
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

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 16);
            recipe.AddIngredient(ItemID.Bunny);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void OnCraft(Recipe recipe)
        {
            Player player = Main.player[item.owner];
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, NPCID.Bunny);
        }
    }

    public class BunnyShiftB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bunny Shift");
            Description.SetDefault("You're a bunny");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("BunnyShift"), player);
            player.buffTime[buffIndex] = 10;
        }
    }

    public class BunnyShift : ModMountData
    {
        public override void SetDefaults()
        {
            mountData.buff = mod.BuffType("BunnyShiftB");
            mountData.spawnDust = 15;

            mountData.heightBoost = -12;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0.4f;
            mountData.runSpeed = 4f;
            mountData.dashSpeed = 6.2f;
            mountData.acceleration = 0.13f;
            mountData.jumpHeight = 15;
            mountData.jumpSpeed = 5.01f;
            mountData.totalFrames = 9;
            mountData.constantJump = true;
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
            mountData.runningFrameCount = 7;
            mountData.runningFrameDelay = 10;
            mountData.runningFrameStart = 0;
            mountData.flyingFrameCount = 0;
            mountData.flyingFrameDelay = 0;
            mountData.flyingFrameStart = 0;
            mountData.inAirFrameCount = 1;
            mountData.inAirFrameDelay = 12;
            mountData.inAirFrameStart = 4;
            mountData.idleFrameCount = 1;
            mountData.idleFrameDelay = 12;
            mountData.idleFrameStart = 4;
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
            player.GetModPlayer<BunnyControl>().controlled = true;
        }

        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
            //Main.NewText(state);
            if (mountedPlayer.GetModPlayer<BunnyControl>().kicking)
            {
                //state = 0;
                //velocity.X = 0;
                int count = 2;
                int delay = 10;
                int start = 7;
                if (state != 2)
                {
                    mountedPlayer.velocity.X = 0;
                }
                mountData.standingFrameCount = count;
                mountData.standingFrameDelay = delay;
                mountData.standingFrameStart = start;
                if (mountedPlayer.GetModPlayer<BunnyControl>().forcedRunKick)
                {
                    mountData.runningFrameCount = 1;
                    mountData.runningFrameDelay = delay;
                    mountData.runningFrameStart = start + 1;
                }
                else
                {
                    mountData.runningFrameCount = 1;
                    mountData.runningFrameDelay = delay;
                    mountData.runningFrameStart = start;
                }

                mountData.flyingFrameCount = count;
                mountData.flyingFrameDelay = delay;
                mountData.flyingFrameStart = start;
                mountData.inAirFrameCount = count;
                mountData.inAirFrameDelay = delay;
                mountData.inAirFrameStart = start;
                mountData.idleFrameCount = count;
                mountData.idleFrameDelay = delay;
                mountData.idleFrameStart = start;
                //mountedPlayer.mount._flipDraw = true;
            }
            else
            {
                mountData.standingFrameCount = 1;
                mountData.standingFrameDelay = 10;
                mountData.standingFrameStart = 0;
                mountData.runningFrameCount = 7;
                mountData.runningFrameDelay = 10;
                mountData.runningFrameStart = 0;
                mountData.flyingFrameCount = 0;
                mountData.flyingFrameDelay = 0;
                mountData.flyingFrameStart = 0;
                mountData.inAirFrameCount = 1;
                mountData.inAirFrameDelay = 12;
                mountData.inAirFrameStart = 4;
                mountData.idleFrameCount = 1;
                mountData.idleFrameDelay = 12;
                mountData.idleFrameStart = 4;
            }
            return true;
        }
    }

    public class BunnyControl : ModPlayer
    {
        public bool controlled = false;

        public override void ResetEffects()
        {
            controlled = false;
        }

        public bool kicking = false;
        public bool digging = false;
        private int kickTimer;
        public bool forcedRunKick = false;

        public override void PostUpdateMiscEffects()
        {
            if (controlled)
            {
                //player.Hitbox.Height = 30;
                player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
                Mount mount = player.mount;
                player.GetModPlayer<ShapeShifterPlayer>().morphed = true;
                //player.height = 30;
                player.noItems = true;
                player.statDefense = 6 + player.GetModPlayer<ShapeShifterPlayer>().morphDef;
                if (player.whoAmI == Main.myPlayer && Main.mouseLeft && Main.mouseLeftRelease && !kicking && !player.HasBuff(mod.BuffType("MorphSickness")) && !digging)
                {
                    kicking = true;
                    //Main.NewText("kick");
                }
                
                if (kicking)
                {
                    kickTimer++;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        // mount._flipDraw = true;
                    }
                    if (kickTimer >= 30)
                    {
                        kicking = false;
                    }
                    else if (kickTimer == 10 && player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(new Vector2(player.Center.X + player.direction * 8, player.Center.Y), Vector2.Zero, mod.ProjectileType("Kick"), (int)(BunnyStone.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), BunnyStone.kb, player.whoAmI, player.direction);
                    }
                    if (kickTimer >= 10 && kickTimer <= 20)
                    {
                        forcedRunKick = true;
                    }
                    else
                    {
                        forcedRunKick = false;
                    }
                }
                else
                {
                    kickTimer = 0;
                    forcedRunKick = false;
                    if (player.whoAmI == Main.myPlayer && Main.mouseRight)
                    {
                        Vector2 instaVel = Vector2.Zero;
                        if (player.controlDown)
                        {
                            instaVel = Vector2.UnitY;
                            
                        }
                        else if (player.controlUp)
                        {
                            instaVel = -Vector2.UnitY;
                        }
                        else if (player.controlRight)
                        {
                            instaVel = Vector2.UnitX;
                            player.direction = 1;
                        }
                        else if (player.controlLeft)
                        {
                            instaVel = -Vector2.UnitX;
                            player.direction = -1;
                        }
                        if (Collision.TileCollision(player.position, instaVel, player.width, player.height) != instaVel)
                        {
                            Main.NewText("Do Dig!");
                            //digging = true;
                            //player.position += instaVel;
                        }
                    }
                }
            }
        }
    }

    public class Kick : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kick");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 60;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 10;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            projectile.Center = new Vector2(Main.player[projectile.owner].Center.X + projectile.ai[0] * 8, Main.player[projectile.owner].Center.Y);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.player[projectile.owner].Center.X < target.Center.X)
            {
                hitDirection = 1;
            }
            else
            {
                hitDirection = -1;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[projectile.owner] = 0;
            if (!target.boss && Main.rand.Next(5) == 0)
            {
                target.AddBuff(mod.BuffType("Stunned"), 240);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }
}