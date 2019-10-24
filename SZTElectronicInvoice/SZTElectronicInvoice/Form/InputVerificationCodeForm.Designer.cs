namespace SZTElectronicInvoice.Form
{
    partial class InputVerificationCodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputVerificationCodeForm));
            this.picVerificationImage = new System.Windows.Forms.PictureBox();
            this.valueTextBoxX = new DevComponents.DotNetBar.Controls.TextBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.picVerificationImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picVerificationImage
            // 
            this.picVerificationImage.BackColor = System.Drawing.Color.White;
            this.picVerificationImage.ForeColor = System.Drawing.Color.Black;
            this.picVerificationImage.Image = ((System.Drawing.Image)(resources.GetObject("picVerificationImage.Image")));
            this.picVerificationImage.Location = new System.Drawing.Point(1, 3);
            this.picVerificationImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picVerificationImage.Name = "picVerificationImage";
            this.picVerificationImage.Size = new System.Drawing.Size(116, 102);
            this.picVerificationImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picVerificationImage.TabIndex = 2;
            this.picVerificationImage.TabStop = false;
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
            this.valueTextBoxX.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.valueTextBoxX.ForeColor = System.Drawing.Color.Black;
            this.valueTextBoxX.Location = new System.Drawing.Point(121, 39);
            this.valueTextBoxX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.valueTextBoxX.Name = "valueTextBoxX";
            this.valueTextBoxX.PreventEnterBeep = true;
            this.valueTextBoxX.Size = new System.Drawing.Size(231, 29);
            this.valueTextBoxX.TabIndex = 3;
            this.valueTextBoxX.WatermarkBehavior = DevComponents.DotNetBar.eWatermarkBehavior.HideNonEmpty;
            this.valueTextBoxX.WatermarkText = "请输入验证码结果后按回车健";
            // 
            // InputVerificationCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 108);
            this.Controls.Add(this.valueTextBoxX);
            this.Controls.Add(this.picVerificationImage);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "InputVerificationCodeForm";
            this.Text = "InputVerificationCodeForm";
            ((System.ComponentModel.ISupportInitialize)(this.picVerificationImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picVerificationImage;
        private DevComponents.DotNetBar.Controls.TextBoxX valueTextBoxX;
    }
}