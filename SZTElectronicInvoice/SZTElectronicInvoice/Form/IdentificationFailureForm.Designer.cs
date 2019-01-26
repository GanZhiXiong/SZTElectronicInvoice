namespace SZTElectronicInvoice.Form
{
    partial class IdentificationFailureForm
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
            this.sureButtonX = new DevComponents.DotNetBar.ButtonX();
            this.descriptionLabelX = new DevComponents.DotNetBar.LabelX();
            this.valueTextBoxX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.SuspendLayout();
            // 
            // sureButtonX
            // 
            this.sureButtonX.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.sureButtonX.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.sureButtonX.Font = new System.Drawing.Font("宋体", 11F);
            this.sureButtonX.Location = new System.Drawing.Point(242, 93);
            this.sureButtonX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.sureButtonX.Name = "sureButtonX";
            this.sureButtonX.Size = new System.Drawing.Size(103, 33);
            this.sureButtonX.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.sureButtonX.TabIndex = 1;
            this.sureButtonX.Text = "确  定";
            this.sureButtonX.Click += new System.EventHandler(this.sureButtonX_Click);
            // 
            // descriptionLabelX
            // 
            this.descriptionLabelX.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.descriptionLabelX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.descriptionLabelX.Dock = System.Windows.Forms.DockStyle.Top;
            this.descriptionLabelX.Font = new System.Drawing.Font("宋体", 12F);
            this.descriptionLabelX.ForeColor = System.Drawing.Color.Black;
            this.descriptionLabelX.Location = new System.Drawing.Point(1, 1);
            this.descriptionLabelX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.descriptionLabelX.Name = "descriptionLabelX";
            this.descriptionLabelX.Size = new System.Drawing.Size(586, 34);
            this.descriptionLabelX.TabIndex = 0;
            this.descriptionLabelX.Text = "labelX1";
            this.descriptionLabelX.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // valueTextBoxX
            // 
            this.valueTextBoxX.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.valueTextBoxX.Border.Class = "TextBoxBorder";
            this.valueTextBoxX.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.valueTextBoxX.DisabledBackColor = System.Drawing.Color.White;
            this.valueTextBoxX.Dock = System.Windows.Forms.DockStyle.Top;
            this.valueTextBoxX.Font = new System.Drawing.Font("宋体", 13F);
            this.valueTextBoxX.ForeColor = System.Drawing.Color.Black;
            this.valueTextBoxX.Location = new System.Drawing.Point(1, 35);
            this.valueTextBoxX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.valueTextBoxX.Name = "valueTextBoxX";
            this.valueTextBoxX.PreventEnterBeep = true;
            this.valueTextBoxX.Size = new System.Drawing.Size(586, 27);
            this.valueTextBoxX.TabIndex = 0;
            // 
            // IdentificationFailureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 138);
            this.Controls.Add(this.valueTextBoxX);
            this.Controls.Add(this.descriptionLabelX);
            this.Controls.Add(this.sureButtonX);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "IdentificationFailureForm";
            this.Text = "IdentificationFailureForm";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX sureButtonX;
        private DevComponents.DotNetBar.LabelX descriptionLabelX;
        private DevComponents.DotNetBar.Controls.TextBoxX valueTextBoxX;
    }
}