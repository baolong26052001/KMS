using Insurance.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Service
{
    public class OCRFileSave
    {
        public string LastSavedFilePath { get; private set; }

        private MemberService _member;

        public OCRFileSave()
        {
            _member = new MemberService();
        }
        private List<string> OCRPathArray = new List<string>();
        public void SaveOCRResult(string ocrJsonResult)
        {
            // Create a folder named with the current date and time
            var folderName = DateTime.Now.ToString("yyyyMMdd");
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var folderPath = Path.Combine(path, "File\\OCRFile", folderName);
            Directory.CreateDirectory(folderPath);

            string jsonFileName = DateTime.Now.ToString("HHmmss") + @".json";
            string jsonFilePath = Path.Combine(folderPath, jsonFileName);
            OCRPathArray.Add(jsonFilePath);

            // Write the JSON response to the file
            try
            {
                File.WriteAllText(jsonFilePath, ocrJsonResult);



                // Update the LastSavedFilePath property
                //LastSavedFilePath = jsonFilePath;
                LastSavedFilePath = OCRPathArray[0];

                _member.UploadOcrFileToApi(LastSavedFilePath);

            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during file writing
                Console.WriteLine("Error saving OCR result: " + ex.Message);
            }
        }

    }
}