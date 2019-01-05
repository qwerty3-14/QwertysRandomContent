using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Dusts
{
	public class DnasDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = false;
			dust.noLight = true;
			dust.scale = 1f;
		}

		
	}
}