using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Animatum.SceneGraph;
using System.Diagnostics;
using SharpGL.SceneGraph;
using Animatum.Dialogs;

// Default height: 145

namespace Animatum.Controls
{
    public partial class PropertiesControl : UserControl
    {
        private string noSel = "Nothing selected." + Environment.NewLine +
            Environment.NewLine + "Select a bone or mesh from the tree above.";
        private string noModel = "Nothing loaded." + Environment.NewLine +
            Environment.NewLine + "Open an animation or import an ASE model.";

        private Model model;
        private Node selNode;
        private bool selChanging;

        public event EventHandler NodeChanged;

        public PropertiesControl()
        {
            InitializeComponent();

            model = null;
            selNode = null;
            selChanging = false;
            //Add colors
            meshColorComboBox.Colors = Colors.ColorPool;
            boneColorComboBox.Colors = Colors.ColorPool;

            updateProps();
        }

        public Model Model
        {
            get { return model; }
            set
            {
                model = value;
                updateProps();
            }
        }

        public Node SelectedNode
        {
            get { return selNode; }
            set
            {
                selNode = value;
                selChanging = true;
                updateProps();
            }
        }

        private void OnNodeChanged()
        {
            if (this.NodeChanged != null)
                this.NodeChanged(this, new EventArgs());
        }

        private void updateProps()
        {
            if (selNode == null)
            {
                if (model == null)
                    noneLabel.Text = noModel;
                else
                    noneLabel.Text = noSel;

                meshPanel.Visible = false;
                bonePanel.Visible = false;
                nonePanel.Visible = true;
            }
            else
            {
                selChanging = true;
                //Determine selection type
                if (selNode is Mesh) //It's a mesh
                {
                    //Set mesh information
                    Mesh mesh = (Mesh)selNode;
                    meshNameLabel.Text = mesh.Name;
                    meshColorComboBox.SelectedColor = mesh.Color;

                    meshPanel.Visible = true;
                    bonePanel.Visible = false;
                    nonePanel.Visible = false;
                }
                else if (selNode is Bone) //It's a bone
                {
                    //Set bone information
                    Bone bone = (Bone)selNode;
                    boneNameLabel.Text = bone.Name;
                    bonePosLabel.Text = bone.Position.ToString();
                    boneColorComboBox.SelectedColor = bone.Color;
                    //Only allow edit of the color if the bone is a root bone
                    /* Maybe later
                    if (bone.Parent == model)
                        boneColorComboBox.Enabled = true;
                    else
                        boneColorComboBox.Enabled = false;
                    */
                    // Selected meshes
                    getSelectedMeshes(bone);
                    // Selected bone
                    getSelectedBone(bone);

                    meshPanel.Visible = false;
                    bonePanel.Visible = true;
                    nonePanel.Visible = false;
                }
            }
            selChanging = false;
        }

        private void getSelectedMeshes(Bone bone)
        {
            string selectedMeshes = "";
            if (bone.Meshes.Count > 0)
            {
                foreach (Mesh mesh in bone.Meshes)
                {
                    selectedMeshes += mesh.Name + ", ";
                }
                selectedMeshes = 
                    selectedMeshes.Substring(0, selectedMeshes.Length - 2);
            }
            else
            {
                selectedMeshes = "(none)";
            }
            meshesLabel.Text = selectedMeshes;
        }

        private void getSelectedBone(Bone childBone)
        {
            parentComboBox.Items.Clear();
            parentComboBox.Items.Add(Bone.Null);
            bool sel = false;
            //Add all bones not already parented to a bone and
            // the parent of this bone (if any)
            foreach (Node node in model.Children)
            {
                if (node is Bone)
                {
                    Bone bone = (Bone)node;
                    if (bone != childBone)
                    {
                        parentComboBox.Items.Add(bone);
                        if (childBone.Parent == bone)
                        {
                            sel = true;
                            parentComboBox.SelectedIndex = parentComboBox.Items.Count - 1;
                        }
                    }
                }
            }
            if (!sel)
                parentComboBox.SelectedIndex = 0;
        }

        private void meshColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selNode is Mesh && !selChanging)
            {
                Mesh mesh = (Mesh)selNode;
                if (mesh.Color != meshColorComboBox.SelectedColor)
                {
                    mesh.Color = meshColorComboBox.SelectedColor;
                    OnNodeChanged();
                }
            }
        }

        private void boneColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selNode is Bone && !selChanging)
            {
                Bone bone = (Bone)selNode;
                if (bone.Color != boneColorComboBox.SelectedColor)
                {
                    bone.Color = boneColorComboBox.SelectedColor;
                    OnNodeChanged();
                }
            }
        }

        private void parentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Node node = (Node)parentComboBox.SelectedItem;
            if (node is Bone && !selChanging)
            {
                Bone bone = (Bone)selNode;
                if (((Bone)node) == Bone.Null)
                {
                    if (bone.Parent != model)
                    {
                        model.Add(bone.Parent.Remove(bone));
                        //boneColorComboBox.Enabled = true;
                        OnNodeChanged();
                    }
                }
                else
                {
                    if (bone.Parent != (Bone)node)
                    {
                        ((Bone)node).Add(bone.Parent.Remove(bone));
                        //boneColorComboBox.Enabled = false;
                        OnNodeChanged();
                    }
                }
            }
        }

        private void meshAlphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void editMeshesButton_Click(object sender, EventArgs e)
        {
            if (selNode is Bone)
            {
                MeshesDialog meshes = new MeshesDialog(model, selNode as Bone);
                if (meshes.ShowDialog() == DialogResult.OK)
                {
                    getSelectedMeshes(selNode as Bone);
                    OnNodeChanged();
                }
            }
        }
    }
}