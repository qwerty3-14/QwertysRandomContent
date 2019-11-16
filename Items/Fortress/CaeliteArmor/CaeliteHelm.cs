using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using QwertysRandomContent;
namespace QwertysRandomContent.Items.Fortress.CaeliteArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class CaeliteHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caelite Helm");
			Tooltip.SetDefault("Enemies killed by thrown or magic attacks drop more money!" + "\n+3 recovery");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 30000;
			item.rare = 3;
			
			
			item.width = 22;
			
			item.defense = 6;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{


            player.GetModPlayer<CaeliteHelmEffect>().hasEffect = true;
            player.GetModPlayer<QwertyPlayer>().recovery += 2;

        }
		public override void DrawHair(ref bool  drawHair, ref bool  drawAltHair )
		{
            drawHair = false;
			
		}
		
		
		
	
		
		
		
		
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CaeliteBar"), 8);
            recipe.AddIngredient(mod.ItemType("CaeliteCore"), 4);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
			
	}
    public class CaeliteHelmEffect : ModPlayer
    {
        public bool hasEffect;
        public override void ResetEffects()
        {
            hasEffect = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(damage > target.life && (proj.magic || proj.thrown))
            {
                target.value *= 2;
            }
        }


    }
		
	
}

