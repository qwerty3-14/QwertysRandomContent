using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Dusts
{
    public class BloodforceDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale = 1f;
        }


    }
}