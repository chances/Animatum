using System.Collections.Generic;
using System.Drawing;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Primitives;
using ASE = libASEsharp;
using System;
using SharpGL.SceneGraph.Effects;
using System.Web.Script.Serialization;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A 3D mesh belonging to a Model
    /// </summary>
    public class Mesh : Node
    {
        private string name;
        private Color color;
        private Polygon polygon;
        private Bone bone;

        public static Mesh Null = new Mesh() { Name = "(none)" };

        /// <summary>
        /// Construct a new Mesh
        /// </summary>
        public Mesh()
        {
            children = new List<Node>();
            name = "";
            color = Colors.Random();
            polygon = new Polygon();
            bone = null;

            setColor();
        }

        /// <summary>
        /// Construct a new mesh from an ASE GeomObject
        /// </summary>
        /// <param name="obj">ASE GeomObject to read from</param>
        public Mesh(ASE.GeomObject obj)
        {
            children = new List<Node>();
            name = obj.name;
            color = Colors.Random();
            polygon = new Polygon();
            polygon.Material = null;
            //Vertices
            foreach (ASE.Vector3D v in obj.mesh.verticies)
                polygon.Vertices.Add(new Vertex(v.x, v.y, v.z));
            //Normals
            foreach (ASE.Vector3D v in obj.mesh.vertexNormals)
                polygon.Normals.Add(new Vertex(v.x, v.y, v.z));
            //Texture coordinates
            foreach (ASE.Vector3D uv in obj.mesh.textureCoordinates)
                polygon.UVs.Add(new UV(uv.x, uv.y));
            //Faces
            foreach (ASE.Face face in obj.mesh.faces)
            {
                Face f = new Face();
                foreach (int i in face.vertex)
                    f.Indices.Add(new Index(i, -1, i));
                f.Material = new Material();
                polygon.Faces.Add(f);
            }

            setColor();

            bone = null;
        }

        /// <summary>
        /// The Mesh's name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The Mesh's color.
        /// </summary>
        [ScriptIgnore()]
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                setColor();
            }
        }

        /// <summary>
        /// The Mesh's number of polygons.
        /// </summary>
        [ScriptIgnore()]
        public int PolygonCount
        {
            get { return polygon.Faces.Count; }
        }

        /// <summary>
        /// The bone assigned to this mesh.
        /// </summary>
        public Bone Bone
        {
            get { return bone; }
            set
            {
                bone = value;
                if (bone != null)
                {
                    polygon.Transformation.TranslateX = bone.Translation.X;
                    polygon.Transformation.TranslateY = bone.Translation.Y;
                    polygon.Transformation.TranslateZ = bone.Translation.Z;
                }
                else
                {
                    polygon.Transformation.TranslateX = 0;
                    polygon.Transformation.TranslateY = 0;
                    polygon.Transformation.TranslateZ = 0;
                }
            }
        }

        /// <summary>
        /// The low-level <see cref="SharpGL.SceneGraph.Primitives.Polygon"/> this Mesh is based on.
        /// </summary>
        [ScriptIgnore()]
        public Polygon Polygon
        {
            get { return polygon; }
        }

        /// <summary>
        /// The verticies that compose this Mesh.
        /// </summary>
        [ScriptIgnore()]
        public List<Vertex> Vertices
        {
            get { return polygon.Vertices; }
            set { polygon.Vertices = value; }
        }

        /// <summary>
        /// The faces of this Mesh.
        /// </summary>
        [ScriptIgnore()]
        public List<Face> Faces
        {
            get { return polygon.Faces; }
            set { polygon.Faces = value; }
        }

        /// <summary>
        /// The normals of this Mesh.
        /// </summary>
        [ScriptIgnore()]
        public List<Vertex> Normals
        {
            get { return polygon.Normals; }
            set { polygon.Normals = value; }
        }

        /// <summary>
        /// Render this Mesh to the current OpenGL context.
        /// </summary>
        /// <param name="gl">The OpenGL render context</param>
        public override void Render(OpenGL gl)
        {
            //Render
            //boneRotation.PushObjectSpace(gl, polygon.Transformation);
            //Do bone and bone's parents rotations
            polygon.PushObjectSpace(gl);
            if (bone != null)
            {
                parentTranslation(gl, bone);
                rotate(gl, bone);
            }
            polygon.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            polygon.PopObjectSpace(gl);
            //Render children
            //(there's never going to be any, but this is just for consistency)
            foreach (Node node in children) node.Render(gl);
        }

        public override string ToString()
        {
            return this.name;
        }

        private void parentTranslation(OpenGL gl, Bone bone)
        {
            if (!(bone.Parent is Model))
            {
                if (bone.Parent is Bone)
                {
                    Bone parent = bone.Parent as Bone;
                    parentTranslation(gl, parent);
                    gl.Translate(parent.Translation.X,
                        parent.Translation.Y, parent.Translation.Z);
                }
            }
        }

        private void rotate(OpenGL gl, Bone bone)
        {
            if (!(bone.Parent is Model))
            {
                if (bone.Parent is Bone)
                {
                    rotate(gl, (Bone)bone.Parent);
                }
            }
            RotateTransform rotation = new RotateTransform();
            rotation.FocalPoint = bone.Position;
            rotation.Rotation = bone.Rotation;
            rotation.Rotate(gl);
        }

        private void setColor()
        {
            foreach (Face face in polygon.Faces)
            {
                face.Material.Emission = color;
            }
        }
    }
}