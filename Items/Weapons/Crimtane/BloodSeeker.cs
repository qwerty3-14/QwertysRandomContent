using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.AbstractClasses;
using QwertysRandomContent.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//copied from example javelin forom example mod
namespace QwertysRandomContent.Items.Weapons.Crimtane
{
    public class BloodSeeker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Seeker");
            Tooltip.SetDefault("Has slight homing");
        }

        public override void SetDefaults()
        {
            // Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
            item.shootSpeed = 10f;
            item.damage = 11;
            item.knockBack = 5f;
            item.useStyle = 1;
            item.useAnimation = 21;
            item.useTime = 21;
            item.width = 68;
            item.height = 68;
            item.rare = 5;
            item.value = Item.sellPrice(silver: 30);
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.melee = true;

            item.UseSound = SoundID.Item1;

            item.shoot = mod.ProjectileType("BloodSeekerP");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 10);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class BloodSeekerP : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Seeker");
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
            projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 5;
            maxStickingJavelins = 10;
            dropItem = mod.ItemType("BloodSeeker");
            rotationOffset = (float)Math.PI / 4;
        }

        private NPC target;

        public override void NonStickingBehavior()
        {
            if (QwertyMethods.ClosestNPC(ref target, 300, projectile.Center))
            {
                float dir = projectile.velocity.ToRotation();
                dir = QwertyMethods.SlowRotation(dir, (target.Center - projectile.Center).ToRotation(), 1);
                projectile.velocity = QwertyMethods.PolarVector(projectile.velocity.Length(), dir);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}