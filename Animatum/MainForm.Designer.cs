namespace Animatum
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerProject = new System.Windows.Forms.SplitContainer();
            this.splitContainerProps = new System.Windows.Forms.SplitContainer();
            this.modelDockPanel = new Animatum.Controls.DockToolPanel();
            this.modelTreeView = new Animatum.Controls.ModelTreeView();
            this.propsDockPanel = new Animatum.Controls.DockToolPanel();
            this.propsControl = new Animatum.Controls.PropertiesControl();
            this.toolbox = new Animatum.Controls.Toolbox();
            this.splitContainerTimeline = new System.Windows.Forms.SplitContainer();
            this.timelineDockPanel = new Animatum.Controls.DockToolPanel();
            this.timeline = new Animatum.Controls.TimelineControl();
            this.importDialog = new System.Windows.Forms.OpenFileDialog();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProject)).BeginInit();
            this.splitContainerProject.Panel1.SuspendLayout();
            this.splitContainerProject.Panel2.SuspendLayout();
            this.splitContainerProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProps)).BeginInit();
            this.splitContainerProps.Panel1.SuspendLayout();
            this.splitContainerProps.Panel2.SuspendLayout();
            this.splitContainerProps.SuspendLayout();
            this.modelDockPanel.Content.SuspendLayout();
            this.modelDockPanel.SuspendLayout();
            this.propsDockPanel.Content.SuspendLayout();
            this.propsDockPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTimeline)).BeginInit();
            this.splitContainerTimeline.Panel2.SuspendLayout();
            this.splitContainerTimeline.SuspendLayout();
            this.timelineDockPanel.Content.SuspendLayout();
            this.timelineDockPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.importToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::Animatum.Properties.Resources.fileopen;
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Image = global::Animatum.Properties.Resources.fileimport;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.importToolStripMenuItem.Text = "&Import ASE...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(192, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::Animatum.Properties.Resources.filesave;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::Animatum.Properties.Resources.filesaveas;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Animatum.Properties.Resources.exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Image = global::Animatum.Properties.Resources.undo;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Image = global::Animatum.Properties.Resources.redo;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::Animatum.Properties.Resources.configure;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportABugToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // reportABugToolStripMenuItem
            // 
            this.reportABugToolStripMenuItem.Image = global::Animatum.Properties.Resources.help;
            this.reportABugToolStripMenuItem.Name = "reportABugToolStripMenuItem";
            this.reportABugToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.reportABugToolStripMenuItem.Text = "Report a bug...";
            this.reportABugToolStripMenuItem.Click += new System.EventHandler(this.reportABugToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(148, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::Animatum.Properties.Resources.info;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainerProject
            // 
            this.splitContainerProject.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerProject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerProject.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerProject.Location = new System.Drawing.Point(0, 24);
            this.splitContainerProject.Name = "splitContainerProject";
            // 
            // splitContainerProject.Panel1
            // 
            this.splitContainerProject.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerProject.Panel1.Controls.Add(this.splitContainerProps);
            this.splitContainerProject.Panel1.Controls.Add(this.toolbox);
            this.splitContainerProject.Panel1MinSize = 220;
            // 
            // splitContainerProject.Panel2
            // 
            this.splitContainerProject.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerProject.Panel2.Controls.Add(this.splitContainerTimeline);
            this.splitContainerProject.Panel2MinSize = 400;
            this.splitContainerProject.Size = new System.Drawing.Size(784, 438);
            this.splitContainerProject.SplitterDistance = 230;
            this.splitContainerProject.SplitterWidth = 3;
            this.splitContainerProject.TabIndex = 1;
            // 
            // splitContainerProps
            // 
            this.splitContainerProps.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerProps.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerProps.IsSplitterFixed = true;
            this.splitContainerProps.Location = new System.Drawing.Point(0, 50);
            this.splitContainerProps.Name = "splitContainerProps";
            this.splitContainerProps.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerProps.Panel1
            // 
            this.splitContainerProps.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerProps.Panel1.Controls.Add(this.modelDockPanel);
            this.splitContainerProps.Panel1MinSize = 200;
            // 
            // splitContainerProps.Panel2
            // 
            this.splitContainerProps.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerProps.Panel2.Controls.Add(this.propsDockPanel);
            this.splitContainerProps.Panel2MinSize = 175;
            this.splitContainerProps.Size = new System.Drawing.Size(228, 386);
            this.splitContainerProps.SplitterDistance = 208;
            this.splitContainerProps.SplitterWidth = 3;
            this.splitContainerProps.TabIndex = 1;
            // 
            // modelDockPanel
            // 
            this.modelDockPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // modelDockPanel.Content
            // 
            this.modelDockPanel.Content.Controls.Add(this.modelTreeView);
            this.modelDockPanel.Content.Location = new System.Drawing.Point(0, 20);
            this.modelDockPanel.Content.Name = "Content";
            this.modelDockPanel.Content.Size = new System.Drawing.Size(228, 188);
            this.modelDockPanel.Content.TabIndex = 0;
            this.modelDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelDockPanel.Icon = global::Animatum.Properties.Resources.modelIc;
            this.modelDockPanel.Location = new System.Drawing.Point(0, 0);
            this.modelDockPanel.Name = "modelDockPanel";
            this.modelDockPanel.Size = new System.Drawing.Size(228, 208);
            this.modelDockPanel.TabIndex = 0;
            this.modelDockPanel.Title = "Model";
            // 
            // modelTreeView
            // 
            this.modelTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelTreeView.ImageIndex = 0;
            this.modelTreeView.Location = new System.Drawing.Point(0, 0);
            this.modelTreeView.Model = null;
            this.modelTreeView.Name = "modelTreeView";
            this.modelTreeView.SelectedImageIndex = 0;
            this.modelTreeView.Size = new System.Drawing.Size(228, 188);
            this.modelTreeView.TabIndex = 0;
            this.modelTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.modelTreeView_AfterSelect);
            // 
            // propsDockPanel
            // 
            this.propsDockPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // propsDockPanel.Content
            // 
            this.propsDockPanel.Content.Controls.Add(this.propsControl);
            this.propsDockPanel.Content.Location = new System.Drawing.Point(0, 20);
            this.propsDockPanel.Content.Name = "Content";
            this.propsDockPanel.Content.Size = new System.Drawing.Size(228, 155);
            this.propsDockPanel.Content.TabIndex = 0;
            this.propsDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propsDockPanel.Icon = global::Animatum.Properties.Resources.info;
            this.propsDockPanel.Location = new System.Drawing.Point(0, 0);
            this.propsDockPanel.Name = "propsDockPanel";
            this.propsDockPanel.Size = new System.Drawing.Size(228, 175);
            this.propsDockPanel.TabIndex = 0;
            this.propsDockPanel.Title = "Properties";
            // 
            // propsControl
            // 
            this.propsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propsControl.Location = new System.Drawing.Point(0, 0);
            this.propsControl.Model = null;
            this.propsControl.Name = "propsControl";
            this.propsControl.SelectedNode = null;
            this.propsControl.Size = new System.Drawing.Size(228, 155);
            this.propsControl.TabIndex = 0;
            this.propsControl.NodeChanged += new System.EventHandler(this.propsControl_NodeChanged);
            // 
            // toolbox
            // 
            this.toolbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbox.Location = new System.Drawing.Point(0, 0);
            this.toolbox.MaximumSize = new System.Drawing.Size(0, 50);
            this.toolbox.MinimumSize = new System.Drawing.Size(125, 50);
            this.toolbox.Name = "toolbox";
            this.toolbox.SelectedTool = ToolboxItem.Select;
            this.toolbox.Size = new System.Drawing.Size(228, 50);
            this.toolbox.TabIndex = 0;
            this.toolbox.SelectedToolChanged += new System.EventHandler(this.toolbox_SelectedToolChanged);
            // 
            // splitContainerTimeline
            // 
            this.splitContainerTimeline.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerTimeline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTimeline.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerTimeline.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTimeline.Name = "splitContainerTimeline";
            this.splitContainerTimeline.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTimeline.Panel1
            // 
            this.splitContainerTimeline.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerTimeline.Panel1MinSize = 220;
            // 
            // splitContainerTimeline.Panel2
            // 
            this.splitContainerTimeline.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerTimeline.Panel2.Controls.Add(this.timelineDockPanel);
            this.splitContainerTimeline.Panel2MinSize = 175;
            this.splitContainerTimeline.Size = new System.Drawing.Size(549, 436);
            this.splitContainerTimeline.SplitterDistance = 258;
            this.splitContainerTimeline.SplitterWidth = 3;
            this.splitContainerTimeline.TabIndex = 0;
            // 
            // timelineDockPanel
            // 
            this.timelineDockPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // timelineDockPanel.Content
            // 
            this.timelineDockPanel.Content.Controls.Add(this.timeline);
            this.timelineDockPanel.Content.Location = new System.Drawing.Point(0, 20);
            this.timelineDockPanel.Content.Name = "Content";
            this.timelineDockPanel.Content.Size = new System.Drawing.Size(549, 155);
            this.timelineDockPanel.Content.TabIndex = 0;
            this.timelineDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timelineDockPanel.Icon = global::Animatum.Properties.Resources.timeline;
            this.timelineDockPanel.Location = new System.Drawing.Point(0, 0);
            this.timelineDockPanel.Name = "timelineDockPanel";
            this.timelineDockPanel.Size = new System.Drawing.Size(549, 175);
            this.timelineDockPanel.TabIndex = 0;
            this.timelineDockPanel.Title = "Timeline";
            // 
            // timeline
            // 
            this.timeline.DebugMode = false;
            this.timeline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeline.Location = new System.Drawing.Point(0, 0);
            this.timeline.Model = null;
            this.timeline.Name = "timeline";
            this.timeline.Size = new System.Drawing.Size(549, 155);
            this.timeline.TabIndex = 0;
            this.timeline.Ready += new System.EventHandler(this.timeline_Ready);
            this.timeline.ModelUpdated += new System.EventHandler(this.timeline_ModelUpdated);
            this.timeline.BeginPlayback += new System.EventHandler(this.timeline_BeginPlayback);
            this.timeline.PausePlayback += new System.EventHandler(this.timeline_PausePlayback);
            this.timeline.StopPlayback += new System.EventHandler(this.timeline_StopPlayback);
            // 
            // importDialog
            // 
            this.importDialog.Filter = "ASE 3D model files (*.ase)|*.ase";
            this.importDialog.RestoreDirectory = true;
            this.importDialog.SupportMultiDottedExtensions = true;
            this.importDialog.Title = "Import ASE 3D model";
            // 
            // openDialog
            // 
            this.openDialog.Filter = "Animation XML (*.xml)|*.xml";
            this.openDialog.Title = "Open Animation XML...";
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "Animation XML (*.xml)|*.xml";
            this.saveDialog.Title = "Save Animation XML...";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.splitContainerProject);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.Text = "Animatum 1.0b";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainerProject.Panel1.ResumeLayout(false);
            this.splitContainerProject.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProject)).EndInit();
            this.splitContainerProject.ResumeLayout(false);
            this.splitContainerProps.Panel1.ResumeLayout(false);
            this.splitContainerProps.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerProps)).EndInit();
            this.splitContainerProps.ResumeLayout(false);
            this.modelDockPanel.Content.ResumeLayout(false);
            this.modelDockPanel.ResumeLayout(false);
            this.propsDockPanel.Content.ResumeLayout(false);
            this.propsDockPanel.ResumeLayout(false);
            this.splitContainerTimeline.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTimeline)).EndInit();
            this.splitContainerTimeline.ResumeLayout(false);
            this.timelineDockPanel.Content.ResumeLayout(false);
            this.timelineDockPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerProject;
        private System.Windows.Forms.SplitContainer splitContainerTimeline;
        private System.Windows.Forms.SplitContainer splitContainerProps;
        private Controls.DockToolPanel modelDockPanel;
        private Controls.DockToolPanel propsDockPanel;
        private Controls.Toolbox toolbox;
        private Controls.DockToolPanel timelineDockPanel;
        private Controls.ModelTreeView modelTreeView;
        private System.Windows.Forms.OpenFileDialog importDialog;
        private Controls.PropertiesControl propsControl;
        private Controls.TimelineControl timeline;
        private System.Windows.Forms.ToolStripMenuItem reportABugToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.OpenFileDialog openDialog;
        private System.Windows.Forms.SaveFileDialog saveDialog;
    }
}