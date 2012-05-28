using System.Drawing;
using SharpGL.SceneGraph;

namespace Animatum
{
    class Convert
    {
        public static GLColor ColorToGLColor(Color c)
        {
            GLColor col = new GLColor();
            col.ColorNET = c;
            return col;
        }
    }
}