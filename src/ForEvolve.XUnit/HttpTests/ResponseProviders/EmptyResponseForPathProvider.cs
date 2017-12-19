﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ForEvolve.XUnit.HttpTests
{
    public class EmptyResponseForPathProvider : IResponseProvider
    {
        private readonly string _expectedPath;
        private readonly string _expectedMethod;

        public EmptyResponseForPathProvider(string expectedMethod, string expectedPath)
        {
            _expectedPath = expectedPath ?? throw new ArgumentNullException(nameof(expectedPath));
            _expectedMethod = expectedMethod ?? throw new ArgumentNullException(nameof(expectedMethod));
        }

        public string ResponseText(HttpContext context)
        {
            var samePath = context.Request.Path.Value == _expectedPath;
            var sameMethod = context.Request.Method == _expectedMethod;
            if (samePath && sameMethod)
            {
                return "";
            }
            throw new WrongEndpointException(
                _expectedMethod, _expectedPath,
                context.Request.Method, context.Request.Path.Value
            );
        }
    }
}
