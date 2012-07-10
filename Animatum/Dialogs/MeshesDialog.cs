using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Animatum.SceneGraph;

namespace Animatum.Dialogs
{
    public partial class MeshesDialog : Form
    {
        private Model model;
        private Bone bone;

        public MeshesDialog(Model model, Bone bone)
        {
            InitializeComponent();

            this.model = model;
            this.bone = bone;

            //Populate meshesComboBox with available meshes
            foreach (Mesh mesh in model.Meshes)
            {
                if (!model.IsMeshAssigned(mesh, model.Children))
                {
                    meshesComboBox.Items.Add(mesh);
                }
            }
            if (meshesComboBox.Items.Count == 0)
            {
                meshesComboBox.Enabled = false;
            }

            //Populate meshesListBox with meshes assigned to bone
            foreach (Mesh mesh in bone.Meshes)
            {
                meshesListBox.Items.Add(mesh);
            }
        }

        private void meshesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (meshesComboBox.SelectedItem != null)
            {
                addButton.Enabled = true;
            }
            else
            {
                addButton.Enabled = false;
            }
        }

        private void meshesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (meshesListBox.SelectedItem != null)
            {
                removeButton.Enabled = true;
            }
            else
            {
                removeButton.Enabled = false;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Mesh selectedMesh = (Mesh)meshesComboBox.SelectedItem;
            meshesComboBox.Items.Remove(selectedMesh);
            meshesListBox.Items.Add(selectedMesh);
            if (meshesComboBox.Items.Count == 0)
            {
                meshesComboBox.Enabled = false;
            }
            addButton.Enabled = false;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            Mesh selectedMesh = (Mesh)meshesListBox.SelectedItem;
            meshesListBox.Items.Remove(selectedMesh);
            meshesComboBox.Items.Add(selectedMesh);
            meshesComboBox.Enabled = true;
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            //Update assigned meshes in bone
            bone.Meshes.Clear();
            foreach (object mesh in meshesListBox.Items)
            {
                bone.Meshes.Add(mesh as Mesh);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}