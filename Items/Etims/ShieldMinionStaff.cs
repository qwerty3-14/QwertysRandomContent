using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using QwertysRandomContent.NPCs.Fortress;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Etims
{
    public class ShieldMinionStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Slams intruders that get too close to you! +\nBurst damage minion");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicNoehtnap ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.damage = 48;
            item.mana = 20;
            item.width = 34;
            item.height = 34;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 6f;
            item.rare = 3;
            item.value = 120000;
            item.UseSound = SoundID.Item44;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ShieldMinion");
            item.summon = true;
            item.buffType = mod.BuffType("ShieldMinion");
            item.buffTime = 3600;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 SPos = Main.MouseWorld;   //this make so the projectile will spawn at the mouse cursor position
            position = SPos;

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("EtimsMaterial"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class ShieldMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield Minion");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 34;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;

            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            projectile.usesLocalNPCImmunity = true;
        }

        private Vector2 flyTo;
        private int identity = 0;
        private int ShieldCount = 0;
        private Vector2 eyeOffset;
        private NPC target;
        private float horizontalEyeMultiploer = 3;
        private float verticalEyeMultiplier = 2;

        private const int guarding = 0;
        private const int charging = 1;
        private const int cooling = 2;
        private int chargeTimer = 0;
        private Vector2 LatestValidVelocity;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<MinionManager>().ShieldMinion)
            {
                projectile.timeLeft = 2;
            }
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == mod.ProjectileType("ShieldMinion") && Main.projectile[p].active && Main.projectile[p].owner == projectile.owner && Main.projectile[p].ai[1] == projectile.ai[1])
                {
                    ShieldCount++;
                }
            }

            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].type == mod.ProjectileType("ShieldMinion") && Main.projectile[p].active && Main.projectile[p].owner == projectile.owner && Main.projectile[p].ai[1] == projectile.ai[1])
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
            projectile.friendly = (projectile.ai[1] == charging);
            if (projectile.ai[1] != charging)
            {
                if (player.velocity.Length() > .1f)
                {
                    LatestValidVelocity = player.velocity;
                }

                float myOffset = (((float)Math.PI / 2) * (float)(identity + 1)) / (ShieldCount + 1) - (float)Math.PI / 4;
                flyTo = player.Center + QwertyMethods.PolarVector(projectile.ai[1] == guarding ? 120 : -50, (QwertysRandomContent.GetLocalCursor(projectile.owner) - player.Center).ToRotation() + myOffset);

                if (flyTo != null && flyTo != Vector2.Zero)
                {
                    projectile.velocity = (flyTo - projectile.Center) * .1f;
                }
            }
            switch ((int)projectile.ai[1])
            {
                case guarding:
                    projectile.frame = 0;

                    if (QwertyMethods.ClosestNPC(ref target, 1000, player.Center, true, player.MinionAttackTargetNPC))
                    {
                        eyeOffset = (target.Center - projectile.Center).SafeNormalize(-Vector2.UnitY);
                        eyeOffset.X *= horizontalEyeMultiploer;
                        eyeOffset.Y *= verticalEyeMultiplier;
                        if ((target.Center - projectile.Center).Length() < 120)
                        {
                            projectile.velocity = QwertyMethods.PolarVector(24, (target.Center - projectile.Center).ToRotation());
                            projectile.ai[1] = charging;
                            chargeTimer = 10;
                            break;
                        }
                    }
                    else
                    {
                        eyeOffset = Vector2.Zero;
                    }

                    break;

                case charging:
                    projectile.frame = 0;
                    chargeTimer--;
                    if (chargeTimer <= 0)
                    {
                        projectile.ai[1] = cooling;
                        chargeTimer = -120;
                    }
                    break;

                case cooling:
                    projectile.frame = 1;
                    chargeTimer++;
                    if (chargeTimer >= 0)
                    {
                        projectile.ai[1] = guarding;
                    }
                    break;
            }

            identity = 0;
            ShieldCount = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = 10;
            target.immune[projectile.owner] = 0;
            if (Main.rand.Next(10) == 0 && !target.boss)
            {
                target.AddBuff(mod.BuffType("Stunned"), 120);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.GetGlobalNPC<FortressNPCGeneral>().fortressNPC)
            {
                for (int i = 0; i < damage / 3; i++)
                {
                    Dust d = Dust.NewDustPerfect(projectile.Center, mod.DustType("BloodforceDust"));
                    d.velocity *= 5f;
                }
                damage *= 2;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.frame == 0)
            {
                Texture2D eye = mod.GetTexture("Items/Etims/ShieldMinionEye");
                spriteBatch.Draw(eye, (projectile.position + new Vector2(14, 13)) + eyeOffset - Main.screenPosition,
                           eye.Frame(), lightColor, projectile.rotation,
                           eye.Size() * .5f, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}