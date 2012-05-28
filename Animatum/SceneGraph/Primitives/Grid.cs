using System.Drawing;
using SharpGL;
using SharpGL.SceneGraph;

namespace Animatum.SceneGraph.Primitives
{
    class Grid : Primitive
    {
        private float lineWidth = 1.0f;
        private int size = 20;
        private Color lightColor = Color.FromArgb(50,50,50);
        private Color darkColor = Color.DarkGray;

        public float LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public Color LightColor
        {
            get { return lightColor; }
            set { lightColor = value; }
        }

        public Color DarkColor
        {
            get { return darkColor; }
            set { darkColor = value; }
        }

        public override void Render(SharpGL.OpenGL gl)
        {
            if (displayList == null)
                CreateDisplayList(gl);
            else
                displayList.Call(gl);
        }

        private void CreateDisplayList(OpenGL gl)
        {
            displayList = new DisplayList();

            displayList.Generate(gl);
            displayList.New(gl, DisplayList.DisplayListMode.CompileAndExecute);

            //Push all attributes, disable lighting and depth testing.
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT | OpenGL.GL_ENABLE_BIT |
                OpenGL.GL_LINE_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.DepthFunc(OpenGL.GL_ALWAYS);

            //Set line width.
            gl.LineWidth(lineWidth);

            //Draw the line.
            gl.Begin(OpenGL.GL_LINES);
            for (int i = (size * -1); i <= size; i++)
            {
                if (i != 0)
                {
                    if ((i % 4) == 0)
                        gl.Color(Convert.ColorToGLColor(darkColor));
                    else
                        gl.Color(Convert.ColorToGLColor(lightColor));
                    gl.Vertex(i, (size * -1), 0);
                    gl.Vertex(i, size, 0);
                    gl.Vertex((size * -1), i, 0);
                    gl.Vertex(size, i, 0);
                }
            }
            gl.End();

            //  Restore attributes.
            gl.PopAttrib();

            displayList.End(gl);
        }
    }
}