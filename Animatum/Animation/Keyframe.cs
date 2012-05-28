using SharpGL.SceneGraph;
using System.Runtime.InteropServices;

namespace Animatum.Animation
{
    /// <summary>
    /// And animation keyframe.
    /// ither a translation or a rotation.
    /// </summary>
    [ComVisible(true)]
    public class Keyframe
    {
        private float time;
        private KeyframeType type;
        private Vertex transform;

        /// <summary>
        /// Construct a new empty keyframe
        /// (Defaulted to a type of Translation)
        /// </summary>
        public Keyframe()
        {
            time = 0.0f;
            type = KeyframeType.Translation;
            transform = new Vertex(0, 0, 0);
        }

        /// <summary>
        /// Constructs a new Keyframe
        /// </summary>
        /// <param name="type">Keyframe type</param>
        public Keyframe(KeyframeType type)
        {
            time = 0.0f;
            this.type = type;
            transform = new Vertex(0, 0, 0);
        }

        /// <summary>
        /// Constructs a new Keyframe
        /// </summary>
        /// <param name="time">Keyframe time</param>
        /// <param name="type">Keyframe type</param>
        /// <param name="transformation">Keyframe transformation</param>
        public Keyframe(float time, KeyframeType type, Vertex transformation)
        {
            this.time = time;
            this.type = type;
            transform = transformation;
        }

        public float Time
        {
            get { return time; }
            set { time = value; }
        }

        public KeyframeType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Vertex Transformation
        {
            get { return transform; }
            set { transform = value; }
        }
    }
}