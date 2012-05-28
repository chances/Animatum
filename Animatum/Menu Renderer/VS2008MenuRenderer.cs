using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VS2008StripRenderingLibrary {
    public class VS2008MenuRenderer : ToolStripRenderer {
        // Make sure the textcolor is black
        protected override void InitializeItem(ToolStripItem item) {
            base.InitializeItem(item);
            item.ForeColor = Color.Black;
        }

        protected override void Initialize(ToolStrip toolStrip) {
            base.Initialize(toolStrip);
            toolStrip.ForeColor = Color.Black;

        }

        // Render horizontal background gradient
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {
            base.OnRenderToolStripBackground(e);
            LinearGradientBrush b = new LinearGradientBrush(e.AffectedBounds,
                clsColor.clrHorBG_GrayBlue, clsColor.clrHorBG_White,
                    LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(b, e.AffectedBounds);
        }

        // Render image margin and gray ItemBackground
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) {
            base.OnRenderImageMargin(e);

            // Draw ImageMargin background gradient
            LinearGradientBrush b = new LinearGradientBrush(e.AffectedBounds, clsColor.clrImageMarginWhite, 
                clsColor.clrImageMarginBlue, LinearGradientMode.Horizontal);

            // Shadow at the right of image margin
            var DarkLine = new SolidBrush(clsColor.clrImageMarginLine);
            var WhiteLine = new SolidBrush(Color.White);
            var rect = new Rectangle(e.AffectedBounds.Width, 2, 1, e.AffectedBounds.Height);
            var rect2 = new Rectangle(e.AffectedBounds.Width + 1, 2, 1, e.AffectedBounds.Height);

            // Gray background
            var SubmenuBGbrush = new SolidBrush(clsColor.clrSubmenuBG);
            var rect3 = new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height);

            // Border
            var borderPen = new Pen(clsColor.clrMenuBorder);
            var rect4 = new Rectangle(0, 1, e.ToolStrip.Width - 1, e.ToolStrip.Height - 2);

            e.Graphics.FillRectangle(SubmenuBGbrush, rect3);
            e.Graphics.FillRectangle(b, e.AffectedBounds);
            e.Graphics.FillRectangle(DarkLine, rect);
            e.Graphics.FillRectangle(WhiteLine, rect2);
            e.Graphics.DrawRectangle(borderPen, rect4);
        }

        // Render checkmark
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e) {
            base.OnRenderItemCheck(e);

            if (e.Item.Selected) {
                var rect = new Rectangle(3, 1, 20, 20);
                var rect2 = new Rectangle(4, 2, 18, 18);
                SolidBrush b = new SolidBrush(clsColor.clrToolstripBtn_Border);
                SolidBrush b2 = new SolidBrush(clsColor.clrCheckBG);

                e.Graphics.FillRectangle(b, rect);
                e.Graphics.FillRectangle(b2, rect2);
                e.Graphics.DrawImage(e.Image, new Point(5, 3));
            } else {
                var rect = new Rectangle(3, 1, 20, 20);
                var rect2 = new Rectangle(4, 2, 18, 18);
                SolidBrush b = new SolidBrush(clsColor.clrSelectedBG_Drop_Border);
                SolidBrush b2 = new SolidBrush(clsColor.clrCheckBG);

                e.Graphics.FillRectangle(b, rect);
                e.Graphics.FillRectangle(b2, rect2);
                e.Graphics.DrawImage(e.Image, new Point(5, 3));
            }
        }

        // Render separator
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {
            base.OnRenderSeparator(e);

            var DarkLine = new SolidBrush(clsColor.clrImageMarginLine);
            var WhiteLine = new SolidBrush(Color.White);
            var rect = new Rectangle(32, 3, e.Item.Width - 32, 1);
            e.Graphics.FillRectangle(DarkLine, rect);
            e.Graphics.FillRectangle(WhiteLine, rect);
        }

        // Render arrow
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
            e.ArrowColor = Color.Black;
            base.OnRenderArrow(e);
        }

        // Render  MenuItem background: lightblue if selected, darkblue if dropped down
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
            base.OnRenderMenuItemBackground(e);

            if (e.Item.Enabled) {
                if (e.Item.IsOnDropDown == false && e.Item.Selected) {
                    // If item is MenuHeader and selected: draw darkblue color
                    var rect = new Rectangle(3, 2, e.Item.Width - 6, e.Item.Height - 4);
                    using (var b = new LinearGradientBrush(rect, clsColor.clrSelectedBG_White,
                        clsColor.clrSelectedBG_Header_Blue, LinearGradientMode.Vertical)) {
                    using (var b2 = new SolidBrush(clsColor.clrCheckBG)) {
                            e.Graphics.FillRectangle(b, rect);
                            clsColor.DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 4, clsColor.clrToolstripBtn_Border);
                            clsColor.DrawRoundedRectangle(e.Graphics, rect.Left - 2, rect.Top - 2, rect.Width + 2, rect.Height + 3, 4, Color.White);
                            e.Item.ForeColor = Color.Black;
                        }
                    }
                } else if (e.Item.IsOnDropDown && e.Item.Selected) {
                   // If item is NOT menuheader (but subitem); and selected: draw lightblue border

                    var rect = new Rectangle(4, 2, e.Item.Width - 6, e.Item.Height - 4);
                    using (var b = new LinearGradientBrush(rect, clsColor.clrSelectedBG_White,
                        clsColor.clrSelectedBG_Blue, LinearGradientMode.Vertical)) {
                        using (var b2 = new SolidBrush(clsColor.clrSelectedBG_Border)) {

                            e.Graphics.FillRectangle(b, rect);
                            clsColor.DrawRoundedRectangle(e.Graphics,
                                rect.Left - 1, rect.Top - 1, rect.Width,
                            rect.Height + 1, 6, clsColor.clrSelectedBG_Border);
                            e.Item.ForeColor = Color.Black;
                        }
                    }
                }

                // If item is MenuHeader and menu is dropped down; selection rectangle is now
                // darker
                if ((e.Item as ToolStripMenuItem).DropDown.Visible &&
                    e.Item.IsOnDropDown == false) {
                    // (e.Item as ToolStripMenuItem).OwnerItem == null
                    var rect = new Rectangle(3, 2, e.Item.Width - 6, e.Item.Height - 4);
                    using (var b = new LinearGradientBrush(rect, Color.White, clsColor.clrSelectedBG_Drop_Blue,
                        LinearGradientMode.Vertical)) {
                        using (var b2 = new SolidBrush(clsColor.clrSelectedBG_Drop_Border)) {
                            e.Graphics.FillRectangle(b, rect);
                            clsColor.DrawRoundedRectangle(
                                e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1,
                            4, clsColor.clrSelectedBG_Drop_Border);
                            clsColor.DrawRoundedRectangle(
                                e.Graphics, rect.Left - 2, rect.Top - 2, rect.Width + 2, rect.Height + 3, 4,
                                Color.White);
                            e.Item.ForeColor = Color.Black;
                        }
                    }
                }
            }
        }


    }
}
    
