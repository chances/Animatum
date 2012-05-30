using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatum.Settings
{
    public partial class OptionsForm : Form
    {
        private Settings settings;

        public OptionsForm()
        {
            InitializeComponent();

            settings = new Settings();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            this.renderGridCheckBox.Checked =
                settings.GetSetting("display/renderGrid", true);
            this.renderAxiesCheckBox.Checked =
                settings.GetSetting("display/renderAxies", true);
            this.frameRateUpDown.Value =
                settings.GetSetting("playback/frameRate", 32);
            this.timelineDebugCheckBox.Checked=
                settings.GetSetting("timeline/debugMode", false);
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            //Save settings
            settings.PutSetting("display/renderGrid", this.renderGridCheckBox.Checked);
            settings.PutSetting("display/renderAxies", this.renderAxiesCheckBox.Checked);
            settings.PutSetting("playback/frameRate", (int)this.frameRateUpDown.Value);
            settings.PutSetting("timeline/debugMode", this.timelineDebugCheckBox.Checked);
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