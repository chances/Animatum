using System;
using SharpGL;
using SharpGL.SceneGraph;
using System.Drawing;
using SharpGL.SceneGraph.Assets;

namespace Animatum.SceneGraph.Primitives
{
    class Sphere : Primitive
    {
        private Vertex pos;
        private double r;
        private int subdivs;
        private Material material;

        public Sphere(Vertex position, double radius, int subdivisions, Color color)
        {
            pos = position;
            r = radius;
            subdivs = subdivisions;
            this.material = new Material();
            this.material.Emission = color;
        }

        public Color Color
        {
            get
            {
                if (material == null)
                    return Color.White;
                else
                    return material.Emission;
            }
            set
            {
                if (material == null)
                    material = new Material();
                material.Emission = value;
            }
        }

        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        public void PushObjectSpace(OpenGL gl)
        {
            gl.PushMatrix();
            gl.Translate(pos.X, pos.Y, pos.Z);
            if (material != null)
                material.Push(gl);
        }

        public void PopObjectSpace(OpenGL gl)
        {
            if (material != null)
                material.Pop(gl);
            gl.PopMatrix();
        }

        public override void Render(OpenGL gl)
        {
            /*if (displayList == null)
                CreateDisplayList(gl);
            else
                displayList.Call(gl);*/

            //Create sphere quadric
            IntPtr quadric = gl.NewQuadric();
            gl.QuadricNormals(quadric, OpenGL.GLU_SMOOTH);

            //Render the sphere
            gl.Sphere(quadric, r, subdivs, subdivs);

            //Delete the sphere
            gl.DeleteQuadric(quadric);
        }

        private void CreateDisplayList(OpenGL gl)
        {
            displayList = new DisplayList();

            displayList.Generate(gl);
            displayList.New(gl, DisplayList.DisplayListMode.CompileAndExecute);

            //Create sphere quadric
            IntPtr quadric = gl.NewQuadric();
            gl.QuadricNormals(quadric, OpenGL.GLU_SMOOTH);

            //Render the sphere
            gl.Sphere(quadric, r, subdivs, subdivs);

            //Delete the sphere
            gl.DeleteQuadric(quadric);

            displayList.End(gl);
        }
    }
}