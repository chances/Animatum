using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Animatum.Animation;
using Animatum.SceneGraph.Primitives;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using ASE = libASEsharp;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A single-vertex Bone belonging to a Model.
    /// </summary>
    public class Bone : Node
    {
        private string name;
        private Vertex position, translate;
        private RotateTransform parentRotate;
        private Color color;
        private List<Mesh> meshes;
        private List<Keyframe> ani;

        //Visual representation
        private Sphere sphere;

        public static Bone Null = new Bone() { Name = "(none)" };

        /// <summary>
        /// Construct a new Bone
        /// </summary>
        public Bone()
        {
            children = new List<Node>();
            name = "";
            position = new Vertex();
            translate = new Vertex();
            parentRotate = null;
            color = Colors.Random();
            meshes = new List<Mesh>();
            ani = new List<Keyframe>();
            sphere = null;
        }

        /// <summary>
        /// Construct a new Bone from an ASE GeomObject
        /// </summary>
        /// <param name="vertex">ASE GeomObject to read from</param>
        public Bone(ASE.GeomObject obj)
        {
            children = new List<Node>();
            name = obj.name;
            color = Colors.Random();
            ASE.Vector3D v = obj.mesh.verticies[0];
            position = new Vertex(v.x, v.y, v.z);
            translate = new Vertex();
            parentRotate = null;
            meshes = new List<Mesh>();
            ani = new List<Keyframe>();

            //Create visual representation
            sphere = makeSphere();
        }

        /// <summary>
        /// The Bone's name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The Bone's position.
        /// </summary>
        public Vertex Position
        {
            get { return position; }
            set
            {
                position = value;
                sphere = makeSphere();
            }
        }

        /// <summary>
        /// The Bone's absolute position after it has been transformed.
        /// </summary>
        public Vertex TransformedPosition
        {
            get
            {
                return getTransformedPosition(this);
            }
        }

        /// <summary>
        /// The translation applied to this Bone.
        /// </summary>
        public Vertex Translation
        {
            get { return translate; }
            set
            {
                translate = value;
                sphere = makeSphere();
            }
        }

        /// <summary>
        /// The rotation applied to this Bone.
        /// </summary>
        public Vertex Rotation { get; set; }

        /// <summary>
        /// The Bone's color.
        /// </summary>
        [ScriptIgnore()]
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                sphere.Color = value;
            }
        }

        /// <summary>
        /// The meshes assigned to this Bone.
        /// </summary>
        [ScriptIgnore()]
        public List<Mesh> Meshes
        {
            get { return meshes; }
            set { meshes = value; }
        }

        /// <summary>
        /// The keyframed animation applied to this Bone.
        /// </summary>
        public List<Keyframe> Animation
        {
            get { return ani; }
            set { ani = value; }
        }

        public override void Render(OpenGL gl)
        {
            gl.PushMatrix();
            if (sphere.Material != null)
                sphere.Material.Push(gl);
            gl.Translate(TransformedPosition.X, TransformedPosition.Y,
                TransformedPosition.Z);
            sphere.Render(gl);
            sphere.PopObjectSpace(gl);
            //Render children and the lines connecting them
            foreach (Bone bone in children)
            {
                DrawLine(gl, TransformedPosition, bone.TransformedPosition, Color, bone.Color);
                bone.Render(gl);
            }
        }

        private Vertex parentTranslationDiff(Bone bone)
        {
            Vertex diff = new Vertex(0, 0, 0);

            if (!(bone.Parent is Model))
            {
                if (bone.Parent is Bone)
                {
                    Bone parent = bone.Parent as Bone;
                    diff += parentTranslationDiff(parent);
                    diff += parent.Translation;
                }
            }

            return diff;
        }

        /// <summary>
        /// Calculates a given Bone's absolute position after being transformed.
        /// </summary>
        /// <param name="bone">The Bone to get the position for</param>
        /// <returns>The given Bone's absolute position after being transformed</returns>
        private Vertex getTransformedPosition(Bone bone)
        {
            Vertex translation = bone.Position;
            translation += Translation;
            Bone parent = null;
            if (!(bone.Parent is Model))
            {
                if (bone.Parent is Bone)
                {
                    parent = bone.Parent as Bone;
                }
            }

            if (parent != null)
            {
                translation += parentTranslationDiff(bone);
                translation = Helpers.VertexRotateTransform(
                    translation, parent.TransformedPosition, parent.Rotation);
            }

            return translation;
        }

        public Keyframe GetLeftMostKeyframe()
        {
            Keyframe keyframe = null;
            GetLeftMostKeyframe(KeyframeType.Translation);
            if (keyframe == null)
                GetLeftMostKeyframe(KeyframeType.Rotation);
            return keyframe;
        }

        public Keyframe GetLeftMostKeyframe(KeyframeType type)
        {
            List<Keyframe> keyframes = Animation.OrderBy(o => o.Time).ToList();
            Keyframe left = null;
            foreach (Keyframe keyframe in keyframes)
            {
                if (keyframe.Type == type)
                {
                    left = keyframe;
                    break;
                }
            }
            return left;
        }

        public Keyframe GetRightMostKeyframe()
        {
            Keyframe keyframe = null;
            GetRightMostKeyframe(KeyframeType.Translation);
            if (keyframe == null)
                GetRightMostKeyframe(KeyframeType.Rotation);
            return keyframe;
        }

        public Keyframe GetRightMostKeyframe(KeyframeType type)
        {
            List<Keyframe> keyframes = Animation.OrderBy(o => o.Time).ToList();
            Keyframe right = null;
            for (int i = keyframes.Count - 1; i >= 0; i--)
            {
                Keyframe keyframe = keyframes[i];
                if (keyframe.Type == type)
                {
                    right = keyframe;
                    break;
                }
            }
            return right;
        }

        public Keyframe GetKeyframeLeftOfTime(KeyframeType type, float time)
        {
            if (Animation.Count > 1)
            {
                List<Keyframe> keyframes = Animation.OrderBy(o => o.Time).ToList();
                Keyframe left = null;
                foreach (Keyframe keyframe in keyframes)
                {
                    if (keyframe.Type == type)
                    {
                        if (time >= keyframe.Time)
                            left = keyframe;
                        else
                            break;
                    }
                }
                return left;
            }
            return null;
        }

        public Keyframe GetKeyframeRightOfTime(KeyframeType type, float time)
        {
            if (Animation.Count > 1)
            {
                List<Keyframe> keyframes = Animation.OrderBy(o => o.Time).ToList();
                Keyframe right = null;
                for (int i = keyframes.Count - 1; i > -1; i--)
                {
                    Keyframe keyframe = keyframes[i];
                    if (keyframe.Type == type)
                    {
                        if (time <= keyframe.Time)
                            right = keyframe;
                        else
                            break;
                    }
                }
                return right;
            }
            return null;
        }

        public override string ToString()
        {
            return this.name;
        }

        private void DrawLine(OpenGL gl, Vertex v1, Vertex v2, Color c1, Color c2)
        {
            Line line = new Line(v1, v2, c1, c2);
            line.Render(gl);
        }

        private Sphere makeSphere()
        {
            return new Sphere(position + translate, (double)(1.0 * 0.15), 6, color);
        }
    }
}