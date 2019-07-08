using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{


    public class BloodyMedalion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Medallion");
            Tooltip.SetDefault("80% increased magic damage" + "\nWhat normaly drains mana drains you instead!");


        }

        public override void SetDefaults()
        {


            item.rare = 1;

            item.value = 1000;
            item.width = 14;
            item.height = 14;

            item.accessory = true;



        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += .8f;
            player.GetModPlayer<BloodMedalionEffect>(mod).effect = true;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CursedMedalion"));
            recipe.AddRecipeGroup("QwertysrandomContent:EvilPowder", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }


    }
    public class BloodMedalionEffect : ModPlayer
    {
        public bool effect;
        public override void ResetEffects()
        {
            effect = false;

        }
    }
    public class BloodMedialionItemEffect : GlobalItem
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        int k;
        public override bool CanUseItem(Item item, Player player)
        {
            if(player.GetModPlayer<BloodMedalionEffect>().effect && item.mana >0)
            {
                int k = player.statMana += (int)(item.mana * Main.player[item.owner].manaCost);
                player.statLife -= (int)(item.mana * Main.player[item.owner].manaCost);
                if (player.statLife <= 0)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " madly drained " + (player.Male ? "his" : "her") + " lifeforce!"), (int)(item.mana * Main.player[item.owner].manaCost), 0);
                }
                return true;
            }
            
            return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.player[item.owner].GetModPlayer<BloodMedalionEffect>().effect)
            {
                foreach (TooltipLine line in tooltips) //runs through all tooltip lines
                {
                    if (line.mod == "Terraria" && line.Name == "UseMana") //this checks if it's the line we're interested in
                    {
                        line.text = "Uses " + (int)(item.mana * Main.player[item.owner].manaCost) + " life!";//change tooltip
                        line.overrideColor = Color.Crimson;
                    }

                }
            }
           
        }
    }


}

