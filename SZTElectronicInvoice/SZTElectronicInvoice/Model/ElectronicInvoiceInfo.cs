using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SZTElectronicInvoice.Model
{
    public class ElectronicInvoiceInfo : INotifyPropertyChanged
    {
        private string _imageFileName;
        private string _cardNum;
        private string _transactionDate;
        private string _downloadResult;
        private string _completeTime;
        private bool _isDownloaded;
        private string _isDownloadedString;
        private decimal _rechargeAmount;

        public string ImageFolder { get; set; }
        public string ImageFileName
        {
            get { return _imageFileName; }
            set
            {
                _imageFileName = value;

                PropertyChanged(this, new PropertyChangedEventArgs("ImageFileName"));
            }
        }

        public string CardNum
        {
            get { return _cardNum; }
            set
            {
                _cardNum = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CardNum"));
            }
        }

        public string TransactionDate
        {
            get { return _transactionDate; }
            set
            {
                _transactionDate = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TransactionDate"));

            }
        }

        public decimal RechargeAmount
        {
            get { return _rechargeAmount; }
            set
            {
                _rechargeAmount = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RechargeAmount"));
            }
        }

        public string DownloadResult
        {
            get { return _downloadResult; }
            set
            {
                _downloadResult = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DownloadResult"));
            }
        }

        public string CompleteTime
        {
            get { return _completeTime; }
            set
            {
                _completeTime = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CompleteTime"));
            }
        }

        public bool IsDownloaded
        {
            get { return _isDownloaded; }
            set
            {
                if (value)
                {
                    IsDownloadedString = "是";
                }
                else
                {
                    IsDownloadedString = "否";
                }
                _isDownloaded = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsDownloaded"));
            }
        }

        public string IsDownloadedString
        {
            get { return _isDownloadedString; }
            set
            {
                _isDownloadedString = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsDownloadedString"));
            }
        }

        public string ocrResult { get; set; }

        public ElectronicInvoiceInfo(string cardNum, string transactionDate, bool isDownloaded, string downloadResult, string completeTime, string imageFileName = "", string imageFolder = "", string ocrResult = "")
        {
            CardNum = cardNum;
            TransactionDate = transactionDate;
            DownloadResult = downloadResult;
            CompleteTime = completeTime;
            IsDownloaded = isDownloaded;
            ImageFileName = imageFileName;
            ImageFolder = imageFolder;
            this.ocrResult = ocrResult;
        }

        public ElectronicInvoiceInfo(string cardNum, string transactionDate, bool isDownloaded, string downloadResult, string completeTime, string imageFileName, decimal rechargeAmount)
        {
            RechargeAmount = rechargeAmount;
            CardNum = cardNum;
            TransactionDate = transactionDate;
            DownloadResult = downloadResult;
            CompleteTime = completeTime;
            IsDownloaded = isDownloaded;
            ImageFileName = imageFileName;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
