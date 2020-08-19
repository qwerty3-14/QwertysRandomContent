using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class AnvilShift : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shift: Anvil!");
            Tooltip.SetDefault("");
        }

        public const int dmg = 52;
        public const int crt = 0;
        public const float kb = 10f;
        public const int def = 30;

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
            item.shoot = mod.ProjectileType("AnvilMorph");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(mod.BuffType("AnvilMorphB"), 2);
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
            recipe.AddRecipeGroup("IronBar", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class AnvilMorphB : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Anvil");
            Description.SetDefault("Thunk!");
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

    public class AnvilMorph : StableMorph
    {
        public override void SetSafeDefaults()
        {
            projectile.width = 32;
            projectile.height = 16;
            buffName = "AnvilMorphB";
            itemName = "AnvilShift";
        }

        public override void Effects(Player player)
        {
            if (projectile.velocity.Y > 0)
            {
                player.immune = true;
                player.immuneTime = 2;
                player.immuneNoBlink = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("AnvilImpact")] <= 0)
                {
                    Projectile.NewProjectile(projectile.Center + projectile.velocity, Vector2.Zero, mod.ProjectileType("AnvilImpact"), projectile.damage, projectile.knockBack, player.whoAmI);
                }
            }
        }

        public override void Movement(Player player)
        {
            projectile.velocity.Y += .5f;
            if (projectile.velocity.Y > 20)
            {
                projectile.velocity.Y = 20;
            }
        }
    }

    public class AnvilImpact : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anvil");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 32;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.GetGlobalProjectile<MorphProjectile>().morph = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            projectile.Center = Main.player[projectile.owner].Center;
            if (Main.player[projectile.owner].velocity.Y > 0f)
            {
                projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }
}