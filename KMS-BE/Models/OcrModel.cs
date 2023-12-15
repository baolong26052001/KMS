namespace KMS.Models
{
    public class OcrModel
    {
        public string status { get; set; }
        public int status_code { get; set; }
        public bool has_card { get; set; }
        public List<Card> cards { get; set; }
    }

    public class Card
    {
        public string side { get; set; }
        public string type { get; set; }
        public bool gray_image { get; set; }
        public bool corner_cut { get; set; }
        public bool missing_corner { get; set; }
        public bool glare_image { get; set; }
        public bool blur_image { get; set; }
        public bool card_check_fake { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public string id { get; set; }
        //public double id_conf { get; set; }
        public string name { get; set; }
        //public double name_conf { get; set; }
        public string domicile { get; set; }
        //public double domicile_conf { get; set; }
        public string country { get; set; }
        //public double country_conf { get; set; }
        public string address { get; set; }
        //public double address_conf { get; set; }
        public string birthday { get; set; }
        //public double birthday_conf { get; set; }
        public string expiry { get; set; }
        //public double expiry_conf { get; set; }
        public string gender { get; set; }
        //public double gender_conf { get; set; }
        public string ethnicity { get; set; }
        //public double ethnicity_conf { get; set; }
        public string issue_by { get; set; }
        //public double issue_by_conf { get; set; }
        public string issue_date { get; set; }
        //public double issue_date_conf { get; set; }
        public string religion { get; set; }
        //public double religion_conf { get; set; }
        public AddressSplit address_split { get; set; }
        public DomicileSplit domicile_split { get; set; }
    }

    public class AddressSplit
    {
        public string province { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string village { get; set; }
    }

    public class DomicileSplit
    {
        public string province { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string village { get; set; }
    }

}
