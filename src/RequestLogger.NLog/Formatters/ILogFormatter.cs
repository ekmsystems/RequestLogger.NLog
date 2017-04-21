using System;
using NLog;

namespace RequestLogger.NLog.Formatters
{
    public interface ILogFormatter
    {
        LogEventInfo Format(RequestData requestData, ResponseData responseData, Exception ex = null);
    }
}
