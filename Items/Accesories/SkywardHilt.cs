using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.Items.Accesories
{
    public class SkywardHilt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyward Hilt");
            Tooltip.SetDefault("Swords deal more damage while airborne");
        }
        public override void SetDefaults()
        {

            item.value = 25000;
            item.rare = 3;


            item.width = item.height = 20;

            item.accessory = true;



        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SkywardHiltEffect>().effect = true;
        }


        public class SkywardHiltEffect : ModPlayer
        {
            public bool effect;
            public override void ResetEffects()
            {
                effect = false;

            }
            public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
            {
                Point origin = player.Bottom.ToTileCoordinates();
                Point point;
                if (effect && !WorldUtils.Find(origin, Searches.Chain(new Searches.Down(3), new GenCondition[]
                                            {
                                            new Conditions.IsSolid()
                                            }), out point) && player.grappling[0] == -1)
                {
                    damage *= 2;
                }
            }
        }
    }
}
