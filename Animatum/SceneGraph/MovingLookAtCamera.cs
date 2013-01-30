using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph;
using System.Drawing;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A LookAtCamera that can be zoomed and orbited horizontally around a point.
    /// </summary>
    class MovingLookAtCamera : LookAtCamera
    {
        private float theta;

        public MovingLookAtCamera()
        {
            theta = 0.0f;
        }

        public float Theta
        {
            get { return theta; }
            set
            {
                theta = value;
                normalizeTheta();
                updateTheta();
            }
        }

        /// <summary>
        /// Zoom the camera given a zoom factor
        /// </summary>
        /// <param name="factor"></param>
        public void Zoom(float factor)
        {
            Vertex A = this.Target;
            Vertex B = this.Position;
            Vertex magnitude = new Vertex(B.X - A.X, B.Y - A.Y, B.Z - A.Z);
            this.Position = magnitude * factor;
        }

        /// <summary>
        /// Rotate the camera around the Target
        /// </summary>
        /// <param name="delta">Change in theta</param>
        public void RotateHorizontal(float delta)
        {
            theta += delta;
            normalizeTheta();
            updateTheta();
        }

        private void updateTheta()
        {
            //Distance from focus to camera on the XY plane.
            //This is the radius of the rotation circle.
            float r = Helpers.PointDistance(
                new PointF(this.Target.X, this.Target.Y),
                new PointF(this.Position.X, this.Position.Y));
            //New X and Y coordinate
            float x = r * (float)Math.Cos(theta);
            float y = r * (float)Math.Sin(theta);
            //Update position
            this.Position = new Vertex(x, y, this.Position.Z);
        }

        private void normalizeTheta()
        {
            //Ensures theta doesn't increase/decrease forever
            // We wouldn't want the float to overflow
            if (theta <= -6.3f)
                theta += 6.3f * (float)Math.Floor(theta / -6.3f);
            if (theta >= 6.3f)
                theta -= 6.3f * (float)Math.Floor(theta / 6.3f);
        }
    }
}