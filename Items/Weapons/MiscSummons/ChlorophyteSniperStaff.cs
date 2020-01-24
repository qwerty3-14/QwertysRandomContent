using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
    public class ChlorophyteSniperStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Sniper Staff");
            Tooltip.SetDefault("Summons a Chlorophyte Sniper to execute your foes!");


        }

        public override void SetDefaults()
        {

            item.damage = 162;
            item.mana = 20;
            item.width = 38;
            item.height = 38;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 5f;
            item.value = Item.sellPrice(0, 4, 52, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item44;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ChlorophyteSniper");
            item.summon = true;
            item.buffType = mod.BuffType("ChlorophyteSniper");
            item.buffTime = 3600;
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
            Vector2 SPos = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);   //this make so the projectile will spawn at the mouse cursor position
            position = SPos;

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

    public class ChlorophyteSniper : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Sniper");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting

        }

        public override void SetDefaults()
        {


            projectile.width = 22;
            projectile.height = 22;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            //projectile.usesLocalNPCImmunity = true;
        }

        Vector2 flyTo;
        int identity = 0;
        int sniperCount = 0;

        NPC target;
        int timer;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            //Main.NewText(moveTo);
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            sniperCount = player.ownedProjectileCounts[mod.ProjectileType("ChlorophyteSniper")];
            if (modPlayer.chlorophyteSniper)
            {
                projectile.timeLeft = 2;
            }
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == mod.ProjectileType("ChlorophyteSniper"))
                {
                    if (p == projectile.whoAmI)
                    {
                        break;
                    }
                    else
                    {
                        identity++;
                    }
                }
            }







            timer++;
            if (sniperCount != 0)
            {
                projectile.ai[0] = 40f;

                flyTo = player.Center + QwertyMethods.PolarVector(projectile.ai[0], -modPlayer.mythrilPrismRotation + (2f * (float)Math.PI * identity) / sniperCount);

                projectile.velocity = (flyTo - projectile.Center) * .1f;
                if (QwertyMethods.ClosestNPC(ref target, 100000, projectile.Center, false, player.MinionAttackTargetNPC) && timer > 180)
                {
                    Projectile.NewProjectile(projectile.Center, QwertyMethods.PolarVector(10, (target.Center - projectile.Center).ToRotation()), mod.ProjectileType("ChlorophyteSnipe"), projectile.damage, projectile.knockBack, player.whoAmI);
                    timer = 0;
                }
                //Main.NewText(projectile.Center);/
                //projectile.Center = flyTo;

            }







            identity = 0;

        }




    }
    public class ChlorophyteSnipe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Snipe");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting

        }

        public override void SetDefaults()
        {


            projectile.width = 2;
            projectile.height = 2;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;

            projectile.knockBack = 10f;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.minion = true;

            //projectile.timeLeft = 2;
            //projectile.aiStyle = 1;
            //aiType = ProjectileID.Bullet;
            projectile.extraUpdates = 9;
            projectile.timeLeft = 1200;
            //projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            //Main.NewText( projectile.whoAmI+", "+projectile.timeLeft);
            for (int num163 = 0; num163 < 10; num163++)
            {
                float x2 = projectile.position.X - projectile.velocity.X / 10f * (float)num163;
                float y2 = projectile.position.Y - projectile.velocity.Y / 10f * (float)num163;
                int num164 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 75, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num164].alpha = projectile.alpha;
                Main.dust[num164].position.X = x2;
                Main.dust[num164].position.Y = y2;
                Main.dust[num164].velocity *= 0f;
                Main.dust[num164].noGravity = true;
            }
        }
    }

}