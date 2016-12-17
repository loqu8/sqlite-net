using System.Linq;
using System.Text;
using SQLite;

#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SetUp = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitializeAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

using System.IO;

namespace SQLite.Tests
{
    [TestFixture]
    public class MultiQueryTest
    {
        TestDb _db;

        [SetUp]
        public void SetUp()
        {
            _db = new TestDb();
            _db.CreateTable<Product>();

            var p1 = new Product { Name = "One", };
            var p2 = new Product { Name = "Two", };
            var p3 = new Product { Name = "Three", };
            _db.InsertAll(new[] { p1, p2, p3 });
        }

        [Test]
        public void InsertNCount()
        {
            var result = _db.ExecuteScalar<int>("INSERT INTO Product (Name) VALUES ('Four');SELECT COUNT(1) FROM Product");
            Assert.AreEqual(result, 4);
        }

        [Test]
        public void InsertNList()
        {
            var cmd = _db.CreateCommand("INSERT INTO Product (Name) VALUES ('Five');SELECT * FROM Product");
            var result = cmd.ExecuteQuery<Product>();
            Assert.AreEqual(result.Count, 4);
        }
    }
}
