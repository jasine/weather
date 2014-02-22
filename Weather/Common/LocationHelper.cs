using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Weather.Common
{
    class LocationHelper
    {
        private Geolocator _geolocator = null;
        private CancellationTokenSource _cts = null;

        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
       // MainPage rootPage = MainPage.Current;

        public LocationHelper()
        {
            _geolocator = new Geolocator();
        }


        /// <summary>
        /// Invoked immediately before the Page is unloaded and is no longer the current source of a parent Frame.
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative
        /// of the navigation that will unload the current Page unless canceled. The
        /// navigation can potentially be canceled by setting e.Cancel to true.
        /// </param>
        //protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        //{
        //    if (_cts != null)
        //    {
        //        _cts.Cancel();
        //        _cts = null;
        //    }

        //    base.OnNavigatingFrom(e);
        //}

 
        async public void GetGeolocation()
        {
            try
            {
                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;

                //rootPage.NotifyUser("Waiting for update...", NotifyType.StatusMessage);

                // Carry out the operation
                Geoposition pos = await _geolocator.GetGeopositionAsync().AsTask(token);
                GetCity(pos.Coordinate.Latitude,pos.Coordinate.Longitude);
                //rootPage.NotifyUser("Updated", NotifyType.StatusMessage);

                //ScenarioOutput_Latitude.Text = pos.Coordinate.Latitude.ToString();
                //ScenarioOutput_Longitude.Text = pos.Coordinate.Longitude.ToString();
                //ScenarioOutput_Accuracy.Text = pos.Coordinate.Accuracy.ToString();
            }
            catch (System.UnauthorizedAccessException)
            {
                //rootPage.NotifyUser("Disabled", NotifyType.StatusMessage);
                //Disable
                //ScenarioOutput_Latitude.Text = "No data";
                //ScenarioOutput_Longitude.Text = "No data";
                //ScenarioOutput_Accuracy.Text = "No data";
            }
            catch (TaskCanceledException)
            {
                //rootPage.NotifyUser("Canceled", NotifyType.StatusMessage);
            }
            finally
            {
                _cts = null;
            }
        }

        /// <summary>
        /// This is the click handler for the 'CancelGetGeolocation' button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelGetGeolocation()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            //GetGeolocationButton.IsEnabled = true;
            //CancelGetGeolocationButton.IsEnabled = false;
        }

        public async void GetCity(double lat,double lon)
        {
            HttpClient hc = new HttpClient();
            try
            {
                string text = await hc.GetStringAsync("http://ditu.google.cn/maps/geo?output=csv&key=abcdef&q=" + lat + "," + lon);
                text = text.Substring(text.IndexOf('"') + 1);
                text = text.Substring(0, text.IndexOf('"'));
                text = text.Contains("中国") ? text.Remove(0, 2) : text;

            }
            catch (Exception)
            {
            }
        }


    }
}
