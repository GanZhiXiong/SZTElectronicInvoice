using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;
using Newtonsoft.Json.Linq;
using SZTElectronicInvoice.Common;
using SZTElectronicInvoice.Form;
using SZTElectronicInvoice.Model;
using TencentYoutuYun.SDK.Csharp;

namespace SZTElectronicInvoice
{
    public partial class MainForm : MetroAppForm
    {
        private bool _startBatchDownload;

        private Stopwatch _autoDownloadStopwatch = new Stopwatch();

        private int _autoDownloadCount = 0;
        private int _autoHaveDownloadedCount = 0;

        private List<ElectronicInvoiceInfo> _currentAutoHaveDownloadElectronicInvoiceInfos =
            new List<ElectronicInvoiceInfo>();

        private decimal _downloadedTotalAmount;

        //        private bool _isRetryFailure;

        #region 窗体构造函数

        public MainForm()
        {
            #region test

            /* string path = @"D:\Program Files\QQRecord\1551935335\FileRecv\MobileFile\2\IMG_2365.PNG";
             long imageFileSize = new FileInfo(path).Length;
             if (imageFileSize >= 1024 * 1024)
             {
                 bool compressResult = GZXImageManager.CompressImage(path, path, 50, 999);
             }*/

            /*      MatchCollection mc =
                      Regex.Matches(
                          @"现在需要从HTML 页面中读取中文，我现在的解决方案是取 >文字<中间的字符，谁有更好的解决方案。还有谁知道 如何用正则判断 < > 吗？val=""100"">100元</td>",
                          @"(?<=^|>)((?![<>]).)+(?=元<|$)");
                  foreach (Match m in mc)
                  {
                      Console.WriteLine(m.Value);
                  }*/

            #endregion

            InitializeComponent();

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Text = "深圳通电子发票";
            this.MaximizeBox = false;
            //            CheckForIllegalCrossThreadCalls = false;

            metroShell1.TitleText = "<font size=\"11\" face=\"微软雅黑\">深圳通电子发票</font>";

            //            webBrowser1.Navigate("https://www.shenzhentong.com/service/fplist_101007009_368124878_20180308.html");

#if !DEBUG
            groupPanel1.Visible = false;
#endif

            #region monthCalendarAdvTransaction

            DateTime dateTime = DateTime.Now;
            //            this.monthCalendarAdvTransaction.DisplayMonth =
            //                new DateTime(dateTime.Year, dateTime.Month - 1, dateTime.Day, 0, 0, 0, 0);
            this.monthCalendarAdvTransaction.DisplayMonth = dateTime.AddMonths(-1);
            monthCalendarAdvTransaction.MultiSelect = false;
            monthCalendarAdvTransaction.SelectedDate = dateTime;

            this.monthCalendarAdvTransaction.DateChanged += MonthCalendarAdvTransaction_DateChanged;
            monthCalendarAdvTransaction.DateSelected += MonthCalendarAdvTransaction_DateSelected;

            #endregion

            #region 自动

            pictureBoxReceipt.Image = null;
            textBoxXBrowseInvoicePhotoFolder.Clear();
            textBoxXInvoiceRecognitionResult.Clear();

            progressBarItemBatchDownload.Text = "";
            progressBarItemBatchDownload.Width = metroStatusBar1.Width - 3;
            //            progressBarItemBatchDownload.Value += 50;
            buttonItemRetryFailure.Enabled = buttonItemStopBulkDownloadInvoice.Enabled = false;

            #endregion

            InitDataGridView();

            #region 腾讯云识别

            // 设置为你自己的密钥对
            string appid = "10126985";
            string secretId = "AKIDWxfJt7oixS0PwOozqeLvV2fBYkolMimh";
            string secretKey = "5FLOYa8lK0XxPkvzwkT2cDe5MSOEyZ0I";
            string userid = "1551935335";

            Conf.Instance().setAppInfo(appid, secretId, secretKey, userid, Conf.Instance().YOUTU_END_POINT);

            #endregion
        }

        #endregion

        #region 重写

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            metroShell1.SelectedTab = metroTabItem1;

            //textBoxXBrowseInvoicePhotoFolder.Text = @"D:\Program Files\QQRecord\1551935335\FileRecv\MobileFile";

            textBoxX1CardNum.TabIndex = 0;
            textBoxX1CardNum.Focus();

            this.pictureBox1.BackColor = ColorTranslator.FromHtml("#007acc");
            picVerificationImage.Image = GZXNetworking.GetValidateImage();

            //            string s= Path.GetDirectoryName(@"C:\Users\15519\Desktop\未下载成功的发票\IMG_1434.JPG");
            //            string s = Path.Combine(Path.GetDirectoryName(@"C: \Users\15519\Desktop\未下载成功的发票"),
            //                "123" + DateTime.Now.ToString(" HHmmss.fff") + ".JPG");
            //            return;

            //buttonItemStartBulkDownloadInvoice.Enabled = false;
            //buttonItemStartDownloadInvoice.Enabled = false;

            #region 读取配置文件

            textBoxXBrowseInvoicePhotoFolder.Text = GlobalManager.UserConfig.BrowseInvoicePhotoFolder;
            checkBoxXSkipDownloadedFile.Checked = GlobalManager.UserConfig.IsSkipDownloadFile;
            //            BindingSource bs = new BindingSource();
            //bs.DataSource= GlobalManager.UserConfig.CompanyInfos;
            //            comboBoxCompanyName.DataSource = bs;
            comboBoxCompanyName.DataSource = GlobalManager.UserConfig.CompanyInfos;
            comboBoxCompanyName.DisplayMember = "CompanyName";

            comboBoxCompanyName.SelectedItem = GlobalManager.UserConfig.SelectedCompanyInfo;
            if (GlobalManager.UserConfig.SelectedCompanyInfo != null)
            {
                textBoxXTaxpayerRegistrationNumber.Text =
                    GlobalManager.UserConfig.SelectedCompanyInfo.TaxpayerRegistrationNumber;
            }

            comboBoxCompanyName.SelectedIndexChanged += ComboBoxCompanyName_SelectedIndexChanged;

            #endregion

            //            pictureBoxReceipt.Image=Image.FromFile(@"C:\Users\15519\Desktop\11\IMG_1917.JPG");
            //
            //            return;

