using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
	[AutoloadEquip(EquipType.Legs)]
	public class HydraLeggings : ModItem
	{
		public override bool Autoload(ref string name)
        {
			if (!Main.dedServ)
                {
                    // Add the female leg variant
                    //mod.AddEquipTexture(null, EquipType.Legs, "HydraLeggings_Female", "QwertysRandomContent/Items/HydraItems/HydraLeggings_FemaleLegs");
                }
				return true;
			
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Leggings");
			Tooltip.SetDefault("+1 life/sec regen rate" + "\n+10% summon damage and movement speed");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 5;
			
			
			item.width = 22;
			item.height = 18;
			item.defense = 14;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			
			player.lifeRegen += 2;
			player.minionDamage  += .1f;
			player.moveSpeed += .1f;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
			if (male) equipSlot = mod.GetEquipSlot("HydraLeggings", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("HydraLeggings_Female", EquipType.Legs);
		}
		
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("HydraScale"), 18);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
			
	}
    public class HydraLeggingsGlowmask : ModPlayer
    {
        public static readonly PlayerLayer HydraShoes = LayerDrawing.DrawOnLegs("HydraLeggings", "Items/HydraItems/HydraLeggings_Legs_Glow", "HydraLeggings_Female", "Items/HydraItems/HydraLeggings_FemaleLegs_Glow");
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                HydraShoes.visible = true;
                layers.Insert(legLayer + 1, HydraShoes);
            }

        }
    }

}

