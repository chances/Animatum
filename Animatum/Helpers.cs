using System;
using System.Drawing;
using SharpGL.SceneGraph;

namespace Animatum
{
    public class Helpers
    {
        /// <summary>
        /// Calculate the distance between two specified 2D points.
        /// </summary>
        /// <param name="p1">The first of the two points</param>
        /// <param name="p2">The second of the two points</param>
        /// <returns>The distance between two points</returns>
        public static float PointDistance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        /// <summary>
        /// Calculate the distance between two specified 3D vectors.
        /// </summary>
        /// <param name="v1">The first of the two vectors</param>
        /// <param name="v2">The second of the two vectors</param>
        /// <returns>The distance between the two points</returns>
        public static float VertexDistance(Vertex v1, Vertex v2)
        {
            return (float)Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2) + Math.Pow(v1.Z - v2.Z, 2));
        }

        /// <summary>
        /// Apply a rotation transformation to a given Vertex.
        /// </summary>
        /// <param name="vertex">The given vertex</param>
        /// <param name="focalPoint">The point to rotate around</param>
        /// <param name="rotation">The rotation to apply</param>
        /// <returns>The transformed vertex</returns>
        public static Vertex VertexRotateTransform(Vertex vertex,
            Vertex focalPoint, Vertex rotation)
        {
            //Convert to SlimMath Vectors
            SlimMath.Vector3 vector = Convert.VertexToSlimMathVector3(
                vertex);
            SlimMath.Vector3 focalPointV = Convert.VertexToSlimMathVector3(
                focalPoint);
            SlimMath.Vector3 rotationV = Convert.VertexToSlimMathVector3(
                rotation);

            SlimMath.Matrix transform = SlimMath.Matrix.Identity;
            transform.TranslationVector = vector;

            transform *= SlimMath.Matrix.Translation(focalPointV * -1);
            transform *= SlimMath.Matrix.RotationX(Convert.degreesToRadians(rotationV.X));
            transform *= SlimMath.Matrix.RotationY(Convert.degreesToRadians(rotationV.Y));
            transform *= SlimMath.Matrix.RotationZ(Convert.degreesToRadians(rotationV.Z));
            transform *= SlimMath.Matrix.Translation(focalPointV);

            return Convert.SlimMathVector3ToVertex(transform.TranslationVector);
        }
    }
}