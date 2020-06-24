using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            item.mountType = mod.MountType("AnvilMorph");
            item.damage = dmg;
            item.crit = crt;
            item.knockBack = kb;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.GetGlobalItem<ShapeShifterItem>().morphDef = def;
            item.GetGlobalItem<ShapeShifterItem>().morphType = ShapeShifterItem.StableShiftType;
        }

        public override bool CanUseItem(Player player)
        {
            player.GetModPlayer<ShapeShifterPlayer>().justStableMorphed(true);

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
            Description.SetDefault("");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("AnvilMorph"), player);
            player.buffTime[buffIndex] = 10;
        }
    }

    public class AnvilMorph : ModMountData
    {
        public override void SetDefaults()
        {
            mountData.buff = mod.BuffType("AnvilMorphB");
            mountData.spawnDust = 15;

            mountData.heightBoost = -26;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 0f;
            mountData.dashSpeed = 0f;
            mountData.acceleration = 0f;
            mountData.jumpHeight = 0;
            mountData.jumpSpeed = 0f;
            mountData.totalFrames = 1;
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

        private bool kicking = false;
        private int kickTimer;
        private bool forcedRunKick = false;

        public override void UpdateEffects(Player player)
        {
            player.maxFallSpeed = 30f;
            //player.velocity.Y *= 2;
            //player.Hitbox.Height = 30;
            player.gravity *= 6;
            player.GetModPlayer<ShapeShifterPlayer>().noDraw = true;
            Mount mount = player.mount;
            player.GetModPlayer<ShapeShifterPlayer>().morphed = true;

            player.noItems = true;
            player.statDefense = AnvilShift.def + player.GetModPlayer<ShapeShifterPlayer>().morphDef;
            if (player.velocity.Y > 0)
            {
                player.immune = true;
                player.immuneTime = 2;
                player.immuneNoBlink = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("AnvilImpact")] <= 0)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("AnvilImpact"), (int)(AnvilShift.dmg * player.GetModPlayer<ShapeShifterPlayer>().morphDamage), AnvilShift.kb, player.whoAmI);
                }
            }
        }
    }

    public class AnvilImpact : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kick");
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