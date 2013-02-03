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
        private float horizontalTheta;
        private float verticalTheta;

        public MovingLookAtCamera()
        {
            horizontalTheta = 0.0f;
        }

        public float HorizontalTheta
        {
            get { return horizontalTheta; }
            set
            {
                horizontalTheta = value;
                normalizeHorizontalTheta();
                updateHorizontalRotation();
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
        /// Rotate the camera around the Target horizontally
        /// </summary>
        /// <param name="delta">Change in theta</param>
        public void RotateHorizontal(float delta)
        {
            horizontalTheta += delta;
            normalizeHorizontalTheta();
            updateHorizontalRotation();
        }

        /*
        /// <summary>
        /// Rotate the camera around the Target vertically
        /// </summary>
        /// <param name="delta">Change in theta</param>
        public void RotateVertical(float delta)
        {
            verticalTheta += delta;
            normalizeVerticalTheta();
            updateVerticalRotation();
        }
        */

        private void updateHorizontalRotation()
        {
            //Distance from focus to camera on the XY plane.
            //This is the radius of the rotation circle.
            float r = Helpers.PointDistance(
                new PointF(this.Target.X, this.Target.Y),
                new PointF(this.Position.X, this.Position.Y));
            //New X and Y coordinate
            float x = r * (float)Math.Cos(horizontalTheta);
            float y = r * (float)Math.Sin(horizontalTheta);
            //Update position
            this.Position = new Vertex(x, y, this.Position.Z);
        }

        private void normalizeHorizontalTheta()
        {
            //Ensures theta doesn't increase/decrease forever
            // We wouldn't want the float to overflow
            if (horizontalTheta <= -6.3f)
                horizontalTheta += 6.3f * (float)Math.Floor(horizontalTheta / -6.3f);
            if (horizontalTheta >= 6.3f)
                horizontalTheta -= 6.3f * (float)Math.Floor(horizontalTheta / 6.3f);
        }

        private void normalizeVerticalTheta()
        {
            //Ensures theta doesn't increase/decrease forever
            // We wouldn't want the float to overflow
            if (verticalTheta <= -6.3f)
                verticalTheta += 6.3f * (float)Math.Floor(verticalTheta / -6.3f);
            if (verticalTheta >= 6.3f)
                verticalTheta -= 6.3f * (float)Math.Floor(verticalTheta / 6.3f);
        }
    }
}