using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Service
{
    public class LogHelper
    {

        //string basePath = @"D:\C#_WPF\ConsoleApp2\Logs";
        public void WriteLog(string value, int logType, string basePath)
        {
            string folderName = "";
            switch (logType)
            {
                case 1:
                    folderName = "Action";
                    break;
                case 2:
                    folderName = "Error";
                    break;
                case 3:
                    folderName = "Data";
                    break;
                default:
                    folderName = "Error";
                    break;
            }

            string fileName = DateTime.Now.ToString("yyyyMMdd");
            string path = Path.Combine(basePath, folderName, $"Action_Logs_{fileName}.txt");

            // Ensure the directory exists
            if (Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));

            // Modified to include seconds
            string timeWithSeconds = DateTime.Now.ToString("h:mm:ss tt");

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine($"[{timeWithSeconds}]: {value}{Environment.NewLine}");
            }
        }


        //[time] - [action] -[noi dung]
        // Logs/acion/error
        //20200129_action.

        //action logs


        //Error logs

        //du lieu logs  

    }
}
