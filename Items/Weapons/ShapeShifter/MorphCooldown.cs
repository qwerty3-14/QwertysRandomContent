using Terraria;
using Terraria.ModLoader;
using QwertysRandomContent.NPCs;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
	public class MorphCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Quick Morph Cool down");
			Description.SetDefault("Can't use another quick morph!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}
       


    }
    
}