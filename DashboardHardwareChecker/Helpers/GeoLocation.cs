using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DashboardHardwareChecker.Helpers
{
    public static class GeoLocation
    {
        public static string GetExternalIPAddress()
        {
            string sTmp = new WebClient().DownloadString("http://ip.gtt.tools");
            //parse out the html
            try
            {
                return sTmp.Substring(sTmp.IndexOf("[") + 1, sTmp.IndexOf("]") - sTmp.IndexOf("[") - 1);
            }
            catch (Exception ex)
            {
                return "";
            }
        }



        public static string GetUserCountryByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            string data = $"Country: {ipInfo.Country}\nCity: {ipInfo.City}\nRegion: {ipInfo.Region}\nExternal IP: {ipInfo.Ip}";

            return data;
        }

        public class IpInfo
        {
            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("region")]
            public string Region { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("loc")]
            public string Loc { get; set; }

            [JsonProperty("org")]
            public string Org { get; set; }

            [JsonProperty("postal")]
            public string Postal { get; set; }
        }

    }
}
