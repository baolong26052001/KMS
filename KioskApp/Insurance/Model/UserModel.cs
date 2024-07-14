using Emgu.CV.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model
{
    class UserModel
    {
        private static UserModel instance;
        public static UserModel Instance
        {
            get { if (instance == null) instance = new UserModel(); return instance; }
            private set { instance = value; }
        }
        private UserModel() {}
        private int userID;
        public int UserID { get { return userID; } set { userID = value; } }

        private string fullName;
        public string FullName { get { return fullName; } set { fullName = value; } }

        private int ageRangeID;
        public int AgeRangeID { get { return ageRangeID; } set { ageRangeID = value; } }

        private string idCard;
        public string IDCard { get { return idCard; } set { idCard = value; } }

        private string idMember;
        public string IDMember { get {  return idMember; } set { idMember = value; } }
       
        private string lastName;
        public string LastName { get { return lastName; } set { lastName = value; } }
      
        private string firstName;
        public string FirstName { get { return firstName; } set { firstName = value; } }

        private string gender;
        public string Gender { get { return gender; } set { gender = value; } }

        private string birthday;
        public string Birthday { get { return birthday; } set { birthday = value; } }

        private string email;
        public string Email { get { return email; } set { email = value; } }

        private string idNum;
        public string IDNum { get { return idNum; } set { idNum = value; } }

        private string address;
        public string Address { get { return address; } set { address = value; } }

        private string ward;
        public string Ward { get { return ward; } set { ward = value; } }

        private string city;
        public string City { get { return city; } set { city = value; } }

        private string occupation;
        public string Occupation { get { return occupation; } set { occupation = value; } }

        private string taxCode;
        public string TaxCode { get { return taxCode; } set { taxCode = value; } }

        private string phoneNum;
        public string PhoneNum { get { return phoneNum; } set { phoneNum = value; } }

        private string popUpTextbox;
        public string PopUpTextbox { get { return popUpTextbox; } set { popUpTextbox = value; } }




        private InsurancePackageModel insurancePackageName;
        public InsurancePackageModel InsurancePackageName { get { return insurancePackageName; } set { insurancePackageName = value; } }

        private InsuranceTransactionModel insuranceTransaction;
        public InsuranceTransactionModel InsuranceTransaction { get {  return insuranceTransaction; } set {  insuranceTransaction = value; } }

        //private InsuranceBeneficiaryModel insuranceBeneficiary;
        //public InsuranceBeneficiaryModel InsuranceBeneficiary { get {  return insuranceBeneficiary; } set { insuranceBeneficiary = value; } }

        private List<InsuranceBeneficiaryModel> insuranceBeneficiary;
        public List<InsuranceBeneficiaryModel> InsuranceBeneficiary
        {
            get { return insuranceBeneficiary; }
            set { insuranceBeneficiary = value; }
        }

        //Saving and Loan
        private int savingLoanAmount;
        public int SavingLoanAmount { get { return savingLoanAmount; } set { savingLoanAmount = value; } }

        private SavingTransactionModel savingTransaction;
        public SavingTransactionModel SavingTransaction { get { return savingTransaction; } set { savingTransaction = value; } }

        //Loan Payback
        private int debtAmount;
        public int DebtAmount { get { return debtAmount; } set { debtAmount = value; } }




        //
        private string ocrFile;
        public string OcrFile { get { return ocrFile; } set { ocrFile = value; } }
        private int style;
        public int Style { get { return style; } set { style = value; } }
        private bool isLogin;
        public bool IsLogin { get { return isLogin; } set { isLogin = value; } }

        private bool isHomePage;
        public bool IsHomePage { get { return isHomePage; } set { isHomePage = value; } }
        private bool faceMatchPassed;
        public bool FaceMatchPassed { get { return faceMatchPassed; } set { faceMatchPassed = value; } }
        public void ClearData()
        {
            instance = null;
        }
        private bool isWelcomeView;
        public bool IsWelcomeView { get { return isWelcomeView; } set { isWelcomeView = value; } }

        private List<string> imgPathArray = new List<string>();
        public List<string> ImagePathArray { get { return imgPathArray; } set { imgPathArray = value; } }
        private List<string> ocrPathArray = new List<string>();
        public List<string> OcrPathArray { get { return ocrPathArray; } set { ocrPathArray = value; } }
        private bool imgOCRTemp = false;
        public bool IsImgOCRTemp { get { return imgOCRTemp; } set { imgOCRTemp = value; } }
        private string idHisTrans;
        public string IdHisTrans { get { return idHisTrans; } set { idHisTrans = value; } }
        private int transId;
        public int TransId { get { return transId; } set { transId = value; } }
        private int timeCheck = 0;
        public int TimeCheck { get { return timeCheck; } set { timeCheck = value; } }
        private int loginCheck = 0;
        public int LoginCheck { get { return loginCheck; } set { loginCheck = value; } }
        private int printCheck = 0;
        public int PrintCheck { get { return printCheck; } set { printCheck = value; } }
        private string insurancePaid;
        public string InsurancePaid { get { return insurancePaid; } set { insurancePaid = value; } }
        private string insuranceTransactionId;
        public string InsuranceTransactionId { get { return insuranceTransactionId; } set { insuranceTransactionId = value; } }
        private string contractId;
        public string ContractId { get { return contractId; } set { contractId = value; } }
        // Withdraw
        private int idSavingSelected;
        public int IdSavingSelected { get { return idSavingSelected; } set { idSavingSelected = value; } }
        private bool idActiveCountDown;
        public bool IdActiveCountDown { get { return idActiveCountDown; } set { idActiveCountDown = value; } }
        private bool isEditInfo;
        public bool IsEditInfo { get { return isEditInfo; } set { isEditInfo = value; } }
        private int kioskId;
        public int KioskId { get { return kioskId; } set {  kioskId = value; } }
        private string paymentMethod;
        public string PaymentMethod { get {  return paymentMethod; } set {  if (paymentMethod != value) { paymentMethod = value; } } }
    }
}
