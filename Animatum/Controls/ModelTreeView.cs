using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Animatum.SceneGraph;
using System.ComponentModel;

namespace Animatum.Controls
{
    [Designer(typeof(ModelTreeViewDesigner))]
    class ModelTreeView : TreeView
    {
        private Model model;

        public ModelTreeView()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.ImageList = new ImageList();
            this.ImageList.Images.Add("mesh", Animatum.Properties.Resources.mesh);
            this.ImageList.Images.Add("bone", Animatum.Properties.Resources.bone);
        }

        public Model Model
        {
            get { return model; }
            set
            {
                model = value;
                if (model != null)
                    updateModel();
            }
        }

        private void updateModel()
        {
            this.Nodes.Clear();
            //Add meshes first
            foreach (Node node in model.Children) {
                if (node is Mesh)
                    addMesh((Mesh)node, null);
            }
            //Then add bones
            foreach (Node node in model.Children)
            {
                if (node is Bone)
                    addBone((Bone)node, null);
            }
            //Expand All
            this.ExpandAll();
        }

        private void addMesh(Mesh mesh, TreeNode parent)
        {
            TreeNode node = new TreeNode(mesh.Name);
            node.ImageKey = "mesh";
            node.SelectedImageKey = "mesh";
            node.Tag = mesh;
            if (parent == null)
                this.Nodes.Add(node);
            else
                parent.Nodes.Add(node);
            //Add children
            foreach (Node child in mesh.Children)
            {
                if (child is Mesh)
                    addMesh((Mesh)child, node);
            }
        }

        private void addBone(Bone bone, TreeNode parent)
        {
            TreeNode node = new TreeNode(bone.Name);
            node.ImageKey = "bone";
            node.SelectedImageKey = "bone";
            node.Tag = bone;
            if (parent == null)
                this.Nodes.Add(node);
            else
                parent.Nodes.Add(node);
            //Add children
            foreach (Node child in bone.Children)
            {
                if (child is Bone)
                    addBone((Bone)child, node);
            }
        }
    }
}