using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Weather
{
    public class WeatherInfo
    {
        public WeatherInfo(string cityId)
        {
            Cityid = cityId;
            UpdateAll();
        }

        #region 属性

        public string Cityid { get; set; }//区域 ID

        public string City_En { get; set; }//城市英文名
        public string City_Cn { get; set; }//城市中文名

        public string CityOfC_En { get; set; }//城市所在市英文名
        public string CityOfC_Cn { get; set; }//城市所在市中文名

        public string CityOfP_En { get; set; }//城市所在省英文名
        public string CityOfP_Cn { get; set; }//城市所在省中文名

        public string CityOfN_En { get; set; }//城市所在国家英文名
        public string CityOfN_Cn { get; set; }//城市所在国家中文名

        public string CityLevel { get; set; }//城市级别
        public string CityNum { get; set; }//城市区号
        public string PostNum { get; set; }//城市邮编
        public string Latitude { get; set; }//经度
        public string Longitude { get; set; }//纬度
        public string Altitude { get; set; }//海拔
        public string RadarNum { get; set; }//雷达站号

        DateTime PublishTime { get; set; }//预报发布时间

        WeatherForecast Day_1 { get; set; }
        WeatherForecast Day_2 { get; set; }
        WeatherForecast Day_3 { get; set; }
        WeatherNow Now { get; set; }

        public string TempDes { get; set; }
        public string Dress { get; set; }


        public string Date_y { get; set; }
        public string Week { get; set; }
        public string Temp { get; set; }
        public string Temp2 { get; set; }
        public string Temp3 { get; set; }
        public string Temp4 { get; set; }
        public string Temp5 { get; set; }
        public string Weather { get; set; }
        public string Weather2 { get; set; }
        public string Weather3 { get; set; }
        public string Weather4 { get; set; }
        public string Weather5 { get; set; }
        public string Wind { get; set; }
        public string Wind2 { get; set; }
        public string Wind3 { get; set; }
        public string Wind4 { get; set; }
        public string Wind5 { get; set; }
        public string Conft { get; set; }
        public string Uv { get; set; }
        public string Xc { get; set; }
        public string Ls { get; set; }
        public string Cl { get; set; }
        public string Gm { get; set; }
        public string Tr { get; set; }
        public string Cy { get; set; }
        public List<SimpleInfo> Forword { get; set; }
        public DateTime UpdateTime { get; set; }
        #endregion

        enum RequestType
        {
            Now = 0,
            Forecast = 1,
            Index = 2
        }

        private async Task<JObject> GetData(RequestType type)
        {
            string dataType = "observe";
            if (type == RequestType.Forecast)
                dataType = "forecast3d";
            else if (type == RequestType.Index)
                dataType = "index";
            HttpClient hc = new HttpClient();
            string text = "";
            var resourceLoader = new ResourceLoader();
            string appid = resourceLoader.GetString("appid");
            string private_key = resourceLoader.GetString("private_key");
            try
            {
                text = await hc.GetStringAsync(GetSmartWeatherApi(
                    "http://webapi.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}",
                    Cityid,
                    dataType,
                    DateTime.Now.ToString("yyyyMMddHHmm"),
                    appid,
                    private_key));
            }
            catch (Exception)
            {
            }

            

            return JObject.Parse(text);
        }
        private async Task GetForecast()
        {
            JObject json=await GetData(RequestType.Forecast);
            Cityid = (string)json["c"]["c1"];
            City_En = (string)json["c"]["c2"];
            City_Cn = (string)json["c"]["c3"];
            CityOfC_En = (string)json["c"]["c4"];
            CityOfC_Cn = (string)json["c"]["c5"];
            CityOfP_En = (string)json["c"]["c6"];
            CityOfP_Cn = (string)json["c"]["c7"];
            CityOfN_En = (string)json["c"]["c8"];
            CityOfN_Cn = (string)json["c"]["c9"];
            CityLevel = (string)json["c"]["c10"];
            CityNum = (string)json["c"]["c11"];
            PostNum = (string)json["c"]["c12"];
            Latitude = (string)json["c"]["c13"];
            Longitude = (string)json["c"]["c14"];
            Altitude = (string)json["c"]["c15"];
            RadarNum = (string)json["c"]["c16"];
            PublishTime = DateTime.ParseExact((string)json["f"]["f0"], "yyyyMMddHHmm", null);
            Day_1 = WeatherForecast.Parse(json["f"]["f1"][0]);
            Day_2 = WeatherForecast.Parse(json["f"]["f1"][1]);
            Day_3 = WeatherForecast.Parse(json["f"]["f1"][2]);
        }

        

        private static string GetWeekDay(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Saturday:
                    return "星期六";
                case DayOfWeek.Sunday:
                    return "星期日";
                default:
                    return "";
            }
        }


        public class SimpleInfo
        {
            public string Weather { get; set; }
            public string Date_y { get; set; }
            public string Temp { get; set; }
            public string Wind { get; set; }
            public string Image { get; set; }
        }



        public static String GetSmartWeatherApi(String _baseUrl, String _areaId, String _type, String _date, String _appID, String _privateKey)
        {

            String publicKey = String.Format(_baseUrl, _areaId, _type, _date, _appID);

            String fullUrl = String.Format(_baseUrl, _areaId, _type, _date, _appID.Substring(0, 6));

            //fullUrl = String.Format("{0}&key={1}", fullUrl, GetSmartWeatherKeyCode(publicKey, _privateKey));
            fullUrl = String.Format("{0}&key={1}", fullUrl, Sha1Encrypt(publicKey, _privateKey));

            return fullUrl;

        }

        public async Task GetCurrent()
        {
            JObject json =await GetData(RequestType.Now);
            Now = WeatherNow.Parse(json);
        }

        public async Task GetIndex()
        {
            JObject json =await GetData(RequestType.Index);
            TempDes = (string)json["i"][0]["i4"];
            Dress = (string)json["i"][0]["i5"];
        }

        public async void UpdateAll()
        {
            await GetCurrent();
            await GetForecast();
            await GetIndex();
        }







        /* public static String GetSmartWeatherKeyCode(String _publicKey, String _privateKey)
         {

             //使用SHA1的HMAC

             HMAC hmac = HMACSHA1.Create();

             Byte[] data = System.Text.Encoding.UTF8.GetBytes(_publicKey);

             //密钥

             Byte[] key = System.Text.Encoding.UTF8.GetBytes(_privateKey);

             hmac.Key = key;



             //对数据进行签名

             var signedData = hmac.ComputeHash(data);

             String keyCode = Convert.ToBase64String(signedData);

             keyCode = System.Web.HttpUtility.UrlEncode(keyCode);
            

             return keyCode;

         }*/

        public static string Sha1Encrypt(string baseString, string keyString)
        {
            var crypt = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            var buffer = CryptographicBuffer.ConvertStringToBinary(baseString, BinaryStringEncoding.Utf8);
            var keyBuffer = CryptographicBuffer.ConvertStringToBinary(keyString, BinaryStringEncoding.Utf8);
            var key = crypt.CreateKey(keyBuffer);

            var sigBuffer = CryptographicEngine.Sign(key, buffer);
            string signature = CryptographicBuffer.EncodeToBase64String(sigBuffer);
            return signature;
        }




    }


}
