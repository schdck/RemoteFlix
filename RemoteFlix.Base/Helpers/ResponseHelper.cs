using RemoteFlix.Base.Classes;
using RemoteFlix.Base.Enums;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace RemoteFlix.Base.Helpers
{
    public static class ResponseHelper
    {
        private static readonly string HtmlTemplate;

        static ResponseHelper()
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream($"RemoteFlix.Base.Resources.remoteflix.html"))
            using (var reader = new StreamReader(stream))
            {
                HtmlTemplate = reader.ReadToEnd();
            }
        }

        public static void SendResponse(HttpListenerContext context, string html)
        {
            if (html != null)
            {
                var content =
                    HtmlTemplate
                        .Replace("{PAGE_CONTENT}", html)
                        .Replace("{COMPUTER_NAME}", Environment.MachineName);

                html = content;
            }

            var buffer = html != null ? Encoding.UTF8.GetBytes(html) : null;

            SendResponse(context, buffer);
        }

        public static void SendResponse(HttpListenerContext context, byte[] buffer)
        {
            try
            {
                using (var response = context.Response)
                {
                    if (buffer != null)
                    {
                        using (var output = response.OutputStream)
                        {
                            response.ContentLength64 = buffer.Length;
                            output.Write(buffer, 0, buffer.Length);
                        }
                    }

                    Logger.Instance.Log(LogLevel.Message, $"Responded with 200 (OK).");
                    response.StatusCode = 200;
                    response.KeepAlive = false;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Error, $"{e.GetType()} when sending response. {e.Message}");
            }
        }

        public static void SendEmptyResponse(HttpListenerContext context, int httpStatus)
        {
            try
            {
                using (var response = context.Response)
                {
                    response.StatusCode = httpStatus;
                    response.KeepAlive = false;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Error, $"{e.GetType()} when sending response. {e.Message}");
            }
        }
    }
}
