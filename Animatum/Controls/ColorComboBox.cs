using System;
using System.Drawing;
using System.Windows.Forms;

namespace Animatum.Controls
{
    public class ColorComboBox : ComboBox
    {
        private Color[] colors = { Color.Black, Color.Red, Color.Blue, Color.Green };

        protected int inMargin;
        protected int boxWidth;
        private Color selColor;

        public EventHandler ColorChanged;

        public ColorComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            inMargin = 2;
            boxWidth = 3;
            BeginUpdate();
            InitCombo();
            EndUpdate();
        }

        public Color[] Colors
        {
            get { return colors; }
            set
            {
                colors = value;
                this.Items.Clear();
                InitCombo();
            }
        }

        public Color SelectedColor
        {
            get { return selColor; }
            set
            {
                foreach (Color color in colors)
                    if (color == value)
                        this.SelectedItem = color;
            }
        }

        private void InitCombo()
        {
            if (colors == null) return;
            if (colors.Length < 1) return;

            foreach (Color color in colors)
            {
                try
                {
                    if (color.IsKnownColor)
                        this.Items.Add(color);
                }
                catch
                {
                    throw new Exception("Invalid Color Name: " + color);
                }
            }

            SelectedColor = colors[0];
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            selColor = colors[SelectedIndex];
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            if ((e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit)
                e.DrawBackground();

            Graphics g = e.Graphics;
            if (e.Index == -1)  //if index is -1 do nothing
                return;
            Color c = (Color)base.Items[e.Index];

            //the color rectangle
            g.FillRectangle(new SolidBrush(c), e.Bounds.X + this.inMargin, e.Bounds.Y +
                this.inMargin, e.Bounds.Width / this.boxWidth - 2 * this.inMargin,
                e.Bounds.Height - 2 * this.inMargin);
            //draw border around color rectangle
            g.DrawRectangle(Pens.Black, e.Bounds.X + this.inMargin,
                e.Bounds.Y + this.inMargin, e.Bounds.Width / this.boxWidth -
                2 * this.inMargin, e.Bounds.Height - 2 * this.inMargin);
            //draw strings
            g.DrawString(c.Name, e.Font, new SolidBrush(ForeColor),
                (float)(e.Bounds.Width / this.boxWidth + 5 * this.inMargin),
                (float)e.Bounds.Y);
        }
    }
}