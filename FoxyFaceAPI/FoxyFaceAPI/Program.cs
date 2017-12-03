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

//𐲯𐳛𐳛𐳨                                                            
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
            FileInfo accountFile = new FileInfo("data/azureKey.txt");
            if (!connectionFile.Exists)
            {
                Console.WriteLine("Couldn't find data/azureKey. file, aborting");
                return;
            }

            string[] key = File.ReadAllLines(accountFile.FullName);
            CloudStorage.Initialize(key[0], key[1]);
            
            FoxyFaceDbManager.Initialize(File.ReadAllText(connectionFile.FullName));
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}