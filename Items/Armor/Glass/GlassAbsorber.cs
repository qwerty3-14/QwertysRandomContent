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
    [AutoloadEquip(EquipType.Body)]
    public class GlassAbsorber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glass Absorber");
            Tooltip.SetDefault("12% chance not to consume ammo\n12% reduced mana usage");

        }


        public override void SetDefaults()
        {

            item.value = 10000;
            item.rare = 1;


            item.width = 22;
            item.height = 12;
            item.defense = 5;



        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<QwertyPlayer>(mod).ammoReduction *= .88f;
            player.manaCost *= .88f;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 45);
            recipe.AddRecipeGroup("QwertysrandomContent:SilverBar", 8);
            recipe.AddTile(TileID.GlassKiln);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }



        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawArms = true;
            drawHands = true;

        }






    }
    public class AbsorberDraw : ModPlayer
    {
        public static readonly PlayerLayer GlassBack = new PlayerLayer("QwertysRandomContent", "GlassBack", PlayerLayer.BackAcc, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            //Main.NewText(drawPlayer.wings);
            if (drawPlayer.body == mod.GetEquipSlot("GlassAbsorber", EquipType.Body) && drawPlayer.back == -1 && drawPlayer.wings == 0)
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Glass/GlassAbsorber_Body_Glass");
                /*
                if (!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/Armor/Robes/ReagalCape_Female");
                }*/
                DrawData value = new DrawData(texture, 
                    new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), new Microsoft.Xna.Framework.Rectangle?(drawPlayer.bodyFrame), color12, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);

                value.shader = (int)drawPlayer.dye[3].dye;
                Main.playerDrawData.Add(value);

            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int capeLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("BackAcc"));
            if (capeLayer != -1)
            {
                GlassBack.visible = true;
                layers.Insert(capeLayer + 1, GlassBack);
            }
        }

    }

}

