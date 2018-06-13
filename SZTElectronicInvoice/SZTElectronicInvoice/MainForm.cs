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

        #region ���幹�캯��

        public MainForm()
        {
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
                textBoxXTaxpayerRegistrationNumber.Text = GlobalManager.UserConfig.SelectedCompanyInfo.TaxpayerRegistrationNumber;
            }
            comboBoxCompanyName.SelectedIndexChanged += ComboBoxCompanyName_SelectedIndexChanged;

            #endregion

            Thread thread = new Thread(() =>
            {
                circularProgressSingleDownload.IsRunning = true;
                circularProgressSingleDownload.ProgressText = "��ȴ�";
                circularProgressSingleDownload.ProgressTextVisible = true;

                picVerificationImage.Image = ZXNetworking.GetValidateImage();
                string orcResult = ZXNetworking.ORC(picVerificationImage.Image);

                this.Invoke(new Action(() =>
                {
                    textBoxXIdentifyCode.Text = orcResult;
                    buttonItemStartBulkDownloadInvoice.Enabled = true;
                    buttonItemStartDownloadInvoice.Enabled = true;
                    circularProgressSingleDownload.IsRunning = false;
                    circularProgressSingleDownload.ProgressTextVisible = false;
                }));
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

        private void ShowBalloon(Control control, string message)
        {
            balloonTip1.SetBalloonText(control, message);
            balloonTip1.ShowBalloon(control);
        }

        private bool IsConfigurationComplete()
        {

            if (GlobalManager.UserConfig.SelectedCompanyInfo == null || string.IsNullOrWhiteSpace(GlobalManager.UserConfig.SelectedCompanyInfo.CompanyName))
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
            progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / blockLength;
            progressBarItemBatchDownload.TextVisible = true;
            UpdateprogressBarItemBatchDownloadText();
        }

        private void UpdateprogressBarItemBatchDownloadText()
        {
            progressBarItemBatchDownload.TextVisible = true;
            progressBarItemBatchDownload.Text = "��ɣ�" + (((double)progressBarItemBatchDownload.Value / progressBarItemBatchDownload.Maximum) * 100).ToString("0.##") + "%" + "   ��ʱ��" + (int)_autoDownloadStopwatch.Elapsed.TotalSeconds + "��";
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
            ZXDataGridViewX myDataGridViewX = (ZXDataGridViewX)sender;
            myDataGridViewX.FirstDisplayedScrollingRowIndex = myDataGridViewX.RowCount - 1;
        }

        private bool DownloadElectronicInvoice(ref string downloadResult, string cardNum, string transactionDate, string downloadFileName)
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

            if (ret1.Contains("��Ʊ��"))
            {
                picVerificationImage.Image = ZXNetworking.GetValidateImage();
                orcResult = ZXNetworking.ORC(picVerificationImage.Image);
                textBoxXIdentifyCode.Invoke(new Action(() =>
                {
                    textBoxXIdentifyCode.Text = orcResult;
                }));

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
                    picVerificationImage.Image = ZXNetworking.GetValidateImage();
                    orcResult = ZXNetworking.ORC(picVerificationImage.Image);
                    textBoxXIdentifyCode.Invoke(new Action(() =>
                    {
                        textBoxXIdentifyCode.Text = orcResult;
                    }));

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
            paramStringBuilder.Append("jfirmfpmc=" + GlobalManager.UserConfig.SelectedCompanyInfo.TaxpayerRegistrationNumber);

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
            string ret3 = ZXNetworking.PostRequest(url, paramStringBuilder.ToString());
            string downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
            if (string.IsNullOrWhiteSpace(downloadUrl))
            {
                Thread.Sleep(1000);
                ret3 = ZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
            }
            if (string.IsNullOrWhiteSpace(downloadUrl))
            {
                Thread.Sleep(1000);
                ret3 = ZXNetworking.PostRequest(url, paramStringBuilder.ToString());
                downloadUrl = StringOperation.GetMiddleText(ret3, @"strs"":""", @"""");
            }
            if (string.IsNullOrWhiteSpace(downloadUrl))
            {
                Thread.Sleep(1000);
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
                    textBoxXIdentifyCode.Invoke(new Action(() =>
                    {
                        textBoxXIdentifyCode.Text = orcResult;
                    }));

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
            textBoxXIdentifyCode.Invoke(new Action(() =>
            {
                textBoxXIdentifyCode.Text = orcResult;
            }));

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

            if (GlobalManager.UserConfig.CompanyInfos.ToList().Exists(p => p.CompanyName.Equals(comboBoxCompanyName.Text)))
            {
                balloonTip1.SetBalloonText(comboBoxCompanyName, "��˾�����Ѵ���");
                balloonTip1.ShowBalloon(comboBoxCompanyName);

                comboBoxCompanyName.Focus();
                comboBoxCompanyName.SelectAll();
                return;
            }

            if (GlobalManager.UserConfig.CompanyInfos.ToList().Exists(p => p.TaxpayerRegistrationNumber.Equals(textBoxXTaxpayerRegistrationNumber.Text)))
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
                GlobalManager.UserConfig.CompanyInfos.ToList().Find(p => p.CompanyName.Equals(comboBoxCompanyName.Text));

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
                try
                {
                    string downloadResult = "";
                    bool ret = DownloadElectronicInvoice(ref downloadResult, cardNum, transactionDate, cardNum + " " + transactionDate + ".pdf");

                    balloonTip1.SetBalloonText(textBoxX1CardNum, downloadResult);
                    balloonTip1.ShowBalloon(textBoxX1CardNum);
                    textBoxX1CardNum.Focus();
                    textBoxX1CardNum.SelectAll();

                    this.Invoke(new Action(() =>
                    {
                        GlobalManager.ElectronicInvoiceInfos.Add(new ElectronicInvoiceInfo(cardNum, transactionDate, ret,
                            downloadResult, DateTime.Now.ToString("HH:mm:ss"), ""));
                    }));
                }
                catch (Exception exception)
                {
                    try
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
                    }
                }

                this.Invoke(new Action(() =>
                {
                    circularProgressSingleDownload.IsRunning = false;
                }));
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

            _autoDownloadStopwatch.Start();
            timerAutoDownloadFile.Start();

            buttonItemStartBulkDownloadInvoice.Enabled = false;
            buttonItemStopBulkDownloadInvoice.Enabled = true;
            _startBatchDownload = true;

            //�Ĵ�����쳣��·���о��зǷ��ַ���
            //            var images = Directory.GetFiles(@"C:\\Users\\15519\\Desktop\\����ͨ���ӷ�Ʊ", "*.png|*.jpg", SearchOption.TopDirectoryOnly);
            //            Directory.GetFiles ����ָ��Ŀ¼���ļ�·�� SearchOption.AllDirectories ָ��������ǰĿ¼����Ŀ¼

            var images = Directory.GetFiles(textBoxXBrowseInvoicePhotoFolder.Text, ".", SearchOption.TopDirectoryOnly)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".JPG") || s.EndsWith(".gif"));

            progressBarItemBatchDownload.Text = "";
            progressBarItemBatchDownload.Value = 0;
            progressBarItemBatchDownload.TextVisible = true;

            Thread thread = new Thread(() =>
            {
                //����string �� images����
                foreach (var path in images)
                {
                    if (!_startBatchDownload)
                    {
                        _autoDownloadStopwatch.Stop();
                        timerAutoDownloadFile.Stop();

                        this.Invoke(new Action(() =>
                        {
                            ShowBalloon(pictureBoxReceipt, "��ֹͣ����");
                        }));
                    
                        return;
                    }

                    Console.WriteLine(path);
                    string cardNum = null, transactionDate = null;
                    //                    try
                    //                    {
                    //                    pictureBoxReceipt.Image = Image.FromFile(path);
                    //                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                    //                    System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                    //                    img.Dispose();
                    //                    pictureBoxReceipt.Image = bmp;

                    //                     FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    //                    pictureBoxReceipt.Image = Image.FromStream(fileStream);
                    //                     fileStream.Close();
                    //                     fileStream.Dispose();

                    /*                    //��ȡ�ļ���
                                     /*   System.IO.FileStream fs = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read);

                                        int byteLength = (int)fs.Length;
                                        byte[] fileBytes = new byte[byteLength];
                                        fs.Read(fileBytes, 0, byteLength);

                                        //�ļ������],�ļ��������
                                        fs.Close();
                                        Image image = Image.FromStream(new MemoryStream(fileBytes));#1#

                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                    System.Drawing.Image bmp = new System.Drawing.Bitmap(img.Width, img.Height, img.PixelFormat);

                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
                    g.DrawImage(img, 0, 0);
                    g.Flush();
                    g.Dispose();
                    img.Dispose();
                    pictureBoxReceipt.Image = img;*/

                    //                    pictureBoxReceipt.Image = Image.FromStream(ByteToStream(SetImageToByteArray(path)));
                    //
                    // ͨ������clone�ķ�ʽ��ʹ��clone����ֵ���Ӷ� FileSourcePath��Ӧ��ͼƬ�õ�����
                    //
                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                    System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                    img.Dispose();

                    pictureBoxReceipt.Image = bmp;

                    string result = Youtu.generalocr(path);

                    JObject jObj = JObject.Parse(result);
                    var items = (from staff1 in jObj["items"].Children()
                                 select (string)staff1["itemstring"]).ToList();

                    if (!items.Contains("���"))
                    {
                        this.Invoke(new Action(() =>
                        {
                            //                                progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / images.Count();
                            UpdateProgressBarItemBatchDownload(images.Count());

                            GlobalManager.ElectronicInvoiceInfos.Add(new ElectronicInvoiceInfo(cardNum, transactionDate, false,
                                "����������ֻ֧�����������վ��ֵ��СƱ", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path)));
                        }));
                        continue;
                    }

                    int cardNumIndex = items.IndexOf("�����") + 1;
                    cardNum = items[cardNumIndex].Replace(" ", "");
                    transactionDate = items[items.IndexOf("���") + 1].Substring(0, 8);
                    transactionDate = Convert
                        .ToDateTime(transactionDate.Insert(transactionDate.LastIndexOf('/') + 1, "20"))
                        .ToString("yyyyMMdd").Replace(" ", "");
                    string transactionNumber = items[items.IndexOf("���׺�") + 1];

                    StringBuilder resultStringBuilder = new StringBuilder();
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

                    textBoxXInvoiceRecognitionResult.Invoke(new Action(() =>
                    {
                        textBoxXInvoiceRecognitionResult.Text = resultStringBuilder.ToString();
                    }));

                    string downloadFileName = cardNum + " " + transactionNumber + ".pdf";
                    if (File.Exists(GlobalManager.DownloadPath + downloadFileName))
                    {
                        if (this.IsHandleCreated)
                        {
                            this.Invoke(new Action(() =>
                            {
                                //                                    progressBarItemBatchDownload.Value += progressBarItemBatchDownload.Maximum / images.Count();
                                UpdateProgressBarItemBatchDownload(images.Count());

                                GlobalManager.ElectronicInvoiceInfos.Add(new ElectronicInvoiceInfo(cardNum,
                                    transactionDate, false,
                                    "���ļ��Ѿ�����", DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path)));
                            }));
                        }
                        continue;
                    }

                    bool ret = false;
                    string downloadResult = "";
                    ret = DownloadElectronicInvoice(ref downloadResult, cardNum, transactionDate, downloadFileName);

                    if (zxDataGridViewXDownloadResult.IsHandleCreated)
                    {
                        zxDataGridViewXDownloadResult.Invoke(new Action(() =>
                        {
                            GlobalManager.ElectronicInvoiceInfos.Add(new ElectronicInvoiceInfo(cardNum,
                                transactionDate, ret,
                                downloadResult, DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path)));
                        }));
                    }
                    picVerificationImage.Image = ZXNetworking.GetValidateImage();
                    string orcResult = ZXNetworking.ORC(picVerificationImage.Image);
                    textBoxXIdentifyCode.BeginInvoke(new EventHandler(delegate
                    {
                        textBoxXIdentifyCode.Text = orcResult;
                    }));

                    if (!ret)
                    {
                        //                        File.Copy(path,Path.path);
                        //                        File.Move(path, Path.Combine(@"C:\Users\15519\Desktop\δ���سɹ��ķ�Ʊ", cardNum + DateTime.Now.ToString(" HHmmss.fff") + ".JPG"));

                    }
                    else
                    {
                        //                        File.Delete(path);
                    }
                    //                    }
                    //                    catch (Exception exception)
                    //                    {
                    //                        try
                    //                        {
                    //                            this.Invoke(new Action(() =>
                    //                            {
                    //                                GlobalManager.ElectronicInvoiceInfos.Add(new ElectronicInvoiceInfo(cardNum, transactionDate, false,
                    //                                    exception.Message, DateTime.Now.ToString("HH:mm:ss"), Path.GetFileName(path)));
                    //                            }));
                    //                        }
                    //                        catch (Exception e1)
                    //                        {
                    //                            Console.WriteLine(e1);
                    //                        }
                    //                    }

                    if (this.IsHandleCreated)
                    {
                        progressBarItemBatchDownload.Invoke(new Action(() =>
                        {
                            UpdateProgressBarItemBatchDownload(images.Count());
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

                this.BeginInvoke(new Action(() =>
                {
                    buttonItemStartBulkDownloadInvoice.Enabled = true;
                    buttonItemStopBulkDownloadInvoice.Enabled = false;

                    MessageBoxEx.Show("�������ط�Ʊ�����");
                }));

                _autoDownloadStopwatch.Stop();
                timerAutoDownloadFile.Stop();
            });
            thread.Start();
        }

        private void buttonItemStopBulkDownloadInvoice_Click(object sender, EventArgs e)
        {
            ShowBalloon(pictureBoxReceipt,"��ǰ�������صķ�Ʊ������ɺ�Ż�ֹͣ����");
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
            textBoxXTaxpayerRegistrationNumber.Text = GlobalManager.UserConfig.SelectedCompanyInfo.TaxpayerRegistrationNumber;
        }

        #endregion

        private void timerAutoDownloadFile_Tick(object sender, EventArgs e)
        {
            UpdateprogressBarItemBatchDownloadText();
        }
    }
}