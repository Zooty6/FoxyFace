using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using DatabaseAccess;
using DatabaseAccess.Model;
using DatabaseAccess.Repositories;
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

            Console.WriteLine("Running teardown.sql script");
            FileInfo info = new FileInfo("data/teardown.sql");
            if (!info.Exists)
            {
                throw new FileNotFoundException("Couldn't find database sql file " + info.FullName);
            }
            dbManager.FoxyFaceDb.ExecuteScript(info.FullName);
            
            Console.WriteLine("Running setup.sql script");
            info = new FileInfo("data/setup.sql");
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
            var ratingRepo = dbManager.RatingRepository;
            var commentRepo = dbManager.CommentRepository;
            var sessionRepo = dbManager.SessionRepository;

            Console.WriteLine("Creating users");
            
            Console.WriteLine("Creating user 'lyze'");
            User lyze = userRepo.Create("lyze", "123", "lyze@ovo");
            
            Console.WriteLine("Creating user 'zooty'");
            User zooty = userRepo.Create("zooty", "123", "zooty@owo");
            
            Console.WriteLine("Creating posts");
            
            Post post1 = postRepo.Create(lyze, "Test Post", "Test Description", "/path/to/file.png");
            Post post2 = postRepo.Create(zooty, "Test Post", "Test Description", "/path/to/file.png");

            Console.WriteLine("Create ratings");

            for (int i = 1; i <= 5; i++)
            {
                ratingRepo.Create(post1, i % 2 == 0 ? lyze : zooty, i);
            }
            for (int i = 1; i <= 5; i++)
            {
                ratingRepo.Create(post2, i % 2 == 0 ? lyze : zooty, i);
            }
           
            Console.WriteLine("Creating comments");
           
            commentRepo.Create(post1, lyze, "OvO this is awesome");
            commentRepo.Create(post1, zooty, "OwO this is awesome");

            commentRepo.Create(post2, lyze, "Meh");
            commentRepo.Create(post2, zooty, "Booo!");

            Session lyzeSession = sessionRepo.Create(lyze);
            Session zootySession = sessionRepo.Create(zooty);

            Assert.AreEqual(lyzeSession, sessionRepo.FindById(lyzeSession.Id));
            Assert.AreEqual(zootySession, sessionRepo.FindByToken(zootySession.Token));
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("Closing db connection");
            dbManager.Close();
        }
    }
}