            //            MessageBoxEx.Show(
            //                "<font size=\"15\" color=\"red\">验证码识别是著者付费购买第三方自动识别验证码平台接口<br/>该平台识别验证码是按照识别个数计算费用，现在验证码识别次数已用完，需要续费才能继续使用<br/>若想继续使用，请在打开的页面，找到【捐助开发者】，微信或支付宝给著者捐助点，著者收到钱后，会将其充值到验证码识别平台<br/>若不想捐助，那就请给作者一个★Star吧<br/>在打开的页面点击右上角的Star即可，不要钱的</font>");
            //            Process.Start("https://github.com/GanZhiXiong/SZTElectronicInvoice");
            //buttonItemStartDownloadInvoice.Enabled = true;
            return;
            Thread thread = new Thread(() =>
            {
                circularProgressSingleDownload.IsRunning = true;
                circularProgressSingleDownload.ProgressText = "请等待";
                circularProgressSingleDownload.ProgressTextVisible = true;

                picVerificationImage.Image = GZXNetworking.GetValidateImage();
                string orcResult = GZXNetworking.ORC(picVerificationImage.Image);

                if (string.IsNullOrWhiteSpace(orcResult))
                {
                    VerificationCodeIdentificationRequiresRenewaFee();
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        textBoxXIdentifyCode.Text = orcResult;
                        buttonItemStartBulkDownloadInvoice.Enabled = true;
                        buttonItemStartDownloadInvoice.Enabled = true;
                        circularProgressSingleDownload.IsRunning = false;
                        circularProgressSingleDownload.ProgressTextVisible = false;
                    }));
                }
            });
            thread.Start();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.X && e.Alt)
            {
                buttonItemSearchElectronicInvoice_Click(null, null);
            }
        }

        #endregion

        #region private method

        private void DownloadResult(ElectronicInvoiceInfo electronicInvoiceInfo)
        {
            this.Invoke(new Action(() =>
            {
                //                                progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / images.Count();
                UpdateProgressBarItemBatchDownload();
                _currentAutoHaveDownloadElectronicInvoiceInfos.Add(electronicInvoiceInfo);
                GlobalManager.ElectronicInvoiceInfos.Add(electronicInvoiceInfo);
            }));
        }

        private void VerificationCodeIdentificationRequiresRenewaFee()
        {
            MessageBoxEx.Show(
                "<font size=\"15\" color=\"red\">验证码识别是著者付费购买第三方自动识别验证码平台接口<br/>该平台识别验证码是按照识别个数计算费用，现在验证码识别次数已用完，需要续费才能继续使用<br/>若想继续使用，请在打开的页面，找到【捐助开发者】，微信或支付宝给著者捐助点，著者收到钱后，会将其充值到验证码识别平台<br/>若不想捐助，那就请给作者一个★Star吧<br/>在打开的页面点击右上角的★Star即可，不要钱的</font>");
            Process.Start("https://github.com/GanZhiXiong/SZTElectronicInvoice");
        }

        private void ShowBalloon(Control control, string message)
        {
            balloonTip1.SetBalloonText(control, message);
            balloonTip1.ShowBalloon(control);
        }

        private bool IsConfigurationComplete()
        {
            if (GlobalManager.UserConfig.SelectedCompanyInfo == null ||
                string.IsNullOrWhiteSpace(GlobalManager.UserConfig.SelectedCompanyInfo.CompanyName))
            {
                //                metroTabPanel3.MetroTabItem= metroToolbar3;

                metroShell1.SelectedTab = metroTabItem3;
                balloonTip1.SetBalloonText(comboBoxCompanyName, "请配置公司名称");
                balloonTip1.ShowBalloon(comboBoxCompanyName);

                comboBoxCompanyName.Focus();
                comboBoxCompanyName.SelectAll();
                return false;
            }

            if (string.IsNullOrWhiteSpace(GlobalManager.UserConfig.SelectedCompanyInfo.TaxpayerRegistrationNumber))
            {
                //                metroTabPanel3.MetroTabItem= metroToolbar3;

                metroShell1.SelectedTab = metroTabItem3;
                balloonTip1.SetBalloonText(textBoxXTaxpayerRegistrationNumber, "请配置纳税人识别号");
                balloonTip1.ShowBalloon(textBoxXTaxpayerRegistrationNumber);

                textBoxXTaxpayerRegistrationNumber.Focus();
                textBoxXTaxpayerRegistrationNumber.SelectAll();
                return false;
            }

            return true;
        }

        private void UpdateProgressBarItemBatchDownload()
        {
            progressBarItemBatchDownload.Value = (int)(progressBarItemBatchDownload.Maximum *
                                                        ((double)_autoHaveDownloadedCount / _autoDownloadCount));
            //            progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / blockLength;
            progressBarItemBatchDownload.TextVisible = true;
            UpdateprogressBarItemBatchDownloadText();
        }

        private void UpdateprogressBarItemBatchDownloadText()
        {
            progressBarItemBatchDownload.TextVisible = true;
            //            double progress = ((double)progressBarItemBatchDownload.Value / progressBarItemBatchDownload.Maximum) *
            //                              100;
            //            progress = progress >= 99 ? 100 : progress;
            double progress = ((double)_autoHaveDownloadedCount / _autoDownloadCount) * 100;
            _downloadedTotalAmount = _currentAutoHaveDownloadElectronicInvoiceInfos
                .FindAll(a => a.IsDownloaded == true && a.DownloadResult == "下载发票成功").Sum(a => a.RechargeAmount);
            //if (downloadedTotalAmount>=3000)
            //{

            //}
            //            progressBarItemBatchDownload.Text = "完成：" + (progress).ToString("0.##") + "%" + "   用时：" + (int)_autoDownloadStopwatch.Elapsed.TotalSeconds + "秒";
            progressBarItemBatchDownload.Text = "已成功/已完成/总共(张):" +
                                                _currentAutoHaveDownloadElectronicInvoiceInfos
                                                    .FindAll(a => a.IsDownloaded).Count + "/" +
                                                _autoHaveDownloadedCount + "/" + _autoDownloadCount + "   合计(元):" +
                                                _downloadedTotalAmount +
                                                "   完成:" + (progress).ToString("0.##") + "%" + "   用时:" +
                                                _autoDownloadStopwatch.Elapsed.Minutes + "分" +
                                                _autoDownloadStopwatch.Elapsed.Seconds + "秒";
        }

        private void InitDataGridView()
        {
            #region DataGridViewX属性设置

            zxDataGridViewXDownloadResult.DataSource = GlobalManager.ElectronicInvoiceInfos;
            zxDataGridViewXDownloadResult.MultiSelect = false;
            zxDataGridViewXDownloadResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            zxDataGridViewXDownloadResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            zxDataGridViewXDownloadResult.AddColumn("ImageFileName", "图片名称");
            zxDataGridViewXDownloadResult.AddColumn("CardNum", "卡号");
            zxDataGridViewXDownloadResult.AddColumn("TransactionDate", "交易日期");
            zxDataGridViewXDownloadResult.AddColumn("RechargeAmount", "金额");
            zxDataGridViewXDownloadResult.AddColumn("IsDownloadedString", "是否成功");

            zxDataGridViewXDownloadResult.AddColumn("CompleteTime", "完成时间");
            zxDataGridViewXDownloadResult.AddColumn("DownloadResult", "下载结果");

            #endregion

            #region DataGridViewX 事件 

            zxDataGridViewXDownloadResult.RowsAdded += ZxDataGridViewXDownloadResult_RowsAdded;
            zxDataGridViewXDownloadResult.CellMouseClick += ZxDataGridViewXDownloadResult_CellMouseClick;
            zxDataGridViewXDownloadResult.CellEndEdit += ZxDataGridViewXDownloadResult_CellEndEdit;
            //            TaskGridView.MouseClick += TaskGridView_MouseClick;
            //            TaskGridView.RowPostPaint += TaskGridView_RowPostPaint;
            //            TaskGridView.RowStateChanged += TaskGridView_RowStateChanged;

            #endregion
        }

        #region DataGridViewX 事件 
        private void ZxDataGridViewXDownloadResult_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //            Console.WriteLine(zxDataGridViewXDownloadResult[e.ColumnIndex, e.RowIndex].Value);
            DataGridViewRow row = zxDataGridViewXDownloadResult.Rows[e.RowIndex];
            ElectronicInvoiceInfo electronicInvoiceInfo = (ElectronicInvoiceInfo) row.DataBoundItem;
            Console.WriteLine(row.Cells[e.ColumnIndex].Value + ", " + electronicInvoiceInfo.CardNum);


        }

        private void ZxDataGridViewXDownloadResult_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) != 0)
            {
                buttonXzxDataGridViewXDownloadResultRightMenu.Popup(MousePosition);
            }
        }

        private void ZxDataGridViewXDownloadResult_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ZXDataGridViewX myDataGridViewX = (ZXDataGridViewX)sender;
            myDataGridViewX.FirstDisplayedScrollingRowIndex = myDataGridViewX.RowCount - 1;
        }

        #endregion

        private bool DownloadElectronicInvoice(ref string downloadResult,
            string cardNum, string transactionDate, string downloadFileName,
            ElectronicInvoiceInfo electronicInvoiceInfo = null)
        {
            try
            {
                string orcResult;
                string url = "https://www.shenzhentong.com/Ajax/ElectronicInvoiceAjax.aspx";

                string ret = GZXNetworking.PostRequest(url, string.Format("tp={0}&yzm={1}&cardnum={2}", "1",
                    textBoxXIdentifyCode.Text, cardNum
                ));
                //            GetValidateImage();

                string state = StringOperation.GetMiddleText(ret, @"state"":""", @"""");
                if (!state.Equals("100"))
                {
                    //picVerificationImage.Image = GZXNetworking.GetValidateImage();
                    //orcResult = GZXNetworking.ORC(picVerificationImage.Image);

                    //textBoxXIdentifyCode.BeginInvoke(new EventHandler(delegate
                    //{
                    //    textBoxXIdentifyCode.Text = orcResult;
                    //}));

                    downloadResult = "第1条充值记录不存在或者已开票";
                    return false;
                }

                string fplistUrl = string.Format("https://www.shenzhentong.com/service/fplist_101007009_{0}_{1}.html",
                    cardNum, transactionDate);
                string ret1 = GZXNetworking.GetRequest(fplistUrl);

                MatchCollection mc = Regex.Matches(ret1, @"(?<=^|>)((?![<>]).)+(?=元<|$)");
                foreach (Match m in mc)
                {
                    Console.WriteLine(m.Value);
                }

                decimal rechargeAmout = 0;
                if (mc.Count != 0)
                {
                    if (!String.IsNullOrWhiteSpace(mc[0].Value))
                    {
                        rechargeAmout = Convert.ToDecimal(mc[0].Value);
                    }
                }

                if (electronicInvoiceInfo != null)
                {
                    electronicInvoiceInfo.RechargeAmount = rechargeAmout;
                }

                GlobalManager.DownloadPath = GlobalManager.MainDownloadPath +
                                             GlobalManager.UserConfig.SelectedCompanyInfo.CompanyName + "/" +
                                             rechargeAmout + "/";
                Directory.CreateDirectory(GlobalManager.DownloadPath);

                if (ret1.Contains("开票中"))
                {
                    //picVerificationImage.Image = GZXNetworking.GetValidateImage();
                    //orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                    //textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

                    downloadResult = "开票中，请稍后再试";
                    return false;
                }

                if (ret1.Contains(".pdf"))
                {
                    if (GlobalManager.UserConfig.IsSkipDownloadFile)
                    {
                        downloadResult = "跳过";
                        return true;
                    }

                    Regex regImg = new Regex(
                        @"(?is)<a[^>]*?href=(['""\s]?)(?<href>([^'""\s]*\.doc)|([^'""\s]*\.docx)|([^'""\s]*\.xls)|([^'""\s]*\.xlsx)|([^'""\s]*\.ppt)|([^'""\s]*\.txt)|([^'""\s]*\.zip)|([^'""\s]*\.rar)|([^'""\s]*\.gz)|([^'""\s]*\.pdf))\1[^>]*?>");
                    MatchCollection match = regImg.Matches(ret1);
                    foreach (Match m in match)
                    {
                        //                Response.Write(m.Groups["href"].Value + "<br/>");
                        //                Console.WriteLine(m.Groups["href"].Value );
                        string pdfDownloadUrl = m.Groups["href"].Value;
                        bool pdfDownloadRet = GZXNetworking.DownloadFile(pdfDownloadUrl, GlobalManager.DownloadPath,
                            downloadFileName, 3);
                        //picVerificationImage.Image = GZXNetworking.GetValidateImage();
                        //orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                        //textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

                        if (pdfDownloadRet)
                        {
                            downloadResult = "下载发票成功";
                            return true;
                        }
                        else
                        {
                            downloadResult = "下载发票失败";
                            return false;
                        }
                    }
                }

                string sj = StringOperation.GetMiddleText(ret1, @"sj=""", @""">");

                if (string.IsNullOrWhiteSpace(sj))
                {
                    downloadResult = StringOperation.GetMiddleText(ret1, @"<p><br>", @"<br><br></p>")
                        .Replace("<br><br>", "");
                    //                downloadResult = "sj为空";
                    return false;
                }

                string lsh = StringOperation.GetMiddleText(ret1, @"lsh=""", @"""");
                string zdh = StringOperation.GetMiddleText(ret1, @"zdh=""", @"""");

                //            tp	3
                //jlsh	14
                //jzdh	262011503
                //jkh	368282314
                //jrq	20180412
                //jsj	223956
                //jfirmfpmc	环汇系统科技（深圳）有限公司
                //jfirmsbh	914403003429400273
                //jfirmaddre	
                //jfirmtel	
                //jfirmyh	
                //jfirmyhzh	
                //jfirmphone	13728754591
                StringBuilder paramStringBuilder = new StringBuilder();
                paramStringBuilder.Append("tp=3");
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jlsh=" + lsh);
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jzdh=" + zdh);
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jkh=" + cardNum);
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jrq=" + transactionDate);
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jsj=" + sj);
                paramStringBuilder.Append("&");
                //            paramStringBuilder.Append("jfirmfpmc=" + "深圳市华汇设计有限公司");
                paramStringBuilder.Append("jfirmfpmc=" + GlobalManager.UserConfig.SelectedCompanyInfo.CompanyName);

                //            paramStringBuilder.Append("jfirmfpmc=" + "环汇系统科技（深圳）有限公司");
                paramStringBuilder.Append("&");
                //            paramStringBuilder.Append("jfirmsbh=" + "914403003429400273");
                //            paramStringBuilder.Append("jfirmsbh=" + "91440300752525766M");
                paramStringBuilder.Append("jfirmsbh=" +
                                          GlobalManager.UserConfig.SelectedCompanyInfo.TaxpayerRegistrationNumber);

                paramStringBuilder.Append("&");


                paramStringBuilder.Append("jfirmaddre=");
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jfirmtel=");
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jfirmyh=");
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jfirmyhzh=");
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jfirmphone=" + "13728754591");
                string ret2 = GZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                string strs = StringOperation.GetMiddleText(ret2, @"strs"":""", @""",");
                if (string.IsNullOrWhiteSpace(strs))
                {
                    string message = StringOperation.GetMiddleText(ret2, @"message"":""", @"""");

                    downloadResult = message;
                    return false;
                }

                paramStringBuilder = new StringBuilder();
                paramStringBuilder.Append("tp=4");
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("pid=" + strs);
                Thread.Sleep(3000);
                string ret3 = GZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                string downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
                if (string.IsNullOrWhiteSpace(downloadUrl))
                {
                    Thread.Sleep(3000);
                    ret3 = GZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                    downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
                }

                if (string.IsNullOrWhiteSpace(downloadUrl))
                {
                    Thread.Sleep(3000);
                    ret3 = GZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                    downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
                }

                if (string.IsNullOrWhiteSpace(downloadUrl))
                {
                    Thread.Sleep(3000);
                    ret3 = GZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                    downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
                }

                if (string.IsNullOrWhiteSpace(downloadUrl))
                {
                    url = string.Format("https://www.shenzhentong.com/service/fplist_101007009_{0}_{1}.html",
                        cardNum, transactionDate);
                    ret1 = GZXNetworking.GetRequest(url);

                    if (ret1.Contains("开票中"))
                    {
                        //picVerificationImage.Image = GZXNetworking.GetValidateImage();
                        //orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                        //textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

                        //                    balloonTip1.SetBalloonText(textBoxX1CardNum, "开票中，请稍后再试");
                        //                    balloonTip1.ShowBalloon(textBoxX1CardNum);

                        textBoxX1CardNum.Focus();
                        textBoxX1CardNum.SelectAll();

                        downloadResult = "开票中，请稍后再试";
                        return false;
                    }

                    if (ret1.Contains(".pdf"))
                    {
                        if (GlobalManager.UserConfig.IsSkipDownloadFile)
                        {
                            downloadResult = "跳过";
                            return true;
                        }

                        Regex regImg = new Regex(
                            @"(?is)<a[^>]*?href=(['""\s]?)(?<href>([^'""\s]*\.doc)|([^'""\s]*\.docx)|([^'""\s]*\.xls)|([^'""\s]*\.xlsx)|([^'""\s]*\.ppt)|([^'""\s]*\.txt)|([^'""\s]*\.zip)|([^'""\s]*\.rar)|([^'""\s]*\.gz)|([^'""\s]*\.pdf))\1[^>]*?>");
                        MatchCollection match = regImg.Matches(ret1);
                        foreach (Match m in match)
                        {
                            //                Response.Write(m.Groups["href"].Value + "<br/>");
                            //                Console.WriteLine(m.Groups["href"].Value );
                            string pdfDownloadUrl = m.Groups["href"].Value;
                            bool pdfDownloadRet = GZXNetworking.DownloadFile(pdfDownloadUrl, GlobalManager.DownloadPath,
                                downloadFileName, 3);

                            if (pdfDownloadRet)
                            {
                                downloadResult = "下载发票成功";
                                return true;
                            }
                            else
                            {
                                downloadResult = "下载发票失败";
                                return false;
                            }
                        }
                    }

                    downloadResult = "下载地址为空";
                    return false;
                }

                bool downloadRet = GZXNetworking.DownloadFile(downloadUrl, GlobalManager.DownloadPath,
                    downloadFileName, 3);
                //picVerificationImage.Image = GZXNetworking.GetValidateImage();
                //orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                //textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

                if (downloadRet)
                {
                    downloadResult = "下载发票成功";
                    return true;
                }
                else
                {
                    downloadResult = "下载发票失败";
                    return false;
                }
            }
            catch (Exception exception)
            {
                downloadResult = exception.Message;
                return false;
                //                throw;
            }
        }

        #endregion

        #region MonthCalendarAdv事件

        private void MonthCalendarAdvTransaction_DateSelected(object sender, DateRangeEventArgs e)
        {
            labelXTransactionDate.Text = monthCalendarAdvTransaction.SelectedDate.ToString("yyyy-MM-dd");
        }

        private void MonthCalendarAdvTransaction_DateChanged(object sender, EventArgs e)
        {
        }

        #endregion

        #region 控件点击事件

        private void buttonItemAddCompanyInfo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxCompanyName.Text))
            {
                balloonTip1.SetBalloonText(comboBoxCompanyName, "请输入公司名称");
                balloonTip1.ShowBalloon(comboBoxCompanyName);

                comboBoxCompanyName.Focus();
                comboBoxCompanyName.SelectAll();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxXTaxpayerRegistrationNumber.Text))
            {
                balloonTip1.SetBalloonText(textBoxXTaxpayerRegistrationNumber, "请输入纳税人识别号");
                balloonTip1.ShowBalloon(textBoxXTaxpayerRegistrationNumber);

                textBoxXTaxpayerRegistrationNumber.Focus();
                textBoxXTaxpayerRegistrationNumber.SelectAll();
                return;
            }

            if (GlobalManager.UserConfig.CompanyInfos.ToList()
                .Exists(p => p.CompanyName.Equals(comboBoxCompanyName.Text)))
            {
                balloonTip1.SetBalloonText(comboBoxCompanyName, "公司名称已存在");
                balloonTip1.ShowBalloon(comboBoxCompanyName);

                comboBoxCompanyName.Focus();
                comboBoxCompanyName.SelectAll();
                return;
            }

            if (GlobalManager.UserConfig.CompanyInfos.ToList().Exists(p =>
                p.TaxpayerRegistrationNumber.Equals(textBoxXTaxpayerRegistrationNumber.Text)))
            {
                balloonTip1.SetBalloonText(textBoxXTaxpayerRegistrationNumber, "纳税人识别号已存在");
                balloonTip1.ShowBalloon(textBoxXTaxpayerRegistrationNumber);

                textBoxXTaxpayerRegistrationNumber.Focus();
                textBoxXTaxpayerRegistrationNumber.SelectAll();
                return;
            }

            CompanyInfo companyInfo = new CompanyInfo
            {
                CompanyName = comboBoxCompanyName.Text,
                TaxpayerRegistrationNumber = textBoxXTaxpayerRegistrationNumber.Text
            };
            GlobalManager.UserConfig.CompanyInfos.Add(companyInfo);
            GlobalManager.UserConfig.SelectedCompanyInfo = companyInfo;
            GlobalManager.SaveUserConfig();
            comboBoxCompanyName.SelectedItem = companyInfo;

            balloonTip1.SetBalloonText(comboBoxCompanyName, "添加成功");
            balloonTip1.ShowBalloon(comboBoxCompanyName);

            comboBoxCompanyName.Focus();
            comboBoxCompanyName.SelectAll();
        }

        private void buttonItemDeleteCompanyInfo_Click(object sender, EventArgs e)
        {
            //            CompanyInfo companyInfo = (CompanyInfo)comboBoxCompanyName.SelectedItem;
            CompanyInfo companyInfo =
                GlobalManager.UserConfig.CompanyInfos.ToList()
                    .Find(p => p.CompanyName.Equals(comboBoxCompanyName.Text));

            GlobalManager.SaveUserConfig();
            if (companyInfo == null)
            {
                balloonTip1.SetBalloonText(comboBoxCompanyName, "请选择要删除的公司名称");
                balloonTip1.ShowBalloon(comboBoxCompanyName);

                return;
            }

            GlobalManager.UserConfig.CompanyInfos.Remove(companyInfo);
            GlobalManager.SaveUserConfig();
            if (comboBoxCompanyName.SelectedItem != null)
            {
                comboBoxCompanyName.SelectedItem = GlobalManager.UserConfig.SelectedCompanyInfo;
            }
            //            else
            //            {
            //                comboBoxCompanyName.SelectedIndex = 0;
            //            }

            balloonTip1.SetBalloonText(comboBoxCompanyName, "删除成功");
            balloonTip1.ShowBalloon(comboBoxCompanyName);

            comboBoxCompanyName.Focus();
        }

        private void picVerificationImage_Click(object sender, EventArgs e)
        {
            picVerificationImage.Image = picVerificationImage.Image = GZXNetworking.GetValidateImage();
            //            textBoxXIdentifyCode.Text = GZXNetworking.ORC(picVerificationImage.Image);
        }

        private void buttonItemSearchElectronicInvoice_Click(object sender, EventArgs e)
        {
            //Process.Start(GlobalManager.MainDownloadPath +
            //              GlobalManager.UserConfig.SelectedCompanyInfo.CompanyName);
            //return;

            //new Thread(() =>
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //        if (GZXMessageBox.MessageBoxResult("失败，要想重试下载失败的发票，请点击【确定】，否则点击【取消】"))
            //        {
            //            //buttonItemRetryFailure_Click(null, null);
            //        }
            //    }));

            //}).Start();
            //return;

            /*   balloonTip1.SetBalloonText(textBoxX1CardNum, "下载发票成功");
               balloonTip1.ShowBalloon(textBoxX1CardNum);
               return;*/

            if (!IsConfigurationComplete())
            {
                return;
            }

            string cardNum = textBoxX1CardNum.Text;
            string transactionDate = monthCalendarAdvTransaction.SelectedDate.ToString("yyyyMMdd");

            if (string.IsNullOrWhiteSpace(cardNum))
            {
                balloonTip1.SetBalloonText(textBoxX1CardNum, "请输入卡号");
                balloonTip1.ShowBalloon(textBoxX1CardNum);

                textBoxX1CardNum.Focus();
                textBoxX1CardNum.SelectAll();
                return;
            }

            if (string.IsNullOrWhiteSpace(cardNum))
            {
                balloonTip1.SetBalloonText(textBoxX1CardNum, "请输入验证码");
                balloonTip1.ShowBalloon(textBoxX1CardNum);

                textBoxX1CardNum.Focus();
                textBoxX1CardNum.SelectAll();
                return;
            }

            circularProgressSingleDownload.IsRunning = true;

            Thread thread = new Thread(() =>
            {
                ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                    transactionDate, false,
                    "正在识别验证码", null, "");
                this.Invoke(new Action(() => { GlobalManager.ElectronicInvoiceInfos.Add(electronicInvoiceInfo); }));

                //                string orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                //                if (string.IsNullOrWhiteSpace(orcResult))
                //                {
                //                    electronicInvoiceInfo.DownloadResult = "验证码识别余额已用完";
                //                    electronicInvoiceInfo.IsDownloaded = false;
                //                    electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                //                    VerificationCodeIdentificationRequiresRenewaFee();
                //                }
                //                else
                //                {
                //                    textBoxXIdentifyCode.BeginInvoke(new EventHandler(delegate
                //                    {
                //                        textBoxXIdentifyCode.Text = orcResult;
                //                    }));

                electronicInvoiceInfo.DownloadResult = "正在下载……";
                //                try
                //                {

                string downloadResult = "";
                bool ret = DownloadElectronicInvoice(ref downloadResult, cardNum, transactionDate,
                    cardNum + " " + transactionDate + ".pdf", electronicInvoiceInfo);

                this.Invoke(new Action(() =>
                {
                    balloonTip1.SetBalloonText(textBoxX1CardNum, downloadResult);
                    balloonTip1.ShowBalloon(textBoxX1CardNum);
                    textBoxX1CardNum.Focus();
                    textBoxX1CardNum.SelectAll();
                }));

                electronicInvoiceInfo.IsDownloaded = ret;
                electronicInvoiceInfo.DownloadResult = downloadResult;
                //                }

                this.Invoke(new Action(() => { circularProgressSingleDownload.IsRunning = false; }));

                picVerificationImage.Image = GZXNetworking.GetValidateImage();
            });
            thread.Start();
        }

        private void buttonItemStartBulkDownloadInvoice_Click(object sender, EventArgs e)
        {
            //            MessageBoxEx.Show("批量下载发票已完成");
            //            return;
            if (!IsConfigurationComplete())
            {
                return;
            }

            if (!Directory.Exists(textBoxXBrowseInvoicePhotoFolder.Text))
            {
                GZXMessageBox.Show("您选择的目录不存在");
                return;
            }

            //改代码会异常：路径中具有非法字符。
            //            var images = Directory.GetFiles(@"C:\\Users\\15519\\Desktop\\深圳通电子发票", "*.png|*.jpg", SearchOption.TopDirectoryOnly);
            //            Directory.GetFiles 返回指定目录的文件路径 SearchOption.AllDirectories 指定搜索当前目录及子目录
            var images = Directory.GetFiles(textBoxXBrowseInvoicePhotoFolder.Text, ".", SearchOption.TopDirectoryOnly)
                .Where(s => s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".jpg") ||
                            s.ToLower().EndsWith(".gif"));
            if (images.Count() == 0)
            {
                GZXMessageBox.Show("您选择的目录无照片文件");
                return;
            }

            _downloadedTotalAmount = 0;
            buttonItemRetryFailure.Enabled = false;
            _autoDownloadCount = images.Count();

            _autoDownloadStopwatch.Restart();
            timerAutoDownloadFile.Start();

            buttonItemStartBulkDownloadInvoice.Enabled = false;
            buttonItemStopBulkDownloadInvoice.Enabled = true;
            _startBatchDownload = true;
            _currentAutoHaveDownloadElectronicInvoiceInfos.Clear();

            _autoHaveDownloadedCount = 0;

            progressBarItemBatchDownload.Text = "";
            progressBarItemBatchDownload.Value = 0;
            progressBarItemBatchDownload.TextVisible = true;

            Thread thread = new Thread(() =>
            {
                //                try
                //                {
                //遍历string 型 images数组
                foreach (var path in images)
                {
                    if (!_startBatchDownload || _downloadedTotalAmount >= 1150)
                    {
                        _autoDownloadStopwatch.Stop();
                        timerAutoDownloadFile.Stop();

                        this.Invoke(new Action(() => { ShowBalloon(pictureBoxReceipt, "已停止下载"); }));

                        return;
                    }

                    Console.WriteLine(path);
                    string cardNum = null, transactionDate = null, originalTransactionDate = null;

                    long imageFileSize = new FileInfo(path).Length;
                    if (imageFileSize >= 1024 * 1024)
                    {
                        bool compressResult = GZXImageManager.CompressImage(path, path, 50, 999);
                    }

                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                    System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                    img.Dispose();

                    pictureBoxReceipt.Image = bmp;
                    textBoxXInvoiceRecognitionResult.Invoke(new Action(() =>
                    {
                        textBoxXInvoiceRecognitionResult.Text =
                            "正在识别……\r\n若识别不正确，可能原因如下：\r\n1、图片文字不是朝正面，请将图片旋转后再操作\r\n2、图片文字不清晰或缺失";
                    }));

                    ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                        transactionDate, false,
                        "正在识别照片", null, Path.GetFileName(path));
                    DownloadResult(electronicInvoiceInfo);

                    //string result = Youtu.generalocr(path);
                    string result = Youtu.handwritingocr(path);

                    JObject jObj = null;
                    try
                    {
                        jObj = JObject.Parse(result);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        //例如：jObj为UNKOWN; status = 413
                        electronicInvoiceInfo.DownloadResult = "识别失败：" + result;
                        continue;
                    }

                    var items = (from staff1 in jObj["items"].Children()
                        select (string)staff1["itemstring"]).ToList();

                    //                    if (!items.Exists(p => p.Length == 2 && (p.Contains("清湖") || p.Contains("清") || p.Contains("湖"))))
                    //                    {
                    //
                    //                        //ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                    //                        //    transactionDate, false,
                    //                        //    "批量下载暂只支持在清湖地铁站充值的小票", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                    //                        electronicInvoiceInfo.DownloadResult = "批量下载暂只支持在清湖地铁站充值的小票";
                    //                        electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                    //                        _autoHaveDownloadedCount++;
                    //
                    //                        //DownloadResult(electronicInvoiceInfo);
                    //                        continue;
                    //                    }

                    try
                    {
                        string item = items.FindAll(p =>
                            (p.Contains("卡号") || p.Contains("号") || p.Contains("卡")) && !p.Contains("编号"))[0];
                        string[] itemStrings = item.Split(new string[] { ":", "，", " " },
                            StringSplitOptions.RemoveEmptyEntries);
                        if (itemStrings.Length == 1)
                        {
                            cardNum = itemStrings[0].Replace("卡", "").Replace("号", "").Replace("卡号", "").Trim();
                        }
                        else if (itemStrings.Length == 2)
                        {
                            cardNum = itemStrings[1];
                        }
                        else
                        {
                            //                            throw new Exception("未识别到卡号");

                            this.Invoke(new EventHandler(delegate
                            {
                                IdentificationFailureForm identificationFailureForm =
                                    new IdentificationFailureForm("识别失败，请输入卡号", item);
                                identificationFailureForm.ShowDialog();

                                cardNum = identificationFailureForm.Value;
                            }));

                        }

                        item = items.Find(p => p.Contains("时间") || p.Contains("时") || p.Contains("间"));
                        //                        itemStrings = item.Split(new string[] { ":", "，", " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (item != null && item.Contains(DateTime.Now.Year.ToString()) && item.Contains("日"))
                        {
                            originalTransactionDate = item.Remove(0, item.IndexOf(DateTime.Now.Year.ToString()));
                            originalTransactionDate = originalTransactionDate.Remove(originalTransactionDate.IndexOf("日") + 1);

                            try
                            {
                                transactionDate = Convert.ToDateTime(originalTransactionDate).ToString("yyyyMMdd")
                                    .Replace(" ", "");
                            }
                            catch (Exception exception)
                            {
                                //                                Console.WriteLine(exception);
                                //                                throw; 
                                this.Invoke(new EventHandler(delegate
                                {
                                    IdentificationFailureForm identificationFailureForm =
                                        new IdentificationFailureForm("识别失败，请修改时间格式为年月日，如20190106", item);
                                    identificationFailureForm.ShowDialog();

                                    originalTransactionDate = item;
                                    transactionDate = identificationFailureForm.Value;
                                }));

                            }
                        }
                        else
                        {
                            //                            throw new Exception("未识别到时间");
                            this.Invoke(new EventHandler(delegate
                            {
                                IdentificationFailureForm identificationFailureForm =
                                    new IdentificationFailureForm("识别失败，请修改时间格式为年月日，如20190106", item);
                                identificationFailureForm.ShowDialog();

                                originalTransactionDate = item;
                                transactionDate = identificationFailureForm.Value;
                            }));

                        }

                        electronicInvoiceInfo.TransactionDate = transactionDate;
                        electronicInvoiceInfo.CardNum = cardNum;
                    }
                    catch (Exception exception)
                    {
                        electronicInvoiceInfo.TransactionDate = originalTransactionDate;
                        electronicInvoiceInfo.DownloadResult = "照片识别识别，请确认照片是否拍摄完整";
                        electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                        _autoHaveDownloadedCount++;

                        //DownloadResult(electronicInvoiceInfo);
                        continue;
                    }

                    try
                    {
                        /*
                                                //                        int cardNumIndex = items.IndexOf("卡编号") + 1;
                                                //                        cardNum = items[cardNumIndex].Replace(" ", "");
                                                string item = items.Find(p => p.Contains("交易号"));
                                                if (string.IsNullOrWhiteSpace(item))
                                                {
                                                    item = items.Find(p => p.Contains("交易"));
                                                    if (string.IsNullOrWhiteSpace(item))
                                                    {
                                                        item = items.Find(p => p.Contains("易号"));
                                                        if (string.IsNullOrWhiteSpace(item))
                                                        {
                                                            item = items.Find(p => p.Contains("交"));
                                                            if (string.IsNullOrWhiteSpace(item))
                                                            {
                                                                item = items.Find(p => p.Contains("易"));
                                                            }
                                                        }
                                                    }
                                                }
                                                int cardNumIndex = items.IndexOf(item);
                                                List<string> cardNumTempList = items.FindAll(a => a.Replace(" ", "").Length >= 8 && items.IndexOf(a) > cardNumIndex);
                                                cardNum = cardNumTempList[0].Replace(" ", "");

                                                //int intTemp = 0;
                                                //if (!int.TryParse(cardNum, out intTemp)) //判断是否可以转换为整型
                                                //{
                                                //    cardNumIndex = items.IndexOf("卡编号") - 1;
                                                //    cardNum = items[cardNumIndex].Replace(" ", "");
                                                //}
                                                //                        List<string> cardNumTempList = items.FindAll(a => a.Replace(" ", "").Length == 9);
                                                //                        if (cardNumTempList.Count == 1)
                                                //                        {
                                                //                            cardNum = cardNumTempList[0];
                                                //                        }
                                                //                        if (cardNumTempList.Count > 1)
                                                //                        {
                                                //                            cardNum = cardNumTempList[1];
                                                //                        }
                                                //                        transactionDate = items[items.IndexOf("清湖") + 1].Substring(0, 8);
                                                transactionDate = items.Find(a => a.Contains(":") || a.Contains("/") || a.Contains(";")).Replace(":;", ":")
                                                    .Replace(";", ":").Trim();
                                                originalTransactionDate = transactionDate;
                                                string date = transactionDate.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                                                if (date.Length == 8)
                                                {
                                                    char[] c = date.ToCharArray();
                                                    c[2] = '/';
                                                    c[5] = '/';
                                                    int i = 0;
                                                    foreach (var VARIABLE in c)
                                                    {
                                                        if (Char.IsLetter(VARIABLE))
                                                        {
                                                            c[i] = '0';
                                                        }
                                                        i++;
                                                    }

                                                    transactionDate = new string(c);

                                                    if (!transactionDate.Contains(" "))
                                                    {
                                                        transactionDate = transactionDate.Insert(8, " ");
                                                    }
                                                    transactionDate = Convert
                                                        .ToDateTime(transactionDate.Insert(transactionDate.LastIndexOf('/') + 1, "20"))
                                                        .ToString("yyyyMMdd").Replace(" ", "");
                                                }
                                                else
                                                {
                                                    string[] dates = date.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                                                    string temp = null;
                                                    if (dates.Length == 1)
                                                    {
                                                        string year = DateTime.Now.Year.ToString().Substring(2, 2);
                                                        string monthAndDay = dates[0].Substring(0, dates[0].Length - 2);
                                                        if (monthAndDay.Length >= 5)
                                                        {
                                                            temp += dates[0].Substring(0, 2);
                                                            temp += "/";
                                                            string day = dates[0].Substring(3, 2);
                                                            if (Convert.ToInt32(day) > 31)
                                                            {
                                                                temp += dates[0].Substring(2, 2);
                                                            }
                                                            else
                                                            {
                                                                temp += day;
                                                            }
                                                        }
                                                        else if (monthAndDay.Length < 5)
                                                        {
                                                            temp += dates[0].Substring(0, 2);
                                                            temp += "/";
                                                            temp += dates[0].Substring(2, 2);
                                                        }

                                                        temp += "/";
                                                        temp += year;
                                                        temp += "/";
                                                    }
                                                    else
                                                    {
                                                        foreach (var s in dates)
                                                        {
                                                            if (s.Length <= 2)
                                                            {
                                                                temp += s;
                                                            }
                                                            else if (s.Length > 2 && s.Length <= 4)
                                                            {
                                                                temp += s.Substring(0, 2);
                                                                temp += "/";
                                                                temp += s.Substring(2, 2);
                                                            }
                                                            else
                                                            {
                                                                temp += s.Substring(0, 2);
                                                            }
                                                            temp += "/";
                                                        }
                                                    }

                                                    transactionDate = temp.Remove(temp.Length - 1);
                                                    transactionDate = Convert
                                                        .ToDateTime(transactionDate.Insert(transactionDate.LastIndexOf('/') + 1, "20"))
                                                        .ToString("yyyyMMdd").Replace(" ", "");
                                                }

                                                electronicInvoiceInfo.TransactionDate = transactionDate;
                                                electronicInvoiceInfo.CardNum = cardNum;
                                                string transactionNumber = items[items.IndexOf("交易号") + 1];*/
                    }
                    catch (Exception exception)
                    {
                        //ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                        //        transactionDate, false,
                        //        "照片识别识别，请确认照片是否拍摄完整", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                        electronicInvoiceInfo.TransactionDate = originalTransactionDate;
                        electronicInvoiceInfo.DownloadResult = "照片识别识别，请确认照片是否拍摄完整";
                        electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                        _autoHaveDownloadedCount++;

                        //DownloadResult(electronicInvoiceInfo);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(cardNum) || string.IsNullOrWhiteSpace(transactionDate))
                    {
                        //ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                        //    transactionDate, false,
                        //    "识别不到卡号和交易日期", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                        electronicInvoiceInfo.DownloadResult = "识别不到卡号和交易日期";
                        electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                        _autoHaveDownloadedCount++;

                        //DownloadResult(electronicInvoiceInfo);
                        continue;
                    }

                    StringBuilder resultStringBuilder = new StringBuilder();
                    try
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            resultStringBuilder.AppendLine(items[j]);
                        }

                        resultStringBuilder.AppendLine();

                        for (int i = 3; i < 13; i++)
                        {
                            if (i % 2 == 0)
                            {
                                continue;
                            }

                            if (i >= 3 && i <= 12)
                            {
                                resultStringBuilder.AppendLine(items[i] + " : " + items[i + 1]);
                            }
                        }

                        resultStringBuilder.AppendLine();

                        resultStringBuilder.AppendLine(items[13]);
                        for (int i = 14; i < 19; i++)
                        {
                            if (i % 2 != 0)
                            {
                                continue;
                            }

                            resultStringBuilder.AppendLine(items[i] + " : " + items[i + 1]);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    textBoxXInvoiceRecognitionResult.Invoke(new Action(() =>
                    {
                        textBoxXInvoiceRecognitionResult.Text = resultStringBuilder.ToString();
                    }));

                    string downloadFileName = cardNum + "_" + Path.GetFileName(path) + ".pdf";
                    if (File.Exists(GlobalManager.DownloadPath + downloadFileName))
                    {
                        if (this.IsHandleCreated)
                        {
                            this.Invoke(new Action(() =>
                            {
                                //                                    progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / images.Count();

                                //ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                                //    transactionDate, true,
                                //    "该文件已经下载", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));

                                electronicInvoiceInfo.IsDownloaded = true;
                                electronicInvoiceInfo.DownloadResult = "该文件已下载";
                                electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                                _autoHaveDownloadedCount++;
                                UpdateProgressBarItemBatchDownload();

                                //DownloadResult(electronicInvoiceInfo);
                            }));
                        }

                        continue;
                    }

                    electronicInvoiceInfo.DownloadResult = "正在识别验证码";
                    picVerificationImage.Image = GZXNetworking.GetValidateImage();
                    //                    string orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                    //                    if (string.IsNullOrWhiteSpace(orcResult))
                    //                    {
                    //                        electronicInvoiceInfo.DownloadResult = "验证码识别余额已用完";
                    //                        electronicInvoiceInfo.IsDownloaded = false;
                    //                        electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                    //                        VerificationCodeIdentificationRequiresRenewaFee();
                    //                        StopOperationAfterDownloading();
                    //                        return;
                    //                    }


                    this.Invoke(new EventHandler(delegate
                    {
                        InputVerificationCodeForm inputVerificationCodeForm =
                            new InputVerificationCodeForm(picVerificationImage.Image);
                        inputVerificationCodeForm.ShowDialog();
                        textBoxXIdentifyCode.Text = inputVerificationCodeForm.Value;
                    }));

                    //                    textBoxXIdentifyCode.BeginInvoke(new EventHandler(delegate
                    //                    {
                    //                        textBoxXIdentifyCode.Text = inputVerificationCodeForm.Value;
                    //                    }));

                    //ElectronicInvoiceInfo currentElectronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                    //    transactionDate, false,
                    //    "正在下载……", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                    //DownloadResult(currentElectronicInvoiceInfo);
                    electronicInvoiceInfo.DownloadResult = "正在下载……";


                    //                    _currentAutoHaveDownloadElectronicInvoiceInfos.Add(currentElectronicInvoiceInfo);
                    //                    if (zxDataGridViewXDownloadResult.IsHandleCreated)
                    //                    {
                    //                        zxDataGridViewXDownloadResult.Invoke(new Action(() =>
                    //                        {
                    //                            GlobalManager.ElectronicInvoiceInfos.Add(currentElectronicInvoiceInfo);
                    //                        }));
                    //                    }

                    bool ret = false;
                    string downloadResult = "";
                    decimal rechargeAmount = 0;
                    ret = DownloadElectronicInvoice(ref downloadResult, cardNum, transactionDate, downloadFileName,
                        electronicInvoiceInfo);

                    electronicInvoiceInfo.DownloadResult = downloadResult;
                    electronicInvoiceInfo.IsDownloaded = ret;
                    electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                    _autoHaveDownloadedCount++;

                    if (this.IsHandleCreated)
                    {
                        progressBarItemBatchDownload.Invoke(new Action(() =>
                        {
                            UpdateProgressBarItemBatchDownload();
                            //                            progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / images.Count();
                        }));
                    }
                }

                StopOperationAfterDownloading();

                int failureCount = _currentAutoHaveDownloadElectronicInvoiceInfos.FindAll(p => p.IsDownloaded == false)
                    .Count;
                if (failureCount > 0)
                {
                    this.Invoke(new Action(() =>
                    {
                        buttonItemRetryFailure.Enabled = true;
                        if (GZXMessageBox.MessageBoxResult(failureCount + "张发票下载失败，要想重试下载失败的发票，请点击【确定】，否则点击【取消】"))
                        {
                            buttonItemRetryFailure_Click(null, null);
                        }
                    }));
                }

                //                }
                //                catch (Exception exception)
                //                {
                //                }
            });
            thread.Start();
        }

        private void textBoxXBrowseInvoicePhotoFolder_ButtonCustomClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxXBrowseInvoicePhotoFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }


        private void buttonItemRetryFailure_Click(object sender, EventArgs e)
        {
            //zxDataGridViewXDownloadResult.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //zxDataGridViewXDownloadResult.MultiSelect = true;
            ////_currentAutoHaveDownloadElectronicInvoiceInfos.ForEach(p =>
            ////{
            ////    Console.WriteLine(p.CardNum);
            ////});
            //return;
            List<ElectronicInvoiceInfo> failureElectronicInvoiceInfos =
                _currentAutoHaveDownloadElectronicInvoiceInfos.FindAll(p => p.IsDownloaded == false);
            buttonItemRetryFailure.Enabled = false;
            _autoDownloadCount = failureElectronicInvoiceInfos
                .Count;

            _autoDownloadStopwatch.Restart();
            timerAutoDownloadFile.Start();

            buttonItemStartBulkDownloadInvoice.Enabled = false;
            buttonItemStopBulkDownloadInvoice.Enabled = true;
            _startBatchDownload = true;
            _currentAutoHaveDownloadElectronicInvoiceInfos.Clear();

            _autoHaveDownloadedCount = 0;

            this.Invoke(new Action(() =>
            {
                progressBarItemBatchDownload.Text = "";
                progressBarItemBatchDownload.Value = 0;
                progressBarItemBatchDownload.TextVisible = true;
            }));

            Thread thread = new Thread(() =>
            {
                //                try
                //                {
                //遍历string 型 images数组
                foreach (var failureElectronicInvoiceInfo in failureElectronicInvoiceInfos)
                {
                    if (!_startBatchDownload)
                    {
                        _autoDownloadStopwatch.Stop();
                        timerAutoDownloadFile.Stop();

                        this.Invoke(new Action(() => { ShowBalloon(pictureBoxReceipt, "已停止下载"); }));

                        return;
                    }


                    //                    Console.WriteLine(path);
                    string cardNum = null, transactionDate = null;
                    cardNum = failureElectronicInvoiceInfo.CardNum;
                    transactionDate = failureElectronicInvoiceInfo.TransactionDate;


                    ElectronicInvoiceInfo currentElectronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                        transactionDate, false,
                        "正在识别验证码", null, failureElectronicInvoiceInfo.ImageFileName);
                    DownloadResult(currentElectronicInvoiceInfo);

                    /*picVerificationImage.Image = GZXNetworking.GetValidateImage();
                    string orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                    if (string.IsNullOrWhiteSpace(orcResult))
                    {
                        currentElectronicInvoiceInfo.DownloadResult = "验证码识别余额已用完";
                        currentElectronicInvoiceInfo.IsDownloaded = false;
                        currentElectronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                        VerificationCodeIdentificationRequiresRenewaFee();
                        StopOperationAfterDownloading();
                        return;
                    }

                    textBoxXIdentifyCode.BeginInvoke(new EventHandler(delegate
                    {
                        textBoxXIdentifyCode.Text = orcResult;
                    }));*/

                    this.Invoke(new EventHandler(delegate
                    {
                        InputVerificationCodeForm inputVerificationCodeForm =
                            new InputVerificationCodeForm(picVerificationImage.Image);
                        inputVerificationCodeForm.ShowDialog();
                        textBoxXIdentifyCode.Text = inputVerificationCodeForm.Value;
                    }));

                    //                    _currentAutoHaveDownloadElectronicInvoiceInfos.Add(currentElectronicInvoiceInfo);
                    //                    if (zxDataGridViewXDownloadResult.IsHandleCreated)
                    //                    {
                    //                        zxDataGridViewXDownloadResult.Invoke(new Action(() =>
                    //                        {
                    //                            GlobalManager.ElectronicInvoiceInfos.Add(currentElectronicInvoiceInfo);
                    //                        }));
                    //                    }
                    currentElectronicInvoiceInfo.DownloadResult = "正在下载……";
                    string downloadFileName = cardNum + "_" + failureElectronicInvoiceInfo.ImageFileName + ".pdf";

                    bool ret = false;
                    string downloadResult = "";
                    decimal rechargeAmount = 0;
                    ret = DownloadElectronicInvoice(ref downloadResult, cardNum, transactionDate, downloadFileName,
                        currentElectronicInvoiceInfo);

                    currentElectronicInvoiceInfo.DownloadResult = downloadResult;
                    currentElectronicInvoiceInfo.IsDownloaded = ret;
                    currentElectronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                    _autoHaveDownloadedCount++;

                    if (this.IsHandleCreated)
                    {
                        progressBarItemBatchDownload.Invoke(new Action(() =>
                        {
                            UpdateProgressBarItemBatchDownload();
                            //                            progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / images.Count();
                        }));
                    }
                }

                foreach (var electronicInvoiceInfo in GlobalManager.ElectronicInvoiceInfos)
                {
                    Console.WriteLine(string.Format("卡号：{0}，交易日期：{1}，是否下载：{2}，下载结果：{3}，完成时间：{4}，文件名：{5}",
                        electronicInvoiceInfo.CardNum, electronicInvoiceInfo.TransactionDate,
                        electronicInvoiceInfo.IsDownloaded, electronicInvoiceInfo.DownloadResult,
                        electronicInvoiceInfo.CompleteTime, electronicInvoiceInfo.ImageFileName));
                }

                StopOperationAfterDownloading();

                int failureCount = _currentAutoHaveDownloadElectronicInvoiceInfos.FindAll(p => p.IsDownloaded == false)
                    .Count;
                if (failureCount > 0)
                {
                    buttonItemRetryFailure.Enabled = true;

                    this.Invoke(new Action(() =>
                    {
                        if (GZXMessageBox.MessageBoxResult(failureCount + "张发票失败，要想重试下载失败的发票，请点击【确定】，否则点击【取消】"))
                        {
                            buttonItemRetryFailure_Click(null, null);
                        }
                    }));
                }

                //                }
                //                catch (Exception exception)
                //                {
                //                }
            });
            thread.Start();
        }

        private void StopOperationAfterDownloading()
        {
            this.BeginInvoke(new Action(() =>
            {
                buttonItemStartBulkDownloadInvoice.Enabled = true;
                buttonItemStopBulkDownloadInvoice.Enabled = false;

                //                        MessageBoxEx.Show("批量下载发票已完成");
            }));

            _autoDownloadStopwatch.Stop();
            timerAutoDownloadFile.Stop();
        }

        private void buttonItemStopBulkDownloadInvoice_Click(object sender, EventArgs e)
        {
            ShowBalloon(pictureBoxReceipt, "当前正在下载的发票下载完成后才会停止下载");
            buttonItemStartBulkDownloadInvoice.Enabled = true;
            buttonItemStopBulkDownloadInvoice.Enabled = false;
            _startBatchDownload = false;
        }

        #region 将文件转换成流

        //public byte[] SetImageToByteArray(string fileName, ref string fileSize)
        /// <summary>
        /// 将文件转换成流
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        /// <returns></returns>
        public byte[] SetImageToByteArray(string fileName)
        {
            byte[] image = null;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                FileInfo fileInfo = new FileInfo(fileName);
                //fileSize = Convert.ToDecimal(fileInfo.Length / 1024).ToString("f2") + " K";
                int streamLength = (int)fs.Length;
                image = new byte[streamLength];
                fs.Read(image, 0, streamLength);
                fs.Close();
                return image;
            }
            catch
            {
                return image;
            }
        }

        #endregion

        #region 将byte转换成MemoryStream类型

        /// <summary>
        /// 将byte转换成MemoryStream类型
        /// </summary>
        /// <param name="mybyte">byte[]变量</param>
        /// <returns></returns>
        public MemoryStream ByteToStream(byte[] mybyte)
        {
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            return mymemorystream;
        }

        #endregion

        #region 将byte转换成Image文件

        /// <summary>
        /// 将byte转换成Image文件
        /// </summary>
        /// <param name="mybyte">byte[]变量</param>
        /// <returns></returns>
        public Image SetByteToImage(byte[] mybyte)
        {
            Image image;
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            image = Image.FromStream(mymemorystream);
            return image;
        }

        #endregion

        #endregion

        private void metroTabItem3_Click(object sender, EventArgs e)
        {
        }

        #region TextBox控件事件

        private void textBoxXTaxpayerRegistrationNumber_TextChanged(object sender, EventArgs e)
        {
            //            GlobalManager.UserConfig.TaxpayerRegistrationNumber = textBoxXTaxpayerRegistrationNumber.Text;
            //            GlobalManager.SaveUserConfig();
        }

        #endregion

        #region CheckBox ComboBox控件事件

        private void checkBoxXSkipDownloadedFile_CheckedChanged(object sender, EventArgs e)
        {
            GlobalManager.UserConfig.IsSkipDownloadFile = checkBoxXSkipDownloadedFile.Checked;
            GlobalManager.SaveUserConfig();
        }

        private void ComboBoxCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalManager.UserConfig.SelectedCompanyInfo = (CompanyInfo)comboBoxCompanyName.SelectedItem;
            GlobalManager.SaveUserConfig();

            if (GlobalManager.UserConfig.SelectedCompanyInfo == null)
            {
                textBoxXTaxpayerRegistrationNumber.Text = "";
                return;
            }

            textBoxXTaxpayerRegistrationNumber.Text =
                GlobalManager.UserConfig.SelectedCompanyInfo.TaxpayerRegistrationNumber;
        }

        #endregion

        private void timerAutoDownloadFile_Tick(object sender, EventArgs e)
        {
            UpdateprogressBarItemBatchDownloadText();
        }

        private void buttonItemAbout_Click(object sender, EventArgs e)
        {
            MessageBoxEx.Show("<font size=\"15\" color=\"red\">人生苦短，请给作者一个★Star吧<br/>在打开的页面点击右上角的Star即可，不要钱的</font>");

            Process.Start("https://github.com/GanZhiXiong/SZTElectronicInvoice");
        }

        #region 右键菜单事件

        private void buttonItemCopyCardNum_Click(object sender, EventArgs e)
        {
            ElectronicInvoiceInfo electronicInvoiceInfo =
                (ElectronicInvoiceInfo)zxDataGridViewXDownloadResult.CurrentRow.DataBoundItem;
            Clipboard.SetDataObject(electronicInvoiceInfo.CardNum);
        }

        private void buttonItemzxDataGridViewXDownloadResultClear_Click(object sender, EventArgs e)
        {
            GlobalManager.ElectronicInvoiceInfos.Clear();
        }

        #endregion

        private void deleteDownloadSuccessButtonItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<ElectronicInvoiceInfo> failureElectronicInvoiceInfos1 =
                    GlobalManager.ElectronicInvoiceInfos.ToList().FindAll(p => p.IsDownloaded == true);
                failureElectronicInvoiceInfos1.ForEach(p =>
                {
                    GlobalManager.ElectronicInvoiceInfos.Remove(p);

                    string path = Path.Combine(textBoxXBrowseInvoicePhotoFolder.Text, p.ImageFileName);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                });

                //foreach (var electronicInvoiceInfo in failureElectronicInvoiceInfos1)
                //{

                //}
                //GlobalManager.ElectronicInvoiceInfos.ToList().RemoveAll(p => p.IsDownloaded == true);

                GZXMessageBox.Show("删除完成");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                GZXMessageBox.Show(exception.Message);
            }
        }

        private void textBoxXBrowseInvoicePhotoFolder_TextChanged(object sender, EventArgs e)
        {
            GlobalManager.UserConfig.BrowseInvoicePhotoFolder = textBoxXBrowseInvoicePhotoFolder.Text;
            GlobalManager.SaveUserConfig();
        }

        private void buttonItemOpenDownloadDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(GlobalManager.MainDownloadPath +
                          GlobalManager.UserConfig.SelectedCompanyInfo.CompanyName);
        }

        private void ButtonX1_Click(object sender, EventArgs e)
        {
            IdentificationFailureForm identificationFailureForm =
                new IdentificationFailureForm("识别失败，请修改为正确的时间格式（年月日，如20190126）", "2019年10月22日");
            identificationFailureForm.ShowDialog();
        }

        private void ButtonX2_Click(object sender, EventArgs e)
        {
            InputVerificationCodeForm inputVerificationCodeForm =
                new InputVerificationCodeForm(picVerificationImage.Image);
            inputVerificationCodeForm.ShowDialog();
        }
    }
}