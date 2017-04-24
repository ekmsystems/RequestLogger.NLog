using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace RequestLogger.NLog.Formatters
{
    public class DefaultLogFormatter : ILogFormatter
    {
        public LogEventInfo Format(RequestData requestData, ResponseData responseData, Exception ex = null)
        {
            var info = new LogEventInfo
            {
                Level = ex == null ? LogLevel.Info : LogLevel.Error,
                Message = ex == null ? "Request processed" : "An error occurred",
                Exception = ex
            };

            PopulateRequestProperties(requestData, info);
            PopulateResponseProperties(responseData, info);

            return info;
        }
        private static void PopulateRequestProperties(RequestData requestData, LogEventInfo info)
        {
            info.Properties["HttpMethod"] = requestData.HttpMethod;
            info.Properties["Uri"] = requestData.Url;
            info.Properties["RequestHeader"] = ParseHeader(requestData.Header);
            info.Properties["RequestBody"] = ParseContent(requestData.Content);
        }

        private static void PopulateResponseProperties(ResponseData responseData, LogEventInfo info)
        {
            info.Properties["StatusCode"] = responseData.StatusCode;
            info.Properties["ReasonPhrase"] = responseData.ReasonPhrase;
            info.Properties["ResponseHeader"] = ParseHeader(responseData.Header);
            info.Properties["ResponseBody"] = ParseContent(responseData.Content);
        }

        private static string ParseHeader(IDictionary<string, string[]> header)
        {
            var values = (header ?? new Dictionary<string, string[]>())
                .Select(x => $"{x.Key}: [{string.Join(", ", x.Value)}]")
                .ToArray();

            return string.Join(", ", values);
        }

        private static string ParseContent(byte[] content)
        {
            return Encoding.UTF8.GetString(content ?? new byte[] { });
        }
    }
}
