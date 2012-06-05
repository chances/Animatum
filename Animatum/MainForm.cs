using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Animatum.Controls;
using Animatum.SceneGraph;
using Animatum.SceneGraph.Serialization;
using Animatum.Settings;
using ASE = libASEsharp;

namespace Animatum
{
    public partial class MainForm : Form
    {
        private Settings.Settings settings;
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
            settings = new Settings.Settings();

            menuStrip.Renderer = new VS2008StripRenderingLibrary.VS2008MenuRenderer();

            //Create model view
            modelView = new ModelViewControl();
            modelView.Dock = DockStyle.Fill;
            this.splitContainerTimeline.Panel1.Controls.Add(modelView);

            //Set settings
            modelView.RenderGrid = settings.GetSetting("display/renderGrid", true);
            modelView.RenderAxies = settings.GetSetting("display/renderAxies", true);
            modelView.FrameRate = settings.GetSetting("playback/frameRate", 32);
            timeline.DebugMode = settings.GetSetting("timeline/debugMode", false);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = openDialog.FileName;
                AnimationXMLSerializer aniXML = new AnimationXMLSerializer(
                    modelView.Model, null);
                aniXML.Deserialize(File.ReadAllText(openDialog.FileName));

                string animationName = Path.GetFileName(currentFile);
                this.Text = title + " - " + animationName;

                //Update the modelTreeView
                this.modelTreeView.Model = this.modelView.Model;
                //Update properties
                this.propsControl.Model = this.modelView.Model;
                //Update timeline
                this.timeline.Model = this.modelView.Model;

                modelView.Invalidate();
            }
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFile != null)
            {
                string animationName = Path.GetFileNameWithoutExtension(currentFile);
                AnimationXMLSerializer aniXML = new AnimationXMLSerializer(
                    modelView.Model, animationName);
                aniXML.Save(currentFile);

                unsaved = false;
                this.Text = title + " - " + animationName + ".xml";
            }
            else
            {
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFile = saveDialog.FileName;
                    string animationName = Path.GetFileNameWithoutExtension(currentFile);
                    AnimationXMLSerializer aniXML = new AnimationXMLSerializer(
                        modelView.Model, animationName);
                    aniXML.Save(currentFile);

                    unsaved = false;
                    this.Text = title + " - " + animationName + ".xml";
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = saveDialog.FileName;
                string animationName = Path.GetFileNameWithoutExtension(currentFile);
                AnimationXMLSerializer aniXML = new AnimationXMLSerializer(
                    modelView.Model, animationName);
                aniXML.Save(currentFile);

                unsaved = false;
                this.Text = title + " - " + animationName + ".xml";
            }
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
                settings = new Settings.Settings();
                modelView.RenderGrid = settings.GetSetting("display/renderGrid", true);
                modelView.RenderAxies = settings.GetSetting("display/renderAxies", true);
                modelView.FrameRate = settings.GetSetting("playback/frameRate", 32);
                timeline.DebugMode = settings.GetSetting("timeline/debugMode", false);
                //Invalidate
                this.modelView.Invalidate();
            }
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://github.com/XESoD/Animatum/issues");
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
            this.timeline.Model = this.modelView.Model;
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
            this.modelView.Model.CurrentTime = 0.0f;
            //Update timeline model
            this.timeline.Model = this.modelView.Model;
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
                this.Text = title;
            }
            unsaved = true;
        }
    }
}