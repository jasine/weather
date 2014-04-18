using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    #region Code Base Class
    class Code
    {
        private string id;
        private string cnName;
        private string enName;

        #region 字段封装
        public string Id
        {
            get { return id; }
            protected set { id = value; }
        }
        public string CnName
        {
            get { return cnName; }
            protected set { cnName = value; }
        }
        public string EnName
        {
            get { return enName; }
            protected set { enName = value; }
        } 
        #endregion

    } 
    #endregion

    #region Weather
    class WeatherCode : Code
    {
        private WeatherCode(string id)
        {
            Id = id;
            #region convert id to name
            switch (id)
            {
                case "00":
                    CnName = "晴";
                    EnName = "Sunny";
                    break;

                case "01":
                    CnName = "多云";
                    EnName = "Cloudy";
                    break;

                case "02":
                    CnName = "阴";
                    EnName = "Overcast";
                    break;

                case "03":
                    CnName = "阵雨";
                    EnName = "Shower";
                    break;

                case "04":
                    CnName = "雷阵雨";
                    EnName = "Thundershower";
                    break;

                case "05":
                    CnName = "雷阵雨伴有冰雹";
                    EnName = "Thundershower with hail";
                    break;

                case "06":
                    CnName = "雨夹雪";
                    EnName = "Sleet";
                    break;

                case "07":
                    CnName = "小雨";
                    EnName = "Light rain";
                    break;

                case "08":
                    CnName = "中雨";
                    EnName = "Moderate rain";
                    break;

                case "09":
                    CnName = "大雨";
                    EnName = "Heavy rain";
                    break;

                case "10":
                    CnName = "暴雨";
                    EnName = "Storm";
                    break;

                case "11":
                    CnName = "大暴雨";
                    EnName = "Heavy storm";
                    break;

                case "12":
                    CnName = "特大暴雨 ";
                    EnName = "Severe storm";
                    break;

                case "13":
                    CnName = "阵雪";
                    EnName = "Snow flurry";
                    break;

                case "14":
                    CnName = "小雪";
                    EnName = "Light snow";
                    break;

                case "15":
                    CnName = "中雪";
                    EnName = "Moderate snow";
                    break;

                case "16":
                    CnName = "大雪";
                    EnName = "Heavy snow";
                    break;

                case "17":
                    CnName = "暴雪";
                    EnName = "Snowstorm";
                    break;

                case "18":
                    CnName = "雾";
                    EnName = "Foggy";
                    break;

                case "19":
                    CnName = "冻雨";
                    EnName = "Ice rain";
                    break;

                case "20":
                    CnName = "沙尘暴 ";
                    EnName = "Duststorm";
                    break;

                case "21":
                    CnName = "小到中雨 ";
                    EnName = "Light to moderate rain";
                    break;

                case "22":
                    CnName = "中到大雨 ";
                    EnName = "Moderate to heavy rain";
                    break;

                case "23":
                    CnName = "大到暴雨 ";
                    EnName = "Heavy rain to storm";
                    break;

                case "24":
                    CnName = "暴雨到大暴雨 ";
                    EnName = "Storm to heavy storm";
                    break;

                case "25":
                    CnName = "大暴雨到特大暴雨";
                    EnName = "Heavy to severe storm";
                    break;

                case "26":
                    CnName = "小到中雪 ";
                    EnName = "Light to moderate snow";
                    break;

                case "27":
                    CnName = "中到大雪 ";
                    EnName = "Moderate to heavy snowm";
                    break;

                case "28":
                    CnName = "大到暴雪 ";
                    EnName = "Heavy snow to snowstorm";
                    break;

                case "29":
                    CnName = "浮尘 ";
                    EnName = "Dust";
                    break;

                case "30":
                    CnName = "扬沙 ";
                    EnName = "Sand";
                    break;

                case "31":
                    CnName = "强沙尘暴 ";
                    EnName = "Sandstorm";
                    break;

                case "99":
                    CnName = "无";
                    EnName = "Unknown";
                    break;

                default:
                    CnName = "错误";
                    EnName = "Error";
                    break;
            }
            #endregion
        }

        public static WeatherCode Parse(string id)
        {
            return new WeatherCode(id);
        }

    } 
    #endregion

    #region Wind Force
    class WindForceCode : Code
    {
        private WindForceCode(string id)
        {
            this.Id = id;
            #region convert id to name
            switch (id)
            {
                case "0":
                    CnName = "微风";
                    EnName = "<10m/h";
                    break;

                case "1":
                    CnName = "3-4 级";
                    EnName = "10-17m/h";
                    break;

                case "2":
                    CnName = "4-5 级";
                    EnName = "17~25m/h";
                    break;

                case "3":
                    CnName = "5-6 级";
                    EnName = "25-34m/h";
                    break;

                case "4":
                    CnName = "6-7 级";
                    EnName = "34-43m/h";
                    break;

                case "5":
                    CnName = "7-8 级";
                    EnName = "43-54m/h";
                    break;

                case "6":
                    CnName = "8-9 级";
                    EnName = "54-65m/h";
                    break;

                case "7":
                    CnName = "9-10 级";
                    EnName = "65-77m/h";
                    break;

                case "8":
                    CnName = "10-11 级";
                    EnName = "77-89m/h";
                    break;

                case "9":
                    CnName = "11-12 级";
                    EnName = "89-102m/h";
                    break;

                default:
                    CnName = "错误";
                    EnName = "Error";
                    break;
            }
            #endregion
        }

        public static WindForceCode Parse(string id)
        {
            //WeatherCode weatherCode = new WeatherCode(id);
            return new WindForceCode(id);
        }
    } 
    #endregion

    #region Wind Direction
    class WindDirectionCode : Code
    {
        private WindDirectionCode(string id)
        {
            this.Id = id;           
            #region convert id to name
            switch (id)
            {
                case "0":
                    CnName = "无持续风向";
                    EnName = "No wind";
                    break;

                case "1":
                    CnName = "东北风";
                    EnName = "Northeast";
                    break;

                case "2":
                    CnName = "东风";
                    EnName = "East";
                    break;

                case "3":
                    CnName = "东南风";
                    EnName = "Southeast";
                    break;

                case "4":
                    CnName = "南风";
                    EnName = "South";
                    break;

                case "5":
                    CnName = "西南风";
                    EnName = "Southwest";
                    break;

                case "6":
                    CnName = "西风";
                    EnName = "West";
                    break;

                case "7":
                    CnName = "西北风";
                    EnName = "Northwest";
                    break;

                case "8":
                    CnName = "北风";
                    EnName = "North";
                    break;

                case "9":
                    CnName = "旋转风";
                    EnName = "Whirl wind";
                    break;

                default:
                    CnName = "错误";
                    EnName = "Error";
                    break;
            }
            #endregion
        }
        public static WindDirectionCode Parse(string id)
        {
            //WeatherCode weatherCode = new WeatherCode(id);
            return new WindDirectionCode(id);
        }
    } 
    #endregion

    class WeatherNow
    {
        string Temp { get; set; }
        string Humidity { get; set; }
        string WindForce { get; set; }    
        WindDirectionCode WindDiretion{get; set;}
        DateTime PublicTime { get; set; }

        public static WeatherNow Parse(JObject json)
        {
            WeatherNow weather = new WeatherNow()
            {
                Temp=(string)json["l"]["l1"],
                Humidity=(string)json["l"]["l2"],
                WindForce=(string)json["l"]["l3"],
                WindDiretion=WindDirectionCode.Parse((string)json["l"]["l4"]),
                PublicTime = DateTime.ParseExact((string)json["l"]["l7"], "HH:mm", null)
            };
            return weather;
        }
    }

    class WeatherForecast
    {
        WeatherCode DayWeather { get; set; }
        WeatherCode NightWeather { get; set; }

        string DayTemp { get; set; }
        string NightTemp { get; set; }
        DateTime UpTime { get; set; }
        DateTime DownTime { get; set; }

        WindForceCode DayWindForce { get; set; }
        WindForceCode NightWindForce { get; set; }

        WindDirectionCode DayWindDirection { get; set; }
        WindDirectionCode NightWindDirection { get; set; }



        internal static WeatherForecast Parse(JToken json)
        {
            string[] tmp=((string)json["fi"]).Split('|');
            return new WeatherForecast(){
                DayWeather=WeatherCode.Parse((string)json["fa"]),
                NightWeather=WeatherCode.Parse((string)json["fb"]),
                DayTemp=(string)json["fc"],
                NightTemp=(string)json["fd"],
                DayWindDirection=WindDirectionCode.Parse((string)json["fe"]),
                NightWindDirection=WindDirectionCode.Parse((string)json["ff"]),
                DayWindForce=WindForceCode.Parse((string)json["fg"]),
                NightWindForce=WindForceCode.Parse((string)json["fh"]),
                UpTime=DateTime.ParseExact(tmp[0],"HH:mm",null),
                DownTime=DateTime.ParseExact(tmp[1],"HH:mm",null)            
            };
        }
    }
}
