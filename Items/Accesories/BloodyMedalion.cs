using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class BloodyMedalion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Medallion");
            Tooltip.SetDefault("Doubles magic damage" + "\nWhat normaly drains mana drains you instead!" + "\nLess effective with certain weapons");
        }

        public override void SetDefaults()
        {
            item.rare = 1;

            item.value = 1000;
            item.width = 14;
            item.height = 14;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BloodMedalionEffect>().effect = true;
        }
    }

    public class BloodMedalionEffect : ModPlayer
    {
        public bool effect;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override bool PreItemCheck()
        {
            if (effect)
            {
                player.spaceGun = false;
                if(player.HeldItem.type == ItemID.CrimsonRod || player.HeldItem.type == ItemID.NimbusRod || player.HeldItem.type == ItemID.MagnetSphere)
                {
                    player.magicDamage = (int)(player.magicDamage  * 1.4f);
                }
                else
                {
                    player.magicDamage *= 2;
                }
                
            }
            return base.PreItemCheck();
        }
    }

    public class BloodMedialionItemEffect : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;
        private int k;

        public override bool CanUseItem(Item item, Player player)
        {
            if (player.GetModPlayer<BloodMedalionEffect>().effect && item.mana > 0)
            {
                int lifeCost = (int)(item.mana * Main.LocalPlayer.manaCost);
                if (lifeCost < 0)
                {
                    lifeCost = 0;
                }
                player.statLife -= lifeCost;
                if (player.statLife <= 0)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + Language.GetTextValue("Mods.QwertysRandomContent.BloodyMedalionInfo1") + (player.Male ? Language.GetTextValue("Mods.QwertysRandomContent.his") : Language.GetTextValue("Mods.QwertysRandomContent.her")) + Language.GetTextValue("Mods.QwertysRandomContent.BloodyMedalionInfo2")), (int)(item.mana * player.manaCost), 0);
                }
                return true;
            }

            return base.CanUseItem(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.GetModPlayer<BloodMedalionEffect>().effect)
            {
                foreach (TooltipLine line in tooltips) //runs through all tooltip lines
                {
                    if (line.mod == "Terraria" && line.Name == "UseMana") //this checks if it's the line we're interested in
                    {
                        int lifeCost = (int)(item.mana * Main.LocalPlayer.manaCost);
                        if(lifeCost <0)
                        {
                            lifeCost = 0;
                        }
                        line.text = Language.GetTextValue("Mods.QwertysRandomContent.BloodyMedalionInfo3") + lifeCost + Language.GetTextValue("Mods.QwertysRandomContent.BloodyMedalionInfo4");//change tooltip
                        line.overrideColor = Color.Crimson;
                    }
                }
            }
        }
    }
}