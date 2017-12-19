﻿using ForEvolve.XUnit.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.XUnit.HttpTests
{
    public class JsonPathResponseProviderTest
    {
        private readonly JsonPathResponseProvider _providerUnderTest;

        private readonly string _expectedMethod;
        private readonly string _expectedPath;
        private readonly JsonPathResponseObject _successObject;

        private readonly string _expectedSuccessResponseText;

        private readonly HttpContextHelper _httpContextHelper;

        public JsonPathResponseProviderTest()
        {
            _httpContextHelper = new HttpContextHelper();

            _expectedMethod = "POST";
            _expectedPath = "/whatever";
            _successObject = new JsonPathResponseObject();

            _expectedSuccessResponseText = JsonConvert.SerializeObject(_successObject);

            _providerUnderTest = new JsonPathResponseProvider(
                _expectedMethod,
                _expectedPath,
                _successObject
            );
        }

        public class JsonPathResponseObject { }

        public class ResponseText : JsonPathResponseProviderTest
        {
            [Fact]
            public void Should_throw_WrongEndpointException_when_Method_is_incorrect()
            {
                // Arrange
                _httpContextHelper.HttpRequest.Method = "GET";
                _httpContextHelper.HttpRequest.Path = _expectedPath;

                // Act & Assert
                var exception = Assert.Throws<WrongEndpointException>(
                    () => _providerUnderTest.ResponseText(_httpContextHelper.HttpContextMock.Object)
                );
                Assert.Equal(_expectedMethod, exception.ExpectedMethod);
                Assert.Equal(_expectedPath, exception.ExpectedPath);
                Assert.Equal("GET", exception.ActualMethod);
                Assert.Equal(_expectedPath, exception.ActualPath);
            }

            [Fact]
            public void Should_throw_WrongEndpointException_when_Path_is_incorrect()
            {
                // Arrange
                _httpContextHelper.HttpRequest.Method = _expectedMethod;
                _httpContextHelper.HttpRequest.Path = "/some/other/path";

                // Act & Assert
                var exception = Assert.Throws<WrongEndpointException>(
                    () => _providerUnderTest.ResponseText(_httpContextHelper.HttpContextMock.Object)
                );
                Assert.Equal(_expectedMethod, exception.ExpectedMethod);
                Assert.Equal(_expectedPath, exception.ExpectedPath);
                Assert.Equal(_expectedMethod, exception.ActualMethod);
                Assert.Equal("/some/other/path", exception.ActualPath);
            }

            [Fact]
            public void Should_return_success_when_Path_and_Method_are_correct()
            {
                // Arrange
                _httpContextHelper.HttpRequest.Method = _expectedMethod;
                _httpContextHelper.HttpRequest.Path = _expectedPath;

                // Act
                var result = _providerUnderTest.ResponseText(_httpContextHelper.HttpContextMock.Object);

                // Assert
                Assert.Equal(_expectedSuccessResponseText, result);
            }
        }
    }
}
