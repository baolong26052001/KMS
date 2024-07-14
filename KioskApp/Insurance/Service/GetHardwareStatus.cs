using BIXOLON_SamplePg;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Insurance.Service
{
    internal class GetHardwareStatus
    {
        DeviceWrapper.LIBWFXEVENTCB m_CBEvent;

        public int getScannerStatus()
        {
            ENUM_LIBWFX_ERRCODE enErrCode;
            string Command = "{\"device-name\":\"A62\",\"source\":\"Camera\",\"savepath\":\"C:\\\\Users\\\\Administrator\\\\Downloads\\\\net6.0-windows\\\\Image\\\\Scan\",\"td2\":true,\"recognize-type\":\"passport\",\"filename-format\":\"PIC-tick-5\",\"resolution\":300}";
            String[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length > 1)
                Command = arguments[1];
            enErrCode = DeviceWrapper.LibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL);
            enErrCode = DeviceWrapper.LibWFX_SetProperty(Command, m_CBEvent, IntPtr.Zero);
            if (enErrCode != ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS)
            {
                KioskModel.Instance.ScannerStatus = 0; // Status: Offline
            }
            else KioskModel.Instance.ScannerStatus = 1; // Status: Online
            DeviceWrapper.LibWFX_CloseDevice();
            DeviceWrapper.LibWFX_DeInit();
            return KioskModel.Instance.ScannerStatus;
        }

        public int getPrinterStatus()
        {
            // Connect to the printer using USB
            if (ConnectPrinter(BXLAPI.IUsb))
            {
                //Get Printer Status
                ShowPrinterStatus();

                // Disconnect the printer
                DisconnectPrinter();
            }
            else
            {
                KioskModel.Instance.PrinterStatus = 0;
            }
            return KioskModel.Instance.PrinterStatus;
        }

        private bool ConnectPrinter(int interfaceType)
        {
            if (BXLAPI.PrinterOpen(interfaceType, "", 0, 0, 0, 0, 0) == BXLAPI.BXL_SUCCESS)
            {
                return true;
            }
            return false;
        }

        private void DisconnectPrinter()
        {
            BXLAPI.PrinterClose();
        }

        public int ShowPrinterStatus()
        {
            string strStatus = "Printer Status = ";
            int lState = (int)BXLAPI.GetPrinterCurrentStatus();

            if (lState == BXLAPI.BXL_STS_NORMAL)
            {
                strStatus += " NORMAL ";
                //MessageBox.Show(strStatus);
                KioskModel.Instance.PrinterStatus = 1;
                //return KioskModel.Instance.PrinterStatus = 1;
            }
            else if ((lState & BXLAPI.BXL_STS_PAPER_NEAR_END) == BXLAPI.BXL_STS_PAPER_NEAR_END)
            {
                strStatus += " PAPER NEAR END ";
                KioskModel.Instance.PrinterStatus = 3;
            }
            else if ((lState & BXLAPI.BXL_STS_PAPEREMPTY) == BXLAPI.BXL_STS_PAPEREMPTY)
            {
                strStatus += " PAPER EMPTY ";
                KioskModel.Instance.PrinterStatus = 2;
            }
            else if ((lState & BXLAPI.BXL_STS_CASHDRAWER_HIGH) == BXLAPI.BXL_STS_CASHDRAWER_HIGH)
                strStatus += " CASHDRAWER HIGH ";
            else if ((lState & BXLAPI.BXL_STS_CASHDRAWER_LOW) == BXLAPI.BXL_STS_CASHDRAWER_LOW)
                strStatus += " CASHDRAWER LOW ";
            else if ((lState & BXLAPI.BXL_STS_COVEROPEN) == BXLAPI.BXL_STS_COVEROPEN)
                strStatus += " COVER OPEN ";
            else if ((lState & BXLAPI.BXL_STS_BATTERY_LOW) == BXLAPI.BXL_STS_BATTERY_LOW)
                strStatus += " BATTERY LOW ";
            else if ((lState & BXLAPI.BXL_STS_PAPER_TO_BE_TAKEN) == BXLAPI.BXL_STS_PAPER_TO_BE_TAKEN)
                strStatus += " PAPER PRESENCE ";
            else if ((lState & BXLAPI.BXL_STS_ERROR) == BXLAPI.BXL_STS_ERROR)
            {
                // OFFLINE
                strStatus += " OFFLINE ";
                KioskModel.Instance.PrinterStatus = 0;
            }
            return KioskModel.Instance.PrinterStatus;
        }

    }
}
