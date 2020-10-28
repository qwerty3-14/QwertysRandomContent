using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
    public class AshFellStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ash Fell Staff");
            Tooltip.SetDefault("Thi Sentry suffocates your foes with ash missiles!");
        }

        public override void SetDefaults()
        {
            item.value = 54000;
            item.rare = 3;
            item.damage = 22;
            item.knockBack = 3f;
            item.width = item.height = 44;
            item.mana = 20;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.UseSound = SoundID.Item44;
            item.summon = true;
            item.sentry = true;
            item.shoot = mod.ProjectileType("AshFell");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteSentryStaff"));
            recipe.AddIngredient(mod.ItemType("Riptide"));
            recipe.AddIngredient(mod.ItemType("ManEaterStaff"));
            recipe.AddIngredient(mod.ItemType("RhuthiniumGuardianStaff"));
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
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

    public class AshFell : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16; //Set the hitbox width
            projectile.height = 26;   //Set the hitbox heinght
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = false;   //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.tileCollide = true; //Tells the game whether or not it can collide with tiles/ terrain
            projectile.sentry = true; //tells the game that this is a sentry
            projectile.timeLeft = Projectile.SentryLifeTime; //allows for the sentry to automaticly be replaced when new sentries are summoned
        }

        private NPC target;
        private int[] missileCounters = new int[2];
        private int missileTime = 60;
        private float missileLoadPosition = 5;

        public override void AI()
        {
            Main.player[projectile.owner].UpdateMaxTurrets();
            Player player = Main.player[projectile.owner];
            for (int i = 0; i < missileCounters.Length; i++)
            {
                if (missileCounters[i] < missileTime)
                {
                    missileCounters[i]++;
                    break;
                }
            }
            if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC))
            {
                for (int i = 0; i < missileCounters.Length; i++)
                {
                    projectile.rotation = (target.Center - projectile.Center).ToRotation();
                    if (missileCounters[i] == missileTime)
                    {
                        //shoot
                        missileCounters[i] = 0;
                        Projectile.NewProjectile(projectile.Center + QwertyMethods.PolarVector(2f, projectile.rotation) + QwertyMethods.PolarVector(missileLoadPosition * (i == 0 ? 1 : -1), projectile.rotation + (float)Math.PI / 2),
                            QwertyMethods.PolarVector(2f, projectile.rotation), mod.ProjectileType("AshMissile"), projectile.damage, projectile.knockBack, projectile.owner);
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D missile = mod.GetTexture("Items/Weapons/MiscSummons/AshMissile");
            for (int i = 0; i < missileCounters.Length; i++)
            {
                spriteBatch.Draw(missile,
                    projectile.Center + QwertyMethods.PolarVector(2f, projectile.rotation) + QwertyMethods.PolarVector(missileLoadPosition * (float)missileCounters[i] / missileTime * (i == 0 ? 1 : -1), projectile.rotation + (float)Math.PI / 2) - Main.screenPosition,
                    missile.Frame(), lightColor, projectile.rotation, missile.Size() * .5f, 1f, SpriteEffects.None, 0);
            }

            return true;
        }
    }

    public class AshMissile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 6; //Set the hitbox width
            projectile.height = 6;   //Set the hitbox heinght
            projectile.hostile = false;    //tells the game if is hostile or not.
            projectile.friendly = true;   //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;    //Tells the game whether or not projectile will be affected by water
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed  -1 is infinity
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.timeLeft = 600;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.timeLeft > finalTime)
            {
                Texture2D texture = Main.projectileTexture[projectile.type];
                spriteBatch.Draw(texture, projectile.Center - Main.screenPosition,
                           texture.Frame(), lightColor, projectile.rotation,
                           new Vector2(texture.Width - 2, texture.Height * .5f), 1f, 0, 0f);
            }
            return false;
        }

        private NPC target;
        private int finalTime = 120;
        private int blastSize = 30;

        public override void AI()
        {
            if (projectile.timeLeft == 600)
            {
                projectile.rotation = projectile.velocity.ToRotation();
            }
            //int num5 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + (float)projectile.height - 2f), projectile.width, 6, 36, 0f, 0f, 50, default, 1f);
            Player player = Main.player[projectile.owner];
            if (projectile.timeLeft <= finalTime)
            {
                if (projectile.width != blastSize)
                {
                    Vector2 oldCenter = projectile.Center;
                    projectile.width = projectile.height = blastSize;
                    projectile.Center = oldCenter;
                }
                projectile.rotation += (float)Math.PI / 10;
                for (int d = 0; d < 5; d++)
                {
                    Dust dust = Dust.NewDustPerfect(projectile.Center, 36, QwertyMethods.PolarVector(4, projectile.rotation + ((float)Math.PI * 2 * d) / 5), Scale: .5f);
                    dust.noGravity = true;
                }
                projectile.velocity = Vector2.Zero;
            }
            else
            {
                if (QwertyMethods.ClosestNPC(ref target, 1000, projectile.Center, false, player.MinionAttackTargetNPC))
                {
                    projectile.rotation = QwertyMethods.SlowRotation(projectile.rotation, (target.Center - projectile.Center).ToRotation(), 2f);
                }
                projectile.velocity = QwertyMethods.PolarVector(8f, projectile.rotation);
                Dust dust = Dust.NewDustPerfect(projectile.Center - QwertyMethods.PolarVector(6, projectile.rotation), 36, Vector2.Zero, Scale: .5f);
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.timeLeft > finalTime)
            {
                projectile.timeLeft = finalTime;
            }

            projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
            target.immune[projectile.owner] = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.timeLeft > finalTime)
            {
                projectile.timeLeft = finalTime;
            }
            return false;
        }
    }
}