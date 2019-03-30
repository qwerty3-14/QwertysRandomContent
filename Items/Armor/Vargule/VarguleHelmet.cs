using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertysRandomContent;
namespace QwertysRandomContent.Items.Armor.Vargule
{
	[AutoloadEquip(EquipType.Head)]
	public class VarguleHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vargule Helmet");
			Tooltip.SetDefault("+3 mining range" + "\nEmits light and improves vison");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 8;
			
			
			item.width = 22;
			item.height = 16;
			item.defense = 9;
			
			
			
		}

        public override void UpdateEquip(Player player)
        {
            //player.AddBuff(11, 10);
            //player.AddBuff(12, 10);
            player.nightVision = true;
            Lighting.AddLight(player.Center, 1.0f, 1.0f, 1.0f);
            if (player.whoAmI == Main.myPlayer && (player.HeldItem.pick > 0 || player.HeldItem.axe > 0 || player.HeldItem.hammer > 0))
            {
                Player.tileRangeX += 3;
                Player.tileRangeY += 3;
            }




        }
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
			drawAltHair=true;
			
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("VarguleChestplate") && legs.type == mod.ItemType("VarguleLeggings");
			
		}
		
	
		
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Spelunker effect and +40% mining speed";
			 player.findTreasure = true;
			player.pickSpeed -= 0.4f;
			
			
			
		}
		
		
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VarguleBar"), 12);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
			
	}
		
	
}

