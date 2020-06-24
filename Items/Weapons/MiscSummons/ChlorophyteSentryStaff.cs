using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
    public class ChlorophyteSentryStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Sentry Staff");
            Tooltip.SetDefault("Summons a Chlorophyte Sentry that launches clouds of spores!");
        }

        public override void SetDefaults()
        {
            item.damage = 42;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = .01f;
            item.value = Item.sellPrice(0, 4, 52, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("ChlorophyteSentry");
            item.summon = true;
            item.sentry = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;   //this make so the projectile will spawn at the mouse cursor position

            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }

    public class ChlorophyteSentry : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Sentry");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.sentry = true;
            projectile.width = 38;
            projectile.height = 38;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = Projectile.SentryLifeTime;
            projectile.knockBack = 10f;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.sentry = true;
            projectile.minion = true;
            projectile.usesLocalNPCImmunity = true;
        }

        private NPC target;

        private float maxDistance = 1000f;

        private int timer;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.UpdateMaxTurrets();
            timer++;

            if (QwertyMethods.ClosestNPC(ref target, maxDistance, projectile.Center, false, player.MinionAttackTargetNPC))
            {
                projectile.rotation = (target.Center - projectile.Center).ToRotation();
                if (timer % 60 == 0 && player.whoAmI == Main.myPlayer)
                {
                    int numOfProj = 8 + Main.rand.Next(8);
                    for (int p = 0; p < numOfProj; p++)
                    {
                        Projectile s = Main.projectile[Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(Main.rand.NextFloat(4, 16), projectile.rotation - (float)Math.PI / 8 + Main.rand.NextFloat((float)Math.PI / 4)), ProjectileID.SporeCloud, projectile.damage, projectile.knockBack, player.whoAmI)];
                        s.melee = false;
                        s.minion = true;
                        if (Main.netMode == 1)
                        {
                            QwertysRandomContent.UpdateProjectileClass(s);
                        }
                    }
                }
            }
        }
    }
}