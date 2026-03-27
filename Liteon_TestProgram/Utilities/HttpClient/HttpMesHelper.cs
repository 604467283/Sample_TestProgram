using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.HttpClient
{
    internal class HttpMesHelper
    {
        public enum MesPostType
        {
            ADD,
            QUERY,
        }

        private readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30) // 设置超时时间为30秒
        };

        public async Task<string> PostWithTimeoutAsync(MesPostType mesPostType, string strSn, string strKey, string strValue = "")
        {
            var url = "http://10.141.33.98/mesinterface/go.aspx";
            CancellationTokenSource cts = new CancellationTokenSource();

            var content = new FormUrlEncodedContent(new[]
               {
                new KeyValuePair<string, string>("c", mesPostType.ToString()),
                new KeyValuePair<string, string>("f", "attr"),
                new KeyValuePair<string, string>("sn", strSn),
                new KeyValuePair<string, string>("key", strKey),
                new KeyValuePair<string, string>("value", strValue)
            });

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content, cts.Token);
                response.EnsureSuccessStatusCode();

                var str_Recv = await response.Content.ReadAsStringAsync();
                UIHandleHelper.ShowRunLog($"Recv http data:\r\n {await response.Content.ReadAsStringAsync()}");
                return str_Recv;
            }
            catch (TaskCanceledException e)
            {
                UIHandleHelper.ShowRunLog($"Request timed out:\r\n {e.Message}", true);
                return null;
            }
            catch (HttpRequestException e)
            {
                UIHandleHelper.ShowRunLog($"Request error:\r\n {e.Message}", true);
                return null;
            }
        }


    }
}
