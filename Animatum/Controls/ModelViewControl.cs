using Animatum.SceneGraph;
using SharpGL;
using SharpGL.SceneGraph;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Animatum.Controls
{
    public partial class ModelViewControl : UserControl
    {
        private bool mouseDown = false;
        private Point mousePos;

        private bool initialized = false;

        public ModelViewControl()
        {
            InitializeComponent();

            //openGLControl.FrameRate = 0;

            Scene = new Animatum.SceneGraph.Scene(openGLControl.OpenGL, this.Size);
            Scene.Add(new Model());
            Scene.Model.AnimationEnded += new EventHandler(model_AnimationEnded);

            CurrentTool = ToolboxItem.Select;
        }

        public Animatum.SceneGraph.Scene Scene { get; private set; }

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
            Scene.Camera.Zoom(factor);
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
                    Scene.Camera.Rotate(deltaX, deltaY);
                    //Project the camera so that stuff looks right
                    Scene.Camera.Project(openGLControl.OpenGL);
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
            if (!DesignMode && initialized && Scene != null)
                Scene.ResizeViewport(this.Size);
        }

        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            //If animating, increment current time
            if (IsPlaying)
            {
                float between = 1000 / FrameRate;
                Scene.Model.CurrentTime += between / 1000;
            }

            Scene.Render();
        }
    }
}