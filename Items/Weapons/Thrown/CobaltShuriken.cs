using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Thrown
{
    public class CobaltShuriken : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cobalt Shuriken");
            Tooltip.SetDefault("Sticks to tiles damaging enemies that touch them");
        }

        public override void SetDefaults()
        {
            item.damage = 35;
            item.melee = true;
            item.knockBack = 5;
            item.value = Item.sellPrice(gold: 2);
            item.rare = 3;
            item.width = 34;
            item.height = 34;
            item.useStyle = 1;
            item.shootSpeed = 14f;
            item.useTime = 18;
            item.useAnimation = 18;
            item.shoot = mod.ProjectileType("CoblatShurikenP");
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CobaltBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 333);
            recipe.AddRecipe();
        }
    }

    public class CoblatShurikenP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cobalt Shuriken");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 2;
            aiType = ProjectileID.Shuriken;
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.penetrate = 7;
            projectile.melee = true;

            projectile.tileCollide = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/Thrown/CoblatShurikenP");
            spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projectile.rotation,
                        new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            projectile.aiStyle = 0;
            return false;
        }
    }
}