using System.Drawing;
using SharpGL;
using SharpGL.SceneGraph;

namespace Animatum.SceneGraph.Primitives
{
    class Line : Primitive
    {
        private float lineWidth = 1.5f;
        private Vertex v1, v2;
        private Color c1, c2;

        public float LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        public Line(Vertex v1, Vertex v2, Color c)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.c1 = c;
            this.c2 = c;
        }

        public Line(Vertex v1, Vertex v2, Color c1, Color c2)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.c1 = c1;
            this.c2 = c2;
        }

        public void PushObjectSpace(OpenGL gl)
        {
            gl.PushMatrix();
        }

        public void PopObjectSpace(OpenGL gl)
        {
            gl.PopMatrix();
        }

        public override void Render(OpenGL gl)
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
            gl.Color(Convert.ColorToGLColor(c1));
            gl.Vertex(v1.X, v1.Y, v1.Z);
            gl.Color(Convert.ColorToGLColor(c2));
            gl.Vertex(v2.X, v2.Y, v2.Z);
            gl.End();

            //  Restore attributes.
            gl.PopAttrib();

            displayList.End(gl);
        }
    }
}
