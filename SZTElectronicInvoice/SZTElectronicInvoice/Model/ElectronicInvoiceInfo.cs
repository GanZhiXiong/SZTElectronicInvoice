using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SZTElectronicInvoice.Model
{
   public class ElectronicInvoiceInfo:INotifyPropertyChanged
    {
        private string _imageFileName;
        private string _cardNum;
        private string _transactionDate;
        private string _downloadResult;
        private string _completeTime;
        private bool _isDownloaded;
        private string _isDownloadedString;

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
                    _isDownloadedString = "是";
                }
                else
                {
                    _isDownloadedString = "否";
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

        public ElectronicInvoiceInfo(string cardNum, string transactionDate, bool isDownloaded, string downloadResult, string completeTime,string imageFileName)
        {
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
