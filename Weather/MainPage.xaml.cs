using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ApplicationSettings;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Weather
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AskLocation askl;
        WeatherInfo saveWeather;
        Color oldColor,newColor,nowColor;
        DispatcherTimer timer;
        public static MainPage Current;
        public MainPage()
        {
            this.InitializeComponent();
            SettingsPane.GetForCurrentView().CommandsRequested += BlankPage_CommandsRequested;
        }


        /// <summary>
        /// 添加设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void  BlankPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("login", "设置",(x) =>
            {
                OpenAddCity();
            });
            args.Request.ApplicationCommands.Add(cmd);

            
            

            SettingsCommand cmd2 = new SettingsCommand("about", "关于", (handle) =>
            {
                Popup popup = CreatePopup.Create(new AboutPage(), 346);
                popup.IsOpen = true;

            });
            args.Request.ApplicationCommands.Add(cmd2);
        }

        /// <summary>
        /// 打开城市管理窗口
        /// </summary>
        private void OpenAddCity()
        {
            if (addCity.IsOpen != true)//防止在（初次）设置时再打开设置
            {
                askl = new AskLocation(true);
                askl.Width = Window.Current.Bounds.Width;
                askl.Height = Window.Current.Bounds.Height;
                addCity.Child = askl;
                addCity.IsOpen = true;
            }
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Current = this;
            nowColor=GetColor();
            background.Background = new SolidColorBrush(nowColor);
            if (e.NavigationMode == NavigationMode.New)
            {
                try
                {
                    var file = await ApplicationData.Current.LocalFolder.GetFileAsync("save.data");//获取城市记录文件
                    string str = await FileIO.ReadTextAsync(file);
                    saveWeather = JsonConvert.DeserializeObject<WeatherInfo>(str);
                    UpdateInfo();
                    TimeSpan sp = DateTime.Now - saveWeather.UpdateTime;
                    if (sp.Minutes>30)
                    {
                        await GetWeather();
                    }                  
                }
                catch (Exception)//第一次打开，文件未创建
                {
                    askl = new AskLocation(false);
                    askl.Width = Window.Current.Bounds.Width;
                    askl.Height = Window.Current.Bounds.Height;
                    addCity.Child = askl;
                    addCity.IsOpen = true;                  
                }              
            }           
        }

        /// <summary>
        /// 获取预报信息并解析
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task GetWeather()
        {

            ChangeBg();
            errorInfo.Visibility = Visibility.Collapsed;
            ps_refresh.Visibility = Visibility.Visible;
            HttpClient hc = new HttpClient();
            try
            {
                string text = await hc.GetStringAsync("http://m.weather.com.cn/data/" + saveWeather.Cityid + ".html");
                saveWeather = WeatherInfo.Parse(text);
                saveWeather.UpdateTime = DateTime.Now;
                errorInfo.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                errorInfo.Visibility = Visibility.Visible;
            }
            ps_refresh.Visibility = Visibility.Collapsed;
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync("save.data");
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(saveWeather));           
            UpdateInfo();      
        }

        /// <summary>
        /// 更新界面显示
        /// </summary>
        private void UpdateInfo()
        {
            tb_cityName.Text = saveWeather.City;
            tb_time.Text = saveWeather.Date_y + "    " + saveWeather.Week;
            tb_weather.Text = saveWeather.Weather;
            tb_temp.Text = saveWeather.Temp;
            tb_tempDes.Text = saveWeather.TempDes;
            tb_wind.Text = saveWeather.Wind;
            tb_conft.Text = "舒适度：" + saveWeather.Conft;
            tb_uv.Text = "紫外线：" + saveWeather.Uv;
            tb_xc.Text = "洗车：" + saveWeather.Xc;
            tb_cl.Text = "晨练：" + saveWeather.Cl;
            tb_tr.Text = "旅行：" + saveWeather.Tr;
            tb_ls.Text = "晾晒：" + saveWeather.Ls;
            tb_gm.Text = "感冒易发率：" + saveWeather.Gm;
            tb_cy.Text = "穿衣建议：" + saveWeather.Cy;         
            img_Today.Weather = saveWeather.Weather;
            itemsViewSource.Source = saveWeather.Forword;
            UpdateTile();


        }


        /// <summary>
        /// 设置中更改城市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void addCity_Closed(object sender, object e)
        {
            if (askl.Result == true)//点确认关闭设置
            {
                if (saveWeather == null)//第一次打开程序
                {
                    saveWeather = new WeatherInfo();
                    await ApplicationData.Current.LocalFolder.CreateFileAsync("save.data");
                }
                saveWeather.Cityid = askl.outCity.Code;
                await GetWeather();   
               
            }         
        }

        /// <summary>
        /// 刷新按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_refresh_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await GetWeather();
        }

        #region 更改背景 背景渐变
        private void ChangeBg()
        {
            oldColor = nowColor;
            nowColor.A = 100;
            var brush = new SolidColorBrush(oldColor);
            background.Background = brush;
            newColor = GetColor();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        void timer_Tick(object sender, object e)
        {
            if (Math.Abs(nowColor.R - newColor.R) > 6)
            {
                if (newColor.R > oldColor.R)
                    nowColor.R += 5;
                else
                    nowColor.R -= 5;
            }
            if (Math.Abs(nowColor.G - newColor.G) > 6)
            {
                if (newColor.G > oldColor.G)
                    nowColor.G += 5;
                else
                    nowColor.G -= 5;
            }
            if (Math.Abs(nowColor.B - newColor.B) > 6)
            {
                if (newColor.B > oldColor.B)
                    nowColor.B += 5;
                else
                    nowColor.B -= 5;
            }
            var brush = new SolidColorBrush(nowColor);
            background.Background = brush;
            if (Math.Abs(nowColor.R - newColor.R) < 6 && Math.Abs(nowColor.B - newColor.B) < 6 && Math.Abs(nowColor.G - newColor.G) < 6)
            {
                timer.Stop();
                oldColor = nowColor;
            }

        }
        private Color GetColor()
        {
            Random rd = new Random();
            Color color = new Color();
            color.R = (byte)rd.Next(0, 256);
            color.G = (byte)rd.Next(0, 256);
            color.B = (byte)rd.Next(0, 256);
            color.A = 100;
            return color;
        } 
        #endregion


        #region 更新磁贴
        /// <summary>
        /// 更新磁贴
        /// </summary>
        void UpdateTile()
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText03);
            XmlDocument wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideSmallImageAndText02);

            XmlNodeList textNodes = tileXml.GetElementsByTagName("text");
            textNodes[0].InnerText = saveWeather.City;
            textNodes[1].InnerText = saveWeather.Weather;
            textNodes[2].InnerText = saveWeather.Temp;
            textNodes[3].InnerText = saveWeather.Wind;

            XmlNodeList wideTextNodes = wideTileXml.GetElementsByTagName("text");
            XmlNodeList wideImageNodes = wideTileXml.GetElementsByTagName("image");


            ((XmlElement)wideImageNodes[0]).SetAttribute("src", ShowImage.GetImageSrc(saveWeather.Weather));
            ((XmlElement)wideImageNodes[0]).SetAttribute("alt", "red graphic");
            wideTextNodes[0].InnerText = saveWeather.Weather;
            wideTextNodes[1].InnerText = saveWeather.City;
            wideTextNodes[2].InnerText = saveWeather.Temp;
            wideTextNodes[3].InnerText = saveWeather.Wind;


            for (int i = 0; i < 5; i++)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(tileXml));
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(wideTileXml));
            }

        } 
        #endregion

        /// <summary>
        /// 从AppBar打开城市管理窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            OpenAddCity();
        }



    }
}
