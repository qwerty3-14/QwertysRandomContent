using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.shoot = mod.ProjectileType("HovercraftMorph");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("HovercraftMorphB"), 2);
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
            if (player.GetModPlayer<ShapeShifterPlayer>().delayThing <= 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }

    public class HovercraftMorph : StableMorph
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetSafeDefaults()
        {
            projectile.width = 40;
            projectile.height = 22;
            buffName = "HovercraftMorphB";
            itemName = "HovercraftShift";
        }

        private int shotCooldown = 0;
        private float hoverHeight = 80;
        private float hoverTo = 0;
        private float hoverDrift = 0f;
        private float currentHeight = 0;
        private const int maxHoverHeight = 160;
        private const int minHoverHeight = 20;
        private float hoverSpeed = 3f;

        public override void Movement(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().hovercraft = true;
            hoverDrift += (float)Math.PI / 60;
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
                if (!Collision.CanHit(projectile.Bottom, 0, 0, projectile.Center + new Vector2(0, currentHeight), 0, 0) || !Collision.CanHit(projectile.BottomLeft, 0, 0, projectile.Center + new Vector2(0, currentHeight), 0, 0) || !Collision.CanHit(projectile.BottomRight, 0, 0, projectile.Center + new Vector2(0, currentHeight), 0, 0))
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
                    projectile.velocity.Y = hoverSpeed;
                    hoverSpeed = 6f;
                }
                if (currentHeight - hoverTo < 0)
                {
                    projectile.velocity.Y = -hoverSpeed;
                    hoverSpeed = 6f;
                }
            }
            else
            {
                projectile.velocity.Y = (currentHeight - hoverTo) * .1f;
            }

            Vector2 shootFrom = projectile.Top;
            shootFrom.Y += 8;
            float pointAt = (QwertysRandomContent.GetLocalCursor(player.whoAmI) - shootFrom).ToRotation();

            player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation = QwertyMethods.SlowRotation(player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation, pointAt, 3);
            //Main.NewText(player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation);

            if (shotCooldown > 0)
            {
                shotCooldown--;
            }
            if (player.whoAmI == Main.myPlayer && Main.mouseLeft && !player.HasBuff(mod.BuffType("MorphSickness")) && shotCooldown == 0)
            {
                shotCooldown = 12;
                Projectile p = Main.projectile[Projectile.NewProjectile(shootFrom + QwertyMethods.PolarVector(19, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), QwertyMethods.PolarVector(12, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), ProjectileID.GreenLaser, projectile.damage, projectile.knockBack, player.whoAmI)];
                p.magic = false;
                p.GetGlobalProjectile<MorphProjectile>().morph = true;
                p.penetrate = 1;
                p.alpha = 0;
                if (Main.netMode == 1)
                {
                    QwertysRandomContent.UpdateProjectileClass(p);
                }
                Main.PlaySound(SoundID.Item12, projectile.Center);
            }

            currentHeight = 0; //reset
            hoverTo = 0;
            hoverSpeed = 3f;
            if (player.controlRight)
            {
                projectile.velocity.X += 6f;
            }
            else if (player.controlLeft)
            {
                projectile.velocity.X -= 6f;
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

            if (Math.Abs(projectile.velocity.X) < 2f)
            {
                projectile.frame = 0;
            }
            else
            {
                projectile.frame = 1;
                Dust d = Dust.NewDustPerfect(projectile.Center + new Vector2(-projectile.width / 2 * projectile.direction, 2 + Main.rand.Next(-3, 3)), 6);
                d.noGravity = true;
                d.noLight = true;
            }
        }

        public override bool DrawMorphExtras(SpriteBatch spriteBatch, Color lightColor)
        {
            Player drawPlayer = Main.player[projectile.owner];
            Texture2D texture = mod.GetTexture("Items/Weapons/Meteor/Hovercraft_Cannon");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White), 0f);
            spriteBatch.Draw(texture,
                new Vector2(projectile.position.X + 20, projectile.position.Y + 8) - Main.screenPosition,
                new Rectangle(0, 0, 24, 10),
                color12,
                drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation,
                new Vector2(5, 5),
                1f,
                0,
                0);
            return true;
        }
    }
}