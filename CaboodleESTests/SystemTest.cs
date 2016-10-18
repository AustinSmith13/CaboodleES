using System;
using System.Collections.Generic;
using CaboodleES;
using CaboodleES.Interface;
using CaboodleES.SubSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CaboodleEsTest
{
    [TestClass]
    public class SystemTest
    {
        Caboodle caboodle;

        [TestInitialize]
        public void TestInit()
        {
            caboodle = new Caboodle();
            Assert.IsNotNull(caboodle, "Failed to initialize Aurora ECS.");
        }

        [TestMethod]
        public void TestSystem()
        {
            caboodle.Systems.Register(new MockSystem1(caboodle));
            caboodle.Systems.Start();
            caboodle.Systems.Update();
        }

        public class MockSystem1 : CaboodleSystem
        {
            public MockSystem1(Caboodle caboodle) : base(caboodle) { }

            public override void Start()
            {

            }
        }
    }
}
