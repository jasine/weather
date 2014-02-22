using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace Weather
{
    public sealed partial class ShowImage : UserControl
    {
        string weather;
        BitmapImage img1,img2;
        DispatcherTimer timer;
        public string ImgPath
        {
            get {
                return (string)GetValue(ImgPathProperty); }
            set
            {
                SetValue(ImgPathProperty,value);
            }
        }
        public static readonly DependencyProperty ImgPathProperty =
            DependencyProperty.Register("ImgPath", typeof(string), typeof(ShowImage), new PropertyMetadata(null));
        
        public string Weather
        {
            get 
            { 
                return weather;
            }
            set 
            {
                weather = value;
                if (weather.Contains("转"))
                {
                    string[] temp = weather.Split('转');
                    img1 = GetImage(temp[0]); 
                    img.Source = img1;
                    img2 = GetImage(temp[1]);                   
                    sbchange.Begin();
                }
                else
                {
                    img1 = GetImage(weather);
                    img2 = img1;
                    img.Source = img1;
                    //sbbig.Begin();
                    sbchange.Begin();
                }
                
            }
        }
  

        public ShowImage()
        {
            this.InitializeComponent();
            this.SetBinding(this.tb_hindden, TextBlock.TextProperty, "ImgPath");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, object e)
        {
            if (tb_hindden.Text != "")
            {
                Weather = tb_hindden.Text;
                stbig.CenterX = img.Width / 2;
                stbig.CenterY = img.Height / 2;
                timer.Stop();
            }
        }
        private void SetBinding(FrameworkElement obj, DependencyProperty p, string path)
        {
            Binding b = new Binding();
            b.Source = this;
            b.Path = new PropertyPath(path);
            b.Mode = BindingMode.OneWay;
            obj.SetBinding(p, b);
            
        }


        #region 由天气信息获取图片或图片路径GetImage(string weather)，public static string GetImageSrc

        private BitmapImage GetImage(string weather)
        {
            return new BitmapImage(new Uri(GetImageSrc(weather)));
        }

        public static string GetImageSrc(string weather)
        {
            string img = "";
            switch (weather)
            {
                case "晴":
                    img = "q";
                    break;
                case "阴":
                    img = "y";
                    break;
                case "多云":
                    img = "dy";
                    break;

                case "阵雨":
                    img = "zy";
                    break;
                case "雷阵雨":
                    img = "lzy";
                    break;
                case "雷阵雨伴有冰雹"://不知道怎么报的
                    img = "lzyjbb";
                    break;

                case "雨夹雪"://不知道怎么报的
                    img = "yjx";
                    break;

                case "小雨":
                    img = "xy";
                    break;
                case "中雨":
                    img = "zhongy";
                    break;
                case "大雨":
                    img = "day";
                    break;
                case "暴雨":
                    img = "by";
                    break;
                case "大暴雨":
                    img = "dby";
                    break;
                case "特大暴雨":
                    img = "tdby";
                    break;

                case "阵雪":
                    img = "zx";
                    break;
                case "小雪":
                    img = "xiaox";
                    break;
                case "中雪":
                    img = "zhongx";
                    break;
                case "大雪":
                    img = "dx";
                    break;
                case "暴雪":
                    img = "bx";
                    break;


                case "雾":
                    img = "w";
                    break;
                case "冻雨":
                    img = "dongy";
                    break;

                case "沙尘暴"://不知道怎么报的
                    img = "scb";
                    break;
                case "强沙尘暴"://不知道怎么报的
                    img = "qscb";
                    break;
                case "浮尘"://不知道怎么报的
                    img = "fc";
                    break;
                case "扬沙"://不知道怎么报的
                    img = "ys";
                    break;

                default:
                    img = "dy";
                    break;
            }
            return "ms-appx:Images/" + img + ".png";
        }
        
        #endregion


        #region 动画逻辑

        private void DoubleAnimation_Completed_1(object sender, object e)
        {
            img.Source = img.Source == img1 ? img2 : img1;
            sbchange2.Begin();
        }

        private void DoubleAnimation_Completed_2(object sender, object e)
        {

            sbchange.Begin();
        }

        private void sbbig2_Completed_1(object sender, object e)
        {
            sbbig.Begin();
        }

        private void sbbig_Completed_1(object sender, object e)
        {
            sbbig2.Begin();
        }

        
        #endregion
    }
}
