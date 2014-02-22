using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.ApplicationSettings;
namespace Weather
{
    public class CreatePopup
    {
        public static Popup Create(UserControl element, double width)
        {
            Popup p = new Popup();
            p.Child = element;
            p.IsLightDismissEnabled = true;
            p.ChildTransitions = new TransitionCollection();
            p.ChildTransitions.Add(new PaneThemeTransition()    //声明边缘 UI（如应用程序栏）的边缘转换位置。
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                        EdgeTransitionLocation.Right :
                        EdgeTransitionLocation.Left
            });//检查SettingsPane的edge,有些国家的超级菜单在左边。

            element.Width = width;
            element.Height = Window.Current.Bounds.Height;           
            p.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (Window.Current.Bounds.Width - width) : 0);//设置距离左边的边距
            p.SetValue(Canvas.TopProperty, 0);
            return p;
        }
    }
}
