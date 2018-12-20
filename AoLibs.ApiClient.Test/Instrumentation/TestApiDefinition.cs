using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.ApiClient.Interfaces;

namespace AoLibs.ApiClient.Test.Instrumentation
{
    public class TestApiDefinition : IApiDefinition
    {
        public string BaseAddress { get; } = "https://reqres.in/api";
    }
}
