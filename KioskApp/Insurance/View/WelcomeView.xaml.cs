using Insurance.ViewModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Insurance.View
{
    public partial class WelcomeView : UserControl
    {
        private List<TslideDetail> slideDetails;
        private int currentSlideIndex;
        private Timer timer;

        public WelcomeView()
        {
            InitializeComponent();
            LoadSlideDetails();
        }
        public class TslideHeader
        {
            public int Id { get; set; }
            public string? Description { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public bool? IsActive { get; set; }
            public int? TimeNext { get; set; }
            public DateTime? DateModified { get; set; }
            public DateTime? DateCreated { get; set; }
        }

        public class TslideDetail
        {
            public int Id { get; set; }
            public string? Description { get; set; }
            public string? TypeContent { get; set; }
            public string? ContentUrl { get; set; }
            public IFormFile? File { get; set; }
            public int? SlideHeaderId { get; set; }
            public DateTime? DateModified { get; set; }
            public DateTime? DateCreated { get; set; }
            public bool? IsActive { get; set; }
        }

        private async Task LoadSlideDetailsAsync(int slideHeaderId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "https://localhost:7017/api/SlideDetail/ShowSlideDetail/" + slideHeaderId;

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        slideDetails = JsonConvert.DeserializeObject<List<TslideDetail>>(content);

                        if (slideDetails.Count > 0)
                        {
                            DescriptionTextBlock.Text = slideDetails[currentSlideIndex].Description;

                            string baseFolderPath = @"D:\RnD Project\TEMP\KMS\KMS-FE\src\images";
                            string relativePath = slideDetails[currentSlideIndex].ContentUrl;
                            string absolutePath = System.IO.Path.Combine(baseFolderPath, relativePath);

                            string fileExtension = System.IO.Path.GetExtension(absolutePath).ToLower();

                            if (IsImageFile(fileExtension))
                            {
                                ImageControl.Source = new BitmapImage(new Uri(absolutePath));
                                ImageControl.Visibility = Visibility.Visible;
                                VideoControl.Visibility = Visibility.Collapsed;
                            }
                            else if (IsVideoFile(fileExtension))
                            {
                                VideoControl.Source = new Uri(absolutePath);
                                ImageControl.Visibility = Visibility.Collapsed;
                                VideoControl.Visibility = Visibility.Visible;

                                VideoControl.MediaEnded += VideoControl_MediaEnded;

                                VideoControl.Play();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private bool IsImageFile(string fileExtension)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return imageExtensions.Contains(fileExtension);
        }

        private bool IsVideoFile(string fileExtension)
        {
            string[] videoExtensions = { ".mp4", ".avi", ".wmv", ".mkv" };
            return videoExtensions.Contains(fileExtension);
        }

        private void SetupTimer()
        {
            // Initialize the timer
            timer = new Timer();
            timer.Interval = 5000; // 5 seconds interval
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void VideoControl_MediaEnded(object sender, RoutedEventArgs e)
        {
            Dispatcher.InvokeAsync(async () => await VideoControl_MediaEndedAsync());
        }

        private async Task VideoControl_MediaEndedAsync()
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                await AdvanceSlideDetails();
                timer.Start();
            });
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                timer.Stop();

                currentSlideIndex++;

                if (currentSlideIndex >= slideDetails.Count)
                {
                    currentSlideIndex = 0;
                }

                await LoadSlideDetailsAsync(slideDetails[currentSlideIndex].SlideHeaderId.GetValueOrDefault());

                if (IsVideoFile(System.IO.Path.GetExtension(slideDetails[currentSlideIndex].ContentUrl)))
                {
                    VideoControl.MediaEnded -= VideoControl_MediaEnded;
                    VideoControl.MediaEnded += VideoControl_MediaEnded;

                    VideoControl.Play();

                    TimeSpan videoDuration = VideoControl.NaturalDuration.TimeSpan;

                    while (VideoControl.Position < videoDuration)
                    {
                        await Task.Delay(500);
                    }

                    await Dispatcher.InvokeAsync(async () =>
                    {
                        await LoadSlideDetailsAsync(slideDetails[currentSlideIndex].SlideHeaderId.GetValueOrDefault());
                    });
                }
                timer.Start();
            });
        }

        private async Task AdvanceSlideDetails()
        {

            if (currentSlideIndex >= slideDetails.Count)
            {
                currentSlideIndex = 0;
            }

            await LoadSlideDetailsAsync(slideDetails[currentSlideIndex].SlideHeaderId.GetValueOrDefault());
        }
        private void LoadSlideDetails()
        {
            // Set default slideHeaderId, in case just set it 2
            int defaultSlideHeaderId = 2;

            _ = LoadSlideDetailsAsync(defaultSlideHeaderId);

            SetupTimer();
        }
    }
}
