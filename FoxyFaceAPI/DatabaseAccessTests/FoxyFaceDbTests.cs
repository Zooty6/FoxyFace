using System;
using System.IO;
using System.Net;
using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseAccessTests
{
    [TestClass]
    public class FoxyFaceDbTests
    {
        private static FoxyFaceDbManager dbManager;
            
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Console.WriteLine("Reading connection string");
            FileInfo connectionFile = new FileInfo("data/connectionString.txt");
            if (!connectionFile.Exists)
            {
                throw new FileNotFoundException("Couldn't find connection string file " + connectionFile.FullName);
            }
            string connectionString = File.ReadAllText(connectionFile.FullName);

            Console.WriteLine("Initializing database");
            dbManager = FoxyFaceDbManager.Initialize(connectionString);

            Console.WriteLine("Running setup.sql script");
            FileInfo info = new FileInfo("data/setup.sql");
            if (!info.Exists)
            {
                throw new FileNotFoundException("Couldn't find database sql file " + info.FullName);
            }
            dbManager.FoxyFaceDb.ExecuteScript(info.FullName);
        }
        
        [TestMethod]
        public void CreateEverything()
        {
            var userRepo = dbManager.UserRepository;
            var postRepo = dbManager.PostRepository;

            Console.WriteLine("Creating users");
            
            Console.WriteLine("Creating user 'lyze'");
            User lyze = userRepo.Create("lyze", "123", "lyze@ovo");
            
            Console.WriteLine("Creating user 'zooty'");
            User zooty = userRepo.Create("zooty", "123", "zooty@owo");
            
            Console.WriteLine("Creating posts");
            
            postRepo.Create(lyze.Id, "Test Post", "Test Description", "/path/to/file.png");
            postRepo.Create(zooty.Id, "Test Post", "Test Description", "/path/to/file.png");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("Running teardown.sql script");
            FileInfo info = new FileInfo("data/teardown.sql");
            if (!info.Exists)
            {
                throw new FileNotFoundException("Couldn't find database sql file " + info.FullName);
            }
            dbManager.FoxyFaceDb.ExecuteScript(info.FullName);
            
            Console.WriteLine("Closing db connection");
            dbManager.Close();
        }
    }
}