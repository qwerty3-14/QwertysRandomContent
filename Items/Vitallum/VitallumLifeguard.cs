using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vitallum
{
    [AutoloadEquip(EquipType.Body)]
    public class VitallumLifeguard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Lifeguard (WIP)");
            Tooltip.SetDefault("Increases max life by 150 \n25% increased throwing damage");
        }
        public override void SetDefaults()
        {
            item.rare = 8;
            item.value = Item.sellPrice(gold: 6);
        }
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 150;
            player.thrownDamage += .25f;
        }
        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;
        }
    }
}
