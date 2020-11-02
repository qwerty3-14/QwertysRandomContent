using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscSpells
{
    public class RunicWave : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runic Wave");
            Tooltip.SetDefault("Cast a wave that draws ice runes in flight");
        }

        public override void SetDefaults()
        {
            item.damage = 180;
            item.magic = true;

            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.knockBack = 100;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.width = 28;
            item.height = 30;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture =  mod.GetTexture("Items/Weapons/MiscSpells/RunicWave_Glow") ;
            }
            item.mana = 12;
            item.shoot = mod.ProjectileType("RunicWaveP");
            item.shootSpeed = 9;
            item.noMelee = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscSpells/RunicWave_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("AncientWave"));
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }

    public class RunicWaveP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Runic Wave");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 48;
            projectile.height = 48;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 60 * 3;
        }

        public int dustTimer;
        public int timer;
        private bool runOnce = true;
        private float iceRuneSpeed = 10;
        private Projectile ice1;
        private Projectile ice2;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.frame = (int)projectile.ai[1];
            dustTimer++;
            timer++;

            if (runOnce)
            {
                float startDistance = 100;

                ice1 = Main.projectile[Projectile.NewProjectile(projectile.Center.X + (float)Math.Cos(0) * startDistance, projectile.Center.Y + (float)Math.Sin(0) * startDistance, 0, 0, mod.ProjectileType("IceRuneTome"), projectile.damage, 3f, Main.myPlayer)];
                ice2 = Main.projectile[Projectile.NewProjectile(player.Center.X + (float)Math.Cos(Math.PI) * startDistance, player.Center.Y + (float)Math.Sin(Math.PI) * startDistance, 0, 0, mod.ProjectileType("IceRuneTome"), projectile.damage, 3f, Main.myPlayer)];
                runOnce = false;
            }
            ice1.rotation += (float)((2 * Math.PI) / (Math.PI * 2 * 100 / iceRuneSpeed));
            ice1.velocity.X = iceRuneSpeed * (float)Math.Cos(ice1.rotation) + projectile.velocity.X;
            ice1.velocity.Y = iceRuneSpeed * (float)Math.Sin(ice1.rotation) + projectile.velocity.Y;

            ice2.rotation += (float)((2 * Math.PI) / (Math.PI * 2 * 100 / iceRuneSpeed));
            ice2.velocity.X = iceRuneSpeed * (float)Math.Cos(ice2.rotation) + projectile.velocity.X;
            ice2.velocity.Y = iceRuneSpeed * (float)Math.Sin(ice2.rotation) + projectile.velocity.Y;

            if (dustTimer > 5)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("IceRuneDeath"), 0, 0, 0, default(Color), .2f);
                dustTimer = 0;
            }

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, texture.Size() * .5f, Vector2.One, 0, 0);
            return false;
        }
    }

    internal class IceRuneTome : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 36;
            projectile.height = 36;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.tileCollide = false;
            projectile.timeLeft = 60 * 3;
            projectile.melee = true;
        }

        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (runOnce)
            {
                projectile.rotation = (player.Center - projectile.Center).ToRotation() - (float)Math.PI / 2;
                runOnce = false;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 1200);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return base.PreDraw(spriteBatch, Color.White);
        }
    }
}