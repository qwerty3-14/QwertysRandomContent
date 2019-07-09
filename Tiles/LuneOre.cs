using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace QwertysRandomContent.Tiles
{
	public class LuneOre: ModTile
{
    public override void SetDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
		Main.tileSpelunker[Type] = true;
        Main.tileValue[Type]=325;

		ModTranslation name = CreateMapEntryName();
        dustType = mod.DustType("LuneDust");
        soundType = 21;
        soundStyle = 2;
        
        AddMapEntry(new Color(102, 143, 204), name);
		name.SetDefault("Lune Ore");
        drop = mod.ItemType("LuneOre");
            
            
                minPick = 1;
            
        }
	

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        r = 0.5f;
        g = 0.5f;
        b = 0.5f;
    }
	 public override bool CanExplode(int i, int j)
	{
		
		if(!NPC.downedBoss3)
	    {
		   return false;
	    }
		else
		{
			return true;
		}
	    
		
		
	}

  
}}