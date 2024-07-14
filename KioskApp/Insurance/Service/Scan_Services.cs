using Emgu.CV.Dnn;
using Emgu.CV.Face;
using Insurance.Model;
using Insurance.Utility;
using Insurance.ViewModel;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Insurance.Service
{
    internal class Scan_Services
    {
        public string LastSavedFilePath { get; private set; }


        public event Action OnProcessComplete;
        public event Action OnOCRProcessComplete;
        private readonly HttpClient _httpClient;
        private string _imagePath;
        private string fileNameImg;
        private string fileNameImgpath;
        private readonly RestClient _restClient;
        private readonly string _apiBaseUrl = "https://localhost:44369/";
        public Scan_Services(string asd)
        {

            //InitializeComponent();
            _httpClient = new HttpClient();
            //uri = @"D:\Loan-Kiosk-full\Loan-Kiosk\KioskApp\Insurance\bin\Debug\net6.0-windows\CCCD\PIC_1146328_00000.jpg ";
            //thay duong dan voa cai asd de chay API 
            //ProcessScannedImage(_imagePath);
            _restClient = new RestClient(_apiBaseUrl);
            ProcessScannedImage(asd);
        }
        //private List<string> imgPathArray = new List<string>();
        //public void ProcessScannedImage(string imagePath)
        //{
        //    try
        //    {
        //        if (UserModel.Instance.TimeCheck == 0) { }
        //        using (var content = new MultipartFormDataContent())
        //        {
        //            string imgPath = Path.GetFullPath(imagePath);
        //            //imgPathArray.Add(imagePath);
        //            UserModel.Instance.ImagePathArray.Add(imgPath);
        //            #region API CALL
        //            var optionss = new RestClientOptions("https://ekyc-test.tcgroup.vn")
        //            {
        //                MaxTimeout = -1,
        //            };
        //            var clienst = new RestClient(optionss);
        //            var requesst = new RestRequest("/home/api/v1/ocr?type=national_id", Method.Post);
        //            requesst.AddHeader("Authorization", "Token 65c813d6d3dbb202899ee94bf0c844d5647b97a6");
        //            requesst.AlwaysMultipartFormData = true;
        //            requesst.AddFile("image", imgPath);
        //            RestResponse responsssse = clienst.Execute(requesst);
        //            #endregion

        //            fileNameImg = Path.GetFileNameWithoutExtension(imagePath);
        //            //fileNameImgpath = Path.GetFullPath(imagePath);

        //            // Continue
        //            if (responsssse.IsSuccessful)
        //            {
        //                // Assuming the response content is the OCR result
        //                string ocrResult = responsssse.Content;
        //                UserModel.Instance.IDCard = UserModel.Instance.ImagePathArray[0];
        //                SaveOCRResult(ocrResult); // Save the OCR result

        //                OnOCRProcessComplete?.Invoke(); // Invoke the OCR process complete event return;
        //            }
        //            else
        //            {
        //                // Handle the exception: back to Welcome Screen

        //            }

        //            // OnProcessComplete?.Invoke();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception: back to Welcome Screen
        //    }
        //}
        public void ProcessScannedImage(string imagePath)
        {
            try
            {
                if (UserModel.Instance.TimeCheck == 0) 
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        string imgPath = Path.GetFullPath(imagePath);
                        //imgPathArray.Add(imagePath);
                        APIHelper.Instance.Log("Info: Get image id card path");
                        UserModel.Instance.ImagePathArray.Add(imgPath);
                        #region API CALL
                        APIHelper.Instance.Log("API: Call API get OCR from image id card");
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
                        APIHelper.Instance.Log("API: return OCR result");
                        fileNameImg = Path.GetFileNameWithoutExtension(imagePath);
                        //fileNameImgpath = Path.GetFullPath(imagePath);

                        // Continue
                        if (responsssse.IsSuccessful)
                        {
                            // Assuming the response content is the OCR result
                            APIHelper.Instance.Log("API: Get OCR Success");
                            string ocrResult = responsssse.Content;
                            UserModel.Instance.IDCard = UserModel.Instance.ImagePathArray[0];
                            SaveOCRResult(ocrResult); // Save the OCR result
                            UserModel.Instance.TimeCheck++;
                            OnOCRProcessComplete?.Invoke(); // Invoke the OCR process complete event return;
                        }
                        else
                        {
                            // Handle the exception: back to Welcome Screen
                            APIHelper.Instance.Log("Error: Error when get OCR");
                            MainWindowVM.Instance.IsBacktoWelcomeVisible = true;
                        }

                        // OnProcessComplete?.Invoke();
                    }
                }
                
            }
            catch (Exception ex)
            {
                // Handle the exception: back to Welcome Screen
                APIHelper.Instance.Log($"Error on Scan service: {ex.Message}");
                MainWindowVM.Instance.IsBacktoWelcomeVisible = true;
            }
        }

        #region not use
        //public void SaveOCRResult(string ocrJsonResult)
        //{
        //    // Create a folder named with the current date and time
        //    var folderName = DateTime.Now.ToString("yyyyMMdd");
        //    //var folderPath = Path.Combine("D:\\C#_WPF\\Loan-Kiosk - Copy\\KioskApp", folderName);
        //    string startUpPAthh = AppDomain.CurrentDomain.BaseDirectory;

        //    var folderPath = Path.Combine(startUpPAthh, @"File\OCR", folderName);

        //    // Ensure the directory exists
        //    if(!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }


        //    // Create a file path for the JSON file within the new directory
        //    string jsonFileName = DateTime.Now.ToString("HHmmss") + ".json";
        //    string jsonFilePath = Path.Combine(folderPath, jsonFileName);
        //    //File.WriteAllText(jsonFilePath, ocrJsonResult);

        //    // Write the JSON response to the file
        //    try
        //    {
        //        File.WriteAllText(jsonFilePath, ocrJsonResult);
        //        Console.WriteLine($"OCR result saved successfully at {jsonFilePath}");


        //        // Update the LastSavedFilePath property
        //        LastSavedFilePath = jsonFilePath;
        //        UserModel.Instance.OcrFile = LastSavedFilePath;
        //        //UploadOcrFileToApi(LastSavedFilePath, fileNameImgpath);

        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors that occurred during file writing
        //        Console.WriteLine("Error saving OCR result: " + ex.Message);
        //    }
        //}
        #endregion
        public void SaveOCRResult(string ocrJsonResult)
        {
            // Create a folder named with the current date and time
            var folderName = DateTime.Now.ToString("yyyyMMdd");
            //var folderPath = Path.Combine("D:\\C#_WPF\\Loan-Kiosk - Copy\\KioskApp", folderName);
            string startUpPAthh = AppDomain.CurrentDomain.BaseDirectory;

            var folderPath = Path.Combine(startUpPAthh, @"File\OCR", folderName);

            // Ensure the directory exists
            if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }


            // Create a file path for the JSON file within the new directory
            string jsonFileName = DateTime.Now.ToString("HHmmss") + ".json";
            string jsonFilePath = Path.Combine(folderPath, jsonFileName);
            UserModel.Instance.OcrPathArray.Add(jsonFilePath);
            //File.WriteAllText(jsonFilePath, ocrJsonResult);

            // Write the JSON response to the file
            try
            {
                File.WriteAllText(jsonFilePath, ocrJsonResult);
                Console.WriteLine($"OCR result saved successfully at {jsonFilePath}");
                APIHelper.Instance.Log($"OCR result saved successfully at {jsonFilePath}");

                // Update the LastSavedFilePath property
                if (!UserModel.Instance.IsImgOCRTemp)
                {
                    if (UserModel.Instance.OcrPathArray.Count > 1)
                    {
                        LastSavedFilePath = UserModel.Instance.OcrPathArray[1];
                    }
                    else
                    {
                        LastSavedFilePath = UserModel.Instance.OcrPathArray[0];
                    }
                    UserModel.Instance.OcrFile = LastSavedFilePath;
                    UserModel.Instance.IsImgOCRTemp = true;
                }
                //UploadOcrFileToApi(LastSavedFilePath, fileNameImgpath);

            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during file writing
                APIHelper.Instance.Log($"Error saving OCR result: {ex.Message}");
            }
        }
    }
}
