using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model
{
    public class KioskModel
    {
        private static KioskModel instance;
        public static KioskModel Instance
        {
            get { if (instance == null) instance = new KioskModel(); return instance; }
            private set { instance = value; }
        }
        private KioskModel() { }

        private int kioskId;
        public int KioskID { get { return kioskId; } set { kioskId = value; } }

        private int printerStatus;
        public int PrinterStatus { get { return printerStatus; } set { printerStatus = value; } }

        private int cameraStatus;
        public int CameraStatus { get { return cameraStatus; } set { cameraStatus = value; } }

        private int scannerStatus;
        public int ScannerStatus { get { return scannerStatus; } set { scannerStatus = value; } }

        private int cashDepositStatus;
        public int CashDepositStatus { get { return cashDepositStatus; } set { cashDepositStatus = value; } }

        private int loginCheck = 0;
        public int LoginCheck { get { return loginCheck; } set { loginCheck = value; } }
        private int stationCode;
        public int StationCode { get { return stationCode; } set { stationCode = value; } }
        private string location;
        public string Location { get { return location; } set { location = value; } }
        private string kioskName;
        public string KioskName { get { return kioskName; } set { kioskName = value; } }
        private string cameraName;
        public string CameraName { get { return cameraName; } set { cameraName = value; } }
        private string scannerName;
        public string ScannerName { get { return scannerName; } set { scannerName = value; } }
        private string cashDepositPort;
        public string CashDepositPort { get { return cashDepositPort; } set { cashDepositPort = value; } }
        private string apiPort;
        public string APIPort { get { return apiPort; } set { apiPort = value; } }
    }
    public class StationKisokModel
    { 
        public int stationCode { get; set; }   
        public string stationName { get; set; }
        public string companyName { get; set; }
        public string address { get; set; }
        public string city {  get; set; }
        //stationName,companyName,address,city
    }
}
