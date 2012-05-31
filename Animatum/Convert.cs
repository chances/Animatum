using System.Drawing;
using SharpGL.SceneGraph;
using System;

namespace Animatum
{
    class Convert
    {
        public static float degreesToRadians(float degrees)
        {
            return (float)(degrees * (Math.PI / 180.0));
        }

        public static float radiansToDegrees(float radians)
        {
            return (float)(radians * (180.0 / Math.PI));
        }

        public static GLColor ColorToGLColor(Color c)
        {
            GLColor col = new GLColor();
            col.ColorNET = c;
            return col;
        }
    }
}