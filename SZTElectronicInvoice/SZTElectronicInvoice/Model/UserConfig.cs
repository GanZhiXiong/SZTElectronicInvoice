using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SZTElectronicInvoice.Model
{
    [Serializable]
    public class UserConfig
    {
        public string BrowseInvoicePhotoFolder { get; set; }

        public bool IsSkipDownloadFile { get; set; } 

        private BindingList<CompanyInfo> _companyInfos;
        private CompanyInfo _selectedCompanyInfo;
        public int SelectedCompanyInfosIndex { get; set; }

        public CompanyInfo SelectedCompanyInfo
        {
            get { return _selectedCompanyInfo; }
            set
            {
                _selectedCompanyInfo = value; 
            }
        }

        public BindingList<CompanyInfo> CompanyInfos
        {
            get
            {
                if (_companyInfos==null)
                {
                    _companyInfos=new BindingList<CompanyInfo>();
                }
                return _companyInfos;
            }
            set
            {
                _companyInfos = value; 
            }
        }
    }

    [Serializable]
    public class CompanyInfo
    {
        public string CompanyName { get; set; }

        public string TaxpayerRegistrationNumber { get; set; }
    }
}
