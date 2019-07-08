using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.HydraItems
{
	[AutoloadEquip(EquipType.Legs)]
	public class HydraLeggings : ModItem
	{
		public override bool Autoload(ref string name)
        {
			if (!Main.dedServ)
                {
                    // Add the female leg variant
                    //mod.AddEquipTexture(null, EquipType.Legs, "HydraLeggings_Female", "QwertysRandomContent/Items/HydraItems/HydraLeggings_FemaleLegs");
                }
				return true;
			
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Leggings");
			Tooltip.SetDefault("+1 life/sec regen rate" + "\n+10% summon damage and movement speed");
			
		}
		

		public override void SetDefaults()
		{
			
			item.value = 50000;
			item.rare = 5;
			
			
			item.width = 22;
			item.height = 18;
			item.defense = 14;
			
			
			
		}
		
		public override void UpdateEquip(Player player)
		{
			
			player.lifeRegen += 2;
			player.minionDamage  += .1f;
			player.moveSpeed += .1f;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
			if (male) equipSlot = mod.GetEquipSlot("HydraLeggings", EquipType.Legs);
            if (!male) equipSlot = mod.GetEquipSlot("HydraLeggings_Female", EquipType.Legs);
		}
		
		
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("HydraScale"), 18);
			
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
			
	}
    public class HydraLeggingsGlowmask : ModPlayer
    {
        /*
        public override void PreUpdate()
        {
            Main.NewText("Legs: " +player.legs);
            Main.NewText("male hydra legs: " + mod.GetEquipSlot("HydraLeggings", EquipType.Legs));
            Main.NewText("female hydra legs: " + mod.GetEquipSlot("HydraLeggings_Female", EquipType.Legs));
        }
        */
        public static readonly PlayerLayer HydraShoes = new PlayerLayer("QwertysRandomContent", "HydraShoes", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("QwertysRandomContent");
            //ExamplePlayer modPlayer = drawPlayer.GetModPlayer<ExamplePlayer>(mod);
            if (drawPlayer.legs == mod.GetEquipSlot("HydraLeggings", EquipType.Legs) || drawPlayer.legs == mod.GetEquipSlot("HydraLeggings_Female", EquipType.Legs))
            {
                //Main.NewText("Legs!");
                //Main.NewText(drawPlayer.bodyFrame);
                Texture2D texture = mod.GetTexture("Items/HydraItems/HydraLeggings_Legs_Glow");
                if (!drawPlayer.Male)
                {
                    texture = mod.GetTexture("Items/HydraItems/HydraLeggings_FemaleLegs_Glow");
                }
                
                
                int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
                int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
                Vector2 Position = drawInfo.position;
                Vector2 origin = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
                Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                //pos.Y -= drawPlayer.mount.PlayerOffset;
                DrawData data = new DrawData(texture, pos, drawPlayer.legFrame, Color.White, 0f, origin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.legArmorShader;
                Main.playerDrawData.Add(data);
            }
        });
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int legLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Legs"));
            if (legLayer != -1)
            {
                HydraShoes.visible = true;
                layers.Insert(legLayer + 1, HydraShoes);
            }

        }
    }

}

