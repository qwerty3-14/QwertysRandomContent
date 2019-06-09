using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace QwertysRandomContent.Items.Armor.Duelist
{
    [AutoloadEquip(EquipType.Legs)]
    public class DuelistPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duelist Pants");
            Tooltip.SetDefault("Melee attacks reduce the cooldown on quick morphs \n4% increased melee damage");

        }
        public override bool Autoload(ref string name)
        {
            if (!Main.dedServ)
            {
                // Add the female leg variant
                mod.AddEquipTexture(new DuelistLegs(), null, EquipType.Legs, "DuelistPants_Legs", "QwertysRandomContent/Items/Armor/Duelist/DuelistPants_Legs");
                mod.AddEquipTexture(new DuelistLegsFemale(), null, EquipType.Legs, "DuelistPants_FemaleLegs", "QwertysRandomContent/Items/Armor/Duelist/DuelistPants_FemaleLegs");
               
            }
            return true;

        }

        public override void SetDefaults()
        {

            item.value = 50000;
            item.rare = 1;
            item.width = 22;
            item.height = 12;
            item.defense = 5;
           


        }
        public override bool DrawLegs()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<DuelistEffects>().legs = true;
            player.meleeDamage += .04f;
        }
        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            if (male) equipSlot = mod.GetEquipSlot("DuelistPants_Legs", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("DuelistPants_FemaleLegs", EquipType.Legs);
        }


    }
    public class DuelistLegs : EquipTexture
    {
    }

    public class DuelistLegsFemale : EquipTexture
    {
        public override bool DrawLegs()
        {
            return false;
        }
    }
    public class DuelestRobeDrawing : ModPlayer
    {
        
        public static readonly PlayerLayer Pants = new PlayerLayer("QwertysRandomContent", "Pants", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), 0f);
            if (drawPlayer.legs == mod.GetEquipSlot("DuelistPants_Legs", EquipType.Legs)|| drawPlayer.legs == mod.GetEquipSlot("DuelistPants_FemaleLegs", EquipType.Legs))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/Duelist/DuelistRobeFront");
                if (!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/Armor/Duelist/DuelistRobeFront_Female");
                }
                Vector2 Position = drawInfo.position;
                Position.Y += 14;
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                DrawData value = new DrawData(texture, pos, new Microsoft.Xna.Framework.Rectangle?(drawPlayer.legFrame), color12, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
                value.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(value);

                
               

            }
        });
       
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
           
            
            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                Pants.visible = true;
                layers.Insert(legLayer + 1, Pants);
            }
        }
    }

}

