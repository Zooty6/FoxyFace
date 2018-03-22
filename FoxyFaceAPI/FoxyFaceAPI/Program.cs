using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAccess;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FoxyFaceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileInfo connectionFile = new FileInfo("data/dbConnectionString.txt");
            if (!connectionFile.Exists)
            {
                Console.WriteLine("Couldn't find data/dbConnectionString.txt, aborting");
                return;
            }
            CloudStorage.Initialize();
            
            FoxyFaceDbManager.Initialize(File.ReadAllText(connectionFile.FullName));
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.UseUrls("http://*:5000")
                .Build();
    }
}