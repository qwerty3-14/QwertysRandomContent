using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Dusts
{
	public class GlassSmoke : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = false;
			dust.scale = 1f;
		}

		public override bool Update(Dust dust)
		{
			dust.velocity.Y -= .1f;
			return true;
		}
	}
}