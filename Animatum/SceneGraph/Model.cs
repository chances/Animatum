using System.Collections.Generic;
using System.Linq;
using SharpGL;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using SharpGL.SceneGraph;
using Animatum.Animation;
using System;

namespace Animatum.SceneGraph
{
    /// <summary>
    /// A 3D model, animatable by the user.
    /// </summary>
    [ComVisible(true)]
    public class Model : Node
    {
        private float curTime;

        public event EventHandler CurrentTimeChanged;
        public event EventHandler AnimationEnded;

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
            set
            {
                curTime = value;
                if (CurrentTimeChanged != null)
                    CurrentTimeChanged(this, new EventArgs());
            }
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
        /// All of the model's bones
        /// </summary>
        public List<Bone> Bones
        {
            get { return getBones(this.children); }
        }

        /// <summary>
        /// The number of bones within this model that have any keyframes
        /// </summary>
        public int BonesWithKeyframesCount
        {
            get { return countBonesWithKeyframes(); }
        }

        public override void Render(OpenGL gl)
        {
            if (!Visible)
                return;

            updateMeshTransforms();
            
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
        }

		public override void RenderForHitTest (OpenGL gl, Dictionary<uint, Node> hitMap, ref uint currentName)
		{
            if (!Visible)
                return;

			updateMeshTransforms();

			foreach (Node node in children)
				node.RenderForHitTest(gl, hitMap, ref currentName);
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

        /// <summary>
        /// Determines if a given Mesh is assigned to a Bone
        /// </summary>
        /// <param name="mesh">The Mesh to check</param>
        /// <param name="start">The SceneGraph Node to begin searching</param>
        /// <returns>Whether or not the Mesh is assigned to a Bone</returns>
        public bool IsMeshAssigned(Mesh mesh, List<Node> start)
        {
            bool assigned = false;
            foreach (Node node in start)
            {
                if (node is Bone)
                {
                    Bone bone = (Bone)node;
                    foreach (Mesh assignedMesh in bone.Meshes)
                    {
                        if (assignedMesh == mesh)
                        {
                            assigned = true;
                            break;
                        }
                    }
                    if (!assigned)
                    {
                        assigned = IsMeshAssigned(mesh, bone.Children);
                    }
                }
            }
            return assigned;
        }

        /// <summary>
        /// Update the SceneGraph's Mesh transformations with the current time
        /// </summary>
        private void updateMeshTransforms()
        {
            int endCount = 0;
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

                        endCount++;
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
						else if (left != null && right != null && left == right)
							translate = left.Transformation;
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
						else if (left != null && right != null && left == right)
							rotate = left.Transformation;
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
                foreach (Mesh mesh in bone.Meshes)
                {
                    mesh.Bone = bone;
                }
            }
            //All bone animations have ended
            if (endCount == countBonesWithKeyframes())
                if (AnimationEnded != null)
                    AnimationEnded(this, new EventArgs());
        }

        /// <summary>
        /// Get a List of all of the meshes belonging to a given Node
        /// </summary>
        /// <param name="start">The Node to begin searching</param>
        /// <returns>A List of all of the meshes belonging to the given Node</returns>
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

        /// <summary>
        /// Get a List of all of the bones belonging to a given Node
        /// </summary>
        /// <param name="start">The Node to begin searching</param>
        /// <returns>A List of all of the bones belonging to the given Node</returns>
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
                        count += countBones(node.Children);
                    }
                }
            }
            return count;
        }

        private int countBonesWithKeyframes()
        {
            int count = 0;
            if (children != null)
            {
                foreach (Bone bone in Bones)
                {
                    if (bone.Animation.Count > 0)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}