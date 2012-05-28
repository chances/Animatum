using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph;
using SharpGL;
using Lighting = SharpGL.SceneGraph.Lighting;
using System.Drawing;
using System.Web.Script.Serialization;

namespace Animatum.SceneGraph
{
    class Light : Node
    {
        private Lighting.Light light;

        public Light()
        {
            //Nothing
        }

        public Light(uint GLCode)
        {
            light = new Lighting.Light();
            light.On = true;
            light.GLCode = GLCode;
        }

        [ScriptIgnore()]
        public bool On
        {
            get { return light.On; }
            set { light.On = value; }
        }

        [ScriptIgnore()]
        public Vertex Position
        {
            get { return light.Position; }
            set { light.Position = value; }
        }

        [ScriptIgnore()]
        public Color Ambient
        {
            get { return light.Ambient; }
            set { light.Ambient = value; }
        }

        [ScriptIgnore()]
        public Color Diffuse
        {
            get { return light.Diffuse; }
            set { light.Diffuse = value; }
        }

        [ScriptIgnore()]
        public Color Specular
        {
            get { return light.Specular; }
            set { light.Specular = value; }
        }

        public override void Render(OpenGL gl)
        {
            light.Push(gl);
        }

        public void Pop(OpenGL gl)
        {
            light.Pop(gl);
        }
    }
}