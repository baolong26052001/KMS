using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Insurance.Command;
using Insurance.Model;
using Insurance.Model.HTTPResponses;
using Insurance.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;
using static Insurance.Utility.APIHelper;

namespace Insurance.ViewModel
{
    // 0: Login; 1:Signup
    public class VerifyFaceIDVM : ViewModelBase
    {
        private VideoCapture _capture;
        private CascadeClassifier faceCascade;
        private DispatcherTimer captureTimer;
        private SolidColorBrush defaultBorderColor = new SolidColorBrush(Colors.Red); // Default border color
        private bool faceLivePassed = false;
        private bool faceMatchPassed = false;
        private readonly int countResultPassed = 0;
        private int folderNumber;
        private string generatedFolderPath;
        private string lastSavedDate;
        private int imageCounter = 0;
        private ImageSource imageSource;
        private FaceIDResponse faceIDResponse = new FaceIDResponse();
        private bool isLogin = false;
        private System.Windows.Controls.Image imageControl;
        public HomeVM HomeViewModel => HomeVM.Instance;
        public System.Windows.Controls.Image ImageControl
        {
            get { return imageControl; }
            set
            {
                if (imageControl != value)
                {
                    imageControl = value;
                    OnPropertyChanged(nameof(ImageControl));
                }
            }
        }
        private Visibility _loadingOverlayVisibility = Visibility.Collapsed;

        public Visibility LoadingOverlayVisibility
        {
            get { return _loadingOverlayVisibility; }
            set
            {
                if (_loadingOverlayVisibility != value)
                {
                    _loadingOverlayVisibility = value;
                    OnPropertyChanged(nameof(LoadingOverlayVisibility));
                }
            }
        }
        public SolidColorBrush DefaultBorderColor
        {
            get { return defaultBorderColor; }
            set { defaultBorderColor = value; }
        }

        private Border _cameraArea;
        public Border CameraArea
        {
            get { return _cameraArea; }
            set
            {
                if (_cameraArea != value)
                {
                    _cameraArea = value;
                    OnPropertyChanged(nameof(CameraArea));
                }
            }
        }


        private string _apiResultText;
        public string ApiResultText
        {
            get { return _apiResultText; }
            set
            {
                if (_apiResultText != value)
                {
                    _apiResultText = value;
                    OnPropertyChanged(nameof(ApiResultText));
                }
            }
        }

        private string _apiResultTextBlock_1;
        public string ApiResultTextBlock_1
        {
            get { return _apiResultTextBlock_1; }
            set
            {
                if (_apiResultTextBlock_1 != value)
                {
                    _apiResultTextBlock_1 = value;
                    OnPropertyChanged(nameof(ApiResultTextBlock_1));
                }
            }
        }
        private Visibility _backToWelcomeVisibility = Visibility.Collapsed;
        public Visibility BackToWelcomeVisibility
        {
            get { return _backToWelcomeVisibility; }
            set
            {
                if (_backToWelcomeVisibility != value)
                {
                    _backToWelcomeVisibility = value;
                    OnPropertyChanged(nameof(BackToWelcomeVisibility));
                }
            }
        }
        private Visibility _tryAgainVisibility = Visibility.Collapsed;
        public Visibility TryAgainVisibility
        {
            get { return _tryAgainVisibility; }
            set
            {
                if (_tryAgainVisibility != value)
                {
                    _tryAgainVisibility = value;
                    OnPropertyChanged(nameof(TryAgainVisibility));
                }
            }
        }


        private Visibility _successVisibility = Visibility.Collapsed;
        public Visibility SuccessVisibility
        {
            get { return _successVisibility; }
            set
            {
                if (_successVisibility != value)
                {
                    _successVisibility = value;
                    OnPropertyChanged(nameof(SuccessVisibility));
                }
            }
        }

        private Visibility _failureVisibility = Visibility.Collapsed;
        public Visibility FailureVisibility
        {
            get { return _failureVisibility; }
            set
            {
                if (_failureVisibility != value)
                {
                    _failureVisibility = value;
                    OnPropertyChanged(nameof(FailureVisibility));
                }
            }
        }
        private Visibility _existAccVisibility = Visibility.Collapsed;
        public Visibility ExistAccVisibility
        {
            get { return _existAccVisibility; }
            set
            {
                if (_existAccVisibility != value)
                {
                    _existAccVisibility = value;
                    OnPropertyChanged(nameof(ExistAccVisibility));
                }
            }
        }
        private Visibility _apiResultTextBlock_1Visibility = Visibility.Collapsed;

