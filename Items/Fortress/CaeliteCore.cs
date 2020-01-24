using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
    public class CaeliteCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shining Core");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {

            item.width = 18;
            item.height = 18;
            item.maxStack = 999;
            item.value = 25000;
            item.rare = 3;

            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;

        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 0;
            item.velocity.X = item.velocity.X * 0.95f;
            if ((double)item.velocity.X < 0.1 && (double)item.velocity.X > -0.1)
            {
                item.velocity.X = 0f;
            }
            item.velocity.Y = item.velocity.Y * 0.95f;
            if ((double)item.velocity.Y < 0.1 && (double)item.velocity.Y > -0.1)
            {
                item.velocity.Y = 0f;
            }
            Dust dust = Main.dust[Dust.NewDust(item.position, item.width, item.height, mod.DustType("CaeliteDust"))];
            dust.scale = .5f;
            Lighting.AddLight(item.Center, 1f, 1f, 1f);
        }





    }
}
