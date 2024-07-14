using Insurance.Command;
using Insurance.Model;
using Insurance.Service;
using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static Insurance.Utility.APIHelper;

namespace Insurance.ViewModel
{
    public class WelcomeVM : ViewModelBase
    {
        public ICommand NextButtonCommand { get; set; }

        private int slideHeaderId = 1;
        private List<SlideDetails> slideDetails;
        private List<SlideHeader> slideHeader;
        private int currentIndex = 0;
        private DispatcherTimer timerSlide;

        public WelcomeVM()
        {
            UserModel.Instance.IsWelcomeView = true;
            KioskModel.Instance.KioskID = 1;
            GetHardwareStatus hardwareStatus = new GetHardwareStatus();
            KioskModel.Instance.PrinterStatus = hardwareStatus.getPrinterStatus();
            APIHelper.Instance.UpdatePrinterStatus(KioskModel.Instance.KioskID, KioskModel.Instance.PrinterStatus);
            NextButtonCommand = new RelayCommand((parameter) =>
            {
                StopTimer();
                MainWindowVM.Instance.setDefaultLayout();
                MainWindowVM.Instance.CurrentView = new VerifyFaceIDVM(0);
                //MainWindowVM.Instance.CurrentView = HomeVM.Instance;
                UserModel.Instance.IsWelcomeView = false;
                MainWindowVM.Instance.UserGuideVisible = Visibility.Collapsed;

            });
            LoadSlides();
            StartTimer();

        }

        private void LoadSlides()
        {
            slideHeader = APIHelper.Instance.SlideHeaderShow(slideHeaderId);
            slideDetails = APIHelper.Instance.SlideDetailShow(slideHeaderId);
            ShowSlide();
        }

        private async void ShowSlide()
        {
            try
            {
                if (currentIndex >= slideDetails.Count)
                {
                    currentIndex = 0;
                }
                if (slideDetails[currentIndex].isActive && IsSlideWithinDateRange(currentIndex))
                {
                    string contentUrl = slideDetails[currentIndex].contentUrl;
                    string imageUrl = $"{KioskModel.Instance.APIPort}images/{contentUrl}";

                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync(imageUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                            BitmapImage bitmap = new BitmapImage();
                            using (MemoryStream stream = new MemoryStream(imageBytes))
                            {
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.StreamSource = stream;
                                bitmap.EndInit();
                            }

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ImageSource = bitmap;
                                APIHelper.Instance.UpdateKioskStatus(KioskModel.Instance.KioskID, 1);
                            });
                            return;
                        }
                    }
                }
                else
                {
                    SetDefaultImage();
                }
            }
            catch (Exception ex)
            {
                SetDefaultImage();
                APIHelper.Instance.Log($"Disconnect with server: {ex.Message}");
                APIHelper.Instance.Log($"Info: Please check the internet connection!!!");
            }
        }

        private void SetDefaultImage()
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                string imagePath = Path.Combine(baseDirectory, "slide", "Pre-Ads.png");

                BitmapImage defaultBitmap = new BitmapImage(new Uri(imagePath));
                ImageSource = defaultBitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading default image: " + ex.Message);
            }
        }


        private bool IsSlideWithinDateRange(int index)
        {
            DateTime currentDate = DateTime.Now;
            DateTime startDate = slideHeader[0].startDate;
            DateTime endDate = slideHeader[0].endDate;
            return currentDate >= startDate && currentDate <= endDate;
        }

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        private void StartTimer()
        {
            timerSlide = new DispatcherTimer();
            int defaultTime = 5;

            if (slideHeader != null && slideHeader.Count > 0 && slideHeader[0] != null)
            {
                double intervalSeconds = (slideHeader[0].timeNext != null && slideHeader[0].timeNext != 0) ? slideHeader[0].timeNext : defaultTime;
                timerSlide.Interval = TimeSpan.FromSeconds(intervalSeconds);
            }
            else
            {
                timerSlide.Interval = TimeSpan.FromSeconds(defaultTime);
            }

            timerSlide.Tick += Timer_Tick_Slides;
            timerSlide.Start();
        }

        private void Timer_Tick_Slides(object sender, EventArgs e)
        {
            if (slideDetails == null || slideDetails.Count == 0)
            {
                return;
            }
            currentIndex++;
            if (currentIndex >= slideDetails.Count)
            {
                currentIndex = 0;
            }
            ShowSlide();
        }

        private void StopTimer()
        {
            if (timerSlide != null)
            {
                timerSlide.Stop();
                timerSlide = null;
            }
        }
    }
}
