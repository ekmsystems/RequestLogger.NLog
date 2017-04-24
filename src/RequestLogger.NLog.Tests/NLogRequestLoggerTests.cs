using System;
using Moq;
using NLog;
using NUnit.Framework;
using RequestLogger.NLog.Formatters;

namespace RequestLogger.NLog.Tests
{
    [TestFixture]
    [Parallelizable]
    public class NLogRequestLoggerTests
    {
        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger>();
            _formatter = new Mock<ILogFormatter>();
            _requestLogger = new NLogRequestLogger(_logger.Object, _formatter.Object);
        }

        private Mock<ILogger> _logger;
        private Mock<ILogFormatter> _formatter;
        private NLogRequestLogger _requestLogger;

        [Test]
        public void Log_ShouldCall_Logger_Log()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData();

            _formatter
                .Setup(x => x.Format(requestData, responseData, null))
                .Returns(new LogEventInfo());

            _requestLogger.Log(requestData, responseData);

            _logger.Verify(x => x.Log(It.IsAny<LogEventInfo>()), Times.Once);
        }

        [Test]
        public void Log_ShouldSet_LoggerName()
        {
            const string loggerName = "MyLogger";
            var requestData = new RequestData();
            var responseData = new ResponseData();

            _formatter
                .Setup(x => x.Format(requestData, responseData, null))
                .Returns(new LogEventInfo());
            _logger
                .SetupGet(x => x.Name)
                .Returns(loggerName);

            _requestLogger.Log(requestData, responseData);

            _logger.Verify(x => x.Log(It.Is<LogEventInfo>(y => y.LoggerName == loggerName)), Times.Once);
        }

        [Test]
        public void LogError_ShouldCall_Logger_Log()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData();
            var ex = new Exception();

            _formatter
                .Setup(x => x.Format(requestData, responseData, ex))
                .Returns(new LogEventInfo());

            _requestLogger.LogError(requestData, responseData, ex);

            _logger.Verify(x => x.Log(It.IsAny<LogEventInfo>()), Times.Once);
        }

        [Test]
        public void LogError_ShouldSet_LoggerName()
        {
            const string loggerName = "MyLogger";
            var requestData = new RequestData();
            var responseData = new ResponseData();
            var ex = new Exception();

            _formatter
                .Setup(x => x.Format(requestData, responseData, ex))
                .Returns(new LogEventInfo());
            _logger
                .SetupGet(x => x.Name)
                .Returns(loggerName);

            _requestLogger.LogError(requestData, responseData, ex);

            _logger.Verify(x => x.Log(It.Is<LogEventInfo>(y => y.LoggerName == loggerName)), Times.Once);
        }
    }
}
