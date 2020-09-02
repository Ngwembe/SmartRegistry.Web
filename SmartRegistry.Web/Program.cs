using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SmartRegistry.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            //.UseKestrel()
            //.UseContentRoot(Directory.GetCurrentDirectory()) //Like this
            //.UseIISIntegration()
            WebHost.CreateDefaultBuilder(args)
            //.UseUrls("http://192.168.4.3")
            .UseUrls("http://*")
            //.UseUrls("http://192.168.4.2")
            //.UseUrls("http://192.168.8.108")
            //.UseUrls("http://192.168.8.4")
            //.UseUrls("http://*:8970")
            .UseStartup<Startup>()
            .Build();
    }
}
