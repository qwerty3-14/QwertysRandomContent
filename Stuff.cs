using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;

namespace QwertysRandomContent
{
    public class QwertyMethods
    {
        public static float SlowRotation(float currentRotation, float targetAngle, float speed)
        {
            int f = 1; //this is used to switch rotation direction
            float actDirection =  new Vector2((float)Math.Cos(currentRotation), (float)Math.Sin(currentRotation)).ToRotation();
            targetAngle = new Vector2((float)Math.Cos(targetAngle), (float)Math.Sin(targetAngle)).ToRotation();

            //this makes f 1 or -1 to rotate the shorter distance
            if (Math.Abs(actDirection - targetAngle) > Math.PI)
            {
                f = -1;
            }
            else
            {
                f = 1;
            }

            if (actDirection <= targetAngle + MathHelper.ToRadians(speed * 2) && actDirection >= targetAngle - MathHelper.ToRadians(speed * 2))
            {
                actDirection = targetAngle;
            }
            else if (actDirection <= targetAngle)
            {
                actDirection += MathHelper.ToRadians(speed) * f;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= MathHelper.ToRadians(speed) * f;
            }
            actDirection = new Vector2((float)Math.Cos(actDirection), (float)Math.Sin(actDirection)).ToRotation();
            
            return actDirection;
        }
        public static Vector2 PolarVector(float radius, float theta)
        {

            return new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
        }

        public static void BreakTiles(int i, int j, int width, int height)
        {
            for(int x =0; x < width; x++)
            {
                for(int y =0; y<height; y++)
                {
                    WorldGen.KillTile(i+x, j+y, false, false, true);
                    WorldGen.KillWall(i + x, j + y, false);
                    Main.tile[i + x, j + y].liquid = 0;
                }

            }
        }
        public static bool ClosestNPC(ref NPC target, float maxDistance, Vector2 position, bool ignoreTiles = false, int overrideTarget =-1)
        {
            bool foundTarget = false;
            if(overrideTarget != -1)
            {
                if ((Main.npc[overrideTarget].Center - position).Length() < maxDistance)
                {
                    target = Main.npc[overrideTarget];
                    return true;
                }
                
            }
            for (int k = 0; k < 200; k++)
            {
                NPC possibleTarget = Main.npc[k];
                float distance = (possibleTarget.Center - position).Length();
                if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0)|| ignoreTiles))
                {
                    target = Main.npc[k];
                    foundTarget = true;


                    maxDistance = (target.Center - position).Length();
                }

            }
            return foundTarget;
        }
        /*
        private struct ColorTriplet
        {
            public float r;

            public float g;

            public float b;

            public ColorTriplet(float R, float G, float B)
            {
                this.r = R;
                this.g = G;
                this.b = B;
            }

            public ColorTriplet(float averageColor)
            {
                this.b = averageColor;
                this.g = averageColor;
                this.r = averageColor;
            }
        }
        private static int firstTileX = 0;
        private static int firstTileY = 0;
        private static int maxTempLights = 2000;
		private static Dictionary<Point16, ColorTriplet> tempLights;
        public static void AntiLight(Vector2 position, Vector3 rgb)
        {
            Lighting.AddLight((int)(position.X / 16f), (int)(position.Y / 16f), rgb.X, rgb.Y, rgb.Z);
        }

        public static void AntiLight(Vector2 position, float R, float G, float B)
        {
            Lighting.AddLight((int)(position.X / 16f), (int)(position.Y / 16f), R, G, B);
        }
        public static void AntiLight(int i, int j, float R, float G, float B)
        {
            if (Main.gamePaused)
            {
                return;
            }
            if (Main.netMode == 2)
            {
                return;
            }
            if (i - firstTileX + Lighting.offScreenTiles >= 0 && i - firstTileX + Lighting.offScreenTiles < Main.screenWidth / 16 + Lighting.offScreenTiles * 2 + 10 && j - firstTileY + Lighting.offScreenTiles >= 0 && j - firstTileY + Lighting.offScreenTiles < Main.screenHeight / 16 + Lighting.offScreenTiles * 2 + 10)
            {
                if (tempLights.Count == maxTempLights)
                {
                    return;
                }
                Point16 key = new Point16(i, j);
                ColorTriplet value;
                if (tempLights.TryGetValue(key, out value))
                {
                    if (Lighting.RGB)
                    {
                        if (value.r < R)
                        {
                            value.r = R;
                        }
                        if (value.g < G)
                        {
                            value.g = G;
                        }
                        if (value.b < B)
                        {
                            value.b = B;
                        }
                        tempLights[key] = value;
                        return;
                    }
                    float num = (R + G + B) / 3f;
                    if (value.r < num)
                    {
                        tempLights[key] = new ColorTriplet(num);
                        return;
                    }
                }
                else
                {
                    if (Lighting.RGB)
                    {
                        value = new ColorTriplet(R, G, B);
                    }
                    else
                    {
                        value = new ColorTriplet((R + G + B) / 3f);
                    }
                    tempLights.Add(key, value);
                }
            }
        }*/
    }

}
