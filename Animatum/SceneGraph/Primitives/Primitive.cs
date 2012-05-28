using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Core;
using SharpGL;
using System.Drawing;

namespace Animatum.SceneGraph.Primitives
{
    /// <summary>
    /// A basic primitive, just to shorten code in places.
    /// </summary>
    abstract class Primitive
    {
        protected DisplayList displayList;

        public abstract void Render(OpenGL gl);
    }
}