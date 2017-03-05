using CaboodleES.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuroraTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UtilityTest
    {
        BitMask mask;

        public UtilityTest()
        {
            mask = new BitMask();
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestBitMask()
        {
            mask.Set(0, true);
            mask.Set(1, true);
            mask.Set(2, false);
            mask.Set(3, true);

            Assert.AreEqual(true, mask.Get(0));
            Assert.AreEqual(true, mask.Get(1));
            Assert.AreEqual(false, mask.Get(2));
            Assert.AreEqual(true, mask.Get(3));

            BitMask mask2 = new BitMask();
            mask2.Set(0, true);
            mask2.Set(1, true);
            mask2.Set(2, false);
            mask2.Set(3, true);

            Assert.AreEqual(true, mask2 == mask);
            mask2.Set(3, false);
            Assert.AreEqual(false, mask2 == mask);
        }

        [TestMethod]
        public void TestBag()
        {
            var bag = new Bag<int>();

            bag.Add(1);
            bag.Add(2);
            bag.Add(3);
            bag.RemoveAt(2);
            Assert.AreEqual(1, bag[0]);
            Assert.AreEqual(2, bag[1]);
            Assert.AreEqual(0, bag[2]);
            bag.RemoveAt(0);
            Assert.AreEqual(2, bag[0]);
        }
    }
}
