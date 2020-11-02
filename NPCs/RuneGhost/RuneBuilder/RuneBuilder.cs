using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.RuneGhost.RuneBuilder
{
    public static class RuneSprites
    {
        public static Texture2D[] runeCycle;
        public static Texture2D[] runes = new Texture2D[4];
        public static Texture2D[][] runeTransition = new Texture2D[4][];

        public static Texture2D[] bigRunes = new Texture2D[4];
        public static Texture2D[][] bigRuneTransition = new Texture2D[4][];

        public static Texture2D runeGhostMoving;
        public static Texture2D[] runeGhostPhaseIn;

        public static Texture2D[] aggroStrike;

        static Texture2D[] runesToCycle = new Texture2D[4];
        static Texture2D[] runeBases = new Texture2D[4];

        public static void BuildRunes()
        {
            runeCycle = new Texture2D[80];
            runesToCycle[0] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/AggroRune");
            runesToCycle[1] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/LeechRune_WhiteSpaced");
            runesToCycle[2] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/IceRune_WhiteSpaced");
            runesToCycle[3] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/PursuitRune_WhiteSpaced");
            Texture2D[] runesToAdd = TextureBuilder.TransitionFrames(runesToCycle[0], runesToCycle[1], 20);
            for (int k = 0; k < 20; k++)
            {
                runeCycle[k] = runesToAdd[k];
            }
            runesToAdd = TextureBuilder.TransitionFrames(runesToCycle[1], runesToCycle[2], 20);
            for (int k = 0; k < 20; k++)
            {
                runeCycle[k + 20] = runesToAdd[k];
            }
            runesToAdd = TextureBuilder.TransitionFrames(runesToCycle[2], runesToCycle[3], 20);
            for (int k = 0; k < 20; k++)
            {
                runeCycle[k + 40] = runesToAdd[k];
            }
            runesToAdd = TextureBuilder.TransitionFrames(runesToCycle[3], runesToCycle[0], 20);
            for (int k = 0; k < 20; k++)
            {
                runeCycle[k + 60] = runesToAdd[k];
            }

            runes[0] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/AggroRune");
            runes[1] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/LeechRune");
            runes[2] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/IceRune");
            runes[3] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/PursuitRune");
            runeBases[0] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/AggroRune_Base");
            runeBases[1] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/LeechRune_Base");
            runeBases[2] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/IceRune_Base");
            runeBases[3] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/PursuitRune_Base");
            for (int i = 0; i < 4; i++)
            {
                runeTransition[i] = TextureBuilder.TransitionFrames(runeBases[i], runes[i], 20);
            }

            bigRunes[0] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/RedRune");
            bigRunes[1] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/GreenRune");
            bigRunes[2] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/CyanRune");
            bigRunes[3] = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/PurpleRune");
            Texture2D bigBaseRune = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/BigRune_Base");
            for (int i = 0; i < 4; i++)
            {
                bigRuneTransition[i] = TextureBuilder.TransitionFrames(bigBaseRune, bigRunes[i], 20);
            }

            runeGhostMoving = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/RuneGhost");
            Texture2D runeGhostBase = QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/RuneGhost_Base");
            runeGhostPhaseIn = TextureBuilder.TransitionFrames(runeGhostBase, runeGhostMoving, 20);

            aggroStrike = TextureBuilder.TransitionFrames(QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/AggroStrike_Base"), QwertysRandomContent.Instance.GetTexture("NPCs/RuneGhost/RuneBuilder/AggroStrike"), 4);
        }
    }
    public enum Runes : byte
    {
        Aggro = 0,
        Leech = 1,
        IceRune = 2,
        Pursuit = 3
    }
}
