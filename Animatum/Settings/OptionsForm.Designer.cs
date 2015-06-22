namespace Animatum
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.renderAxiesCheckBox = new System.Windows.Forms.CheckBox();
            this.renderGridCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.frameRateUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.okayButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.timelineDebugCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.handednessComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.upComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateUpDown)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.renderAxiesCheckBox);
            this.groupBox1.Controls.Add(this.renderGridCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display";
            // 
            // renderAxiesCheckBox
            // 
            this.renderAxiesCheckBox.AutoSize = true;
            this.renderAxiesCheckBox.Location = new System.Drawing.Point(7, 44);
            this.renderAxiesCheckBox.Name = "renderAxiesCheckBox";
            this.renderAxiesCheckBox.Size = new System.Drawing.Size(88, 17);
            this.renderAxiesCheckBox.TabIndex = 1;
            this.renderAxiesCheckBox.Text = "Render axies";
            this.renderAxiesCheckBox.UseVisualStyleBackColor = true;
            // 
            // renderGridCheckBox
            // 
            this.renderGridCheckBox.AutoSize = true;
            this.renderGridCheckBox.Location = new System.Drawing.Point(7, 20);
            this.renderGridCheckBox.Name = "renderGridCheckBox";
            this.renderGridCheckBox.Size = new System.Drawing.Size(105, 17);
            this.renderGridCheckBox.TabIndex = 0;
            this.renderGridCheckBox.Text = "Render grid lines";
            this.renderGridCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.frameRateUpDown);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 87);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Playback";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 26);
            this.label3.TabIndex = 3;
            this.label3.Text = "(32 frames per second has been\r\nfound to be most optimal.)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "FPS";
            // 
            // frameRateUpDown
            // 
            this.frameRateUpDown.Location = new System.Drawing.Point(76, 20);
            this.frameRateUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frameRateUpDown.Name = "frameRateUpDown";
            this.frameRateUpDown.Size = new System.Drawing.Size(103, 20);
            this.frameRateUpDown.TabIndex = 1;
            this.frameRateUpDown.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Frame rate:";
            // 
            // okayButton
            // 
            this.okayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okayButton.Location = new System.Drawing.Point(74, 322);
            this.okayButton.Name = "okayButton";
            this.okayButton.Size = new System.Drawing.Size(75, 23);
            this.okayButton.TabIndex = 2;
            this.okayButton.Text = "Okay";
            this.okayButton.UseVisualStyleBackColor = true;
            this.okayButton.Click += new System.EventHandler(this.okayButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(156, 322);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.timelineDebugCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(13, 181);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(222, 47);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Timeline";
            // 
            // timelineDebugCheckBox
            // 
            this.timelineDebugCheckBox.AutoSize = true;
            this.timelineDebugCheckBox.Location = new System.Drawing.Point(7, 21);
            this.timelineDebugCheckBox.Name = "timelineDebugCheckBox";
            this.timelineDebugCheckBox.Size = new System.Drawing.Size(121, 17);
            this.timelineDebugCheckBox.TabIndex = 0;
            this.timelineDebugCheckBox.Text = "Enable debug mode";
            this.timelineDebugCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.upComboBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.handednessComboBox);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(13, 235);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(222, 78);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "XML";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Handedness:";
            // 
            // handednessComboBox
            // 
            this.handednessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.handednessComboBox.FormattingEnabled = true;
            this.handednessComboBox.Items.AddRange(new object[] {
            "Left",
            "Right"});
            this.handednessComboBox.Location = new System.Drawing.Point(85, 20);
            this.handednessComboBox.Name = "handednessComboBox";
            this.handednessComboBox.Size = new System.Drawing.Size(127, 21);
            this.handednessComboBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Up:";
            // 
            // upComboBox
            // 
            this.upComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.upComboBox.FormattingEnabled = true;
            this.upComboBox.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.upComboBox.Location = new System.Drawing.Point(85, 47);
            this.upComboBox.Name = "upComboBox";
            this.upComboBox.Size = new System.Drawing.Size(127, 21);
            this.upComboBox.TabIndex = 3;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.okayButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(243, 357);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options...";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameRateUpDown)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox renderAxiesCheckBox;
        private System.Windows.Forms.CheckBox renderGridCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown frameRateUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okayButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox timelineDebugCheckBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox handednessComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox upComboBox;
        private System.Windows.Forms.Label label5;
    }
}