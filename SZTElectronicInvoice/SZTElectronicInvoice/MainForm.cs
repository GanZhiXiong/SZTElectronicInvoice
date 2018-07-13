using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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

        #region ���幹�캯��

        public MainForm()
        {
            #region test

            MatchCollection mc =
                Regex.Matches(
                    @"������Ҫ��HTML ҳ���ж�ȡ���ģ������ڵĽ��������ȡ >����<�м���ַ���˭�и��õĽ������������˭֪�� ����������ж� < > ��val=""100"">100Ԫ</td>",
                    @"(?<=^|>)((?![<>]).)+(?=Ԫ<|$)");
            foreach (Match m in mc)
            {
                Console.WriteLine(m.Value);
            }

            #endregion

            InitializeComponent();

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Text = "����ͨ���ӷ�Ʊ";
            this.MaximizeBox = false;
            //            CheckForIllegalCrossThreadCalls = false;

            metroShell1.TitleText = "<font size=\"11\">����ͨ���ӷ�Ʊ</font>";

            //            webBrowser1.Navigate("https://www.shenzhentong.com/service/fplist_101007009_368124878_20180308.html");

            #region monthCalendarAdvTransaction

            DateTime dateTime = DateTime.Now;
            this.monthCalendarAdvTransaction.DisplayMonth =
                new DateTime(dateTime.Year, dateTime.Month - 1, dateTime.Day, 0, 0, 0, 0);
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
            buttonItemStopBulkDownloadInvoice.Enabled = false;

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

            textBoxXBrowseInvoicePhotoFolder.Text = @"D:\Program Files\QQRecord\1551935335\FileRecv\MobileFile";

            textBoxX1CardNum.TabIndex = 0;
            textBoxX1CardNum.Focus();

            this.pictureBox1.BackColor = ColorTranslator.FromHtml("#007acc");

            //            string s= Path.GetDirectoryName(@"C:\Users\15519\Desktop\δ���سɹ��ķ�Ʊ\IMG_1434.JPG");
            //            string s = Path.Combine(Path.GetDirectoryName(@"C: \Users\15519\Desktop\δ���سɹ��ķ�Ʊ"),
            //                "123" + DateTime.Now.ToString(" HHmmss.fff") + ".JPG");
            //            return;

            buttonItemStartBulkDownloadInvoice.Enabled = false;
            buttonItemStartDownloadInvoice.Enabled = false;

            #region ��ȡ�����ļ�

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
//            return;
            Thread thread = new Thread(() =>
            {
                circularProgressSingleDownload.IsRunning = true;
                circularProgressSingleDownload.ProgressText = "��ȴ�";
                circularProgressSingleDownload.ProgressTextVisible = true;

                picVerificationImage.Image = ZXNetworking.GetValidateImage();
                string orcResult = ZXNetworking.ORC(picVerificationImage.Image);

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

            if (e.KeyCode == Keys.Enter && e.Alt)
            {
                buttonItemSearchElectronicInvoice_Click(null, null);
            }
        }

        #endregion

        #region private method

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

        private void UpdateProgressBarItemBatchDownload(int blockLength)
        {
            _autoHaveDownloadedCount++;
            progressBarItemBatchDownload.Value = (int) (progressBarItemBatchDownload.Maximum *
                                                        ((double) _autoHaveDownloadedCount / _autoDownloadCount));
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
            double progress = ((double) _autoHaveDownloadedCount / _autoDownloadCount) * 100;
            //            progressBarItemBatchDownload.Text = "��ɣ�" + (progress).ToString("0.##") + "%" + "   ��ʱ��" + (int)_autoDownloadStopwatch.Elapsed.TotalSeconds + "��";
            progressBarItemBatchDownload.Text = "�ѳɹ�/�����/�ܹ�(��):" +
                                                _currentAutoHaveDownloadElectronicInvoiceInfos
                                                    .FindAll(a => a.IsDownloaded).Count + "/" +
                                                _autoHaveDownloadedCount + "/" + _autoDownloadCount + "   �ϼ�(Ԫ):" +
                                                _currentAutoHaveDownloadElectronicInvoiceInfos
                                                    .FindAll(a => a.IsDownloaded == true).Sum(a => a.RechargeAmount) +
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
            zxDataGridViewXDownloadResult.AddColumn("IsDownloadedString", "�Ƿ�������");

            zxDataGridViewXDownloadResult.AddColumn("CompleteTime", "���ʱ��");
            zxDataGridViewXDownloadResult.AddColumn("DownloadResult", "���ؽ��");

            #endregion

            #region DataGridViewX �¼� 

            zxDataGridViewXDownloadResult.RowsAdded += ZxDataGridViewXDownloadResult_RowsAdded;
            //            TaskGridView.MouseClick += TaskGridView_MouseClick;
            //            TaskGridView.RowPostPaint += TaskGridView_RowPostPaint;
            //            TaskGridView.RowStateChanged += TaskGridView_RowStateChanged;

            #endregion
        }

        private void ZxDataGridViewXDownloadResult_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ZXDataGridViewX myDataGridViewX = (ZXDataGridViewX) sender;
            myDataGridViewX.FirstDisplayedScrollingRowIndex = myDataGridViewX.RowCount - 1;
        }

        private bool DownloadElectronicInvoice(ref string downloadResult,
            string cardNum, string transactionDate, string downloadFileName,
            ElectronicInvoiceInfo electronicInvoiceInfo = null)
        {
            try
            {
                string orcResult;
                string url = "https://www.shenzhentong.com/Ajax/ElectronicInvoiceAjax.aspx";

                string ret = ZXNetworking.PostRequest(url, string.Format("tp={0}&yzm={1}&cardnum={2}", "1",
                    textBoxXIdentifyCode.Text, cardNum
                ));
                //            GetValidateImage();

                string state = StringOperation.GetMiddleText(ret, @"state"":""", @"""");
                if (!state.Equals("100"))
                {
                    picVerificationImage.Image = ZXNetworking.GetValidateImage();
                    orcResult = ZXNetworking.ORC(picVerificationImage.Image);

                    textBoxXIdentifyCode.BeginInvoke(new EventHandler(delegate
                    {
                        textBoxXIdentifyCode.Text = orcResult;
                    }));

                    downloadResult = "��1����ֵ��¼�����ڻ����ѿ�Ʊ";
                    return false;
                }
                string fplistUrl = string.Format("https://www.shenzhentong.com/service/fplist_101007009_{0}_{1}.html",
                    cardNum, transactionDate);
                string ret1 = ZXNetworking.GetRequest(fplistUrl);

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
                    picVerificationImage.Image = ZXNetworking.GetValidateImage();
                    orcResult = ZXNetworking.ORC(picVerificationImage.Image);
                    textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

                    downloadResult = "��Ʊ�У����Ժ�����";
                    return false;
                }

                if (ret1.Contains(".pdf"))
                {
                    if (GlobalManager.UserConfig.IsSkipDownloadFile)
                    {
                        downloadResult = "����";
                        return false;
                    }

                    Regex regImg = new Regex(
                        @"(?is)<a[^>]*?href=(['""\s]?)(?<href>([^'""\s]*\.doc)|([^'""\s]*\.docx)|([^'""\s]*\.xls)|([^'""\s]*\.xlsx)|([^'""\s]*\.ppt)|([^'""\s]*\.txt)|([^'""\s]*\.zip)|([^'""\s]*\.rar)|([^'""\s]*\.gz)|([^'""\s]*\.pdf))\1[^>]*?>");
                    MatchCollection match = regImg.Matches(ret1);
                    foreach (Match m in match)
                    {
                        //                Response.Write(m.Groups["href"].Value + "<br/>");
                        //                Console.WriteLine(m.Groups["href"].Value );
                        string pdfDownloadUrl = m.Groups["href"].Value;
                        bool pdfDownloadRet = ZXNetworking.DownloadFile(pdfDownloadUrl, GlobalManager.DownloadPath,
                            downloadFileName, 3);
                        picVerificationImage.Image = ZXNetworking.GetValidateImage();
                        orcResult = ZXNetworking.ORC(picVerificationImage.Image);
                        textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

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
                    downloadResult = "sjΪ��";
                    return false;
                }


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
                paramStringBuilder.Append("jlsh=14");
                paramStringBuilder.Append("&");
                paramStringBuilder.Append("jzdh=262011503");
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
                string ret2 = ZXNetworking.PostRequest(url, paramStringBuilder.ToString());
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
                string ret3 = ZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                string downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
                if (string.IsNullOrWhiteSpace(downloadUrl))
                {
                    Thread.Sleep(3000);
                    ret3 = ZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                    downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
                }
                if (string.IsNullOrWhiteSpace(downloadUrl))
                {
                    Thread.Sleep(3000);
                    ret3 = ZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                    downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
                }
                if (string.IsNullOrWhiteSpace(downloadUrl))
                {
                    Thread.Sleep(3000);
                    ret3 = ZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                    downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
                }
                if (string.IsNullOrWhiteSpace(downloadUrl))
                {
                    url = string.Format("https://www.shenzhentong.com/service/fplist_101007009_{0}_{1}.html",
                        cardNum, transactionDate);
                    ret1 = ZXNetworking.GetRequest(url);

                    if (ret1.Contains("��Ʊ��"))
                    {
                        picVerificationImage.Image = ZXNetworking.GetValidateImage();
                        orcResult = ZXNetworking.ORC(picVerificationImage.Image);
                        textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

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
                            bool pdfDownloadRet = ZXNetworking.DownloadFile(pdfDownloadUrl, GlobalManager.DownloadPath,
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
                bool downloadRet = ZXNetworking.DownloadFile(downloadUrl, GlobalManager.DownloadPath,
                    downloadFileName, 3);
                picVerificationImage.Image = ZXNetworking.GetValidateImage();
                orcResult = ZXNetworking.ORC(picVerificationImage.Image);
                textBoxXIdentifyCode.Invoke(new Action(() => { textBoxXIdentifyCode.Text = orcResult; }));

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
            picVerificationImage.Image = picVerificationImage.Image = ZXNetworking.GetValidateImage();
            textBoxXIdentifyCode.Text = ZXNetworking.ORC(picVerificationImage.Image);
        }

        private void buttonItemSearchElectronicInvoice_Click(object sender, EventArgs e)
        {
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
                    "�������ء���", DateTime.Now.ToString("HH:mm:ss"), "");

                try
                {
                    this.Invoke(new Action(() => { GlobalManager.ElectronicInvoiceInfos.Add(electronicInvoiceInfo); }));

                    string downloadResult = "";
                    bool ret = DownloadElectronicInvoice(ref downloadResult, cardNum, transactionDate,
                        cardNum + " " + transactionDate + ".pdf", electronicInvoiceInfo);

                    balloonTip1.SetBalloonText(textBoxX1CardNum, downloadResult);
                    balloonTip1.ShowBalloon(textBoxX1CardNum);
                    textBoxX1CardNum.Focus();
                    textBoxX1CardNum.SelectAll();

                    electronicInvoiceInfo.IsDownloaded = ret;
                    electronicInvoiceInfo.DownloadResult = downloadResult;
                    //                    this.Invoke(new Action(() =>
                    //                    {
                    //                        GlobalManager.ElectronicInvoiceInfos.Add(electronicInvoiceInfo);
                    //                    }));
                }
                catch (Exception exception)
                {
                    electronicInvoiceInfo.DownloadResult = exception.Message;
                    /*    try
                        {
                            this.Invoke(new Action(() =>
                            {
                                GlobalManager.ElectronicInvoiceInfos.Add(new ElectronicInvoiceInfo(cardNum, transactionDate, false,
                                    exception.Message, DateTime.Now.ToString("HH:mm:ss"), ""));
                            }));
                        }
                        catch (Exception e1)
                        {
                            Console.WriteLine(e1);
                        }*/
                }

                this.Invoke(new Action(() => { circularProgressSingleDownload.IsRunning = false; }));
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

        private void buttonItemStartBulkDownloadInvoice_Click(object sender, EventArgs e)
        {
            //            MessageBoxEx.Show("�������ط�Ʊ�����");
            //            return;
            if (!IsConfigurationComplete())
            {
                return;
            }

            _autoDownloadStopwatch.Restart();
            timerAutoDownloadFile.Start();

            buttonItemStartBulkDownloadInvoice.Enabled = false;
            buttonItemStopBulkDownloadInvoice.Enabled = true;
            _startBatchDownload = true;
            _currentAutoHaveDownloadElectronicInvoiceInfos.Clear();

            //�Ĵ�����쳣��·���о��зǷ��ַ���
            //            var images = Directory.GetFiles(@"C:\\Users\\15519\\Desktop\\����ͨ���ӷ�Ʊ", "*.png|*.jpg", SearchOption.TopDirectoryOnly);
            //            Directory.GetFiles ����ָ��Ŀ¼���ļ�·�� SearchOption.AllDirectories ָ��������ǰĿ¼����Ŀ¼

            var images = Directory.GetFiles(textBoxXBrowseInvoicePhotoFolder.Text, ".", SearchOption.TopDirectoryOnly)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".JPG") || s.EndsWith(".gif"));
            _autoDownloadCount = images.Count();
            _autoHaveDownloadedCount = 0;

            progressBarItemBatchDownload.Text = "";
            progressBarItemBatchDownload.Value = 0;
            progressBarItemBatchDownload.TextVisible = true;

            Thread thread = new Thread(() =>
            {
                try
                {
                    //����string �� images����
                    foreach (var path in images)
                    {
                        if (!_startBatchDownload)
                        {
                            _autoDownloadStopwatch.Stop();
                            timerAutoDownloadFile.Stop();

                            this.Invoke(new Action(() => { ShowBalloon(pictureBoxReceipt, "��ֹͣ����"); }));

                            return;
                        }

                        Console.WriteLine(path);
                        string cardNum = null, transactionDate = null;

                        System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                        System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                        img.Dispose();

                        pictureBoxReceipt.Image = bmp;
                        textBoxXInvoiceRecognitionResult.Invoke(new Action(() =>
                        {
                            textBoxXInvoiceRecognitionResult.Text =
                                "����ʶ�𡭡�\r\n��ʶ����ȷ������ԭ�����£�\r\n1��ͼƬ���ֲ��ǳ����棬�뽫ͼƬ��ת���ٲ���\r\n2��ͼƬ���ֲ�������ȱʧ";
                        }));

                        string result = Youtu.generalocr(path);

                        JObject jObj = JObject.Parse(result);
                        var items = (from staff1 in jObj["items"].Children()
                            select (string) staff1["itemstring"]).ToList();

                        if (!items.Contains("���"))
                        {
                            this.Invoke(new Action(() =>
                            {
                                //                                progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / images.Count();
                                UpdateProgressBarItemBatchDownload(images.Count());

                                ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                                    transactionDate, false,
                                    "����������ֻ֧�����������վ��ֵ��СƱ", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                                _currentAutoHaveDownloadElectronicInvoiceInfos.Add(electronicInvoiceInfo);
                                GlobalManager.ElectronicInvoiceInfos.Add(electronicInvoiceInfo);
                            }));
                            continue;
                        }


                        int cardNumIndex = items.IndexOf("�����") + 1;
                        cardNum = items[cardNumIndex].Replace(" ", "");
                        int intTemp = 0;
                        if (!int.TryParse(cardNum, out intTemp)) //�ж��Ƿ����ת��Ϊ����
                        {
                            cardNumIndex = items.IndexOf("�����") - 1;
                            cardNum = items[cardNumIndex].Replace(" ", "");
                        }
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
                        transactionDate = items.Find(a => a.Contains(":") || a.Contains("/")).Replace(":;", ":")
                            .Replace(";", ":").Trim();
                        if (!transactionDate.Contains(" "))
                        {
                            transactionDate = transactionDate.Insert(8, " ");
                        }
                        transactionDate = Convert
                            .ToDateTime(transactionDate.Insert(transactionDate.LastIndexOf('/') + 1, "20"))
                            .ToString("yyyyMMdd").Replace(" ", "");
                        string transactionNumber = items[items.IndexOf("���׺�") + 1];
                        if (string.IsNullOrWhiteSpace(cardNum) || string.IsNullOrWhiteSpace(transactionDate))
                        {
                            ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                                transactionDate, false,
                                "ʶ�𲻵����źͽ�������", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                            _currentAutoHaveDownloadElectronicInvoiceInfos.Add(electronicInvoiceInfo);
                            GlobalManager.ElectronicInvoiceInfos.Add(electronicInvoiceInfo);
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
                                    UpdateProgressBarItemBatchDownload(images.Count());

                                    ElectronicInvoiceInfo electronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                                        transactionDate, false,
                                        "���ļ��Ѿ�����", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                                    _currentAutoHaveDownloadElectronicInvoiceInfos.Add(electronicInvoiceInfo);
                                    GlobalManager.ElectronicInvoiceInfos.Add(electronicInvoiceInfo);
                                }));
                            }
                            continue;
                        }

                        ElectronicInvoiceInfo currentElectronicInvoiceInfo = new ElectronicInvoiceInfo(cardNum,
                            transactionDate, false,
                            "�������ء���", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path));
                        _currentAutoHaveDownloadElectronicInvoiceInfos.Add(currentElectronicInvoiceInfo);
                        if (zxDataGridViewXDownloadResult.IsHandleCreated)
                        {
                            zxDataGridViewXDownloadResult.Invoke(new Action(() =>
                            {
                                GlobalManager.ElectronicInvoiceInfos.Add(currentElectronicInvoiceInfo);
                            }));
                        }

                        bool ret = false;
                        string downloadResult = "";
                        decimal rechargeAmount = 0;
                        ret = DownloadElectronicInvoice(ref downloadResult, cardNum, transactionDate, downloadFileName,
                            currentElectronicInvoiceInfo);

                        currentElectronicInvoiceInfo.DownloadResult = downloadResult;
                        currentElectronicInvoiceInfo.IsDownloaded = ret;

                        if (this.IsHandleCreated)
                        {
                            progressBarItemBatchDownload.Invoke(new Action(() =>
                            {
                                UpdateProgressBarItemBatchDownload(images.Count());
                                //                            progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / images.Count();
                            }));
                        }

                        picVerificationImage.Image = ZXNetworking.GetValidateImage();
                        string orcResult = ZXNetworking.ORC(picVerificationImage.Image);
                        textBoxXIdentifyCode.BeginInvoke(new EventHandler(delegate
                        {
                            textBoxXIdentifyCode.Text = orcResult;
                        }));
                    }

                    foreach (var electronicInvoiceInfo in GlobalManager.ElectronicInvoiceInfos)
                    {
                        Console.WriteLine(string.Format("���ţ�{0}���������ڣ�{1}���Ƿ����أ�{2}�����ؽ����{3}�����ʱ�䣺{4}���ļ�����{5}",
                            electronicInvoiceInfo.CardNum, electronicInvoiceInfo.TransactionDate,
                            electronicInvoiceInfo.IsDownloaded, electronicInvoiceInfo.DownloadResult,
                            electronicInvoiceInfo.CompleteTime, electronicInvoiceInfo.ImageFileName));
                    }

                    this.BeginInvoke(new Action(() =>
                    {
                        buttonItemStartBulkDownloadInvoice.Enabled = true;
                        buttonItemStopBulkDownloadInvoice.Enabled = false;

                        //                        MessageBoxEx.Show("�������ط�Ʊ�����");
                    }));

                    _autoDownloadStopwatch.Stop();
                    timerAutoDownloadFile.Stop();
                }
                catch (Exception exception)
                {
                }
            });
            thread.Start();
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
                int streamLength = (int) fs.Length;
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
            GlobalManager.UserConfig.SelectedCompanyInfo = (CompanyInfo) comboBoxCompanyName.SelectedItem;
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
    }
}