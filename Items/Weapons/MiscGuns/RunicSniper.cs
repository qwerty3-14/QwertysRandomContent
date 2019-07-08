using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.MiscGuns
{
	public class RunicSniper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Runic Sniper");
			Tooltip.SetDefault("x2 damage to enemies far away from you" + "\nRight click to zoom");
			
		}
		public override void SetDefaults()
		{
			item.damage = 190;
			item.ranged = true;
            item.autoReuse = true;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.knockBack = 5;
            item.value = 500000;
            item.rare = 9;
            item.UseSound = SoundID.Item11;
            if (!Main.dedServ)
            {
                //item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/MiscGuns/RunicSniper_Glow");
            }
            //item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -26;
            //item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -2;
            item.width = 74;
			item.height = 30;
			item.crit = 25;
			item.shoot = 97;
			item.useAmmo = 97;
			item.shootSpeed =36;
			item.noMelee=true;
			
			
			
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("AncientSniper"));
            recipe.AddIngredient(mod.ItemType("CraftingRune"), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        /*
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/Weapons/MiscGuns/RunicSniper_Glow");
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
        }*/
        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, -6);
		}
		public override void HoldItem(Player player)
		{
			player.scope = true;
		}	

		
	}
	public class DoubleSnipeDamage : ModPlayer
    {
        
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if(proj.ranged && player.inventory[player.selectedItem].type ==mod.ItemType("RunicSniper"))
            {
                if ((target.Center - player.Center).Length() > 700)
                    damage *= 2;
            }
        }
        
    }
	
}

