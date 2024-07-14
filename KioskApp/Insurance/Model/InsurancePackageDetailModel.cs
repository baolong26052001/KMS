using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Insurance.Model
{
    public class InsurancePackageDetailModel
    {
        public int idHeader { get; set; }
        public string contentHeader { get; set; }
        public string coverageHeader { get; set; }
        public string descriptionHeader { get; set; }
        public int packageId { get; set; }
        public List<InsurancePackageBenefitDetail> details { get; set; }
    }

    public class InsurancePackageBenefitDetail
    {
        public int idDetail { get; set; }
        public string contentDetail { get; set; }
        public string coverageDetail { get; set; }
        public int benefitId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isCoverageNumeric;
        public bool IsCoverageNumeric
        {
            get { return _isCoverageNumeric; }
            set
            {
                _isCoverageNumeric = value;
                OnPropertyChanged(nameof(IsCoverageNumeric));
            }
        }
    }
}
