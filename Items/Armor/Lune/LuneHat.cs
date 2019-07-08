using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Lune
{
	[AutoloadEquip(EquipType.Head)]
	public class LuneHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Hat");
			Tooltip.SetDefault("+35% ranged velocity" + "\nImproves vision");
			
		}
		

		public override void SetDefaults()
		{

            item.value = 20000;
            item.rare = 1;


            item.width = 26;
			item.height = 16;
			item.defense = 5;
			
			
			
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(mod.ItemType("LuneBar"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void UpdateEquip(Player player)
		{
            player.GetModPlayer<QwertyPlayer>(mod).rangedVelocity += .35f;
            /*
            if(player.HeldItem.ranged)
            {
                player.HeldItem.shootSpeed *= 1.35f;
            }
            */
            player.nightVision = true;
		}
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
			drawAltHair=true;
			
		}
		
		
		
	
		
		
			
	}
    
    public class RangedVel : GlobalItem
    {
        
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(item.ranged)
            {
                speedX *= player.GetModPlayer<QwertyPlayer>(mod).rangedVelocity;
                speedY *= player.GetModPlayer<QwertyPlayer>(mod).rangedVelocity;
            }
            return true;
        }
    }
    
    
		
	
}

