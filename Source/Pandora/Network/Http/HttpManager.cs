using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Pandora.Managers;

namespace Pandora.Network.Http
{
    public static class HttpManager
    {
        public delegate void PathHandler(string[] parameters, HttpListenerContext context);

        private static readonly Dictionary<string, PathHandler> pathHandlers = new Dictionary<string, PathHandler>();
        private static HttpListener listener;

        public static void Initialise()
        {
            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add("http://*:2200/");
                listener.Start();
                listener.BeginGetContext(OnConnection, null);
            }
            catch
            {
                LogManager.Write("HTTP", "An exception occured while initialising the HTTP listener!");
                throw;
            }

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                foreach (MethodInfo methodInfo in type.GetMethods())
                    foreach (HttpPathAttribute path in methodInfo.GetCustomAttributes<HttpPathAttribute>())
                        pathHandlers[path.Path] = (PathHandler)Delegate.CreateDelegate(typeof(PathHandler), methodInfo);
        }

        private static void OnConnection(IAsyncResult ar)
        {
            var context = listener.EndGetContext(ar);
            listener.BeginGetContext(OnConnection, null);

            string path = "";
            if (context.Request.HttpMethod != "GET")
                return;

            string[] segments = context.Request.Url.Segments;
            for (uint i = 0u; i < segments.Length; i++)
            {
                path += segments[i];

                PathHandler pathHandler;
                if (!pathHandlers.TryGetValue(path, out pathHandler))
                    continue;

                string[] parameters = new string[(segments.Length - i) - 1];
                Array.Copy(segments, i + 1, parameters, 0, parameters.Length);

                for (uint j = 0u; j < parameters.Length; j++)
                    parameters[j] = parameters[j].TrimEnd('/');

                pathHandler.Invoke(parameters, context);
            }
        }
    }
}
