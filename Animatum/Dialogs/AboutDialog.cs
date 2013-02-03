using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatum.Dialogs
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void AboutDialog_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        private void contentPanel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
