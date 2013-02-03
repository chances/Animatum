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
    /// A LookAtCamera that can be zoomed and orbited around a point.
    /// </summary>
    class MovingLookAtCamera : LookAtCamera
    {
        private Vertex initialPosition;
        private bool thetaSet = false;
        private float horizontalTheta;
        private float verticalTheta;

        public MovingLookAtCamera()
        {
            horizontalTheta = 0.0f;
        }

        /// <summary>
        /// Gets or sets the current horizontal rotation of the camera around its Target.
        /// </summary>
        public float HorizontalTheta
        {
            get { return horizontalTheta; }
            set
            {
                if (!thetaSet)
                {
                    initialPosition = this.Position;
                    thetaSet = true;
                }
                horizontalTheta = value;
                normalizeHorizontalTheta();
                updateRotation();
            }
        }

        /// <summary>
        /// Gets or sets the current vertical rotation of the camera around its Target.
        /// </summary>
        public float VerticalTheta
        {
            get { return verticalTheta; }
            set
            {
                if (!thetaSet)
                {
                    initialPosition = this.Position;
                    thetaSet = true;
                }
                verticalTheta = value;
                normalizeVerticalTheta();
                updateRotation();
            }
        }

        /// <summary>
        /// Zoom the camera given a zoom factor.
        /// </summary>
        /// <param name="factor">The zoom factor</param>
        public void Zoom(float factor)
        {
            Vertex A = this.Target;
            Vertex B = this.Position;
            Vertex magnitude = new Vertex(B.X - A.X, B.Y - A.Y, B.Z - A.Z);
            this.Position = magnitude * factor;
        }

        /// <summary>
        /// Rotate the camera around the Target horizontally.
        /// </summary>
        /// <param name="delta">Change in theta</param>
        public void RotateHorizontal(float delta)
        {
            horizontalTheta += delta;
            normalizeHorizontalTheta();
            updateRotation();
        }

        /// <summary>
        /// Rotate the camera around the Target vertically.
        /// </summary>
        /// <param name="delta">Change in theta</param>
        public void RotateVertical(float delta)
        {
            verticalTheta += delta;
            normalizeVerticalTheta();
            updateRotation();
        }

        /// <summary>
        /// Rotate the camera around the Target both horizontally and vertically.
        /// </summary>
        /// <param name="horizontalDelta">Change in horizontal theta</param>
        /// <param name="verticalDelta">Change in vertical theta</param>
        public void Rotate(float horizontalDelta, float verticalDelta)
        {
            horizontalTheta += horizontalDelta;
            normalizeHorizontalTheta();

            verticalTheta += verticalDelta;
            normalizeVerticalTheta();

            updateRotation();
        }

        private void updateRotation()
        {
            this.Position = Helpers.VertexRotateRadianTransform(
                initialPosition, this.Target, new Vertex(0, verticalTheta, horizontalTheta));
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