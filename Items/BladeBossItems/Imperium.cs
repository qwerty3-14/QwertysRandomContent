using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using QwertysRandomContent.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.BladeBossItems
{
    public class Imperium : ModItem
    {
        public override void SetDefaults()
        {
            item.shootSpeed = 17f;
            item.damage = 160;
            item.knockBack = 5f;
            item.useStyle = 1;
            item.useAnimation = 30;
            item.useTime = 30;
            item.width = 68;
            item.height = 68;
            item.maxStack = 1;
            item.rare = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.consumable = false;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.melee = true;

            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("ImperiumP");
        }
    }

    public class ImperiumP : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Imperium");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
            projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 32;
            maxStickingJavelins = 12;
            dropItem = mod.ItemType("Imperium");
            rotationOffset = (float)Math.PI / 4;
            maxTicks = 60f;
            maxStickingJavelins = 10;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Items/BladeBossItems/ImperiumP");
            spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}