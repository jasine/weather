using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Weather
{
    public class WeatherHelper
    {




        //在原作者基础上进行的修改，调用GetSmartWeatherApi即可获取完整url

        //_baseUrl一般为http://webapi.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}



        public static String GetSmartWeatherApi(String _baseUrl, String _areaId, String _type, String _date, String _appID, String _privateKey)
        {

            String publicKey = String.Format(_baseUrl, _areaId, _type, _date, _appID);

            String fullUrl = String.Format(_baseUrl, _areaId, _type, _date, _appID.Substring(0, 6));

            //fullUrl = String.Format("{0}&key={1}", fullUrl, GetSmartWeatherKeyCode(publicKey, _privateKey));
            fullUrl = String.Format("{0}&key={1}", fullUrl, Sha1Encrypt(publicKey, _privateKey));

            return fullUrl;

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
