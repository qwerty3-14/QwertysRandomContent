using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Dungeon
{
    [AutoloadEquip(EquipType.Neck)]
    public class AmuletOfPatience : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicDungeon ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amulet Of Patience");
            Tooltip.SetDefault("Deal more damage if you do haven't dealt damage in a while");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(silver: 54);
            item.rare = 2;

            item.width = 14;
            item.height = 18;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AmuletOfPatienceEffect>().effect = true;
            if (!hideVisual && player.GetModPlayer<AmuletOfPatienceEffect>().patienceCount == 180 && Main.rand.Next(6) == 0)
            {
                Dust d = Dust.NewDustPerfect(player.Center + new Vector2((2 * player.direction) + (player.direction == -1 ? -1 : 0), 0), 172);
                d.noGravity = true;
                d.velocity *= .1f;
                d.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
            }
        }
    }

    public class AmuletOfPatienceEffect : ModPlayer
    {
        public bool effect = false;
        public int patienceCount;

        public override void ResetEffects()
        {
            effect = false;
        }

        public override void PreUpdate()
        {
            if (effect && patienceCount < 180)
            {
                patienceCount++;
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (patienceCount > 60)
            {
                damage += (int)((float)damage * (((float)patienceCount - 60) / 60f));
            }
            patienceCount = 0;
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (patienceCount > 60)
            {
                damage += (int)((float)damage * (((float)patienceCount - 60) / 60f));
            }
            patienceCount = 0;
        }
    }
}