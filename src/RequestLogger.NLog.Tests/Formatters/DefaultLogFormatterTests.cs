using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NLog;
using NUnit.Framework;
using RequestLogger.NLog.Formatters;

namespace RequestLogger.NLog.Tests.Formatters
{
    [TestFixture]
    [Parallelizable]
    public class DefaultLogFormatterTests
    {
        [SetUp]
        public void SetUp()
        {
            _formatter = new DefaultLogFormatter();
        }

        private ILogFormatter _formatter;

        [Test]
        public void Format_ShouldReturn_LogEventInfo()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData();

            var result = _formatter.Format(requestData, responseData);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<LogEventInfo>(result);
        }

        [Test]
        public void Format_ShouldSet_LevelToInfo()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData();

            var result = _formatter.Format(requestData, responseData);

            Assert.AreEqual(LogLevel.Info, result.Level);
        }

        [Test]
        public void Format_ShouldSet_Message()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData();

            var result = _formatter.Format(requestData, responseData);

            Assert.AreEqual("Request processed", result.Message);
        }

        [Test]
        public void Format_ShouldSet_PropertiesForRequestData()
        {
            var requestData = new RequestData
            {
                HttpMethod = "GET",
                Url = new Uri("http://ekm.com/"),
                Header = new Dictionary<string, string[]>
                {
                    {"Content-Type", new[] {"application/json"}}
                },
                Content = Encoding.UTF8.GetBytes("Ping")
            };
            var responseData = new ResponseData();

            var result = _formatter.Format(requestData, responseData);

            Assert.AreEqual(requestData.HttpMethod, result.Properties["HttpMethod"]);
            Assert.AreEqual(requestData.Url, result.Properties["Uri"]);
            Assert.AreEqual("Content-Type: [application/json]", result.Properties["RequestHeader"]);
            Assert.AreEqual("Ping", result.Properties["RequestBody"]);
        }

        [Test]
        public void Format_ShouldSet_PropertiesForResponseData()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData
            {
                StatusCode = 200,
                ReasonPhrase = "200 OK",
                Header = new Dictionary<string, string[]>
                {
                    {"Content-Type", new[] {"application/json"}}
                },
                Content = Encoding.UTF8.GetBytes("Pong")
            };

            var result = _formatter.Format(requestData, responseData);

            Assert.AreEqual(responseData.StatusCode, result.Properties["StatusCode"]);
            Assert.AreEqual(responseData.ReasonPhrase, result.Properties["ReasonPhrase"]);
            Assert.AreEqual("Content-Type: [application/json]", result.Properties["ResponseHeader"]);
            Assert.AreEqual("Pong", result.Properties["ResponseBody"]);
        }

        [Test]
        public void Format_WithException_ShouldSet_ExceptionProperty()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData();
            var ex = new Exception("Something went wrong");

            var result = _formatter.Format(requestData, responseData, ex);

            Assert.AreEqual(ex, result.Exception);
        }

        [Test]
        public void Format_WithException_ShouldSet_LevelToError()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData();
            var ex = new Exception();

            var result = _formatter.Format(requestData, responseData, ex);

            Assert.AreEqual(LogLevel.Error, result.Level);
        }

        [Test]
        public void Format_WithException_ShouldSet_Message()
        {
            var requestData = new RequestData();
            var responseData = new ResponseData();
            var ex = new Exception();

            var result = _formatter.Format(requestData, responseData, ex);

            Assert.AreEqual("An error occurred", result.Message);
        }

        [Test]
        public void Format_ShouldConvert_HeaderToString()
        {
            var headers = new Dictionary<string, string[]>
            {
                {"Header-1", new[] {"value1", "value2"}},
                {"Header-2", new[] {"value3", "value4"}}
            };
            var requestData = new RequestData
            {
                Header = new ReadOnlyDictionary<string, string[]>(headers)
            };
            var responseData = new ResponseData
            {
                Header = new ReadOnlyDictionary<string, string[]>(headers)
            };
            const string expected = "Header-1: [value1, value2], Header-2: [value3, value4]";

            var result = _formatter.Format(requestData, responseData);

            Assert.AreEqual(expected, result.Properties["RequestHeader"]);
            Assert.AreEqual(expected, result.Properties["ResponseHeader"]);
        }

        [Test]
        public void Format_ShouldConvert_ContentToString()
        {
            const string message = "EKM";
            var content = Encoding.UTF8.GetBytes(message);
            var requestData = new RequestData
            {
                Content = content
            };
            var responseData = new ResponseData
            {
                Content = content
            };

            var result = _formatter.Format(requestData, responseData);

            Assert.AreEqual(message, result.Properties["RequestBody"]);
            Assert.AreEqual(message, result.Properties["ResponseBody"]);
        }
    }
}
