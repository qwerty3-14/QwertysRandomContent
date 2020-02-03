using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Arrow");
            Tooltip.SetDefault("Does more damage to enemies farther away from you");
            
        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? base.Texture + "_Old" : base.Texture;
        public override void SetDefaults()
        {
            item.shootSpeed = 3f;
            item.shoot = mod.ProjectileType("RhuthiniumArrowP");
            item.damage = 5;
            item.width = 14;
            item.height = 32;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = AmmoID.Arrow;
            item.knockBack = 2f;
            item.rare = 3;
            item.value = 5;
            item.ranged = true;


        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
    public class RhuthiniumArrowP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Arrow");
            

        }
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? base.Texture + "_Old" : base.Texture;
        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.arrow = true;

            projectile.tileCollide = true;


        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            float distance = (player.Center - target.Center).Length();
            if (distance > 1500)
            {
                distance = 1500;
            }
            damage = damage + (int)(((float)damage * distance / 1500f) / 2f);

        }





    }

}

