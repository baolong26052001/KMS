using System;

namespace Insurance.Model
{
    public class UserInfoModel
    {
        public int id { get; set; }
        public string fullName { get; set; }
        //public int age { get; set; }
        public int ageRangeId { get; set; }
        public string description { get; set; }
        public DateTime birthday { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }

        public string address1{ get; set; }
        public string address2 { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public string ward{ get; set; }
        public string idenNumber { get; set; }
        public string occupation { get; set; }
        public string email { get; set; }
        public string taxCode { get; set; }
    }
}