        public Visibility ApiResultTextBlock_1Visibility
        {
            get { return _apiResultTextBlock_1Visibility; }
            set
            {
                if (_apiResultTextBlock_1Visibility != value)
                {
                    _apiResultTextBlock_1Visibility = value;
                    OnPropertyChanged(nameof(ApiResultTextBlock_1Visibility));
                }
            }
        }
        public new event PropertyChangedEventHandler? PropertyChanged;

        public new void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
        public ICommand TryAgainCommand => new RelayCommand(TryAgain);
        public ICommand BackToWelcomeCommand => new RelayCommand(BackToWelcome);
        public ICommand SignUpCommand => new RelayCommand(SignUp);
        private void UpdateUI(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
        private string folderPath;

        public VerifyFaceIDVM(int style_view)
        {
            if (style_view == 0) UserModel.Instance.Style = 0;
            else UserModel.Instance.Style = style_view;
            string targetCameraName = KioskModel.Instance.CameraName;
            //string targetCameraName = "USB2.0 HD UVC WebCam";
            List<DsDevice> videoDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
            int targetCameraIndex = videoDevices.FindIndex(device => device.Name == targetCameraName);
            if (targetCameraIndex >= 0)
            {
                // Initialize _capture with the specified camera index
                _capture = new VideoCapture(targetCameraIndex);
                KioskModel.Instance.CameraStatus = 1; // Camera Status: Online
            }
            else
            {
                KioskModel.Instance.CameraStatus = 0; // Camera Status: Offline
                MessageBox.Show($"Target camera '{targetCameraName}' not found.");
            }
            APIHelper.Instance.UpdateCamStatus(KioskModel.Instance.KioskID, KioskModel.Instance.CameraStatus); // update camera status
            faceCascade = new CascadeClassifier(); // Initialize captureTimer
            imageControl = new System.Windows.Controls.Image(); // Initialize imageControl
            _cameraArea = new Border(); // Initialize cameraArea (replace with your actual initialization)
            _apiResultText = string.Empty; // Initialize _apiResultText
            _apiResultTextBlock_1 = string.Empty; // Initialize _apiResultTextBlock_1
            folderPath = GenerateFolderName();
            Initialize();

        }
        private int count;
        private bool IsexistAcc;
        private void FaceIDResponse_CaptureFinish()
        {
            if (imageCounter >= 1)
            {
                captureTimer.Tick -= CaptureImage;
            }

            switch (UserModel.Instance.Style)
            {
                case 0:
                    { //login
                        faceLivePassed = true;
                        string fileName = $"face_{0}.jpeg";
                        string filePath = Path.Combine(folderPath, fileName);

                        List<PersonalInforShow> faceIdResults = APIHelper.Instance.FaceID_Find(filePath);

                        if (faceIdResults != null && faceIdResults.Count > 0)
                        {
                            PersonalInforShow firstResult = faceIdResults[0];
                            UserModel.Instance.FullName = firstResult.FullName;
                            UserModel.Instance.UserID = firstResult.Id;
                        }
                        ShowResult(faceLivePassed, UserModel.Instance.FaceMatchPassed);
                    };
                    break;
                case 1:
                    {
                        int result;
                        faceLivePassed = true;
                        string fileName = $"face_{0}.jpeg";
                        string faceCapture = Path.Combine(folderPath, fileName);
                        string imageIDCard = UserModel.Instance.IDCard;
                        string ocrFile = UserModel.Instance.OcrFile;
                        result = APIHelper.Instance.FaceID_Match(faceCapture, imageIDCard);
                        if (result == 200)
                        {
                            faceMatchPassed = true;
                            result = APIHelper.Instance.FaceID_Create_User(ocrFile, imageIDCard);                           
                            if (result == 400)
                            {
                                IsexistAcc = true;
                            }
                            UserModel.Instance.TimeCheck = 0;
                        }
                        ShowResult_SU(faceLivePassed, faceMatchPassed, IsexistAcc);
                    }
                    break;
            }
            Application.Current.Dispatcher.Invoke(() => HideLoadingOverlay());
        }
        //private bool tryAgainCheck = false;
        private void TryAgain(object parameter)
        {
            imageSource = null;
            _capture.ImageGrabbed -= ProcessFrame;
            FailureVisibility = Visibility.Collapsed;
            TryAgainVisibility = Visibility.Collapsed;
            imageCounter = 0;
            captureTimer = null;
            InitializeWebcam();
        }

        private void Initialize()
        {
            string cascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_alt2.xml");
            faceCascade = new CascadeClassifier(cascadePath);
            InitializeWebcam();
            defaultBorderColor = new SolidColorBrush(Colors.Red);
        }

        private void InitializeWebcam()
        {
            try
            {
                _capture.ImageGrabbed += ProcessFrame; // error when camera not found
                _capture.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing webcam: {ex.Message}");
            }
        }

        private ImageSource ConvertToBitmapSource(Mat image)
        {
            using (var bitmap = image.ToBitmap())
            {
                var bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    bitmap.PixelFormat);

                var bitmapSource = BitmapSource.Create(
                    bitmapData.Width, bitmapData.Height,
                    bitmap.HorizontalResolution, bitmap.VerticalResolution,
                    PixelFormats.Bgr24, null,
                    bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

                bitmap.UnlockBits(bitmapData);

                return bitmapSource;
            }
        }


        private void SignUp(object parameter)
        {

            MainWindowVM.Instance.CurrentView = new VerifyIDVM();
            _capture.Dispose();
        }

        private void ProcessFrame(object? sender, EventArgs e)
        {
            Mat frame = new Mat();
            _capture?.Retrieve(frame);

            if (frame != null)
            {
                Mat gray = new Mat();
                CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);
                Rectangle[] faces = faceCascade.DetectMultiScale(gray.ToImage<Gray, byte>(), 1.1, 3);
                UpdateUI(() =>
                {
                    if (faces.Length == 1)
                    {

                        CameraArea.BorderBrush = new SolidColorBrush(Colors.Green); // Change border color
                        ImageControl.Source = ConvertToBitmapSource(frame);
                    }
                    else
                    {

                        CameraArea.BorderBrush = DefaultBorderColor;// Change border color
                        ImageControl.Source = ConvertToBitmapSource(frame);
                    }
                });
                // Delay for 1 second before capturing
                Task.Delay(1000).ContinueWith(_ =>
                {
                    UpdateUI(() =>
                    {
                        Mat faceRegion = new Mat();
                        if (faces.Length == 1 && imageCounter < 1)
                        {

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            Rectangle firstFace = faces[0];

                            int expansionFactor = 20;
                            int expandedX = Math.Max(0, firstFace.X - (int)(firstFace.Width * (expansionFactor - 1) / 2));
                            int expandedY = Math.Max(0, firstFace.Y - (int)(firstFace.Height * (expansionFactor - 1) / 2));
                            int expandedWidth = Math.Min(frame.Width - expandedX, (int)(firstFace.Width * expansionFactor));
                            int expandedHeight = Math.Min(frame.Height - expandedY, (int)(firstFace.Height * expansionFactor));

                            Rectangle expandedFace = new Rectangle(expandedX, expandedY, expandedWidth, expandedHeight);
                            faceRegion = new Mat(frame, expandedFace);

                            // Save the captured image in the current folder
                            string fileName = $"face_{imageCounter}.jpeg";
                            string filePath = Path.Combine(folderPath, fileName);
                            CvInvoke.Imwrite(filePath, faceRegion);
                            imageCounter++;

                        }
                        if (imageCounter == 1)
                        {
                            if (!faceRegion.IsEmpty) UpdateUI(() => { ImageControl.Source = ConvertToBitmapSource(faceRegion); });

                            if (captureTimer == null)
                            {
                                captureTimer = new DispatcherTimer();
                                captureTimer.Interval = TimeSpan.FromSeconds(0.4);
                                captureTimer.Tick += CaptureImage;
                                captureTimer.Start();
                                ShowLoadingOverlay();
                                if (UserModel.Instance.Style == 0) { StopCamera(); }
                                else { _capture.Stop(); }
                            }
                        }

                    });
                });
            }

        }

