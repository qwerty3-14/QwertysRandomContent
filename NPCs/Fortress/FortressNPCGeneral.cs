using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.Fortress
{
    public class FortressNPCGeneral : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public bool fortressNPC = false;

    }
}
