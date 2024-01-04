using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Insurance.Command;

namespace Insurance.ViewModel
{
    class VirtualNumpadVM : ViewModelBase
    {
        public ICommand NumberCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        
        public VirtualNumpadVM()
        {
            MainWindowVM.Instance.setBlankLayout();

            NumberCommand = new RelayCommand((parameter) =>
            {
                AddToDisplay(parameter.ToString());
            });

            DeleteCommand = new RelayCommand((parameter) =>
            {
                DeleteFromDisplay();
            });

            ClearCommand = new RelayCommand((parameter) =>
            {
                ClearFromDisplay();
            });
        }

        private string _displayText1;
        public string DisplayText1
        {
            get { return _displayText1; }
            set
            {
                if (_displayText1 != value)
                {
                    _displayText1 = value;
                    OnPropertyChanged(nameof(DisplayText1));
                }
            }
        }
        private string _displayText2;
        public string DisplayText2
        {
            get { return _displayText2; }
            set
            {
                if (_displayText2 != value)
                {
                    _displayText2 = value;
                    OnPropertyChanged(nameof(DisplayText2));
                }
            }
        }
        private string _displayText3;
        public string DisplayText3
        {
            get { return _displayText3; }
            set
            {
                if (_displayText3 != value)
                {
                    _displayText3 = value;
                    OnPropertyChanged(nameof(DisplayText3));
                }
            }
        }
        private string _displayText4;
        public string DisplayText4
        {
            get { return _displayText4; }
            set
            {
                if (_displayText4 != value)
                {
                    _displayText4 = value;
                    OnPropertyChanged(nameof(DisplayText4));
                }
            }
        }

        private void AddToDisplay(string value)
        {
            if (DisplayText1 == null)
            {
                DisplayText1 += value;
            }
            else if (DisplayText1 != null && DisplayText2 == null)
            {
                DisplayText2 += value;
            }
            else if (DisplayText1 != null && DisplayText2 != null && DisplayText3 == null)
            {
                DisplayText3 += value;
            }
            else if (DisplayText1 != null && DisplayText2 != null && DisplayText3 != null && DisplayText4 == null)
            {
                DisplayText4 += value;
            }
        }

        private void DeleteFromDisplay()
        {
            if (!string.IsNullOrEmpty(DisplayText4))
            {
                DisplayText4 = null;
            }
            else if (!string.IsNullOrEmpty(DisplayText3))
            {
                DisplayText3 = null;
            }
            else if (!string.IsNullOrEmpty(DisplayText2))
            {
                DisplayText2 = null;
            }
            else if (!string.IsNullOrEmpty(DisplayText1))
            {
                DisplayText1 = null;
            }
        }

        private void ClearFromDisplay()
        {
            DisplayText1 = null;
            DisplayText2 = null;
            DisplayText3 = null;
            DisplayText4 = null;
        }
    }

}