        private async void CaptureImage(object? sender, EventArgs e)
        {
            try
            {
                string pathImage = Path.GetFullPath(folderPath);
            }
            catch
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                captureTimer.Tick -= CaptureImage;
                captureTimer.Stop();
                imageCounter = 0;
            }
            if (imageCounter == 1)
            {
                await Task.Run(() =>
                {
                    ShowLoadingOverlay();
                    FaceIDResponse_CaptureFinish();
                });
                HideLoadingOverlay();
            }
            captureTimer.Stop();
        }

        private string GenerateFolderName()
        {
            // Read folderNumber and lastSavedDate from XML before capturing images
            ReadFolderNumberAndLastSavedDateFromXml();

            // Get the current date
            string currentDate = DateTime.Now.ToString("yyyyMMdd");

            // Check if the current date is different from the last saved date
            if (lastSavedDate != currentDate)
            {
                // Reset folderNumber to 0 for a new date
                folderNumber = 0;
                SaveFolderNumberAndLastSavedDateToXml(currentDate);
            }
            switch (UserModel.Instance.Style)
            {
                case 0:
                    {
                        // Create a new folder path based on the current date and folderNumber
                        generatedFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image\\FaceID\\LogIn", $"{currentDate}_{folderNumber}");

                        // Check if the folder already exists
                        if (Directory.Exists(generatedFolderPath))
                        {
                            // Increment folderNumber and update the XML file
                            folderNumber++;
                            SaveFolderNumberAndLastSavedDateToXml(currentDate);
                            generatedFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image\\FaceID\\LogIn", $"{currentDate}_{folderNumber}");
                        }
                    }
                    break;
                case 1:
                    {
                        // Create a new folder path based on the current date and folderNumber
                        generatedFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image\\FaceID\\SignUp", $"{currentDate}_{folderNumber}");

                        // Check if the folder already exists
                        if (Directory.Exists(generatedFolderPath))
                        {
                            // Increment folderNumber and update the XML file
                            folderNumber++;
                            SaveFolderNumberAndLastSavedDateToXml(currentDate);
                            generatedFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image\\FaceID\\SignUp", $"{currentDate}_{folderNumber}");
                        }
                    }
                    break;
            }

            return generatedFolderPath;
        }

