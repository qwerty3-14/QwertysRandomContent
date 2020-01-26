using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.AncientItems
{
    public class AncientWave : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Wave");
            Tooltip.SetDefault("Blows enemies away");
            if (ModContent.GetInstance<SpriteSettings>().ClassicAncient)
            {
                Main.itemTexture[item.type] = mod.GetTexture("Items/AncientItems/Old/AncientWave_Old");
            }

        }
        public override void SetDefaults()
        {
            item.damage = 42;
            item.magic = true;

            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.knockBack = 100;
            item.value = 150000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.width = 28;
            item.height = 30;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture =  ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/Old/AncientWave_Old_Glow") : mod.GetTexture("Items/AncientItems/AncientWave_Glow");
            }
            item.mana = 12;
            item.shoot = mod.ProjectileType("AncientWaveP");
            item.shootSpeed = 9;
            item.noMelee = true;
            //item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/AncientItems/AncientWave_Glow");



        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture =  ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/Old/AncientWave_Old_Glow") : mod.GetTexture("Items/AncientItems/AncientWave_Glow");
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

    }
    public class AncientWaveP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Wave");
            if (ModContent.GetInstance<SpriteSettings>().ClassicAncient)
            {
                Main.projectileTexture[projectile.type] = mod.GetTexture("Items/AncientItems/Old/AncientWaveP_Old");
            }

        }
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 80;
            projectile.height = 80;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 60 * 15;



        }
        public int dustTimer;
        public override void AI()
        {
            dustTimer++;
            if (dustTimer > 5)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("AncientGlow"), 0, 0, 0, default(Color), .2f);
                dustTimer = 0;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {

            spriteBatch.Draw(ModContent.GetInstance<SpriteSettings>().ClassicAncient ? mod.GetTexture("Items/AncientItems/Old/AncientWaveP_Old") : mod.GetTexture("Items/AncientItems/AncientWaveP"), new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y),
                        new Rectangle(0, 0, 80, 48), Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(0, 0, 0, 0), (float)projectile.alpha / 255f), projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

    }

}

