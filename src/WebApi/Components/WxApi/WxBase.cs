using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SystemCommonLibrary.Json;
using WebApi.Models;

namespace WebApi.Components.WxApi
{
    public static class WxBase
    {
        public static string BaseUrl = "https://api.weixin.qq.com/";
        private static string _tokenUrl = $"cgi-bin/token?grant_type=client_credential&appid={HelperConfig.Current.WxAppid}&secret={HelperConfig.Current.WxSecret}";

        private static string _accessToken = string.Empty;
        private static DateTime _expireTime = DateTime.Now.AddHours(-2);

        public static string GetAccessToken()
        {
            if (DateTime.Now >= _expireTime)
            {
                try
                {
                    var client = new HttpClient() { BaseAddress = new Uri(BaseUrl) };
                    var response = client.GetAsync(_tokenUrl).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        dynamic json = DynamicJson.Parse(result);
                        _accessToken = json.access_token;
                        _expireTime = DateTime.Now.AddSeconds(json.expires_in);

                        return _accessToken;
                    }
                    else
                        throw new Exception("微信服务器错误");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
                return _accessToken;


        }
    }
}
