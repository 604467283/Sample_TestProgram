using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities.HttpClient
{
    internal class HttpWithBodyHelper
    {
        /*旧方法
        //private readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(30) // 设置超时时间为30秒
        //};

        private readonly System.Net.Http.HttpClient client;

        public HttpWithBodyHelper()
        {
            var handler = new HttpClientHandler();
            // 关闭SSL验证
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            client = new System.Net.Http.HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30) // 设置超时时间为30秒
            };
        }


        public HttpContent StringToByteArrayContent(string content, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] byteArray = encoding.GetBytes(content);
            return new ByteArrayContent(byteArray);
        }

        public  async Task<string> PostJsonAsync(string url, string requestBody)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            try
            {
                //var json = SerializeToJson(requestBody);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // 设置 Content-Type 为 application/json
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                //HttpResponseMessage response = await client.PostAsync(url, StringToByteArrayContent(requestBody));
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var str_Recv = await response.Content.ReadAsStringAsync();
                //UIHandleHelper.ShowRunLog($"Recv http data:\r\n {await response.Content.ReadAsStringAsync()}");
                return str_Recv;
            }
            catch (TaskCanceledException e)
            {
                UIHandleHelper.ShowRunLog($"Post request timed out:\r\n {e.Message}", true);
                return null;
            }
            catch (HttpRequestException e)
            {
                UIHandleHelper.ShowRunLog($"Post request error:\r\n {e.Message}", true);
                return null;
            }
        }

        private  string SerializeToJson(object obj)
        {
            // 使用 System.Text.Json 进行序列化
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
        */




        private readonly System.Net.Http.HttpClient client;

        public HttpWithBodyHelper()
        {
            var handler = new HttpClientHandler();

            // 增强 SSL/TLS 配置
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                // 接受所有证书（类似于 Postman 关闭 SSL 验证）
                return true;
            };

            // 禁用证书吊销检查
            handler.CheckCertificateRevocationList = false;

            // 禁用代理，避免代理干扰
            handler.UseProxy = false;
            handler.Proxy = null;

            // 启用压缩
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            client = new System.Net.Http.HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            // 设置更完整的默认请求头
            ConfigureDefaultHeaders();
        }

        private void ConfigureDefaultHeaders()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; TestClient/1.0)");
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        public HttpContent StringToByteArrayContent(string content, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] byteArray = encoding.GetBytes(content);
            return new ByteArrayContent(byteArray);
        }

        public async Task<string> PostJsonAsync(string url, string requestBody)
        {
            // 输入验证
            if (string.IsNullOrWhiteSpace(url))
            {
                UIHandleHelper.ShowRunLog("Error: URL is null or empty", true);
                return null;
            }

            // 最大重试次数
            int maxRetries = 3;

            for (int retry = 0; retry < maxRetries; retry++)
            {
                try
                {
                    UIHandleHelper.ShowRunLog($"HTTP POST Attempt {retry + 1} to {url}");

                    // 检查网络连接
                    if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    {
                        UIHandleHelper.ShowRunLog("Network is not available", true);
                        if (retry < maxRetries - 1)
                        {
                            await Task.Delay(1000 * (retry + 1));
                            continue;
                        }
                        return null;
                    }

                    // 配置请求头
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // 创建请求内容
                    var content = new StringContent(requestBody ?? string.Empty, Encoding.UTF8, "application/json");

                    // 使用 CancellationToken 进行超时控制
                    using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                    {
                        HttpResponseMessage response = await client.PostAsync(url, content, cts.Token);

                        // 记录响应状态码
                        UIHandleHelper.ShowRunLog($"HTTP Status: {(int)response.StatusCode} {response.StatusCode}");

                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            UIHandleHelper.ShowRunLog($"Request successful");
                            return responseContent;
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            UIHandleHelper.ShowRunLog($"HTTP Error {(int)response.StatusCode}: {response.StatusCode}");

                            // 对于 404 等错误，不需要重试
                            if (response.StatusCode == HttpStatusCode.NotFound ||
                                response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                UIHandleHelper.ShowRunLog($"Non-retryable error, stopping retries");
                                return null;
                            }

                            // 其他服务器错误可以重试
                            if (retry < maxRetries - 1)
                            {
                                UIHandleHelper.ShowRunLog($"Will retry after delay...");
                                await Task.Delay(1000 * (retry + 1));
                                continue;
                            }

                            return null;
                        }
                    }
                }
                catch (TaskCanceledException ex)
                {
                    UIHandleHelper.ShowRunLog($"Request timed out (attempt {retry + 1}): {ex.Message}", true);

                    if (retry == maxRetries - 1)
                    {
                        UIHandleHelper.ShowRunLog($"All retries failed due to timeout", true);
                        return null;
                    }

                    await Task.Delay(1000 * (retry + 1));
                }
                catch (HttpRequestException ex)
                {
                    UIHandleHelper.ShowRunLog($"HTTP request error (attempt {retry + 1}): {ex.Message}", true);

                    // 提供更详细的错误信息
                    if (ex.InnerException != null)
                    {
                        UIHandleHelper.ShowRunLog($"Inner exception: {ex.InnerException.Message}", true);
                    }

                    if (retry == maxRetries - 1)
                    {
                        UIHandleHelper.ShowRunLog($"All retries failed due to HTTP error", true);
                        return null;
                    }

                    await Task.Delay(1000 * (retry + 1));
                }
                catch (Exception ex)
                {
                    UIHandleHelper.ShowRunLog($"Unexpected error (attempt {retry + 1}): {ex.Message}", true);

                    if (retry == maxRetries - 1)
                    {
                        UIHandleHelper.ShowRunLog($"All retries failed due to unexpected error", true);
                        return null;
                    }

                    await Task.Delay(1000 * (retry + 1));
                }
            }

            return null;
        }

        private string SerializeToJson(object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }




    }
}
