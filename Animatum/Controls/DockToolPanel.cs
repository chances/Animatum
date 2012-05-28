using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Animatum.Controls
{
    [Designer(typeof(DockToolPanelDesigner))]
    public partial class DockToolPanel : UserControl
    {
        private string title;
        private Bitmap icon;

        public DockToolPanel()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;

            title = "DockToolWindow";
            icon = null;
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.Invalidate();
            }
        }

        public Bitmap Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                this.Invalidate();
            }
        }

        [Category("Appearance"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel Content
        {
            get { return panel; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Draw title bar
            Rectangle bounds = new Rectangle(0, 0, Width, Height - panel.Height);
            LinearGradientBrush grad = new LinearGradientBrush(
                bounds, Color.FromArgb(59, 128, 237), Color.FromArgb(50, 107, 200),
                LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(grad, bounds);
            //Icon (is any)
            if (icon != null)
                e.Graphics.DrawImage(icon, new Rectangle(4, 2, 16, 16));
            //Title
            RectangleF textBounds;
            if (icon == null)
                textBounds = new RectangleF(2, 3, Width - 3, Height - panel.Height);
            else
                textBounds = new RectangleF(21, 3, Width - 20, Height - panel.Height);
            StringFormat format = new StringFormat();
            format.Trimming = StringTrimming.EllipsisCharacter;
            e.Graphics.DrawString(title, this.Font, new SolidBrush(Color.White),
                textBounds, format);

            base.OnPaint(e);
        }

        private void DockToolPanel_Resize(object sender, EventArgs e)
        {
            this.panel.Location = new Point(0, 20);
            this.panel.Size = new Size(Width, Height - 20);
        }

        private void panel_Resize(object sender, EventArgs e)
        {
            this.panel.Location = new Point(0, 20);
            this.panel.Size = new Size(Width, Height - 20);
        }
    }
}
