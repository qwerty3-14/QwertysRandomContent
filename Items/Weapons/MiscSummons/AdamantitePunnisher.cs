using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSummons
{
    public class AdamantitePunnisherStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Punnisher staff");
            Tooltip.SetDefault("Banned from boxing tournaments");
        }

        public override void SetDefaults()
        {
            item.damage = 25;
            item.mana = 20;
            item.width = 32;
            item.height = 32;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 10f;
            item.value = 138000;
            item.rare = 4;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("AdamantitePunnisher");
            item.summon = true;
            item.sentry = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 12);
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

    public class AdamantitePunnisher : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Punnisher");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.sentry = true;
            projectile.width = 42;
            projectile.height = 42;
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
            //projectile.hide = true; // Prevents projectile from being drawn normally. Use in conjunction with DrawBehind.
        }

        private NPC target;

        private int timer;
        private bool runOnce = true;
        private Projectile Fist;

        //bool returnFist;
        private float FistExtension = 17f;

        private float FistExtensionSpeed = 12f;
        private float maxFistExtension = 900f;
        private int wait;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.UpdateMaxTurrets();
            if (runOnce)
            {
                Fist = Main.projectile[Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("PunnishFist"), projectile.damage, 0, player.whoAmI, projectile.whoAmI)];
                runOnce = false;
            }

            if (projectile.ai[1] == 1)
            {
                if (FistExtension <= 17)
                {
                    FistExtension = 17;
                    projectile.ai[1] = 0;
                }
                else
                {
                    FistExtension -= FistExtensionSpeed;
                }
                wait = 0;
            }
            else if (QwertyMethods.ClosestNPC(ref target, 800f, projectile.Center, false, player.MinionAttackTargetNPC))
            {
                wait++;
                projectile.rotation = (target.Center - projectile.Center).ToRotation();
                if (wait == 10)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        Fist.localNPCImmunity[k] = 0;
                    }
                }
                if (wait > 10)
                {
                    FistExtension += FistExtensionSpeed;
                    if (FistExtension > maxFistExtension)
                    {
                        projectile.ai[1] = 1;
                    }
                }
            }
            else
            {
                projectile.ai[1] = 1;
            }

            Fist.timeLeft = 2;
            Fist.Center = projectile.Center + QwertyMethods.PolarVector(FistExtension, projectile.rotation);
            Fist.rotation = projectile.rotation + (float)Math.PI / 2;
        }

        private float sawRotation = 0;
        private float sawRotation2 = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            /*
            spriteBatch.Draw(mod.GetTexture("Items/Weapons/MiscSummons/TitaniumGrinder"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                       new Rectangle(0, 0, projectile.width, projectile.height), lightColor, projectile.rotation,
                       new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
                       */
            Vector2 playerCenter = Fist.Center;
            Vector2 center = projectile.Center + QwertyMethods.PolarVector(-25, projectile.rotation);
            Vector2 distToProj = playerCenter - projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            while (distance > 13f && !float.IsNaN(distance))
            {
                distToProj.Normalize();                 //get unit vector
                distToProj *= 8f;                      //speed = 12
                center += distToProj;                   //update draw position
                distToProj = playerCenter - center;    //update distance
                distance = distToProj.Length();
                Color drawColor = lightColor;

                //Draw chain
                spriteBatch.Draw(mod.GetTexture("Items/Weapons/MiscSummons/AdamantiteChain"), new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 14, 8), drawColor, projRotation,
                    new Vector2(14 * 0.5f, 8 * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
        }
    }

    public class PunnishFist : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Arrow");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.minion = true;
            //projectile.arrow = true;
            projectile.timeLeft = 2;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            //Main.PlaySound(2, -1, -1, 59, 1f, -.2f);
        }

        private Projectile parent;

        public override void AI()
        {
            parent = Main.projectile[(int)projectile.ai[0]];
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            knockback = 0;
            if (crit)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PUNCH").WithVolume(.8f).WithPitchVariance(.5f), (int)target.position.X, (int)target.position.Y);
                if (!target.boss && !target.immortal)
                {
                    target.velocity = QwertyMethods.PolarVector(2f, projectile.rotation - (float)Math.PI / 2);
                }
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PUNCH").WithVolume(.4f).WithPitchVariance(.5f), (int)target.position.X, (int)target.position.Y);
                if (!target.boss && !target.immortal)
                {
                    target.velocity = QwertyMethods.PolarVector(4f, projectile.rotation - (float)Math.PI / 2);
                }
            }
            for (int k = 0; k < 200; k++)
            {
                projectile.localNPCImmunity[k] = -1;
                Main.npc[k].immune[projectile.owner] = 0;
            }
            parent.ai[1] = 1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            parent.ai[1] = 1;
            return false;
        }
    }
}