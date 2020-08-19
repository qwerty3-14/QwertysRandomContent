using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using System;
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
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.shoot = mod.ProjectileType("BunnyShift");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("BunnyShiftB"), 2);
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
            if (player.GetModPlayer<ShapeShifterPlayer>().delayThing <= 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }

    public class BunnyShift : StableMorph
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetSafeDefaults()
        {
            projectile.width = 48;
            projectile.height = 30;
            buffName = "BunnyShiftB";
            itemName = "BunnyStone";
        }

        public bool kicking = false;
        public int digging = 0;
        private int kickTimer;
        public bool forcedRunKick = false;

        public override void Effects(Player player)
        {
            projectile.frameCounter++;
            if (Math.Abs(projectile.velocity.X) > 2f)
            {
                projectile.frameCounter++;
            }
            if (projectile.frameCounter > 8)
            {
                projectile.frameCounter = 0;
                if (kicking)
                {
                    if (forcedRunKick)
                    {
                        projectile.frame = 7;
                    }
                    else
                    {
                        projectile.frame = 6;
                    }
                }
                else
                {
                    if (Math.Abs(projectile.velocity.X) < .1f)
                    {
                        projectile.frame = 0;
                    }
                    else if (projectile.velocity.Y == 0)
                    {
                        projectile.frame++;
                        if (projectile.frame >= 7)
                        {
                            projectile.frame = 0;
                        }
                    }
                    else
                    {
                        projectile.frame = 5;
                    }
                }
            }
        }

        public override void Movement(Player player)
        {
            //player.height = 30;
            player.noItems = true;
            player.statDefense = 6 + player.GetModPlayer<ShapeShifterPlayer>().morphDef;
            digging--;
            if (digging > 0)
            {
                player.gravity = 0;
                player.velocity = Vector2.Zero;
            }
            else if (player.whoAmI == Main.myPlayer && Main.mouseLeft && Main.mouseLeftRelease && !kicking && !player.HasBuff(mod.BuffType("MorphSickness")))
            {
                kicking = true;
                //Main.NewText("kick");
            }

            if (kicking)
            {
                if (projectile.velocity.X != 0)
                {
                    projectile.spriteDirection *= -1;
                }

                kickTimer++;
                if (player.whoAmI == Main.myPlayer)
                {
                    // mount._flipDraw = true;
                }
                if (kickTimer >= 30)
                {
                    kicking = false;
                    kickTimer = 0;
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
            base.Movement(player);
        }

        public override bool Running()
        {
            if (projectile.velocity.Y == 0 && forcedRunKick)
            {
                speed = .01f;
            }
            else
            {
                speed = 6;
            }

            acceleration = .3f;
            jumpHeight = 15;
            jumpSpeed = 5.01f;

            return true;
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