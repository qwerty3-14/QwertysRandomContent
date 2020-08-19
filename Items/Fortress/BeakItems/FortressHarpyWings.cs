using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.BeakItems
{
    [AutoloadEquip(EquipType.Wings)]
    public class FortressHarpyWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows flight and slow fall" + "\nHold down to hover" + "\nAllows you to cling onto walls");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 36;
            item.value = Item.sellPrice(gold: 8);
            item.rare = 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<Hover>().hasHoverWing = true;
            player.wingTimeMax = 100;
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.customDashSpeed < 5f)
            {
                modPlayer.customDashSpeed = 5f;
            }
            player.spikedBoots = 2;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.5f;
            ascentWhenRising = 0.1f;
            maxCanAscendMultiplier = .5f;
            maxAscentMultiplier = 1.8f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            if (player.controlDown && player.controlJump && player.wingTime > 0f)
            {
                if (!player.controlLeft && !player.controlRight)
                {
                    player.wingTime += .8f;
                }
                speed = 8f;
                acceleration *= 8f;
            }
            else
            {
                speed = 6f;
            }
        }

        public override bool WingUpdate(Player player, bool inUse)
        {
            return base.WingUpdate(player, inUse);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("FortressHarpyFeather"));
            recipe.AddIngredient(ItemID.SoulofFlight, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class Hover : ModPlayer
    {
        public bool hasHoverWing = false;

        public override void ResetEffects()
        {
            hasHoverWing = false;
        }

        public override void PostUpdateEquips()
        {
            if (hasHoverWing && player.controlDown && player.controlJump && player.wingTime > 0f && !player.merman)
            {
                player.velocity.Y = player.velocity.Y * 0.9f;
                if (player.velocity.Y > -2f && player.velocity.Y < 1f)
                {
                    player.velocity.Y = 1E-05f;
                    player.position.Y += .1f;
                }
            }
        }
    }
}