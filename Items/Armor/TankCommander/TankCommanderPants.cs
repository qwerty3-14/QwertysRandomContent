using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Armor.TankCommander
{
    [AutoloadEquip(EquipType.Legs)]
    public class TankCommanderPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tank Commander Pants");
            Tooltip.SetDefault("Minions do 20% more damage when you're morphed \nThe tank morph gains the ability to fly short distances!");

        }
        public override void SetDefaults()
        {

            item.value = 100000;
            item.rare = 1;


            item.width = 22;
            item.height = 12;
            item.defense = 1;
            item.GetGlobalItem<ShapeShifterItem>().equipedMorphDefense = 8;


        }


        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<TankComPantsEffects>().effect = true;
        }

    }

    public class TankComPantsEffects : ModPlayer
    {
        public bool effect;
        public override void ResetEffects()
        {
            effect = false;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (effect && player.GetModPlayer<ShapeShifterPlayer>().morphed && proj.minion)
            {
                damage = (int)(damage * 1.2f);
            }
        }
    }
}

