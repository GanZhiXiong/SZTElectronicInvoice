namespace SZTElectronicInvoice
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.metroShell1 = new DevComponents.DotNetBar.Metro.MetroShell();
            this.metroTabPanel2 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.textBoxXInvoiceRecognitionResult = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.pictureBoxReceipt = new System.Windows.Forms.PictureBox();
            this.textBoxXBrowseInvoicePhotoFolder = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.metroToolbar4 = new DevComponents.DotNetBar.Metro.MetroToolbar();
            this.buttonItemStartBulkDownloadInvoice = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemStopBulkDownloadInvoice = new DevComponents.DotNetBar.ButtonItem();
            this.metroTabPanel3 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.checkBoxXSkipDownloadedFile = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.comboBoxCompanyName = new System.Windows.Forms.ComboBox();
            this.textBoxXTaxpayerRegistrationNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.metroToolbar3 = new DevComponents.DotNetBar.Metro.MetroToolbar();
            this.buttonItemAddCompanyInfo = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemDeleteCompanyInfo = new DevComponents.DotNetBar.ButtonItem();
            this.metroTabPanel1 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.circularProgressSingleDownload = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.labelXTransactionDate = new DevComponents.DotNetBar.LabelX();
            this.textBoxXIdentifyCode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxX1CardNum = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.monthCalendarAdvTransaction = new DevComponents.Editors.DateTimeAdv.MonthCalendarAdv();
            this.picVerificationImage = new System.Windows.Forms.PictureBox();
            this.metroToolbar1 = new DevComponents.DotNetBar.Metro.MetroToolbar();
            this.buttonItemStartDownloadInvoice = new DevComponents.DotNetBar.ButtonItem();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.metroTabPanel4 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.metroTabItem1 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem2 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem3 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem4 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroStatusBar1 = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this.progressBarItemBatchDownload = new DevComponents.DotNetBar.ProgressBarItem();
            this.superTooltip1 = new DevComponents.DotNetBar.SuperTooltip();
            this.balloonTip1 = new DevComponents.DotNetBar.BalloonTip();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerAutoDownloadFile = new System.Windows.Forms.Timer(this.components);
            this.zxDataGridViewXDownloadResult = new SZTElectronicInvoice.ZXDataGridViewX();
            this.buttonItemAbout = new DevComponents.DotNetBar.ButtonItem();
            this.metroShell1.SuspendLayout();
            this.metroTabPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReceipt)).BeginInit();
            this.metroTabPanel3.SuspendLayout();
            this.metroTabPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picVerificationImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zxDataGridViewXDownloadResult)).BeginInit();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerColorTint = System.Drawing.Color.Magenta;
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.OfficeMobile2014;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204))))));
            // 
            // metroShell1
            // 
            this.metroShell1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroShell1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShell1.CaptionVisible = true;
            this.metroShell1.Controls.Add(this.metroTabPanel3);
            this.metroShell1.Controls.Add(this.metroTabPanel2);
            this.metroShell1.Controls.Add(this.metroTabPanel1);
            this.metroShell1.Controls.Add(this.metroTabPanel4);
            this.metroShell1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroShell1.ForeColor = System.Drawing.Color.Black;
            this.metroShell1.HelpButtonText = null;
            this.metroShell1.HelpButtonVisible = false;
            this.metroShell1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.metroTabItem1,
            this.metroTabItem2,
            this.metroTabItem3,
            this.metroTabItem4,
            this.buttonItemAbout});
            this.metroShell1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.metroShell1.Location = new System.Drawing.Point(1, 1);
            this.metroShell1.Name = "metroShell1";
            this.metroShell1.SettingsButtonVisible = false;
            this.metroShell1.ShowIcon = false;
            this.metroShell1.Size = new System.Drawing.Size(764, 533);
            this.metroShell1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShell1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShell1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShell1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShell1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShell1.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShell1.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShell1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShell1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShell1.SystemText.QatDialogOkButton = "OK";
            this.metroShell1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShell1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShell1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShell1.TabIndex = 1;
            this.metroShell1.TabStripFont = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // metroTabPanel2
            // 
            this.metroTabPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.metroTabPanel2.Controls.Add(this.textBoxXInvoiceRecognitionResult);
            this.metroTabPanel2.Controls.Add(this.labelX5);
            this.metroTabPanel2.Controls.Add(this.pictureBoxReceipt);
            this.metroTabPanel2.Controls.Add(this.textBoxXBrowseInvoicePhotoFolder);
            this.metroTabPanel2.Controls.Add(this.labelX4);
            this.metroTabPanel2.Controls.Add(this.metroToolbar4);
            this.metroTabPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel2.Location = new System.Drawing.Point(0, 63);
            this.metroTabPanel2.Name = "metroTabPanel2";
            this.metroTabPanel2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.metroTabPanel2.Size = new System.Drawing.Size(764, 470);
            // 
            // 
            // 
            this.metroTabPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel2.TabIndex = 2;
            this.metroTabPanel2.Visible = false;
            // 
            // textBoxXInvoiceRecognitionResult
            // 
            this.textBoxXInvoiceRecognitionResult.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxXInvoiceRecognitionResult.Border.Class = "TextBoxBorder";
            this.textBoxXInvoiceRecognitionResult.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXInvoiceRecognitionResult.DisabledBackColor = System.Drawing.Color.White;
            this.textBoxXInvoiceRecognitionResult.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.textBoxXInvoiceRecognitionResult.ForeColor = System.Drawing.Color.Black;
            this.textBoxXInvoiceRecognitionResult.Location = new System.Drawing.Point(449, 84);
            this.textBoxXInvoiceRecognitionResult.Multiline = true;
            this.textBoxXInvoiceRecognitionResult.Name = "textBoxXInvoiceRecognitionResult";
            this.textBoxXInvoiceRecognitionResult.PreventEnterBeep = true;
            this.textBoxXInvoiceRecognitionResult.Size = new System.Drawing.Size(292, 381);
            this.textBoxXInvoiceRecognitionResult.TabIndex = 9;
            this.textBoxXInvoiceRecognitionResult.Text = "MTR港铁(深圳)\r\n清湖\r\n03/19/1807:45:12\r\n\r\n设备号 : 503\r\n操作员 : 001978\r\n收据号 : 00024202\r\n交易号 :" +
    " 58158\r\n卡编号 : 687676235\r\n\r\n票充值\r\n余额 : 4.65\r\n付费额 : 50.00\r\n新余额 : 54.65";
            this.textBoxXInvoiceRecognitionResult.WatermarkText = "这里显示识别结果";
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Location = new System.Drawing.Point(369, 244);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(80, 59);
            this.labelX5.Symbol = "";
            this.labelX5.SymbolSize = 44F;
            this.labelX5.TabIndex = 8;
            this.labelX5.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // pictureBoxReceipt
            // 
            this.pictureBoxReceipt.BackColor = System.Drawing.Color.White;
            this.pictureBoxReceipt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxReceipt.ForeColor = System.Drawing.Color.Black;
            this.pictureBoxReceipt.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxReceipt.Image")));
            this.pictureBoxReceipt.Location = new System.Drawing.Point(23, 84);
            this.pictureBoxReceipt.Name = "pictureBoxReceipt";
            this.pictureBoxReceipt.Size = new System.Drawing.Size(340, 381);
            this.pictureBoxReceipt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxReceipt.TabIndex = 7;
            this.pictureBoxReceipt.TabStop = false;
            // 
            // textBoxXBrowseInvoicePhotoFolder
            // 
            this.textBoxXBrowseInvoicePhotoFolder.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxXBrowseInvoicePhotoFolder.Border.Class = "TextBoxBorder";
            this.textBoxXBrowseInvoicePhotoFolder.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXBrowseInvoicePhotoFolder.ButtonCustom.Text = "浏览";
            this.textBoxXBrowseInvoicePhotoFolder.ButtonCustom.Visible = true;
            this.textBoxXBrowseInvoicePhotoFolder.DisabledBackColor = System.Drawing.Color.White;
            this.textBoxXBrowseInvoicePhotoFolder.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.textBoxXBrowseInvoicePhotoFolder.ForeColor = System.Drawing.Color.Black;
            this.textBoxXBrowseInvoicePhotoFolder.Location = new System.Drawing.Point(179, 47);
            this.textBoxXBrowseInvoicePhotoFolder.Name = "textBoxXBrowseInvoicePhotoFolder";
            this.textBoxXBrowseInvoicePhotoFolder.PreventEnterBeep = true;
            this.textBoxXBrowseInvoicePhotoFolder.Size = new System.Drawing.Size(562, 29);
            this.textBoxXBrowseInvoicePhotoFolder.TabIndex = 4;
            this.textBoxXBrowseInvoicePhotoFolder.ButtonCustomClick += new System.EventHandler(this.textBoxXBrowseInvoicePhotoFolder_ButtonCustomClick);
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Location = new System.Drawing.Point(2, 49);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(184, 23);
            this.labelX4.TabIndex = 5;
            this.labelX4.Text = "发票照片所在文件夹：";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // metroToolbar4
            // 
            this.metroToolbar4.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroToolbar4.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.metroToolbar4.BackgroundStyle.BorderBottomWidth = 1;
            this.metroToolbar4.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarCaptionBackground;
            this.metroToolbar4.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.metroToolbar4.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.metroToolbar4.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.metroToolbar4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroToolbar4.ContainerControlProcessDialogKey = true;
            this.metroToolbar4.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroToolbar4.DragDropSupport = true;
            this.metroToolbar4.ExpandButtonVisible = false;
            this.metroToolbar4.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroToolbar4.ForeColor = System.Drawing.Color.Black;
            this.metroToolbar4.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemStartBulkDownloadInvoice,
            this.buttonItemStopBulkDownloadInvoice});
            this.metroToolbar4.ItemSpacing = 2;
            this.metroToolbar4.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroToolbar4.Location = new System.Drawing.Point(3, 4);
            this.metroToolbar4.Name = "metroToolbar4";
            this.metroToolbar4.Size = new System.Drawing.Size(758, 37);
            this.metroToolbar4.TabIndex = 3;
            this.metroToolbar4.Text = "metroToolbar4";
            // 
            // buttonItemStartBulkDownloadInvoice
            // 
            this.buttonItemStartBulkDownloadInvoice.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemStartBulkDownloadInvoice.FontBold = true;
            this.buttonItemStartBulkDownloadInvoice.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.buttonItemStartBulkDownloadInvoice.Name = "buttonItemStartBulkDownloadInvoice";
            this.superTooltip1.SetSuperTooltip(this.buttonItemStartBulkDownloadInvoice, new DevComponents.DotNetBar.SuperTooltipInfo("", "", "1、请用手机拍摄深圳通电子发票\r\n2、通过手机微信将照片发送到电脑微信\r\n3、将照片保存到一个文件夹中\r\n4、选择电子发票照片所在的文件夹，点击“开始批量下载发票" +
            "”按钮即可自动\r\n   识别照片上发票信息，自动下载发票", null, null, DevComponents.DotNetBar.eTooltipColor.System, true, true, new System.Drawing.Size(444, 77)));
            this.buttonItemStartBulkDownloadInvoice.Symbol = "";
            this.buttonItemStartBulkDownloadInvoice.Text = "开始批量下载发票";
            this.buttonItemStartBulkDownloadInvoice.Click += new System.EventHandler(this.buttonItemStartBulkDownloadInvoice_Click);
            // 
            // buttonItemStopBulkDownloadInvoice
            // 
            this.buttonItemStopBulkDownloadInvoice.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemStopBulkDownloadInvoice.FontBold = true;
            this.buttonItemStopBulkDownloadInvoice.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.buttonItemStopBulkDownloadInvoice.Name = "buttonItemStopBulkDownloadInvoice";
            this.superTooltip1.SetSuperTooltip(this.buttonItemStopBulkDownloadInvoice, new DevComponents.DotNetBar.SuperTooltipInfo("", "", "1、请用手机拍摄深圳通电子发票\r\n2、通过手机微信将照片发送到电脑微信\r\n3、将照片保存到一个文件夹中\r\n4、选择电子发票照片所在的文件夹，点击“开始批量下载发票" +
            "”按钮即可自动\r\n   识别照片上发票信息，自动下载发票", null, null, DevComponents.DotNetBar.eTooltipColor.System, true, true, new System.Drawing.Size(444, 77)));
            this.buttonItemStopBulkDownloadInvoice.Symbol = "";
            this.buttonItemStopBulkDownloadInvoice.Text = "停止批量下载发票";
            this.buttonItemStopBulkDownloadInvoice.Click += new System.EventHandler(this.buttonItemStopBulkDownloadInvoice_Click);
            // 
            // metroTabPanel3
            // 
            this.metroTabPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.metroTabPanel3.Controls.Add(this.checkBoxXSkipDownloadedFile);
            this.metroTabPanel3.Controls.Add(this.comboBoxCompanyName);
            this.metroTabPanel3.Controls.Add(this.textBoxXTaxpayerRegistrationNumber);
            this.metroTabPanel3.Controls.Add(this.labelX7);
            this.metroTabPanel3.Controls.Add(this.labelX6);
            this.metroTabPanel3.Controls.Add(this.metroToolbar3);
            this.metroTabPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel3.Location = new System.Drawing.Point(0, 63);
            this.metroTabPanel3.Name = "metroTabPanel3";
            this.metroTabPanel3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.metroTabPanel3.Size = new System.Drawing.Size(764, 470);
            // 
            // 
            // 
            this.metroTabPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel3.TabIndex = 3;
            // 
            // checkBoxXSkipDownloadedFile
            // 
            this.checkBoxXSkipDownloadedFile.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.checkBoxXSkipDownloadedFile.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxXSkipDownloadedFile.CheckSignSize = new System.Drawing.Size(15, 15);
            this.checkBoxXSkipDownloadedFile.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.checkBoxXSkipDownloadedFile.ForeColor = System.Drawing.Color.Black;
            this.checkBoxXSkipDownloadedFile.Location = new System.Drawing.Point(169, 162);
            this.checkBoxXSkipDownloadedFile.Name = "checkBoxXSkipDownloadedFile";
            this.checkBoxXSkipDownloadedFile.Size = new System.Drawing.Size(194, 23);
            this.checkBoxXSkipDownloadedFile.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.superTooltip1.SetSuperTooltip(this.checkBoxXSkipDownloadedFile, new DevComponents.DotNetBar.SuperTooltipInfo("", "", "如果该发票文件之前已经生成过（可能下载了，也可能只是生成了而未下载），则跳过不下载。\r\n如下情况可以勾选该选项：\r\n1、防止重复下载该发票文件；\r\n2、防止下载的" +
            "发票公司信息不是配置的公司（发票已被其他人生成过）。", null, null, DevComponents.DotNetBar.eTooltipColor.System, true, true, new System.Drawing.Size(300, 88)));
            this.checkBoxXSkipDownloadedFile.TabIndex = 2;
            this.checkBoxXSkipDownloadedFile.Text = "跳过已下载的发票文件";
            this.checkBoxXSkipDownloadedFile.CheckedChanged += new System.EventHandler(this.checkBoxXSkipDownloadedFile_CheckedChanged);
            // 
            // comboBoxCompanyName
            // 
            this.comboBoxCompanyName.BackColor = System.Drawing.Color.White;
            this.comboBoxCompanyName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.comboBoxCompanyName.ForeColor = System.Drawing.Color.Black;
            this.comboBoxCompanyName.FormattingEnabled = true;
            this.comboBoxCompanyName.Location = new System.Drawing.Point(169, 47);
            this.comboBoxCompanyName.Name = "comboBoxCompanyName";
            this.comboBoxCompanyName.Size = new System.Drawing.Size(525, 29);
            this.comboBoxCompanyName.TabIndex = 0;
            // 
            // textBoxXTaxpayerRegistrationNumber
            // 
            this.textBoxXTaxpayerRegistrationNumber.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxXTaxpayerRegistrationNumber.Border.Class = "TextBoxBorder";
            this.textBoxXTaxpayerRegistrationNumber.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXTaxpayerRegistrationNumber.DisabledBackColor = System.Drawing.Color.White;
            this.textBoxXTaxpayerRegistrationNumber.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.textBoxXTaxpayerRegistrationNumber.ForeColor = System.Drawing.Color.Black;
            this.textBoxXTaxpayerRegistrationNumber.Location = new System.Drawing.Point(169, 104);
            this.textBoxXTaxpayerRegistrationNumber.Name = "textBoxXTaxpayerRegistrationNumber";
            this.textBoxXTaxpayerRegistrationNumber.PreventEnterBeep = true;
            this.textBoxXTaxpayerRegistrationNumber.Size = new System.Drawing.Size(525, 29);
            this.textBoxXTaxpayerRegistrationNumber.TabIndex = 1;
            this.textBoxXTaxpayerRegistrationNumber.TextChanged += new System.EventHandler(this.textBoxXTaxpayerRegistrationNumber_TextChanged);
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX7.ForeColor = System.Drawing.Color.Black;
            this.labelX7.Location = new System.Drawing.Point(23, 106);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(149, 23);
            this.labelX7.TabIndex = 6;
            this.labelX7.Text = "纳税人识别号：";
            this.labelX7.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Location = new System.Drawing.Point(74, 50);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(98, 23);
            this.labelX6.TabIndex = 4;
            this.labelX6.Text = "公司名称：";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // metroToolbar3
            // 
            this.metroToolbar3.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroToolbar3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroToolbar3.ContainerControlProcessDialogKey = true;
            this.metroToolbar3.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroToolbar3.DragDropSupport = true;
            this.metroToolbar3.ExpandButtonVisible = false;
            this.metroToolbar3.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroToolbar3.ForeColor = System.Drawing.Color.Black;
            this.metroToolbar3.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemAddCompanyInfo,
            this.buttonItemDeleteCompanyInfo});
            this.metroToolbar3.ItemSpacing = 2;
            this.metroToolbar3.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroToolbar3.Location = new System.Drawing.Point(3, 4);
            this.metroToolbar3.Name = "metroToolbar3";
            this.metroToolbar3.Size = new System.Drawing.Size(758, 37);
            this.metroToolbar3.TabIndex = 2;
            this.metroToolbar3.Text = "metroToolbar3";
            // 
            // buttonItemAddCompanyInfo
            // 
            this.buttonItemAddCompanyInfo.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemAddCompanyInfo.FontBold = true;
            this.buttonItemAddCompanyInfo.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.buttonItemAddCompanyInfo.Name = "buttonItemAddCompanyInfo";
            this.buttonItemAddCompanyInfo.Symbol = "";
            this.buttonItemAddCompanyInfo.Text = "添加";
            this.buttonItemAddCompanyInfo.Click += new System.EventHandler(this.buttonItemAddCompanyInfo_Click);
            // 
            // buttonItemDeleteCompanyInfo
            // 
            this.buttonItemDeleteCompanyInfo.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemDeleteCompanyInfo.FontBold = true;
            this.buttonItemDeleteCompanyInfo.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.buttonItemDeleteCompanyInfo.Name = "buttonItemDeleteCompanyInfo";
            this.buttonItemDeleteCompanyInfo.Symbol = "";
            this.buttonItemDeleteCompanyInfo.Text = "删除";
            this.buttonItemDeleteCompanyInfo.Click += new System.EventHandler(this.buttonItemDeleteCompanyInfo_Click);
            // 
            // metroTabPanel1
            // 
            this.metroTabPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.metroTabPanel1.Controls.Add(this.circularProgressSingleDownload);
            this.metroTabPanel1.Controls.Add(this.labelXTransactionDate);
            this.metroTabPanel1.Controls.Add(this.textBoxXIdentifyCode);
            this.metroTabPanel1.Controls.Add(this.textBoxX1CardNum);
            this.metroTabPanel1.Controls.Add(this.monthCalendarAdvTransaction);
            this.metroTabPanel1.Controls.Add(this.picVerificationImage);
            this.metroTabPanel1.Controls.Add(this.metroToolbar1);
            this.metroTabPanel1.Controls.Add(this.labelX3);
            this.metroTabPanel1.Controls.Add(this.labelX2);
            this.metroTabPanel1.Controls.Add(this.labelX1);
            this.metroTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.metroTabPanel1.Location = new System.Drawing.Point(0, 63);
            this.metroTabPanel1.Name = "metroTabPanel1";
            this.metroTabPanel1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.metroTabPanel1.Size = new System.Drawing.Size(764, 470);
            // 
            // 
            // 
            this.metroTabPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel1.TabIndex = 1;
            this.metroTabPanel1.Visible = false;
            // 
            // circularProgressSingleDownload
            // 
            this.circularProgressSingleDownload.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.circularProgressSingleDownload.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circularProgressSingleDownload.BackgroundStyle.Font = new System.Drawing.Font("微软雅黑", 5F);
            this.circularProgressSingleDownload.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.circularProgressSingleDownload.Font = new System.Drawing.Font("微软雅黑", 2F);
            this.circularProgressSingleDownload.Location = new System.Drawing.Point(3, 347);
            this.circularProgressSingleDownload.Name = "circularProgressSingleDownload";
            this.circularProgressSingleDownload.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Dot;
            this.circularProgressSingleDownload.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.circularProgressSingleDownload.ProgressTextFormat = "";
            this.circularProgressSingleDownload.ProgressTextVisible = true;
            this.circularProgressSingleDownload.Size = new System.Drawing.Size(758, 120);
            this.circularProgressSingleDownload.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circularProgressSingleDownload.TabIndex = 10;
            // 
            // labelXTransactionDate
            // 
            this.labelXTransactionDate.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelXTransactionDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelXTransactionDate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelXTransactionDate.ForeColor = System.Drawing.Color.Black;
            this.labelXTransactionDate.Location = new System.Drawing.Point(135, 110);
            this.labelXTransactionDate.Name = "labelXTransactionDate";
            this.labelXTransactionDate.Size = new System.Drawing.Size(170, 23);
            this.labelXTransactionDate.TabIndex = 9;
            this.labelXTransactionDate.Text = "请选择一个日期";
            // 
            // textBoxXIdentifyCode
            // 
            this.textBoxXIdentifyCode.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxXIdentifyCode.Border.Class = "TextBoxBorder";
            this.textBoxXIdentifyCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXIdentifyCode.DisabledBackColor = System.Drawing.Color.White;
            this.textBoxXIdentifyCode.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.textBoxXIdentifyCode.ForeColor = System.Drawing.Color.Black;
            this.textBoxXIdentifyCode.Location = new System.Drawing.Point(135, 302);
            this.textBoxXIdentifyCode.Name = "textBoxXIdentifyCode";
            this.textBoxXIdentifyCode.PreventEnterBeep = true;
            this.textBoxXIdentifyCode.Size = new System.Drawing.Size(170, 29);
            this.textBoxXIdentifyCode.TabIndex = 1;
            // 
            // textBoxX1CardNum
            // 
            this.textBoxX1CardNum.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxX1CardNum.Border.Class = "TextBoxBorder";
            this.textBoxX1CardNum.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX1CardNum.DisabledBackColor = System.Drawing.Color.White;
            this.textBoxX1CardNum.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.textBoxX1CardNum.ForeColor = System.Drawing.Color.Black;
            this.textBoxX1CardNum.Location = new System.Drawing.Point(135, 58);
            this.textBoxX1CardNum.Name = "textBoxX1CardNum";
            this.textBoxX1CardNum.PreventEnterBeep = true;
            this.textBoxX1CardNum.Size = new System.Drawing.Size(170, 29);
            this.textBoxX1CardNum.TabIndex = 0;
            // 
            // monthCalendarAdvTransaction
            // 
            this.monthCalendarAdvTransaction.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.monthCalendarAdvTransaction.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderBottomWidth = 1;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderLeftWidth = 1;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderRightWidth = 1;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.monthCalendarAdvTransaction.BackgroundStyle.BorderTopWidth = 1;
            this.monthCalendarAdvTransaction.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.monthCalendarAdvTransaction.Colors.Selection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            // 
            // 
            // 
            this.monthCalendarAdvTransaction.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.monthCalendarAdvTransaction.ContainerControlProcessDialogKey = true;
            this.monthCalendarAdvTransaction.DisplayMonth = new System.DateTime(2007, 10, 1, 0, 0, 0, 0);
            this.monthCalendarAdvTransaction.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.monthCalendarAdvTransaction.ForeColor = System.Drawing.Color.Black;
            this.monthCalendarAdvTransaction.Location = new System.Drawing.Point(135, 140);
            this.monthCalendarAdvTransaction.MultiSelect = true;
            this.monthCalendarAdvTransaction.Name = "monthCalendarAdvTransaction";
            // 
            // 
            // 
            this.monthCalendarAdvTransaction.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.monthCalendarAdvTransaction.Size = new System.Drawing.Size(532, 139);
            this.monthCalendarAdvTransaction.TabIndex = 6;
            this.monthCalendarAdvTransaction.Text = "monthCalendarAdv2";
            // 
            // picVerificationImage
            // 
            this.picVerificationImage.BackColor = System.Drawing.Color.White;
            this.picVerificationImage.ForeColor = System.Drawing.Color.Black;
            this.picVerificationImage.Image = global::SZTElectronicInvoice.Properties.Resources.logo;
            this.picVerificationImage.Location = new System.Drawing.Point(323, 288);
            this.picVerificationImage.Name = "picVerificationImage";
            this.picVerificationImage.Size = new System.Drawing.Size(100, 50);
            this.picVerificationImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.superTooltip1.SetSuperTooltip(this.picVerificationImage, new DevComponents.DotNetBar.SuperTooltipInfo("点击图片，换一张验证码", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Default));
            this.picVerificationImage.TabIndex = 1;
            this.picVerificationImage.TabStop = false;
            this.picVerificationImage.Click += new System.EventHandler(this.picVerificationImage_Click);
            // 
            // metroToolbar1
            // 
            this.metroToolbar1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroToolbar1.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.metroToolbar1.BackgroundStyle.BorderBottomWidth = 1;
            this.metroToolbar1.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarCaptionBackground;
            this.metroToolbar1.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.metroToolbar1.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.metroToolbar1.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.metroToolbar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroToolbar1.ContainerControlProcessDialogKey = true;
            this.metroToolbar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroToolbar1.DragDropSupport = true;
            this.metroToolbar1.ExpandButtonVisible = false;
            this.metroToolbar1.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroToolbar1.ForeColor = System.Drawing.Color.Black;
            this.metroToolbar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItemStartDownloadInvoice});
            this.metroToolbar1.ItemSpacing = 2;
            this.metroToolbar1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroToolbar1.Location = new System.Drawing.Point(3, 4);
            this.metroToolbar1.Name = "metroToolbar1";
            this.metroToolbar1.Size = new System.Drawing.Size(758, 37);
            this.metroToolbar1.TabIndex = 2;
            this.metroToolbar1.Text = "metroToolbar1";
            // 
            // buttonItemStartDownloadInvoice
            // 
            this.buttonItemStartDownloadInvoice.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItemStartDownloadInvoice.FontBold = true;
            this.buttonItemStartDownloadInvoice.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.buttonItemStartDownloadInvoice.Name = "buttonItemStartDownloadInvoice";
            this.buttonItemStartDownloadInvoice.Symbol = "";
            this.buttonItemStartDownloadInvoice.Text = "下载（Alt+X）";
            this.buttonItemStartDownloadInvoice.Click += new System.EventHandler(this.buttonItemSearchElectronicInvoice_Click);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Location = new System.Drawing.Point(40, 302);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(98, 23);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "验  证  码：";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(40, 110);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(98, 23);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "交易日期：";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(40, 58);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(98, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "卡       号：";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // metroTabPanel4
            // 
            this.metroTabPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel4.Location = new System.Drawing.Point(0, 0);
            this.metroTabPanel4.Name = "metroTabPanel4";
            this.metroTabPanel4.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.metroTabPanel4.Size = new System.Drawing.Size(764, 533);
            // 
            // 
            // 
            this.metroTabPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel4.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel4.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel4.TabIndex = 4;
            this.metroTabPanel4.Visible = false;
            // 
            // metroTabItem1
            // 
            this.metroTabItem1.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.metroTabItem1.Name = "metroTabItem1";
            this.metroTabItem1.Panel = this.metroTabPanel1;
            this.metroTabItem1.Text = "手动";
            // 
            // metroTabItem2
            // 
            this.metroTabItem2.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.metroTabItem2.Name = "metroTabItem2";
            this.metroTabItem2.Panel = this.metroTabPanel2;
            this.metroTabItem2.Text = "自动";
            // 
            // metroTabItem3
            // 
            this.metroTabItem3.Checked = true;
            this.metroTabItem3.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this.metroTabItem3.Name = "metroTabItem3";
            this.metroTabItem3.Panel = this.metroTabPanel3;
            this.metroTabItem3.Text = "配置";
            this.metroTabItem3.Click += new System.EventHandler(this.metroTabItem3_Click);
            // 
            // metroTabItem4
            // 
            this.metroTabItem4.Name = "metroTabItem4";
            this.metroTabItem4.Panel = this.metroTabPanel4;
            this.metroTabItem4.Text = "metroTabItem4";
            // 
            // metroStatusBar1
            // 
            this.metroStatusBar1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroStatusBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroStatusBar1.ContainerControlProcessDialogKey = true;
            this.metroStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroStatusBar1.DragDropSupport = true;
            this.metroStatusBar1.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroStatusBar1.ForeColor = System.Drawing.Color.Black;
            this.metroStatusBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.progressBarItemBatchDownload});
            this.metroStatusBar1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroStatusBar1.Location = new System.Drawing.Point(1, 671);
            this.metroStatusBar1.Name = "metroStatusBar1";
            this.metroStatusBar1.Size = new System.Drawing.Size(764, 30);
            this.metroStatusBar1.TabIndex = 2;
            this.metroStatusBar1.Text = "metroStatusBar1";
            // 
            // progressBarItemBatchDownload
            // 
            // 
            // 
            // 
            this.progressBarItemBatchDownload.BackStyle.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.progressBarItemBatchDownload.BackStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarItemBatchDownload.BackStyle.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.progressBarItemBatchDownload.BackStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.progressBarItemBatchDownload.BackStyle.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.progressBarItemBatchDownload.ChunkColor = System.Drawing.Color.Yellow;
            this.progressBarItemBatchDownload.ChunkColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.progressBarItemBatchDownload.ChunkGradientAngle = 0F;
            this.progressBarItemBatchDownload.Height = 22;
            this.progressBarItemBatchDownload.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.progressBarItemBatchDownload.Name = "progressBarItemBatchDownload";
            this.progressBarItemBatchDownload.RecentlyUsed = false;
            this.progressBarItemBatchDownload.Stretch = true;
            this.progressBarItemBatchDownload.Text = "234";
            this.progressBarItemBatchDownload.TextVisible = true;
            // 
            // superTooltip1
            // 
            this.superTooltip1.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this.superTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            // 
            // balloonTip1
            // 
            this.balloonTip1.AutoCloseTimeOut = 3;
            this.balloonTip1.DefaultBalloonWidth = 100;
            this.balloonTip1.Enabled = false;
            this.balloonTip1.MinimumBalloonWidth = 50;
            this.balloonTip1.ShowBalloonOnFocus = true;
            this.balloonTip1.Style = DevComponents.DotNetBar.eBallonStyle.Alert;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.ForeColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = global::SZTElectronicInvoice.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // timerAutoDownloadFile
            // 
            this.timerAutoDownloadFile.Interval = 1000;
            this.timerAutoDownloadFile.Tick += new System.EventHandler(this.timerAutoDownloadFile_Tick);
            // 
            // zxDataGridViewXDownloadResult
            // 
            this.zxDataGridViewXDownloadResult.BackgroundColor = System.Drawing.Color.White;
            this.zxDataGridViewXDownloadResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.zxDataGridViewXDownloadResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.zxDataGridViewXDownloadResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.zxDataGridViewXDownloadResult.DefaultCellStyle = dataGridViewCellStyle2;
            this.zxDataGridViewXDownloadResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zxDataGridViewXDownloadResult.EnableHeadersVisualStyles = false;
            this.zxDataGridViewXDownloadResult.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.zxDataGridViewXDownloadResult.IsShowNumRowHeader = true;
            this.zxDataGridViewXDownloadResult.Location = new System.Drawing.Point(1, 534);
            this.zxDataGridViewXDownloadResult.Name = "zxDataGridViewXDownloadResult";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.zxDataGridViewXDownloadResult.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.zxDataGridViewXDownloadResult.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.zxDataGridViewXDownloadResult.RowTemplate.Height = 23;
            this.zxDataGridViewXDownloadResult.Size = new System.Drawing.Size(764, 137);
            this.zxDataGridViewXDownloadResult.TabIndex = 4;
            // 
            // buttonItemAbout
            // 
            this.buttonItemAbout.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.buttonItemAbout.Name = "buttonItemAbout";
            this.buttonItemAbout.Text = "关于";
            this.buttonItemAbout.Click += new System.EventHandler(this.buttonItemAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 702);
            this.Controls.Add(this.zxDataGridViewXDownloadResult);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.metroStatusBar1);
            this.Controls.Add(this.metroShell1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MetroForm";
            this.metroShell1.ResumeLayout(false);
            this.metroShell1.PerformLayout();
            this.metroTabPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReceipt)).EndInit();
            this.metroTabPanel3.ResumeLayout(false);
            this.metroTabPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picVerificationImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zxDataGridViewXDownloadResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.Metro.MetroShell metroShell1;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel3;
        private DevComponents.DotNetBar.Metro.MetroToolbar metroToolbar3;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel2;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel1;
        private DevComponents.DotNetBar.Metro.MetroToolbar metroToolbar1;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem1;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem2;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox picVerificationImage;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXIdentifyCode;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1CardNum;
        private DevComponents.Editors.DateTimeAdv.MonthCalendarAdv monthCalendarAdvTransaction;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonItem buttonItemStartDownloadInvoice;
        private DevComponents.DotNetBar.SuperTooltip superTooltip1;
        private DevComponents.DotNetBar.LabelX labelXTransactionDate;
        private DevComponents.DotNetBar.BalloonTip balloonTip1;
        private DevComponents.DotNetBar.Metro.MetroToolbar metroToolbar4;
        private DevComponents.DotNetBar.ButtonItem buttonItemStartBulkDownloadInvoice;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXBrowseInvoicePhotoFolder;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXInvoiceRecognitionResult;
        private DevComponents.DotNetBar.LabelX labelX5;
        private System.Windows.Forms.PictureBox pictureBoxReceipt;
        private ZXDataGridViewX zxDataGridViewXDownloadResult;
        private DevComponents.DotNetBar.ButtonItem buttonItemStopBulkDownloadInvoice;
        private DevComponents.DotNetBar.Controls.CircularProgress circularProgressSingleDownload;
        private DevComponents.DotNetBar.ProgressBarItem progressBarItemBatchDownload;
        private DevComponents.DotNetBar.Metro.MetroStatusBar metroStatusBar1;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXTaxpayerRegistrationNumber;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel4;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem4;
        private System.Windows.Forms.ComboBox comboBoxCompanyName;
        private DevComponents.DotNetBar.ButtonItem buttonItemAddCompanyInfo;
        private DevComponents.DotNetBar.ButtonItem buttonItemDeleteCompanyInfo;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxXSkipDownloadedFile;
        private System.Windows.Forms.Timer timerAutoDownloadFile;
        private DevComponents.DotNetBar.ButtonItem buttonItemAbout;
    }
}