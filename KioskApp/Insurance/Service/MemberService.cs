using RestSharp;
using System;
using System.IO;

namespace Insurance.Service
{
    public class MemberService
    {
        private readonly RestClient _restClient;
        private readonly string _apiBaseUrl = "https://localhost:7017/"; 

        public MemberService()
        {
            _restClient = new RestClient(_apiBaseUrl);
        }

        public void UploadOcrFileToApi(string ocrFilePath)
        {
            try
            {
		//string fileName = Path.GetFileNameWithoutExtension(ocrFilePath);
                string apiEndpoint = $"/api/Member/GetMemberInformationFromScanner2";

                var request = new RestRequest(apiEndpoint, Method.Post);
                request.AlwaysMultipartFormData = true;
                //fileName = fileName.Substring(0, 4);
                request.AddFile("file", Path.GetFullPath(ocrFilePath));

                RestResponse response = _restClient.Execute(request);

                if (response.IsSuccessful)
                {
                    Console.WriteLine("File uploaded successfully. Server response: " + response.Content);
                }
                else
                {
                    Console.WriteLine("Error uploading file. Status Code: " + response.StatusCode);
                    Console.WriteLine("Response Content: " + response.Content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in uploading file: " + ex.Message);
            }
        }
    }
}
