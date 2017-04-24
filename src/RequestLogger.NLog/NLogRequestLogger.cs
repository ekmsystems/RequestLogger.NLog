using System;
using NLog;
using RequestLogger.NLog.Formatters;

namespace RequestLogger.NLog
{
    public class NLogRequestLogger : IRequestLogger
    {
        private readonly ILogger _logger;
        private readonly ILogFormatter _formatter;

        public NLogRequestLogger(ILogger logger, ILogFormatter formatter = null)
        {
            _logger = logger;
            _formatter = formatter ?? new DefaultLogFormatter();
        }

        public void Log(RequestData requestData, ResponseData responseData)
        {
            var info = _formatter.Format(requestData, responseData);

            info.LoggerName = _logger.Name;
            
            _logger.Log(info);
        }

        public void LogError(RequestData requestData, ResponseData responseData, Exception ex)
        {
            var info = _formatter.Format(requestData, responseData, ex);

            info.LoggerName = _logger.Name;

            _logger.Log(info);
        }
    }
}
