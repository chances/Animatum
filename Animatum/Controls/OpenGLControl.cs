using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Animatum.Controls
{
    class OpenGLControl : SharpGL.OpenGLControl
    {
		private Animatum.SharpGL.MultisampledNativeWindowRenderContextProvider renderContext;
        private int frameRate = 0;

        public OpenGLControl()
        {
            InitializeComponent();

            //  Set the user draw styles.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			renderContext = null;

            FrameRate = 0;
        }

        [Description("The rate at which the control should be re-drawn, in Hertz. If zero, call Invalidate() to draw."), Category("SharpGL")]
        public new int FrameRate
        {
            get { return frameRate; }
            set
            {
                frameRate = value;
                if (frameRate <= 0)
                {
                    timerDrawing.Interval = 1000;
                    timerDrawing.Enabled = false;
                }
                else
                {
                    timerDrawing.Interval = 1000 / frameRate;
                    timerDrawing.Enabled = true;
                }
            }
        }

		/// <summary>
		/// Gets the current OpenGL render context.
		/// </summary>
		/// <value>The render context.</value>
		public Animatum.SharpGL.MultisampledNativeWindowRenderContextProvider RenderContext
		{
			get { return renderContext; }
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OpenGLControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.Name = "OpenGLControl";
            this.ResumeLayout(false);
        }

		protected override void InitialiseOpenGL()
		{
			object parameter = null;

			//  Native render context providers need a little bit more attention.
			if(RenderContextType == RenderContextType.NativeWindow)
			{
				SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
				parameter = Handle;
			}

			//  Create the render context.
			renderContext = new Animatum.SharpGL.MultisampledNativeWindowRenderContextProvider ();

			renderContext.Create(OpenGLVersion, gl, Width, Height, 32, parameter);

			//  Set the most basic OpenGL styles.
			gl.ShadeModel(OpenGL.GL_SMOOTH);
			gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			gl.ClearDepth(1.0f);
			gl.Enable(OpenGL.GL_DEPTH_TEST);
			gl.DepthFunc(OpenGL.GL_LEQUAL);
			gl.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);

			//  Fire the OpenGL initialized event.
			DoOpenGLInitialized();

			//  Set the draw timer.
			timerDrawing.Tick += timerDrawing_Tick;
		}

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                RenderContextType = SharpGL.RenderContextType.NativeWindow;
                InitialiseOpenGL();
            }
            base.OnLoad(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (DesignMode)
            {
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
                e.Graphics.DrawString(this.Name + " - OpenGL Render Control", this.Font, new SolidBrush(this.ForeColor), 5, 5);
            }
            //Don't paint if not in design mode
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //  Start the stopwatch so that we can time the rendering.
            stopwatch.Restart();

            //  Only draw if we're not in the designer
            if (!DesignMode)
            {
                OpenGL.MakeCurrent();

                DoOpenGLDraw(e);

                //  Draw the FPS.
                if (DrawFPS)
                {
                    OpenGL.DrawText(5, 5, 1.0f, 0.0f, 0.0f, "Courier New", 12.0f,
                        string.Format("Draw Time: {0:0.0000} ms ~ {1:0.0} FPS", frameTime, 1000.0 / frameTime));
                    OpenGL.Flush();
                }

                //	Blit our offscreen bitmap.
                IntPtr handleDeviceContext = e.Graphics.GetHdc();
                OpenGL.Blit(handleDeviceContext);
                e.Graphics.ReleaseHdc(handleDeviceContext);
            }

            //  Stop the stopwatch.
            stopwatch.Stop();

            //  Store the frame time.
            frameTime = stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}