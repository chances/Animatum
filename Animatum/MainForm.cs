using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Animatum.Controls;
using Animatum.SceneGraph;
using Animatum.SceneGraph.Serialization;
using ASE = libASEsharp;

namespace Animatum
{
    public partial class MainForm : Form
    {
        private Timer updateTimer;
        private ModelViewControl modelView;
        private string title = "";
        private bool unsaved = false;
        private string currentFile = null;

        public MainForm()
        {
            InitializeComponent();

            title = Text;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Hide until the timeline has loaded
            this.Visible = false;

            //Init settings
            Settings.Load();

            //Create model view
            modelView = new ModelViewControl();
            modelView.Dock = DockStyle.Fill;
            this.splitContainerTimeline.Panel1.Controls.Add(modelView);

            //Set settings
            modelView.Scene.RenderGrid = Settings.GetSetting("display/renderGrid", true);
            modelView.Scene.RenderAxies = Settings.GetSetting("display/renderAxies", true);
            modelView.FrameRate = Settings.GetSetting("playback/frameRate", 32);
            timeline.DebugMode = Settings.GetSetting("timeline/debugMode", false);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (unsaved)
            {
                if (MessageBox.Show("Are you sure you want to exit?\n\n" +
                    "Unsaved changes will be lost.", "Unsaved Changes",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                    DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            open();
            this.Cursor = Cursors.Default;
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            import();
            this.Cursor = Cursors.Default;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            save();
            this.Cursor = Cursors.Default;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            saveAs();
            this.Cursor = Cursors.Default;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            if (options.ShowDialog() == DialogResult.OK)
            {
                //Update settings
                modelView.Scene.RenderGrid = Settings.GetSetting("display/renderGrid", true);
                modelView.Scene.RenderAxies = Settings.GetSetting("display/renderAxies", true);
                modelView.FrameRate = Settings.GetSetting("playback/frameRate", 32);
                timeline.DebugMode = Settings.GetSetting("timeline/debugMode", false);
                //Invalidate
                this.modelView.Invalidate();
            }
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://animatum.uservoice.com/");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Dialogs.AboutDialog().Show();
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
            //Let the user know there's a change
            updateTitle();
            //Update
            this.modelTreeView.Model = this.propsControl.Model;
            this.timeline.Model = this.propsControl.Model;
            //Invalidate
            this.modelView.Invalidate();
        }

        private void timeline_Ready(object sender, EventArgs e)
        {
            //Show the form. We don't want to see an ugly browser ;)
            this.Visible = true;
            //Update timeline model
            this.timeline.Model = this.modelView.Scene.Model;
        }

        private bool timeline_KeyCommand(object sender, int key)
        {
            if (ModifierKeys == Keys.Control && key == (int)Keys.I)
            {
                import();
                return true;
            }

            if (ModifierKeys == Keys.Control && key == (int)Keys.O)
            {
                open();
                return true;
            }

            if (ModifierKeys == Keys.Control && key == (int)Keys.S)
            {
                save();
                return true;
            }

            if (ModifierKeys == (Keys.Control | Keys.Shift) && key == (int)Keys.S)
            {
                saveAs();
                return true;
            }

            return false;
        }

        private void timeline_ModelUpdated(object sender, EventArgs e)
        {
            //Let the user know there's a change
            updateTitle();
            //Invalidate
            this.modelView.Invalidate();
        }

        private void timeline_BeginPlayback(object sender, EventArgs e)
        {
            this.modelView.Play();
        }

        private void timeline_PausePlayback(object sender, EventArgs e)
        {
            this.modelView.Pause();
        }

        private void timeline_StopPlayback(object sender, EventArgs e)
        {
            this.modelView.Pause();
            this.modelView.Scene.Model.CurrentTime = 0.0f;
            //Update timeline model
            this.timeline.Model = this.modelView.Scene.Model;
            //Invalidate
            this.modelView.Invalidate();
        }

        private void updateTitle()
        {
            if (currentFile != null)
            {
                string animationName = Path.GetFileName(currentFile);
                this.Text = title + " - " + animationName + "*";
            }
            else
            {
                this.Text = title + " *";
            }
            unsaved = true;
        }

        private void import()
        {
            if (importDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.propsControl.SelectedNode = null;

                //Load ASE file
                ASE.Parser parser = new ASE.Parser();
                ASE.Scene scn = parser.loadFilename(importDialog.FileName);
                Model model = modelView.Scene.Model;
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

                this.Text = title;
                currentFile = null;
                unsaved = true;

                //Update the modelTreeView
                this.modelTreeView.Model = model;
                //Update properties
                this.propsControl.Model = model;
                //Update timeline
                this.timeline.Model = model;

                modelView.Invalidate();
            }
        }

        private void open()
        {
            if (unsaved)
            {
                if (MessageBox.Show("Unsaved changes will be lost.\n\n" +
                    "Do you want to save these changes?", "Unsaved Changes",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                    DialogResult.Yes)
                    save();
            }
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = openDialog.FileName;
                AnimationXMLSerializer aniXML = new AnimationXMLSerializer(
                    modelView.Scene.Model, null);
                aniXML.Deserialize(File.ReadAllText(openDialog.FileName));

                string animationName = Path.GetFileName(currentFile);
                this.Text = title + " - " + animationName;

                //Update the modelTreeView
                this.modelTreeView.Model = this.modelView.Scene.Model;
                //Update properties
                this.propsControl.Model = this.modelView.Scene.Model;
                //Update timeline
                this.timeline.Model = this.modelView.Scene.Model;

                modelView.Invalidate();
            }
        }

        private void save()
        {
            if (currentFile != null)
            {
                string animationName = Path.GetFileNameWithoutExtension(currentFile);
                AnimationXMLSerializer aniXML = new AnimationXMLSerializer(
                    modelView.Scene.Model, animationName);
                aniXML.Save(currentFile);

                unsaved = false;
                this.Text = title + " - " + animationName + ".xml";
            }
            else
            {
                saveAs();
            }
        }

        private void saveAs()
        {
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = saveDialog.FileName;
                string animationName = Path.GetFileNameWithoutExtension(currentFile);
                AnimationXMLSerializer aniXML = new AnimationXMLSerializer(
                    modelView.Scene.Model, animationName);
                aniXML.Save(currentFile);

                unsaved = false;
                this.Text = title + " - " + animationName + ".xml";
            }
        }
    }
}