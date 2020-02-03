
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using QwertysRandomContent.Buffs;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//copied from example javelin forom example mod
namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Javelin");
            Tooltip.SetDefault("Throws two at once!");
           

        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? base.Texture + "_Old" : base.Texture;
        public override void SetDefaults()
        {
            // Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
            item.shootSpeed = 10f;
            item.damage = 21;
            item.knockBack = 5f;
            item.useStyle = 1;
            item.useAnimation = 23;
            item.useTime = 23;
            item.width = 68;
            item.height = 68;
            item.maxStack = 999;
            item.rare = 5;
            item.crit = 5;
            item.value = 30;
            item.consumable = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.thrown = true;

            item.UseSound = SoundID.Item1;

            item.shoot = mod.ProjectileType("RhuthiniumJavelinP");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 1);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 150);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            float angle = (new Vector2(speedX, speedY)).ToRotation();
            float trueSpeed = (new Vector2(speedX, speedY)).Length();
            Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(Main.rand.Next(-5, 6))) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(Main.rand.Next(-5, 6))) * trueSpeed, type, damage, knockBack, Main.myPlayer, 0f, 0f);
            Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, (float)Math.Cos(angle + MathHelper.ToRadians(Main.rand.Next(-5, 6))) * trueSpeed, (float)Math.Sin(angle + MathHelper.ToRadians(Main.rand.Next(-5, 6))) * trueSpeed, type, damage, knockBack, Main.myPlayer, 0f, 0f);
            return false;
        }
    }
    public class RhuthiniumJavelinP : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RhuthiniumJavelin");
        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? base.Texture + "_Old" : base.Texture;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.thrown = true;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
            projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 2;
            maxStickingJavelins = 12;
            dropItem = mod.ItemType("RhuthiniumJavelin");
            rotationOffset = (float)Math.PI / 4;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/Rhuthinium/RhuthiniumJavelinP" + (ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? "_Old" : ""));
            spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
