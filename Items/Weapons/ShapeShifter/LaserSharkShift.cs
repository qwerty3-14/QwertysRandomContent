using Microsoft.Xna.Framework;
using QwertysRandomContent.AbstractClasses;
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
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.shoot = mod.ProjectileType("LaserSharkMorph");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("LaserSharkShiftB"), 2);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
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
            if (player.GetModPlayer<ShapeShifterPlayer>().delayThing <= 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }

    public class LaserSharkMorph : StableMorph
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetSafeDefaults()
        {
            projectile.width = 120;
            projectile.height = 42;
            buffName = "LaserSharkShiftB";
            itemName = "LaserSharkShift";
        }

        public override void Effects(Player player)
        {
            player.gills = true;
        }

        private int shotCooldown = 60;
        private float frameTimer = 0;

        public override void Movement(Player player)
        {
            if (player.controlJump && projectile.wet)
            {
                projectile.velocity.Y -= .4f;
            }
            else
            {
                projectile.velocity.Y += .4f;
            }

            if (projectile.velocity.Y > 10)
            {
                projectile.velocity.Y = 10;
            }
            if (projectile.velocity.Y < -10)
            {
                projectile.velocity.Y = -10;
            }
            if (projectile.velocity.Y == 0 && !projectile.wet)
            {
                projectile.velocity.X = 0;
            }

            if (player.controlRight && projectile.wet)
            {
                projectile.velocity.X += 0.5f;
            }
            if (player.controlLeft && projectile.wet)
            {
                projectile.velocity.X -= 0.5f;
            }
            if (projectile.velocity.X > 6)
            {
                projectile.velocity.X = 6;
            }
            if (projectile.velocity.X < -6)
            {
                projectile.velocity.X = -6;
            }

            frameTimer += (float)projectile.velocity.Length();
            if (frameTimer > 64f)
            {
                frameTimer = 0;
                projectile.frame++;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }

            if (player.whoAmI == Main.myPlayer && projectile.wet && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
            {
                shotCooldown = 60;
                Projectile.NewProjectile(player.Center + Vector2.UnitX * 58 * projectile.direction, Vector2.UnitX * 12f * player.direction, mod.ProjectileType("SharkLaser"), (int)projectile.damage, projectile.knockBack, player.whoAmI);
            }
            else if (shotCooldown > 0)
            {
                shotCooldown--;
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

        private int shotCooldown = 0;

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
                    Projectile.NewProjectile(player.Center + Vector2.UnitX * 58 * player.direction, Vector2.UnitX * 12f * player.direction, mod.ProjectileType("SharkLaser"), (int)(LaserSharkShift.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), LaserSharkShift.kb, player.whoAmI);
                }
                else if (shotCooldown > 0)
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