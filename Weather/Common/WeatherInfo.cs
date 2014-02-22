using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Weather
{
    public class WeatherInfo
    {
        public string City { get; set; }
        public string Cityid { get; set; }
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
        public string TempDes { get; set; }
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

        public static WeatherInfo Parse(string text)
        {
            JObject json = JObject.Parse(text);//用Jobject解析json数据  
            WeatherInfo weather = new WeatherInfo()
            {
                City = (string)json["weatherinfo"]["city"],
                Cityid = (string)json["weatherinfo"]["cityid"],
                Date_y = (string)json["weatherinfo"]["date_y"],
                Week = (string)json["weatherinfo"]["week"],
                Wind = (string)json["weatherinfo"]["wind1"],
                Temp = (string)json["weatherinfo"]["temp1"],
                Weather = (string)json["weatherinfo"]["weather1"],
                TempDes = (string)json["weatherinfo"]["index"],
                Conft = (string)json["weatherinfo"]["index_co"],
                Uv = (string)json["weatherinfo"]["index_uv"],
                Xc = (string)json["weatherinfo"]["index_xc"],
                Ls = (string)json["weatherinfo"]["index_ls"],
                Cl = (string)json["weatherinfo"]["index_cl"],
                Gm = (string)json["weatherinfo"]["index_ag"],
                Tr = (string)json["weatherinfo"]["index_tr"],
                Cy = (string)json["weatherinfo"]["index_d"],
                Forword = new List<SimpleInfo>()
            };
            for (int i = 1; i < 5; i++)
            {
                SimpleInfo si = new SimpleInfo();
                string today = ((string)json["weatherinfo"]["date_y"]).Replace('年', '/').Replace('月', '/');
                today = today.Remove(today.Length - 1);
                DateTime time = DateTime.Parse(today);
                time = time.AddDays(i);
                si.Date_y = time.Year + "年" + time.Month + "月" + time.Day + "日   " + GetWeekDay(time.DayOfWeek);
                si.Weather = (string)json["weatherinfo"]["weather" + i];
                si.Temp = (string)json["weatherinfo"]["temp" + i];
                si.Wind = (string)json["weatherinfo"]["wind" + i];
                weather.Forword.Add(si);
            }
            return weather;
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
    }


}
