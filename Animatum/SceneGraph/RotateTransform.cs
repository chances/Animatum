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

        public Vertex FocalPoint
        {
            get { return focalPoint; }
            set { focalPoint = value; }
        }

        public float RotateX
        {
            get { return rotation.X; }
            set { rotation.X = value; }
        }

        public float RotateY
        {
            get { return rotation.Y; }
            set { rotation.Y = value; }
        }

        public float RotateZ
        {
            get { return rotation.Z; }
            set { rotation.Z = value; }
        }

        public Vertex Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public void Rotate(OpenGL gl)
        {
            Vertex negFocal = focalPoint * -1;
            gl.Translate(focalPoint.X, focalPoint.Y, focalPoint.Z);
            gl.Rotate(RotateX, RotateY, RotateZ);
            gl.Translate(negFocal.X, negFocal.Y, negFocal.Z);
        }

        public void PushObjectSpace(OpenGL gl, LinearTransformation tranformation)
        {
            gl.PushMatrix();
            tranformation.Transform(gl);
            Rotate(gl);
        }

        public void PushObjectSpace(OpenGL gl, Vertex translation)
        {
            gl.PushMatrix();
            gl.Translate(translation.X, translation.Y, translation.Z);
            Rotate(gl);
        }
    }
}