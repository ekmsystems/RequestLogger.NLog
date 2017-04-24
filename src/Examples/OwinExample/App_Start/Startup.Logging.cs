using NLog;
using Owin;
using RequestLogger.NLog;
using RequestLogger.Owin;

namespace OwinExample
{
    public partial class Startup
    {
        private static void ConfigureLogging(IAppBuilder app)
        {
            var logger = LogManager.GetLogger("RequestLogger");
            var requestLogger = new NLogRequestLogger(logger);

            app.UseRequestLoggerMiddleware(requestLogger);
        }
    }
}