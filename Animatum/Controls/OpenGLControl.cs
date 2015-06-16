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
		private int frameRate = 0;

        /// <summary>
        /// The timer used for drawing the control.
        /// </summary>
        private readonly Timer timerDrawing = new Timer();

        public OpenGLControl(): base()
        {
            FrameRate = 0;

            this.OpenGLInitialized += OpenGLControl_OpenGLInitialized;

            SetupDrawingTimer();
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

        void OpenGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            timerDrawing.Tick += timerDrawing_Tick;
        }

        private void timerDrawing_Tick(object sender, EventArgs e)
        {
            //  If we're in manual mode, we do not care about the timer.
            if (RenderTrigger == RenderTrigger.Manual)
                return;

            //  The timer for drawing simply invalidates the control at a regular interval.
            Invalidate();
        }

        /// <summary>
        /// Setups the drawing timer, based on the framerate settings.
        /// </summary>
        private void SetupDrawingTimer()
        {
            //  First, if the framerate is less than zero, set it to zero.
            if (frameRate < 0)
                frameRate = 0;

            //  Now, if the framerate is zero, we're going to disable the timer.
            if (frameRate == 0)
            {
                //  Disable the timer - at this stage we're done.
                timerDrawing.Enabled = false;
                return;
            }

            //  Now set the interval.
            timerDrawing.Interval = (int)(1000.0 / FrameRate);

            //  Finally, if the timer is not enabled, enable it now.
            if (timerDrawing.Enabled == false)
                timerDrawing.Enabled = true;
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

                DoOpenGLDraw(new RenderEventArgs(e.Graphics));

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