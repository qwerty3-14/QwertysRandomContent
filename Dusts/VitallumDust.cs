using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Dusts
{
    public class VitallumDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale = 1f;
        }
    }
}