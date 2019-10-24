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

        #region ���幹�캯��

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
                          @"������Ҫ��HTML ҳ���ж�ȡ���ģ������ڵĽ��������ȡ >����<�м���ַ���˭�и��õĽ������������˭֪�� ����������ж� < > ��val=""100"">100Ԫ</td>",
                          @"(?<=^|>)((?![<>]).)+(?=Ԫ<|$)");
                  foreach (Match m in mc)
                  {
                      Console.WriteLine(m.Value);
                  }*/

            #endregion

            InitializeComponent();

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Text = "����ͨ���ӷ�Ʊ";
            this.MaximizeBox = false;
            //            CheckForIllegalCrossThreadCalls = false;

            metroShell1.TitleText = "<font size=\"11\" face=\"΢���ź�\">����ͨ���ӷ�Ʊ</font>";

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

            #region �Զ�

            pictureBoxReceipt.Image = null;
            textBoxXBrowseInvoicePhotoFolder.Clear();
            textBoxXInvoiceRecognitionResult.Clear();

            progressBarItemBatchDownload.Text = "";
            progressBarItemBatchDownload.Width = metroStatusBar1.Width - 3;
            //            progressBarItemBatchDownload.Value += 50;
            buttonItemRetryFailure.Enabled = buttonItemStopBulkDownloadInvoice.Enabled = false;

            #endregion

            InitDataGridView();

            #region ��Ѷ��ʶ��

            // ����Ϊ���Լ�����Կ��
            string appid = "10126985";
            string secretId = "AKIDWxfJt7oixS0PwOozqeLvV2fBYkolMimh";
            string secretKey = "5FLOYa8lK0XxPkvzwkT2cDe5MSOEyZ0I";
            string userid = "1551935335";

            Conf.Instance().setAppInfo(appid, secretId, secretKey, userid, Conf.Instance().YOUTU_END_POINT);

            #endregion
        }

        #endregion

        #region ��д

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            metroShell1.SelectedTab = metroTabItem1;

            //textBoxXBrowseInvoicePhotoFolder.Text = @"D:\Program Files\QQRecord\1551935335\FileRecv\MobileFile";

            textBoxX1CardNum.TabIndex = 0;
            textBoxX1CardNum.Focus();

            this.pictureBox1.BackColor = ColorTranslator.FromHtml("#007acc");
            picVerificationImage.Image = GZXNetworking.GetValidateImage();

            //            string s= Path.GetDirectoryName(@"C:\Users\15519\Desktop\δ���سɹ��ķ�Ʊ\IMG_1434.JPG");
            //            string s = Path.Combine(Path.GetDirectoryName(@"C: \Users\15519\Desktop\δ���سɹ��ķ�Ʊ"),
            //                "123" + DateTime.Now.ToString(" HHmmss.fff") + ".JPG");
            //            return;

            //buttonItemStartBulkDownloadInvoice.Enabled = false;
            //buttonItemStartDownloadInvoice.Enabled = false;

            #region ��ȡ�����ļ�

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
            //                "<font size=\"15\" color=\"red\">��֤��ʶ�������߸��ѹ���������Զ�ʶ����֤��ƽ̨�ӿ�<br/>��ƽ̨ʶ����֤���ǰ���ʶ�����������ã�������֤��ʶ����������꣬��Ҫ���Ѳ��ܼ���ʹ��<br/>�������ʹ�ã����ڴ򿪵�ҳ�棬�ҵ������������ߡ���΢�Ż�֧���������߾����㣬�����յ�Ǯ�󣬻Ὣ���ֵ����֤��ʶ��ƽ̨<br/>������������Ǿ��������һ����Star��<br/>�ڴ򿪵�ҳ�������Ͻǵ�Star���ɣ���ҪǮ��</font>");
            //            Process.Start("https://github.com/GanZhiXiong/SZTElectronicInvoice");
            //buttonItemStartDownloadInvoice.Enabled = true;
            return;
            Thread thread = new Thread(() =>
            {
                circularProgressSingleDownload.IsRunning = true;
                circularProgressSingleDownload.ProgressText = "��ȴ�";
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
                "<font size=\"15\" color=\"red\">��֤��ʶ�������߸��ѹ���������Զ�ʶ����֤��ƽ̨�ӿ�<br/>��ƽ̨ʶ����֤���ǰ���ʶ�����������ã�������֤��ʶ����������꣬��Ҫ���Ѳ��ܼ���ʹ��<br/>�������ʹ�ã����ڴ򿪵�ҳ�棬�ҵ������������ߡ���΢�Ż�֧���������߾����㣬�����յ�Ǯ�󣬻Ὣ���ֵ����֤��ʶ��ƽ̨<br/>������������Ǿ��������һ����Star��<br/>�ڴ򿪵�ҳ�������Ͻǵġ�Star���ɣ���ҪǮ��</font>");
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
                balloonTip1.SetBalloonText(comboBoxCompanyName, "�����ù�˾����");
                balloonTip1.ShowBalloon(comboBoxCompanyName);

                comboBoxCompanyName.Focus();
                comboBoxCompanyName.SelectAll();
                return false;
            }

            if (string.IsNullOrWhiteSpace(GlobalManager.UserConfig.SelectedCompanyInfo.TaxpayerRegistrationNumber))
            {
                //                metroTabPanel3.MetroTabItem= metroToolbar3;

                metroShell1.SelectedTab = metroTabItem3;
                balloonTip1.SetBalloonText(textBoxXTaxpayerRegistrationNumber, "��������˰��ʶ���");
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
                .FindAll(a => a.IsDownloaded == true && a.DownloadResult == "���ط�Ʊ�ɹ�").Sum(a => a.RechargeAmount);
            //if (downloadedTotalAmount>=3000)
            //{

            //}
            //            progressBarItemBatchDownload.Text = "��ɣ�" + (progress).ToString("0.##") + "%" + "   ��ʱ��" + (int)_autoDownloadStopwatch.Elapsed.TotalSeconds + "��";
            progressBarItemBatchDownload.Text = "�ѳɹ�/�����/�ܹ�(��):" +
                                                _currentAutoHaveDownloadElectronicInvoiceInfos
                                                    .FindAll(a => a.IsDownloaded).Count + "/" +
                                                _autoHaveDownloadedCount + "/" + _autoDownloadCount + "   �ϼ�(Ԫ):" +
                                                _downloadedTotalAmount +
                                                "   ���:" + (progress).ToString("0.##") + "%" + "   ��ʱ:" +
                                                _autoDownloadStopwatch.Elapsed.Minutes + "��" +
                                                _autoDownloadStopwatch.Elapsed.Seconds + "��";
        }

        private void InitDataGridView()
        {
            #region DataGridViewX��������

            zxDataGridViewXDownloadResult.DataSource = GlobalManager.ElectronicInvoiceInfos;
            zxDataGridViewXDownloadResult.MultiSelect = false;
            zxDataGridViewXDownloadResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            zxDataGridViewXDownloadResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            zxDataGridViewXDownloadResult.AddColumn("ImageFileName", "ͼƬ����");
            zxDataGridViewXDownloadResult.AddColumn("CardNum", "����");
            zxDataGridViewXDownloadResult.AddColumn("TransactionDate", "��������");
            zxDataGridViewXDownloadResult.AddColumn("RechargeAmount", "���");
            zxDataGridViewXDownloadResult.AddColumn("IsDownloadedString", "�Ƿ�ɹ�");

            zxDataGridViewXDownloadResult.AddColumn("CompleteTime", "���ʱ��");
            zxDataGridViewXDownloadResult.AddColumn("DownloadResult", "���ؽ��");

            #endregion

            #region DataGridViewX �¼� 

            zxDataGridViewXDownloadResult.RowsAdded += ZxDataGridViewXDownloadResult_RowsAdded;
            zxDataGridViewXDownloadResult.CellMouseClick += ZxDataGridViewXDownloadResult_CellMouseClick;
            zxDataGridViewXDownloadResult.CellEndEdit += ZxDataGridViewXDownloadResult_CellEndEdit;
            //            TaskGridView.MouseClick += TaskGridView_MouseClick;
            //            TaskGridView.RowPostPaint += TaskGridView_RowPostPaint;
            //            TaskGridView.RowStateChanged += TaskGridView_RowStateChanged;

            #endregion
        }

        #region DataGridViewX �¼� 
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

                    downloadResult = "��1����ֵ��¼�����ڻ����ѿ�Ʊ";
                    return false;
                }

                string fplistUrl = string.Format("https://www.shenzhentong.com/service/fplist_101007009_{0}_{1}.html",
                    cardNum, transactionDate);
                string ret1 = GZXNetworking.GetRequest(fplistUrl);

                MatchCollection mc = Regex.Matches(ret1, @"(?<=^|>)((?![<>]).)+(?=Ԫ<|$)");
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

                if (ret1.Contains("��Ʊ��"))
                {
                    //picVerificationImage.Image = GZXNetworking.GetValidateImage();
                    //orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                    //textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

                    downloadResult = "��Ʊ�У����Ժ�����";
                    return false;
                }

                if (ret1.Contains(".pdf"))
                {
                    if (GlobalManager.UserConfig.IsSkipDownloadFile)
                    {
                        downloadResult = "����";
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
                            downloadResult = "���ط�Ʊ�ɹ�";
                            return true;
                        }
                        else
                        {
                            downloadResult = "���ط�Ʊʧ��";
                            return false;
                        }
                    }
                }

                string sj = StringOperation.GetMiddleText(ret1, @"sj=""", @""">");

                if (string.IsNullOrWhiteSpace(sj))
                {
                    downloadResult = StringOperation.GetMiddleText(ret1, @"<p><br>", @"<br><br></p>")
                        .Replace("<br><br>", "");
                    //                downloadResult = "sjΪ��";
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
                //jfirmfpmc	����ϵͳ�Ƽ������ڣ����޹�˾
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
                //            paramStringBuilder.Append("jfirmfpmc=" + "�����л���������޹�˾");
                paramStringBuilder.Append("jfirmfpmc=" + GlobalManager.UserConfig.SelectedCompanyInfo.CompanyName);

                //            paramStringBuilder.Append("jfirmfpmc=" + "����ϵͳ�Ƽ������ڣ����޹�˾");
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

                    if (ret1.Contains("��Ʊ��"))
                    {
                        //picVerificationImage.Image = GZXNetworking.GetValidateImage();
                        //orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                        //textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

                        //                    balloonTip1.SetBalloonText(textBoxX1CardNum, "��Ʊ�У����Ժ�����");
                        //                    balloonTip1.ShowBalloon(textBoxX1CardNum);

                        textBoxX1CardNum.Focus();
                        textBoxX1CardNum.SelectAll();

                        downloadResult = "��Ʊ�У����Ժ�����";
                        return false;
                    }

                    if (ret1.Contains(".pdf"))
                    {
                        if (GlobalManager.UserConfig.IsSkipDownloadFile)
                        {
                            downloadResult = "����";
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
                                downloadResult = "���ط�Ʊ�ɹ�";
                                return true;
                            }
                            else
                            {
                                downloadResult = "���ط�Ʊʧ��";
                                return false;
                            }
                        }
                    }

                    downloadResult = "���ص�ַΪ��";
                    return false;
                }

                bool downloadRet = GZXNetworking.DownloadFile(downloadUrl, GlobalManager.DownloadPath,
                    downloadFileName, 3);
                //picVerificationImage.Image = GZXNetworking.GetValidateImage();
                //orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                //textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

                if (downloadRet)
                {
                    downloadResult = "���ط�Ʊ�ɹ�";
                    return true;
                }
                else
                {
                    downloadResult = "���ط�Ʊʧ��";
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

        #region MonthCalendarAdv�¼�

        private void MonthCalendarAdvTransaction_DateSelected(object sender, DateRangeEventArgs e)
        {
            labelXTransactionDate.Text = monthCalendarAdvTransaction.SelectedDate.ToString("yyyy-MM-dd");
        }

        private void MonthCalendarAdvTransaction_DateChanged(object sender, EventArgs e)
        {
        }

        #endregion

        #region �ؼ�����¼�

        private void buttonItemAddCompanyInfo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxCompanyName.Text))
            {
                balloonTip1.SetBalloonText(comboBoxCompanyName, "�����빫˾����");
                balloonTip1.ShowBalloon(comboBoxCompanyName);

                comboBoxCompanyName.Focus();
                comboBoxCompanyName.SelectAll();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxXTaxpayerRegistrationNumber.Text))
            {
                balloonTip1.SetBalloonText(textBoxXTaxpayerRegistrationNumber, "��������˰��ʶ���");
                balloonTip1.ShowBalloon(textBoxXTaxpayerRegistrationNumber);

                textBoxXTaxpayerRegistrationNumber.Focus();
                textBoxXTaxpayerRegistrationNumber.SelectAll();
                return;
            }

            if (GlobalManager.UserConfig.CompanyInfos.ToList()
                .Exists(p => p.CompanyName.Equals(comboBoxCompanyName.Text)))
            {
                balloonTip1.SetBalloonText(comboBoxCompanyName, "��˾�����Ѵ���");
                balloonTip1.ShowBalloon(comboBoxCompanyName);

                comboBoxCompanyName.Focus();
                comboBoxCompanyName.SelectAll();
                return;
            }

            if (GlobalManager.UserConfig.CompanyInfos.ToList().Exists(p =>
                p.TaxpayerRegistrationNumber.Equals(textBoxXTaxpayerRegistrationNumber.Text)))
            {
                balloonTip1.SetBalloonText(textBoxXTaxpayerRegistrationNumber, "��˰��ʶ����Ѵ���");
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

            balloonTip1.SetBalloonText(comboBoxCompanyName, "��ӳɹ�");
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
                balloonTip1.SetBalloonText(comboBoxCompanyName, "��ѡ��Ҫɾ���Ĺ�˾����");
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

            balloonTip1.SetBalloonText(comboBoxCompanyName, "ɾ���ɹ�");
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
            //        if (GZXMessageBox.MessageBoxResult("ʧ�ܣ�Ҫ����������ʧ�ܵķ�Ʊ��������ȷ��������������ȡ����"))
            //        {
            //            //buttonItemRetryFailure_Click(null, null);
            //        }
            //    }));

            //}).Start();
            //return;

            /*   balloonTip1.SetBalloonText(textBoxX1CardNum, "���ط�Ʊ�ɹ�");
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
                balloonTip1.SetBalloonText(textBoxX1CardNum, "�����뿨��");
                balloonTip1.ShowBalloon(textBoxX1CardNum);

                textBoxX1CardNum.Focus();
                textBoxX1CardNum.SelectAll();
                return;
            }

            if (string.IsNullOrWhiteSpace(cardNum))
            {
                balloonTip1.SetBalloonText(textBoxX1CardNum, "��������֤��");
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
                    "����ʶ����֤��", null, "");
                this.Invoke(new Action(() => { GlobalManager.ElectronicInvoiceInfos.Add(electronicInvoiceInfo); }));

                //                string orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                //                if (string.IsNullOrWhiteSpace(orcResult))
                //                {
                //                    electronicInvoiceInfo.DownloadResult = "��֤��ʶ�����������";
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

                electronicInvoiceInfo.DownloadResult = "�������ء���";
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
            //            MessageBoxEx.Show("�������ط�Ʊ�����");
            //            return;
            if (!IsConfigurationComplete())
            {
                return;
            }

            if (!Directory.Exists(textBoxXBrowseInvoicePhotoFolder.Text))
            {
                GZXMessageBox.Show("��ѡ���Ŀ¼������");
                return;
            }

            //�Ĵ�����쳣��·���о��зǷ��ַ���
            //            var images = Directory.GetFiles(@"C:\\Users\\15519\\Desktop\\����ͨ���ӷ�Ʊ", "*.png|*.jpg", SearchOption.TopDirectoryOnly);
            //            Directory.GetFiles ����ָ��Ŀ¼���ļ�·�� SearchOption.AllDirectories ָ��������ǰĿ¼����Ŀ¼
            var images = Directory.GetFiles(textBoxXBrowseInvoicePhotoFolder.Text, ".", SearchOption.TopDirectoryOnly)
                .Where(s => s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".jpg") ||
                            s.ToLower().EndsWith(".gif"));
            if (images.Count() == 0)
            {
                GZXMessageBox.Show("��ѡ���Ŀ¼����Ƭ�ļ�");
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
                //����string �� images����
                foreach (var path in images)
                {
                    if (!_startBatchDownload || _downloadedTotalAmount >= 1150)
                    {
                        _autoDownloadStopwatch.Stop();
                        timerAutoDownloadFile.Stop();

                        this.Invoke(new Action(() => { ShowBalloon(pictureBoxReceipt, "��ֹͣ����"); }));

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
                            "����ʶ�𡭡�\r\n��ʶ����ȷ������ԭ�����£�\r\n1��ͼƬ���ֲ��ǳ����棬�뽫ͼƬ��ת���ٲ���\r\n2��ͼƬ���ֲ�������ȱʧ";
                    }));

                    ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                        transactionDate, false,
                        "����ʶ����Ƭ", null, Path.GetFileName(path));
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
                        //���磺jObjΪUNKOWN; status = 413
                        electronicInvoiceInfo.DownloadResult = "ʶ��ʧ�ܣ�" + result;
                        continue;
                    }

                    var items = (from staff1 in jObj["items"].Children()
                        select (string)staff1["itemstring"]).ToList();

                    //                    if (!items.Exists(p => p.Length == 2 && (p.Contains("���") || p.Contains("��") || p.Contains("��"))))
                    //                    {
                    //
                    //                        //ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                    //                        //    transactionDate, false,
                    //                        //    "����������ֻ֧�����������վ��ֵ��СƱ", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                    //                        electronicInvoiceInfo.DownloadResult = "����������ֻ֧�����������վ��ֵ��СƱ";
                    //                        electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                    //                        _autoHaveDownloadedCount++;
                    //
                    //                        //DownloadResult(electronicInvoiceInfo);
                    //                        continue;
                    //                    }

                    try
                    {
                        string item = items.FindAll(p =>
                            (p.Contains("����") || p.Contains("��") || p.Contains("��")) && !p.Contains("���"))[0];
                        string[] itemStrings = item.Split(new string[] { ":", "��", " " },
                            StringSplitOptions.RemoveEmptyEntries);
                        if (itemStrings.Length == 1)
                        {
                            cardNum = itemStrings[0].Replace("��", "").Replace("��", "").Replace("����", "").Trim();
                        }
                        else if (itemStrings.Length == 2)
                        {
                            cardNum = itemStrings[1];
                        }
                        else
                        {
                            //                            throw new Exception("δʶ�𵽿���");

                            this.Invoke(new EventHandler(delegate
                            {
                                IdentificationFailureForm identificationFailureForm =
                                    new IdentificationFailureForm("ʶ��ʧ�ܣ������뿨��", item);
                                identificationFailureForm.ShowDialog();

                                cardNum = identificationFailureForm.Value;
                            }));

                        }

                        item = items.Find(p => p.Contains("ʱ��") || p.Contains("ʱ") || p.Contains("��"));
                        //                        itemStrings = item.Split(new string[] { ":", "��", " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (item != null && item.Contains(DateTime.Now.Year.ToString()) && item.Contains("��"))
                        {
                            originalTransactionDate = item.Remove(0, item.IndexOf(DateTime.Now.Year.ToString()));
                            originalTransactionDate = originalTransactionDate.Remove(originalTransactionDate.IndexOf("��") + 1);

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
                                        new IdentificationFailureForm("ʶ��ʧ�ܣ����޸�ʱ���ʽΪ�����գ���20190106", item);
                                    identificationFailureForm.ShowDialog();

                                    originalTransactionDate = item;
                                    transactionDate = identificationFailureForm.Value;
                                }));

                            }
                        }
                        else
                        {
                            //                            throw new Exception("δʶ��ʱ��");
                            this.Invoke(new EventHandler(delegate
                            {
                                IdentificationFailureForm identificationFailureForm =
                                    new IdentificationFailureForm("ʶ��ʧ�ܣ����޸�ʱ���ʽΪ�����գ���20190106", item);
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
                        electronicInvoiceInfo.DownloadResult = "��Ƭʶ��ʶ����ȷ����Ƭ�Ƿ���������";
                        electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                        _autoHaveDownloadedCount++;

                        //DownloadResult(electronicInvoiceInfo);
                        continue;
                    }

                    try
                    {
                        /*
                                                //                        int cardNumIndex = items.IndexOf("�����") + 1;
                                                //                        cardNum = items[cardNumIndex].Replace(" ", "");
                                                string item = items.Find(p => p.Contains("���׺�"));
                                                if (string.IsNullOrWhiteSpace(item))
                                                {
                                                    item = items.Find(p => p.Contains("����"));
                                                    if (string.IsNullOrWhiteSpace(item))
                                                    {
                                                        item = items.Find(p => p.Contains("�׺�"));
                                                        if (string.IsNullOrWhiteSpace(item))
                                                        {
                                                            item = items.Find(p => p.Contains("��"));
                                                            if (string.IsNullOrWhiteSpace(item))
                                                            {
                                                                item = items.Find(p => p.Contains("��"));
                                                            }
                                                        }
                                                    }
                                                }
                                                int cardNumIndex = items.IndexOf(item);
                                                List<string> cardNumTempList = items.FindAll(a => a.Replace(" ", "").Length >= 8 && items.IndexOf(a) > cardNumIndex);
                                                cardNum = cardNumTempList[0].Replace(" ", "");

                                                //int intTemp = 0;
                                                //if (!int.TryParse(cardNum, out intTemp)) //�ж��Ƿ����ת��Ϊ����
                                                //{
                                                //    cardNumIndex = items.IndexOf("�����") - 1;
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
                                                //                        transactionDate = items[items.IndexOf("���") + 1].Substring(0, 8);
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
                                                string transactionNumber = items[items.IndexOf("���׺�") + 1];*/
                    }
                    catch (Exception exception)
                    {
                        //ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                        //        transactionDate, false,
                        //        "��Ƭʶ��ʶ����ȷ����Ƭ�Ƿ���������", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                        electronicInvoiceInfo.TransactionDate = originalTransactionDate;
                        electronicInvoiceInfo.DownloadResult = "��Ƭʶ��ʶ����ȷ����Ƭ�Ƿ���������";
                        electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                        _autoHaveDownloadedCount++;

                        //DownloadResult(electronicInvoiceInfo);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(cardNum) || string.IsNullOrWhiteSpace(transactionDate))
                    {
                        //ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                        //    transactionDate, false,
                        //    "ʶ�𲻵����źͽ�������", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                        electronicInvoiceInfo.DownloadResult = "ʶ�𲻵����źͽ�������";
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
                                //    "���ļ��Ѿ�����", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));

                                electronicInvoiceInfo.IsDownloaded = true;
                                electronicInvoiceInfo.DownloadResult = "���ļ�������";
                                electronicInvoiceInfo.CompleteTime = DateTime.Now.ToString("HH:mm:ss");
                                _autoHaveDownloadedCount++;
                                UpdateProgressBarItemBatchDownload();

                                //DownloadResult(electronicInvoiceInfo);
                            }));
                        }

                        continue;
                    }

                    electronicInvoiceInfo.DownloadResult = "����ʶ����֤��";
                    picVerificationImage.Image = GZXNetworking.GetValidateImage();
                    //                    string orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                    //                    if (string.IsNullOrWhiteSpace(orcResult))
                    //                    {
                    //                        electronicInvoiceInfo.DownloadResult = "��֤��ʶ�����������";
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
                    //    "�������ء���", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                    //DownloadResult(currentElectronicInvoiceInfo);
                    electronicInvoiceInfo.DownloadResult = "�������ء���";


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
                        if (GZXMessageBox.MessageBoxResult(failureCount + "�ŷ�Ʊ����ʧ�ܣ�Ҫ����������ʧ�ܵķ�Ʊ��������ȷ��������������ȡ����"))
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
                //����string �� images����
                foreach (var failureElectronicInvoiceInfo in failureElectronicInvoiceInfos)
                {
                    if (!_startBatchDownload)
                    {
                        _autoDownloadStopwatch.Stop();
                        timerAutoDownloadFile.Stop();

                        this.Invoke(new Action(() => { ShowBalloon(pictureBoxReceipt, "��ֹͣ����"); }));

                        return;
                    }


                    //                    Console.WriteLine(path);
                    string cardNum = null, transactionDate = null;
                    cardNum = failureElectronicInvoiceInfo.CardNum;
                    transactionDate = failureElectronicInvoiceInfo.TransactionDate;


                    ElectronicInvoiceInfo currentElectronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                        transactionDate, false,
                        "����ʶ����֤��", null, failureElectronicInvoiceInfo.ImageFileName);
                    DownloadResult(currentElectronicInvoiceInfo);

                    /*picVerificationImage.Image = GZXNetworking.GetValidateImage();
                    string orcResult = GZXNetworking.ORC(picVerificationImage.Image);
                    if (string.IsNullOrWhiteSpace(orcResult))
                    {
                        currentElectronicInvoiceInfo.DownloadResult = "��֤��ʶ�����������";
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
                    currentElectronicInvoiceInfo.DownloadResult = "�������ء���";
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
                    Console.WriteLine(string.Format("���ţ�{0}���������ڣ�{1}���Ƿ����أ�{2}�����ؽ����{3}�����ʱ�䣺{4}���ļ�����{5}",
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
                        if (GZXMessageBox.MessageBoxResult(failureCount + "�ŷ�Ʊʧ�ܣ�Ҫ����������ʧ�ܵķ�Ʊ��������ȷ��������������ȡ����"))
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

                //                        MessageBoxEx.Show("�������ط�Ʊ�����");
            }));

            _autoDownloadStopwatch.Stop();
            timerAutoDownloadFile.Stop();
        }

        private void buttonItemStopBulkDownloadInvoice_Click(object sender, EventArgs e)
        {
            ShowBalloon(pictureBoxReceipt, "��ǰ�������صķ�Ʊ������ɺ�Ż�ֹͣ����");
            buttonItemStartBulkDownloadInvoice.Enabled = true;
            buttonItemStopBulkDownloadInvoice.Enabled = false;
            _startBatchDownload = false;
        }

        #region ���ļ�ת������

        //public byte[] SetImageToByteArray(string fileName, ref string fileSize)
        /// <summary>
        /// ���ļ�ת������
        /// </summary>
        /// <param name="fileName">�ļ�ȫ·��</param>
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

        #region ��byteת����MemoryStream����

        /// <summary>
        /// ��byteת����MemoryStream����
        /// </summary>
        /// <param name="mybyte">byte[]����</param>
        /// <returns></returns>
        public MemoryStream ByteToStream(byte[] mybyte)
        {
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            return mymemorystream;
        }

        #endregion

        #region ��byteת����Image�ļ�

        /// <summary>
        /// ��byteת����Image�ļ�
        /// </summary>
        /// <param name="mybyte">byte[]����</param>
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

        #region TextBox�ؼ��¼�

        private void textBoxXTaxpayerRegistrationNumber_TextChanged(object sender, EventArgs e)
        {
            //            GlobalManager.UserConfig.TaxpayerRegistrationNumber = textBoxXTaxpayerRegistrationNumber.Text;
            //            GlobalManager.SaveUserConfig();
        }

        #endregion

        #region CheckBox ComboBox�ؼ��¼�

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
            MessageBoxEx.Show("<font size=\"15\" color=\"red\">������̣��������һ����Star��<br/>�ڴ򿪵�ҳ�������Ͻǵ�Star���ɣ���ҪǮ��</font>");

            Process.Start("https://github.com/GanZhiXiong/SZTElectronicInvoice");
        }

        #region �Ҽ��˵��¼�

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

                GZXMessageBox.Show("ɾ�����");
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
                new IdentificationFailureForm("ʶ��ʧ�ܣ����޸�Ϊ��ȷ��ʱ���ʽ�������գ���20190126��", "2019��10��22��");
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