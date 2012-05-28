using System.Collections.Generic;
using System.Linq;
using SharpGL;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using SharpGL.SceneGraph;
using Animatum.Animation;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A 3D model, the root of a scene graph.
    /// </summary>
    [ComVisible(true)]
    public class Model : Node
    {
        private float curTime;

        /// <summary>
        /// Construct a new model.
        /// </summary>
        public Model()
        {
            children = new List<Node>();
            curTime = 0.0f;
        }

        public float CurrentTime
        {
            get { return curTime; }
            set { curTime = value; }
        }

        /// <summary>
        /// Number of meshes in model
        /// </summary>
        [ScriptIgnore()]
        public int MeshCount
        {
            get { return countMeshes(children); }
        }

        /// <summary>
        /// All of the model's meshes
        /// </summary>
        [ScriptIgnore()]
        public List<Mesh> Meshes
        {
            get { return getMeshes(this.children); }
        }

        /// <summary>
        /// Number of bones in model
        /// </summary>
        [ScriptIgnore()]
        public int BoneCount
        {
            get { return countBones(children); }
        }

        /// <summary>
        /// All of the model's bones
        /// </summary>
        public List<Bone> Bones
        {
            get { return getBones(this.children); }
        }

        public override void Render(OpenGL gl)
        {
            updateMeshTransforms();
            //Render
            foreach (Node node in children)
            {
                if (node is Light)
                    node.Render(gl);
            }
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.CullFace(OpenGL.GL_BACK);
            foreach (Node node in children)
            {
                if (node.GetType() != typeof(Light))
                {
                    node.Render(gl);
                }
            }
            gl.Disable(OpenGL.GL_CULL_FACE);
            foreach (Node node in children)
            {
                if (node is Light)
                    ((Light)node).Pop(gl);
            }
        }

        /// <summary>
        /// Clear all meshes and bones from model
        /// </summary>
        public void Clear()
        {
            Mesh[] meshes = getMeshes(children).ToArray();
            Bone[] bones = getBones(children).ToArray();
            foreach (Mesh mesh in meshes)
                mesh.Parent.Remove(mesh);
            foreach (Bone bone in bones)
            {
                if (bone.Parent != null)
                    bone.Parent.Remove(bone);
            }
        }

        public bool IsMeshAssigned(Mesh mesh, List<Node> start)
        {
            bool assigned = false;
            foreach (Node node in start)
            {
                if (node is Bone)
                {
                    Bone bone = (Bone)node;
                    if (bone.Mesh == mesh)
                    {
                        assigned = true;
                        break;
                    }
                    assigned = IsMeshAssigned(mesh, bone.Children);
                }
            }
            return assigned;
        }

        private void updateMeshTransforms()
        {
            foreach (Bone bone in Bones)
            {
                Vertex translate = bone.Translation;
                Vertex rotate = new Vertex(0, 0, 0);
                if (bone.Animation.Count == 1)
                {
                    Keyframe keyframe = bone.Animation[0];
                    if (keyframe.Type == KeyframeType.Translation)
                        translate = keyframe.Transformation;
                    else
                        rotate = keyframe.Transformation;
                }
                else if (bone.Animation.Count > 1)
                {
                    //Get the list of keyframes sorted by time
                    List<Keyframe> keyframes = bone.Animation.OrderBy(o => o.Time).ToList();
                    //If the current time is less than, or equal
                    // to the first keyframe's time, set the
                    // transformation to that keyframe's
                    // tranformation. Else, calculate the inbetween.
                    if (curTime <= keyframes[0].Time)
                    {
                        Keyframe leftTranslate = bone
                            .GetLeftMostKeyframe(KeyframeType.Translation);
                        if (leftTranslate != null)
                            translate = leftTranslate.Transformation;
                        Keyframe leftRotatate = bone
                            .GetLeftMostKeyframe(KeyframeType.Rotation);
                        if (leftRotatate != null)
                            rotate = leftRotatate.Transformation;
                    }
                    else if (curTime >= keyframes[keyframes.Count - 1].Time)
                    {
                        Keyframe rightTranslate = bone
                            .GetRightMostKeyframe(KeyframeType.Translation);
                        if (rightTranslate != null)
                            translate = rightTranslate.Transformation;
                        Keyframe rightRotatate = bone
                            .GetRightMostKeyframe(KeyframeType.Rotation);
                        if (rightRotatate != null)
                            rotate = rightRotatate.Transformation;
                    }
                    else
                    {
                        //Translation inbetween
                        Keyframe left = bone
                            .GetKeyframeLeftOfTime(KeyframeType.Translation, curTime);
                        Keyframe right = bone
                            .GetKeyframeRightOfTime(KeyframeType.Translation, curTime);
                        if (left != null && right == null)
                            translate = left.Transformation;
                        else if (left == null && right != null)
                            translate = right.Transformation;
                        else if (left != null && right != null)
                        {
                            //Difference in time between keyframes
                            float timeDiff = right.Time - left.Time;
                            float timeBetween = curTime - left.Time;
                            timeBetween /= timeDiff;
                            //Difference in transformation between keyframes
                            Vertex transDiff = right.Transformation - left.Transformation;
                            translate = left.Transformation + transDiff * timeBetween;
                        }
                        //Rotation inbetween
                        left = bone
                            .GetKeyframeLeftOfTime(KeyframeType.Rotation, curTime);
                        right = bone
                            .GetKeyframeRightOfTime(KeyframeType.Rotation, curTime);
                        if (left != null && right == null)
                            rotate = left.Transformation;
                        else if (left == null && right != null)
                            rotate = right.Transformation;
                        else if (left != null && right != null)
                        {
                            //Difference in time between keyframes
                            float timeDiff = right.Time - left.Time;
                            float timeBetween = curTime - left.Time;
                            timeBetween /= timeDiff;
                            //Difference in transformation between keyframes
                            Vertex transDiff = right.Transformation - left.Transformation;
                            rotate = left.Transformation + transDiff * timeBetween;
                        }
                    }
                }
                bone.Translation = translate;
                bone.Rotation = rotate;
                if (bone.Mesh != null)
                {
                    bone.Mesh.Bone = bone;
                }
            }
        }

        private List<Mesh> getMeshes(List<Node> start)
        {
            List<Mesh> meshes = new List<Mesh>();
            foreach (Node node in start)
            {
                if (node is Mesh)
                {
                    meshes.Add((Mesh)node);
                    meshes.AddRange(getMeshes(node.Children));
                }
            }
            return meshes;
        }

        private List<Bone> getBones(List<Node> start)
        {
            List<Bone> bones = new List<Bone>();
            foreach (Node node in start)
            {
                if (node is Bone)
                {
                    bones.Add((Bone)node);
                    bones.AddRange(getBones(node.Children));
                }
            }
            return bones;
        }

        private int countMeshes(List<Node> children)
        {
            int count = 0;
            if (children != null)
            {
                foreach (Node node in children)
                {
                    if (node is Mesh)
                    {
                        count++;
                        count += countMeshes(node.Children);
                    }
                }
            }
            return count;
        }

        private int countBones(List<Node> children)
        {
            int count = 0;
            if (children != null)
            {
                foreach (Node node in children)
                {
                    if (node is Bone)
                    {
                        count++;
                        count += countMeshes(node.Children);
                    }
                }
            }
            return count;
        }
    }
}