using BIXOLON_SamplePg;
using Insurance.Model;
using Insurance.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Insurance.Utility.APIHelper;

namespace Insurance.Service
{
    public class Print_Services
    {
        private BXLAPI.BxlCallBackDelegate statusCallBackDelegate = null;

        public Print_Services()
        {

        }


        public void Print()
        {
            // Connect to the printer using USB
            if (ConnectPrinter(BXLAPI.IUsb))
            {
                //Get Printer Status
                ShowPrinterStatus();
                APIHelper.Instance.UpdatePrinterStatus(KioskModel.Instance.KioskID, KioskModel.Instance.PrinterStatus);
                // Print 
                PrintForm();

                // Disconnect the printer
                DisconnectPrinter();
            }
            else
            {
                KioskModel.Instance.PrinterStatus = 0;
                APIHelper.Instance.Log($"Info: Failed to connect printer");
            }
        }

        private bool ConnectPrinter(int interfaceType)
        {
            if (BXLAPI.PrinterOpen(interfaceType, "", 0, 0, 0, 0, 0) == BXLAPI.BXL_SUCCESS)
            {
                APIHelper.Instance.Log($"Info: Connect printer success");
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
            else if((lState & BXLAPI.BXL_STS_COVEROPEN) == BXLAPI.BXL_STS_COVEROPEN)
                strStatus += " COVER OPEN ";
            else if((lState & BXLAPI.BXL_STS_BATTERY_LOW) == BXLAPI.BXL_STS_BATTERY_LOW)
                strStatus += " BATTERY LOW ";
            else if((lState & BXLAPI.BXL_STS_PAPER_TO_BE_TAKEN) == BXLAPI.BXL_STS_PAPER_TO_BE_TAKEN)
                strStatus += " PAPER PRESENCE ";
            else if((lState & BXLAPI.BXL_STS_ERROR) == BXLAPI.BXL_STS_ERROR)
            {
                // OFFLINE
                strStatus += " OFFLINE ";
                KioskModel.Instance.PrinterStatus = 0;
            }
            return KioskModel.Instance.PrinterStatus;
            //MessageBox.Show(strStatus);
        }


        private void PrintForm()
        {
            // Format fee with commas or dots as separators
            string formattedFee = UserModel.Instance.InsurancePackageName.fee.ToString("#,##0").Replace(",", ".");
            int packageNameLength = VietnameseConverter.ConvertToSimpleForm(UserModel.Instance.InsurancePackageName.packageName).Length;
            int spacing = Math.Max(0, 24 - packageNameLength);
            BXLAPI.TransactionStart();
            BXLAPI.InitializePrinter();
            BXLAPI.SetCharacterSet(BXLAPI.BXL_CS_PC1258);
            BXLAPI.SetInterChrSet(BXLAPI.BXL_ICS_LATIN);
            // Print title
            BXLAPI.PrintText($"Speed POS Viet Nam\nDia chi: 728 Vo Van Kiet, HCMC\nKiosk Id: {KioskModel.Instance.KioskID}\n\n", BXLAPI.BXL_ALIGNMENT_CENTER, BXLAPI.BXL_FT_DEFAULT, BXLAPI.BXL_TS_0WIDTH | BXLAPI.BXL_TS_0HEIGHT);

            BXLAPI.PrintText($"Ma Hop Dong: \t\t{UserModel.Instance.ContractId}\nNha Cung Cap: \t\t{VietnameseConverter.ConvertToSimpleForm(UserModel.Instance.InsurancePackageName.provider)}\nNgay Giao Dich: \t{UserModel.Instance.InsuranceTransaction.transactionDate}\n\n", BXLAPI.BXL_ALIGNMENT_LEFT, BXLAPI.BXL_FT_DEFAULT, BXLAPI.BXL_TS_0WIDTH | BXLAPI.BXL_TS_0HEIGHT);

            BXLAPI.PrintText("Ten Goi\t\t\tGia tien\n----------------------------------------\n", BXLAPI.BXL_ALIGNMENT_CENTER, BXLAPI.BXL_FT_DEFAULT, BXLAPI.BXL_TS_0WIDTH | BXLAPI.BXL_TS_0HEIGHT);

            BXLAPI.PrintText($"{VietnameseConverter.ConvertToSimpleForm(UserModel.Instance.InsurancePackageName.packageName)}" + new string(' ', spacing) + $"{formattedFee}\n\n", BXLAPI.BXL_ALIGNMENT_CENTER, BXLAPI.BXL_FT_BOLD, BXLAPI.BXL_TS_0WIDTH | BXLAPI.BXL_TS_0HEIGHT);

            BXLAPI.PrintText($"Ten Nguoi Mua: \t\t{VietnameseConverter.ConvertToSimpleForm(UserModel.Instance.FullName)}\nNgay Het Han: \t\t{UserModel.Instance.InsuranceTransaction.expireDate}\n\n", BXLAPI.BXL_ALIGNMENT_LEFT, BXLAPI.BXL_FT_DEFAULT, BXLAPI.BXL_TS_0WIDTH | BXLAPI.BXL_TS_0HEIGHT);

            BXLAPI.PrintText("\tThong tin chi tiet xin lien he\n\t\thotline: 0923893471\n", BXLAPI.BXL_ALIGNMENT_LEFT, BXLAPI.BXL_FT_DEFAULT, BXLAPI.BXL_TS_0WIDTH | BXLAPI.BXL_TS_0HEIGHT);


            BXLAPI.OpenDrawer(BXLAPI.BXL_CASHDRAWER_PIN2);
            BXLAPI.CutPaper();

            //Only BK3-31 with presenter supported
            BXLAPI.PaperEject(BXLAPI.BXL_EJT_HOLD);

            // Leaves 'Transaction' mode, and then sends print data in the buffer to start printing.
            if (BXLAPI.TransactionEnd(true, 3000 /* 3 seconds */) != BXLAPI.BXL_SUCCESS)
            {
                // failed to read a response from the printer after sending the print-data.
                MessageBox.Show("TransactionEnd failed.");
            }
        }


    }
}
