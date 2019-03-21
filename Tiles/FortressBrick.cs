using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace QwertysRandomContent.Tiles
{
    public class FortressBrick : ModTile
    {
        
        
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            //Main.tileSpelunker[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileBlendAll[Type] = false;

            dustType = mod.DustType("FortressDust");
            soundType = 21;
            soundStyle = 2;
            minPick = 50;
            AddMapEntry(new Color(162, 184, 185));
            mineResist = 1;
            drop = mod.ItemType("FortressBrick");
            
        }


        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }
        public override bool CanExplode(int i, int j)
        {


            return true;




        }


    }
}