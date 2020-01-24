using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.Vitallum
{
    public class VitallumCoreUncharged : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Core");
            Tooltip.SetDefault("Uncharged, kill enemies to charge it.\nWhen charged can be used in crafting, removing the charge. \n");
        }
        int charge = 0;
        int maxCharge = 50;
        public override void SetDefaults()
        {
            item.width = 13;
            item.height = 13;
            item.value = 10000;
            item.rare = 8;
            item.maxStack = 1;
        }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
        public override void UpdateInventory(Player player)
        {

            player.GetModPlayer<CoreManager>().gainCharges = true;
            if (player.GetModPlayer<CoreManager>().chargesToAdd > 0)
            {
                Main.NewText("Hey");
                charge += player.GetModPlayer<CoreManager>().chargesToAdd;
                player.GetModPlayer<CoreManager>().chargesToAdd = 0;
            }
            if (charge > maxCharge)
            {

                item.TurnToAir();
                player.QuickSpawnItem(mod.ItemType("VitallumCoreCharged"), 1);
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            String s = charge + "/" + maxCharge + " enemies killed.";
            foreach (TooltipLine line in tooltips) //runs through all tooltip lines
            {
                if (line.mod == "Terraria" && line.Name == "Tooltip2") //this checks if it's the line we're interested in
                {
                    line.text = s;//change tooltip
                }

            }
        }
    }
    public class VitallumCoreCharged : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vitallum Core");
            Tooltip.SetDefault("Charged, ready for crafting.");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 6));
        }
        public override void SetDefaults()
        {
            item.width = 13;
            item.height = 13;
            item.value = 10000;
            item.rare = 8;
            item.maxStack = 1;
        }
    }
    public class CoreManager : ModPlayer
    {
        public int chargesToAdd = 0;
        public bool gainCharges = false;
        public override void ResetEffects()
        {
            //chargesToAdd = 0;
            gainCharges = false;
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {

            if (gainCharges && target.life < 0)
            {
                chargesToAdd++;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (gainCharges && target.life < 0)
            {
                chargesToAdd++;
            }
        }
    }
    public class CoreDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if(npc.type == NPCID.Plantera && !Main.expertMode)
            {
                Item.NewItem(npc.Hitbox, mod.ItemType("VitallumCoreUncharged"));
            }
        }
    }
    public class CoreBagDrop : GlobalItem
    {

        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.PlanteraBossBag)
            {
                player.QuickSpawnItem(mod.ItemType("VitallumCoreUncharged"));
            }
        }
    }
}
