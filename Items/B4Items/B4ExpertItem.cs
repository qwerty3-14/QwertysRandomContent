using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.B4Items
{
    public class B4ExpertItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Rod");
            Tooltip.SetDefault("Moves you toward your cursor when used");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.maxStack = 1;
            item.value = 0;
            item.rare = 10;
            item.value = 500;
            item.rare = 9;
            item.expert = true;
            item.useStyle = 5;
            item.useAnimation = 2;
            item.useTime = 2;
            item.autoReuse = true;
        }

        public override bool UseItem(Player player)
        {
            float direction = (Main.MouseWorld - player.Center).ToRotation();
            float distance = (Main.MouseWorld - player.Center).Length();
            player.armorEffectDrawShadow = true;
            player.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;

            player.velocity = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction)) * distance / 10;

            int dust = Dust.NewDust(player.position, player.width, player.height, mod.DustType("B4PDust"), 0, 0);
            player.noFallDmg = true;
            return true;
        }
    }
}