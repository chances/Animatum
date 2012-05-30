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

namespace Animatum.Controls
{
    public partial class ModelViewControl : UserControl
    {
        private MovingLookAtCamera camera;
        private Grid grid;
        private Axies axies;
        private OpenGLAttributesEffect attrs;

        private Stopwatch frameTimer;

        private bool initialized = false;

        public ModelViewControl()
        {
            InitializeComponent();

            openGLControl.FrameRate = 0;

            ClearColor = Color.Black;
            camera = new MovingLookAtCamera()
            {
                Position = new Vertex(10f, 0f, 7.5f),
                Target = new Vertex(0f, 0f, 0f),
                UpVector = new Vertex(0f, 0f, 1f),
                Near = 0,
                Far = 250,
                Theta = 0.785f
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
            attrs.EnableAttributes.EnableBlend = true;
            attrs.ColorBufferAttributes.BlendingSourceFactor = BlendingSourceFactor.SourceAlpha;
            attrs.ColorBufferAttributes.BlendingDestinationFactor = BlendingDestinationFactor.OneMinusSourceAlpha;
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

            frameTimer = new Stopwatch();

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
            float delta = 0.0f;
            if (e.Delta > 0)
                delta = 1.0f - (e.Delta / (float)(e.Delta * 6));
            else
                delta = (e.Delta * 0.01f) * -1.0f;
            camera.Zoom(delta);
            openGLControl.Invalidate();
            base.OnMouseWheel(e);
        }

        bool mouseDown = false;
        Point mousePos;

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
                    //Rotation in degrees
                    float delta = diffX * 0.01f;
                    camera.RotateHorizontal(delta);
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

        private void openGLControl_OpenGLDraw(object sender, PaintEventArgs e)
        {
            //If animating, increment current time
            if (IsPlaying)
            {
                /*frameTimer.Stop();
                long elapsed = frameTimer.ElapsedMilliseconds;
                frameTimer.Reset();
                //Desired milliseconds between frames
                float between = 1000 / FrameRate;
                //If the actuall time passed is longer, compensate
                if (elapsed > between)
                {
                    model.CurrentTime += elapsed / 1000;
                }
                else //We're okay, no compensation
                {
                    model.CurrentTime += between / 1000;
                }*/
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

            if (Model != null)
                Model.Render(gl);

            attrs.Pop(gl, null);

            gl.Flush();

            //Start measuring time between frames
            if (IsPlaying)
            {
                //frameTimer.Start();
            }
        }
    }
}