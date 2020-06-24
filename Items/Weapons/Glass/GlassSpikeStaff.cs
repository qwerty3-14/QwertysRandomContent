using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Glass
{
    public class GlassSpikeStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Spike Staff");
            Tooltip.SetDefault("Summon spikes that rest on the ground, damaging enemies that step on them \nWill reposition if you walk away");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicGlass ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.damage = 12;
            item.mana = 20;
            item.width = 38;
            item.height = 38;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = 10000;
            item.rare = 1;
            item.UseSound = SoundID.Item44;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GlassSpike");
            item.summon = true;
            item.buffType = mod.BuffType("GlassSpike");
            item.buffTime = 3600;
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

    public class GlassSpike : ModProjectile
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicGlass ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Spike");
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 1;

            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 2;
            projectile.aiStyle = -1;
            projectile.usesLocalNPCImmunity = true;
        }

        private float orientation = 0;
        private bool orientationSet = false;
        private bool spin = true;

        public override void AI()
        {
            if (spin)
            {
                projectile.rotation += projectile.velocity.Length() * (float)Math.PI / 60 * (projectile.velocity.X > 0 ? 1 : -1);
            }
            spin = true;
            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<MinionManager>().GlassSpike)
            {
                projectile.timeLeft = 2;
            }

            if ((player.Center - projectile.Center).Length() > 400)
            {
                projectile.tileCollide = false;
            }

            if (projectile.tileCollide)
            {
                projectile.velocity.Y = 7;
                projectile.velocity.X *= .9f;
            }
            else
            {
                Vector2 flyTo = player.Center + new Vector2(orientation, -20);
                if ((projectile.Center - flyTo).Length() < 20)
                {
                    projectile.tileCollide = true;
                }
                Vector2 vel = (flyTo - projectile.Center) * .07f;
                if (vel.Length() < 3)
                {
                    vel = vel.SafeNormalize(-Vector2.UnitY) * 3;
                }
                projectile.velocity = vel;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projectile.rotation,
                        new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            if (!orientationSet)
            {
                orientation = projectile.Center.X - Main.player[projectile.owner].Center.X;
                orientationSet = true;
            }
            spin = false;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.localNPCImmunity[target.whoAmI] = 20;
            target.immune[projectile.owner] = 0;
        }
    }
}