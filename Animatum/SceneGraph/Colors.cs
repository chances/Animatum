using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace Animatum.SceneGraph
{
    public class Colors
    {
        public static Color[] ColorPool =
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Purple,
            Color.Orange,
            Color.Yellow,
            Color.Cyan,
            Color.Fuchsia,
            Color.Brown,
            Color.Pink,
            Color.Silver,
            Color.Teal,
            Color.DarkGray,
            Color.OrangeRed,
            Color.Indigo,
            Color.Khaki,
            Color.SeaGreen,
            Color.Gray,
            Color.GreenYellow,
            Color.PaleVioletRed,
            Color.PaleGreen,
            Color.BlueViolet,
            Color.LimeGreen,
            Color.Goldenrod,
            Color.Salmon,
            Color.Aqua,
            Color.CornflowerBlue,
            Color.Coral,
            Color.RosyBrown
        };

        public static Color Random()
        {
            Random randomGen = new System.Random();
            int randIndex = randomGen.Next(ColorPool.Length);
            Thread.Sleep(10);
            return ColorPool[randIndex];
        }

        public static Color RandomKnownColor()
        {
            Random randomGen = new System.Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            return Color.FromKnownColor(randomColorName);
        }
    }
}