        private void ReadFolderNumberAndLastSavedDateFromXml()
        {
            try
            {
                switch (UserModel.Instance.Style)
                {
                    case 0:
                        {
                            XDocument doc = XDocument.Load("FolderNumber.xml");
                            XElement? folderNumberElement = doc.Root?.Element("FolderNumber");
                            XElement? lastSavedDateElement = doc.Root?.Element("LastSavedDate");

                            if (folderNumberElement != null && int.TryParse(folderNumberElement.Value, out int parsedFolderNumber))
                            {
                                folderNumber = parsedFolderNumber;
                            }

                            lastSavedDate = lastSavedDateElement?.Value ?? "";
                        }
                        break;
                    case 1:
                        {
                            XDocument doc = XDocument.Load("FolderNumber_SU.xml"); //SU = SingUp
                            XElement? folderNumberElement = doc.Root?.Element("FolderNumber");
                            XElement? lastSavedDateElement = doc.Root?.Element("LastSavedDate");

                            if (folderNumberElement != null && int.TryParse(folderNumberElement.Value, out int parsedFolderNumber))
                            {
                                folderNumber = parsedFolderNumber;
                            }

                            lastSavedDate = lastSavedDateElement?.Value ?? "";
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading FolderNumber and LastSavedDate from XML: {ex.Message}");
                // Handle the exception as needed
            }
        }

        private void SaveFolderNumberAndLastSavedDateToXml(string currentDate)
        {
            try
            {
                switch (UserModel.Instance.Style)
                {
                    case 0:
                        {
                            XDocument doc = new XDocument(
                            new XElement("Settings",
                            new XElement("FolderNumber", folderNumber),
                            new XElement("LastSavedDate", currentDate)
                        )
                    );

                            doc.Save("FolderNumber.xml");
                        }
                        break;
                    case 1:
                        {
                            XDocument doc = new XDocument(
                            new XElement("Settings",
                            new XElement("FolderNumber", folderNumber),
                            new XElement("LastSavedDate", currentDate)
                        )
                    );

                            doc.Save("FolderNumber_SU.xml");
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving FolderNumber and LastSavedDate to XML: {ex.Message}");
            }
        }

        private void ShowLoadingOverlay()
        {
            LoadingOverlayVisibility = Visibility.Visible;
            TryAgainVisibility = Visibility.Collapsed;
            MainWindowVM.Instance.LoadingOverlayVisibility_Main = Visibility.Visible;
            //captureTimer.Stop();
        }

        private void HideLoadingOverlay()
        {
            LoadingOverlayVisibility = Visibility.Collapsed;
            MainWindowVM.Instance.LoadingOverlayVisibility_Main = Visibility.Collapsed;
        }

        private void StopCamera()
        {
            if (_capture != null && _capture.IsOpened)
            {
                _capture.Stop();
                _capture.Dispose();
            }
        }
        private bool _resultShown = false;

        private void ShowResult(bool faceLivePassed, bool faceMatchPassed)
        {
            if (_resultShown) return;
            _resultShown = true;

            HideLoadingOverlay();
            if (faceLivePassed && faceMatchPassed)
            {
                ShowSuccessResult();
                UserModel.Instance.IsLogin = true;
                UserModel.Instance.IsHomePage = true;
                _capture.Stop();
                _capture.Dispose();

                EnsureSingleNavigationToHome();
            }
            else
            {
                _capture.Stop();
                _capture.Dispose();
                ShowFailureResult();

                EnsureSingleNavigationFailure();
            }
        }

        private void EnsureSingleNavigationToHome()
        {
            if (KioskModel.Instance.LoginCheck == 0)
            {
                KioskModel.Instance.LoginCheck++;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _cameraArea.BorderBrush = null;
                    MainWindowVM.Instance.CurrentView = HomeVM.Instance;
                    MainWindowVM.Instance.VisibilityBtnHead = Visibility.Visible;
                });
            }
        }

        private void EnsureSingleNavigationFailure()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _cameraArea.BorderBrush = null;
                MainWindowVM.Instance.CurrentView = new VerifyIDVM();
                MainWindowVM.Instance.VisibilityBtnHead = Visibility.Visible;

            });
        }

        private void ShowSuccessResult()
        {
            SuccessVisibility = Visibility.Visible;
        }

        private void ShowFailureResult()
        {
            FailureVisibility = Visibility.Visible;
        }

        private void ShowResult_SU(bool faceLivePassed, bool faceMatchPassed, bool IsexistAcc)
        {
            // Hide the loading overlay
            HideLoadingOverlay();

            if (faceLivePassed && faceMatchPassed && !IsexistAcc)
            {

                ShowSuccessResult_SU();
                Thread.Sleep(1000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _cameraArea.BorderBrush = null;
                    MainWindowVM.Instance.CurrentView = PersonalInfoVM.Instance;
                });
                _capture.Stop();
                _capture.Dispose();
            }
            else if (faceLivePassed && faceMatchPassed && IsexistAcc)
            {
                ShowExistAccResult_SU();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _cameraArea.BorderBrush = null;
                });
                _capture.Stop();
                _capture.Dispose();
                MainWindowVM.Instance.VisibilityBtnHead = Visibility.Collapsed;
            }
            else if (faceLivePassed && !faceMatchPassed && IsexistAcc)
            {
                ShowFailureResult_SU();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _cameraArea.BorderBrush = null;
                });
                _capture.Stop();
                MainWindowVM.Instance.VisibilityBtnHead = Visibility.Collapsed;
            }
            else
            {
                ShowFailureResult_SU();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _cameraArea.BorderBrush = null;
                });
            }
        }
        private void ShowSuccessResult_SU()
        {
            SuccessVisibility = Visibility.Visible;
            TryAgainVisibility = Visibility.Collapsed;

        }
        private void ShowFailureResult_SU()
        {
            FailureVisibility = Visibility.Visible;
            TryAgainVisibility = Visibility.Visible;
        }
        private void ShowExistAccResult_SU()
        {
            BackToWelcomeVisibility = Visibility.Visible;
            ExistAccVisibility = Visibility.Visible;
        }
        private void BackToWelcome(object parameter)
        {
            UserModel.Instance.ClearData();
            MainWindowVM.Instance.DeactiveCountDown();
            MainWindowVM.Instance.UserGuideVisible = Visibility.Visible;
            //HomeVM.Instance.CleardataHome();
            MainWindowVM.Instance.CurrentView = new WelcomeVM();
            _capture.Dispose();
        }
    }
}
