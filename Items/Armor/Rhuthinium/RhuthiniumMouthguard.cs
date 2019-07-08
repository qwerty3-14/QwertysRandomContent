using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace QwertysRandomContent.Items.Armor.Rhuthinium
{
    [AutoloadEquip(EquipType.Head)]
    public class RhuthiniumMouthguard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Mouthguard");
            Tooltip.SetDefault("+1 max sentries" + "\n15% increased minion and throwing damage");

        }


        public override void SetDefaults()
        {

            item.value = 50000;
            item.rare = 3;


            item.width = 22;
            item.height = 12;
            item.defense = 2;



        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamage += .15f;
            player.thrownDamage += .15f;
            player.maxTurrets += 1;


        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("RhuthiniumChestplate") && legs.type == mod.ItemType("RhuthiniumGreaves");

        }



        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.QwertysRandomContent.RMouthguardkSet");
            
            for (int p = 0; p < 1000; p++)
            {
                if (Main.projectile[p].active && Main.projectile[p].sentry && Main.projectile[p].owner == player.whoAmI && (Main.projectile[p].Center - player.Center).Length() < 300)
                {
                    player.thrownDamage += .1f;
                    player.thrownVelocity += 1f;
                    break;
                }
            }

        }




        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 12);

            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
    

}

