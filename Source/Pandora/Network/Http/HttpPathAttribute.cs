using System;

namespace Pandora.Network.Http
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPathAttribute : Attribute
    {
        public string Path { get; }

        public HttpPathAttribute(string path)
        {
            Path = path;
        }
    }
}
