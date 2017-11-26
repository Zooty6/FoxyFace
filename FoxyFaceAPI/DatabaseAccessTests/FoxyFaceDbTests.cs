using System.IO;
using System.Net;
using DatabaseAccess;
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
            FileInfo connectionFile = new FileInfo("../../../data/connectionString.txt");
            if (!connectionFile.Exists)
            {
                throw new FileNotFoundException("Couldn't find connection string file " + connectionFile.FullName);
            }
            string connectionString = File.ReadAllText(connectionFile.FullName);

            dbManager = FoxyFaceDbManager.Initialize(connectionString);
            
            FileInfo info = new FileInfo("../../../../../database.sql");
            if (!info.Exists)
            {
                throw new FileNotFoundException("Couldn't find database sql file " + info.FullName);
            }
            
            dbManager.FoxyFaceDb.ExecuteScript(info.FullName);
        }
        
        [TestMethod]
        public void TestMethod1()
        {
            
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            dbManager.Close();
        }
    }
}