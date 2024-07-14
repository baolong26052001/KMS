using Insurance.Service;
using RestSharp;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Insurance.ViewModel
{
    public class API_service
    {
        public event Action OnProcessComplete;
        public event Action OnOCRProcessComplete;
        private readonly HttpClient _httpClient;
        private readonly FileSystemWatcher _watcher;
        //private readonly string _watchPath = @"D:\Loan-Kiosk-full\Loan-Kiosk\KioskApp\Insurance\bin\Debug\net6.0-windows\CCCD\"; // Update with the path
        //private string uri { get; set; } = @"D:\Loan-Kiosk-full\Loan-Kiosk\KioskApp\Insurance\bin\Debug\net6.0-windows\CCCD\PIC_928281_00000.jpg";

        public API_service(string asd)
        {
            //InitializeComponent();
            _httpClient = new HttpClient();
            //uri = @"D:\Loan-Kiosk-full\Loan-Kiosk\KioskApp\Insurance\bin\Debug\net6.0-windows\CCCD\PIC_1146328_00000.jpg ";
            //thay duong dan voa cai asd de chay API 
            ProcessScannedImage(asd);

        }

       

        public void ProcessScannedImage(string imagePath)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    
                    string imgPath = Path.GetFullPath(imagePath);
                    #region API CALL
                    var optionss = new RestClientOptions("https://ekyc-test.tcgroup.vn")
                    {
                        MaxTimeout = -1,
                    };
                    var clienst = new RestClient(optionss);
                    var requesst = new RestRequest("/home/api/v1/ocr?type=national_id", Method.Post);
                    requesst.AddHeader("Authorization", "Token 65c813d6d3dbb202899ee94bf0c844d5647b97a6");
                    requesst.AlwaysMultipartFormData = true;
                    requesst.AddFile("image", imgPath);
                    RestResponse responsssse = clienst.Execute(requesst);
                    #endregion

                    //string fileName = Path.GetFileNameWithoutExtension(imagePath);

                    //Continue
                    if (responsssse.IsSuccessful)
                    {
                        // Assuming the response content is the OCR result
                        string ocrResult = responsssse.Content;

                        var ocrFileSave = new OCRFileSave();
                        ocrFileSave.SaveOCRResult(ocrResult); // Save the OCR result

                        OnOCRProcessComplete?.Invoke(); // Invoke the OCR process complete event
                    }
                    else
                    {
                        // Handle the case where the API response is not successful
                        MessageBox.Show("Error: API response was not successful.");
                    }

                    //OnProcessComplete?.Invoke();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing the scanned image: {ex.Message}");
            }
        }




    }
}