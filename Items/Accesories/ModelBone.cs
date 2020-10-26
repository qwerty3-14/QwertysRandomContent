using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class ModelBone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Model Bone");
            Tooltip.SetDefault("A big sturdy bone, basing your moprhs on this should improve thier defense");
        }

        public override void SetDefaults()
        {
            item.value = 10000;
            item.rare = 1;
            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 10;
        }


    }
    public class ModelBoneDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if(npc.type == NPCID.AngryBones || npc.type == NPCID.DarkCaster)
            {
                if(Main.rand.Next(100) == 0)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("ModelBone"));
                }
            }
        }
    }
}
