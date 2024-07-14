using Emgu.CV.Dnn;
using Emgu.CV.Face;
using Insurance;
using Insurance.Model;
using Insurance.Utility;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace ID_Scanner
{
    class Scanner
    {
        //Singe
        private static Scanner instance = null;
        string szScanImageList;
        private int imageList = 1;
        public static Scanner Instance
        {
            get
            {
                if (instance == null)
                    instance = new Scanner();
                return instance;
            }
        }

        private bool isFinish = false;
        public bool IsFinish
        {
            get => isFinish;
            set { isFinish = value; OnFisishChange(); }
        }


        public event EventHandler onCapture;

        private event EventHandler onFinish;
        public event EventHandler OnFinish
        {
            add { onFinish += value; }
            remove { onFinish -= value; }
        }

        void OnFisishChange()
        {
            if (onFinish != null)
            {
                onFinish(this, new EventArgs());
            }
        }

        DeviceWrapper.LIBWFXEVENTCB m_CBEvent;




        public void AutoCapture()
        {
            int imageScan = 0;
            ENUM_LIBWFX_ERRCODE enErrCode;
            bool DoScan = false;
            int timer = 0, sum = 0;
            IntPtr pScanImageList, pOCRResultList, pExceptionRet, pEventRet;
            string scannerName = KioskModel.Instance.ScannerName;
            //string Command = "{\"device-name\":\"A62\",\"source\":\"Camera\",\"autoscan\":true,\"savepath\":\"D:\\\\RnD Project\\\\Loan-Kiosk-App\\\\KioskApp\\\\Insurance\\\\bin\\\\Debug\\\\net6.0-windows\\\\Image\\\\Scan\",\"td2\":true,\"recognize-type\":\"passport\",\"filename-format\":\"PIC-tick-5\",\"resolution\":300}";
            //string Command = "{\"device-name\":\"A62\",\"source\":\"Camera\",\"savepath\":\"C:\\\\Users\\\\Administrator\\\\Downloads\\\\net6.0-windows\\\\Image\\\\Scan\",\"td2\":true,\"recognize-type\":\"passport\",\"filename-format\":\"PIC-tick-5\",\"resolution\":300}";
            string Command = $"{{\"device-name\":\"A62\",\"source\":\"Camera\",\"savepath\":\"{scannerName}\",\"td2\":true,\"recognize-type\":\"passport\",\"filename-format\":\"PIC-tick-5\",\"resolution\":300}}";
            #region Command path
            //{ "device-name":"A62","source":"Camera","autoscan":true,"savepath":"C:\\Users\\Administrator\\Downloads\\net6.0-windows\\Image\\Scan","recognize-type":"passport","resolution":250}
            //string Command = "{\"device-name\":\"A62\",\"source\":\"Camera\",\"autoscan\":true,\"savepath\":\"D:\\\\RnD Project\\\\KioskApp\\\\Insurance\\\\bin\\\\Debug\\\\net6.0-windows\\\\Image\\\\Scan\",\"td2\":true,\"recognize-type\":\"passport\",\"filename-format\":\"PIC-tick-5\",\"resolution\":300}";
            //D:\TaiLieu\Speed Pos\Code\LoanKiosk\KioskApp\Insurance\bin\Debug\net6.0-windows\CCCD
            //D:\TaiLieu\\\\Speed Pos\\\\Code\\\\Loan-Kiosk\\\\KioskApp\\\\Insurance\bin\Debug\net6.0-windows\CCCD
            //C:\Loan-Kiosk-App-Dung-NewVer\KioskApp\Insurance\bin\Debug\net6.0-windows\Image\Scan\
            //C:\\\\Users\\\\Administrator\\\\Downloads\\\\net6.0-windows\\\\Image\\\\Scan\
            //C:\\\\Loan-Kiosk-App-Dung-NewVer\\\\KioskApp\\\\Insurance\\\\bin\\\\Debug\\\\net6.0-windows\\\\Image\\\\Scan\",

            // POS scanner command:
            // string Command = "{\"device-name\":\"A62\",\"source\":\"Camera\",\"savepath\":\"C:\\\\Users\\\\Administrator\\\\Downloads\\\\net6.0-windows\\\\Image\\\\Scan\",\"td2\":true,\"recognize-type\":\"passport\",\"resolution\":300}";

            //"\"savepath\":\"C:\\\\Users\\\\Administrator\\\\Downloads\\\\net6.0-windows\\\\Image\\\\Scan\","
            //D:\RnD Project\Loan-Kiosk\KioskApp\Insurance\bin\Debug\net6.0-windows
            //D:\RnD Project\KioskApp\Insurance\bin\Debug\net6.0-windows\Image\Scan
            #endregion
            //get command from bat file "AutoCaptureDemo-CSharp.bat"
            String[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length > 1)
                Command = arguments[1];

            enErrCode = DeviceWrapper.LibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL);
            #region OCR
            //MessageBox.Show($"{enErrCode}");
            //if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_OCR)
            //{
            //    System.Console.WriteLine(@"Status:[No Recognize Tool]"); 
            //}

            //else if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_AVI_OCR)
            //{
            //    System.Console.WriteLine(@"Status:[No AVI Recognize Tool]"); 
            //}
            //else if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DOC_OCR)
            //{
            //    System.Console.WriteLine(@"Status:[No DOC Recognize Tool]"); 
            //}
            //else if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PATH_TOO_LONG)
            //{
            //    System.Console.WriteLine(@"Status:[Path Is Too Long (max limit: 130 bits)]");
            //    System.Console.WriteLine(@"Status:[LibWFX_InitEx Fail]");
            //}
            //else if (enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            //{
            //    System.Console.WriteLine(@"Status:[LibWFX_InitEx Fail [" + ((int)enErrCode).ToString() + "]] "); //get fail message
            //    return;
            //}
            #endregion 
            enErrCode = DeviceWrapper.LibWFX_SetProperty(Command, m_CBEvent, IntPtr.Zero);
            if (enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                KioskModel.Instance.ScannerStatus = 0; // Status: Offline
                APIHelper.Instance.Log($"Error: Failed to connect scanner - {enErrCode}");
            } else {
                KioskModel.Instance.ScannerStatus = 1; // Status: Online
                APIHelper.Instance.Log($"Info: Connect Scanner Success");
            }
            APIHelper.Instance.UpdateScanStatus(KioskModel.Instance.KioskID, KioskModel.Instance.ScannerStatus);
            while (imageList != imageScan)
            {
                timer = 0;
                sum = 0;
                while (timer < 3)
                {
                    Thread.Sleep(100);

                    sum++;
                    enErrCode = DeviceWrapper.LibWFX_PaperReady();
                    if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
                        timer++;

                    if (sum == 4)
                    {
                        sum = 0;
                        timer = 0;
                        if (DoScan)
                            DoScan = false;
                        //MessageBox.Show(@"Please put the card");
                        //Thread.Sleep(1000); //option
                    }
                }

                if (DoScan)
                {
                    //MessageBox.Show(@"The card is continuously detected, please remove the card.");
                    //Thread.Sleep(1000);  //option
                    continue;
                }

                enErrCode = DeviceWrapper.LibWFX_SynchronizeScan(Command, out pScanImageList, out pOCRResultList, out pExceptionRet, out pEventRet);
                imageScan++;
                string szExceptionRet = Marshal.PtrToStringUni(pExceptionRet);
                string szEventRet = Marshal.PtrToStringUni(pEventRet);

                if (enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS && enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                {
                    IntPtr pstr;
                    DeviceWrapper.LibWFX_GetLastErrorCode(enErrCode, out pstr);
                    string szErrorMsg = Marshal.PtrToStringUni(pstr);
                    //MessageBox.Show(@"Status:[LibWFX_SynchronizeScan Fail [" + ((int)enErrCode).ToString() + "]] " + szErrorMsg.ToString()); //get fail message
                }
                else if (szEventRet.Length > 1) //event happen
                {
                    //MessageBox.Show(@"Status:[Device Ready!]");
                    //MessageBox.Show(szEventRet);  //get event message

                    if (szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" && szEventRet != "LIBWFX_EVENT_UVSECURITY_DETECTED[1]")
                    {

                        //MessageBox.Show(@"Status:[Scan End]\n");
                        //return;
                        DoScan = true;
                        //isFinish = true;
                        return;

                    }

                    if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                        //MessageBox.Show(@"Status:[There are some mismatched key in command]");

                        szScanImageList = Marshal.PtrToStringUni(pScanImageList);
                    //string szOCRResultList = Marshal.PtrToStringUni(pOCRResultList);


                    string[] ScanImageWords = szScanImageList.Split(']');
                    //string[] OCRResultWords = szOCRResultList.Split(']');



                    
                    for (int idx = 0; idx < ScanImageWords.Length - 1; idx++)
                    {
                        ScanImageWords[idx].Trim();  //get each image path
                        //OCRResultWords[idx].Trim();  //get each ocr result
                    }
                }
                else
                {
                    //MessageBox.Show(@"Status:[Device Ready!]");

                    if (enErrCode == ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
                        //MessageBox.Show(@"Status:[There are some mismatched key in command]");

                        if (szExceptionRet.Length > 1) //exception happen
                        {
                            //MessageBox.Show(@"Status:[Device Ready!]");
                            //MessageBox.Show(@szExceptionRet);  //get exception message
                        }


                    szScanImageList = Marshal.PtrToStringUni(pScanImageList);
                    //string szOCRResultList = Marshal.PtrToStringUni(pOCRResultList);


                    string[] ScanImageWords = szScanImageList.Split(']');
                    //string[] OCRResultWords = szOCRResultList.Split(']');


                    //onCapture?.Invoke(szScanImageList.Split(']'), new EventArgs());
                    Globals.Url = ScanImageWords[0];
                    imageScan += 1;
                    //apiService.ProcessImages(ScanImageWords[0]);


                    for (int idx = 0; idx < ScanImageWords.Length - 1; idx++)
                    {
                        ScanImageWords[idx].Trim();  //get each image path
                        //OCRResultWords[idx].Trim();  //get each ocr result
                    }
                }
                DoScan = true;
                //MessageBox.Show(@"Status:[Scan End]");
                if (imageScan != imageList)
                {
                    APIHelper.Instance.Log($"Info: Close scanner");
                    DeviceWrapper.LibWFX_CloseDevice();
                    DeviceWrapper.LibWFX_DeInit();
                    break;
                }
            }
            onCapture?.Invoke(szScanImageList.Split(']'), new EventArgs());
        }

    }
}