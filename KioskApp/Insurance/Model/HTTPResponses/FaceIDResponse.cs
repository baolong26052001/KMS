using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Model.HTTPResponses
{
    public class FaceIDResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int id { get; set; }

        public string FullName { get; set; }
        public string Birthday { get; set; }
        public string address1 { get; set; }
        public string idenNumber { get; set; }
        public string ward { get; set; }
        public string city { get; set; } 

        #region Capture Event
        private bool iscaptureFinish = false;
        public bool IscaptureFinish
        {
            get { return iscaptureFinish; }
            set { iscaptureFinish = value; OnFinishCapture(); }
        }
        private event EventHandler captureFinish;
        public event EventHandler CaptureFinish
        {
            add { captureFinish += value; }
            remove { captureFinish -= value; }
        }

        void OnFinishCapture()
        {
            if (captureFinish == null)
            {
                captureFinish(this, new EventArgs());
            }
        }
        #endregion
    }
}
