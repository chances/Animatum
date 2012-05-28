using System;
using System.Windows.Forms;
using Animatum.SceneGraph;
using ASE = libASEsharp;
using Animatum.Controls;

namespace Animatum
{
    public partial class MainForm : Form
    {
        private ModelViewControl modelView;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Hide until the timeline has loaded
            this.Visible = false;

            menuStrip.Renderer = new VS2008StripRenderingLibrary.VS2008MenuRenderer();

            //Create model view
            modelView = new ModelViewControl();
            modelView.Dock = DockStyle.Fill;
            this.splitContainerTimeline.Panel1.Controls.Add(modelView);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (importDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.propsControl.SelectedNode = null;

                //Load ASE file
                ASE.Parser parser = new ASE.Parser();
                ASE.Scene scn = parser.loadFilename(importDialog.FileName);
                Model model = modelView.Model;
                model.Clear();
                //Get meshes and bones
                foreach (ASE.GeomObject obj in scn.objs)
                {
                    //If the mesh has only one vertex, it's a bone
                    if (obj.mesh.vertexCount == 1)
                        model.Add(new Bone(obj));
                    else //It's a mesh
                        model.Add(new Mesh(obj));
                }
                //Update the modelTreeView
                this.modelTreeView.Model = model;
                //Update properties
                this.propsControl.Model = model;
                //Update timeline
                this.timeline.Model = model;

                modelView.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolbox_SelectedToolChanged(object sender, EventArgs e)
        {
            this.modelView.CurrentTool = this.toolbox.SelectedTool;
        }

        private void modelTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.propsControl.SelectedNode = (Node)e.Node.Tag;
        }

        private void propsControl_NodeChanged(object sender, EventArgs e)
        {
            this.timeline.Model = this.propsControl.Model;
            //Invalidate
            this.modelView.Invalidate();
        }

        private void timeline_Ready(object sender, EventArgs e)
        {
            //Show the form. We don't want to see an ugly browser ;)
            this.Visible = true;
            //Update timeline model
            this.timeline.Model = this.modelView.Model;
        }

        private void timeline_ModelUpdated(object sender, EventArgs e)
        {
            //Invalidate
            this.modelView.Invalidate();
        }
    }
}