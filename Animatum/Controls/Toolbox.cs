using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatum.Controls
{
    public partial class Toolbox : UserControl
    {

        [Category("Property Changed")]
        public event EventHandler SelectedToolChanged;

        public Toolbox()
        {
            InitializeComponent();

            SelectedTool = ToolboxItem.Select;
        }

        public ToolboxItem SelectedTool
        {
            get;
            set;
        }

        private void OnSelectedToolChanged()
        {
            if (SelectedToolChanged != null)
                SelectedToolChanged(this, new EventArgs());
        }

        private void select_CheckedChanged(object sender, EventArgs e)
        {
            if (select.Checked)
            {
                SelectedTool = ToolboxItem.Select;
                orbit.Checked = false;
                OnSelectedToolChanged();
            }
        }

        private void orbit_CheckedChanged(object sender, EventArgs e)
        {
            if (orbit.Checked)
            {
                SelectedTool = ToolboxItem.Orbit;
                select.Checked = false;
                OnSelectedToolChanged();
            }
        }
    }
}
