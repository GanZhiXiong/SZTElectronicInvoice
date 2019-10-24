using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace SZTElectronicInvoice.Form
{
    public partial class InputVerificationCodeForm : MetroAppForm
    {
        public string Description { get; set; }

        public string Value { get; set; }

        public Image Image { get; set; }

        public InputVerificationCodeForm(Image image)
        {
            InitializeComponent();

            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;

//            Description = description;
//            Value = value;
            Image = image;

            picVerificationImage.Image = image;
//            descriptionLabelX.Text = description;
//            valueTextBoxX.Text = value;

            valueTextBoxX.KeyDown += ValueTextBoxX_KeyDown;
        }

        private void ValueTextBoxX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sureButtonX_Click(null, null);
            }
        }

        private void sureButtonX_Click(object sender, EventArgs e)
        {
            Value = valueTextBoxX.Text.Trim();

            this.Close();
        }
    }
}
