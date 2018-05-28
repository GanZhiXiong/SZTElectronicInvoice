using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace SZTElectronicInvoice
{
    public partial class ZXDataGridViewX : DataGridViewX
    {
        #region 属性
        private bool isShowNumRowHeader = true;

        /// <summary>
        /// 是否显示行号
        /// </summary>
        public bool IsShowNumRowHeader
        {
            get { return isShowNumRowHeader; }
            set { isShowNumRowHeader = value; }
        }
        #endregion

        public ZXDataGridViewX()
        {
            InitializeComponent();

            this.AutoGenerateColumns = false;

            //设置列标题
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
//            headerStyle.Font = new Font("宋体", 11, FontStyle.Bold);
            this.ColumnHeadersDefaultCellStyle = headerStyle;

            //设置单元格
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        #region 方法
        /// <summary>
        /// DataGridViewX列和行设置
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="ColumnHeadersHeight"></param>
        /// <param name="RowTemplateHeight"></param>
        public void DataGridViewStyleSet(int ColumnHeadersHeight, int RowTemplateHeight)
        {
            this.RowsDefaultCellStyle.BackColor = Color.White;

            //设置列宽和行高
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.ColumnHeadersHeight = ColumnHeadersHeight;

            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.RowTemplate.Height = RowTemplateHeight;
            this.EnableHeadersVisualStyles = false;

            //设置列标题
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            headerStyle.Font = new Font("宋体", 11, FontStyle.Bold);
            this.ColumnHeadersDefaultCellStyle = headerStyle;

            //设置单元格 
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.DefaultCellStyle.Font = new Font("宋体", 10);

            //禁止用户改变DataGridView1の所有行的行高  
            this.AllowUserToResizeRows = false;

            // 禁止用户改变列头的高度  
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            //只能选择一行
            //this.MultiSelect = false;

            //选择整行
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToAddRows = false;

//            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.RowHeadersVisible = true;
        }
        #endregion

        #region DataGridViewX方法
        /// <summary>
        /// 新增一列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="headerText"></param>
        public virtual void AddColumn(string name, string headerText, bool visible = true)
        {
            DataGridViewTextBoxColumn txtBoxColumn = new DataGridViewTextBoxColumn();
            txtBoxColumn.DataPropertyName = txtBoxColumn.Name = name;
            txtBoxColumn.HeaderText = headerText;
            txtBoxColumn.Visible = visible;
            //txtBoxColumn.FillWeight = 100;
            this.Columns.Add(txtBoxColumn);
        }

        /// <summary>
        /// 新增一列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="headerText"></param>
        public virtual void AddColumn(string name, string headerText, int fillWeight, bool visible = true)
        {
            DataGridViewTextBoxColumn txtBoxColumn = new DataGridViewTextBoxColumn();
            txtBoxColumn.DataPropertyName = txtBoxColumn.Name = name;
            txtBoxColumn.HeaderText = headerText;
            txtBoxColumn.Visible = visible;
            txtBoxColumn.FillWeight = fillWeight;
            this.Columns.Add(txtBoxColumn);
        }
        #endregion

        #region 重写

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
//            DataGridViewStyleSet(30, 25);
            //设置列宽
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
//            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            base.OnDataBindingComplete(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (isShowNumRowHeader)
            {
                Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                            e.RowBounds.Location.Y,
                                            this.RowHeadersWidth,
                                            e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                        this.RowHeadersDefaultCellStyle.Font,
                        rectangle,
                        this.RowHeadersDefaultCellStyle.ForeColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            }
            base.OnRowPostPaint(e);
        }

        #endregion
    }
}
