using Insurance.Command;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Insurance.ViewModel
{
    class VirtualKeyboardVM : ViewModelBase
    {
        public ICommand NumberCommand { get; set; }
        public ICommand SpaceCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ShiftCommand { get; set; }

        public VirtualKeyboardVM()
        {
            NumberCommand = new RelayCommand((parameter) =>
            {
                AddToDisplay(parameter.ToString());
            });

            SpaceCommand = new RelayCommand((parameter) =>
            {
                SpaceText();
            });

            DeleteCommand = new RelayCommand((parameter) =>
            {
                DeleteFromDisplay();
            });

            ShiftCommand = new RelayCommand((parameter) =>
            {
                ToggleShift();
            });
        }

        //private string _displayText;
        //public string DisplayText
        //{
        //    get { return _displayText; }
        //    set
        //    {
        //        if (_displayText != value)
        //        {
        //            _displayText = value;

        //            OnPropertyChanged(nameof(DisplayText));

        //        }
        //    }
        //}


        private string _displayText;

        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged(nameof(DisplayText));
                    OnPropertyChanged(nameof(DisplayText1));
                    OnPropertyChanged(nameof(DisplayText2));
                }
            }
        }

        public string DisplayText1
        {
            get { return DisplayText; }
            set { DisplayText = value; }
        }

        public string DisplayText2
        {
            get { return DisplayText; }
            set { DisplayText = value; }
        }



        private bool _isShifted;
        public bool IsShifted
        {
            get { return _isShifted; }
            set
            {
                if (_isShifted != value)
                {
                    _isShifted = value;
                    OnPropertyChanged(nameof(IsShifted));
                }
            }
        }

        private void SpaceText()
        {
            DisplayText += " ";
        }


        private void AddToDisplay(string value)
        {
            value = IsShifted ? value.ToUpper() : value.ToLower();
            DisplayText += value;
            IsShifted = false;
        }

        private void DeleteFromDisplay()
        {
            if (!DisplayText.Equals(string.Empty))
                DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
            IsShifted = false;
        }

        private void ToggleShift()
        {
            IsShifted = !IsShifted;
        }



    }
}

