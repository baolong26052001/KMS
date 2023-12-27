using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KMS.Models
{
    public class IpInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }


        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region_name")]
        public string Region { get; set; }

        [JsonProperty("country_name")]
        public string Country { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }


        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }
    }
}
