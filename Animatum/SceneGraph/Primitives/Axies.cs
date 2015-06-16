using SharpGL;
using SharpGL.SceneGraph;

namespace Animatum.SceneGraph.Primitives
{
    class Axies : Primitive
    {
        private float size = 20.0f;
        private float lineWidth = 1.25f;

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public float LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
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
            for (float i = (size * -1); i < size; i+=0.4f)
            {
                float add = 0.2f;
                if (i > 0) add = 0.4f;
                if (i < 0 && i > -0.2f) add = 0.4f;
                gl.Color(1f, 0f, 0f, 1f);
                gl.Vertex(i, 0, 0);
                gl.Vertex(i + add, 0, 0);
                gl.Color(0f, 1f, 0f, 1f);
                gl.Vertex(0, i, 0);
                gl.Vertex(0, i + add, 0);
                gl.Color(0f, 0f, 1f, 1f);
                gl.Vertex(0, 0, i);
                gl.Vertex(0, 0, i + add);
            }
            gl.End();

            //  Restore attributes.
            gl.PopAttrib();

            displayList.End(gl);
        }
    }
}