namespace Animatum.Controls
{
    partial class Toolbox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.select = new System.Windows.Forms.CheckBox();
            this.orbit = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // select
            // 
            this.select.Appearance = System.Windows.Forms.Appearance.Button;
            this.select.Checked = true;
            this.select.CheckState = System.Windows.Forms.CheckState.Checked;
            this.select.Dock = System.Windows.Forms.DockStyle.Fill;
            this.select.Image = global::Animatum.Properties.Resources.lc_selectmode;
            this.select.Location = new System.Drawing.Point(3, 3);
            this.select.Name = "select";
            this.select.Size = new System.Drawing.Size(56, 44);
            this.select.TabIndex = 0;
            this.select.Text = "Select";
            this.select.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.select.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip.SetToolTip(this.select, "Click to select a mesh.");
            this.select.UseVisualStyleBackColor = true;
            this.select.CheckedChanged += new System.EventHandler(this.select_CheckedChanged);
            // 
            // orbit
            // 
            this.orbit.Appearance = System.Windows.Forms.Appearance.Button;
            this.orbit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orbit.Image = global::Animatum.Properties.Resources.selRot;
            this.orbit.Location = new System.Drawing.Point(65, 3);
            this.orbit.Name = "orbit";
            this.orbit.Size = new System.Drawing.Size(57, 44);
            this.orbit.TabIndex = 1;
            this.orbit.Text = "Orbit";
            this.orbit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.orbit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip.SetToolTip(this.orbit, "Click and drag to orbit around the model.");
            this.orbit.UseVisualStyleBackColor = true;
            this.orbit.CheckedChanged += new System.EventHandler(this.orbit_CheckedChanged);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.select, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.orbit, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(125, 50);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // Toolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.MaximumSize = new System.Drawing.Size(0, 50);
            this.MinimumSize = new System.Drawing.Size(125, 50);
            this.Name = "Toolbox";
            this.Size = new System.Drawing.Size(125, 50);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox select;
        private System.Windows.Forms.CheckBox orbit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
