using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.ShapeShifter
{
    public class NewClassDamageTest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This is a modded sword.");  //The (English) text shown below your weapon's name
        }

        public override void SetDefaults()
        {
            item.damage = 50;           //The damage of your weapon
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.thrown = false;
            item.summon = false;
            item.GetGlobalItem<ShapeShifterItem>().morph = true;
            item.width = 40;            //Weapon's texture's width
            item.height = 40;           //Weapon's texture's height
            item.useTime = 20;          //The time span of using the weapon. Remember in terraria, 60 frames is a second.
            item.useAnimation = 20;         //The time span of the using animation of the weapon, suggest set it the same as useTime.
            item.useStyle = 1;          //The use style of weapon, 1 for swinging, 2 for drinking, 3 act like shortsword, 4 for use like life crystal, 5 for use staffs or guns
            item.knockBack = 6;         //The force of knockback of the weapon. Maximum is 20
            item.value = Item.buyPrice(gold: 1);           //The value of the weapon
            item.rare = 2;              //The rarity of the weapon, from -1 to 13
            item.UseSound = SoundID.Item1;      //The sound when the weapon is using
            item.autoReuse = true;          //Whether the weapon can use automatically by pressing mousebutton
        }
    }
}