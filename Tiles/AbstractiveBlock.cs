using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace QwertysRandomContent.Tiles
{
	public class Abstractive: ModTile
{
    public override void SetDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
		//Main.tileSpelunker[Type] = true;

		
        dustType = mod.DustType("AbstractiveDust");
        soundType = 21;
        soundStyle = 2;
        minPick = 50; 
        AddMapEntry(new Color(47, 240, 240));
		
        drop = mod.ItemType("Abstractive");
		
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

  
}}