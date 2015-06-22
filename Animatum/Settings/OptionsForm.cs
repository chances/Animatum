using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatum
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            this.renderGridCheckBox.Checked =
                Settings.GetSetting("display/renderGrid", true);
            this.renderAxiesCheckBox.Checked =
                Settings.GetSetting("display/renderAxies", true);
            this.frameRateUpDown.Value =
                Settings.GetSetting("playback/frameRate", 32);
            this.timelineDebugCheckBox.Checked=
                Settings.GetSetting("timeline/debugMode", false);
            string handedness = Settings.GetSetting("xml/handedness", "right");
            if (handedness == "left")
                this.handednessComboBox.SelectedIndex = 0;
            if (handedness == "right")
                this.handednessComboBox.SelectedIndex = 1;
            string up = Settings.GetSetting("xml/up", "z");
            if (up == "x")
                this.upComboBox.SelectedIndex = 0;
            if (up == "y")
                this.upComboBox.SelectedIndex = 1;
            if (up == "z")
                this.upComboBox.SelectedIndex = 2;
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            //Save settings
            Settings.PutSetting("display/renderGrid", this.renderGridCheckBox.Checked);
            Settings.PutSetting("display/renderAxies", this.renderAxiesCheckBox.Checked);
            Settings.PutSetting("playback/frameRate", (int)this.frameRateUpDown.Value);
            Settings.PutSetting("timeline/debugMode", this.timelineDebugCheckBox.Checked);
            Settings.PutSetting("xml/handedness",
                this.handednessComboBox.SelectedItem.ToString().ToLower());
            Settings.PutSetting("xml/up",
                this.upComboBox.SelectedItem.ToString().ToLower());
            //Okay
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