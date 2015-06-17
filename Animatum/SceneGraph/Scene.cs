using Animatum.SceneGraph.Primitives;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A 3D scene, the root of a scene graph.
    /// </summary>
    public class Scene : Node
    {
        private MovingLookAtCamera camera;
        private Size viewportSize;
        private Grid grid;
        private Axies axies;
        private OpenGLAttributesEffect attrs;
        private bool postprocessingEnabled;

        private Model model;

        private OpenGL gl;

        /// <summary>
        /// Construct a new scene, optionally enableing post-processing, if available.
        /// </summary>
        /// <param name="postprocessingEnabled">Whether or not to enable post-processing; defaults to false</param>
        public Scene(OpenGL gl, Size viewportSize)
        {
            this.gl = gl;
            this.viewportSize = viewportSize;

            model = null;

            postprocessingEnabled = false;

            ClearColor = Color.Black;
            camera = new MovingLookAtCamera()
            {
                Position = new Vertex(10f, 0f, 0f),
                Target = new Vertex(0f, 0f, 0f),
                UpVector = new Vertex(0f, 0f, 1f),
                Near = 0,
                Far = 250,
                HorizontalTheta = 0.785f,
                VerticalTheta = -0.785f
            };
            RenderGrid = true;
            RenderAxies = true;
            grid = new Grid();
            grid.Size = 150;
            axies = new Axies();
            axies.Size = 150;
            //Set attributes
            attrs = new OpenGLAttributesEffect();
            attrs.EnableAttributes.EnableDepthTest = true;
            attrs.EnableAttributes.EnableNormalize = true;
            attrs.EnableAttributes.EnableLighting = true;
            attrs.EnableAttributes.EnableTexture2D = true;
            attrs.LightingAttributes.TwoSided = false;

            Color col = Color.FromArgb(40, 40, 40);

            // Nice soft-ish lighting
            Light light = new Light(OpenGL.GL_LIGHT0)
            {
                Position = new Vertex(-9, -9, 11),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            this.Children.Add(light);
            light = new Light(OpenGL.GL_LIGHT1)
            {
                Position = new Vertex(9, -9, 11),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            this.Children.Add(light);
            light = new Light(OpenGL.GL_LIGHT2)
            {
                Position = new Vertex(0, 15, 15),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            this.Children.Add(light);
        }

        public Size ViewportSize
        {
            get { return viewportSize; }
            set
            {
                ResizeViewport(value);
            }
        }

        public Color ClearColor { get; set; }

        public MovingLookAtCamera Camera
        {
            get { return camera; }
        }

        public bool RenderGrid { get; set; }

        public bool RenderAxies { get; set; }

        public Model Model
        {
            get { return model; }
        }

        public bool PostProcessingEnabled {
            get { return postprocessingEnabled; }
            set
            {

            }
        }

        /// <summary>
        /// Add a child to this Scene.
        /// </summary>
        /// <param name="node">Node to add</param>
        public new void Add(Node node)
        {
            if (node is Model)
            {
                if (model == null)
                {
                    model = node as Model;
                    model.Parent = this;
                }
            }
            else
            {
                base.Add(node);
            }
        }

        public void ResizeViewport(Size newSize)
        {
            viewportSize = newSize;

            if (camera != null)
            {
                camera.AspectRatio = (float)viewportSize.Width / (float)viewportSize.Height;
                camera.Project(gl);
            }
        }

        public override void Render(OpenGL gl)
        {
            Render();
        }

        public void Render()
        {
            float[] clear = Convert.ColorToGLColor(ClearColor);
            gl.ClearColor(clear[0], clear[1], clear[2], clear[3]);

            camera.Project(gl);

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT |
                OpenGL.GL_STENCIL_BUFFER_BIT);

            attrs.Push(gl, null);

            if (RenderGrid)
                grid.Render(gl);
            if (RenderAxies)
                axies.Render(gl);

            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_ONE, OpenGL.GL_ONE);

            foreach (Node node in children)
            {
                if (node is Light)
                    node.Render(gl);
            }

            if (Model != null)
                Model.Render(gl);

            foreach (Node node in children)
            {
                if (node is Light)
                    ((Light)node).Pop(gl);
            }

            gl.Disable(OpenGL.GL_BLEND);

            attrs.Pop(gl, null);

            gl.Flush();
        }

        public override void RenderForHitTest(OpenGL gl, Dictionary<uint, Node> hitMap, ref uint currentName)
        {
            RenderForHitTest(hitMap, ref currentName);
        }

        public void RenderForHitTest(Dictionary<uint, Node> hitMap, ref uint currentName)
        {
            model.RenderForHitTest(gl, hitMap, ref currentName);
        }

        /// <summary>
        /// Performs a hit test on the Model.
        /// </summary>
        /// <returns>The list of <see cref="Animatum.SharpGL.SceneGraph.Node"/>'s hit at the specified point</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        private IEnumerable<Node> HitTest(int x, int y)
        {
            if (model == null || (model.Meshes.Count == 0 && model.Bones.Count == 0))
                return null;

            List<Node> resultSet = new List<Node>();
            Dictionary<uint, Node> hitMap = new Dictionary<uint, Node>();

            int[] viewport = new int[4];

            // Get the viewport, then convert the mouse point to an opengl point.
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);
            y = viewport[3] - y;

            uint[] selectBuffer = new uint[512];
            gl.SelectBuffer(512, selectBuffer);

            gl.RenderMode(OpenGL.GL_SELECT);

            // Initialise the names, and add the first name.
            gl.InitNames();
            gl.PushName(0);

            // Push picking projection  matrix
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.PickMatrix(x, y, 1, 1, viewport);
            camera.TransformProjectionMatrix(gl);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();

            uint currentName = 1;

            this.RenderForHitTest(gl, hitMap, ref currentName);

            // Pop matrix and flush commands.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.Flush();

            // End selection.
            int hits = gl.RenderMode(OpenGL.GL_RENDER);

            // Get hit Nodes
            uint posinarray = 0;
            for (int hit = 0; hit < hits; hits++)
            {
                uint nameCount = selectBuffer[posinarray++];
                uint z1 = selectBuffer[posinarray++];
                uint z2 = selectBuffer[posinarray++];

                if (nameCount == 0)
                    continue;

                // Add each hit element to the result set
                for (int name = 0; name < nameCount; name++)
                {
                    uint hitName = selectBuffer[posinarray++];
                    resultSet.Add(hitMap[hitName]);
                }
            }

            return resultSet;
        }
    }
}
