using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Lune
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	
	public class LuneBracelet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lune Bracelet");
			Tooltip.SetDefault("Lets you dash (3 dash power)" + "\n+2 dash power at night");


        }
		
		public override void SetDefaults()
		{
			
			item.value = 10000;
			item.rare = 1;
			
			
			item.width = 28;
			item.height = 22;
			
			item.accessory = true;
			
			
			
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
            if (modPlayer.customDashSpeed < 3f)
            {
                modPlayer.customDashSpeed = 3f;
            }
            if(!Main.dayTime)
            {
                modPlayer.customDashBonusSpeed += 2;
            }
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("LuneBar"), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
			
	}
		
	
}

