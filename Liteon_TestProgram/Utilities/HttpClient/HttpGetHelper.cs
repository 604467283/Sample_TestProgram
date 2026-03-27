using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.HttpClient
{
    internal class HttpGetHelper
    {

        //private readonly HttpClient client;
        //private readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(30) // 设置超时时间为30秒
        //};


        private readonly System.Net.Http.HttpClient client;

        public HttpGetHelper()
        {
            var handler = new HttpClientHandler();
            // 关闭SSL验证
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            client = new  System.Net.Http.HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30) // 设置超时时间为30秒
            };
        }



        public async Task<bool> GetDataFromUrlAsync(string url, string str_expect)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                UIHandleHelper.ShowRunLog($"Http Get Recv: {responseBody}");

                if ( responseBody.Contains(str_expect) )
                {
                    return true;
                }
            }
            catch (HttpRequestException e)
            {
               UIHandleHelper.ShowRunLog("Http Get Request error: " + e.Message, true);
                return false;
            }

            return false;
        }
    }
}
