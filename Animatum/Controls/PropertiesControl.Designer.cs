namespace Animatum.Controls
{
    partial class PropertiesControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.meshPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.meshNameLabel = new System.Windows.Forms.Label();
            this.meshColorComboBox = new Animatum.Controls.ColorComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.meshAlphaCheckBox = new System.Windows.Forms.CheckBox();
            this.bonePanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.bonePosLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.boneNameLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.meshComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.parentComboBox = new System.Windows.Forms.ComboBox();
            this.boneColorComboBox = new Animatum.Controls.ColorComboBox();
            this.nonePanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.noneLabel = new System.Windows.Forms.Label();
            this.meshPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.bonePanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.nonePanel.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // meshPanel
            // 
            this.meshPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.meshPanel.Controls.Add(this.tableLayoutPanel1);
            this.meshPanel.Location = new System.Drawing.Point(0, 0);
            this.meshPanel.Name = "meshPanel";
            this.meshPanel.Size = new System.Drawing.Size(228, 82);
            this.meshPanel.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.meshNameLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.meshColorComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.meshAlphaCheckBox, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(228, 82);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 27);
            this.label3.TabIndex = 4;
            this.label3.Text = "Color:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // meshNameLabel
            // 
            this.meshNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.meshNameLabel.Location = new System.Drawing.Point(60, 0);
            this.meshNameLabel.Name = "meshNameLabel";
            this.meshNameLabel.Size = new System.Drawing.Size(165, 27);
            this.meshNameLabel.TabIndex = 2;
            this.meshNameLabel.Text = "{Mesh Name}";
            this.meshNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // meshColorComboBox
            // 
            this.meshColorComboBox.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Red,
        System.Drawing.Color.Blue,
        System.Drawing.Color.Green};
            this.meshColorComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.meshColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.meshColorComboBox.FormattingEnabled = true;
            this.meshColorComboBox.Items.AddRange(new object[] {
            System.Drawing.Color.Black,
            System.Drawing.Color.Red,
            System.Drawing.Color.Blue,
            System.Drawing.Color.Green,
            System.Drawing.Color.Black,
            System.Drawing.Color.Red,
            System.Drawing.Color.Blue,
            System.Drawing.Color.Green,
            System.Drawing.Color.Black,
            System.Drawing.Color.Red,
            System.Drawing.Color.Blue,
            System.Drawing.Color.Green});
            this.meshColorComboBox.Location = new System.Drawing.Point(60, 30);
            this.meshColorComboBox.Name = "meshColorComboBox";
            this.meshColorComboBox.SelectedColor = System.Drawing.Color.Black;
            this.meshColorComboBox.Size = new System.Drawing.Size(137, 21);
            this.meshColorComboBox.TabIndex = 5;
            this.meshColorComboBox.SelectedIndexChanged += new System.EventHandler(this.meshColorComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 28);
            this.label5.TabIndex = 6;
            this.label5.Text = "Alpha:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // meshAlphaCheckBox
            // 
            this.meshAlphaCheckBox.AutoSize = true;
            this.meshAlphaCheckBox.Enabled = false;
            this.meshAlphaCheckBox.Location = new System.Drawing.Point(60, 60);
            this.meshAlphaCheckBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.meshAlphaCheckBox.Name = "meshAlphaCheckBox";
            this.meshAlphaCheckBox.Size = new System.Drawing.Size(96, 17);
            this.meshAlphaCheckBox.TabIndex = 7;
            this.meshAlphaCheckBox.Text = "Is transparent?";
            this.meshAlphaCheckBox.UseVisualStyleBackColor = true;
            this.meshAlphaCheckBox.CheckedChanged += new System.EventHandler(this.meshAlphaCheckBox_CheckedChanged);
            // 
            // bonePanel
            // 
            this.bonePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bonePanel.Controls.Add(this.tableLayoutPanel2);
            this.bonePanel.Location = new System.Drawing.Point(0, 0);
            this.bonePanel.Name = "bonePanel";
            this.bonePanel.Size = new System.Drawing.Size(228, 142);
            this.bonePanel.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.bonePosLabel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.boneNameLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.meshComboBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.parentComboBox, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.boneColorComboBox, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(228, 142);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 28);
            this.label4.TabIndex = 0;
            this.label4.Text = "Name:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bonePosLabel
            // 
            this.bonePosLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bonePosLabel.Location = new System.Drawing.Point(60, 28);
            this.bonePosLabel.Name = "bonePosLabel";
            this.bonePosLabel.Size = new System.Drawing.Size(165, 28);
            this.bonePosLabel.TabIndex = 3;
            this.bonePosLabel.Text = "{Bone Position}";
            this.bonePosLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 28);
            this.label6.TabIndex = 4;
            this.label6.Text = "Color:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // boneNameLabel
            // 
            this.boneNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boneNameLabel.Location = new System.Drawing.Point(60, 0);
            this.boneNameLabel.Name = "boneNameLabel";
            this.boneNameLabel.Size = new System.Drawing.Size(165, 28);
            this.boneNameLabel.TabIndex = 2;
            this.boneNameLabel.Text = "{Bone Name}";
            this.boneNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 28);
            this.label8.TabIndex = 1;
            this.label8.Text = "Position:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 28);
            this.label9.TabIndex = 6;
            this.label9.Text = "Mesh:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // meshComboBox
            // 
            this.meshComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.meshComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.meshComboBox.FormattingEnabled = true;
            this.meshComboBox.Location = new System.Drawing.Point(60, 89);
            this.meshComboBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.meshComboBox.Name = "meshComboBox";
            this.meshComboBox.Size = new System.Drawing.Size(165, 21);
            this.meshComboBox.TabIndex = 7;
            this.meshComboBox.SelectedIndexChanged += new System.EventHandler(this.meshComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 30);
            this.label2.TabIndex = 8;
            this.label2.Text = "Parent:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // parentComboBox
            // 
            this.parentComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parentComboBox.FormattingEnabled = true;
            this.parentComboBox.Location = new System.Drawing.Point(60, 117);
            this.parentComboBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.parentComboBox.Name = "parentComboBox";
            this.parentComboBox.Size = new System.Drawing.Size(165, 21);
            this.parentComboBox.TabIndex = 9;
            this.parentComboBox.SelectedIndexChanged += new System.EventHandler(this.parentComboBox_SelectedIndexChanged);
            // 
            // boneColorComboBox
            // 
            this.boneColorComboBox.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Red,
        System.Drawing.Color.Blue,
        System.Drawing.Color.Green};
            this.boneColorComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.boneColorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boneColorComboBox.FormattingEnabled = true;
            this.boneColorComboBox.Items.AddRange(new object[] {
            System.Drawing.Color.Black,
            System.Drawing.Color.Red,
            System.Drawing.Color.Blue,
            System.Drawing.Color.Green,
            System.Drawing.Color.Black,
            System.Drawing.Color.Red,
            System.Drawing.Color.Blue,
            System.Drawing.Color.Green,
            System.Drawing.Color.Black,
            System.Drawing.Color.Red,
            System.Drawing.Color.Blue,
            System.Drawing.Color.Green});
            this.boneColorComboBox.Location = new System.Drawing.Point(60, 59);
            this.boneColorComboBox.Name = "boneColorComboBox";
            this.boneColorComboBox.SelectedColor = System.Drawing.Color.Black;
            this.boneColorComboBox.Size = new System.Drawing.Size(137, 21);
            this.boneColorComboBox.TabIndex = 10;
            this.boneColorComboBox.SelectedIndexChanged += new System.EventHandler(this.boneColorComboBox_SelectedIndexChanged);
            // 
            // nonePanel
            // 
            this.nonePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nonePanel.Controls.Add(this.tableLayoutPanel3);
            this.nonePanel.Location = new System.Drawing.Point(0, 0);
            this.nonePanel.Name = "nonePanel";
            this.nonePanel.Size = new System.Drawing.Size(228, 145);
            this.nonePanel.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.noneLabel, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(228, 145);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // noneLabel
            // 
            this.noneLabel.AutoSize = true;
            this.noneLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.noneLabel.Location = new System.Drawing.Point(8, 53);
            this.noneLabel.Name = "noneLabel";
            this.noneLabel.Size = new System.Drawing.Size(211, 39);
            this.noneLabel.TabIndex = 0;
            this.noneLabel.Text = "Nothing selected.\r\n\r\nSelect a bone or mesh from the tree above.";
            this.noneLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nonePanel);
            this.Controls.Add(this.bonePanel);
            this.Controls.Add(this.meshPanel);
            this.Name = "PropertiesControl";
            this.Size = new System.Drawing.Size(228, 145);
            this.meshPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.bonePanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.nonePanel.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel meshPanel;
        private System.Windows.Forms.Label meshNameLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel bonePanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label bonePosLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label boneNameLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox meshComboBox;
        private System.Windows.Forms.Panel nonePanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label noneLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox parentComboBox;
        private ColorComboBox meshColorComboBox;
        private ColorComboBox boneColorComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox meshAlphaCheckBox;
    }
}
