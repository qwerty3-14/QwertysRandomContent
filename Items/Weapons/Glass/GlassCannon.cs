using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
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
        public const int dmg = 50;
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
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
            item.shoot = mod.ProjectileType("GlassCannonMorph");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("GlassCannonMorphB"), 2);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
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
            if (player.GetModPlayer<ShapeShifterPlayer>().delayThing <= 0)
            {
                player.buffTime[buffIndex] = 2;
            }
        }
    }

    public class GlassCannonMorph : StableMorph
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicGlass ? base.Texture + "_Old" : base.Texture;
        private int shotCooldown;

        public override void SetSafeDefaults()
        {
            projectile.width = 30;
            projectile.height = 18;
            buffName = "GlassCannonMorphB";
            itemName = "GlassCannonShift";
        }

        public override void Movement(Player player)
        {
            projectile.velocity.Y += .4f;
            if (projectile.velocity.Y > 10)
            {
                projectile.velocity.Y = 10;
            }
            player.GetModPlayer<ShapeShifterPlayer>().glassCannon = true;

            Vector2 shootFrom = projectile.Top;
            //shootFrom.Y -= 4;
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
                shotCooldown = 15;
                Projectile.NewProjectile(shootFrom + QwertyMethods.PolarVector(30, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), QwertyMethods.PolarVector(16, player.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation), mod.ProjectileType("GlassCannonball"), projectile.damage, projectile.knockBack, player.whoAmI);
            }
        }

        public override bool DrawMorphExtras(SpriteBatch spriteBatch, Color lightColor)
        {
            Player drawPlayer = Main.player[projectile.owner];
            Texture2D texture = mod.GetTexture("Items/Weapons/Glass/GlassCannon" + (ModContent.GetInstance<SpriteSettings>().ClassicGlass ? "_Old" : ""));
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White), 0f);
            spriteBatch.Draw(texture,
                 new Vector2(projectile.position.X + 15, projectile.position.Y) - Main.screenPosition,
                 new Rectangle(0, 0, 30, 8),
                 color12,
                 drawPlayer.GetModPlayer<ShapeShifterPlayer>().tankCannonRotation,
                 new Vector2(4, 4),
                 1f,
                 0,
                 0);
            return true;
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
    }
}