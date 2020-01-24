using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Dusts
{
    public class HydraBreathGlow : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = 1f;

        }
        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, .8f, .8f, .8f);
            return true;
        }


    }
}