using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Weather.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Windows.Data.Xml.Dom;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace Weather
{
    public sealed partial class AskLocation : UserControl
    {
        Dictionary<string, City> cities;
        ObservableCollection<City> searched;
        private Geolocator _geolocator = null;
        private CancellationTokenSource _cts = null;

        public bool Result { get; set; }
        public City outCity{ get; set; }

        public AskLocation(bool type)
        {
            this.InitializeComponent();
            if (type)
            {
                cancleCity.Visibility = Visibility.Visible;
            }
            Init();
           
        }
        
       

        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            CancelGetLocation();
            searched.Clear(); 
            foreach (string str in cities.Keys)
            {
                if (str.Contains(tb_searchCity.Text))
                {
                    searched.Add(cities[str]);            
                }
                
            }         
            if (searched.Count == 0)
            {
                searched.Add(new City() {Name="未找到，请检查输入再试",Code="-1"});
            }
            cityList.ItemsSource = searched;
        }

        private async void Init()
        {
            searched = new ObservableCollection<City>();
            StorageFolder sf = Package.Current.InstalledLocation;//获取应用安装目录
            var fileFolder = await sf.GetFolderAsync("Files");
            var file = await fileFolder.GetFileAsync("citylist.txt");
            var text = await FileIO.ReadTextAsync(file);
            cities = JsonConvert.DeserializeObject<Dictionary<string, City>>(text);
            _geolocator = new Geolocator();
            GetLocation();

        }

        private void confirmCity_Tapped(object sender, TappedRoutedEventArgs e)
        {
            City city = (cityList.SelectedItem as City);
            if (cityList.SelectedItem != null&&cities.Keys.Contains(city.Name))
            {
                CancelGetLocation();
                Result = true;
                outCity = city;
                (this.Parent as Popup).IsOpen = false;
            }
        }

        private void cancleCity_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            CancelGetLocation();
            Result = false;
            (this.Parent as Popup).IsOpen = false;
        }




        async public void GetLocation()
        {
            try
            {
                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;
                Geoposition pos = await _geolocator.GetGeopositionAsync().AsTask(token);
                GetCity(pos.Coordinate.Latitude, pos.Coordinate.Longitude);
            }
            catch (System.UnauthorizedAccessException)
            {
                tb_locaInfo.Text = "无权限，位置获取失败，请手动搜索";
                pb_loca.Visibility = Visibility.Collapsed;
            }
            catch (TaskCanceledException)
            {
                //取消获取
                //rootPage.NotifyUser("Canceled", NotifyType.StatusMessage);
            }
            catch(Exception)
            {
                tb_locaInfo.Text = "位置获取失败，请手动搜索";
                pb_loca.Visibility = Visibility.Collapsed;
            }
            finally
            {
                _cts = null;           
            }
        }

        /// <summary>
        /// 取消获取位置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelGetLocation()
        {
            pb_loca.Visibility = Visibility.Collapsed;
            tb_locaInfo.Visibility = Visibility.Collapsed;
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }

        private async void GetCity(double lat, double lon)
        {
            HttpClient hc = new HttpClient();
            try
            {
                //string text = await hc.GetStringAsync("http://ditu.google.cn/maps/geo?output=csv&key=abcdef&q=" + lat + "," + lon);
                string text = await hc.GetStringAsync("http://maps.googleapis.com/maps/api/geocode/xml?latlng=" + lat + "," + lon + "&sensor=true&language=zh-CN");
                XmlDocument doc = new XmlDocument();//创建XML文档对象
                if (!string.IsNullOrEmpty(text))
                {
                    doc.LoadXml(text);//加载xml字符串

                    //获取状态信息
                    string xpath = @"GeocodeResponse/status";
                    var node = doc.SelectSingleNode(xpath);
                    string status = node.InnerText.ToString();

                    if (status == "OK")
                    {
                        //获取地址信息
                        xpath = @"GeocodeResponse/result/formatted_address";
                        node = doc.SelectSingleNode(xpath);
                        text = node.InnerText.ToString();
                        text = text.Contains("中国") ? text.Remove(0, 2) : text;

                        string s1 = "";
                        decimal d1 = 0.0M;
                        //if (text.Contains("省"))
                        //{
                        //    int index = text.IndexOf("省");
                        //    text = text.Substring(index + 1);
                        //}
                        foreach (string name in cities.Keys)
                        {
                            decimal mm = LevenshteinDistance.Instance.LevenshteinDistancePercent(text, name);
                            if (mm > d1)
                            {
                                s1 = name;
                                d1 = mm;
                            }
                        }
                        searched.Add(cities[s1]);
                        cityList.ItemsSource = searched;
                        tb_locaInfo.Text = "若定位有误请手动搜索";
                        pb_loca.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        tb_locaInfo.Text = "位置获取失败，请手动搜索";
                        pb_loca.Visibility = Visibility.Collapsed;
                    }

                }
                else
                {
                    tb_locaInfo.Text = "位置获取失败，请手动搜索";
                    pb_loca.Visibility = Visibility.Collapsed;
                }
                //text = text.Substring(text.IndexOf('"') + 1);
                //text = text.Substring(0, text.IndexOf('"'));
                
            }
            catch (Exception)
            {
                tb_locaInfo.Text = "位置获取失败，请手动搜索";
                pb_loca.Visibility = Visibility.Collapsed;
            }
        }


    }
}
