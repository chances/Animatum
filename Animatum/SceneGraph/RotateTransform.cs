using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph;
using SharpGL;
using SharpGL.SceneGraph.Transformations;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A rotate transformation around a focal point.
    /// </summary>
    public class RotateTransform
    {
        private Vertex focalPoint;
        private Vertex rotation;

        /// <summary>
        /// Construct a new empty RotateTransform
        /// </summary>
        public RotateTransform()
        {
            focalPoint = new Vertex(0, 0, 0);
            rotation = new Vertex(0, 0, 0);
        }

        /// <summary>
        /// The point around which the rotation will occur.
        /// </summary>
        public Vertex FocalPoint
        {
            get { return focalPoint; }
            set { focalPoint = value; }
        }

        /// <summary>
        /// Rotation around the x-axis.
        /// </summary>
        public float RotateX
        {
            get { return rotation.X; }
            set { rotation.X = value; }
        }

        /// <summary>
        /// Rotation anround the y-axis.
        /// </summary>
        public float RotateY
        {
            get { return rotation.Y; }
            set { rotation.Y = value; }
        }

        /// <summary>
        /// Rotation around the z-axis.
        /// </summary>
        public float RotateZ
        {
            get { return rotation.Z; }
            set { rotation.Z = value; }
        }

        /// <summary>
        /// This transforms entire rotation around the x, y, and z axies.
        /// </summary>
        public Vertex Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Apply the rotation transformation to the current object space.
        /// </summary>
        /// <param name="gl">The OpenGL render context</param>
        public void Rotate(OpenGL gl)
        {
            Vertex negFocal = focalPoint * -1;
            gl.Translate(focalPoint.X, focalPoint.Y, focalPoint.Z);
            gl.Rotate(RotateX, RotateY, RotateZ);
            gl.Translate(negFocal.X, negFocal.Y, negFocal.Z);
        }

        /// <summary>
        /// Push a new object space onto the stack and then apply a <see cref="SharpGL.SceneGraph.Transformations.LinearTransformation"/>.
        /// </summary>
        /// <param name="gl">The OpenGL render context.</param>
        /// <param name="tranformation">The <see cref="SharpGL.SceneGraph.Transformations.LinearTransformation"/> to apply</param>
        public void PushObjectSpace(OpenGL gl, LinearTransformation tranformation)
        {
            gl.PushMatrix();
            tranformation.Transform(gl);
            Rotate(gl);
        }

        /// <summary>
        /// Push a new object space onto the stack and then apply a translation transformation.
        /// </summary>
        /// <param name="gl">The OpenGL render context</param>
        /// <param name="translation">The translation transformation to apply.</param>
        public void PushObjectSpace(OpenGL gl, Vertex translation)
        {
            gl.PushMatrix();
            gl.Translate(translation.X, translation.Y, translation.Z);
            Rotate(gl);
        }
    }
}