using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Dungeon
{
    public class BurstMiner : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicDungeon ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stream Miner");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 11;
            item.melee = true;

            item.useTime = 9;
            item.useAnimation = 9;
            item.tileBoost = -1;

            item.useStyle = 1;
            item.knockBack = 3;
            item.value = Item.sellPrice(silver: 54);
            item.rare = 2;
            item.UseSound = SoundID.Item1;

            item.width = 30;
            item.height = 30;
            item.noMelee = true;
            item.autoReuse = true;
            item.pick = 95;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int num292 = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 172, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default(Color), 0.9f);
            Main.dust[num292].noGravity = true;
            Main.dust[num292].velocity *= 0.1f;
        }
    }
}