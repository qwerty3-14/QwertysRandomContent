using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    public class ConspiratorEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Conspirator Emblem");
            Tooltip.SetDefault("15% increased morph damage");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.accessory = true;
            item.value = 100000;
            item.rare = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShapeShifterPlayer>().morphDamage += .15f;
        }
    }

    public class EmblemBagDrop : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.WallOfFleshBossBag)
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        player.QuickSpawnItem(mod.ItemType("ConspiratorEmblem"));
                        break;
                }
            }
        }
    }

    public class EmblemDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh && !Main.expertMode)
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        Item.NewItem(npc.Hitbox, mod.ItemType("ConspiratorEmblem"));
                        break;
                }
            }
        }
    }
}