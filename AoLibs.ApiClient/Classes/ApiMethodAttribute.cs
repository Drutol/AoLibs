using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace AoLibs.ApiClient
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiMethodAttribute : Attribute
    {
        public enum HttpVerb
        {
            Get,
            Post,
            Put,
            Head,
            Delete,
            Patch,
        }

        public string Path { get; set; }

        public HttpMethod HttpMethod { get; private set; }

        public ApiMethodAttribute(HttpVerb verb)
        {
            HttpMethod = new HttpMethod(verb.ToString().ToUpper());
        }

        public ApiMethodAttribute(HttpVerb verb, string path)
        {
            Path = path;
            HttpMethod = new HttpMethod(verb.ToString().ToUpper());
        }

        public ApiMethodAttribute([CallerMemberName] string methodName = null)
        {
            if(methodName.StartsWith("Get"))
                HttpMethod = HttpMethod.Get;
            else if(methodName.StartsWith("Post"))
                HttpMethod = HttpMethod.Post;
            else if(methodName.StartsWith("Delete"))
                HttpMethod = HttpMethod.Delete;
            else if(methodName.StartsWith("Head"))
                HttpMethod = HttpMethod.Head;
            else if(methodName.StartsWith("Put"))
                HttpMethod = HttpMethod.Put;
            else if(methodName.StartsWith("Patch"))
                HttpMethod = new HttpMethod("PATCH");
        }
    }
}
