using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
	
	
	public class Metronome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metronome");
			Tooltip.SetDefault("Each kill grant +2% non summon damage up to a max of 200%" + "\nBonus resets when you take damage or switch weapons");
			
		}
		
		public override void SetDefaults()
		{
			
			item.value = 300000;
			item.rare = 3;
			
			
			item.width = 20;
			item.height = 18;
			
			item.accessory = true;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			var modPlayer = player.GetModPlayer<QwertyPlayer>(mod);
			modPlayer.Metronome = true;
			player.magicDamage += modPlayer.killCount*.02f;
			player.meleeDamage += modPlayer.killCount*.02f;
			player.rangedDamage += modPlayer.killCount*.02f;
			player.thrownDamage += modPlayer.killCount*.02f;
			
		}
		
		
			
	}
		
	
}

