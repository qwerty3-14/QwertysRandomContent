using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace QwertysRandomContent.Items.Armor.Glass
{
    [AutoloadEquip(EquipType.Legs)]
    public class GlassLimbguards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Limbguards");
            Tooltip.SetDefault("Walk right for 12% increased ranged damage\nWalk left for 12% increased magic damage");

        }
       
        

        public override void SetDefaults()
        {

            item.value = 10000;
            item.rare = 1;


            item.width = 22;
            item.height = 12;
            item.defense = 4;



        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 30);
            recipe.AddRecipeGroup("QwertysrandomContent:SilverBar", 6);
            recipe.AddTile(TileID.GlassKiln);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override bool DrawLegs()
        {
            return false;
        }
        public override void UpdateEquip(Player player)
        {
            if(player.velocity.X > 0)
            {
                player.rangedDamage += .12f;
            }
            else if(player.velocity.X <0)
            {
                player.magicDamage += .12f;
            }
        }
      










    }
    public class GlassLimbguardsFemale : EquipTexture
    {
        public override bool DrawLegs()
        {
            return false;
        }
    }
    public class LimbguardDraw : ModPlayer
    {
        public static readonly PlayerLayer GlassLegs = new PlayerLayer("QwertysRandomContent", "GlassLegs", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");

            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.legs == mod.GetEquipSlot("GlassLimbguards", EquipType.Legs) )
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Glass/GlassLimbguards_Legs_Glass");
              
                Vector2 Position = drawInfo.position;
                Position.Y += 14;
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                DrawData value = new DrawData(texture, pos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), color12, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = (int)drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(value);

            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                GlassLegs.visible = true;
                layers.Insert(legLayer + 1, GlassLegs);
            }
        }
        }

}

