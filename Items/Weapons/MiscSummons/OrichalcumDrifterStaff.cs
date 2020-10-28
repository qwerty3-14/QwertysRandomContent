using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
    public class OrichalcumDrifterStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalcum Drifter Staff");
            Tooltip.SetDefault("Summons an Orichalcum Drifter to fight for you!");
        }

        public override void SetDefaults()
        {
            item.damage = 60;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 8f;
            item.value = 126500;
            item.rare = 4;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("OrichalcumDrifter");
            item.summon = true;
            item.buffType = mod.BuffType("OrichalcumDrifter");
            item.buffTime = 3600;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 12);
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

    public class OrichalcumDrifter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orichalcum Drifter");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 18;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 1;
            projectile.knockBack = 10f;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
        }

        private int identity = 0;
        private int drifterCount = 0;
        private NPC target;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            drifterCount = player.ownedProjectileCounts[mod.ProjectileType("OrichalcumDrifter")];
            if (player.GetModPlayer<MinionManager>().OrichalcumDrifter)
            {
                projectile.timeLeft = 2;
            }
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == mod.ProjectileType("OrichalcumDrifter"))
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

            if(QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC, 
                delegate (NPC possibleTarget) 
                {
                    return QwertyMethods.AngularDifference((possibleTarget.Center - projectile.Center).ToRotation(), projectile.rotation) < (float)Math.PI/2f && Collision.CanHit(player.Center, 0, 0, possibleTarget.Center, 0, 0);
                }))
            {
                projectile.rotation.SlowRotation((target.Center - projectile.Center).ToRotation(), (float)Math.PI/60f);
            }
            else
            {
                if(drifterCount != 0)
                {
                    projectile.rotation.SlowRotation((player.Center + QwertyMethods.PolarVector(40f, player.GetModPlayer<MinionManager>().mythrilPrismRotation + (2f * (float)Math.PI * identity) / drifterCount) - projectile.Center).ToRotation(), (float)Math.PI / 60f);
                }
                
            }

            projectile.velocity = QwertyMethods.PolarVector(6f, projectile.rotation);


           
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            target.immune[projectile.owner] = 0;
        }
    }
}