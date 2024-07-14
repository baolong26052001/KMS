using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Insurance.Model.HTTPResponses;
using Insurance.Model;
using Newtonsoft.Json.Linq;
using Insurance.ViewModel;
using static Emgu.CV.VideoCapture;
using System.Collections.Immutable;
using System.IO;
using System.Xml.Linq;
using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Emgu.CV.Dnn;
using Emgu.CV.Face;
using Emgu.CV;
using System.Diagnostics;
using static Insurance.Utility.APIHelper;

namespace Insurance.Utility
{
    public class APIHelper
    {
        private static APIHelper instance;
        public static APIHelper Instance
        {
            get { if (instance == null) instance = new APIHelper(); return instance; }
            private set { instance = value; }
        }
        private string host;
        private APIHelper()
        {
            //this.host = "http://192.168.1.85:88";
            this.host = $"{KioskModel.Instance.APIPort}";
            //this.host = "https://localhost:7017";
        }
        //private string portAPI = "http://192.168.1.85:88";
        private string portAPI = $"{KioskModel.Instance.APIPort}";


        //Fetch User's Name, Age, Age RangeID example: Nguyen A, 31, 2
        public List<UserInfoModel> FetchUser(int id)
        {
            List<UserInfoModel> models = null;

            string url = "/api/AgeRange/CheckAgeRange";
            url += @"/" + id;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<UserInfoModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when check age: {ex.Message}");
                    }

                }
            }
            return models;
        }

        //Fetch User's Name, Id, Address, Occupation, Email,...
        public List<UserInfoModel> FetchUserInfo(int id)
        {
            List<UserInfoModel> models = null;
            string url = "/api/Member/ShowMember";
            url += @"/" + id;
            Log($"API call: Show Member");
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<UserInfoModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when show member: {ex.Message}");
                    }

                }
            }
            return models;
        }

        //Fetch insurance Provider example: Bao Viet, FWD
        public List<InsuranceProviderModel> FetchInsuranceProvider()
        {
            List<InsuranceProviderModel> models = null;
            Log("Info: Show insurance provider.");
            string url = "/api/InsuranceProvider/ShowInsuranceProvider";
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    //try
                    //{
                    //    models = JsonConvert.DeserializeObject<List<InsuranceProviderModel>>(jToken.ToString());
                    //}
                    try
                    {
                        Log("Info (API return): return provider.");
                        models = JsonConvert.DeserializeObject<List<InsuranceProviderModel>>(jToken.ToString());
                        foreach (var item in models)
                        {
                            item.ProviderImageBitmap = ConvertToBitmapImage(item.providerImage);
                        }

                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when show insurance provider: {ex.Message}");
                    }

                }
            }
            return models;
        }

        private BitmapImage ConvertToBitmapImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return null;
            }

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(Convert.FromBase64String(base64String));
            bitmapImage.EndInit();

            return bitmapImage;
        }

        //Fetch insurance Type example: An Gia, An Binh Yen Vui
        public List<InsuranceTypeModel> FetchInsuranceType()
        {
            List<InsuranceTypeModel> models = null;

            string url = "/api/InsuranceType/ShowInsuranceType";
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<InsuranceTypeModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when show insurance type: {ex.Message}");
                    }

                }
            }
            return models;
        }

        //Fetch An Gia Hanh Phuc Term: Term A or Term B
        public List<InsuranceTermModel> FetchInsuranceTerm()
        {
            List<InsuranceTermModel> models = null;

            string url = "/api/Term/ShowTerm";
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<InsuranceTermModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when show term: {ex.Message}");
                    }

                }
            }
            return models;
        }


        //Fetch Insurance package, example: Ten goi cua An Binh Yen Vui
        public List<InsurancePackageModel> FetchInsurancePackage(int id)
        {
            List<InsurancePackageModel> models = null;
            Log("Info: Show insurance package.");
            string url = "/api/InsurancePackageHeader/ShowInsurancePackageHeaderByInsuranceType";
            url += @"/" + id;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            Log("Info: receive package from API.");
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                //List<MemberModels> models = null;
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<InsurancePackageModel>>(jToken.ToString());
                        foreach (var item in models)
                        {
                            item.PackageFee = FetchAnBinhPackage(item.id);//item.id
                            if (item.PackageFee != null)
                            {
                                item.fee = item.PackageFee[0].fee;
                                // Hoan doi vi tri headerID va Id de khop voi db.
                                int n = item.id;
                                item.headerId = n;
                                item.id = item.PackageFee[0].id;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Log($"Error when fetch Insurance package: {ex.Message}");
                        models = null;
                    }

                }
            }
            return models;
        }

        //Fetch Gia goi cua An Binh Yen Vui. Note: An Binh Yen Vui lay ten goi va gia goi o 2 API khac nhau
        public List<InsurancePackageFee> FetchAnBinhPackage(int id)
        {
            List<InsurancePackageFee> models = null;
            string url = "/api/InsurancePackageDetail/ShowInsurancePackageDetailByHeaderId";
            url += @"/" + id;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                //List<MemberModels> models = null;
                if (jToken != null)
                {
                    //FileHelper.Instance.WriteLog("[Read JToken]", path);

                    try
                    {
                        models = JsonConvert.DeserializeObject<List<InsurancePackageFee>>(jToken.ToString());

                    }
                    catch (Exception ex)
                    {
                        Log($"Error when fetch package details: {ex.Message}");
                        models = null;
                    }

                }
            }
            return models;
        }

        public List<InsurancePackageModel> FetchAnGiaPackage(int id, int term)
        {
            List<InsurancePackageModel> models = null;

            string url = "/api/InsurancePackageDetail/ShowInsurancePackageDetailByAgeRangeIdAndTermId";
            url += @"/" + id + @"/" + term;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                //List<MemberModels> models = null;
                if (jToken != null)
                {
                    //FileHelper.Instance.WriteLog("[Read JToken]", path);

                    try
                    {
                        models = JsonConvert.DeserializeObject<List<InsurancePackageModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when Fetch Detail package va Fee: {ex.Message}");
                    }

                }
            }
            return models;
        }

        private bool IsNumber(string text)
        {
            text = text.Replace(".", "");
            return double.TryParse(text, out _);
        }
        public List<InsurancePackageDetailModel> FetchInsurancePackageDetailForKioskApp(int packageId)
        {
            List<InsurancePackageDetailModel> models = null;
            string url = $"/api/InsurancePackage/ShowBenefitForKioskApp/{packageId}";
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                Log($"Error response received: {response.RData}");
                return models;
            }

            if (response.RData != null)
            {
                try
                {
                    models = JsonConvert.DeserializeObject<List<InsurancePackageDetailModel>>(response.RData.ToString());
                    foreach (var header in models)
                    {
                        foreach (var detail in header.details)
                        {
                            detail.IsCoverageNumeric = IsNumber(detail.coverageDetail);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"Error during deserialization: {ex.Message}");
                    models = null;
                }
            }

            return models;
        }

        public int UpdateUserInformation(int id)
        {
            int result = 0;
            Log("Info: Update user information");
            string url = "/api/Member/UpdateMember";
            url += @"/" + id;
            RestRequest request = new RestRequest(url, Method.Put);

            // Declare the body variable outside of the if block
            string body = string.Empty;

            var userInformation = UserModel.Instance;
            if (userInformation != null)
            {
                // Serialize the last beneficiary
                body = JsonConvert.SerializeObject(userInformation);
            }

            // Add the body to the request
            request.AddStringBody(body, RestSharp.DataFormat.Json);
            Log("API call: Update user information");
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var aa = response.Message;

                Log($"API: error when Update User information {response.Message}");
                result = 0;
            }
            else if (response.Status == 200)
            {
                Log($"API: Update User information success");
                result = 200;
            }

            return result;
        }


        public int InsuranceTransaction()
        {
            int result = 0;
            Log($"Info: Insurance Transaction");
            string url = @"/api/InsuranceTransaction/SaveInsuranceTransaction";
            RestRequest request = new RestRequest(url, Method.Post);
            string body = JsonConvert.SerializeObject(UserModel.Instance.InsuranceTransaction);
            request.AddStringBody(body, RestSharp.DataFormat.Json);
            Log($"API: Save buyer and insurance package information");
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var aa = response.Message;
                Log($"API error when save insurance transaction: {response.Message}");
                result = 0;
            }
            else if (response.Status == 200)
            {
                result = 200;
                Log($"API: Insurance transaction success");
                JToken resultToken = JToken.Parse(response.RData.ToString());
                string contractID = "";
                contractID = resultToken.SelectToken("contractId").ToString();
                UserModel.Instance.ContractId = contractID;
            }

            return result;
        }

        public int InsuranceBeneficiary()
        {
            int result = 0;
            Log($"Info: Save beneficiary");
            string url = @"/api/InsuranceTransaction/SaveBeneficiary";
            RestRequest request = new RestRequest(url, Method.Post);
            Log($"API: Send information to server");
            string body = string.Empty;

            var lastBeneficiary = UserModel.Instance.InsuranceBeneficiary?.LastOrDefault();
            if (lastBeneficiary != null)
            {
                // Serialize the last beneficiary
                body = JsonConvert.SerializeObject(lastBeneficiary);
            }

            // Add the body to the request
            request.AddStringBody(body, RestSharp.DataFormat.Json);

            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var aa = response.Message;
                Log($"API: error when save beneficiary {response.Message}");
                result = 0;
            }
            else if (response.Status == 200)
            {
                Log($"API: Save beneficiary success");
                result = 200;
            }

            return result;
        }

        //----------------------------------SAVING------------------------------------
        public int SavingTransaction()
        {
            int result = 0;

            string url = @"/api/SavingTransaction/SaveSavingTransaction";
            RestRequest request = new RestRequest(url, Method.Post);
            string body = JsonConvert.SerializeObject(UserModel.Instance.SavingTransaction);
            request.AddStringBody(body, RestSharp.DataFormat.Json);

            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var aa = response.Message;
                result = 0;
            }
            else if (response.Status == 200)
            {
                result = 200;
            }

            return result;
        }
        public List<SavingTransactionModel> FetchSavingHeader(int memberId,int status)
        {
            List<SavingTransactionModel> models = null;

            string url = @"/api/SavingTransaction/ShowSavingTransactionByMemberIdAndStatus";
            url += "/" + memberId + "/" + status;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<SavingTransactionModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                    }
                }
            }
            return models;
        }
        public List<SavingTransactionModel> FetchSavingDetail(int memberId, int status, int savingId)
        {
            List<SavingTransactionModel> models = null;

            string url = @"/api/SavingTransaction/ShowSavingTransactionByMemberIdAndStatusAndSavingId";
            url += "/" + memberId + "/" + status + "/" + savingId;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<SavingTransactionModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                    }
                }
            }
            return models;
        }
        public List<SavingTransactionModel> FetchSavingDetailbsavingId(int savingId)
        {
            List<SavingTransactionModel> models = null;

            string url = @"/api/SavingTransaction/ShowWithdrawBySavingId";
            url += "/" + savingId;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<SavingTransactionModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                    }
                }
            }
            return models;
        }
        public List<SavingTransactionModel> FetchSavingHistoryHeader(int memberId)
        {
            List<SavingTransactionModel> models = null;

            string url = @"/api/SavingTransaction/ShowSavingTransactionByMemberId";
            url += "/" + memberId;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<SavingTransactionModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                    }
                }
            }
            return models;
        }

        //-----------------------------Payback Loan---------------------------------------

        //Fetch Loans, example: Fetch các khoản vay 
        public List<LoanPaybackModel> FetchLoans(int id, int status)
        {
            List<LoanPaybackModel> models = null;

            string url = "/api/LoanTransaction/ShowLoanTransactionByMemberIdAndStatus";
            url += @"/" + id + @"/" + status;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                //List<MemberModels> models = null;
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<LoanPaybackModel>>(jToken.ToString());
                        
                        int i = 1;
                        foreach (var item in models)
                        {
                            item.LoanPackageName = $"Khoản vay {i}";
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        models = null;
                    }

                }
            }
            return models;
        }

        //Fetch Loans, example: Fetch các khoản vay 
        public List<LoanPaybackModel> FetchLoanDetails(int id)
        {
            List<LoanPaybackModel> models = null;

            string url = "/api/LoanTransaction/ShowLoanTransaction";
            url += @"/" + id;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                //List<MemberModels> models = null;
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<LoanPaybackModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                    }

                }
            }
            return models;
        }


        public int FaceID_Match(string enroll, string verify)
        {
            int result = 0;
            Log("Start compare image ID card with captured face");
            try
            {
                XDocument appconfig = XDocument.Load("AppConfig.xml");
                if (appconfig == null) return result;
                XElement username = appconfig.Descendants("ApiFaceIDUsername").FirstOrDefault();
                if (username == null) return result;
                string url = @"face/match/" + username.Value;
                RestRequest request = new RestRequest(url, Method.Post);

                string enrollPath = Path.GetFullPath(enroll);
                string verifyPath = Path.GetFullPath(verify);

                // Check if file paths are not null or empty
                if (string.IsNullOrEmpty(enrollPath) || string.IsNullOrEmpty(verifyPath))
                {
                    Log("Error: File path is null or empty.");
                    return result; // Return error code or handle as needed
                }

                request.AddFile("enroll", enrollPath);
                request.AddFile("verif", verifyPath);
                Log($"Face Image Path: {enrollPath}");
                Log($"ID Image Path: {verifyPath}");
                Log("Info: Call API compare image ID card with captured face");
                HTTPResponses response = SendRequest("https://services.verigram.cloud/", request);
                if (response.Status == 200)
                {
                    result = 200;
                    JToken resultToken = JToken.Parse(response.RData.ToString());
                    string IsMatch = "";
                    try
                    {
                        IsMatch = resultToken.SelectToken("status").ToString();
                        Log($"Status return: {IsMatch}");
                    }
                    catch
                    {
                        IsMatch = "";
                    }
                    if (string.IsNullOrEmpty(IsMatch)) return result;
                    if (IsMatch.CompareTo("match") == 0)
                    {
                        Log("API return: Face Match");
                        return result = 200;
                    }
                    else 
                    {
                        Log("API return: Mo Match");
                        return result = 0;
                    }
                    
                }
                else result = 0;
                
            }
            catch (Exception ex)
            {
                // Handle the exception: back to Welcome Screen
                MainWindowVM.Instance.IsBacktoWelcomeVisible = true;
                Log($"An error occurred: {ex.Message}");
            }
            return result;
        }

        public int FaceID_One_Shot(string folderPath)
        {
            int result = 0;
            XDocument appconfig = XDocument.Load("AppConfig.xml");
            if (appconfig == null) return result;
            XElement username = appconfig.Descendants("ApiFaceIDUsername").FirstOrDefault();
            if (username == null) return result;
            string url = @"liveness/one-shot/" + username.Value;
            RestRequest request = new RestRequest(url, Method.Post);

            for (int i = 0; i < 3; i++)
            {
                string fullPath = Path.Combine(folderPath, $"face_{i}.jpeg");
                string imagePath = Path.GetFullPath(fullPath);
                request.AddFile("images", imagePath);
            }
            HTTPResponses response = SendRequest("https://services.verigram.cloud/", request);
            if (response.Status == 200)
            {
                result = 200;
                JToken resultToken = JToken.Parse(response.RData.ToString());
                string IsPassed = "";
                try
                {
                    IsPassed = resultToken.SelectToken("passed").ToString();
                }
                catch
                {
                    IsPassed = "";
                }
                if (string.IsNullOrEmpty(IsPassed)) { return result; }
                if (IsPassed.CompareTo("True") == 0)
                {
                    return result = 200;
                }
                else return result = 0;
            }
            else result = 0;
            return result;
        }

        public int FaceID_Create_User(string ocrFile, string folderPath)
        {
            int result = 0;
            try
            {
                string url = @"/api/Member/GetMemberInformationFromScanner2";
                Log("API Call: Save user information to database");
                RestRequest request = new RestRequest(url, Method.Post);
                string imagePath = Path.GetFullPath(folderPath);

                try
                {
                    Log("Info: Get image Id card file");
                    request.AddFile("imageIdCardFile", imagePath);
                    Log("Info: Get OCR file");
                    string ocrFilePath = Path.GetFullPath(ocrFile);
                    request.AddFile("file", ocrFilePath);
                    Log("Info: Get file success");
                }
                catch (Exception ex)
                {
                    // Handle the exception: back to Welcome Screen
                    MainWindowVM.Instance.IsBacktoWelcomeVisible = true;
                    Console.WriteLine($"An error occurred while processing file paths: {ex.Message}");
                    Log($"Error: An error occurred while processing file paths: {ex.Message}");
                }
                Log("API Sign up Call: Send request");
                HTTPResponses response = SendRequest(portAPI, request);
                Log("API Sign up Call: return result");
                if (response.Status == 200)
                {
                    result = 200;
                    JToken resultToken = JToken.Parse(response.RData.ToString());
                    string IsInserted = "";
                    int MemberId = 0;
                    try
                    {
                        IsInserted = resultToken.SelectToken("Code").ToString();
                        MemberId = ((int)resultToken.SelectToken("PersonId"));
                    }
                    catch
                    {
                        Log($"API code: {IsInserted}");
                        IsInserted = "";
                        MemberId = 0;
                        Log("API Sign up Return: Account already exists");
                        return result = 400;
                    }
                    if (string.IsNullOrEmpty(IsInserted)) { return result; }
                    if (IsInserted.CompareTo("200") == 0)
                    {
                        Log("API Sign up Return: Success");
                        UserModel.Instance.UserID = MemberId;
                        return result = 200;
                    }
                    else if (IsInserted.CompareTo("400") == 0)
                    {
                        Log("API Sign up Return: Account already exists");
                        return result = 400;
                    }

                }
                else result = 0;
            }
            catch (Exception ex)
            {
                MainWindowVM.Instance.IsBacktoWelcomeVisible = true;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Log($"API Sign up - An error occurred: {ex.Message}");
                // Handle the exception: back to Welcome Screen
            }
            return result;
        }
        public class PersonalInforShow
        {
            public string FullName { get; set; }
            public string Birthday { get; set; }
            public string Address1 { get; set; }
            public string IdenNumber { get; set; }
            public string Ward { get; set; }
            public string City { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Id { get; set; }
            public string Phone { get; set; }
        }

        public List<PersonalInforShow> FaceID_Find(string folderPath)
        {
            Log("Login start");
            List<PersonalInforShow> result = new List<PersonalInforShow>();
            Log("API call: Face regconize");
            string url = @"/api/FaceID/FindPersonByImage";
            RestRequest request = new RestRequest(url, Method.Post);

            string imagePath = Path.GetFullPath(folderPath);
            request.AddFile("img_file", imagePath);

            HTTPResponses response = SendRequest(portAPI, request);

            if (response.Status == 200)
            {
                JToken resultToken = JToken.Parse(response.RData.ToString());
                UserModel.Instance.FaceMatchPassed = true;
                Log("API Find FaceID return: Success");
                // Check if the response contains an array named "Data"
                JToken dataArray = resultToken.SelectToken("Data");
                if (dataArray != null && dataArray.Type == JTokenType.Array)
                {
                    // Deserialize each element in the array into a PersonData object
                    result = dataArray.Select(item => item.ToObject<PersonalInforShow>()).ToList();
                }
                Log("Info: Login Success");
                KioskModel.Instance.LoginCheck = 0;
            }
            if (response.Status != 200)
            {
                Log("API Face regconize return: Login Failed");
                Log("Info: Move to Sign up");
            }
            return result;
        }
        #region not use
        //public int FaceID_Find_Status(string folderPath)
        //{
        //    int result = 0;
        //    string url = @"/api/FaceID/FindPersonByImage";
        //    RestRequest request = new RestRequest(url, Method.Post);

        //    string imagePath = Path.GetFullPath(folderPath);
        //    request.AddFile("img_file", imagePath);
        //    HTTPResponses response = SendRequest(portAPI, request);
        //    if (response.Status == 200)
        //    {
        //        result = 200;
        //        JToken resultToken = JToken.Parse(response.RData.ToString());
        //        string IsSimilar = "";
        //        try
        //        {
        //            IsSimilar = resultToken.SelectToken("Code").ToString();
        //        }
        //        catch
        //        {
        //            IsSimilar = "";
        //        }
        //        if (string.IsNullOrEmpty(IsSimilar)) { return result; }
        //        if (IsSimilar.CompareTo("200") == 0)
        //        {
        //            return result = 200;
        //        }
        //    }
        //    else result = 0;
        //    return result;
        //}
        #endregion


        public List<InsuranceHistoryModel> FetchHisInsurancePackInfo(int id)
        {
            List<InsuranceHistoryModel> models = null;
            string url = @"/api/InsuranceTransaction/ShowInsuranceTransaction";
            url += @"/" + id;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {

                    try
                    {
                        models = JsonConvert.DeserializeObject<List<InsuranceHistoryModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when show insurance transaction: {ex.Message}");
                    }

                }
            }
            return models;
        }
        public class InsuranceTransactionHeader
        {
            public DateTime transactionDate { get; set; }
            public int id { get; set; }
            public int memberId { get; set; }
            public string fullName { get; set; }
            public int contractId { get; set; }
            public string packageName { get; set; }
            public int annualPay { get; set; }
            public string FormattedAnnualPay { get; set; }
            public DateTime registrationDate { get; set; }
            public DateTime expireDate { get; set; }
            public int status { get; set; }

            public string StatusDescription
            {
                get { return status == 1 ? "Còn hiệu lực" : "Hết hạn"; }
            }
        }

        public List<InsuranceTransactionHeader> InsuranceHeader(int memberId)
        {
            List<InsuranceTransactionHeader> models = null;

            string url = @"/api/InsuranceTransaction/ShowInsuranceTransactionByMemberId";
            url += "/" + memberId;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<InsuranceTransactionHeader>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when show insurance transaction list: {ex.Message}");
                    }
                }
            }
            return models;
        }

        public class SlideHeader
        {
            public int id { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public int timeNext { get; set; }
            public bool isActive { get; set; }
        }
        public class SlideDetails
        {
            public int id { get; set; }
            public string contentUrl { get; set; }
            public bool isActive { get; set; }
            public int sequence { get; set; }
            public int slideHeaderId { get; set; }
        }

        public List<SlideHeader> SlideHeaderShow(int slideHeaderId)
        {
            List<SlideHeader> models = null;
            string url = @"/api/SlideHeader/ShowSlideHeader";
            url += "/" + slideHeaderId;
            Log($"API call: show slide header");
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        Log($"API receive: return slide header");
                        models = JsonConvert.DeserializeObject<List<SlideHeader>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        Log($"Error when show slide header: {ex.Message}");
                        models = null;
                    }
                }
            }
            return models;
        }

        public List<SlideDetails> SlideDetailShow(int slideHeaderId)
        {
            List<SlideDetails> models = null;
            string url = @"/api/SlideDetail/ShowSlideDetailForKioskApp";
            url += "/" + slideHeaderId;
            Log($"API call: show slide details");
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        Log($"API receive: return slide details");
                        models = JsonConvert.DeserializeObject<List<SlideDetails>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        Log($"Error when show slide details: {ex.Message}");
                        models = null;
                    }
                }
            }
            return models;
        }
        public int UpdatePrinterStatus(int kioskId, int status)
        {
            int result = 0;

            string url = $"/api/Kiosk/UpdatePrinterStatus/{kioskId}/{status}";
            RestRequest request = new RestRequest(url, Method.Put);

            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var errorMessage = response.Message;
                result = 0;
            }
            else if (response.Status == 200)
            {
                result = 200;
            }

            return result;
        }

        public int UpdateCamStatus(int kioskId, int status)
        {
            int result = 0;

            string url = $"/api/Kiosk/UpdateCameraStatus/{kioskId}/{status}";
            RestRequest request = new RestRequest(url, Method.Put);
            var requestBody = new { description = "Update Camera Status" };

            request.AddJsonBody(requestBody);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var errorMessage = response.Message;
                result = 0;
            }
            else if (response.Status == 200)
            {
                result = 200;
            }

            return result;
        }

        public int UpdateScanStatus(int kioskId, int status)
        {
            int result = 0;

            string url = $"/api/Kiosk/UpdateScannerStatus/{kioskId}/{status}";
            RestRequest request = new RestRequest(url, Method.Put);
            var requestBody = new { description = "Update Scanner Status" };

            request.AddJsonBody(requestBody);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var errorMessage = response.Message;
                result = 0;
            }
            else if (response.Status == 200)
            {
                result = 200;
            }

            return result;
        }

        public int UpdateCashDepositStatus(int kioskId, int status)
        {
            int result = 0;

            string url = $"/api/Kiosk/UpdateCashDepositStatus/{kioskId}/{status}";
            RestRequest request = new RestRequest(url, Method.Put);
            var requestBody = new { description = "Update Cash Deposit Status" };

            request.AddJsonBody(requestBody);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var errorMessage = response.Message;
                result = 0;
            }
            else if (response.Status == 200)
            {
                result = 200;
            }

            return result;
        }

        public int UpdateKioskStatus(int kioskId, int status)
        {
            int result = 0;
            string url = $"/api/Kiosk/UpdateKioskStatus/{kioskId}/{status}";
            RestRequest request = new RestRequest(url, Method.Put);

            var requestBody = new { description = "Update Kiosk Status" };

            request.AddJsonBody(requestBody);

            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200)
            {
                var errorMessage = response.Message;
                result = 0;
            }
            else if (response.Status == 200)
            {
                result = 200;
            }

            return result;
        }

        public void Log(string message)
        {
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string logFileName = $"log_{today}.log";
            //string logFilePath = Path.Combine(@"C:\Users\Administrator\Downloads\net6.0-windows\logfile", logFileName);
            string logFilePath = Path.Combine(@"E:\", logFileName);
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        public List<KioskModel> KioskDetail(int kioskid)
        {
            List<KioskModel> models = null;
            string url = @"/api/Kiosk/ShowKioskSetup";
            url += "/" + kioskid;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<KioskModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when show kiosk details: {ex.Message}");
                    }
                }
            }
            return models;
        }
        public List<StationKisokModel> StationDetail(int stationid)
        {
            List<StationKisokModel> models = null;
            string url = @"/api/Station/ShowStation";
            url += "/" + stationid;
            RestRequest request = new RestRequest(url, Method.Get);
            HTTPResponses response = SendRequest(host, request);
            if (response.Status != 200) return models;
            if (response.RData != null)
            {
                JToken jToken = JToken.Parse(response.RData.ToString());
                if (jToken != null)
                {
                    try
                    {
                        models = JsonConvert.DeserializeObject<List<StationKisokModel>>(jToken.ToString());
                    }
                    catch (Exception ex)
                    {
                        models = null;
                        Log($"Error when show station details: {ex.Message}");
                    }
                }
            }
            return models;
        }
        private HTTPResponses SendRequest(string endPoint, RestRequest request = null, Method method = Method.Post, string contentType = "application/json", string jData = null)
        {
            HTTPResponses httpResponse = new HTTPResponses();
            try
            {

                RestClientOptions optionss = new RestClientOptions(endPoint)
                {
                    MaxTimeout = -1,
                };
                RestClient client = new RestClient(optionss);
                if (request == null)
                {
                    request = new RestRequest("", method);
                }
                if (!string.IsNullOrEmpty(jData))
                {
                    request.AddParameter(contentType, jData, ParameterType.RequestBody);
                }
                RestResponse Rresponse = client.Execute(request);
                if (Rresponse.IsSuccessful)
                {
                    httpResponse.Status = 200;
                    httpResponse.Message = "Thành công";
                    httpResponse.RData = Rresponse.Content;
                    httpResponse.Description = "Success";
                }
                else
                {
                    httpResponse.Status = 0;
                    httpResponse.Message = Rresponse.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                httpResponse.Status = 500;
                httpResponse.Message = ex.Message;
                httpResponse.RData = null;
            }
            return httpResponse;
        }
    }
}
