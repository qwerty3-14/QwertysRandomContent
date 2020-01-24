using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{


    public class Metronome : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Metronome");
            Tooltip.SetDefault("Kills grant +2% damage up to 200%" + "\nBonus resets when you take damage or switch weapons" + "\n");

        }

        public override void SetDefaults()
        {

            item.value = 300000;
            item.rare = 3;


            item.width = 20;
            item.height = 18;

            item.accessory = true;



        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            foreach (TooltipLine line in tooltips) //runs through all tooltip lines
            {
                if (line.mod == "Terraria" && line.Name == "Tooltip2") //this checks if it's the line we're interested in
                {
                    line.text = "Current Bonus: " + (Main.player[item.owner].GetModPlayer<QwertyPlayer>().killCount * 2) + "% increased damage";//change tooltip
                }

            }
        }
        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.Metronome = true;


        }



    }


}

