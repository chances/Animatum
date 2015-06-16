using System;
using System.Drawing;
using System.Windows.Forms;
using Animatum.SceneGraph;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Cameras;
using Animatum.SceneGraph.Primitives;
using SharpGL.SceneGraph.Effects;
using SharpGL.Enumerations;
using System.Diagnostics;
using System.Collections.Generic;

namespace Animatum.Controls
{
    public partial class ModelViewControl : UserControl
    {
        private MovingLookAtCamera camera;
        private Grid grid;
        private Axies axies;
        private OpenGLAttributesEffect attrs;

        private bool mouseDown = false;
        private Point mousePos;

        private bool initialized = false;

        public ModelViewControl()
        {
            InitializeComponent();

            //openGLControl.FrameRate = 0;

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

            Model = new Model();
            Model.AnimationEnded += new EventHandler(model_AnimationEnded);

            Color col = Color.FromArgb(40, 40, 40);

            //Nice soft-ish lighting
            Light light = new Light(OpenGL.GL_LIGHT0)
            {
                Position = new Vertex(-9, -9, 11),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            Model.Children.Add(light);
            light = new Light(OpenGL.GL_LIGHT1)
            {
                Position = new Vertex(9, -9, 11),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            Model.Children.Add(light);
            light = new Light(OpenGL.GL_LIGHT2)
            {
                Position = new Vertex(0, 15, 15),
                Ambient = Color.Black,
                Diffuse = col,
                Specular = col
            };
            Model.Children.Add(light);

            CurrentTool = ToolboxItem.Select;
        }

        public Model Model { get; set; }

        public Color ClearColor { get; set; }

        public bool RenderGrid { get; set; }

        public bool RenderAxies { get; set; }

        public int FrameRate { get; set; }

        public ToolboxItem CurrentTool { get; set; }

        public bool IsPlaying
        {
            get { return openGLControl.FrameRate > 0; }
        }

        public void Play()
        {
            openGLControl.FrameRate = FrameRate;
        }

        public void Pause()
        {
            openGLControl.FrameRate = 0;
        }

        void model_AnimationEnded(object sender, EventArgs e)
        {
            Pause();
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            openGLControl.Invalidate();
            base.OnInvalidated(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //Zoom by moving the camera closer or farther away from the origin
            float factor = 0.0f;
            if (e.Delta > 0)
                factor = 1 - (float)e.Delta / 960f;
            else
                factor = 1 + ((float)(e.Delta * -1) / 960f);
            camera.Zoom(factor);
            openGLControl.Invalidate();
            base.OnMouseWheel(e);
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            mousePos = e.Location;
            openGLControl.Invalidate();
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
           if (mouseDown && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (CurrentTool == ToolboxItem.Orbit)
                {
                    int diffX = mousePos.X - e.X;
                    int diffY = mousePos.Y - e.Y;
                    float deltaX = diffX * 0.01f;
                    float deltaY = diffY * 0.01f;
                    //Rotation in radians
                    camera.Rotate(deltaX, deltaY);
                    //Project the camera so that stuff looks right
                    camera.Project(openGLControl.OpenGL);
                    openGLControl.Invalidate();
                }
            }
            mousePos = e.Location;
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            openGLControl.Invalidate();
        }

        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            initialized = true;
        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            if (!DesignMode && initialized && camera != null)
            {
                camera.AspectRatio = (float)openGLControl.Width / (float)openGLControl.Height;
                camera.Project(openGLControl.OpenGL);
            }
        }

        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            //If animating, increment current time
            if (IsPlaying)
            {
                float between = 1000 / FrameRate;
                Model.CurrentTime += between / 1000;
            }

            //Render
            OpenGL gl = openGLControl.OpenGL;

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

            if (Model != null)
                Model.Render(gl);

			gl.Disable(OpenGL.GL_BLEND);

            attrs.Pop(gl, null);

            gl.Flush();
        }

		/// <summary>
		/// Performs a hit test on the Model.
		/// </summary>
		/// <returns>The list of <see cref="Animatum.SharpGL.SceneGraph.Node"/>'s hit at the specified point</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		private IEnumerable<Node> HitTest(int x, int y)
		{
			if (Model.Meshes.Count == 0 && Model.Bones.Count == 0)
				return null;

			List<Node> resultSet = new List<Node>();
			Dictionary<uint, Node> hitMap = new Dictionary<uint, Node>();

			OpenGL gl = openGLControl.OpenGL;

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

			Model.RenderForHitTest(gl, hitMap, ref currentName);

			// Pop matrix and flush commands.
			gl.MatrixMode(OpenGL.GL_PROJECTION);
			gl.PopMatrix();
			gl.MatrixMode(OpenGL.GL_MODELVIEW);
			gl.Flush();

			// End selection.
			int hits = gl.RenderMode(OpenGL.GL_RENDER);

			// Get hit Nodes
			uint posinarray = 0;
			for (int hit = 0; hit < hits; hits++) {
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